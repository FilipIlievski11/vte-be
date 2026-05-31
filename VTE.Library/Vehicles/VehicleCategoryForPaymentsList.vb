
<Serializable()> _
Public Class VehicleCategoryForPaymentsList
  Inherits ReadOnlyListBase(Of VehicleCategoryForPaymentsList, VehicleCategoryForPaymentsInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleCategoryForPayments"
  Private Const SpZemiPoCategoryAndBodytype As String = "getVehicleCategoryPaymentsByCategoryAndBodytype"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleCategoryForPaymentsList() As VehicleCategoryForPaymentsList

    Return DataPortal.Fetch(Of VehicleCategoryForPaymentsList)()

  End Function
  Public Function GetVehicleCategoryForPaymentsInfo(ByVal inID As Integer) As VehicleCategoryForPaymentsInfo
    For Each child As VehicleCategoryForPaymentsInfo In Me
      If child.Id = inID Then
        Return child
      End If
    Next
    Return Nothing
  End Function
  Public Shared Function GetVehicleCategoryForPaymentsListByCategory(ByVal inCategory As Integer, ByVal inBodytype As Integer) As VehicleCategoryForPaymentsList

    Return DataPortal.Fetch(Of VehicleCategoryForPaymentsList)(New CriteriaByCategoryAndBodyType(inCategory, inBodytype))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "
  <Serializable()> _
Private Class CriteriaByCategoryAndBodyType
    Private _idCategory As Integer
    Private _idBodytype As Integer

    Public ReadOnly Property IdCategory() As Integer
      Get
        Return _idCategory
      End Get
    End Property

    Public ReadOnly Property IdBodytype() As Integer
      Get
        Return _idBodytype
      End Get
    End Property

    Public Sub New(ByVal idCategory As Integer, ByVal idBodytype As Integer)
      _idCategory = idCategory
      _idBodytype = idBodytype
    End Sub
  End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleCategoryForPaymentsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleCategoryForPaymentsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleCategoryForPaymentsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleCategoryForPaymentsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByCategoryAndBodyType)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleCategoryForPaymentsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiPoCategoryAndBodytype
          cm.Parameters.AddWithValue("@idCategoty", criteria.IdCategory)
          cm.Parameters.AddWithValue("@idBodytype", criteria.IdBodytype)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleCategoryForPaymentsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleCategoryForPaymentsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleCategoryForPaymentsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
#End Region ' Data Access
End Class