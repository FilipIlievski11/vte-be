
<Serializable()> _
Public Class DocumentAttachments
  Inherits Csla.BusinessListBase(Of DocumentAttachments, DocumentAttachment)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DocumentAttachment = DocumentAttachment.NewDocumentAttachmentChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentAttachments")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentAttachments")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentAttachments")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentAttachments")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

    Friend Shared Function NewDocumentAttachments() As DocumentAttachments
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a DocumentAttachment")
        'End If
        Return DataPortal.CreateChild(Of DocumentAttachments)()
    End Function

    Friend Shared Function GetDocumentAttachments(ByVal dr As SafeDataReader) As DocumentAttachments
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a DocumentAttachment")
        'End If
        Return DataPortal.FetchChild(Of DocumentAttachments)(dr)
    End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("DocumentAttachments.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(DocumentAttachment.GetDocumentAttachment(dr))
      End While
    Catch ex As Exception
      Database.LogException("DocumentAttachments.Child_Fetch", ex)
      Throw New DbCslaException("DocumentAttachments.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
