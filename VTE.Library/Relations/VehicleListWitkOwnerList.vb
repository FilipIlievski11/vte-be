
<Serializable()> _
Public Class VehicleListWitkOwnerList
  Inherits ReadOnlyListBase(Of VehicleListWitkOwnerList, VehicleListWitkOwnerInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehiclesListWithOwners"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleListWitkOwnerList() As VehicleListWitkOwnerList

    Return DataPortal.Fetch(Of VehicleListWitkOwnerList)()

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleListWitkOwnerInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleListWitkOwnerInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleListWitkOwnerInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleListWitkOwnerInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class