
<Serializable()> _
Public Class printPermisionInfo
  Inherits ReadOnlyBase(Of printPermisionInfo)

#Region " Calculated Fields "
  Private _customerDisplay As String = String.Empty
  Private _ownerDisplay As String = String.Empty
  Private _lastRegistrationNumber As String = String.Empty

  Public ReadOnly Property CustomerDisplay() As String
    Get
      Return UCase(ToLat(_customerDisplay))
    End Get
  End Property

  Public ReadOnly Property OwnerDisplay() As String
    Get
      Return UCase(ToLat(_ownerDisplay))
    End Get
  End Property

  Public ReadOnly Property CustomerDisplayCyr() As String
    Get
      Return UCase(_customerDisplay) 'ToCyr(_customerDisplay))
    End Get
  End Property

  Public ReadOnly Property OwnerDisplayCyr() As String
    Get
      Return UCase(_ownerDisplay) 'ToCyr(_ownerDisplay))
    End Get
  End Property
  Public ReadOnly Property OwnerDiplayLine1() As String
    Get
      If _ownerBLK = String.Empty Or _ownerBLK = "" Then
        Return UCase(ToLat(_ownerDisplay))
      Else
        Return UCase(ToLat(_ownerDisplay & " бр.лк." & _ownerBLK))
      End If

    End Get
  End Property

  Public ReadOnly Property OwnerDiplayLine2() As String
    Get

      Return UCase(ToLat(_ownerStreet & " " & _ownerLivingAddressNumber & ", " & _ownerLivingCity)) '("ul." & _ownerStreet & " br." & _ownerLivingAddressNumber & ", " & _ownerLivingCity))

    End Get
  End Property

  Public ReadOnly Property VehicleDisplay() As String
    Get
      If _TNG Then
        Return UCase(ToLat(_companyName & " " & _modelName & _VehicleModelAdding & " TNG "))
      Else
        Return UCase(ToLat(_companyName & " " & _modelName & " " & _VehicleModelAdding))
      End If

    End Get
  End Property

  Public ReadOnly Property LastRegistrationNumber() As String
    Get
      Return _lastRegistrationNumber
    End Get
  End Property

  Public ReadOnly Property IssuedAtCityAndDate() As String
    Get
            Return UCase(ToLat(_CityOfIssuing & ", " & Format(Date.Now, "dd/MM/yyyy")))
    End Get
  End Property

