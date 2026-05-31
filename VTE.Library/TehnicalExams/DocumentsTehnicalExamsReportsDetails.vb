
<Serializable()> _
Public Class DocumentsTehnicalExamsReportsDetails
  Inherits Csla.BusinessListBase(Of DocumentsTehnicalExamsReportsDetails, DocumentsTehnicalExamsReportsDetail)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DocumentsTehnicalExamsReportsDetail = DocumentsTehnicalExamsReportsDetail.NewDocumentsTehnicalExamsReportsDetailChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsTehnicalExamsReportsDetails")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsTehnicalExamsReportsDetails")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsTehnicalExamsReportsDetails")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsTehnicalExamsReportsDetails")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

  Friend Shared Function NewDocumentsTehnicalExamsReportsDetails() As DocumentsTehnicalExamsReportsDetails
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User Not authorized to add a DocumentsTehnicalExamsReports")
        End If
        Return DataPortal.CreateChild(Of DocumentsTehnicalExamsReportsDetails)()
  End Function

  Friend Shared Function GetDocumentsTehnicalExamsReportsDetails(ByVal dr As SafeDataReader) As DocumentsTehnicalExamsReportsDetails
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User Not authorized to view a DocumentsTehnicalExamsReports")
        End If
        Return DataPortal.FetchChild(Of DocumentsTehnicalExamsReportsDetails)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("DocumentsTehnicalExamsReportsDetails.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(DocumentsTehnicalExamsReportsDetail.GetDocumentsTehnicalExamsReportsDetail(dr))
      End While
    Catch ex As Exception
      Database.LogException("DocumentsTehnicalExamsReportsDetails.Child_Fetch", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportsDetails.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
