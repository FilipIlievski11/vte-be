
<Serializable()> _
Public Class PrintDocumentPermisionForCustomerList
  Inherits ReadOnlyListBase(Of PrintDocumentPermisionForCustomerList, PrintDocumentPermisionForCustomerInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "PrintDocumentPermisionForCustomer"
  Private Const SpZemiPoId As String = "PrintDocumentPermisionForCustomerByIdDocumentPermision"
#End Region

#Region " Factory Methods "

  Public Shared Function GetPrintDocumentPermisionForCustomerList() As PrintDocumentPermisionForCustomerList

    Return DataPortal.Fetch(Of PrintDocumentPermisionForCustomerList)()

  End Function

  Public Shared Function GetPrintDocumentPermisionForCustomerList(ByVal idIn As Long) As PrintDocumentPermisionForCustomerList

    Return DataPortal.Fetch(Of PrintDocumentPermisionForCustomerList)(New CriteriaById(idIn))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  <Serializable()> _
Private Class CriteriaById

    Private _InId As Long

    Public ReadOnly Property InId() As Long
      Get
        Return _InId
      End Get
    End Property


    Public Sub New(ByVal InId As Long)
      _InId = InId
    End Sub

  End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("PrintDocumentPermisionForCustomerInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New PrintDocumentPermisionForCustomerInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("PrintDocumentPermisionForCustomerInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("PrintDocumentPermisionForCustomerInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaById)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("PrintDocumentPermisionForCustomerInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiPoId
          cm.Parameters.AddWithValue("@Id", criteria.InId)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New PrintDocumentPermisionForCustomerInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("PrintDocumentPermisionForCustomerInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("PrintDocumentPermisionForCustomerInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
#End Region ' Data Access
End Class