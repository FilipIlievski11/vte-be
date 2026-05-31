<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxPaymentPivotReport
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxPaymentPivotReport))
    Me.PivotGridControl1 = New DevExpress.XtraPivotGrid.PivotGridControl
    Me.PrintPaymentDocumetnByIdDocumetnListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.fieldDokumentNaslov = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCustomerDisplayName = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldPrice = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCenaBezDDV = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldDDVIznos = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldVkupnoZaRed = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldDiscountValue = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldOstanataSumaZaPlakanje = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldPlatenaSuma = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldDocumentID = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldDocumentDate = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldIdPaymentType = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldPaymentType = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldIdCustomer = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldIdVehicle = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldIdDetal = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldIdPriceCatalog = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldPriceName = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldDiscount = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldDDV = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldNote = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCustomerSurname = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCustomerFirstName = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCityName = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCityZip = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldShellNumber = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldRegistrationNumber = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldStorno = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldStornoMk = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldPayed = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldPayedMk = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldOperatorName = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldDocumentNumber = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCommunityName = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldPaymentCategory = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldLivingAddressNumber = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCategory = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCategoryForPayments = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldVehicleMakerModel = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldVehicleMakerModelAddingTng = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldTNG = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldTehnicalExamsType = New DevExpress.XtraPivotGrid.PivotGridField
    Me.PrintingSystem1 = New DevExpress.XtraPrinting.PrintingSystem(Me.components)
    Me.PrintableComponentLink1 = New DevExpress.XtraPrinting.PrintableComponentLink(Me.components)
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.btnFullPlusRata = New DevExpress.XtraEditors.SimpleButton
    Me.btnShowRati = New DevExpress.XtraEditors.SimpleButton
    Me.rataDo = New DevExpress.XtraEditors.DateEdit
    Me.rataOd = New DevExpress.XtraEditors.DateEdit
    Me.LookUpEditCustomer = New VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
    Me.CustomersSearchListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.btnCustomer = New DevExpress.XtraEditors.SimpleButton
    Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
    Me.btnStornoSamoSmetka = New DevExpress.XtraEditors.SimpleButton
    Me.CommunityLookUpEdit = New DevExpress.XtraEditors.LookUpEdit
    Me.OperatorLookUpEdit = New DevExpress.XtraEditors.LookUpEdit
    Me.UsersListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.btnDogovor = New DevExpress.XtraEditors.SimpleButton
    Me.btnRata = New DevExpress.XtraEditors.SimpleButton
    Me.btnStorno = New DevExpress.XtraEditors.SimpleButton
    Me.btnPrintSmetka = New DevExpress.XtraEditors.SimpleButton
    Me.btnExit = New DevExpress.XtraEditors.SimpleButton
    Me.deEndDate = New DevExpress.XtraEditors.DateEdit
    Me.btnPrint = New DevExpress.XtraEditors.SimpleButton
    Me.ceRange = New DevExpress.XtraEditors.CheckEdit
    Me.deStartDate = New DevExpress.XtraEditors.DateEdit
    Me.btnRefresh = New DevExpress.XtraEditors.SimpleButton
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlGroup3 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlGroup4 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem14 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem13 = New DevExpress.XtraLayout.LayoutControlItem
    Me.RatiLayout = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem16 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem17 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlGroup5 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem15 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem18 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem19 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem20 = New DevExpress.XtraLayout.LayoutControlItem
    Me.CustomersListShortBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    CType(Me.PivotGridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PrintPaymentDocumetnByIdDocumetnListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.rataDo.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.rataDo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.rataOd.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.rataOd.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LookUpEditCustomer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CustomersSearchListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CommunityLookUpEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.OperatorLookUpEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.UsersListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deEndDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deEndDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.ceRange.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deStartDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deStartDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RatiLayout, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem17, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup5, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem19, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CustomersListShortBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'PivotGridControl1
    '
    Me.PivotGridControl1.AccessibleDescription = Nothing
    Me.PivotGridControl1.AccessibleName = Nothing
    resources.ApplyResources(Me.PivotGridControl1, "PivotGridControl1")
    Me.PivotGridControl1.BackgroundImage = Nothing
    Me.PivotGridControl1.Cursor = System.Windows.Forms.Cursors.Default
    Me.PivotGridControl1.DataSource = Me.PrintPaymentDocumetnByIdDocumetnListBindingSource
    Me.PivotGridControl1.Fields.AddRange(New DevExpress.XtraPivotGrid.PivotGridField() {Me.fieldDokumentNaslov, Me.fieldCustomerDisplayName, Me.fieldPrice, Me.fieldCenaBezDDV, Me.fieldDDVIznos, Me.fieldVkupnoZaRed, Me.fieldDiscountValue, Me.fieldOstanataSumaZaPlakanje, Me.fieldPlatenaSuma, Me.fieldDocumentID, Me.fieldDocumentDate, Me.fieldIdPaymentType, Me.fieldPaymentType, Me.fieldIdCustomer, Me.fieldIdVehicle, Me.fieldIdDetal, Me.fieldIdPriceCatalog, Me.fieldPriceName, Me.fieldDiscount, Me.fieldDDV, Me.fieldNote, Me.fieldCustomerSurname, Me.fieldCustomerFirstName, Me.fieldCityName, Me.fieldCityZip, Me.fieldShellNumber, Me.fieldRegistrationNumber, Me.fieldStorno, Me.fieldStornoMk, Me.fieldPayed, Me.fieldPayedMk, Me.fieldOperatorName, Me.fieldDocumentNumber, Me.fieldCommunityName, Me.fieldPaymentCategory, Me.fieldLivingAddressNumber, Me.fieldCategory, Me.fieldCategoryForPayments, Me.fieldVehicleMakerModel, Me.fieldVehicleMakerModelAddingTng, Me.fieldTNG, Me.fieldTehnicalExamsType})
    Me.PivotGridControl1.Name = "PivotGridControl1"
    Me.PivotGridControl1.OLAPConnectionString = Nothing
    Me.PivotGridControl1.OptionsBehavior.ApplyBestFitOnFieldDragging = True
    Me.PivotGridControl1.OptionsPrint.PageSettings.Landscape = True
    Me.PivotGridControl1.OptionsPrint.PageSettings.Margins = New System.Drawing.Printing.Margins(30, 30, 30, 30)
    Me.PivotGridControl1.OptionsPrint.PrintHeadersOnEveryPage = True
    Me.PivotGridControl1.OptionsPrint.UsePrintAppearance = True
    '
    'PrintPaymentDocumetnByIdDocumetnListBindingSource
    '
    Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.DataSource = GetType(VTE.Library.PrintPaymentDocumetnByIdDocumetnList)
    '
    'fieldDokumentNaslov
    '
    Me.fieldDokumentNaslov.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldDokumentNaslov.AreaIndex = 0
    resources.ApplyResources(Me.fieldDokumentNaslov, "fieldDokumentNaslov")
    Me.fieldDokumentNaslov.Name = "fieldDokumentNaslov"
    Me.fieldDokumentNaslov.Visible = False
    '
    'fieldCustomerDisplayName
    '
    Me.fieldCustomerDisplayName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
    Me.fieldCustomerDisplayName.AreaIndex = 1
    resources.ApplyResources(Me.fieldCustomerDisplayName, "fieldCustomerDisplayName")
    Me.fieldCustomerDisplayName.Name = "fieldCustomerDisplayName"
    '
    'fieldPrice
    '
    Me.fieldPrice.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldPrice.AreaIndex = 10
    resources.ApplyResources(Me.fieldPrice, "fieldPrice")
    Me.fieldPrice.Name = "fieldPrice"
    '
    'fieldCenaBezDDV
    '
    Me.fieldCenaBezDDV.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea
    Me.fieldCenaBezDDV.AreaIndex = 0
    resources.ApplyResources(Me.fieldCenaBezDDV, "fieldCenaBezDDV")
    Me.fieldCenaBezDDV.Name = "fieldCenaBezDDV"
    '
    'fieldDDVIznos
    '
    Me.fieldDDVIznos.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea
    Me.fieldDDVIznos.AreaIndex = 1
    resources.ApplyResources(Me.fieldDDVIznos, "fieldDDVIznos")
    Me.fieldDDVIznos.Name = "fieldDDVIznos"
    '
    'fieldVkupnoZaRed
    '
    Me.fieldVkupnoZaRed.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea
    Me.fieldVkupnoZaRed.AreaIndex = 2
    resources.ApplyResources(Me.fieldVkupnoZaRed, "fieldVkupnoZaRed")
    Me.fieldVkupnoZaRed.Name = "fieldVkupnoZaRed"
    '
    'fieldDiscountValue
    '
    Me.fieldDiscountValue.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldDiscountValue.AreaIndex = 4
    resources.ApplyResources(Me.fieldDiscountValue, "fieldDiscountValue")
    Me.fieldDiscountValue.Name = "fieldDiscountValue"
    '
    'fieldOstanataSumaZaPlakanje
    '
    Me.fieldOstanataSumaZaPlakanje.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
    Me.fieldOstanataSumaZaPlakanje.AreaIndex = 4
    resources.ApplyResources(Me.fieldOstanataSumaZaPlakanje, "fieldOstanataSumaZaPlakanje")
    Me.fieldOstanataSumaZaPlakanje.Name = "fieldOstanataSumaZaPlakanje"
    '
    'fieldPlatenaSuma
    '
    Me.fieldPlatenaSuma.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldPlatenaSuma.AreaIndex = 0
    resources.ApplyResources(Me.fieldPlatenaSuma, "fieldPlatenaSuma")
    Me.fieldPlatenaSuma.Name = "fieldPlatenaSuma"
    '
    'fieldDocumentID
    '
    Me.fieldDocumentID.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
    Me.fieldDocumentID.AreaIndex = 0
    resources.ApplyResources(Me.fieldDocumentID, "fieldDocumentID")
    Me.fieldDocumentID.Name = "fieldDocumentID"
    Me.fieldDocumentID.Visible = False
    '
    'fieldDocumentDate
    '
    Me.fieldDocumentDate.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldDocumentDate.AreaIndex = 12
    resources.ApplyResources(Me.fieldDocumentDate, "fieldDocumentDate")
    Me.fieldDocumentDate.Name = "fieldDocumentDate"
    '
    'fieldIdPaymentType
    '
    Me.fieldIdPaymentType.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldIdPaymentType.AreaIndex = 0
    resources.ApplyResources(Me.fieldIdPaymentType, "fieldIdPaymentType")
    Me.fieldIdPaymentType.Name = "fieldIdPaymentType"
    Me.fieldIdPaymentType.Visible = False
    '
    'fieldPaymentType
    '
    Me.fieldPaymentType.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
    Me.fieldPaymentType.AreaIndex = 3
    resources.ApplyResources(Me.fieldPaymentType, "fieldPaymentType")
    Me.fieldPaymentType.Name = "fieldPaymentType"
    '
    'fieldIdCustomer
    '
    Me.fieldIdCustomer.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldIdCustomer.AreaIndex = 0
    resources.ApplyResources(Me.fieldIdCustomer, "fieldIdCustomer")
    Me.fieldIdCustomer.Name = "fieldIdCustomer"
    Me.fieldIdCustomer.Visible = False
    '
    'fieldIdVehicle
    '
    Me.fieldIdVehicle.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldIdVehicle.AreaIndex = 1
    resources.ApplyResources(Me.fieldIdVehicle, "fieldIdVehicle")
    Me.fieldIdVehicle.Name = "fieldIdVehicle"
    '
    'fieldIdDetal
    '
    Me.fieldIdDetal.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldIdDetal.AreaIndex = 0
    resources.ApplyResources(Me.fieldIdDetal, "fieldIdDetal")
    Me.fieldIdDetal.Name = "fieldIdDetal"
    Me.fieldIdDetal.Visible = False
    '
    'fieldIdPriceCatalog
    '
    Me.fieldIdPriceCatalog.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldIdPriceCatalog.AreaIndex = 0
    resources.ApplyResources(Me.fieldIdPriceCatalog, "fieldIdPriceCatalog")
    Me.fieldIdPriceCatalog.Name = "fieldIdPriceCatalog"
    Me.fieldIdPriceCatalog.Visible = False
    '
    'fieldPriceName
    '
    Me.fieldPriceName.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldPriceName.AreaIndex = 9
    resources.ApplyResources(Me.fieldPriceName, "fieldPriceName")
    Me.fieldPriceName.Name = "fieldPriceName"
    '
    'fieldDiscount
    '
    Me.fieldDiscount.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldDiscount.AreaIndex = 7
    resources.ApplyResources(Me.fieldDiscount, "fieldDiscount")
    Me.fieldDiscount.Name = "fieldDiscount"
    '
    'fieldDDV
    '
    Me.fieldDDV.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldDDV.AreaIndex = 5
    resources.ApplyResources(Me.fieldDDV, "fieldDDV")
    Me.fieldDDV.Name = "fieldDDV"
    '
    'fieldNote
    '
    Me.fieldNote.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldNote.AreaIndex = 3
    resources.ApplyResources(Me.fieldNote, "fieldNote")
    Me.fieldNote.Name = "fieldNote"
    '
    'fieldCustomerSurname
    '
    Me.fieldCustomerSurname.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCustomerSurname.AreaIndex = 0
    resources.ApplyResources(Me.fieldCustomerSurname, "fieldCustomerSurname")
    Me.fieldCustomerSurname.Name = "fieldCustomerSurname"
    Me.fieldCustomerSurname.Visible = False
    '
    'fieldCustomerFirstName
    '
    Me.fieldCustomerFirstName.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCustomerFirstName.AreaIndex = 0
    resources.ApplyResources(Me.fieldCustomerFirstName, "fieldCustomerFirstName")
    Me.fieldCustomerFirstName.Name = "fieldCustomerFirstName"
    Me.fieldCustomerFirstName.Visible = False
    '
    'fieldCityName
    '
    Me.fieldCityName.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCityName.AreaIndex = 0
    resources.ApplyResources(Me.fieldCityName, "fieldCityName")
    Me.fieldCityName.Name = "fieldCityName"
    Me.fieldCityName.Visible = False
    '
    'fieldCityZip
    '
    Me.fieldCityZip.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCityZip.AreaIndex = 0
    resources.ApplyResources(Me.fieldCityZip, "fieldCityZip")
    Me.fieldCityZip.Name = "fieldCityZip"
    Me.fieldCityZip.Visible = False
    '
    'fieldShellNumber
    '
    Me.fieldShellNumber.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldShellNumber.AreaIndex = 11
    resources.ApplyResources(Me.fieldShellNumber, "fieldShellNumber")
    Me.fieldShellNumber.Name = "fieldShellNumber"
    '
    'fieldRegistrationNumber
    '
    Me.fieldRegistrationNumber.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldRegistrationNumber.AreaIndex = 6
    resources.ApplyResources(Me.fieldRegistrationNumber, "fieldRegistrationNumber")
    Me.fieldRegistrationNumber.Name = "fieldRegistrationNumber"
    '
    'fieldStorno
    '
    Me.fieldStorno.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldStorno.AreaIndex = 3
    resources.ApplyResources(Me.fieldStorno, "fieldStorno")
    Me.fieldStorno.Name = "fieldStorno"
    Me.fieldStorno.Visible = False
    '
    'fieldStornoMk
    '
    Me.fieldStornoMk.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldStornoMk.AreaIndex = 8
    resources.ApplyResources(Me.fieldStornoMk, "fieldStornoMk")
    Me.fieldStornoMk.Name = "fieldStornoMk"
    '
    'fieldPayed
    '
    Me.fieldPayed.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldPayed.AreaIndex = 0
    resources.ApplyResources(Me.fieldPayed, "fieldPayed")
    Me.fieldPayed.Name = "fieldPayed"
    Me.fieldPayed.Visible = False
    '
    'fieldPayedMk
    '
    Me.fieldPayedMk.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldPayedMk.AreaIndex = 18
    resources.ApplyResources(Me.fieldPayedMk, "fieldPayedMk")
    Me.fieldPayedMk.Name = "fieldPayedMk"
    '
    'fieldOperatorName
    '
    Me.fieldOperatorName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
    Me.fieldOperatorName.AreaIndex = 2
    resources.ApplyResources(Me.fieldOperatorName, "fieldOperatorName")
    Me.fieldOperatorName.Name = "fieldOperatorName"
    '
    'fieldDocumentNumber
    '
    Me.fieldDocumentNumber.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
    Me.fieldDocumentNumber.AreaIndex = 0
    resources.ApplyResources(Me.fieldDocumentNumber, "fieldDocumentNumber")
    Me.fieldDocumentNumber.Name = "fieldDocumentNumber"
    '
    'fieldCommunityName
    '
    Me.fieldCommunityName.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCommunityName.AreaIndex = 2
    resources.ApplyResources(Me.fieldCommunityName, "fieldCommunityName")
    Me.fieldCommunityName.Name = "fieldCommunityName"
    '
    'fieldPaymentCategory
    '
    Me.fieldPaymentCategory.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldPaymentCategory.AreaIndex = 13
    resources.ApplyResources(Me.fieldPaymentCategory, "fieldPaymentCategory")
    Me.fieldPaymentCategory.Name = "fieldPaymentCategory"
    '
    'fieldLivingAddressNumber
    '
    Me.fieldLivingAddressNumber.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldLivingAddressNumber.AreaIndex = 14
    resources.ApplyResources(Me.fieldLivingAddressNumber, "fieldLivingAddressNumber")
    Me.fieldLivingAddressNumber.Name = "fieldLivingAddressNumber"
    Me.fieldLivingAddressNumber.Visible = False
    '
    'fieldCategory
    '
    Me.fieldCategory.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCategory.AreaIndex = 15
    resources.ApplyResources(Me.fieldCategory, "fieldCategory")
    Me.fieldCategory.Name = "fieldCategory"
    '
    'fieldCategoryForPayments
    '
    Me.fieldCategoryForPayments.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCategoryForPayments.AreaIndex = 17
    resources.ApplyResources(Me.fieldCategoryForPayments, "fieldCategoryForPayments")
    Me.fieldCategoryForPayments.Name = "fieldCategoryForPayments"
    '
    'fieldVehicleMakerModel
    '
    Me.fieldVehicleMakerModel.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldVehicleMakerModel.AreaIndex = 18
    resources.ApplyResources(Me.fieldVehicleMakerModel, "fieldVehicleMakerModel")
    Me.fieldVehicleMakerModel.Name = "fieldVehicleMakerModel"
    Me.fieldVehicleMakerModel.Visible = False
    '
    'fieldVehicleMakerModelAddingTng
    '
    Me.fieldVehicleMakerModelAddingTng.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldVehicleMakerModelAddingTng.AreaIndex = 16
    resources.ApplyResources(Me.fieldVehicleMakerModelAddingTng, "fieldVehicleMakerModelAddingTng")
    Me.fieldVehicleMakerModelAddingTng.Name = "fieldVehicleMakerModelAddingTng"
    '
    'fieldTNG
    '
    Me.fieldTNG.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldTNG.AreaIndex = 14
    resources.ApplyResources(Me.fieldTNG, "fieldTNG")
    Me.fieldTNG.Name = "fieldTNG"
    '
    'fieldTehnicalExamsType
    '
    Me.fieldTehnicalExamsType.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldTehnicalExamsType.AreaIndex = 19
    resources.ApplyResources(Me.fieldTehnicalExamsType, "fieldTehnicalExamsType")
    Me.fieldTehnicalExamsType.Name = "fieldTehnicalExamsType"
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
    Me.PrintingSystem1.ShowMarginsWarning = False
    '
    'PrintableComponentLink1
    '
    Me.PrintableComponentLink1.Component = Me.PivotGridControl1
    Me.PrintableComponentLink1.CustomPaperSize = New System.Drawing.Size(0, 0)
    Me.PrintableComponentLink1.ImageStream = CType(resources.GetObject("PrintableComponentLink1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
    Me.PrintableComponentLink1.Landscape = True
    Me.PrintableComponentLink1.PageHeaderFooter = New DevExpress.XtraPrinting.PageHeaderFooter(New DevExpress.XtraPrinting.PageHeaderArea(New String() {Global.WinApp.My.Resources.Resources.String1, "   " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10), Global.WinApp.My.Resources.Resources.String1}, New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte)), DevExpress.XtraPrinting.BrickAlignment.Near), Nothing)
    Me.PrintableComponentLink1.PrintingSystem = Me.PrintingSystem1
    Me.PrintableComponentLink1.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart
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
    Me.LayoutControl1.Controls.Add(Me.btnFullPlusRata)
    Me.LayoutControl1.Controls.Add(Me.btnShowRati)
    Me.LayoutControl1.Controls.Add(Me.rataDo)
    Me.LayoutControl1.Controls.Add(Me.rataOd)
    Me.LayoutControl1.Controls.Add(Me.LookUpEditCustomer)
    Me.LayoutControl1.Controls.Add(Me.btnCustomer)
    Me.LayoutControl1.Controls.Add(Me.SimpleButton2)
    Me.LayoutControl1.Controls.Add(Me.btnStornoSamoSmetka)
    Me.LayoutControl1.Controls.Add(Me.CommunityLookUpEdit)
    Me.LayoutControl1.Controls.Add(Me.OperatorLookUpEdit)
    Me.LayoutControl1.Controls.Add(Me.btnDogovor)
    Me.LayoutControl1.Controls.Add(Me.btnRata)
    Me.LayoutControl1.Controls.Add(Me.btnStorno)
    Me.LayoutControl1.Controls.Add(Me.btnPrintSmetka)
    Me.LayoutControl1.Controls.Add(Me.btnExit)
    Me.LayoutControl1.Controls.Add(Me.deEndDate)
    Me.LayoutControl1.Controls.Add(Me.btnPrint)
    Me.LayoutControl1.Controls.Add(Me.ceRange)
    Me.LayoutControl1.Controls.Add(Me.deStartDate)
    Me.LayoutControl1.Controls.Add(Me.PivotGridControl1)
    Me.LayoutControl1.Controls.Add(Me.btnRefresh)
    Me.LayoutControl1.Font = Nothing
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.OptionsItemText.TextAlignMode = DevExpress.XtraLayout.TextAlignMode.AutoSize
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    '
    'btnFullPlusRata
    '
    Me.btnFullPlusRata.AccessibleDescription = Nothing
    Me.btnFullPlusRata.AccessibleName = Nothing
    resources.ApplyResources(Me.btnFullPlusRata, "btnFullPlusRata")
    Me.btnFullPlusRata.BackgroundImage = Nothing
    Me.btnFullPlusRata.Name = "btnFullPlusRata"
    Me.btnFullPlusRata.StyleController = Me.LayoutControl1
    '
    'btnShowRati
    '
    Me.btnShowRati.AccessibleDescription = Nothing
    Me.btnShowRati.AccessibleName = Nothing
    resources.ApplyResources(Me.btnShowRati, "btnShowRati")
    Me.btnShowRati.BackgroundImage = Nothing
    Me.btnShowRati.Name = "btnShowRati"
    Me.btnShowRati.StyleController = Me.LayoutControl1
    '
    'rataDo
    '
    resources.ApplyResources(Me.rataDo, "rataDo")
    Me.rataDo.BackgroundImage = Nothing
    Me.rataDo.EditValue = Nothing
    Me.rataDo.Name = "rataDo"
    Me.rataDo.Properties.AccessibleDescription = Nothing
    Me.rataDo.Properties.AccessibleName = Nothing
    Me.rataDo.Properties.AutoHeight = CType(resources.GetObject("rataDo.Properties.AutoHeight"), Boolean)
    Me.rataDo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("rataDo.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.rataDo.Properties.Mask.AutoComplete = CType(resources.GetObject("rataDo.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.rataDo.Properties.Mask.BeepOnError = CType(resources.GetObject("rataDo.Properties.Mask.BeepOnError"), Boolean)
    Me.rataDo.Properties.Mask.EditMask = resources.GetString("rataDo.Properties.Mask.EditMask")
    Me.rataDo.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("rataDo.Properties.Mask.IgnoreMaskBlank"), Boolean)
    Me.rataDo.Properties.Mask.MaskType = CType(resources.GetObject("rataDo.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.rataDo.Properties.Mask.PlaceHolder = CType(resources.GetObject("rataDo.Properties.Mask.PlaceHolder"), Char)
    Me.rataDo.Properties.Mask.SaveLiteral = CType(resources.GetObject("rataDo.Properties.Mask.SaveLiteral"), Boolean)
    Me.rataDo.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("rataDo.Properties.Mask.ShowPlaceHolders"), Boolean)
    Me.rataDo.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("rataDo.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.rataDo.Properties.VistaTimeProperties.AccessibleDescription = Nothing
    Me.rataDo.Properties.VistaTimeProperties.AccessibleName = Nothing
    Me.rataDo.Properties.VistaTimeProperties.AutoHeight = CType(resources.GetObject("rataDo.Properties.VistaTimeProperties.AutoHeight"), Boolean)
    Me.rataDo.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.rataDo.Properties.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("rataDo.Properties.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.rataDo.Properties.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("rataDo.Properties.VistaTimeProperties.Mask.BeepOnError"), Boolean)
    Me.rataDo.Properties.VistaTimeProperties.Mask.EditMask = resources.GetString("rataDo.Properties.VistaTimeProperties.Mask.EditMask")
    Me.rataDo.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("rataDo.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
    Me.rataDo.Properties.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("rataDo.Properties.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.rataDo.Properties.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("rataDo.Properties.VistaTimeProperties.Mask.PlaceHolder"), Char)
    Me.rataDo.Properties.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("rataDo.Properties.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
    Me.rataDo.Properties.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("rataDo.Properties.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
    Me.rataDo.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("rataDo.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.rataDo.StyleController = Me.LayoutControl1
    '
    'rataOd
    '
    resources.ApplyResources(Me.rataOd, "rataOd")
    Me.rataOd.BackgroundImage = Nothing
    Me.rataOd.EditValue = Nothing
    Me.rataOd.Name = "rataOd"
    Me.rataOd.Properties.AccessibleDescription = Nothing
    Me.rataOd.Properties.AccessibleName = Nothing
    Me.rataOd.Properties.AutoHeight = CType(resources.GetObject("rataOd.Properties.AutoHeight"), Boolean)
    Me.rataOd.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("rataOd.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.rataOd.Properties.Mask.AutoComplete = CType(resources.GetObject("rataOd.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.rataOd.Properties.Mask.BeepOnError = CType(resources.GetObject("rataOd.Properties.Mask.BeepOnError"), Boolean)
    Me.rataOd.Properties.Mask.EditMask = resources.GetString("rataOd.Properties.Mask.EditMask")
    Me.rataOd.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("rataOd.Properties.Mask.IgnoreMaskBlank"), Boolean)
    Me.rataOd.Properties.Mask.MaskType = CType(resources.GetObject("rataOd.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.rataOd.Properties.Mask.PlaceHolder = CType(resources.GetObject("rataOd.Properties.Mask.PlaceHolder"), Char)
    Me.rataOd.Properties.Mask.SaveLiteral = CType(resources.GetObject("rataOd.Properties.Mask.SaveLiteral"), Boolean)
    Me.rataOd.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("rataOd.Properties.Mask.ShowPlaceHolders"), Boolean)
    Me.rataOd.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("rataOd.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.rataOd.Properties.VistaTimeProperties.AccessibleDescription = Nothing
    Me.rataOd.Properties.VistaTimeProperties.AccessibleName = Nothing
    Me.rataOd.Properties.VistaTimeProperties.AutoHeight = CType(resources.GetObject("rataOd.Properties.VistaTimeProperties.AutoHeight"), Boolean)
    Me.rataOd.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.rataOd.Properties.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("rataOd.Properties.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.rataOd.Properties.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("rataOd.Properties.VistaTimeProperties.Mask.BeepOnError"), Boolean)
    Me.rataOd.Properties.VistaTimeProperties.Mask.EditMask = resources.GetString("rataOd.Properties.VistaTimeProperties.Mask.EditMask")
    Me.rataOd.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("rataOd.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
    Me.rataOd.Properties.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("rataOd.Properties.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.rataOd.Properties.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("rataOd.Properties.VistaTimeProperties.Mask.PlaceHolder"), Char)
    Me.rataOd.Properties.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("rataOd.Properties.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
    Me.rataOd.Properties.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("rataOd.Properties.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
    Me.rataOd.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("rataOd.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.rataOd.StyleController = Me.LayoutControl1
    '
    'LookUpEditCustomer
    '
    resources.ApplyResources(Me.LookUpEditCustomer, "LookUpEditCustomer")
    Me.LookUpEditCustomer.BackgroundImage = Nothing
    Me.LookUpEditCustomer.EditValue = Nothing
    Me.LookUpEditCustomer.Name = "LookUpEditCustomer"
    Me.LookUpEditCustomer.Properties.AccessibleDescription = Nothing
    Me.LookUpEditCustomer.Properties.AccessibleName = Nothing
    Me.LookUpEditCustomer.Properties.AutoHeight = CType(resources.GetObject("LookUpEditCustomer.Properties.AutoHeight"), Boolean)
    Me.LookUpEditCustomer.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCustomer.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCustomer.Properties.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditCustomer.Properties.Buttons2"), CType(resources.GetObject("LookUpEditCustomer.Properties.Buttons3"), Integer), CType(resources.GetObject("LookUpEditCustomer.Properties.Buttons4"), Boolean), CType(resources.GetObject("LookUpEditCustomer.Properties.Buttons5"), Boolean), CType(resources.GetObject("LookUpEditCustomer.Properties.Buttons6"), Boolean), CType(resources.GetObject("LookUpEditCustomer.Properties.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing)})
    Me.LookUpEditCustomer.Properties.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("MB", "MB", 20, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CustomerSurname", "CustomerSurname", 94, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
    Me.LookUpEditCustomer.Properties.DataSource = Me.CustomersSearchListBindingSource
    Me.LookUpEditCustomer.Properties.DisplayMember = "MB"
    Me.LookUpEditCustomer.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
    Me.LookUpEditCustomer.Properties.ValueMember = "Id"
    Me.LookUpEditCustomer.StyleController = Me.LayoutControl1
    '
    'CustomersSearchListBindingSource
    '
    Me.CustomersSearchListBindingSource.DataSource = GetType(VTE.Library.CustomersSearchList)
    '
    'btnCustomer
    '
    Me.btnCustomer.AccessibleDescription = Nothing
    Me.btnCustomer.AccessibleName = Nothing
    resources.ApplyResources(Me.btnCustomer, "btnCustomer")
    Me.btnCustomer.Appearance.Options.UseTextOptions = True
    Me.btnCustomer.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.btnCustomer.BackgroundImage = Nothing
    Me.btnCustomer.MaximumSize = New System.Drawing.Size(0, 44)
    Me.btnCustomer.MinimumSize = New System.Drawing.Size(0, 44)
    Me.btnCustomer.Name = "btnCustomer"
    Me.btnCustomer.StyleController = Me.LayoutControl1
    '
    'SimpleButton2
    '
    Me.SimpleButton2.AccessibleDescription = Nothing
    Me.SimpleButton2.AccessibleName = Nothing
    resources.ApplyResources(Me.SimpleButton2, "SimpleButton2")
    Me.SimpleButton2.BackgroundImage = Nothing
    Me.SimpleButton2.Name = "SimpleButton2"
    Me.SimpleButton2.StyleController = Me.LayoutControl1
    '
    'btnStornoSamoSmetka
    '
    Me.btnStornoSamoSmetka.AccessibleDescription = Nothing
    Me.btnStornoSamoSmetka.AccessibleName = Nothing
    resources.ApplyResources(Me.btnStornoSamoSmetka, "btnStornoSamoSmetka")
    Me.btnStornoSamoSmetka.BackgroundImage = Nothing
    Me.btnStornoSamoSmetka.Name = "btnStornoSamoSmetka"
    Me.btnStornoSamoSmetka.StyleController = Me.LayoutControl1
    '
    'CommunityLookUpEdit
    '
    resources.ApplyResources(Me.CommunityLookUpEdit, "CommunityLookUpEdit")
    Me.CommunityLookUpEdit.BackgroundImage = Nothing
    Me.CommunityLookUpEdit.EditValue = Nothing
    Me.CommunityLookUpEdit.Name = "CommunityLookUpEdit"
    Me.CommunityLookUpEdit.Properties.AccessibleDescription = Nothing
    Me.CommunityLookUpEdit.Properties.AccessibleName = Nothing
    Me.CommunityLookUpEdit.Properties.AutoHeight = CType(resources.GetObject("CommunityLookUpEdit.Properties.AutoHeight"), Boolean)
    Me.CommunityLookUpEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("CommunityLookUpEdit.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.CommunityLookUpEdit.Properties.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CommunityCode", "CommunityCode", 84, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CommunityName", "CommunityName", 86, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("RegistrationCode", "RegistrationCode", 89, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name", 33, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near)})
    Me.CommunityLookUpEdit.Properties.DisplayMember = "CommunityName"
    Me.CommunityLookUpEdit.Properties.ValueMember = "Id"
    Me.CommunityLookUpEdit.StyleController = Me.LayoutControl1
    '
    'OperatorLookUpEdit
    '
    resources.ApplyResources(Me.OperatorLookUpEdit, "OperatorLookUpEdit")
    Me.OperatorLookUpEdit.BackgroundImage = Nothing
    Me.OperatorLookUpEdit.EditValue = Nothing
    Me.OperatorLookUpEdit.Name = "OperatorLookUpEdit"
    Me.OperatorLookUpEdit.Properties.AccessibleDescription = Nothing
    Me.OperatorLookUpEdit.Properties.AccessibleName = Nothing
    Me.OperatorLookUpEdit.Properties.AutoHeight = CType(resources.GetObject("OperatorLookUpEdit.Properties.AutoHeight"), Boolean)
    Me.OperatorLookUpEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("OperatorLookUpEdit.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.OperatorLookUpEdit.Properties.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "ID", 30, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FullName", "FullName", 49, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FirstName", "FirstName", 54, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("SureName", "SureName", 55, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Address", "Address", 45, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("EMBG", "EMBG", 33, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("BLK", "BLK", 23, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
    Me.OperatorLookUpEdit.Properties.DataSource = Me.UsersListBindingSource
    Me.OperatorLookUpEdit.Properties.DisplayMember = "FullName"
    Me.OperatorLookUpEdit.Properties.ValueMember = "ID"
    Me.OperatorLookUpEdit.StyleController = Me.LayoutControl1
    '
    'UsersListBindingSource
    '
    Me.UsersListBindingSource.DataSource = GetType(VTE.Library.UsersList)
    '
    'btnDogovor
    '
    Me.btnDogovor.AccessibleDescription = Nothing
    Me.btnDogovor.AccessibleName = Nothing
    resources.ApplyResources(Me.btnDogovor, "btnDogovor")
    Me.btnDogovor.Appearance.Options.UseTextOptions = True
    Me.btnDogovor.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.btnDogovor.BackgroundImage = Nothing
    Me.btnDogovor.MaximumSize = New System.Drawing.Size(0, 44)
    Me.btnDogovor.Name = "btnDogovor"
    Me.btnDogovor.StyleController = Me.LayoutControl1
    '
    'btnRata
    '
    Me.btnRata.AccessibleDescription = Nothing
    Me.btnRata.AccessibleName = Nothing
    resources.ApplyResources(Me.btnRata, "btnRata")
    Me.btnRata.BackgroundImage = Nothing
    Me.btnRata.Name = "btnRata"
    Me.btnRata.StyleController = Me.LayoutControl1
    '
    'btnStorno
    '
    Me.btnStorno.AccessibleDescription = Nothing
    Me.btnStorno.AccessibleName = Nothing
    resources.ApplyResources(Me.btnStorno, "btnStorno")
    Me.btnStorno.Appearance.Options.UseTextOptions = True
    Me.btnStorno.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.btnStorno.BackgroundImage = Nothing
    Me.btnStorno.MaximumSize = New System.Drawing.Size(75, 0)
    Me.btnStorno.Name = "btnStorno"
    Me.btnStorno.StyleController = Me.LayoutControl1
    '
    'btnPrintSmetka
    '
    Me.btnPrintSmetka.AccessibleDescription = Nothing
    Me.btnPrintSmetka.AccessibleName = Nothing
    resources.ApplyResources(Me.btnPrintSmetka, "btnPrintSmetka")
    Me.btnPrintSmetka.Appearance.Options.UseTextOptions = True
    Me.btnPrintSmetka.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.btnPrintSmetka.BackgroundImage = Nothing
    Me.btnPrintSmetka.MaximumSize = New System.Drawing.Size(75, 0)
    Me.btnPrintSmetka.Name = "btnPrintSmetka"
    Me.btnPrintSmetka.StyleController = Me.LayoutControl1
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
    'deEndDate
    '
    resources.ApplyResources(Me.deEndDate, "deEndDate")
    Me.deEndDate.BackgroundImage = Nothing
    Me.deEndDate.EditValue = Nothing
    Me.deEndDate.Name = "deEndDate"
    Me.deEndDate.Properties.AccessibleDescription = Nothing
    Me.deEndDate.Properties.AccessibleName = Nothing
    Me.deEndDate.Properties.AutoHeight = CType(resources.GetObject("deEndDate.Properties.AutoHeight"), Boolean)
    Me.deEndDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("deEndDate.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.deEndDate.Properties.DisplayFormat.FormatString = "g"
    Me.deEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    Me.deEndDate.Properties.EditFormat.FormatString = "g"
    Me.deEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    Me.deEndDate.Properties.Mask.AutoComplete = CType(resources.GetObject("deEndDate.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.deEndDate.Properties.Mask.BeepOnError = CType(resources.GetObject("deEndDate.Properties.Mask.BeepOnError"), Boolean)
    Me.deEndDate.Properties.Mask.EditMask = resources.GetString("deEndDate.Properties.Mask.EditMask")
    Me.deEndDate.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("deEndDate.Properties.Mask.IgnoreMaskBlank"), Boolean)
    Me.deEndDate.Properties.Mask.MaskType = CType(resources.GetObject("deEndDate.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.deEndDate.Properties.Mask.PlaceHolder = CType(resources.GetObject("deEndDate.Properties.Mask.PlaceHolder"), Char)
    Me.deEndDate.Properties.Mask.SaveLiteral = CType(resources.GetObject("deEndDate.Properties.Mask.SaveLiteral"), Boolean)
    Me.deEndDate.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("deEndDate.Properties.Mask.ShowPlaceHolders"), Boolean)
    Me.deEndDate.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("deEndDate.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.deEndDate.Properties.VistaTimeProperties.AccessibleDescription = Nothing
    Me.deEndDate.Properties.VistaTimeProperties.AccessibleName = Nothing
    Me.deEndDate.Properties.VistaTimeProperties.AutoHeight = CType(resources.GetObject("deEndDate.Properties.VistaTimeProperties.AutoHeight"), Boolean)
    Me.deEndDate.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.deEndDate.Properties.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("deEndDate.Properties.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.deEndDate.Properties.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("deEndDate.Properties.VistaTimeProperties.Mask.BeepOnError"), Boolean)
    Me.deEndDate.Properties.VistaTimeProperties.Mask.EditMask = resources.GetString("deEndDate.Properties.VistaTimeProperties.Mask.EditMask")
    Me.deEndDate.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("deEndDate.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
    Me.deEndDate.Properties.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("deEndDate.Properties.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.deEndDate.Properties.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("deEndDate.Properties.VistaTimeProperties.Mask.PlaceHolder"), Char)
    Me.deEndDate.Properties.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("deEndDate.Properties.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
    Me.deEndDate.Properties.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("deEndDate.Properties.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
    Me.deEndDate.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("deEndDate.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.deEndDate.StyleController = Me.LayoutControl1
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
    'ceRange
    '
    resources.ApplyResources(Me.ceRange, "ceRange")
    Me.ceRange.BackgroundImage = Nothing
    Me.ceRange.Name = "ceRange"
    Me.ceRange.Properties.AccessibleDescription = Nothing
    Me.ceRange.Properties.AccessibleName = Nothing
    Me.ceRange.Properties.AutoHeight = CType(resources.GetObject("ceRange.Properties.AutoHeight"), Boolean)
    Me.ceRange.Properties.Caption = resources.GetString("ceRange.Properties.Caption")
    Me.ceRange.StyleController = Me.LayoutControl1
    '
    'deStartDate
    '
    resources.ApplyResources(Me.deStartDate, "deStartDate")
    Me.deStartDate.BackgroundImage = Nothing
    Me.deStartDate.EditValue = Nothing
    Me.deStartDate.Name = "deStartDate"
    Me.deStartDate.Properties.AccessibleDescription = Nothing
    Me.deStartDate.Properties.AccessibleName = Nothing
    Me.deStartDate.Properties.AutoHeight = CType(resources.GetObject("deStartDate.Properties.AutoHeight"), Boolean)
    Me.deStartDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("deStartDate.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.deStartDate.Properties.DisplayFormat.FormatString = "g"
    Me.deStartDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    Me.deStartDate.Properties.EditFormat.FormatString = "g"
    Me.deStartDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    Me.deStartDate.Properties.Mask.AutoComplete = CType(resources.GetObject("deStartDate.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.deStartDate.Properties.Mask.BeepOnError = CType(resources.GetObject("deStartDate.Properties.Mask.BeepOnError"), Boolean)
    Me.deStartDate.Properties.Mask.EditMask = resources.GetString("deStartDate.Properties.Mask.EditMask")
    Me.deStartDate.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("deStartDate.Properties.Mask.IgnoreMaskBlank"), Boolean)
    Me.deStartDate.Properties.Mask.MaskType = CType(resources.GetObject("deStartDate.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.deStartDate.Properties.Mask.PlaceHolder = CType(resources.GetObject("deStartDate.Properties.Mask.PlaceHolder"), Char)
    Me.deStartDate.Properties.Mask.SaveLiteral = CType(resources.GetObject("deStartDate.Properties.Mask.SaveLiteral"), Boolean)
    Me.deStartDate.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("deStartDate.Properties.Mask.ShowPlaceHolders"), Boolean)
    Me.deStartDate.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("deStartDate.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.deStartDate.Properties.VistaTimeProperties.AccessibleDescription = Nothing
    Me.deStartDate.Properties.VistaTimeProperties.AccessibleName = Nothing
    Me.deStartDate.Properties.VistaTimeProperties.AutoHeight = CType(resources.GetObject("deStartDate.Properties.VistaTimeProperties.AutoHeight"), Boolean)
    Me.deStartDate.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.deStartDate.Properties.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("deStartDate.Properties.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.deStartDate.Properties.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("deStartDate.Properties.VistaTimeProperties.Mask.BeepOnError"), Boolean)
    Me.deStartDate.Properties.VistaTimeProperties.Mask.EditMask = resources.GetString("deStartDate.Properties.VistaTimeProperties.Mask.EditMask")
    Me.deStartDate.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("deStartDate.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
    Me.deStartDate.Properties.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("deStartDate.Properties.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.deStartDate.Properties.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("deStartDate.Properties.VistaTimeProperties.Mask.PlaceHolder"), Char)
    Me.deStartDate.Properties.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("deStartDate.Properties.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
    Me.deStartDate.Properties.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("deStartDate.Properties.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
    Me.deStartDate.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("deStartDate.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.deStartDate.StyleController = Me.LayoutControl1
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
    'LayoutControlGroup1
    '
    resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem5, Me.LayoutControlGroup2, Me.LayoutControlItem6, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem8, Me.LayoutControlItem9, Me.LayoutControlItem10, Me.LayoutControlItem14, Me.LayoutControlItem13, Me.RatiLayout, Me.LayoutControlItem16, Me.LayoutControlItem17, Me.LayoutControlGroup5, Me.LayoutControlItem20})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(886, 487)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.TextVisible = False
    '
    'LayoutControlItem5
    '
    Me.LayoutControlItem5.Control = Me.btnExit
    resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
    Me.LayoutControlItem5.Location = New System.Drawing.Point(823, 0)
    Me.LayoutControlItem5.MaxSize = New System.Drawing.Size(0, 55)
    Me.LayoutControlItem5.MinSize = New System.Drawing.Size(59, 55)
    Me.LayoutControlItem5.Name = "LayoutControlItem5"
    Me.LayoutControlItem5.Size = New System.Drawing.Size(59, 55)
    Me.LayoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem5.TextToControlDistance = 0
    Me.LayoutControlItem5.TextVisible = False
    '
    'LayoutControlGroup2
    '
    resources.ApplyResources(Me.LayoutControlGroup2, "LayoutControlGroup2")
    Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem7, Me.LayoutControlGroup3, Me.LayoutControlGroup4})
    Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
    Me.LayoutControlGroup2.Size = New System.Drawing.Size(441, 110)
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.deStartDate
    resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem1.MaxSize = New System.Drawing.Size(163, 31)
    Me.LayoutControlItem1.MinSize = New System.Drawing.Size(163, 31)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(163, 31)
    Me.LayoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(15, 20)
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.deEndDate
    resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
    Me.LayoutControlItem2.Location = New System.Drawing.Point(163, 0)
    Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(164, 31)
    Me.LayoutControlItem2.MinSize = New System.Drawing.Size(164, 31)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(164, 31)
    Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(14, 20)
    '
    'LayoutControlItem7
    '
    Me.LayoutControlItem7.Control = Me.ceRange
    resources.ApplyResources(Me.LayoutControlItem7, "LayoutControlItem7")
    Me.LayoutControlItem7.Location = New System.Drawing.Point(327, 0)
    Me.LayoutControlItem7.MaxSize = New System.Drawing.Size(108, 29)
    Me.LayoutControlItem7.MinSize = New System.Drawing.Size(108, 29)
    Me.LayoutControlItem7.Name = "LayoutControlItem7"
    Me.LayoutControlItem7.Size = New System.Drawing.Size(108, 31)
    Me.LayoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem7.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem7.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem7.TextToControlDistance = 0
    Me.LayoutControlItem7.TextVisible = False
    '
    'LayoutControlGroup3
    '
    resources.ApplyResources(Me.LayoutControlGroup3, "LayoutControlGroup3")
    Me.LayoutControlGroup3.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem11})
    Me.LayoutControlGroup3.Location = New System.Drawing.Point(0, 31)
    Me.LayoutControlGroup3.Name = "LayoutControlGroup3"
    Me.LayoutControlGroup3.Size = New System.Drawing.Size(217, 55)
    Me.LayoutControlGroup3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    '
    'LayoutControlItem11
    '
    Me.LayoutControlItem11.Control = Me.OperatorLookUpEdit
    resources.ApplyResources(Me.LayoutControlItem11, "LayoutControlItem11")
    Me.LayoutControlItem11.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem11.Name = "LayoutControlItem11"
    Me.LayoutControlItem11.Size = New System.Drawing.Size(211, 31)
    Me.LayoutControlItem11.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.LayoutControlItem11.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem11.TextSize = New System.Drawing.Size(48, 20)
    '
    'LayoutControlGroup4
    '
    resources.ApplyResources(Me.LayoutControlGroup4, "LayoutControlGroup4")
    Me.LayoutControlGroup4.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem12})
    Me.LayoutControlGroup4.Location = New System.Drawing.Point(217, 31)
    Me.LayoutControlGroup4.Name = "LayoutControlGroup4"
    Me.LayoutControlGroup4.Size = New System.Drawing.Size(218, 55)
    Me.LayoutControlGroup4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    '
    'LayoutControlItem12
    '
    Me.LayoutControlItem12.Control = Me.CommunityLookUpEdit
    resources.ApplyResources(Me.LayoutControlItem12, "LayoutControlItem12")
    Me.LayoutControlItem12.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem12.Name = "LayoutControlItem12"
    Me.LayoutControlItem12.Size = New System.Drawing.Size(212, 31)
    Me.LayoutControlItem12.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.LayoutControlItem12.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem12.TextSize = New System.Drawing.Size(57, 20)
    '
    'LayoutControlItem6
    '
    Me.LayoutControlItem6.Control = Me.PivotGridControl1
    resources.ApplyResources(Me.LayoutControlItem6, "LayoutControlItem6")
    Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 198)
    Me.LayoutControlItem6.Name = "LayoutControlItem6"
    Me.LayoutControlItem6.Size = New System.Drawing.Size(882, 285)
    Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem6.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem6.TextToControlDistance = 0
    Me.LayoutControlItem6.TextVisible = False
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.btnRefresh
    resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
    Me.LayoutControlItem3.Location = New System.Drawing.Point(441, 0)
    Me.LayoutControlItem3.MaxSize = New System.Drawing.Size(0, 55)
    Me.LayoutControlItem3.MinSize = New System.Drawing.Size(63, 55)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(114, 55)
    Me.LayoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem3.TextToControlDistance = 0
    Me.LayoutControlItem3.TextVisible = False
    '
    'LayoutControlItem4
    '
    Me.LayoutControlItem4.Control = Me.btnPrint
    resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
    Me.LayoutControlItem4.Location = New System.Drawing.Point(555, 0)
    Me.LayoutControlItem4.MaxSize = New System.Drawing.Size(0, 55)
    Me.LayoutControlItem4.MinSize = New System.Drawing.Size(61, 55)
    Me.LayoutControlItem4.Name = "LayoutControlItem4"
    Me.LayoutControlItem4.Size = New System.Drawing.Size(109, 55)
    Me.LayoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem4.TextToControlDistance = 0
    Me.LayoutControlItem4.TextVisible = False
    '
    'LayoutControlItem8
    '
    Me.LayoutControlItem8.Control = Me.btnPrintSmetka
    resources.ApplyResources(Me.LayoutControlItem8, "LayoutControlItem8")
    Me.LayoutControlItem8.Location = New System.Drawing.Point(664, 0)
    Me.LayoutControlItem8.MinSize = New System.Drawing.Size(70, 33)
    Me.LayoutControlItem8.Name = "LayoutControlItem8"
    Me.LayoutControlItem8.Size = New System.Drawing.Size(89, 55)
    Me.LayoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem8.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem8.TextToControlDistance = 0
    Me.LayoutControlItem8.TextVisible = False
    '
    'LayoutControlItem9
    '
    Me.LayoutControlItem9.Control = Me.btnStorno
    resources.ApplyResources(Me.LayoutControlItem9, "LayoutControlItem9")
    Me.LayoutControlItem9.Location = New System.Drawing.Point(753, 0)
    Me.LayoutControlItem9.MinSize = New System.Drawing.Size(70, 33)
    Me.LayoutControlItem9.Name = "LayoutControlItem9"
    Me.LayoutControlItem9.Size = New System.Drawing.Size(70, 55)
    Me.LayoutControlItem9.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem9.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem9.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem9.TextToControlDistance = 0
    Me.LayoutControlItem9.TextVisible = False
    '
    'LayoutControlItem10
    '
    Me.LayoutControlItem10.Control = Me.btnDogovor
    resources.ApplyResources(Me.LayoutControlItem10, "LayoutControlItem10")
    Me.LayoutControlItem10.Location = New System.Drawing.Point(555, 55)
    Me.LayoutControlItem10.Name = "LayoutControlItem10"
    Me.LayoutControlItem10.Size = New System.Drawing.Size(109, 66)
    Me.LayoutControlItem10.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem10.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem10.TextToControlDistance = 0
    Me.LayoutControlItem10.TextVisible = False
    '
    'LayoutControlItem14
    '
    Me.LayoutControlItem14.Control = Me.SimpleButton2
    resources.ApplyResources(Me.LayoutControlItem14, "LayoutControlItem14")
    Me.LayoutControlItem14.Location = New System.Drawing.Point(757, 55)
    Me.LayoutControlItem14.Name = "LayoutControlItem14"
    Me.LayoutControlItem14.Size = New System.Drawing.Size(125, 33)
    Me.LayoutControlItem14.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem14.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem14.TextToControlDistance = 0
    Me.LayoutControlItem14.TextVisible = False
    '
    'LayoutControlItem13
    '
    Me.LayoutControlItem13.Control = Me.btnStornoSamoSmetka
    resources.ApplyResources(Me.LayoutControlItem13, "LayoutControlItem13")
    Me.LayoutControlItem13.Location = New System.Drawing.Point(757, 88)
    Me.LayoutControlItem13.Name = "LayoutControlItem13"
    Me.LayoutControlItem13.Size = New System.Drawing.Size(125, 33)
    Me.LayoutControlItem13.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem13.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem13.TextToControlDistance = 0
    Me.LayoutControlItem13.TextVisible = False
    '
    'RatiLayout
    '
    Me.RatiLayout.Control = Me.btnRata
    resources.ApplyResources(Me.RatiLayout, "RatiLayout")
    Me.RatiLayout.Location = New System.Drawing.Point(664, 55)
    Me.RatiLayout.MaxSize = New System.Drawing.Size(0, 55)
    Me.RatiLayout.MinSize = New System.Drawing.Size(92, 55)
    Me.RatiLayout.Name = "RatiLayout"
    Me.RatiLayout.Size = New System.Drawing.Size(93, 66)
    Me.RatiLayout.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.RatiLayout.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.RatiLayout.TextLocation = DevExpress.Utils.Locations.Left
    Me.RatiLayout.TextSize = New System.Drawing.Size(0, 0)
    Me.RatiLayout.TextToControlDistance = 0
    Me.RatiLayout.TextVisible = False
    '
    'LayoutControlItem16
    '
    Me.LayoutControlItem16.Control = Me.btnCustomer
    resources.ApplyResources(Me.LayoutControlItem16, "LayoutControlItem16")
    Me.LayoutControlItem16.Location = New System.Drawing.Point(441, 55)
    Me.LayoutControlItem16.Name = "LayoutControlItem16"
    Me.LayoutControlItem16.Size = New System.Drawing.Size(114, 66)
    Me.LayoutControlItem16.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem16.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem16.TextToControlDistance = 0
    Me.LayoutControlItem16.TextVisible = False
    '
    'LayoutControlItem17
    '
    Me.LayoutControlItem17.Control = Me.LookUpEditCustomer
    resources.ApplyResources(Me.LayoutControlItem17, "LayoutControlItem17")
    Me.LayoutControlItem17.Location = New System.Drawing.Point(0, 167)
    Me.LayoutControlItem17.Name = "LayoutControlItem17"
    Me.LayoutControlItem17.Size = New System.Drawing.Size(441, 31)
    Me.LayoutControlItem17.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem17.TextSize = New System.Drawing.Size(174, 20)
    '
    'LayoutControlGroup5
    '
    resources.ApplyResources(Me.LayoutControlGroup5, "LayoutControlGroup5")
    Me.LayoutControlGroup5.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem15, Me.LayoutControlItem18, Me.LayoutControlItem19})
    Me.LayoutControlGroup5.Location = New System.Drawing.Point(0, 110)
    Me.LayoutControlGroup5.Name = "LayoutControlGroup5"
    Me.LayoutControlGroup5.Size = New System.Drawing.Size(441, 57)
    '
    'LayoutControlItem15
    '
    Me.LayoutControlItem15.Control = Me.rataOd
    resources.ApplyResources(Me.LayoutControlItem15, "LayoutControlItem15")
    Me.LayoutControlItem15.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem15.Name = "LayoutControlItem15"
    Me.LayoutControlItem15.Size = New System.Drawing.Size(175, 33)
    Me.LayoutControlItem15.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem15.TextSize = New System.Drawing.Size(15, 20)
    '
    'LayoutControlItem18
    '
    Me.LayoutControlItem18.Control = Me.rataDo
    resources.ApplyResources(Me.LayoutControlItem18, "LayoutControlItem18")
    Me.LayoutControlItem18.Location = New System.Drawing.Point(175, 0)
    Me.LayoutControlItem18.Name = "LayoutControlItem18"
    Me.LayoutControlItem18.Size = New System.Drawing.Size(165, 33)
    Me.LayoutControlItem18.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem18.TextSize = New System.Drawing.Size(14, 20)
    '
    'LayoutControlItem19
    '
    Me.LayoutControlItem19.Control = Me.btnShowRati
    resources.ApplyResources(Me.LayoutControlItem19, "LayoutControlItem19")
    Me.LayoutControlItem19.Location = New System.Drawing.Point(340, 0)
    Me.LayoutControlItem19.Name = "LayoutControlItem19"
    Me.LayoutControlItem19.Size = New System.Drawing.Size(95, 33)
    Me.LayoutControlItem19.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem19.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem19.TextToControlDistance = 0
    Me.LayoutControlItem19.TextVisible = False
    '
    'LayoutControlItem20
    '
    Me.LayoutControlItem20.Control = Me.btnFullPlusRata
    resources.ApplyResources(Me.LayoutControlItem20, "LayoutControlItem20")
    Me.LayoutControlItem20.Location = New System.Drawing.Point(441, 121)
    Me.LayoutControlItem20.Name = "LayoutControlItem20"
    Me.LayoutControlItem20.Size = New System.Drawing.Size(441, 77)
    Me.LayoutControlItem20.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem20.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem20.TextToControlDistance = 0
    Me.LayoutControlItem20.TextVisible = False
    '
    'CustomersListShortBindingSource
    '
    Me.CustomersListShortBindingSource.DataSource = GetType(VTE.Library.CustomersListShort)
    '
    'uxPaymentPivotReport
    '
    Me.AccessibleDescription = Nothing
    Me.AccessibleName = Nothing
    resources.ApplyResources(Me, "$this")
    Me.BackgroundImage = Nothing
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "uxPaymentPivotReport"
    Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
    CType(Me.PivotGridControl1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PrintPaymentDocumetnByIdDocumetnListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.rataDo.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.rataDo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.rataOd.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.rataOd.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LookUpEditCustomer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CustomersSearchListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CommunityLookUpEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.OperatorLookUpEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.UsersListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deEndDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deEndDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.ceRange.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deStartDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deStartDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RatiLayout, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem17, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup5, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem19, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CustomersListShortBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents PivotGridControl1 As DevExpress.XtraPivotGrid.PivotGridControl
  Friend WithEvents PrintPaymentDocumetnByIdDocumetnListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents PrintingSystem1 As DevExpress.XtraPrinting.PrintingSystem
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents btnRefresh As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnPrint As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents deEndDate As DevExpress.XtraEditors.DateEdit
  Friend WithEvents deStartDate As DevExpress.XtraEditors.DateEdit
  Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents ceRange As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents PrintableComponentLink1 As DevExpress.XtraPrinting.PrintableComponentLink
  Friend WithEvents btnPrintSmetka As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents btnStorno As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents btnRata As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents RatiLayout As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents btnDogovor As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents CommunityLookUpEdit As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents OperatorLookUpEdit As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlGroup3 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents UsersListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents LayoutControlGroup4 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents btnStornoSamoSmetka As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem13 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem14 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents btnCustomer As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem16 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents CustomersListShortBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents LookUpEditCustomer As VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
  Friend WithEvents LayoutControlItem17 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents rataDo As DevExpress.XtraEditors.DateEdit
  Friend WithEvents rataOd As DevExpress.XtraEditors.DateEdit
  Friend WithEvents LayoutControlGroup5 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItem15 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem18 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents btnShowRati As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem19 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents fieldDokumentNaslov As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCustomerDisplayName As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldPrice As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCenaBezDDV As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldDDVIznos As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldVkupnoZaRed As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldDiscountValue As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldOstanataSumaZaPlakanje As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldPlatenaSuma As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldDocumentID As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldDocumentDate As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldIdPaymentType As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldPaymentType As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldIdCustomer As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldIdVehicle As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldIdDetal As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldIdPriceCatalog As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldPriceName As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldDiscount As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldDDV As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldNote As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCustomerSurname As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCustomerFirstName As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCityName As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCityZip As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldShellNumber As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldRegistrationNumber As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldStorno As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldStornoMk As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldPayed As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldPayedMk As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldOperatorName As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldDocumentNumber As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCommunityName As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldPaymentCategory As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldLivingAddressNumber As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCategory As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCategoryForPayments As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldVehicleMakerModel As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldVehicleMakerModelAddingTng As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldTNG As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents btnFullPlusRata As DevExpress.XtraEditors.SimpleButton
 Friend WithEvents LayoutControlItem20 As DevExpress.XtraLayout.LayoutControlItem
 Friend WithEvents CustomersSearchListBindingSource As System.Windows.Forms.BindingSource
 Friend WithEvents fieldTehnicalExamsType As DevExpress.XtraPivotGrid.PivotGridField

End Class
