
<Serializable()> _
Public Class CustumerFinanceList
    Inherits ReadOnlyListBase(Of CustumerFinanceList, CustomerFinanceDepInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "depCustomerFinansicalState"
#End Region

#Region " Factory Methods "

    Public Shared Function GetCustumerFinanceList() As CustumerFinanceList

        Return DataPortal.Fetch(Of CustumerFinanceList)()

    End Function

    Private Sub New()
        ' require use of factory methods
        AddHandler Request.CustomerFinanceSaved, AddressOf CustomerFinance_saved
        AddHandler PaymentDocument.PaymentDocumentSaved, AddressOf CustomerFinance_saved
        AddHandler DocumentsTehnicalExamsReport.DocumentsTehnicalExamsReportSaved, AddressOf CustomerFinance_saved
    End Sub

    Private Sub CustomerFinance_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
        IsReadOnly = False
        Me.Clear()
        IsReadOnly = True
        DataPortal_Fetch()
        Me.ResetBindings()
    End Sub

#End Region ' Factory Methods

#Region " Data Access "

    <Serializable()> _
    Private Class Criteria
        ' no criteria - retrieve all projects
    End Class

    Private Overloads Sub DataPortal_Fetch()
        RaiseListChangedEvents = False
        IsReadOnly = False
        Using cn As SqlConnection = Database.VTE_SqlConnection

            Using cm As SqlCommand = cn.CreateCommand()
                cm.CommandType = CommandType.StoredProcedure
    cm.CommandText = SpZemiSite
    Dim objCurentStationId As Integer = CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id
    cm.Parameters.AddWithValue("@IdOrganization", objCurentStationId)
                Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                    While dr.Read()
                        Me.Add(CustomerFinanceDepInfo.GetCustomerFinanceDepInfo(dr))
                    End While
                End Using
            End Using
        End Using
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access
End Class