<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxKopcinja
    Inherits DevExpress.XtraEditors.XtraUserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxKopcinja))
        Me.cmdAdd = New DevExpress.XtraEditors.SimpleButton
        Me.KopcinjaLayoutControl = New DevExpress.XtraLayout.LayoutControl
        Me.cmdCancel = New DevExpress.XtraEditors.SimpleButton
        Me.cmdDelete = New DevExpress.XtraEditors.SimpleButton
        Me.cmdExit = New DevExpress.XtraEditors.SimpleButton
        Me.cmdSave = New DevExpress.XtraEditors.SimpleButton
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.cmdDeleteLayoutControlItem = New DevExpress.XtraLayout.LayoutControlItem
        Me.cmdExitLayoutControlItem = New DevExpress.XtraLayout.LayoutControlItem
        Me.cmdAddLayoutControlItem = New DevExpress.XtraLayout.LayoutControlItem
        Me.cmdSaveLayoutControlItem = New DevExpress.XtraLayout.LayoutControlItem
        Me.cmdCancelLayoutControlItem = New DevExpress.XtraLayout.LayoutControlItem
        CType(Me.KopcinjaLayoutControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.KopcinjaLayoutControl.SuspendLayout()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmdDeleteLayoutControlItem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmdExitLayoutControlItem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmdAddLayoutControlItem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmdSaveLayoutControlItem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmdCancelLayoutControlItem, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdAdd
        '
        resources.ApplyResources(Me.cmdAdd, "cmdAdd")
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.StyleController = Me.KopcinjaLayoutControl
        '
        'KopcinjaLayoutControl
        '
        Me.KopcinjaLayoutControl.Controls.Add(Me.cmdCancel)
        Me.KopcinjaLayoutControl.Controls.Add(Me.cmdAdd)
        Me.KopcinjaLayoutControl.Controls.Add(Me.cmdDelete)
        Me.KopcinjaLayoutControl.Controls.Add(Me.cmdExit)
        Me.KopcinjaLayoutControl.Controls.Add(Me.cmdSave)
        resources.ApplyResources(Me.KopcinjaLayoutControl, "KopcinjaLayoutControl")
        Me.KopcinjaLayoutControl.Name = "KopcinjaLayoutControl"
        Me.KopcinjaLayoutControl.OptionsView.AllowHotTrack = True
        Me.KopcinjaLayoutControl.OptionsView.DrawItemBorders = True
        Me.KopcinjaLayoutControl.Root = Me.LayoutControlGroup1
        '
        'cmdCancel
        '
        resources.ApplyResources(Me.cmdCancel, "cmdCancel")
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.StyleController = Me.KopcinjaLayoutControl
        '
        'cmdDelete
        '
        resources.ApplyResources(Me.cmdDelete, "cmdDelete")
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.StyleController = Me.KopcinjaLayoutControl
        '
        'cmdExit
        '
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.StyleController = Me.KopcinjaLayoutControl
        '
        'cmdSave
        '
        resources.ApplyResources(Me.cmdSave, "cmdSave")
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.StyleController = Me.KopcinjaLayoutControl
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.DefaultLayoutType = DevExpress.XtraLayout.Utils.LayoutType.Horizontal
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.cmdDeleteLayoutControlItem, Me.cmdExitLayoutControlItem, Me.cmdAddLayoutControlItem, Me.cmdSaveLayoutControlItem, Me.cmdCancelLayoutControlItem})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(500, 41)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'cmdDeleteLayoutControlItem
        '
        Me.cmdDeleteLayoutControlItem.AllowHotTrack = False
        Me.cmdDeleteLayoutControlItem.Control = Me.cmdDelete
        resources.ApplyResources(Me.cmdDeleteLayoutControlItem, "cmdDeleteLayoutControlItem")
        Me.cmdDeleteLayoutControlItem.Location = New System.Drawing.Point(200, 0)
        Me.cmdDeleteLayoutControlItem.MinSize = New System.Drawing.Size(68, 33)
        Me.cmdDeleteLayoutControlItem.Name = "cmdDeleteLayoutControlItem"
        Me.cmdDeleteLayoutControlItem.Size = New System.Drawing.Size(99, 39)
        Me.cmdDeleteLayoutControlItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.cmdDeleteLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Left
        Me.cmdDeleteLayoutControlItem.TextSize = New System.Drawing.Size(0, 0)
        Me.cmdDeleteLayoutControlItem.TextToControlDistance = 0
        Me.cmdDeleteLayoutControlItem.TextVisible = False
        '
        'cmdExitLayoutControlItem
        '
        Me.cmdExitLayoutControlItem.AllowHotTrack = False
        Me.cmdExitLayoutControlItem.Control = Me.cmdExit
        resources.ApplyResources(Me.cmdExitLayoutControlItem, "cmdExitLayoutControlItem")
        Me.cmdExitLayoutControlItem.Location = New System.Drawing.Point(399, 0)
        Me.cmdExitLayoutControlItem.MinSize = New System.Drawing.Size(59, 33)
        Me.cmdExitLayoutControlItem.Name = "cmdExitLayoutControlItem"
        Me.cmdExitLayoutControlItem.Size = New System.Drawing.Size(99, 39)
        Me.cmdExitLayoutControlItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.cmdExitLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Left
        Me.cmdExitLayoutControlItem.TextSize = New System.Drawing.Size(0, 0)
        Me.cmdExitLayoutControlItem.TextToControlDistance = 0
        Me.cmdExitLayoutControlItem.TextVisible = False
        '
        'cmdAddLayoutControlItem
        '
        Me.cmdAddLayoutControlItem.AllowHotTrack = False
        Me.cmdAddLayoutControlItem.Control = Me.cmdAdd
        resources.ApplyResources(Me.cmdAddLayoutControlItem, "cmdAddLayoutControlItem")
        Me.cmdAddLayoutControlItem.Location = New System.Drawing.Point(0, 0)
        Me.cmdAddLayoutControlItem.MinSize = New System.Drawing.Size(85, 33)
        Me.cmdAddLayoutControlItem.Name = "cmdAddLayoutControlItem"
        Me.cmdAddLayoutControlItem.Size = New System.Drawing.Size(100, 39)
        Me.cmdAddLayoutControlItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.cmdAddLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Left
        Me.cmdAddLayoutControlItem.TextSize = New System.Drawing.Size(0, 0)
        Me.cmdAddLayoutControlItem.TextToControlDistance = 0
        Me.cmdAddLayoutControlItem.TextVisible = False
        '
        'cmdSaveLayoutControlItem
        '
        Me.cmdSaveLayoutControlItem.AllowHotTrack = False
        Me.cmdSaveLayoutControlItem.Control = Me.cmdSave
        resources.ApplyResources(Me.cmdSaveLayoutControlItem, "cmdSaveLayoutControlItem")
        Me.cmdSaveLayoutControlItem.Location = New System.Drawing.Point(100, 0)
        Me.cmdSaveLayoutControlItem.MinSize = New System.Drawing.Size(66, 33)
        Me.cmdSaveLayoutControlItem.Name = "cmdSaveLayoutControlItem"
        Me.cmdSaveLayoutControlItem.Size = New System.Drawing.Size(100, 39)
        Me.cmdSaveLayoutControlItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.cmdSaveLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Left
        Me.cmdSaveLayoutControlItem.TextSize = New System.Drawing.Size(0, 0)
        Me.cmdSaveLayoutControlItem.TextToControlDistance = 0
        Me.cmdSaveLayoutControlItem.TextVisible = False
        '
        'cmdCancelLayoutControlItem
        '
        Me.cmdCancelLayoutControlItem.AllowHotTrack = False
        Me.cmdCancelLayoutControlItem.Control = Me.cmdCancel
        resources.ApplyResources(Me.cmdCancelLayoutControlItem, "cmdCancelLayoutControlItem")
        Me.cmdCancelLayoutControlItem.Location = New System.Drawing.Point(299, 0)
        Me.cmdCancelLayoutControlItem.MinSize = New System.Drawing.Size(64, 33)
        Me.cmdCancelLayoutControlItem.Name = "cmdCancelLayoutControlItem"
        Me.cmdCancelLayoutControlItem.Size = New System.Drawing.Size(100, 39)
        Me.cmdCancelLayoutControlItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.cmdCancelLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Left
        Me.cmdCancelLayoutControlItem.TextSize = New System.Drawing.Size(0, 0)
        Me.cmdCancelLayoutControlItem.TextToControlDistance = 0
        Me.cmdCancelLayoutControlItem.TextVisible = False
        '
        'uxKopcinja
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.KopcinjaLayoutControl)
        Me.Name = "uxKopcinja"
        CType(Me.KopcinjaLayoutControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.KopcinjaLayoutControl.ResumeLayout(False)
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmdDeleteLayoutControlItem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmdExitLayoutControlItem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmdAddLayoutControlItem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmdSaveLayoutControlItem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmdCancelLayoutControlItem, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

  Public WithEvents cmdAdd As DevExpress.XtraEditors.SimpleButton
  Public WithEvents cmdSave As DevExpress.XtraEditors.SimpleButton
  Public WithEvents cmdDelete As DevExpress.XtraEditors.SimpleButton
  Public WithEvents cmdCancel As DevExpress.XtraEditors.SimpleButton
  Public WithEvents cmdExit As DevExpress.XtraEditors.SimpleButton
  Public WithEvents KopcinjaLayoutControl As DevExpress.XtraLayout.LayoutControl
  Public WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Public WithEvents cmdDeleteLayoutControlItem As DevExpress.XtraLayout.LayoutControlItem
  Public WithEvents cmdExitLayoutControlItem As DevExpress.XtraLayout.LayoutControlItem
  Public WithEvents cmdCancelLayoutControlItem As DevExpress.XtraLayout.LayoutControlItem
  Public WithEvents cmdAddLayoutControlItem As DevExpress.XtraLayout.LayoutControlItem
  Public WithEvents cmdSaveLayoutControlItem As DevExpress.XtraLayout.LayoutControlItem

End Class
