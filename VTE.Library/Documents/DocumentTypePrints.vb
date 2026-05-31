
<Serializable()> _
Public Class DocumentTypePrints
  Inherits Csla.BusinessListBase(Of DocumentTypePrints, DocumentTypePrint)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentTypePrinByID"
  Private Const spGetAll As String = "GetDocumentTypePrint"
  Private Const spUpdate As String = "updateDocumentTypePrin"
  Private Const spAdd As String = "addDocumentTypePrin"
  Private Const spDelete As String = "deleteDocumentTypePrin"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DocumentTypePrint = DocumentTypePrint.NewDocumentTypePrintChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentTypePrints")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentTypePrints")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentTypePrints")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentTypePrints")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetDocumentTypePrints() As DocumentTypePrints
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a DocumentTypePrints")
    End If
    Return DataPortal.Fetch(Of DocumentTypePrints)()
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("DocumentTypePrints.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(DocumentTypePrint.GetDocumentTypePrint(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DocumentTypePrints.Child_Fetch", ex)
      Throw New DbCslaException("DocumentTypePrints.Child_Fetch", ex)
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
