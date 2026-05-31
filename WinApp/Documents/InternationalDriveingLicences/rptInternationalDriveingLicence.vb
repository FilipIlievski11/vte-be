Imports System.Drawing.Imaging
Imports DevExpress.XtraReports.UI
Public Class rptInternationalDriveingLicence
  Private WithEvents _docInternationalLicence As PrintDocumentInternationalDriveingLicenceList
  Private WithEvents _countryList As CountriesList
  Private WithEvents _tehnicalOrganizationList As TehnicalExamOrganizationsList
  Private WithEvents _cityList As CityList


  Public Sub New(ByVal inDocId As Long)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    Me.Margins.Top = objOpcii.MegunarodnaTopMargin
    Me.Margins.Left = objOpcii.MegunarodnaLeftMargin
    Me.Margins.Right = objOpcii.MegunarodnaRightmargin
    Me.Margins.Bottom = objOpcii.MegunarodnaButtonMargin
    _docInternationalLicence = PrintDocumentInternationalDriveingLicenceList.GetPrintDocumentInternationalDriveingLicenceList(inDocId)
    BindingSourceInternationalDriveingLicence.DataSource = _docInternationalLicence

  End Sub
  Public Function GetStringWidth(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) As Single
    Dim factor As Int32

    Dim gr As Graphics = Graphics.FromHwnd(IntPtr.Zero)
    If Me.ReportUnit = ReportUnit.HundredthsOfAnInch Then
      gr.PageUnit = GraphicsUnit.Inch
      factor = 100
    Else
      gr.PageUnit = GraphicsUnit.Millimeter
      factor = 11
    End If

    Dim size As SizeF = gr.MeasureString(CType(sender, XRLabel).Text, CType(sender, XRLabel).Font)
    Dim tempgolemina As Single = CType(sender, XRLabel).Font.Size
    While size.Width * factor > CType(sender, XRLabel).Height()
      tempgolemina = CType(sender, XRLabel).Font.Size
      CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, tempgolemina - 1)
      size = gr.MeasureString(CType(sender, XRLabel).Text, CType(sender, XRLabel).Font)
    End While

    gr.Dispose()
    Return tempgolemina
  End Function
  'Private Sub XrLabel3_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel3.BeforePrint
  ' Dim golemina As Single = GetStringWidth(sender, e)
  ' CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Bold)

  'End Sub

  Private Sub lblOrganization_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles lblOrganization.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e)
    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Bold)
    'lblOrganization.Angle = 90
  End Sub
End Class