

<Serializable()> _
Public Class VehicleListShort
    Inherits ReadOnlyListBase(Of VehicleListShort, VehicleInfoShort)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "getVehiclesListShort"
    Private Const SpZemiPoShellReg As String = "getVehiclesListShortByString"
    ' Private Const SpZemiOdjaveniVozila As String = "getVehiclesListOdjaveniVozila"
    'Private Const SpZemiVozilaZaBel As String = "getVehiclesListVozilaZaBel"
#End Region

#Region " Factory Methods "

    Public Shared Function GetVehicleListShort() As VehicleListShort
        Try
            Return DataPortal.Fetch(Of VehicleListShort)()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    'Public Shared Function GetVehicleListOdjaveniVozila(ByVal inPom As Integer) As VehicleList

    '    Return DataPortal.Fetch(Of VehicleList)(New FiltCriteria(inPom))

    'End Function

    Public Shared Function EmptyList() As VehicleListShort

        Return New VehicleListShort

    End Function
    Public Shared Function GetVehicleByShellOrReg(ByVal num As String) As VehicleListShort
        Return (DataPortal.Fetch(New filterCriteria(num)))
    End Function


    Public Function GetVehicleListShortById(ByVal idIn As Integer) As VehicleInfoShort
        For Each child As VehicleInfoShort In Me
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
Private Class filterCriteria
        Private _inNum As String

        Public ReadOnly Property InNum() As String
            Get
                Return _inNum
            End Get
        End Property

        Public Sub New(ByVal inNum As String)
            _inNum = inNum
        End Sub
    End Class
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
                            Dim Info As New VehicleInfoShort(dr)
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
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As filterCriteria)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("VehicleInfo.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = SpZemiPoShellReg
                    cm.Parameters.AddWithValue("@num", criteria.InNum)
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New VehicleInfoShort(dr)
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
