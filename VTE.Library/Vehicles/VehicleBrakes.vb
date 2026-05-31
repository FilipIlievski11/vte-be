
<Serializable()> _
Public Class VehicleBrakes
  Inherits Csla.BusinessListBase(Of VehicleBrakes, VehicleBrake)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleBrakeByID"
  Private Const spGetAll As String = "GetVehicleBrakes"
  Private Const spUpdate As String = "updateVehicleBrake"
  Private Const spAdd As String = "addVehicleBrake"
  Private Const spDelete As String = "deleteVehicleBrake"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleBrake = VehicleBrake.NewVehicleBrakeChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleBrakes")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleBrakes")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleBrakes")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleBrakes")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

    Public Shared Function GetVehicleBrakes() As VehicleBrakes
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a VehicleBrakes")
        End If
        Return DataPortal.Fetch(Of VehicleBrakes)()
    End Function

  Public Overrides Function Save() As VehicleBrakes
    Dim result As VehicleBrakes = MyBase.Save

    OnVehicleBrakesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleBrakes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleBrake.GetVehicleBrake(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleBrakes.Child_Fetch", ex)
      Throw New DbCslaException("VehicleBrakes.Child_Fetch", ex)
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
  Public Shared Event VehicleBrakesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnVehicleBrakesSaved(ByVal sender As VehicleBrakes, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent VehicleBrakesSaved(sender, e)
  End Sub
#End Region

End Class
