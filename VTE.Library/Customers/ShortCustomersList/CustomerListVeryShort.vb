
<Serializable()> _
Public Class CustomerListVeryShort
 Inherits ReadOnlyListBase(Of CustomerListVeryShort, CustomerInfoVeryShort)

#Region " Stored Procedures Names "
 Private Const SpZemiSite As String = "getCustomersListVeryShort"
 Private Const SpZemiPoId As String = "getCustomersListVeryShortById"
#End Region

#Region " Factory Methods "

 Public Shared Function GetCustomersList() As CustomerListVeryShort

  Return DataPortal.Fetch(Of CustomerListVeryShort)()

 End Function
 Public Shared Function GetCustomersListShortById(ByVal inId As Long) As CustomerInfoVeryShort

  Return (DataPortal.Fetch(Of CustomerListVeryShort)(New filterCriteriaByID(inId))).Item(0)

 End Function

 Private Sub New()
  
 End Sub

#End Region ' Factory Methods

#Region " Data Access "
 <Serializable()> _
Private Class filterCriteriaByID
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

 Private Overloads Sub DataPortal_Fetch()
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("CustomersInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = SpZemiSite
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New CustomerInfoVeryShort(dr)
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
 Private Overloads Sub DataPortal_Fetch(ByVal criterya As filterCriteriaByID)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("CustomersInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = SpZemiPoId
     cm.Parameters.AddWithValue("@id", criterya.InId)
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New CustomerInfoVeryShort(dr)
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
