
<Serializable()> _
Public Class VehicleCategoriesRelations
  Inherits Csla.BusinessListBase(Of VehicleCategoriesRelations, VehicleCategoriesRelation)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleCategoriesRelation = VehicleCategoriesRelation.NewVehicleCategoriesRelationChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleCategoriesRelations")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleCategoriesRelations")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleCategoriesRelations")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleCategoriesRelations")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

  Friend Shared Function NewVehicleCategoriesRelations() As VehicleCategoriesRelations
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a VehicleCategoriesRelations")
        'End If
        Return DataPortal.CreateChild(Of VehicleCategoriesRelations)()
  End Function

  Friend Shared Function GetVehicleCategoriesRelations(ByVal dr As SafeDataReader) As VehicleCategoriesRelations
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a VehicleCategoriesRelations")
        'End If
        Return DataPortal.FetchChild(Of VehicleCategoriesRelations)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

  Public Overrides Function Save() As VehicleCategoriesRelations
    Dim result As VehicleCategoriesRelations = MyBase.Save

    OnVehicleCategoriesRelationsSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleCategoriesRelations.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(VehicleCategoriesRelation.GetVehicleCategoriesRelation(dr))
      End While
    Catch ex As Exception
      Database.LogException("VehicleCategoriesRelations.Child_Fetch", ex)
      Throw New DbCslaException("VehicleCategoriesRelations.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

#Region " Readonlylist refresh "

  Public Shared Event VehicleCategoriesRelationsSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnVehicleCategoriesRelationsSaved(ByVal sender As VehicleCategoriesRelations, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent VehicleCategoriesRelationsSaved(sender, e)
  End Sub
#End Region

End Class
