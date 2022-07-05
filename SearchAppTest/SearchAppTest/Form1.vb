Imports System.IO

Public Class Form1
    Dim table As New DataTable()
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.Fill
        table.Columns.Add("ID", Type.GetType("System.String"))
        table.Columns.Add("FirstName", Type.GetType("System.String"))
        table.Columns.Add("LastName", Type.GetType("System.String"))
        table.Columns.Add("Age", Type.GetType("System.String"))
        DataGridView1.DataSource = table

        Dim line() As String
        Dim vals() As String

        line = File.ReadAllLines("C:\Users\phat0\Desktop\test.idx")

        For i As Integer = 0 To line.Length - 1 Step +1
            vals = line(i).ToString().Split("	")
            Dim row(vals.Length - 2) As String

            For j As Integer = 0 To vals.Length - 2 Step +1
                row(j) = vals(j).Trim()
            Next j
            table.Rows.Add(row)
            '-------------------------
            If table.Rows.Count > 1 Then
                Exit For
            End If
            '-------------------------
        Next i

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim form As New Form2
        form.Show()

    End Sub


End Class
