
<Serializable()> _
Public Class VehicleInfoShort
    Inherits ReadOnlyBase(Of VehicleInfoShort)

    Private _id As Long
    Public ReadOnly Property Id() As Long
        Get
            Return _id
        End Get
    End Property
#Region "Polinja sto ne trebaat"

    'Private _idVehicleBodyType As Integer
    'Public ReadOnly Property IdVehicleBodyType() As Integer
    '    Get
    '        Return _idVehicleBodyType
    '    End Get
    'End Property
    'Private _idVehicleCategories As Integer
    'Public ReadOnly Property IdVehicleCategories() As Integer
    '    Get
    '        Return _idVehicleCategories
    '    End Get
    'End Property
    'Private _idVehicleUse As Integer
    'Public ReadOnly Property IdVehicleUse() As Integer
    '    Get
    '        Return _idVehicleUse
    '    End Get
    'End Property

    'Private _idEngineType As Integer
    'Public ReadOnly Property IdEngineType() As Integer
    '    Get
    '        Return _idEngineType
    '    End Get
    'End Property
    'Private _idEnginePowerSource As Integer
    'Public ReadOnly Property IdEnginePowerSource() As Integer
    '    Get
    '        Return _idEnginePowerSource
    '    End Get
    'End Property
    'Private _idEngineSecondPowerSource As Integer
    'Public ReadOnly Property IdEngineSecondPowerSource() As Integer
    '    Get
    '        Return _idEngineSecondPowerSource
    '    End Get
    'End Property

    'Private _idGearBox As Integer
    'Public ReadOnly Property IdGearBox() As Integer
    '    Get
    '        Return _idGearBox
    '    End Get
    'End Property
    'Private _idBreakes As Integer
    'Public ReadOnly Property IdBreakes() As Integer
    '    Get
    '        Return _idBreakes
    '    End Get
    'End Property
    'Private _idSupporting As Integer
    'Public ReadOnly Property IdSupporting() As Integer
    '    Get
    '        Return _idSupporting
    '    End Get
    'End Property
    'Private _idVehicleModel As Integer
    'Public ReadOnly Property IdVehicleModel() As Integer
    '    Get
    '        Return _idVehicleModel
    '    End Get
    'End Property
    'Private _idVehicleCategoryForPayments As Integer
    'Public ReadOnly Property IdVehicleCategoryForPayments() As Integer
    '    Get
    '        Return _idVehicleCategoryForPayments
    '    End Get
    'End Property

    'Private _engineNumber As String
    'Public ReadOnly Property EngineNumber() As String
    '    Get
    '        Return _engineNumber
    '    End Get
    'End Property
    'Private _enginePower As Single
    'Public ReadOnly Property EnginePower() As Single
    '    Get
    '        Return _enginePower
    '    End Get
    'End Property
    'Private _engineTorque As String
    'Public ReadOnly Property EngineTorque() As String
    '    Get
    '        Return _engineTorque
    '    End Get
    'End Property
    'Private _engineWorkingCapacity As Single
    'Public ReadOnly Property EngineWorkingCapacity() As Single
    '    Get
    '        Return _engineWorkingCapacity
    '    End Get
    'End Property
    'Private _enginePowerOutput As Single
    'Public ReadOnly Property EnginePowerOutput() As Single
    '    Get
    '        Return _enginePowerOutput
    '    End Get
    'End Property
   

    'Private _makeDate As Date
    'Public ReadOnly Property MakeDate() As Date
    '    Get
    '        Return _makeDate
    '    End Get
    'End Property
    'Private _numberOfDoors As Integer
    'Public ReadOnly Property NumberOfDoors() As Integer
    '    Get
    '        Return _numberOfDoors
    '    End Get
    'End Property
    'Private _numberOfSeats As Short
    'Public ReadOnly Property NumberOfSeats() As Short
    '    Get
    '        Return _numberOfSeats
    '    End Get
    'End Property
    'Private _numberOfStandingSeats As Short
    'Public ReadOnly Property NumberOfStandingSeats() As Short
    '    Get
    '        Return _numberOfStandingSeats
    '    End Get
    'End Property
    'Private _numberOfLieingSeats As Short
    'Public ReadOnly Property NumberOfLieingSeats() As Short
    '    Get
    '        Return _numberOfLieingSeats
    '    End Get
    'End Property
    'Private _emptyWaight As Single
    'Public ReadOnly Property EmptyWaight() As Single
    '    Get
    '        Return _emptyWaight
    '    End Get
    'End Property
    'Private _maximunAllowedWaight As Single
    'Public ReadOnly Property MaximunAllowedWaight() As Single 'odnesuva na carringCapasity
    '    Get
    '        Return _maximunAllowedWaight
    '    End Get
    'End Property

    'Public ReadOnly Property CarringCapacity() As Single
    '    Get
    '        Return _maximunAllowedWaight - _emptyWaight
    '    End Get
    'End Property
    'Public ReadOnly Property TotalWaight() As Single
    '    Get
    '        Return _maximunAllowedWaight + _emptyWaight
    '    End Get
    'End Property
    'Private _trailerWaightWithBreak As String
    'Public ReadOnly Property TrailerWaightWithBreak() As String
    '    Get
    '        Return _trailerWaightWithBreak
    '    End Get
    'End Property
    'Private _trailerWaightWithoutBreak As String
    'Public ReadOnly Property TrailerWaightWithoutBreak() As String
    '    Get
    '        Return _trailerWaightWithoutBreak
    '    End Get
    'End Property
    'Private _numberOfAxis As Integer
    'Public ReadOnly Property NumberOfAxis() As Integer
    '    Get
    '        Return _numberOfAxis
    '    End Get
    'End Property
    'Private _propulsionAxis As Integer
    'Public ReadOnly Property PropulsionAxis() As Integer
    '    Get
    '        Return _propulsionAxis
    '    End Get
    'End Property
    'Private _numberOfWheels As Integer
    'Public ReadOnly Property NumberOfWheels() As Integer
    '    Get
    '        Return _numberOfWheels
    '    End Get
    'End Property
    'Private _numberOfPropulsionWheels As Integer
    'Public ReadOnly Property NumberOfPropulsionWheels() As Integer
    '    Get
    '        Return _numberOfPropulsionWheels
    '    End Get
    'End Property
    'Private _vehicleSizeHight As Single
    'Public ReadOnly Property VehicleSizeHight() As Single
    '    Get
    '        Return _vehicleSizeHight
    '    End Get
    'End Property
    'Private _vehicleSizeWidth As Single
    'Public ReadOnly Property VehicleSizeWidth() As Single
    '    Get
    '        Return _vehicleSizeWidth
    '    End Get
    'End Property
    'Private _vehicleSizeLength As Single
    'Public ReadOnly Property VehicleSizeLength() As Single
    '    Get
    '        Return _vehicleSizeLength
    '    End Get
    'End Property
    'Private _suffocation As Boolean
    'Public ReadOnly Property Suffocation() As Boolean
    '    Get
    '        Return _suffocation
    '    End Get
    'End Property
    'Private _hook As Boolean
    'Public ReadOnly Property Hook() As Boolean
    '    Get
    '        Return _hook
    '    End Get
    'End Property
    'Private _vitlo As Boolean
    'Public ReadOnly Property Vitlo() As Boolean
    '    Get
    '        Return _vitlo
    '    End Get
    'End Property
    'Private _idPrimaryColor As Integer
    'Public ReadOnly Property IdPrimaryColor() As Integer
    '    Get
    '        Return _idPrimaryColor
    '    End Get
    'End Property
    'Private _idSecondaryColor As Integer
    'Public ReadOnly Property IdSecondaryColor() As Integer
    '    Get
    '        Return _idSecondaryColor
    '    End Get
    'End Property
    'Private _hologationSertificateNumber As String
    'Public ReadOnly Property HologationSertificateNumber() As String
    '    Get
    '        Return _hologationSertificateNumber
    '    End Get
    'End Property
    'Private _noiseStatic As Single
    'Public ReadOnly Property NoiseStatic() As Single
    '    Get
    '        Return _noiseStatic
    '    End Get
    'End Property
    'Private _noiseMovment As Single
    'Public ReadOnly Property NoiseMovment() As Single
    '    Get
    '        Return _noiseMovment
    '    End Get
    'End Property
    'Private _co As Single
    'Public ReadOnly Property CO() As Single
    '    Get
    '        Return _co
    '    End Get
    'End Property
    'Private _hc As Single
    'Public ReadOnly Property HC() As Single
    '    Get
    '        Return _hc
    '    End Get
    'End Property
    'Private _nOx As Single
    'Public ReadOnly Property NOx() As Single
    '    Get
    '        Return _nOx
    '    End Get
    'End Property
    'Private _hCNOx As Single
    'Public ReadOnly Property HCNOx() As Single
    '    Get
    '        Return _hCNOx
    '    End Get
    'End Property
    'Private _blackening As String
    'Public ReadOnly Property Blackening() As String
    '    Get
    '        Return _blackening
    '    End Get
    'End Property
    'Private _pinpoints As String
    'Public ReadOnly Property Pinpoints() As String
    '    Get
    '        Return _pinpoints
    '    End Get
    'End Property
    'Private _cO2 As Single
    'Public ReadOnly Property CO2() As Single
    '    Get
    '        Return _cO2
    '    End Get
    'End Property
    'Private _fuelConsumption As String
    'Public ReadOnly Property FuelConsumption() As String
    '    Get
    '        Return _fuelConsumption
    '    End Get
    'End Property
    'Private _capacityFuelTank As Single
    'Public ReadOnly Property CapacityFuelTank() As Single
    '    Get
    '        Return _capacityFuelTank
    '    End Get
    'End Property
    'Private _note As String
    'Public ReadOnly Property Note() As String
    '    Get
    '        Return _note
    '    End Get
    'End Property
    'Private _isSocialNotPrivate As Boolean
    'Public ReadOnly Property IsSocialNotPrivate() As Boolean
    '    Get
    '        Return _isSocialNotPrivate
    '    End Get
    'End Property
    'Private _forPrivateTransportNotPublic As Boolean
    'Public ReadOnly Property ForPrivateTransportNotPublic() As Boolean
    '    Get
    '        Return _forPrivateTransportNotPublic
    '    End Get
    'End Property

