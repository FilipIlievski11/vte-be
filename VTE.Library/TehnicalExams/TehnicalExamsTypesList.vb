
<Serializable()> _
Public Class TehnicalExamsTypesList
  Inherits ReadOnlyListBase(Of TehnicalExamsTypesList, TehnicalExamsTypesInfo)

  Public Function GetInfoById(ByVal id As Integer) As TehnicalExamsTypesInfo
    For Each it As TehnicalExamsTypesInfo In Me
      If it.Id = id Then
        Return it
      End If
    Next
    Return Nothing
  End Function

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getTehnicalExamsTypes"
#End Region

#Region " Factory Methods "

  Public Shared Function GetTehnicalExamsTypesList() As TehnicalExamsTypesList

    Return DataPortal.Fetch(Of TehnicalExamsTypesList)()

  End Function
  Public Function GetTehnicalExamsTypesListById(ByVal InId As Integer) As TehnicalExamsTypesInfo
    For Each child As TehnicalExamsTypesInfo In Me
      If child.Id = InId Then
        Return child
      End If
    Next
    Return Nothing
  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler TehnicalExamsTypes.TehnicalExamsTypesSaved, AddressOf TehnicalExamsTypes_saved
  End Sub

  Private Sub TehnicalExamsTypes_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
    IsReadOnly = False
    Me.Clear()
    IsReadOnly = True
    DataPortal_Fetch()
    Me.ResetBindings()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
        Database.LogInfo("TehnicalExamsTypesInfo.DataPortal_Fetch", GetHashCode())

        Dim infoNull As New TehnicalExamsTypesInfo(0, "[Нема]", "[Нема]", 0, 0, False)
        Me.Add(infoNull)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New TehnicalExamsTypesInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("TehnicalExamsTypesInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("TehnicalExamsTypesInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class