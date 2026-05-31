
<Serializable()> _
Public Class CustomerBankAccount
  Inherits Csla.BusinessBase(Of CustomerBankAccount)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCustomersBankAccountByID"
  Private Const spGetAll As String = "GetCustomersBankAccounts"
  Private Const spUpdate As String = "updateCustomersBankAccount"
  Private Const spAdd As String = "addCustomersBankAccount"
  Private Const spDelete As String = "deleteCustomersBankAccount"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(CustomerBankAccount), New PropertyInfo(Of Long)("Id"))
  Private Shared IdCustomerProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(CustomerBankAccount), New PropertyInfo(Of Long)("IdCustomer"))
  Private Shared BankAccountProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomerBankAccount), New PropertyInfo(Of String)("BankAccount"))
  Private Shared DeponentBankProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomerBankAccount), New PropertyInfo(Of String)("DeponentBank"))
  Private Shared TaxNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomerBankAccount), New PropertyInfo(Of String)("TaxNumber"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Long
    Get
      Return GetProperty(Of Long)(IdProperty)
    End Get
  End Property
  Public Property IdCustomer() As Long
    Get
      Return GetProperty(Of Long)(IdCustomerProperty)
    End Get
    Set(ByVal value As Long)
      SetProperty(Of Long)(IdCustomerProperty, value)
    End Set
  End Property
  Public Property BankAccount() As String
    Get
      Return GetProperty(Of String)(BankAccountProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(BankAccountProperty, value)
    End Set
  End Property
  Public Property DeponentBank() As String
    Get
      Return GetProperty(Of String)(DeponentBankProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(DeponentBankProperty, value)
    End Set
  End Property
  Public Property TaxNumber() As String
    Get
      Return GetProperty(Of String)(TaxNumberProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(TaxNumberProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' BankAccountProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, BankAccountProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BankAccountProperty, 50))
    ' DeponentBankProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, DeponentBankProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(DeponentBankProperty, 50))
    ' TaxNumberProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(TaxNumberProperty, 15))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewCustomerBankAccountChild() As CustomerBankAccount
    Return DataPortal.CreateChild(Of CustomerBankAccount)()
  End Function

  Friend Shared Function GetCustomerBankAccount(ByVal dr As SafeDataReader) As CustomerBankAccount
    Return DataPortal.FetchChild(Of CustomerBankAccount)(dr)
  End Function

  Private Sub New()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Create "
  Protected Overloads Sub Child_Create()
  End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("CustomerBankAccount.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Long)(IdCustomerProperty, dr.GetInt64("IdCustomer"))
      LoadProperty(Of String)(BankAccountProperty, dr.GetString("BankAccount"))
      LoadProperty(Of String)(DeponentBankProperty, dr.GetString("DeponentBank"))
      LoadProperty(Of String)(TaxNumberProperty, dr.GetString("TaxNumber"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("CustomerBankAccount.Child_Fetch", ex)
      Throw New DbCslaException("CustomerBankAccount.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As Customer)
    Try
      Using cn As SqlConnection = ApplicationContext.LocalContext("cn")
        If cn.State = ConnectionState.Closed Then
          cn.ConnectionString = Database.VTEConnection
          cn.Open()
        End If
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spAdd
            'Smeni go Id so Parent.Id
            .Parameters.AddWithValue("@IdCustomer", parent.Id)
            .Parameters.AddWithValue("@BankAccount", ReadProperty(Of String)(BankAccountProperty))
            .Parameters.AddWithValue("@DeponentBank", ReadProperty(Of String)(DeponentBankProperty))
            .Parameters.AddWithValue("@TaxNumber", ReadProperty(Of String)(TaxNumberProperty))

            Dim param As New SqlParameter("@newId", SqlDbType.Int)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)
            param = New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            LoadProperty(Of Long)(IdProperty, CInt(.Parameters("@newId").Value))
            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
      End Using
    Catch ex As Exception
      Database.LogException("CustomerBankAccount.Child_Insert", ex)
      Throw New DbCslaException("CustomerBankAccount.Child_Insert", ex)
    Finally
      Database.LogInfo("CustomerBankAccount.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As Customer)
    Database.LogInfo("CustomerBankAccount.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = ApplicationContext.LocalContext("cn")
        If cn.State = ConnectionState.Closed Then
          cn.ConnectionString = Database.VTEConnection
          cn.Open()
        End If
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
            .Parameters.AddWithValue("@IdCustomer", parent.Id)
            .Parameters.AddWithValue("@BankAccount", ReadProperty(Of String)(BankAccountProperty))
            .Parameters.AddWithValue("@DeponentBank", ReadProperty(Of String)(DeponentBankProperty))
            .Parameters.AddWithValue("@TaxNumber", ReadProperty(Of String)(TaxNumberProperty))
            .Parameters.AddWithValue("@lastChanged", _lastChanged)
            Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
      End Using
    Catch ex As Exception
      Database.LogException("CustomerBankAccount.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("CustomerBankAccount.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("CustomerBankAccount.Child_DeleteSelf", GetHashCode)
    Try
      Using cn As SqlConnection = Csla.ApplicationContext.LocalContext("cn")
        If cn.State = ConnectionState.Closed Then
          cn.ConnectionString = Database.VTEConnection
          cn.Open()
        End If
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spDelete
            .Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
            .ExecuteNonQuery()
          End With
        End Using

      End Using
    Catch ex As Exception
      Database.LogException("CustomerBankAccount.Child_Fetch", ex)
      Throw New DbCslaException("CustomerBankAccount.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
