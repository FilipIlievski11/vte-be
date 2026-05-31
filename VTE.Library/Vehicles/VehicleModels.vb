
<Serializable()> _
Public Class VehicleModels
  Inherits Csla.BusinessListBase(Of VehicleModels, VehicleModel)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleModeByID"
  Private Const spGetAll As String = "GetVehicleModel"
  Private Const spUpdate As String = "updateVehicleMode"
  Private Const spAdd As String = "addVehicleMode"
  Private Const spDelete As String = "deleteVehicleMode"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleModel = VehicleModel.NewVehicleModelChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleModels")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleModels")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleModels")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleModels")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetVehicleModels() As VehicleModels
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a VehicleModels")
    End If
    Return DataPortal.Fetch(Of VehicleModels)()
  End Function

  Public Overrides Function Save() As VehicleModels
    Dim result As VehicleModels = MyBase.Save

    OnVehicleModelsSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleModels.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleModel.GetVehicleModel(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleModels.Child_Fetch", ex)
      Throw New DbCslaException("VehicleModels.Child_Fetch", ex)
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
  Public Shared Event VehicleModelsSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnVehicleModelsSaved(ByVal sender As VehicleModels, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent VehicleModelsSaved(sender, e)
  End Sub
#End Region

End Class