#End Region

    Private _shellNumber As String
    Public ReadOnly Property ShellNumber() As String
        Get
            Return _shellNumber
        End Get
    End Property
   

    ''Private _vehicleTrafficLicenceNumber As String
    ''Public ReadOnly Property VehicleTrafficLicenceNumber() As String
    ''  Get
    ''    Return _vehicleTrafficLicenceNumber
    ''  End Get
    ''End Property
    Private _modelName As String
    Public ReadOnly Property ModelName() As String
        Get
            Return _modelName
        End Get
    End Property
    Private _vehiceMaker As String
    Public ReadOnly Property VehiceMaker() As String
        Get
            Return _vehiceMaker
        End Get
    End Property
    Private _LastRegistratinNumber As String
    Public ReadOnly Property LastRegistratinNumber() As String
        Get
            Return _LastRegistratinNumber
        End Get
    End Property
    Public ReadOnly Property LastRegAndShellNumber() As String
        Get
            Return _LastRegistratinNumber & " " & _shellNumber
        End Get
    End Property
    'Protected Overrides Function GetIdValue() As Object
    '    Return _id
    'End Function

    'Public Overrides Function ToString() As String
    '    Return _id
    'End Function

  

    'Private _idMadeCountry As Integer
    'Public ReadOnly Property IdMadeCountry() As Integer
    '    Get
    '        Return _idMadeCountry
    '    End Get
    'End Property



    Friend Sub New(ByVal dr As SafeDataReader)
        _id = dr.GetInt64("Id")

        _vehiceMaker = dr.GetString("MakerName")
        _modelName = dr.GetString("ModelName")
        _shellNumber = dr.GetString("ShellNumber")
        _LastRegistratinNumber = dr.GetString("LastRegistratinNumber")
        '_idMadeCountry = dr.GetInt32("IdMadeCountry")

        
        ' _vehicleTrafficLicenceNumber = Vehicle.GetTrafficLicenceNumber(_id)
        '_idVehicleBodyType = dr.GetInt32("IdVehicleBodyType")
        '_idVehicleCategories = dr.GetInt32("IdVehicleCategories")
        '_idVehicleUse = dr.GetInt32("IdVehicleUse")
        '_idEngineType = dr.GetInt32("IdEngineType")
        '_idEnginePowerSource = dr.GetInt32("IdEnginePowerSource")
        '_idEngineSecondPowerSource = dr.GetInt32("IdEngineSecondPowerSource")

        '_idGearBox = dr.GetInt32("IdGearBox")
        '_idBreakes = dr.GetInt32("IdBreakes")
        '_idSupporting = dr.GetInt32("IdSupporting")
        '_idVehicleModel = dr.GetInt32("IdVehicleModel")
        '_engineNumber = dr.GetString("EngineNumber")
        '_enginePower = dr.GetValue("EnginePower")
        '_engineTorque = dr.GetString("EngineTorque")
        '_engineWorkingCapacity = dr.GetValue("EngineWorkingCapacity")
        '_enginePowerOutput = dr.GetValue("enginePowerOutput")

        '_makeDate = dr.GetDateTime("MakeDate")
        '_numberOfDoors = dr.GetInt32("NumberOfDoors")
        '_numberOfSeats = dr.GetInt16("NumberOfSeats")
        '_numberOfStandingSeats = dr.GetInt16("NumberOfStandingSeats")
        '_numberOfLieingSeats = dr.GetInt16("NumberOfLieingSeats")
        '_emptyWaight = dr.GetValue("EmptyWaight")
        '_maximunAllowedWaight = dr.GetValue("MaximunAllowedWaight")
        '_trailerWaightWithBreak = dr.GetString("TrailerWaightWithBreak")
        '_trailerWaightWithoutBreak = dr.GetString("TrailerWaightWithoutBreak")
        '_numberOfAxis = dr.GetInt32("NumberOfAxis")
        '_propulsionAxis = dr.GetInt32("PropulsionAxis")
        '_numberOfWheels = dr.GetInt32("NumberOfWheels")
        '_numberOfPropulsionWheels = dr.GetInt32("NumberOfPropulsionWheels")
        '_vehicleSizeHight = dr.GetValue("VehicleSizeHight")
        '_vehicleSizeWidth = dr.GetValue("VehicleSizeWidth")
        '_vehicleSizeLength = dr.GetValue("VehicleSizeLength")
        '_suffocation = dr.GetBoolean("Suffocation")
        '_hook = dr.GetBoolean("Hook")
        '_vitlo = dr.GetBoolean("Vitlo")
        '_idPrimaryColor = dr.GetInt32("IdPrimaryColor")
        '_idSecondaryColor = dr.GetInt32("IdSecondaryColor")
        '_idVehicleCategoryForPayments = dr.GetInt32("IdVehicleCategoryForPayments")
        '_hologationSertificateNumber = dr.GetString("HologationSertificateNumber")
        '_noiseStatic = dr.GetValue("NoiseStatic")
        '_noiseMovment = dr.GetValue("NoiseMovment")
        '_co = dr.GetValue("CO")
        '_hc = dr.GetValue("HC")
        '_nOx = dr.GetValue("NOx")
        '_hCNOx = dr.GetValue("HCNOx")
        '_blackening = dr.GetString("Blackening")
        '_pinpoints = dr.GetString("Pinpoints")
        '_cO2 = dr.GetValue("CO2")
        '_fuelConsumption = dr.GetString("FuelConsumption")
        '_capacityFuelTank = dr.GetValue("CapacityFuelTank")
        '_note = dr.GetString("Note")
        '_isSocialNotPrivate = dr.GetBoolean("IsSocialNotPrivate")
        '_forPrivateTransportNotPublic = dr.GetBoolean("ForPrivateTransportNotPublic")

    End Sub

End Class
