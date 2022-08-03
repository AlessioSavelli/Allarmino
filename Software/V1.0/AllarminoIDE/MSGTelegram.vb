Public Class MSGTelegram
    Dim loader As New PopupAttesa
    Dim allLineSetup() As String
    Public Filedisetup As String = ""
    Dim needToSave As Boolean = True

#Region "Gestione anteprima keyboard"
    Private Sub VersioneFirmware_TextChanged(sender As Object, e As EventArgs) Handles VersioneFirmware.TextChanged
        TestVersioneFW.Text = "/" & VersioneFirmware.Text
    End Sub

    Private Sub HelpValue_TextChanged(sender As Object, e As EventArgs) Handles HelpValue.TextChanged
        helpLabel.Text = "/" & HelpValue.Text
    End Sub

    Private Sub Out2OFF_TextChanged(sender As Object, e As EventArgs) Handles Out2OFF.TextChanged
        TestOut2OFF.Text = "/" & Out2OFF.Text
    End Sub

    Private Sub Out1TurnON_TextChanged(sender As Object, e As EventArgs) Handles Out1TurnON.TextChanged
        TestOut1ON.Text = "/" & Out1TurnON.Text
    End Sub

    Private Sub GetInput1_TextChanged(sender As Object, e As EventArgs) Handles GetInput1.TextChanged
        TestInput1.Text = "/" & GetInput1.Text
    End Sub

    Private Sub GetInput2_TextChanged(sender As Object, e As EventArgs) Handles GetInput2.TextChanged
        TestInput2.Text = "/" & GetInput2.Text
    End Sub

    Private Sub DisattivaAllarme_TextChanged(sender As Object, e As EventArgs) Handles DisattivaAllarme.TextChanged
        TestDisattivaAllarme.Text = "/" & DisattivaAllarme.Text
    End Sub

    Private Sub AttivaAllarme_TextChanged(sender As Object, e As EventArgs) Handles AttivaAllarme.TextChanged
        TestAttivaAllarme.Text = "/" & AttivaAllarme.Text
    End Sub

    Private Sub StatoAllarme_TextChanged(sender As Object, e As EventArgs) Handles StatoAllarme.TextChanged
        TestTastoAllarme.Text = "/" & StatoAllarme.Text
    End Sub

    Private Sub GetTamper1_TextChanged(sender As Object, e As EventArgs) Handles GetTamper1.TextChanged
        TestGetTamper.Text = "/" & GetTamper1.Text
    End Sub

    Private Sub GetZone_TextChanged(sender As Object, e As EventArgs) Handles GetZone.TextChanged
        TestZone.Text = "/" & GetZone.Text
    End Sub


