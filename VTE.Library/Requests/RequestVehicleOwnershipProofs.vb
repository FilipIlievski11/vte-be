
<Serializable()> _
Public Class RequestVehicleOwnershipProofs
  Inherits Csla.BusinessListBase(Of RequestVehicleOwnershipProofs, RequestVehicleOwnershipProof)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As RequestVehicleOwnershipProof = RequestVehicleOwnershipProof.NewRequestVehicleOwnershipProofChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("RequestVehicleOwnershipProofs")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("RequestVehicleOwnershipProofs")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("RequestVehicleOwnershipProofs")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("RequestVehicleOwnershipProofs")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

  Friend Shared Function NewRequestVehicleOwnershipProofs() As RequestVehicleOwnershipProofs
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User Not authorized to add a RequestVehicleOwnershipProofs")
        'End If
        Return DataPortal.CreateChild(Of RequestVehicleOwnershipProofs)()
  End Function

  Friend Shared Function GetRequestVehicleOwnershipProofs(ByVal dr As SafeDataReader) As RequestVehicleOwnershipProofs
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User Not authorized to view a RequestVehicleOwnershipProofs")
        'End If
        Return DataPortal.FetchChild(Of RequestVehicleOwnershipProofs)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("RequestVehicleOwnershipProofs.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(RequestVehicleOwnershipProof.GetRequestVehicleOwnershipProof(dr))
      End While
    Catch ex As Exception
      Database.LogException("RequestVehicleOwnershipProofs.Child_Fetch", ex)
      Throw New DbCslaException("RequestVehicleOwnershipProofs.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
