
<Serializable()> _
Public Class IncorectTehnicalExamsList
    Inherits ReadOnlyListBase(Of IncorectTehnicalExamsList, IncorectTehnicalExamsDepInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "depListOfIncorectTehnicalExams"
#End Region

#Region " Factory Methods "

    Public Shared Function GetIncorectTehnicalExamsList() As IncorectTehnicalExamsList

        Return DataPortal.Fetch(Of IncorectTehnicalExamsList)()

    End Function

    Private Sub New()
        ' require use of factory methods
        AddHandler DocumentsTehnicalExamsReport.DocumentsTehnicalExamsReportSaved, AddressOf DocumentsTehnicalExamsReport_saved
    End Sub

    Private Sub DocumentsTehnicalExamsReport_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
                cm.Parameters.AddWithValue("@idOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
    'cm.Parameters.AddWithValue("@date", Format(Today.Date, "yyyy-MM-dd"))
    'cm.Parameters.AddWithValue("@StartDate", Format(Today.Date.AddDays(-7), "yyyy-MM-dd"))

                Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                    While dr.Read()
                        Me.Add(IncorectTehnicalExamsDepInfo.GetIncorectTehnicalExamsDepInfo(dr))
                    End While
                End Using
            End Using
        End Using
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access
End Class