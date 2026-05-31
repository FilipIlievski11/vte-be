
<Serializable()> _
Public Class printVehiclePivotReportList
  Inherits ReadOnlyListBase(Of printVehiclePivotReportList, printVehiclePivotReportInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "printVehiclePivotReport"
#End Region

#Region " Factory Methods "

    Public Shared Function GetprintVehiclePivotReportList(ByVal startDate As Date,endDate As date) As printVehiclePivotReportList

        Return DataPortal.Fetch(Of printVehiclePivotReportList)(New DateCritetria(startDate, endDate))

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
        Database.LogInfo("printVehiclePivotReportInfo.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = SpZemiSite
                    cm.Parameters.AddWithValue("@dateStart", Format(criteria.StartDate, "yyyy-MM-dd"))
                    cm.Parameters.AddWithValue("@dateEnd", Format(criteria.EndDate.AddDays(1), "yyyy-MM-dd"))
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New printVehiclePivotReportInfo(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("printVehiclePivotReportInfo.DataPortal_Fetch", ex)
            Throw New DbCslaException("printVehiclePivotReportInfo.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access
End Class