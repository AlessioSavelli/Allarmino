Imports System.Reflection
Imports System.Threading
Public NotInheritable Class SplashScreen

    'TODO: questo form può essere facilmente impostato come schermata iniziale per l'applicazione dalla scheda "Applicazione"
    '  di Creazione progetti (scegliere "Proprietà" dal menu "Progetto").
    Dim thUpdate As New Thread(AddressOf updatesoftware)

    Private Sub SplashScreen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Imposta il testo della finestra di dialogo in fase di esecuzione in base alle informazioni sull'assembly dell'applicazione.  

        'TODO: personalizzare le informazioni sull'assembly dell'applicazione nel riquadro "Applicazione" 
        '  della finestra delle proprietà del progetto (accessibile dal menu "Progetto").

        'Titolo dell'applicazione
        Dim sapplicationTitle As String = Assembly.GetExecutingAssembly().GetCustomAttribute(Of AssemblyTitleAttribute)()?.Title

        If String.IsNullOrEmpty(sapplicationTitle) Then
            sapplicationTitle = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().GetName().Name)
        End If

        ApplicationTitle.Text = sapplicationTitle

        Version.Text = String.Format("Version {0}", Assembly.GetExecutingAssembly().GetName().Version)
        'Informazioni sul copyright


        thUpdate.IsBackground = False
        thUpdate.SetApartmentState(ApartmentState.STA)
        thUpdate.Start()
    End Sub
    Private Sub updatesoftware()
        If Not IO.File.Exists(Environment.CurrentDirectory & "\Licenza.txt") Then
            Dim licenza As New Dialog1
            If licenza.ShowDialog = DialogResult.Yes Then
                If licenza.CheckBox1.Checked Then IO.File.WriteAllText(Environment.CurrentDirectory & "\Licenza.txt", My.Resources.LICENZA)
            Else
                Process.GetCurrentProcess.Kill()
            End If
        End If
        Invoke(Sub() Me.TopMost = True)

        'controlla se esiste il file delle info
        If Not File.Exists(Environment.CurrentDirectory & "\info.inf") Then
            IO.File.WriteAllText(Environment.CurrentDirectory & "\info.inf", My.Resources.info)
        End If
        'controlla se esiste la cartella risorse
        If Not IO.Directory.Exists(Environment.CurrentDirectory & "\Progetti") Then
            IO.Directory.CreateDirectory(Environment.CurrentDirectory & "\Progetti")
        End If
        If Not IO.Directory.Exists(Environment.CurrentDirectory & "\Resource") Then
            Invoke(Sub() Copyright.Text = "Creazione cartella Resource...")
            Thread.Sleep(2000)
            Dim stream As IO.FileStream = IO.File.Create(Environment.CurrentDirectory & "\val.zip")
            stream.Write(My.Resources.Resource, 0, My.Resources.Resource.Length)
            stream.Dispose()
            stream.Close()
            IO.Compression.ZipFile.ExtractToDirectory(Environment.CurrentDirectory & "\val.zip", Environment.CurrentDirectory)
            IO.File.Delete(Environment.CurrentDirectory & "\val.zip")



        End If
        'controlla se esiste l'ide
        If Not IO.Directory.Exists(Environment.CurrentDirectory & "\IDEPortable") Then
            'estrazione dell'ide
            Invoke(Sub() Copyright.Text = "Creazione dell'IDE...")
            Thread.Sleep(2000)
            Dim stream As IO.FileStream = IO.File.Create(Environment.CurrentDirectory & "\ide.zip")
            stream.Write(My.Resources.IDEPortable, 0, My.Resources.IDEPortable.Length)
            stream.Dispose()
            stream.Close()
            IO.Compression.ZipFile.ExtractToDirectory(Environment.CurrentDirectory & "\ide.zip", Environment.CurrentDirectory)
            IO.File.Delete(Environment.CurrentDirectory & "\ide.zip")
        End If

        Invoke(Sub() Copyright.Text = "Controllo Aggiornamenti...")



        Thread.Sleep(2500)
        Invoke(Sub() Me.DialogResult = DialogResult.OK)
    End Sub


End Class
