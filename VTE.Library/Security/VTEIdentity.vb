
Imports System.Security.Principal

Namespace Security

  <Serializable()> _
 Public Class VTEIdentity
    Inherits ReadOnlyBase(Of VTEIdentity)

    Implements IIdentity

    Private _FieldsPrivileges As FieldsPrivilegesList

    Public ReadOnly Property FildPrivileges() As FieldsPrivilegesList
      Get
        Return _FieldsPrivileges
      End Get
    End Property

    Private _ObjectPrivileges As ObjectPrivilegeList

    Public ReadOnly Property ObjectPrivileges() As ObjectPrivilegeList
      Get
        Return _ObjectPrivileges
      End Get
    End Property

    Private _vteConnectionString As String = String.Empty
    Public ReadOnly Property VteConnectionString() As String
      Get
        Return _vteConnectionString
      End Get
    End Property


#Region " Business Methods "


    Protected Overrides Function GetIdValue() As Object

      Return _name

    End Function

#Region " IsInRole "

    Private _roles As New List(Of String)

    Friend Function IsInRole(ByVal role As String) As Boolean

      Return _roles.Contains(role)

    End Function

#End Region

#Region " IIdentity "

    Private _isAuthenticated As Boolean
    Private _name As String = ""
    Private _fullName As String = ""
        Private _roleName As String = ""
        Private _idStation As Integer = 0

    Public ReadOnly Property AuthenticationType() As String _
      Implements System.Security.Principal.IIdentity.AuthenticationType
      Get
        Return "Csla"
      End Get
    End Property

    Public ReadOnly Property IsAuthenticated() As Boolean _
      Implements System.Security.Principal.IIdentity.IsAuthenticated
      Get
        Return _isAuthenticated
      End Get
    End Property

    Public ReadOnly Property Name() As String _
      Implements System.Security.Principal.IIdentity.Name
      Get
        Return _name
      End Get
    End Property

        Public ReadOnly Property FullName() As String
            Get
                Return _fullName
            End Get
        End Property

    Public ReadOnly Property RoleName() As String
      Get
        Return _roleName
      End Get
    End Property
        Public ReadOnly Property IdStation() As Integer
            Get
                Return _idStation
            End Get
        End Property


#End Region

#End Region

#Region " Factory Methods "

    Friend Shared Function UnauthenticatedIdentity() As VTEIdentity
      Csla.ApplicationContext.LocalContext.Remove("EmployeeID")

      Return New VTEIdentity

    End Function

    Friend Shared Function GetIdentity( _
      ByVal username As String, ByVal password As String) As VTEIdentity

      Return DataPortal.Fetch(Of VTEIdentity)(New CredentialsCriteria(username, password))

    End Function

    Friend Shared Function GetIdentity( _
      ByVal username As String) As VTEIdentity

      Return DataPortal.Fetch(Of VTEIdentity)(New LoadOnlyCriteria(username))

    End Function

    Private Sub New()
      ' require use of factory methods
      _ObjectPrivileges = ObjectPrivilegeList.EmptyList
      _FieldsPrivileges = FieldsPrivilegesList.EmtyList
    End Sub

#End Region

