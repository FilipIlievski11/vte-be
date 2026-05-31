
<Serializable()> _
Public Class VehicleTireTypeList
  Inherits ReadOnlyListBase(Of VehicleTireTypeList, VehicleTireTypeInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleTireTypes"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleTireTypeList() As VehicleTireTypeList

    Return DataPortal.Fetch(Of VehicleTireTypeList)()

  End Function

  Public Shared Function GetVehicleTireTypeListByIdModel(ByVal inIdModel As Integer) As VehicleTireTypeList

    Return DataPortal.Fetch(Of VehicleTireTypeList)(New CriteriaByModel(inIdModel))

  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler VehicleTireTypes.VehicleTireTypesSaved, AddressOf VehicleTireTypes_saved
  End Sub

  Private Sub VehicleTireTypes_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
    IsReadOnly = False
    Me.Clear()
    IsReadOnly = True
    DataPortal_Fetch()
    Me.ResetBindings()
  End Sub
#End Region ' Factory Methods

#Region " Data Access "
  <Serializable()> _
Private Class CriteriaByModel
    Private _idVehicleModel As Integer

    Public ReadOnly Property IdVehicleModel() As Integer
      Get
        Return _idVehicleModel
      End Get
    End Property

    Public Sub New(ByVal idVehicleModel As Integer)
      _idVehicleModel = idVehicleModel
    End Sub
  End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleTireTypeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleTireTypeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleTireTypeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleTireTypeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByModel)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleTireTypeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getVehicleTireTypesByIdModel"
          cm.Parameters.AddWithValue("@idModel", criteria.IdVehicleModel)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleTireTypeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleTireTypeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleTireTypeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
#End Region ' Data Access

End Class