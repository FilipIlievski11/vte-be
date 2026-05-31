
<Serializable()> _
Public Class RequestTypeList
  Inherits ReadOnlyListBase(Of RequestTypeList, RequestTypeInfo)

  Public Function getDisplayText(ByVal selectedId As Integer) As String
    Dim result As String = String.Empty
    recText(result, selectedId)
    Return result
  End Function

  Private Sub recText(ByRef strText As String, ByVal selectedId As Integer)
    If selectedId <> 0 Then
      For Each it As RequestTypeInfo In Me
        If it.Id = selectedId Then
          strText = it.TypeName & vbCrLf & strText
          recText(strText, it.IdRequestType)
        End If
      Next
    End If
  End Sub

    Public Function getInfoById(ByVal id As Integer) As RequestTypeInfo
        For Each it As RequestTypeInfo In Me
            If it.Id = id Then
                Return it
            End If
        Next
        Return Nothing
    End Function

  Public Function getParentId(ByVal id As Integer) As Integer
    For Each it As RequestTypeInfo In Me
      If it.Id = id Then
        Return it.IdRequestType
      End If
    Next
    Return Nothing
  End Function


#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getRequestTypes"
#End Region

#Region " Factory Methods "
  Public Shared Function EmptyList() As RequestTypeList
    Return New RequestTypeList
  End Function
  Public Shared Function GetRequestTypeList() As RequestTypeList

    Return DataPortal.Fetch(Of RequestTypeList)()

  End Function

  Public Shared Function getChildList(ByVal parentId As Integer) As RequestTypeList
    Return DataPortal.Fetch(Of RequestTypeList)(New SingleCriteria(Of RequestTypeList, Integer)(parentId))
  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler RequestTypes.RequestTypesSaved, AddressOf saved
  End Sub

  Private Sub saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("RequestTypeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New RequestTypeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("RequestTypeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("RequestTypeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of RequestTypeList, Integer))
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("RequestTypeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getRequestTypeByIdRequestType"
          cm.Parameters.AddWithValue("@id", criteria.Value)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New RequestTypeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("RequestTypeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("RequestTypeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class