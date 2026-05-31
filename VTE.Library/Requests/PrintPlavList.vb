<Serializable()> _
Public Class PrintPlavList
  Inherits ReadOnlyListBase(Of PrintPlavList, PrintPlavInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "printPlav"
#End Region

#Region " Factory Methods "

  Public Shared Function GetPrintPlavList(ByVal idRequest As Long) As PrintPlavList

    Return DataPortal.Fetch(Of PrintPlavList)(New SingleCriteria(Of PrintPlavList, Long)(idRequest))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of PrintPlavList, Long))
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
              Dim Info As New PrintPlavInfo(dr)
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
