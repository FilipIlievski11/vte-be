<Serializable()> _
Public Class TrafficLicencesExtensions
  Inherits Csla.BusinessListBase(Of TrafficLicencesExtensions, TrafficLicencesExtension)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As TrafficLicencesExtension = TrafficLicencesExtension.NewTrafficLicencesExtensionChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("TrafficLicencesExtensions")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("TrafficLicencesExtensions")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("TrafficLicencesExtensions")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("TrafficLicencesExtensions")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

  Friend Shared Function NewTrafficLicencesExtensions() As TrafficLicencesExtensions
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User Not authorized to add a TrafficLicencesExtensions")
        'End If
        Return DataPortal.CreateChild(Of TrafficLicencesExtensions)()
  End Function

  Friend Shared Function GetTrafficLicencesExtensions(ByVal dr As SafeDataReader) As TrafficLicencesExtensions
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User Not authorized to view a TrafficLicencesExtensions")
        'End If
        Return DataPortal.FetchChild(Of TrafficLicencesExtensions)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("TrafficLicencesExtensions.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(TrafficLicencesExtension.GetTrafficLicencesExtension(dr))
      End While
    Catch ex As Exception
      Database.LogException("TrafficLicencesExtensions.Child_Fetch", ex)
      Throw New DbCslaException("TrafficLicencesExtensions.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class