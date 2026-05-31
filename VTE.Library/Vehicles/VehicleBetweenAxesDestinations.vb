
<Serializable()> _
Public Class VehicleBetweenAxesDestinations
  Inherits Csla.BusinessListBase(Of VehicleBetweenAxesDestinations, VehicleBetweenAxesDestination)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleBetweenAxesDestination = VehicleBetweenAxesDestination.NewVehicleBetweenAxesDestinationChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleBetweenAxesDestinations")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleBetweenAxesDestinations")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleBetweenAxesDestinations")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleBetweenAxesDestinations")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

  Friend Shared Function NewVehicleBetweenAxesDestinations() As VehicleBetweenAxesDestinations
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a VehicleBetweenAxesDestinations")
        'End If
        Return DataPortal.CreateChild(Of VehicleBetweenAxesDestinations)()
  End Function

  Friend Shared Function GetVehicleBetweenAxesDestinations(ByVal dr As SafeDataReader) As VehicleBetweenAxesDestinations
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a VehicleBetweenAxesDestinations")
        'End If
        Return DataPortal.FetchChild(Of VehicleBetweenAxesDestinations)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleBetweenAxesDestinations.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(VehicleBetweenAxesDestination.GetVehicleBetweenAxesDestination(dr))
      End While
    Catch ex As Exception
      Database.LogException("VehicleBetweenAxesDestinations.Child_Fetch", ex)
      Throw New DbCslaException("VehicleBetweenAxesDestinations.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
