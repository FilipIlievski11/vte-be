
<Serializable()> _
Public Class RoolList
  Inherits ReadOnlyListBase(Of RoolList, RoolInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getRoles"
#End Region

#Region " Factory Methods "

  Public Shared Function GetRoolList() As RoolList

    Return DataPortal.Fetch(Of RoolList)()

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("RoolInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New RoolInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("RoolInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("RoolInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class