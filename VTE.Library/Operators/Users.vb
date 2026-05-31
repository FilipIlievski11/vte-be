
<Serializable()> _
Public Class Users
  Inherits Csla.BusinessListBase(Of Users, User)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetUserByID"
  Private Const spGetAll As String = "GetUsers"
  Private Const spUpdate As String = "updateUser"
  Private Const spAdd As String = "addUser"
  Private Const spDelete As String = "deleteUser"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As User = User.NewUserChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Users")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Users")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Users")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Users")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetUsers() As Users
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a Users")
    End If
    Return DataPortal.Fetch(Of Users)()
  End Function
    Public Shared Function GetUsersByStation(ByVal idStation As Integer, ByVal idDtabase As Integer) As Users
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User Not authorized to view a Users")
        End If
        Return DataPortal.Fetch(Of Users)(New CriteriaByStation(idStation, idDtabase))
    End Function
  Public Shared Function GetUsersByCompany(ByVal idStation As Integer, ByVal idDtabase As Integer) As Users
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a Users")
    End If
    Return DataPortal.Fetch(Of Users)(New CriteriaByCompany(idStation, idDtabase))
  End Function
#End Region ' Factory Methods

#Region " Data Access "
    <Serializable()> _
Private Class CriteriaByStation
        Private _InIdStat As Integer
        Private _InIdData As Integer
        Public ReadOnly Property InIdStat() As Integer
            Get
                Return _InIdStat
            End Get
        End Property
        Public ReadOnly Property InIdData() As Integer
            Get
                Return _InIdData
            End Get
        End Property

        Public Sub New(ByVal InIdStat As Integer, ByVal InIdData As Integer)
            _InIdStat = InIdStat
            _InIdData = InIdData
        End Sub

  End Class

  <Serializable()> _
Private Class CriteriaByCompany
    Private _InIdStat As Integer
    Private _InIdData As Integer
    Public ReadOnly Property InIdStat() As Integer
      Get
        Return _InIdStat
      End Get
    End Property
    Public ReadOnly Property InIdData() As Integer
      Get
        Return _InIdData
      End Get
    End Property

    Public Sub New(ByVal InIdStat As Integer, ByVal InIdData As Integer)
      _InIdStat = InIdStat
      _InIdData = InIdData
    End Sub

  End Class

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("Users.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(User.GetUser(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("Users.Child_Fetch", ex)
      Throw New DbCslaException("Users.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByStation)
        RaiseListChangedEvents = False
        Database.LogInfo("Users.Child_Fetch", GetHashCode())
        Try

            Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "getUsersByIdStationIdDatabase"
                    cm.Parameters.AddWithValue("@idStat", criteria.InIdStat)
                    cm.Parameters.AddWithValue("@idDatab", criteria.InIdData)
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        While dr.Read()
                            Me.Add(User.GetUser(dr))
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("Users.Child_Fetch", ex)
            Throw New DbCslaException("Users.Child_Fetch", ex)
        End Try
        RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByCompany)
    RaiseListChangedEvents = False
    Database.LogInfo("Users.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          Select Case criteria.InIdData
            Case 9
              cm.CommandText = "getUsersByIdCompanyIdDatabaseRadovis"
            Case 14
              cm.CommandText = "getUsersByIdCompanyIdDatabaseSkopje"
            Case Else
              cm.CommandText = "getUsersListByIdCompanyDatabase"
          End Select
          ' cm.CommandText = "getUsersByIdCompanyIdDatabase"
          cm.Parameters.AddWithValue("@idStation", criteria.InIdStat)
          cm.Parameters.AddWithValue("@idDatabase", criteria.InIdData)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(User.GetUser(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("Users.Child_Fetch", ex)
      Throw New DbCslaException("Users.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub
  Protected Overrides Sub DataPortal_Update()
    RaiseListChangedEvents = False
    Child_Update()
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access


End Class
