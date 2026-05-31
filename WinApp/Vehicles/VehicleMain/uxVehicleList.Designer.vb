<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxVehicleList
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxVehicleList))
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.btnPrint = New DevExpress.XtraEditors.SimpleButton
    Me.btnRefresh = New DevExpress.XtraEditors.SimpleButton
    Me.btnSelect = New DevExpress.XtraEditors.SimpleButton
    Me.btnExit = New DevExpress.XtraEditors.SimpleButton
    Me.btnAddNew = New DevExpress.XtraEditors.SimpleButton
    Me.VehicleListGridControl = New DevExpress.XtraGrid.GridControl
    Me.VehicleListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.colCustomerSurname = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colCustomerFirstName = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colCustomer = New DevExpress.XtraGrid.Columns.GridColumn
    Me.HyperLinkEditCustomer = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Me.colMB = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colShellNumber = New DevExpress.XtraGrid.Columns.GridColumn
    Me.HyperLinkEditShelNum = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Me.colModelName = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colVehiceMaker = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colLastRegistratinNumber = New DevExpress.XtraGrid.Columns.GridColumn
    Me.HyperLinkEditShellNumber = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Me.rideLastRegDate = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Me.HyperLinkEditRegNumber = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Me.HyperLinkEditRegBr = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
    Me.PrintingSystem1 = New DevExpress.XtraPrinting.PrintingSystem(Me.components)
    Me.PrintableComponentLink1 = New DevExpress.XtraPrinting.PrintableComponentLink(Me.components)
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.VehicleListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.VehicleListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.HyperLinkEditCustomer, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.HyperLinkEditShelNum, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.HyperLinkEditShellNumber, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.rideLastRegDate, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.rideLastRegDate.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.HyperLinkEditRegNumber, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.HyperLinkEditRegBr, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'LayoutControl1
    '
    Me.LayoutControl1.AccessibleDescription = Nothing
    Me.LayoutControl1.AccessibleName = Nothing
    resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
    Me.LayoutControl1.BackgroundImage = Nothing
    Me.LayoutControl1.Controls.Add(Me.btnPrint)
    Me.LayoutControl1.Controls.Add(Me.btnRefresh)
    Me.LayoutControl1.Controls.Add(Me.btnSelect)
    Me.LayoutControl1.Controls.Add(Me.btnExit)
    Me.LayoutControl1.Controls.Add(Me.btnAddNew)
    Me.LayoutControl1.Controls.Add(Me.VehicleListGridControl)
    Me.LayoutControl1.Font = Nothing
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    '
    'btnPrint
    '
    Me.btnPrint.AccessibleDescription = Nothing
    Me.btnPrint.AccessibleName = Nothing
    resources.ApplyResources(Me.btnPrint, "btnPrint")
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
    Me.btnRefresh.BackgroundImage = Nothing
    Me.btnRefresh.MinimumSize = New System.Drawing.Size(0, 41)
    Me.btnRefresh.Name = "btnRefresh"
    Me.btnRefresh.StyleController = Me.LayoutControl1
    '
    'btnSelect
    '
    Me.btnSelect.AccessibleDescription = Nothing
    Me.btnSelect.AccessibleName = Nothing
    resources.ApplyResources(Me.btnSelect, "btnSelect")
    Me.btnSelect.BackgroundImage = Nothing
    Me.btnSelect.Name = "btnSelect"
    Me.btnSelect.StyleController = Me.LayoutControl1
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
    'btnAddNew
    '
    Me.btnAddNew.AccessibleDescription = Nothing
    Me.btnAddNew.AccessibleName = Nothing
    resources.ApplyResources(Me.btnAddNew, "btnAddNew")
    Me.btnAddNew.BackgroundImage = Nothing
    Me.btnAddNew.Name = "btnAddNew"
    Me.btnAddNew.StyleController = Me.LayoutControl1
    '
    'VehicleListGridControl
    '
    Me.VehicleListGridControl.AccessibleDescription = Nothing
    Me.VehicleListGridControl.AccessibleName = Nothing
    resources.ApplyResources(Me.VehicleListGridControl, "VehicleListGridControl")
    Me.VehicleListGridControl.BackgroundImage = Nothing
    Me.VehicleListGridControl.DataSource = Me.VehicleListBindingSource
    Me.VehicleListGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
    Me.VehicleListGridControl.EmbeddedNavigator.AccessibleName = Nothing
    Me.VehicleListGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("VehicleListGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.VehicleListGridControl.EmbeddedNavigator.BackgroundImage = Nothing
    Me.VehicleListGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("VehicleListGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
    Me.VehicleListGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("VehicleListGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
    Me.VehicleListGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("VehicleListGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
    Me.VehicleListGridControl.EmbeddedNavigator.ToolTip = resources.GetString("VehicleListGridControl.EmbeddedNavigator.ToolTip")
    Me.VehicleListGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("VehicleListGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
    Me.VehicleListGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("VehicleListGridControl.EmbeddedNavigator.ToolTipTitle")
    Me.VehicleListGridControl.Font = Nothing
    Me.VehicleListGridControl.MainView = Me.GridView1
    Me.VehicleListGridControl.Name = "VehicleListGridControl"
    Me.VehicleListGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.HyperLinkEditShellNumber, Me.rideLastRegDate, Me.HyperLinkEditRegNumber, Me.HyperLinkEditRegBr, Me.HyperLinkEditShelNum, Me.HyperLinkEditCustomer})
    Me.VehicleListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
    '
    'VehicleListBindingSource
    '
    Me.VehicleListBindingSource.DataSource = GetType(VTE.Library.VehiclesListShortListAll)
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
    Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colCustomerSurname, Me.colCustomerFirstName, Me.colCustomer, Me.colMB, Me.colShellNumber, Me.colModelName, Me.colVehiceMaker, Me.colLastRegistratinNumber})
    Me.GridView1.GridControl = Me.VehicleListGridControl
    Me.GridView1.Name = "GridView1"
    Me.GridView1.OptionsView.ShowAutoFilterRow = True
    Me.GridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways
    Me.GridView1.OptionsView.ShowFooter = True
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
    'colCustomer
    '
    resources.ApplyResources(Me.colCustomer, "colCustomer")
    Me.colCustomer.ColumnEdit = Me.HyperLinkEditCustomer
    Me.colCustomer.FieldName = "OwnerName"
    Me.colCustomer.Name = "colCustomer"
    Me.colCustomer.OptionsColumn.ReadOnly = True
    Me.colCustomer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
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
    'colMB
    '
    resources.ApplyResources(Me.colMB, "colMB")
    Me.colMB.FieldName = "OwnerMB"
    Me.colMB.Name = "colMB"
    Me.colMB.OptionsColumn.AllowEdit = False
    Me.colMB.OptionsColumn.AllowFocus = False
    Me.colMB.OptionsColumn.ReadOnly = True
    Me.colMB.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    '
    'colShellNumber
    '
    resources.ApplyResources(Me.colShellNumber, "colShellNumber")
    Me.colShellNumber.ColumnEdit = Me.HyperLinkEditShelNum
    Me.colShellNumber.FieldName = "ShellNumber"
    Me.colShellNumber.Name = "colShellNumber"
    Me.colShellNumber.OptionsColumn.ReadOnly = True
    Me.colShellNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    Me.colShellNumber.SummaryItem.DisplayFormat = resources.GetString("colShellNumber.SummaryItem.DisplayFormat")
    Me.colShellNumber.SummaryItem.FieldName = "Id"
    Me.colShellNumber.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
    '
    'HyperLinkEditShelNum
    '
    Me.HyperLinkEditShelNum.AccessibleDescription = Nothing
    Me.HyperLinkEditShelNum.AccessibleName = Nothing
    resources.ApplyResources(Me.HyperLinkEditShelNum, "HyperLinkEditShelNum")
    Me.HyperLinkEditShelNum.Mask.AutoComplete = CType(resources.GetObject("HyperLinkEditShelNum.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.HyperLinkEditShelNum.Mask.BeepOnError = CType(resources.GetObject("HyperLinkEditShelNum.Mask.BeepOnError"), Boolean)
    Me.HyperLinkEditShelNum.Mask.EditMask = resources.GetString("HyperLinkEditShelNum.Mask.EditMask")
    Me.HyperLinkEditShelNum.Mask.IgnoreMaskBlank = CType(resources.GetObject("HyperLinkEditShelNum.Mask.IgnoreMaskBlank"), Boolean)
    Me.HyperLinkEditShelNum.Mask.MaskType = CType(resources.GetObject("HyperLinkEditShelNum.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.HyperLinkEditShelNum.Mask.PlaceHolder = CType(resources.GetObject("HyperLinkEditShelNum.Mask.PlaceHolder"), Char)
    Me.HyperLinkEditShelNum.Mask.SaveLiteral = CType(resources.GetObject("HyperLinkEditShelNum.Mask.SaveLiteral"), Boolean)
    Me.HyperLinkEditShelNum.Mask.ShowPlaceHolders = CType(resources.GetObject("HyperLinkEditShelNum.Mask.ShowPlaceHolders"), Boolean)
    Me.HyperLinkEditShelNum.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("HyperLinkEditShelNum.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.HyperLinkEditShelNum.Name = "HyperLinkEditShelNum"
    '
    'colModelName
    '
    resources.ApplyResources(Me.colModelName, "colModelName")
    Me.colModelName.FieldName = "ModelName"
    Me.colModelName.Name = "colModelName"
    Me.colModelName.OptionsColumn.AllowEdit = False
    Me.colModelName.OptionsColumn.AllowFocus = False
    Me.colModelName.OptionsColumn.ReadOnly = True
    Me.colModelName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    Me.colModelName.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList
    '
    'colVehiceMaker
    '
    resources.ApplyResources(Me.colVehiceMaker, "colVehiceMaker")
    Me.colVehiceMaker.FieldName = "VehiceMaker"
    Me.colVehiceMaker.Name = "colVehiceMaker"
    '
    'colLastRegistratinNumber
    '
    resources.ApplyResources(Me.colLastRegistratinNumber, "colLastRegistratinNumber")
    Me.colLastRegistratinNumber.FieldName = "LastRegistration"
    Me.colLastRegistratinNumber.Name = "colLastRegistratinNumber"
    '
    'HyperLinkEditShellNumber
    '
    Me.HyperLinkEditShellNumber.AccessibleDescription = Nothing
    Me.HyperLinkEditShellNumber.AccessibleName = Nothing
    resources.ApplyResources(Me.HyperLinkEditShellNumber, "HyperLinkEditShellNumber")
    Me.HyperLinkEditShellNumber.Mask.AutoComplete = CType(resources.GetObject("HyperLinkEditShellNumber.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.HyperLinkEditShellNumber.Mask.BeepOnError = CType(resources.GetObject("HyperLinkEditShellNumber.Mask.BeepOnError"), Boolean)
    Me.HyperLinkEditShellNumber.Mask.EditMask = resources.GetString("HyperLinkEditShellNumber.Mask.EditMask")
    Me.HyperLinkEditShellNumber.Mask.IgnoreMaskBlank = CType(resources.GetObject("HyperLinkEditShellNumber.Mask.IgnoreMaskBlank"), Boolean)
    Me.HyperLinkEditShellNumber.Mask.MaskType = CType(resources.GetObject("HyperLinkEditShellNumber.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.HyperLinkEditShellNumber.Mask.PlaceHolder = CType(resources.GetObject("HyperLinkEditShellNumber.Mask.PlaceHolder"), Char)
    Me.HyperLinkEditShellNumber.Mask.SaveLiteral = CType(resources.GetObject("HyperLinkEditShellNumber.Mask.SaveLiteral"), Boolean)
    Me.HyperLinkEditShellNumber.Mask.ShowPlaceHolders = CType(resources.GetObject("HyperLinkEditShellNumber.Mask.ShowPlaceHolders"), Boolean)
    Me.HyperLinkEditShellNumber.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("HyperLinkEditShellNumber.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.HyperLinkEditShellNumber.Name = "HyperLinkEditShellNumber"
    '
    'rideLastRegDate
    '
    Me.rideLastRegDate.AccessibleDescription = Nothing
    Me.rideLastRegDate.AccessibleName = Nothing
    resources.ApplyResources(Me.rideLastRegDate, "rideLastRegDate")
    Me.rideLastRegDate.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("rideLastRegDate.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.rideLastRegDate.Mask.AutoComplete = CType(resources.GetObject("rideLastRegDate.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.rideLastRegDate.Mask.BeepOnError = CType(resources.GetObject("rideLastRegDate.Mask.BeepOnError"), Boolean)
    Me.rideLastRegDate.Mask.EditMask = resources.GetString("rideLastRegDate.Mask.EditMask")
    Me.rideLastRegDate.Mask.IgnoreMaskBlank = CType(resources.GetObject("rideLastRegDate.Mask.IgnoreMaskBlank"), Boolean)
    Me.rideLastRegDate.Mask.MaskType = CType(resources.GetObject("rideLastRegDate.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.rideLastRegDate.Mask.PlaceHolder = CType(resources.GetObject("rideLastRegDate.Mask.PlaceHolder"), Char)
    Me.rideLastRegDate.Mask.SaveLiteral = CType(resources.GetObject("rideLastRegDate.Mask.SaveLiteral"), Boolean)
    Me.rideLastRegDate.Mask.ShowPlaceHolders = CType(resources.GetObject("rideLastRegDate.Mask.ShowPlaceHolders"), Boolean)
    Me.rideLastRegDate.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("rideLastRegDate.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.rideLastRegDate.Name = "rideLastRegDate"
    Me.rideLastRegDate.VistaTimeProperties.AccessibleDescription = Nothing
    Me.rideLastRegDate.VistaTimeProperties.AccessibleName = Nothing
    Me.rideLastRegDate.VistaTimeProperties.AutoHeight = CType(resources.GetObject("rideLastRegDate.VistaTimeProperties.AutoHeight"), Boolean)
    Me.rideLastRegDate.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.rideLastRegDate.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("rideLastRegDate.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.rideLastRegDate.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("rideLastRegDate.VistaTimeProperties.Mask.BeepOnError"), Boolean)
    Me.rideLastRegDate.VistaTimeProperties.Mask.EditMask = resources.GetString("rideLastRegDate.VistaTimeProperties.Mask.EditMask")
    Me.rideLastRegDate.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("rideLastRegDate.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
    Me.rideLastRegDate.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("rideLastRegDate.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.rideLastRegDate.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("rideLastRegDate.VistaTimeProperties.Mask.PlaceHolder"), Char)
    Me.rideLastRegDate.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("rideLastRegDate.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
    Me.rideLastRegDate.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("rideLastRegDate.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
    Me.rideLastRegDate.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("rideLastRegDate.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
    '
    'HyperLinkEditRegNumber
    '
    Me.HyperLinkEditRegNumber.AccessibleDescription = Nothing
    Me.HyperLinkEditRegNumber.AccessibleName = Nothing
    resources.ApplyResources(Me.HyperLinkEditRegNumber, "HyperLinkEditRegNumber")
    Me.HyperLinkEditRegNumber.Mask.AutoComplete = CType(resources.GetObject("HyperLinkEditRegNumber.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.HyperLinkEditRegNumber.Mask.BeepOnError = CType(resources.GetObject("HyperLinkEditRegNumber.Mask.BeepOnError"), Boolean)
    Me.HyperLinkEditRegNumber.Mask.EditMask = resources.GetString("HyperLinkEditRegNumber.Mask.EditMask")
    Me.HyperLinkEditRegNumber.Mask.IgnoreMaskBlank = CType(resources.GetObject("HyperLinkEditRegNumber.Mask.IgnoreMaskBlank"), Boolean)
    Me.HyperLinkEditRegNumber.Mask.MaskType = CType(resources.GetObject("HyperLinkEditRegNumber.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.HyperLinkEditRegNumber.Mask.PlaceHolder = CType(resources.GetObject("HyperLinkEditRegNumber.Mask.PlaceHolder"), Char)
    Me.HyperLinkEditRegNumber.Mask.SaveLiteral = CType(resources.GetObject("HyperLinkEditRegNumber.Mask.SaveLiteral"), Boolean)
    Me.HyperLinkEditRegNumber.Mask.ShowPlaceHolders = CType(resources.GetObject("HyperLinkEditRegNumber.Mask.ShowPlaceHolders"), Boolean)
    Me.HyperLinkEditRegNumber.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("HyperLinkEditRegNumber.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.HyperLinkEditRegNumber.Name = "HyperLinkEditRegNumber"
    '
    'HyperLinkEditRegBr
    '
    Me.HyperLinkEditRegBr.AccessibleDescription = Nothing
    Me.HyperLinkEditRegBr.AccessibleName = Nothing
    resources.ApplyResources(Me.HyperLinkEditRegBr, "HyperLinkEditRegBr")
    Me.HyperLinkEditRegBr.Mask.AutoComplete = CType(resources.GetObject("HyperLinkEditRegBr.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.HyperLinkEditRegBr.Mask.BeepOnError = CType(resources.GetObject("HyperLinkEditRegBr.Mask.BeepOnError"), Boolean)
    Me.HyperLinkEditRegBr.Mask.EditMask = Nothing
    Me.HyperLinkEditRegBr.Mask.IgnoreMaskBlank = CType(resources.GetObject("HyperLinkEditRegBr.Mask.IgnoreMaskBlank"), Boolean)
    Me.HyperLinkEditRegBr.Mask.MaskType = CType(resources.GetObject("HyperLinkEditRegBr.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.HyperLinkEditRegBr.Mask.PlaceHolder = CType(resources.GetObject("HyperLinkEditRegBr.Mask.PlaceHolder"), Char)
    Me.HyperLinkEditRegBr.Mask.SaveLiteral = CType(resources.GetObject("HyperLinkEditRegBr.Mask.SaveLiteral"), Boolean)
    Me.HyperLinkEditRegBr.Mask.ShowPlaceHolders = CType(resources.GetObject("HyperLinkEditRegBr.Mask.ShowPlaceHolders"), Boolean)
    Me.HyperLinkEditRegBr.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("HyperLinkEditRegBr.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.HyperLinkEditRegBr.Name = "HyperLinkEditRegBr"
    '
    'GridView2
    '
    resources.ApplyResources(Me.GridView2, "GridView2")
    Me.GridView2.GridControl = Me.VehicleListGridControl
    Me.GridView2.Name = "GridView2"
    '
    'LayoutControlGroup1
    '
    resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem3, Me.LayoutControlItem5, Me.LayoutControlItem6})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(759, 577)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.TextVisible = False
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.VehicleListGridControl
    resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(755, 521)
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem1.TextToControlDistance = 0
    Me.LayoutControlItem1.TextVisible = False
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.btnAddNew
    resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
    Me.LayoutControlItem2.Location = New System.Drawing.Point(151, 521)
    Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(0, 52)
    Me.LayoutControlItem2.MinSize = New System.Drawing.Size(50, 52)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(153, 52)
    Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem2.TextToControlDistance = 0
    Me.LayoutControlItem2.TextVisible = False
    '
    'LayoutControlItem4
    '
    Me.LayoutControlItem4.Control = Me.btnSelect
    resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
    Me.LayoutControlItem4.Location = New System.Drawing.Point(459, 521)
    Me.LayoutControlItem4.MaxSize = New System.Drawing.Size(0, 52)
    Me.LayoutControlItem4.MinSize = New System.Drawing.Size(70, 52)
    Me.LayoutControlItem4.Name = "LayoutControlItem4"
    Me.LayoutControlItem4.Size = New System.Drawing.Size(149, 52)
    Me.LayoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem4.TextToControlDistance = 0
    Me.LayoutControlItem4.TextVisible = False
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.btnExit
    resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
    Me.LayoutControlItem3.Location = New System.Drawing.Point(608, 521)
    Me.LayoutControlItem3.MaxSize = New System.Drawing.Size(0, 52)
    Me.LayoutControlItem3.MinSize = New System.Drawing.Size(60, 52)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(147, 52)
    Me.LayoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem3.TextToControlDistance = 0
    Me.LayoutControlItem3.TextVisible = False
    '
    'LayoutControlItem5
    '
    Me.LayoutControlItem5.Control = Me.btnRefresh
    resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
    Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 521)
    Me.LayoutControlItem5.Name = "LayoutControlItem5"
    Me.LayoutControlItem5.Size = New System.Drawing.Size(151, 52)
    Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem5.TextToControlDistance = 0
    Me.LayoutControlItem5.TextVisible = False
    '
    'LayoutControlItem6
    '
    Me.LayoutControlItem6.Control = Me.btnPrint
    resources.ApplyResources(Me.LayoutControlItem6, "LayoutControlItem6")
    Me.LayoutControlItem6.Location = New System.Drawing.Point(304, 521)
    Me.LayoutControlItem6.Name = "LayoutControlItem6"
    Me.LayoutControlItem6.Size = New System.Drawing.Size(155, 52)
    Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem6.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem6.TextToControlDistance = 0
    Me.LayoutControlItem6.TextVisible = False
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
    Me.PrintableComponentLink1.Component = Me.VehicleListGridControl
    Me.PrintableComponentLink1.CustomPaperSize = New System.Drawing.Size(0, 0)
    Me.PrintableComponentLink1.ImageStream = CType(resources.GetObject("PrintableComponentLink1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
    Me.PrintableComponentLink1.Landscape = True
    Me.PrintableComponentLink1.Margins = New System.Drawing.Printing.Margins(30, 30, 30, 30)
    Me.PrintableComponentLink1.PaperKind = System.Drawing.Printing.PaperKind.A4
    Me.PrintableComponentLink1.PrintingSystem = Me.PrintingSystem1
    Me.PrintableComponentLink1.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart
    '
    'uxVehicleList
    '
    Me.AccessibleDescription = Nothing
    Me.AccessibleName = Nothing
    resources.ApplyResources(Me, "$this")
    Me.BackgroundImage = Nothing
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "uxVehicleList"
    Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.VehicleListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.VehicleListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.HyperLinkEditCustomer, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.HyperLinkEditShelNum, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.HyperLinkEditShellNumber, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.rideLastRegDate.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.rideLastRegDate, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.HyperLinkEditRegNumber, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.HyperLinkEditRegBr, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents btnSelect As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnAddNew As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents VehicleListGridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents VehicleListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents HyperLinkEditShellNumber As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents rideLastRegDate As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents HyperLinkEditRegNumber As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents colCustomerSurname As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerFirstName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomer As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMB As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colShellNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colModelName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents HyperLinkEditCustomer As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents HyperLinkEditShelNum As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents HyperLinkEditRegBr As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents btnRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents colVehiceMaker As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLastRegistratinNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btnPrint As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents PrintingSystem1 As DevExpress.XtraPrinting.PrintingSystem
    Friend WithEvents PrintableComponentLink1 As DevExpress.XtraPrinting.PrintableComponentLink

End Class
