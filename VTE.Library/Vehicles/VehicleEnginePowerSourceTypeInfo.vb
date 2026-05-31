
<Serializable()> _
Public Class VehicleEnginePowerSourceTypeInfo
  Inherits ReadOnlyBase(Of VehicleEnginePowerSourceTypeInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _powerSourceName As String
  Public ReadOnly Property PowerSourceName() As String
    Get
      Return _powerSourceName
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
    _powersourcename = dr.GetString("PowerSourceName")
  End Sub

  Friend Sub New(ByVal intId As Integer, ByVal strPowerSource As String)
    _id = intId
    _powerSourceName = strPowerSource
  End Sub

End Class