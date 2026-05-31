
<Serializable()> _
Public Class VehicleLastTehnicalExamInfo
  Inherits ReadOnlyBase(Of VehicleLastTehnicalExamInfo)

  Private _organizationName As String
  Public ReadOnly Property OrganizationName() As String
    Get
      Return _organizationName
    End Get
  End Property
  Private _station As String
  Public ReadOnly Property Station() As String
    Get
      Return _station
    End Get
  End Property
  Private _cityName As String
  Public ReadOnly Property CityName() As String
    Get
      Return _cityName
    End Get
  End Property
  Private _communityName As String
  Public ReadOnly Property CommunityName() As String
    Get
      Return _communityName
    End Get
  End Property
  Private _countryName As String
  Public ReadOnly Property CountryName() As String
    Get
      Return _countryName
    End Get
  End Property
  Private _idCustomerVehicleRelation As Integer
  Public ReadOnly Property IdCustomerVehicleRelation() As Integer
    Get
      Return _idCustomerVehicleRelation
    End Get
  End Property
  Private _madeDate As Date
  Public ReadOnly Property MadeDate() As Date
    Get
      Return _madeDate
    End Get
  End Property
  Private _validTillDate As Date
  Public ReadOnly Property ValidTillDate() As Date
    Get
      Return _validTillDate
    End Get
  End Property
  Private _idOrganizationForTehnicalExam As Integer
  Public ReadOnly Property IdOrganizationForTehnicalExam() As Integer
    Get
      Return _idOrganizationForTehnicalExam
    End Get
  End Property
  Private _idFirsControler As Integer
  Public ReadOnly Property IdFirsControler() As Integer
    Get
      Return _idFirsControler
    End Get
  End Property
  Private _idSecondControler As Integer
  Public ReadOnly Property IdSecondControler() As Integer
    Get
      Return _idSecondControler
    End Get
  End Property
  Private _vehicleIsRight As Boolean
  Public ReadOnly Property VehicleIsRight() As Boolean
    Get
      Return _vehicleIsRight
    End Get
  End Property
  Private _explanationNote As String
  Public ReadOnly Property ExplanationNote() As String
    Get
      Return _explanationNote
    End Get
  End Property
  Private _driversWarning As String
  Public ReadOnly Property DriversWarning() As String
    Get
      Return _driversWarning
    End Get
  End Property
  Private _idVehicle As Long
  Public ReadOnly Property IdVehicle() As Long
    Get
      Return _idVehicle
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _organizationname
  End Function

  Public Overrides Function ToString() As String
    Return _organizationname
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _organizationname = dr.GetString("OrganizationName")
    _station = dr.GetString("Station")
    _cityname = dr.GetString("CityName")
    _communityname = dr.GetString("CommunityName")
    _countryname = dr.GetString("CountryName")
    _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
    _madedate = dr.GetDateTime("MadeDate")
    _validtilldate = dr.GetDateTime("ValidTillDate")
    _idorganizationfortehnicalexam = dr.GetInt32("IdOrganizationForTehnicalExam")
    _idfirscontroler = dr.GetInt32("IdFirsControler")
    _idsecondcontroler = dr.GetInt32("IdSecondControler")
    _vehicleisright = dr.GetBoolean("VehicleIsRight")
    _explanationnote = dr.GetString("ExplanationNote")
    _driverswarning = dr.GetString("DriversWarning")
    _idvehicle = dr.GetInt64("IdVehicle")
  End Sub

End Class