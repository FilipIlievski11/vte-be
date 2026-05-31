
<Serializable()> _
Public Class VehicleModelList
  Inherits ReadOnlyListBase(Of VehicleModelList, VehicleModelInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleModelsList"
#End Region

  Public Function GetInfo(ByVal idModel As Integer) As VehicleModelInfo
    For Each it As VehicleModelInfo In Me
      If it.Id = idModel Then
        Return it
      End If
    Next
    Return Nothing
  End Function

#Region " Factory Methods "

  Public Shared Function GetVehicleModelList() As VehicleModelList

    Return DataPortal.Fetch(Of VehicleModelList)()

  End Function

  Public Function GetVehicleModelInfoById(ByVal IdIn As Integer) As VehicleModelInfo
    For Each child As VehicleModelInfo In Me
      If child.Id = IdIn Then
        Return child
      End If
    Next
    Return Nothing
  End Function


  Private Sub New()
    ' require use of factory methods
    AddHandler VehicleModels.VehicleModelsSaved, AddressOf VehicleModels_saved
  End Sub

  Private Sub VehicleModels_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("VehicleModelInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleModelInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleModelInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleModelInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class