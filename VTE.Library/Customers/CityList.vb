
<Serializable()> _
Public Class CityList
  Inherits ReadOnlyListBase(Of CityList, CityInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getCitiesCommunitiesCountriesList"
#End Region

#Region " Factory Methods "

  Public Shared Function GetCityList() As CityList

    Return DataPortal.Fetch(Of CityList)()

  End Function
  Public Function GetCityListById(ByVal idIn As Integer) As CityInfo
    For Each child As CityInfo In Me
      If child.id = idIn Then
        Return child
      End If
    Next
    Return Nothing

  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler Cities.CitiesSaved, AddressOf Cities_saved
  End Sub
  Private Sub Cities_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("CityInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CityInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CityInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CityInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class
