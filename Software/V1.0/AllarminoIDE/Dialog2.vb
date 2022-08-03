Imports System.Windows.Forms
Imports System.Reflection
Public Class Dialog2
    Dim GitHubLink As String = Nothing
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        If GitHubLink IsNot Nothing Then
            Dim batfile As String = "start " & GitHubLink
            IO.File.WriteAllText(Environment.CurrentDirectory & "\ShowGH.bat", batfile)
            Dim myProcesso As New Process()
            Dim strInfo As ProcessStartInfo = myProcesso.StartInfo
            strInfo.FileName = Environment.CurrentDirectory & "\ShowGH.bat"
            strInfo.Arguments = ""
            strInfo.UseShellExecute = False
            strInfo.CreateNoWindow = True
            strInfo.RedirectStandardOutput = True
            strInfo.RedirectStandardError = True
            strInfo.RedirectStandardInput = True
            myProcesso.Start()

        End If
    End Sub

    Private Sub Dialog2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If IO.File.Exists(Environment.CurrentDirectory & "\info.inf") Then
            Dim AssemblyStringInfo As String = String.Format("Versione : {0}", Assembly.GetExecutingAssembly().GetName().Version) & vbCrLf & String.Format("Nome : {0}", Assembly.GetExecutingAssembly().GetName().Name) & vbCrLf & String.Format("Compatibilita' : {0}", Assembly.GetExecutingAssembly().GetName().VersionCompatibility)

            Dim rd As String() = IO.File.ReadAllLines(Environment.CurrentDirectory & "\info.inf")
            For Each line As String In rd
                If line IsNot Nothing Then
                    If line.Contains("[GitHubLink]") Then
                        GitHubLink = line.Replace("GitHubLink", "")
                    Else
                        line = line.Replace("[Assembly Info]", AssemblyStringInfo)
                    End If
                    GeneralInfo.AppendText(line & vbCrLf)
                End If
            Next
        Else
            GeneralInfo.AppendText("info non disponibili, file info.inf non trovato.")
        End If

        If IO.File.Exists(Environment.CurrentDirectory & "\Licenza.txt") Then
            Dim rd As String = IO.File.ReadAllText(Environment.CurrentDirectory & "\Licenza.txt")
            Eula.AppendText(rd)
        Else
            Eula.AppendText("Eula non disponibile, file Licenza.txt non trovato.")
        End If

    End Sub


End Class
