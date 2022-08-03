<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuovoProgetto
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.NomeProgetto = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PercorsoFile = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.Versione = New System.Windows.Forms.ComboBox()
        Me.BackgroundWorker2 = New System.ComponentModel.BackgroundWorker()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(123, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Nome Progetto : "
        '
        'NomeProgetto
        '
        Me.NomeProgetto.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.NomeProgetto.Location = New System.Drawing.Point(141, 6)
        Me.NomeProgetto.Name = "NomeProgetto"
        Me.NomeProgetto.PlaceholderText = "Inserisci Nome Progetto"
        Me.NomeProgetto.Size = New System.Drawing.Size(336, 27)
        Me.NomeProgetto.TabIndex = 151
        Me.NomeProgetto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(59, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 20)
        Me.Label2.TabIndex = 152
        Me.Label2.Text = "Percorso : "
        '
        'PercorsoFile
        '
        Me.PercorsoFile.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PercorsoFile.Location = New System.Drawing.Point(141, 42)
        Me.PercorsoFile.Name = "PercorsoFile"
        Me.PercorsoFile.ReadOnly = True
        Me.PercorsoFile.Size = New System.Drawing.Size(296, 27)
        Me.PercorsoFile.TabIndex = 153
        Me.PercorsoFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Panel1
        '
        Me.Panel1.BackgroundImage = Global.AllarminoIDE.My.Resources.Resources.openfolder
        Me.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Panel1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Panel1.Location = New System.Drawing.Point(443, 42)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(34, 27)
        Me.Panel1.TabIndex = 154
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(1, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(134, 20)
        Me.Label3.TabIndex = 155
        Me.Label3.Text = "Vers. Fw Progetto : "
        '
        'Versione
        '
        Me.Versione.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Versione.DisplayMember = "0"
        Me.Versione.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Versione.FormattingEnabled = True
        Me.Versione.Location = New System.Drawing.Point(141, 75)
        Me.Versione.Name = "Versione"
        Me.Versione.Size = New System.Drawing.Size(296, 28)
        Me.Versione.TabIndex = 156
        Me.Versione.ValueMember = "0"
        '
        'Panel2
        '
        Me.Panel2.BackgroundImage = Global.AllarminoIDE.My.Resources.Resources.add
        Me.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Panel2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Panel2.Location = New System.Drawing.Point(443, 76)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(34, 27)
        Me.Panel2.TabIndex = 155
        '
        'NuovoProgetto
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(493, 116)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Versione)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.PercorsoFile)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.NomeProgetto)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "NuovoProgetto"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Nuovo Progetto"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Public WithEvents NomeProgetto As TextBox
    Friend WithEvents Label2 As Label
    Public WithEvents PercorsoFile As TextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents BackgroundWorker2 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Panel2 As Panel
    Public WithEvents Versione As ComboBox
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
End Class
