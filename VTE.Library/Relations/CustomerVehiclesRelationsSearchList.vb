<Serializable()> _
Public Class CustomerVehiclesRelationsSearchList
 Inherits ReadOnlyListBase(Of CustomerVehiclesRelationsSearchList, CustomerVehiclesRelationsSearchInfo)

#Region " Stored Procedures Names "
 
 Private Const SpZemiSiteByStringVozilo As String = "getCustomerVehiclesRelationsListByStringVehicle"
 Private Const SpZemiSiteByStringCust As String = "getCustomerVehiclesRelationsListByStringCustomer"
 Private Const SpZemiSiteByStringId As String = "getCustomerVehiclesRelationsListByStringId"
#End Region

#Region " Factory Methods "

 Public Shared Function GetCustomerVehiclesRelationsListByString(ByVal inStr As String, ByVal vehicle As Boolean) As CustomerVehiclesRelationsSearchList

  Return DataPortal.Fetch(Of CustomerVehiclesRelationsSearchList)(New CriteriaByString(inStr, vehicle))

 End Function

 Public Shared Function GetCustomerVehiclesRelationsListById(ByVal inId As Long) As CustomerVehiclesRelationsSearchList

  Return DataPortal.Fetch(Of CustomerVehiclesRelationsSearchList)(New CriteriaById(inId))

 End Function

 Private Sub New()

 End Sub

#End Region ' Factory Methods

#Region " Data Access "
 <Serializable()> _
Private Class CriteriaByString
  Private _inStr As String
  Private _vehicle As Boolean

  Public ReadOnly Property InString() As String
   Get
    Return _inStr
   End Get
  End Property

  Public ReadOnly Property Vehicle() As Boolean
   Get
    Return _vehicle
   End Get
  End Property

  Public Sub New(ByVal inStr As String, ByVal vehicle As Boolean)
   _inStr = inStr
   _vehicle = vehicle
  End Sub
 End Class
 
 <Serializable()> _
Private Class CriteriaById
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

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByString)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("CustomerVehiclesRelationsInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     If criteria.Vehicle Then
      cm.CommandText = SpZemiSiteByStringVozilo
     Else
      cm.CommandText = SpZemiSiteByStringCust
     End If
     cm.Parameters.AddWithValue("@str", criteria.InString)
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New CustomerVehiclesRelationsSearchInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub
 
 Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaById)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("CustomerVehiclesRelationsInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
    
     cm.CommandText = SpZemiSiteByStringId

     cm.Parameters.AddWithValue("@id", criteria.InId)
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New CustomerVehiclesRelationsSearchInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub


#End Region


End Class
