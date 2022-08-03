
Public Class settingsCompilatore
    Dim dirfile As String = "\Resource\confcomp.allst"
    Dim dirmylib As String = "\Resource\librerie"
    Private Sub settingsCompilatore_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        RefreshPort_Click(Nothing, Nothing)
        Dim reader As System.IO.StreamReader
        reader = IO.File.OpenText(Environment.CurrentDirectory & dirfile)
        Dim dati As String
        dati = reader.ReadToEnd
        Dim ObjectItem As String() = Split(dati, vbCrLf)
        percorsoIDE.Text = ObjectItem(0)
        devboard.Text = ObjectItem(1)
        uploadspeed.Text = ObjectItem(2)
        portaCOM.Text = ObjectItem(3)
        reader.Close()
        reader.Dispose()
        aggiornoAllinemantoLib()

    End Sub
    Private Sub Panel1_Click(sender As Object, e As EventArgs) Handles Panel1.Click
        FolderBrowserDialog1.SelectedPath = Environment.CurrentDirectory
        If (FolderBrowserDialog1.ShowDialog = DialogResult.OK) Then
            percorsoIDE.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub RefreshPort_Click(sender As Object, e As EventArgs) Handles RefreshPort.Click
        Dim hold As String = portaCOM.Text
        portaCOM.Items.Clear()
        portaCOM.Items.AddRange(IO.Ports.SerialPort.GetPortNames())
        portaCOM.Text = hold
    End Sub
    Private Sub saveButton_Click(sender As Object, e As EventArgs) Handles saveButton.Click
        Dim settingsFile As String = percorsoIDE.Text & vbCrLf
        settingsFile = settingsFile & devboard.Text & vbCrLf
        settingsFile = settingsFile & uploadspeed.Text & vbCrLf
        settingsFile = settingsFile & portaCOM.Text & vbCrLf
        settingsFile = settingsFile & IDECheck.Text & vbCrLf
        settingsFile = settingsFile & IDEESPCheck.Text & vbCrLf
        settingsFile = settingsFile & percorsoIDE.Text & "\arduino-builder.exe" ' scrive il percorso di dovre si trova il compilatore di arduino


        File.WriteAllText(Environment.CurrentDirectory & dirfile, settingsFile) 'sostituisce il file
        Me.Close()
    End Sub
    Private Sub findArduino_Click(sender As Object, e As EventArgs) Handles findArduino.Click
        percorsoIDE.Text = SeachArduinoFolder()
        percorsoIDE_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub percorsoIDE_TextChanged(sender As Object, e As EventArgs) Handles percorsoIDE.TextChanged
        If (CehckArduinoProgrammer(percorsoIDE.Text)) Then
            IDECheck.ForeColor = Color.Green
            IDECheck.Text = "SI"
        Else
            IDECheck.ForeColor = Color.Red
            IDECheck.Text = "NO"
        End If
        Dim ESPRoot As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\IDEPortable\hardwareesp32\tools\esptool_py"
        If IO.Directory.Exists(Environment.CurrentDirectory & "\IDEPortable") Then
            ESPRoot = Environment.CurrentDirectory & "\IDEPortable\hardware\ESPp\packages\esp32\tools\esptool_py"

        End If

        If (CehckESPProgrammer(ESPRoot)) Then
            IDEESPCheck.ForeColor = Color.Green
            IDEESPCheck.Text = "SI"
        Else
            IDEESPCheck.ForeColor = Color.Red
            IDEESPCheck.Text = "ASSENTE"
        End If

        If percorsoIDE.Text = Environment.CurrentDirectory & "\IDEPortable" Then
            GroupBox2.Enabled = False
        Else
            GroupBox2.Enabled = True
        End If

    End Sub
    Private Sub aggiornoAllinemantoLib()
        If Not checkAllineamentoLibrerie() Then ' fa il check nella root dei documenti
            libCheck.Text = "NO"
            libCheck.ForeColor = Color.Red
            AllineaLib.Enabled = True
        Else
            libCheck.Text = "SI"
            libCheck.ForeColor = Color.Green
            AllineaLib.Enabled = False
        End If
    End Sub
    Private Sub AllineaLib_Click(sender As Object, e As EventArgs) Handles AllineaLib.Click
        AllineaLib.Enabled = False
        Dim thrAllineamento As New Threading.Thread(AddressOf AllineamentoLibrerie)
        thrAllineamento.SetApartmentState(Threading.ApartmentState.STA)
        thrAllineamento.IsBackground = False
        libCheck.Text = "Wait"
        libCheck.ForeColor = Color.Purple
        thrAllineamento.Start()

    End Sub
    Private Function checkAllineamentoLibrerie() As Boolean

        Dim basicFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Arduino\libraries"
        If Not Directory.Exists(basicFolder) Then ' se non esite ne ricerca un'altra
            Dim reader As IO.StreamReader
            reader = IO.File.OpenText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Arduino15\preferences.txt")
            Dim row As String = reader.ReadLine
            While row IsNot Nothing
                If row.Contains("sketchbook.path=") Then
                    basicFolder = row.Replace("sketchbook.path=", "") & "\libraries"
                    Exit While
                End If
                row = reader.ReadLine
            End While
        End If
        If Directory.Exists(basicFolder) Then ' se non esite ne ricerca un'altra
            For Each Libreria As String In Directory.GetDirectories(Environment.CurrentDirectory & dirmylib)
                Dim subroot As String() = Libreria.Split("\")
                If Not Directory.Exists(basicFolder & "\" & subroot(subroot.Length - 1)) Then
                    Return False
                End If
            Next
            Return True
        End If
        Throw New IO.DirectoryNotFoundException("Arduino\libreries directory does not exist or could not be found: " & basicFolder)
    End Function
    Private Sub AllineamentoLibrerie()
        Dim basicFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Arduino\libraries"
        For Each Libreria As String In Directory.GetDirectories(Environment.CurrentDirectory & dirmylib)
            Dim subroot As String() = Libreria.Split("\")
            If Not Directory.Exists(basicFolder & "\" & subroot(subroot.Length - 1)) Then
                DirectoryCopy(Libreria, basicFolder & "\" & subroot(subroot.Length - 1)) ' copia la libreria mancante
            End If
        Next
        Invoke(Sub() aggiornoAllinemantoLib())
    End Sub
    Private Function SeachArduinoFolder() As String
        If IO.Directory.Exists(Environment.CurrentDirectory & "\IDEPortable") Then
            Return Environment.CurrentDirectory & "\IDEPortable"
        Else
            Dim mainRoots() As String = Directory.GetLogicalDrives()
            For Each mainRoot As String In mainRoots
                If Directory.Exists(mainRoot & "Program Files\Arduino") Then
                    Return mainRoot & "Program Files\Arduino"
                ElseIf Directory.Exists(mainRoot & "Program Files (x86)\Arduino") Then
                    Return mainRoot & "Program Files (x86)\Arduino"
                End If
            Next
        End If

        Return "C:\"
    End Function
    Private Function CehckArduinoProgrammer(ByRef root As String) As Boolean

        If File.Exists(root & "\arduino-builder.exe") Then
            Return True
        End If
        Return False
    End Function
    Private Function CehckESPProgrammer(ByRef root As String) As Boolean

        For Each vers As String In Directory.GetDirectories(root)
            If File.Exists(vers & "\esptool.exe") Then
                Return True
            End If
        Next

        Return False
    End Function



End Class