
<Serializable()> _
Public Class CustomersContactPersons
  Inherits Csla.BusinessListBase(Of CustomersContactPersons, CustomersContactPerson)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As CustomersContactPerson = CustomersContactPerson.NewCustomersContactPersonChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("CustomersContactPersons")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("CustomersContactPersons")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("CustomersContactPersons")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("CustomersContactPersons")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

    Friend Shared Function NewCustomersContactPersons() As CustomersContactPersons
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a CustomersContactPerson")
        'End If
        Return DataPortal.CreateChild(Of CustomersContactPersons)()
    End Function

  Friend Shared Function GetCustomersContactPersons(ByVal dr As SafeDataReader) As CustomersContactPersons
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a CustomersContactPerson")
        'End If
        Return DataPortal.FetchChild(Of CustomersContactPersons)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("CustomersContactPersons.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(CustomersContactPerson.GetCustomersContactPerson(dr))
      End While
    Catch ex As Exception
      Database.LogException("CustomersContactPersons.Child_Fetch", ex)
      Throw New DbCslaException("CustomersContactPersons.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
