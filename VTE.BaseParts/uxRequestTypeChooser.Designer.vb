<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxRequestTypeChooser
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
    Me.components = New System.ComponentModel.Container
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxRequestTypeChooser))
    Me.RequestChooserLayoutControl = New DevExpress.XtraLayout.LayoutControl
    Me.btnNext = New DevExpress.XtraEditors.SimpleButton
    Me.btnBack = New DevExpress.XtraEditors.SimpleButton
    Me.RequestTypesRadioGroup = New DevExpress.XtraEditors.RadioGroup
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.CaptionEmptySpaceItem = New DevExpress.XtraLayout.EmptySpaceItem
    Me.MessageEmptySpaceItem = New DevExpress.XtraLayout.EmptySpaceItem
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    Me.RequestTypeListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    CType(Me.RequestChooserLayoutControl, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.RequestChooserLayoutControl.SuspendLayout()
    CType(Me.RequestTypesRadioGroup.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CaptionEmptySpaceItem, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.MessageEmptySpaceItem, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RequestTypeListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'RequestChooserLayoutControl
    '
    Me.RequestChooserLayoutControl.AccessibleDescription = Nothing
    Me.RequestChooserLayoutControl.AccessibleName = Nothing
    resources.ApplyResources(Me.RequestChooserLayoutControl, "RequestChooserLayoutControl")
    Me.RequestChooserLayoutControl.Appearance.DisabledLayoutGroupCaption.ForeColor = System.Drawing.SystemColors.GrayText
    Me.RequestChooserLayoutControl.Appearance.DisabledLayoutGroupCaption.Options.UseForeColor = True
    Me.RequestChooserLayoutControl.Appearance.DisabledLayoutItem.ForeColor = System.Drawing.SystemColors.GrayText
    Me.RequestChooserLayoutControl.Appearance.DisabledLayoutItem.Options.UseForeColor = True
    Me.RequestChooserLayoutControl.BackgroundImage = Nothing
    Me.RequestChooserLayoutControl.Controls.Add(Me.btnNext)
    Me.RequestChooserLayoutControl.Controls.Add(Me.btnBack)
    Me.RequestChooserLayoutControl.Controls.Add(Me.RequestTypesRadioGroup)
    Me.RequestChooserLayoutControl.Font = Nothing
    Me.RequestChooserLayoutControl.Name = "RequestChooserLayoutControl"
    Me.RequestChooserLayoutControl.Root = Me.LayoutControlGroup1
    '
    'btnNext
    '
    Me.btnNext.AccessibleDescription = Nothing
    Me.btnNext.AccessibleName = Nothing
    resources.ApplyResources(Me.btnNext, "btnNext")
    Me.btnNext.BackgroundImage = Nothing
    Me.btnNext.Name = "btnNext"
    Me.btnNext.StyleController = Me.RequestChooserLayoutControl
    '
    'btnBack
    '
    Me.btnBack.AccessibleDescription = Nothing
    Me.btnBack.AccessibleName = Nothing
    resources.ApplyResources(Me.btnBack, "btnBack")
    Me.btnBack.BackgroundImage = Nothing
    Me.btnBack.Name = "btnBack"
    Me.btnBack.StyleController = Me.RequestChooserLayoutControl
    '
    'RequestTypesRadioGroup
    '
    resources.ApplyResources(Me.RequestTypesRadioGroup, "RequestTypesRadioGroup")
    Me.RequestTypesRadioGroup.BackgroundImage = Nothing
    Me.RequestTypesRadioGroup.EditValue = Nothing
    Me.RequestTypesRadioGroup.Name = "RequestTypesRadioGroup"
    Me.RequestTypesRadioGroup.Properties.AccessibleDescription = Nothing
    Me.RequestTypesRadioGroup.Properties.AccessibleName = Nothing
    Me.RequestTypesRadioGroup.StyleController = Me.RequestChooserLayoutControl
    '
    'LayoutControlGroup1
    '
    resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.CaptionEmptySpaceItem, Me.MessageEmptySpaceItem, Me.LayoutControlItem2, Me.LayoutControlItem3})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(753, 190)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.TextVisible = False
    '
    'CaptionEmptySpaceItem
    '
    Me.CaptionEmptySpaceItem.AppearanceItemCaption.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.CaptionEmptySpaceItem.AppearanceItemCaption.Options.UseFont = True
    resources.ApplyResources(Me.CaptionEmptySpaceItem, "CaptionEmptySpaceItem")
    Me.CaptionEmptySpaceItem.Location = New System.Drawing.Point(0, 0)
    Me.CaptionEmptySpaceItem.MaxSize = New System.Drawing.Size(0, 31)
    Me.CaptionEmptySpaceItem.MinSize = New System.Drawing.Size(10, 31)
    Me.CaptionEmptySpaceItem.Name = "CaptionEmptySpaceItem"
    Me.CaptionEmptySpaceItem.Size = New System.Drawing.Size(751, 31)
    Me.CaptionEmptySpaceItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.CaptionEmptySpaceItem.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.CaptionEmptySpaceItem.TextSize = New System.Drawing.Size(0, 20)
    Me.CaptionEmptySpaceItem.TextVisible = True
    '
    'MessageEmptySpaceItem
    '
    resources.ApplyResources(Me.MessageEmptySpaceItem, "MessageEmptySpaceItem")
    Me.MessageEmptySpaceItem.Location = New System.Drawing.Point(0, 31)
    Me.MessageEmptySpaceItem.Name = "MessageEmptySpaceItem"
    Me.MessageEmptySpaceItem.Size = New System.Drawing.Size(25, 120)
    Me.MessageEmptySpaceItem.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.MessageEmptySpaceItem.TextSize = New System.Drawing.Size(0, 0)
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.RequestTypesRadioGroup
    resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
    Me.LayoutControlItem1.Location = New System.Drawing.Point(25, 31)
    Me.LayoutControlItem1.MinSize = New System.Drawing.Size(170, 46)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(726, 120)
    Me.LayoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(74, 20)
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.btnBack
    resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
    Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 151)
    Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(0, 37)
    Me.LayoutControlItem2.MinSize = New System.Drawing.Size(55, 37)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(375, 37)
    Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem2.TextToControlDistance = 0
    Me.LayoutControlItem2.TextVisible = False
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.btnNext
    resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
    Me.LayoutControlItem3.Location = New System.Drawing.Point(375, 151)
    Me.LayoutControlItem3.MaxSize = New System.Drawing.Size(0, 37)
    Me.LayoutControlItem3.MinSize = New System.Drawing.Size(76, 37)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(376, 37)
    Me.LayoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem3.TextToControlDistance = 0
    Me.LayoutControlItem3.TextVisible = False
    '
    'RequestTypeListBindingSource
    '
    Me.RequestTypeListBindingSource.DataSource = GetType(VTE.Library.RequestTypeList)
    '
    'uxRequestTypeChooser
    '
    Me.AccessibleDescription = Nothing
    Me.AccessibleName = Nothing
    resources.ApplyResources(Me, "$this")
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.BackgroundImage = Nothing
    Me.Controls.Add(Me.RequestChooserLayoutControl)
    Me.Name = "uxRequestTypeChooser"
    CType(Me.RequestChooserLayoutControl, System.ComponentModel.ISupportInitialize).EndInit()
    Me.RequestChooserLayoutControl.ResumeLayout(False)
    CType(Me.RequestTypesRadioGroup.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CaptionEmptySpaceItem, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.MessageEmptySpaceItem, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RequestTypeListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents RequestChooserLayoutControl As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents RequestTypeListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents CaptionEmptySpaceItem As DevExpress.XtraLayout.EmptySpaceItem
  Friend WithEvents RequestTypesRadioGroup As DevExpress.XtraEditors.RadioGroup
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents btnNext As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnBack As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents MessageEmptySpaceItem As DevExpress.XtraLayout.EmptySpaceItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem

End Class
