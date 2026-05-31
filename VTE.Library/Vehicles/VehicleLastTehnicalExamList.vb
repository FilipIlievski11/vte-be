
<Serializable()> _
Public Class VehicleLastTehnicalExamList
  Inherits ReadOnlyListBase(Of VehicleLastTehnicalExamList, VehicleLastTehnicalExamInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getLastVehicleTehnicalExam"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleLastTehnicalExamList(ByVal idVehicle As Long) As VehicleLastTehnicalExamList

    Return DataPortal.Fetch(Of VehicleLastTehnicalExamList)(New CriteriaIdVehicle(idVehicle))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "
  <Serializable()> _
Private Class CriteriaIdVehicle

    Private _idVehicle As Long

    Public ReadOnly Property IdVehicle() As Long
      Get
        Return _idVehicle
      End Get
    End Property

    Public Sub New(ByVal IdVehicle As Long)
      _idVehicle = IdVehicle
    End Sub

  End Class
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaIdVehicle)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleLastTehnicalExamInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          cm.Parameters.AddWithValue("@idVehicle", criteria.IdVehicle)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleLastTehnicalExamInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleLastTehnicalExamInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleLastTehnicalExamInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class