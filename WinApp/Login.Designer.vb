<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Login
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Login))
        Me.UsernameTextBox = New DevExpress.XtraEditors.TextEdit
        Me.PasswordTextBox = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
        Me.btnLogin = New DevExpress.XtraEditors.SimpleButton
        Me.btnCancel = New DevExpress.XtraEditors.SimpleButton
        CType(Me.UsernameTextBox.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PasswordTextBox.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'UsernameTextBox
        '
        resources.ApplyResources(Me.UsernameTextBox, "UsernameTextBox")
        Me.UsernameTextBox.BackgroundImage = Nothing
        Me.UsernameTextBox.EditValue = Nothing
        Me.UsernameTextBox.Name = "UsernameTextBox"
        Me.UsernameTextBox.Properties.AccessibleDescription = Nothing
        Me.UsernameTextBox.Properties.AccessibleName = Nothing
        Me.UsernameTextBox.Properties.AutoHeight = CType(resources.GetObject("UsernameTextBox.Properties.AutoHeight"), Boolean)
        Me.UsernameTextBox.Properties.Mask.AutoComplete = CType(resources.GetObject("UsernameTextBox.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.UsernameTextBox.Properties.Mask.BeepOnError = CType(resources.GetObject("UsernameTextBox.Properties.Mask.BeepOnError"), Boolean)
        Me.UsernameTextBox.Properties.Mask.EditMask = resources.GetString("UsernameTextBox.Properties.Mask.EditMask")
        Me.UsernameTextBox.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("UsernameTextBox.Properties.Mask.IgnoreMaskBlank"), Boolean)
        Me.UsernameTextBox.Properties.Mask.MaskType = CType(resources.GetObject("UsernameTextBox.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.UsernameTextBox.Properties.Mask.PlaceHolder = CType(resources.GetObject("UsernameTextBox.Properties.Mask.PlaceHolder"), Char)
        Me.UsernameTextBox.Properties.Mask.SaveLiteral = CType(resources.GetObject("UsernameTextBox.Properties.Mask.SaveLiteral"), Boolean)
        Me.UsernameTextBox.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("UsernameTextBox.Properties.Mask.ShowPlaceHolders"), Boolean)
        Me.UsernameTextBox.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("UsernameTextBox.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
        '
        'PasswordTextBox
        '
        resources.ApplyResources(Me.PasswordTextBox, "PasswordTextBox")
        Me.PasswordTextBox.BackgroundImage = Nothing
        Me.PasswordTextBox.EditValue = Nothing
        Me.PasswordTextBox.Name = "PasswordTextBox"
        Me.PasswordTextBox.Properties.AccessibleDescription = Nothing
        Me.PasswordTextBox.Properties.AccessibleName = Nothing
        Me.PasswordTextBox.Properties.AutoHeight = CType(resources.GetObject("PasswordTextBox.Properties.AutoHeight"), Boolean)
        Me.PasswordTextBox.Properties.Mask.AutoComplete = CType(resources.GetObject("PasswordTextBox.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.PasswordTextBox.Properties.Mask.BeepOnError = CType(resources.GetObject("PasswordTextBox.Properties.Mask.BeepOnError"), Boolean)
        Me.PasswordTextBox.Properties.Mask.EditMask = resources.GetString("PasswordTextBox.Properties.Mask.EditMask")
        Me.PasswordTextBox.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("PasswordTextBox.Properties.Mask.IgnoreMaskBlank"), Boolean)
        Me.PasswordTextBox.Properties.Mask.MaskType = CType(resources.GetObject("PasswordTextBox.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.PasswordTextBox.Properties.Mask.PlaceHolder = CType(resources.GetObject("PasswordTextBox.Properties.Mask.PlaceHolder"), Char)
        Me.PasswordTextBox.Properties.Mask.SaveLiteral = CType(resources.GetObject("PasswordTextBox.Properties.Mask.SaveLiteral"), Boolean)
        Me.PasswordTextBox.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("PasswordTextBox.Properties.Mask.ShowPlaceHolders"), Boolean)
        Me.PasswordTextBox.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("PasswordTextBox.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
        Me.PasswordTextBox.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        '
        'LabelControl1
        '
        Me.LabelControl1.AccessibleDescription = Nothing
        Me.LabelControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.LabelControl1, "LabelControl1")
        Me.LabelControl1.Name = "LabelControl1"
        '
        'LabelControl2
        '
        Me.LabelControl2.AccessibleDescription = Nothing
        Me.LabelControl2.AccessibleName = Nothing
        resources.ApplyResources(Me.LabelControl2, "LabelControl2")
        Me.LabelControl2.Name = "LabelControl2"
        '
        'btnLogin
        '
        Me.btnLogin.AccessibleDescription = Nothing
        Me.btnLogin.AccessibleName = Nothing
        resources.ApplyResources(Me.btnLogin, "btnLogin")
        Me.btnLogin.BackgroundImage = Nothing
        Me.btnLogin.Name = "btnLogin"
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleDescription = Nothing
        Me.btnCancel.AccessibleName = Nothing
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.BackgroundImage = Nothing
        Me.btnCancel.Name = "btnCancel"
        '
        'Login
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.PasswordTextBox)
        Me.Controls.Add(Me.UsernameTextBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = Nothing
        Me.KeyPreview = True
        Me.LookAndFeel.SkinName = "Money Twins"
        Me.Name = "Login"
        Me.TopMost = True
        CType(Me.UsernameTextBox.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PasswordTextBox.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
  Friend WithEvents UsernameTextBox As DevExpress.XtraEditors.TextEdit
  Friend WithEvents PasswordTextBox As DevExpress.XtraEditors.TextEdit
  Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents btnLogin As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
End Class
