
<Serializable()> _
Public Class PaymentCategories
 Inherits Csla.BusinessListBase(Of PaymentCategories, PaymentCategorie)

#Region " Stored Procedures Names "
 Private Const spGetByID As String = "GetPaymentCategorieByID"
 Private Const spGetAll As String = "GetPaymentCategories"
 Private Const spUpdate As String = "updatePaymentCategorie"
 Private Const spAdd As String = "addPaymentCategorie"
 Private Const spDelete As String = "deletePaymentCategorie"
 Private Const spGetAllByIdComp As String = "getPaymentCategoriesByIdCompany"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

 Protected Overrides Function AddNewCore() As Object
  Dim item As PaymentCategorie = PaymentCategorie.NewPaymentCategorieChild()
  'tuka stavi gi site kategorii kako items
  Dim cat As VehicleCategoryForPaymentsList = VehicleCategoryForPaymentsList.GetVehicleCategoryForPaymentsList
  For Each catI As VehicleCategoryForPaymentsInfo In cat
   Dim newI As PaymentItem = item.PaymentsItems.AddNew()
   newI.IdVehicleCategoryForPayments = catI.Id
   newI.ItemName = "за " & catI.Name
  Next
  Me.Add(item)
  Return item
 End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

 Public Shared Function CanGetObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("PaymentCategories")
 End Function

 Public Shared Function CanAddObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("PaymentCategories")
 End Function

 Public Shared Function CanEditObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("PaymentCategories")
 End Function

 Public Shared Function CanDeleteObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("PaymentCategories")
 End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
 Private Sub New()
  AllowNew = True
 End Sub

 Public Shared Function GetPaymentCategoriesByIdCompany() As PaymentCategories
  If Not CanGetObject() Then
   Throw New System.Security.SecurityException("User Not authorized to view a PaymentCategories")
  End If
  Return DataPortal.Fetch(Of PaymentCategories)()
 End Function

 Public Overrides Function Save() As PaymentCategories
  Dim result As PaymentCategories = MyBase.Save

  OnPaymentCategoriesSaved(Me, New Csla.Core.SavedEventArgs(result))

  Return result
 End Function

#End Region ' Factory Methods

#Region " Data Access "

 Private Overloads Sub DataPortal_Fetch()
  RaiseListChangedEvents = False
  Database.LogInfo("PaymentCategories.Child_Fetch", GetHashCode())
  Try

   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     Dim tehOrg As TehnicalExamOrganizationsInfo = Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization")
     cm.Parameters.AddWithValue("@IdCompany", tehOrg.IdCompany)
     cm.CommandText = spGetAllByIdComp
     Using dr As New SafeDataReader(cm.ExecuteReader)
      While dr.Read()
       Me.Add(PaymentCategorie.GetPaymentCategorie(dr))
      End While
     End Using
    End Using
   End Using

  Catch ex As Exception
   Database.LogException("PaymentCategories.Child_Fetch", ex)
   Throw New DbCslaException("PaymentCategories.Child_Fetch", ex)
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
 Public Shared Event PaymentCategoriesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
 Protected Shared Sub OnPaymentCategoriesSaved(ByVal sender As PaymentCategories, ByVal e As Csla.Core.SavedEventArgs)
  RaiseEvent PaymentCategoriesSaved(sender, e)
 End Sub
#End Region

End Class
