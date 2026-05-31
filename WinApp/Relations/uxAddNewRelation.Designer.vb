<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxAddNewRelation
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxAddNewRelation))
  Me.AddNewRelationLayoutControl = New DevExpress.XtraLayout.LayoutControl
  Me.btnCancel = New DevExpress.XtraEditors.SimpleButton
  Me.btnOk = New DevExpress.XtraEditors.SimpleButton
  Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
  Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
  Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
  Me.VehicleCustomLookUpEdit = New VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
  Me.CustomerVehiclesRelationBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.CustomerCustomLookUpEdit = New VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
  Me.CustomersSearchListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.RelationTypeLookUpEdit = New VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
  Me.CustomerVehicleRelationTypeListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
  Me.VehicleListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  CType(Me.AddNewRelationLayoutControl, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.AddNewRelationLayoutControl.SuspendLayout()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.VehicleCustomLookUpEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CustomerVehiclesRelationBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CustomerCustomLookUpEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CustomersSearchListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.RelationTypeLookUpEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CustomerVehicleRelationTypeListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.VehicleListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'AddNewRelationLayoutControl
  '
  Me.AddNewRelationLayoutControl.AccessibleDescription = Nothing
  Me.AddNewRelationLayoutControl.AccessibleName = Nothing
  resources.ApplyResources(Me.AddNewRelationLayoutControl, "AddNewRelationLayoutControl")
  Me.AddNewRelationLayoutControl.Appearance.DisabledLayoutGroupCaption.ForeColor = System.Drawing.SystemColors.GrayText
  Me.AddNewRelationLayoutControl.Appearance.DisabledLayoutGroupCaption.Options.UseForeColor = True
  Me.AddNewRelationLayoutControl.Appearance.DisabledLayoutItem.ForeColor = System.Drawing.SystemColors.GrayText
  Me.AddNewRelationLayoutControl.Appearance.DisabledLayoutItem.Options.UseForeColor = True
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.AddNewRelationLayoutControl, False)
  Me.AddNewRelationLayoutControl.BackgroundImage = Nothing
  Me.AddNewRelationLayoutControl.Controls.Add(Me.btnCancel)
  Me.AddNewRelationLayoutControl.Controls.Add(Me.btnOk)
  Me.AddNewRelationLayoutControl.Controls.Add(Me.VehicleCustomLookUpEdit)
  Me.AddNewRelationLayoutControl.Controls.Add(Me.CustomerCustomLookUpEdit)
  Me.AddNewRelationLayoutControl.Controls.Add(Me.RelationTypeLookUpEdit)
  Me.AddNewRelationLayoutControl.Font = Nothing
  Me.AddNewRelationLayoutControl.Name = "AddNewRelationLayoutControl"
  Me.AddNewRelationLayoutControl.OptionsItemText.TextAlignMode = DevExpress.XtraLayout.TextAlignMode.AutoSize
  Me.AddNewRelationLayoutControl.Root = Me.LayoutControlGroup1
  '
  'btnCancel
  '
  Me.btnCancel.AccessibleDescription = Nothing
  Me.btnCancel.AccessibleName = Nothing
  resources.ApplyResources(Me.btnCancel, "btnCancel")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.btnCancel, False)
  Me.btnCancel.BackgroundImage = Nothing
  Me.btnCancel.Name = "btnCancel"
  Me.btnCancel.StyleController = Me.AddNewRelationLayoutControl
  '
  'btnOk
  '
  Me.btnOk.AccessibleDescription = Nothing
  Me.btnOk.AccessibleName = Nothing
  resources.ApplyResources(Me.btnOk, "btnOk")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.btnOk, False)
  Me.btnOk.BackgroundImage = Nothing
  Me.btnOk.Name = "btnOk"
  Me.btnOk.StyleController = Me.AddNewRelationLayoutControl
  '
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem3, Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem5})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(658, 115)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.CustomerVehiclesRelationBindingSource
  '
  'VehicleCustomLookUpEdit
  '
  resources.ApplyResources(Me.VehicleCustomLookUpEdit, "VehicleCustomLookUpEdit")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.VehicleCustomLookUpEdit, False)
  Me.VehicleCustomLookUpEdit.BackgroundImage = Nothing
  Me.VehicleCustomLookUpEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.CustomerVehiclesRelationBindingSource, "IdVehicle", True))
  Me.VehicleCustomLookUpEdit.EditValue = Nothing
  Me.VehicleCustomLookUpEdit.Name = "VehicleCustomLookUpEdit"
  Me.VehicleCustomLookUpEdit.Properties.AccessibleDescription = Nothing
  Me.VehicleCustomLookUpEdit.Properties.AccessibleName = Nothing
  Me.VehicleCustomLookUpEdit.Properties.AutoHeight = CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.AutoHeight"), Boolean)
  Me.VehicleCustomLookUpEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("VehicleCustomLookUpEdit.Properties.Buttons2"), CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons3"), Integer), CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons4"), Boolean), CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons5"), Boolean), CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons6"), Boolean), CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons8"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("VehicleCustomLookUpEdit.Properties.Buttons9"), CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons10"), Integer), CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons11"), Boolean), CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons12"), Boolean), CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons13"), Boolean), CType(resources.GetObject("VehicleCustomLookUpEdit.Properties.Buttons14"), DevExpress.XtraEditors.ImageLocation), Nothing)})
  Me.VehicleCustomLookUpEdit.Properties.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ShellNumber", "ShellNumber", 65, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ModelName", "ModelName", 61, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("VehiceMaker", "VehiceMaker", 66, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("LastRegistratinNumber", "LastRegistratinNumber", 115, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("LastRegAndShellNumber", "Возило", 123, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near)})
  Me.VehicleCustomLookUpEdit.Properties.DataSource = Me.VehicleListBindingSource
  Me.VehicleCustomLookUpEdit.Properties.DisplayMember = "LastRegAndShellNumber"
  Me.VehicleCustomLookUpEdit.Properties.NullText = Global.WinApp.My.Resources.Resources.String1
  Me.VehicleCustomLookUpEdit.Properties.PopupWidth = 500
  Me.VehicleCustomLookUpEdit.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup
  Me.VehicleCustomLookUpEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
  Me.VehicleCustomLookUpEdit.Properties.ValueMember = "Id"
  Me.VehicleCustomLookUpEdit.StyleController = Me.AddNewRelationLayoutControl
  '
  'CustomerVehiclesRelationBindingSource
  '
  Me.CustomerVehiclesRelationBindingSource.DataSource = GetType(VTE.Library.CustomerVehiclesRelation)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CustomerVehiclesRelationBindingSource, False)
  '
  'CustomerCustomLookUpEdit
  '
  resources.ApplyResources(Me.CustomerCustomLookUpEdit, "CustomerCustomLookUpEdit")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.CustomerCustomLookUpEdit, False)
  Me.CustomerCustomLookUpEdit.BackgroundImage = Nothing
  Me.CustomerCustomLookUpEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.CustomerVehiclesRelationBindingSource, "IdCustomer", True))
  Me.CustomerCustomLookUpEdit.EditValue = Nothing
  Me.CustomerCustomLookUpEdit.Name = "CustomerCustomLookUpEdit"
  Me.CustomerCustomLookUpEdit.Properties.AccessibleDescription = Nothing
  Me.CustomerCustomLookUpEdit.Properties.AccessibleName = Nothing
  Me.CustomerCustomLookUpEdit.Properties.AutoHeight = CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.AutoHeight"), Boolean)
  Me.CustomerCustomLookUpEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("CustomerCustomLookUpEdit.Properties.Buttons2"), CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons3"), Integer), CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons4"), Boolean), CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons5"), Boolean), CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons6"), Boolean), CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons8"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("CustomerCustomLookUpEdit.Properties.Buttons9"), CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons10"), Integer), CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons11"), Boolean), CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons12"), Boolean), CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons13"), Boolean), CType(resources.GetObject("CustomerCustomLookUpEdit.Properties.Buttons14"), DevExpress.XtraEditors.ImageLocation), Nothing)})
  Me.CustomerCustomLookUpEdit.Properties.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("MB", "MB", 20, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CustomerSurname", "CustomerSurname", 94, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
  Me.CustomerCustomLookUpEdit.Properties.DataSource = Me.CustomersSearchListBindingSource
  Me.CustomerCustomLookUpEdit.Properties.DisplayMember = "MB"
  Me.CustomerCustomLookUpEdit.Properties.NullText = Global.WinApp.My.Resources.Resources.String1
  Me.CustomerCustomLookUpEdit.Properties.PopupWidth = 450
  Me.CustomerCustomLookUpEdit.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup
  Me.CustomerCustomLookUpEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
  Me.CustomerCustomLookUpEdit.Properties.ValueMember = "Id"
  Me.CustomerCustomLookUpEdit.StyleController = Me.AddNewRelationLayoutControl
  '
  'CustomersSearchListBindingSource
  '
  Me.CustomersSearchListBindingSource.DataSource = GetType(VTE.Library.CustomersSearchList)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CustomersSearchListBindingSource, False)
  '
  'RelationTypeLookUpEdit
  '
  resources.ApplyResources(Me.RelationTypeLookUpEdit, "RelationTypeLookUpEdit")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.RelationTypeLookUpEdit, False)
  Me.RelationTypeLookUpEdit.BackgroundImage = Nothing
  Me.RelationTypeLookUpEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.CustomerVehiclesRelationBindingSource, "IdRelationType", True))
  Me.RelationTypeLookUpEdit.EditValue = Nothing
  Me.RelationTypeLookUpEdit.Name = "RelationTypeLookUpEdit"
  Me.RelationTypeLookUpEdit.Properties.AccessibleDescription = Nothing
  Me.RelationTypeLookUpEdit.Properties.AccessibleName = Nothing
  Me.RelationTypeLookUpEdit.Properties.AutoHeight = CType(resources.GetObject("RelationTypeLookUpEdit.Properties.AutoHeight"), Boolean)
  Me.RelationTypeLookUpEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RelationTypeLookUpEdit.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
  Me.RelationTypeLookUpEdit.Properties.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("RelationTypeName", "Ралација", 100, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsCustomerOnly", "Само комитент", 50, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsOwner", "Е сопстваник", 50, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsAuthorized", "Е авторизиран", 50, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("RelationDescription", "Опис", 98, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
  Me.RelationTypeLookUpEdit.Properties.DataSource = Me.CustomerVehicleRelationTypeListBindingSource
  Me.RelationTypeLookUpEdit.Properties.DisplayMember = "RelationTypeName"
  Me.RelationTypeLookUpEdit.Properties.PopupWidth = 250
  Me.RelationTypeLookUpEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
  Me.RelationTypeLookUpEdit.Properties.ValueMember = "Id"
  Me.RelationTypeLookUpEdit.StyleController = Me.AddNewRelationLayoutControl
  '
  'CustomerVehicleRelationTypeListBindingSource
  '
  Me.CustomerVehicleRelationTypeListBindingSource.DataSource = GetType(VTE.Library.CustomerVehicleRelationTypeList)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CustomerVehicleRelationTypeListBindingSource, False)
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.VehicleCustomLookUpEdit
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(327, 31)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(235, 20)
  '
  'LayoutControlItem3
  '
  Me.LayoutControlItem3.Control = Me.RelationTypeLookUpEdit
  resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
  Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 31)
  Me.LayoutControlItem3.Name = "LayoutControlItem3"
  Me.LayoutControlItem3.Size = New System.Drawing.Size(654, 31)
  Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem3.TextSize = New System.Drawing.Size(82, 20)
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.CustomerCustomLookUpEdit
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(327, 0)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(327, 31)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(233, 20)
  '
  'LayoutControlItem4
  '
  Me.LayoutControlItem4.Control = Me.btnOk
  resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
  Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 62)
  Me.LayoutControlItem4.MinSize = New System.Drawing.Size(92, 33)
  Me.LayoutControlItem4.Name = "LayoutControlItem4"
  Me.LayoutControlItem4.Size = New System.Drawing.Size(327, 49)
  Me.LayoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
  Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem4.TextToControlDistance = 0
  Me.LayoutControlItem4.TextVisible = False
  '
  'LayoutControlItem5
  '
  Me.LayoutControlItem5.Control = Me.btnCancel
  resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
  Me.LayoutControlItem5.Location = New System.Drawing.Point(327, 62)
  Me.LayoutControlItem5.MinSize = New System.Drawing.Size(92, 33)
  Me.LayoutControlItem5.Name = "LayoutControlItem5"
  Me.LayoutControlItem5.Size = New System.Drawing.Size(327, 49)
  Me.LayoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
  Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem5.TextToControlDistance = 0
  Me.LayoutControlItem5.TextVisible = False
  '
  'VehicleListBindingSource
  '
  Me.VehicleListBindingSource.DataSource = GetType(VTE.Library.VehicleListShort)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.VehicleListBindingSource, False)
  '
  'uxAddNewRelation
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
  resources.ApplyResources(Me, "$this")
  Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.AddNewRelationLayoutControl)
  Me.Name = "uxAddNewRelation"
  CType(Me.AddNewRelationLayoutControl, System.ComponentModel.ISupportInitialize).EndInit()
  Me.AddNewRelationLayoutControl.ResumeLayout(False)
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.VehicleCustomLookUpEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CustomerVehiclesRelationBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CustomerCustomLookUpEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CustomersSearchListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.RelationTypeLookUpEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CustomerVehicleRelationTypeListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.VehicleListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents AddNewRelationLayoutControl As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnOk As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents VehicleCustomLookUpEdit As VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
  Friend WithEvents CustomerCustomLookUpEdit As VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
  Friend WithEvents RelationTypeLookUpEdit As VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents VehicleListBindingSource As System.Windows.Forms.BindingSource
 Friend WithEvents CustomerVehicleRelationTypeListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents CustomerVehiclesRelationBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
 Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
 Friend WithEvents CustomersSearchListBindingSource As System.Windows.Forms.BindingSource

End Class
