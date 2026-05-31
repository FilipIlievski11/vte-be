
<Serializable()> _
Public Class VehicleTireList
    Inherits ReadOnlyListBase(Of VehicleTireList, VehicleTireInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "getVehicleTiresByIdVehicle"
#End Region

#Region " Factory Methods "


    Public Shared Function GetVehicleTireTypeListByIdVehicle(ByVal inId As Integer) As VehicleTireList

        Return DataPortal.Fetch(Of VehicleTireList)(New CriteriaByIdVehicle(inId))

    End Function

    Private Sub New()
        ' require use of factory methods
    End Sub

#End Region ' Factory Methods

#Region " Data Access "
    <Serializable()> _
  Private Class CriteriaByIdVehicle
        Private _idVehicle As Integer

        Public ReadOnly Property IdVehicle() As Integer
            Get
                Return _idVehicle
            End Get
        End Property

        Public Sub New(ByVal idVehicle As Integer)
            _idVehicle = idVehicle
        End Sub
    End Class

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByIdVehicle)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("VehicleTireInfo.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = SpZemiSite
                    cm.Parameters.AddWithValue("@id", criteria.IdVehicle)
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New VehicleTireInfo(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("VehicleTireInfo.DataPortal_Fetch", ex)
            Throw New DbCslaException("VehicleTireInfo.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
#End Region ' Data Access

End Class