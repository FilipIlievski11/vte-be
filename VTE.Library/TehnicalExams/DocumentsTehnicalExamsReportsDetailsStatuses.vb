
<Serializable()> _
Public Class DocumentsTehnicalExamsReportsDetailsStatuses
  Inherits Csla.BusinessListBase(Of DocumentsTehnicalExamsReportsDetailsStatuses, DocumentsTehnicalExamsReportsDetailsStatus)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentsTehnicalExamsReportsDetailsStatuByID"
  Private Const spGetAll As String = "GetDocumentsTehnicalExamsReportsDetailsStatus"
  Private Const spUpdate As String = "updateDocumentsTehnicalExamsReportsDetailsStatu"
  Private Const spAdd As String = "addDocumentsTehnicalExamsReportsDetailsStatu"
  Private Const spDelete As String = "deleteDocumentsTehnicalExamsReportsDetailsStatu"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DocumentsTehnicalExamsReportsDetailsStatus = DocumentsTehnicalExamsReportsDetailsStatus.NewDocumentsTehnicalExamsReportsDetailsStatusChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsTehnicalExamsReportsDetailsStatuses")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsTehnicalExamsReportsDetailsStatuses")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsTehnicalExamsReportsDetailsStatuses")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsTehnicalExamsReportsDetailsStatuses")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetDocumentsTehnicalExamsReportsDetailsStatuses() As DocumentsTehnicalExamsReportsDetailsStatuses
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a DocumentsTehnicalExamsReportsDetailsStatuses")
    End If
    Return DataPortal.Fetch(Of DocumentsTehnicalExamsReportsDetailsStatuses)()
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("DocumentsTehnicalExamsReportsDetailsStatuses.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(DocumentsTehnicalExamsReportsDetailsStatus.GetDocumentsTehnicalExamsReportsDetailsStatus(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DocumentsTehnicalExamsReportsDetailsStatuses.Child_Fetch", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportsDetailsStatuses.Child_Fetch", ex)
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
