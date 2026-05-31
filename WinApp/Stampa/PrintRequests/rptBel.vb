Public Class rptBel
  Private WithEvents _vehicles As VehicleInfo 'PrintVehicleByIdWhiteRequestInfo
  Private WithEvents _customers As PrintCustomerByIdList
  Private WithEvents _document As Document
  Public Sub New(ByVal indexOptions As Integer, ByVal inVehicleId As Integer, _
                 ByVal inCustomerId As Integer, ByVal inDocument As Document)
    InitializeComponent()

        _vehicles = VehicleList.GetVehicleById(inVehicleId)
    '_vehicles = PrintVehicleByIdWhiteRequestList.GetPrintVehicleByIdWhiteRequestList(inVehicleId).Item(0)
    _customers = PrintCustomerByIdList.GetPrintCustomerByIdList(inCustomerId)
    _document = inDocument
    If _customers.Item(0).IsCompany Then
      lblCompanyName.Text = _customers.Item(0).CustomerFirstName
    Else
      lblCompanyName.Text = "ФИЗИЧКО ЛИЦЕ"
    End If
    Me.BindingSourceDocument.DataSource = _document
    Me.BindingSourceVehicle.DataSource = _vehicles
    Me.BindingSourceCustomer.DataSource = _customers

    Dim _lastTehnicalExam As VehicleLastTehnicalExamList = _
    VehicleLastTehnicalExamList.GetVehicleLastTehnicalExamList(_vehicles.Id)
    Me.BindingSourceLastTehnicalExam.DataSource = _lastTehnicalExam

    Dim powerSources As VehicleEnginePowerSourceTypeList = _
    VehicleEnginePowerSourceTypeList.GetVehicleEnginePowerSourceTypeList

    lblPrimaryPowerSource.Text = powerSources.GetPowerSourceTypeInfo _
    (_vehicles.IdEnginePowerSource).PowerSourceName
    lblSecondaryPowerSource.Text = powerSources.GetPowerSourceTypeInfo _
    (_vehicles.IdEngineSecondPowerSource).PowerSourceName

    If lblSecondaryPowerSource.Text = "[нема]" Then
      lblSecondaryPowerSource.Visible = False
    End If
    Select Case _vehicles.IdVehicleCategoryForPayments
      Case 6
        chkRabotnaMasina.Visible = True
      Case 3
        chkVelosipedSoMoteor.Visible = True
      Case 8
        chkZemjodelskiTraktor.Visible = True
      Case 12, 13, 14
        chkPriklucno.Visible = True
    End Select
    Select Case indexOptions
      Case 0
        CheckBoxDocTyprOption1.Visible = True
      Case 1
        CheckBoxDocTyprOption2.Visible = True
      Case 2
        CheckBoxDocTyprOption3.Visible = True
      Case 3
        CheckBoxDocTyprOption4.Visible = True
      Case 4
        CheckBoxDocTyprOption5.Visible = True
    End Select

    If _vehicles.IdVehicleCategories > 0 Then
      lblVehicleCtegory.Text = VehicleCategoryList.GetVehicleCategoryList.GetVehicleCategoryInfo _
      (_vehicles.IdVehicleCategories).Category
    Else
      lblVehicleCtegory.Text = ""
    End If

    If _vehicles.IdMadeCountry > 0 Then
      lblCountryMade.Text = CountriesList.GetCountriesList.GetCountriesListById _
      (_vehicles.IdMadeCountry).CountryName
    Else
      lblCountryMade.Text = ""
    End If

    If _vehicles.FirstRegistration = "непозната" Then
      XrLabel21.Visible = False
      XrLabel18.Visible = False
    End If

  End Sub


End Class