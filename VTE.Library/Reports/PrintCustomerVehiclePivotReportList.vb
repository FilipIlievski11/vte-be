
<Serializable()> _
Public Class PrintCustomerVehiclePivotReportList
  Inherits ReadOnlyListBase(Of PrintCustomerVehiclePivotReportList, PrintCustomerVehiclePivotReportInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "printCustomerVehiclePivotReport"
  Private Const SpZemiActivniSopstvenici As String = "printCustomerVehiclePivotReportCurrentOwners"
  Private Const SpZemiPoranesniSopstvenici As String = "printCustomerVehiclePivotReportPrevOwners"
#End Region

#Region " Factory Methods "

    Public Shared Function GetPrintCustomerVehiclePivotReportList(ByVal startDate As Date, ByVal endDate As Date) As PrintCustomerVehiclePivotReportList

        Return DataPortal.Fetch(Of PrintCustomerVehiclePivotReportList)(New DateCritetria(startDate, endDate))

    End Function
    Public Shared Function GetPrintCustomerVehiclePivotReportListCurrentOwners(ByVal startDate As Date, ByVal endDate As Date) As PrintCustomerVehiclePivotReportList

        Return DataPortal.Fetch(Of PrintCustomerVehiclePivotReportList)(New DateCritetriaBoolean(startDate, endDate, True))

    End Function
    Public Shared Function GetPrintCustomerVehiclePivotReportListPrevOwners(ByVal startDate As Date, ByVal endDate As Date) As PrintCustomerVehiclePivotReportList

        Return DataPortal.Fetch(Of PrintCustomerVehiclePivotReportList)(New DateCritetriaBoolean(startDate, endDate, False))

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
    <Serializable()> _
    Private Class DateCritetriaBoolean

        Public StartDate As Date
        Public EndDate As Date
        Public bool As Boolean

        Public Sub New(ByVal StartDate As Date, ByVal EndDate As Date, ByVal bool As Boolean)
            Me.bool = bool
            Me.StartDate = StartDate
            Me.EndDate = EndDate
        End Sub
    End Class
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As DateCritetria)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("PrintCustomerVehiclePivotReportInfo.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = SpZemiSite
                    cm.Parameters.AddWithValue("@dateStart", Format(criteria.StartDate, "yyyy-MM-dd"))
                    cm.Parameters.AddWithValue("@dateEnd", Format(criteria.EndDate.AddDays(1), "yyyy-MM-dd"))

                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New PrintCustomerVehiclePivotReportInfo(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("PrintCustomerVehiclePivotReportInfo.DataPortal_Fetch", ex)
            Throw New DbCslaException("PrintCustomerVehiclePivotReportInfo.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As DateCritetriaBoolean)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("PrintCustomerVehiclePivotReportInfo.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    If criteria.bool Then
                        cm.CommandText = SpZemiActivniSopstvenici
                    Else
                        cm.CommandText = SpZemiPoranesniSopstvenici
                    End If
                    cm.Parameters.AddWithValue("@dateStart", Format(criteria.StartDate, "yyyy-MM-dd"))
                    cm.Parameters.AddWithValue("@dateEnd", Format(criteria.EndDate.AddDays(1), "yyyy-MM-dd"))

                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New PrintCustomerVehiclePivotReportInfo(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("PrintCustomerVehiclePivotReportInfo.DataPortal_Fetch", ex)
            Throw New DbCslaException("PrintCustomerVehiclePivotReportInfo.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access
End Class