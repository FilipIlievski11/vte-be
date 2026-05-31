
<Serializable()> _
Public Class PrintCustomerInfo
  Inherits ReadOnlyBase(Of PrintCustomerInfo)

#Region " Calculated Fields "
  Public ReadOnly Property LivingAddressFull() As String
    Get
      Return UCase("ул. " & _streetNameLiving & " бр." & _livingAddressNumber & ", " & _aCityNameLiving & " - " & _countryNameLiving)
    End Get
  End Property
  Public ReadOnly Property LivingAddress() As String
        Get
            Dim pom As String
            If _aCityNameLiving <> _communityNameLiving Then
                pom = (_aCityNameLiving & ",  " & _communityNameLiving)
            Else
                pom = _communityNameLiving
            End If
            If Trim(_livingAddressNumber) = "" Then
                If Trim(_streetNameLiving) = "" Then
                    Return pom
                Else
                    Return UCase("ул. " & _streetNameLiving & " " & pom)
                End If
            Else
                Return UCase("ул. " & _streetNameLiving & " бр." & _livingAddressNumber & " " & pom)
            End If
        End Get
  End Property
  Public ReadOnly Property CustomerLastFirstname() As String
    Get
      Return UCase(_customerSurname & " " & _customerFirstName)
    End Get
  End Property

