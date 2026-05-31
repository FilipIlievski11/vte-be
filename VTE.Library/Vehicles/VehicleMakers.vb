
<Serializable()> _
Public Class VehicleMakers
  Inherits Csla.BusinessListBase(Of VehicleMakers, VehicleMaker)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleMakerByID"
  Private Const spGetAll As String = "GetVehicleMakers"
  Private Const spUpdate As String = "updateVehicleMaker"
  Private Const spAdd As String = "addVehicleMaker"
  Private Const spDelete As String = "deleteVehicleMaker"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleMaker = VehicleMaker.NewVehicleMakerChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleMakers")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleMakers")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleMakers")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleMakers")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetVehicleMakers() As VehicleMakers
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a VehicleMakers")
    End If
    Return DataPortal.Fetch(Of VehicleMakers)()
  End Function

  Public Overrides Function Save() As VehicleMakers
    Dim result As VehicleMakers = MyBase.Save()

    OnVehicleMakersSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleMakers.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleMaker.GetVehicleMaker(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleMakers.Child_Fetch", ex)
      Throw New DbCslaException("VehicleMakers.Child_Fetch", ex)
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
  Public Shared Event VehicleMakersSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnVehicleMakersSaved(ByVal sender As VehicleMakers, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent VehicleMakersSaved(sender, e)
  End Sub
#End Region

End Class
