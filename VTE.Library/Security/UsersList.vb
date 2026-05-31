
<Serializable()> _
Public Class UsersList
  Inherits ReadOnlyListBase(Of UsersList, UsersInfo)

  Public Function getInfoById(ByVal id As Integer) As UsersInfo
    For Each ch As UsersInfo In Me
      If ch.ID = id Then
        Return ch
      End If
    Next
    Return Nothing
  End Function

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getUsers"
  'Private Const SpZemiSiteList As String = "getUsersListByIdCompanyDatabase" '"getUsersByIdCompanyIdDatabaseRadovis" '
#End Region

#Region " Factory Methods "

  Public Shared Function GetUsersList() As UsersList

    Return DataPortal.Fetch(Of UsersList)()

  End Function
  Public Shared Function GetUsersListByStation(ByVal idStation As Integer, ByVal idDatabase As Integer) As UsersList

    Return DataPortal.Fetch(Of UsersList)(New filterCritetria(idStation, idDatabase))

  End Function
  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "
  <Serializable()> _
Private Class filterCritetria

    Public idStation As Integer
    Public idDatabase As Integer

    Public Sub New(ByVal idStation As Integer, ByVal idDatabase As Integer)
      Me.idStation = idStation
      Me.idDatabase = idDatabase
    End Sub
  End Class
  <RunLocal()> _
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("UsersInfo.DataPortal_Fetch", GetHashCode())
    Dim infoNull As New UsersInfo(0, 0, 0, "[Нема]", "[Нема]", "", 0, "")
    Me.Add(infoNull)
    Try

      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite

          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New UsersInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("UsersInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("UsersInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
  <RunLocal()> _
Private Overloads Sub DataPortal_Fetch(ByVal criteria As filterCritetria)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("UsersInfo.DataPortal_Fetch", GetHashCode())
    Dim infoNull As New UsersInfo(0, 0, 0, "[Нема]", "[Нема]", "", 0, "")
    Me.Add(infoNull)
    Try

      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          Select Case criteria.idDatabase
            Case 9
              cm.CommandText = "getUsersByIdCompanyIdDatabaseRadovis"
            Case 14
              cm.CommandText = "getUsersByIdCompanyIdDatabaseSkopje"
            Case Else
              cm.CommandText = "getUsersListByIdCompanyDatabase"
          End Select

          cm.Parameters.AddWithValue("@idStation", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
          cm.Parameters.AddWithValue("@idDatabase", criteria.idDatabase)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New UsersInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("UsersInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("UsersInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
#End Region ' Data Access
End Class