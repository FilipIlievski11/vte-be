<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxVehicleModels
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxVehicleModels))
        Me.GridView3 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId2 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdVehicleModel1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colTireType1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDimenzions1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colSeria1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNote1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.VehicleModelsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdVehicleMaker = New DevExpress.XtraGrid.Columns.GridColumn
        Me.LookUpEditVehicleMakers = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.VehicleMakerListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.colModelCode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colModelName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colYearOfBeginingProduction = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemDateEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
        Me.colYearOfEndingProduction = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemDateEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
        Me.colYearOfProduction = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colYearOfBeginingProductionString = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemTextEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
        Me.colYearOfEndingProductionString = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemTextEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
        Me.LookUpEditBodytype = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.VehicleBodytypeListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
        Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
        Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        CType(Me.GridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleModelsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LookUpEditVehicleMakers, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleMakerListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit2.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemTextEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LookUpEditBodytype, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleBodytypeListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GridView3
        '
        Me.GridView3.Appearance.HeaderPanel.Options.UseTextOptions = True
        Me.GridView3.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView3.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView3.Appearance.Row.Options.UseTextOptions = True
        Me.GridView3.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView3.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        resources.ApplyResources(Me.GridView3, "GridView3")
        Me.GridView3.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId2, Me.colIdVehicleModel1, Me.colTireType1, Me.colDimenzions1, Me.colSeria1, Me.colNote1})
        Me.GridView3.GridControl = Me.GridControl1
        Me.GridView3.Name = "GridView3"
        '
        'colId2
        '
        resources.ApplyResources(Me.colId2, "colId2")
        Me.colId2.FieldName = "Id"
        Me.colId2.Name = "colId2"
        Me.colId2.OptionsColumn.ReadOnly = True
        '
        'colIdVehicleModel1
        '
        resources.ApplyResources(Me.colIdVehicleModel1, "colIdVehicleModel1")
        Me.colIdVehicleModel1.FieldName = "IdVehicleModel"
        Me.colIdVehicleModel1.Name = "colIdVehicleModel1"
        '
        'colTireType1
        '
        resources.ApplyResources(Me.colTireType1, "colTireType1")
        Me.colTireType1.FieldName = "TireType"
        Me.colTireType1.Name = "colTireType1"
        '
        'colDimenzions1
        '
        resources.ApplyResources(Me.colDimenzions1, "colDimenzions1")
        Me.colDimenzions1.FieldName = "Dimenzions"
        Me.colDimenzions1.Name = "colDimenzions1"
        '
        'colSeria1
        '
        resources.ApplyResources(Me.colSeria1, "colSeria1")
        Me.colSeria1.FieldName = "Seria"
        Me.colSeria1.Name = "colSeria1"
        '
        'colNote1
        '
        resources.ApplyResources(Me.colNote1, "colNote1")
        Me.colNote1.FieldName = "Note"
        Me.colNote1.Name = "colNote1"
        '
        'GridControl1
        '
        Me.GridControl1.AccessibleDescription = Nothing
        Me.GridControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.GridControl1, "GridControl1")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.GridControl1, False)
        Me.GridControl1.BackgroundImage = Nothing
        Me.GridControl1.DataSource = Me.VehicleModelsBindingSource
        Me.GridControl1.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.GridControl1.EmbeddedNavigator.AccessibleName = Nothing
        Me.GridControl1.EmbeddedNavigator.Anchor = CType(resources.GetObject("GridControl1.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.GridControl1.EmbeddedNavigator.BackgroundImage = Nothing
        Me.GridControl1.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("GridControl1.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.GridControl1.EmbeddedNavigator.ImeMode = CType(resources.GetObject("GridControl1.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.GridControl1.EmbeddedNavigator.TextLocation = CType(resources.GetObject("GridControl1.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.GridControl1.EmbeddedNavigator.ToolTip = resources.GetString("GridControl1.EmbeddedNavigator.ToolTip")
        Me.GridControl1.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("GridControl1.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.GridControl1.EmbeddedNavigator.ToolTipTitle = resources.GetString("GridControl1.EmbeddedNavigator.ToolTipTitle")
        Me.GridControl1.Font = Nothing
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.LookUpEditVehicleMakers, Me.LookUpEditBodytype, Me.RepositoryItemDateEdit1, Me.RepositoryItemDateEdit2, Me.RepositoryItemTextEdit1, Me.RepositoryItemTextEdit2})
        Me.GridControl1.UseEmbeddedNavigator = True
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView3})
        '
        'VehicleModelsBindingSource
        '
        Me.VehicleModelsBindingSource.DataSource = GetType(VTE.Library.VehicleModels)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.VehicleModelsBindingSource, False)
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
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdVehicleMaker, Me.colModelCode, Me.colModelName, Me.colYearOfBeginingProduction, Me.colYearOfEndingProduction, Me.colYearOfProduction, Me.colYearOfBeginingProductionString, Me.colYearOfEndingProductionString})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsNavigation.AutoFocusNewRow = True
        Me.GridView1.OptionsNavigation.EnterMoveNextColumn = True
        Me.GridView1.OptionsView.ShowAutoFilterRow = True
        '
        'colId
        '
        resources.ApplyResources(Me.colId, "colId")
        Me.colId.FieldName = "Id"
        Me.colId.Name = "colId"
        Me.colId.OptionsColumn.ReadOnly = True
        '
        'colIdVehicleMaker
        '
        resources.ApplyResources(Me.colIdVehicleMaker, "colIdVehicleMaker")
        Me.colIdVehicleMaker.ColumnEdit = Me.LookUpEditVehicleMakers
        Me.colIdVehicleMaker.FieldName = "IdVehicleMaker"
        Me.colIdVehicleMaker.Name = "colIdVehicleMaker"
        '
        'LookUpEditVehicleMakers
        '
        Me.LookUpEditVehicleMakers.AccessibleDescription = Nothing
        Me.LookUpEditVehicleMakers.AccessibleName = Nothing
        resources.ApplyResources(Me.LookUpEditVehicleMakers, "LookUpEditVehicleMakers")
        Me.LookUpEditVehicleMakers.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditVehicleMakers.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditVehicleMakers.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditVehicleMakers.Buttons2"), CType(resources.GetObject("LookUpEditVehicleMakers.Buttons3"), Integer), CType(resources.GetObject("LookUpEditVehicleMakers.Buttons4"), Boolean), CType(resources.GetObject("LookUpEditVehicleMakers.Buttons5"), Boolean), CType(resources.GetObject("LookUpEditVehicleMakers.Buttons6"), Boolean), CType(resources.GetObject("LookUpEditVehicleMakers.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing)})
        Me.LookUpEditVehicleMakers.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdCountry", "IdCountry", 55, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CompanyName", "CompanyName", 78, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CompanyTrademark", "CompanyTrademark", 102, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CompanyNameAndTrademark", "Компанија : Трговска марка", 148, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near)})
        Me.LookUpEditVehicleMakers.DataSource = Me.VehicleMakerListBindingSource
        Me.LookUpEditVehicleMakers.DisplayMember = "CompanyNameAndTrademark"
        Me.LookUpEditVehicleMakers.Name = "LookUpEditVehicleMakers"
        Me.LookUpEditVehicleMakers.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.LookUpEditVehicleMakers.ValueMember = "Id"
        '
        'VehicleMakerListBindingSource
        '
        Me.VehicleMakerListBindingSource.DataSource = GetType(VTE.Library.VehicleMakerList)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.VehicleMakerListBindingSource, False)
        '
        'colModelCode
        '
        resources.ApplyResources(Me.colModelCode, "colModelCode")
        Me.colModelCode.FieldName = "ModelCode"
        Me.colModelCode.Name = "colModelCode"
        '
        'colModelName
        '
        resources.ApplyResources(Me.colModelName, "colModelName")
        Me.colModelName.FieldName = "ModelName"
        Me.colModelName.Name = "colModelName"
        '
        'colYearOfBeginingProduction
        '
        resources.ApplyResources(Me.colYearOfBeginingProduction, "colYearOfBeginingProduction")
        Me.colYearOfBeginingProduction.ColumnEdit = Me.RepositoryItemDateEdit1
        Me.colYearOfBeginingProduction.DisplayFormat.FormatString = "yyyy"
        Me.colYearOfBeginingProduction.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.colYearOfBeginingProduction.FieldName = "YearOfBeginingProduction"
        Me.colYearOfBeginingProduction.Name = "colYearOfBeginingProduction"
        '
        'RepositoryItemDateEdit1
        '
        Me.RepositoryItemDateEdit1.AccessibleDescription = Nothing
        Me.RepositoryItemDateEdit1.AccessibleName = Nothing
        resources.ApplyResources(Me.RepositoryItemDateEdit1, "RepositoryItemDateEdit1")
        Me.RepositoryItemDateEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemDateEdit1.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.RepositoryItemDateEdit1.DisplayFormat.FormatString = "yyyy"
        Me.RepositoryItemDateEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.RepositoryItemDateEdit1.EditFormat.FormatString = "yyyy"
        Me.RepositoryItemDateEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.RepositoryItemDateEdit1.Mask.AutoComplete = CType(resources.GetObject("RepositoryItemDateEdit1.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.RepositoryItemDateEdit1.Mask.BeepOnError = CType(resources.GetObject("RepositoryItemDateEdit1.Mask.BeepOnError"), Boolean)
        Me.RepositoryItemDateEdit1.Mask.EditMask = resources.GetString("RepositoryItemDateEdit1.Mask.EditMask")
        Me.RepositoryItemDateEdit1.Mask.IgnoreMaskBlank = CType(resources.GetObject("RepositoryItemDateEdit1.Mask.IgnoreMaskBlank"), Boolean)
        Me.RepositoryItemDateEdit1.Mask.MaskType = CType(resources.GetObject("RepositoryItemDateEdit1.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.RepositoryItemDateEdit1.Mask.PlaceHolder = CType(resources.GetObject("RepositoryItemDateEdit1.Mask.PlaceHolder"), Char)
        Me.RepositoryItemDateEdit1.Mask.SaveLiteral = CType(resources.GetObject("RepositoryItemDateEdit1.Mask.SaveLiteral"), Boolean)
        Me.RepositoryItemDateEdit1.Mask.ShowPlaceHolders = CType(resources.GetObject("RepositoryItemDateEdit1.Mask.ShowPlaceHolders"), Boolean)
        Me.RepositoryItemDateEdit1.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("RepositoryItemDateEdit1.Mask.UseMaskAsDisplayFormat"), Boolean)
        Me.RepositoryItemDateEdit1.Name = "RepositoryItemDateEdit1"
        Me.RepositoryItemDateEdit1.NullDate = Global.WinApp.My.Resources.Resources.String1
        Me.RepositoryItemDateEdit1.VistaTimeProperties.AccessibleDescription = Nothing
        Me.RepositoryItemDateEdit1.VistaTimeProperties.AccessibleName = Nothing
        Me.RepositoryItemDateEdit1.VistaTimeProperties.AutoHeight = CType(resources.GetObject("RepositoryItemDateEdit1.VistaTimeProperties.AutoHeight"), Boolean)
        Me.RepositoryItemDateEdit1.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.RepositoryItemDateEdit1.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("RepositoryItemDateEdit1.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.RepositoryItemDateEdit1.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("RepositoryItemDateEdit1.VistaTimeProperties.Mask.BeepOnError"), Boolean)
        Me.RepositoryItemDateEdit1.VistaTimeProperties.Mask.EditMask = resources.GetString("RepositoryItemDateEdit1.VistaTimeProperties.Mask.EditMask")
        Me.RepositoryItemDateEdit1.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("RepositoryItemDateEdit1.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
        Me.RepositoryItemDateEdit1.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("RepositoryItemDateEdit1.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.RepositoryItemDateEdit1.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("RepositoryItemDateEdit1.VistaTimeProperties.Mask.PlaceHolder"), Char)
        Me.RepositoryItemDateEdit1.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("RepositoryItemDateEdit1.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
        Me.RepositoryItemDateEdit1.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("RepositoryItemDateEdit1.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
        Me.RepositoryItemDateEdit1.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("RepositoryItemDateEdit1.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
        '
        'colYearOfEndingProduction
        '
        resources.ApplyResources(Me.colYearOfEndingProduction, "colYearOfEndingProduction")
        Me.colYearOfEndingProduction.ColumnEdit = Me.RepositoryItemDateEdit2
        Me.colYearOfEndingProduction.FieldName = "YearOfEndingProduction"
        Me.colYearOfEndingProduction.Name = "colYearOfEndingProduction"
        '
        'RepositoryItemDateEdit2
        '
        Me.RepositoryItemDateEdit2.AccessibleDescription = Nothing
        Me.RepositoryItemDateEdit2.AccessibleName = Nothing
        resources.ApplyResources(Me.RepositoryItemDateEdit2, "RepositoryItemDateEdit2")
        Me.RepositoryItemDateEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemDateEdit2.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.RepositoryItemDateEdit2.DisplayFormat.FormatString = "yyyy"
        Me.RepositoryItemDateEdit2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.RepositoryItemDateEdit2.EditFormat.FormatString = "yyyy"
        Me.RepositoryItemDateEdit2.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.RepositoryItemDateEdit2.Mask.AutoComplete = CType(resources.GetObject("RepositoryItemDateEdit2.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.RepositoryItemDateEdit2.Mask.BeepOnError = CType(resources.GetObject("RepositoryItemDateEdit2.Mask.BeepOnError"), Boolean)
        Me.RepositoryItemDateEdit2.Mask.EditMask = resources.GetString("RepositoryItemDateEdit2.Mask.EditMask")
        Me.RepositoryItemDateEdit2.Mask.IgnoreMaskBlank = CType(resources.GetObject("RepositoryItemDateEdit2.Mask.IgnoreMaskBlank"), Boolean)
        Me.RepositoryItemDateEdit2.Mask.MaskType = CType(resources.GetObject("RepositoryItemDateEdit2.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.RepositoryItemDateEdit2.Mask.PlaceHolder = CType(resources.GetObject("RepositoryItemDateEdit2.Mask.PlaceHolder"), Char)
        Me.RepositoryItemDateEdit2.Mask.SaveLiteral = CType(resources.GetObject("RepositoryItemDateEdit2.Mask.SaveLiteral"), Boolean)
        Me.RepositoryItemDateEdit2.Mask.ShowPlaceHolders = CType(resources.GetObject("RepositoryItemDateEdit2.Mask.ShowPlaceHolders"), Boolean)
        Me.RepositoryItemDateEdit2.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("RepositoryItemDateEdit2.Mask.UseMaskAsDisplayFormat"), Boolean)
        Me.RepositoryItemDateEdit2.Name = "RepositoryItemDateEdit2"
        Me.RepositoryItemDateEdit2.NullDate = Global.WinApp.My.Resources.Resources.String1
        Me.RepositoryItemDateEdit2.VistaTimeProperties.AccessibleDescription = Nothing
        Me.RepositoryItemDateEdit2.VistaTimeProperties.AccessibleName = Nothing
        Me.RepositoryItemDateEdit2.VistaTimeProperties.AutoHeight = CType(resources.GetObject("RepositoryItemDateEdit2.VistaTimeProperties.AutoHeight"), Boolean)
        Me.RepositoryItemDateEdit2.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.RepositoryItemDateEdit2.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("RepositoryItemDateEdit2.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.RepositoryItemDateEdit2.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("RepositoryItemDateEdit2.VistaTimeProperties.Mask.BeepOnError"), Boolean)
        Me.RepositoryItemDateEdit2.VistaTimeProperties.Mask.EditMask = resources.GetString("RepositoryItemDateEdit2.VistaTimeProperties.Mask.EditMask")
        Me.RepositoryItemDateEdit2.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("RepositoryItemDateEdit2.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
        Me.RepositoryItemDateEdit2.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("RepositoryItemDateEdit2.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.RepositoryItemDateEdit2.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("RepositoryItemDateEdit2.VistaTimeProperties.Mask.PlaceHolder"), Char)
        Me.RepositoryItemDateEdit2.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("RepositoryItemDateEdit2.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
        Me.RepositoryItemDateEdit2.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("RepositoryItemDateEdit2.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
        Me.RepositoryItemDateEdit2.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("RepositoryItemDateEdit2.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
        '
        'colYearOfProduction
        '
        resources.ApplyResources(Me.colYearOfProduction, "colYearOfProduction")
        Me.colYearOfProduction.FieldName = "YearOfProduction"
        Me.colYearOfProduction.Name = "colYearOfProduction"
        '
        'colYearOfBeginingProductionString
        '
        resources.ApplyResources(Me.colYearOfBeginingProductionString, "colYearOfBeginingProductionString")
        Me.colYearOfBeginingProductionString.ColumnEdit = Me.RepositoryItemTextEdit1
        Me.colYearOfBeginingProductionString.FieldName = "YearOfBeginingProductionString"
        Me.colYearOfBeginingProductionString.Name = "colYearOfBeginingProductionString"
        Me.colYearOfBeginingProductionString.OptionsColumn.FixedWidth = True
        '
        'RepositoryItemTextEdit1
        '
        Me.RepositoryItemTextEdit1.AccessibleDescription = Nothing
        Me.RepositoryItemTextEdit1.AccessibleName = Nothing
        resources.ApplyResources(Me.RepositoryItemTextEdit1, "RepositoryItemTextEdit1")
        Me.RepositoryItemTextEdit1.Mask.AutoComplete = CType(resources.GetObject("RepositoryItemTextEdit1.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.RepositoryItemTextEdit1.Mask.BeepOnError = CType(resources.GetObject("RepositoryItemTextEdit1.Mask.BeepOnError"), Boolean)
        Me.RepositoryItemTextEdit1.Mask.EditMask = resources.GetString("RepositoryItemTextEdit1.Mask.EditMask")
        Me.RepositoryItemTextEdit1.Mask.IgnoreMaskBlank = CType(resources.GetObject("RepositoryItemTextEdit1.Mask.IgnoreMaskBlank"), Boolean)
        Me.RepositoryItemTextEdit1.Mask.MaskType = CType(resources.GetObject("RepositoryItemTextEdit1.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.RepositoryItemTextEdit1.Mask.PlaceHolder = CType(resources.GetObject("RepositoryItemTextEdit1.Mask.PlaceHolder"), Char)
        Me.RepositoryItemTextEdit1.Mask.SaveLiteral = CType(resources.GetObject("RepositoryItemTextEdit1.Mask.SaveLiteral"), Boolean)
        Me.RepositoryItemTextEdit1.Mask.ShowPlaceHolders = CType(resources.GetObject("RepositoryItemTextEdit1.Mask.ShowPlaceHolders"), Boolean)
        Me.RepositoryItemTextEdit1.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("RepositoryItemTextEdit1.Mask.UseMaskAsDisplayFormat"), Boolean)
        Me.RepositoryItemTextEdit1.Name = "RepositoryItemTextEdit1"
        '
        'colYearOfEndingProductionString
        '
        resources.ApplyResources(Me.colYearOfEndingProductionString, "colYearOfEndingProductionString")
        Me.colYearOfEndingProductionString.ColumnEdit = Me.RepositoryItemTextEdit2
        Me.colYearOfEndingProductionString.FieldName = "YearOfEndingProductionString"
        Me.colYearOfEndingProductionString.Name = "colYearOfEndingProductionString"
        Me.colYearOfEndingProductionString.OptionsColumn.FixedWidth = True
        '
        'RepositoryItemTextEdit2
        '
        Me.RepositoryItemTextEdit2.AccessibleDescription = Nothing
        Me.RepositoryItemTextEdit2.AccessibleName = Nothing
        resources.ApplyResources(Me.RepositoryItemTextEdit2, "RepositoryItemTextEdit2")
        Me.RepositoryItemTextEdit2.Mask.AutoComplete = CType(resources.GetObject("RepositoryItemTextEdit2.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.RepositoryItemTextEdit2.Mask.BeepOnError = CType(resources.GetObject("RepositoryItemTextEdit2.Mask.BeepOnError"), Boolean)
        Me.RepositoryItemTextEdit2.Mask.EditMask = resources.GetString("RepositoryItemTextEdit2.Mask.EditMask")
        Me.RepositoryItemTextEdit2.Mask.IgnoreMaskBlank = CType(resources.GetObject("RepositoryItemTextEdit2.Mask.IgnoreMaskBlank"), Boolean)
        Me.RepositoryItemTextEdit2.Mask.MaskType = CType(resources.GetObject("RepositoryItemTextEdit2.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.RepositoryItemTextEdit2.Mask.PlaceHolder = CType(resources.GetObject("RepositoryItemTextEdit2.Mask.PlaceHolder"), Char)
        Me.RepositoryItemTextEdit2.Mask.SaveLiteral = CType(resources.GetObject("RepositoryItemTextEdit2.Mask.SaveLiteral"), Boolean)
        Me.RepositoryItemTextEdit2.Mask.ShowPlaceHolders = CType(resources.GetObject("RepositoryItemTextEdit2.Mask.ShowPlaceHolders"), Boolean)
        Me.RepositoryItemTextEdit2.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("RepositoryItemTextEdit2.Mask.UseMaskAsDisplayFormat"), Boolean)
        Me.RepositoryItemTextEdit2.Name = "RepositoryItemTextEdit2"
        '
        'LookUpEditBodytype
        '
        Me.LookUpEditBodytype.AccessibleDescription = Nothing
        Me.LookUpEditBodytype.AccessibleName = Nothing
        resources.ApplyResources(Me.LookUpEditBodytype, "LookUpEditBodytype")
        Me.LookUpEditBodytype.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditBodytype.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditBodytype.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditBodytype.Buttons2"), CType(resources.GetObject("LookUpEditBodytype.Buttons3"), Integer), CType(resources.GetObject("LookUpEditBodytype.Buttons4"), Boolean), CType(resources.GetObject("LookUpEditBodytype.Buttons5"), Boolean), CType(resources.GetObject("LookUpEditBodytype.Buttons6"), Boolean), CType(resources.GetObject("LookUpEditBodytype.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing)})
        Me.LookUpEditBodytype.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("BodytypeCode", "BodytypeCode", 77, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("BodytypeDescriprion", "BodytypeDescriprion", 105, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("BodytypeFull", "BodytypeFull", 68, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near)})
        Me.LookUpEditBodytype.DataSource = Me.VehicleBodytypeListBindingSource
        Me.LookUpEditBodytype.DisplayMember = "BodytypeFull"
        Me.LookUpEditBodytype.Name = "LookUpEditBodytype"
        Me.LookUpEditBodytype.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.LookUpEditBodytype.ValueMember = "Id"
        '
        'VehicleBodytypeListBindingSource
        '
        Me.VehicleBodytypeListBindingSource.DataSource = GetType(VTE.Library.VehicleBodytypeList)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.VehicleBodytypeListBindingSource, False)
        '
        'LayoutControl1
        '
        Me.LayoutControl1.AccessibleDescription = Nothing
        Me.LayoutControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LayoutControl1, False)
        Me.LayoutControl1.BackgroundImage = Nothing
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
        Me.LayoutControl1.Font = Nothing
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        '
        'UxKopcinja1
        '
        Me.UxKopcinja1.AccessibleDescription = Nothing
        Me.UxKopcinja1.AccessibleName = Nothing
        resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UxKopcinja1, False)
        Me.UxKopcinja1.BackgroundImage = Nothing
        Me.UxKopcinja1.Name = "UxKopcinja1"
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.SplitterItem1})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(750, 613)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.UxKopcinja1
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(748, 85)
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.GridControl1
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 91)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(748, 520)
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'SplitterItem1
        '
        resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
        Me.SplitterItem1.Location = New System.Drawing.Point(0, 85)
        Me.SplitterItem1.Name = "SplitterItem1"
        Me.SplitterItem1.Size = New System.Drawing.Size(748, 6)
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        '
        'uxVehicleModels
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "uxVehicleModels"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.GridView3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleModelsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LookUpEditVehicleMakers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleMakerListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit2.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemTextEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LookUpEditBodytype, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleBodytypeListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents VehicleModelsBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdVehicleMaker As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents LookUpEditVehicleMakers As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents colModelName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colYearOfBeginingProduction As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colYearOfEndingProduction As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents LookUpEditBodytype As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents VehicleMakerListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents VehicleBodytypeListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView3 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId2 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdVehicleModel1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colTireType1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents RepositoryItemDateEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
  Friend WithEvents RepositoryItemDateEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
  Friend WithEvents colDimenzions1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colSeria1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colNote1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colYearOfProduction As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colModelCode As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colYearOfBeginingProductionString As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents RepositoryItemTextEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
  Friend WithEvents colYearOfEndingProductionString As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents RepositoryItemTextEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit

End Class
