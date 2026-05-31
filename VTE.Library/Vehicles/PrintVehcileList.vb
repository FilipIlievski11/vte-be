
<Serializable()> _
Public Class PrintVehcileList
  Inherits ReadOnlyListBase(Of PrintVehcileList, PrintVehcileInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "printVehcile"
#End Region

#Region " Factory Methods "
  Public Shared Function EmptyList() As PrintVehcileList
    Return New PrintVehcileList
  End Function

  Public Shared Function GetPrintVehcileList(ByVal idVehicle As Long) As PrintVehcileList

    Return DataPortal.Fetch(Of PrintVehcileList)(New SingleCriteria(Of PrintVehcileList, Long)(idVehicle))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of PrintVehcileList, Long))
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("PrintVehcileInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          cm.Parameters.AddWithValue("@id", criteria.Value)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New PrintVehcileInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("PrintVehcileInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("PrintVehcileInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access
End Class