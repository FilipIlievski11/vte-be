Public Class TechExamReportNewRptList

    Inherits ReadOnlyListBase(Of TechExamReportNewRptList, TechExamReportNewRptInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "getTechExamReportNewRptInfoById"
#End Region

#Region " Factory Methods "

    Public Shared Function GetTechExamReportNewRptListById(ByVal inId As Long) As TechExamReportNewRptList

        Return DataPortal.Fetch(Of TechExamReportNewRptList)(New FilterCriteria(inId))

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
  Private Class FilterCriteria
        Private _inId As Long

        Public ReadOnly Property InId() As Long
            Get
                Return _inId
            End Get
        End Property

        Public Sub New(ByVal inId As Long)
            _inId = inId
        End Sub
    End Class
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As FilterCriteria)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Using cn As SqlConnection = Database.VTE_SqlConnection
            Using cm As SqlCommand = cn.CreateCommand()
                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = SpZemiSite
                cm.Parameters.AddWithValue("@Id", criteria.InId)
                Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                    While dr.Read()
                        Me.Add(TechExamReportNewRptInfo.GetTechExamReportNewRptInfo(dr))
                    End While
                End Using
            End Using
        End Using
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access
End Class
