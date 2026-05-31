
<Serializable()> _
Public Class PrintCustomerList
  Inherits ReadOnlyListBase(Of PrintCustomerList, PrintCustomerInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "PrintCustomer"
#End Region

#Region " Factory Methods "

  Public Shared Function EmptyList() As PrintCustomerList
    Return New PrintCustomerList
  End Function

  Public Shared Function GetPrintCustomerList(ByVal idCustomer As Long) As PrintCustomerList

    Return DataPortal.Fetch(Of PrintCustomerList)(New SingleCriteria(Of PrintCustomerList, Long)(idCustomer))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of PrintCustomerList, Long))
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("PrintCustomerInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          cm.Parameters.AddWithValue("@id", criteria.Value)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New PrintCustomerInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("PrintCustomerInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("PrintCustomerInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class