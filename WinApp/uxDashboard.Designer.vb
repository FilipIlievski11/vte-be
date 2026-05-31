<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxDashboard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxDashboard))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.btnRefreshTechReports = New DevExpress.XtraEditors.SimpleButton
        Me.btnRefreshFinances = New DevExpress.XtraEditors.SimpleButton
        Me.btnNewCalculation = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.btnNewInternationalDriLic = New DevExpress.XtraEditors.SimpleButton
        Me.btnNewPermission = New DevExpress.XtraEditors.SimpleButton
        Me.btnNewBill = New DevExpress.XtraEditors.SimpleButton
        Me.btnTehnicalExamPrint = New DevExpress.XtraEditors.SimpleButton
        Me.btnDogovori = New DevExpress.XtraEditors.SimpleButton
        Me.btnPrint = New DevExpress.XtraEditors.SimpleButton
        Me.RequestTypeListTreeList = New DevExpress.XtraTreeList.TreeList
        Me.colIdDocumentPrint1 = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.colIsTehnicalExamRequired = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.colIsPayRequired = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.colIsNewRegistration = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.colIsRelationDeleted = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.colIsVehicleDeleted = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.colIsNewCustomer = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.colIsVehicleChanged = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.colIsCustomerChanged = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.colIsSufficient = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.colTypeName = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.RequestTypeRepositoryItemHyperLinkEdit = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
        Me.colTypeDescription = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.colIsPreviosRegistrationReqired = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.colId2 = New DevExpress.XtraTreeList.Columns.TreeListColumn
        Me.RequestTypeListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.btnCreateCalculation = New DevExpress.XtraEditors.SimpleButton
        Me.btnCancel = New DevExpress.XtraEditors.SimpleButton
        Me.btnEditDocument = New DevExpress.XtraEditors.SimpleButton
        Me.btnApproveRequest = New DevExpress.XtraEditors.SimpleButton
        Me.btnRefreshRequests = New DevExpress.XtraEditors.SimpleButton
        Me.GridControl3 = New DevExpress.XtraGrid.GridControl
        Me.ActiveDocumentsDepListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView3 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colLastRegistrationNumber = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colVehicleDisplay2 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colVehicleModelMaker = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerDisplay2 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colId1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colRequestPrintName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdCustomerVehicleRelation1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdCustomer1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdVehicle1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colMB = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerSurname1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerFirstName1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colShellNumber1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdVehicleModel = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colModelName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdVehicleMaker = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colVehicleMaker = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdOperatorCreated = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemLookUpEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.UsersListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.colIdOperatorModified = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdOperatorEnded = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdTechnicalExamReport = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDateCreated = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDateModified = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDateEnded = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdDocumentPrint = New DevExpress.XtraGrid.Columns.GridColumn
        Me.btnTechnicalExamNew = New DevExpress.XtraEditors.SimpleButton
        Me.btnTehnicalExamDismis = New DevExpress.XtraEditors.SimpleButton
        Me.btnTechnicalExamEdit = New DevExpress.XtraEditors.SimpleButton
        Me.btnFinancePaymentReport = New DevExpress.XtraEditors.SimpleButton
        Me.btnFinanceCreateBill = New DevExpress.XtraEditors.SimpleButton
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.CustumerFinanceDepListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNote = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colPrice = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colPayed = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colVehicleDisplay = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerDisplay = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colVehicleLastRegistrationDate = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colPriceName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemLookUpEdit3 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.PaymentCataologListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.colVehicleLastRegistrationNumber = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridControl2 = New DevExpress.XtraGrid.GridControl
        Me.IncorectTehnicalExamsDepListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colIdTehnicalExam = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colMadeDate = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colValidTillDate = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdFirsControler = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemLookUpEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.colIdSecondControler = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colExplanationNote = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDriversWarning = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerDisplay1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerSurname = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerFirstName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colVehicleDisplay1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colVehicleLastRegistrationNumber1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colVehicleLastRegistrationDate1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colShellNumber = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colEngineNumber = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdCustomerVehicleRelation = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdCustomer = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdVehicle = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdTypeOfTehnicalExam = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemLookUpEdit4 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem18 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem22 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem23 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlGroup3 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem17 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem24 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem2 = New DevExpress.XtraLayout.SplitterItem
        Me.LayoutControlGroup4 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem15 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem13 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem16 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem3 = New DevExpress.XtraLayout.SplitterItem
        Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
        Me.LayoutControlItem14 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem4 = New DevExpress.XtraLayout.SplitterItem
        Me.LayoutControlItem19 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem20 = New DevExpress.XtraLayout.LayoutControlItem
        Me.DocumentTypesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.RequestTypeListTreeList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RequestTypeRepositoryItemHyperLinkEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RequestTypeListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ActiveDocumentsDepListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemLookUpEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UsersListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CustumerFinanceDepListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemLookUpEdit3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PaymentCataologListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IncorectTehnicalExamsDepListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemLookUpEdit4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem22, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem23, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem17, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem24, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem19, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DocumentTypesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.LayoutControl1.Controls.Add(Me.btnRefreshTechReports)
        Me.LayoutControl1.Controls.Add(Me.btnRefreshFinances)
        Me.LayoutControl1.Controls.Add(Me.btnNewCalculation)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl1.Controls.Add(Me.btnNewInternationalDriLic)
        Me.LayoutControl1.Controls.Add(Me.btnNewPermission)
        Me.LayoutControl1.Controls.Add(Me.btnNewBill)
        Me.LayoutControl1.Controls.Add(Me.btnTehnicalExamPrint)
        Me.LayoutControl1.Controls.Add(Me.btnDogovori)
        Me.LayoutControl1.Controls.Add(Me.btnPrint)
        Me.LayoutControl1.Controls.Add(Me.RequestTypeListTreeList)
        Me.LayoutControl1.Controls.Add(Me.btnCreateCalculation)
        Me.LayoutControl1.Controls.Add(Me.btnCancel)
        Me.LayoutControl1.Controls.Add(Me.btnEditDocument)
        Me.LayoutControl1.Controls.Add(Me.btnApproveRequest)
        Me.LayoutControl1.Controls.Add(Me.btnRefreshRequests)
        Me.LayoutControl1.Controls.Add(Me.GridControl3)
        Me.LayoutControl1.Controls.Add(Me.btnTechnicalExamNew)
        Me.LayoutControl1.Controls.Add(Me.btnTehnicalExamDismis)
        Me.LayoutControl1.Controls.Add(Me.btnTechnicalExamEdit)
        Me.LayoutControl1.Controls.Add(Me.btnFinancePaymentReport)
        Me.LayoutControl1.Controls.Add(Me.btnFinanceCreateBill)
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Controls.Add(Me.GridControl2)
        Me.LayoutControl1.Font = Nothing
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        '
        'btnRefreshTechReports
        '
        Me.btnRefreshTechReports.AccessibleDescription = Nothing
        Me.btnRefreshTechReports.AccessibleName = Nothing
        resources.ApplyResources(Me.btnRefreshTechReports, "btnRefreshTechReports")
        Me.btnRefreshTechReports.BackgroundImage = Nothing
        Me.btnRefreshTechReports.Name = "btnRefreshTechReports"
        Me.btnRefreshTechReports.StyleController = Me.LayoutControl1
        '
        'btnRefreshFinances
        '
        Me.btnRefreshFinances.AccessibleDescription = Nothing
        Me.btnRefreshFinances.AccessibleName = Nothing
        resources.ApplyResources(Me.btnRefreshFinances, "btnRefreshFinances")
        Me.btnRefreshFinances.Appearance.Options.UseTextOptions = True
        Me.btnRefreshFinances.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.btnRefreshFinances.BackgroundImage = Nothing
        Me.btnRefreshFinances.MinimumSize = New System.Drawing.Size(0, 44)
        Me.btnRefreshFinances.Name = "btnRefreshFinances"
        Me.btnRefreshFinances.StyleController = Me.LayoutControl1
        '
        'btnNewCalculation
        '
        Me.btnNewCalculation.AccessibleDescription = Nothing
        Me.btnNewCalculation.AccessibleName = Nothing
        resources.ApplyResources(Me.btnNewCalculation, "btnNewCalculation")
        Me.btnNewCalculation.Appearance.Options.UseTextOptions = True
        Me.btnNewCalculation.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.btnNewCalculation.BackgroundImage = Nothing
        Me.btnNewCalculation.MinimumSize = New System.Drawing.Size(0, 44)
        Me.btnNewCalculation.Name = "btnNewCalculation"
        Me.btnNewCalculation.StyleController = Me.LayoutControl1
        '
        'SimpleButton1
        '
        Me.SimpleButton1.AccessibleDescription = Nothing
        Me.SimpleButton1.AccessibleName = Nothing
        resources.ApplyResources(Me.SimpleButton1, "SimpleButton1")
        Me.SimpleButton1.Appearance.Options.UseTextOptions = True
        Me.SimpleButton1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.SimpleButton1.BackgroundImage = Nothing
        Me.SimpleButton1.MaximumSize = New System.Drawing.Size(0, 44)
        Me.SimpleButton1.MinimumSize = New System.Drawing.Size(0, 44)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.StyleController = Me.LayoutControl1
        '
        'btnNewInternationalDriLic
        '
        Me.btnNewInternationalDriLic.AccessibleDescription = Nothing
        Me.btnNewInternationalDriLic.AccessibleName = Nothing
        resources.ApplyResources(Me.btnNewInternationalDriLic, "btnNewInternationalDriLic")
        Me.btnNewInternationalDriLic.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnNewInternationalDriLic.Appearance.Options.UseFont = True
        Me.btnNewInternationalDriLic.BackgroundImage = Nothing
        Me.btnNewInternationalDriLic.Name = "btnNewInternationalDriLic"
        Me.btnNewInternationalDriLic.StyleController = Me.LayoutControl1
        '
        'btnNewPermission
        '
        Me.btnNewPermission.AccessibleDescription = Nothing
        Me.btnNewPermission.AccessibleName = Nothing
        resources.ApplyResources(Me.btnNewPermission, "btnNewPermission")
        Me.btnNewPermission.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnNewPermission.Appearance.Options.UseFont = True
        Me.btnNewPermission.BackgroundImage = Nothing
        Me.btnNewPermission.Name = "btnNewPermission"
        Me.btnNewPermission.StyleController = Me.LayoutControl1
        '
        'btnNewBill
        '
        Me.btnNewBill.AccessibleDescription = Nothing
        Me.btnNewBill.AccessibleName = Nothing
        resources.ApplyResources(Me.btnNewBill, "btnNewBill")
        Me.btnNewBill.Appearance.Options.UseTextOptions = True
        Me.btnNewBill.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.btnNewBill.BackgroundImage = Nothing
        Me.btnNewBill.MaximumSize = New System.Drawing.Size(0, 44)
        Me.btnNewBill.MinimumSize = New System.Drawing.Size(0, 44)
        Me.btnNewBill.Name = "btnNewBill"
        Me.btnNewBill.StyleController = Me.LayoutControl1
        '
        'btnTehnicalExamPrint
        '
        Me.btnTehnicalExamPrint.AccessibleDescription = Nothing
        Me.btnTehnicalExamPrint.AccessibleName = Nothing
        resources.ApplyResources(Me.btnTehnicalExamPrint, "btnTehnicalExamPrint")
        Me.btnTehnicalExamPrint.BackgroundImage = Nothing
        Me.btnTehnicalExamPrint.Name = "btnTehnicalExamPrint"
        Me.btnTehnicalExamPrint.StyleController = Me.LayoutControl1
        '
        'btnDogovori
        '
        Me.btnDogovori.AccessibleDescription = Nothing
        Me.btnDogovori.AccessibleName = Nothing
        resources.ApplyResources(Me.btnDogovori, "btnDogovori")
        Me.btnDogovori.BackgroundImage = Nothing
        Me.btnDogovori.MinimumSize = New System.Drawing.Size(0, 23)
        Me.btnDogovori.Name = "btnDogovori"
        Me.btnDogovori.StyleController = Me.LayoutControl1
        '
        'btnPrint
        '
        Me.btnPrint.AccessibleDescription = Nothing
        Me.btnPrint.AccessibleName = Nothing
        resources.ApplyResources(Me.btnPrint, "btnPrint")
        Me.btnPrint.BackgroundImage = Nothing
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.StyleController = Me.LayoutControl1
        '
        'RequestTypeListTreeList
        '
        Me.RequestTypeListTreeList.AccessibleDescription = Nothing
        Me.RequestTypeListTreeList.AccessibleName = Nothing
        resources.ApplyResources(Me.RequestTypeListTreeList, "RequestTypeListTreeList")
        Me.RequestTypeListTreeList.BackgroundImage = Nothing
        Me.RequestTypeListTreeList.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.colIdDocumentPrint1, Me.colIsTehnicalExamRequired, Me.colIsPayRequired, Me.colIsNewRegistration, Me.colIsRelationDeleted, Me.colIsVehicleDeleted, Me.colIsNewCustomer, Me.colIsVehicleChanged, Me.colIsCustomerChanged, Me.colIsSufficient, Me.colTypeName, Me.colTypeDescription, Me.colIsPreviosRegistrationReqired, Me.colId2})
        Me.RequestTypeListTreeList.DataSource = Me.RequestTypeListBindingSource
        Me.RequestTypeListTreeList.Font = Nothing
        Me.RequestTypeListTreeList.KeyFieldName = "Id"
        Me.RequestTypeListTreeList.Name = "RequestTypeListTreeList"
        Me.RequestTypeListTreeList.ParentFieldName = "IdRequestType"
        Me.RequestTypeListTreeList.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RequestTypeRepositoryItemHyperLinkEdit})
        '
        'colIdDocumentPrint1
        '
        resources.ApplyResources(Me.colIdDocumentPrint1, "colIdDocumentPrint1")
        Me.colIdDocumentPrint1.FieldName = "IdDocumentPrint"
        Me.colIdDocumentPrint1.Format.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.colIdDocumentPrint1.Name = "colIdDocumentPrint1"
        Me.colIdDocumentPrint1.OptionsColumn.ReadOnly = True
        '
        'colIsTehnicalExamRequired
        '
        resources.ApplyResources(Me.colIsTehnicalExamRequired, "colIsTehnicalExamRequired")
        Me.colIsTehnicalExamRequired.FieldName = "IsTehnicalExamRequired"
        Me.colIsTehnicalExamRequired.Name = "colIsTehnicalExamRequired"
        Me.colIsTehnicalExamRequired.OptionsColumn.ReadOnly = True
        '
        'colIsPayRequired
        '
        resources.ApplyResources(Me.colIsPayRequired, "colIsPayRequired")
        Me.colIsPayRequired.FieldName = "IsPayRequired"
        Me.colIsPayRequired.Name = "colIsPayRequired"
        Me.colIsPayRequired.OptionsColumn.ReadOnly = True
        '
        'colIsNewRegistration
        '
        resources.ApplyResources(Me.colIsNewRegistration, "colIsNewRegistration")
        Me.colIsNewRegistration.FieldName = "IsNewRegistration"
        Me.colIsNewRegistration.Name = "colIsNewRegistration"
        Me.colIsNewRegistration.OptionsColumn.ReadOnly = True
        '
        'colIsRelationDeleted
        '
        resources.ApplyResources(Me.colIsRelationDeleted, "colIsRelationDeleted")
        Me.colIsRelationDeleted.FieldName = "IsRelationDeleted"
        Me.colIsRelationDeleted.Name = "colIsRelationDeleted"
        Me.colIsRelationDeleted.OptionsColumn.ReadOnly = True
        '
        'colIsVehicleDeleted
        '
        resources.ApplyResources(Me.colIsVehicleDeleted, "colIsVehicleDeleted")
        Me.colIsVehicleDeleted.FieldName = "IsVehicleDeleted"
        Me.colIsVehicleDeleted.Name = "colIsVehicleDeleted"
        Me.colIsVehicleDeleted.OptionsColumn.ReadOnly = True
        '
        'colIsNewCustomer
        '
        resources.ApplyResources(Me.colIsNewCustomer, "colIsNewCustomer")
        Me.colIsNewCustomer.FieldName = "IsNewCustomer"
        Me.colIsNewCustomer.Name = "colIsNewCustomer"
        Me.colIsNewCustomer.OptionsColumn.ReadOnly = True
        '
        'colIsVehicleChanged
        '
        resources.ApplyResources(Me.colIsVehicleChanged, "colIsVehicleChanged")
        Me.colIsVehicleChanged.FieldName = "IsVehicleChanged"
        Me.colIsVehicleChanged.Name = "colIsVehicleChanged"
        Me.colIsVehicleChanged.OptionsColumn.ReadOnly = True
        '
        'colIsCustomerChanged
        '
        resources.ApplyResources(Me.colIsCustomerChanged, "colIsCustomerChanged")
        Me.colIsCustomerChanged.FieldName = "IsCustomerChanged"
        Me.colIsCustomerChanged.Name = "colIsCustomerChanged"
        Me.colIsCustomerChanged.OptionsColumn.ReadOnly = True
        '
        'colIsSufficient
        '
        resources.ApplyResources(Me.colIsSufficient, "colIsSufficient")
        Me.colIsSufficient.FieldName = "IsSufficient"
        Me.colIsSufficient.Name = "colIsSufficient"
        Me.colIsSufficient.OptionsColumn.ReadOnly = True
        '
        'colTypeName
        '
        resources.ApplyResources(Me.colTypeName, "colTypeName")
        Me.colTypeName.ColumnEdit = Me.RequestTypeRepositoryItemHyperLinkEdit
        Me.colTypeName.FieldName = "TypeName"
        Me.colTypeName.Name = "colTypeName"
        '
        'RequestTypeRepositoryItemHyperLinkEdit
        '
        Me.RequestTypeRepositoryItemHyperLinkEdit.AccessibleDescription = Nothing
        Me.RequestTypeRepositoryItemHyperLinkEdit.AccessibleName = Nothing
        resources.ApplyResources(Me.RequestTypeRepositoryItemHyperLinkEdit, "RequestTypeRepositoryItemHyperLinkEdit")
        Me.RequestTypeRepositoryItemHyperLinkEdit.Mask.AutoComplete = CType(resources.GetObject("RequestTypeRepositoryItemHyperLinkEdit.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.RequestTypeRepositoryItemHyperLinkEdit.Mask.BeepOnError = CType(resources.GetObject("RequestTypeRepositoryItemHyperLinkEdit.Mask.BeepOnError"), Boolean)
        Me.RequestTypeRepositoryItemHyperLinkEdit.Mask.EditMask = resources.GetString("RequestTypeRepositoryItemHyperLinkEdit.Mask.EditMask")
        Me.RequestTypeRepositoryItemHyperLinkEdit.Mask.IgnoreMaskBlank = CType(resources.GetObject("RequestTypeRepositoryItemHyperLinkEdit.Mask.IgnoreMaskBlank"), Boolean)
        Me.RequestTypeRepositoryItemHyperLinkEdit.Mask.MaskType = CType(resources.GetObject("RequestTypeRepositoryItemHyperLinkEdit.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.RequestTypeRepositoryItemHyperLinkEdit.Mask.PlaceHolder = CType(resources.GetObject("RequestTypeRepositoryItemHyperLinkEdit.Mask.PlaceHolder"), Char)
        Me.RequestTypeRepositoryItemHyperLinkEdit.Mask.SaveLiteral = CType(resources.GetObject("RequestTypeRepositoryItemHyperLinkEdit.Mask.SaveLiteral"), Boolean)
        Me.RequestTypeRepositoryItemHyperLinkEdit.Mask.ShowPlaceHolders = CType(resources.GetObject("RequestTypeRepositoryItemHyperLinkEdit.Mask.ShowPlaceHolders"), Boolean)
        Me.RequestTypeRepositoryItemHyperLinkEdit.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("RequestTypeRepositoryItemHyperLinkEdit.Mask.UseMaskAsDisplayFormat"), Boolean)
        Me.RequestTypeRepositoryItemHyperLinkEdit.Name = "RequestTypeRepositoryItemHyperLinkEdit"
        '
        'colTypeDescription
        '
        resources.ApplyResources(Me.colTypeDescription, "colTypeDescription")
        Me.colTypeDescription.FieldName = "TypeDescription"
        Me.colTypeDescription.Name = "colTypeDescription"
        Me.colTypeDescription.OptionsColumn.ReadOnly = True
        '
        'colIsPreviosRegistrationReqired
        '
        resources.ApplyResources(Me.colIsPreviosRegistrationReqired, "colIsPreviosRegistrationReqired")
        Me.colIsPreviosRegistrationReqired.FieldName = "IsPreviosRegistrationReqired"
        Me.colIsPreviosRegistrationReqired.Name = "colIsPreviosRegistrationReqired"
        Me.colIsPreviosRegistrationReqired.OptionsColumn.ReadOnly = True
        '
        'colId2
        '
        resources.ApplyResources(Me.colId2, "colId2")
        Me.colId2.FieldName = "Id"
        Me.colId2.Name = "colId2"
        '
        'RequestTypeListBindingSource
        '
        Me.RequestTypeListBindingSource.DataSource = GetType(VTE.Library.RequestTypeList)
        '
        'btnCreateCalculation
        '
        Me.btnCreateCalculation.AccessibleDescription = Nothing
        Me.btnCreateCalculation.AccessibleName = Nothing
        resources.ApplyResources(Me.btnCreateCalculation, "btnCreateCalculation")
        Me.btnCreateCalculation.Appearance.Options.UseTextOptions = True
        Me.btnCreateCalculation.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.btnCreateCalculation.BackgroundImage = Nothing
        Me.btnCreateCalculation.MaximumSize = New System.Drawing.Size(0, 44)
        Me.btnCreateCalculation.MinimumSize = New System.Drawing.Size(0, 44)
        Me.btnCreateCalculation.Name = "btnCreateCalculation"
        Me.btnCreateCalculation.StyleController = Me.LayoutControl1
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleDescription = Nothing
        Me.btnCancel.AccessibleName = Nothing
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.BackgroundImage = Nothing
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.StyleController = Me.LayoutControl1
        '
        'btnEditDocument
        '
        Me.btnEditDocument.AccessibleDescription = Nothing
        Me.btnEditDocument.AccessibleName = Nothing
        resources.ApplyResources(Me.btnEditDocument, "btnEditDocument")
        Me.btnEditDocument.BackgroundImage = Nothing
        Me.btnEditDocument.Name = "btnEditDocument"
        Me.btnEditDocument.StyleController = Me.LayoutControl1
        '
        'btnApproveRequest
        '
        Me.btnApproveRequest.AccessibleDescription = Nothing
        Me.btnApproveRequest.AccessibleName = Nothing
        resources.ApplyResources(Me.btnApproveRequest, "btnApproveRequest")
        Me.btnApproveRequest.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnApproveRequest.Appearance.Options.UseBackColor = True
        Me.btnApproveRequest.BackgroundImage = Nothing
        Me.btnApproveRequest.Name = "btnApproveRequest"
        Me.btnApproveRequest.StyleController = Me.LayoutControl1
        '
        'btnRefreshRequests
        '
        Me.btnRefreshRequests.AccessibleDescription = Nothing
        Me.btnRefreshRequests.AccessibleName = Nothing
        resources.ApplyResources(Me.btnRefreshRequests, "btnRefreshRequests")
        Me.btnRefreshRequests.Appearance.BackColor = System.Drawing.Color.Black
        Me.btnRefreshRequests.Appearance.Options.UseBackColor = True
        Me.btnRefreshRequests.BackgroundImage = Nothing
        Me.btnRefreshRequests.Name = "btnRefreshRequests"
        Me.btnRefreshRequests.StyleController = Me.LayoutControl1
        '
        'GridControl3
        '
        Me.GridControl3.AccessibleDescription = Nothing
        Me.GridControl3.AccessibleName = Nothing
        resources.ApplyResources(Me.GridControl3, "GridControl3")
        Me.GridControl3.BackgroundImage = Nothing
        Me.GridControl3.DataSource = Me.ActiveDocumentsDepListBindingSource
        Me.GridControl3.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.GridControl3.EmbeddedNavigator.AccessibleName = Nothing
        Me.GridControl3.EmbeddedNavigator.Anchor = CType(resources.GetObject("GridControl3.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.GridControl3.EmbeddedNavigator.BackgroundImage = Nothing
        Me.GridControl3.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("GridControl3.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.GridControl3.EmbeddedNavigator.ImeMode = CType(resources.GetObject("GridControl3.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.GridControl3.EmbeddedNavigator.TextLocation = CType(resources.GetObject("GridControl3.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.GridControl3.EmbeddedNavigator.ToolTip = resources.GetString("GridControl3.EmbeddedNavigator.ToolTip")
        Me.GridControl3.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("GridControl3.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.GridControl3.EmbeddedNavigator.ToolTipTitle = resources.GetString("GridControl3.EmbeddedNavigator.ToolTipTitle")
        Me.GridControl3.Font = Nothing
        Me.GridControl3.MainView = Me.GridView3
        Me.GridControl3.Name = "GridControl3"
        Me.GridControl3.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemLookUpEdit2})
        Me.GridControl3.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView3})
        '
        'ActiveDocumentsDepListBindingSource
        '
        Me.ActiveDocumentsDepListBindingSource.DataSource = GetType(VTE.Library.ActiveDocumentList)
        '
        'GridView3
        '
        resources.ApplyResources(Me.GridView3, "GridView3")
        Me.GridView3.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colLastRegistrationNumber, Me.colVehicleDisplay2, Me.colVehicleModelMaker, Me.colCustomerDisplay2, Me.colId1, Me.colRequestPrintName, Me.colIdCustomerVehicleRelation1, Me.colIdCustomer1, Me.colIdVehicle1, Me.colMB, Me.colCustomerSurname1, Me.colCustomerFirstName1, Me.colShellNumber1, Me.colIdVehicleModel, Me.colModelName, Me.colIdVehicleMaker, Me.colVehicleMaker, Me.colIdOperatorCreated, Me.colIdOperatorModified, Me.colIdOperatorEnded, Me.colIdTechnicalExamReport, Me.colDateCreated, Me.colDateModified, Me.colDateEnded, Me.colIdDocumentPrint})
        Me.GridView3.GridControl = Me.GridControl3
        Me.GridView3.Name = "GridView3"
        Me.GridView3.OptionsView.ShowAutoFilterRow = True
        '
        'colLastRegistrationNumber
        '
        resources.ApplyResources(Me.colLastRegistrationNumber, "colLastRegistrationNumber")
        Me.colLastRegistrationNumber.FieldName = "LastRegistrationNumber"
        Me.colLastRegistrationNumber.Name = "colLastRegistrationNumber"
        Me.colLastRegistrationNumber.OptionsColumn.ReadOnly = True
        '
        'colVehicleDisplay2
        '
        Me.colVehicleDisplay2.AppearanceCell.Options.UseTextOptions = True
        Me.colVehicleDisplay2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.colVehicleDisplay2.AppearanceHeader.Options.UseTextOptions = True
        Me.colVehicleDisplay2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        resources.ApplyResources(Me.colVehicleDisplay2, "colVehicleDisplay2")
        Me.colVehicleDisplay2.FieldName = "VehicleDisplay"
        Me.colVehicleDisplay2.Name = "colVehicleDisplay2"
        Me.colVehicleDisplay2.OptionsColumn.ReadOnly = True
        Me.colVehicleDisplay2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
        '
        'colVehicleModelMaker
        '
        resources.ApplyResources(Me.colVehicleModelMaker, "colVehicleModelMaker")
        Me.colVehicleModelMaker.FieldName = "VehicleModelMaker"
        Me.colVehicleModelMaker.Name = "colVehicleModelMaker"
        Me.colVehicleModelMaker.OptionsColumn.ReadOnly = True
        '
        'colCustomerDisplay2
        '
        Me.colCustomerDisplay2.AppearanceCell.Options.UseTextOptions = True
        Me.colCustomerDisplay2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.colCustomerDisplay2.AppearanceHeader.Options.UseTextOptions = True
        Me.colCustomerDisplay2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        resources.ApplyResources(Me.colCustomerDisplay2, "colCustomerDisplay2")
        Me.colCustomerDisplay2.FieldName = "CustomerDisplay"
        Me.colCustomerDisplay2.Name = "colCustomerDisplay2"
        Me.colCustomerDisplay2.OptionsColumn.ReadOnly = True
        Me.colCustomerDisplay2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
        '
        'colId1
        '
        Me.colId1.AppearanceCell.Options.UseTextOptions = True
        Me.colId1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.colId1.AppearanceHeader.Options.UseTextOptions = True
        Me.colId1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        resources.ApplyResources(Me.colId1, "colId1")
        Me.colId1.FieldName = "Id"
        Me.colId1.Name = "colId1"
        Me.colId1.OptionsColumn.ReadOnly = True
        '
        'colRequestPrintName
        '
        resources.ApplyResources(Me.colRequestPrintName, "colRequestPrintName")
        Me.colRequestPrintName.FieldName = "RequestPrintName"
        Me.colRequestPrintName.Name = "colRequestPrintName"
        Me.colRequestPrintName.OptionsColumn.ReadOnly = True
        '
        'colIdCustomerVehicleRelation1
        '
        resources.ApplyResources(Me.colIdCustomerVehicleRelation1, "colIdCustomerVehicleRelation1")
        Me.colIdCustomerVehicleRelation1.FieldName = "IdCustomerVehicleRelation"
        Me.colIdCustomerVehicleRelation1.Name = "colIdCustomerVehicleRelation1"
        Me.colIdCustomerVehicleRelation1.OptionsColumn.ReadOnly = True
        '
        'colIdCustomer1
        '
        resources.ApplyResources(Me.colIdCustomer1, "colIdCustomer1")
        Me.colIdCustomer1.FieldName = "IdCustomer"
        Me.colIdCustomer1.Name = "colIdCustomer1"
        Me.colIdCustomer1.OptionsColumn.ReadOnly = True
        '
        'colIdVehicle1
        '
        resources.ApplyResources(Me.colIdVehicle1, "colIdVehicle1")
        Me.colIdVehicle1.FieldName = "IdVehicle"
        Me.colIdVehicle1.Name = "colIdVehicle1"
        Me.colIdVehicle1.OptionsColumn.ReadOnly = True
        '
        'colMB
        '
        resources.ApplyResources(Me.colMB, "colMB")
        Me.colMB.FieldName = "MB"
        Me.colMB.Name = "colMB"
        Me.colMB.OptionsColumn.ReadOnly = True
        '
        'colCustomerSurname1
        '
        resources.ApplyResources(Me.colCustomerSurname1, "colCustomerSurname1")
        Me.colCustomerSurname1.FieldName = "CustomerSurname"
        Me.colCustomerSurname1.Name = "colCustomerSurname1"
        Me.colCustomerSurname1.OptionsColumn.ReadOnly = True
        '
        'colCustomerFirstName1
        '
        resources.ApplyResources(Me.colCustomerFirstName1, "colCustomerFirstName1")
        Me.colCustomerFirstName1.FieldName = "CustomerFirstName"
        Me.colCustomerFirstName1.Name = "colCustomerFirstName1"
        Me.colCustomerFirstName1.OptionsColumn.ReadOnly = True
        '
        'colShellNumber1
        '
        resources.ApplyResources(Me.colShellNumber1, "colShellNumber1")
        Me.colShellNumber1.FieldName = "ShellNumber"
        Me.colShellNumber1.Name = "colShellNumber1"
        Me.colShellNumber1.OptionsColumn.ReadOnly = True
        '
        'colIdVehicleModel
        '
        resources.ApplyResources(Me.colIdVehicleModel, "colIdVehicleModel")
        Me.colIdVehicleModel.FieldName = "IdVehicleModel"
        Me.colIdVehicleModel.Name = "colIdVehicleModel"
        Me.colIdVehicleModel.OptionsColumn.ReadOnly = True
        '
        'colModelName
        '
        resources.ApplyResources(Me.colModelName, "colModelName")
        Me.colModelName.FieldName = "ModelName"
        Me.colModelName.Name = "colModelName"
        Me.colModelName.OptionsColumn.ReadOnly = True
        '
        'colIdVehicleMaker
        '
        resources.ApplyResources(Me.colIdVehicleMaker, "colIdVehicleMaker")
        Me.colIdVehicleMaker.FieldName = "IdVehicleMaker"
        Me.colIdVehicleMaker.Name = "colIdVehicleMaker"
        Me.colIdVehicleMaker.OptionsColumn.ReadOnly = True
        '
        'colVehicleMaker
        '
        resources.ApplyResources(Me.colVehicleMaker, "colVehicleMaker")
        Me.colVehicleMaker.FieldName = "VehicleMaker"
        Me.colVehicleMaker.Name = "colVehicleMaker"
        Me.colVehicleMaker.OptionsColumn.ReadOnly = True
        '
        'colIdOperatorCreated
        '
        resources.ApplyResources(Me.colIdOperatorCreated, "colIdOperatorCreated")
        Me.colIdOperatorCreated.ColumnEdit = Me.RepositoryItemLookUpEdit2
        Me.colIdOperatorCreated.FieldName = "IdOperatorCreated"
        Me.colIdOperatorCreated.Name = "colIdOperatorCreated"
        Me.colIdOperatorCreated.OptionsColumn.AllowEdit = False
        Me.colIdOperatorCreated.OptionsColumn.ReadOnly = True
        '
        'RepositoryItemLookUpEdit2
        '
        Me.RepositoryItemLookUpEdit2.AccessibleDescription = Nothing
        Me.RepositoryItemLookUpEdit2.AccessibleName = Nothing
        resources.ApplyResources(Me.RepositoryItemLookUpEdit2, "RepositoryItemLookUpEdit2")
        Me.RepositoryItemLookUpEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit2.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.RepositoryItemLookUpEdit2.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "ID", 30, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FullName", "FullName", 49, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FirstName", "FirstName", 54, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("SureName", "SureName", 55, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Address", "Address", 45, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("EMBG", "EMBG", 33, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("BLK", "BLK", 23, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
        Me.RepositoryItemLookUpEdit2.DataSource = Me.UsersListBindingSource
        Me.RepositoryItemLookUpEdit2.DisplayMember = "FullName"
        Me.RepositoryItemLookUpEdit2.Name = "RepositoryItemLookUpEdit2"
        Me.RepositoryItemLookUpEdit2.ReadOnly = True
        Me.RepositoryItemLookUpEdit2.ValueMember = "ID"
        '
        'UsersListBindingSource
        '
        Me.UsersListBindingSource.DataSource = GetType(VTE.Library.UsersList)
        '
        'colIdOperatorModified
        '
        resources.ApplyResources(Me.colIdOperatorModified, "colIdOperatorModified")
        Me.colIdOperatorModified.FieldName = "IdOperatorModified"
        Me.colIdOperatorModified.Name = "colIdOperatorModified"
        Me.colIdOperatorModified.OptionsColumn.ReadOnly = True
        '
        'colIdOperatorEnded
        '
        resources.ApplyResources(Me.colIdOperatorEnded, "colIdOperatorEnded")
        Me.colIdOperatorEnded.FieldName = "IdOperatorEnded"
        Me.colIdOperatorEnded.Name = "colIdOperatorEnded"
        Me.colIdOperatorEnded.OptionsColumn.ReadOnly = True
        '
        'colIdTechnicalExamReport
        '
        resources.ApplyResources(Me.colIdTechnicalExamReport, "colIdTechnicalExamReport")
        Me.colIdTechnicalExamReport.FieldName = "IdTechnicalExamReport"
        Me.colIdTechnicalExamReport.Name = "colIdTechnicalExamReport"
        Me.colIdTechnicalExamReport.OptionsColumn.ReadOnly = True
        '
        'colDateCreated
        '
        Me.colDateCreated.AppearanceCell.Options.UseTextOptions = True
        Me.colDateCreated.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.colDateCreated.AppearanceHeader.Options.UseTextOptions = True
        Me.colDateCreated.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        resources.ApplyResources(Me.colDateCreated, "colDateCreated")
        Me.colDateCreated.FieldName = "DateCreated"
        Me.colDateCreated.Name = "colDateCreated"
        Me.colDateCreated.OptionsColumn.ReadOnly = True
        '
        'colDateModified
        '
        resources.ApplyResources(Me.colDateModified, "colDateModified")
        Me.colDateModified.FieldName = "DateModified"
        Me.colDateModified.Name = "colDateModified"
        Me.colDateModified.OptionsColumn.ReadOnly = True
        '
        'colDateEnded
        '
        resources.ApplyResources(Me.colDateEnded, "colDateEnded")
        Me.colDateEnded.FieldName = "DateEnded"
        Me.colDateEnded.Name = "colDateEnded"
        Me.colDateEnded.OptionsColumn.ReadOnly = True
        '
        'colIdDocumentPrint
        '
        resources.ApplyResources(Me.colIdDocumentPrint, "colIdDocumentPrint")
        Me.colIdDocumentPrint.FieldName = "IdDocumentPrint"
        Me.colIdDocumentPrint.Name = "colIdDocumentPrint"
        Me.colIdDocumentPrint.OptionsColumn.ReadOnly = True
        '
        'btnTechnicalExamNew
        '
        Me.btnTechnicalExamNew.AccessibleDescription = Nothing
        Me.btnTechnicalExamNew.AccessibleName = Nothing
        resources.ApplyResources(Me.btnTechnicalExamNew, "btnTechnicalExamNew")
        Me.btnTechnicalExamNew.BackgroundImage = Nothing
        Me.btnTechnicalExamNew.Name = "btnTechnicalExamNew"
        Me.btnTechnicalExamNew.StyleController = Me.LayoutControl1
        '
        'btnTehnicalExamDismis
        '
        Me.btnTehnicalExamDismis.AccessibleDescription = Nothing
        Me.btnTehnicalExamDismis.AccessibleName = Nothing
        resources.ApplyResources(Me.btnTehnicalExamDismis, "btnTehnicalExamDismis")
        Me.btnTehnicalExamDismis.BackgroundImage = Nothing
        Me.btnTehnicalExamDismis.Name = "btnTehnicalExamDismis"
        Me.btnTehnicalExamDismis.StyleController = Me.LayoutControl1
        '
        'btnTechnicalExamEdit
        '
        Me.btnTechnicalExamEdit.AccessibleDescription = Nothing
        Me.btnTechnicalExamEdit.AccessibleName = Nothing
        resources.ApplyResources(Me.btnTechnicalExamEdit, "btnTechnicalExamEdit")
        Me.btnTechnicalExamEdit.BackgroundImage = Nothing
        Me.btnTechnicalExamEdit.Name = "btnTechnicalExamEdit"
        Me.btnTechnicalExamEdit.StyleController = Me.LayoutControl1
        '
        'btnFinancePaymentReport
        '
        Me.btnFinancePaymentReport.AccessibleDescription = Nothing
        Me.btnFinancePaymentReport.AccessibleName = Nothing
        resources.ApplyResources(Me.btnFinancePaymentReport, "btnFinancePaymentReport")
        Me.btnFinancePaymentReport.Appearance.Options.UseTextOptions = True
        Me.btnFinancePaymentReport.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.btnFinancePaymentReport.BackgroundImage = Nothing
        Me.btnFinancePaymentReport.MaximumSize = New System.Drawing.Size(0, 44)
        Me.btnFinancePaymentReport.MinimumSize = New System.Drawing.Size(0, 44)
        Me.btnFinancePaymentReport.Name = "btnFinancePaymentReport"
        Me.btnFinancePaymentReport.StyleController = Me.LayoutControl1
        '
        'btnFinanceCreateBill
        '
        Me.btnFinanceCreateBill.AccessibleDescription = Nothing
        Me.btnFinanceCreateBill.AccessibleName = Nothing
        resources.ApplyResources(Me.btnFinanceCreateBill, "btnFinanceCreateBill")
        Me.btnFinanceCreateBill.Appearance.Options.UseTextOptions = True
        Me.btnFinanceCreateBill.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.btnFinanceCreateBill.BackgroundImage = Nothing
        Me.btnFinanceCreateBill.MaximumSize = New System.Drawing.Size(0, 44)
        Me.btnFinanceCreateBill.MinimumSize = New System.Drawing.Size(0, 44)
        Me.btnFinanceCreateBill.Name = "btnFinanceCreateBill"
        Me.btnFinanceCreateBill.StyleController = Me.LayoutControl1
        '
        'GridControl1
        '
        Me.GridControl1.AccessibleDescription = Nothing
        Me.GridControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.GridControl1, "GridControl1")
        Me.GridControl1.BackgroundImage = Nothing
        Me.GridControl1.DataSource = Me.CustumerFinanceDepListBindingSource
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
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemLookUpEdit3})
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'CustumerFinanceDepListBindingSource
        '
        Me.CustumerFinanceDepListBindingSource.DataSource = GetType(VTE.Library.CustumerFinanceList)
        '
        'GridView1
        '
        resources.ApplyResources(Me.GridView1, "GridView1")
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colNote, Me.colPrice, Me.colPayed, Me.colVehicleDisplay, Me.colCustomerDisplay, Me.colVehicleLastRegistrationDate, Me.colPriceName, Me.colVehicleLastRegistrationNumber})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.GroupCount = 2
        Me.GridView1.GroupSummary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Price", Nothing, "(Total  {0:c})")})
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ShowAutoFilterRow = True
        Me.GridView1.OptionsView.ShowFooter = True
        Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colCustomerDisplay, DevExpress.Data.ColumnSortOrder.Ascending), New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colVehicleDisplay, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colId
        '
        resources.ApplyResources(Me.colId, "colId")
        Me.colId.FieldName = "Id"
        Me.colId.Name = "colId"
        Me.colId.OptionsColumn.ReadOnly = True
        '
        'colNote
        '
        resources.ApplyResources(Me.colNote, "colNote")
        Me.colNote.FieldName = "Note"
        Me.colNote.Name = "colNote"
        Me.colNote.OptionsColumn.ReadOnly = True
        '
        'colPrice
        '
        Me.colPrice.AppearanceCell.Options.UseTextOptions = True
        Me.colPrice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.colPrice.AppearanceHeader.Options.UseTextOptions = True
        Me.colPrice.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        resources.ApplyResources(Me.colPrice, "colPrice")
        Me.colPrice.FieldName = "Price"
        Me.colPrice.Name = "colPrice"
        Me.colPrice.OptionsColumn.ReadOnly = True
        Me.colPrice.SummaryItem.DisplayFormat = resources.GetString("colPrice.SummaryItem.DisplayFormat")
        Me.colPrice.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        '
        'colPayed
        '
        resources.ApplyResources(Me.colPayed, "colPayed")
        Me.colPayed.FieldName = "Payed"
        Me.colPayed.Name = "colPayed"
        Me.colPayed.OptionsColumn.ReadOnly = True
        '
        'colVehicleDisplay
        '
        resources.ApplyResources(Me.colVehicleDisplay, "colVehicleDisplay")
        Me.colVehicleDisplay.FieldName = "VehicleDisplay"
        Me.colVehicleDisplay.Name = "colVehicleDisplay"
        Me.colVehicleDisplay.OptionsColumn.ReadOnly = True
        Me.colVehicleDisplay.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
        '
        'colCustomerDisplay
        '
        resources.ApplyResources(Me.colCustomerDisplay, "colCustomerDisplay")
        Me.colCustomerDisplay.FieldName = "CustomerDisplay"
        Me.colCustomerDisplay.Name = "colCustomerDisplay"
        Me.colCustomerDisplay.OptionsColumn.ReadOnly = True
        Me.colCustomerDisplay.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
        '
        'colVehicleLastRegistrationDate
        '
        resources.ApplyResources(Me.colVehicleLastRegistrationDate, "colVehicleLastRegistrationDate")
        Me.colVehicleLastRegistrationDate.FieldName = "VehicleLastRegistrationDate"
        Me.colVehicleLastRegistrationDate.Name = "colVehicleLastRegistrationDate"
        Me.colVehicleLastRegistrationDate.OptionsColumn.ReadOnly = True
        '
        'colPriceName
        '
        resources.ApplyResources(Me.colPriceName, "colPriceName")
        Me.colPriceName.ColumnEdit = Me.RepositoryItemLookUpEdit3
        Me.colPriceName.FieldName = "IdPriceCatalog"
        Me.colPriceName.Name = "colPriceName"
        '
        'RepositoryItemLookUpEdit3
        '
        Me.RepositoryItemLookUpEdit3.AccessibleDescription = Nothing
        Me.RepositoryItemLookUpEdit3.AccessibleName = Nothing
        resources.ApplyResources(Me.RepositoryItemLookUpEdit3, "RepositoryItemLookUpEdit3")
        Me.RepositoryItemLookUpEdit3.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit3.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines), Global.WinApp.My.Resources.Resources.String1, CType(resources.GetObject("RepositoryItemLookUpEdit3.Buttons1"), Integer), CType(resources.GetObject("RepositoryItemLookUpEdit3.Buttons2"), Boolean), CType(resources.GetObject("RepositoryItemLookUpEdit3.Buttons3"), Boolean), CType(resources.GetObject("RepositoryItemLookUpEdit3.Buttons4"), Boolean), CType(resources.GetObject("RepositoryItemLookUpEdit3.Buttons5"), DevExpress.XtraEditors.ImageLocation), Nothing)})
        Me.RepositoryItemLookUpEdit3.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("PaymentName", "PaymentName", 150, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdPaymentCategory", "IdPaymentCategory", 103, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdPaymentItem", "IdPaymentItem", 80, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdPaymentParametar", "IdPaymentParametar", 108, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdDDV", "IdDDV", 36, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("DDVName", "DDVName", 53, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("DDVValue", "DDVValue", 52, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CategoryName", "CategoryName", 78, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("TrigerdByRequest", "TrigerdByRequest", 92, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("TrigerdByTechnicalExam", "TrigerdByTechnicalExam", 122, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("TrigerdByTrafficLicence", "TrigerdByTrafficLicence", 118, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("TrigerdByPremisionForVehicle", "TrigerdByPremisionForVehicle", 146, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("TrigerdByInternationalDrivierLicence", "TrigerdByInternationalDrivierLicence", 180, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("VehicleCategoriesForPaymantCode", "VehicleCategoriesForPaymantCode", 174, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("VehicleCategoriesForPaymantName", "VehicleCategoriesForPaymantName", 176, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdVehicleCategoryForPayments", "IdVehicleCategoryForPayments", 157, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItemName", "ItemName", 55, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("PrametarName", "PrametarName", 77, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("VehicleField", "VehicleField", 61, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ParametarFrom", "ParametarFrom", 80, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ParametarTo", "ParametarTo", 68, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Price", "Price", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsOptional", "IsOptional", 55, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
        Me.RepositoryItemLookUpEdit3.DataSource = Me.PaymentCataologListBindingSource
        Me.RepositoryItemLookUpEdit3.DisplayMember = "PaymentName"
        Me.RepositoryItemLookUpEdit3.Name = "RepositoryItemLookUpEdit3"
        Me.RepositoryItemLookUpEdit3.ValueMember = "IdPaymentParametar"
        '
        'PaymentCataologListBindingSource
        '
        Me.PaymentCataologListBindingSource.DataSource = GetType(VTE.Library.PaymentCataologList)
        '
        'colVehicleLastRegistrationNumber
        '
        resources.ApplyResources(Me.colVehicleLastRegistrationNumber, "colVehicleLastRegistrationNumber")
        Me.colVehicleLastRegistrationNumber.FieldName = "VehicleLastRegistrationNumber"
        Me.colVehicleLastRegistrationNumber.Name = "colVehicleLastRegistrationNumber"
        '
        'GridControl2
        '
        Me.GridControl2.AccessibleDescription = Nothing
        Me.GridControl2.AccessibleName = Nothing
        resources.ApplyResources(Me.GridControl2, "GridControl2")
        Me.GridControl2.BackgroundImage = Nothing
        Me.GridControl2.DataSource = Me.IncorectTehnicalExamsDepListBindingSource
        Me.GridControl2.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.GridControl2.EmbeddedNavigator.AccessibleName = Nothing
        Me.GridControl2.EmbeddedNavigator.Anchor = CType(resources.GetObject("GridControl2.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.GridControl2.EmbeddedNavigator.BackgroundImage = Nothing
        Me.GridControl2.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("GridControl2.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.GridControl2.EmbeddedNavigator.ImeMode = CType(resources.GetObject("GridControl2.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.GridControl2.EmbeddedNavigator.TextLocation = CType(resources.GetObject("GridControl2.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.GridControl2.EmbeddedNavigator.ToolTip = resources.GetString("GridControl2.EmbeddedNavigator.ToolTip")
        Me.GridControl2.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("GridControl2.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.GridControl2.EmbeddedNavigator.ToolTipTitle = resources.GetString("GridControl2.EmbeddedNavigator.ToolTipTitle")
        Me.GridControl2.Font = Nothing
        Me.GridControl2.MainView = Me.GridView2
        Me.GridControl2.Name = "GridControl2"
        Me.GridControl2.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemLookUpEdit1, Me.RepositoryItemLookUpEdit4})
        Me.GridControl2.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView2})
        '
        'IncorectTehnicalExamsDepListBindingSource
        '
        Me.IncorectTehnicalExamsDepListBindingSource.DataSource = GetType(VTE.Library.IncorectTehnicalExamsList)
        '
        'GridView2
        '
        resources.ApplyResources(Me.GridView2, "GridView2")
        Me.GridView2.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colIdTehnicalExam, Me.colMadeDate, Me.colValidTillDate, Me.colIdFirsControler, Me.colIdSecondControler, Me.colExplanationNote, Me.colDriversWarning, Me.colCustomerDisplay1, Me.colCustomerSurname, Me.colCustomerFirstName, Me.colVehicleDisplay1, Me.colVehicleLastRegistrationNumber1, Me.colVehicleLastRegistrationDate1, Me.colShellNumber, Me.colEngineNumber, Me.colIdCustomerVehicleRelation, Me.colIdCustomer, Me.colIdVehicle, Me.colIdTypeOfTehnicalExam})
        Me.GridView2.GridControl = Me.GridControl2
        Me.GridView2.Name = "GridView2"
        Me.GridView2.OptionsView.ShowAutoFilterRow = True
        Me.GridView2.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colIdTehnicalExam, DevExpress.Data.ColumnSortOrder.Descending)})
        '
        'colIdTehnicalExam
        '
        resources.ApplyResources(Me.colIdTehnicalExam, "colIdTehnicalExam")
        Me.colIdTehnicalExam.FieldName = "IdTehnicalExam"
        Me.colIdTehnicalExam.Name = "colIdTehnicalExam"
        Me.colIdTehnicalExam.OptionsColumn.ReadOnly = True
        '
        'colMadeDate
        '
        Me.colMadeDate.AppearanceCell.Options.UseTextOptions = True
        Me.colMadeDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.colMadeDate.AppearanceHeader.Options.UseTextOptions = True
        Me.colMadeDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        resources.ApplyResources(Me.colMadeDate, "colMadeDate")
        Me.colMadeDate.FieldName = "MadeDate"
        Me.colMadeDate.Name = "colMadeDate"
        Me.colMadeDate.OptionsColumn.ReadOnly = True
        '
        'colValidTillDate
        '
        Me.colValidTillDate.AppearanceCell.Options.UseTextOptions = True
        Me.colValidTillDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.colValidTillDate.AppearanceHeader.Options.UseTextOptions = True
        Me.colValidTillDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        resources.ApplyResources(Me.colValidTillDate, "colValidTillDate")
        Me.colValidTillDate.FieldName = "ValidTillDate"
        Me.colValidTillDate.Name = "colValidTillDate"
        Me.colValidTillDate.OptionsColumn.ReadOnly = True
        '
        'colIdFirsControler
        '
        Me.colIdFirsControler.AppearanceCell.Options.UseTextOptions = True
        Me.colIdFirsControler.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.colIdFirsControler.AppearanceHeader.Options.UseTextOptions = True
        Me.colIdFirsControler.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        resources.ApplyResources(Me.colIdFirsControler, "colIdFirsControler")
        Me.colIdFirsControler.ColumnEdit = Me.RepositoryItemLookUpEdit1
        Me.colIdFirsControler.FieldName = "IdFirsControler"
        Me.colIdFirsControler.Name = "colIdFirsControler"
        Me.colIdFirsControler.OptionsColumn.ReadOnly = True
        Me.colIdFirsControler.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
        '
        'RepositoryItemLookUpEdit1
        '
        Me.RepositoryItemLookUpEdit1.AccessibleDescription = Nothing
        Me.RepositoryItemLookUpEdit1.AccessibleName = Nothing
        resources.ApplyResources(Me.RepositoryItemLookUpEdit1, "RepositoryItemLookUpEdit1")
        Me.RepositoryItemLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines), Global.WinApp.My.Resources.Resources.String1, CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons1"), Integer), CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons2"), Boolean), CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons3"), Boolean), CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons4"), Boolean), CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons5"), DevExpress.XtraEditors.ImageLocation), Nothing)})
        Me.RepositoryItemLookUpEdit1.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "ID", 30, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FullName", "FullName", 150, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FirstName", "FirstName", 54, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("SureName", "SureName", 55, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Address", "Address", 45, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("EMBG", "EMBG", 33, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("BLK", "BLK", 23, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
        Me.RepositoryItemLookUpEdit1.DataSource = Me.UsersListBindingSource
        Me.RepositoryItemLookUpEdit1.DisplayMember = "FullName"
        Me.RepositoryItemLookUpEdit1.Name = "RepositoryItemLookUpEdit1"
        Me.RepositoryItemLookUpEdit1.ReadOnly = True
        Me.RepositoryItemLookUpEdit1.ValueMember = "ID"
        '
        'colIdSecondControler
        '
        resources.ApplyResources(Me.colIdSecondControler, "colIdSecondControler")
        Me.colIdSecondControler.FieldName = "IdSecondControler"
        Me.colIdSecondControler.Name = "colIdSecondControler"
        Me.colIdSecondControler.OptionsColumn.ReadOnly = True
        '
        'colExplanationNote
        '
        resources.ApplyResources(Me.colExplanationNote, "colExplanationNote")
        Me.colExplanationNote.FieldName = "ExplanationNote"
        Me.colExplanationNote.Name = "colExplanationNote"
        Me.colExplanationNote.OptionsColumn.ReadOnly = True
        '
        'colDriversWarning
        '
        resources.ApplyResources(Me.colDriversWarning, "colDriversWarning")
        Me.colDriversWarning.FieldName = "DriversWarning"
        Me.colDriversWarning.Name = "colDriversWarning"
        Me.colDriversWarning.OptionsColumn.ReadOnly = True
        '
        'colCustomerDisplay1
        '
        resources.ApplyResources(Me.colCustomerDisplay1, "colCustomerDisplay1")
        Me.colCustomerDisplay1.FieldName = "CustomerDisplay"
        Me.colCustomerDisplay1.Name = "colCustomerDisplay1"
        Me.colCustomerDisplay1.OptionsColumn.ReadOnly = True
        Me.colCustomerDisplay1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
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
        'colVehicleDisplay1
        '
        Me.colVehicleDisplay1.AppearanceCell.Options.UseTextOptions = True
        Me.colVehicleDisplay1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.colVehicleDisplay1.AppearanceHeader.Options.UseTextOptions = True
        Me.colVehicleDisplay1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        resources.ApplyResources(Me.colVehicleDisplay1, "colVehicleDisplay1")
        Me.colVehicleDisplay1.FieldName = "VehicleDisplay"
        Me.colVehicleDisplay1.Name = "colVehicleDisplay1"
        Me.colVehicleDisplay1.OptionsColumn.ReadOnly = True
        Me.colVehicleDisplay1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
        '
        'colVehicleLastRegistrationNumber1
        '
        resources.ApplyResources(Me.colVehicleLastRegistrationNumber1, "colVehicleLastRegistrationNumber1")
        Me.colVehicleLastRegistrationNumber1.FieldName = "VehicleLastRegistrationNumber"
        Me.colVehicleLastRegistrationNumber1.Name = "colVehicleLastRegistrationNumber1"
        Me.colVehicleLastRegistrationNumber1.OptionsColumn.ReadOnly = True
        Me.colVehicleLastRegistrationNumber1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
        '
        'colVehicleLastRegistrationDate1
        '
        resources.ApplyResources(Me.colVehicleLastRegistrationDate1, "colVehicleLastRegistrationDate1")
        Me.colVehicleLastRegistrationDate1.FieldName = "VehicleLastRegistrationDate"
        Me.colVehicleLastRegistrationDate1.Name = "colVehicleLastRegistrationDate1"
        Me.colVehicleLastRegistrationDate1.OptionsColumn.ReadOnly = True
        '
        'colShellNumber
        '
        resources.ApplyResources(Me.colShellNumber, "colShellNumber")
        Me.colShellNumber.FieldName = "ShellNumber"
        Me.colShellNumber.Name = "colShellNumber"
        Me.colShellNumber.OptionsColumn.ReadOnly = True
        '
        'colEngineNumber
        '
        resources.ApplyResources(Me.colEngineNumber, "colEngineNumber")
        Me.colEngineNumber.FieldName = "EngineNumber"
        Me.colEngineNumber.Name = "colEngineNumber"
        Me.colEngineNumber.OptionsColumn.ReadOnly = True
        '
        'colIdCustomerVehicleRelation
        '
        resources.ApplyResources(Me.colIdCustomerVehicleRelation, "colIdCustomerVehicleRelation")
        Me.colIdCustomerVehicleRelation.FieldName = "IdCustomerVehicleRelation"
        Me.colIdCustomerVehicleRelation.Name = "colIdCustomerVehicleRelation"
        Me.colIdCustomerVehicleRelation.OptionsColumn.ReadOnly = True
        '
        'colIdCustomer
        '
        resources.ApplyResources(Me.colIdCustomer, "colIdCustomer")
        Me.colIdCustomer.FieldName = "IdCustomer"
        Me.colIdCustomer.Name = "colIdCustomer"
        Me.colIdCustomer.OptionsColumn.ReadOnly = True
        '
        'colIdVehicle
        '
        resources.ApplyResources(Me.colIdVehicle, "colIdVehicle")
        Me.colIdVehicle.FieldName = "IdVehicle"
        Me.colIdVehicle.Name = "colIdVehicle"
        Me.colIdVehicle.OptionsColumn.ReadOnly = True
        '
        'colIdTypeOfTehnicalExam
        '
        resources.ApplyResources(Me.colIdTypeOfTehnicalExam, "colIdTypeOfTehnicalExam")
        Me.colIdTypeOfTehnicalExam.FieldName = "TypeOfTehnicalExam"
        Me.colIdTypeOfTehnicalExam.Name = "colIdTypeOfTehnicalExam"
        '
        'RepositoryItemLookUpEdit4
        '
        Me.RepositoryItemLookUpEdit4.AccessibleDescription = Nothing
        Me.RepositoryItemLookUpEdit4.AccessibleName = Nothing
        resources.ApplyResources(Me.RepositoryItemLookUpEdit4, "RepositoryItemLookUpEdit4")
        Me.RepositoryItemLookUpEdit4.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit4.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.RepositoryItemLookUpEdit4.Name = "RepositoryItemLookUpEdit4"
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2, Me.LayoutControlGroup3, Me.SplitterItem2, Me.LayoutControlGroup4, Me.SplitterItem3, Me.SplitterItem1, Me.LayoutControlItem14, Me.SplitterItem4, Me.LayoutControlItem19, Me.LayoutControlItem20})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(1043, 738)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlGroup2
        '
        resources.ApplyResources(Me.LayoutControlGroup2, "LayoutControlGroup2")
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem3, Me.LayoutControlItem1, Me.LayoutControlItem11, Me.LayoutControlItem18, Me.LayoutControlItem2, Me.LayoutControlItem21, Me.LayoutControlItem22, Me.LayoutControlItem23})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(528, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(513, 304)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.btnFinancePaymentReport
        resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
        Me.LayoutControlItem3.Location = New System.Drawing.Point(358, 225)
        Me.LayoutControlItem3.MaxSize = New System.Drawing.Size(0, 33)
        Me.LayoutControlItem3.MinSize = New System.Drawing.Size(1, 33)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(80, 55)
        Me.LayoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl1
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(507, 225)
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.btnCreateCalculation
        resources.ApplyResources(Me.LayoutControlItem11, "LayoutControlItem11")
        Me.LayoutControlItem11.Location = New System.Drawing.Point(209, 225)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(74, 55)
        Me.LayoutControlItem11.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem11.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem11.TextToControlDistance = 0
        Me.LayoutControlItem11.TextVisible = False
        '
        'LayoutControlItem18
        '
        Me.LayoutControlItem18.Control = Me.btnNewBill
        resources.ApplyResources(Me.LayoutControlItem18, "LayoutControlItem18")
        Me.LayoutControlItem18.Location = New System.Drawing.Point(75, 225)
        Me.LayoutControlItem18.Name = "LayoutControlItem18"
        Me.LayoutControlItem18.Size = New System.Drawing.Size(62, 55)
        Me.LayoutControlItem18.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem18.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem18.TextToControlDistance = 0
        Me.LayoutControlItem18.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.btnFinanceCreateBill
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(137, 225)
        Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(0, 33)
        Me.LayoutControlItem2.MinSize = New System.Drawing.Size(72, 33)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(72, 55)
        Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.Control = Me.SimpleButton1
        resources.ApplyResources(Me.LayoutControlItem21, "LayoutControlItem21")
        Me.LayoutControlItem21.Location = New System.Drawing.Point(438, 225)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.Size = New System.Drawing.Size(69, 55)
        Me.LayoutControlItem21.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem21.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem21.TextToControlDistance = 0
        Me.LayoutControlItem21.TextVisible = False
        '
        'LayoutControlItem22
        '
        Me.LayoutControlItem22.Control = Me.btnNewCalculation
        resources.ApplyResources(Me.LayoutControlItem22, "LayoutControlItem22")
        Me.LayoutControlItem22.Location = New System.Drawing.Point(283, 225)
        Me.LayoutControlItem22.Name = "LayoutControlItem22"
        Me.LayoutControlItem22.Size = New System.Drawing.Size(75, 55)
        Me.LayoutControlItem22.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem22.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem22.TextToControlDistance = 0
        Me.LayoutControlItem22.TextVisible = False
        '
        'LayoutControlItem23
        '
        Me.LayoutControlItem23.Control = Me.btnRefreshFinances
        resources.ApplyResources(Me.LayoutControlItem23, "LayoutControlItem23")
        Me.LayoutControlItem23.Location = New System.Drawing.Point(0, 225)
        Me.LayoutControlItem23.Name = "LayoutControlItem23"
        Me.LayoutControlItem23.Size = New System.Drawing.Size(75, 55)
        Me.LayoutControlItem23.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem23.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem23.TextToControlDistance = 0
        Me.LayoutControlItem23.TextVisible = False
        '
        'LayoutControlGroup3
        '
        resources.ApplyResources(Me.LayoutControlGroup3, "LayoutControlGroup3")
        Me.LayoutControlGroup3.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem4, Me.LayoutControlItem6, Me.LayoutControlItem7, Me.LayoutControlItem17, Me.LayoutControlItem5, Me.LayoutControlItem24})
        Me.LayoutControlGroup3.Location = New System.Drawing.Point(528, 310)
        Me.LayoutControlGroup3.Name = "LayoutControlGroup3"
        Me.LayoutControlGroup3.Size = New System.Drawing.Size(513, 426)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.GridControl2
        resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(507, 369)
        Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextToControlDistance = 0
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.btnTehnicalExamDismis
        resources.ApplyResources(Me.LayoutControlItem6, "LayoutControlItem6")
        Me.LayoutControlItem6.Location = New System.Drawing.Point(408, 369)
        Me.LayoutControlItem6.MaxSize = New System.Drawing.Size(0, 33)
        Me.LayoutControlItem6.MinSize = New System.Drawing.Size(58, 33)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(99, 33)
        Me.LayoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem6.TextToControlDistance = 0
        Me.LayoutControlItem6.TextVisible = False
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.btnTechnicalExamNew
        resources.ApplyResources(Me.LayoutControlItem7, "LayoutControlItem7")
        Me.LayoutControlItem7.Location = New System.Drawing.Point(307, 369)
        Me.LayoutControlItem7.MaxSize = New System.Drawing.Size(0, 33)
        Me.LayoutControlItem7.MinSize = New System.Drawing.Size(45, 33)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(101, 33)
        Me.LayoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem7.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem7.TextToControlDistance = 0
        Me.LayoutControlItem7.TextVisible = False
        '
        'LayoutControlItem17
        '
        Me.LayoutControlItem17.Control = Me.btnTehnicalExamPrint
        resources.ApplyResources(Me.LayoutControlItem17, "LayoutControlItem17")
        Me.LayoutControlItem17.Location = New System.Drawing.Point(211, 369)
        Me.LayoutControlItem17.Name = "LayoutControlItem17"
        Me.LayoutControlItem17.Size = New System.Drawing.Size(96, 33)
        Me.LayoutControlItem17.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem17.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem17.TextToControlDistance = 0
        Me.LayoutControlItem17.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.btnTechnicalExamEdit
        resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
        Me.LayoutControlItem5.Location = New System.Drawing.Point(115, 369)
        Me.LayoutControlItem5.MaxSize = New System.Drawing.Size(0, 33)
        Me.LayoutControlItem5.MinSize = New System.Drawing.Size(42, 33)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(96, 33)
        Me.LayoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem5.TextToControlDistance = 0
        Me.LayoutControlItem5.TextVisible = False
        '
        'LayoutControlItem24
        '
        Me.LayoutControlItem24.Control = Me.btnRefreshTechReports
        resources.ApplyResources(Me.LayoutControlItem24, "LayoutControlItem24")
        Me.LayoutControlItem24.Location = New System.Drawing.Point(0, 369)
        Me.LayoutControlItem24.Name = "LayoutControlItem24"
        Me.LayoutControlItem24.Size = New System.Drawing.Size(115, 33)
        Me.LayoutControlItem24.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem24.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem24.TextToControlDistance = 0
        Me.LayoutControlItem24.TextVisible = False
        '
        'SplitterItem2
        '
        resources.ApplyResources(Me.SplitterItem2, "SplitterItem2")
        Me.SplitterItem2.Location = New System.Drawing.Point(528, 304)
        Me.SplitterItem2.Name = "SplitterItem2"
        Me.SplitterItem2.Size = New System.Drawing.Size(513, 6)
        '
        'LayoutControlGroup4
        '
        resources.ApplyResources(Me.LayoutControlGroup4, "LayoutControlGroup4")
        Me.LayoutControlGroup4.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem8, Me.LayoutControlItem9, Me.LayoutControlItem15, Me.LayoutControlItem12, Me.LayoutControlItem13, Me.LayoutControlItem10, Me.LayoutControlItem16})
        Me.LayoutControlGroup4.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup4.Name = "LayoutControlGroup4"
        Me.LayoutControlGroup4.Size = New System.Drawing.Size(522, 259)
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.GridControl3
        resources.ApplyResources(Me.LayoutControlItem8, "LayoutControlItem8")
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(516, 167)
        Me.LayoutControlItem8.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem8.TextToControlDistance = 0
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.btnEditDocument
        resources.ApplyResources(Me.LayoutControlItem9, "LayoutControlItem9")
        Me.LayoutControlItem9.Location = New System.Drawing.Point(0, 167)
        Me.LayoutControlItem9.MaxSize = New System.Drawing.Size(0, 34)
        Me.LayoutControlItem9.MinSize = New System.Drawing.Size(98, 34)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(149, 34)
        Me.LayoutControlItem9.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem9.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem9.TextToControlDistance = 0
        Me.LayoutControlItem9.TextVisible = False
        '
        'LayoutControlItem15
        '
        Me.LayoutControlItem15.Control = Me.btnPrint
        resources.ApplyResources(Me.LayoutControlItem15, "LayoutControlItem15")
        Me.LayoutControlItem15.Location = New System.Drawing.Point(0, 201)
        Me.LayoutControlItem15.Name = "LayoutControlItem15"
        Me.LayoutControlItem15.Size = New System.Drawing.Size(149, 34)
        Me.LayoutControlItem15.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem15.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem15.TextToControlDistance = 0
        Me.LayoutControlItem15.TextVisible = False
        '
        'LayoutControlItem12
        '
        Me.LayoutControlItem12.Control = Me.btnRefreshRequests
        resources.ApplyResources(Me.LayoutControlItem12, "LayoutControlItem12")
        Me.LayoutControlItem12.Location = New System.Drawing.Point(149, 201)
        Me.LayoutControlItem12.MaxSize = New System.Drawing.Size(0, 34)
        Me.LayoutControlItem12.MinSize = New System.Drawing.Size(71, 34)
        Me.LayoutControlItem12.Name = "LayoutControlItem12"
        Me.LayoutControlItem12.Size = New System.Drawing.Size(239, 34)
        Me.LayoutControlItem12.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem12.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem12.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem12.TextToControlDistance = 0
        Me.LayoutControlItem12.TextVisible = False
        '
        'LayoutControlItem13
        '
        Me.LayoutControlItem13.Control = Me.btnApproveRequest
        resources.ApplyResources(Me.LayoutControlItem13, "LayoutControlItem13")
        Me.LayoutControlItem13.Location = New System.Drawing.Point(388, 201)
        Me.LayoutControlItem13.MaxSize = New System.Drawing.Size(0, 34)
        Me.LayoutControlItem13.MinSize = New System.Drawing.Size(23, 34)
        Me.LayoutControlItem13.Name = "LayoutControlItem13"
        Me.LayoutControlItem13.Size = New System.Drawing.Size(128, 34)
        Me.LayoutControlItem13.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem13.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem13.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem13.TextToControlDistance = 0
        Me.LayoutControlItem13.TextVisible = False
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.btnCancel
        resources.ApplyResources(Me.LayoutControlItem10, "LayoutControlItem10")
        Me.LayoutControlItem10.Location = New System.Drawing.Point(149, 167)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(239, 34)
        Me.LayoutControlItem10.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem10.TextToControlDistance = 0
        Me.LayoutControlItem10.TextVisible = False
        '
        'LayoutControlItem16
        '
        Me.LayoutControlItem16.Control = Me.btnDogovori
        resources.ApplyResources(Me.LayoutControlItem16, "LayoutControlItem16")
        Me.LayoutControlItem16.Location = New System.Drawing.Point(388, 167)
        Me.LayoutControlItem16.Name = "LayoutControlItem16"
        Me.LayoutControlItem16.Size = New System.Drawing.Size(128, 34)
        Me.LayoutControlItem16.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem16.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem16.TextToControlDistance = 0
        Me.LayoutControlItem16.TextVisible = False
        '
        'SplitterItem3
        '
        resources.ApplyResources(Me.SplitterItem3, "SplitterItem3")
        Me.SplitterItem3.Location = New System.Drawing.Point(0, 259)
        Me.SplitterItem3.Name = "SplitterItem3"
        Me.SplitterItem3.Size = New System.Drawing.Size(522, 6)
        '
        'SplitterItem1
        '
        resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
        Me.SplitterItem1.Location = New System.Drawing.Point(522, 0)
        Me.SplitterItem1.Name = "SplitterItem1"
        Me.SplitterItem1.Size = New System.Drawing.Size(6, 736)
        '
        'LayoutControlItem14
        '
        Me.LayoutControlItem14.Control = Me.RequestTypeListTreeList
        resources.ApplyResources(Me.LayoutControlItem14, "LayoutControlItem14")
        Me.LayoutControlItem14.Location = New System.Drawing.Point(0, 304)
        Me.LayoutControlItem14.Name = "LayoutControlItem14"
        Me.LayoutControlItem14.Size = New System.Drawing.Size(522, 432)
        Me.LayoutControlItem14.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem14.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem14.TextToControlDistance = 0
        Me.LayoutControlItem14.TextVisible = False
        '
        'SplitterItem4
        '
        resources.ApplyResources(Me.SplitterItem4, "SplitterItem4")
        Me.SplitterItem4.Location = New System.Drawing.Point(0, 298)
        Me.SplitterItem4.Name = "SplitterItem4"
        Me.SplitterItem4.Size = New System.Drawing.Size(522, 6)
        '
        'LayoutControlItem19
        '
        Me.LayoutControlItem19.Control = Me.btnNewPermission
        resources.ApplyResources(Me.LayoutControlItem19, "LayoutControlItem19")
        Me.LayoutControlItem19.Location = New System.Drawing.Point(0, 265)
        Me.LayoutControlItem19.Name = "LayoutControlItem19"
        Me.LayoutControlItem19.Size = New System.Drawing.Size(265, 33)
        Me.LayoutControlItem19.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem19.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem19.TextToControlDistance = 0
        Me.LayoutControlItem19.TextVisible = False
        '
        'LayoutControlItem20
        '
        Me.LayoutControlItem20.Control = Me.btnNewInternationalDriLic
        resources.ApplyResources(Me.LayoutControlItem20, "LayoutControlItem20")
        Me.LayoutControlItem20.Location = New System.Drawing.Point(265, 265)
        Me.LayoutControlItem20.Name = "LayoutControlItem20"
        Me.LayoutControlItem20.Size = New System.Drawing.Size(257, 33)
        Me.LayoutControlItem20.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem20.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem20.TextToControlDistance = 0
        Me.LayoutControlItem20.TextVisible = False
        '
        'DocumentTypesBindingSource
        '
        Me.DocumentTypesBindingSource.DataSource = GetType(VTE.Library.DocumentTypes)
        '
        'uxDashboard
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "uxDashboard"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.RequestTypeListTreeList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RequestTypeRepositoryItemHyperLinkEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RequestTypeListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ActiveDocumentsDepListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemLookUpEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UsersListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CustumerFinanceDepListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemLookUpEdit3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PaymentCataologListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IncorectTehnicalExamsDepListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemLookUpEdit4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem22, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem23, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem17, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem24, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem19, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DocumentTypesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
    Friend WithEvents CustumerFinanceDepListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNote As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPayed As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVehicleDisplay As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerDisplay As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVehicleLastRegistrationDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPriceName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVehicleLastRegistrationNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btnFinanceCreateBill As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnFinancePaymentReport As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents btnTechnicalExamEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GridControl2 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlGroup3 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents IncorectTehnicalExamsDepListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents colIdTehnicalExam As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMadeDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colValidTillDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdFirsControler As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdSecondControler As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colExplanationNote As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDriversWarning As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerDisplay1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerSurname As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerFirstName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVehicleDisplay1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVehicleLastRegistrationNumber1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVehicleLastRegistrationDate1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colShellNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colEngineNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdCustomerVehicleRelation As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdCustomer As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdVehicle As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btnTehnicalExamDismis As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SplitterItem2 As DevExpress.XtraLayout.SplitterItem
    Friend WithEvents btnTechnicalExamNew As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents RepositoryItemLookUpEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents UsersListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GridControl3 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView3 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SplitterItem3 As DevExpress.XtraLayout.SplitterItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents ActiveDocumentsDepListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents RepositoryItemLookUpEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents LayoutControlGroup4 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents btnRefreshRequests As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnApproveRequest As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem13 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents DocumentTypesBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents btnEditDocument As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents RepositoryItemLookUpEdit3 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents PaymentCataologListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents btnCreateCalculation As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents colLastRegistrationNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVehicleDisplay2 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVehicleModelMaker As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerDisplay2 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colId1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colRequestPrintName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdCustomerVehicleRelation1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdCustomer1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdVehicle1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMB As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerSurname1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerFirstName1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colShellNumber1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdVehicleModel As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colModelName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdVehicleMaker As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVehicleMaker As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdOperatorCreated As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdOperatorModified As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdOperatorEnded As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdTechnicalExamReport As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDateCreated As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDateModified As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDateEnded As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdDocumentPrint As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RequestTypeListTreeList As DevExpress.XtraTreeList.TreeList
    Friend WithEvents colIdDocumentPrint1 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colIsTehnicalExamRequired As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colIsPayRequired As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colIsNewRegistration As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colIsRelationDeleted As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colIsVehicleDeleted As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colIsNewCustomer As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colIsVehicleChanged As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colIsCustomerChanged As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colIsSufficient As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTypeName As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTypeDescription As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colIsPreviosRegistrationReqired As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents RequestTypeListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents LayoutControlItem14 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents RequestTypeRepositoryItemHyperLinkEdit As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents colId2 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents btnPrint As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem15 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnDogovori As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem16 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnTehnicalExamPrint As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem17 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnNewBill As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem18 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents colIdTypeOfTehnicalExam As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemLookUpEdit4 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents SplitterItem4 As DevExpress.XtraLayout.SplitterItem
    Friend WithEvents btnNewInternationalDriLic As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnNewPermission As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem19 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem20 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnNewCalculation As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem22 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnRefreshFinances As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem23 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnRefreshTechReports As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem24 As DevExpress.XtraLayout.LayoutControlItem

End Class
