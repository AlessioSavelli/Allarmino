<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SimulatoreEmail
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.orariouno = New System.Windows.Forms.Label()
        Me.mittenteuno = New System.Windows.Forms.Label()
        Me.DisplayNameuno = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ricevente = New System.Windows.Forms.Label()
        Me.orariodue = New System.Windows.Forms.Label()
        Me.mittentedue = New System.Windows.Forms.Label()
        Me.Displatnamedue = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'orariouno
        '
        Me.orariouno.AutoSize = True
        Me.orariouno.BackColor = System.Drawing.Color.Transparent
        Me.orariouno.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.orariouno.ForeColor = System.Drawing.Color.DarkGray
        Me.orariouno.Location = New System.Drawing.Point(465, 166)
        Me.orariouno.Name = "orariouno"
        Me.orariouno.Size = New System.Drawing.Size(132, 20)
        Me.orariouno.TabIndex = 5
        Me.orariouno.Text = "99 mai 1900, 00:00"
        '
        'mittenteuno
        '
        Me.mittenteuno.AutoSize = True
        Me.mittenteuno.BackColor = System.Drawing.Color.Transparent
        Me.mittenteuno.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.mittenteuno.ForeColor = System.Drawing.Color.DarkGray
        Me.mittenteuno.Location = New System.Drawing.Point(211, 169)
        Me.mittenteuno.Name = "mittenteuno"
        Me.mittenteuno.Size = New System.Drawing.Size(85, 20)
        Me.mittenteuno.TabIndex = 4
        Me.mittenteuno.Text = "<Mittente>"
        '
        'DisplayNameuno
        '
        Me.DisplayNameuno.AutoSize = True
        Me.DisplayNameuno.BackColor = System.Drawing.Color.Transparent
        Me.DisplayNameuno.Font = New System.Drawing.Font("Segoe UI", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.DisplayNameuno.Location = New System.Drawing.Point(98, 167)
        Me.DisplayNameuno.Name = "DisplayNameuno"
        Me.DisplayNameuno.Size = New System.Drawing.Size(116, 23)
        Me.DisplayNameuno.TabIndex = 3
        Me.DisplayNameuno.Text = "DisplayName"
        '
        'Panel1
        '
        Me.Panel1.BackgroundImage = Global.AllarminoIDE.My.Resources.Resources.scroll_down
        Me.Panel1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Panel1.Location = New System.Drawing.Point(144, 197)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(12, 15)
        Me.Panel1.TabIndex = 6
        '
        'Panel2
        '
        Me.Panel2.BackgroundImage = Global.AllarminoIDE.My.Resources.Resources.test_email2
        Me.Panel2.Controls.Add(Me.ricevente)
        Me.Panel2.Controls.Add(Me.orariodue)
        Me.Panel2.Controls.Add(Me.mittentedue)
        Me.Panel2.Controls.Add(Me.Displatnamedue)
        Me.Panel2.Location = New System.Drawing.Point(144, 218)
        Me.Panel2.MaximumSize = New System.Drawing.Size(547, 196)
        Me.Panel2.MinimumSize = New System.Drawing.Size(547, 196)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(547, 196)
        Me.Panel2.TabIndex = 7
        Me.Panel2.Visible = False
        '
        'ricevente
        '
        Me.ricevente.AutoSize = True
        Me.ricevente.BackColor = System.Drawing.Color.Transparent
        Me.ricevente.Font = New System.Drawing.Font("Segoe UI", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.ricevente.ForeColor = System.Drawing.Color.Black
        Me.ricevente.Location = New System.Drawing.Point(218, 49)
        Me.ricevente.Name = "ricevente"
        Me.ricevente.Size = New System.Drawing.Size(111, 25)
        Me.ricevente.TabIndex = 13
        Me.ricevente.Text = "<Ricevente>"
        '
        'orariodue
        '
        Me.orariodue.AutoSize = True
        Me.orariodue.BackColor = System.Drawing.Color.Transparent
        Me.orariodue.Font = New System.Drawing.Font("Segoe UI", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.orariodue.ForeColor = System.Drawing.Color.Black
        Me.orariodue.Location = New System.Drawing.Point(165, 80)
        Me.orariodue.Name = "orariodue"
        Me.orariodue.Size = New System.Drawing.Size(164, 25)
        Me.orariodue.TabIndex = 12
        Me.orariodue.Text = "99 mai 1900, 00:00"
        '
        'mittentedue
        '
        Me.mittentedue.AutoSize = True
        Me.mittentedue.BackColor = System.Drawing.Color.Transparent
        Me.mittentedue.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.mittentedue.ForeColor = System.Drawing.Color.Black
        Me.mittentedue.Location = New System.Drawing.Point(278, 24)
        Me.mittentedue.Name = "mittentedue"
        Me.mittentedue.Size = New System.Drawing.Size(85, 20)
        Me.mittentedue.TabIndex = 11
        Me.mittentedue.Text = "<Mittente>"
        '
        'Displatnamedue
        '
        Me.Displatnamedue.AutoSize = True
        Me.Displatnamedue.BackColor = System.Drawing.Color.Transparent
        Me.Displatnamedue.Font = New System.Drawing.Font("Segoe UI", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Displatnamedue.Location = New System.Drawing.Point(164, 22)
        Me.Displatnamedue.Name = "Displatnamedue"
        Me.Displatnamedue.Size = New System.Drawing.Size(116, 23)
        Me.Displatnamedue.TabIndex = 7
        Me.Displatnamedue.Text = "DisplayName"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Transparent
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Panel3.Location = New System.Drawing.Point(28, 18)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(32, 30)
        Me.Panel3.TabIndex = 7
        '
        'SimulatoreEmail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.AllarminoIDE.My.Resources.Resources.test_emaill
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(857, 816)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.orariouno)
        Me.Controls.Add(Me.mittenteuno)
        Me.Controls.Add(Me.DisplayNameuno)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximumSize = New System.Drawing.Size(857, 816)
        Me.MinimumSize = New System.Drawing.Size(857, 816)
        Me.Name = "SimulatoreEmail"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SimulatoreEmail"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents orariouno As Label
    Friend WithEvents mittenteuno As Label
    Friend WithEvents DisplayNameuno As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Displatnamedue As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents ricevente As Label
    Friend WithEvents orariodue As Label
    Friend WithEvents mittentedue As Label
End Class
