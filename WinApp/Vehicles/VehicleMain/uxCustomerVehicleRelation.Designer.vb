<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxCustomerVehicleRelation
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxCustomerVehicleRelation))
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
  Me.CustomerVehiclesRelationsListGridControl = New DevExpress.XtraGrid.GridControl
  Me.CustomerVehiclesRelationsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIdRelationType = New DevExpress.XtraGrid.Columns.GridColumn
  Me.LookUpEditRelationType = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Me.CustomerVehicleRelationTypeListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.colIdCustomer = New DevExpress.XtraGrid.Columns.GridColumn
  Me.LookUpEditCustomers = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Me.CustomersListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.colIdVehicle = New DevExpress.XtraGrid.Columns.GridColumn
  Me.LookUpEditVehicles = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Me.VehicleListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.colStartDate = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colEndDate = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colBeginNote = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colTerminationNote = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCustomerFirstName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCustomerSurname = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colMB = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colShellNumber = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colLastRegistration = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCustomerName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
  Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
  Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
  Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.LayoutControl1.SuspendLayout()
  CType(Me.CustomerVehiclesRelationsListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CustomerVehiclesRelationsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LookUpEditRelationType, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CustomerVehicleRelationTypeListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LookUpEditCustomers, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CustomersListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LookUpEditVehicles, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.VehicleListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'LayoutControl1
  '
  Me.LayoutControl1.AccessibleDescription = Nothing
  Me.LayoutControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
  Me.LayoutControl1.BackgroundImage = Nothing
  Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
  Me.LayoutControl1.Controls.Add(Me.CustomerVehiclesRelationsListGridControl)
  Me.LayoutControl1.Font = Nothing
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
  '
  'UxKopcinja1
  '
  Me.UxKopcinja1.AccessibleDescription = Nothing
  Me.UxKopcinja1.AccessibleName = Nothing
  resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
  Me.UxKopcinja1.BackgroundImage = Nothing
  Me.UxKopcinja1.Name = "UxKopcinja1"
  '
  'CustomerVehiclesRelationsListGridControl
  '
  Me.CustomerVehiclesRelationsListGridControl.AccessibleDescription = Nothing
  Me.CustomerVehiclesRelationsListGridControl.AccessibleName = Nothing
  resources.ApplyResources(Me.CustomerVehiclesRelationsListGridControl, "CustomerVehiclesRelationsListGridControl")
  Me.CustomerVehiclesRelationsListGridControl.BackgroundImage = Nothing
  Me.CustomerVehiclesRelationsListGridControl.DataSource = Me.CustomerVehiclesRelationsBindingSource
  Me.CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
  Me.CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.AccessibleName = Nothing
  Me.CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
  Me.CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.BackgroundImage = Nothing
  Me.CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
  Me.CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
  Me.CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
  Me.CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.ToolTip = resources.GetString("CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.ToolTip")
  Me.CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
  Me.CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("CustomerVehiclesRelationsListGridControl.EmbeddedNavigator.ToolTipTitle")
  Me.CustomerVehiclesRelationsListGridControl.Font = Nothing
  Me.CustomerVehiclesRelationsListGridControl.MainView = Me.GridView1
  Me.CustomerVehiclesRelationsListGridControl.Name = "CustomerVehiclesRelationsListGridControl"
  Me.CustomerVehiclesRelationsListGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.LookUpEditRelationType, Me.LookUpEditCustomers, Me.LookUpEditVehicles})
  Me.CustomerVehiclesRelationsListGridControl.UseEmbeddedNavigator = True
  Me.CustomerVehiclesRelationsListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'CustomerVehiclesRelationsBindingSource
  '
  Me.CustomerVehiclesRelationsBindingSource.DataSource = GetType(VTE.Library.CustomerVehiclesRelationsList)
  '
  'GridView1
  '
  Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
  Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  Me.GridView1.Appearance.Row.Options.UseTextOptions = True
  Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  resources.ApplyResources(Me.GridView1, "GridView1")
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdRelationType, Me.colIdCustomer, Me.colIdVehicle, Me.colStartDate, Me.colEndDate, Me.colBeginNote, Me.colTerminationNote, Me.colCustomerFirstName, Me.colCustomerSurname, Me.colMB, Me.colShellNumber, Me.colLastRegistration, Me.colCustomerName})
  Me.GridView1.GridControl = Me.CustomerVehiclesRelationsListGridControl
  Me.GridView1.Name = "GridView1"
  Me.GridView1.OptionsNavigation.AutoFocusNewRow = True
  Me.GridView1.OptionsNavigation.EnterMoveNextColumn = True
  Me.GridView1.OptionsView.ShowAutoFilterRow = True
  Me.GridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways
  Me.GridView1.OptionsView.ShowFooter = True
  '
  'colId
  '
  resources.ApplyResources(Me.colId, "colId")
  Me.colId.FieldName = "Id"
  Me.colId.Name = "colId"
  Me.colId.OptionsColumn.ReadOnly = True
  '
  'colIdRelationType
  '
  resources.ApplyResources(Me.colIdRelationType, "colIdRelationType")
  Me.colIdRelationType.ColumnEdit = Me.LookUpEditRelationType
  Me.colIdRelationType.FieldName = "IdRelationType"
  Me.colIdRelationType.Name = "colIdRelationType"
  '
  'LookUpEditRelationType
  '
  Me.LookUpEditRelationType.AccessibleDescription = Nothing
  Me.LookUpEditRelationType.AccessibleName = Nothing
  resources.ApplyResources(Me.LookUpEditRelationType, "LookUpEditRelationType")
  Me.LookUpEditRelationType.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditRelationType.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditRelationType.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditRelationType.Buttons2"), CType(resources.GetObject("LookUpEditRelationType.Buttons3"), Integer), CType(resources.GetObject("LookUpEditRelationType.Buttons4"), Boolean), CType(resources.GetObject("LookUpEditRelationType.Buttons5"), Boolean), CType(resources.GetObject("LookUpEditRelationType.Buttons6"), Boolean), CType(resources.GetObject("LookUpEditRelationType.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing)})
  Me.LookUpEditRelationType.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("RelationTypeName", "RelationTypeName", 96, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsCustomerOnly", "IsCustomerOnly", 83, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsOwner", "IsOwner", 47, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsAuthorized", "IsAuthorized", 67, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("RelationDescription", "RelationDescription", 98, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
  Me.LookUpEditRelationType.DataSource = Me.CustomerVehicleRelationTypeListBindingSource
  Me.LookUpEditRelationType.DisplayMember = "RelationTypeName"
  Me.LookUpEditRelationType.Name = "LookUpEditRelationType"
  Me.LookUpEditRelationType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
  Me.LookUpEditRelationType.ValueMember = "Id"
  '
  'CustomerVehicleRelationTypeListBindingSource
  '
  Me.CustomerVehicleRelationTypeListBindingSource.DataSource = GetType(VTE.Library.CustomerVehicleRelationTypeList)
  '
  'colIdCustomer
  '
  resources.ApplyResources(Me.colIdCustomer, "colIdCustomer")
  Me.colIdCustomer.ColumnEdit = Me.LookUpEditCustomers
  Me.colIdCustomer.FieldName = "IdCustomer"
  Me.colIdCustomer.Name = "colIdCustomer"
  '
  'LookUpEditCustomers
  '
  Me.LookUpEditCustomers.AccessibleDescription = Nothing
  Me.LookUpEditCustomers.AccessibleName = Nothing
  resources.ApplyResources(Me.LookUpEditCustomers, "LookUpEditCustomers")
  Me.LookUpEditCustomers.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCustomers.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCustomers.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditCustomers.Buttons2"), CType(resources.GetObject("LookUpEditCustomers.Buttons3"), Integer), CType(resources.GetObject("LookUpEditCustomers.Buttons4"), Boolean), CType(resources.GetObject("LookUpEditCustomers.Buttons5"), Boolean), CType(resources.GetObject("LookUpEditCustomers.Buttons6"), Boolean), CType(resources.GetObject("LookUpEditCustomers.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCustomers.Buttons8"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditCustomers.Buttons9"), CType(resources.GetObject("LookUpEditCustomers.Buttons10"), Integer), CType(resources.GetObject("LookUpEditCustomers.Buttons11"), Boolean), CType(resources.GetObject("LookUpEditCustomers.Buttons12"), Boolean), CType(resources.GetObject("LookUpEditCustomers.Buttons13"), Boolean), CType(resources.GetObject("LookUpEditCustomers.Buttons14"), DevExpress.XtraEditors.ImageLocation), Nothing)})
  Me.LookUpEditCustomers.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("MB", "MB", 20, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CustomerSurname", "CustomerSurname", 94, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CustomerFirstName", "CustomerFirstName", 100, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("PhoneNumber", "PhoneNumber", 73, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Fax", "Fax", 24, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("LivingAddressNumber", "LivingAddressNumber", 109, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdLivingAddress", "IdLivingAddress", 82, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdLivingCity", "IdLivingCity", 62, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdBirhCity", "IdBirhCity", 53, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdBirthAddress", "IdBirthAddress", 77, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("BrithAddressNumber", "BrithAddressNumber", 104, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Citizenship", "Citizenship", 57, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsCompany", "IsCompany", 60, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Occupation", "Occupation", 60, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("WorksInCompany", "WorksInCompany", 91, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdBusinessType", "IdBusinessType", 81, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("BirthDate", "BirthDate", 51, DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy", False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CityName", "CityName", 52, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CountryName", "CountryName", 72, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("StreetName", "StreetName", 63, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CommunityName", "CommunityName", 86, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name", 33, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("AddressOfLiving", "AddressOfLiving", 84, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
  Me.LookUpEditCustomers.DataSource = Me.CustomersListBindingSource
  Me.LookUpEditCustomers.DisplayMember = "Name"
  Me.LookUpEditCustomers.Name = "LookUpEditCustomers"
  Me.LookUpEditCustomers.ValueMember = "Id"
  '
  'CustomersListBindingSource
  '
  Me.CustomersListBindingSource.DataSource = GetType(VTE.Library.CustomersList)
  '
  'colIdVehicle
  '
  resources.ApplyResources(Me.colIdVehicle, "colIdVehicle")
  Me.colIdVehicle.ColumnEdit = Me.LookUpEditVehicles
  Me.colIdVehicle.FieldName = "IdVehicle"
  Me.colIdVehicle.Name = "colIdVehicle"
  '
  'LookUpEditVehicles
  '
  Me.LookUpEditVehicles.AccessibleDescription = Nothing
  Me.LookUpEditVehicles.AccessibleName = Nothing
  resources.ApplyResources(Me.LookUpEditVehicles, "LookUpEditVehicles")
  Me.LookUpEditVehicles.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditVehicles.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditVehicles.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditVehicles.Buttons2"), CType(resources.GetObject("LookUpEditVehicles.Buttons3"), Integer), CType(resources.GetObject("LookUpEditVehicles.Buttons4"), Boolean), CType(resources.GetObject("LookUpEditVehicles.Buttons5"), Boolean), CType(resources.GetObject("LookUpEditVehicles.Buttons6"), Boolean), CType(resources.GetObject("LookUpEditVehicles.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditVehicles.Buttons8"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditVehicles.Buttons9"), CType(resources.GetObject("LookUpEditVehicles.Buttons10"), Integer), CType(resources.GetObject("LookUpEditVehicles.Buttons11"), Boolean), CType(resources.GetObject("LookUpEditVehicles.Buttons12"), Boolean), CType(resources.GetObject("LookUpEditVehicles.Buttons13"), Boolean), CType(resources.GetObject("LookUpEditVehicles.Buttons14"), DevExpress.XtraEditors.ImageLocation), Nothing)})
  Me.LookUpEditVehicles.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ShellNumber", "Шасија бр.", 65, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("LastRegistration", "LastRegistration", 84, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("LastRegistrationPlace", "LastRegistrationPlace", 109, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FirstRegistration", "FirstRegistration", 85, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FirstRegistrationDate", "FirstRegistrationDate", 108, DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy", False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FirstRegistrationPlace", "FirstRegistrationPlace", 110, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("DateOfLastRegistrationa", "DateOfLastRegistrationa", 125, DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy", False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("LastRegistrationValidTill", "LastRegistrationValidTill", 118, DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy", False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FirstRegistrationValidTill", "FirstRegistrationValidTill", 119, DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy", False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ModelName", "ModelName", 61, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("VehiceMaker", "VehiceMaker", 66, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdLastOwner", "IdLastOwner", 68, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdCurrentOwner", "IdCurrentOwner", 85, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("OwnerName", "OwnerName", 65, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("OwnerMB", "OwnerMB", 52, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
  Me.LookUpEditVehicles.DataSource = Me.VehicleListBindingSource
  Me.LookUpEditVehicles.DisplayMember = "ShellNumber"
  Me.LookUpEditVehicles.Name = "LookUpEditVehicles"
  Me.LookUpEditVehicles.ValueMember = "Id"
  '
  'VehicleListBindingSource
  '
  Me.VehicleListBindingSource.DataSource = GetType(VTE.Library.VehicleListShort)
  '
  'colStartDate
  '
  resources.ApplyResources(Me.colStartDate, "colStartDate")
  Me.colStartDate.FieldName = "StartDate"
  Me.colStartDate.Name = "colStartDate"
  '
  'colEndDate
  '
  resources.ApplyResources(Me.colEndDate, "colEndDate")
  Me.colEndDate.FieldName = "EndDate"
  Me.colEndDate.Name = "colEndDate"
  '
  'colBeginNote
  '
  resources.ApplyResources(Me.colBeginNote, "colBeginNote")
  Me.colBeginNote.FieldName = "BeginNote"
  Me.colBeginNote.Name = "colBeginNote"
  '
  'colTerminationNote
  '
  resources.ApplyResources(Me.colTerminationNote, "colTerminationNote")
  Me.colTerminationNote.FieldName = "TerminationNote"
  Me.colTerminationNote.Name = "colTerminationNote"
  '
  'colCustomerFirstName
  '
  resources.ApplyResources(Me.colCustomerFirstName, "colCustomerFirstName")
  Me.colCustomerFirstName.FieldName = "CustomerFirstName"
  Me.colCustomerFirstName.Name = "colCustomerFirstName"
  '
  'colCustomerSurname
  '
  resources.ApplyResources(Me.colCustomerSurname, "colCustomerSurname")
  Me.colCustomerSurname.FieldName = "CustomerSurname"
  Me.colCustomerSurname.Name = "colCustomerSurname"
  '
  'colMB
  '
  resources.ApplyResources(Me.colMB, "colMB")
  Me.colMB.FieldName = "MB"
  Me.colMB.Name = "colMB"
  '
  'colShellNumber
  '
  resources.ApplyResources(Me.colShellNumber, "colShellNumber")
  Me.colShellNumber.FieldName = "ShellNumber"
  Me.colShellNumber.Name = "colShellNumber"
  '
  'colLastRegistration
  '
  resources.ApplyResources(Me.colLastRegistration, "colLastRegistration")
  Me.colLastRegistration.FieldName = "LastRegistration"
  Me.colLastRegistration.Name = "colLastRegistration"
  '
  'colCustomerName
  '
  resources.ApplyResources(Me.colCustomerName, "colCustomerName")
  Me.colCustomerName.FieldName = "CustomerName"
  Me.colCustomerName.Name = "colCustomerName"
  '
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.SplitterItem1, Me.LayoutControlItem2})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "Root"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(672, 509)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.CustomerVehiclesRelationsListGridControl
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 64)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(668, 441)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem1.TextToControlDistance = 0
  Me.LayoutControlItem1.TextVisible = False
  '
  'SplitterItem1
  '
  resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
  Me.SplitterItem1.Location = New System.Drawing.Point(0, 58)
  Me.SplitterItem1.Name = "SplitterItem1"
  Me.SplitterItem1.Size = New System.Drawing.Size(668, 6)
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.UxKopcinja1
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(668, 58)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem2.TextToControlDistance = 0
  Me.LayoutControlItem2.TextVisible = False
  '
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.CustomerVehiclesRelationsBindingSource
  '
  'uxCustomerVehicleRelation
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.LayoutControl1)
  Me.Name = "uxCustomerVehicleRelation"
  Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.CustomerVehiclesRelationsListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CustomerVehiclesRelationsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LookUpEditRelationType, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CustomerVehicleRelationTypeListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LookUpEditCustomers, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CustomersListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LookUpEditVehicles, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.VehicleListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents CustomerVehiclesRelationsListGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents LookUpEditRelationType As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents CustomerVehicleRelationTypeListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents LookUpEditCustomers As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents CustomersListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents LookUpEditVehicles As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents VehicleListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents CustomerVehiclesRelationsBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdRelationType As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdCustomer As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdVehicle As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colStartDate As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colEndDate As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colBeginNote As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colTerminationNote As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents colCustomerFirstName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerSurname As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMB As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colShellNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLastRegistration As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerName As DevExpress.XtraGrid.Columns.GridColumn

End Class
