
<Serializable()> _
Public Class PrintCustomerVehiclePivotReportInfo
  Inherits ReadOnlyBase(Of PrintCustomerVehiclePivotReportInfo)

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
  Private _makeDate As Date
  Public ReadOnly Property MakeDate() As Date
    Get
      Return _makeDate
    End Get
  End Property
  Private _numberOfWheels As Integer
  Public ReadOnly Property NumberOfWheels() As Integer
    Get
      Return _numberOfWheels
    End Get
  End Property
  Private _numberOfPropulsionWheels As Integer
  Public ReadOnly Property NumberOfPropulsionWheels() As Integer
    Get
      Return _numberOfPropulsionWheels
    End Get
  End Property
  Private _vehicleId As Long
  Public ReadOnly Property VehicleId() As Long
    Get
      Return _vehicleId
    End Get
  End Property
  Private _idEnginePowerSource As Integer
  Public ReadOnly Property IdEnginePowerSource() As Integer
    Get
      Return _idEnginePowerSource
    End Get
  End Property
  Private _idEngineSecondPowerSource As Integer
  Public ReadOnly Property IdEngineSecondPowerSource() As Integer
    Get
      Return _idEngineSecondPowerSource
    End Get
  End Property
  Private _numberOfAxis As Integer
  Public ReadOnly Property NumberOfAxis() As Integer
    Get
      Return _numberOfAxis
    End Get
  End Property
  Private _propulsionAxis As Integer
  Public ReadOnly Property PropulsionAxis() As Integer
    Get
      Return _propulsionAxis
    End Get
  End Property
  Private _numberOfDoors As Integer
  Public ReadOnly Property NumberOfDoors() As Integer
    Get
      Return _numberOfDoors
    End Get
  End Property
  Private _numberOfSeats As Short
  Public ReadOnly Property NumberOfSeats() As Short
    Get
      Return _numberOfSeats
    End Get
  End Property
  Private _numberOfStandingSeats As Short
  Public ReadOnly Property NumberOfStandingSeats() As Short
    Get
      Return _numberOfStandingSeats
    End Get
  End Property
  Private _numberOfLieingSeats As Short
  Public ReadOnly Property NumberOfLieingSeats() As Short
    Get
      Return _numberOfLieingSeats
    End Get
  End Property
  Private _maximunAllowedWaight As Single
  Public ReadOnly Property MaximunAllowedWaight() As Single
    Get
      Return _maximunAllowedWaight
    End Get
  End Property
  Private _emptyWaight As Single
  Public ReadOnly Property EmptyWaight() As Single
    Get
      Return _emptyWaight
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
  Public ReadOnly Property Customer() As String
    Get
      Return _customerFirstName & " " & _customerSurname
    End Get
  End Property
  Private _mb As String
  Public ReadOnly Property MB() As String
    Get
      Return _mb
    End Get
  End Property
  Private _streetName As String
  Public ReadOnly Property StreetName() As String
    Get
      Return _streetName
    End Get
  End Property
  Public ReadOnly Property LivingaAddress() As String
    Get
      Return _streetName & " " & _livingAddressNumber
    End Get
  End Property
  Private _livingAddressNumber As String
  Public ReadOnly Property LivingAddressNumber() As String
    Get
      Return _livingAddressNumber
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
  Private _customerId As Long
  Public ReadOnly Property CustomerId() As Long
    Get
      Return _customerId
    End Get
  End Property
  Private _bodytypeCode As String
  Public ReadOnly Property BodytypeCode() As String
    Get
      Return _bodytypeCode
    End Get
  End Property
  Private _bodytypeDescriprion As String
  Public ReadOnly Property BodytypeDescriprion() As String
    Get
      Return _bodytypeDescriprion
    End Get
  End Property
  Public ReadOnly Property BodytypeCodeDescription() As String
    Get
      Return _bodytypeCode & "-" & _bodytypeDescriprion
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
  Private _companyTrademark As String
  Public ReadOnly Property CompanyTrademark() As String
    Get
      Return _companyTrademark
    End Get
  End Property
  Public ReadOnly Property CompanyNameTrademark() As String
    Get
      Return _communityName & "-" & _companyTrademark
    End Get
  End Property
  Private _code As String
  Public ReadOnly Property Code() As String
    Get
      Return _code
    End Get
  End Property
  Private _name As String
  Public ReadOnly Property Name() As String
    Get
      Return _name
    End Get
  End Property
  Public ReadOnly Property CategoryForPaymentsCodeName() As String
    Get
      Return _code & "-" & _name
    End Get
  End Property
  Private _categoryCode As String
  Public ReadOnly Property CategoryCode() As String
    Get
      Return _categoryCode
    End Get
  End Property
  Public ReadOnly Property CategoryCodeName() As String
    Get
      Return _categoryCode & "-" & _categoryName
    End Get
  End Property
  Private _categoryName As String
  Public ReadOnly Property CategoryName() As String
    Get
      Return _categoryName
    End Get
  End Property
  Private _engineTypeCode As String
  Public ReadOnly Property EngineTypeCode() As String
    Get
      Return _engineTypeCode
    End Get
  End Property
  Private _techincalDescription As String
  Public ReadOnly Property TechincalDescription() As String
    Get
      Return _techincalDescription
    End Get
  End Property
  Private _startDate As Date
  Public ReadOnly Property StartDate() As Date
    Get
      Return _startDate
    End Get
  End Property
  Private _endDate As Date
  Public ReadOnly Property EndDate() As Date
    Get
      Return _endDate
    End Get
  End Property
  Private _lastRegistrationNumber As String
  Public ReadOnly Property LastRegistration() As String
    Get
      Return _lastRegistrationNumber
    End Get
  End Property
    'Private _lastRegistrationPlace As String
    'Public ReadOnly Property LastRegistrationPlace() As String
    '  Get
    '    Return _lastRegistrationPlace
    '  End Get
    'End Property
  Private _dateOfLastRegistration As Date
  Public ReadOnly Property DateOfLastRegistrationa() As Date
    Get
      Return _dateOfLastRegistration
    End Get
  End Property
  Private _lastRegistrationValidTill As Date
  Public ReadOnly Property LastRegistrationValidTill() As Date
    Get
      Return _lastRegistrationValidTill
    End Get
  End Property
  Private _IsCompany As Boolean
  Public ReadOnly Property IsCompany() As Boolean
    Get
      Return _IsCompany
    End Get
  End Property
  Private _PhoneNumber As String
  Public ReadOnly Property PhoneNumber() As String
    Get
      Return _PhoneNumber
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _enginenumber
  End Function

  Public Overrides Function ToString() As String
    Return _enginenumber
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _enginenumber = dr.GetString("EngineNumber")
    _shellnumber = dr.GetString("ShellNumber")
    _makedate = dr.GetDateTime("MakeDate")
    _numberofwheels = dr.GetInt32("NumberOfWheels")
    _numberofpropulsionwheels = dr.GetInt32("NumberOfPropulsionWheels")
    _vehicleid = dr.GetInt64("VehicleId")
    _idenginepowersource = dr.GetInt32("IdEnginePowerSource")
    _idenginesecondpowersource = dr.GetInt32("IdEngineSecondPowerSource")
    _numberofaxis = dr.GetInt32("NumberOfAxis")
    _propulsionaxis = dr.GetInt32("PropulsionAxis")
    _numberofdoors = dr.GetInt32("NumberOfDoors")
    _numberofseats = dr.GetInt16("NumberOfSeats")
    _numberofstandingseats = dr.GetInt16("NumberOfStandingSeats")
    _numberoflieingseats = dr.GetInt16("NumberOfLieingSeats")
    _maximunAllowedWaight = dr.GetValue("MaximunAllowedWaight")
    _emptyWaight = dr.GetValue("EmptyWaight")
    _customersurname = dr.GetString("CustomerSurname")
    _customerfirstname = dr.GetString("CustomerFirstName")
    _mb = dr.GetString("MB")
    _streetname = dr.GetString("StreetName")
    _livingaddressnumber = dr.GetString("LivingAddressNumber")
    _cityname = dr.GetString("CityName")
    _communityname = dr.GetString("CommunityName")
    _customerid = dr.GetInt64("CustomerId")
    _bodytypecode = dr.GetString("BodytypeCode")
    _bodytypedescriprion = dr.GetString("BodytypeDescriprion")
    _modelname = dr.GetString("ModelName")
    _companyname = dr.GetString("CompanyName")
    _companytrademark = dr.GetString("CompanyTrademark")
    _code = dr.GetString("Code")
    _name = dr.GetString("Name")
    _categorycode = dr.GetString("CategoryCode")
    _categoryname = dr.GetString("CategoryName")
    _enginetypecode = dr.GetString("EngineTypeCode")
    _techincaldescription = dr.GetString("TechincalDescription")
    _startdate = dr.GetDateTime("StartDate")
    _endDate = dr.GetDateTime("EndDate")
        'Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_vehicleId)
        _lastRegistrationNumber = dr.GetString("LastRegistratinNumber") 'regInfo.RegistrationNumber
        _dateOfLastRegistration = dr.GetDateTime("LastRegistrationMakeDate") 'regInfo.RegistrationDate
        ' _lastRegistrationPlace = dr.GetString("LastRegistrationCommunity") 'regInfo.RegistrationPlace
        _lastRegistrationValidTill = dr.GetDateTime("LastRegistrationValidTill") 'regInfo.DateRegistrationValidTill
    _IsCompany = dr.GetBoolean("IsCompany")
    _PhoneNumber = dr.GetString("PhoneNumber")
  End Sub

End Class