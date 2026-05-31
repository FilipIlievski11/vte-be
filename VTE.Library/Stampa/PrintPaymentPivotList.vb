
<Serializable()> _
Public Class PrintPaymentPivotList

    Inherits ReadOnlyListBase(Of PrintPaymentPivotList, PrintPaymentPivotInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "PrintPaymentPivotByDate"
    Private Const SpZemiSiteForCustomer As String = "PrintPaymentPivotByCustomer"
    Private Const SpZemiSiteBezOpseg As String = "PrintPaymentPivot"
#End Region

#Region " Factory Methods "

    Public Shared Function GetPrintPaymentPivotList() As PrintPaymentPivotList

        Return DataPortal.Fetch(Of PrintPaymentPivotList)()

    End Function

    Public Shared Function GetPrintPaymentPivotListByDate(ByVal StartDate As Date, ByVal EndDate As Date) As PrintPaymentPivotList

        Return DataPortal.Fetch(Of PrintPaymentPivotList)(New DateCritetria(StartDate, EndDate))

    End Function

    Public Shared Function GetPrintPaymentPivotListByIdCustomer(ByVal idCustomer As Long) As PrintPaymentPivotList

        Return DataPortal.Fetch(Of PrintPaymentPivotList) _
          (New CustomerCritetria(idCustomer))

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
        '  Public isPayDocDate As Boolean

        Public Sub New(ByVal StartDate As Date, ByVal EndDate As Date)
            Me.StartDate = StartDate.Date
            Me.EndDate = EndDate.Date
            ' Me.isPayDocDate = isPayDocDate
        End Sub
    End Class
    <Serializable()> _
    Private Class CustomerCritetria

        Public idCustomer As Long


        Public Sub New(ByVal idCustomer As Long)
            Me.idCustomer = idCustomer

        End Sub
    End Class
    Private Overloads Sub DataPortal_Fetch()
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("PrintPaymentPivotList.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
                    cm.CommandText = SpZemiSiteBezOpseg
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New PrintPaymentPivotInfo(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("PrintPaymentPivotList.DataPortal_Fetch", ex)
            Throw New DbCslaException("PrintPaymentPivotList.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As DateCritetria)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("PrintPaymentPivotList.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure

                    cm.CommandText = SpZemiSite

                    cm.Parameters.AddWithValue("@StartDate", Format(criteria.StartDate, "yyyy-MM-dd"))
     cm.Parameters.AddWithValue("@EndDate", Format(criteria.EndDate.AddDays(1), "yyyy-MM-dd"))
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New PrintPaymentPivotInfo(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("PrintPaymentPivotList.DataPortal_Fetch", ex)
            Throw New DbCslaException("PrintPaymentPivotList.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As CustomerCritetria)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("PrintPaymentPivotList.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = SpZemiSiteForCustomer
     cm.Parameters.AddWithValue("@idCustomer", criteria.idCustomer)
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New PrintPaymentPivotInfo(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("PrintPaymentPivotList.DataPortal_Fetch", ex)
            Throw New DbCslaException("PrintPaymentPivotList.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
#End Region ' Data Access


End Class
