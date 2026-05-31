
<Serializable()> _
Public Class printPermisionList
  Inherits ReadOnlyListBase(Of printPermisionList, printPermisionInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "printDocumentPermisionById"
#End Region

#Region " Factory Methods "

  Public Shared Function GetprintPermisionList(ByVal idPremision As Integer) As printPermisionList

    Return DataPortal.Fetch(Of printPermisionList)(New SingleCriteria(Of printPermisionList, Integer)(idPremision))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of printPermisionList, Integer))
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("printPermisionInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          cm.Parameters.AddWithValue("@id", criteria.Value)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New printPermisionInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("printPermisionInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("printPermisionInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class