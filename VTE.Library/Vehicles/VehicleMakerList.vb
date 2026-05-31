
<Serializable()> _
Public Class VehicleMakerList
  Inherits ReadOnlyListBase(Of VehicleMakerList, VehicleMakerInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleMakers"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleMakerList() As VehicleMakerList

    Return DataPortal.Fetch(Of VehicleMakerList)()

  End Function


  Public Function GetVehicleMakerListById(ByVal IdIn As Integer) As VehicleMakerInfo
    For Each child As VehicleMakerInfo In Me
      If child.Id = IdIn Then
        Return child
      End If
    Next
    Return Nothing
  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler VehicleMakers.VehicleMakersSaved, AddressOf VehicleMakers_saved
  End Sub

  Private Sub VehicleMakers_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
    IsReadOnly = False
    Me.Clear()
    IsReadOnly = True
    DataPortal_Fetch()
    Me.ResetBindings()
  End Sub
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleMakerInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleMakerInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleMakerInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleMakerInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class