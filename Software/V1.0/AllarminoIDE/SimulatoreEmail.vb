Public Class SimulatoreEmail
    Private Sub Panel1_Click(sender As Object, e As EventArgs) Handles Panel1.Click
        Panel2.Visible = Not Panel2.Visible
    End Sub
    Private Sub Panel3_Click(sender As Object, e As EventArgs) Handles Panel3.Click
        Me.Close()
    End Sub

    Private Sub SimulatoreEmail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel2.MaximumSize = New Size(547, 196)
        Panel2.MinimumSize = New Size(547, 196)
        Panel2.Size = New Size(547, 196)
    End Sub
End Class