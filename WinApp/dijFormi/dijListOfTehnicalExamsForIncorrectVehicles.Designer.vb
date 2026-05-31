<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijListOfTehnicalExamsForIncorrectVehicles
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
  Me.components = New System.ComponentModel.Container
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dijListOfTehnicalExamsForIncorrectVehicles))
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.btnRefresh = New DevExpress.XtraEditors.SimpleButton
  Me.DateEdit2 = New DevExpress.XtraEditors.DateEdit
  Me.DateEdit1 = New DevExpress.XtraEditors.DateEdit
  Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
  Me.btnExit = New DevExpress.XtraEditors.SimpleButton
  Me.btnSelect = New DevExpress.XtraEditors.SimpleButton
  Me.DocumentsTehnicalExamsReportListGridControl = New DevExpress.XtraGrid.GridControl
  Me.DocumentsTehnicalExamsReportListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.HyperLinkEditId = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
  Me.colMadeDate = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colValidTillDate = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIdOrganizationForTehnicalExam = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIdFirsControler = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIdSecondControler = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colVehicleIsRight = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCustomerSurname = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCustomerFirstName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCustomerName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.HyperLinkCustomerName = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
  Me.colIsCompany = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colShellNumber = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colEngineNumber = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colExplanationNote = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colDriversWarning = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIsSocialNotPrivate = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colForPrivateTransportNotPublic = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIdTypeOfTehnicalExam = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIdCustomerVehicleRelation = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIdVehicle = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colLastRegistration = New DevExpress.XtraGrid.Columns.GridColumn
  Me.HyperLinkRegNumber = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
  Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.LayoutControl1.SuspendLayout()
  CType(Me.DateEdit2.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DateEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DateEdit1.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DocumentsTehnicalExamsReportListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DocumentsTehnicalExamsReportListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.HyperLinkEditId, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.HyperLinkCustomerName, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.HyperLinkRegNumber, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'LayoutControl1
  '
  Me.LayoutControl1.AccessibleDescription = Nothing
  Me.LayoutControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
  Me.LayoutControl1.Appearance.DisabledLayoutGroupCaption.ForeColor = System.Drawing.SystemColors.GrayText
  Me.LayoutControl1.Appearance.DisabledLayoutGroupCaption.Options.UseForeColor = True
  Me.LayoutControl1.Appearance.DisabledLayoutItem.ForeColor = System.Drawing.SystemColors.GrayText
  Me.LayoutControl1.Appearance.DisabledLayoutItem.Options.UseForeColor = True
  Me.LayoutControl1.BackgroundImage = Nothing
  Me.LayoutControl1.Controls.Add(Me.btnRefresh)
  Me.LayoutControl1.Controls.Add(Me.DateEdit2)
  Me.LayoutControl1.Controls.Add(Me.DateEdit1)
  Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
  Me.LayoutControl1.Controls.Add(Me.btnExit)
  Me.LayoutControl1.Controls.Add(Me.btnSelect)
  Me.LayoutControl1.Controls.Add(Me.DocumentsTehnicalExamsReportListGridControl)
  Me.LayoutControl1.Font = Nothing
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
  '
  'btnRefresh
  '
  Me.btnRefresh.AccessibleDescription = Nothing
  Me.btnRefresh.AccessibleName = Nothing
  resources.ApplyResources(Me.btnRefresh, "btnRefresh")
  Me.btnRefresh.BackgroundImage = Nothing
  Me.btnRefresh.Name = "btnRefresh"
  Me.btnRefresh.StyleController = Me.LayoutControl1
  '
  'DateEdit2
  '
  resources.ApplyResources(Me.DateEdit2, "DateEdit2")
  Me.DateEdit2.BackgroundImage = Nothing
  Me.DateEdit2.EditValue = Nothing
  Me.DateEdit2.Name = "DateEdit2"
  Me.DateEdit2.Properties.AccessibleDescription = Nothing
  Me.DateEdit2.Properties.AccessibleName = Nothing
  Me.DateEdit2.Properties.AutoHeight = CType(resources.GetObject("DateEdit2.Properties.AutoHeight"), Boolean)
  Me.DateEdit2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("DateEdit2.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
  Me.DateEdit2.Properties.Mask.AutoComplete = CType(resources.GetObject("DateEdit2.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.DateEdit2.Properties.Mask.BeepOnError = CType(resources.GetObject("DateEdit2.Properties.Mask.BeepOnError"), Boolean)
  Me.DateEdit2.Properties.Mask.EditMask = resources.GetString("DateEdit2.Properties.Mask.EditMask")
  Me.DateEdit2.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DateEdit2.Properties.Mask.IgnoreMaskBlank"), Boolean)
  Me.DateEdit2.Properties.Mask.MaskType = CType(resources.GetObject("DateEdit2.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.DateEdit2.Properties.Mask.PlaceHolder = CType(resources.GetObject("DateEdit2.Properties.Mask.PlaceHolder"), Char)
  Me.DateEdit2.Properties.Mask.SaveLiteral = CType(resources.GetObject("DateEdit2.Properties.Mask.SaveLiteral"), Boolean)
  Me.DateEdit2.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("DateEdit2.Properties.Mask.ShowPlaceHolders"), Boolean)
  Me.DateEdit2.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("DateEdit2.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.DateEdit2.Properties.VistaTimeProperties.AccessibleDescription = Nothing
  Me.DateEdit2.Properties.VistaTimeProperties.AccessibleName = Nothing
  Me.DateEdit2.Properties.VistaTimeProperties.AutoHeight = CType(resources.GetObject("DateEdit2.Properties.VistaTimeProperties.AutoHeight"), Boolean)
  Me.DateEdit2.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
  Me.DateEdit2.Properties.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("DateEdit2.Properties.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.DateEdit2.Properties.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("DateEdit2.Properties.VistaTimeProperties.Mask.BeepOnError"), Boolean)
  Me.DateEdit2.Properties.VistaTimeProperties.Mask.EditMask = resources.GetString("DateEdit2.Properties.VistaTimeProperties.Mask.EditMask")
  Me.DateEdit2.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DateEdit2.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
  Me.DateEdit2.Properties.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("DateEdit2.Properties.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.DateEdit2.Properties.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("DateEdit2.Properties.VistaTimeProperties.Mask.PlaceHolder"), Char)
  Me.DateEdit2.Properties.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("DateEdit2.Properties.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
  Me.DateEdit2.Properties.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("DateEdit2.Properties.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
  Me.DateEdit2.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("DateEdit2.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.DateEdit2.StyleController = Me.LayoutControl1
  '
  'DateEdit1
  '
  resources.ApplyResources(Me.DateEdit1, "DateEdit1")
  Me.DateEdit1.BackgroundImage = Nothing
  Me.DateEdit1.EditValue = Nothing
  Me.DateEdit1.Name = "DateEdit1"
  Me.DateEdit1.Properties.AccessibleDescription = Nothing
  Me.DateEdit1.Properties.AccessibleName = Nothing
  Me.DateEdit1.Properties.AutoHeight = CType(resources.GetObject("DateEdit1.Properties.AutoHeight"), Boolean)
  Me.DateEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("DateEdit1.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
  Me.DateEdit1.Properties.Mask.AutoComplete = CType(resources.GetObject("DateEdit1.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.DateEdit1.Properties.Mask.BeepOnError = CType(resources.GetObject("DateEdit1.Properties.Mask.BeepOnError"), Boolean)
  Me.DateEdit1.Properties.Mask.EditMask = resources.GetString("DateEdit1.Properties.Mask.EditMask")
  Me.DateEdit1.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DateEdit1.Properties.Mask.IgnoreMaskBlank"), Boolean)
  Me.DateEdit1.Properties.Mask.MaskType = CType(resources.GetObject("DateEdit1.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.DateEdit1.Properties.Mask.PlaceHolder = CType(resources.GetObject("DateEdit1.Properties.Mask.PlaceHolder"), Char)
  Me.DateEdit1.Properties.Mask.SaveLiteral = CType(resources.GetObject("DateEdit1.Properties.Mask.SaveLiteral"), Boolean)
  Me.DateEdit1.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("DateEdit1.Properties.Mask.ShowPlaceHolders"), Boolean)
  Me.DateEdit1.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("DateEdit1.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.DateEdit1.Properties.VistaTimeProperties.AccessibleDescription = Nothing
  Me.DateEdit1.Properties.VistaTimeProperties.AccessibleName = Nothing
  Me.DateEdit1.Properties.VistaTimeProperties.AutoHeight = CType(resources.GetObject("DateEdit1.Properties.VistaTimeProperties.AutoHeight"), Boolean)
  Me.DateEdit1.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
  Me.DateEdit1.Properties.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("DateEdit1.Properties.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.DateEdit1.Properties.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("DateEdit1.Properties.VistaTimeProperties.Mask.BeepOnError"), Boolean)
  Me.DateEdit1.Properties.VistaTimeProperties.Mask.EditMask = resources.GetString("DateEdit1.Properties.VistaTimeProperties.Mask.EditMask")
  Me.DateEdit1.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DateEdit1.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
  Me.DateEdit1.Properties.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("DateEdit1.Properties.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.DateEdit1.Properties.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("DateEdit1.Properties.VistaTimeProperties.Mask.PlaceHolder"), Char)
  Me.DateEdit1.Properties.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("DateEdit1.Properties.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
  Me.DateEdit1.Properties.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("DateEdit1.Properties.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
  Me.DateEdit1.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("DateEdit1.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.DateEdit1.StyleController = Me.LayoutControl1
  '
  'SimpleButton1
  '
  Me.SimpleButton1.AccessibleDescription = Nothing
  Me.SimpleButton1.AccessibleName = Nothing
  resources.ApplyResources(Me.SimpleButton1, "SimpleButton1")
  Me.SimpleButton1.BackgroundImage = Nothing
  Me.SimpleButton1.Name = "SimpleButton1"
  Me.SimpleButton1.StyleController = Me.LayoutControl1
  '
  'btnExit
  '
  Me.btnExit.AccessibleDescription = Nothing
  Me.btnExit.AccessibleName = Nothing
  resources.ApplyResources(Me.btnExit, "btnExit")
  Me.btnExit.BackgroundImage = Nothing
  Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
  Me.btnExit.Name = "btnExit"
  Me.btnExit.StyleController = Me.LayoutControl1
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
  'DocumentsTehnicalExamsReportListGridControl
  '
  Me.DocumentsTehnicalExamsReportListGridControl.AccessibleDescription = Nothing
  Me.DocumentsTehnicalExamsReportListGridControl.AccessibleName = Nothing
  resources.ApplyResources(Me.DocumentsTehnicalExamsReportListGridControl, "DocumentsTehnicalExamsReportListGridControl")
  Me.DocumentsTehnicalExamsReportListGridControl.BackgroundImage = Nothing
  Me.DocumentsTehnicalExamsReportListGridControl.DataSource = Me.DocumentsTehnicalExamsReportListBindingSource
  Me.DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
  Me.DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.AccessibleName = Nothing
  Me.DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
  Me.DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.BackgroundImage = Nothing
  Me.DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.BackgroundImageLayo" & _
          "ut"), System.Windows.Forms.ImageLayout)
  Me.DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
  Me.DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
  Me.DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.ToolTip = resources.GetString("DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.ToolTip")
  Me.DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
  Me.DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("DocumentsTehnicalExamsReportListGridControl.EmbeddedNavigator.ToolTipTitle")
  Me.DocumentsTehnicalExamsReportListGridControl.Font = Nothing
  Me.DocumentsTehnicalExamsReportListGridControl.MainView = Me.GridView1
  Me.DocumentsTehnicalExamsReportListGridControl.Name = "DocumentsTehnicalExamsReportListGridControl"
  Me.DocumentsTehnicalExamsReportListGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.HyperLinkEditId, Me.HyperLinkCustomerName, Me.HyperLinkRegNumber})
  Me.DocumentsTehnicalExamsReportListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'DocumentsTehnicalExamsReportListBindingSource
  '
  Me.DocumentsTehnicalExamsReportListBindingSource.DataSource = GetType(VTE.Library.DocumentsTehnicalExamsReportInfo)
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
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colMadeDate, Me.colValidTillDate, Me.colIdOrganizationForTehnicalExam, Me.colIdFirsControler, Me.colIdSecondControler, Me.colVehicleIsRight, Me.colCustomerSurname, Me.colCustomerFirstName, Me.colCustomerName, Me.colIsCompany, Me.colShellNumber, Me.colEngineNumber, Me.colExplanationNote, Me.colDriversWarning, Me.colIsSocialNotPrivate, Me.colForPrivateTransportNotPublic, Me.colIdTypeOfTehnicalExam, Me.colIdCustomerVehicleRelation, Me.colIdVehicle, Me.colLastRegistration})
  Me.GridView1.GridControl = Me.DocumentsTehnicalExamsReportListGridControl
  Me.GridView1.Name = "GridView1"
  Me.GridView1.OptionsFilter.UseNewCustomFilterDialog = True
  Me.GridView1.OptionsView.ShowAutoFilterRow = True
  Me.GridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways
  Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colMadeDate, DevExpress.Data.ColumnSortOrder.Descending)})
  '
  'colId
  '
  resources.ApplyResources(Me.colId, "colId")
  Me.colId.ColumnEdit = Me.HyperLinkEditId
  Me.colId.FieldName = "Id"
  Me.colId.Name = "colId"
  Me.colId.OptionsColumn.ReadOnly = True
  '
  'HyperLinkEditId
  '
  Me.HyperLinkEditId.AccessibleDescription = Nothing
  Me.HyperLinkEditId.AccessibleName = Nothing
  resources.ApplyResources(Me.HyperLinkEditId, "HyperLinkEditId")
  Me.HyperLinkEditId.Mask.AutoComplete = CType(resources.GetObject("HyperLinkEditId.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.HyperLinkEditId.Mask.BeepOnError = CType(resources.GetObject("HyperLinkEditId.Mask.BeepOnError"), Boolean)
  Me.HyperLinkEditId.Mask.EditMask = resources.GetString("HyperLinkEditId.Mask.EditMask")
  Me.HyperLinkEditId.Mask.IgnoreMaskBlank = CType(resources.GetObject("HyperLinkEditId.Mask.IgnoreMaskBlank"), Boolean)
  Me.HyperLinkEditId.Mask.MaskType = CType(resources.GetObject("HyperLinkEditId.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.HyperLinkEditId.Mask.PlaceHolder = CType(resources.GetObject("HyperLinkEditId.Mask.PlaceHolder"), Char)
  Me.HyperLinkEditId.Mask.SaveLiteral = CType(resources.GetObject("HyperLinkEditId.Mask.SaveLiteral"), Boolean)
  Me.HyperLinkEditId.Mask.ShowPlaceHolders = CType(resources.GetObject("HyperLinkEditId.Mask.ShowPlaceHolders"), Boolean)
  Me.HyperLinkEditId.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("HyperLinkEditId.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.HyperLinkEditId.Name = "HyperLinkEditId"
  '
  'colMadeDate
  '
  resources.ApplyResources(Me.colMadeDate, "colMadeDate")
  Me.colMadeDate.FieldName = "MadeDate"
  Me.colMadeDate.Name = "colMadeDate"
  Me.colMadeDate.OptionsColumn.ReadOnly = True
  '
  'colValidTillDate
  '
  resources.ApplyResources(Me.colValidTillDate, "colValidTillDate")
  Me.colValidTillDate.FieldName = "ValidTillDate"
  Me.colValidTillDate.Name = "colValidTillDate"
  Me.colValidTillDate.OptionsColumn.ReadOnly = True
  '
  'colIdOrganizationForTehnicalExam
  '
  resources.ApplyResources(Me.colIdOrganizationForTehnicalExam, "colIdOrganizationForTehnicalExam")
  Me.colIdOrganizationForTehnicalExam.FieldName = "IdOrganizationForTehnicalExam"
  Me.colIdOrganizationForTehnicalExam.Name = "colIdOrganizationForTehnicalExam"
  Me.colIdOrganizationForTehnicalExam.OptionsColumn.ReadOnly = True
  '
  'colIdFirsControler
  '
  resources.ApplyResources(Me.colIdFirsControler, "colIdFirsControler")
  Me.colIdFirsControler.FieldName = "IdFirsControler"
  Me.colIdFirsControler.Name = "colIdFirsControler"
  Me.colIdFirsControler.OptionsColumn.ReadOnly = True
  '
  'colIdSecondControler
  '
  resources.ApplyResources(Me.colIdSecondControler, "colIdSecondControler")
  Me.colIdSecondControler.FieldName = "IdSecondControler"
  Me.colIdSecondControler.Name = "colIdSecondControler"
  Me.colIdSecondControler.OptionsColumn.ReadOnly = True
  '
  'colVehicleIsRight
  '
  resources.ApplyResources(Me.colVehicleIsRight, "colVehicleIsRight")
  Me.colVehicleIsRight.FieldName = "VehicleIsRight"
  Me.colVehicleIsRight.Name = "colVehicleIsRight"
  Me.colVehicleIsRight.OptionsColumn.ReadOnly = True
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
  'colCustomerName
  '
  resources.ApplyResources(Me.colCustomerName, "colCustomerName")
  Me.colCustomerName.ColumnEdit = Me.HyperLinkCustomerName
  Me.colCustomerName.FieldName = "CustomerName"
  Me.colCustomerName.Name = "colCustomerName"
  Me.colCustomerName.OptionsColumn.ReadOnly = True
  '
  'HyperLinkCustomerName
  '
  Me.HyperLinkCustomerName.AccessibleDescription = Nothing
  Me.HyperLinkCustomerName.AccessibleName = Nothing
  resources.ApplyResources(Me.HyperLinkCustomerName, "HyperLinkCustomerName")
  Me.HyperLinkCustomerName.Mask.AutoComplete = CType(resources.GetObject("HyperLinkCustomerName.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.HyperLinkCustomerName.Mask.BeepOnError = CType(resources.GetObject("HyperLinkCustomerName.Mask.BeepOnError"), Boolean)
  Me.HyperLinkCustomerName.Mask.EditMask = resources.GetString("HyperLinkCustomerName.Mask.EditMask")
  Me.HyperLinkCustomerName.Mask.IgnoreMaskBlank = CType(resources.GetObject("HyperLinkCustomerName.Mask.IgnoreMaskBlank"), Boolean)
  Me.HyperLinkCustomerName.Mask.MaskType = CType(resources.GetObject("HyperLinkCustomerName.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.HyperLinkCustomerName.Mask.PlaceHolder = CType(resources.GetObject("HyperLinkCustomerName.Mask.PlaceHolder"), Char)
  Me.HyperLinkCustomerName.Mask.SaveLiteral = CType(resources.GetObject("HyperLinkCustomerName.Mask.SaveLiteral"), Boolean)
  Me.HyperLinkCustomerName.Mask.ShowPlaceHolders = CType(resources.GetObject("HyperLinkCustomerName.Mask.ShowPlaceHolders"), Boolean)
  Me.HyperLinkCustomerName.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("HyperLinkCustomerName.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.HyperLinkCustomerName.Name = "HyperLinkCustomerName"
  '
  'colIsCompany
  '
  resources.ApplyResources(Me.colIsCompany, "colIsCompany")
  Me.colIsCompany.FieldName = "IsCompany"
  Me.colIsCompany.Name = "colIsCompany"
  Me.colIsCompany.OptionsColumn.ReadOnly = True
  '
  'colShellNumber
  '
  resources.ApplyResources(Me.colShellNumber, "colShellNumber")
  Me.colShellNumber.FieldName = "ShellNumber"
  Me.colShellNumber.Name = "colShellNumber"
  Me.colShellNumber.OptionsColumn.AllowEdit = False
  Me.colShellNumber.OptionsColumn.AllowFocus = False
  Me.colShellNumber.OptionsColumn.ReadOnly = True
  '
  'colEngineNumber
  '
  resources.ApplyResources(Me.colEngineNumber, "colEngineNumber")
  Me.colEngineNumber.FieldName = "EngineNumber"
  Me.colEngineNumber.Name = "colEngineNumber"
  Me.colEngineNumber.OptionsColumn.AllowEdit = False
  Me.colEngineNumber.OptionsColumn.AllowFocus = False
  Me.colEngineNumber.OptionsColumn.ReadOnly = True
  '
  'colExplanationNote
  '
  resources.ApplyResources(Me.colExplanationNote, "colExplanationNote")
  Me.colExplanationNote.FieldName = "ExplanationNote"
  Me.colExplanationNote.Name = "colExplanationNote"
  '
  'colDriversWarning
  '
  resources.ApplyResources(Me.colDriversWarning, "colDriversWarning")
  Me.colDriversWarning.FieldName = "DriversWarning"
  Me.colDriversWarning.Name = "colDriversWarning"
  '
  'colIsSocialNotPrivate
  '
  resources.ApplyResources(Me.colIsSocialNotPrivate, "colIsSocialNotPrivate")
  Me.colIsSocialNotPrivate.FieldName = "IsSocialNotPrivate"
  Me.colIsSocialNotPrivate.Name = "colIsSocialNotPrivate"
  '
  'colForPrivateTransportNotPublic
  '
  resources.ApplyResources(Me.colForPrivateTransportNotPublic, "colForPrivateTransportNotPublic")
  Me.colForPrivateTransportNotPublic.FieldName = "ForPrivateTransportNotPublic"
  Me.colForPrivateTransportNotPublic.Name = "colForPrivateTransportNotPublic"
  '
  'colIdTypeOfTehnicalExam
  '
  resources.ApplyResources(Me.colIdTypeOfTehnicalExam, "colIdTypeOfTehnicalExam")
  Me.colIdTypeOfTehnicalExam.FieldName = "IdTypeOfTehnicalExam"
  Me.colIdTypeOfTehnicalExam.Name = "colIdTypeOfTehnicalExam"
  '
  'colIdCustomerVehicleRelation
  '
  resources.ApplyResources(Me.colIdCustomerVehicleRelation, "colIdCustomerVehicleRelation")
  Me.colIdCustomerVehicleRelation.FieldName = "IdCustomerVehicleRelation"
  Me.colIdCustomerVehicleRelation.Name = "colIdCustomerVehicleRelation"
  '
  'colIdVehicle
  '
  resources.ApplyResources(Me.colIdVehicle, "colIdVehicle")
  Me.colIdVehicle.FieldName = "IdVehicle"
  Me.colIdVehicle.Name = "colIdVehicle"
  '
  'colLastRegistration
  '
  resources.ApplyResources(Me.colLastRegistration, "colLastRegistration")
  Me.colLastRegistration.ColumnEdit = Me.HyperLinkRegNumber
  Me.colLastRegistration.FieldName = "LastRegistration"
  Me.colLastRegistration.Name = "colLastRegistration"
  '
  'HyperLinkRegNumber
  '
  Me.HyperLinkRegNumber.AccessibleDescription = Nothing
  Me.HyperLinkRegNumber.AccessibleName = Nothing
  resources.ApplyResources(Me.HyperLinkRegNumber, "HyperLinkRegNumber")
  Me.HyperLinkRegNumber.Mask.AutoComplete = CType(resources.GetObject("HyperLinkRegNumber.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.HyperLinkRegNumber.Mask.BeepOnError = CType(resources.GetObject("HyperLinkRegNumber.Mask.BeepOnError"), Boolean)
  Me.HyperLinkRegNumber.Mask.EditMask = resources.GetString("HyperLinkRegNumber.Mask.EditMask")
  Me.HyperLinkRegNumber.Mask.IgnoreMaskBlank = CType(resources.GetObject("HyperLinkRegNumber.Mask.IgnoreMaskBlank"), Boolean)
  Me.HyperLinkRegNumber.Mask.MaskType = CType(resources.GetObject("HyperLinkRegNumber.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.HyperLinkRegNumber.Mask.PlaceHolder = CType(resources.GetObject("HyperLinkRegNumber.Mask.PlaceHolder"), Char)
  Me.HyperLinkRegNumber.Mask.SaveLiteral = CType(resources.GetObject("HyperLinkRegNumber.Mask.SaveLiteral"), Boolean)
  Me.HyperLinkRegNumber.Mask.ShowPlaceHolders = CType(resources.GetObject("HyperLinkRegNumber.Mask.ShowPlaceHolders"), Boolean)
  Me.HyperLinkRegNumber.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("HyperLinkRegNumber.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.HyperLinkRegNumber.Name = "HyperLinkRegNumber"
  '
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlGroup2})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(992, 666)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.DocumentsTehnicalExamsReportListGridControl
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 57)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(988, 572)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem1.TextToControlDistance = 0
  Me.LayoutControlItem1.TextVisible = False
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.btnSelect
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 629)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(376, 33)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem2.TextToControlDistance = 0
  Me.LayoutControlItem2.TextVisible = False
  '
  'LayoutControlItem3
  '
  Me.LayoutControlItem3.Control = Me.btnExit
  resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
  Me.LayoutControlItem3.Location = New System.Drawing.Point(701, 629)
  Me.LayoutControlItem3.Name = "LayoutControlItem3"
  Me.LayoutControlItem3.Size = New System.Drawing.Size(287, 33)
  Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem3.TextToControlDistance = 0
  Me.LayoutControlItem3.TextVisible = False
  '
  'LayoutControlItem4
  '
  Me.LayoutControlItem4.Control = Me.SimpleButton1
  resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
  Me.LayoutControlItem4.Location = New System.Drawing.Point(376, 629)
  Me.LayoutControlItem4.Name = "LayoutControlItem4"
  Me.LayoutControlItem4.Size = New System.Drawing.Size(325, 33)
  Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem4.TextToControlDistance = 0
  Me.LayoutControlItem4.TextVisible = False
  '
  'LayoutControlGroup2
  '
  resources.ApplyResources(Me.LayoutControlGroup2, "LayoutControlGroup2")
  Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem5, Me.LayoutControlItem6, Me.LayoutControlItem7})
  Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
  Me.LayoutControlGroup2.Size = New System.Drawing.Size(988, 57)
  '
  'LayoutControlItem5
  '
  Me.LayoutControlItem5.Control = Me.DateEdit1
  resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
  Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem5.Name = "LayoutControlItem5"
  Me.LayoutControlItem5.Size = New System.Drawing.Size(246, 33)
  Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem5.TextSize = New System.Drawing.Size(19, 20)
  '
  'LayoutControlItem6
  '
  Me.LayoutControlItem6.Control = Me.DateEdit2
  resources.ApplyResources(Me.LayoutControlItem6, "LayoutControlItem6")
  Me.LayoutControlItem6.Location = New System.Drawing.Point(246, 0)
  Me.LayoutControlItem6.Name = "LayoutControlItem6"
  Me.LayoutControlItem6.Size = New System.Drawing.Size(247, 33)
  Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem6.TextSize = New System.Drawing.Size(19, 20)
  '
  'LayoutControlItem7
  '
  Me.LayoutControlItem7.Control = Me.btnRefresh
  resources.ApplyResources(Me.LayoutControlItem7, "LayoutControlItem7")
  Me.LayoutControlItem7.Location = New System.Drawing.Point(493, 0)
  Me.LayoutControlItem7.Name = "LayoutControlItem7"
  Me.LayoutControlItem7.Size = New System.Drawing.Size(489, 33)
  Me.LayoutControlItem7.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem7.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem7.TextToControlDistance = 0
  Me.LayoutControlItem7.TextVisible = False
  '
  'dijListOfTehnicalExamsForIncorrectVehicles
  '
  Me.AcceptButton = Me.btnSelect
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  resources.ApplyResources(Me, "$this")
  Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
  Me.CancelButton = Me.btnExit
  Me.Controls.Add(Me.LayoutControl1)
  Me.Icon = Nothing
  Me.Name = "dijListOfTehnicalExamsForIncorrectVehicles"
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.DateEdit2.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DateEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DateEdit1.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DocumentsTehnicalExamsReportListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DocumentsTehnicalExamsReportListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.HyperLinkEditId, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.HyperLinkCustomerName, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.HyperLinkRegNumber, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents DocumentsTehnicalExamsReportListGridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents DocumentsTehnicalExamsReportListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMadeDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colValidTillDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdOrganizationForTehnicalExam As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdFirsControler As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdSecondControler As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVehicleIsRight As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerSurname As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerFirstName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIsCompany As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colShellNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colEngineNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnSelect As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents HyperLinkEditId As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents colExplanationNote As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDriversWarning As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIsSocialNotPrivate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colForPrivateTransportNotPublic As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdTypeOfTehnicalExam As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdCustomerVehicleRelation As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdVehicle As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLastRegistration As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents HyperLinkCustomerName As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents HyperLinkRegNumber As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents DateEdit2 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents DateEdit1 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
End Class
