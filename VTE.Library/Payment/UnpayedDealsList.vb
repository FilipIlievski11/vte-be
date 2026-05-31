
<Serializable()> _
Public Class UnpayedDealsList
 Inherits ReadOnlyListBase(Of UnpayedDealsList, UnpayedDealsInfo)

#Region " Stored Procedures Names "
 Private Const SpZemiSite As String = "getUnpayedDeals"
#End Region

#Region " Factory Methods "

 Public Shared Function GetUnpayedDealsList(ByVal isPayed As Boolean) As UnpayedDealsList

  Return DataPortal.Fetch(Of UnpayedDealsList)(New filterCriteria(isPayed))

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
 <Serializable()> _
Private Class filterCriteria
  Private _payed As Boolean

  Public ReadOnly Property payed() As Boolean
   Get
    Return _payed
   End Get
  End Property

  Public Sub New(ByVal payed As Boolean)
   _payed = payed
  End Sub
 End Class
 Private Overloads Sub DataPortal_Fetch(ByVal criteria As filterCriteria)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Using cn As SqlConnection = Database.VTE_SqlConnection

   Using cm As SqlCommand = cn.CreateCommand()
    cm.CommandType = CommandType.StoredProcedure
    cm.CommandText = SpZemiSite
    cm.Parameters.AddWithValue("@payed", criteria.payed)
    cm.Parameters.AddWithValue("@idCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)

    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
     While dr.Read()
      Me.Add(UnpayedDealsInfo.GetUnpayedDealsInfo(dr))
     End While
    End Using
   End Using
  End Using
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub

#End Region ' Data Access
End Class