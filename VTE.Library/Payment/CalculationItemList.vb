
<Serializable()> _
Public Class CalculationItemList
  Inherits ReadOnlyListBase(Of CalculationItemList, CalculationItemInfo)

  Public Function GetCalculationItemById(ByVal id As Integer) As CalculationItemInfo
    For Each ch As CalculationItemInfo In Me
      If ch.Id = id Then
        Return ch
      End If
    Next
    Return Nothing
  End Function

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getCalculationItems"
#End Region

#Region " Factory Methods "

  Public Shared Function GetCalculationItemList() As CalculationItemList

    Return DataPortal.Fetch(Of CalculationItemList)()

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CalculationItemInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
     Dim nullItem As New CalculationItemInfo(0, My.Resources.Nema)
          Me.Add(nullItem)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CalculationItemInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CalculationItemInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CalculationItemInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class