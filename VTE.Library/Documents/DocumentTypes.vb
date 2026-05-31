
<Serializable()> _
Public Class DocumentTypes
  Inherits Csla.BusinessListBase(Of DocumentTypes, DocumentType)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentTypeByID"
  Private Const spGetAll As String = "GetDocumentTypes"
  Private Const spUpdate As String = "updateDocumentType"
  Private Const spAdd As String = "addDocumentType"
  Private Const spDelete As String = "deleteDocumentType"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DocumentType = DocumentType.NewDocumentTypeChild()
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
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetDocumentTypes() As DocumentTypes
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a DocumentTypes")
    End If
    Return DataPortal.Fetch(Of DocumentTypes)()
  End Function
  Public Function GetDocumentTypesById(ByVal idIn As Integer) As DocumentType
    For Each child As DocumentType In Me
      If child.Id = idIn Then
        Return child
      End If
    Next
    Return Nothing
  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("DocumentTypes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(DocumentType.GetDocumentType(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DocumentTypes.Child_Fetch", ex)
      Throw New DbCslaException("DocumentTypes.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


  Protected Overrides Sub DataPortal_Update()
    RaiseListChangedEvents = False
    Child_Update()
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access


End Class
