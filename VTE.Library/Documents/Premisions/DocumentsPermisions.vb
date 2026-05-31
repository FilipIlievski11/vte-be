
<Serializable()> _
Public Class DocumentsPermisions
  Inherits Csla.BusinessListBase(Of DocumentsPermisions, DocumentsPermision)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentsPermisionByID"
  Private Const spGetAll As String = "GetDocumentsPermisions"
  Private Const spUpdate As String = "updateDocumentsPermision"
  Private Const spAdd As String = "addDocumentsPermision"
  Private Const spDelete As String = "deleteDocumentsPermision"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DocumentsPermision = DocumentsPermision.NewDocumentsPermisionChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsPermisions")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsPermisions")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsPermisions")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsPermisions")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetDocumentsPermisions() As DocumentsPermisions
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a DocumentsPermisions")
    End If
    Return DataPortal.Fetch(Of DocumentsPermisions)()
  End Function
  Public Function GetDocumentsPermisionsById(ByVal InId As Long) As DocumentsPermision
    For Each child As DocumentsPermision In Me
      If child.Id = InId Then
        Return child
      End If
    Next
    Return Nothing
  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("DocumentsPermisions.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(DocumentsPermision.GetDocumentsPermision(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DocumentsPermisions.Child_Fetch", ex)
      Throw New DbCslaException("DocumentsPermisions.Child_Fetch", ex)
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
