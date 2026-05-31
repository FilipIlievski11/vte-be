
<Serializable()> _
Public Class ObjectPrivilegeList
  Inherits ReadOnlyListBase(Of ObjectPrivilegeList, ObjectPrivilegeInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "GetChildObjectPrivileges"
#End Region

#Region " Factory Methods "

  Public Shared Function EmptyList() As ObjectPrivilegeList
    Return New ObjectPrivilegeList
  End Function

  Public Shared Function GetObjectPrivilegeList(ByVal strUser As String, ByVal strPWD As String) As ObjectPrivilegeList
    Return DataPortal.Fetch(Of ObjectPrivilegeList)(New Criteria(strUser, strPWD))
  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  <Serializable()> _
  Private Class Criteria

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
  <RunLocal()> _
  Private Overloads Sub DataPortal_Fetch(ByVal crit As Criteria)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("ObjectPrivilegeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          cm.Parameters.AddWithValue("@user", crit.Username)
          cm.Parameters.AddWithValue("@pw", crit.Password)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New ObjectPrivilegeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("ObjectPrivilegeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("ObjectPrivilegeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class