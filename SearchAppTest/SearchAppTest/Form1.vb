Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class Form1
    Private mvStr_AppPath As String = Application.StartupPath()
    Private mvDic_DirectoryInfo As New IO.DirectoryInfo(mvStr_AppPath)
    Private mvFso_GetAllFileIdx As IO.FileInfo() = mvDic_DirectoryInfo.GetFiles("*.idx")
    Private mvLst_StreamReaderIdx As New List(Of StreamReader)
    Private mvDic_PathDataFile As New IO.DirectoryInfo(mvStr_AppPath + "\DATA")
    Private mvFso_GetAllFileDat As IO.FileInfo() = mvDic_PathDataFile.GetFiles("*.DAT")
    Private mvLst_ListWorkType As New List(Of String)
    Private mvInt_TotalTextBoxNotNull As Integer = 0
    Private mvLst_WorkTypeNameFromIni As New List(Of String)
    Private listDataGridView As New List(Of String)
    Private lineWordDetail As List(Of String)
    Private lineInConfigIni() As String
    Private listIdWorkTypeFromListBox As New List(Of String)
    Private Sub a()
        lineInConfigIni = File.ReadAllLines(mvStr_AppPath + "\config.ini")
        lineWordDetail = getIniWorkTypeNameLine("[WorkTypeDetail]", "[DenpyoType]")
        mvLst_WorkTypeNameFromIni = getIniWorkTypeNameLine("[WorkType]", "[WorkTypeDetail]")
    End Sub
    Private t2 As New Thread(AddressOf a)
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        For f As Integer = 0 To mvFso_GetAllFileIdx.Length - 1 Step +1
            Dim objReader As New StreamReader(mvStr_AppPath + "\" + mvFso_GetAllFileIdx(f).Name)
            mvLst_StreamReaderIdx.Add(objReader)
        Next f
        'Set key press = true
        Me.KeyPreview = True
        '--------------------
        'Check exist .IDK
        Dim totalFileIdx As Integer
        totalFileIdx = mvFso_GetAllFileIdx.Length
        If totalFileIdx = 0 Then
            MsgBox("Not Found IDX FILE", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical,
               "Warning")
            Close()
        End If
        '-------------------------------------------------------
        'Check .ini
        Dim aryFileIni As IO.FileInfo() = mvDic_DirectoryInfo.GetFiles("*.ini")
        Dim totalFileIni As Integer
        totalFileIni = aryFileIni.Length
        If totalFileIni = 0 Then
            MsgBox("Not Found INI FILE", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical,
               "Warning")
            Close()
        End If

        t2.Start()
        '-------------------------------------------------------
        'WorkType In ListBox
        Dim line() As String
        line = File.ReadAllLines(mvStr_AppPath + "\config.ini")
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
                        lstWorkType.Items.Add(sb)
                    End If
                    If line(n) = "[WorkTypeDetail]" Then
                        Exit For
                    End If
                Next n
            End If
        Next m
        '-------------------------------------------------------
        lstWorkType.SelectionMode = SelectionMode.MultiExtended
        Dim i As Integer
        For i = 0 To Me.lstWorkType.Items.Count - 1
            Me.lstWorkType.SetSelected(i, True)
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
        dgvInformationRecord.DataSource = tableDataGrid
        '--------------------------------------------------------------------
        'Set DATE TIME
        setDefaultDateTime()
        '------------------------------------------------------------------- 
        'ComboBox
        cmbFontReceiptSize.Items.Add("9")
        cmbFontReceiptSize.Items.Add("12")
        cmbFontReceiptSize.Items.Add("15")
        cmbFontReceiptSize.Items.Add("18")
        cmbFontReceiptSize.Items.Add("21")
        cmbFontReceiptSize.Items.Add("24")
        cmbFontReceiptSize.SelectedIndex = 0
        cmbFontReceiptSize.MaxLength = 2
        'TextBox
        txtShopCode.TextAlign = HorizontalAlignment.Right
        txtShopCode.MaxLength = 4
        txtPosNo.TextAlign = HorizontalAlignment.Right
        txtPosNo.MaxLength = 2
        txtReceiptCode.TextAlign = HorizontalAlignment.Right
        txtReceiptCode.MaxLength = 8
        txtShopCode.Text = 0
        txtPosNo.Text = 0
        txtReceiptCode.Text = 0
        'Block Type RichTextBox
        rtbReceiptDetail.ReadOnly = True
        rtbReceiptDetail.ScrollBars = RichTextBoxScrollBars.Both
        'RichTextBox1.WordWrap = False
        flagClose = 1
    End Sub
    'Get list line tu key1 den key 2 trong file ini co chua dau '='
    Private Function getIniWorkTypeNameLine(Key1 As String, Key2 As String) As List(Of String)

        Dim blist As New List(Of String)
        For m As Integer = 0 To lineInConfigIni.Length - 1 Step +1
            If lineInConfigIni(m) = Key1 Then
                For n As Integer = m + 2 To lineInConfigIni.Length - 1 Step +1
                    Dim match As Match = Regex.Match(lineInConfigIni(n),
                                         "=",
                                         RegexOptions.IgnoreCase)
                    If match.Success Then
                        blist.Add(lineInConfigIni(n))
                    End If
                    If lineInConfigIni(n) = Key2 Then
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
        Dim Flag1 As Integer
        For i As Integer = 0 To listLine.Count - 1 Step +1
            If InStr(listLine(i), Key) Then
                Flag1 = i
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

    Private Function lineCheckDateTime(line As String, day1 As Date, day2 As Date) As Boolean
        Dim vals() As String
        'val(0) : date value
        vals = line.ToString().Split("	")
        Dim flag1 As String
        flag1 = vals(0)
        Dim dayCheck As Date
        Dim Year As String
        Dim Month As String
        Dim day As String
        Year = flag1(0) + flag1(1) + flag1(2) + flag1(3)
        Month = flag1(4) + flag1(5)
        day = flag1(6) + flag1(7)
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
        For k As Integer = 0 To mvLst_WorkTypeNameFromIni.Count - 1 Step +1
            lineWorkTypeTemp.Add(mvLst_WorkTypeNameFromIni(k))
        Next k
        For j As Integer = 0 To mvLst_ListWorkType.Count - 1 Step +1
            For i As Integer = 0 To lineWorkTypeTemp.Count - 1 Step +1
                If lineWorkTypeTemp(i).Contains(mvLst_ListWorkType(j)) Then
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
        listListBoxId = listIdWorkTypeFromListBox           '1'
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
        If mvInt_TotalTextBoxNotNull = 0 Then
            Return True
        End If
        If Key1 = vals(1) And Key3 = vals(3) And Key2 = vals(2) Then
            Return True
        End If
        If mvInt_TotalTextBoxNotNull = 1 Then
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
        If mvInt_TotalTextBoxNotNull = 2 Then
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

    Private Sub setDefaultDateTime()
        Dim name As String = ""
        name = mvFso_GetAllFileIdx(0).Name
        Dim Year As String
        Dim Month As String
        Dim day As String
        Year = name(0) + name(1) + name(2) + name(3)
        Month = name(4) + name(5)
        day = name(6) + name(7)
        dtmStart.Value = New Date(Convert.ToInt32(Year), Convert.ToInt32(Month), Convert.ToInt32(day))
    End Sub
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        'e.SuppressKeyPress = True
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.C Then
                e.Handled = True
                txtShopCode.Focus()
            End If
        End If
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.P Then
                e.Handled = True
                txtPosNo.Focus()
            End If
        End If
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.R Then
                e.Handled = True
                txtReceiptCode.Focus()
            End If
        End If
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.D Then
                e.Handled = True
                dtmStart.Focus()
            End If
        End If
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.T Then
                e.Handled = True
                dtmFinish.Focus()
            End If
        End If
        If Control.ModifierKeys = Keys.Alt Then
            If e.KeyCode = Keys.Y Then
                e.Handled = True
                If Not rtbReceiptDetail.Text = "" Then
                    If PrintDialog1.ShowDialog = DialogResult.OK Then
                        PrintDocument1.Print()
                    End If
                End If
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
                cmbFontReceiptSize.Focus()
            End If
        End If
    End Sub
    Private Sub mnsQuit_Click(sender As Object, e As EventArgs) Handles mnsQuit.Click
        If MsgBox("Exit ??", MsgBoxStyle.YesNo Or MsgBoxStyle.Question,
              "Warning") = vbYes Then
            Close()
        End If
    End Sub
    Private flagClose As Integer = 0
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If flagClose = 1 Then
            If MsgBox("Exit ??", MsgBoxStyle.YesNo Or MsgBoxStyle.Question,
               "Warning") = vbNo Then
                e.Cancel = True
            End If
        End If

    End Sub
    Private Sub txtShopCode_Click(sender As Object, e As EventArgs) Handles txtShopCode.Click
        txtShopCode.SelectAll()
    End Sub
    Private Sub txtPosNo_Click(sender As Object, e As EventArgs) Handles txtPosNo.Click
        txtPosNo.SelectAll()
    End Sub
    Private Sub txtReceiptCode_Click(sender As Object, e As EventArgs) Handles txtReceiptCode.Click
        txtReceiptCode.SelectAll()
    End Sub
    Private Sub txtShopCode_TextChanged(sender As Object, e As EventArgs) Handles txtShopCode.TextChanged
        If txtShopCode.TextLength = 0 Then
            txtShopCode.Text = 0
            txtShopCode.SelectAll()
        End If
    End Sub
    Private Sub txtPosNo_TextChanged(sender As Object, e As EventArgs) Handles txtPosNo.TextChanged
        If txtPosNo.TextLength = 0 Then
            txtPosNo.Text = 0
            txtPosNo.SelectAll()
        End If
    End Sub
    Private Sub txtReceiptCode_TextChanged(sender As Object, e As EventArgs) Handles txtReceiptCode.TextChanged
        If txtReceiptCode.TextLength = 0 Then
            txtReceiptCode.Text = 0
            txtReceiptCode.SelectAll()
        End If
    End Sub
    Private Sub txtShopCode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtShopCode.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
        If txtShopCode.TextLength = 1 And e.KeyChar = "0" And txtShopCode.Text = "0" Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtPosNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPosNo.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
        If txtShopCode.TextLength = 1 And e.KeyChar = "0" And txtPosNo.Text = "0" Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtReceiptCode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtReceiptCode.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
        If txtShopCode.TextLength = 1 And e.KeyChar = "0" And txtReceiptCode.Text = "0" Then
            e.Handled = True
        End If
    End Sub
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstWorkType.SelectedIndexChanged
        For i = mvLst_ListWorkType.Count - 1 To 0 Step -1
            mvLst_ListWorkType.RemoveAt(i)
        Next
        For l As Integer = 0 To lstWorkType.SelectedItems.Count - 1 Step +1
            mvLst_ListWorkType.Add(lstWorkType.SelectedItems.Item(l).ToString)
        Next l
    End Sub
    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles mnsPrint.Click
        'PrintDialog1.ShowDialog()
        If Not rtbReceiptDetail.Text = "" Then
            If PrintDialog1.ShowDialog = DialogResult.OK Then
                PrintDocument1.Print()
            End If
        End If
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Static currentChar As Integer
        Static currentLine As Integer
        Dim textfont As Font = rtbReceiptDetail.Font
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
        If Not rtbReceiptDetail.WordWrap Then
            format = New StringFormat(StringFormatFlags.NoWrap)
            format.Trimming = StringTrimming.EllipsisWord
            Dim i As Integer
            For i = currentLine To Math.Min(currentLine + lines, rtbReceiptDetail.Lines.Length - 1)
                e.Graphics.DrawString(rtbReceiptDetail.Lines(i), textfont, Brushes.Black, New RectangleF(left, top + textfont.Height * (i - currentLine), w, textfont.Height), format)
            Next
            currentLine += lines
            If currentLine >= txtShopCode.Lines.Length Then
                e.HasMorePages = False
                currentLine = 0
            Else
                e.HasMorePages = True
            End If
            Exit Sub
        End If
        format = New StringFormat(StringFormatFlags.LineLimit)
        Dim line, chars As Integer
        e.Graphics.MeasureString(Mid(rtbReceiptDetail.Text, currentChar + 1), textfont, New SizeF(w, h), format, chars, line)
        If currentChar + chars < rtbReceiptDetail.Text.Length Then
            If rtbReceiptDetail.Text.Substring(currentChar + chars, 1) <> " " And rtbReceiptDetail.Text.Substring(currentChar + chars, 1) <> vbLf Then
                While chars > 0
                    rtbReceiptDetail.Text.Substring(currentChar + chars, 1)
                    rtbReceiptDetail.Text.Substring(currentChar + chars, 1)
                    chars -= 1
                End While
                chars += 1
            End If
        End If
        e.Graphics.DrawString(rtbReceiptDetail.Text.Substring(currentChar, chars), textfont, Brushes.Black, b, format)
        currentChar = currentChar + chars
        If currentChar < rtbReceiptDetail.Text.Length Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
            currentChar = 0
        End If
    End Sub
    Private Sub DataGridView1_Sorted(sender As Object, e As EventArgs) Handles dgvInformationRecord.Sorted
        If dgvInformationRecord.RowCount > 1 Then
            For i As Integer = 0 To dgvInformationRecord.Rows.Count - 2
                dgvInformationRecord.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                i = i + 1
            Next
        End If
    End Sub
    Private Sub DataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvInformationRecord.DataBindingComplete
        If dgvInformationRecord.RowCount > 1 Then
            For i As Integer = 0 To dgvInformationRecord.Rows.Count - 2
                dgvInformationRecord.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                i = i + 1
            Next
        End If
    End Sub
    Private Function DeleteChar0(key As String) As String
        Dim value As String = ""
        Dim flag1 As Integer = 0
        For i As Integer = 0 To key.Length - 1 Step +1
            If key(i) <> "0" Then
                flag1 = i
                Exit For
            End If
        Next
        For j As Integer = flag1 To key.Length - 1 Step +1
            value = value + key(j)
        Next j
        Return value
    End Function
    Private Function dayFormat(key As String) As String
        Dim value As String = ""
        value = key(0) + key(1) + key(2) + key(3) + "/" + key(4) + key(5) + "/" + key(6) + key(7)
        Return value
    End Function
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInformationRecord.CellClick
        rtbReceiptDetail.Clear()
        If e.RowIndex < dgvInformationRecord.RowCount - 1 Then
            'Dim list As System.Collections.ObjectModel.
            ' ReadOnlyCollection(Of String)
            'list = My.Computer.FileSystem.FindInFiles(appPath + "\", DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("Manage Code").Value.ToString, True, FileIO.SearchOption.SearchTopLevelOnly)
            ''--------------------------------------------------------
            Dim value() As String
            'Dim line() As String
            'line = File.ReadAllLines(list(0))
            'For m As Integer = 0 To line.Length - 1 Step +1
            '    If line(m).Contains(DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("Manage Code").Value.ToString) Then
            '        value = line(m).ToString.Split("	")
            '    End If
            'Next m
            If mvFso_GetAllFileDat.Length > 0 Then
                For m As Integer = 0 To listDataGridView.Count - 1 Step +1
                    If listDataGridView(m).Contains(dgvInformationRecord.Rows(dgvInformationRecord.CurrentRow.Index).Cells("Manage Code").Value.ToString) Then
                        value = listDataGridView(m).Split(vbTab)
                        Exit For
                    End If
                Next
                'value = listDataGridView(dgvInformationRecord.CurrentRow.Index).Split("	")
                Dim Start As String
                Dim count As String
                Dim link As String
                Start = value(8)
                count = value(9)
                link = value(7)
                '-----------------------------------------------------------
                'Dim lineData() As String
                'lineData = File.ReadAllLines(appPath + "\" + link)
                Dim lineData As New List(Of String)
                Using sr As New System.IO.StreamReader(mvStr_AppPath + "\" + link, System.Text.Encoding.Default)          '1
                    Do While sr.Peek() <> -1
                        Dim Line As String = sr.ReadLine()
                        lineData.Add(Line)
                    Loop
                    sr.Close()
                End Using

                'Dong 0 
                Dim line0 As String = lineData(Convert.ToInt32(Start) - 1)
                Dim val0() As String = line0.ToString.Split("	")
                'Dong 1
                Dim line1 As New List(Of String)
                For temp As Integer = Convert.ToInt32(Start) To Convert.ToInt32(Start) + Convert.ToInt32(count) - 2 Step +1
                    Dim valline1() As String
                    valline1 = lineData(temp).Split("	")
                    If valline1(0) = "1" Then
                        line1.Add(lineData(temp))
                    End If
                Next temp
                'Dong 2 
                Dim line2 As New List(Of String)
                For tempLine2 As Integer = Convert.ToInt32(Start) + line2.Count To Convert.ToInt32(Start) + Convert.ToInt32(count) - 2 Step +1
                    Dim valline2() As String
                    valline2 = lineData(tempLine2).Split("	")
                    If valline2(0) = "2" Then
                        line2.Add(lineData(tempLine2))
                    End If
                Next tempLine2
                'Dong 3
                Dim line3 As String = lineData(Convert.ToInt32(Start) + Convert.ToInt32(count) - 2)
                Dim val3() As String = line3.ToString.Split("	")

                Dim i As Integer
                With dgvInformationRecord
                    If e.RowIndex >= 0 Then
                        i = .CurrentRow.Index
                        rtbReceiptDetail.AppendText(" [" + .Rows(i).Cells("WorkType").Value.ToString + "] ")
                        rtbReceiptDetail.AppendText(.Rows(i).Cells("Shop CODE").Value.ToString + "-")
                        rtbReceiptDetail.AppendText(.Rows(i).Cells("Pos NO").Value.ToString + "-")
                        rtbReceiptDetail.AppendText(.Rows(i).Cells("Receipt Code").Value.ToString + Environment.NewLine)
                        rtbReceiptDetail.AppendText("------------------------------------------------" + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Store Name                    " + vbTab + ": " + val0(9) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("POS NO                         " + vbTab + ": " + DeleteChar0(.Rows(i).Cells("Pos NO").Value.ToString) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Receipt Code                  " + vbTab + ": " + DeleteChar0(.Rows(i).Cells("Receipt Code").Value.ToString) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Work Day                       " + vbTab + ": " + dayFormat(.Rows(i).Cells("Date").Value.ToString) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Pos DayTime                  " + vbTab + ": " + val0(6) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("CD Staff                        " + vbTab + ": " + val0(15) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Name Staff                     " + vbTab + ": " + val0(16) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("CD Customer                  " + vbTab + ": " + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Name Customer              " + vbTab + ": " + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Total                             " + vbTab + ": " + line1.Count.ToString + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Total Money                    " + vbTab + ": " + val0(19) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Tổng phụ                       " + vbTab + ": " + val0(20) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Tiền đưa                        " + vbTab + ": " + val0(21) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Tiền thối                        " + vbTab + ": " + val0(22) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Không  thuế                   " + vbTab + ": " + val0(23) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Tiền thuế                       " + vbTab + ": " + val0(23) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Điểm sử dụng                 " + vbTab + ": " + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Điểm cộng                      " + vbTab + ": " + Environment.NewLine)
                        rtbReceiptDetail.AppendText("------------------------------------------------" + Environment.NewLine)

                        For l1 As Integer = 0 To line1.Count - 1 Step +1
                            Dim valline2() As String
                            valline2 = line1(l1).Split("	")
                            rtbReceiptDetail.AppendText("ID item                          " + vbTab + ": " + (l1 + 1).ToString + Environment.NewLine)
                            rtbReceiptDetail.AppendText("ID PLU                          " + vbTab + ": " + valline2(12) + Environment.NewLine)
                            rtbReceiptDetail.AppendText("Item Name                     " + vbTab + ": " + valline2(10) + Environment.NewLine)
                            rtbReceiptDetail.AppendText("Tax Type                       " + vbTab + ": " + valline2(11) + Environment.NewLine)
                            rtbReceiptDetail.AppendText("Total Sell                      " + vbTab + ": " + valline2(7) + Environment.NewLine)
                            rtbReceiptDetail.AppendText("Sau chiết khấu                 " + vbTab + ": " + valline2(8) + Environment.NewLine)
                            rtbReceiptDetail.AppendText("Tiền chiết khấu                " + vbTab + ": " + valline2(9) + Environment.NewLine)
                            rtbReceiptDetail.AppendText("------------------------------------------------" + Environment.NewLine)
                        Next l1

                        For l2 As Integer = 0 To line2.Count - 1 Step +1
                            Dim valline2() As String
                            valline2 = line2(l2).Split("	")
                            rtbReceiptDetail.AppendText("Loaị Thanh Toán              " + vbTab + ": " + valline2(11) + Environment.NewLine)
                            rtbReceiptDetail.AppendText("Số Tiền                          " + vbTab + ": " + valline2(5) + Environment.NewLine)
                            rtbReceiptDetail.AppendText("------------------------------------------------" + Environment.NewLine)
                        Next l2
                        rtbReceiptDetail.AppendText("Chiết khấu                      " + vbTab + ": " + val3(7) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Số Tiền                          " + vbTab + ": " + val3(5) + Environment.NewLine)
                    End If
                End With
            Else
                MsgBox("Not Found DATA FILE", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical,
               "Warning")
            End If
        Else
            rtbReceiptDetail.Clear()
        End If
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFontReceiptSize.SelectedIndexChanged
        rtbReceiptDetail.WordWrap = False
        Dim fontSize As String
        fontSize = cmbFontReceiptSize.SelectedItem.ToString
        rtbReceiptDetail.Font = New Font("MS UI Gothic", Convert.ToInt32(fontSize), FontStyle.Regular)
    End Sub
    Private Sub ComboBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbFontReceiptSize.KeyPress
        cmbFontReceiptSize.SelectAll()
        e.Handled = True
    End Sub
    Private Sub DataGridView1_KeyUp(sender As Object, e As KeyEventArgs) Handles dgvInformationRecord.KeyUp
        rtbReceiptDetail.Clear()
        If dgvInformationRecord.CurrentRow.Index < dgvInformationRecord.RowCount - 1 Then
            'RichTextBox1.AppendText(DataGridView1.CurrentRow.Index.ToString + vbTab + DataGridView1.RowCount.ToString)
            Dim value() As String
            If mvFso_GetAllFileDat.Length > 0 Then
                value = listDataGridView(dgvInformationRecord.CurrentRow.Index).Split("	")
                Dim Start As String
                Dim count As String
                Dim link As String
                Start = value(8)
                count = value(9)
                link = value(7)
                '-----------------------------------------------------------
                'Dim lineData() As String
                'lineData = File.ReadAllLines(appPath + "\" + link)
                Dim lineData As New List(Of String)
                Using sr As New System.IO.StreamReader(mvStr_AppPath + "\" + link, System.Text.Encoding.Default)          '1
                    Do While sr.Peek() <> -1
                        Dim Line As String = sr.ReadLine()
                        lineData.Add(Line)
                    Loop
                    sr.Close()
                End Using
                'Dong 0 
                Dim line0 As String = lineData(Convert.ToInt32(Start) - 1)
                Dim val0() As String = line0.ToString.Split("	")
                'Dong 1
                Dim line1 As New List(Of String)
                For temp As Integer = Convert.ToInt32(Start) To Convert.ToInt32(Start) + Convert.ToInt32(count) - 2 Step +1
                    Dim valline1() As String
                    valline1 = lineData(temp).Split("	")
                    If valline1(0) = "1" Then
                        line1.Add(lineData(temp))
                    End If
                Next temp
                'Dong 2 
                Dim line2 As New List(Of String)
                For tempLine2 As Integer = Convert.ToInt32(Start) + line2.Count To Convert.ToInt32(Start) + Convert.ToInt32(count) - 2 Step +1
                    Dim valline2() As String
                    valline2 = lineData(tempLine2).Split("	")
                    If valline2(0) = "2" Then
                        line2.Add(lineData(tempLine2))
                    End If
                Next tempLine2
                'Dong 3
                Dim line3 As String = lineData(Convert.ToInt32(Start) + Convert.ToInt32(count) - 2)
                Dim val3() As String = line3.ToString.Split("	")
                Dim i As Integer
                With dgvInformationRecord
                    i = .CurrentRow.Index
                    rtbReceiptDetail.AppendText(" [" + .Rows(i).Cells("WorkType").Value.ToString + "] ")
                    rtbReceiptDetail.AppendText(.Rows(i).Cells("Shop CODE").Value.ToString + "-")
                    rtbReceiptDetail.AppendText(.Rows(i).Cells("Pos NO").Value.ToString + "-")
                    rtbReceiptDetail.AppendText(.Rows(i).Cells("Receipt Code").Value.ToString + Environment.NewLine)
                    rtbReceiptDetail.AppendText("----------------------------------------------------------" + Environment.NewLine)

                    rtbReceiptDetail.AppendText("Store Name                    " + vbTab + ": " + val0(9) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("POS NO                         " + vbTab + ": " + DeleteChar0(.Rows(i).Cells("Pos NO").Value.ToString) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Receipt Code                  " + vbTab + ": " + DeleteChar0(.Rows(i).Cells("Receipt Code").Value.ToString) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Work Day                       " + vbTab + ": " + dayFormat(.Rows(i).Cells("Date").Value.ToString) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Pos DayTime                  " + vbTab + ": " + val0(6) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("CD Staff                        " + vbTab + ": " + val0(15) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Name Staff                     " + vbTab + ": " + val0(16) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("CD Customer                  " + vbTab + ": " + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Name Customer              " + vbTab + ": " + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Total                             " + vbTab + ": " + line1.Count.ToString + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Total Money                    " + vbTab + ": " + val0(19) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Tổng phụ                       " + vbTab + ": " + val0(20) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Tiền đưa                        " + vbTab + ": " + val0(21) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Tiền thối                        " + vbTab + ": " + val0(22) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Không  thuế                   " + vbTab + ": " + val0(23) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Tiền thuế                       " + vbTab + ": " + val0(23) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Điểm sử dụng                 " + vbTab + ": " + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Điểm cộng                      " + vbTab + ": " + Environment.NewLine)
                    rtbReceiptDetail.AppendText("----------------------------------------------------------" + Environment.NewLine)

                    For l1 As Integer = 0 To line1.Count - 1 Step +1
                        Dim valline2() As String
                        valline2 = line1(l1).Split("	")
                        rtbReceiptDetail.AppendText("ID item                          " + vbTab + ": " + (l1 + 1).ToString + Environment.NewLine)
                        rtbReceiptDetail.AppendText("ID PLU                          " + vbTab + ": " + valline2(12) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Item Name                     " + vbTab + ": " + valline2(10) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Tax Type                       " + vbTab + ": " + valline2(11) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Total Sell                      " + vbTab + ": " + valline2(7) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Sau chiết khấu                 " + vbTab + ": " + valline2(8) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Tiền chiết khấu                " + vbTab + ": " + valline2(9) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("----------------------------------------------------------" + Environment.NewLine)
                    Next l1

                    For l2 As Integer = 0 To line2.Count - 1 Step +1
                        Dim valline2() As String
                        valline2 = line2(l2).Split("	")
                        rtbReceiptDetail.AppendText("Loaị Thanh Toán              " + vbTab + ": " + valline2(11) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("Số Tiền                          " + vbTab + ": " + valline2(5) + Environment.NewLine)
                        rtbReceiptDetail.AppendText("----------------------------------------------------------" + Environment.NewLine)
                    Next l2
                    rtbReceiptDetail.AppendText("Chiết khấu                      " + vbTab + ": " + val3(7) + Environment.NewLine)
                    rtbReceiptDetail.AppendText("Số Tiền                          " + vbTab + ": " + val3(5) + Environment.NewLine)
                End With
            Else
                MsgBox("Not Found DATA FILE", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical,
                   "Warning")
            End If
        Else
            rtbReceiptDetail.Clear()
        End If
    End Sub
    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles mnsHelp.Click
        'FileOpen(1, appPath + "\jlview.hlp", OpenMode.Input)
        Process.Start(mvStr_AppPath + "\jlview.hlp")
    End Sub
    Private Sub ComboBox1_Click(sender As Object, e As EventArgs) Handles cmbFontReceiptSize.Click
        cmbFontReceiptSize.DroppedDown = True
    End Sub

    '---------------------------------------------------------------------------------------------------------
    'Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
    '    'Button1.Text = "Cancel (E)"
    '    listIdWorkTypeFromListBox = getIdWorkTypeFromListBox()
    '    mvInt_TotalTextBoxNotNull = 0
    '    If Check(txtShopCode.Text.ToString) Then
    '        mvInt_TotalTextBoxNotNull = mvInt_TotalTextBoxNotNull + 1
    '    End If
    '    If Check(txtPosNo.Text.ToString) Then
    '        mvInt_TotalTextBoxNotNull = mvInt_TotalTextBoxNotNull + 1
    '    End If
    '    If Check(txtReceiptCode.Text.ToString) Then
    '        mvInt_TotalTextBoxNotNull = mvInt_TotalTextBoxNotNull + 1
    '    End If
    '    Dim tableDataGrid As New DataTable()
    '    'Datagridview
    '    tableDataGrid.Columns.Add("WorkType", Type.GetType("System.String"))
    '    tableDataGrid.Columns.Add("Date", Type.GetType("System.String"))
    '    tableDataGrid.Columns.Add("Shop CODE", Type.GetType("System.String"))
    '    tableDataGrid.Columns.Add("Pos NO", Type.GetType("System.String"))
    '    tableDataGrid.Columns.Add("Receipt Code", Type.GetType("System.String"))
    '    tableDataGrid.Columns.Add("Manage Code", Type.GetType("System.String"))
    '    dgvInformationRecord.DataSource = tableDataGrid
    '    '--------------------------------------------------------------------
    '    'If Flag Mod 2 = 0 Then
    '    '    Button1.Text = "Search (S)"
    '    'Else
    '    '    Button1.Text = "Cancel (E)"
    '    'End If
    '    'Cancel
    '    '--------------------------------------------------------------------------
    '    'Search
    '    Dim list As List(Of String)
    '    list = GetAllLineAfterSearch()
    '    Dim valsDataGrid() As String
    '    For x As Integer = 0 To list.Count - 1 Step +1
    '        If tableDataGrid.Rows.Count > 2999 Then
    '            MsgBox("Not Over 3000", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
    '                "Warning")
    '            Exit For
    '        End If
    '        valsDataGrid = list(x).ToString().Split("	")
    '        listDataGridView.Add(list(x))
    '        Dim row(valsDataGrid.Length - 5) As String
    '        'row(0) = valsDataGrid(valsDataGrid.Length - 5).Trim()
    '        Dim typeWordId As String = ""
    '        typeWordId = valsDataGrid(valsDataGrid.Length - 5).Trim()
    '        row(0) = getIniWorkTypeName(lineWordDetail, typeWordId)
    '        '-------------------------------------------------------------------
    '        For y As Integer = 1 To valsDataGrid.Length - 5 Step +1
    '            row(y) = valsDataGrid(y - 1).Trim()
    '        Next y
    '        tableDataGrid.Rows.Add(row)
    '    Next x
    '    lblTotalRecord.Text = "Total Record: " + tableDataGrid.Rows.Count.ToString
    'End Sub
    ''get ALL line afterSeach By text
    'Private Function GetAllLineAfterSearch() As List(Of String)
    '    Dim day1 As Date
    '    Dim day2 As Date
    '    day1 = dtmStart.Value
    '    day2 = dtmFinish.Value
    '    Dim key1 As String = txtShopCode.Text
    '    Dim key2 As String = txtPosNo.Text
    '    Dim key3 As String = txtReceiptCode.Text
    '    If key1 <> "0" Then
    '        key1 = getTextValue(txtShopCode.Text, 4)
    '    End If
    '    If key2 <> "0" Then
    '        key2 = getTextValue(txtPosNo.Text, 2)
    '    End If
    '    If key3 <> "0" Then
    '        key3 = getTextValue(txtReceiptCode.Text, 8)
    '    End If
    '    Dim lineData As New List(Of String)
    '    For f As Integer = 0 To mvFso_GetAllFileIdx.Length - 1 Step +1
    '        Using objReader As New StreamReader(mvStr_AppPath + "\" + mvFso_GetAllFileIdx(f).Name)          '1
    '            Do While objReader.Peek() <> -1
    '                Dim Line As String = objReader.ReadLine()
    '                'Check TextBox---------------ListBox---------------DateTime--------------------------
    '                'If lineCheckText(Line, key1, key2, key3) And lineCheckListBox(Line) And lineCheckDateTime(Line, day1, day2) Then
    '                '    lineData.Add(Line)
    '                'End If
    '                '---------------------------------------------------------------------
    '                If lineCheckDateTime(Line, day1, day2) Then                                 '2'
    '                    If lineCheckText(Line, key1, key2, key3) Then                            '3'
    '                        If lineCheckListBox(Line) Then                                       '4
    '                            lineData.Add(Line)
    '                        End If
    '                    End If
    '                End If
    '            Loop
    '            objReader.Close()
    '        End Using
    '        'Using listStreamReaderIdx(f)
    '        '    Do While listStreamReaderIdx(f).Peek() <> -1
    '        '        Dim Line As String = listStreamReaderIdx(f).ReadLine()
    '        '        If lineCheckDateTime(Line, day1, day2) Then
    '        '            If lineCheckText(Line, key1, key2, key3) Then
    '        '                If lineCheckListBox(Line) Then
    '        '                    lineData.Add(Line)
    '        '                End If
    '        '            End If
    '        '        End If
    '        '    Loop
    '        'End Using
    '    Next f
    '    Return lineData
    'End Function
    '---------------------------------------------------------------------------------------------------------------
    Private index As Integer = 0
    Private Sub GetAllLineAfterSearch()

        If index Mod 2 = 0 Then
            lblTotalRecord.Text = "Total Record: "
            cmdSearch.Text = "Cancel(E)"
            index = index + 1
            Dim day1 As Date
            Dim day2 As Date
            day1 = dtmStart.Value
            day2 = dtmFinish.Value
            Dim key1 As String = txtShopCode.Text
            Dim key2 As String = txtPosNo.Text
            Dim key3 As String = txtReceiptCode.Text
            If key1 <> "0" Then
                key1 = getTextValue(txtShopCode.Text, 4)
            End If
            If key2 <> "0" Then
                key2 = getTextValue(txtPosNo.Text, 2)
            End If
            If key3 <> "0" Then
                key3 = getTextValue(txtReceiptCode.Text, 8)
            End If
            Dim tableDataGrid As New DataTable()
            'Datagridview
            tableDataGrid.Columns.Add("WorkType", Type.GetType("System.String"))
            tableDataGrid.Columns.Add("Date", Type.GetType("System.String"))
            tableDataGrid.Columns.Add("Shop CODE", Type.GetType("System.String"))
            tableDataGrid.Columns.Add("Pos NO", Type.GetType("System.String"))
            tableDataGrid.Columns.Add("Receipt Code", Type.GetType("System.String"))
            tableDataGrid.Columns.Add("Manage Code", Type.GetType("System.String"))
            dgvInformationRecord.DataSource = tableDataGrid
            Dim temp As Integer = 1
            For f As Integer = 0 To mvFso_GetAllFileIdx.Length - 1 Step +1
                Using objReader As New StreamReader(mvStr_AppPath + "\" + mvFso_GetAllFileIdx(f).Name)          '1
                    Do While objReader.Peek() <> -1
                        If index Mod 2 = 0 Then
                            Exit For
                        End If
                        temp = temp + 1
                        Dim Line As String = objReader.ReadLine()
                        If lineCheckDateTime(Line, day1, day2) Then                                 '2'
                            If lineCheckText(Line, key1, key2, key3) Then                            '3'
                                If lineCheckListBox(Line) Then                                       '4

                                    If dgvInformationRecord.Rows.Count > 3000 Then
                                        MsgBox("Not Over 3000", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                                        "Warning")
                                        index = index + 1
                                        cmdSearch.Text = "Search(S)"
                                        lblTotalRecord.Text = "Total Record: 3000"
                                        Exit For
                                    End If
                                    Dim valsDataGrid() As String
                                    valsDataGrid = Line.ToString().Split("	")
                                    listDataGridView.Add(Line)
                                    Dim row(valsDataGrid.Length - 5) As String
                                    Dim typeWordId As String = ""
                                    typeWordId = valsDataGrid(valsDataGrid.Length - 5).Trim()
                                    row(0) = getIniWorkTypeName(lineWordDetail, typeWordId)
                                    For y As Integer = 1 To valsDataGrid.Length - 5 Step +1
                                        row(y) = valsDataGrid(y - 1).Trim()
                                    Next y
                                    tableDataGrid.Rows.Add(row)
                                    'System.Threading.Thread.Sleep(0)
                                    'Application.DoEvents()
                                    'Application.ExitThread()
                                End If
                            End If
                        End If
                        If temp Mod 100 = 0 Then
                            Application.DoEvents()
                        End If
                    Loop
                    objReader.Close()
                End Using
            Next f

        Else
            index = index + 1
            cmdSearch.Text = "Search(S)"
            lblTotalRecord.Text = "Total Record: " + dgvInformationRecord.Rows.Count.ToString
        End If
    End Sub
    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click

        'Button1.Text = "Cancel (E)"
        listIdWorkTypeFromListBox = getIdWorkTypeFromListBox()
        mvInt_TotalTextBoxNotNull = 0
        If Check(txtShopCode.Text.ToString) Then
            mvInt_TotalTextBoxNotNull = mvInt_TotalTextBoxNotNull + 1
        End If
        If Check(txtPosNo.Text.ToString) Then
            mvInt_TotalTextBoxNotNull = mvInt_TotalTextBoxNotNull + 1
        End If
        If Check(txtReceiptCode.Text.ToString) Then
            mvInt_TotalTextBoxNotNull = mvInt_TotalTextBoxNotNull + 1
        End If
        GetAllLineAfterSearch()
    End Sub
End Class