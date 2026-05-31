
<Serializable()> _
Public Class CustomersSearchList
 Inherits ReadOnlyListBase(Of CustomersSearchList, CustomersSearchInfo)

#Region " Stored Procedures Names "
 Private Const SpZemiPoString As String = "getCustomersListShortByString"
 Private Const SpZemiPoImePrezime As String = "getCustomersListShortByImePrezime"
 Private Const SpZemiPoId As String = "getCustomersListShortById"
#End Region

#Region " Factory Methods "

 Public Shared Function GetCustomersListShortByString(ByVal inString As String) As CustomersSearchList

  Return (DataPortal.Fetch(Of CustomersSearchList)(New filterCriteriaByString(inString)))

 End Function

 Public Shared Function GetCustomersListShortById(ByVal inId As String) As CustomersSearchList

  Return (DataPortal.Fetch(Of CustomersSearchList)(New filterCriteriaById(inId)))

 End Function

 Private Sub New()

 End Sub

#End Region ' Factory Methods

#Region " Data Access "
 <Serializable()> _
  Private Class filterCriteriaByString
  Private _inString As String

  Public ReadOnly Property InString() As String
   Get
    Return _inString
   End Get
  End Property

  Public Sub New(ByVal inString As String)
   _inString = inString
  End Sub
 End Class

 <Serializable()> _
 Private Class filterCriteriaById
  Private _inId As Long

  Public ReadOnly Property InId() As Long
   Get
    Return _inId
   End Get
  End Property

  Public Sub New(ByVal inId As Long)
   _inId = inId
  End Sub
 End Class

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As filterCriteriaByString)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("CustomersInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     Dim br As Long
     Dim eBr As Boolean
     Try
      br = CType(criteria.InString, Long)
      eBr = True
     Catch ex As Exception
      eBr = False
     End Try
     If eBr Then
      cm.CommandText = SpZemiPoString
     Else
      cm.CommandText = SpZemiPoImePrezime
     End If

     cm.Parameters.AddWithValue("@str", criteria.InString)
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New CustomersSearchInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("CustomersInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("CustomersInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub


 Private Overloads Sub DataPortal_Fetch(ByVal criteria As filterCriteriaById)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("CustomersInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
   
     cm.CommandText = SpZemiPoId


     cm.Parameters.AddWithValue("@id", criteria.InId)
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New CustomersSearchInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("CustomersInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("CustomersInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub

#End Region ' Data Access


End Class
