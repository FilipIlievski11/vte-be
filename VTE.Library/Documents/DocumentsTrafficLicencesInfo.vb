
<Serializable()> _
Public Class DocumentsTrafficLicencesInfo
  Inherits ReadOnlyBase(Of DocumentsTrafficLicencesInfo)

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
  Private _idTehnicalExamOrganizationsIssuedBy As Integer
  Public ReadOnly Property IdTehnicalExamOrganizationsIssuedBy() As Integer
    Get
      Return _idTehnicalExamOrganizationsIssuedBy
    End Get
  End Property
  Private _trafficLicenceNumber As String
  Public ReadOnly Property TrafficLicenceNumber() As String
    Get
      Return _trafficLicenceNumber
    End Get
  End Property
  Private _madeDate As Date
  Public ReadOnly Property MadeDate() As Date
    Get
      Return _madeDate
    End Get
  End Property
  Private _endDate As Date
    Public Property EndDate() As Date
        Get
            Return _endDate
        End Get
        Set(ByVal value As Date)
            _endDate = value
        End Set
    End Property
  Private _note As String
  Public ReadOnly Property Note() As String
    Get
      Return _note
    End Get
  End Property
  Private _shellNumber As String
  Public ReadOnly Property ShellNumber() As String
    Get
      Return _shellNumber
    End Get
  End Property
  Private _engineNumber As String
  Public ReadOnly Property EngineNumber() As String
    Get
      Return _engineNumber
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
  Private _customerMB As String
  Public ReadOnly Property CustomerMB() As String
    Get
      Return _customerMB
    End Get
  End Property
  Private _idCustomer As Integer
  Public ReadOnly Property IdCustomer() As Integer
    Get
      Return _idCustomer
    End Get
  End Property
  Private _idVehicle As Integer
  Public ReadOnly Property IdVehicle() As Integer
    Get
      Return _idVehicle
    End Get
  End Property
  Private _vehicleModel As String
  Public ReadOnly Property VehicleModel() As String
    Get
      Return _vehicleModel
    End Get
  End Property
  Private _vehicleMaker As String
  Public ReadOnly Property VehicleMaker() As String
    Get
      Return _VehicleMaker
    End Get
  End Property
  Private _vehicleLastRegistration As String
  Public ReadOnly Property VehicleLastRegistration() As String
    Get
      Return _vehicleLastRegistration
    End Get
  End Property
  Private _IssuerName As String
  Public ReadOnly Property IssuerName() As String
    Get
      Return _IssuerName
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
    _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
    _idtehnicalexamorganizationsissuedby = dr.GetInt32("IdTehnicalExamOrganizationsIssuedBy")
    _trafficlicencenumber = dr.GetString("TrafficLicenceNumber")
    _madedate = dr.GetDateTime("MadeDate")
    _enddate = dr.GetDateTime("EndDate")
    _note = dr.GetString("Note")
    _shellnumber = dr.GetString("ShellNumber")
    _engineNumber = dr.GetString("EngineNumber")
    _customersurname = dr.GetString("CustomerSurname")
    _customerFirstName = dr.GetString("CustomerFirstName")
    _customerMB = dr.GetString("MB")
    _idCustomer = dr.GetInt64("IdCustomer")
    _idVehicle = dr.GetInt64("IdVehicle")
    _vehicleModel = dr.GetString("ModelName")
    _vehicleMaker = dr.GetString("VehicleMaker")
    _IssuerName = dr.GetString("IssuerName")
        'Dim lastRegInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_idVehicle)
        _vehicleLastRegistration = dr.GetString("LastRegistratinNumber") 'lastRegInfo.RegistrationNumber
  End Sub

End Class