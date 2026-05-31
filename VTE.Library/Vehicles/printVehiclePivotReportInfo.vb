
<Serializable()> _
Public Class printVehiclePivotReportInfo
  Inherits ReadOnlyBase(Of printVehiclePivotReportInfo)

  Private _engineNumber As String
  Public ReadOnly Property EngineNumber() As String
    Get
      Return _engineNumber
    End Get
  End Property
 
  Private _colorCode As String
  Public ReadOnly Property ColorCode() As String
    Get
      Return _colorCode
    End Get
  End Property
  Private _enginePower As Single
  Public ReadOnly Property EnginePower() As Single
    Get
      Return _enginePower
    End Get
  End Property
    Private _engineTorque As String
    Public ReadOnly Property EngineTorque() As String
        Get
            Return _engineTorque
        End Get
    End Property
  Private _engineTorqueUnderGass As Single
  Public ReadOnly Property EngineTorqueUnderGass() As Single
    Get
      Return _engineTorqueUnderGass
    End Get
  End Property
  Private _engineWorkingCapacity As Single
  Public ReadOnly Property EngineWorkingCapacity() As Single
    Get
      Return _engineWorkingCapacity
    End Get
  End Property
  Private _enginePowerOutPut As Single
  Public ReadOnly Property EnginePowerOutPut() As Single
    Get
      Return _enginePowerOutPut
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
  Private _emptyWaight As Single
  Public ReadOnly Property EmptyWaight() As Single
    Get
      Return _emptyWaight
    End Get
  End Property
  Private _maximunAllowedWaight As Single
  Public ReadOnly Property MaximunAllowedWaight() As Single
    Get
      Return _maximunAllowedWaight
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
  Private _vehicleSizeHight As Single
  Public ReadOnly Property VehicleSizeHight() As Single
    Get
      Return _vehicleSizeHight
    End Get
  End Property
  Private _vehicleSizeWidth As Single
  Public ReadOnly Property VehicleSizeWidth() As Single
    Get
      Return _vehicleSizeWidth
    End Get
  End Property
  Private _vehicleSizeLength As Single
  Public ReadOnly Property VehicleSizeLength() As Single
    Get
      Return _vehicleSizeLength
    End Get
  End Property
  Private _bodytypeCode As String
  Public ReadOnly Property BodytypeCode() As String
    Get
      Return _bodytypeCode
    End Get
  End Property
  Public ReadOnly Property BodytypeCodeDescription() As String
    Get
      Return _bodytypeCode & "-" & _bodytypeDescriprion
    End Get
  End Property
  Private _bodytypeDescriprion As String
  Public ReadOnly Property BodytypeDescriprion() As String
    Get
      Return _bodytypeDescriprion
    End Get
  End Property
  Private _categoryCode As String
  Public ReadOnly Property CategoryCode() As String
    Get
      Return _categoryCode
    End Get
  End Property
  Private _categoryName As String
  Public ReadOnly Property CategoryName() As String
    Get
      Return _categoryName
    End Get
  End Property
  Public ReadOnly Property CategoryCodeName() As String
    Get
      Return _categoryCode & "-" & _categoryName
    End Get
  End Property
  Private _ecoProgram As String
  Public ReadOnly Property EcoProgram() As String
    Get
      Return _ecoProgram
    End Get
  End Property
  Private _techincalDescription As String
  Public ReadOnly Property TechincalDescription() As String
    Get
      Return _techincalDescription
    End Get
  End Property
  Private _engineTypeCode As String
  Public ReadOnly Property EngineTypeCode() As String
    Get
      Return _engineTypeCode
    End Get
  End Property
  Private _engineTechincalDescription As String
  Public ReadOnly Property EngineTechincalDescription() As String
    Get
      Return _engineTechincalDescription
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
  Public ReadOnly Property CompanyNameAndTrademark() As String
    Get
      Return _companyName & ":" & _companyTrademark
    End Get
  End Property
  Private _code As String
  Public ReadOnly Property CodeCategoryForPayments() As String
    Get
      Return _code
    End Get
  End Property
  Private _name As String
  Public ReadOnly Property NameCategoryForPayments() As String
    Get
      Return _name
    End Get
  End Property
  Public ReadOnly Property CategoryForPaymentsCodeName() As String
    Get
      Return _code & "-" & _name
    End Get
  End Property

    Private _PrimaryPowerSource As String
    Public ReadOnly Property PrimaryPowerSource() As String
        Get
            Return _PrimaryPowerSource
        End Get
    End Property
    Private _SecondaryPowerSource As String
    Public ReadOnly Property SecondaryPowerSource() As String
        Get
            Return _SecondaryPowerSource
        End Get
    End Property
    Private _PrimaryColorCode As String
    Private _PrimaryColorDescription As String
    Private _SecondaryColorCode As String
    Private _SecondaryColorDescription As String
    Public ReadOnly Property PrimaryColorCode() As String
        Get
            Return _PrimaryColorCode
        End Get
    End Property
    Public ReadOnly Property PrimaryColorDescription() As String
        Get
            Return _PrimaryColorDescription
        End Get
  End Property
  Public ReadOnly Property PrimaryColor() As String
    Get
      Return _PrimaryColorCode & "-" & _PrimaryColorDescription
    End Get
  End Property
    Public ReadOnly Property SecondaryColorCode() As String
        Get
            Return _SecondaryColorCode
        End Get
    End Property
    Public ReadOnly Property SecondaryColorDescription() As String
        Get
            Return _SecondaryColorDescription
        End Get
  End Property
  Public ReadOnly Property SecondaryColor() As String
    Get
      Return _SecondaryColorCode & "-" & _SecondaryColorDescription
    End Get
  End Property
  Private _lastRegistrationNumber As String
  Public ReadOnly Property LastRegistration() As String
    Get
      Return _lastRegistrationNumber
    End Get
  End Property
   
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
  Private _id As Long
  Public ReadOnly Property Id()
    Get
      Return _id
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _engineNumber
  End Function

  Public Overrides Function ToString() As String
    Return _engineNumber
    End Function
    Private _co As Double
    Public ReadOnly Property CO()
        Get
            Return _co
        End Get
    End Property
    Private _co2 As Double
    Public ReadOnly Property CO2()
        Get
            Return _co2
        End Get
    End Property
    Private _Blackening As String
    Public ReadOnly Property Blackening()
        Get
            Return _Blackening
        End Get
    End Property
    Private _Pinpoints As String
    Public ReadOnly Property Pinpoints()
        Get
            Return _Pinpoints
        End Get
    End Property
  Public ReadOnly Property YearOfProduction()
    Get
      Return _makeDate.Year
    End Get
  End Property
  Friend Sub New(ByVal dr As SafeDataReader)
    _engineNumber = dr.GetString("EngineNumber")
    _colorCode = dr.GetString("ColorCode")
    _enginePower = dr.GetValue("EnginePower")
        _engineTorque = dr.GetString("EngineTorque")
    _engineTorqueUnderGass = dr.GetValue("EngineTorqueUnderGass")
    _engineWorkingCapacity = dr.GetValue("EngineWorkingCapacity")
    _enginePowerOutPut = dr.GetValue("EnginePowerOutPut")
    _shellNumber = dr.GetString("ShellNumber")
    _makeDate = dr.GetDateTime("MakeDate")
    _numberOfDoors = dr.GetInt32("NumberOfDoors")
    _numberOfSeats = dr.GetInt16("NumberOfSeats")
    _numberOfStandingSeats = dr.GetInt16("NumberOfStandingSeats")
    _numberOfLieingSeats = dr.GetInt16("NumberOfLieingSeats")
    _emptyWaight = dr.GetValue("EmptyWaight")
    _maximunAllowedWaight = dr.GetValue("MaximunAllowedWaight")
    _numberOfAxis = dr.GetInt32("NumberOfAxis")
    _propulsionAxis = dr.GetInt32("PropulsionAxis")
    _numberOfWheels = dr.GetInt32("NumberOfWheels")
    _numberOfPropulsionWheels = dr.GetInt32("NumberOfPropulsionWheels")
    _vehicleSizeHight = dr.GetValue("VehicleSizeHight")
    _vehicleSizeWidth = dr.GetValue("VehicleSizeWidth")
    _vehicleSizeLength = dr.GetValue("VehicleSizeLength")
    _bodytypeCode = dr.GetString("BodytypeCode")
    _bodytypeDescriprion = dr.GetString("BodytypeDescriprion")
    _categoryCode = dr.GetString("CategoryCode")
    _categoryName = dr.GetString("CategoryName")
    _ecoProgram = dr.GetString("EcoProgram")
    _techincalDescription = dr.GetString("TechincalDescription")
    _engineTypeCode = dr.GetString("EngineTypeCode")
    _engineTechincalDescription = dr.GetString("EngineTechincalDescription")
       
    _modelName = dr.GetString("ModelName")
      
    _companyName = dr.GetString("CompanyName")
    _companyTrademark = dr.GetString("CompanyTrademark")
    _code = dr.GetString("Code")
    _name = dr.GetString("Name")
        _id = dr.GetInt64("Id")
        _lastRegistrationNumber = dr.GetString("LastRegistratinNumber")
        _dateOfLastRegistration = dr.GetDateTime("LastRegistrationMakeDate")
        _lastRegistrationValidTill = dr.GetDateTime("LastRegistrationValidTill")


        'MVR vo sp vo baza nese dodadeni za dr bazi osven mvr
        _co = dr.GetValue("CO")
        _co2 = dr.GetValue("CO2")
        _Blackening = dr.GetString("Blackening")
        _Pinpoints = dr.GetString("Pinpoints")
        _PrimaryPowerSource = dr.GetString("PrimaryPowerSource")
        _SecondaryPowerSource = dr.GetString("SecondaryPowerSource")
        _PrimaryColorCode = dr.GetString("PrimaryColorCode")
        _PrimaryColorDescription = dr.GetString("PrimaryColorDescription")
        _SecondaryColorCode = dr.GetString("SecondaryColorCode")
        _SecondaryColorDescription = dr.GetString("SecondaryColorDescription")
  End Sub

End Class