#Region " Data Access "

    <Serializable()> _
    Private Class CredentialsCriteria

      Private _username As String
      Private _password As String

      Public ReadOnly Property Username() As String
        Get
          Return _username
        End Get
      End Property

      Public ReadOnly Property Password() As String
        Get
          Return _password
        End Get
      End Property

      Public Sub New(ByVal username As String, ByVal password As String)
        _username = username
        _password = password
      End Sub
    End Class

    <Serializable()> _
    Private Class LoadOnlyCriteria

      Private _username As String

      Public ReadOnly Property Username() As String
        Get
          Return _username
        End Get
      End Property

      Public Sub New(ByVal username As String)
        _username = username
      End Sub
    End Class

    <RunLocal()> _
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As CredentialsCriteria)

      Using cn As New SqlConnection(Database.SecurityConnection)
        cn.Open()
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandText = "Login"
          cm.CommandType = CommandType.StoredProcedure
          cm.Parameters.AddWithValue("@user", criteria.Username)
          cm.Parameters.AddWithValue("@pw", criteria.Password)
          Using dr As SqlDataReader = cm.ExecuteReader()
            Fetch(dr)
          End Using
        End Using
        _vteConnectionString = GetVteConnectionString(cn, Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
      End Using

      _ObjectPrivileges = ObjectPrivilegeList.GetObjectPrivilegeList(criteria.Username, criteria.Password)
      _FieldsPrivileges = FieldsPrivilegesList.GetFieldsPrivilegesList(criteria.Username, criteria.Password)

    End Sub

    Private Function GetVteConnectionString(ByVal cn As SqlConnection, ByVal idUser As Integer) As String
      Dim result As String = String.Empty
      Using cm As SqlCommand = cn.CreateCommand
        cm.CommandType = CommandType.StoredProcedure
        cm.CommandText = "getConnectionString"
        cm.Parameters.AddWithValue("@userId", idUser)
        Using dr As New SafeDataReader(cm.ExecuteReader)
          If dr.Read Then
            result = String.Format(dr.GetString("ConnetionString"), dr.GetString("DatabaseName"))
          End If
        End Using
      End Using

      Return result
    End Function

    <RunLocal()> _
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As LoadOnlyCriteria)

      Using cn As New SqlConnection(Database.SecurityConnection)
        cn.Open()
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandText = "GetUser"
          cm.CommandType = CommandType.StoredProcedure
          cm.Parameters.AddWithValue("@user", criteria.Username)
          Using dr As SqlDataReader = cm.ExecuteReader()
            Fetch(dr)
          End Using
        End Using

      End Using

    End Sub

    Private Sub Fetch(ByVal dr As SqlDataReader)

      If dr.Read Then
        _name = dr.GetString(4)
        _fullName = dr.GetString(3)
        _isAuthenticated = True
        _roleName = dr.GetString(6)
        _roles.Add(dr.GetString(6))
                _idStation = dr.GetInt32(11)
                Console.WriteLine(dr.GetInt64(10))
                If Csla.ApplicationContext.LocalContext.Contains("EmployeeID") Then
                    Csla.ApplicationContext.LocalContext.Remove("EmployeeID")
                End If
                Csla.ApplicationContext.LocalContext.Add("EmployeeID", dr.GetInt64(10))
                If Csla.ApplicationContext.LocalContext.Contains("EmployeeFullName") Then
                    Csla.ApplicationContext.LocalContext.Remove("EmployeeFullName")
                End If
                Csla.ApplicationContext.LocalContext.Add("EmployeeFullName", _fullName)

                If Csla.ApplicationContext.LocalContext.Contains("IdStation") Then
                    Csla.ApplicationContext.LocalContext.Remove("IdStation")
                End If
                Csla.ApplicationContext.LocalContext.Add("IdStation", _idStation)
        Dim cry As New Crypt("VTEConnection")
      Else
        _name = ""
        _isAuthenticated = False
        _roleName = ""
        _roles.Clear()
      End If

    End Sub

#End Region




#Region " VerifyUser "

    Friend Shared Function VerifyIdentity( _
    ByVal username As String, ByVal password As String) As Integer

      Return IdentityExitstCommand.IsIdentityExists(username, password)

    End Function

    <Serializable()> _
   Private Class IdentityExitstCommand
      Inherits CommandBase
      Private _userName As String
      Private _password As String
      Private _IdentityExists As Integer = 0
      Public ReadOnly Property IdentityExists() As Integer
        Get
          Return _IdentityExists
        End Get
      End Property

      Public Shared Function IsIdentityExists(ByVal username As String, ByVal password As String) As Integer

        Dim result As IdentityExitstCommand
        result = DataPortal.Execute(Of IdentityExitstCommand)(New IdentityExitstCommand(username, password))
        Return result.IdentityExists

      End Function

      Private Sub New(ByVal username As String, ByVal password As String)
        _userName = username
        _password = password
        _IdentityExists = 0
      End Sub

      <RunLocal()> _
      Protected Overrides Sub DataPortal_Execute()
        Using cn As New SqlConnection(Database.SecurityConnection)
          cn.Open()
          Using cm As SqlCommand = cn.CreateCommand
            cm.CommandType = CommandType.StoredProcedure
            cm.CommandText = "Login"
            cm.Parameters.AddWithValue("@user", _userName)
            cm.Parameters.AddWithValue("@pw", _password)
            Using dr As New SafeDataReader(cm.ExecuteReader)
              If dr.Read Then
                _IdentityExists = dr.GetInt64("IdEmployee")
              End If
            End Using
          End Using
        End Using
      End Sub

    End Class

#End Region

  End Class
End Namespace


