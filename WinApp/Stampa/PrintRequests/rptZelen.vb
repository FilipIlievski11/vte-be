Public Class rptZelen

  Private WithEvents _vehicles As VehicleInfo
  Private WithEvents _customers As CustomersInfo
  Private WithEvents _category As VehicleCategorie
  Private WithEvents _document As Document
  Private WithEvents _categoryPaymentList As VehicleCategoryForPaymentsList
  Private WithEvents _cityList As CityList
  Private WithEvents _streetList As StreetsList
  Public Sub New(ByVal indexOptions As Integer, ByVal indexDetails As Integer, ByVal inVehicle As VehicleInfo, _
                 ByVal inCustomer As CustomersInfo, ByVal inDocument As Document)
    'lblCarreingCapacity .Text =
    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    _vehicles = inVehicle
    Try
      _category = VehicleCategorie.GetVehicleCategorie(_vehicles.IdVehicleCategories)
    Catch ex As Exception

    End Try
    If _vehicles.IdEngineType > 0 Then
      lblEngineType.Text = VehicleEngineTypeList.GetVehicleEngineTypeList.GetVehicleEngineTypeById _
      (_vehicles.IdEngineType).TechincalDescriptionEcoProgram
    Else
      lblEngineType.Text = ""
    End If
    _document = inDocument
    _customers = CustomersList.GetCustomersList.GetCustomersListById _
    (CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsList.GetInfoRelationById _
     (_document.IdCustomerVehicleRelation).IdCustomer) 'inCustomer

    ' Add any initialization after the InitializeComponent() call.
    Me.BindingSourceVehicle.DataSource = _vehicles
    Me.BindingSourceCustomer.DataSource = _customers
    Me.BindingSourceDocument.DataSource = _document
    ' Me.BindingSourceAddresses.DataSource = StreetsList.GetStreetsList
    'Me.BindingSourceBusinessType.DataSource = BusinessTypeList.GetBusinessTypeList
    _cityList = CityList.GetCityList
    _streetList = StreetsList.GetStreetsList
    ' Me.BindingSourceColors.DataSource = ColorsList.GetColorsList
    ' Me.BindingSourceCommunities.DataSource = CommunitiesList.GetCommunitiesList
    'Me.BindingSourceCountries.DataSource = CountriesList.GetCountriesList
    _categoryPaymentList = VehicleCategoryForPaymentsList.GetVehicleCategoryForPaymentsList
    'Me.BindingSourcePaymentCategory.DataSource = _categoryPaymentList
    ' Me.BindingSourceEngineTypes.DataSource = VehicleEngineTypeList.GetVehicleEngineTypeList
    ' Me.BindingSourceVehicleBodytypes.DataSource = VehicleBodytypeList.GetVehicleBodytypeList
    'Me.BindingSourceVehicleCategory.DataSource = VehicleCategoryList.GetVehicleCategoryList
    ' Me.BindingSourceVehicleMakers.DataSource = VehicleMakerList.GetVehicleMakerList
    'Me.BindingSourceVehicleModels.DataSource = VehicleModelList.GetVehicleModelList
    Dim _lastTehnicalExam As VehicleLastTehnicalExamList = VehicleLastTehnicalExamList.GetVehicleLastTehnicalExamList(_vehicles.Id)
    Me.BindingSourceLastTehnicalExam.DataSource = _lastTehnicalExam
    If _vehicles.IdVehicleCategoryForPayments > 0 Then
      lblCategoryPayment.Text = _categoryPaymentList.GetVehicleCategoryForPaymentsInfo(_vehicles.IdVehicleCategoryForPayments).Name
    Else
      lblCategoryPayment.Text = ""
    End If
    If _vehicles.IdVehicleBodyType > 0 Then
      lblBodyType.Text = VehicleBodytypeList.GetVehicleBodytypeList.GetVehicleBodytypeInfo(_vehicles.IdVehicleBodyType).BodytypeDescriprion
    Else
      lblBodyType.Text = ""
    End If
    If _document.IdVehicleOwnershipProof > 0 Then
      lblOwnershipProof.Text = DocumentVehicleOwnershipProofList.GetDocumentVehicleOwnershipProofList.GetDocumentVehicleOwnershipProofInfo _
      (_document.IdVehicleOwnershipProof).VehicleOwnershipProofName & " " & _document.VehicleOwnershipProof
    Else
      lblOwnershipProof.Text = ""
    End If
    If _document.IdPaymentProof > 0 Then
      lblPaymentProof.Text = DocumentPaymentProofList.GetDocumentPaymentProofList.GetDocumentPaymentProofInfo(_document.IdPaymentProof).PaymentProofName & _
      " " & _document.PaymentProof
    Else
      lblPaymentProof.Text = ""
    End If
        lblOpstina.Text = objCommunityList.GetCommunitiesListById(objCurentTehExamOrganization.IdCommunity).CommunityName
    'If _customers.IdBirhCity > 0 Then
    '  Try
    '    lblNewBirthCity.Text = _cityList.GetCityListById(_customers.IdBirhCity).CityName
    '  Catch ex As Exception
    '    lblNewBirthCity.Text = ""
    '  End Try
    'Else
    '  lblNewBirthCity.Text = ""
    'End If

    If _customers.IdLivingAddress > 0 Then
      Dim pomStreet As String = StreetsList.GetStreetsList.GetStreetInfo(_customers.IdLivingAddress).StreetName
      lblStreet.Text = pomStreet
      lblNewLivingAddress.Text = pomStreet
    Else
      lblStreet.Text = ""
      lblNewLivingAddress.Text = ""
    End If
    Dim colorPrimary As ColorsInfo
    If _vehicles.IdPrimaryColor > 0 Then
      colorPrimary = ColorsList.GetColorsList.GetColorsInfo(_vehicles.IdPrimaryColor)
    Else
      colorPrimary = Nothing
    End If

    Dim colorSecondary As ColorsInfo
    If _vehicles.IdSecondaryColor > 0 Then
      colorSecondary = ColorsList.GetColorsList.GetColorsInfo(_vehicles.IdSecondaryColor)
    Else
      colorSecondary = Nothing
    End If

    If colorPrimary IsNot Nothing Then
      lblColor1.Text = colorPrimary.ColorDescription
      lblColor11.Text = colorPrimary.ColorCode
    Else
      lblColor1.Text = ""
      lblColor11.Text = ""
    End If
    If colorSecondary IsNot Nothing Then
      lblColor2.Text = colorSecondary.ColorDescription
      lblColor21.Text = colorSecondary.ColorCode
    Else
      lblColor2.Text = ""
      lblColor21.Text = ""
    End If
    Try
      Dim pomModel As VehicleModelInfo = VehicleModelList.GetVehicleModelList.GetVehicleModelInfoById(_vehicles.IdVehicleModel)
      lblModel.Text = pomModel.ModelName
      lblType.Text = pomModel.VehicleMaker
    Catch ex As Exception

    End Try
    ResetCheckBox()
    Try
      If _lastTehnicalExam.Item(0).VehicleIsRight Then
        lblIsReady.Visible = True
      Else
        lblIsNotReady.Visible = True
      End If
    Catch ex As Exception
      MessageBox.Show("Не е извршен технички преглед")

      'Me.ClosePreview()
    End Try
    If _document.IdDocumentTypeOption = DocumentTypesList.GetDocumentTypesList.Item(0).Id Then
      PromenaNapodatociIliSopstvenost()
    End If
    Dim categoryNumberOld As String
    categoryNumberOld = VehicleCategoryForPaymentsList.GetVehicleCategoryForPaymentsList.GetVehicleCategoryForPaymentsInfo(_vehicles.IdVehicleCategoryForPayments).Code
    Select Case Strings.Left(categoryNumberOld, 1)
      Case "1"
        XrCheckBox1.Visible = True
      Case "2"
        XrCheckBox2.Visible = True
      Case "3"
        XrCheckBox3.Visible = True
      Case "4"
        XrCheckBox4.Visible = True
      Case "5"
        XrCheckBox5.Visible = True
      Case "6"
        XrCheckBox6.Visible = True
      Case "7"
        XrCheckBox7.Visible = True
      Case "8"
        XrCheckBox8.Visible = True
    End Select
    Select Case indexOptions
      Case 0, 3
        CheckBoxA.Visible = True
        Select Case indexDetails
          Case 0
            CheckBoxA1.Visible = True
            CheckBoxA1.Checked = True
          Case 1
            CheckBoxA2.Visible = True
            CheckBoxA2.Checked = True
          Case 2
            CheckBoxA3.Visible = True
            CheckBoxA3.Checked = True
          Case 3
            CheckBoxA4.Visible = True
            CheckBoxA4.Checked = True
        End Select
      Case 1
        CheckBoxB.Visible = True
      Case 2
        CheckBoxV.Visible = True
        Select Case indexDetails
          Case 0
            CheckBoxV1.Visible = True
            CheckBoxV1.Checked = True
          Case 1
            CheckBoxV2.Visible = True
            CheckBoxV2.Checked = True
          Case 2
            CheckBoxV3.Visible = True
            CheckBoxV3.Checked = True
          Case 3
            CheckBoxV4.Visible = True
            CheckBoxV4.Checked = True
          Case 4
            CheckBoxV5.Visible = True
            CheckBoxV5.Checked = True
          Case 5
            CheckBoxV6.Visible = True
            CheckBoxV6.Checked = True
          Case 6
            CheckBoxV7.Visible = True
            CheckBoxV7.Checked = True
          Case 7
            CheckBoxV8.Visible = True
            CheckBoxV8.Checked = True

        End Select
    End Select
  End Sub
  Private Sub ResetCheckBox()
    CheckBoxA.Visible = False
    CheckBoxB.Visible = False
    CheckBoxV.Visible = False
    CheckBoxV1.Visible = False
    CheckBoxV2.Visible = False
    CheckBoxV3.Visible = False
    CheckBoxV4.Visible = False
    CheckBoxV5.Visible = False
    CheckBoxV6.Visible = False
    CheckBoxV7.Visible = False
    CheckBoxV8.Visible = False
    CheckBoxA1.Visible = False
    CheckBoxA2.Visible = False
    CheckBoxA3.Visible = False
    CheckBoxA4.Visible = False
    CheckBoxIsCompany.Visible = False
    CheckBoxIsNotCompany.Visible = False
    lblIsReady.Visible = False
    lblIsNotReady.Visible = False

    If _document.IdDocumentTypeOption = 3 Then
      Me.panelPokriVozilo.Visible = True
      Me.panelPokriVozilo.BringToFront()
      Me.lblLastRegistration.Visible = False
    Else
      Me.panelPokriVozilo.Visible = False
      Me.panelPokriVozilo.SendToBack()
      Me.lblLastRegistration.Visible = True
    End If
  End Sub
  Private Sub PromenaNapodatociIliSopstvenost()

    If _document.IdCustomerVehicleRelationHistory > 0 Then

      Dim NewCustomer As CustomersInfo = CustomersList.GetCustomersList.GetCustomersListById _
      (CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsList.GetInfoRelationById _
           (_document.IdCustomerVehicleRelationHistory).IdCustomer)
      If NewCustomer.IsCompany Then
        lblNewSurnameCompany.Text = NewCustomer.CustomerSurname
        lblNewFirstNameCompany.Text = NewCustomer.CustomerFirstName
        lblNewEMBGCompany.Text = NewCustomer.MB
        If NewCustomer.IdBusinessType > 0 Then
          lblNewBusinessTypeCompany.Text = BusinessTypeList.GetBusinessTypeList.GetBusinessTypeInfo _
            (NewCustomer.IdBusinessType).BusinessTypeDescription
        Else
          lblNewBusinessTypeCompany.Text = ""
        End If

        lblNewCityCompany.Text = NewCustomer.CityName
        lblNewCommunityCompany.Text = NewCustomer.CommunityName
        lblNewLivingCountryCompany.Text = NewCustomer.CountryName
        lblNewLivingAddressCompany.Text = NewCustomer.AddressOfLiving

        lblNewSurnameCompany.Visible = True
        lblNewFirstNameCompany.Visible = True
        lblNewEMBGCompany.Visible = True
        lblNewBusinessTypeCompany.Visible = True
        lblNewCityCompany.Visible = True
        lblNewCommunityCompany.Visible = True
        lblNewLivingCountryCompany.Visible = True
        lblNewLivingAddressCompany.Visible = True

        CheckBoxIsCompany.Visible = True
        CheckBoxIsCompany.Checked = True
      Else
        lblNewSurname.Text = NewCustomer.CustomerSurname
        lblNewFirstName.Text = NewCustomer.CustomerFirstName
        lblNewEMBG.Text = NewCustomer.MB
        lblNewBirthDate.Text = NewCustomer.BirthDate
        If NewCustomer.IdBirhCity > 0 Then
          lblNewBirthCity.Text = _cityList.GetCityListById(NewCustomer.IdBirhCity).CityName
          lblNewBirthCommunity.Text = _cityList.GetCityListById(NewCustomer.IdBirhCity).CommunityName
          lblNewBirthCountry.Text = _cityList.GetCityListById(NewCustomer.IdBirhCity).CountryName
        Else
          lblNewBirthCity.Text = ""
          lblNewBirthCommunity.Text = ""
          lblNewBirthCountry.Text = ""
        End If
        If NewCustomer.IdCitizenship > 0 Then
          lblNewCitizenship.Text = CountriesList.GetCountriesList.GetCountriesListById(NewCustomer.IdCitizenship).Citizenship
        Else
          lblNewCitizenship.Text = ""
        End If
        lblNewOccupation.Text = NewCustomer.Occupation
        lblNewWorksInCompany.Text = NewCustomer.WorksInCompany
        If NewCustomer.IdLivingCity > 0 Then
          lblNewCity.Text = _cityList.GetCityListById(NewCustomer.IdLivingCity).CityName
          lblNewCommunity.Text = _cityList.GetCityListById(NewCustomer.IdLivingCity).CommunityName
          lblNewLivingCountry.Text = _cityList.GetCityListById(NewCustomer.IdLivingCity).CountryName
        Else
          lblNewCity.Text = ""
          lblNewCommunity.Text = ""
          lblNewLivingCountry.Text = ""
        End If
        If NewCustomer.IdLivingAddress > 0 Then
          lblNewLivingAddress.Text = _streetList.GetStreetInfo(NewCustomer.IdLivingAddress).StreetName _
          & " " & NewCustomer.LivingAddressNumber
        Else
          lblNewLivingAddress.Text = ""
        End If

        lblNewSurname.Visible = True
        lblNewFirstName.Visible = True
        lblNewEMBG.Visible = True
        lblNewBirthDate.Visible = True
        lblNewBirthCity.Visible = True
        lblNewBirthCommunity.Visible = True
        lblNewBirthCountry.Visible = True
        lblNewCitizenship.Visible = True
        lblNewOccupation.Visible = True
        lblNewWorksInCompany.Visible = True
        lblNewCity.Visible = True
        lblNewCommunity.Visible = True
        lblNewLivingCountry.Visible = True
        lblNewLivingAddress.Visible = True

        CheckBoxIsNotCompany.Visible = True
        CheckBoxIsNotCompany.Checked = True
      End If
    Else
      If _customers.IsCompany Then
        lblNewSurnameCompany.Text = _customers.CustomerSurname
        lblNewFirstNameCompany.Text = _customers.CustomerFirstName
        lblNewEMBGCompany.Text = _customers.MB
        If _customers.IdBusinessType > 0 Then
          lblNewBusinessTypeCompany.Text = BusinessTypeList.GetBusinessTypeList.GetBusinessTypeInfo _
        (_customers.IdBusinessType).BusinessTypeDescription
        Else
          lblNewBusinessTypeCompany.Text = ""
        End If

        lblNewCityCompany.Text = _customers.CityName
        lblNewCommunityCompany.Text = _customers.CommunityName
        lblNewLivingCountryCompany.Text = _customers.CountryName
        lblNewLivingAddressCompany.Text = _customers.AddressOfLiving

        lblNewSurnameCompany.Visible = True
        lblNewFirstNameCompany.Visible = True
        lblNewEMBGCompany.Visible = True
        lblNewBusinessTypeCompany.Visible = True
        lblNewCityCompany.Visible = True
        lblNewCommunityCompany.Visible = True
        lblNewLivingCountryCompany.Visible = True
        lblNewLivingAddressCompany.Visible = True

        CheckBoxIsCompany.Visible = True
        CheckBoxIsCompany.Checked = True
      Else
        lblNewSurname.Text = _customers.CustomerSurname
        lblNewFirstName.Text = _customers.CustomerFirstName
        lblNewEMBG.Text = _customers.MB
        lblNewBirthDate.Text = _customers.BirthDate
        If _customers.IdBirhCity > 0 Then
          lblNewBirthCity.Text = _cityList.GetCityListById(_customers.IdBirhCity).CityName
          lblNewBirthCommunity.Text = _cityList.GetCityListById(_customers.IdBirhCity).CommunityName
          lblNewBirthCountry.Text = _cityList.GetCityListById(_customers.IdBirhCity).CountryName
        Else
          lblNewBirthCity.Text = ""
          lblNewBirthCommunity.Text = ""
          lblNewBirthCountry.Text = ""
        End If
        If _customers.IdCitizenship > 0 Then
          lblNewCitizenship.Text = CountriesList.GetCountriesList.GetCountriesListById(_customers.IdCitizenship).Citizenship
        Else
          lblNewCitizenship.Text = ""
        End If
        lblNewOccupation.Text = _customers.Occupation
        lblNewWorksInCompany.Text = _customers.WorksInCompany
        If _customers.IdLivingAddress > 0 Then
          lblNewCity.Text = _cityList.GetCityListById(_customers.IdLivingCity).CityName
          lblNewCommunity.Text = _cityList.GetCityListById(_customers.IdLivingCity).CommunityName
          lblNewLivingCountry.Text = _cityList.GetCityListById(_customers.IdLivingCity).CountryName
        Else
          lblNewCity.Text = ""
          lblNewCommunity.Text = ""
          lblNewLivingCountry.Text = ""
        End If
        If _customers.IdLivingAddress > 0 Then
          lblNewLivingAddress.Text = _streetList.GetStreetInfo(_customers.IdLivingAddress).StreetName _
          & " " & _customers.LivingAddressNumber
        Else
          lblNewLivingAddress.Text = ""
        End If

        lblNewSurname.Visible = True
        lblNewFirstName.Visible = True
        lblNewEMBG.Visible = True
        lblNewBirthDate.Visible = True
        lblNewBirthCity.Visible = True
        lblNewBirthCommunity.Visible = True
        lblNewBirthCountry.Visible = True
        lblNewCitizenship.Visible = True
        lblNewOccupation.Visible = True
        lblNewWorksInCompany.Visible = True
        lblNewCity.Visible = True
        lblNewCommunity.Visible = True
        lblNewLivingCountry.Visible = True
        lblNewLivingAddress.Visible = True
        CheckBoxIsNotCompany.Visible = True
        CheckBoxIsNotCompany.Checked = True
      End If
    End If

  End Sub

End Class