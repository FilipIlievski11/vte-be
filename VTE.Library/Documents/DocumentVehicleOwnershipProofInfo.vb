
<Serializable()> _
Public Class DocumentVehicleOwnershipProofInfo
  Inherits ReadOnlyBase(Of DocumentVehicleOwnershipProofInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _vehicleOwnershipProofName As String
  Public ReadOnly Property VehicleOwnershipProofName() As String
    Get
      Return _vehicleOwnershipProofName
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal intId As Integer, ByVal strVehicleOwnershipProofName As String)
    _id = intId
    _vehicleOwnershipProofName = strVehicleOwnershipProofName
  End Sub

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("Id")
    _vehicleownershipproofname = dr.GetString("VehicleOwnershipProofName")
  End Sub

End Class