Public Class rptPlav
  Private WithEvents _vehicles As VehicleInfo
  Private WithEvents _customers As CustomersInfo
  Private WithEvents _document As Document
  Private WithEvents _colors As ColorsList
  Private WithEvents _countryList As CountriesList
  Private WithEvents _cityList As CityList
  Private WithEvents _customerList As CustomersList
  Private WithEvents _relationList As CustomerVehiclesRelationsList

  Public Sub New(ByVal indexOptions As Integer, ByVal inVehicle As VehicleInfo, _
                 ByVal inCustomer As CustomersInfo, ByVal inDocument As Document)
    InitializeComponent()
    Me.Margins.Top = objOpcii.PlavTopMargin
    Me.Margins.Left = objOpcii.PlavLeftMargin
    Me.Margins.Right = objOpcii.PlavRightMargin
    Me.Margins.Bottom = objOpcii.PlavButtonMargin

    _vehicles = inVehicle
    ClearPowerSourses()
    If _vehicles.Hook Then
      CheckBoxHookYes.Visible = True
      CheckBoxHookNo.Visible = False
      ' CheckBoxHookYes.CheckState = True
    Else
      CheckBoxHookYes.Visible = False
      CheckBoxHookNo.Visible = True
    End If

    _document = inDocument
    _cityList = CityList.GetCityList
    _countryList = CountriesList.GetCountriesList()
    _customerList = CustomersList.GetCustomersList
    _relationList = CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsList
    _customers = _customerList.GetCustomersListById(_relationList.GetInfoRelationById(_document.IdCustomerVehicleRelation).IdCustomer) 'inCustomer
    If Not _customers.IsCompany Then
      If _customers.IdCitizenship > 0 Then
        lblNotCompanyCitizenship.Text = _countryList.GetCountriesListById(_customers.IdCitizenship).Citizenship
      End If
      If _customers.IdBirhCity > 0 Then
        Dim BCity As CityInfo = _cityList.GetCityListById(_customers.IdBirhCity)
        lblNotCompanyCity.Text = BCity.CityName
        lblNotCompanyCommunity.Text = BCity.CommunityName
        lblNotCompanyCountry.Text = BCity.CountryName
      Else
        lblNotCompanyCity.Text = ""
        lblNotCompanyCommunity.Text = ""
        lblNotCompanyCountry.Text = ""
      End If
    Else
      If _customers.IdBusinessType > 0 Then
        lblCompanyBusinessType.Text = BusinessTypeList.GetBusinessTypeList.GetBusinessTypeInfo _
        (_customers.IdBusinessType).BusinessTypeDescription
      Else
        lblCompanyBusinessType.Text = ""
      End If
    End If
    Me.BindingSourceDocument.DataSource = _document
    Me.BindingSourceVehicle.DataSource = _vehicles
    Me.BindingSourceCustomer.DataSource = _customers
 
    Dim _lastTehnicalExam As VehicleLastTehnicalExamList = VehicleLastTehnicalExamList.GetVehicleLastTehnicalExamList(_vehicles.Id)
    Me.BindingSourceLastTehnicalExam.DataSource = _lastTehnicalExam
    _colors = ColorsList.GetColorsList
    _countryList = CountriesList.GetCountriesList
    Dim categoryNumberOld As String
    categoryNumberOld = VehicleCategoryForPaymentsList.GetVehicleCategoryForPaymentsList.GetVehicleCategoryForPaymentsInfo(_vehicles.IdVehicleCategoryForPayments).Code
    Select Case Strings.Left(categoryNumberOld, 1)
      Case "1"
        CheckBoxVehicleCategory1.Visible = True
      Case "2"
        CheckBoxVehicleCategory2.Visible = True
      Case "3"
        CheckBoxVehicleCategory3.Visible = True
      Case "4"
        CheckBoxVehicleCategory4.Visible = True
      Case "5"
        CheckBoxVehicleCategory5.Visible = True
      Case "6"
        CheckBoxVehicleCategory6.Visible = True
      Case "7"
        CheckBoxVehicleCategory7.Visible = True
      Case "8"
        CheckBoxVehicleCategory8.Visible = True
    End Select
    If _vehicles.IdEngineSecondPowerSource > 0 Then
      CheckBoxPowerSource2.Visible = True
    Else
      Select Case _vehicles.IdEnginePowerSource
        Case 1
          CheckBoxPowerSource1.Visible = True
        Case 2
          CheckBoxPowerSource3.Visible = True
        Case 3
          CheckBoxPowerSource4.Visible = True
        Case 4
          CheckBoxPowerSource5.Visible = True
        Case 5
          CheckBoxPowerSource6.Visible = True
      End Select
    End If
    Select Case indexOptions
      Case 0
        Me.CheckBoxPrvPat.Visible = True
      Case 1
        Dim prethodenSopstvenik As CustomersInfo = _customerList.GetCustomersListById _
        (_relationList.GetInfoRelationById(_document.IdCustomerVehicleRelationHistory).IdCustomer)
        Me.lblPrethodnaRegFirstName.Text = prethodenSopstvenik.CustomerFirstName
        Me.lblPrethodnaRegSurName.Text = prethodenSopstvenik.CustomerSurname
        Me.lblPrethodnaRegAddress.Text = prethodenSopstvenik.AddressOfLiving
        Me.lblPrethodnaRegMB.Text = prethodenSopstvenik.MB

        Dim prethodnaReg As RegistrationsInfo = RegistrationsList.GetRegistrationInfoById(_document.IdPreviousRegistration).Item(0)
        Me.lblPrethodnaRegBr.Text = prethodnaReg.RegistrationNumber
        Me.lblPrethodnaRegDtum.Text = prethodnaReg.DateRegistrationValidTill.Date
        Me.lblPrethodnaRegOrgan.Text = prethodnaReg.IssuerName

        Me.CheckBoxPovtorno.Visible = True
        Me.lblPrethodnaRegAddress.Visible = True
        Me.lblPrethodnaRegBr.Visible = True
        Me.lblPrethodnaRegDtum.Visible = True
        Me.lblPrethodnaRegFirstName.Visible = True
        Me.lblPrethodnaRegMB.Visible = True
        Me.lblPrethodnaRegOrgan.Visible = True
        Me.lblPrethodnaRegSurName.Visible = True
      Case 2
        Me.CheckBoxPrivremeno.Visible = True
    End Select
    If _vehicles.IdVehicleBodyType > 0 Then
      lblBodyType.Text = VehicleBodytypeList.GetVehicleBodytypeList.GetVehicleBodytypeInfo _
      (_vehicles.IdVehicleBodyType).BodytypeDescriprion
    Else
      lblBodyType.Text = ""
    End If
    If _vehicles.IdEngineType > 0 Then
      lblEngineType.Text = VehicleEngineTypeList.GetVehicleEngineTypeList.GetVehicleEngineTypeById _
      (_vehicles.IdEngineType).TechincalDescriptionEcoProgram
    Else
      lblEngineType.Text = ""
    End If
    If _vehicles.IdVehicleUse > 0 Then
      lblVehicleuse.Text = VehicleUseList.GetVehicleUseList.GetUseIById(_vehicles.IdVehicleUse).UseDescription
    Else
      lblVehicleuse.Text = ""
    End If
   
    If _vehicles.IdMadeCountry > 0 Then
      Try
        lblVehicleMadeCountry.Text = _countryList.GetCountriesListById(_vehicles.IdMadeCountry).CountryName
      Catch ex As Exception
        lblVehicleMadeCountry.Text = ""
      End Try

    End If
   
    If _vehicles.IdPrimaryColor > 0 Then
      Dim firsColor As ColorsInfo = _colors.GetColorsInfo(_vehicles.IdPrimaryColor)
      lblFirstColor.Text = firsColor.ColorDescription
      lblFirstColorCode.Text = firsColor.ColorCode
    End If
    If _vehicles.IdSecondaryColor > 0 Then
      Dim secondColor As ColorsInfo = _colors.GetColorsInfo(_vehicles.IdSecondaryColor)
      lblSecondColor.Text = secondColor.ColorDescription
      lblScondColorCode.Text = secondColor.ColorCode
    End If
    Select Case _customers.IsCompany
      Case True
        CheckBoxIsNotCompany.Visible = False
        CheckBoxIsCompany.Visible = True
        Me.lblCompanyAddress.Visible = True
        Me.lblCompanyBusinessType.Visible = True
        Me.lblCompanyCity.Visible = True
        Me.lblCompanyCommunity.Visible = True
        Me.lblCompanyCountry.Visible = True
        Me.lblCompanyName.Visible = True
      Case False
        CheckBoxIsCompany.Visible = False
        CheckBoxIsNotCompany.Visible = True
        Me.lblNotCompanyCitizenship.Visible = True
        Me.lblNotCompanyCity.Visible = True
        Me.lblNotCompanyCountry.Visible = True
        Me.lblNotCompanyCommunity.Visible = True
        Me.lblNotCompanyFirstName.Visible = True
        Me.lblNotCompanyLivingAddress.Visible = True
        Me.lblNotCompanyLivingCity.Visible = True
        Me.lblNotCompanyLivingCommunity.Visible = True
        Me.lblNotCompanyLivingCountry.Visible = True
        Me.lblNotCompanyMB.Visible = True
        Me.lblNotCompanyOccupation.Visible = True
        Me.lblNotCompanySurname.Visible = True
        Me.lblNotCompanyWorksIn.Visible = True
        Me.lblNotCompanyDateOfBirth.Visible = True
    End Select
    If _vehicles.FirstRegistration = "непозната" Then
      XrLabel3.Visible = False
      XrLabel4.Visible = False
      XrLabel5.Visible = False
    End If
  End Sub
  Private Sub ClearPowerSourses()
    CheckBoxPowerSource1.Visible = False
    CheckBoxPowerSource2.Visible = False
    CheckBoxPowerSource3.Visible = False
    CheckBoxPowerSource4.Visible = False
    CheckBoxPowerSource5.Visible = False
    CheckBoxPowerSource6.Visible = False
  End Sub
End Class