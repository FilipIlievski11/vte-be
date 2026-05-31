
<Serializable()> _
Public Class PaymentCategoryList
 Inherits ReadOnlyListBase(Of PaymentCategoryList, PaymentCategoryInfo)

#Region " Stored Procedures Names "
 Private Const SpZemiSite As String = "getPaymentCategoriesList"
#End Region

#Region " Factory Methods "

 Public Shared Function GetPaymentCategoryList() As PaymentCategoryList

  Return DataPortal.Fetch(Of PaymentCategoryList)(New Criteria)

 End Function

 Private Sub New()
  ' require use of factory methods
 End Sub

#End Region ' Factory Methods

#Region " Data Access "

 <Serializable()> _
 Private Class Criteria
  ' no criteria - retrieve all projects
 End Class

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Using cn As SqlConnection = Database.VTE_SqlConnection

   Using cm As SqlCommand = cn.CreateCommand()
    cm.CommandType = CommandType.StoredProcedure
    cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
    cm.CommandText = SpZemiSite
    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
     While dr.Read()
      Me.Add(PaymentCategoryInfo.GetPaymentCategoryInfo(dr))
     End While
    End Using
   End Using
  End Using
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub

#End Region ' Data Access
End Class