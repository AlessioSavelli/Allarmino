<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MDIParent1
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MDIParent1))
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.FileMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolBarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusBarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.CentralinaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfiguratoreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AvanzatoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NotificheTelegramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdminTGToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CertificatiTLSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AggiungiEspansioneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProgrammaModuliToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CanBUSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisplayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SpenzioniIOToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProgrammaToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.WindowsMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.CascadeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TileVerticalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TileHorizontalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ArrangeIconsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip = New System.Windows.Forms.ToolStrip()
        Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.OpenToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.HelpToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.MenuStrip.SuspendLayout()
        Me.ToolStrip.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip
        '
        Me.MenuStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileMenu, Me.ViewMenu, Me.ToolsMenu, Me.WindowsMenu, Me.InfoToolStripMenuItem})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.MdiWindowListItem = Me.WindowsMenu
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Padding = New System.Windows.Forms.Padding(7, 2, 0, 2)
        Me.MenuStrip.Size = New System.Drawing.Size(1085, 28)
        Me.MenuStrip.TabIndex = 5
        Me.MenuStrip.Text = "MenuStrip"
        '
        'FileMenu
        '
        Me.FileMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.OpenToolStripMenuItem, Me.ToolStripSeparator3, Me.ExitToolStripMenuItem})
        Me.FileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder
        Me.FileMenu.Name = "FileMenu"
        Me.FileMenu.Size = New System.Drawing.Size(37, 24)
        Me.FileMenu.Text = "&File"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Image = CType(resources.GetObject("NewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.NewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.NewToolStripMenuItem.Text = "&Nuovo"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Image = CType(resources.GetObject("OpenToolStripMenuItem.Image"), System.Drawing.Image)
        Me.OpenToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.OpenToolStripMenuItem.Text = "&Apri"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(158, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ExitToolStripMenuItem.Text = "E&sci"
        '
        'ViewMenu
        '
        Me.ViewMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolBarToolStripMenuItem, Me.StatusBarToolStripMenuItem, Me.ToolStripSeparator2, Me.CentralinaToolStripMenuItem, Me.ProgrammaModuliToolStripMenuItem})
        Me.ViewMenu.Enabled = False
        Me.ViewMenu.Name = "ViewMenu"
        Me.ViewMenu.Size = New System.Drawing.Size(69, 24)
        Me.ViewMenu.Text = "&Visualizza"
        '
        'ToolBarToolStripMenuItem
        '
        Me.ToolBarToolStripMenuItem.Checked = True
        Me.ToolBarToolStripMenuItem.CheckOnClick = True
        Me.ToolBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolBarToolStripMenuItem.Name = "ToolBarToolStripMenuItem"
        Me.ToolBarToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.ToolBarToolStripMenuItem.Text = "&Barra degli strumenti"
        '
        'StatusBarToolStripMenuItem
        '
        Me.StatusBarToolStripMenuItem.Checked = True
        Me.StatusBarToolStripMenuItem.CheckOnClick = True
        Me.StatusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.StatusBarToolStripMenuItem.Name = "StatusBarToolStripMenuItem"
        Me.StatusBarToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.StatusBarToolStripMenuItem.Text = "Barra di &stato"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(181, 6)
        '
        'CentralinaToolStripMenuItem
        '
        Me.CentralinaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConfiguratoreToolStripMenuItem, Me.AvanzatoToolStripMenuItem, Me.AggiungiEspansioneToolStripMenuItem})
        Me.CentralinaToolStripMenuItem.Name = "CentralinaToolStripMenuItem"
        Me.CentralinaToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.CentralinaToolStripMenuItem.Text = "Centralina"
        '
        'ConfiguratoreToolStripMenuItem
        '
        Me.ConfiguratoreToolStripMenuItem.Image = Global.AllarminoIDE.My.Resources.Resources.confBasic
        Me.ConfiguratoreToolStripMenuItem.Name = "ConfiguratoreToolStripMenuItem"
        Me.ConfiguratoreToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.ConfiguratoreToolStripMenuItem.Text = "&Configuratore"
        '
        'AvanzatoToolStripMenuItem
        '
        Me.AvanzatoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NotificheTelegramToolStripMenuItem, Me.AdminTGToolStripMenuItem, Me.CertificatiTLSToolStripMenuItem})
        Me.AvanzatoToolStripMenuItem.Enabled = False
        Me.AvanzatoToolStripMenuItem.Image = Global.AllarminoIDE.My.Resources.Resources.confAvanzato
        Me.AvanzatoToolStripMenuItem.Name = "AvanzatoToolStripMenuItem"
        Me.AvanzatoToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.AvanzatoToolStripMenuItem.Text = "&Configuratore Avanzato"
        '
        'NotificheTelegramToolStripMenuItem
        '
        Me.NotificheTelegramToolStripMenuItem.Image = Global.AllarminoIDE.My.Resources.Resources.telegram
        Me.NotificheTelegramToolStripMenuItem.Name = "NotificheTelegramToolStripMenuItem"
        Me.NotificheTelegramToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.NotificheTelegramToolStripMenuItem.Text = "&Notifiche Telegram"
        '
        'AdminTGToolStripMenuItem
        '
        Me.AdminTGToolStripMenuItem.Image = CType(resources.GetObject("AdminTGToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AdminTGToolStripMenuItem.Name = "AdminTGToolStripMenuItem"
        Me.AdminTGToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.AdminTGToolStripMenuItem.Text = "&Admin Telegram"
        '
        'CertificatiTLSToolStripMenuItem
        '
        Me.CertificatiTLSToolStripMenuItem.Image = Global.AllarminoIDE.My.Resources.Resources.balance
        Me.CertificatiTLSToolStripMenuItem.Name = "CertificatiTLSToolStripMenuItem"
        Me.CertificatiTLSToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.CertificatiTLSToolStripMenuItem.Text = "&Certificati TLS"
        '
        'AggiungiEspansioneToolStripMenuItem
        '
        Me.AggiungiEspansioneToolStripMenuItem.Enabled = False
        Me.AggiungiEspansioneToolStripMenuItem.Name = "AggiungiEspansioneToolStripMenuItem"
        Me.AggiungiEspansioneToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.AggiungiEspansioneToolStripMenuItem.Text = "Aggiungi Espansione"
        '
        'ProgrammaModuliToolStripMenuItem
        '
        Me.ProgrammaModuliToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CanBUSToolStripMenuItem, Me.DisplayToolStripMenuItem, Me.SpenzioniIOToolStripMenuItem})
        Me.ProgrammaModuliToolStripMenuItem.Enabled = False
        Me.ProgrammaModuliToolStripMenuItem.Name = "ProgrammaModuliToolStripMenuItem"
        Me.ProgrammaModuliToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.ProgrammaModuliToolStripMenuItem.Text = "Espansioni"
        '
        'CanBUSToolStripMenuItem
        '
        Me.CanBUSToolStripMenuItem.Name = "CanBUSToolStripMenuItem"
        Me.CanBUSToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.CanBUSToolStripMenuItem.Text = "&Attivatori"
        '
        'DisplayToolStripMenuItem
        '
        Me.DisplayToolStripMenuItem.Name = "DisplayToolStripMenuItem"
        Me.DisplayToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.DisplayToolStripMenuItem.Text = "&Display"
        '
        'SpenzioniIOToolStripMenuItem
        '
        Me.SpenzioniIOToolStripMenuItem.Name = "SpenzioniIOToolStripMenuItem"
        Me.SpenzioniIOToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.SpenzioniIOToolStripMenuItem.Text = "&Espanzioni I/O"
        '
        'ToolsMenu
        '
        Me.ToolsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ProgrammaToolStripItem, Me.ToolStripSeparator4})
        Me.ToolsMenu.Enabled = False
        Me.ToolsMenu.Name = "ToolsMenu"
        Me.ToolsMenu.Size = New System.Drawing.Size(71, 24)
        Me.ToolsMenu.Text = "&Strumenti"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Image = Global.AllarminoIDE.My.Resources.Resources.settingsCompilatore
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(192, 22)
        Me.ToolStripMenuItem1.Text = "&Opzioni Compilatore"
        '
        'ProgrammaToolStripItem
        '
        Me.ProgrammaToolStripItem.Image = Global.AllarminoIDE.My.Resources.Resources.carica4
        Me.ProgrammaToolStripItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.ProgrammaToolStripItem.Name = "ProgrammaToolStripItem"
        Me.ProgrammaToolStripItem.Size = New System.Drawing.Size(192, 22)
        Me.ProgrammaToolStripItem.Text = "&Programma centralina"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(189, 6)
        '
        'WindowsMenu
        '
        Me.WindowsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CascadeToolStripMenuItem, Me.TileVerticalToolStripMenuItem, Me.TileHorizontalToolStripMenuItem, Me.CloseAllToolStripMenuItem, Me.ArrangeIconsToolStripMenuItem})
        Me.WindowsMenu.Enabled = False
        Me.WindowsMenu.Name = "WindowsMenu"
        Me.WindowsMenu.Size = New System.Drawing.Size(60, 24)
        Me.WindowsMenu.Text = "&Finestre"
        '
        'CascadeToolStripMenuItem
        '
        Me.CascadeToolStripMenuItem.Name = "CascadeToolStripMenuItem"
        Me.CascadeToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.CascadeToolStripMenuItem.Text = "&Sovrapponi"
        '
        'TileVerticalToolStripMenuItem
        '
        Me.TileVerticalToolStripMenuItem.Name = "TileVerticalToolStripMenuItem"
        Me.TileVerticalToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.TileVerticalToolStripMenuItem.Text = "Affianca &verticalmente"
        '
        'TileHorizontalToolStripMenuItem
        '
        Me.TileHorizontalToolStripMenuItem.Name = "TileHorizontalToolStripMenuItem"
        Me.TileHorizontalToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.TileHorizontalToolStripMenuItem.Text = "Affianca &orizzontalmente"
        '
        'CloseAllToolStripMenuItem
        '
        Me.CloseAllToolStripMenuItem.Name = "CloseAllToolStripMenuItem"
        Me.CloseAllToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.CloseAllToolStripMenuItem.Text = "C&hiudi tutte"
        '
        'ArrangeIconsToolStripMenuItem
        '
        Me.ArrangeIconsToolStripMenuItem.Name = "ArrangeIconsToolStripMenuItem"
        Me.ArrangeIconsToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.ArrangeIconsToolStripMenuItem.Text = "&Disponi icone"
        '
        'InfoToolStripMenuItem
        '
        Me.InfoToolStripMenuItem.Image = Global.AllarminoIDE.My.Resources.Resources.information
        Me.InfoToolStripMenuItem.Name = "InfoToolStripMenuItem"
        Me.InfoToolStripMenuItem.Size = New System.Drawing.Size(60, 24)
        Me.InfoToolStripMenuItem.Text = "&Info"
        '
        'ToolStrip
        '
        Me.ToolStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.OpenToolStripButton, Me.ToolStripSeparator1, Me.HelpToolStripButton})
        Me.ToolStrip.Location = New System.Drawing.Point(0, 28)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Size = New System.Drawing.Size(1085, 27)
        Me.ToolStrip.TabIndex = 6
        Me.ToolStrip.Text = "ToolStrip"
        '
        'NewToolStripButton
        '
        Me.NewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.NewToolStripButton.Image = CType(resources.GetObject("NewToolStripButton.Image"), System.Drawing.Image)
        Me.NewToolStripButton.ImageTransparentColor = System.Drawing.Color.Black
        Me.NewToolStripButton.Name = "NewToolStripButton"
        Me.NewToolStripButton.Size = New System.Drawing.Size(24, 24)
        Me.NewToolStripButton.Text = "Nuovo"
        '
        'OpenToolStripButton
        '
        Me.OpenToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.OpenToolStripButton.Image = CType(resources.GetObject("OpenToolStripButton.Image"), System.Drawing.Image)
        Me.OpenToolStripButton.ImageTransparentColor = System.Drawing.Color.Black
        Me.OpenToolStripButton.Name = "OpenToolStripButton"
        Me.OpenToolStripButton.Size = New System.Drawing.Size(24, 24)
        Me.OpenToolStripButton.Text = "Apri"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 27)
        '
        'HelpToolStripButton
        '
        Me.HelpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.HelpToolStripButton.Image = CType(resources.GetObject("HelpToolStripButton.Image"), System.Drawing.Image)
        Me.HelpToolStripButton.ImageTransparentColor = System.Drawing.Color.Black
        Me.HelpToolStripButton.Name = "HelpToolStripButton"
        Me.HelpToolStripButton.Size = New System.Drawing.Size(24, 24)
        Me.HelpToolStripButton.Text = "?"
        Me.HelpToolStripButton.Visible = False
        '
        'StatusStrip
        '
        Me.StatusStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 493)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 17, 0)
        Me.StatusStrip.Size = New System.Drawing.Size(1085, 22)
        Me.StatusStrip.TabIndex = 7
        Me.StatusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(121, 17)
        Me.ToolStripStatusLabel.Text = "By Alessio Savelli V1.0"
        '
        'MDIParent1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1085, 515)
        Me.Controls.Add(Me.ToolStrip)
        Me.Controls.Add(Me.MenuStrip)
        Me.Controls.Add(Me.StatusStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "MDIParent1"
        Me.Text = "Configuratore V1.0.0.1"
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ArrangeIconsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowsMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CascadeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TileVerticalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TileHorizontalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents NewToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents OpenToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents ViewMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolBarToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusBarToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ProgrammaToolStripItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents CentralinaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConfiguratoreToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AvanzatoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NotificheTelegramToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AdminTGToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AggiungiEspansioneToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ProgrammaModuliToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CanBUSToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DisplayToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SpenzioniIOToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CertificatiTLSToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents InfoToolStripMenuItem As ToolStripMenuItem
End Class
