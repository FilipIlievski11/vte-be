
<Serializable()> _
Public Class VehicleTireInfo
    Inherits ReadOnlyBase(Of VehicleTireInfo)

    Private _id As Integer
    Public ReadOnly Property Id() As Integer
        Get
            Return _id
        End Get
    End Property
    Private _idVehicle As Integer
    Public ReadOnly Property IdVehicle() As Integer
        Get
            Return _idVehicle
        End Get
    End Property
   
    Private _tireType As String
    Public ReadOnly Property TireType() As String
        Get
            Return _tireType
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
        _idVehicle = dr.GetInt32("IdVehicle")
        _tireType = dr.GetString("TireType")
        
    End Sub

End Class