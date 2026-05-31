
<Serializable()> _
Public Class DDVCatalogs
  Inherits Csla.BusinessListBase(Of DDVCatalogs, DDVCatalog)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDDVCataloByID"
  Private Const spGetAll As String = "GetDDVCatalog"
  Private Const spUpdate As String = "updateDDVCatalo"
  Private Const spAdd As String = "addDDVCatalo"
  Private Const spDelete As String = "deleteDDVCatalo"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DDVCatalog = DDVCatalog.NewDDVCatalogChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DDVCatalogs")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DDVCatalogs")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DDVCatalogs")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DDVCatalogs")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetDDVCatalogs() As DDVCatalogs
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a DDVCatalogs")
    End If
    Return DataPortal.Fetch(Of DDVCatalogs)()
  End Function

  Public Overrides Function Save() As DDVCatalogs
    Dim result As DDVCatalogs = MyBase.Save

    OnDDVCatalogsSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("DDVCatalogs.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(DDVCatalog.GetDDVCatalog(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DDVCatalogs.Child_Fetch", ex)
      Throw New DbCslaException("DDVCatalogs.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


  Protected Overrides Sub DataPortal_Update()
    RaiseListChangedEvents = False
    Child_Update()
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access

#Region " Readonlylist refresh "
  Public Shared Event DDVCatalogsSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnDDVCatalogsSaved(ByVal sender As DDVCatalogs, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent DDVCatalogsSaved(sender, e)
  End Sub
#End Region
End Class
