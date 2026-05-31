
<Serializable()> _
Public Class PrintCustomerByIdInfo
  Inherits ReadOnlyBase(Of PrintCustomerByIdInfo)

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
  Public ReadOnly Property CustomerName() As String
    Get
      Return _customerFirstName & " " & _customerSurname
    End Get
  End Property
  Private _idLivingAddress As Integer
  Public ReadOnly Property IdLivingAddress() As Integer
    Get
      Return _idLivingAddress
    End Get
  End Property
  Private _livingAddressNumber As String
  Public ReadOnly Property LivingAddressNumber() As String
    Get
      Return _livingAddressNumber
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
  Private _citizenship As String
  Public ReadOnly Property Citizenship() As String
    Get
      Return _citizenship
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
  Private _livingStreet As String
  Public ReadOnly Property LivingStreet() As String
    Get
      Return _livingStreet
    End Get
  End Property

  Public ReadOnly Property LivingAddress() As String
    Get
      If _livingStreet IsNot Nothing Then
        If _livingCity IsNot Nothing Then
          If _livingCommunity IsNot Nothing Then
            Return _livingStreet & " " & _livingAddressNumber & "; " & _livingCity & ", " & _livingCommunity
          Else
            Return _livingStreet & " " & _livingAddressNumber & "; " & _livingCity
          End If
        Else
          Return _livingStreet & " " & _livingAddressNumber
        End If
      Else
        Return " "
      End If
    End Get
  End Property

  Private _idBusinessType As Integer
  Public ReadOnly Property IdBusinessType() As Integer
    Get
      Return _idBusinessType
    End Get
  End Property
  Private _businessTypeDescription As String
  Public ReadOnly Property BusinessTypeDescription() As String
    Get
      Return _businessTypeDescription
    End Get
  End Property
  Private _livingCity As String
  Public ReadOnly Property LivingCity() As String
    Get
      Return _livingCity
    End Get
  End Property
  Private _idCommunityCode As Integer
  Public ReadOnly Property IdCommunityCode() As Integer
    Get
      Return _idCommunityCode
    End Get
  End Property
  Private _livingCommunity As String
  Public ReadOnly Property LivingCommunity() As String
    Get
      Return _livingCommunity
    End Get
  End Property
  Private _livingCountry As String
  Public ReadOnly Property LivingCountry() As String
    Get
      Return _livingCountry
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
    _idlivingaddress = dr.GetInt32("IdLivingAddress")
    _livingaddressnumber = dr.GetString("LivingAddressNumber")
    _idlivingcity = dr.GetInt32("IdLivingCity")
    _idbirhcity = dr.GetInt32("IdBirhCity")
    _idbirthaddress = dr.GetInt32("IdBirthAddress")
    _brithaddressnumber = dr.GetString("BrithAddressNumber")
    _citizenship = dr.GetString("Citizenship")
    _iscompany = dr.GetBoolean("IsCompany")
    _occupation = dr.GetString("Occupation")
    _worksincompany = dr.GetString("WorksInCompany")
    _livingstreet = dr.GetString("LivingStreet")
    _idbusinesstype = dr.GetInt32("IdBusinessType")
    _businesstypedescription = dr.GetString("BusinessTypeDescription")
    _livingcity = dr.GetString("LivingCity")
    _idcommunitycode = dr.GetInt32("IdCommunityCode")
    _livingcommunity = dr.GetString("LivingCommunity")
    _livingcountry = dr.GetString("LivingCountry")
  End Sub

End Class