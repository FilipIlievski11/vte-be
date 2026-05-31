
<Serializable()> _
Public Class VehicleEnginePowerSourceTypes
  Inherits Csla.BusinessListBase(Of VehicleEnginePowerSourceTypes, VehicleEnginePowerSourceType)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleEnginePowerSourceTypeByID"
  Private Const spGetAll As String = "GetVehicleEnginePowerSourceTypes"
  Private Const spUpdate As String = "updateVehicleEnginePowerSourceType"
  Private Const spAdd As String = "addVehicleEnginePowerSourceType"
  Private Const spDelete As String = "deleteVehicleEnginePowerSourceType"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleEnginePowerSourceType = VehicleEnginePowerSourceType.NewVehicleEnginePowerSourceTypeChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleEnginePowerSourceTypes")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleEnginePowerSourceTypes")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleEnginePowerSourceTypes")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleEnginePowerSourceTypes")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetVehicleEnginePowerSourceTypes() As VehicleEnginePowerSourceTypes
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a VehicleEnginePowerSourceTypes")
    End If
    Return DataPortal.Fetch(Of VehicleEnginePowerSourceTypes)()
  End Function

  Public Overrides Function Save() As VehicleEnginePowerSourceTypes
    Dim result As VehicleEnginePowerSourceTypes = MyBase.Save

    OnVehicleEnginePowerSourceTypesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
  End Function


#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleEnginePowerSourceTypes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleEnginePowerSourceType.GetVehicleEnginePowerSourceType(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleEnginePowerSourceTypes.Child_Fetch", ex)
      Throw New DbCslaException("VehicleEnginePowerSourceTypes.Child_Fetch", ex)
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
  Public Shared Event VehicleEnginePowerSourceTypesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnVehicleEnginePowerSourceTypesSaved(ByVal sender As VehicleEnginePowerSourceTypes, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent VehicleEnginePowerSourceTypesSaved(sender, e)
  End Sub
#End Region

End Class
