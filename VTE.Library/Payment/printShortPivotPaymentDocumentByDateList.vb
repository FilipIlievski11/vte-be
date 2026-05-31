
<Serializable()> _
Public Class printShortPivotPaymentDocumentByDateList
    Inherits ReadOnlyListBase(Of printShortPivotPaymentDocumentByDateList, printShortPivotPaymentDocumentByDateInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "printShortPivotPaymentDocumentByDate"
#End Region

#Region " Factory Methods "

    Public Shared Function GetprintShortPivotPaymentDocumentByDateList(ByVal StartDate As Date, ByVal EndDate As Date) As printShortPivotPaymentDocumentByDateList

        Return DataPortal.Fetch(Of printShortPivotPaymentDocumentByDateList)(New DateCritetria(StartDate, EndDate))

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
            Me.StartDate = StartDate
            Me.EndDate = EndDate
        End Sub
    End Class


    Private Overloads Sub DataPortal_Fetch(ByVal criteria As DateCritetria)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Using cn As SqlConnection = Database.VTE_SqlConnection
            Using cm As SqlCommand = cn.CreateCommand()
                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = SpZemiSite
                cm.Parameters.AddWithValue("@StartDate", Format(criteria.StartDate, "yyyy-MM-dd"))
    cm.Parameters.AddWithValue("@EndDate", Format(criteria.EndDate.AddDays(1), "yyyy-MM-dd"))
    cm.Parameters.AddWithValue("@IdOrganisation", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
                Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                    While dr.Read()
                        Me.Add(printShortPivotPaymentDocumentByDateInfo.GetprintShortPivotPaymentDocumentByDateInfo(dr))
                    End While
                End Using
            End Using
        End Using
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access
End Class