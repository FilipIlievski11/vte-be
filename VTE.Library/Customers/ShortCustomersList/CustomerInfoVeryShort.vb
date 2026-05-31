

<Serializable()> _
Public Class CustomerInfoVeryShort
 Inherits ReadOnlyBase(Of CustomerInfoVeryShort)

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
 
 Private _livingAddressNumber As String
 Public ReadOnly Property LivingAddressNumber() As String
  Get
   Return _livingAddressNumber
  End Get
 End Property
 Private _isCompany As Boolean
 Public ReadOnly Property IsCompany() As Boolean
  Get
   Return _isCompany
  End Get
 End Property
 Private _cityName As String
 Public ReadOnly Property CityName() As String
  Get
   Return _cityName
  End Get
 End Property
 
 Private _opstina As String
 Public ReadOnly Property Opstina() As String
  Get
   Return _opstina
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
  _customerSurname = dr.GetString("Ime")

  _livingAddressNumber = dr.GetString("Ulica")

  _isCompany = dr.GetBoolean("IsCompany")

  _cityName = dr.GetString("CommunityName")

  _opstina = dr.GetString("CityName")

 End Sub


End Class
