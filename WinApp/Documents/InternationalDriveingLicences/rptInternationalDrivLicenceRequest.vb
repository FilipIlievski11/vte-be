Public Class rptInternationalDrivLicenceRequest
  Private WithEvents _docInternationalLicence As PrintDocumentInternationalDriveingLicenceList
  ' Private WithEvents _countryList As CountriesList
  'Private WithEvents _tehnicalOrganizationList As TehnicalExamOrganizationsList
  'Private WithEvents _cityList As CityList


  Public Sub New(ByVal inDocId As Long)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    _docInternationalLicence = PrintDocumentInternationalDriveingLicenceList.GetPrintDocumentInternationalDriveingLicenceList(inDocId)
    BindingSourceInternationalDrivLicence.DataSource = _docInternationalLicence
    Try
      lblGrad.Text = objCityList.GetCityListById(objCurentTehExamOrganization.IdCity).CityName
      'Dim pom As String = objRegistrationIssuerList.GetRegistrationIssuerInfo(objCurentTehExamOrganization.IdDefaultRegistrationIssuer).IssuerName
      'lblOrganDozvola.Text = ToLat(pom)
      'lblOrganBLK.Text = ToLat(pom)
      'lblOrganPasos.Text = ToLat(objCurentTehExamOrganization.OdgovorenOrgan)
    Catch ex As Exception
      lblGrad.Text = ""
    End Try

  End Sub
End Class