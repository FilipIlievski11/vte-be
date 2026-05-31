
<Serializable()> _
Public Class TechnicalExamPivotReportList
 Inherits ReadOnlyListBase(Of TechnicalExamPivotReportList, TechnicalExamPivotReportInfo)

#Region " Stored Procedures Names "
 Private Const SpZemiSite As String = "TechnicalExamPivotReport"
#End Region

#Region " Factory Methods "

 Public Shared Function GetTechnicalExamPivotReportList(ByVal inod As Date, ByVal indo As Date) As TechnicalExamPivotReportList

  Return DataPortal.Fetch(Of TechnicalExamPivotReportList)(New CriteriaOdDo(inod, indo))

 End Function

 Private Sub New()
  ' require use of factory methods
 End Sub

#End Region ' Factory Methods

#Region " Data Access "
 <Serializable()> _
Private Class CriteriaOdDo
  Private _od As Date
  Private _do As Date
  Public ReadOnly Property DatumOd() As Date
   Get
    Return _od.Date
   End Get
  End Property
  Public ReadOnly Property DatumDo() As Date
   Get
    Return _do.Date
   End Get
  End Property
  Public Sub New(ByVal datumOd As Date, ByVal datumDo As Date)
   _od = datumOd.Date
   _do = datumDo.Date
  End Sub

 End Class
 <Serializable()> _
 Private Class Criteria
  ' no criteria - retrieve all projects
 End Class

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaOdDo)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Using cn As SqlConnection = Database.VTE_SqlConnection
   Using cm As SqlCommand = cn.CreateCommand()
    cm.CommandType = CommandType.StoredProcedure
    cm.CommandText = SpZemiSite
    cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
    cm.Parameters.AddWithValue("@od", Format(criteria.DatumOd, "yyyy-MM-dd"))
    cm.Parameters.AddWithValue("@do", Format(criteria.DatumDo.AddDays(1), "yyyy-MM-dd"))
    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
     While dr.Read()
      Me.Add(TechnicalExamPivotReportInfo.GetTechnicalExamPivotReportInfo(dr))
     End While
    End Using
   End Using
  End Using
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub

#End Region ' Data Access

End Class