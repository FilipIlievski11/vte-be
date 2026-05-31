
<Serializable()> _
Public Class DataBasesList
  Inherits ReadOnlyListBase(Of DataBasesList, DataBasesInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getDataBases"
#End Region

#Region " Factory Methods "

  Public Shared Function GetDataBasesList() As DataBasesList

    Return DataPortal.Fetch(Of DataBasesList)()

  End Function

 

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("DataBasesInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DataBasesInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DataBasesInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DataBasesInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class