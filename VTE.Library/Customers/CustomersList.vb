
<Serializable()> _
Public Class CustomersList
  Inherits ReadOnlyListBase(Of CustomersList, CustomersInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getCustomersList"
  Private Const SpZemiPoIsCompany As String = "getCustomersListByIsCompany"
  Private Const SpZemiPoIsCompanyMinusTraffL As String = "getCustomersListByIsCompanyMinusInternationalDriveingLicences"
#End Region

#Region " Factory Methods "

  Public Shared Function GetCustomersList() As CustomersList

    Return DataPortal.Fetch(Of CustomersList)()

  End Function


  Public Shared Function GetCustomersListIsCompany(ByVal inIsCompany As Boolean) As CustomersList

    Return DataPortal.Fetch(Of CustomersList)(New CriteriaIsCompany(inIsCompany))

  End Function

  Public Shared Function GetCustomersListIsCompanyMinusTraffL(ByVal inIsCompany As Boolean, ByVal pomInt As Integer) As CustomersList

    Return DataPortal.Fetch(Of CustomersList)(New CriteriaIsCompanyMinusTraffLicence(inIsCompany, pomInt))

  End Function

  Public Function GetCustomersListById(ByVal idIn As Integer) As CustomersInfo
    For Each child As CustomersInfo In Me
      If child.Id = idIn Then
        Return child
      End If
    Next
    Return Nothing
  End Function

  Private Sub New()
    ' require use of factory methods
        ' AddHandler Customers.CustomersSaved, AddressOf Customers_saved
        ' AddHandler Customer.CustomerSaved, AddressOf Customers_saved
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
Private Class CriteriaIsCompany
    Private _isCompany As Boolean

    Public ReadOnly Property IsCompany() As Boolean
      Get
        Return _isCompany
      End Get
    End Property

    Public Sub New(ByVal IsCompany As Boolean)
      _isCompany = IsCompany
    End Sub
  End Class


  <Serializable()> _
Private Class CriteriaIsCompanyMinusTraffLicence
    Private _isCompany As Boolean
    Private _pomInt As Integer

    Public ReadOnly Property IsCompany() As Boolean
      Get
        Return _isCompany
      End Get
    End Property

    Public ReadOnly Property PomInt() As Integer
      Get
        Return _pomInt
      End Get
    End Property

    Public Sub New(ByVal IsCompany As Boolean, ByVal PomInt As Integer)
      _isCompany = IsCompany
      _pomInt = PomInt
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
              Dim Info As New CustomersInfo(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaIsCompany)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CustomersInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiPoIsCompany
          cm.Parameters.AddWithValue("@isCompany", criteria.IsCompany)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CustomersInfo(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaIsCompanyMinusTraffLicence)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CustomersInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiPoIsCompanyMinusTraffL
          cm.Parameters.AddWithValue("@isCompany", criteria.IsCompany)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CustomersInfo(dr)
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