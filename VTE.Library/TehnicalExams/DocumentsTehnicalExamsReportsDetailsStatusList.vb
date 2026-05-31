
<Serializable()> _
Public Class DocumentsTehnicalExamsReportsDetailsStatusList
  Inherits ReadOnlyListBase(Of DocumentsTehnicalExamsReportsDetailsStatusList, DocumentsTehnicalExamsReportsDetailsStatusInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getDocumentsTehnicalExamsReportsDetailsStatus"
#End Region

#Region " Factory Methods "

  Public Shared Function GetDocumentsTehnicalExamsReportsDetailsStatusList() As DocumentsTehnicalExamsReportsDetailsStatusList

    Return DataPortal.Fetch(Of DocumentsTehnicalExamsReportsDetailsStatusList)()

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("DocumentsTehnicalExamsReportsDetailsStatusInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentsTehnicalExamsReportsDetailsStatusInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentsTehnicalExamsReportsDetailsStatusInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportsDetailsStatusInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class