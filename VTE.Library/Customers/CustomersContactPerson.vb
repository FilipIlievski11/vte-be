
<Serializable()> _
Public Class CustomersContactPerson
  Inherits Csla.BusinessBase(Of CustomersContactPerson)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCustomersContactPersonByID"
  Private Const spGetAll As String = "GetCustomersContactPersons"
  Private Const spUpdate As String = "updateCustomersContactPerson"
  Private Const spAdd As String = "addCustomersContactPerson"
  Private Const spDelete As String = "deleteCustomersContactPerson"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(CustomersContactPerson), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdCustomerProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(CustomersContactPerson), New PropertyInfo(Of Long)("IdCustomer"))
  Private Shared MbProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomersContactPerson), New PropertyInfo(Of String)("Mb"))
  Private Shared PersonNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomersContactPerson), New PropertyInfo(Of String)("PersonName"))
  Private Shared PersonSurnameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomersContactPerson), New PropertyInfo(Of String)("PersonSurname"))
  Private Shared PhoneNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomersContactPerson), New PropertyInfo(Of String)("PhoneNumber"))
  Private Shared MobileNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomersContactPerson), New PropertyInfo(Of String)("MobileNumber"))
  Private Shared EmailProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomersContactPerson), New PropertyInfo(Of String)("Email"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
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
  Public Property Mb() As String
    Get
      Return GetProperty(Of String)(MbProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(MbProperty, value)
    End Set
  End Property
  Public Property PersonName() As String
    Get
      Return GetProperty(Of String)(PersonNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(PersonNameProperty, value)
    End Set
  End Property
  Public Property PersonSurname() As String
    Get
      Return GetProperty(Of String)(PersonSurnameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(PersonSurnameProperty, value)
    End Set
  End Property
  Public Property PhoneNumber() As String
    Get
      Return GetProperty(Of String)(PhoneNumberProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(PhoneNumberProperty, value)
    End Set
  End Property
  Public Property MobileNumber() As String
    Get
      Return GetProperty(Of String)(MobileNumberProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(MobileNumberProperty, value)
    End Set
  End Property
  Public Property Email() As String
    Get
      Return GetProperty(Of String)(EmailProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(EmailProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
  
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(MbProperty, 13))
    ' PersonNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, PersonNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(PersonNameProperty, 50))
    ' PersonSurnameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, PersonSurnameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(PersonSurnameProperty, 50))
    ' PhoneNumberProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(PhoneNumberProperty, 20))
    ' MobileNumberProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(MobileNumberProperty, 20))
    ' EmailProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(EmailProperty, 50))
  End Sub
#End Region ' Validation Rules

#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCustomer") Then
      AuthorizationRules.AllowWrite("IdCustomer", roleName)
    Else
      AuthorizationRules.DenyWrite("IdCustomer", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdCustomer")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Mb") Then
      AuthorizationRules.AllowWrite("Mb", roleName)
    Else
      AuthorizationRules.DenyWrite("Mb", roleName)
    End If
    'AuthorizationRules.AllowWrite("Mb")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PersonName") Then
      AuthorizationRules.AllowWrite("PersonName", roleName)
    Else
      AuthorizationRules.DenyWrite("PersonName", roleName)
    End If
    'AuthorizationRules.AllowWrite("PersonName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PersonSurname") Then
      AuthorizationRules.AllowWrite("PersonSurname", roleName)
    Else
      AuthorizationRules.DenyWrite("PersonSurname", roleName)
    End If
    'AuthorizationRules.AllowWrite("PersonSurname")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PhoneNumber") Then
      AuthorizationRules.AllowWrite("PhoneNumber", roleName)
    Else
      AuthorizationRules.DenyWrite("PhoneNumber", roleName)
    End If
    'AuthorizationRules.AllowWrite("PhoneNumber")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("MobileNumber") Then
      AuthorizationRules.AllowWrite("MobileNumber", roleName)
    Else
      AuthorizationRules.DenyWrite("MobileNumber", roleName)
    End If
    'AuthorizationRules.AllowWrite("MobileNumber")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Email") Then
      AuthorizationRules.AllowWrite("Email", roleName)
    Else
      AuthorizationRules.DenyWrite("Email", roleName)
    End If
    'AuthorizationRules.AllowWrite("Email")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("CustomersContactPerson")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("CustomersContactPerson")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("CustomersContactPerson")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("CustomersContactPerson")
    End Function
#End Region ' Authorization Rules

#Region " Factory Methods "

    Friend Shared Function NewCustomersContactPersonChild() As CustomersContactPerson
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a CustomersContactPerson")
        'End If
        Return DataPortal.CreateChild(Of CustomersContactPerson)()
    End Function

    Friend Shared Function GetCustomersContactPerson(ByVal dr As SafeDataReader) As CustomersContactPerson
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a CustomersContactPerson")
        'End If
        Return DataPortal.FetchChild(Of CustomersContactPerson)(dr)
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
    Database.LogInfo("CustomersContactPerson.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Long)(IdCustomerProperty, dr.GetInt64("IdCustomer"))
      LoadProperty(Of String)(MbProperty, dr.GetString("MB"))
      LoadProperty(Of String)(PersonNameProperty, dr.GetString("PersonName"))
      LoadProperty(Of String)(PersonSurnameProperty, dr.GetString("PersonSurname"))
      LoadProperty(Of String)(PhoneNumberProperty, dr.GetString("PhoneNumber"))
      LoadProperty(Of String)(MobileNumberProperty, dr.GetString("MobileNumber"))
      LoadProperty(Of String)(EmailProperty, dr.GetString("Email"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("CustomersContactPerson.Child_Fetch", ex)
      Throw New DbCslaException("CustomersContactPerson.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@IdCustomer", (parent.Id))
            .Parameters.AddWithValue("@MB", ReadProperty(Of String)(MbProperty))
            .Parameters.AddWithValue("@PersonName", ReadProperty(Of String)(PersonNameProperty))
            .Parameters.AddWithValue("@PersonSurname", ReadProperty(Of String)(PersonSurnameProperty))
            .Parameters.AddWithValue("@PhoneNumber", ReadProperty(Of String)(PhoneNumberProperty))
            .Parameters.AddWithValue("@MobileNumber", ReadProperty(Of String)(MobileNumberProperty))
            .Parameters.AddWithValue("@Email", ReadProperty(Of String)(EmailProperty))

            Dim param As New SqlParameter("@newId", SqlDbType.Int)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)
            param = New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            LoadProperty(Of Integer)(IdProperty, CInt(.Parameters("@newId").Value))
            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
      End Using
    Catch ex As Exception
      Database.LogException("CustomersContactPerson.Child_Insert", ex)
      Throw New DbCslaException("CustomersContactPerson.Child_Insert", ex)
    Finally
      Database.LogInfo("CustomersContactPerson.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As Customer)
    Database.LogInfo("CustomersContactPerson.Child_Update", GetHashCode)
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

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
            .Parameters.AddWithValue("@MB", ReadProperty(Of String)(MbProperty))
            .Parameters.AddWithValue("@PersonName", ReadProperty(Of String)(PersonNameProperty))
            .Parameters.AddWithValue("@PersonSurname", ReadProperty(Of String)(PersonSurnameProperty))
            .Parameters.AddWithValue("@PhoneNumber", ReadProperty(Of String)(PhoneNumberProperty))
            .Parameters.AddWithValue("@MobileNumber", ReadProperty(Of String)(MobileNumberProperty))
            .Parameters.AddWithValue("@Email", ReadProperty(Of String)(EmailProperty))
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
      Database.LogException("CustomersContactPerson.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("CustomersContactPerson.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("CustomersContactPerson.Child_DeleteSelf", GetHashCode)
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
            .Parameters.AddWithValue("@id", ReadProperty(Of Integer)(IdProperty))
            .ExecuteNonQuery()
          End With
        End Using

      End Using
    Catch ex As Exception
      Database.LogException("CustomersContactPerson.Child_Fetch", ex)
      Throw New DbCslaException("CustomersContactPerson.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
