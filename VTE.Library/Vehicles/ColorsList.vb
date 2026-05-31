
<Serializable()> _
Public Class ColorsList
  Inherits ReadOnlyListBase(Of ColorsList, ColorsInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getColors"
#End Region

#Region " Factory Methods "

  Public Shared Function GetColorsList() As ColorsList

    Return DataPortal.Fetch(Of ColorsList)()

  End Function

  Public Function GetColorsInfo(ByVal inId As Integer) As ColorsInfo
    For Each col As ColorsInfo In Me
      If col.Id = inId Then
        Return col
      End If
    Next
    Return Nothing
  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler Colors.ColorsSaved, AddressOf VehicleColors_saved
  End Sub

  Private Sub VehicleColors_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("ColorsInfo.DataPortal_Fetch", GetHashCode())
    Dim infoNull As New ColorsInfo(0, "[Нема]", "[Нема]", "", 0, "")
    Me.Add(infoNull)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New ColorsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("ColorsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("ColorsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access


End Class