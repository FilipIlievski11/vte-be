
<Serializable()> _
Public Class RequestPivotReportList
 Inherits ReadOnlyListBase(Of RequestPivotReportList, RequestPivotReportInfo)

#Region " Stored Procedures Names "
 Private Const SpZemiSite As String = "RequestPivotReport"
 Private Const SpZemiPoDatum As String = "RequestPivotReportByDate"
#End Region

#Region " Factory Methods "

 Public Shared Function GetRequestPivotReportList() As RequestPivotReportList

  Return DataPortal.Fetch(Of RequestPivotReportList)()

 End Function
 Public Shared Function GetRequestPivotReportListByDate(ByVal startDate As Date, ByVal endDate As Date) As RequestPivotReportList

  Return DataPortal.Fetch(Of RequestPivotReportList)(New DateCritetria(startDate, endDate))

 End Function
 Private Sub New()
  ' require use of factory methods
 End Sub

#End Region ' Factory Methods

#Region " Data Access "

 <Serializable()> _
 Private Class DateCritetria
  Public StartDate As Date
  Public EndDate As Date

  Public Sub New(ByVal StartDate As Date, ByVal EndDate As Date)
   Me.StartDate = StartDate.Date
   Me.EndDate = EndDate.Date
  End Sub
 End Class
 Private Overloads Sub DataPortal_Fetch()
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("RequestPivotReportInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
     cm.CommandText = SpZemiSite
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New RequestPivotReportInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("RequestPivotReportInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("RequestPivotReportInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub
 Private Overloads Sub DataPortal_Fetch(ByVal criteria As DateCritetria)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("RequestPivotReportInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = SpZemiPoDatum
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
     cm.Parameters.AddWithValue("@StartDate", Format(criteria.StartDate, "yyyy-MM-dd"))
     cm.Parameters.AddWithValue("@EndDate", Format(criteria.EndDate.AddDays(1), "yyyy-MM-dd"))
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New RequestPivotReportInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("RequestPivotReportInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("RequestPivotReportInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub
#End Region ' Data Access
End Class
