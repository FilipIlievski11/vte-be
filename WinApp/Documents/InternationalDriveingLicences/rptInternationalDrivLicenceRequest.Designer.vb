<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class rptInternationalDrivLicenceRequest
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
    Me.XrLabel6 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel1 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel2 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel3 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel4 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel5 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel8 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel12 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblOrganDozvola = New DevExpress.XtraReports.UI.XRLabel
    Me.lblOrganPasos = New DevExpress.XtraReports.UI.XRLabel
    Me.lblOrganBLK = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel7 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel9 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel11 = New DevExpress.XtraReports.UI.XRLabel
    Me.lblGrad = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel15 = New DevExpress.XtraReports.UI.XRLabel
    Me.XrLabel17 = New DevExpress.XtraReports.UI.XRLabel
    Me.BindingSourceInternationalDrivLicence = New System.Windows.Forms.BindingSource(Me.components)
    CType(Me.BindingSourceInternationalDrivLicence, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
    '
    'Detail
    '
    Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel6, Me.XrLabel1, Me.XrLabel2, Me.XrLabel3, Me.XrLabel4, Me.XrLabel5, Me.XrLabel8, Me.XrLabel12, Me.lblOrganDozvola, Me.lblOrganPasos, Me.lblOrganBLK, Me.XrLabel7, Me.XrLabel9, Me.XrLabel11, Me.lblGrad, Me.XrLabel15, Me.XrLabel17})
    Me.Detail.Dpi = 254.0!
    Me.Detail.Height = 3000
    Me.Detail.Name = "Detail"
    Me.Detail.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254.0!)
    Me.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
    '
    'XrLabel6
    '
    Me.XrLabel6.CanShrink = True
    Me.XrLabel6.Dpi = 254.0!
    Me.XrLabel6.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel6.Location = New System.Drawing.Point(614, 2730)
    Me.XrLabel6.Name = "XrLabel6"
    Me.XrLabel6.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel6.Size = New System.Drawing.Size(826, 64)
    Me.XrLabel6.StylePriority.UseFont = False
    Me.XrLabel6.StylePriority.UseTextAlignment = False
    Me.XrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
    Me.XrLabel6.Visible = False
    '
    'XrLabel1
    '
    Me.XrLabel1.CanShrink = True
    Me.XrLabel1.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "CustomerName", Global.WinApp.My.Resources.Resources.String1)})
    Me.XrLabel1.Dpi = 254.0!
    Me.XrLabel1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel1.Location = New System.Drawing.Point(508, 910)
    Me.XrLabel1.Name = "XrLabel1"
    Me.XrLabel1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel1.Size = New System.Drawing.Size(1418, 64)
    Me.XrLabel1.StylePriority.UseFont = False
    Me.XrLabel1.StylePriority.UseTextAlignment = False
    Me.XrLabel1.Text = "XrLabel1"
    Me.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'XrLabel2
    '
    Me.XrLabel2.CanShrink = True
    Me.XrLabel2.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "DateOfBirth", "{0:dd/MM/yyyy}")})
    Me.XrLabel2.Dpi = 254.0!
    Me.XrLabel2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel2.Location = New System.Drawing.Point(508, 1080)
    Me.XrLabel2.Name = "XrLabel2"
    Me.XrLabel2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel2.Size = New System.Drawing.Size(1418, 64)
    Me.XrLabel2.StylePriority.UseFont = False
    Me.XrLabel2.StylePriority.UseTextAlignment = False
    Me.XrLabel2.Text = "XrLabel2"
    Me.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'XrLabel3
    '
    Me.XrLabel3.CanShrink = True
    Me.XrLabel3.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "BirthCity", Global.WinApp.My.Resources.Resources.String1)})
    Me.XrLabel3.Dpi = 254.0!
    Me.XrLabel3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel3.Location = New System.Drawing.Point(508, 1249)
    Me.XrLabel3.Name = "XrLabel3"
    Me.XrLabel3.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel3.Size = New System.Drawing.Size(1418, 64)
    Me.XrLabel3.StylePriority.UseFont = False
    Me.XrLabel3.StylePriority.UseTextAlignment = False
    Me.XrLabel3.Text = "XrLabel3"
    Me.XrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'XrLabel4
    '
    Me.XrLabel4.CanShrink = True
    Me.XrLabel4.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Citizenship", Global.WinApp.My.Resources.Resources.String1)})
    Me.XrLabel4.Dpi = 254.0!
    Me.XrLabel4.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel4.Location = New System.Drawing.Point(508, 1418)
    Me.XrLabel4.Name = "XrLabel4"
    Me.XrLabel4.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel4.Size = New System.Drawing.Size(1418, 64)
    Me.XrLabel4.StylePriority.UseFont = False
    Me.XrLabel4.StylePriority.UseTextAlignment = False
    Me.XrLabel4.Text = "XrLabel4"
    Me.XrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'XrLabel5
    '
    Me.XrLabel5.CanShrink = True
    Me.XrLabel5.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "NumberOfNationalLicence", Global.WinApp.My.Resources.Resources.String1)})
    Me.XrLabel5.Dpi = 254.0!
    Me.XrLabel5.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel5.Location = New System.Drawing.Point(677, 1545)
    Me.XrLabel5.Name = "XrLabel5"
    Me.XrLabel5.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel5.Size = New System.Drawing.Size(339, 64)
    Me.XrLabel5.StylePriority.UseFont = False
    Me.XrLabel5.StylePriority.UseTextAlignment = False
    Me.XrLabel5.Text = "XrLabel5"
    Me.XrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'XrLabel8
    '
    Me.XrLabel8.CanShrink = True
    Me.XrLabel8.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "PassNum", Global.WinApp.My.Resources.Resources.String1)})
    Me.XrLabel8.Dpi = 254.0!
    Me.XrLabel8.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel8.Location = New System.Drawing.Point(677, 1736)
    Me.XrLabel8.Name = "XrLabel8"
    Me.XrLabel8.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel8.Size = New System.Drawing.Size(339, 64)
    Me.XrLabel8.StylePriority.UseFont = False
    Me.XrLabel8.StylePriority.UseTextAlignment = False
    Me.XrLabel8.Text = "XrLabel8"
    Me.XrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'XrLabel12
    '
    Me.XrLabel12.CanShrink = True
    Me.XrLabel12.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "BLK", Global.WinApp.My.Resources.Resources.String1)})
    Me.XrLabel12.Dpi = 254.0!
    Me.XrLabel12.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel12.Location = New System.Drawing.Point(677, 1905)
    Me.XrLabel12.Name = "XrLabel12"
    Me.XrLabel12.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel12.Size = New System.Drawing.Size(339, 64)
    Me.XrLabel12.StylePriority.UseFont = False
    Me.XrLabel12.StylePriority.UseTextAlignment = False
    Me.XrLabel12.Text = "XrLabel12"
    Me.XrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'lblOrganDozvola
    '
    Me.lblOrganDozvola.CanShrink = True
    Me.lblOrganDozvola.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "DriveingLicenceIssuer", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblOrganDozvola.Dpi = 254.0!
    Me.lblOrganDozvola.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblOrganDozvola.Location = New System.Drawing.Point(1291, 1545)
    Me.lblOrganDozvola.Name = "lblOrganDozvola"
    Me.lblOrganDozvola.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblOrganDozvola.Size = New System.Drawing.Size(296, 64)
    Me.lblOrganDozvola.StylePriority.UseFont = False
    Me.lblOrganDozvola.StylePriority.UseTextAlignment = False
    Me.lblOrganDozvola.Text = "lblOrganDozvola"
    Me.lblOrganDozvola.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'lblOrganPasos
    '
    Me.lblOrganPasos.CanShrink = True
    Me.lblOrganPasos.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "PassIssuer", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblOrganPasos.Dpi = 254.0!
    Me.lblOrganPasos.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblOrganPasos.Location = New System.Drawing.Point(1291, 1736)
    Me.lblOrganPasos.Name = "lblOrganPasos"
    Me.lblOrganPasos.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblOrganPasos.Size = New System.Drawing.Size(296, 64)
    Me.lblOrganPasos.StylePriority.UseFont = False
    Me.lblOrganPasos.StylePriority.UseTextAlignment = False
    Me.lblOrganPasos.Text = "lblOrganPasos"
    Me.lblOrganPasos.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'lblOrganBLK
    '
    Me.lblOrganBLK.CanShrink = True
    Me.lblOrganBLK.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "BLKIssuer", Global.WinApp.My.Resources.Resources.String1)})
    Me.lblOrganBLK.Dpi = 254.0!
    Me.lblOrganBLK.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblOrganBLK.Location = New System.Drawing.Point(1291, 1905)
    Me.lblOrganBLK.Name = "lblOrganBLK"
    Me.lblOrganBLK.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblOrganBLK.Size = New System.Drawing.Size(296, 64)
    Me.lblOrganBLK.StylePriority.UseFont = False
    Me.lblOrganBLK.StylePriority.UseTextAlignment = False
    Me.lblOrganBLK.Text = "lblOrganBLK"
    Me.lblOrganBLK.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'XrLabel7
    '
    Me.XrLabel7.CanShrink = True
    Me.XrLabel7.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "DriveingLicenceDateIssued", "{0:dd/MM/yyyy}")})
    Me.XrLabel7.Dpi = 254.0!
    Me.XrLabel7.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel7.Location = New System.Drawing.Point(1736, 1545)
    Me.XrLabel7.Name = "XrLabel7"
    Me.XrLabel7.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel7.Size = New System.Drawing.Size(254, 64)
    Me.XrLabel7.StylePriority.UseFont = False
    Me.XrLabel7.StylePriority.UseTextAlignment = False
    Me.XrLabel7.Text = "XrLabel7"
    Me.XrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'XrLabel9
    '
    Me.XrLabel9.CanShrink = True
    Me.XrLabel9.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "PassDateIssued", "{0:dd/MM/yyyy}")})
    Me.XrLabel9.Dpi = 254.0!
    Me.XrLabel9.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel9.Location = New System.Drawing.Point(1736, 1736)
    Me.XrLabel9.Name = "XrLabel9"
    Me.XrLabel9.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel9.Size = New System.Drawing.Size(254, 64)
    Me.XrLabel9.StylePriority.UseFont = False
    Me.XrLabel9.StylePriority.UseTextAlignment = False
    Me.XrLabel9.Text = "XrLabel9"
    Me.XrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'XrLabel11
    '
    Me.XrLabel11.CanShrink = True
    Me.XrLabel11.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "BLKDateIssued", "{0:dd/MM/yyyy}")})
    Me.XrLabel11.Dpi = 254.0!
    Me.XrLabel11.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel11.Location = New System.Drawing.Point(1736, 1905)
    Me.XrLabel11.Name = "XrLabel11"
    Me.XrLabel11.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel11.Size = New System.Drawing.Size(254, 64)
    Me.XrLabel11.StylePriority.UseFont = False
    Me.XrLabel11.StylePriority.UseTextAlignment = False
    Me.XrLabel11.Text = "XrLabel11"
    Me.XrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
    '
    'lblGrad
    '
    Me.lblGrad.CanShrink = True
    Me.lblGrad.Dpi = 254.0!
    Me.lblGrad.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.lblGrad.Location = New System.Drawing.Point(275, 2604)
    Me.lblGrad.Name = "lblGrad"
    Me.lblGrad.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.lblGrad.Size = New System.Drawing.Size(762, 64)
    Me.lblGrad.StylePriority.UseFont = False
    Me.lblGrad.StylePriority.UseTextAlignment = False
    Me.lblGrad.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
    '
    'XrLabel15
    '
    Me.XrLabel15.CanShrink = True
    Me.XrLabel15.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "DateCreated", "{0:dd/MM/yyyy}")})
    Me.XrLabel15.Dpi = 254.0!
    Me.XrLabel15.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel15.Location = New System.Drawing.Point(1355, 2604)
    Me.XrLabel15.Name = "XrLabel15"
    Me.XrLabel15.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel15.Size = New System.Drawing.Size(572, 64)
    Me.XrLabel15.StylePriority.UseFont = False
    Me.XrLabel15.StylePriority.UseTextAlignment = False
    Me.XrLabel15.Text = "XrLabel15"
    Me.XrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
    '
    'XrLabel17
    '
    Me.XrLabel17.CanShrink = True
    Me.XrLabel17.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "LivingAddressStreetAndNumber", Global.WinApp.My.Resources.Resources.String1)})
    Me.XrLabel17.Dpi = 254.0!
    Me.XrLabel17.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.XrLabel17.Location = New System.Drawing.Point(635, 2879)
    Me.XrLabel17.Name = "XrLabel17"
    Me.XrLabel17.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
    Me.XrLabel17.Size = New System.Drawing.Size(1291, 64)
    Me.XrLabel17.StylePriority.UseFont = False
    Me.XrLabel17.StylePriority.UseTextAlignment = False
    Me.XrLabel17.Text = "XrLabel17"
    Me.XrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
    '
    'BindingSourceInternationalDrivLicence
    '
    Me.BindingSourceInternationalDrivLicence.DataSource = GetType(VTE.Library.PrintDocumentInternationalDriveingLicenceList)
    '
    'rptInternationalDrivLicenceRequest
    '
    Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.Detail})
    Me.DataSource = Me.BindingSourceInternationalDrivLicence
    Me.Dpi = 254.0!
    Me.Margins = New System.Drawing.Printing.Margins(0, 0, 0, 0)
    Me.PageHeight = 2969
    Me.PageWidth = 2101
    Me.PaperKind = System.Drawing.Printing.PaperKind.A4
    Me.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter
    Me.Version = "8.2"
    CType(Me.BindingSourceInternationalDrivLicence, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

  End Sub
    Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
    Friend WithEvents XrLabel4 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel3 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel2 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel1 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel15 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents lblGrad As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents lblOrganBLK As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel12 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel11 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents lblOrganPasos As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel9 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel8 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel7 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents lblOrganDozvola As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel5 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel17 As DevExpress.XtraReports.UI.XRLabel
  Friend WithEvents BindingSourceInternationalDrivLicence As System.Windows.Forms.BindingSource
  Friend WithEvents XrLabel6 As DevExpress.XtraReports.UI.XRLabel
End Class
