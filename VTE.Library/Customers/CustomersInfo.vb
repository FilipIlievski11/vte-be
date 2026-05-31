
<Serializable()> _
Public Class CustomersInfo
  Inherits ReadOnlyBase(Of CustomersInfo)

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
  Private _idLivingCity As Integer
  Public ReadOnly Property IdLivingCity() As Integer
    Get
      Return _idLivingCity
    End Get
  End Property
  Private _idBirhCity As Integer
  Public ReadOnly Property IdBirhCity() As Integer
    Get
      Return _idBirhCity
    End Get
  End Property
  Private _idBirthAddress As Integer
  Public ReadOnly Property IdBirthAddress() As Integer
    Get
      Return _idBirthAddress
    End Get
  End Property
  Private _brithAddressNumber As String
  Public ReadOnly Property BrithAddressNumber() As String
    Get
      Return _brithAddressNumber
    End Get
  End Property
  Private _Idcitizenship As Integer
  Public ReadOnly Property IdCitizenship() As Integer
    Get
      Return _Idcitizenship
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
      Return _occupation
    End Get
  End Property
  Private _worksInCompany As String
  Public ReadOnly Property WorksInCompany() As String
    Get
      Return _worksInCompany
    End Get
  End Property

  Private _idBusinessType As Integer
  Public ReadOnly Property IdBusinessType() As Integer
    Get
      Return _idBusinessType
    End Get
  End Property

  Private _birthDate As Date
  Public ReadOnly Property BirthDate() As String
    Get
      If _birthDate.Date = Date.MinValue Then
        Return ""
      Else
        Return _birthDate
      End If

    End Get
  End Property
  Private _cityName As String
  Public ReadOnly Property CityName() As String
    Get
      Return _cityName
    End Get
  End Property
  Private _countryName As String
  Public ReadOnly Property CountryName() As String
    Get
      Return _countryName
    End Get
  End Property
  Private _streetName As String
  Public ReadOnly Property StreetName() As String
    Get
      Return _streetName
    End Get
  End Property

  Private _communityName As String
  Public ReadOnly Property CommunityName() As String
    Get
      Return _communityName
    End Get
  End Property
  Public ReadOnly Property Name() As String
    Get
      Return _customerFirstName & " " & _customerSurname
    End Get
  End Property

  Public ReadOnly Property AddressOfLiving() As String
    Get
      If _streetName = String.Empty Then
                If _cityName <> String.Empty Then
                    Return _cityName
                Else
                    Return Nothing
                End If
            Else
                If _livingAddressNumber = String.Empty Then
                    If _cityName = String.Empty Then
                        Return _streetName
                    Else
                        Return _streetName & "; " & _cityName
                    End If
                    Return _streetName
                Else
                    Return _streetName & " бр." & _livingAddressNumber & "; " & _cityName
                End If
            End If
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
    _customerSurname = dr.GetString("CustomerSurname")
    _customerFirstName = dr.GetString("CustomerFirstName")
    _phoneNumber = dr.GetString("PhoneNumber")
    _fax = dr.GetString("Fax")
    _livingAddressNumber = dr.GetString("LivingAddressNumber")
    _idLivingAddress = dr.GetInt32("IdLivingAddress")
    _idLivingCity = dr.GetInt32("IdLivingCity")
    _idBirhCity = dr.GetInt32("IdBirhCity")
    _idBirthAddress = dr.GetInt32("IdBirthAddress")
    _brithAddressNumber = dr.GetString("BrithAddressNumber")
    _Idcitizenship = dr.GetInt32("IdCitizenship")
    _isCompany = dr.GetBoolean("IsCompany")
    _occupation = dr.GetString("Occupation")
    _worksInCompany = dr.GetString("WorksInCompany")
    _idBusinessType = dr.GetInt32("IdBusinessType")
    _birthDate = dr.GetDateTime("DateOfBirth")
    _cityName = dr.GetString("CityName")
    _countryName = dr.GetString("CountryName")
    _streetName = dr.GetString("StreetName")
    _communityName = dr.GetString("CommunityName")

  End Sub

End Class