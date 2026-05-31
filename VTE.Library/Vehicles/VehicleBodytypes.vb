
<Serializable()> _
Public Class VehicleBodytypes
  Inherits Csla.BusinessListBase(Of VehicleBodytypes, VehicleBodytype)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleBodytypByID"
  Private Const spGetAll As String = "GetVehicleBodytype"
  Private Const spUpdate As String = "updateVehicleBodytyp"
  Private Const spAdd As String = "addVehicleBodytyp"
  Private Const spDelete As String = "deleteVehicleBodytyp"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleBodytype = VehicleBodytype.NewVehicleBodytypeChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleBodytypes")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleBodytypes")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleBodytypes")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleBodytypes")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetVehicleBodytypes() As VehicleBodytypes
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a VehicleBodytypes")
    End If
    Return DataPortal.Fetch(Of VehicleBodytypes)()
  End Function

  Public Overrides Function Save() As VehicleBodytypes
    Dim result As VehicleBodytypes = MyBase.Save()

    OnVehicleBodytypesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleBodytypes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleBodytype.GetVehicleBodytype(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleBodytypes.Child_Fetch", ex)
      Throw New DbCslaException("VehicleBodytypes.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


  Protected Overrides Sub DataPortal_Update()
    RaiseListChangedEvents = False
    Child_Update()
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access

#Region " Readonly list refresh "
  Public Shared Event VehicleBodytypesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnVehicleBodytypesSaved(ByVal sender As VehicleBodytypes, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent VehicleBodytypesSaved(sender, e)
  End Sub
#End Region

End Class
