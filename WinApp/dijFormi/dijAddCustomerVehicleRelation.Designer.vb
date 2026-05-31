<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijAddCustomerVehicleRelation
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
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.VehicleLookUpEdit = New VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.CustomLookUpEdit2 = New VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem
    Me.CustomLookUpEdit3 = New VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.VehicleLookUpEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CustomLookUpEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CustomLookUpEdit3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'LayoutControl1
    '
    Me.LayoutControl1.Controls.Add(Me.CustomLookUpEdit3)
    Me.LayoutControl1.Controls.Add(Me.CustomLookUpEdit2)
    Me.LayoutControl1.Controls.Add(Me.VehicleLookUpEdit)
    Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.OptionsItemText.TextAlignMode = DevExpress.XtraLayout.TextAlignMode.AutoSize
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    Me.LayoutControl1.Size = New System.Drawing.Size(748, 127)
    Me.LayoutControl1.TabIndex = 0
    Me.LayoutControl1.Text = "LayoutControl1"
    '
    'LayoutControlGroup1
    '
    Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.EmptySpaceItem1, Me.LayoutControlItem3})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(748, 127)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.Text = "LayoutControlGroup1"
    Me.LayoutControlGroup1.TextVisible = False
    '
    'VehicleLookUpEdit
    '
    Me.VehicleLookUpEdit.Location = New System.Drawing.Point(108, 7)
    Me.VehicleLookUpEdit.Name = "VehicleLookUpEdit"
    Me.VehicleLookUpEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.VehicleLookUpEdit.Properties.PopupWidth = 250
    Me.VehicleLookUpEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
    Me.VehicleLookUpEdit.Size = New System.Drawing.Size(261, 20)
    Me.VehicleLookUpEdit.StyleController = Me.LayoutControl1
    Me.VehicleLookUpEdit.TabIndex = 4
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.VehicleLookUpEdit
    Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(373, 31)
    Me.LayoutControlItem1.Text = "LayoutControlItem1"
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(96, 20)
    '
    'CustomLookUpEdit2
    '
    Me.CustomLookUpEdit2.Location = New System.Drawing.Point(481, 7)
    Me.CustomLookUpEdit2.Name = "CustomLookUpEdit2"
    Me.CustomLookUpEdit2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.CustomLookUpEdit2.Properties.PopupWidth = 250
    Me.CustomLookUpEdit2.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
    Me.CustomLookUpEdit2.Size = New System.Drawing.Size(261, 20)
    Me.CustomLookUpEdit2.StyleController = Me.LayoutControl1
    Me.CustomLookUpEdit2.TabIndex = 5
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.CustomLookUpEdit2
    Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
    Me.LayoutControlItem2.Location = New System.Drawing.Point(373, 0)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(373, 31)
    Me.LayoutControlItem2.Text = "LayoutControlItem2"
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(96, 20)
    '
    'EmptySpaceItem1
    '
    Me.EmptySpaceItem1.CustomizationFormText = "EmptySpaceItem1"
    Me.EmptySpaceItem1.Location = New System.Drawing.Point(0, 62)
    Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
    Me.EmptySpaceItem1.Size = New System.Drawing.Size(746, 63)
    Me.EmptySpaceItem1.Text = "EmptySpaceItem1"
    Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
    '
    'CustomLookUpEdit3
    '
    Me.CustomLookUpEdit3.Location = New System.Drawing.Point(108, 38)
    Me.CustomLookUpEdit3.Name = "CustomLookUpEdit3"
    Me.CustomLookUpEdit3.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.CustomLookUpEdit3.Properties.PopupWidth = 250
    Me.CustomLookUpEdit3.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
    Me.CustomLookUpEdit3.Size = New System.Drawing.Size(634, 20)
    Me.CustomLookUpEdit3.StyleController = Me.LayoutControl1
    Me.CustomLookUpEdit3.TabIndex = 6
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.CustomLookUpEdit3
    Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
    Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 31)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(746, 31)
    Me.LayoutControlItem3.Text = "LayoutControlItem3"
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(96, 20)
    '
    'dijAddCustomerVehicleRelation
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(748, 127)
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "dijAddCustomerVehicleRelation"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Додадете релација меѓу комитент и возило"
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.VehicleLookUpEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CustomLookUpEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CustomLookUpEdit3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents CustomLookUpEdit3 As VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
  Friend WithEvents CustomLookUpEdit2 As VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
  Friend WithEvents VehicleLookUpEdit As VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
End Class
