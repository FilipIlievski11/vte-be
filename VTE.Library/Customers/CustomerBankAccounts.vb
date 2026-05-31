
<Serializable()> _
Public Class CustomerBankAccounts
  Inherits Csla.BusinessListBase(Of CustomerBankAccounts, CustomerBankAccount)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As CustomerBankAccount = CustomerBankAccount.NewCustomerBankAccountChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Factory Methods "

  Friend Shared Function NewCustomerBankAccounts() As CustomerBankAccounts
    Return DataPortal.CreateChild(Of CustomerBankAccounts)()
  End Function

  Friend Shared Function GetCustomerBankAccounts(ByVal dr As SafeDataReader) As CustomerBankAccounts
    Return DataPortal.FetchChild(Of CustomerBankAccounts)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("CustomerBankAccounts.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(CustomerBankAccount.GetCustomerBankAccount(dr))
      End While
    Catch ex As Exception
      Database.LogException("CustomerBankAccounts.Child_Fetch", ex)
      Throw New DbCslaException("CustomerBankAccounts.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