#End Region


    Private Sub MSGTelegram_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Enabled = False

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
    Private Sub MSGTelegram_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
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
    Private Function load_setup() As Boolean
        If Not IO.File.Exists(Filedisetup) Then
            Return False
        End If
        Dim setupinput As String = IO.File.ReadAllText(Filedisetup)
        allLineSetup = Split(setupinput, vbCrLf)
        For Each line As String In allLineSetup
            Dim parmsrow() As String = Split(line.Replace("""" & "//", """" & " //"), " ")
            If parmsrow.Length > 1 Then
                Select Case parmsrow(1).Replace("#define ", "")
                    Case "MSG_TG_WAKEUP_MESSAGE"
                        Wekeup.Text = found_value(parmsrow).Text.Replace("""", "").Replace("""", "")
                    Case "MSG_TG_ALLARME_ZONA1"
                        Zona1Allarme.Text = found_value(parmsrow).Text.Replace("""", "").Replace("""", "")
                    Case "MSG_TG_ALLARME_ZONA2"
                        Zona2Allarme.Text = found_value(parmsrow).Text.Replace("""", "").Replace("""", "")
                    Case "MSG_TG_ALLARME_ZONA3"
                        Zona3Allarme.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ALLARME_ZONA4"
                        Zona4Allarme.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ALLARME_TAMPER"
                        AllarmeTamper.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_RIPOSO_ZONA1"
                        RiposoZona1.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_RIPOSO_ZONA2"
                        RiposoZona2.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_RIPOSO_ZONA3"
                        RiposoZona3.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_RIPOSO_ZONA4"
                        RiposoZona4.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_RIPOSO_TAMPER"
                        RiposoTamper.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ESCLUSIONE_ZONA1"
                        EsclusioneZona1.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ESCLUSIONE_ZONA2"
                        EsclusioneZona2.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ESCLUSIONE_ZONA3"
                        EsclusioneZona3.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ESCLUSIONE_ZONA4"
                        EsclusioneZona4.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ESCLUSIONE_TAMPER"
                        EsclusioneTamper.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ALLERT_ZONE_ESCLUSE"
                        AllertEsclusioni.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ALLERT_TAMPER_ESCLUSO_ALLARME_ON"
                        AllertEsclusioneTamper.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ALLERT_INTRUSIONE_RILEVATA"
                        AllertIntrusione.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_INPUT1_ON"
                        Input1ON.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_INPUT2_ON"
                        Input2ON.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_INPUT1_OFF"
                        Input1OFF.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_INPUT2_OFF"
                        Input2OFF.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_OUTPUT1_ON"
                        Output1ON.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_OUTPUT2_ON"
                        Output2ON.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_OUTPUT1_OFF"
                        Output1OFF.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_OUTPUT2_OFF"
                        Output2OFF.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_INSERIMENTO_ALLARME"
                        InserimentoAllarme.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_DISINSERIMENTO_ALLARME"
                        DisinserimentoAllarme.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ALLARME_INSERITA"
                        AllarmeInseritoOK.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ALLARME_DISINSERITA"
                        DisinserimentoOK.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ALLARME_NON_INSERIBILE"
                        allarmeNonInseribile.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ETHERNET_CONNESSO"
                        EthernetConnesso.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ETHERNET_DISCONNESSO"
                        EthernetDisconnesso.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_WiFi_CONNESSO"
                        WiFiDisconnesso.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_WiFi_DISCONNESSO"
                        WiFiConnesso.Text = found_value(parmsrow).Text.Replace("""", "")
                    'Case "MSG_TG_NTP_NO_CONNECTION"
                    Case "MSG_TG_MANCANZA_POWERLINE"
                        LineaACAssente.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_PRESENZA_POWERLINE"
                        LineaACPresente.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_RIPRISTINO_POWERLINE"
                        LineaACRipristinata.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_WELCOM_FROM"
                        WelcomeAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_LOGOUT_MESSAGE"
                        LogOutAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_INCORRECT_PSW"
                        InvalidPassAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_CORRECT_PSW"
                        CorrectPassAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_INFO_IDLE_TIME"
                        InfoSessioneAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_INFO_ABORT_UPDATE"
                        UpdateAbortAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_INFO_CONFIRM_UPDATE"
                        UpdateAcceptAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_INFO_UPDATE_FAIL"
                        ErrorUpdateAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_INFO_UPDATE_NO_SPACE"
                        ErrorSpaceAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_ALLERT_INVALIDFILE"
                        InvalidFirmwareAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_ALLERT_LOGIN_BLOCKED"
                        AdminBlocked.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_ALLERT_IS_FULL"
                        FullSessionAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_ALLERT_EXPIRIED_TIME"
                        ExpirieSessionAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_ALLERT_DISABLED_PRG_JMPR"
                        PRG_JMP_DISABLED.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_ACCEPT_FW_UPDATE"
                        AcceptedFirmwareAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_INVALID_SIZE_FW_UPDATE"
                        InvalidFirmwareSizeAdmin.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_ADMIN_SESSION_START_FW_UPDATE"
                        StartFlashig.Text = found_value(parmsrow).Text.Replace("""", "")
                    Case "MSG_TG_HELP_COMMAND"
                        HelpValue.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_TURN_ON_OUTPUT1"
                        Out1TurnON.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_TURN_ON_OUTPUT2"
                        Out2ON.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_TURN_OFF_OUTPUT1"
                        Out1TurnOFF.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_TURN_OFF_OUTPUT2"
                        Out2OFF.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_READ_INPUT1"
                        GetInput1.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_READ_INPUT2"
                        GetInput2.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_READ_POWERLINE"
                        GetPowerLine.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_ST_ALlARME"
                        StatoAllarme.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_TURN_ON_ALLARME"
                        AttivaAllarme.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_TURN_OFF_ALLARME"
                        DisattivaAllarme.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_STATUS_TAMPER"
                        GetTamper1.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_STATUS_ZONE"
                        GetZone.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_FW_VERS"
                        VersioneFirmware.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
                    Case "BUTTON_TG_ADMIN_LOGIN"
                        StartAdmin.Text = found_value(parmsrow).Text.Replace("""", "").Replace("/", "")
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
                    Case "MSG_TG_WAKEUP_MESSAGE"
                        replace_str_value(line, parmsrow, Wekeup.Text)
                    Case "MSG_TG_ALLARME_ZONA1"
                        replace_str_value(line, parmsrow, Zona1Allarme.Text)
                    Case "MSG_TG_ALLARME_ZONA2"
                        replace_str_value(line, parmsrow, Zona2Allarme.Text)
                    Case "MSG_TG_ALLARME_ZONA3"
                        replace_str_value(line, parmsrow, Zona3Allarme.Text)
                    Case "MSG_TG_ALLARME_ZONA4"
                        replace_str_value(line, parmsrow, Zona4Allarme.Text)
                    Case "MSG_TG_ALLARME_TAMPER"
                        replace_str_value(line, parmsrow, AllarmeTamper.Text)
                    Case "MSG_TG_RIPOSO_ZONA1"
                        replace_str_value(line, parmsrow, RiposoZona1.Text)
                    Case "MSG_TG_RIPOSO_ZONA2"
                        replace_str_value(line, parmsrow, RiposoZona2.Text)
                    Case "MSG_TG_RIPOSO_ZONA3"
                        replace_str_value(line, parmsrow, RiposoZona3.Text)
                    Case "MSG_TG_RIPOSO_ZONA4"
                        replace_str_value(line, parmsrow, RiposoZona4.Text)
                    Case "MSG_TG_RIPOSO_TAMPER"
                        replace_str_value(line, parmsrow, RiposoTamper.Text)
                    Case "MSG_TG_ESCLUSIONE_ZONA1"
                        replace_str_value(line, parmsrow, EsclusioneZona1.Text)
                    Case "MSG_TG_ESCLUSIONE_ZONA2"
                        replace_str_value(line, parmsrow, EsclusioneZona2.Text)
                    Case "MSG_TG_ESCLUSIONE_ZONA3"
                        replace_str_value(line, parmsrow, EsclusioneZona3.Text)
                    Case "MSG_TG_ESCLUSIONE_ZONA4"
                        replace_str_value(line, parmsrow, EsclusioneZona4.Text)
                    Case "MSG_TG_ESCLUSIONE_TAMPER"
                        replace_str_value(line, parmsrow, EsclusioneTamper.Text)
                    Case "MSG_TG_ALLERT_ZONE_ESCLUSE"
                        replace_str_value(line, parmsrow, AllertEsclusioni.Text)
                    Case "MSG_TG_ALLERT_TAMPER_ESCLUSO_ALLARME_ON"
                        replace_str_value(line, parmsrow, AllertEsclusioneTamper.Text)
                    Case "MSG_TG_ALLERT_INTRUSIONE_RILEVATA"
                        replace_str_value(line, parmsrow, AllertIntrusione.Text)
                    Case "MSG_TG_INPUT1_ON"
                        replace_str_value(line, parmsrow, Input1ON.Text)
                    Case "MSG_TG_INPUT2_ON"
                        replace_str_value(line, parmsrow, Input2ON.Text)
                    Case "MSG_TG_INPUT1_OFF"
                        replace_str_value(line, parmsrow, Input1OFF.Text)
                    Case "MSG_TG_INPUT2_OFF"
                        replace_str_value(line, parmsrow, Input2OFF.Text)
                    Case "MSG_TG_OUTPUT1_ON"
                        replace_str_value(line, parmsrow, Output1ON.Text)
                    Case "MSG_TG_OUTPUT2_ON"
                        replace_str_value(line, parmsrow, Output2ON.Text)
                    Case "MSG_TG_OUTPUT1_OFF"
                        replace_str_value(line, parmsrow, Output1OFF.Text)
                    Case "MSG_TG_OUTPUT2_OFF"
                        replace_str_value(line, parmsrow, Output2OFF.Text)
                    Case "MSG_TG_INSERIMENTO_ALLARME"
                        replace_str_value(line, parmsrow, InserimentoAllarme.Text)
                    Case "MSG_TG_DISINSERIMENTO_ALLARME"
                        replace_str_value(line, parmsrow, DisinserimentoAllarme.Text)
                    Case "MSG_TG_ALLARME_INSERITA"
                        replace_str_value(line, parmsrow, AllarmeInseritoOK.Text)
                    Case "MSG_TG_ALLARME_DISINSERITA"
                        replace_str_value(line, parmsrow, DisinserimentoOK.Text)
                    Case "MSG_TG_ALLARME_NON_INSERIBILE"
                        replace_str_value(line, parmsrow, allarmeNonInseribile.Text)
                    Case "MSG_TG_ETHERNET_CONNESSO"
                        replace_str_value(line, parmsrow, EthernetConnesso.Text)
                    Case "MSG_TG_ETHERNET_DISCONNESSO"
                        replace_str_value(line, parmsrow, EthernetDisconnesso.Text)
                    Case "MSG_TG_WiFi_CONNESSO"
                        replace_str_value(line, parmsrow, WiFiDisconnesso.Text)
                    Case "MSG_TG_WiFi_DISCONNESSO"
                        replace_str_value(line, parmsrow, WiFiConnesso.Text)
                    'Case "MSG_TG_NTP_NO_CONNECTION"
                    Case "MSG_TG_MANCANZA_POWERLINE"
                        replace_str_value(line, parmsrow, LineaACAssente.Text)
                    Case "MSG_TG_PRESENZA_POWERLINE"
                        replace_str_value(line, parmsrow, LineaACPresente.Text)
                    Case "MSG_TG_RIPRISTINO_POWERLINE"
                        replace_str_value(line, parmsrow, LineaACRipristinata.Text)
                    Case "MSG_TG_ADMIN_SESSION_WELCOM_FROM"
                        replace_str_value(line, parmsrow, WelcomeAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_LOGOUT_MESSAGE"
                        replace_str_value(line, parmsrow, LogOutAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_INCORRECT_PSW"
                        replace_str_value(line, parmsrow, InvalidPassAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_CORRECT_PSW"
                        replace_str_value(line, parmsrow, CorrectPassAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_INFO_IDLE_TIME"
                        replace_str_value(line, parmsrow, InfoSessioneAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_INFO_ABORT_UPDATE"
                        replace_str_value(line, parmsrow, UpdateAbortAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_INFO_CONFIRM_UPDATE"
                        replace_str_value(line, parmsrow, UpdateAcceptAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_INFO_UPDATE_FAIL"
                        replace_str_value(line, parmsrow, ErrorUpdateAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_INFO_UPDATE_NO_SPACE"
                        replace_str_value(line, parmsrow, ErrorSpaceAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_ALLERT_INVALIDFILE"
                        replace_str_value(line, parmsrow, InvalidFirmwareAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_ALLERT_LOGIN_BLOCKED"
                        replace_str_value(line, parmsrow, AdminBlocked.Text)
                    Case "MSG_TG_ADMIN_SESSION_ALLERT_IS_FULL"
                        replace_str_value(line, parmsrow, FullSessionAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_ALLERT_EXPIRIED_TIME"
                        replace_str_value(line, parmsrow, ExpirieSessionAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_ALLERT_DISABLED_PRG_JMPR"
                        replace_str_value(line, parmsrow, PRG_JMP_DISABLED.Text)
                    Case "MSG_TG_ADMIN_SESSION_ACCEPT_FW_UPDATE"
                        replace_str_value(line, parmsrow, AcceptedFirmwareAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_INVALID_SIZE_FW_UPDATE"
                        replace_str_value(line, parmsrow, InvalidFirmwareSizeAdmin.Text)
                    Case "MSG_TG_ADMIN_SESSION_START_FW_UPDATE"
                        replace_str_value(line, parmsrow, StartFlashig.Text)
                    Case "MSG_TG_HELP_COMMAND"
                        replace_str_value(line, parmsrow, "/" & HelpValue.Text)
                    Case "BUTTON_TG_TURN_ON_OUTPUT1"
                        replace_str_value(line, parmsrow, "/" & Out1TurnON.Text)
                    Case "BUTTON_TG_TURN_ON_OUTPUT2"
                        replace_str_value(line, parmsrow, "/" & Out2ON.Text)
                    Case "BUTTON_TG_TURN_OFF_OUTPUT1"
                        replace_str_value(line, parmsrow, "/" & Out1TurnOFF.Text)
                    Case "BUTTON_TG_TURN_OFF_OUTPUT2"
                        replace_str_value(line, parmsrow, "/" & Out2OFF.Text)
                    Case "BUTTON_TG_READ_INPUT1"
                        replace_str_value(line, parmsrow, "/" & GetInput1.Text)
                    Case "BUTTON_TG_READ_INPUT2"
                        replace_str_value(line, parmsrow, "/" & GetInput2.Text)
                    Case "BUTTON_TG_READ_POWERLINE"
                        replace_str_value(line, parmsrow, "/" & GetPowerLine.Text)
                    Case "BUTTON_TG_ST_ALlARME"
                        replace_str_value(line, parmsrow, "/" & StatoAllarme.Text)
                    Case "BUTTON_TG_TURN_ON_ALLARME"
                        replace_str_value(line, parmsrow, "/" & AttivaAllarme.Text)
                    Case "BUTTON_TG_TURN_OFF_ALLARME"
                        replace_str_value(line, parmsrow, "/" & DisattivaAllarme.Text)
                    Case "BUTTON_TG_STATUS_TAMPER"
                        replace_str_value(line, parmsrow, "/" & GetTamper1.Text)
                    Case "BUTTON_TG_STATUS_ZONE"
                        replace_str_value(line, parmsrow, "/" & GetZone.Text)
                    Case "BUTTON_TG_FW_VERS"
                        replace_str_value(line, parmsrow, "/" & VersioneFirmware.Text)
                    Case "BUTTON_TG_ADMIN_LOGIN"
                        replace_str_value(line, parmsrow, "/" & StartAdmin.Text)
                End Select
            End If
            StremOutput.WriteLine(line) ' scrive le linee senza modificare nulla
        Next
        StremOutput.Dispose()
        StremOutput.Close()
        Return True
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
    Public Sub replace_str_value(ByRef line As String, ByRef parmsrow As String(), ByVal newValue As String) 'cerca e sostituisce il valore del Define associato
        Dim str As String = found_value(parmsrow).Text.Replace("""", "")
        If str.Replace("""", "") = "" Then
            line = line.Replace(str, """" & newValue & """")
        Else
            line = line.Replace(str.Replace("""", ""), newValue)
        End If
    End Sub



#End Region
End Class