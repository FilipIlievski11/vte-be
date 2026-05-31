
<Serializable()> _
Public Class PrintCustomerByIdList
  Inherits ReadOnlyListBase(Of PrintCustomerByIdList, PrintCustomerByIdInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "PrintCustomerById"
#End Region

#Region " Factory Methods "

  Public Shared Function GetPrintCustomerByIdList(ByVal InIdCustomer As Long) As PrintCustomerByIdList

    Return DataPortal.Fetch(Of PrintCustomerByIdList)(New Criteria(InIdCustomer))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  <Serializable()> _
Private Class Criteria

    Private _InIdCustomer As Long

    Public ReadOnly Property InIdCustomer() As Long
      Get
        Return _InIdCustomer
      End Get
    End Property


    Public Sub New(ByVal InIdCustomer As Long)
      _InIdCustomer = InIdCustomer
    End Sub

  End Class
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("PrintCustomerByIdInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          cm.Parameters.AddWithValue("@id", criteria.InIdCustomer)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New PrintCustomerByIdInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("PrintCustomerByIdInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("PrintCustomerByIdInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class