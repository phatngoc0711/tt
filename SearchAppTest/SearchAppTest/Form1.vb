Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class Form1

    Private appPath As String = Application.StartupPath()
    Private di As New IO.DirectoryInfo(appPath)
    Private aryFileIdx As IO.FileInfo() = di.GetFiles("*.idx")
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
        'ComboBox
        ComboBox1.Items.Add("9")
        ComboBox1.Items.Add("12")
        ComboBox1.Items.Add("15")
        ComboBox1.Items.Add("18")
        ComboBox1.Items.Add("21")
        ComboBox1.Items.Add("24")
        ComboBox1.SelectedIndex = 0
        'Datagridview
        Dim tableDataGrid As New DataTable()
        tableDataGrid.Columns.Add("WorkType", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Date", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Shop CODE", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Pos NO", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Receipt Code", Type.GetType("System.String"))
        tableDataGrid.Columns.Add("Manage Code", Type.GetType("System.String"))
        DataGridView1.DataSource = tableDataGrid
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

        'RichtestBox Test
        Dim demo As List(Of String)
        'demo = getIniWorkTypeNameLine("[WorkTypeDetail]", "[DenpyoType]")
        demo = getDataLineAfterSearch("00000001")
        For k As Integer = 0 To demo.Count - 1 Step +1
            RichTextBox1.AppendText(demo(k))
            RichTextBox1.AppendText(vbNewLine)
        Next k


    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Dim frm As New Form2
        frm.Show()
    End Sub
    'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    'Cancel

    '    '---------------------------------------------------------------------------
    '    Dim lineDataGrid() As String
    '    Dim valsDataGrid() As String
    '    Dim Flag As Integer = 0
    '    For f As Integer = 0 To aryFileIdx.Length - 1 Step +1
    '        'lineDataGrid = File.ReadAllLines(appPath + "\20110501_20110531.IDX")
    '        lineDataGrid = File.ReadAllLines(appPath + "\" + aryFileIdx(f).Name)

    '        For x As Integer = 0 To lineDataGrid.Length - 1 Step +1
    '            'If tableDataGrid.Rows.Count > 2999 Then
    '            '    Flag = 1
    '            '    Exit For
    '            'End If
    '            valsDataGrid = lineDataGrid(x).ToString().Split("	")
    '            Dim row(valsDataGrid.Length - 5) As String


    '            'row(0) = valsDataGrid(valsDataGrid.Length - 5).Trim()
    '            Dim typeWordId As String = ""
    '            typeWordId = valsDataGrid(valsDataGrid.Length - 5).Trim()
    '            Dim lineWordDetail As List(Of String)
    '            lineWordDetail = getIniWorkTypeNameLine("[WorkTypeDetail]", "[DenpyoType]")
    '            row(0) = getIniWorkTypeName(lineWordDetail, typeWordId)
    '            '-------------------------------------------------------------------
    '            For y As Integer = 1 To valsDataGrid.Length - 5 Step +1
    '                row(y) = valsDataGrid(y - 1).Trim()
    '            Next y

    '            tableDataGrid.Rows.Add(row)
    '            If x Mod 2 <> 1 Then
    '                DataGridView1.Rows(x).DefaultCellStyle.BackColor = Color.LightGreen
    '            End If
    '        Next x
    '        If Flag = 1 Then
    '            MsgBox("Not Over 3000", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
    '                "Warning")
    '            Exit For
    '        End If
    '    Next f
    '    Label7.Text = "Total Record: " + tableDataGrid.Rows.Count.ToString
    'End Sub
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

    'Tim ra nhung line co Chua Key 2 
    ' Dua line nay thay the cho line o ben tren datagrid view
    'Du lieu dau vao la key (TEXTBOX)
    Private Function getDataLineAfterSearch(Key2 As String) As List(Of String)
        Dim lineData As New List(Of String)
        Dim lineDataGrid() As String
        Dim vals() As String
        For f As Integer = 0 To aryFileIdx.Length - 1 Step +1
            lineDataGrid = File.ReadAllLines(appPath + "\" + aryFileIdx(f).Name)
            For x As Integer = 0 To lineDataGrid.Length - 1 Step +1
                vals = lineDataGrid(x).ToString().Split("	")

                If vals(3).Trim() = Key2 Then
                    lineData.Add(lineDataGrid(x))
                End If
            Next x
        Next f

        Return lineData
    End Function
    'Get ALL line
    Private Function GetAllLine() As List(Of String)
        Dim lineData As New List(Of String)
        Dim lineDataGrid() As String
        For f As Integer = 0 To aryFileIdx.Length - 1 Step +1
            lineDataGrid = File.ReadAllLines(appPath + "\" + aryFileIdx(f).Name)
            For x As Integer = 0 To lineDataGrid.Length - 1 Step +1

                lineData.Add(lineDataGrid(x))
            Next x
        Next f
        Return lineData
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

        'Cancel
        '--------------------------------------------------------------------------
        'Search
        Dim list As List(Of String)
        If TextBox1.Text = "0" And TextBox2.Text = "0" And TextBox3.Text = "0" Then
            list = getDataLineAfterSearch(TextBox3.Text)
        Else
            list = getDataLineAfterSearch(TextBox3.Text)
        End If

        '---------------------------------------------------------------------------
        Dim valsDataGrid() As String
        'lineDataGrid = File.ReadAllLines(appPath + "\20110501_20110531.IDX")
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
            If x Mod 2 <> 1 Then
                DataGridView1.Rows(x).DefaultCellStyle.BackColor = Color.LightGreen
            End If
        Next x
        'If DataGridView1.Rows.Count > 0 Then
        '    For b As Integer = 0 To DataGridView1.Rows.Count - 1 Step +1
        '        If b Mod 2 <> 1 Then
        '            DataGridView1.Rows(b).DefaultCellStyle.BackColor = Color.LightGreen
        '        End If
        '    Next b
        'End If
        Label7.Text = "Total Record: " + tableDataGrid.Rows.Count.ToString

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
    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click

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
    End Sub
    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
    End Sub
    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
    End Sub

End Class