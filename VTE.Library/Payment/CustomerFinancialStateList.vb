
<Serializable()> _
Public Class CustomerFinancialStateList
  Inherits ReadOnlyListBase(Of CustomerFinancialStateList, CustomerFinancialStateInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getCustomerFinancialStateList"
  Private Const SpZemiSitePoIdCustomerVehicleRelation As String = "getCustomerFinancialStateByIdCustomerVehicleRelation"
#End Region

#Region " Factory Methods "

  Public Shared Function GetCustomerFinancialStateList() As CustomerFinancialStateList

    Return DataPortal.Fetch(Of CustomerFinancialStateList)()

  End Function

  Public Shared Function GetCustomerFinancialStateListByIdCustomerVehicleRelation(ByVal InIdRelation As Long) As CustomerFinancialStateList

    Return DataPortal.Fetch(Of CustomerFinancialStateList)(New CriteriaByIdRelation(InIdRelation))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "
  <Serializable()> _
Private Class CriteriaByIdRelation
    Private _InIdRelation As Long

    Public ReadOnly Property InIdRelation() As Long
      Get
        Return _InIdRelation
      End Get
    End Property

    Public Sub New(ByVal InIdRelation As Long)
      _InIdRelation = InIdRelation
    End Sub

  End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CustomerFinancialStateInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CustomerFinancialStateInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CustomerFinancialStateInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CustomerFinancialStateInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByIdRelation)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CustomerFinancialStateInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSitePoIdCustomerVehicleRelation
     cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", criteria.InIdRelation)
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CustomerFinancialStateInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CustomerFinancialStateInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CustomerFinancialStateInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
#End Region ' Data Access
End Class