
<Serializable()> _
Public Class VehicleUses
  Inherits Csla.BusinessListBase(Of VehicleUses, VehicleUse)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleUsByID"
  Private Const spGetAll As String = "GetVehicleUse"
  Private Const spUpdate As String = "updateVehicleUs"
  Private Const spAdd As String = "addVehicleUs"
  Private Const spDelete As String = "deleteVehicleUs"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleUse = VehicleUse.NewVehicleUseChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleUses")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleUses")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleUses")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleUses")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetVehicleUses() As VehicleUses
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a VehicleUses")
    End If
    Return DataPortal.Fetch(Of VehicleUses)()
  End Function


  Public Overrides Function Save() As VehicleUses
    Dim result As VehicleUses = MyBase.Save

    OnVehicleUsesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleUses.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleUse.GetVehicleUse(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleUses.Child_Fetch", ex)
      Throw New DbCslaException("VehicleUses.Child_Fetch", ex)
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
  Public Shared Event VehicleUsesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnVehicleUsesSaved(ByVal sender As VehicleUses, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent VehicleUsesSaved(sender, e)
  End Sub
#End Region
End Class
