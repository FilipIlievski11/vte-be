
<Serializable()> _
Public Class BusinessTypeList
  Inherits ReadOnlyListBase(Of BusinessTypeList, BusinessTypeInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getBusinessTypes"
#End Region

#Region " Factory Methods "

  Public Shared Function GetBusinessTypeList() As BusinessTypeList

    Return DataPortal.Fetch(Of BusinessTypeList)()

  End Function

  Public Function GetBusinessTypeInfo(ByVal inId As Integer) As BusinessTypeInfo
    For Each child As BusinessTypeInfo In Me
      If child.Id = inId Then
        Return child
      End If
    Next
    Return Nothing
  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler BusinessTypes.BusinessTypesSaved, AddressOf BusinessTypes_saved
  End Sub

  Private Sub BusinessTypes_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("BusinessTypeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
     Dim nullItem As New BusinessTypeInfo(0, My.Resources.Nema, "")
          Me.Add(nullItem)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New BusinessTypeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("BusinessTypeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("BusinessTypeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access


End Class