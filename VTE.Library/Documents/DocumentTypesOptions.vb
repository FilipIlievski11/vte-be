
<Serializable()> _
Public Class DocumentTypesOptions
  Inherits Csla.BusinessListBase(Of DocumentTypesOptions, DocumentTypesOption)

  Public Function GetDocumentTypesOptionById(ByVal id As Integer) As DocumentTypesOption
    For Each ch As DocumentTypesOption In Me
      If ch.Id = id Then
        Return ch
      End If
    Next
    Return Nothing
  End Function

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DocumentTypesOption = DocumentTypesOption.NewDocumentTypesOptionChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides


#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentTypes")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentTypes")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentTypes")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentTypes")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

    Friend Shared Function NewDocumentTypesOptions() As DocumentTypesOptions
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User Not authorized to add a DocumentTypesOptions")
        End If
        Return DataPortal.CreateChild(Of DocumentTypesOptions)()
    End Function

    Friend Shared Function GetDocumentTypesOptions(ByVal dr As SafeDataReader) As DocumentTypesOptions
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User Not authorized to view a DocumentTypesOptions")
        End If
        Return DataPortal.FetchChild(Of DocumentTypesOptions)(dr)
    End Function


  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("DocumentTypesOptions.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(DocumentTypesOption.GetDocumentTypesOption(dr))
      End While
    Catch ex As Exception
      Database.LogException("DocumentTypesOptions.Child_Fetch", ex)
      Throw New DbCslaException("DocumentTypesOptions.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
