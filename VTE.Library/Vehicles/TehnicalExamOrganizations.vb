
<Serializable()> _
Public Class TehnicalExamOrganizations
 Inherits Csla.BusinessListBase(Of TehnicalExamOrganizations, TehnicalExamOrganization)

#Region " Stored Procedures Names "
 Private Const spGetByID As String = "GetTehnicalExamOrganizationByID"
 Private Const spGetAll As String = "GetTehnicalExamOrganizationsByIdCompany"
 Private Const spUpdate As String = "updateTehnicalExamOrganization"
 Private Const spAdd As String = "addTehnicalExamOrganization"
 Private Const spDelete As String = "deleteTehnicalExamOrganization"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

 Protected Overrides Function AddNewCore() As Object
  Dim item As TehnicalExamOrganization = TehnicalExamOrganization.NewTehnicalExamOrganizationChild()
  Me.Add(item)
  Return item
 End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

 Public Shared Function CanGetObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("TehnicalExamOrganizations")
 End Function

 Public Shared Function CanAddObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("TehnicalExamOrganizations")
 End Function

 Public Shared Function CanEditObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("TehnicalExamOrganizations")
 End Function

 Public Shared Function CanDeleteObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("TehnicalExamOrganizations")
 End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
 Private Sub New()
  AllowNew = True
 End Sub

 Public Shared Function GetTehnicalExamOrganizations() As TehnicalExamOrganizations
  If Not CanGetObject() Then
   Throw New System.Security.SecurityException("User Not authorized to view a TehnicalExamOrganizations")
  End If
  Return DataPortal.Fetch(Of TehnicalExamOrganizations)()
 End Function
 Public Overrides Function Save() As TehnicalExamOrganizations
  Dim result As TehnicalExamOrganizations = MyBase.Save()

  OnTehnicalExamOrganizationsSaved(Me, New Csla.Core.SavedEventArgs(result))

  Return result

 End Function
#End Region ' Factory Methods

#Region " Data Access "

 Private Overloads Sub DataPortal_Fetch()
  RaiseListChangedEvents = False
  Database.LogInfo("TehnicalExamOrganizations.Child_Fetch", GetHashCode())
  Try

   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
     cm.CommandText = spGetAll
     Using dr As New SafeDataReader(cm.ExecuteReader)
      While dr.Read()
       Me.Add(TehnicalExamOrganization.GetTehnicalExamOrganization(dr))
      End While
     End Using
    End Using
   End Using

  Catch ex As Exception
   Database.LogException("TehnicalExamOrganizations.Child_Fetch", ex)
   Throw New DbCslaException("TehnicalExamOrganizations.Child_Fetch", ex)
  End Try
  RaiseListChangedEvents = True
 End Sub


 Protected Overrides Sub DataPortal_Update()
  RaiseListChangedEvents = False
  Child_Update()
  RaiseListChangedEvents = True
 End Sub

#End Region ' Data Access

#Region " Readonly list refresh "
 Public Shared Event TehnicalExamOrganizationsSaved As EventHandler(Of Csla.Core.SavedEventArgs)
 Protected Shared Sub OnTehnicalExamOrganizationsSaved(ByVal sender As TehnicalExamOrganizations, ByVal e As Csla.Core.SavedEventArgs)
  RaiseEvent TehnicalExamOrganizationsSaved(sender, e)
 End Sub
#End Region

End Class
