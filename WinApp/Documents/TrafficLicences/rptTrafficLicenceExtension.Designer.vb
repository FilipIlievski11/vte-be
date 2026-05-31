<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class rptTrafficLicenceExtension
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
  Me.Detail = New DevExpress.XtraReports.UI.DetailBand
  Me.lblNote = New DevExpress.XtraReports.UI.XRLabel
  Me.lbl = New DevExpress.XtraReports.UI.XRLabel
  CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
  '
  'Detail
  '
  Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.lblNote, Me.lbl})
  Me.Detail.Dpi = 254.0!
  Me.Detail.Height = 1394
  Me.Detail.Name = "Detail"
  Me.Detail.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254.0!)
  Me.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
  '
  'lblNote
  '
  Me.lblNote.CanGrow = False
  Me.lblNote.Dpi = 254.0!
  Me.lblNote.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
  Me.lblNote.Location = New System.Drawing.Point(720, 85)
  Me.lblNote.Multiline = True
  Me.lblNote.Name = "lblNote"
  Me.lblNote.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
  Me.lblNote.Size = New System.Drawing.Size(677, 466)
  Me.lblNote.StylePriority.UseFont = False
  Me.lblNote.StylePriority.UseTextAlignment = False
  Me.lblNote.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
  '
  'lbl
  '
  Me.lbl.Dpi = 254.0!
  Me.lbl.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
  Me.lbl.Location = New System.Drawing.Point(42, 64)
  Me.lbl.Name = "lbl"
  Me.lbl.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
  Me.lbl.Size = New System.Drawing.Size(280, 120)
  Me.lbl.StylePriority.UseFont = False
  Me.lbl.StylePriority.UseTextAlignment = False
  Me.lbl.Text = "lbl"
  Me.lbl.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
  '
  'rptTrafficLicenceExtension
  '
  Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.Detail})
  Me.Dpi = 254.0!
  Me.Margins = New System.Drawing.Printing.Margins(50, 0, 0, 0)
  Me.PageHeight = 1500
  Me.PageWidth = 2000
  Me.PaperKind = System.Drawing.Printing.PaperKind.Custom
  Me.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter
  Me.Version = "8.1"
  CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

 End Sub
    Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
    Friend WithEvents lbl As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents lblNote As DevExpress.XtraReports.UI.XRLabel
End Class
