Public Class ListaSMTP
    Public SelectedPort As UShort
    Public SelectedHost As String

    Dim dirfile As String = "\Resource\listsmtp.allst"

    Private Sub ListaSMTP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim reader As System.IO.StreamReader
        reader = IO.File.OpenText(Environment.CurrentDirectory & dirfile)
        Dim dati As String
        dati = reader.ReadToEnd
        Dim ObjectItem As String() = Split(dati, vbCrLf)
        For Each config As String In ObjectItem
            Try
                Dim subitems As String() = Split(config, "[&h&]")
                Dim items As New ListViewItem
                items.Text = subitems(1)
                items.SubItems.AddRange(New String() {subitems(2), subitems(0)})
                ListView1.Items.Add(items)
            Catch
            End Try
        Next
        reader.Dispose()
        reader.Close()
    End Sub

    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        SelectedHost = ListView1.SelectedItems(0).SubItems(0).Text
        SelectedPort = ListView1.SelectedItems(0).SubItems(1).Text
        Me.DialogResult = DialogResult.OK
    End Sub
End Class