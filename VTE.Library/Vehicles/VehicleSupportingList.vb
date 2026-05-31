
<Serializable()> _
Public Class VehicleSupportingList
  Inherits ReadOnlyListBase(Of VehicleSupportingList, VehicleSupportingInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleSupporting"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleSupportingList() As VehicleSupportingList

    Return DataPortal.Fetch(Of VehicleSupportingList)()

  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler VehicleSupportings.VehicleSupportingsSaved, AddressOf VehicleSupportings_saved
  End Sub

  Private Sub VehicleSupportings_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("VehicleSupportingInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleSupportingInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleSupportingInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleSupportingInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class