
<Serializable()> _
Public Class VehicleBrakesList
  Inherits ReadOnlyListBase(Of VehicleBrakesList, VehicleBrakesInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleBrakes"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleBrakesList() As VehicleBrakesList

    Return DataPortal.Fetch(Of VehicleBrakesList)()

  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler VehicleBrakes.VehicleBrakesSaved, AddressOf VehicleBrakes_saved
  End Sub

  Private Sub VehicleBrakes_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("VehicleBrakesInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleBrakesInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleBrakesInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleBrakesInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class