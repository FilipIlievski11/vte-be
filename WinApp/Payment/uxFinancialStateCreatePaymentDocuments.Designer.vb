<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class uxFinancialStateCreatePaymentDocuments
    Inherits VTE.BaseParts.uxWinPart

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxFinancialStateCreatePaymentDocuments))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.btnExit = New DevExpress.XtraEditors.SimpleButton
        Me.btnCreateBill = New DevExpress.XtraEditors.SimpleButton
        Me.CustomerFinancialStateListGridControl = New DevExpress.XtraGrid.GridControl
        Me.CustomerFinancialStateListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdCustomerVehicleRelation = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdDocument = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdDocumentTehnicalExam = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdDocumentsTrafficLicences = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdDocumentIternationalDriveingLicence = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdDocumentPermisions = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdPriceCatalog = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNote = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colPrice = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colPayed = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colShellNumber = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerSurname = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerFirstName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdDDVCatalog = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDDVName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDDVValue = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDDVPriceValue = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colPriceWithDDV = New DevExpress.XtraGrid.Columns.GridColumn
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CustomerFinancialStateListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CustomerFinancialStateListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.AccessibleDescription = Nothing
        Me.LayoutControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
        Me.LayoutControl1.BackgroundImage = Nothing
        Me.LayoutControl1.Controls.Add(Me.btnExit)
        Me.LayoutControl1.Controls.Add(Me.btnCreateBill)
        Me.LayoutControl1.Controls.Add(Me.CustomerFinancialStateListGridControl)
        Me.LayoutControl1.Font = Nothing
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
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
        'btnCreateBill
        '
        Me.btnCreateBill.AccessibleDescription = Nothing
        Me.btnCreateBill.AccessibleName = Nothing
        resources.ApplyResources(Me.btnCreateBill, "btnCreateBill")
        Me.btnCreateBill.BackgroundImage = Nothing
        Me.btnCreateBill.Name = "btnCreateBill"
        Me.btnCreateBill.StyleController = Me.LayoutControl1
        '
        'CustomerFinancialStateListGridControl
        '
        Me.CustomerFinancialStateListGridControl.AccessibleDescription = Nothing
        Me.CustomerFinancialStateListGridControl.AccessibleName = Nothing
        resources.ApplyResources(Me.CustomerFinancialStateListGridControl, "CustomerFinancialStateListGridControl")
        Me.CustomerFinancialStateListGridControl.BackgroundImage = Nothing
        Me.CustomerFinancialStateListGridControl.DataSource = Me.CustomerFinancialStateListBindingSource
        Me.CustomerFinancialStateListGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.CustomerFinancialStateListGridControl.EmbeddedNavigator.AccessibleName = Nothing
        Me.CustomerFinancialStateListGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("CustomerFinancialStateListGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.CustomerFinancialStateListGridControl.EmbeddedNavigator.BackgroundImage = Nothing
        Me.CustomerFinancialStateListGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("CustomerFinancialStateListGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.CustomerFinancialStateListGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("CustomerFinancialStateListGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.CustomerFinancialStateListGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("CustomerFinancialStateListGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.CustomerFinancialStateListGridControl.EmbeddedNavigator.ToolTip = resources.GetString("CustomerFinancialStateListGridControl.EmbeddedNavigator.ToolTip")
        Me.CustomerFinancialStateListGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("CustomerFinancialStateListGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.CustomerFinancialStateListGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("CustomerFinancialStateListGridControl.EmbeddedNavigator.ToolTipTitle")
        Me.CustomerFinancialStateListGridControl.Font = Nothing
        Me.CustomerFinancialStateListGridControl.MainView = Me.GridView1
        Me.CustomerFinancialStateListGridControl.Name = "CustomerFinancialStateListGridControl"
        Me.CustomerFinancialStateListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'CustomerFinancialStateListBindingSource
        '
        Me.CustomerFinancialStateListBindingSource.DataSource = GetType(VTE.Library.CustomerFinancialStateInfo)
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
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdCustomerVehicleRelation, Me.colIdDocument, Me.colIdDocumentTehnicalExam, Me.colIdDocumentsTrafficLicences, Me.colIdDocumentIternationalDriveingLicence, Me.colIdDocumentPermisions, Me.colIdPriceCatalog, Me.colNote, Me.colPrice, Me.colPayed, Me.colShellNumber, Me.colCustomerSurname, Me.colCustomerFirstName, Me.colCustomerName, Me.colName, Me.colIdDDVCatalog, Me.colDDVName, Me.colDDVValue, Me.colDDVPriceValue, Me.colPriceWithDDV})
        Me.GridView1.GridControl = Me.CustomerFinancialStateListGridControl
        Me.GridView1.GroupCount = 2
        Me.GridView1.GroupSummary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Price", Me.colPrice, Global.WinApp.My.Resources.Resources.String1), New DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PriceWithDDV", Me.colPriceWithDDV, Global.WinApp.My.Resources.Resources.String1)})
        Me.GridView1.Name = "GridView1"
        Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colCustomerName, DevExpress.Data.ColumnSortOrder.Ascending), New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colShellNumber, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colId
        '
        resources.ApplyResources(Me.colId, "colId")
        Me.colId.FieldName = "Id"
        Me.colId.Name = "colId"
        Me.colId.OptionsColumn.ReadOnly = True
        '
        'colIdCustomerVehicleRelation
        '
        resources.ApplyResources(Me.colIdCustomerVehicleRelation, "colIdCustomerVehicleRelation")
        Me.colIdCustomerVehicleRelation.FieldName = "IdCustomerVehicleRelation"
        Me.colIdCustomerVehicleRelation.Name = "colIdCustomerVehicleRelation"
        Me.colIdCustomerVehicleRelation.OptionsColumn.ReadOnly = True
        '
        'colIdDocument
        '
        resources.ApplyResources(Me.colIdDocument, "colIdDocument")
        Me.colIdDocument.FieldName = "IdDocument"
        Me.colIdDocument.Name = "colIdDocument"
        Me.colIdDocument.OptionsColumn.ReadOnly = True
        '
        'colIdDocumentTehnicalExam
        '
        resources.ApplyResources(Me.colIdDocumentTehnicalExam, "colIdDocumentTehnicalExam")
        Me.colIdDocumentTehnicalExam.FieldName = "IdDocumentTehnicalExam"
        Me.colIdDocumentTehnicalExam.Name = "colIdDocumentTehnicalExam"
        Me.colIdDocumentTehnicalExam.OptionsColumn.ReadOnly = True
        '
        'colIdDocumentsTrafficLicences
        '
        resources.ApplyResources(Me.colIdDocumentsTrafficLicences, "colIdDocumentsTrafficLicences")
        Me.colIdDocumentsTrafficLicences.FieldName = "IdDocumentsTrafficLicences"
        Me.colIdDocumentsTrafficLicences.Name = "colIdDocumentsTrafficLicences"
        Me.colIdDocumentsTrafficLicences.OptionsColumn.ReadOnly = True
        '
        'colIdDocumentIternationalDriveingLicence
        '
        resources.ApplyResources(Me.colIdDocumentIternationalDriveingLicence, "colIdDocumentIternationalDriveingLicence")
        Me.colIdDocumentIternationalDriveingLicence.FieldName = "IdDocumentIternationalDriveingLicence"
        Me.colIdDocumentIternationalDriveingLicence.Name = "colIdDocumentIternationalDriveingLicence"
        Me.colIdDocumentIternationalDriveingLicence.OptionsColumn.ReadOnly = True
        '
        'colIdDocumentPermisions
        '
        resources.ApplyResources(Me.colIdDocumentPermisions, "colIdDocumentPermisions")
        Me.colIdDocumentPermisions.FieldName = "IdDocumentPermisions"
        Me.colIdDocumentPermisions.Name = "colIdDocumentPermisions"
        Me.colIdDocumentPermisions.OptionsColumn.ReadOnly = True
        '
        'colIdPriceCatalog
        '
        resources.ApplyResources(Me.colIdPriceCatalog, "colIdPriceCatalog")
        Me.colIdPriceCatalog.FieldName = "IdPriceCatalog"
        Me.colIdPriceCatalog.Name = "colIdPriceCatalog"
        Me.colIdPriceCatalog.OptionsColumn.ReadOnly = True
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
        resources.ApplyResources(Me.colPrice, "colPrice")
        Me.colPrice.FieldName = "Price"
        Me.colPrice.Name = "colPrice"
        Me.colPrice.OptionsColumn.ReadOnly = True
        '
        'colPayed
        '
        resources.ApplyResources(Me.colPayed, "colPayed")
        Me.colPayed.FieldName = "Payed"
        Me.colPayed.Name = "colPayed"
        Me.colPayed.OptionsColumn.ReadOnly = True
        '
        'colShellNumber
        '
        resources.ApplyResources(Me.colShellNumber, "colShellNumber")
        Me.colShellNumber.FieldName = "ShellNumber"
        Me.colShellNumber.Name = "colShellNumber"
        Me.colShellNumber.OptionsColumn.ReadOnly = True
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
        Me.colCustomerName.FieldName = "CustomerName"
        Me.colCustomerName.Name = "colCustomerName"
        '
        'colName
        '
        resources.ApplyResources(Me.colName, "colName")
        Me.colName.FieldName = "Name"
        Me.colName.Name = "colName"
        '
        'colIdDDVCatalog
        '
        resources.ApplyResources(Me.colIdDDVCatalog, "colIdDDVCatalog")
        Me.colIdDDVCatalog.FieldName = "IdDDVCatalog"
        Me.colIdDDVCatalog.Name = "colIdDDVCatalog"
        '
        'colDDVName
        '
        resources.ApplyResources(Me.colDDVName, "colDDVName")
        Me.colDDVName.FieldName = "DDVName"
        Me.colDDVName.Name = "colDDVName"
        '
        'colDDVValue
        '
        resources.ApplyResources(Me.colDDVValue, "colDDVValue")
        Me.colDDVValue.FieldName = "DDVValue"
        Me.colDDVValue.Name = "colDDVValue"
        '
        'colDDVPriceValue
        '
        resources.ApplyResources(Me.colDDVPriceValue, "colDDVPriceValue")
        Me.colDDVPriceValue.FieldName = "DDVPriceValue"
        Me.colDDVPriceValue.Name = "colDDVPriceValue"
        '
        'colPriceWithDDV
        '
        resources.ApplyResources(Me.colPriceWithDDV, "colPriceWithDDV")
        Me.colPriceWithDDV.FieldName = "PriceWithDDV"
        Me.colPriceWithDDV.Name = "colPriceWithDDV"
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.SplitterItem1, Me.LayoutControlItem3})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(676, 559)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CustomerFinancialStateListGridControl
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(672, 516)
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.btnCreateBill
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 522)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(340, 33)
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'SplitterItem1
        '
        resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
        Me.SplitterItem1.Location = New System.Drawing.Point(0, 516)
        Me.SplitterItem1.Name = "SplitterItem1"
        Me.SplitterItem1.Size = New System.Drawing.Size(672, 6)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.btnExit
        resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
        Me.LayoutControlItem3.Location = New System.Drawing.Point(340, 522)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(332, 33)
        Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'uxFinancialStateCreatePaymentDocuments
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "uxFinancialStateCreatePaymentDocuments"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CustomerFinancialStateListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CustomerFinancialStateListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CustomerFinancialStateListGridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents CustomerFinancialStateListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdCustomerVehicleRelation As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdDocument As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdDocumentTehnicalExam As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdDocumentsTrafficLicences As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdDocumentIternationalDriveingLicence As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdDocumentPermisions As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdPriceCatalog As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNote As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPayed As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colShellNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerSurname As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerFirstName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdDDVCatalog As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDDVName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDDVValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDDVPriceValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPriceWithDDV As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCreateBill As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem

End Class
