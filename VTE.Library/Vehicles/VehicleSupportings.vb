
<Serializable()> _
Public Class VehicleSupportings
  Inherits Csla.BusinessListBase(Of VehicleSupportings, VehicleSupporting)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleSupportinByID"
  Private Const spGetAll As String = "GetVehicleSupporting"
  Private Const spUpdate As String = "updateVehicleSupportin"
  Private Const spAdd As String = "addVehicleSupportin"
  Private Const spDelete As String = "deleteVehicleSupportin"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleSupporting = VehicleSupporting.NewVehicleSupportingChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleSupportings")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleSupportings")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleSupportings")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleSupportings")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetVehicleSupportings() As VehicleSupportings
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a VehicleSupportings")
    End If
    Return DataPortal.Fetch(Of VehicleSupportings)()
  End Function

  Public Overrides Function Save() As VehicleSupportings
    Dim result As VehicleSupportings = MyBase.Save

    OnVehicleSupportingsSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleSupportings.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleSupporting.GetVehicleSupporting(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleSupportings.Child_Fetch", ex)
      Throw New DbCslaException("VehicleSupportings.Child_Fetch", ex)
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
  Public Shared Event VehicleSupportingsSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnVehicleSupportingsSaved(ByVal sender As VehicleSupportings, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent VehicleSupportingsSaved(sender, e)
  End Sub
#End Region

End Class
