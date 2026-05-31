<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxDocTehnicalExamReportTouch
    Inherits VTE.BaseParts.uxWinPart

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
    Me.components = New System.ComponentModel.Container
    Me.PanelParts = New DevExpress.XtraEditors.PanelControl
    Me.PanelCategory = New DevExpress.XtraEditors.PanelControl
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.btnCancel = New DevExpress.XtraEditors.SimpleButton
    Me.btnOK = New DevExpress.XtraEditors.SimpleButton
    Me.LookUpEditCustomer = New DevExpress.XtraEditors.LookUpEdit
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItemParts = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItemCategory = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.VehicleListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.CustomersListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.LookUpEditVehicle = New DevExpress.XtraEditors.LookUpEdit
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    CType(Me.PanelParts, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PanelCategory, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.LookUpEditCustomer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItemParts, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItemCategory, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.VehicleListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CustomersListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LookUpEditVehicle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'PanelParts
    '
    Me.PanelParts.Location = New System.Drawing.Point(8, 70)
    Me.PanelParts.Name = "PanelParts"
    Me.PanelParts.Size = New System.Drawing.Size(630, 286)
    Me.PanelParts.TabIndex = 1
    '
    'PanelCategory
    '
    Me.PanelCategory.Location = New System.Drawing.Point(8, 39)
    Me.PanelCategory.Name = "PanelCategory"
    Me.PanelCategory.Size = New System.Drawing.Size(630, 20)
    Me.PanelCategory.TabIndex = 2
    '
    'LayoutControl1
    '
    Me.LayoutControl1.Controls.Add(Me.LookUpEditVehicle)
    Me.LayoutControl1.Controls.Add(Me.btnCancel)
    Me.LayoutControl1.Controls.Add(Me.btnOK)
    Me.LayoutControl1.Controls.Add(Me.LookUpEditCustomer)
    Me.LayoutControl1.Controls.Add(Me.PanelParts)
    Me.LayoutControl1.Controls.Add(Me.PanelCategory)
    Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    Me.LayoutControl1.Size = New System.Drawing.Size(645, 463)
    Me.LayoutControl1.TabIndex = 3
    Me.LayoutControl1.Text = "LayoutControl1"
    '
    'btnCancel
    '
    Me.btnCancel.Appearance.Font = New System.Drawing.Font("Tahoma", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.btnCancel.Appearance.Options.UseFont = True
    Me.btnCancel.Location = New System.Drawing.Point(328, 367)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(310, 89)
    Me.btnCancel.StyleController = Me.LayoutControl1
    Me.btnCancel.TabIndex = 8
    Me.btnCancel.Text = "CANCEL"
    '
    'btnOK
    '
    Me.btnOK.Appearance.Font = New System.Drawing.Font("Tahoma", 30.0!)
    Me.btnOK.Appearance.Options.UseFont = True
    Me.btnOK.Location = New System.Drawing.Point(8, 367)
    Me.btnOK.Name = "btnOK"
    Me.btnOK.Size = New System.Drawing.Size(309, 89)
    Me.btnOK.StyleController = Me.LayoutControl1
    Me.btnOK.TabIndex = 7
    Me.btnOK.Text = "OK"
    '
    'LookUpEditCustomer
    '
    Me.LookUpEditCustomer.Location = New System.Drawing.Point(384, 8)
    Me.LookUpEditCustomer.Name = "LookUpEditCustomer"
    Me.LookUpEditCustomer.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.LookUpEditCustomer.Properties.DataSource = Me.CustomersListBindingSource
    Me.LookUpEditCustomer.Properties.DisplayMember = "Name"
    Me.LookUpEditCustomer.Properties.ReadOnly = True
    Me.LookUpEditCustomer.Properties.ValueMember = "Id"
    Me.LookUpEditCustomer.Size = New System.Drawing.Size(254, 20)
    Me.LookUpEditCustomer.StyleController = Me.LayoutControl1
    Me.LookUpEditCustomer.TabIndex = 6
    '
    'LayoutControlGroup1
    '
    Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItemParts, Me.LayoutControlItemCategory, Me.LayoutControlItem5, Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(645, 463)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.Text = "LayoutControlGroup1"
    Me.LayoutControlGroup1.TextVisible = False
    '
    'LayoutControlItemParts
    '
    Me.LayoutControlItemParts.Control = Me.PanelParts
    Me.LayoutControlItemParts.CustomizationFormText = "LayoutControlItemParts"
    Me.LayoutControlItemParts.Location = New System.Drawing.Point(0, 62)
    Me.LayoutControlItemParts.Name = "LayoutControlItemParts"
    Me.LayoutControlItemParts.Size = New System.Drawing.Size(641, 297)
    Me.LayoutControlItemParts.Text = "LayoutControlItemParts"
    Me.LayoutControlItemParts.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItemParts.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItemParts.TextToControlDistance = 0
    Me.LayoutControlItemParts.TextVisible = False
    '
    'LayoutControlItemCategory
    '
    Me.LayoutControlItemCategory.Control = Me.PanelCategory
    Me.LayoutControlItemCategory.CustomizationFormText = "LayoutControlItemCategory"
    Me.LayoutControlItemCategory.Location = New System.Drawing.Point(0, 31)
    Me.LayoutControlItemCategory.Name = "LayoutControlItemCategory"
    Me.LayoutControlItemCategory.Size = New System.Drawing.Size(641, 31)
    Me.LayoutControlItemCategory.Text = "LayoutControlItemCategory"
    Me.LayoutControlItemCategory.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItemCategory.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItemCategory.TextToControlDistance = 0
    Me.LayoutControlItemCategory.TextVisible = False
    '
    'LayoutControlItem5
    '
    Me.LayoutControlItem5.Control = Me.LookUpEditCustomer
    Me.LayoutControlItem5.CustomizationFormText = "LayoutControlItem5"
    Me.LayoutControlItem5.Location = New System.Drawing.Point(325, 0)
    Me.LayoutControlItem5.MaxSize = New System.Drawing.Size(0, 31)
    Me.LayoutControlItem5.MinSize = New System.Drawing.Size(162, 31)
    Me.LayoutControlItem5.Name = "LayoutControlItem5"
    Me.LayoutControlItem5.Size = New System.Drawing.Size(316, 31)
    Me.LayoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem5.Text = "Customer"
    Me.LayoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem5.TextSize = New System.Drawing.Size(46, 20)
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.btnOK
    Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 359)
    Me.LayoutControlItem1.MaxSize = New System.Drawing.Size(0, 100)
    Me.LayoutControlItem1.MinSize = New System.Drawing.Size(92, 100)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(320, 100)
    Me.LayoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem1.Text = "LayoutControlItem1"
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem1.TextToControlDistance = 0
    Me.LayoutControlItem1.TextVisible = False
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.btnCancel
    Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
    Me.LayoutControlItem2.Location = New System.Drawing.Point(320, 359)
    Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(0, 100)
    Me.LayoutControlItem2.MinSize = New System.Drawing.Size(92, 100)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(321, 100)
    Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem2.Text = "LayoutControlItem2"
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem2.TextToControlDistance = 0
    Me.LayoutControlItem2.TextVisible = False
    '
    'VehicleListBindingSource
    '
    Me.VehicleListBindingSource.DataSource = GetType(VTE.Library.VehicleList)
    '
    'CustomersListBindingSource
    '
    Me.CustomersListBindingSource.DataSource = GetType(VTE.Library.CustomersList)
    '
    'LookUpEditVehicle
    '
    Me.LookUpEditVehicle.Location = New System.Drawing.Point(46, 8)
    Me.LookUpEditVehicle.Name = "LookUpEditVehicle"
    Me.LookUpEditVehicle.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.LookUpEditVehicle.Properties.DataSource = Me.VehicleListBindingSource
    Me.LookUpEditVehicle.Properties.DisplayMember = "LastRegistration"
    Me.LookUpEditVehicle.Properties.ReadOnly = True
    Me.LookUpEditVehicle.Properties.ValueMember = "Id"
    Me.LookUpEditVehicle.Size = New System.Drawing.Size(276, 20)
    Me.LookUpEditVehicle.StyleController = Me.LayoutControl1
    Me.LookUpEditVehicle.TabIndex = 9
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.LookUpEditVehicle
    Me.LayoutControlItem3.CustomizationFormText = "Vehicle"
    Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(325, 31)
    Me.LayoutControlItem3.Text = "Vehicle"
    Me.LayoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(33, 20)
    '
    'uxDocTehnicalExamReportTouch
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "uxDocTehnicalExamReportTouch"
    Me.Size = New System.Drawing.Size(645, 482)
    Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
    CType(Me.PanelParts, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PanelCategory, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.LookUpEditCustomer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItemParts, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItemCategory, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.VehicleListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CustomersListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LookUpEditVehicle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents PanelParts As DevExpress.XtraEditors.PanelControl
  Friend WithEvents PanelCategory As DevExpress.XtraEditors.PanelControl
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItemParts As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItemCategory As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LookUpEditCustomer As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents CustomersListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents VehicleListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents LookUpEditVehicle As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem

End Class