#End Region

  Private _id As Long
  Public ReadOnly Property Id() As Long
    Get
      Return _id
    End Get
  End Property
  Private _mb As String
  Public ReadOnly Property MB() As String
    Get
      Return _mb
    End Get
  End Property
  Private _customerSurname As String
  Public ReadOnly Property CustomerSurname() As String
    Get
      Return UCase(_customerSurname)
    End Get
  End Property
  Private _customerFirstName As String
  Public ReadOnly Property CustomerFirstName() As String
    Get
      Return UCase(_customerFirstName)
    End Get
  End Property
  Private _dateOfBirth As Date
  Public ReadOnly Property DateOfBirth() As String
    Get
      If _dateOfBirth.Date = Date.MinValue Then
        Return ""
      Else
        Return _dateOfBirth
      End If
    End Get
  End Property
  Private _isCompany As Boolean
  Public ReadOnly Property IsCompany() As Boolean
    Get
      Return _isCompany
    End Get
  End Property
  Private _occupation As String
  Public ReadOnly Property Occupation() As String
    Get
      Return UCase(_occupation)
    End Get
  End Property
  Private _worksInCompany As String
  Public ReadOnly Property WorksInCompany() As String
    Get
            Return UCase(_worksInCompany)
    End Get
  End Property
  Private _bLK As String
  Public ReadOnly Property BLK() As String
    Get
      Return _bLK
    End Get
  End Property
  Private _passportNumber As String
  Public ReadOnly Property PassportNumber() As String
    Get
      Return _passportNumber
    End Get
  End Property
  Private _brithAddressNumber As String
  Public ReadOnly Property BrithAddressNumber() As String
    Get
      Return _brithAddressNumber
    End Get
  End Property
  Private _livingAddressNumber As String
  Public ReadOnly Property LivingAddressNumber() As String
    Get
      Return _livingAddressNumber
    End Get
  End Property
  Private _idLivingAddress As Integer
  Public ReadOnly Property IdLivingAddress() As Integer
    Get
      Return _idLivingAddress
    End Get
  End Property
  Private _streetNameLiving As String
  Public ReadOnly Property StreetNameLiving() As String
    Get
      Return _streetNameLiving
    End Get
  End Property
  Private _idLivingCity As Integer
  Public ReadOnly Property IdLivingCity() As Integer
    Get
      Return UCase(_idLivingCity)
    End Get
  End Property
  Private _idBirhCity As Integer
  Public ReadOnly Property IdBirhCity() As Integer
    Get
      Return _idBirhCity
    End Get
  End Property
  Private _cityNameBirh As String
  Public ReadOnly Property CityNameBirh() As String
    Get
      Return UCase(_cityNameBirh)
    End Get
  End Property
  Private _cityZipBirh As Integer
  Public ReadOnly Property CityZipBirh() As Integer
    Get
      Return _cityZipBirh
    End Get
  End Property
  Private _aCityNameLiving As String
  Public ReadOnly Property ACityNameLiving() As String
    Get
      Return UCase(_aCityNameLiving)
    End Get
  End Property
  Private _cityZipLiving As Integer
  Public ReadOnly Property CityZipLiving() As Integer
    Get
      Return _cityZipLiving
    End Get
  End Property
  Private _idCountryLiving As Integer
  Public ReadOnly Property IdCountryLiving() As Integer
    Get
      Return _idCountryLiving
    End Get
  End Property
  Private _countryShortNameLiving As String
  Public ReadOnly Property CountryShortNameLiving() As String
    Get
      Return _countryShortNameLiving
    End Get
  End Property
  Private _countryNameLiving As String
  Public ReadOnly Property CountryNameLiving() As String
    Get
      Return _countryNameLiving
    End Get
  End Property
  Private _idCommunityCode As Integer
  Public ReadOnly Property IdCommunityCode() As Integer
    Get
      Return _idCommunityCode
    End Get
  End Property
  Private _communityCodeLiving As String
  Public ReadOnly Property CommunityCodeLiving() As String
    Get
      Return _communityCodeLiving
    End Get
  End Property
  Private _communityNameLiving As String
  Public ReadOnly Property CommunityNameLiving() As String
    Get
      Return UCase(_communityNameLiving)
    End Get
  End Property
  Private _idCommunityCodeBirth As Integer
  Public ReadOnly Property IdCommunityCodeBirth() As Integer
    Get
      Return _idCommunityCodeBirth
    End Get
  End Property
  Private _communityCodeBirth As String
  Public ReadOnly Property CommunityCodeBirth() As String
    Get
      Return UCase(_communityCodeBirth)
    End Get
  End Property
  Private _communityNameBirth As String
  Public ReadOnly Property CommunityNameBirth() As String
    Get
      Return _communityNameBirth
    End Get
  End Property
  Private _idCountryBirth As Integer
  Public ReadOnly Property IdCountryBirth() As Integer
    Get
      Return _idCountryBirth
    End Get
  End Property
  Private _countryNameBirth As String
  Public ReadOnly Property CountryNameBirth() As String
    Get
      Return _countryNameBirth
    End Get
  End Property
  Private _countryShortNameBirht As String
  Public ReadOnly Property CountryShortNameBirht() As String
    Get
      Return _countryShortNameBirht
    End Get
  End Property
  Private _idCitizenship As Integer
  Public ReadOnly Property IdCitizenship() As Integer
    Get
      Return _idCitizenship
    End Get
  End Property
  Private _citizenship As String
  Public ReadOnly Property Citizenship() As String
    Get
      Return _citizenship
    End Get
  End Property
  Private _IdBusinessType As Integer
  Public ReadOnly Property IdBusinessType() As Integer
    Get
      Return _IdBusinessType
    End Get
  End Property
  Private _BusinessTypeCode As String
  Public ReadOnly Property BusinessTypeCode() As String
    Get
      Return _BusinessTypeCode
    End Get
  End Property
  Private _BusinessTypeDescription As String
  Public ReadOnly Property BusinessTypeDescription() As String
    Get
      Return UCase(_BusinessTypeDescription)
    End Get
  End Property

  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt64("Id")
    _mb = dr.GetString("MB")
    _customersurname = dr.GetString("CustomerSurname")
    _customerfirstname = dr.GetString("CustomerFirstName")
    _dateofbirth = dr.GetDateTime("DateOfBirth")
    _iscompany = dr.GetBoolean("IsCompany")
    _occupation = dr.GetString("Occupation")
    _worksincompany = dr.GetString("WorksInCompany")
    _blk = dr.GetString("BLK")
    _passportnumber = dr.GetString("PassportNumber")
    _brithaddressnumber = dr.GetString("BrithAddressNumber")
    _livingaddressnumber = dr.GetString("LivingAddressNumber")
    _idlivingaddress = dr.GetInt32("IdLivingAddress")
    _streetnameliving = dr.GetString("StreetNameLiving")
    _idlivingcity = dr.GetInt32("IdLivingCity")
    _idbirhcity = dr.GetInt32("IdBirhCity")
    _citynamebirh = dr.GetString("CityNameBirh")
    _cityzipbirh = dr.GetInt32("CityZipBirh")
    _acitynameliving = dr.GetString("aCityNameLiving")
    _cityzipliving = dr.GetInt32("CityZipLiving")
    _idcountryliving = dr.GetInt32("IdCountryLiving")
    _countryshortnameliving = dr.GetString("CountryShortNameLiving")
    _countrynameliving = dr.GetString("CountryNameLiving")
    _idcommunitycode = dr.GetInt32("IdCommunityCode")
    _communitycodeliving = dr.GetString("CommunityCodeLiving")
    _communitynameliving = dr.GetString("CommunityNameLiving")
    _idcommunitycodebirth = dr.GetInt32("IdCommunityCodeBirth")
    _communitycodebirth = dr.GetString("CommunityCodeBirth")
    _communitynamebirth = dr.GetString("CommunityNameBirth")
    _idcountrybirth = dr.GetInt32("IdCountryBirth")
    _countrynamebirth = dr.GetString("CountryNameBirth")
    _countryshortnamebirht = dr.GetString("CountryShortNameBirht")
    _idcitizenship = dr.GetInt32("IdCitizenship")
    _citizenship = dr.GetString("Citizenship")
    _IdBusinessType = dr.GetInt32("IdBusinessType")
    _BusinessTypeCode = dr.GetString("BusinessTypeCode")
    _BusinessTypeDescription = dr.GetString("BusinessTypeDescription")
  End Sub

End Class