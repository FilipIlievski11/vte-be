Namespace Security

  <Serializable()> _
  Public Class FieldsPrivilegesList
    Inherits ReadOnlyListBase(Of FieldsPrivilegesList, FieldsPrivilegeInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "GetChildFieldsPrivileges"
#End Region

#Region " Factory Methods "

    Public Shared Function EmtyList() As FieldsPrivilegesList

      Return New FieldsPrivilegesList

    End Function

    Public Shared Function GetFieldsPrivilegesList(ByVal strUser As String, ByVal strPWD As String) As FieldsPrivilegesList

      Return DataPortal.Fetch(Of FieldsPrivilegesList)(New Criteria(strUser, strPWD))

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
      Database.LogInfo("FieldsPrivilegeInfo.DataPortal_Fetch", GetHashCode())
      Try
        Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
          Using cm As SqlCommand = cn.CreateCommand()
            cm.CommandType = CommandType.StoredProcedure
            cm.CommandText = SpZemiSite
            cm.Parameters.AddWithValue("@user", crit.Username)
            cm.Parameters.AddWithValue("@pw", crit.Password)
            Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
              While dr.Read()
                Dim Info As New FieldsPrivilegeInfo(dr)
                Me.Add(Info)
              End While
            End Using
          End Using
        End Using
      Catch ex As Exception
        Database.LogException("FieldsPrivilegeInfo.DataPortal_Fetch", ex)
        Throw New DbCslaException("FieldsPrivilegeInfo.DataPortal_Fetch", ex)
      End Try
      IsReadOnly = True
      RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access
  End Class

End Namespace