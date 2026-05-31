
<Serializable()> _
Public Class VehicleEngineTypes
  Inherits Csla.BusinessListBase(Of VehicleEngineTypes, VehicleEngineType)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleEngineTypeByID"
  Private Const spGetAll As String = "GetVehicleEngineTypes"
  Private Const spUpdate As String = "updateVehicleEngineType"
  Private Const spAdd As String = "addVehicleEngineType"
  Private Const spDelete As String = "deleteVehicleEngineType"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleEngineType = VehicleEngineType.NewVehicleEngineTypeChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleEngineTypes")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleEngineTypes")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleEngineTypes")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleEngineTypes")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetVehicleEngineTypes() As VehicleEngineTypes
        'If Not CanGetObject() Then
        '  Throw New System.Security.SecurityException("User Not authorized to view a VehicleEngineTypes")
        'End If
    Return DataPortal.Fetch(Of VehicleEngineTypes)()
  End Function


  Public Overrides Function Save() As VehicleEngineTypes
    Dim result As VehicleEngineTypes = MyBase.Save

    OnVehicleEngineTypesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleEngineTypes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleEngineType.GetVehicleEngineType(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleEngineTypes.Child_Fetch", ex)
      Throw New DbCslaException("VehicleEngineTypes.Child_Fetch", ex)
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
  Public Shared Event VehicleEngineTypesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnVehicleEngineTypesSaved(ByVal sender As VehicleEngineTypes, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent VehicleEngineTypesSaved(sender, e)
  End Sub
#End Region

End Class
