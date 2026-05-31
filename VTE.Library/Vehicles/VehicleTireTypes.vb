
<Serializable()> _
Public Class VehicleTireTypes
  Inherits Csla.BusinessListBase(Of VehicleTireTypes, VehicleTireType)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleTireTypeByID"
  Private Const spGetAll As String = "GetVehicleTireTypes"
  Private Const spUpdate As String = "updateVehicleTireType"
  Private Const spAdd As String = "addVehicleTireType"
  Private Const spDelete As String = "deleteVehicleTireType"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleTireType = VehicleTireType.NewVehicleTireTypeChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleTireTypes")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleTireTypes")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleTireTypes")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleTireTypes")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetVehicleTireTypes() As VehicleTireTypes
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a VehicleTireTypes")
    End If
    Return DataPortal.Fetch(Of VehicleTireTypes)()
  End Function

  Public Overrides Function Save() As VehicleTireTypes
    Dim result As VehicleTireTypes = MyBase.Save

    OnVehicleTireTypesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleTireTypes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleTireType.GetVehicleTireType(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleTireTypes.Child_Fetch", ex)
      Throw New DbCslaException("VehicleTireTypes.Child_Fetch", ex)
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
  Public Shared Event VehicleTireTypesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnVehicleTireTypesSaved(ByVal sender As VehicleTireTypes, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent VehicleTireTypesSaved(sender, e)
  End Sub
#End Region

End Class
