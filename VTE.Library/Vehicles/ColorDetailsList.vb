
<Serializable()> _
Public Class ColorDetailsList
  Inherits ReadOnlyListBase(Of ColorDetailsList, ColorDetailsInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getColorsDetailsList"
#End Region

#Region " Factory Methods "

  Public Shared Function GetColorDetailsList() As ColorDetailsList

    Return DataPortal.Fetch(Of ColorDetailsList)()

  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler Colors.ColorsSaved, AddressOf ColorDetails_saved
  End Sub

  Private Sub ColorDetails_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("ColorDetailsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New ColorDetailsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("ColorDetailsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("ColorDetailsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class