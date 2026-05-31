
<Serializable()> _
Public Class VehicleEngineEcoProgramList
  Inherits ReadOnlyListBase(Of VehicleEngineEcoProgramList, VehicleEngineEcoProgramInfo)
  Public Function GetInfoById(ByVal id As Integer) As VehicleEngineEcoProgramInfo
    For Each it As VehicleEngineEcoProgramInfo In Me
      If it.Id = id Then
        Return it
      End If
    Next
    Return Nothing
  End Function
#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleEngineEcoProgram"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleEngineEcoProgramList() As VehicleEngineEcoProgramList

    Return DataPortal.Fetch(Of VehicleEngineEcoProgramList)()

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleEngineEcoProgramInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleEngineEcoProgramInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleEngineEcoProgramInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleEngineEcoProgramInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class