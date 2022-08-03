Public Class Programmatore
    Public FirmwareVersion As String = "1_0_V"
    Dim pathParmsCompiler As String = "\Resource\confcomp.ini"
    Dim pathCompiler As String = "\Resource\confcomp.allst"
    Dim extralibreries As String = "\Resource\librerie"
    Public ProjectDir As String = ""
    Dim source As String = "\Source\main"

    Private Sub Dettagli_CheckedChanged(sender As Object, e As EventArgs) Handles Dettagli.CheckedChanged
        If Dettagli.Checked Then
            Me.Size = New Size(650, 365)
        Else
            Me.Size = New Size(650, 120)
        End If
    End Sub

    Private Sub Programmatore_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Size = New Size(650, 120)
        'aggiungiamo la versione del progetto al project dir
        Dim versionproject() As String = IO.Directory.GetDirectories(ProjectDir)
        ProjectDir = versionproject(versionproject.Length - 1)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False
        If Button1.Text = "Inizia" Then
            DebugConsole.Clear()
            Dim Programmazione As New Threading.Thread(AddressOf ProgrammaCentralina)
            Programmazione.IsBackground = False
            Programmazione.SetApartmentState(Threading.ApartmentState.STA)
            Programmazione.Start()
        End If
    End Sub
    Private Sub errorMessage(ByVal consoleoutput As String)
        Invoke(Sub() Status.Text = "Errore di caricamento!")
        Invoke(Sub() DebugConsole.SelectionColor = Color.Red)
        Invoke(Sub() DebugConsole.SelectedText = vbCrLf & consoleoutput)
        Invoke(Sub() Button1.Enabled = True)
    End Sub
    Private Sub ProgrammaCentralina()
        Dim reader As System.IO.StreamReader

        Dim arduinofolder As String = ""
        Dim espfolder As String = ""
        Dim uploadspeed As String = ""
        Dim portacom As String = ""
        Dim rootCompilatore As String

        Dim standardstring As String = ""
        Dim prebuildstring As String = ""
        Dim compilestring As String = ""

        Dim esptoolspath As String = ""
        Dim esptoolsparms As String = ""

        'inizializzo la console di debug
        Invoke(Sub() DebugConsole.Clear())
        Invoke(Sub() DebugConsole.ForeColor = Color.Green)

