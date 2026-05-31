<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxCustomerPivotReport
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxCustomerPivotReport))
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.btnCancel = New DevExpress.XtraEditors.SimpleButton
  Me.btnPrint = New DevExpress.XtraEditors.SimpleButton
  Me.PivotGridControl1 = New DevExpress.XtraPivotGrid.PivotGridControl
  Me.PrintCustomerPivotReportListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.fieldMB = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldCustomerSurname = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldCustomerFirstName = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldPhoneNumber = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldFax = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldStreetName = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldCityName = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldCommunityCode = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldCommunityName = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldLivingAddress = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldLivingAddressNumber = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldDateOfBirth = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldIsCompany = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldOccupation = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldWorksInCompany = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldBusinessTypeDescription = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldEMail = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldPassportNumber = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldCountryName = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldIsCompanyString = New DevExpress.XtraPivotGrid.PivotGridField
  Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
  Me.PrintingSystem1 = New DevExpress.XtraPrinting.PrintingSystem(Me.components)
  Me.PrintableComponentLink1 = New DevExpress.XtraPrinting.PrintableComponentLink(Me.components)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.LayoutControl1.SuspendLayout()
  CType(Me.PivotGridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.PrintCustomerPivotReportListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'LayoutControl1
  '
  Me.LayoutControl1.AccessibleDescription = Nothing
  Me.LayoutControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
  Me.LayoutControl1.BackgroundImage = Nothing
  Me.LayoutControl1.Controls.Add(Me.btnCancel)
  Me.LayoutControl1.Controls.Add(Me.btnPrint)
  Me.LayoutControl1.Controls.Add(Me.PivotGridControl1)
  Me.LayoutControl1.Font = Nothing
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
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
  'btnPrint
  '
  Me.btnPrint.AccessibleDescription = Nothing
  Me.btnPrint.AccessibleName = Nothing
  resources.ApplyResources(Me.btnPrint, "btnPrint")
  Me.btnPrint.BackgroundImage = Nothing
  Me.btnPrint.Name = "btnPrint"
  Me.btnPrint.StyleController = Me.LayoutControl1
  '
  'PivotGridControl1
  '
  Me.PivotGridControl1.AccessibleDescription = Nothing
  Me.PivotGridControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.PivotGridControl1, "PivotGridControl1")
  Me.PivotGridControl1.BackgroundImage = Nothing
  Me.PivotGridControl1.Cursor = System.Windows.Forms.Cursors.Default
  Me.PivotGridControl1.DataSource = Me.PrintCustomerPivotReportListBindingSource
  Me.PivotGridControl1.Fields.AddRange(New DevExpress.XtraPivotGrid.PivotGridField() {Me.fieldMB, Me.fieldCustomerSurname, Me.fieldCustomerFirstName, Me.fieldPhoneNumber, Me.fieldFax, Me.fieldStreetName, Me.fieldCityName, Me.fieldCommunityCode, Me.fieldCommunityName, Me.fieldLivingAddress, Me.fieldLivingAddressNumber, Me.fieldDateOfBirth, Me.fieldIsCompany, Me.fieldOccupation, Me.fieldWorksInCompany, Me.fieldBusinessTypeDescription, Me.fieldEMail, Me.fieldPassportNumber, Me.fieldCountryName, Me.fieldIsCompanyString})
  Me.PivotGridControl1.Name = "PivotGridControl1"
  Me.PivotGridControl1.OLAPConnectionString = Nothing
  '
  'PrintCustomerPivotReportListBindingSource
  '
  Me.PrintCustomerPivotReportListBindingSource.DataSource = GetType(VTE.Library.printCustomerPivotReportList)
  '
  'fieldMB
  '
  Me.fieldMB.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldMB.AreaIndex = 3
  resources.ApplyResources(Me.fieldMB, "fieldMB")
  Me.fieldMB.Name = "fieldMB"
  '
  'fieldCustomerSurname
  '
  Me.fieldCustomerSurname.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldCustomerSurname.AreaIndex = 2
  resources.ApplyResources(Me.fieldCustomerSurname, "fieldCustomerSurname")
  Me.fieldCustomerSurname.Name = "fieldCustomerSurname"
  '
  'fieldCustomerFirstName
  '
  Me.fieldCustomerFirstName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldCustomerFirstName.AreaIndex = 1
  resources.ApplyResources(Me.fieldCustomerFirstName, "fieldCustomerFirstName")
  Me.fieldCustomerFirstName.Name = "fieldCustomerFirstName"
  Me.fieldCustomerFirstName.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Count
  '
  'fieldPhoneNumber
  '
  Me.fieldPhoneNumber.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldPhoneNumber.AreaIndex = 7
  resources.ApplyResources(Me.fieldPhoneNumber, "fieldPhoneNumber")
  Me.fieldPhoneNumber.Name = "fieldPhoneNumber"
  '
  'fieldFax
  '
  Me.fieldFax.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldFax.AreaIndex = 1
  resources.ApplyResources(Me.fieldFax, "fieldFax")
  Me.fieldFax.Name = "fieldFax"
  '
  'fieldStreetName
  '
  Me.fieldStreetName.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldStreetName.AreaIndex = 5
  resources.ApplyResources(Me.fieldStreetName, "fieldStreetName")
  Me.fieldStreetName.Name = "fieldStreetName"
  Me.fieldStreetName.Visible = False
  '
  'fieldCityName
  '
  Me.fieldCityName.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea
  Me.fieldCityName.AreaIndex = 0
  resources.ApplyResources(Me.fieldCityName, "fieldCityName")
  Me.fieldCityName.Name = "fieldCityName"
  Me.fieldCityName.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Count
  '
  'fieldCommunityCode
  '
  Me.fieldCommunityCode.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldCommunityCode.AreaIndex = 4
  resources.ApplyResources(Me.fieldCommunityCode, "fieldCommunityCode")
  Me.fieldCommunityCode.Name = "fieldCommunityCode"
  '
  'fieldCommunityName
  '
  Me.fieldCommunityName.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea
  Me.fieldCommunityName.AreaIndex = 0
  resources.ApplyResources(Me.fieldCommunityName, "fieldCommunityName")
  Me.fieldCommunityName.Name = "fieldCommunityName"
  Me.fieldCommunityName.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Count
  '
  'fieldLivingAddress
  '
  Me.fieldLivingAddress.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldLivingAddress.AreaIndex = 4
  resources.ApplyResources(Me.fieldLivingAddress, "fieldLivingAddress")
  Me.fieldLivingAddress.Name = "fieldLivingAddress"
  '
  'fieldLivingAddressNumber
  '
  Me.fieldLivingAddressNumber.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldLivingAddressNumber.AreaIndex = 9
  resources.ApplyResources(Me.fieldLivingAddressNumber, "fieldLivingAddressNumber")
  Me.fieldLivingAddressNumber.Name = "fieldLivingAddressNumber"
  Me.fieldLivingAddressNumber.Visible = False
  '
  'fieldDateOfBirth
  '
  Me.fieldDateOfBirth.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldDateOfBirth.AreaIndex = 2
  resources.ApplyResources(Me.fieldDateOfBirth, "fieldDateOfBirth")
  Me.fieldDateOfBirth.Name = "fieldDateOfBirth"
  '
  'fieldIsCompany
  '
  Me.fieldIsCompany.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea
  Me.fieldIsCompany.AreaIndex = 1
  resources.ApplyResources(Me.fieldIsCompany, "fieldIsCompany")
  Me.fieldIsCompany.Name = "fieldIsCompany"
  Me.fieldIsCompany.Visible = False
  '
  'fieldOccupation
  '
  Me.fieldOccupation.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldOccupation.AreaIndex = 5
  resources.ApplyResources(Me.fieldOccupation, "fieldOccupation")
  Me.fieldOccupation.Name = "fieldOccupation"
  '
  'fieldWorksInCompany
  '
  Me.fieldWorksInCompany.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldWorksInCompany.AreaIndex = 0
  resources.ApplyResources(Me.fieldWorksInCompany, "fieldWorksInCompany")
  Me.fieldWorksInCompany.Name = "fieldWorksInCompany"
  '
  'fieldBusinessTypeDescription
  '
  Me.fieldBusinessTypeDescription.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldBusinessTypeDescription.AreaIndex = 6
  resources.ApplyResources(Me.fieldBusinessTypeDescription, "fieldBusinessTypeDescription")
  Me.fieldBusinessTypeDescription.Name = "fieldBusinessTypeDescription"
  '
  'fieldEMail
  '
  Me.fieldEMail.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldEMail.AreaIndex = 3
  resources.ApplyResources(Me.fieldEMail, "fieldEMail")
  Me.fieldEMail.Name = "fieldEMail"
  '
  'fieldPassportNumber
  '
  Me.fieldPassportNumber.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldPassportNumber.AreaIndex = 5
  resources.ApplyResources(Me.fieldPassportNumber, "fieldPassportNumber")
  Me.fieldPassportNumber.Name = "fieldPassportNumber"
  '
  'fieldCountryName
  '
  Me.fieldCountryName.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldCountryName.AreaIndex = 6
  resources.ApplyResources(Me.fieldCountryName, "fieldCountryName")
  Me.fieldCountryName.Name = "fieldCountryName"
  '
  'fieldIsCompanyString
  '
  Me.fieldIsCompanyString.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldIsCompanyString.AreaIndex = 0
  resources.ApplyResources(Me.fieldIsCompanyString, "fieldIsCompanyString")
  Me.fieldIsCompanyString.Name = "fieldIsCompanyString"
  '
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(768, 534)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.PivotGridControl1
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 33)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(764, 497)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem1.TextToControlDistance = 0
  Me.LayoutControlItem1.TextVisible = False
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.btnPrint
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(382, 33)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem2.TextToControlDistance = 0
  Me.LayoutControlItem2.TextVisible = False
  '
  'LayoutControlItem3
  '
  Me.LayoutControlItem3.Control = Me.btnCancel
  resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
  Me.LayoutControlItem3.Location = New System.Drawing.Point(382, 0)
  Me.LayoutControlItem3.Name = "LayoutControlItem3"
  Me.LayoutControlItem3.Size = New System.Drawing.Size(382, 33)
  Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem3.TextToControlDistance = 0
  Me.LayoutControlItem3.TextVisible = False
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
  Me.PrintableComponentLink1.Component = Me.PivotGridControl1
  Me.PrintableComponentLink1.CustomPaperSize = New System.Drawing.Size(0, 0)
  Me.PrintableComponentLink1.ImageStream = CType(resources.GetObject("PrintableComponentLink1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
  Me.PrintableComponentLink1.Landscape = True
  Me.PrintableComponentLink1.PaperKind = System.Drawing.Printing.PaperKind.A4
  Me.PrintableComponentLink1.PrintingSystem = Me.PrintingSystem1
  Me.PrintableComponentLink1.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart
  '
  'uxCustomerPivotReport
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.LayoutControl1)
  Me.Name = "uxCustomerPivotReport"
  Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.PivotGridControl1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.PrintCustomerPivotReportListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents PivotGridControl1 As DevExpress.XtraPivotGrid.PivotGridControl
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents PrintCustomerPivotReportListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnPrint As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents PrintingSystem1 As DevExpress.XtraPrinting.PrintingSystem
  Friend WithEvents PrintableComponentLink1 As DevExpress.XtraPrinting.PrintableComponentLink
  Friend WithEvents fieldMB As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCustomerSurname As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCustomerFirstName As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldPhoneNumber As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldFax As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldStreetName As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCityName As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCommunityCode As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCommunityName As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldLivingAddress As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldLivingAddressNumber As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldDateOfBirth As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldIsCompany As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldOccupation As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldWorksInCompany As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldBusinessTypeDescription As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldEMail As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldPassportNumber As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldCountryName As DevExpress.XtraPivotGrid.PivotGridField
  Friend WithEvents fieldIsCompanyString As DevExpress.XtraPivotGrid.PivotGridField

End Class
