
<Serializable()> _
Public Class VehicleCustomerForNewTechReportList
    Inherits ReadOnlyListBase(Of VehicleCustomerForNewTechReportList, VehicleCustomerForNewTechReportInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "getCustomerVehicleDataForNewTechReport"
#End Region

#Region " Factory Methods "

    Public Shared Function GetVehicleCustomerForNewTechReportList() As VehicleCustomerForNewTechReportList

        Return DataPortal.Fetch(Of VehicleCustomerForNewTechReportList)(New Criteria)

    End Function

    Public Shared Function GetVehicleCustomerForNewTechReportByIdRelation(ByVal inIdRelation As Long) As VehicleCustomerForNewTechReportInfo

        Return DataPortal.Fetch(Of VehicleCustomerForNewTechReportList)(New FilterCriteria(inIdRelation)).Item(0)

    End Function

    Private Sub New()
        ' require use of factory methods
    End Sub

#End Region ' Factory Methods

#Region " Data Access "

    <Serializable()> _
    Private Class Criteria
        ' no criteria - retrieve all projects
    End Class
    <Serializable()> _
Private Class FilterCriteria
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
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Using cn As SqlConnection = Database.VTE_SqlConnection
            cn.Open()
            Using cm As SqlCommand = cn.CreateCommand()
                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = SpZemiSite
                Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                    While dr.Read()
                        Me.Add(VehicleCustomerForNewTechReportInfo.GetVehicleCustomerForNewTechReportInfo(dr))
                    End While
                End Using
            End Using
        End Using
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As FilterCriteria)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Using cn As SqlConnection = Database.VTE_SqlConnection

            Using cm As SqlCommand = cn.CreateCommand()
                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = "getCustomerVehicleDataForNewTechReportByIdRelation"
                cm.Parameters.AddWithValue("@idRelation", criteria.InId)
                Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                    While dr.Read()
                        Me.Add(VehicleCustomerForNewTechReportInfo.GetVehicleCustomerForNewTechReportInfo(dr))
                    End While
                End Using
            End Using
        End Using
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
#End Region ' Data Access
End Class