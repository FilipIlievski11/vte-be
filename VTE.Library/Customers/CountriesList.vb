
<Serializable()> _
Public Class CountriesList
  Inherits ReadOnlyListBase(Of CountriesList, CountriesInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getCountries"
#End Region

#Region " Factory Methods "

  Public Shared Function GetCountriesList() As CountriesList

    Return DataPortal.Fetch(Of CountriesList)()

  End Function

  Public Function GetCountriesListById(ByVal IdIn As Integer) As CountriesInfo
    For Each child As CountriesInfo In Me
      If child.Id = IdIn Then
        Return child
      End If
    Next
    Return Nothing
  End Function
  Private Sub New()
    ' require use of factory methods
    AddHandler Countries.CountriesSaved, AddressOf Countries_saved
  End Sub

  Private Sub Countries_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("CountriesInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CountriesInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CountriesInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CountriesInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access


End Class