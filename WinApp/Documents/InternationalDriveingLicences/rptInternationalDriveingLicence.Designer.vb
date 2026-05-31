<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class rptInternationalDriveingLicence
    Inherits DevExpress.XtraReports.UI.XtraReport

    'XtraReport overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Designer
    'It can be modified using the Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container
    Me.Detail = New DevExpress.XtraReports.UI.DetailBand
    Me.XrLabel13 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel9 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblCountry = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel11 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblBirthPlace = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel7 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel6 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel5 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblCiteIssuedBy = New DevExpress.XtraReports.UI.XRLabel
    Me.lblOrganization = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel2 = New DevExpress.XtraReports.UI.XRLabel
    Me.BindingSourceInternationalDriveingLicence = New System.Windows.Forms.BindingSource(Me.components)
    CType(Me.BindingSourceInternationalDriveingLicence, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
    '
    'Detail
    '
    Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel13, Me.XrLabel9, Me.lblCountry, Me.XrLabel11, Me.lblBirthPlace, Me.XrLabel7, Me.XrLabel6, Me.XrLabel5, Me.lblCiteIssuedBy, Me.lblOrganization, Me.XrLabel2})
    Me.Detail.Dpi = 254.0!
    Me.Detail.Height = 4313
    Me.Detail.Name = "Detail"
    Me.Detail.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254.0!)
    Me.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
    '
    'XrLabel13
    '
    Me.XrLabel13.Angle = 90.0!
    Me.XrLabel13.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Note", Global.WinApp.My.Resources.Resources.String1)})
    Me.XrLabel13.Dpi = 254.0!
    Me.XrLabel13.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel13.Location = New System.Drawing.Point(1055, 3238)
    Me.XrLabel13.Multiline = True
    Me.XrLabel13.Name = "XrLabel13"
    Me.XrLabel13.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel13.Size = New System.Drawing.Size(120, 880)
    Me.XrLabel13.StylePriority.UseFont = False
    Me.XrLabel13.StylePriority.UseTextAlignment = False
    Me.XrLabel13.Text = "XrLabel13"
    Me.XrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'XrLabel9
    '
    Me.XrLabel9.Angle = 90.0!
    Me.XrLabel9.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "CustomerSurname", Global.WinApp.My.Resources.Resources.String1)})
    Me.XrLabel9.Dpi = 254.0!
    Me.XrLabel9.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel9.Location = New System.Drawing.Point(42, 2244)
    Me.XrLabel9.Name = "XrLabel9"
    Me.XrLabel9.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel9.Size = New System.Drawing.Size(40, 698)
    Me.XrLabel9.StylePriority.UseFont = False
    Me.XrLabel9.StylePriority.UseTextAlignment = False
    Me.XrLabel9.Text = "XrLabel9"
    Me.XrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft
    '
    'lblCountry
    '
    Me.lblCountry.Angle = 90.0!
    Me.lblCountry.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "LivingCity", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblCountry.Dpi = 254.0!
    Me.lblCountry.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblCountry.Location = New System.Drawing.Point(169, 2244)
    Me.lblCountry.Name = "lblCountry"
    Me.lblCountry.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblCountry.Size = New System.Drawing.Size(40, 698)
    Me.lblCountry.StylePriority.UseFont = False
    Me.lblCountry.StylePriority.UseTextAlignment = False
    Me.lblCountry.Text = "lblCountry"
    Me.lblCountry.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft
    '
    'XrLabel11
    '
    Me.XrLabel11.Angle = 90.0!
    Me.XrLabel11.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "DateOfBirth", "{0:dd/MM/yyyy}")})
    Me.XrLabel11.Dpi = 254.0!
    Me.XrLabel11.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel11.Location = New System.Drawing.Point(127, 2244)
    Me.XrLabel11.Name = "XrLabel11"
    Me.XrLabel11.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel11.Size = New System.Drawing.Size(40, 698)
    Me.XrLabel11.StylePriority.UseFont = False
    Me.XrLabel11.StylePriority.UseTextAlignment = False
    Me.XrLabel11.Text = "XrLabel11"
    Me.XrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft
    '
    'lblBirthPlace
    '
    Me.lblBirthPlace.Angle = 90.0!
    Me.lblBirthPlace.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "BirthCity", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblBirthPlace.Dpi = 254.0!
    Me.lblBirthPlace.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblBirthPlace.Location = New System.Drawing.Point(85, 2244)
    Me.lblBirthPlace.Name = "lblBirthPlace"
    Me.lblBirthPlace.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblBirthPlace.Size = New System.Drawing.Size(40, 698)
    Me.lblBirthPlace.StylePriority.UseFont = False
    Me.lblBirthPlace.StylePriority.UseTextAlignment = False
    Me.lblBirthPlace.Text = "lblBirthPlace"
    Me.lblBirthPlace.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft
    '
    'XrLabel7
    '
    Me.XrLabel7.Angle = 90.0!
    Me.XrLabel7.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "CustomerFirstName", Global.WinApp.My.Resources.Resources.String1)})
    Me.XrLabel7.Dpi = 254.0!
    Me.XrLabel7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel7.Location = New System.Drawing.Point(0, 2244)
    Me.XrLabel7.Name = "XrLabel7"
    Me.XrLabel7.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel7.Size = New System.Drawing.Size(40, 698)
    Me.XrLabel7.StylePriority.UseFont = False
    Me.XrLabel7.StylePriority.UseTextAlignment = False
    Me.XrLabel7.Text = "XrLabel7"
    Me.XrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft
    '
    'XrLabel6
    '
    Me.XrLabel6.AnchorVertical = CType((DevExpress.XtraReports.UI.VerticalAnchorStyles.Top Or DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom), DevExpress.XtraReports.UI.VerticalAnchorStyles)
    Me.XrLabel6.Angle = 90.0!
    Me.XrLabel6.CanGrow = False
    Me.XrLabel6.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "NumberOfNationalLicence", Global.WinApp.My.Resources.Resources.String1)})
    Me.XrLabel6.Dpi = 254.0!
    Me.XrLabel6.Font = New System.Drawing.Font("Arial Unicode MS", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.XrLabel6.Location = New System.Drawing.Point(1111, 106)
    Me.XrLabel6.Name = "XrLabel6"
    Me.XrLabel6.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel6.Size = New System.Drawing.Size(66, 550)
    Me.XrLabel6.StylePriority.UseFont = False
    Me.XrLabel6.StylePriority.UseTextAlignment = False
    Me.XrLabel6.Text = "XrLabel6"
    Me.XrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'XrLabel5
    '
    Me.XrLabel5.Angle = 90.0!
    Me.XrLabel5.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "DateCreated", "{0:dd.MM.yyyy}")})
    Me.XrLabel5.Dpi = 254.0!
    Me.XrLabel5.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel5.Location = New System.Drawing.Point(1016, 106)
    Me.XrLabel5.Name = "XrLabel5"
    Me.XrLabel5.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel5.Size = New System.Drawing.Size(65, 770)
    Me.XrLabel5.StylePriority.UseFont = False
    Me.XrLabel5.StylePriority.UseTextAlignment = False
    Me.XrLabel5.Text = "XrLabel5"
    Me.XrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblCiteIssuedBy
    '
    Me.lblCiteIssuedBy.Angle = 90.0!
    Me.lblCiteIssuedBy.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "CityIssuedFrom", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblCiteIssuedBy.Dpi = 254.0!
    Me.lblCiteIssuedBy.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblCiteIssuedBy.Location = New System.Drawing.Point(931, 106)
    Me.lblCiteIssuedBy.Name = "lblCiteIssuedBy"
    Me.lblCiteIssuedBy.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblCiteIssuedBy.Size = New System.Drawing.Size(65, 770)
    Me.lblCiteIssuedBy.StylePriority.UseFont = False
    Me.lblCiteIssuedBy.StylePriority.UseTextAlignment = False
    Me.lblCiteIssuedBy.Text = "lblCiteIssuedBy"
    Me.lblCiteIssuedBy.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblOrganization
    '
    Me.lblOrganization.Angle = 90.0!
    Me.lblOrganization.CanGrow = False
    Me.lblOrganization.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "OrganizationName", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblOrganization.Dpi = 254.0!
    Me.lblOrganization.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblOrganization.Location = New System.Drawing.Point(857, 53)
    Me.lblOrganization.Name = "lblOrganization"
    Me.lblOrganization.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblOrganization.Size = New System.Drawing.Size(66, 823)
    Me.lblOrganization.StylePriority.UseFont = False
    Me.lblOrganization.StylePriority.UseTextAlignment = False
    Me.lblOrganization.Text = "lblOrganization"
    Me.lblOrganization.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    Me.lblOrganization.WordWrap = False
    '
    'XrLabel2
    '
    Me.XrLabel2.Angle = 90.0!
    Me.XrLabel2.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "ValidTillDate", "{0:dd.MM.yyyy}")})
    Me.XrLabel2.Dpi = 254.0!
    Me.XrLabel2.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel2.Location = New System.Drawing.Point(773, 106)
    Me.XrLabel2.Name = "XrLabel2"
    Me.XrLabel2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel2.Size = New System.Drawing.Size(65, 770)
    Me.XrLabel2.StylePriority.UseFont = False
    Me.XrLabel2.StylePriority.UseTextAlignment = False
    Me.XrLabel2.Text = "XrLabel2"
    Me.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'BindingSourceInternationalDriveingLicence
    '
    Me.BindingSourceInternationalDriveingLicence.DataSource = GetType(VTE.Library.PrintDocumentInternationalDriveingLicenceList)
    '
    'rptInternationalDriveingLicence
    '
    Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.Detail})
    Me.DataSource = Me.BindingSourceInternationalDriveingLicence
    Me.Dpi = 254.0!
    Me.GridSize = New System.Drawing.Size(4, 4)
    Me.Margins = New System.Drawing.Printing.Margins(0, 0, 0, 0)
    Me.PageHeight = 2100
    Me.PageWidth = 1738
    Me.PaperKind = System.Drawing.Printing.PaperKind.Custom
    Me.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter
    Me.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify
    Me.Version = "8.2"
    CType(Me.BindingSourceInternationalDriveingLicence, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

  End Sub
 Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
 Friend WithEvents XrLabel2 As DevExpress.XtraReports.UI.XRLabel
 Friend WithEvents BindingSourceInternationalDriveingLicence As System.Windows.Forms.BindingSource
 Friend WithEvents XrLabel6 As DevExpress.XtraReports.UI.XRLabel
 Friend WithEvents XrLabel5 As DevExpress.XtraReports.UI.XRLabel
 Friend WithEvents lblCiteIssuedBy As DevExpress.XtraReports.UI.XRLabel
 Friend WithEvents lblOrganization As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents XrLabel7 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents XrLabel9 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblCountry As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents XrLabel11 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblBirthPlace As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents XrLabel13 As DevExpress.XtraReports.UI.XRLabel
End Class
