
<Serializable()> _
Public Class VehicleCategories
  Inherits Csla.BusinessListBase(Of VehicleCategories, VehicleCategorie)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleCategorieByID"
  Private Const spGetAll As String = "GetVehicleCategories"
  Private Const spUpdate As String = "updateVehicleCategorie"
  Private Const spAdd As String = "addVehicleCategorie"
  Private Const spDelete As String = "deleteVehicleCategorie"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleCategorie = VehicleCategorie.NewVehicleCategorieChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleCategories")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleCategories")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleCategories")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleCategories")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetVehicleCategories() As VehicleCategories
        'If Not CanGetObject() Then
        '  Throw New System.Security.SecurityException("User Not authorized to view a VehicleCategories")
        'End If
    Return DataPortal.Fetch(Of VehicleCategories)()
  End Function
  Public Function GetVehicleCategory(ByVal inId As Integer) As VehicleCategorie
        'If Not CanGetObject() Then
        '  Throw New System.Security.SecurityException("User Not authorized to view a VehicleCategories")
        'End If
    For Each child As VehicleCategorie In Me
      If child.Id = inId Then
        Return child
      End If
    Next
    Return Nothing
  End Function
  Public Overrides Function Save() As VehicleCategories
    Dim result As VehicleCategories = MyBase.Save

    OnVehicleCategoriesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleCategories.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleCategorie.GetVehicleCategorie(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleCategories.Child_Fetch", ex)
      Throw New DbCslaException("VehicleCategories.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


  Protected Overrides Sub DataPortal_Update()
    RaiseListChangedEvents = False
    Child_Update()
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access


#Region " Readonlylist refresh "
  Public Shared Event VehicleCategoriesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnVehicleCategoriesSaved(ByVal sender As VehicleCategories, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent VehicleCategoriesSaved(sender, e)
  End Sub
#End Region

End Class

