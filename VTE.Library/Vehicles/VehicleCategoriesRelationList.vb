
<Serializable()> _
Public Class VehicleCategoriesRelationList
  Inherits ReadOnlyListBase(Of VehicleCategoriesRelationList, VehicleCategoriesRelationInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleCategoriesRelations"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleCategoriesRelationList() As VehicleCategoriesRelationList

    Return DataPortal.Fetch(Of VehicleCategoriesRelationList)()

  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler VehicleCategoriesRelations.VehicleCategoriesRelationsSaved, AddressOf VehicleCategoriesRelations_saved
  End Sub

  Private Sub VehicleCategoriesRelations_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("VehicleCategoriesRelationInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleCategoriesRelationInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleCategoriesRelationInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleCategoriesRelationInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class