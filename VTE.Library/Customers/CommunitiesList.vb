
<Serializable()> _
Public Class CommunitiesList
  Inherits ReadOnlyListBase(Of CommunitiesList, CommunitiesInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getCommunities"
#End Region

#Region " Factory Methods "

  Public Shared Function GetCommunitiesList() As CommunitiesList

    Return DataPortal.Fetch(Of CommunitiesList)()

  End Function

  Public Function GetCommunitiesListById(ByVal inId As Integer) As CommunitiesInfo
    For Each child As CommunitiesInfo In Me
      If child.Id = inId Then
        Return child
      End If
    Next
    Return Nothing
  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler Communities.CommunitiesSaved, AddressOf Communities_saved
  End Sub

  Private Sub Communities_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
        Database.LogInfo("CommunitiesInfo.DataPortal_Fetch", GetHashCode())
        Dim infoNull As New CommunitiesInfo(0, "", "[Сите]", "")
        Me.Add(infoNull)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CommunitiesInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CommunitiesInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CommunitiesInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class