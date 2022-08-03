<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdminSession
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AdminSession))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.AdminIDLETime = New System.Windows.Forms.NumericUpDown()
        Me.ShowWiFiPass = New System.Windows.Forms.CheckBox()
        Me.AdminPassword = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.AdminIDLETime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(195, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(239, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Sessione Amministratore Telegram"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(182, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 20)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Password : "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(77, 74)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(186, 20)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Durata massima sessione : "
        '
        'AdminIDLETime
        '
        Me.AdminIDLETime.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.AdminIDLETime.Location = New System.Drawing.Point(269, 72)
        Me.AdminIDLETime.Maximum = New Decimal(New Integer() {3600, 0, 0, 0})
        Me.AdminIDLETime.Minimum = New Decimal(New Integer() {120, 0, 0, 0})
        Me.AdminIDLETime.Name = "AdminIDLETime"
        Me.AdminIDLETime.Size = New System.Drawing.Size(83, 27)
        Me.AdminIDLETime.TabIndex = 157
        Me.AdminIDLETime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.AdminIDLETime.Value = New Decimal(New Integer() {300, 0, 0, 0})
        '
        'ShowWiFiPass
        '
        Me.ShowWiFiPass.AutoSize = True
        Me.ShowWiFiPass.Location = New System.Drawing.Point(565, 41)
        Me.ShowWiFiPass.Name = "ShowWiFiPass"
        Me.ShowWiFiPass.Size = New System.Drawing.Size(77, 24)
        Me.ShowWiFiPass.TabIndex = 156
        Me.ShowWiFiPass.Text = "Mostra"
        Me.ShowWiFiPass.UseVisualStyleBackColor = True
        '
        'AdminPassword
        '
        Me.AdminPassword.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.AdminPassword.Location = New System.Drawing.Point(269, 39)
        Me.AdminPassword.Name = "AdminPassword"
        Me.AdminPassword.PlaceholderText = "Password"
        Me.AdminPassword.Size = New System.Drawing.Size(290, 27)
        Me.AdminPassword.TabIndex = 155
        Me.AdminPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.AdminPassword.UseSystemPasswordChar = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(358, 74)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 20)
        Me.Label4.TabIndex = 158
        Me.Label4.Text = "secondi"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.BackgroundImage = CType(resources.GetObject("FlowLayoutPanel1.BackgroundImage"), System.Drawing.Image)
        Me.FlowLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(2, 2)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(78, 77)
        Me.FlowLayoutPanel1.TabIndex = 159
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label5.Location = New System.Drawing.Point(21, 102)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(609, 92)
        Me.Label5.TabIndex = 160
        Me.Label5.Text = resources.GetString("Label5.Text")
        '
        'AdminSession
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(652, 205)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.AdminIDLETime)
        Me.Controls.Add(Me.ShowWiFiPass)
        Me.Controls.Add(Me.AdminPassword)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "AdminSession"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Configurazione sessione admin"
        CType(Me.AdminIDLETime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents AdminIDLETime As NumericUpDown
    Friend WithEvents ShowWiFiPass As CheckBox
    Friend WithEvents AdminPassword As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents Label5 As Label
End Class
