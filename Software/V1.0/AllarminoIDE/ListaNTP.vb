Public Class ListaNTP
    Public SelectedHost As String

    Dim dirfile As String = "\Resource\listsntp.allst"
    Private Sub ListaNTP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim reader As System.IO.StreamReader
        reader = IO.File.OpenText(Environment.CurrentDirectory & dirfile)
        Dim dati As String
        dati = reader.ReadToEnd
        Dim ObjectItem As String() = Split(dati, vbCrLf)
        For Each config As String In ObjectItem
            Try
                Dim subitems As String() = Split(config, "[&h&]")
                Dim items As New ListViewItem
                items.Text = subitems(0)
                items.SubItems.AddRange(New String() {subitems(1)})
                ListView1.Items.Add(items)
            Catch
            End Try
        Next
        reader.Close()
        reader.Dispose()
    End Sub
    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        SelectedHost = ListView1.SelectedItems(0).SubItems(1).Text
        Me.DialogResult = DialogResult.OK
    End Sub
End Class