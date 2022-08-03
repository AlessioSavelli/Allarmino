Imports System.ComponentModel
Imports System.Text

Public Class Form1
    Dim loader As New PopupAttesa
    Dim allLineSetup() As String
    Public Filedisetup As String = ""
    Dim needToSave As Boolean = True
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Enabled = False
        Input1.SelectedIndex = 1
        Input2.SelectedIndex = 2
        Output1.SelectedIndex = 5
        Output2.SelectedIndex = 5

        loader.Show()
        If load_setup() = False Then
            MessageBox.Show("File di setup non valido o corrotto!")
            loader.Close()
            Me.Close()
        Else
            Me.Enabled = True
        End If
        loader.Close()
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If needToSave Then
            e.Cancel = True
            Dim th As New Threading.Thread(AddressOf alternativeClosing)
            th.SetApartmentState(Threading.ApartmentState.STA)
            th.IsBackground = False
            th.Start()
        End If
    End Sub
    Private Sub alternativeClosing()
        Invoke(Sub() write_setup())
        needToSave = False
        Invoke(Sub() Me.Close())
    End Sub

#Region "gestione file di setup"
    Private Sub input_tocombox(ByRef value As String, ByRef box As ComboBox)
        Select Case value
            Case "INPUT_AS_NONE"
                box.SelectedIndex = 0
            Case "INPUT_AS_KEY"
                box.SelectedIndex = 1
            Case "INPUT_AS_POWER_CHECK"
                box.SelectedIndex = 2
        End Select
    End Sub
    Private Function input_fromcombox(ByRef box As ComboBox) As String
        Select Case box.SelectedIndex
            Case 0
                Return "INPUT_AS_NONE"
            Case 1
                Return "INPUT_AS_KEY"
            Case 2
                Return "INPUT_AS_POWER_CHECK"
        End Select
        Return "INPUT_AS_NONE"
    End Function
    Private Sub output_tocombox(ByRef value As String, ByRef box As ComboBox)
        Select Case value
            Case "OUTPUT_AS_ST_ALLARM"
                box.SelectedIndex = 0
            Case "OUTPUT_AS_NO_INTERNET"
                box.SelectedIndex = 1
            Case "OUTPUT_AS_NO_POWER"
                box.SelectedIndex = 2
            Case "OUTPUT_AS_TAMPER_ESCLUSO"
                box.SelectedIndex = 3
            Case "OUTPUT_AS_TAMPER_ALLARME"
                box.SelectedIndex = 4
            Case "OUTPUT_AS_TELEGRAM"
                box.SelectedIndex = 5
        End Select
    End Sub
    Private Function output_fromcombox(ByRef box As ComboBox) As String
        Select Case box.SelectedIndex
            Case 0
                Return "OUTPUT_AS_ST_ALLARM"
            Case 1
                Return "OUTPUT_AS_NO_INTERNET"
            Case 2
                Return "OUTPUT_AS_NO_POWER"
            Case 3
                Return "OUTPUT_AS_TAMPER_ESCLUSO"
            Case 4
                Return "OUTPUT_AS_TAMPER_ALLARME"
            Case 5
                Return "OUTPUT_AS_TELEGRAM"
        End Select
        Return "OUTPUT_AS_TELEGRAM"
    End Function
    Private Function found_value(ByRef array() As String, Optional ByVal index As Int16 = 2) As ObjValue
        For index = index To array.Length - 1
            If array(index) <> "" Then
                If (array(index).Contains("""") And Not array(index).EndsWith("""")) Or (array(index).Contains("""" & "//")) Then  ' se contiene le apici allora è una stringa e legge tutto anche gli spazi
                    Dim ricostruisci As String = array(index)
                    For x = index + 1 To array.Length - 1
                        If array(x).Contains("""") Then
                            ricostruisci = ricostruisci & " " & array(x).Split("//")(0) ' rimuove eventuali commenti inseriti subito dopo il testo da prendere , ES "mypassword"//commento inserito senza spazio
                            Return New ObjValue(ricostruisci, index + x)
                        Else
                            ricostruisci = ricostruisci & " " & array(x)
                        End If
                    Next
                End If
                Return New ObjValue(array(index), index)
            End If
        Next
        Return Nothing
    End Function
    Private Function load_setup() As Boolean

        If Not IO.File.Exists(Filedisetup) Then
            Return False
        End If
        Dim GET_SETUP_VERSION As String = "1.0V"
        Dim setupinput As String = IO.File.ReadAllText(Filedisetup)
        allLineSetup = Split(setupinput, vbCrLf)
        For Each line As String In allLineSetup
            Dim parmsrow() As String = Split(line.Replace("""" & "//", """" & " //"), " ")
            If parmsrow.Length > 1 Then
                Select Case parmsrow(1).Replace("#define ", "")
                    Case "FW_VERSION"
                        GET_SETUP_VERSION = found_value(parmsrow).Text
                    Case "INPUT1_AS_EVENT"
                        input_tocombox(found_value(parmsrow).Text, Input1)
                    Case "INPUT2_AS_EVENT"
                        input_tocombox(found_value(parmsrow).Text, Input2)
                    Case "OUTPUT1_AS_EVENT"
                        output_tocombox(found_value(parmsrow).Text, Output1)
                    Case "OUTPUT2_AS_EVENT"
                        output_tocombox(found_value(parmsrow).Text, Output2)
                    Case "MAX_TIME_SIRENA_ON"
                        Dim objval As ObjValue = found_value(parmsrow)
                        Dim nr As Int16 = CInt(objval.Text)
                        objval = found_value(parmsrow, objval.offset + 1)
                        Dim secndval As String = objval.Text.Replace("*", "")
                        If secndval = "MINUTES" Then nr = nr * 60
                        MaxTimeSirena.Value = nr
                    Case "CYCLE_TIMER_SIRENA_ON"
                        Dim objval As ObjValue = found_value(parmsrow)
                        Dim nr As Int16 = CInt(objval.Text)
                        objval = found_value(parmsrow, objval.offset + 1)
                        Dim secndval As String = objval.Text.Replace("*", "")
                        If secndval = "MINUTES" Then nr = nr * 60
                        TimerCicloSirena.Value = nr
                    Case "SIRENA_ON_DELAYED"
                        Dim objval As ObjValue = found_value(parmsrow)
                        Dim nr As Int16 = CInt(objval.Text)
                        objval = found_value(parmsrow, objval.offset + 1)
                        Dim secndval As String = objval.Text.Replace("*", "")
                        If secndval = "MINUTES" Then nr = nr * 60
                        TempoIngresso.Value = nr
                    Case "ALLARME_ON_DELAYED"
                        Dim objval As ObjValue = found_value(parmsrow)
                        Dim nr As Int16 = CInt(objval.Text)
                        objval = found_value(parmsrow, objval.offset + 1)
                        Dim secndval As String = objval.Text.Replace("*", "")
                        If secndval = "MINUTES" Then nr = nr * 60
                        TempoUscita.Value = nr
                    Case "ZONA1_ESCLUDIBILE"
                        If found_value(parmsrow).Text = "true" Then
                            Zona1Escludibile.Checked = True
                        End If
                    Case "ZONA2_ESCLUDIBILE"
                        If found_value(parmsrow).Text = "true" Then
                            Zona2Escludibile.Checked = True
                        End If
                    Case "ZONA3_ESCLUDIBILE"
                        If found_value(parmsrow).Text = "true" Then
                            Zona3Escludibile.Checked = True
                        End If
                    Case "ZONA4_ESCLUDIBILE"
                        If found_value(parmsrow).Text = "true" Then
                            Zona4Escludibile.Checked = True
                        End If
                    Case "TAMPER_ESCLUDIBILE"
                        If found_value(parmsrow).Text = "true" Then
                            TamperEscludibile.Checked = True
                        End If
                    Case "DEBOUNCE_ZONA1"
                        Zona1Debounce.Value = CInt(found_value(parmsrow).Text)
                    Case "DEBOUNCE_ZONA2"
                        Zona2Debounce.Value = CInt(found_value(parmsrow).Text)
                    Case "DEBOUNCE_ZONA3"
                        Zona3Debounce.Value = CInt(found_value(parmsrow).Text)
                    Case "DEBOUNCE_ZONA4"
                        Zona4Debounce.Value = CInt(found_value(parmsrow).Text)
                    Case "DEBOUNCE_TAMPER"
                        Tamper4Debounce.Value = CInt(found_value(parmsrow).Text)
                    Case "DEBOUNCE_INPUT1"
                         'non fatto
                    Case "DEBOUNCE_INPUT2"
                        'non fatto
                    Case "WiFi_SSID"
                        WiFiSSID.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "WiFi_PASS"
                        WiFiPassword.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "WiFi_CONNECTION_TRIES_DELAY"
                        WiFiTriesDelay.Value = CInt(found_value(parmsrow).Text)
                    Case "WiFi_CONNECTION_ERROR_AFTER_TRIES"
                        WiFiErrorTryes.Value = CInt(found_value(parmsrow).Text)
                    Case "ENABLE_TELEGRAM"
                        If Not parmsrow(0).Contains("//") Then
                            EnableTelegram.Checked = True
                        End If
                    Case "TELEGRAM_TOKEN"
                        TelegramToken.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "TELEGRAM_GROUP_NAME"
                        TelegramGroup.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "TELEGRAM_GROUP_ID"
                        TelegramGroupID.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "GMT_TIME"
                        GMTStnd.Value = CInt(found_value(parmsrow).Text)
                    Case "NTP_SERVER"
                        NTPServer.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "AUTO_ADJUSTMENT"
                        If Not parmsrow(0).Contains("//") Then
                            NTPAutoAdj.Checked = True
                        End If
                    Case "GMT_AUTOADJUSTMENT"
                        GMTAutoadj.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "ENABLE_EMAIL"
                        If Not parmsrow(0).Contains("//") Then
                            EnableEmail.Checked = True
                        End If
                    Case "SMTP_SENDER_NAME"
                        SMTP_SENDER_NAME.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "SMTP_HOST"
                        SMTP_HOST.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "SMTP_PORT"
                        SMTP_PORT.Value = CInt(found_value(parmsrow).Text)
                    Case "SMTP_AUTHOR_EMAIL"
                        SMTP_AUTHOR_EMAIL.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "SMTP_AUTHOR_PASSWORD"
                        SMTP_AUTHOR_PASSWORD.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "SMTP_USER_DOMAIN"
                    Case "RECIPIENT_EMAIL"
                        RECIPIENT_EMAIL.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "ORARIO_INVIO_REPORT"
                        Dim orario() As String = found_value(parmsrow).Text.Replace("""", "").Split(":")
                        OraReport.Value = CInt(orario(0))
                        ReportMinuti.Value = CInt(orario(1))
                    Case "INTERNET_MODE"
                        SelectEthernet.Visible = True
                        If found_value(parmsrow).Text = "INTERNET_Eth" Then
                            SelectEthernet.Checked = True
                        End If
                    Case "ETHERNET_CHIPSET"
                        ChipSetSelection.SelectedIndex = 1
                        If found_value(parmsrow).Text = "EHT_CHIPSET_W5500" Then
                            ChipSetSelection.SelectedIndex = 0
                        Else
                            ChipSetSelection.SelectedIndex = 1
                        End If
                    Case "ETH_MAC"
                        EthMAC.Text = found_value(parmsrow).Text.Replace(" ", "")
                    Case "ETH_SPI_PORT"
                        If found_value(parmsrow).Text = "CS2_SPI" Then
                            EthSpiSelected.SelectedIndex = 1
                        Else
                            EthSpiSelected.SelectedIndex = 0
                        End If
                    Case "TELEGRAM_PASSWORD_INSTALLATORE"

                    Case "TELEGRAM_INSTALLATORE_IDLE"

                End Select



            End If

        Next
        Return True
    End Function
    Private Function write_setup() As Boolean
        'in allLineSetup è già caricato il file, ora lo modifichiamo 
        Dim StremOutput As IO.StreamWriter
        IO.File.Delete(Filedisetup)
        StremOutput = IO.File.CreateText(Filedisetup)
        For Each line As String In allLineSetup
            Dim parmsrow() As String = Split(line, " ")
            If parmsrow.Length > 1 Then
                Select Case parmsrow(1).Replace("#define ", "")
                    Case "INPUT1_AS_EVENT"
                        line = line.Replace(found_value(parmsrow).Text, input_fromcombox(Input1))
                    Case "INPUT2_AS_EVENT"
                        line = line.Replace(found_value(parmsrow).Text, input_fromcombox(Input2))
                    Case "OUTPUT1_AS_EVENT"
                        line = line.Replace(found_value(parmsrow).Text, output_fromcombox(Output1))
                    Case "OUTPUT2_AS_EVENT"
                        line = line.Replace(found_value(parmsrow).Text, output_fromcombox(Output2))
                    Case "MAX_TIME_SIRENA_ON"
                        Dim objval As ObjValue = found_value(parmsrow)
                        Dim nr As Int16 = CInt(objval.Text)
                        line = line.Replace(nr, MaxTimeSirena.Value).Replace("*MINUTES", "*SECONDS").Replace("* MINUTES", "* SECONDS")
                    Case "CYCLE_TIMER_SIRENA_ON"
                        Dim objval As ObjValue = found_value(parmsrow)
                        Dim nr As Int16 = CInt(objval.Text)
                        line = line.Replace(nr, TimerCicloSirena.Value).Replace("*MINUTES", "*SECONDS").Replace("* MINUTES", "* SECONDS")
                    Case "SIRENA_ON_DELAYED"
                        Dim objval As ObjValue = found_value(parmsrow)
                        Dim nr As Int16 = CInt(objval.Text)
                        line = line.Replace(nr, TempoIngresso.Value).Replace("*MINUTES", "*SECONDS").Replace("* MINUTES", "* SECONDS")
                    Case "ALLARME_ON_DELAYED"
                        Dim objval As ObjValue = found_value(parmsrow)
                        Dim nr As Int16 = CInt(objval.Text)
                        line = line.Replace(nr, TempoUscita.Value).Replace("*MINUTES", "*SECONDS").Replace("* MINUTES", "* SECONDS")
                    Case "ZONA1_ESCLUDIBILE"
                        Dim val As String = "false"
                        If Zona1Escludibile.Checked Then val = "true"
                        line = line.Replace(found_value(parmsrow).Text, val)
                    Case "ZONA2_ESCLUDIBILE"
                        Dim val As String = "false"
                        If Zona2Escludibile.Checked Then val = "true"
                        line = line.Replace(found_value(parmsrow).Text, val)
                    Case "ZONA3_ESCLUDIBILE"
                        Dim val As String = "false"
                        If Zona3Escludibile.Checked Then val = "true"
                        line = line.Replace(found_value(parmsrow).Text, val)
                    Case "ZONA4_ESCLUDIBILE"
                        Dim val As String = "false"
                        If Zona4Escludibile.Checked Then val = "true"
                        line = line.Replace(found_value(parmsrow).Text, val)
                    Case "TAMPER_ESCLUDIBILE"
                        Dim val As String = "false"
                        If TamperEscludibile.Checked Then val = "true"
                        line = line.Replace(found_value(parmsrow).Text, val)
                    Case "DEBOUNCE_ZONA1"
                        line = line.Replace(found_value(parmsrow).Text, Zona1Debounce.Value)
                    Case "DEBOUNCE_ZONA2"
                        line = line.Replace(found_value(parmsrow).Text, Zona2Debounce.Value)
                    Case "DEBOUNCE_ZONA3"
                        line = line.Replace(found_value(parmsrow).Text, Zona3Debounce.Value)
                    Case "DEBOUNCE_ZONA4"
                        line = line.Replace(found_value(parmsrow).Text, Zona4Debounce.Value)
                    Case "DEBOUNCE_TAMPER"
                        line = line.Replace(found_value(parmsrow).Text, Tamper4Debounce.Value)
                    Case "DEBOUNCE_INPUT1"
                         'non fatto
                    Case "DEBOUNCE_INPUT2"
                        'non fatto
                    Case "WiFi_SSID"
                        replace_str_value(line, parmsrow, WiFiSSID.Text)
                    Case "WiFi_PASS"
                        replace_str_value(line, parmsrow, WiFiPassword.Text)
                    Case "WiFi_CONNECTION_TRIES_DELAY"
                        line = line.Replace(found_value(parmsrow).Text, WiFiTriesDelay.Value)
                    Case "WiFi_CONNECTION_ERROR_AFTER_TRIES"
                        line = line.Replace(found_value(parmsrow).Text, WiFiErrorTryes.Value)
                    Case "ENABLE_TELEGRAM"
                        If EnableTelegram.Checked Then
                            If parmsrow(0).Contains("//") Then line = line.Replace("//#define ", "#define ")
                        Else
                            If Not parmsrow(0).Contains("//") Then line = line.Replace("#define ", "//#define ")
                        End If
                    Case "TELEGRAM_TOKEN"
                        replace_str_value(line, parmsrow, TelegramToken.Text)
                    Case "TELEGRAM_GROUP_NAME"
                        replace_str_value(line, parmsrow, TelegramGroup.Text)
                    Case "TELEGRAM_GROUP_ID"
                        replace_str_value(line, parmsrow, TelegramGroupID.Text)
                    Case "GMT_TIME"
                        line = line.Replace(found_value(parmsrow).Text, GMTStnd.Value)
                    Case "NTP_SERVER"
                        replace_str_value(line, parmsrow, NTPServer.Text)
                    Case "AUTO_ADJUSTMENT"
                        If NTPAutoAdj.Checked Then
                            If parmsrow(0).Contains("//") Then line = line.Replace("//#define ", "#define ")
                        Else
                            If Not parmsrow(0).Contains("//") Then line = line.Replace("#define ", "//#define ")
                        End If
                    Case "GMT_AUTOADJUSTMENT"
                        replace_str_value(line, parmsrow, GMTAutoadj.Text)
                    Case "ENABLE_EMAIL"
                        If EnableEmail.Checked Then
                            If parmsrow(0).Contains("//") Then line = line.Replace("//#define ", "#define ")
                        Else
                            If Not parmsrow(0).Contains("//") Then line = line.Replace("#define ", "//#define ")
                        End If
                    Case "SMTP_SENDER_NAME"
                        replace_str_value(line, parmsrow, SMTP_SENDER_NAME.Text)
                    Case "SMTP_HOST"
                        replace_str_value(line, parmsrow, SMTP_HOST.Text)
                    Case "SMTP_PORT"
                        replace_str_value(line, parmsrow, SMTP_PORT.Value)
                    Case "SMTP_AUTHOR_EMAIL"
                        replace_str_value(line, parmsrow, SMTP_AUTHOR_EMAIL.Text)
                    Case "SMTP_AUTHOR_PASSWORD"
                        replace_str_value(line, parmsrow, SMTP_AUTHOR_PASSWORD.Text)
                    Case "SMTP_USER_DOMAIN"
                    Case "RECIPIENT_EMAIL"
                        replace_str_value(line, parmsrow, RECIPIENT_EMAIL.Text)
                    Case "ORARIO_INVIO_REPORT"
                        Dim orario() As String = found_value(parmsrow).Text.Replace("""", "").Split(":")
                        OraReport.Value = CInt(orario(0))
                        ReportMinuti.Value = CInt(orario(1))
                        Dim h, m As String
                        h = OraReport.Value
                        m = ReportMinuti.Value
                        If OraReport.Value = 0 Then h = "00"
                        If ReportMinuti.Value = 0 Then m = "00"
                        line = line.Replace(orario(0) & ":" & orario(1), h & ":" & m)

                    Case "INTERNET_MODE"
                        Dim type As String = "INTERNET_WiFi"
                        If SelectEthernet.Checked Then
                            type = "INTERNET_Eth"
                        End If
                        replace_str_value(line, parmsrow, type)
                    Case "ETHERNET_CHIPSET"
                        Dim type As String = "EHT_CHIPSET_ENC28J60"
                        If ChipSetSelection.SelectedIndex = 0 Then
                            type = "EHT_CHIPSET_W5500"
                        End If
                        replace_str_value(line, parmsrow, type)
                    Case "ETH_MAC"
                        If EthMAC.Text.Replace(" ", "") = "" Then
                            RandomMac.PerformClick()
                        End If
                        line = "#define ETH_MAC" & vbTab & EthMAC.Text
                        'replace_str_value(line, parmsrow, EthMAC.Text)
                    Case "ETH_SPI_PORT"
                        Dim type As String = "CS1_SPI"
                        If EthSpiSelected.SelectedIndex = 1 Then
                            type = "CS2_SPI"
                        End If
                End Select
            End If
            StremOutput.WriteLine(line) ' scrive le linee senza modificare nulla
        Next
        StremOutput.Dispose()
        StremOutput.Close()

        Return True
    End Function
    Public Sub replace_str_value(ByRef line As String, ByRef parmsrow As String(), ByVal newValue As String) 'cerca e sostituisce il valore del Define associato
        Dim str As String = found_value(parmsrow).Text
        If str.Replace("""", "") = "" Then
            line = line.Replace(str, """" & newValue & """")
        Else
            line = line.Replace(str.Replace("""", ""), newValue)
        End If
    End Sub

#End Region
    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles MaxTimeSirena.ValueChanged
        TimerCicloSirena.Maximum = MaxTimeSirena.Value
    End Sub

    Private Sub ShowWiFiPass_CheckedChanged(sender As Object, e As EventArgs) Handles ShowWiFiPass.CheckedChanged
        WiFiPassword.UseSystemPasswordChar = Not ShowWiFiPass.Checked
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        GMTAutoadj.Visible = CheckBox1.Checked
        LinkLabel1.Visible = CheckBox1.Checked
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MessageBox.Show("Nel caso in cui si voglia gestire anche l’ora legale, si può utilizzare il formato std offset 'st[offset], start[/time],end[/time]'.
Std indica il nome del fuso orario e offset, invece, indica il tempo da aggiungere a quello locale per raggiungere UTC.
Il dst e l'offset associato identificano il fuso orario per l’ora legale. Se questo secondo offset è omesso, di default sarà impostato su +1 rispetto all’orario solare.
Infine, start ed end identificano il giorno e ora in cui inizia e finisce l’ora legale.
Nel caso italiano, l’ora legale è associata al fuso orario CEST (Central European Summer Time) che è attivo dalle 02:00 dell’ultima domenica di marzo fino alle 03:00 dell’ultima domenica di ottobre.")
    End Sub
#Region "Gestione GUI interattiva"

    Private Sub Panel1_MouseEnter(sender As Object, e As EventArgs) Handles Panel1.MouseEnter
        Panel1.BorderStyle = BorderStyle.FixedSingle
    End Sub

    Private Sub Panel1_MouseLeave(sender As Object, e As EventArgs) Handles Panel1.MouseLeave
        Panel1.BorderStyle = BorderStyle.None
    End Sub
    Private Sub Panel1_Click(sender As Object, e As EventArgs) Handles Panel1.Click
        Dim SmtpList As New ListaSMTP
        If SmtpList.ShowDialog = DialogResult.OK Then
            SMTP_PORT.Value = SmtpList.SelectedPort
            SMTP_HOST.Text = SmtpList.SelectedHost
        End If
    End Sub

    Private Sub ListNTP_MouseEnter(sender As Object, e As EventArgs) Handles ListNTP.MouseEnter
        ListNTP.BorderStyle = BorderStyle.FixedSingle
    End Sub
    Private Sub ListNTP_MouseLeave(sender As Object, e As EventArgs) Handles ListNTP.MouseLeave
        ListNTP.BorderStyle = BorderStyle.None
    End Sub
    Private Sub ListNTP_Click(sender As Object, e As EventArgs) Handles ListNTP.Click
        Dim NTPList As New ListaNTP
        If NTPList.ShowDialog = DialogResult.OK Then
            NTPServer.Text = NTPList.SelectedHost
        End If
    End Sub
    Private Sub TelegramGroupID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TelegramGroupID.KeyPress
        ' Accetto solo l'immissione di numeri interi

        ' Recupero il codice ascii del tasto digitato
        ' il tasto digitato è memorizzato nella proprietà "KeyChar"
        ' dell'oggetto System.Windows.Forms.KeyPressEventArgs
        Dim KeyAscii As Short = Asc(e.KeyChar)

        ' I numeri interi hanno il codice ascii compreso tra
        ' 48 e 57. Devo comunque fare in modo che l'utente
        ' sia in grado di digitare anche il tasto BackSpace
        ' (ascii=8) e il tasto Canc (ascii=24), se il codice
        ' ascii non rientra in quelli ammessi, lo imposto io
        ' su Zero, che è il carattere nullo.
        If TelegramGroupID.TextLength = 0 Then
            If KeyAscii = 43 And KeyAscii = 45 Then
                KeyAscii = 0
            End If
        ElseIf KeyAscii < 48 And KeyAscii <> 24 And KeyAscii <> 8 Then
            KeyAscii = 0
        ElseIf KeyAscii > 57 Then
            KeyAscii = 0
        End If

        ' Aggiungo un'ulteriore finezza facendo in modo che lo
        ' zero sia ammesso, ma non come primo carattere, lo faccio
        ' controllando la lunghezza del testo
        If e.KeyChar = "0" And TelegramGroupID.TextLength = 0 Then
            KeyAscii = 0
        End If

        ' Alla fine reimposto il KeyChar, che sarà uguale a zero
        ' per i caratteri non consentiti (e quindi nella TextBox
        ' non comparirà nulla)
        e.KeyChar = Chr(KeyAscii)
    End Sub

    Private Sub Showpassemail_CheckedChanged(sender As Object, e As EventArgs) Handles Showpassemail.CheckedChanged
        SMTP_AUTHOR_PASSWORD.UseSystemPasswordChar = Not Showpassemail.Checked
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim simult As New SimulatoreEmail
        simult.orariouno.Text = "99 mai 1900, " & OraReport.Value & ":" & ReportMinuti.Value
        simult.orariodue.Text = simult.orariouno.Text
        simult.DisplayNameuno.Text = SMTP_SENDER_NAME.Text
        simult.Displatnamedue.Text = simult.DisplayNameuno.Text
        simult.mittenteuno.Text = "<" & SMTP_AUTHOR_EMAIL.Text & ">"
        simult.mittentedue.Text = simult.mittenteuno.Text
        simult.ricevente.Text = "<" & RECIPIENT_EMAIL.Text & ">"
        simult.ShowDialog()

    End Sub

    Private Sub SelectEthernet_CheckedChanged(sender As Object, e As EventArgs) Handles SelectEthernet.CheckedChanged
        If SelectEthernet.Checked Then
            ChipSetSelection.Visible = True
            ChipSetSelection.SelectedIndex = 1
            EthSpiSelected.Visible = True
            RandomMac.Visible = True
            EthMAC.Visible = True
            EthSpiSelected.SelectedIndex = 0
            Label36.Visible = True
            Label37.Visible = True
        Else

            EthSpiSelected.Visible = False
            ChipSetSelection.Visible = False
            RandomMac.Visible = False
            EthMAC.Visible = False
            Label36.Visible = False
            Label37.Visible = False
        End If
    End Sub

    Private Sub RandomMac_Click(sender As Object, e As EventArgs) Handles RandomMac.Click
        Dim random As New Random
        Dim mac(6) As Byte
        random.NextBytes(mac)
        Dim str As New StringBuilder(mac.Length * 2)

        str.Append("0x" + Conversion.Hex(mac(0)) + ",")
        str.Append("0x" + Conversion.Hex(mac(1)) + ",")
        str.Append("0x" + Conversion.Hex(mac(2)) + ",")
        str.Append("0x" + Conversion.Hex(mac(3)) + ",")
        str.Append("0x" + Conversion.Hex(mac(4)) + ",")
        str.Append("0x" + Conversion.Hex(mac(5)))
        EthMAC.Text = str.ToString
    End Sub



#End Region
End Class
Public Class ObjValue
    Public Text As String
    Public offset As Int16
    Sub New(ByVal value As String, ByVal index As Int16)
        Text = value
        offset = index
    End Sub
End Class