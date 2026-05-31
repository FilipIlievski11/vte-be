<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class rptOdobrenieZaTugoV
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
    Me.lblIssuedAtCityAndDate = New DevExpress.XtraReports.UI.XRLabel
    Me.lblDateTo2 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblDateFrom2 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblTrafficLicenceNum2 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblRegNum2 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblMadeAndModel2 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblIzdadenOd = New DevExpress.XtraReports.UI.XRLabel
    Me.lblCustomerPassportNumber = New DevExpress.XtraReports.UI.XRLabel
    Me.lblCustomerDisplay2 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblOwnerLine2 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblOwnerLine1 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblTriptiqueNumber = New DevExpress.XtraReports.UI.XRLabel
    Me.lblTrafficLicenceNum1 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblRegNum1 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblMadeAndModel1 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblDateTo1 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblDateFrom1 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblCustomerDisplay1 = New DevExpress.XtraReports.UI.XRLabel
    Me.BindingSourcePermission = New System.Windows.Forms.BindingSource(Me.components)
    CType(Me.BindingSourcePermission, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
    '
    'Detail
    '
    Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.lblIssuedAtCityAndDate, Me.lblDateTo2, Me.lblDateFrom2, Me.lblTrafficLicenceNum2, Me.lblRegNum2, Me.lblMadeAndModel2, Me.lblIzdadenOd, Me.lblCustomerPassportNumber, Me.lblCustomerDisplay2, Me.lblOwnerLine2, Me.lblOwnerLine1, Me.lblTriptiqueNumber, Me.lblTrafficLicenceNum1, Me.lblRegNum1, Me.lblMadeAndModel1, Me.lblDateTo1, Me.lblDateFrom1, Me.lblCustomerDisplay1})
    Me.Detail.Dpi = 254.0!
    Me.Detail.Height = 1884
    Me.Detail.Name = "Detail"
    Me.Detail.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254.0!)
    Me.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
    '
    'lblIssuedAtCityAndDate
    '
    Me.lblIssuedAtCityAndDate.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "IssuedAtCityAndDate", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblIssuedAtCityAndDate.Dpi = 254.0!
    Me.lblIssuedAtCityAndDate.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblIssuedAtCityAndDate.Location = New System.Drawing.Point(1111, 1185)
    Me.lblIssuedAtCityAndDate.Name = "lblIssuedAtCityAndDate"
    Me.lblIssuedAtCityAndDate.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblIssuedAtCityAndDate.Size = New System.Drawing.Size(940, 65)
    Me.lblIssuedAtCityAndDate.StylePriority.UseFont = False
    Me.lblIssuedAtCityAndDate.StylePriority.UseTextAlignment = False
    Me.lblIssuedAtCityAndDate.Text = "lblIssuedAtCityAndDate"
    Me.lblIssuedAtCityAndDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblDateTo2
    '
    Me.lblDateTo2.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "ValidTillDate", "{0:dd.MM.yyyy}")})
    Me.lblDateTo2.Dpi = 254.0!
    Me.lblDateTo2.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblDateTo2.Location = New System.Drawing.Point(1799, 1101)
    Me.lblDateTo2.Name = "lblDateTo2"
    Me.lblDateTo2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblDateTo2.Size = New System.Drawing.Size(257, 66)
    Me.lblDateTo2.StylePriority.UseFont = False
    Me.lblDateTo2.StylePriority.UseTextAlignment = False
    Me.lblDateTo2.Text = "lblDateTo2"
    Me.lblDateTo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblDateFrom2
    '
    Me.lblDateFrom2.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "DateCreated", "{0:dd.MM.yyyy}")})
    Me.lblDateFrom2.Dpi = 254.0!
    Me.lblDateFrom2.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblDateFrom2.Location = New System.Drawing.Point(1492, 1101)
    Me.lblDateFrom2.Name = "lblDateFrom2"
    Me.lblDateFrom2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblDateFrom2.Size = New System.Drawing.Size(257, 66)
    Me.lblDateFrom2.StylePriority.UseFont = False
    Me.lblDateFrom2.StylePriority.UseTextAlignment = False
    Me.lblDateFrom2.Text = "lblDateFrom2"
    Me.lblDateFrom2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblTrafficLicenceNum2
    '
    Me.lblTrafficLicenceNum2.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "TrafficLicenceNumber", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblTrafficLicenceNum2.Dpi = 254.0!
    Me.lblTrafficLicenceNum2.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblTrafficLicenceNum2.Location = New System.Drawing.Point(1418, 974)
    Me.lblTrafficLicenceNum2.Name = "lblTrafficLicenceNum2"
    Me.lblTrafficLicenceNum2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblTrafficLicenceNum2.Size = New System.Drawing.Size(625, 65)
    Me.lblTrafficLicenceNum2.StylePriority.UseFont = False
    Me.lblTrafficLicenceNum2.StylePriority.UseTextAlignment = False
    Me.lblTrafficLicenceNum2.Text = "lblTrafficLicenceNum2"
    Me.lblTrafficLicenceNum2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblRegNum2
    '
    Me.lblRegNum2.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "LastRegistrationNumber", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblRegNum2.Dpi = 254.0!
    Me.lblRegNum2.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblRegNum2.Location = New System.Drawing.Point(1619, 804)
    Me.lblRegNum2.Name = "lblRegNum2"
    Me.lblRegNum2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblRegNum2.Size = New System.Drawing.Size(423, 66)
    Me.lblRegNum2.StylePriority.UseFont = False
    Me.lblRegNum2.StylePriority.UseTextAlignment = False
    Me.lblRegNum2.Text = "lblRegNum2"
    Me.lblRegNum2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblMadeAndModel2
    '
    Me.lblMadeAndModel2.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "VehicleDisplay", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblMadeAndModel2.Dpi = 254.0!
    Me.lblMadeAndModel2.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblMadeAndModel2.Location = New System.Drawing.Point(1132, 804)
    Me.lblMadeAndModel2.Name = "lblMadeAndModel2"
    Me.lblMadeAndModel2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblMadeAndModel2.Size = New System.Drawing.Size(435, 65)
    Me.lblMadeAndModel2.StylePriority.UseFont = False
    Me.lblMadeAndModel2.StylePriority.UseTextAlignment = False
    Me.lblMadeAndModel2.Text = "lblMadeAndModel2"
    Me.lblMadeAndModel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblIzdadenOd
    '
    Me.lblIzdadenOd.CanShrink = True
    Me.lblIzdadenOd.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "IssuerName", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblIzdadenOd.Dpi = 254.0!
    Me.lblIzdadenOd.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblIzdadenOd.Location = New System.Drawing.Point(1630, 582)
    Me.lblIzdadenOd.Name = "lblIzdadenOd"
    Me.lblIzdadenOd.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblIzdadenOd.Size = New System.Drawing.Size(415, 65)
    Me.lblIzdadenOd.StylePriority.UseFont = False
    Me.lblIzdadenOd.StylePriority.UseTextAlignment = False
    Me.lblIzdadenOd.Text = "lblIzdadenOd"
    Me.lblIzdadenOd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    Me.lblIzdadenOd.WordWrap = False
    '
    'lblCustomerPassportNumber
    '
    Me.lblCustomerPassportNumber.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "CustomerPasswordNumber", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblCustomerPassportNumber.Dpi = 254.0!
    Me.lblCustomerPassportNumber.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblCustomerPassportNumber.Location = New System.Drawing.Point(1111, 582)
    Me.lblCustomerPassportNumber.Name = "lblCustomerPassportNumber"
    Me.lblCustomerPassportNumber.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblCustomerPassportNumber.Size = New System.Drawing.Size(405, 65)
    Me.lblCustomerPassportNumber.StylePriority.UseFont = False
    Me.lblCustomerPassportNumber.StylePriority.UseTextAlignment = False
    Me.lblCustomerPassportNumber.Text = "lblCustomerPassportNumber"
    Me.lblCustomerPassportNumber.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblCustomerDisplay2
    '
    Me.lblCustomerDisplay2.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "CustomerDisplay", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblCustomerDisplay2.Dpi = 254.0!
    Me.lblCustomerDisplay2.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblCustomerDisplay2.Location = New System.Drawing.Point(1111, 444)
    Me.lblCustomerDisplay2.Name = "lblCustomerDisplay2"
    Me.lblCustomerDisplay2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblCustomerDisplay2.Size = New System.Drawing.Size(940, 65)
    Me.lblCustomerDisplay2.StylePriority.UseFont = False
    Me.lblCustomerDisplay2.StylePriority.UseTextAlignment = False
    Me.lblCustomerDisplay2.Text = "lblCustomerDisplay2"
    Me.lblCustomerDisplay2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblOwnerLine2
    '
    Me.lblOwnerLine2.CanGrow = False
    Me.lblOwnerLine2.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "OwnerDiplayLine2", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblOwnerLine2.Dpi = 254.0!
    Me.lblOwnerLine2.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblOwnerLine2.Location = New System.Drawing.Point(64, 1154)
    Me.lblOwnerLine2.Name = "lblOwnerLine2"
    Me.lblOwnerLine2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblOwnerLine2.Size = New System.Drawing.Size(940, 65)
    Me.lblOwnerLine2.StylePriority.UseFont = False
    Me.lblOwnerLine2.StylePriority.UseTextAlignment = False
    Me.lblOwnerLine2.Text = "lblOwnerLine2"
    Me.lblOwnerLine2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblOwnerLine1
    '
    Me.lblOwnerLine1.CanGrow = False
    Me.lblOwnerLine1.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "OwnerDiplayLine1", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblOwnerLine1.Dpi = 254.0!
    Me.lblOwnerLine1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblOwnerLine1.Location = New System.Drawing.Point(64, 1080)
    Me.lblOwnerLine1.Name = "lblOwnerLine1"
    Me.lblOwnerLine1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblOwnerLine1.Size = New System.Drawing.Size(940, 65)
    Me.lblOwnerLine1.StylePriority.UseFont = False
    Me.lblOwnerLine1.StylePriority.UseTextAlignment = False
    Me.lblOwnerLine1.Text = "lblOwnerLine1"
    Me.lblOwnerLine1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblTriptiqueNumber
    '
    Me.lblTriptiqueNumber.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "TriptiqueNumber", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblTriptiqueNumber.Dpi = 254.0!
    Me.lblTriptiqueNumber.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblTriptiqueNumber.Location = New System.Drawing.Point(476, 963)
    Me.lblTriptiqueNumber.Name = "lblTriptiqueNumber"
    Me.lblTriptiqueNumber.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblTriptiqueNumber.Size = New System.Drawing.Size(540, 65)
    Me.lblTriptiqueNumber.StylePriority.UseFont = False
    Me.lblTriptiqueNumber.StylePriority.UseTextAlignment = False
    Me.lblTriptiqueNumber.Text = "lblTriptiqueNumber"
    Me.lblTriptiqueNumber.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblTrafficLicenceNum1
    '
    Me.lblTrafficLicenceNum1.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "TrafficLicenceNumber", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblTrafficLicenceNum1.Dpi = 254.0!
    Me.lblTrafficLicenceNum1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblTrafficLicenceNum1.Location = New System.Drawing.Point(381, 836)
    Me.lblTrafficLicenceNum1.Name = "lblTrafficLicenceNum1"
    Me.lblTrafficLicenceNum1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblTrafficLicenceNum1.Size = New System.Drawing.Size(640, 65)
    Me.lblTrafficLicenceNum1.StylePriority.UseFont = False
    Me.lblTrafficLicenceNum1.StylePriority.UseTextAlignment = False
    Me.lblTrafficLicenceNum1.Text = "lblTrafficLicenceNum1"
    Me.lblTrafficLicenceNum1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblRegNum1
    '
    Me.lblRegNum1.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "LastRegistrationNumber", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblRegNum1.Dpi = 254.0!
    Me.lblRegNum1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblRegNum1.Location = New System.Drawing.Point(328, 709)
    Me.lblRegNum1.Name = "lblRegNum1"
    Me.lblRegNum1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblRegNum1.Size = New System.Drawing.Size(690, 65)
    Me.lblRegNum1.StylePriority.UseFont = False
    Me.lblRegNum1.StylePriority.UseTextAlignment = False
    Me.lblRegNum1.Text = "lblRegNum1"
    Me.lblRegNum1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblMadeAndModel1
    '
    Me.lblMadeAndModel1.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "VehicleDisplay", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblMadeAndModel1.Dpi = 254.0!
    Me.lblMadeAndModel1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblMadeAndModel1.Location = New System.Drawing.Point(64, 529)
    Me.lblMadeAndModel1.Name = "lblMadeAndModel1"
    Me.lblMadeAndModel1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblMadeAndModel1.Size = New System.Drawing.Size(940, 65)
    Me.lblMadeAndModel1.StylePriority.UseFont = False
    Me.lblMadeAndModel1.StylePriority.UseTextAlignment = False
    Me.lblMadeAndModel1.Text = "lblMadeAndModel1"
    Me.lblMadeAndModel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblDateTo1
    '
    Me.lblDateTo1.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "ValidTillDate", "{0:dd.MM.yyyy}")})
    Me.lblDateTo1.Dpi = 254.0!
    Me.lblDateTo1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblDateTo1.Location = New System.Drawing.Point(773, 307)
    Me.lblDateTo1.Name = "lblDateTo1"
    Me.lblDateTo1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblDateTo1.Size = New System.Drawing.Size(250, 65)
    Me.lblDateTo1.StylePriority.UseFont = False
    Me.lblDateTo1.StylePriority.UseTextAlignment = False
    Me.lblDateTo1.Text = "lblDateTo1"
    Me.lblDateTo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblDateFrom1
    '
    Me.lblDateFrom1.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "DateCreated", "{0:dd.MM.yyyy}")})
    Me.lblDateFrom1.Dpi = 254.0!
    Me.lblDateFrom1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblDateFrom1.Location = New System.Drawing.Point(349, 307)
    Me.lblDateFrom1.Name = "lblDateFrom1"
    Me.lblDateFrom1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblDateFrom1.Size = New System.Drawing.Size(340, 65)
    Me.lblDateFrom1.StylePriority.UseFont = False
    Me.lblDateFrom1.StylePriority.UseTextAlignment = False
    Me.lblDateFrom1.Text = "lblDateFrom1"
    Me.lblDateFrom1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    '
    'lblCustomerDisplay1
    '
    Me.lblCustomerDisplay1.CanGrow = False
    Me.lblCustomerDisplay1.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "CustomerDisplay", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblCustomerDisplay1.Dpi = 254.0!
    Me.lblCustomerDisplay1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblCustomerDisplay1.Location = New System.Drawing.Point(402, 148)
    Me.lblCustomerDisplay1.Name = "lblCustomerDisplay1"
    Me.lblCustomerDisplay1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblCustomerDisplay1.Size = New System.Drawing.Size(620, 65)
    Me.lblCustomerDisplay1.StylePriority.UseFont = False
    Me.lblCustomerDisplay1.StylePriority.UseTextAlignment = False
    Me.lblCustomerDisplay1.Text = "lblCustomerDisplay1"
    Me.lblCustomerDisplay1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
    Me.lblCustomerDisplay1.WordWrap = False
    '
    'BindingSourcePermission
    '
    Me.BindingSourcePermission.DataSource = GetType(VTE.Library.printPermisionList)
    '
    'rptOdobrenieZaTugoV
    '
    Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.Detail})
    Me.DataSource = Me.BindingSourcePermission
    Me.DefaultPrinterSettingsUsing.UseLandscape = True
    Me.Dpi = 254.0!
    Me.GridSize = New System.Drawing.Size(4, 4)
    Me.Margins = New System.Drawing.Printing.Margins(0, 148, 0, 0)
    Me.PageHeight = 4318
    Me.PageWidth = 2794
    Me.PaperKind = System.Drawing.Printing.PaperKind.Tabloid
    Me.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter
    Me.Version = "8.2"
    CType(Me.BindingSourcePermission, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

  End Sub
  Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
  Friend WithEvents BindingSourcePermission As System.Windows.Forms.BindingSource
  Friend WithEvents lblDateTo1 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblDateFrom1 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblCustomerDisplay1 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblTriptiqueNumber As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblTrafficLicenceNum1 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblRegNum1 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblMadeAndModel1 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblRegNum2 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblMadeAndModel2 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblIzdadenOd As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblCustomerPassportNumber As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblCustomerDisplay2 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblOwnerLine2 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblOwnerLine1 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblIssuedAtCityAndDate As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblDateTo2 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblDateFrom2 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents lblTrafficLicenceNum2 As DevExpress.XtraReports.UI.XRLabel
End Class
