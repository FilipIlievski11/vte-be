
<Serializable()> _
Public Class VehicleEngineTypeInfo
  Inherits ReadOnlyBase(Of VehicleEngineTypeInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _idVehicleMaker As Integer
  Public ReadOnly Property IdVehicleMaker() As Integer
    Get
      Return _idVehicleMaker
    End Get
  End Property
  Private _engineTypeCode As String
  Public ReadOnly Property EngineTypeCode() As String
    Get
      Return _engineTypeCode
    End Get
  End Property
  Private _techincalDescriptionEcoProgram As String
  Public ReadOnly Property TechincalDescriptionEcoProgram() As String
    Get
      Return _techincalDescriptionEcoProgram
    End Get
  End Property
  Private _idDefaultPowerSource As Integer
  Public ReadOnly Property IdDefaultPowerSource() As Integer
    Get
      Return _idDefaultPowerSource
    End Get
  End Property
  Private _defaultPower As Single
  Public ReadOnly Property DefaultPower() As Single
    Get
      Return _defaultPower
    End Get
  End Property
  Private _defaultPowerOutPut As Single
  Public ReadOnly Property DefaultPowerOutPut() As Single
    Get
      Return _defaultPowerOutPut
    End Get
  End Property
    Private _defaultTorque As String
    Public ReadOnly Property DefaultTorque() As String
        Get
            Return _defaultTorque
        End Get
    End Property
    Private _CompanyName As String
    Public ReadOnly Property CompanyName() As String
        Get
            Return _CompanyName
        End Get
    End Property
  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("Id")
    _idvehiclemaker = dr.GetInt32("IdVehicleMaker")
    _enginetypecode = dr.GetString("EngineTypeCode")
    _techincalDescriptionEcoProgram = dr.GetString("TechincalDescription")
    _iddefaultpowersource = dr.GetInt32("IdDefaultPowerSource")
    _defaultPower = dr.GetValue("DefaultPower")
    _defaultPowerOutPut = dr.GetValue("DefaultPowerOutPut")
        _defaultTorque = dr.GetString("DefaultTorque")
        _CompanyName = dr.GetString("CompanyName")
  End Sub

End Class