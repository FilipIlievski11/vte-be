<Serializable()> _
Public Class PrintZelenList
  Inherits ReadOnlyListBase(Of PrintZelenList, PrintZelenInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "printZelen"
#End Region

#Region " Factory Methods "

  Public Shared Function GetPrintZelenList(ByVal idRequest As Long) As PrintZelenList

    Return DataPortal.Fetch(Of PrintZelenList)(New SingleCriteria(Of PrintZelenList, Long)(idRequest))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of PrintZelenList, Long))
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("PrintZelenInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          cm.Parameters.AddWithValue("@id", criteria.Value)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New PrintZelenInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("PrintZelenInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("PrintZelenInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class