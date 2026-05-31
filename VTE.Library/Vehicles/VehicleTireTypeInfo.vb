
<Serializable()> _
Public Class VehicleTireTypeInfo
  Inherits ReadOnlyBase(Of VehicleTireTypeInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _idVehicleModel As Integer
  Public ReadOnly Property IdVehicleModel() As Integer
    Get
      Return _idVehicleModel
    End Get
  End Property
  Private _seria As String
  Public ReadOnly Property Seria() As String
    Get
      Return _seria
    End Get
  End Property
  Private _tireType As String
  Public ReadOnly Property TireType() As String
    Get
      Return _tireType
    End Get
  End Property
  Public ReadOnly Property TireDescription() As String
    Get
      Return _seria & " (" & _dimenzions & "''" & ") " & ": " & _tireType & _note
    End Get
  End Property
  Private _dimenzions As Decimal
  Public ReadOnly Property Dimenzions() As Decimal
    Get
      Return _dimenzions
    End Get
  End Property
  Private _note As String
  Public ReadOnly Property Note() As String
    Get
      Return _note
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
    _idvehiclemodel = dr.GetInt32("IdVehicleModel")
    _seria = dr.GetString("Seria")
    _tiretype = dr.GetString("TireType")
    _dimenzions = dr.GetDecimal("Dimenzions")
    _note = dr.GetString("Note")
  End Sub

End Class