
<Serializable()> _
Public Class StreetsList
  Inherits ReadOnlyListBase(Of StreetsList, StreetsInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getStreets"
#End Region

#Region " Factory Methods "

  Public Shared Function GetStreetsList() As StreetsList

    Return DataPortal.Fetch(Of StreetsList)()

  End Function
  Public Function GetStreetInfo(ByVal InID As Integer) As StreetsInfo
    For Each child As StreetsInfo In Me
      If child.Id = InID Then
        Return child
      End If
    Next
    Return Nothing
  End Function
  Private Sub New()
    ' require use of factory methods
    AddHandler Streets.StreetsSaved, AddressOf Streets_saved
  End Sub

  Private Sub Streets_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
        Database.LogInfo("StreetsInfo.DataPortal_Fetch", GetHashCode())
        Dim infoNull As New StreetsInfo(0, "", "")
        Me.Add(infoNull)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New StreetsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("StreetsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("StreetsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access

End Class