#End Region

  Private _id As Long
  Public ReadOnly Property Id() As Long
    Get
      Return _id
    End Get
  End Property
  Private _idCustomerVehicleRelation As Long
  Public ReadOnly Property IdCustomerVehicleRelation() As Long
    Get
      Return _idCustomerVehicleRelation
    End Get
  End Property
  Private _customerMB As String
  Public ReadOnly Property CustomerMB() As String
    Get
      Return _customerMB
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
  Private _idCustomerVehicleRelationOwner As Long
  Public ReadOnly Property IdCustomerVehicleRelationOwner() As Long
    Get
      Return _idCustomerVehicleRelationOwner
    End Get
  End Property
  Private _idCustomer As Long
  Public ReadOnly Property IdCustomer() As Long
    Get
      Return _idCustomer
    End Get
  End Property
  Private _ownerMB As String
  Public ReadOnly Property OwnerMB() As String
    Get
      Return _ownerMB
    End Get
  End Property
  Private _ownerSurname As String
  Public ReadOnly Property OwnerSurname() As String
    Get
      Return _ownerSurname
    End Get
  End Property
  Private _ownerFirstName As String
  Public ReadOnly Property OwnerFirstName() As String
    Get
      Return _ownerFirstName
    End Get
  End Property
  Private _idVehicle As Long
  Public ReadOnly Property IdVehicle() As Long
    Get
      Return _idVehicle
    End Get
  End Property
  Private _engineNumber As String
  Public ReadOnly Property EngineNumber() As String
    Get
      Return _engineNumber
    End Get
  End Property
  Private _shellNumber As String
  Public ReadOnly Property ShellNumber() As String
    Get
      Return _shellNumber
    End Get
  End Property
  Private _idVehicleModel As Integer
  Public ReadOnly Property IdVehicleModel() As Integer
    Get
      Return _idVehicleModel
    End Get
  End Property
  Private _modelName As String
  Public ReadOnly Property ModelName() As String
    Get
      Return _modelName
    End Get
  End Property
  Private _companyName As String
  Public ReadOnly Property CompanyName() As String
    Get
      Return _companyName
    End Get
  End Property
  Private _trafficLicenceNumber As String
  Public ReadOnly Property TrafficLicenceNumber() As String
    Get
      Return UCase(ToLat(_trafficLicenceNumber))
    End Get
  End Property
  Private _triptiqueNumber As String
  Public ReadOnly Property TriptiqueNumber() As String
    Get
      Return UCase(ToLat(_triptiqueNumber))
    End Get
  End Property
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
  Private _note As String
  Public ReadOnly Property Note() As String
    Get
      Return _note
    End Get
  End Property
  Private _customerPasswordNumber As String
  Public ReadOnly Property CustomerPasswordNumber() As String
    Get
      Return _customerPasswordNumber
    End Get
  End Property
  Private _ownerLivingAddressNumber As String
  Public ReadOnly Property OwnerLivingAddressNumber() As String
    Get
      Return _ownerLivingAddressNumber
    End Get
  End Property
  Private _customerLivingAddressNumber As String
  Public ReadOnly Property CustomerLivingAddressNumber() As String
    Get
      Return _customerLivingAddressNumber
    End Get
  End Property
  Private _idOwnerLivingAdress As Integer
  Public ReadOnly Property IdOwnerLivingAdress() As Integer
    Get
      Return _idOwnerLivingAdress
    End Get
  End Property
  Private _ownerStreet As String
  Public ReadOnly Property OwnerStreet() As String
    Get
      Return _ownerStreet
    End Get
  End Property
  Private _customerStreet As String
  Public ReadOnly Property CustomerStreet() As String
    Get
      Return _customerStreet
    End Get
  End Property
  Private _idOwnerLivingCity As Integer
  Public ReadOnly Property IdOwnerLivingCity() As Integer
    Get
      Return _idOwnerLivingCity
    End Get
  End Property
  Private _ownerLivingCity As String
  Public ReadOnly Property OwnerLivingCity() As String
    Get
      Return _ownerLivingCity
    End Get
  End Property
  Private _customerLivingCity As String
  Public ReadOnly Property CustomerLivingCity() As String
    Get
      Return _customerLivingCity
    End Get
  End Property
  Private _ownerBLK As String
  Public ReadOnly Property OwnerBLK() As String
    Get
      Return _ownerBLK
    End Get
  End Property
  Private _idIssuer As Integer
  Public ReadOnly Property IdIssuer() As Integer
    Get
      Return _idIssuer
    End Get
  End Property
  Private _issuerName As String
  Public ReadOnly Property IssuerName() As String
    Get
      Return UCase(ToLat(_issuerName))
    End Get
  End Property
  Private _IdCityOfIssuing As Integer
  Public ReadOnly Property IdCityOfIssuing() As Integer
    Get
      Return _IdCityOfIssuing
    End Get
  End Property
  Private _CityOfIssuing As String
  Public ReadOnly Property CityOfIssuing() As String
    Get
      Return UCase(ToLat(_CityOfIssuing))
    End Get
  End Property
  Private _TNG As Boolean
  Public ReadOnly Property TNG() As Boolean
    Get
      Return _TNG
    End Get
  End Property
  Private _VehicleModelAdding As String
  Public ReadOnly Property VehicleModelAdding() As String
    Get
      Return _VehicleModelAdding
    End Get
  End Property

  Private _customerAddress As String
  Public Property CustomerAddress() As String
    Get
      Return _customerAddress
    End Get
    Set(ByVal value As String)
      _customerAddress = value
    End Set
  End Property

  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt64("Id")
    _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
    _customerMB = dr.GetString("CustomerMB")
    _customerSurname = dr.GetString("CustomerSurname")
    _customerFirstName = dr.GetString("CustomerFirstName")
    _idCustomerVehicleRelationOwner = dr.GetInt64("IdCustomerVehicleRelationOwner")
    _idCustomer = dr.GetInt64("IdCustomer")
    _ownerMB = dr.GetString("OwnerMB")
    _ownerSurname = dr.GetString("OwnerSurname")
    _ownerFirstName = dr.GetString("OwnerFirstName")
    _idVehicle = dr.GetInt64("IdVehicle")
    _engineNumber = dr.GetString("EngineNumber")
    _shellNumber = dr.GetString("ShellNumber")
    _idVehicleModel = dr.GetInt32("IdVehicleModel")
    _modelName = dr.GetString("ModelName")
    _customerLivingAddressNumber = dr.GetString("CustomerLivingAddressNumber")
    _customerLivingCity = dr.GetString("CustomerCityName")
    _customerStreet = dr.GetString("CustomerStreetName")
    _companyName = dr.GetString("CompanyName")
    _trafficLicenceNumber = dr.GetString("TrafficLicenceNumber")
    _triptiqueNumber = dr.GetString("TriptiqueNumber")
    _dateCreated = dr.GetDateTime("DateCreated")
    _validTillDate = dr.GetDateTime("ValidTillDate")
    _note = dr.GetString("Note")
    _customerPasswordNumber = dr.GetString("CustomerPasswordNumber")
    _ownerLivingAddressNumber = dr.GetString("OwnerLivingAddressNumber")
    _idOwnerLivingAdress = dr.GetInt32("IdOwnerLivingAdress")
    _ownerStreet = dr.GetString("OwnerStreet")
    _idOwnerLivingCity = dr.GetInt32("IdOwnerLivingCity")
    _ownerLivingCity = dr.GetString("OwnerLivingCity")
    _ownerBLK = dr.GetString("OwnerBLK")
    _idIssuer = dr.GetInt32("IdIssuer")
    _issuerName = dr.GetString("IssuerName")
    _IdCityOfIssuing = dr.GetInt32("IdCityOfIssuing")
    _CityOfIssuing = dr.GetString("CityOfIssuing")
    CustomerAddress = CustomerStreet & " " & CustomerLivingAddressNumber.ToString & " " & CustomerLivingCity
    If _customerSurname <> String.Empty Then
      _customerDisplay = _customerSurname & " " & _customerFirstName
    Else
      _customerDisplay = _customerFirstName
    End If
    If _ownerSurname <> String.Empty Then
      _ownerDisplay = _ownerSurname & " " & _ownerFirstName
    Else
      _ownerDisplay = _ownerFirstName
    End If
    'Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_idVehicle)
    _lastRegistrationNumber = dr.GetString("LastRegistratinNumber") 'regInfo.RegistrationNumber
    _TNG = dr.GetBoolean("TNG")
    _VehicleModelAdding = dr.GetString("VehicleModelAdding")
  End Sub

End Class