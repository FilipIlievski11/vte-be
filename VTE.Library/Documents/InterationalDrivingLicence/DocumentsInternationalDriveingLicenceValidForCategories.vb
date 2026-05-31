
<Serializable()> _
Public Class DocumentsInternationalDriveingLicenceValidForCategories
  Inherits Csla.BusinessListBase(Of DocumentsInternationalDriveingLicenceValidForCategories, DocumentsInternationalDriveingLicenceValidForCategorie)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DocumentsInternationalDriveingLicenceValidForCategorie = DocumentsInternationalDriveingLicenceValidForCategorie.NewDocumentsInternationalDriveingLicenceValidForCategorieChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsInternationalDriveingLicenceValidForCategories")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsInternationalDriveingLicenceValidForCategories")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsInternationalDriveingLicenceValidForCategories")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsInternationalDriveingLicenceValidForCategories")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

  Friend Shared Function NewDocumentsInternationalDriveingLicenceValidForCategories() As DocumentsInternationalDriveingLicenceValidForCategories
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a DocumentsInternationalDriveingLicenceValidForCategorie")
        'End If
        Return DataPortal.CreateChild(Of DocumentsInternationalDriveingLicenceValidForCategories)()
  End Function

  Friend Shared Function GetDocumentsInternationalDriveingLicenceValidForCategories(ByVal dr As SafeDataReader) As DocumentsInternationalDriveingLicenceValidForCategories
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a DocumentsInternationalDriveingLicenceValidForCategorie")
        'End If
        Return DataPortal.FetchChild(Of DocumentsInternationalDriveingLicenceValidForCategories)(dr)
  End Function

    Public Overrides Function Save() As DocumentsInternationalDriveingLicenceValidForCategories
        Dim result As DocumentsInternationalDriveingLicenceValidForCategories = MyBase.Save
        Return result
    End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("DocumentsInternationalDriveingLicenceValidForCategories.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(DocumentsInternationalDriveingLicenceValidForCategorie.GetDocumentsInternationalDriveingLicenceValidForCategorie(dr))
      End While
    Catch ex As Exception
      Database.LogException("DocumentsInternationalDriveingLicenceValidForCategories.Child_Fetch", ex)
      Throw New DbCslaException("DocumentsInternationalDriveingLicenceValidForCategories.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access


End Class
