
<Serializable()> _
Public Class DDVList
  Inherits ReadOnlyListBase(Of DDVList, DDVInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getDDVCatalog"
#End Region

#Region " Factory Methods "

  Public Function GetInfo(ByVal id As Integer) As DDVInfo
    For Each info As DDVInfo In Me
      If info.Id = id Then
        Return info
      End If
    Next
    Return Nothing
  End Function

  Public Shared Function GetDDVList() As DDVList

    Return DataPortal.Fetch(Of DDVList)()

  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler DDVCatalogs.DDVCatalogsSaved, AddressOf DDVCatalogs_saved
  End Sub

  Private Sub DDVCatalogs_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("DDVInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DDVInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DDVInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DDVInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class