#Region "Preparazione del sorgente da compilare"
        Invoke(Sub() Status.ForeColor = Color.Black)
        Invoke(Sub() Status.Text = "Preparazione file...")
        Invoke(Sub() DebugConsole.AppendText(vbCrLf & "Move Setup.h to Source\main"))
        'va listato tutto quello che c'è nella cartella Setup e poi sovrascritto in source



        Try
            For Each setup_files As String In Directory.GetFiles(ProjectDir & "\Setup")
                setup_files = setup_files.Replace(ProjectDir, "") 'radice del percorso
                Dim FileName As String = setup_files.Replace("\Setup\", "") 'estra il nome del file
                If IO.File.Exists(ProjectDir & source & "\" & FileName) Then IO.File.Delete(ProjectDir & source & "\" & FileName) 'se il file esiste nella cartella del codice sorgente lo elimina
                'e lo rimpiazza con quello presente nella cartella Setup
                IO.File.Copy(ProjectDir & setup_files, ProjectDir & source & "\" & FileName)
                Invoke(Sub() DebugConsole.AppendText(vbCrLf & "sovrascritto : " & ProjectDir & source & "\" & FileName & " - >" & ProjectDir & setup_files))
                Invoke(Sub() DebugConsole.ScrollToCaret())
            Next
        Catch ex As Exception
            errorMessage(ex.ToString)
            Exit Sub
        End Try
#End Region

#Region "Caricamento dati iniziali con verifica compilatore arduino"
        'verificare se esiste il compilatore di arduino
        Invoke(Sub() Status.Text = "Verifica compilatore...")
        Invoke(Sub() DebugConsole.AppendText(vbCrLf & "Check arduino compiler..."))

        reader = IO.File.OpenText(Environment.CurrentDirectory & pathCompiler)
        Dim dati As String
        dati = reader.ReadToEnd
        Dim ObjectItem As String() = Split(dati, vbCrLf)
        arduinofolder = ObjectItem(0)



        portacom = ObjectItem(3) ' estraggo la porta su cui deve fare l'upload del firmware
        rootCompilatore = ObjectItem(6) ' estraggo la root del compilatore
        uploadspeed = ObjectItem(2) ' estraggo la velocità di scrittura della board
        reader.Close()
        reader.Dispose()

        If IO.Directory.Exists(Environment.CurrentDirectory & "\IDEPortable\hardware\ESPp\packages") Then ' usa il compilatore ESP32 portatile
            espfolder = arduinofolder & "\hardware\ESPp\packages"
        Else
            errorMessage("Esp32 tools not found in : " & Environment.CurrentDirectory & "\IDEPortable\hardware\ESPp\packages")
            Exit Sub
        End If
#End Region

#Region "Controllo porta seriale"
        If CheckBox1.Checked Then
            'controllo connessione della porta seriale
            Invoke(Sub() Status.Text = "Connessione alla centralina...")
            Invoke(Sub() DebugConsole.AppendText(vbCrLf & "connect to devices on port : " & portacom & " BoundRate : " & uploadspeed))
            Dim checker As New IO.Ports.SerialPort
            checker.PortName = portacom
            checker.BaudRate = uploadspeed
            Try
                checker.Open()
                Threading.Thread.Sleep(500)
                If Not checker.IsOpen Then
                    Try
                        checker.Close()
                    Catch
                    End Try
                    errorMessage(" - port not available")
                    Exit Sub
                End If
                checker.Close()
                Invoke(Sub() DebugConsole.AppendText(" - port connected"))
                If rootCompilatore.Replace(" ", "") = "" Then
                    errorMessage("Invalid path, please set your arduino compiler!!!")
                    Exit Sub
                End If
            Catch ex As Exception
                errorMessage(" - port not available")
                Exit Sub
            End Try
        End If
#End Region
        If Compilazione.Checked Then
#Region "Preparazione aria di compilazione"

            If Not IO.File.Exists(rootCompilatore) Then
                errorMessage("Invalid path, please set your arduino compiler!!!")
                Exit Sub
            End If
            'se il compilatore esiste prepara la cartella di build
            If IO.Directory.Exists(ProjectDir & "\build") Then IO.Directory.Delete(ProjectDir & "\build", True)
            IO.Directory.CreateDirectory(ProjectDir & "\build")
            IO.Directory.CreateDirectory(ProjectDir & "\build\cache")
            IO.Directory.CreateDirectory(ProjectDir & "\build\out")
            'legge il file di configurazione avanzata del compilatore
#End Region
        End If
#Region "Caricamento dei parametri di compilazione"
        Invoke(Sub() Status.Text = "Caricamento parametri...")
        Invoke(Sub() DebugConsole.AppendText(vbCrLf & "Load parms..."))

        Dim reader2 As System.IO.StreamReader
        reader2 = IO.File.OpenText(Environment.CurrentDirectory & pathParmsCompiler)
        If Not reader2.ReadLine.Contains("#@usecustombuild") Then 'identifica i dati di build che deve prendere
            While Not reader2.ReadLine.Contains("[CUSTOM PARMS]")
                'Attende di arrivare a leggere i parametri custom
            End While
            Dim out As String = reader2.ReadLine

            While Not out.Contains("[ESPTOOL_PY]")

                If out.StartsWith("arduino-builder=") Then
                    rootCompilatore = out.Replace("arduino-builder=", "")
                ElseIf out.StartsWith("precompileparms=") Then
                    prebuildstring = out.Replace("precompileparms=", "")
                ElseIf out.StartsWith("compileparms=") Then
                    compilestring = out.Replace("compileparms=", "")
                ElseIf out.Replace(" ", "").StartsWith("#") Then ' skip della riga
                Else
                    standardstring = standardstring & out & " "
                End If
                out = reader2.ReadLine
            End While

            While out IsNot Nothing
                If out.StartsWith("esptoolspath=") Then
                    esptoolspath = out.Replace("esptoolspath=", "")
                ElseIf out.StartsWith("esptoolparms=") Then
                    esptoolsparms = out.Replace("esptoolparms=", "")
                ElseIf out.StartsWith("esptoolboot=") Then
                    esptoolsparms = esptoolsparms & " " & out.Replace("esptoolboot=", "")
                ElseIf out.StartsWith("esptoolbootloader_qio=") Then
                    esptoolsparms = esptoolsparms & " " & out.Replace("esptoolbootloader_qio=", "")
                ElseIf out.StartsWith("main.ino.bin=") Then
                    esptoolsparms = esptoolsparms & " " & out.Replace("main.ino.bin=", "")
                ElseIf out.StartsWith("main.ino.partitions.bin=") Then
                    esptoolsparms = esptoolsparms & " " & out.Replace("main.ino.partitions.bin=", "")
                ElseIf out.Replace(" ", "").StartsWith("#") Then ' skip della riga
                Else
                    If out.Replace(" ", "") <> Nothing Then esptoolsparms = esptoolsparms & out & " "
                End If
                out = reader2.ReadLine
            End While
        Else
            Dim out As String = reader2.ReadLine
            While Not out.Contains("[ESPTOOL_PY]")

                If out.StartsWith("arduino-builder=") Then ' skip della riga
                ElseIf out.StartsWith("precompileparms=") Then
                    prebuildstring = out.Replace("precompileparms=", "")
                ElseIf out.StartsWith("compileparms=") Then
                    compilestring = out.Replace("compileparms=", "")
                ElseIf out.Replace(" ", "").StartsWith("#") Then ' skip della riga
                Else
                    If out.Replace(" ", "") <> Nothing Then standardstring = standardstring & out & " "
                End If
                out = reader2.ReadLine
            End While
            'ora passiamo alla lettura della configurazione per l'esptool_py
            While Not out.Contains("[CUSTOM PARMS]")
                If out.StartsWith("esptoolspath=") Then
                    esptoolspath = out.Replace("esptoolspath=", "")
                ElseIf out.StartsWith("esptoolparms=") Then
                    esptoolsparms = out.Replace("esptoolparms=", "")
                ElseIf out.StartsWith("esptoolboot=") Then
                    esptoolsparms = esptoolsparms & " " & out.Replace("esptoolboot=", "")
                ElseIf out.StartsWith("esptoolbootloader_qio=") Then
                    esptoolsparms = esptoolsparms & " " & out.Replace("esptoolbootloader_qio=", "")
                ElseIf out.StartsWith("main.ino.bin=") Then
                    esptoolsparms = esptoolsparms & " " & out.Replace("main.ino.bin=", "")
                ElseIf out.StartsWith("main.ino.partitions.bin=") Then
                    esptoolsparms = esptoolsparms & " " & out.Replace("main.ino.partitions.bin=", "")
                ElseIf out.Replace(" ", "").StartsWith("#") Then ' skip della riga
                Else
                    If out.Replace(" ", "") <> Nothing Then esptoolsparms = esptoolsparms & out & " "
                End If
                out = reader2.ReadLine
            End While
            reader2.Close()
            reader2.Dispose()
            'auto compila gli standard string

        End If
#End Region



#Region "Interpreta gli input dinamici"


        rootCompilatore = rootCompilatore.Replace("[autopatch_arduino-builder]", arduinofolder & "\arduino-builder.exe")


        standardstring = standardstring.Replace("[autopatch_arduino-builder]", arduinofolder & "\arduino-builder.exe")
        standardstring = standardstring.Replace("[autopatch_arduino-hardware]", arduinofolder & "\hardware")

        standardstring = standardstring.Replace("[autopatch_tools-builder]", arduinofolder & "\tools-builder")
        standardstring = standardstring.Replace("[autopatch_tools\avr]", arduinofolder & "\hardware\tools\avr")


        standardstring = standardstring.Replace("[autopatch_arduino-packages]", espfolder)


        standardstring = standardstring.Replace("[autopatch_built-in-libraries]", arduinofolder & "\libraries")
        standardstring = standardstring.Replace("[autopatch_built-extra-libraries]", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Arduino\libraries") 'Environment.CurrentDirectory & extralibreries
        standardstring = standardstring.Replace("[speed_esp32]", uploadspeed)

        standardstring = standardstring.Replace("[autopatch_build]", ProjectDir & "\build\out")
        standardstring = standardstring.Replace("[autopatch_cache]", ProjectDir & "\build\cache")


        Dim esproot As String = espfolder & "\esp32\tools\xtensa-esp32-elf-gcc\"
        Dim vers As String = getLastVersion(esproot)
        standardstring = standardstring.Replace("[autopatch_extensa-esp32]", esproot & vers)
        standardstring = standardstring.Replace("[autovers_extensa-esp32]", vers)
        esproot = espfolder & "\esp32\tools\mkspiffs\"
        vers = getLastVersion(esproot)
        standardstring = standardstring.Replace("[autopatch_mkspiffs]", esproot & vers)
        standardstring = standardstring.Replace("[autovers_mkspiffs]", vers)
        esproot = espfolder & "\esp32\tools\esptool_py\"
        vers = getLastVersion(esproot)
        standardstring = standardstring.Replace("[autopatch_esptool]", esproot & vers)
        espfolder = espfolder.Replace("[autopatch_esptool]", esproot & vers)
        standardstring = standardstring.Replace("[autovers_esptool]", vers)
        standardstring = standardstring.Replace("[autopatch_soruce.ino]", ProjectDir & source & "\main.ino")

        esproot = espfolder & "\esp32\tools\esptool_py\"
        vers = getLastVersion(esproot)

        esptoolspath = esptoolspath.Replace("""", "").Replace("[autopatch_esptool]", esproot & vers & "/esptool.exe")
        esptoolsparms = esptoolsparms.Replace("[autopatch_esptool]", esptoolspath)

        esptoolsparms = esptoolsparms.Replace("[serialport_esp32]", portacom)
        esptoolsparms = esptoolsparms.Replace("[speed_esp32]", uploadspeed)
        esproot = espfolder & "\esp32\hardware\esp32\"
        vers = getLastVersion(esproot)
        esptoolsparms = esptoolsparms.Replace("[autopatch_boot_app]", esproot & vers & "/tools/partitions/boot_app0.bin")
        esptoolsparms = esptoolsparms.Replace("[autopatch_bootloader_qio]", esproot & vers & "/tools/sdk/esp32/bin/bootloader_qio_80m.bin")
        esptoolsparms = esptoolsparms.Replace("[autopatch_main.ino.bin]", ProjectDir & "\build\out/main.ino.bin")
        esptoolsparms = esptoolsparms.Replace("[autopatch_main.ino.partitions.bin]", ProjectDir & "\build\out/main.ino.partitions.bin")



#End Region

        If Compilazione.Checked Then
#Region "Precompilazione"

            Invoke(Sub() ProgressBar1.Maximum = 250)
            Invoke(Sub() Status.Text = "Precompilazione...")
            Invoke(Sub() DebugConsole.AppendText(vbCrLf & """" & rootCompilatore & """" & " " & prebuildstring & " " & standardstring & vbCrLf & vbCrLf))
            If Not StartProcess(rootCompilatore, prebuildstring & " " & standardstring) Then
                errorMessage("Error, compiler not available")
            End If
            Invoke(Sub() DebugConsole.AppendText(vbCrLf & "----DONE---"))
#End Region

#Region "Compilazione"
            Invoke(Sub() ProgressBar1.Maximum = 2000)
            Invoke(Sub() Status.Text = "Compilazione...")
            Invoke(Sub() DebugConsole.AppendText(vbCrLf & """" & rootCompilatore & """" & " " & compilestring & " " & standardstring & vbCrLf & vbCrLf))
            If Not StartProcess(rootCompilatore, compilestring & " " & standardstring) Then
                errorMessage("Error, compiler not available")
                Exit Sub
            End If
            If Invoke(Sub() DebugConsole.Text.Contains("The filename or extension is too long.")) Then
                errorMessage("Error, The filename or extension is too long, try to move the ArduinoIDE in to C:\ArduinoIDE and try again")
                Exit Sub
            End If
            'controllo se la compilazione è andata a buon fine
            If Not IO.File.Exists(ProjectDir & "\build\out/main.ino.partitions.bin") Then ' se non esiste il file .bin da caricare sulla board
                errorMessage("Error, compiler do not create : " & ProjectDir & "\build\out/main.ino.partitions.bin")
                Exit Sub
            End If
            Invoke(Sub() DebugConsole.AppendText(vbCrLf & "----DONE---"))
#End Region
            Invoke(Sub() Compilazione.Enabled = True)
        End If

#Region "apre la cartella con il file bin per telegram"
        If Not CheckBox1.Checked Then ' vuole caricare il file main
            IO.File.Copy(ProjectDir & "\build\out\main.ino.bin", ProjectDir & "\build\FW_ALLARMINO_" & FirmwareVersion & ".bin")
            Dim folder As New ProcessStartInfo(ProjectDir & "\build\")
            folder.UseShellExecute = True
            Invoke(Sub() ProgressBar1.Value = ProgressBar1.Maximum)
            Process.Start(folder) ' apre la certella di output del file


            Invoke(Sub() Button1.Enabled = True)
            Exit Sub
        End If
#End Region


#Region "Upload del codice sulla board"

        Invoke(Sub() ProgressBar1.Maximum = 130)
        Invoke(Sub() Status.Text = "Caricamento porta : " & portacom)
        Invoke(Sub() DebugConsole.AppendText(vbCrLf & "Uploading code...."))
        Invoke(Sub() DebugConsole.AppendText(vbCrLf & """" & esptoolspath & """" & " " & esptoolsparms))
        Invoke(Sub() DebugConsole.ScrollToCaret())

        If Not StartProcess(esptoolspath, esptoolsparms) Then
            errorMessage("Error, ESP32 compiler not available")
            Exit Sub
        End If
        Dim result As Boolean = True
        'Invoke(Sub() result = DebugConsole.Text.EndsWith("Hash of data verified." & vbCrLf)) ' da fare meglio
        If result Then
            Invoke(Sub() DebugConsole.AppendText(vbCrLf & "----END---"))
            Invoke(Sub() Status.Text = "Caricamento completato")
            Invoke(Sub() DebugConsole.ScrollToCaret())
            Invoke(Sub() ProgressBar1.Value = 0)
        Else
            errorMessage("Error, invalid upload")
            Exit Sub
        End If
#End Region

        Invoke(Sub() Button1.Enabled = True)
    End Sub

    Private Function getLastVersion(ByVal rootesp As String)
        Dim subdir As String() = IO.Directory.GetDirectories(rootesp)
        Dim lastdir As String = subdir(subdir.Length - 1)
        Dim subsubdir() As String = lastdir.Split("\")
        Return subsubdir(subsubdir.Length - 1) ' ritorna l'ultimo oggetto che corrisponde alla versione più recente
    End Function
#Region "Interfacciamento con il compilatore"
    Dim myProcesso As Process
    Dim threadLeach1 As Threading.Thread
    Private Function StartProcess(ByVal nome As String, ByVal parms As String) As Boolean
        Invoke(Sub() ProgressBar1.Value = 0)
        Try
            myProcesso = New Process()
            Dim strInfo As ProcessStartInfo = myProcesso.StartInfo
            strInfo.FileName = nome
            strInfo.Arguments = parms
            strInfo.UseShellExecute = False
            strInfo.CreateNoWindow = True
            strInfo.RedirectStandardOutput = True
            strInfo.RedirectStandardError = True
            strInfo.RedirectStandardInput = True
            'strInfo = Nothing

            Try
                myProcesso.Start() ' fa partire il processo
            Catch 'se non riesce ad aprire il processo ritorna errore
                Return False
            End Try
            Try
                Dim output As String = myProcesso.StandardOutput.ReadLine
                If output <> Nothing Then
                    Invoke(Sub() DebugConsole.AppendText(vbCrLf & output))

                    Do
                        output = myProcesso.StandardOutput.ReadLine()
                        If output IsNot Nothing Then
                            Invoke(Sub() DebugConsole.AppendText(vbCrLf & output))
                            Invoke(Sub() DebugConsole.ScrollToCaret())
                            Invoke(Sub() ProgressBar1.Increment(2))
                        End If
                    Loop While (output <> Nothing)

                End If
            Catch
            End Try
            Try
                myProcesso.Kill()
            Catch
            End Try
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
#End Region
End Class