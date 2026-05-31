
<Serializable()> _
Public Class VehicleEngineTypeList
  Inherits ReadOnlyListBase(Of VehicleEngineTypeList, VehicleEngineTypeInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "getVehicleEngineTypesList"
  Private Const SpZemiPoIdModel As String = "getVehicleEngineTypesByIdVehicleModel"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleEngineTypeList() As VehicleEngineTypeList

    Return DataPortal.Fetch(Of VehicleEngineTypeList)()

  End Function
  Public Shared Function GetVehicleEngineTypeList(ByVal inIdVehicleModel As Integer) As VehicleEngineTypeList

    Return DataPortal.Fetch(Of VehicleEngineTypeList)(New CriteriaByVehicleModel(inIdVehicleModel))

  End Function
  Public Function GetVehicleEngineTypeById(ByVal inId As Integer) As VehicleEngineTypeInfo

    For Each child As VehicleEngineTypeInfo In Me
      If child.Id = inId Then
        Return child
      End If
    Next
    Return Nothing
  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler VehicleEngineTypes.VehicleEngineTypesSaved, AddressOf VehicleEngineTypes_saved
  End Sub

  Private Sub VehicleEngineTypes_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
    IsReadOnly = False
    Me.Clear()
    IsReadOnly = True
    DataPortal_Fetch()
    Me.ResetBindings()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  <Serializable()> _
Private Class CriteriaByVehicleModel
    Private _idVehicleModel As Integer

    Public ReadOnly Property IdVehicleModel() As Integer
      Get
        Return _idVehicleModel
      End Get
    End Property

    Public Sub New(ByVal IdVehicleModel As Long)
      _idVehicleModel = IdVehicleModel
    End Sub
  End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleEngineTypeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleEngineTypeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleEngineTypeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleEngineTypeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByVehicleModel)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleEngineTypeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiPoIdModel
          cm.Parameters.AddWithValue("@idVehicleModel", criteria.IdVehicleModel)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleEngineTypeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleEngineTypeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleEngineTypeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
#End Region ' Data Access

End Class