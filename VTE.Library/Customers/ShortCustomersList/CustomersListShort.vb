
<Serializable()> _
Public Class CustomersListShort
    Inherits ReadOnlyListBase(Of CustomersListShort, CustomersInfoShort)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "getCustomersListShort"
    Private Const SpZemiPoId As String = "getCustomersListShortById"
    Private Const SpZemiPoString As String = "getCustomersListShortByString"
    Private Const SpZemiPoImePrezime As String = "getCustomersListShortByImePrezime"
    'Private Const SpZemiPoIsCompany As String = "getCustomersListByIsCompany"
    'Private Const SpZemiPoIsCompanyMinusTraffL As String = "getCustomersListByIsCompanyMinusInternationalDriveingLicences"
#End Region

#Region " Factory Methods "

    Public Shared Function GetCustomersList() As CustomersListShort

        Return DataPortal.Fetch(Of CustomersListShort)()

    End Function
    Public Shared Function GetCustomersListShortById(ByVal inId As Long) As CustomersListShort

        Return (DataPortal.Fetch(Of CustomersListShort)(New filterCriteriaByID(inId)))

    End Function

    Public Shared Function GetCustomersListShortByString(ByVal inString As String) As CustomersListShort

        Return (DataPortal.Fetch(Of CustomersListShort)(New filterCriteriaByString(inString)))

    End Function
    'Public Shared Function GetCustomersListIsCompany(ByVal inIsCompany As Boolean) As CustomersList

    '    Return DataPortal.Fetch(Of CustomersList)(New CriteriaIsCompany(inIsCompany))

    'End Function

    'Public Shared Function GetCustomersListIsCompanyMinusTraffL(ByVal inIsCompany As Boolean, ByVal pomInt As Integer) As CustomersList

    '    Return DataPortal.Fetch(Of CustomersList)(New CriteriaIsCompanyMinusTraffLicence(inIsCompany, pomInt))

    'End Function

    'Public Function GetCustomersListById(ByVal idIn As Integer) As CustomersInfo
    '    For Each child As CustomersInfo In Me
    '        If child.Id = idIn Then
    '            Return child
    '        End If
    '    Next
    '    Return Nothing
    'End Function

    Private Sub New()
        ' require use of factory methods
        ' AddHandler Customers.CustomersSaved, AddressOf Customers_saved
        'AddHandler Customer.CustomerSaved, AddressOf Customers_saved
    End Sub

    Private Sub Customers_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
        IsReadOnly = False
        Me.Clear()
        IsReadOnly = True
        DataPortal_Fetch()
        Me.ResetBindings()
    End Sub

#End Region ' Factory Methods

#Region " Data Access "
    <Serializable()> _
  Private Class filterCriteriaByID
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
  
    <Serializable()> _
  Private Class filterCriteriaByString
        Private _inString As String

        Public ReadOnly Property InString() As String
            Get
                Return _inString
            End Get
        End Property

        Public Sub New(ByVal inString As String)
            _inString = inString
        End Sub
    End Class

    Private Overloads Sub DataPortal_Fetch()
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("CustomersInfo.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = SpZemiSite
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New CustomersInfoShort(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("CustomersInfo.DataPortal_Fetch", ex)
            Throw New DbCslaException("CustomersInfo.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
    Private Overloads Sub DataPortal_Fetch(ByVal criterya As filterCriteriaByID)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("CustomersInfo.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = SpZemiPoId
                    cm.Parameters.AddWithValue("@id", criterya.InId)
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New CustomersInfoShort(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("CustomersInfo.DataPortal_Fetch", ex)
            Throw New DbCslaException("CustomersInfo.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As filterCriteriaByString)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("CustomersInfo.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    Dim br As Long
                    Dim eBr As Boolean
                    Try
                        br = CType(criteria.InString, Long)
                        eBr = True
                    Catch ex As Exception
                        eBr = False
                    End Try
                    If eBr Then
                        cm.CommandText = SpZemiPoString
                    Else
                        cm.CommandText = SpZemiPoImePrezime
                    End If

                    cm.Parameters.AddWithValue("@str", criteria.InString)
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New CustomersInfoShort(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("CustomersInfo.DataPortal_Fetch", ex)
            Throw New DbCslaException("CustomersInfo.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
#End Region ' Data Access


End Class
