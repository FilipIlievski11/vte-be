
<Serializable()> _
Public Class ReportByCategoryForPaymentList
 Inherits ReadOnlyListBase(Of ReportByCategoryForPaymentList, ReportByCategoryForPaymentInfo)

#Region " Stored Procedures Names "
 Private Const SpZemiSite As String = "ReportByCategoryForPayment"
#End Region

#Region " Factory Methods "

 Public Shared Function GetReportByCategoryForPaymentList(ByVal PayCat As Integer, ByVal StartDate As Date, ByVal EndDate As Date) As ReportByCategoryForPaymentList

  Return DataPortal.Fetch(Of ReportByCategoryForPaymentList)(New FilterCriteria(PayCat, StartDate, EndDate))

 End Function

 Private Sub New()
  ' require use of factory methods
 End Sub

#End Region ' Factory Methods

#Region " Data Access "

 <Serializable()> _
 Private Class FilterCriteria
  Public PayCat As Integer
  Public StartDate As Date
  Public EndDate As Date

  Public Sub New(ByVal idPayCat As Integer, ByVal StartDate As Date, ByVal EndDate As Date)
   Me.PayCat = idPayCat
   Me.StartDate = StartDate.Date
   Me.EndDate = EndDate.Date
  End Sub
 End Class
 <Serializable()> _
 Private Class Criteria
  ' no criteria - retrieve all projects
 End Class

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As FilterCriteria)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Using cn As SqlConnection = Database.VTE_SqlConnection

   Using cm As SqlCommand = cn.CreateCommand()
    cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = SpZemiSite
                cm.CommandTimeout = 0
    cm.Parameters.AddWithValue("@idPaymentCategory", criteria.PayCat)
    cm.Parameters.AddWithValue("@StartDate", Format(criteria.StartDate, "yyyy-MM-dd"))
    cm.Parameters.AddWithValue("@EndDate", Format(criteria.EndDate.AddDays(1), "yyyy-MM-dd"))
    cm.Parameters.AddWithValue("@idCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
     While dr.Read()
      Me.Add(ReportByCategoryForPaymentInfo.GetReportByCategoryForPaymentInfo(dr))
     End While
    End Using
   End Using
  End Using
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub

#End Region ' Data Access
End Class