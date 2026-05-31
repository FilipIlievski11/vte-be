
<Serializable()> _
Public Class VehicleList
  Inherits ReadOnlyListBase(Of VehicleList, VehicleInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehiclesList"
  Private Const SpZemiOdjaveniVozila As String = "getVehiclesListOdjaveniVozila"
    Private Const SpZemiVozilaZaBel As String = "getVehiclesListVozilaZaBel"
    Private Const SpZemiVehicleById As String = "getVehiclesListById"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleList() As VehicleList
    Try
      Return DataPortal.Fetch(Of VehicleList)()
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Function

  Public Shared Function GetVehicleListOdjaveniVozila(ByVal inPom As Integer) As VehicleList

    Return DataPortal.Fetch(Of VehicleList)(New FiltCriteria(inPom))

  End Function

    Public Shared Function GetVehicleById(ByVal inId As Integer) As VehicleInfo

        Return (DataPortal.Fetch(Of VehicleList)(New FiltCriteriaById(inId))).Item(0)

    End Function
  Public Shared Function EmptyList() As VehicleList

    Return New VehicleList

  End Function


  Public Function GetVehicleListById(ByVal idIn As Integer) As VehicleInfo
    For Each child As VehicleInfo In Me
      If child.Id = idIn Then
        Return child
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
Private Class FiltCriteria
    Private _idIn As Integer

    Public ReadOnly Property IdIn() As Integer
      Get
        Return _idIn
      End Get
    End Property

    Public Sub New(ByVal idIn As Long)
      _idIn = idIn
    End Sub
  End Class

    <Serializable()> _
  Private Class FiltCriteriaById
        Private _idIn As Long

        Public ReadOnly Property IdIn() As Long
            Get
                Return _idIn
            End Get
        End Property

        Public Sub New(ByVal idIn As Long)
            _idIn = idIn
        End Sub
    End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As FiltCriteria)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          If criteria.IdIn = 100 Then
            cm.CommandText = SpZemiVozilaZaBel
          Else
            cm.CommandText = SpZemiOdjaveniVozila
          End If

          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
    End Sub
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As FiltCriteriaById)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("VehicleInfo.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                 
                    cm.CommandText = SpZemiVehicleById
                    cm.Parameters.AddWithValue("@id", criteria.IdIn)

                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New VehicleInfo(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("VehicleInfo.DataPortal_Fetch", ex)
            Throw New DbCslaException("VehicleInfo.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
#End Region ' Data Access

End Class