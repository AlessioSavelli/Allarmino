Imports System.ComponentModel

Public Class AdminSession
    Dim loader As New PopupAttesa
    Dim allLineSetup() As String
    Dim needToSave As Boolean = True
    Public Filedisetup As String = ""
    Private Sub AdminSession_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loader.Show()
        If load_setup() = False Then
            MessageBox.Show("File di setup non valido o corrotto!")
            loader.Close()
        Else
            Me.Enabled = True
        End If
        loader.Close()

        Dim tips As New ToolTip

        tips.AutoPopDelay = 5000
        tips.InitialDelay = 1000
        tips.ReshowDelay = 500
        tips.ShowAlways = True

        tips.IsBalloon = True

        tips.ToolTipTitle = "Password"

        tips.SetToolTip(AdminPassword, "Usa almeno 8 caratteri" + vbCrLf + "Usa almeno una lettera maiuscola" + vbCrLf + "Usa lameno un carattere speciale" + vbCrLf + "Es. B14%mk34!")
    End Sub
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
        Dim esito As Boolean = False
        Dim setupinput As String = IO.File.ReadAllText(Filedisetup)
        allLineSetup = Split(setupinput, vbCrLf)
        For Each line As String In allLineSetup
            Dim parmsrow() As String = Split(line.Replace("""" & "//", """" & " //"), " ")
            If parmsrow.Length > 1 Then
                Select Case parmsrow(1).Replace("#define ", "")
                    Case "TELEGRAM_PASSWORD_INSTALLATORE"
                        AdminPassword.Text = found_value(parmsrow).Text.Replace("""", "")
                        esito = True
                    Case "TELEGRAM_INSTALLATORE_IDLE"
                        Dim objval As ObjValue = found_value(parmsrow)
                        Dim nr As Int16 = CInt(objval.Text)
                        objval = found_value(parmsrow, objval.offset + 1)
                        Dim secndval As String = objval.Text.Replace("*", "")
                        If secndval = "MINUTES" Then nr = nr * 60
                        AdminIDLETime.Value = nr
                        esito = True
                End Select
            End If
        Next
        Return esito
    End Function
    Public Sub replace_str_value(ByRef line As String, ByRef parmsrow As String(), ByVal newValue As String) 'cerca e sostituisce il valore del Define associato
        Dim str As String = found_value(parmsrow).Text
        If str.Replace("""", "") = "" Then
            line = line.Replace(str, """" & newValue & """")
        Else
            line = line.Replace(str.Replace("""", ""), newValue)
        End If
    End Sub
    Private Function write_setup() As Boolean
        'in allLineSetup è già caricato il file, ora lo modifichiamo 
        Dim StremOutput As IO.StreamWriter
        IO.File.Delete(Filedisetup)
        StremOutput = IO.File.CreateText(Filedisetup)
        For Each line As String In allLineSetup
            Dim parmsrow() As String = Split(line, " ")
            If parmsrow.Length > 1 Then
                Select Case parmsrow(1).Replace("#define ", "")
                    Case "TELEGRAM_PASSWORD_INSTALLATORE"
                        replace_str_value(line, parmsrow, AdminPassword.Text)
                    Case "TELEGRAM_INSTALLATORE_IDLE"
                        Dim objval As ObjValue = found_value(parmsrow)
                        Dim nr As Int16 = CInt(objval.Text)
                        line = line.Replace(nr, AdminIDLETime.Value).Replace("*MINUTES", "*SECONDS").Replace("* MINUTES", "* SECONDS")
                End Select
            End If
            StremOutput.WriteLine(line) ' scrive le linee senza modificare nulla
        Next

        StremOutput.Dispose()
        StremOutput.Close()
        Return True
    End Function

    Private Sub AdminSession_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
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

    Private Sub ShowWiFiPass_CheckedChanged(sender As Object, e As EventArgs) Handles ShowWiFiPass.CheckedChanged

        AdminPassword.UseSystemPasswordChar = Not ShowWiFiPass.Checked
    End Sub


End Class