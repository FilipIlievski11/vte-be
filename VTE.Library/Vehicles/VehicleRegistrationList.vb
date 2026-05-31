
<Serializable()> _
Public Class VehicleRegistrationList
  Inherits ReadOnlyListBase(Of VehicleRegistrationList, VehicleRegistrationInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleRegistrations"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleRegistrationList() As VehicleRegistrationList

    Return DataPortal.Fetch(Of VehicleRegistrationList)()

  End Function

  Public Shared Function GetVehicleRegistrationList(ByVal idVehicle As Integer) As VehicleRegistrationList

    Return DataPortal.Fetch(Of VehicleRegistrationList)( _
    New SingleCriteria(Of VehicleRegistrationList, Integer)(idVehicle))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleRegistrationList, Integer))
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleRegistrationInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getVehicleRegistrationByIdVehicleList"
          cm.Parameters.AddWithValue("@id", criteria.Value)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleRegistrationInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleRegistrationInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleRegistrationInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True

  End Sub

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleRegistrationInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleRegistrationInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleRegistrationInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleRegistrationInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class