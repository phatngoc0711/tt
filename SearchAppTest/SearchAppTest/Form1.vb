Imports System.IO

Public Class Form1
    Dim table As New DataTable()
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        table.Columns.Add("ID", Type.GetType("System.String"))
        table.Columns.Add("FirstName", Type.GetType("System.String"))
        table.Columns.Add("LastName", Type.GetType("System.String"))
        table.Columns.Add("Age", Type.GetType("System.String"))
        DataGridView1.DataSource = table

        Dim line() As String
        Dim vals() As String

        line = File.ReadAllLines("C:\Users\phat0\Desktop\test.idx")

        For i As Integer = 0 To line.Length - 1 Step +1
            vals = line(i).ToString().Split(" ")
            Dim row(vals.Length - 1) As String

            For j As Integer = 0 To vals.Length - 1 Step +1
                row(j) = vals(j).Trim()
            Next j
            table.Rows.Add(row)
        Next i

    End Sub
End Class
