Public Class rptNewTehnicalExamReport
  Private WithEvents _techExamReport As TechExamReportNewRptList
  Private WithEvents _relacija As VehicleCustomerForNewTechReportInfo
  Public Sub New(ByVal inIdRelation As Long, ByVal inIdTechReport As Long)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    _techExamReport = TechExamReportNewRptList.GetTechExamReportNewRptListById(inIdTechReport)
    BindingSourceTechReport.DataSource = _techExamReport

    PictureBoxLogo.ImageUrl = objCurentTehExamOrganization.LogoPath

    _relacija = VehicleCustomerForNewTechReportList.GetVehicleCustomerForNewTechReportByIdRelation(inIdRelation)
    BindingSourceCustomerVehicle.DataSource = _relacija

    lblCompany.Text = objCurentTehExamOrganization.OrganizationName 'GetTehnicalExamOrganizationsInfoById(objCurentTehExamOrganization.IdCompany).OrganizationName
    lblCustomer.Text = _relacija.CustomerFirstName & " " & _relacija.CustomerSurname & "; " & _
    _relacija.StreetName & " " & _relacija.LivingAddressNumber


    ' Add any initialization after the InitializeComponent() call.
    TehnickiPromeni()
  End Sub

  Private Sub TehnickiPromeni()
    If _techExamReport(0).TechnicalChanges <> String.Empty Then
      Dim pomString As String = _techExamReport(0).TechnicalChanges
      For i As Integer = 1 To 4
        Dim pomIndeks As Integer = pomString.IndexOf(";")
        Select Case i
          Case 1
            lblTehnickiPromeni1.Text = pomString.Substring(0, pomIndeks - 1)
          Case 2
            lblTehnickiPromeni2.Text = pomString.Substring(0, pomIndeks - 1)
          Case 3
            lblTehnickiPromeni3.Text = pomString.Substring(0, pomIndeks - 1)
          Case 4
            lblTehnickiPromeni4.Text = pomString
        End Select
        pomString = pomString.Substring(pomIndeks + 1)
      Next
    End If
  End Sub
End Class