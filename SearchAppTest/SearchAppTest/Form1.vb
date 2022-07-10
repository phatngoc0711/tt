Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Public Class Form1
    Private appPath As String = Application.StartupPath()
    Private di As New IO.DirectoryInfo(appPath)
    Private aryFileIdx As IO.FileInfo() = di.GetFiles("*.idx")
    Private workType As New List(Of String)
    Private WorkDetail As New List(Of String)
    Private Flag As Integer = 0
    Private workTypeNameFromIniPermanent As New List(Of String)
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Set key press = true
        Me.KeyPreview = True
        '--------------------
        'Check exist .IDK
        Dim totalFileIdx As Integer
        totalFileIdx = aryFileIdx.Length
        If totalFileIdx = 0 Then
            MsgBox("Not Found IDX FILE", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical,
               "Warning")
            Close()
        End If
        '-------------------------------------------------------
        'Check .ini
        Dim aryFileIni As IO.FileInfo() = di.GetFiles("*.ini")
        Dim totalFileIni As Integer
        totalFileIni = aryFileIni.Length
        If totalFileIni = 0 Then
            MsgBox("Not Found INI FILE", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical,
               "Warning")
            Close()
        End If
        workTypeNameFromIniPermanent = getIniWorkTypeNameLine("[WorkType]", "[WorkTypeDetail]")
        '-------------------------------------------------------
        'WorkType In ListBox
        Dim line() As String
        line = File.ReadAllLines(appPath + "\config.ini")
        For m As Integer = 0 To line.Length - 1 Step +1
            If line(m) = "[WorkType]" Then
                For n As Integer = m + 2 To line.Length - 1 Step +1
                    Dim match As Match = Regex.Match(line(n),
                                         "=",
                                         RegexOptions.IgnoreCase)
                    If match.Success Then
                        Dim sb As New StringBuilder()
                        For Each c As Char In line(n)
                            If [Char].IsLetter(c) Or c = " " Then
                                sb.Append(c)
                            End If
                        Next
                        ListBox1.Items.Add(sb)
                    End If
                    If line(n) = "[WorkTypeDetail]" Then
                        Exit For
                    End If
                Next n
            End If
        Next m
        '-------------------------------------------------------
        ListBox1.SelectionMode = SelectionMode.MultiExtended
        Dim i As Integer
        For i = 0 To Me.ListBox1.Items.Count - 1
            Me.ListBox1.SetSelected(i, True)
        Next i
        '-------------------------------------------------------
        'Datagridview
        Dim tableDataGrid As New DataTable()
        tableDataGrid.Columns.Add("WorkType", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Date", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Shop CODE", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Pos NO", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Receipt Code", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Manage Code", Type.GetType("System.String"))
        DataGridView1.DataSource = tableDataGrid
        '--------------------------------------------------------------------
        'Set DATE TIME
        setDefaultDateTime()
        '------------------------------------------------------------------- 
        'ComboBox
        ComboBox1.Items.Add("9")
        ComboBox1.Items.Add("12")
        ComboBox1.Items.Add("15")
        ComboBox1.Items.Add("18")
        ComboBox1.Items.Add("21")
        ComboBox1.Items.Add("24")
        ComboBox1.SelectedIndex = 0
        'TextBox
        TextBox1.TextAlign = HorizontalAlignment.Right
        TextBox1.MaxLength = 4
        TextBox2.TextAlign = HorizontalAlignment.Right
        TextBox2.MaxLength = 2
        TextBox3.TextAlign = HorizontalAlignment.Right
        TextBox3.MaxLength = 8
        TextBox1.Text = 0
        TextBox2.Text = 0
        TextBox3.Text = 0
        'Block Type RichTextBox
        RichTextBox1.ReadOnly = True
    End Sub
    'Get list line tu key1 den key 2 trong file ini co chua dau '='
    Private Function getIniWorkTypeNameLine(Key1 As String, Key2 As String) As List(Of String)
        Dim line() As String
        line = File.ReadAllLines(appPath + "\config.ini")
        Dim blist As New List(Of String)
        For m As Integer = 0 To line.Length - 1 Step +1
            If line(m) = Key1 Then
                For n As Integer = m + 2 To line.Length - 1 Step +1
                    Dim match As Match = Regex.Match(line(n),
                                         "=",
                                         RegexOptions.IgnoreCase)
                    If match.Success Then
                        blist.Add(line(n))
                    End If
                    If line(n) = Key2 Then
                        Exit For
                    End If
                Next n
            End If
        Next m
        Return blist
    End Function
    'Lay  Name tu 1 list Line cho truoc lay ra mot dong co chua Key
    Private Function getIniWorkTypeName(listLine As List(Of String), Key As String) As String
        Dim StringNameWordType As String = ""
        Dim Flag As Integer
        For i As Integer = 0 To listLine.Count - 1 Step +1
            If InStr(listLine(i), Key) Then
                Flag = i
                Dim sb As New StringBuilder()
                For Each c As Char In listLine(i)
                    If [Char].IsLetter(c) Or c = " " Then
                        sb.Append(c)
                    End If
                Next
                StringNameWordType = sb.ToString
                Exit For
            End If
        Next i
        Return StringNameWordType
    End Function
    'Intput TextBox.text
    'KEY la do dai chuoi qui dinh trong file int
    'Return chuoi co so 0 dang truoc neu Textbox.text ko du gia tri 
    Private Function getTextValue(text As String, len As Integer) As String
        Dim valueText As String = ""
        Dim str_len As Integer
        str_len = text.Length
        For i As Integer = 0 To len - str_len - 1 Step +1
            valueText = valueText + "0"
        Next i
        valueText = valueText + text
        Return valueText
    End Function
    'get ALL line afterSeach By text
    Private Function GetAllLineAfterSearch() As List(Of String)
        Dim day1 As Date
        Dim day2 As Date
        day1 = DateTimePicker1.Value
        day2 = DateTimePicker2.Value
        Dim key1 As String = TextBox1.Text
        Dim key2 As String = TextBox2.Text
        Dim key3 As String = TextBox3.Text
        If key1 <> "0" Then
            key1 = getTextValue(TextBox1.Text, 4)
        End If
        If key2 <> "0" Then
            key2 = getTextValue(TextBox2.Text, 2)
        End If
        If key3 <> "0" Then
            key3 = getTextValue(TextBox3.Text, 8)
        End If
        Dim lineData As New List(Of String)
        For f As Integer = 0 To aryFileIdx.Length - 1 Step +1
            Using objReader As New StreamReader(appPath + "\" + aryFileIdx(f).Name)
                Do While objReader.Peek() <> -1
                    Dim Line As String = objReader.ReadLine()
                    'Check TextBox---------------ListBox---------------DateTime--------------------------
                    If lineCheckText(Line, key1, key2, key3) And lineCheckListBox(Line) And lineCheckDateTime(Line, day1, day2) Then
                        lineData.Add(Line)
                    End If
                    '---------------------------------------------------------------------
                Loop
                objReader.Close()
            End Using
        Next f
        Return lineData
    End Function
    Private Function lineCheckDateTime(line As String, day1 As Date, day2 As Date) As Boolean
        Dim vals() As String
        'val(0) : date value
        vals = line.ToString().Split("	")
        Dim flag As String
        flag = vals(0)
        Dim dayCheck As Date
        Dim Year As String
        Dim Month As String
        Dim day As String
        Year = flag(0) + flag(1) + flag(2) + flag(3)
        Month = flag(4) + flag(5)
        day = flag(6) + flag(7)
        dayCheck = New Date(Convert.ToInt32(Year), Convert.ToInt32(Month), Convert.ToInt32(day))
        If dayCheck >= day1 And dayCheck <= day2 Then
            Return True
        End If
        Return False
    End Function
    'Get String tu vi tri so key1 den vi tri so key 2
    Private Function getString(line As String, key1 As Integer, key2 As Integer) As String
        Dim result As String = ""
        For i As Integer = key1 To key2 Step +1
            result = result + line(i)
        Next i
        Return result
    End Function
    'Return list WorkTypeId From ListWorkTypeName selected
    Private Function getIdWorkTypeFromListBox() As List(Of String)
        Dim listWordTypeId As New List(Of String)
        Dim lineWorkTypeTemp As New List(Of String)
        'lineWorkTypeTemp = getIniWorkTypeNameLine("[WorkType]", "[WorkTypeDetail]")
        For k As Integer = 0 To workTypeNameFromIniPermanent.Count - 1 Step +1
            lineWorkTypeTemp.Add(workTypeNameFromIniPermanent(k))
        Next k
        For j As Integer = 0 To workType.Count - 1 Step +1
            For i As Integer = 0 To lineWorkTypeTemp.Count - 1 Step +1
                If lineWorkTypeTemp(i).Contains(workType(j)) Then
                    listWordTypeId.Add(getString(lineWorkTypeTemp(i), 2, 5))
                    lineWorkTypeTemp.RemoveAt(i)
                    i = i - 1
                    Exit For
                End If
            Next i
        Next j
        Return listWordTypeId
    End Function
    'Check check 1 String co nam trong 1 ListString hay khong
    Private Function checkStringExistInListString(list As List(Of String), key As String) As Boolean
        For i As Integer = 0 To list.Count - 1 Step +1
            If list(i) = key Then
                Return True
            End If
        Next i
        Return False
    End Function
    Private Function lineCheckListBox(line As String) As Boolean
        Dim vals() As String
        'val(5) : id worktype
        vals = line.ToString().Split("	")
        Dim key As String = ""
        key = getString(vals(5), 0, 3)
        Dim listListBoxId As New List(Of String)
        listListBoxId = getIdWorkTypeFromListBox()
        Return checkStringExistInListString(listListBoxId, key)
    End Function
    Private Function Check(Key1 As String) As Boolean
        If Key1 <> "0" Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function lineCheckText(line As String, Key1 As String, Key2 As String, Key3 As String) As Boolean
        Dim vals() As String
        vals = line.ToString().Split("	")
        Dim flag As Integer = 0
        If Check(Key1) Then
            flag = flag + 1
        End If
        If Check(Key2) Then
            flag = flag + 1
        End If
        If Check(Key3) Then
            flag = flag + 1
        End If
        If flag = 0 Then
            Return True
        End If
        If Key1 = vals(1) And Key3 = vals(3) And Key2 = vals(2) Then
            Return True
        End If
        If flag = 1 Then
            If Key1 = vals(1) Then
                Return True
            End If
            If Key2 = vals(2) Then
                Return True
            End If
            If Key3 = vals(3) Then
                Return True
            End If
        End If
        If flag = 2 Then
            If Key1 = vals(1) And Key2 = vals(2) Then
                Return True
            End If
            If Key1 = vals(1) And Key3 = vals(3) Then
                Return True
            End If
            If Key3 = vals(3) And Key2 = vals(2) Then
                Return True
            End If
        End If
        Return False
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim tableDataGrid As New DataTable()
        'Datagridview
        tableDataGrid.Columns.Add("WorkType", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Date", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Shop CODE", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Pos NO", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Receipt Code", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Manage Code", Type.GetType("System.String"))
        DataGridView1.DataSource = tableDataGrid
        '--------------------------------------------------------------------
        If Flag Mod 2 = 0 Then
            Button1.Text = "Search (S)"
        Else
            Button1.Text = "Cancel (E)"
        End If
        'Cancel
        '--------------------------------------------------------------------------
        'Search
        Dim list As List(Of String)
        list = GetAllLineAfterSearch()
        Dim valsDataGrid() As String
        For x As Integer = 0 To list.Count - 1 Step +1
            If tableDataGrid.Rows.Count > 2999 Then
                MsgBox("Not Over 3000", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                    "Warning")
                Exit For
            End If
            valsDataGrid = list(x).ToString().Split("	")
            Dim row(valsDataGrid.Length - 5) As String
            'row(0) = valsDataGrid(valsDataGrid.Length - 5).Trim()
            Dim typeWordId As String = ""
            typeWordId = valsDataGrid(valsDataGrid.Length - 5).Trim()
            Dim lineWordDetail As List(Of String)
            lineWordDetail = getIniWorkTypeNameLine("[WorkTypeDetail]", "[DenpyoType]")
            row(0) = getIniWorkTypeName(lineWordDetail, typeWordId)
            '-------------------------------------------------------------------
            For y As Integer = 1 To valsDataGrid.Length - 5 Step +1
                row(y) = valsDataGrid(y - 1).Trim()
            Next y
            tableDataGrid.Rows.Add(row)
        Next x
        Label7.Text = "Total Record: " + tableDataGrid.Rows.Count.ToString
    End Sub
    Private Sub setDefaultDateTime()
        Dim name As String = ""
        name = aryFileIdx(0).Name
        Dim Year As String
        Dim Month As String
        Dim day As String
        Year = name(0) + name(1) + name(2) + name(3)
        Month = name(4) + name(5)
        day = name(6) + name(7)
        DateTimePicker1.Value = New Date(Convert.ToInt32(Year), Convert.ToInt32(Month), Convert.ToInt32(day))
    End Sub
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        'e.SuppressKeyPress = True
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.C Then
                e.Handled = True
                TextBox1.Focus()
            End If
        End If
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.P Then
                e.Handled = True
                TextBox2.Focus()
            End If
        End If
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.R Then
                e.Handled = True
                TextBox3.Focus()
            End If
        End If
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.D Then
                e.Handled = True
                DateTimePicker1.Focus()
            End If
        End If
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.T Then
                e.Handled = True
                DateTimePicker2.Focus()
            End If
        End If
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.Y Then
                e.Handled = True
                Dim frm As New Form2
                frm.Show()
            End If
        End If
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.X Then
                e.Handled = True
                If MsgBox("Exit ??", MsgBoxStyle.YesNo Or MsgBoxStyle.Question,
               "Warning") = vbYes Then
                    Close()
                End If
            End If
        End If
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.F Then
                e.Handled = True
                ComboBox1.Focus()
            End If
        End If
    End Sub
    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        If MsgBox("Exit ??", MsgBoxStyle.YesNo Or MsgBoxStyle.Question,
               "Warning") = vbYes Then
            Close()
        End If
    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If MsgBox("Exit ??", MsgBoxStyle.YesNo Or MsgBoxStyle.Question,
               "Warning") = vbNo Then
            e.Cancel = True
        End If
    End Sub
    Private Sub TextBox1_Click(sender As Object, e As EventArgs) Handles TextBox1.Click
        TextBox1.SelectAll()
    End Sub
    Private Sub TextBox2_Click(sender As Object, e As EventArgs) Handles TextBox2.Click
        TextBox2.SelectAll()
    End Sub
    Private Sub TextBox3_Click(sender As Object, e As EventArgs) Handles TextBox3.Click
        TextBox3.SelectAll()
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.TextLength = 0 Then
            TextBox1.Text = 0
            TextBox1.SelectAll()
        End If
    End Sub
    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If TextBox2.TextLength = 0 Then
            TextBox2.Text = 0
            TextBox2.SelectAll()
        End If
    End Sub
    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        If TextBox3.TextLength = 0 Then
            TextBox3.Text = 0
            TextBox3.SelectAll()
        End If
    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
        If TextBox1.TextLength = 1 And e.KeyChar = "0" And TextBox1.Text = "0" Then
            e.Handled = True
        End If
    End Sub
    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
        If TextBox1.TextLength = 1 And e.KeyChar = "0" And TextBox2.Text = "0" Then
            e.Handled = True
        End If
    End Sub
    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
        If TextBox1.TextLength = 1 And e.KeyChar = "0" And TextBox3.Text = "0" Then
            e.Handled = True
        End If
    End Sub
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        For i = workType.Count - 1 To 0 Step -1
            workType.RemoveAt(i)
        Next
        For l As Integer = 0 To ListBox1.SelectedItems.Count - 1 Step +1
            workType.Add(ListBox1.SelectedItems.Item(l).ToString)
        Next l
    End Sub
    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        'PrintDialog1.ShowDialog()
        If PrintDialog1.ShowDialog = DialogResult.OK Then
            PrintDocument1.Print()
        End If
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Static currentChar As Integer
        Static currentLine As Integer
        Dim textfont As Font = RichTextBox1.Font
        Dim h, w As Integer
        Dim left, top As Integer
        With PrintDocument1.DefaultPageSettings
            h = .PaperSize.Height - .Margins.Top - .Margins.Bottom
            w = .PaperSize.Width - .Margins.Left - .Margins.Right
            left = PrintDocument1.DefaultPageSettings.Margins.Left
            top = PrintDocument1.DefaultPageSettings.Margins.Top
        End With
        'Optional Rectangle Blue.
        'e.Graphics.DrawRectangle(Pens.Blue, New Rectangle(left, top, w, h))
        If PrintDocument1.DefaultPageSettings.Landscape Then
            Dim a As Integer
            a = h
            h = w
            w = a
        End If
        Dim lines As Integer = CInt(Math.Round(h / textfont.Height))
        Dim b As New Rectangle(left, top, w, h)
        Dim format As StringFormat
        If Not RichTextBox1.WordWrap Then
            format = New StringFormat(StringFormatFlags.NoWrap)
            format.Trimming = StringTrimming.EllipsisWord
            Dim i As Integer
            For i = currentLine To Math.Min(currentLine + lines, RichTextBox1.Lines.Length - 1)
                e.Graphics.DrawString(RichTextBox1.Lines(i), textfont, Brushes.Black, New RectangleF(left, top + textfont.Height * (i - currentLine), w, textfont.Height), format)
            Next
            currentLine += lines
            If currentLine >= TextBox1.Lines.Length Then
                e.HasMorePages = False
                currentLine = 0
            Else
                e.HasMorePages = True
            End If
            Exit Sub
        End If
        format = New StringFormat(StringFormatFlags.LineLimit)
        Dim line, chars As Integer
        e.Graphics.MeasureString(Mid(RichTextBox1.Text, currentChar + 1), textfont, New SizeF(w, h), format, chars, line)
        If currentChar + chars < RichTextBox1.Text.Length Then
            If RichTextBox1.Text.Substring(currentChar + chars, 1) <> " " And RichTextBox1.Text.Substring(currentChar + chars, 1) <> vbLf Then
                While chars > 0
                    RichTextBox1.Text.Substring(currentChar + chars, 1)
                    RichTextBox1.Text.Substring(currentChar + chars, 1)
                    chars -= 1
                End While
                chars += 1
            End If
        End If
        e.Graphics.DrawString(RichTextBox1.Text.Substring(currentChar, chars), textfont, Brushes.Black, b, format)
        currentChar = currentChar + chars
        If currentChar < RichTextBox1.Text.Length Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
            currentChar = 0
        End If
    End Sub
    Private Sub DataGridView1_Sorted(sender As Object, e As EventArgs) Handles DataGridView1.Sorted
        If DataGridView1.RowCount > 1 Then
            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                i = i + 1
            Next
        End If
    End Sub
    Private Sub DataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridView1.DataBindingComplete
        If DataGridView1.RowCount > 1 Then
            For i As Integer = 0 To DataGridView1.Rows.Count - 2
                DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                i = i + 1
            Next
        End If
    End Sub
End Class