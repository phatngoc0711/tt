Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic

Public Class Form2

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'get name workType
        Dim line() As String
        line = File.ReadAllLines("C:\Users\phat0\Desktop\test.txt")

        For i As Integer = 0 To line.Length - 1 Step +1
            Dim sb As New StringBuilder()
            For Each c As Char In line(i)
                If [Char].IsLetter(c) Then
                    sb.Append(c)
                End If
            Next
            ListBox1.Items.Add(sb)
        Next i
        '---------------------------------------------------------------------

        TextBox1.TextAlign = HorizontalAlignment.Right
        TextBox1.MaxLength = 5
        TextBox2.TextAlign = HorizontalAlignment.Right
        TextBox2.MaxLength = 10

        '---------------------------------------------------------------------

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

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
    End Sub
End Class