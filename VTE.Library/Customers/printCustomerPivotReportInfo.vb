
<Serializable()> _
Public Class printCustomerPivotReportInfo
  Inherits ReadOnlyBase(Of printCustomerPivotReportInfo)

  Private _mb As String
  Public ReadOnly Property MB() As String
    Get
      Return _mb
    End Get
  End Property
  Private _customerSurname As String
  Public ReadOnly Property CustomerSurname() As String
    Get
      Return _customerSurname
    End Get
  End Property
  Private _customerFirstName As String
  Public ReadOnly Property CustomerFirstName() As String
    Get
      Return _customerFirstName
    End Get
  End Property
  Private _phoneNumber As String
  Public ReadOnly Property PhoneNumber() As String
    Get
      Return _phoneNumber
    End Get
  End Property
  Private _fax As String
  Public ReadOnly Property Fax() As String
    Get
      Return _fax
    End Get
  End Property
  Private _streetName As String
  Public ReadOnly Property StreetName() As String
    Get
      Return _streetName
    End Get
  End Property
  Private _cityName As String
  Public ReadOnly Property CityName() As String
    Get
      Return _cityName
    End Get
  End Property
  Private _communityCode As String
  Public ReadOnly Property CommunityCode() As String
    Get
      Return _communityCode
    End Get
  End Property
  Private _communityName As String
  Public ReadOnly Property CommunityName() As String
    Get
      Return _communityName
    End Get
  End Property
  Private _livingAddressNumber As String

  Public ReadOnly Property LivingAddress() As String
    Get
      Return _streetName & " " & _livingAddressNumber
    End Get
  End Property
  Public ReadOnly Property LivingAddressNumber() As String
    Get
      Return _livingAddressNumber
    End Get
  End Property
  Private _dateOfBirth As Date
  Public ReadOnly Property DateOfBirth() As Date
    Get
      Return _dateOfBirth
    End Get
  End Property

  Private _isCompany As Boolean
  Public ReadOnly Property IsCompany() As Boolean
    Get
      Return _isCompany
    End Get
  End Property
  Public ReadOnly Property IsCompanyString() As String
    Get
      If _isCompany Then
    Return My.Resources.pravnoLice ' "Правно лице"
      Else
    Return My.Resources.fizickoLice '"Физичко лице"
      End If
    End Get
  End Property
  Private _occupation As String
  Public ReadOnly Property Occupation() As String
    Get
      Return _occupation
    End Get
  End Property
  Private _worksInCompany As String
  Public ReadOnly Property WorksInCompany() As String
    Get
      Return _worksInCompany
    End Get
  End Property
  Private _businessTypeDescription As String
  Public ReadOnly Property BusinessTypeDescription() As String
    Get
      Return _businessTypeDescription
    End Get
  End Property
  Private _eMail As String
  Public ReadOnly Property EMail() As String
    Get
      Return _eMail
    End Get
  End Property
  Private _passportNumber As String
  Public ReadOnly Property PassportNumber() As String
    Get
      Return _passportNumber
    End Get
  End Property
  Private _countryName As String
  Public ReadOnly Property CountryName() As String
    Get
      Return _countryName
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _mb
  End Function

  Public Overrides Function ToString() As String
    Return _mb
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _mb = dr.GetString("MB")
    _customersurname = dr.GetString("CustomerSurname")
    _customerfirstname = dr.GetString("CustomerFirstName")
    _phonenumber = dr.GetString("PhoneNumber")
    _fax = dr.GetString("Fax")
    _streetname = dr.GetString("StreetName")
    _cityname = dr.GetString("CityName")
    _communitycode = dr.GetString("CommunityCode")
    _communityname = dr.GetString("CommunityName")
    _livingaddressnumber = dr.GetString("LivingAddressNumber")
    _dateofbirth = dr.GetDateTime("DateOfBirth")
    _iscompany = dr.GetBoolean("IsCompany")
    _occupation = dr.GetString("Occupation")
    _worksincompany = dr.GetString("WorksInCompany")
    _businesstypedescription = dr.GetString("BusinessTypeDescription")
    _email = dr.GetString("eMail")
    _passportnumber = dr.GetString("PassportNumber")
    _countryname = dr.GetString("CountryName")
  End Sub

End Class