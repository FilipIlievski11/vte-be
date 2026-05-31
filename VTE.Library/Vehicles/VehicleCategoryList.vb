
<Serializable()> _
Public Class VehicleCategoryList
  Inherits ReadOnlyListBase(Of VehicleCategoryList, VehicleCategoryInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleCategories"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleCategoryList() As VehicleCategoryList

    Return DataPortal.Fetch(Of VehicleCategoryList)()

  End Function

  Public Function GetVehicleCategoryInfo(ByVal inId As Integer) As VehicleCategoryInfo
    For Each child As VehicleCategoryInfo In Me
      If child.Id = inId Then
        Return child
        Exit Function
      End If
    Next
    Return Nothing
  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler VehicleCategories.VehicleCategoriesSaved, AddressOf VehicleCategories_saved
  End Sub

  Private Sub VehicleCategories_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("VehicleCategoryInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleCategoryInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleCategoryInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleCategoryInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class