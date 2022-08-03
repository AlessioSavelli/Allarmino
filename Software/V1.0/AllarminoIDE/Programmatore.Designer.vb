<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Programmatore
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Programmatore))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Status = New System.Windows.Forms.Label()
        Me.Dettagli = New System.Windows.Forms.CheckBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.DebugConsole = New System.Windows.Forms.RichTextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Compilazione = New System.Windows.Forms.CheckBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Status
        '
        resources.ApplyResources(Me.Status, "Status")
        Me.Status.Name = "Status"
        '
        'Dettagli
        '
        resources.ApplyResources(Me.Dettagli, "Dettagli")
        Me.Dettagli.Name = "Dettagli"
        Me.Dettagli.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        resources.ApplyResources(Me.ProgressBar1, "ProgressBar1")
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'DebugConsole
        '
        resources.ApplyResources(Me.DebugConsole, "DebugConsole")
        Me.DebugConsole.BackColor = System.Drawing.SystemColors.InfoText
        Me.DebugConsole.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.DebugConsole.Name = "DebugConsole"
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Compilazione
        '
        resources.ApplyResources(Me.Compilazione, "Compilazione")
        Me.Compilazione.Checked = True
        Me.Compilazione.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Compilazione.Name = "Compilazione"
        Me.Compilazione.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        resources.ApplyResources(Me.CheckBox1, "CheckBox1")
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Programmatore
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Compilazione)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.DebugConsole)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Dettagli)
        Me.Controls.Add(Me.Status)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Programmatore"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Status As Label
    Friend WithEvents Dettagli As CheckBox
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents DebugConsole As RichTextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Compilazione As CheckBox
    Friend WithEvents CheckBox1 As CheckBox
End Class
