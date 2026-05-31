

<Serializable()> _
Public Class Companies
 Inherits Csla.BusinessListBase(Of Companies, Company)

#Region " Stored Procedures Names "
 Private Const spGetByID As String = "getCompanieById"
 Private Const spGetAll As String = "getCompanies"
 Private Const spUpdate As String = "updateCompanie"
 Private Const spAdd As String = "addCompanie"
 Private Const spDelete As String = "deleteCompanie"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

 Protected Overrides Function AddNewCore() As Object
  Dim item As Company = Company.NewCompanyChild()
  Me.Add(item)
  Return item
 End Function

#End Region ' BindingList Overrides

 '#Region " Authorization Rules "

 ' Public Shared Function CanGetObject() As Boolean
 '  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Companies")
 ' End Function

 ' Public Shared Function CanAddObject() As Boolean
 '  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Companies")
 ' End Function

 ' Public Shared Function CanEditObject() As Boolean
 '  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Companies")
 ' End Function

 ' Public Shared Function CanDeleteObject() As Boolean
 '  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Companies")
 ' End Function

 '#End Region ' Authorization Rules

#Region " Factory Methods "
 Private Sub New()
  AllowNew = True
 End Sub

 Public Shared Function GetCompanies() As Companies
  'If Not CanGetObject() Then
  ' Throw New System.Security.SecurityException("User Not authorized to view a Companies")
  'End If
  Return DataPortal.Fetch(Of Companies)()
 End Function


 Public Overrides Function Save() As Companies
  Dim result As Companies = MyBase.Save()

  OnCompaniesSaved(Me, New Csla.Core.SavedEventArgs(result))

  Return result

 End Function
#End Region ' Factory Methods

#Region " Data Access "

 Private Overloads Sub DataPortal_Fetch()
  RaiseListChangedEvents = False
  Database.LogInfo("Companies.Child_Fetch", GetHashCode())
  Try

   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = spGetAll
     Using dr As New SafeDataReader(cm.ExecuteReader)
      While dr.Read()
       Me.Add(Company.GetCompany(dr))
      End While
     End Using
    End Using
   End Using

  Catch ex As Exception
   Database.LogException("Companies.Child_Fetch", ex)
   Throw New DbCslaException("Companies.Child_Fetch", ex)
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
 Public Shared Event CompaniesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
 Protected Shared Sub OnCompaniesSaved(ByVal sender As Companies, ByVal e As Csla.Core.SavedEventArgs)
  RaiseEvent CompaniesSaved(sender, e)
 End Sub
#End Region

End Class

