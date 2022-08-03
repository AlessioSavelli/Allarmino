<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class settingsCompilatore
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.IDEESPCheck = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.findArduino = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.IDECheck = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.percorsoIDE = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.AllineaLib = New System.Windows.Forms.Button()
        Me.libCheck = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.RefreshPort = New System.Windows.Forms.Panel()
        Me.portaCOM = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.uploadspeed = New System.Windows.Forms.ComboBox()
        Me.devboard = New System.Windows.Forms.ComboBox()
        Me.saveButton = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.IDEESPCheck)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.findArduino)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.IDECheck)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.percorsoIDE)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(944, 118)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "IDE Arduino"
        '
        'IDEESPCheck
        '
        Me.IDEESPCheck.AutoSize = True
        Me.IDEESPCheck.Font = New System.Drawing.Font("Segoe UI", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.IDEESPCheck.ForeColor = System.Drawing.Color.Red
        Me.IDEESPCheck.Location = New System.Drawing.Point(387, 69)
        Me.IDEESPCheck.Name = "IDEESPCheck"
        Me.IDEESPCheck.Size = New System.Drawing.Size(47, 25)
        Me.IDEESPCheck.TabIndex = 157
        Me.IDEESPCheck.Text = "N/A"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(307, 73)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(87, 20)
        Me.Label7.TabIndex = 156
        Me.Label7.Text = "IDE ESP32 : "
        '
        'findArduino
        '
        Me.findArduino.Location = New System.Drawing.Point(736, 69)
        Me.findArduino.Name = "findArduino"
        Me.findArduino.Size = New System.Drawing.Size(165, 29)
        Me.findArduino.TabIndex = 1
        Me.findArduino.Text = "Ricerca Automatica"
        Me.findArduino.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackgroundImage = Global.AllarminoIDE.My.Resources.Resources.openfolder
        Me.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Panel1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Panel1.Location = New System.Drawing.Point(904, 29)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(34, 27)
        Me.Panel1.TabIndex = 155
        '
        'IDECheck
        '
        Me.IDECheck.AutoSize = True
        Me.IDECheck.Font = New System.Drawing.Font("Segoe UI", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.IDECheck.ForeColor = System.Drawing.Color.Red
        Me.IDECheck.Location = New System.Drawing.Point(119, 65)
        Me.IDECheck.Name = "IDECheck"
        Me.IDECheck.Size = New System.Drawing.Size(47, 25)
        Me.IDECheck.TabIndex = 3
        Me.IDECheck.Text = "N/A"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 69)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(121, 20)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Percorso valido : "
        '
        'percorsoIDE
        '
        Me.percorsoIDE.Location = New System.Drawing.Point(124, 29)
        Me.percorsoIDE.Name = "percorsoIDE"
        Me.percorsoIDE.ReadOnly = True
        Me.percorsoIDE.Size = New System.Drawing.Size(777, 27)
        Me.percorsoIDE.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(103, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Percorso IDE : "
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.AllineaLib)
        Me.GroupBox2.Controls.Add(Me.libCheck)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 136)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(307, 63)
        Me.GroupBox2.TabIndex = 156
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Librerie Aggiuntive"
        '
        'AllineaLib
        '
        Me.AllineaLib.Location = New System.Drawing.Point(217, 23)
        Me.AllineaLib.Name = "AllineaLib"
        Me.AllineaLib.Size = New System.Drawing.Size(84, 29)
        Me.AllineaLib.TabIndex = 1
        Me.AllineaLib.Text = "Allinea"
        Me.AllineaLib.UseVisualStyleBackColor = True
        '
        'libCheck
        '
        Me.libCheck.AutoSize = True
        Me.libCheck.Font = New System.Drawing.Font("Segoe UI", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.libCheck.ForeColor = System.Drawing.Color.Red
        Me.libCheck.Location = New System.Drawing.Point(164, 23)
        Me.libCheck.Name = "libCheck"
        Me.libCheck.Size = New System.Drawing.Size(47, 25)
        Me.libCheck.TabIndex = 3
        Me.libCheck.Text = "N/A"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(16, 27)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(155, 20)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Allinamento Librerie : "
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.RefreshPort)
        Me.GroupBox3.Controls.Add(Me.portaCOM)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.uploadspeed)
        Me.GroupBox3.Controls.Add(Me.devboard)
        Me.GroupBox3.Location = New System.Drawing.Point(325, 136)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(625, 105)
        Me.GroupBox3.TabIndex = 157
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "ESP32 Board"
        '
        'RefreshPort
        '
        Me.RefreshPort.BackgroundImage = Global.AllarminoIDE.My.Resources.Resources.refresh
        Me.RefreshPort.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.RefreshPort.Cursor = System.Windows.Forms.Cursors.Hand
        Me.RefreshPort.Location = New System.Drawing.Point(567, 63)
        Me.RefreshPort.Name = "RefreshPort"
        Me.RefreshPort.Size = New System.Drawing.Size(34, 27)
        Me.RefreshPort.TabIndex = 158
        '
        'portaCOM
        '
        Me.portaCOM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.portaCOM.FormattingEnabled = True
        Me.portaCOM.Location = New System.Drawing.Point(423, 63)
        Me.portaCOM.Name = "portaCOM"
        Me.portaCOM.Size = New System.Drawing.Size(137, 28)
        Me.portaCOM.TabIndex = 162
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(328, 71)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(89, 20)
        Me.Label9.TabIndex = 161
        Me.Label9.Text = "Porta Com : "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(302, 27)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(115, 20)
        Me.Label8.TabIndex = 160
        Me.Label8.Text = "Upload Speed : "
        '
        'uploadspeed
        '
        Me.uploadspeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uploadspeed.FormattingEnabled = True
        Me.uploadspeed.Items.AddRange(New Object() {"921600", "115200", "256000", "230400", "512000"})
        Me.uploadspeed.Location = New System.Drawing.Point(423, 24)
        Me.uploadspeed.Name = "uploadspeed"
        Me.uploadspeed.Size = New System.Drawing.Size(178, 28)
        Me.uploadspeed.TabIndex = 159
        '
        'devboard
        '
        Me.devboard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.devboard.FormattingEnabled = True
        Me.devboard.Items.AddRange(New Object() {"ESP32 Dev Module"})
        Me.devboard.Location = New System.Drawing.Point(6, 24)
        Me.devboard.Name = "devboard"
        Me.devboard.Size = New System.Drawing.Size(240, 28)
        Me.devboard.TabIndex = 158
        '
        'saveButton
        '
        Me.saveButton.Location = New System.Drawing.Point(12, 207)
        Me.saveButton.Name = "saveButton"
        Me.saveButton.Size = New System.Drawing.Size(307, 35)
        Me.saveButton.TabIndex = 158
        Me.saveButton.Text = "Salva"
        Me.saveButton.UseVisualStyleBackColor = True
        '
        'settingsCompilatore
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(973, 258)
        Me.Controls.Add(Me.saveButton)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "settingsCompilatore"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Settings Compilatore"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents percorsoIDE As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents IDECheck As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents findArduino As Button
    Friend WithEvents IDEESPCheck As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents AllineaLib As Button
    Friend WithEvents libCheck As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents devboard As ComboBox
    Friend WithEvents RefreshPort As Panel
    Friend WithEvents portaCOM As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents uploadspeed As ComboBox
    Friend WithEvents saveButton As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
End Class
