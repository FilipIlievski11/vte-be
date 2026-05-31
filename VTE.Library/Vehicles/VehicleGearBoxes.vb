
<Serializable()> _
Public Class VehicleGearBoxes
  Inherits Csla.BusinessListBase(Of VehicleGearBoxes, VehicleGearBox)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleGearBoxByID"
  Private Const spGetAll As String = "getVehicleGearBox"
  Private Const spUpdate As String = "updateVehicleGearBox"
  Private Const spAdd As String = "addVehicleGearBox"
  Private Const spDelete As String = "deleteVehicleGearBox"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleGearBox = VehicleGearBox.NewVehicleGearBoxChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleGearBoxes")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleGearBoxes")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleGearBoxes")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleGearBoxes")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetVehicleGearBoxes() As VehicleGearBoxes
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to get a VehicleGearBoxes")
        End If
        Return DataPortal.Fetch(Of VehicleGearBoxes)()
  End Function


  Public Overrides Function Save() As VehicleGearBoxes
    Dim result As VehicleGearBoxes = MyBase.Save

    OnVehicleGearBoxesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleGearBoxes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleGearBox.GetVehicleGearBox(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleGearBoxes.Child_Fetch", ex)
      Throw New DbCslaException("VehicleGearBoxes.Child_Fetch", ex)
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
  Public Shared Event VehicleGearBoxesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnVehicleGearBoxesSaved(ByVal sender As VehicleGearBoxes, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent VehicleGearBoxesSaved(sender, e)
  End Sub
#End Region

End Class
