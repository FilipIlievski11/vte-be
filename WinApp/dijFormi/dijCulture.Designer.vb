<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijCulture
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dijCulture))
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.btnExit = New DevExpress.XtraEditors.SimpleButton
    Me.btnOK = New DevExpress.XtraEditors.SimpleButton
    Me.ComboBoxEdit1 = New DevExpress.XtraEditors.ComboBoxEdit
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.ComboBoxEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'LayoutControl1
    '
    Me.LayoutControl1.AccessibleDescription = Nothing
    Me.LayoutControl1.AccessibleName = Nothing
    resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
    Me.LayoutControl1.BackgroundImage = Nothing
    Me.LayoutControl1.Controls.Add(Me.btnExit)
    Me.LayoutControl1.Controls.Add(Me.btnOK)
    Me.LayoutControl1.Controls.Add(Me.ComboBoxEdit1)
    Me.LayoutControl1.Font = Nothing
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    '
    'btnExit
    '
    Me.btnExit.AccessibleDescription = Nothing
    Me.btnExit.AccessibleName = Nothing
    resources.ApplyResources(Me.btnExit, "btnExit")
    Me.btnExit.BackgroundImage = Nothing
    Me.btnExit.Name = "btnExit"
    Me.btnExit.StyleController = Me.LayoutControl1
    '
    'btnOK
    '
    Me.btnOK.AccessibleDescription = Nothing
    Me.btnOK.AccessibleName = Nothing
    resources.ApplyResources(Me.btnOK, "btnOK")
    Me.btnOK.BackgroundImage = Nothing
    Me.btnOK.Name = "btnOK"
    Me.btnOK.StyleController = Me.LayoutControl1
    '
    'ComboBoxEdit1
    '
    resources.ApplyResources(Me.ComboBoxEdit1, "ComboBoxEdit1")
    Me.ComboBoxEdit1.BackgroundImage = Nothing
    Me.ComboBoxEdit1.EditValue = Nothing
    Me.ComboBoxEdit1.Name = "ComboBoxEdit1"
    Me.ComboBoxEdit1.Properties.AccessibleDescription = Nothing
    Me.ComboBoxEdit1.Properties.AccessibleName = Nothing
    Me.ComboBoxEdit1.Properties.AutoHeight = CType(resources.GetObject("ComboBoxEdit1.Properties.AutoHeight"), Boolean)
    Me.ComboBoxEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("ComboBoxEdit1.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.ComboBoxEdit1.Properties.Items.AddRange(New Object() {resources.GetString("ComboBoxEdit1.Properties.Items"), resources.GetString("ComboBoxEdit1.Properties.Items1")})
    Me.ComboBoxEdit1.StyleController = Me.LayoutControl1
    '
    'LayoutControlGroup1
    '
    resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(297, 84)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.TextVisible = False
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.ComboBoxEdit1
    resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(295, 31)
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(47, 20)
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.btnOK
    resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
    Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 31)
    Me.LayoutControlItem2.MinSize = New System.Drawing.Size(92, 33)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(152, 51)
    Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem2.TextToControlDistance = 0
    Me.LayoutControlItem2.TextVisible = False
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.btnExit
    resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
    Me.LayoutControlItem3.Location = New System.Drawing.Point(152, 31)
    Me.LayoutControlItem3.MinSize = New System.Drawing.Size(92, 33)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(143, 51)
    Me.LayoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem3.TextToControlDistance = 0
    Me.LayoutControlItem3.TextVisible = False
    '
    'dijCulture
    '
    Me.AccessibleDescription = Nothing
    Me.AccessibleName = Nothing
    resources.ApplyResources(Me, "$this")
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.LayoutControl1)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
    Me.Icon = Nothing
    Me.Name = "dijCulture"
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.ComboBoxEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents ComboBoxEdit1 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
End Class
