
<Serializable()> _
Public Class VehicleGearBoxInfo
  Inherits ReadOnlyBase(Of VehicleGearBoxInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _gearBoxCode As Integer
  Public ReadOnly Property GearBoxCode() As Integer
    Get
      Return _gearBoxCode
    End Get
  End Property
  Private _gearBoxDescription As String
  Public ReadOnly Property GearBoxDescription() As String
    Get
      Return _gearBoxDescription
    End Get
  End Property
  Public ReadOnly Property GearBox() As String
    Get
      Return GearBoxCode & "-" & GearBoxDescription
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
    _gearboxcode = dr.GetInt32("GearBoxCode")
    _gearboxdescription = dr.GetString("GearBoxDescription")
  End Sub

End Class