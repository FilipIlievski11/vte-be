
<Serializable()> _
Public Class Customers
  Inherits Csla.BusinessListBase(Of Customers, Customer)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCustomerByID"
  Private Const spGetAll As String = "GetCustomers"
  Private Const spUpdate As String = "updateCustomer"
  Private Const spAdd As String = "addCustomer"
  Private Const spDelete As String = "deleteCustomer"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As Customer = Customer.NewCustomerChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Customers")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Customers")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Customers")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Customers")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

    Public Shared Function GetCustomers(ByVal idLivingCity As Integer, ByVal idBirhCity As Integer, ByVal idBusinessType As Integer) As Customers
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a Customer")
        End If
        Return DataPortal.Fetch(Of Customers)()
    End Function

  Public Overrides Function Save() As Customers
    Dim result As Customers = MyBase.Save()

        'OnCustomersSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("Customers.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(Customer.GetCustomer(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("Customers.Child_Fetch", ex)
      Throw New DbCslaException("Customers.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


  Protected Overrides Sub DataPortal_Update()
    RaiseListChangedEvents = False
    Child_Update()
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access

    '#Region " Readonlylist refresh "
    '  Public Shared Event CustomersSaved As EventHandler(Of Csla.Core.SavedEventArgs)
    '  Protected Shared Sub OnCustomersSaved(ByVal sender As Customers, ByVal e As Csla.Core.SavedEventArgs)
    '    RaiseEvent CustomersSaved(sender, e)
    '  End Sub
    '#End Region

End Class
