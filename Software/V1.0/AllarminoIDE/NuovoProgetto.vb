Public Class NuovoProgetto
    Dim dirfile As String = "\Resource\Firmware"
    Private Sub NuovoProgetto_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        FolderBrowserDialog1.SelectedPath = Environment.CurrentDirectory & "\Progetti"
        If IO.Directory.Exists(Environment.CurrentDirectory & "\Progetti") Then
            IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\Progetti")
        End If
        PercorsoFile.Text = Environment.CurrentDirectory & "\Progetti"
        For Each version As String In IO.Directory.GetDirectories(Environment.CurrentDirectory & dirfile)
            Versione.Items.Add(version.Replace(Environment.CurrentDirectory & dirfile & "\", ""))
        Next
        Versione.SelectedIndex = Versione.Items.Count - 1
    End Sub
    Private Sub Panel1_Click(sender As Object, e As EventArgs) Handles Panel1.Click
        'FolderBrowserDialog1.SelectedPath = Environment.CurrentDirectory
        If (FolderBrowserDialog1.ShowDialog = DialogResult.OK) Then
            PercorsoFile.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
    Private Sub Panel2_Click(sender As Object, e As EventArgs) Handles Panel2.Click
        If NomeProgetto.Text.Replace(" ", "") = "" Then
            MessageBox.Show("Inserire un nome valido al progetto")
            Return
        End If
        If PercorsoFile.Text.Replace(" ", "") = "" Then
            MessageBox.Show("Inserire un percorso valido al progetto")
            Return
        End If
        If IO.Directory.Exists(Environment.CurrentDirectory & "\" & NomeProgetto.Text) Then
            MessageBox.Show("Il progetto già esiste, cambiare nome")
        Else
            Try
                DirectoryCopy(Environment.CurrentDirectory & dirfile & "\" & Versione.Text, PercorsoFile.Text & "\" & NomeProgetto.Text & "\" & Versione.Text)
                IO.File.WriteAllText(PercorsoFile.Text & "\" & NomeProgetto.Text & "\conf.ini", "Progetto-Allarmino" & vbCrLf & NomeProgetto.Text & vbCrLf & Versione.Text)

            Catch ex As IO.DirectoryNotFoundException
                MessageBox.Show("Errore creazione progetto : " & vbCrLf & ex.ToString)
            End Try
            Me.DialogResult = DialogResult.OK
        End If
    End Sub


End Class