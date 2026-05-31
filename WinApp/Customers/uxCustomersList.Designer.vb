<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxCustomersList
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxCustomersList))
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.btnPrint = New DevExpress.XtraEditors.SimpleButton
  Me.btnRefresh = New DevExpress.XtraEditors.SimpleButton
  Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
  Me.btnExit = New DevExpress.XtraEditors.SimpleButton
  Me.btnAddNew = New DevExpress.XtraEditors.SimpleButton
  Me.btnShowDetails = New DevExpress.XtraEditors.SimpleButton
  Me.CustomersListGridControl = New DevExpress.XtraGrid.GridControl
  Me.CustomersListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colMB = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCustomerSurname = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCustomerFirstName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colLivingAddressNumber = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIsCompany = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCityName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCountryName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colStreetName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.HyperLinkEditCustomer = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
  Me.colAddressOfLiving = New DevExpress.XtraGrid.Columns.GridColumn
  Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
  Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
  Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
  Me.PrintingSystem1 = New DevExpress.XtraPrinting.PrintingSystem(Me.components)
  Me.PrintableComponentLink1 = New DevExpress.XtraPrinting.PrintableComponentLink(Me.components)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.LayoutControl1.SuspendLayout()
  CType(Me.CustomersListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CustomersListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.HyperLinkEditCustomer, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'LayoutControl1
  '
  Me.LayoutControl1.AccessibleDescription = Nothing
  Me.LayoutControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LayoutControl1, False)
  Me.LayoutControl1.BackgroundImage = Nothing
  Me.LayoutControl1.Controls.Add(Me.btnPrint)
  Me.LayoutControl1.Controls.Add(Me.btnRefresh)
  Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
  Me.LayoutControl1.Controls.Add(Me.btnExit)
  Me.LayoutControl1.Controls.Add(Me.btnAddNew)
  Me.LayoutControl1.Controls.Add(Me.btnShowDetails)
  Me.LayoutControl1.Controls.Add(Me.CustomersListGridControl)
  Me.LayoutControl1.Font = Nothing
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
  '
  'btnPrint
  '
  Me.btnPrint.AccessibleDescription = Nothing
  Me.btnPrint.AccessibleName = Nothing
  resources.ApplyResources(Me.btnPrint, "btnPrint")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.btnPrint, False)
  Me.btnPrint.BackgroundImage = Nothing
  Me.btnPrint.MaximumSize = New System.Drawing.Size(0, 41)
  Me.btnPrint.Name = "btnPrint"
  Me.btnPrint.StyleController = Me.LayoutControl1
  '
  'btnRefresh
  '
  Me.btnRefresh.AccessibleDescription = Nothing
  Me.btnRefresh.AccessibleName = Nothing
  resources.ApplyResources(Me.btnRefresh, "btnRefresh")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.btnRefresh, False)
  Me.btnRefresh.BackgroundImage = Nothing
  Me.btnRefresh.MinimumSize = New System.Drawing.Size(0, 39)
  Me.btnRefresh.Name = "btnRefresh"
  Me.btnRefresh.StyleController = Me.LayoutControl1
  '
  'SimpleButton1
  '
  Me.SimpleButton1.AccessibleDescription = Nothing
  Me.SimpleButton1.AccessibleName = Nothing
  resources.ApplyResources(Me.SimpleButton1, "SimpleButton1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SimpleButton1, False)
  Me.SimpleButton1.BackgroundImage = Nothing
  Me.SimpleButton1.MinimumSize = New System.Drawing.Size(0, 39)
  Me.SimpleButton1.Name = "SimpleButton1"
  Me.SimpleButton1.StyleController = Me.LayoutControl1
  '
  'btnExit
  '
  Me.btnExit.AccessibleDescription = Nothing
  Me.btnExit.AccessibleName = Nothing
  resources.ApplyResources(Me.btnExit, "btnExit")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.btnExit, False)
  Me.btnExit.BackgroundImage = Nothing
  Me.btnExit.Name = "btnExit"
  Me.btnExit.StyleController = Me.LayoutControl1
  '
  'btnAddNew
  '
  Me.btnAddNew.AccessibleDescription = Nothing
  Me.btnAddNew.AccessibleName = Nothing
  resources.ApplyResources(Me.btnAddNew, "btnAddNew")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.btnAddNew, False)
  Me.btnAddNew.BackgroundImage = Nothing
  Me.btnAddNew.Name = "btnAddNew"
  Me.btnAddNew.StyleController = Me.LayoutControl1
  '
  'btnShowDetails
  '
  Me.btnShowDetails.AccessibleDescription = Nothing
  Me.btnShowDetails.AccessibleName = Nothing
  resources.ApplyResources(Me.btnShowDetails, "btnShowDetails")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.btnShowDetails, False)
  Me.btnShowDetails.BackgroundImage = Nothing
  Me.btnShowDetails.Name = "btnShowDetails"
  Me.btnShowDetails.StyleController = Me.LayoutControl1
  '
  'CustomersListGridControl
  '
  Me.CustomersListGridControl.AccessibleDescription = Nothing
  Me.CustomersListGridControl.AccessibleName = Nothing
  resources.ApplyResources(Me.CustomersListGridControl, "CustomersListGridControl")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.CustomersListGridControl, False)
  Me.CustomersListGridControl.BackgroundImage = Nothing
  Me.CustomersListGridControl.DataSource = Me.CustomersListBindingSource
  Me.CustomersListGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
  Me.CustomersListGridControl.EmbeddedNavigator.AccessibleName = Nothing
  Me.CustomersListGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("CustomersListGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
  Me.CustomersListGridControl.EmbeddedNavigator.BackgroundImage = Nothing
  Me.CustomersListGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("CustomersListGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
  Me.CustomersListGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("CustomersListGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
  Me.CustomersListGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("CustomersListGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
  Me.CustomersListGridControl.EmbeddedNavigator.ToolTip = resources.GetString("CustomersListGridControl.EmbeddedNavigator.ToolTip")
  Me.CustomersListGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("CustomersListGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
  Me.CustomersListGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("CustomersListGridControl.EmbeddedNavigator.ToolTipTitle")
  Me.CustomersListGridControl.Font = Nothing
  Me.CustomersListGridControl.MainView = Me.GridView1
  Me.CustomersListGridControl.Name = "CustomersListGridControl"
  Me.CustomersListGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.HyperLinkEditCustomer})
  Me.CustomersListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'CustomersListBindingSource
  '
  Me.CustomersListBindingSource.DataSource = GetType(VTE.Library.CustomersListShort)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CustomersListBindingSource, False)
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
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colMB, Me.colCustomerSurname, Me.colCustomerFirstName, Me.colLivingAddressNumber, Me.colIsCompany, Me.colCityName, Me.colCountryName, Me.colStreetName, Me.colName, Me.colAddressOfLiving})
  Me.GridView1.GridControl = Me.CustomersListGridControl
  Me.GridView1.Name = "GridView1"
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
  'colMB
  '
  resources.ApplyResources(Me.colMB, "colMB")
  Me.colMB.FieldName = "MB"
  Me.colMB.Name = "colMB"
  Me.colMB.OptionsColumn.AllowEdit = False
  Me.colMB.OptionsColumn.AllowFocus = False
  Me.colMB.OptionsColumn.ReadOnly = True
  Me.colMB.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
  '
  'colCustomerSurname
  '
  resources.ApplyResources(Me.colCustomerSurname, "colCustomerSurname")
  Me.colCustomerSurname.FieldName = "CustomerSurname"
  Me.colCustomerSurname.Name = "colCustomerSurname"
  Me.colCustomerSurname.OptionsColumn.ReadOnly = True
  '
  'colCustomerFirstName
  '
  resources.ApplyResources(Me.colCustomerFirstName, "colCustomerFirstName")
  Me.colCustomerFirstName.FieldName = "CustomerFirstName"
  Me.colCustomerFirstName.Name = "colCustomerFirstName"
  Me.colCustomerFirstName.OptionsColumn.ReadOnly = True
  '
  'colLivingAddressNumber
  '
  resources.ApplyResources(Me.colLivingAddressNumber, "colLivingAddressNumber")
  Me.colLivingAddressNumber.FieldName = "LivingAddressNumber"
  Me.colLivingAddressNumber.Name = "colLivingAddressNumber"
  Me.colLivingAddressNumber.OptionsColumn.ReadOnly = True
  '
  'colIsCompany
  '
  resources.ApplyResources(Me.colIsCompany, "colIsCompany")
  Me.colIsCompany.FieldName = "IsCompany"
  Me.colIsCompany.Name = "colIsCompany"
  Me.colIsCompany.OptionsColumn.AllowEdit = False
  Me.colIsCompany.OptionsColumn.AllowFocus = False
  Me.colIsCompany.OptionsColumn.ReadOnly = True
  '
  'colCityName
  '
  resources.ApplyResources(Me.colCityName, "colCityName")
  Me.colCityName.FieldName = "CityName"
  Me.colCityName.Name = "colCityName"
  Me.colCityName.OptionsColumn.ReadOnly = True
  '
  'colCountryName
  '
  resources.ApplyResources(Me.colCountryName, "colCountryName")
  Me.colCountryName.FieldName = "CountryName"
  Me.colCountryName.Name = "colCountryName"
  Me.colCountryName.OptionsColumn.ReadOnly = True
  '
  'colStreetName
  '
  resources.ApplyResources(Me.colStreetName, "colStreetName")
  Me.colStreetName.FieldName = "StreetName"
  Me.colStreetName.Name = "colStreetName"
  Me.colStreetName.OptionsColumn.ReadOnly = True
  '
  'colName
  '
  resources.ApplyResources(Me.colName, "colName")
  Me.colName.ColumnEdit = Me.HyperLinkEditCustomer
  Me.colName.FieldName = "Name"
  Me.colName.Name = "colName"
  Me.colName.OptionsColumn.ReadOnly = True
  Me.colName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
  Me.colName.SummaryItem.DisplayFormat = resources.GetString("colName.SummaryItem.DisplayFormat")
  Me.colName.SummaryItem.FieldName = "Id"
  Me.colName.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
  '
  'HyperLinkEditCustomer
  '
  Me.HyperLinkEditCustomer.AccessibleDescription = Nothing
  Me.HyperLinkEditCustomer.AccessibleName = Nothing
  resources.ApplyResources(Me.HyperLinkEditCustomer, "HyperLinkEditCustomer")
  Me.HyperLinkEditCustomer.Mask.AutoComplete = CType(resources.GetObject("HyperLinkEditCustomer.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.HyperLinkEditCustomer.Mask.BeepOnError = CType(resources.GetObject("HyperLinkEditCustomer.Mask.BeepOnError"), Boolean)
  Me.HyperLinkEditCustomer.Mask.EditMask = resources.GetString("HyperLinkEditCustomer.Mask.EditMask")
  Me.HyperLinkEditCustomer.Mask.IgnoreMaskBlank = CType(resources.GetObject("HyperLinkEditCustomer.Mask.IgnoreMaskBlank"), Boolean)
  Me.HyperLinkEditCustomer.Mask.MaskType = CType(resources.GetObject("HyperLinkEditCustomer.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.HyperLinkEditCustomer.Mask.PlaceHolder = CType(resources.GetObject("HyperLinkEditCustomer.Mask.PlaceHolder"), Char)
  Me.HyperLinkEditCustomer.Mask.SaveLiteral = CType(resources.GetObject("HyperLinkEditCustomer.Mask.SaveLiteral"), Boolean)
  Me.HyperLinkEditCustomer.Mask.ShowPlaceHolders = CType(resources.GetObject("HyperLinkEditCustomer.Mask.ShowPlaceHolders"), Boolean)
  Me.HyperLinkEditCustomer.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("HyperLinkEditCustomer.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.HyperLinkEditCustomer.Name = "HyperLinkEditCustomer"
  '
  'colAddressOfLiving
  '
  resources.ApplyResources(Me.colAddressOfLiving, "colAddressOfLiving")
  Me.colAddressOfLiving.FieldName = "AddressOfLiving"
  Me.colAddressOfLiving.Name = "colAddressOfLiving"
  Me.colAddressOfLiving.OptionsColumn.AllowEdit = False
  Me.colAddressOfLiving.OptionsColumn.AllowFocus = False
  Me.colAddressOfLiving.OptionsColumn.ReadOnly = True
  Me.colAddressOfLiving.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
  '
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem3, Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem5, Me.LayoutControlItem6, Me.LayoutControlItem7})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "Root"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(810, 585)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.CustomersListGridControl
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(806, 531)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem1.TextToControlDistance = 0
  Me.LayoutControlItem1.TextVisible = False
  '
  'LayoutControlItem3
  '
  Me.LayoutControlItem3.Control = Me.btnAddNew
  resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
  Me.LayoutControlItem3.Location = New System.Drawing.Point(144, 531)
  Me.LayoutControlItem3.MaxSize = New System.Drawing.Size(0, 50)
  Me.LayoutControlItem3.MinSize = New System.Drawing.Size(73, 50)
  Me.LayoutControlItem3.Name = "LayoutControlItem3"
  Me.LayoutControlItem3.Size = New System.Drawing.Size(139, 50)
  Me.LayoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
  Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem3.TextToControlDistance = 0
  Me.LayoutControlItem3.TextVisible = False
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.btnShowDetails
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(406, 531)
  Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(0, 50)
  Me.LayoutControlItem2.MinSize = New System.Drawing.Size(91, 50)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(128, 50)
  Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem2.TextToControlDistance = 0
  Me.LayoutControlItem2.TextVisible = False
  '
  'LayoutControlItem4
  '
  Me.LayoutControlItem4.Control = Me.btnExit
  resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
  Me.LayoutControlItem4.Location = New System.Drawing.Point(670, 531)
  Me.LayoutControlItem4.MaxSize = New System.Drawing.Size(0, 50)
  Me.LayoutControlItem4.MinSize = New System.Drawing.Size(49, 50)
  Me.LayoutControlItem4.Name = "LayoutControlItem4"
  Me.LayoutControlItem4.Size = New System.Drawing.Size(136, 50)
  Me.LayoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
  Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem4.TextToControlDistance = 0
  Me.LayoutControlItem4.TextVisible = False
  '
  'LayoutControlItem5
  '
  Me.LayoutControlItem5.Control = Me.SimpleButton1
  resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
  Me.LayoutControlItem5.Location = New System.Drawing.Point(534, 531)
  Me.LayoutControlItem5.Name = "LayoutControlItem5"
  Me.LayoutControlItem5.Size = New System.Drawing.Size(136, 50)
  Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem5.TextToControlDistance = 0
  Me.LayoutControlItem5.TextVisible = False
  '
  'LayoutControlItem6
  '
  Me.LayoutControlItem6.Control = Me.btnRefresh
  resources.ApplyResources(Me.LayoutControlItem6, "LayoutControlItem6")
  Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 531)
  Me.LayoutControlItem6.Name = "LayoutControlItem6"
  Me.LayoutControlItem6.Size = New System.Drawing.Size(144, 50)
  Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem6.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem6.TextToControlDistance = 0
  Me.LayoutControlItem6.TextVisible = False
  '
  'LayoutControlItem7
  '
  Me.LayoutControlItem7.Control = Me.btnPrint
  resources.ApplyResources(Me.LayoutControlItem7, "LayoutControlItem7")
  Me.LayoutControlItem7.Location = New System.Drawing.Point(283, 531)
  Me.LayoutControlItem7.Name = "LayoutControlItem7"
  Me.LayoutControlItem7.Size = New System.Drawing.Size(123, 50)
  Me.LayoutControlItem7.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem7.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem7.TextToControlDistance = 0
  Me.LayoutControlItem7.TextVisible = False
  '
  'PrintingSystem1
  '
  Me.PrintingSystem1.ExportOptions.Csv.EncodingType = CType(resources.GetObject("PrintingSystem1.ExportOptions.Csv.EncodingType"), DevExpress.XtraPrinting.EncodingType)
  Me.PrintingSystem1.ExportOptions.Html.CharacterSet = resources.GetString("PrintingSystem1.ExportOptions.Html.CharacterSet")
  Me.PrintingSystem1.ExportOptions.Html.Title = resources.GetString("PrintingSystem1.ExportOptions.Html.Title")
  Me.PrintingSystem1.ExportOptions.Mht.CharacterSet = resources.GetString("PrintingSystem1.ExportOptions.Mht.CharacterSet")
  Me.PrintingSystem1.ExportOptions.Mht.Title = resources.GetString("PrintingSystem1.ExportOptions.Mht.Title")
  Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Application = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Application")
  Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Author = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Author")
  Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Keywords = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Keywords")
  Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Subject = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Subject")
  Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Title = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Title")
  Me.PrintingSystem1.ExportOptions.Text.EncodingType = CType(resources.GetObject("PrintingSystem1.ExportOptions.Text.EncodingType"), DevExpress.XtraPrinting.EncodingType)
  Me.PrintingSystem1.ExportOptions.Xls.SheetName = resources.GetString("PrintingSystem1.ExportOptions.Xls.SheetName")
  Me.PrintingSystem1.Links.AddRange(New Object() {Me.PrintableComponentLink1})
  '
  'PrintableComponentLink1
  '
  Me.PrintableComponentLink1.Component = Me.CustomersListGridControl
  Me.PrintableComponentLink1.CustomPaperSize = New System.Drawing.Size(0, 0)
  Me.PrintableComponentLink1.ImageStream = CType(resources.GetObject("PrintableComponentLink1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
  Me.PrintableComponentLink1.Landscape = True
  Me.PrintableComponentLink1.Margins = New System.Drawing.Printing.Margins(30, 30, 30, 30)
  Me.PrintableComponentLink1.PaperKind = System.Drawing.Printing.PaperKind.A4
  Me.PrintableComponentLink1.PrintingSystem = Me.PrintingSystem1
  Me.PrintableComponentLink1.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart
  '
  'uxCustomersList
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.LayoutControl1)
  Me.Name = "uxCustomersList"
  Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.CustomersListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CustomersListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.HyperLinkEditCustomer, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnAddNew As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnShowDetails As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents CustomersListGridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents CustomersListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents HyperLinkEditCustomer As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
    Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
    Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMB As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerSurname As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerFirstName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLivingAddressNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIsCompany As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCityName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCountryName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colStreetName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAddressOfLiving As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents PrintingSystem1 As DevExpress.XtraPrinting.PrintingSystem
    Friend WithEvents PrintableComponentLink1 As DevExpress.XtraPrinting.PrintableComponentLink
    Friend WithEvents btnPrint As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem

End Class
