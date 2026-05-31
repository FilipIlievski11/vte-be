
<Serializable()> _
Public Class CustomerFinancialStateTempList
    Inherits ReadOnlyListBase(Of CustomerFinancialStateTempList, CustomerFinancialStateTempInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "getCustomerFinancialStateTemp"
    Private Const SpZemiSiteZaVozilo As String = "getCustomerFinancialStateTempByVehicle"
#End Region

#Region " Factory Methods "

    Public Shared Function GetCustomerFinancialStateTempList() As CustomerFinancialStateTempList

        Return DataPortal.Fetch(Of CustomerFinancialStateTempList)(New Criteria)

    End Function
    Public Shared Function GetCustomerFinancialStateTempListByVehicle(ByVal inIdV As Long) As CustomerFinancialStateTempList

        Return DataPortal.Fetch(Of CustomerFinancialStateTempList)(New CriteriaVehicle(inIdV))

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
  Private Class CriteriaVehicle
        ' no criteria - retrieve all projects
        Private _inIdV As Long

        Public ReadOnly Property InIdV() As Long
            Get
                Return _inIdV
            End Get
        End Property

        Public Sub New(ByVal inIdV As Long)
            _inIdV = inIdV
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
                        Me.Add(CustomerFinancialStateTempInfo.GetCustomerFinancialStateTempInfo(dr))
                    End While
                End Using
            End Using
        End Using
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaVehicle)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Using cn As SqlConnection = Database.VTE_SqlConnection

            Using cm As SqlCommand = cn.CreateCommand()
                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = SpZemiSiteZaVozilo
                cm.Parameters.AddWithValue("@idVehicle", criteria.InIdV)
                Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                    While dr.Read()
                        Me.Add(CustomerFinancialStateTempInfo.GetCustomerFinancialStateTempInfo(dr))
                    End While
                End Using
            End Using
        End Using
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access
End Class