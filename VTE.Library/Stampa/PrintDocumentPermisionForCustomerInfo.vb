

<Serializable()> _
Public Class PrintDocumentPermisionForCustomerInfo
  Inherits ReadOnlyBase(Of PrintDocumentPermisionForCustomerInfo)

  Private _dateCreated As Date
  Public ReadOnly Property DateCreated() As Date
    Get
      Return _dateCreated
    End Get
  End Property
  Private _validTillDate As Date
  Public ReadOnly Property ValidTillDate() As Date
    Get
      Return _validTillDate
    End Get
  End Property
  Private _idVehicle As Long
  Public ReadOnly Property IdVehicle() As Long
    Get
      Return _idVehicle
    End Get
  End Property
  Private _customerSurname As String
  Public ReadOnly Property CustomerSurname() As String
    Get
      Return _customerSurname
    End Get
  End Property

  Public ReadOnly Property CustomerName() As String
    Get
      Return _customerFirstName & " " & _customerSurname
    End Get
  End Property
  Private _customerFirstName As String
  Public ReadOnly Property CustomerFirstName() As String
    Get
      Return _customerFirstName
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
  Private _passportNumber As String
  Public ReadOnly Property PassportNumber() As String
    Get
      Return _passportNumber
    End Get
  End Property
  Private _idLivingCity As Integer
  Public ReadOnly Property IdLivingCity() As Integer
    Get
      Return _idLivingCity
    End Get
  End Property
  Private _cityName As String
  Public ReadOnly Property CityName() As String
    Get
      Return _cityName
    End Get
  End Property
  Private _streetName As String
  Public ReadOnly Property StreetName() As String
    Get
      Return _streetName
    End Get
  End Property
  Private _idCustomerVehicleRelation As Long
  Public ReadOnly Property IdCustomerVehicleRelation() As Long
    Get
      Return _idCustomerVehicleRelation
    End Get
  End Property
  Private _idCustomerVehicleRelationOwner As Long
  Public ReadOnly Property IdCustomerVehicleRelationOwner() As Long
    Get
      Return _idCustomerVehicleRelationOwner
    End Get
  End Property

  Public ReadOnly Property FullLivingAddress() As String
    Get
      If _cityName <> String.Empty Then
        Return _streetName & " " & _livingAddressNumber & "; " & _cityName
      Else
        Return _streetName & " " & _livingAddressNumber
      End If
    End Get
  End Property
  Private _idCustomer As Long
  Public ReadOnly Property IdCustomer() As Long
    Get
      Return _idCustomer
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _datecreated
  End Function

  Public Overrides Function ToString() As String
    Return _datecreated
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _datecreated = dr.GetDateTime("DateCreated")
    _validtilldate = dr.GetDateTime("ValidTillDate")
    _idvehicle = dr.GetInt64("IdVehicle")
    _customersurname = dr.GetString("CustomerSurname")
    _customerfirstname = dr.GetString("CustomerFirstName")
    _idlivingaddress = dr.GetInt32("IdLivingAddress")
    _livingaddressnumber = dr.GetString("LivingAddressNumber")
    _passportnumber = dr.GetString("PassportNumber")
    _idlivingcity = dr.GetInt32("IdLivingCity")
    _cityname = dr.GetString("CityName")
    _streetname = dr.GetString("StreetName")
    _idcustomervehiclerelation = dr.GetInt64("IdCustomerVehicleRelation")
    _idcustomervehiclerelationowner = dr.GetInt64("IdCustomerVehicleRelationOwner")
    _idcustomer = dr.GetInt64("IdCustomer")
  End Sub

End Class