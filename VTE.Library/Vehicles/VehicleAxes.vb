
<Serializable()> _
Public Class VehicleAxes
  Inherits Csla.BusinessListBase(Of VehicleAxes, VehicleAxis)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleAxis = VehicleAxis.NewVehicleAxisChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides


#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleAxes")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleAxes")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleAxes")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleAxes")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

  Friend Shared Function NewVehicleAxes() As VehicleAxes
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a VehicleAxes")
        'End If
        Return DataPortal.CreateChild(Of VehicleAxes)()
  End Function

  Friend Shared Function GetVehicleAxes(ByVal dr As SafeDataReader) As VehicleAxes
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a VehicleAxes")
        'End If
        Return DataPortal.FetchChild(Of VehicleAxes)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleAxes.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(VehicleAxis.GetVehicleAxis(dr))
      End While
    Catch ex As Exception
      Database.LogException("VehicleAxes.Child_Fetch", ex)
      Throw New DbCslaException("VehicleAxes.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
