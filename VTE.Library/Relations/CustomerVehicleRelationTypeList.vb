
<Serializable()> _
Public Class CustomerVehicleRelationTypeList
  Inherits ReadOnlyListBase(Of CustomerVehicleRelationTypeList, CustomerVehicleRelationTypeInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getCustomerVehiclesRelationTypes"
#End Region

#Region " Factory Methods "

  Public Shared Function GetCustomerVehicleRelationTypeList() As CustomerVehicleRelationTypeList

    Return DataPortal.Fetch(Of CustomerVehicleRelationTypeList)()

  End Function
  Public Shared Function GetCustomerVehicleRelationTypeListBYIsCustomerOnly(ByVal isCustomerOnlyIn As Boolean) As CustomerVehicleRelationTypeList

    Return DataPortal.Fetch(Of CustomerVehicleRelationTypeList)(New CriteriaIsCustomerOnly(isCustomerOnlyIn))

  End Function
  Public Function GetCustomerVehicleRelationTypeListById(ByVal idIn As Integer) As CustomerVehicleRelationTypeInfo
    For Each child As CustomerVehicleRelationTypeInfo In Me
      If child.Id = idIn Then
        Return child
        Exit Function
      End If
    Next
    Return Nothing

  End Function
  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  <Serializable()> _
Private Class CriteriaIsCustomerOnly

    Private _isCustomerOnly As Boolean

    Public ReadOnly Property IsCustomerOnly() As Boolean
      Get
        Return _isCustomerOnly
      End Get
    End Property

    Public Sub New(ByVal IsCustomerOnly As Boolean)
      _isCustomerOnly = IsCustomerOnly
    End Sub

  End Class

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CustomerVehicleRelationTypeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CustomerVehicleRelationTypeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CustomerVehicleRelationTypeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CustomerVehicleRelationTypeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaIsCustomerOnly)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CustomerVehicleRelationTypeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getCustomerVehiclesRelationTypesByIsCustomerOnly"
          cm.Parameters.AddWithValue("@isCustomerOnly", criteria.IsCustomerOnly)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CustomerVehicleRelationTypeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CustomerVehicleRelationTypeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CustomerVehicleRelationTypeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
#End Region ' Data Access
End Class