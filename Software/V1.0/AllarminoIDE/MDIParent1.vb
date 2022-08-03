Imports System.Windows.Forms

Public Class MDIParent1
    Dim NomeProgramma As String = Me.Text
    Dim Denominazione_Impaianto As String = "NULL"
    Dim Path_Progetto As String = ""
    Dim Versione As String = ""


    Dim Configutare As New Form1
    Dim AdminSession As New AdminSession
    Dim CertificatiTLS As New CertificatiTLS
    Dim MessaggiTLG As New MSGTelegram

    Dim settignsCompilatore As New settingsCompilatore
    Dim compilatoreProgetto As New Programmatore
    Private Sub MDIParent1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NomeProgramma = Me.Text
        Dim mainScreen As New SplashScreen
        Me.Enabled = False
        mainScreen.ShowDialog()
        Me.Enabled = True
    End Sub
    Private Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs) Handles NewToolStripMenuItem.Click, NewToolStripButton.Click

        Dim progetto As New NuovoProgetto
        If progetto.ShowDialog = DialogResult.OK Then
            Denominazione_Impaianto = progetto.NomeProgetto.Text
            Path_Progetto = progetto.PercorsoFile.Text & "\" & progetto.NomeProgetto.Text
            Versione = progetto.Versione.Text
            Me.Text = NomeProgramma & " - [" & Denominazione_Impaianto & "]"

            ViewMenu.Enabled = True
            ToolsMenu.Enabled = True
            WindowsMenu.Enabled = True

            version_enable(Versione)
        End If
    End Sub

    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click, OpenToolStripButton.Click
        FolderBrowserDialog1.SelectedPath = Environment.CurrentDirectory
        If (FolderBrowserDialog1.ShowDialog = DialogResult.OK) Then
            Path_Progetto = FolderBrowserDialog1.SelectedPath
            If IO.File.Exists(Path_Progetto & "\conf.ini") Then
                Dim confFile As String = IO.File.ReadAllText(Path_Progetto & "\conf.ini")
                If Not confFile.Contains("Progetto-Allarmino") Then
                    MessageBox.Show("Progetto non valido!!")
                    Return
                End If
                Dim confdetails() As String = confFile.Split(vbCrLf)
                Denominazione_Impaianto = confdetails(1)
                Me.Text = NomeProgramma & " - [" & Denominazione_Impaianto & "]"
                Versione = confdetails(2)
                ViewMenu.Enabled = True
                ToolsMenu.Enabled = True
                WindowsMenu.Enabled = True
                version_enable(Versione)
            Else
                MessageBox.Show("Progetto non valido!!")
            End If

        End If
    End Sub

    Private Sub version_enable(ByVal version As String) 'questa funziona ha il compito di abilitare e disabiltiare tutto cioè che non viene usato nella versione del firmware scelta
        'di base disabilita tutto
        ConfiguratoreToolStripMenuItem.Enabled = False
        AvanzatoToolStripMenuItem.Enabled = False
        AggiungiEspansioneToolStripMenuItem.Enabled = False
        ProgrammaModuliToolStripMenuItem.Enabled = False
        Select Case version
            Case "1.0V"
                ConfiguratoreToolStripMenuItem.Enabled = True
            Case "1.1V"
                ConfiguratoreToolStripMenuItem.Enabled = True
                AvanzatoToolStripMenuItem.Enabled = True
            Case Else 'nel caso di una versione sconosciuta abbilità tutti i tab
                ConfiguratoreToolStripMenuItem.Enabled = True
                AvanzatoToolStripMenuItem.Enabled = True
        End Select
    End Sub



    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
        Me.ToolStrip.Visible = Me.ToolBarToolStripMenuItem.Checked
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        Me.StatusStrip.Visible = Me.StatusBarToolStripMenuItem.Checked
    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ArrangeIconsToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Chiude tutti i form figlio del form padre.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub
    Private Sub ConfiguratoreToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles ConfiguratoreToolStripMenuItem.Click
        Try
            ' Imposta il form come figlio di questo form MDI prima di visualizzarlo.
            If Configutare.MdiParent IsNot Me Then Configutare.MdiParent = Me
            Configutare.Text = "Configuratore - " & Denominazione_Impaianto
            Configutare.Filedisetup = Path_Progetto & "\" & Versione & "\Setup\setup.h"
            Configutare.Show()
        Catch
            Configutare.Dispose()
            Configutare = New Form1
            ConfiguratoreToolStripMenuItem.PerformClick()
        End Try

    End Sub
    Private Sub AdminTGToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdminTGToolStripMenuItem.Click

        Try
            ' Imposta il form come figlio di questo form MDI prima di visualizzarlo.
            If AdminSession.MdiParent IsNot Me Then AdminSession.MdiParent = Me
            AdminSession.Text = "Configuratore Admin Telegram - " & Denominazione_Impaianto
            AdminSession.Filedisetup = Path_Progetto & "\" & Versione & "\Setup\setup.h"
            AdminSession.Show()
        Catch
            AdminSession.Dispose()
            AdminSession = New AdminSession
            AdminTGToolStripMenuItem.PerformClick()
        End Try
    End Sub
    Private Sub CertificatiTLSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CertificatiTLSToolStripMenuItem.Click

        Try
            ' Imposta il form come figlio di questo form MDI prima di visualizzarlo.
            If CertificatiTLS.MdiParent IsNot Me Then CertificatiTLS.MdiParent = Me
            CertificatiTLS.Text = "Certificati TLS - " & Denominazione_Impaianto
            CertificatiTLS.Filedisetup = Path_Progetto & "\" & Versione & "\Setup\ssl_cert.h"
            CertificatiTLS.Show()
        Catch
            CertificatiTLS.Dispose()
            CertificatiTLS = New CertificatiTLS
            CertificatiTLSToolStripMenuItem.PerformClick()
        End Try


    End Sub
    Private Sub NotificheTelegramToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NotificheTelegramToolStripMenuItem.Click
        Try
            ' Imposta il form come figlio di questo form MDI prima di visualizzarlo.
            If MessaggiTLG.MdiParent IsNot Me Then MessaggiTLG.MdiParent = Me
            MessaggiTLG.Filedisetup = Path_Progetto & "\" & Versione & "\Setup\CustomMessage.h"
            MessaggiTLG.Text = "Configurazione Messaggi Telegram [" & Denominazione_Impaianto & "]"
            MessaggiTLG.Show()
        Catch
            MessaggiTLG.Dispose()
            MessaggiTLG = New MSGTelegram
            ProgrammaToolStripItem.PerformClick()
        End Try
    End Sub
    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click

        Try
            ' Imposta il form come figlio di questo form MDI prima di visualizzarlo.
            If settignsCompilatore.MdiParent IsNot Me Then settignsCompilatore.MdiParent = Me
            settignsCompilatore.Show()
        Catch
            settignsCompilatore.Dispose()
            settignsCompilatore = New settingsCompilatore
            ToolStripMenuItem1.PerformClick()
        End Try
    End Sub



    Private Sub ProgrammaToolStripButton_Click(sender As Object, e As EventArgs)
        ProgrammaToolStripItem.PerformClick()
    End Sub

    Private Sub ProgrammaToolStripItem_Click(sender As Object, e As EventArgs) Handles ProgrammaToolStripItem.Click
        Try
            ' Imposta il form come figlio di questo form MDI prima di visualizzarlo.
            If compilatoreProgetto.MdiParent IsNot Me Then compilatoreProgetto.MdiParent = Me
            compilatoreProgetto.ProjectDir = Path_Progetto
            compilatoreProgetto.FirmwareVersion = Versione.Replace(".", "_").Replace("V", "_V")
            compilatoreProgetto.Text = "Programmazione [" & Denominazione_Impaianto & "]"
            compilatoreProgetto.Show()
            MessageBox.Show("Affinchè questa operazione vadi a buon fine è neccessario salvare e chiudere tutte le altre schede")
        Catch
            compilatoreProgetto.Dispose()
            compilatoreProgetto = New Programmatore
            ProgrammaToolStripItem.PerformClick()
        End Try
    End Sub

    Private Sub InfoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InfoToolStripMenuItem.Click
        Dim info As New Dialog2
        If info.ShowDialog = DialogResult.OK Then

        End If
    End Sub
End Class
