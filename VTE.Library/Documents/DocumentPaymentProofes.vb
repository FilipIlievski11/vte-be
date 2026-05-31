
<Serializable()> _
Public Class DocumentPaymentProofes
  Inherits Csla.BusinessListBase(Of DocumentPaymentProofes, DocumentPaymentProof)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentPaymentProoByID"
  Private Const spGetAll As String = "GetDocumentPaymentProof"
  Private Const spUpdate As String = "updateDocumentPaymentProo"
  Private Const spAdd As String = "addDocumentPaymentProo"
  Private Const spDelete As String = "deleteDocumentPaymentProo"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DocumentPaymentProof = DocumentPaymentProof.NewDocumentPaymentProofChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentPaymentProofes")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentPaymentProofes")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentPaymentProofes")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentPaymentProofes")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetDocumentPaymentProofes() As DocumentPaymentProofes
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a DocumentPaymentProofes")
    End If
    Return DataPortal.Fetch(Of DocumentPaymentProofes)()
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("DocumentPaymentProofes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(DocumentPaymentProof.GetDocumentPaymentProof(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DocumentPaymentProofes.Child_Fetch", ex)
      Throw New DbCslaException("DocumentPaymentProofes.Child_Fetch", ex)
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
