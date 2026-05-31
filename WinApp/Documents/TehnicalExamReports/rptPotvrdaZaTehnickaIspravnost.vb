Imports DevExpress.XtraReports.UI

Public Class rptPotvrdaZaTehnickaIspravnost
  Private WithEvents _printTehExamReportList As PrintDocumentsTehnicalExamsReportsList
  Public Sub New(ByVal idTexReport As Long)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    Try
      _printTehExamReportList = PrintDocumentsTehnicalExamsReportsList.GetPrintDocumentsTehnicalExamsReportsList(idTexReport)
      Me.BindingSource1.DataSource = _printTehExamReportList
      If objCurentTehExamOrganization.LogoPath <> String.Empty Then
        pictureLogoTop.Image = Image.FromFile(objCurentTehExamOrganization.LogoPath)
        pictureLogoTop.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage
        'Me.Watermark.Image = Image.FromFile(objCurentTehExamOrganization.LogoPath)
        'Me.Watermark.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Zoom
        'Me.Watermark.ImageTransparency = 190
      Else
        ' Me.Watermark.Image = Nothing
        pictureLogoTop.Image = Nothing
      End If

      ' Dim organization As TehnicalExamOrganizationsInfo = _
      ' TehnicalExamOrganizationsList.GetTehnicalExamOrganizationsList. _
      'GetTehnicalExamOrganizationsInfoById(objCurentTehExamOrganization.Company)
      Dim city As CityInfo = CityList.GetCityList.GetCityListById(objCurentTehExamOrganization.IdCity)

      lblOIzdava.Text = UCase(objCurentTehExamOrganization.OrganizationName)
      lblOAddress.Text = objCurentTehExamOrganization.StationAddress

      lblTel.Text = "тел: " & objCurentTehExamOrganization.Tel & "   факс: " & objCurentTehExamOrganization.Fax
      'lblFax.Text = objCurentTehExamOrganization.Fax
      lblCityNameCode.Text = city.CityZip & " " & city.CityName
      lblMestoIVreme.Text = city.CityName & ", " & Now.Date
      Dim pom As CommunitiesInfo
      pom = objCommunityList.GetCommunitiesListById(_printTehExamReportList.Item(0).communityId)

      If _printTehExamReportList.Item(0).IsNewRegistration Then 'If _printTehExamReportList.Item(0).IsNewCustomer Then '
        lblNovaReg.Text = pom.RegistrationCode & "-"
      Else
        lblNovaReg.Text = _printTehExamReportList.Item(0).LastRegistration

      End If
    Catch
    End Try
  End Sub

  Private Sub lblOIzdava_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles lblOIzdava.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e)
    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Bold)
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
    While size.Width * factor > CType(sender, XRLabel).Width()
      tempgolemina = CType(sender, XRLabel).Font.Size
      CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, tempgolemina - 1)
      size = gr.MeasureString(CType(sender, XRLabel).Text, CType(sender, XRLabel).Font)
    End While

    gr.Dispose()
    Return tempgolemina
  End Function

  Private Sub lblTel_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles lblTel.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e)
    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Bold)
  End Sub
End Class