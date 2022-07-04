Public Class Form1
    Dim table As New DataTable()
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        table.Columns.Add("ID", Type.GetType("System.Int32"))
        table.Columns.Add("FirstName", Type.GetType("System.Int32"))
        table.Columns.Add("LastName", Type.GetType("System.Int32"))
        table.Columns.Add("Age", Type.GetType("System.Int32"))
        DataGridView1.DataSource = table
    End Sub
End Class
