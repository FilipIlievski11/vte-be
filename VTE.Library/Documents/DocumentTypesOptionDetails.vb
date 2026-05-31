
<Serializable()> _
Public Class DocumentTypesOptionDetails
  Inherits Csla.BusinessListBase(Of DocumentTypesOptionDetails, DocumentTypesOptionDetail)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DocumentTypesOptionDetail = DocumentTypesOptionDetail.NewDocumentTypesOptionDetailChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentTypesOptionDetails")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentTypesOptionDetails")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentTypesOptionDetails")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentTypesOptionDetails")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

    Friend Shared Function NewDocumentTypesOptionDetails() As DocumentTypesOptionDetails
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User Not authorized to add a DocumentTypesOptionDetails")
        End If
        Return DataPortal.CreateChild(Of DocumentTypesOptionDetails)()
    End Function

  Friend Shared Function GetDocumentTypesOptionDetails(ByVal dr As SafeDataReader) As DocumentTypesOptionDetails
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User Not authorized to view a DocumentTypes")
        End If
        Return DataPortal.FetchChild(Of DocumentTypesOptionDetails)(dr)
  End Function

  Friend Function GetDocumentTypesOptionDetailById(ByVal inId As Integer) As DocumentTypesOptionDetail
    For Each child As DocumentTypesOptionDetail In Me
      If child.Id = inId Then
        Return child
      End If
    Next
    Return Nothing
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("DocumentTypesOptionDetails.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(DocumentTypesOptionDetail.GetDocumentTypesOptionDetail(dr))
      End While
    Catch ex As Exception
      Database.LogException("DocumentTypesOptionDetails.Child_Fetch", ex)
      Throw New DbCslaException("DocumentTypesOptionDetails.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
