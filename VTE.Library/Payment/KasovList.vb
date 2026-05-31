

<Serializable()> _
Public Class KasovList
    Inherits ReadOnlyListBase(Of KasovList, KasovInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiZaSiteOperatori As String = "KasovIzvestaj"
    Private Const SpZemiZaEdenOperator As String = "KasovIzvestajPoOperator"
#End Region

#Region " Factory Methods "

    Public Shared Function GetKasovList(ByVal startDate As Date, ByVal endDate As Date, ByVal inIdOperator As Integer) As KasovList

        Return DataPortal.Fetch(Of KasovList)(New DateOperatorCritetria(startDate, endDate, inIdOperator))

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
    Private Class DateOperatorCritetria

        Public StartDate As Date
        Public EndDate As Date
        Public idOperator As Integer

        Public Sub New(ByVal StartDate As Date, ByVal EndDate As Date, ByVal inIdOperator As Integer)
            Me.StartDate = StartDate
            Me.EndDate = EndDate
            Me.idOperator = inIdOperator
        End Sub
    End Class
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As DateOperatorCritetria)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Using cn As SqlConnection = Database.VTE_SqlConnection

            Using cm As SqlCommand = cn.CreateCommand()
                cm.CommandType = CommandType.StoredProcedure
                If criteria.idOperator > 0 Then
                    cm.CommandText = SpZemiZaEdenOperator
                    cm.Parameters.AddWithValue("@IdOperator", criteria.idOperator)
                Else
                    cm.CommandText = SpZemiZaSiteOperatori
                End If
                cm.Parameters.AddWithValue("@StartDate", Format(criteria.StartDate, "yyyy-MM-dd"))
                cm.Parameters.AddWithValue("@EndDate", Format(criteria.EndDate.AddDays(1), "yyyy-MM-dd"))
                Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                    While dr.Read()
                        Me.Add(KasovInfo.GetKasovInfo(dr))
                    End While
                End Using
            End Using
        End Using
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access

End Class
