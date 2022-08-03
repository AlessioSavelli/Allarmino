Public Class CertificatiTLS
    Dim loader As New PopupAttesa
    Public Filedisetup As String = ""
    Dim needToSave As Boolean = True
    Private Sub CertificatiTLS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loader.Show()
        Dim t As New Threading.Thread(AddressOf load_file)
        t.SetApartmentState(Threading.ApartmentState.STA)
        t.IsBackground = False
        Threading.Thread.Sleep(100)
        t.Start()


    End Sub
    Private Sub CertificatiTLS_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If needToSave Then
            e.Cancel = True
            Dim th As New Threading.Thread(AddressOf alternativeClosing)
            th.SetApartmentState(Threading.ApartmentState.STA)
            th.IsBackground = False
            th.Start()
        End If
    End Sub
    Private Sub alternativeClosing()
        Invoke(Sub() write_file())
        needToSave = False
        Invoke(Sub() Me.Close())
    End Sub
    Private Sub write_file()
        Dim StremOutput As IO.StreamWriter
        IO.File.Delete(Filedisetup)
        StremOutput = IO.File.CreateText(Filedisetup)
        StremOutput.WriteLine(source.Text) ' scrive le linee senza modificare nulla
        StremOutput.Dispose()
        StremOutput.Close()

    End Sub
    Private Sub load_file()
        Invoke(Sub() source.Text = IO.File.ReadAllText(Filedisetup))
        Invoke(Sub() colorTextbox())
        Invoke(Sub() loader.Close())
    End Sub
    Private Sub colorTextbox()
        colorWord("#ifndef", Color.SaddleBrown)
        colorWord("#define", Color.SaddleBrown)
        colorWord("#ifdef", Color.SaddleBrown)
        colorWord("#endif", Color.SaddleBrown)
        colorWord("#define", Color.SaddleBrown)
        colorWord("_TAs_NUM", Color.SaddleBrown)


        colorWord("""", Color.DarkGray)
        colorWord("/*", Color.Green)
        colorWord("*/", Color.Green)
        colorWord("*", Color.Green)

        colorWord("extern", Color.BlueViolet)
        colorWord("static", Color.BlueViolet)
        colorWord("const", Color.BlueViolet)
        colorWord("unsigned", Color.BlueViolet)
        colorWord("char", Color.BlueViolet)
        colorWord("sizeof", Color.BlueViolet)


        colorWord("0x", Color.Gray)

        colorWord("br_x509_trust_anchor", Color.Lime)
        colorWord("BR_X509_TA_CA", Color.Lime)
        colorWord("BR_KEYTYPE_RSA", Color.Lime)

        colorWord("_TA_RSA_N0", Color.DarkOrange)
        colorWord("_TA_RSA_E0", Color.DarkOrange)
        colorWord("_TAs", Color.DarkOrange)

    End Sub
    Private Sub colorWord(ByVal word As String, ByVal color As Color)
        For i As Integer = 0 To source.TextLength
            Try
                If source.Text.ElementAt(i).ToString = word.ElementAt(0).ToString Then
                    Dim found As Boolean = False
                    For j As Integer = 1 To word.Count - 1
                        If source.Text.ElementAt(i + j) = word.ElementAt(j) Then
                            found = True
                        Else
                            found = False
                            Exit For
                        End If
                    Next
                    If found = True Then
                        source.Select(i, word.Length)
                        source.SelectionColor = color
                    End If
                End If
            Catch ex As Exception
                Continue For
            End Try
        Next
    End Sub

    Private Sub source_TextChanged(sender As Object, e As EventArgs) Handles source.TextChanged
    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If MessageBox.Show("Funziona non acnora disponibile in questa versione." & vbCrLf & "Usa il seguente link e modifica il codice sorgente nella sezione avanzato" & vbCrLf & "https://openslab-osu.github.io/bearssl-certificate-utility/" & vbCrLf & vbCrLf & "Vuoi aprire il link?", "Attenzione", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Process.Start("cmd.exe", "https://openslab-osu.github.io/bearssl-certificate-utility/")

        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MessageBox.Show("Funziona non acnora disponibile in questa versione." & vbCrLf & "Usa il seguente link e modifica il codice sorgente nella sezione avanzato" & vbCrLf & "https://openslab-osu.github.io/bearssl-certificate-utility/" & vbCrLf & vbCrLf & "Vuoi aprire il link?", "Attenzione", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Process.Start("cmd.exe", "https://openslab-osu.github.io/bearssl-certificate-utility/")

        End If
    End Sub


End Class