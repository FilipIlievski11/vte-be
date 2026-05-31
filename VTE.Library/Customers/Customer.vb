
<Serializable()> _
Public Class Customer
  Inherits Csla.BusinessBase(Of Customer)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCustomerByID"
  Private Const spGetAll As String = "GetCustomers"
  Private Const spUpdate As String = "updateCustomer"
  Private Const spAdd As String = "addCustomer"
  Private Const spDelete As String = "deleteCustomer"
  Private Const spGetChildrenContacts As String = "getCustomersContactPersonByIdCustomer"
  Private Const spGetChildrenBankAccounts As String = "getCustomersBankAccountByIdCustomer"
  Private Const spGetByMB As String = "GetCustomerByMB"
#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Customer), New PropertyInfo(Of Long)("Id"))
  Private Shared MbProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("Mb"))
  Private Shared CustomerSurnameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("CustomerSurname"))
  Private Shared CustomerFirstNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("CustomerFirstName"))
  Private Shared PhoneNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("PhoneNumber"))
  Private Shared FaxProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("Fax"))
  Private Shared IdLivingAddressProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Customer), New PropertyInfo(Of Integer)("IdLivingAddress"))
  Private Shared LivingAddressNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("LivingAddressNumber"))
  Private Shared IdLivingCityProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Customer), New PropertyInfo(Of Integer)("IdLivingCity"))
  Private Shared IdBirhCityProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Customer), New PropertyInfo(Of Integer)("IdBirhCity"))
  Private Shared IdBirthAddressProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Customer), New PropertyInfo(Of Integer)("IdBirthAddress"))
  Private Shared BrithAddressNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("BrithAddressNumber"))
  Private Shared DateOfBirthProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(Customer), New PropertyInfo(Of SmartDate)("DateOfBirth", "DateOfBirth", New SmartDate(DateTime.MinValue, True)))
  Private Shared IdCitizenshipProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Customer), New PropertyInfo(Of Integer)("IdCitizenship"))

  Private Shared IsCompanyProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(Customer), New PropertyInfo(Of Boolean)("IsCompany"))
  Private Shared OccupationProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("Occupation"))
  Private Shared WorksInCompanyProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("WorksInCompany"))
  Private Shared IdBusinessTypeProperty As PropertyInfo(Of Integer) = _
    RegisterProperty(Of Integer)(GetType(Customer), New PropertyInfo(Of Integer)("IdBusinessType"))
  Private Shared EmailProperty As PropertyInfo(Of String) = _
    RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("Email", "Email", String.Empty))
  Private Shared PassportNumberProperty As PropertyInfo(Of String) = _
    RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("PassportNumber"))
  Private Shared BLKProperty As PropertyInfo(Of String) = _
    RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("BLK"))
  Private Shared CanSendNotificationsProperty As PropertyInfo(Of Boolean) = _
    RegisterProperty(Of Boolean)(GetType(Customer), New PropertyInfo(Of Boolean)("CanSendNotifications", "CanSendNotifications", False))
  Private Shared TaxNumberProperty As PropertyInfo(Of String) = _
 RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("TaxNumber"))
  Private Shared NoteProperty As PropertyInfo(Of String) = _
     RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("Note"))
  Private Shared StatusProperty As PropertyInfo(Of String) = _
     RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("Status"))

  Private Shared ParentNameProperty As PropertyInfo(Of String) = _
  RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("ParentName"))
  Private Shared BLKDateIssuedProperty As PropertyInfo(Of SmartDate) = _
  RegisterProperty(Of SmartDate)(GetType(Customer), New PropertyInfo(Of SmartDate)("BLKDateIssued", "BLKDateIssued", New SmartDate(DateTime.MinValue, True)))
  Private Shared BLKIssuerProperty As PropertyInfo(Of Integer) = _
  RegisterProperty(Of Integer)(GetType(Customer), New PropertyInfo(Of Integer)("BLKIssuer"))
  Private Shared DriveingLicenceNumberProperty As PropertyInfo(Of String) = _
  RegisterProperty(Of String)(GetType(Customer), New PropertyInfo(Of String)("DriveingLicenceNumber"))
  Private Shared DriveingLicenceDateIssuedProperty As PropertyInfo(Of SmartDate) = _
RegisterProperty(Of SmartDate)(GetType(Customer), New PropertyInfo(Of SmartDate)("DriveingLicenceDateIssued", "DriveingLicenceDateIssued", New SmartDate(DateTime.MinValue, True)))
  Private Shared DriveingLicenceIssuerProperty As PropertyInfo(Of Integer) = _
  RegisterProperty(Of Integer)(GetType(Customer), New PropertyInfo(Of Integer)("DriveingLicenceIssuer"))
  Private Shared PassDateIssuedProperty As PropertyInfo(Of SmartDate) = _
RegisterProperty(Of SmartDate)(GetType(Customer), New PropertyInfo(Of SmartDate)("PassDateIssued", "PassDateIssued", New SmartDate(DateTime.MinValue, True)))
  Private Shared PassIssuerProperty As PropertyInfo(Of Integer) = _
  RegisterProperty(Of Integer)(GetType(Customer), New PropertyInfo(Of Integer)("PassIssuer"))


  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Long
    Get
      Return GetProperty(Of Long)(IdProperty)
    End Get
  End Property

  Public Property CanSendNotifications() As Boolean
    Get
      Return GetProperty(Of Boolean)(CanSendNotificationsProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(CanSendNotificationsProperty, value)
    End Set
  End Property

  Public Property BLK() As String
    Get
      Return GetProperty(Of String)(BLKProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(BLKProperty, value)
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

  Public Property Email() As String
    Get
      Return GetProperty(Of String)(EmailProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(EmailProperty, value)
    End Set
  End Property

  Public Property PassportNumber() As String
    Get
      Return GetProperty(Of String)(PassportNumberProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(PassportNumberProperty, value)
    End Set
  End Property

  Public Property Mb() As String
    Get
      Return GetProperty(Of String)(MbProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(MbProperty, value)
      Try
        Dim birthDate As New Date(IIf(Mb.Substring(4, 3) > 100, "1", "2") + Mb.Substring(4, 3), Mb.Substring(2, 2), Mb.Substring(0, 2))
        DateOfBirth = birthDate
      Catch ex As Exception
        MsgBox(ex.Message())
      End Try
    End Set
  End Property
  Public Property CustomerSurname() As String
    Get
      Return GetProperty(Of String)(CustomerSurnameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CustomerSurnameProperty, value)
    End Set
  End Property
  Public Property CustomerFirstName() As String
    Get
      Return GetProperty(Of String)(CustomerFirstNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CustomerFirstNameProperty, value)
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
  Public Property Fax() As String
    Get
      Return GetProperty(Of String)(FaxProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(FaxProperty, value)
    End Set
  End Property

  Public Property IdLivingAddress() As Integer
    Get
      Return GetProperty(Of Integer)(IdLivingAddressProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdLivingAddressProperty, value)
    End Set
  End Property
  Public Property LivingAddressNumber() As String
    Get
      Return GetProperty(Of String)(LivingAddressNumberProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(LivingAddressNumberProperty, value)
    End Set
  End Property
  Public Property IdLivingCity() As Integer
    Get
      Return GetProperty(Of Integer)(IdLivingCityProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdLivingCityProperty, value)
    End Set
  End Property
  Public Property IdBirhCity() As Integer
    Get
      Return GetProperty(Of Integer)(IdBirhCityProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdBirhCityProperty, value)
    End Set
  End Property
  Public Property IdBirthAddress() As Integer
    Get
      Return GetProperty(Of Integer)(IdBirthAddressProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdBirthAddressProperty, value)
    End Set
  End Property
  Public Property IdCitizenship() As Integer
    Get
      Return GetProperty(Of Integer)(IdCitizenshipProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdCitizenshipProperty, value)
    End Set
  End Property

  Public Property BrithAddressNumber() As String
    Get
      Return GetProperty(Of String)(BrithAddressNumberProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(BrithAddressNumberProperty, value)
    End Set
  End Property
  Public Property DateOfBirth() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(DateOfBirthProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(DateOfBirthProperty, value)
    End Set
  End Property


  Public Property IsCompany() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsCompanyProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsCompanyProperty, value)
    End Set
  End Property

  Public Property Occupation() As String
    Get
      Return GetProperty(Of String)(OccupationProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(OccupationProperty, value)
    End Set
  End Property
  Public Property WorksInCompany() As String
    Get
      Return GetProperty(Of String)(WorksInCompanyProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(WorksInCompanyProperty, value)
    End Set
  End Property
  Public Property IdBusinessType() As Integer
    Get
      Return GetProperty(Of Integer)(IdBusinessTypeProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdBusinessTypeProperty, value)
    End Set
  End Property

  Private Shared ContactPersonsProperty As PropertyInfo(Of CustomersContactPersons) = _
RegisterProperty(Of CustomersContactPersons)(GetType(Customer), New PropertyInfo(Of CustomersContactPersons)("contactPersons"))

  Public ReadOnly Property ContactPersons() As CustomersContactPersons
    Get
      If Not FieldManager.FieldExists(ContactPersonsProperty) Then
        SetProperty(Of CustomersContactPersons) _
        (ContactPersonsProperty, CustomersContactPersons.NewCustomersContactPersons)
      End If
      Return GetProperty(Of CustomersContactPersons)(ContactPersonsProperty)
    End Get
  End Property

  Private Shared BankAccountsProperty As PropertyInfo(Of CustomerBankAccounts) = _
RegisterProperty(Of CustomerBankAccounts)(GetType(Customer), New PropertyInfo(Of CustomerBankAccounts)("BankAccounts"))

  Public ReadOnly Property BankAccounts() As CustomerBankAccounts
    Get
      If Not FieldManager.FieldExists(BankAccountsProperty) Then
        SetProperty(Of CustomerBankAccounts) _
        (BankAccountsProperty, CustomerBankAccounts.NewCustomerBankAccounts)
      End If
      Return GetProperty(Of CustomerBankAccounts)(BankAccountsProperty)
    End Get
  End Property
  Public Property Note() As String
    Get
      Return GetProperty(Of String)(NoteProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(NoteProperty, value)
    End Set
  End Property
  Public Property Status() As String
    Get
      Return GetProperty(Of String)(StatusProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(StatusProperty, value)
    End Set
  End Property

  Public Property ParentName() As String
    Get
      Return GetProperty(Of String)(ParentNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(ParentNameProperty, value)
    End Set
  End Property
  Public Property BLKDateIssued() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(BLKDateIssuedProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(BLKDateIssuedProperty, value)
    End Set
  End Property
  Public Property BLKIssuer() As Integer
    Get
      Return GetProperty(Of Integer)(BLKIssuerProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(BLKIssuerProperty, value)
    End Set
  End Property
  Public Property DriveingLicenceNumber() As String
    Get
      Return GetProperty(Of String)(DriveingLicenceNumberProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(DriveingLicenceNumberProperty, value)
    End Set
  End Property
  Public Property DriveingLicenceDateIssued() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(DriveingLicenceDateIssuedProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(DriveingLicenceDateIssuedProperty, value)
    End Set
  End Property
  Public Property DriveingLicenceIssuer() As Integer
    Get
      Return GetProperty(Of Integer)(DriveingLicenceIssuerProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(DriveingLicenceIssuerProperty, value)
    End Set
  End Property
  Public Property PassDateIssued() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(PassDateIssuedProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(PassDateIssuedProperty, value)
    End Set
  End Property
  Public Property PassIssuer() As Integer
    Get
      Return GetProperty(Of Integer)(PassIssuerProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(PassIssuerProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function

#End Region 'Business Properties and Methods

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' MbProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, MbProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(MbProperty, 13))
    ' CustomerSurnameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CustomerSurnameProperty, 100))
    ' CustomerFirstNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CustomerFirstNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CustomerFirstNameProperty, 100))
    ' PhoneNumberProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(PhoneNumberProperty, 20))
    ' FaxProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(FaxProperty, 20))
    ' LivingAddressNumberProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(LivingAddressNumberProperty, 100))
    ' BrithAddressNumberProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BrithAddressNumberProperty, 100))
    ' OccupationProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(OccupationProperty, 50))

    'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, New Validation.IntegerMinValueRuleArgs(IdLivingCityProperty, 1))

    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, LivingAddressNumberProperty)
    ' ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    '                       New Validation.IntegerMinValueRuleArgs(IdLivingAddressProperty, 1))
    'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    '                       New Validation.IntegerMinValueRuleArgs(IdBirhCityProperty, 1))
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, DateOfBirthProperty)
    'ValidationRules.AddRule(Of Customer)(AddressOf CorrectMB, MbProperty)
    ValidationRules.AddRule(Of Customer)(AddressOf CheckEmail, EmailProperty)
    'ValidationRules.AddRule(Of Customer)(AddressOf NoDuplicates, MbProperty)

    ValidationRules.AddRule(Of Customer)(AddressOf EmailRequired, CanSendNotificationsProperty)
    ValidationRules.AddRule(Of Customer)(AddressOf EmailRequired, EmailProperty)
    ValidationRules.AddDependentProperty(EmailProperty, CanSendNotificationsProperty, True)
  End Sub

  Private Shared Function CheckEmail(Of T As Customer)( _
  ByVal target As T, ByVal e As Csla.Validation.RuleArgs) As Boolean

    If String.IsNullOrEmpty(target.Email) Then
      Return True
    Else
      Dim emailRule As New Csla.Validation.RuleHandler( _
        AddressOf Csla.Validation.CommonRules.RegExMatch)
      Dim args As New Csla.Validation.CommonRules.RegExRuleArgs( _
         e.PropertyName, Validation.CommonRules.RegExPatterns.Email)
      Dim result As Boolean = emailRule.Invoke(target, args)
      If Not result Then
        e.Description = args.Description
        e.Severity = args.Severity
        e.StopProcessing = False  'args.StopProcessing
      End If
      Return result
    End If

  End Function

  Private Shared Function EmailRequired(Of T As Customer)(ByVal target As T, _
    ByVal e As Csla.Validation.RuleArgs) As Boolean
    If target.CanSendNotifications Then
      If (target.Email = String.Empty) Or (target.Email = "") Then
        e.Description = "Потребно е да се внесе Email"
        Return False
      End If
    End If
    Return True
  End Function

  'Private Shared Function NoDuplicates(Of T As Customer)(ByVal target As T, _
  '  ByVal e As Csla.Validation.RuleArgs) As Boolean
  '    If target.Mb <> "" AndAlso target.Mb <> String.Empty Then


  '        If Customer.MBExists(target.Mb, target.Id) Then
  '            e.Description = "Матичниот број мора да биде единствен"
  '            Return False
  '        Else
  '            Return True
  '        End If
  '    Else
  '        Return True
  '    End If
  'End Function
  Private Shared Function CorrectMB(Of T As Customer)(ByVal target As T, _
  ByVal e As Csla.Validation.RuleArgs) As Boolean
    If Not target.IsCompany Then
      If target.Mb <> "" AndAlso target.Mb <> String.Empty Then


        If Not MaticenBroj.CheckMaticenBroj(target.Mb) Then
          e.Description = My.Resources.MBNoDuplicates
          Return False
        Else
          Return True
        End If
      Else
        Return True
      End If
    Else
      Return True
    End If
  End Function
#End Region ' Validation Rules

#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Mb") Then
      AuthorizationRules.AllowWrite("Mb", roleName)
    Else
      AuthorizationRules.DenyWrite("Mb", roleName)
    End If
    'AuthorizationRules.AllowWrite("Mb")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CustomerSurname") Then
      AuthorizationRules.AllowWrite("CustomerSurname", roleName)
    Else
      AuthorizationRules.DenyWrite("CustomerSurname", roleName)
    End If
    'AuthorizationRules.AllowWrite("CustomerSurname")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CustomerFirstName") Then
      AuthorizationRules.AllowWrite("CustomerFirstName", roleName)
    Else
      AuthorizationRules.DenyWrite("CustomerFirstName", roleName)
    End If
    'AuthorizationRules.AllowWrite("CustomerFirstName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PhoneNumber") Then
      AuthorizationRules.AllowWrite("PhoneNumber", roleName)
    Else
      AuthorizationRules.DenyWrite("PhoneNumber", roleName)
    End If
    'AuthorizationRules.AllowWrite("PhoneNumber")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Fax") Then
      AuthorizationRules.AllowWrite("Fax", roleName)
    Else
      AuthorizationRules.DenyWrite("Fax", roleName)
    End If
    'AuthorizationRules.AllowWrite("Fax")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DeponentBank") Then
      AuthorizationRules.AllowWrite("DeponentBank", roleName)
    Else
      AuthorizationRules.DenyWrite("DeponentBank", roleName)
    End If
    'AuthorizationRules.AllowWrite("DeponentBank")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TaxNumber") Then
      AuthorizationRules.AllowWrite("TaxNumber", roleName)
    Else
      AuthorizationRules.DenyWrite("TaxNumber", roleName)
    End If
    'AuthorizationRules.AllowWrite("TaxNumber")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("BankAccount") Then
      AuthorizationRules.AllowWrite("BankAccount", roleName)
    Else
      AuthorizationRules.DenyWrite("BankAccount", roleName)
    End If
    'AuthorizationRules.AllowWrite("BankAccount")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdLivingAddress") Then
      AuthorizationRules.AllowWrite("IdLivingAddress", roleName)
    Else
      AuthorizationRules.DenyWrite("IdLivingAddress", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdLivingAddress")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("LivingAddressNumber") Then
      AuthorizationRules.AllowWrite("LivingAddressNumber", roleName)
    Else
      AuthorizationRules.DenyWrite("LivingAddressNumber", roleName)
    End If
    'AuthorizationRules.AllowWrite("LivingAddressNumber")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdLivingCity") Then
      AuthorizationRules.AllowWrite("IdLivingCity", roleName)
    Else
      AuthorizationRules.DenyWrite("IdLivingCity", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdLivingCity")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdBirhCity") Then
      AuthorizationRules.AllowWrite("IdBirhCity", roleName)
    Else
      AuthorizationRules.DenyWrite("IdBirhCity", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdBirhCity")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdBirthAddress") Then
      AuthorizationRules.AllowWrite("IdBirthAddress", roleName)
    Else
      AuthorizationRules.DenyWrite("IdBirthAddress", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdBirthAddress")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("BrithAddressNumber") Then
      AuthorizationRules.AllowWrite("BrithAddressNumber", roleName)
    Else
      AuthorizationRules.DenyWrite("BrithAddressNumber", roleName)
    End If

    'AuthorizationRules.AllowWrite("BrithAddressNumber")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCitizenship") Then
      AuthorizationRules.AllowWrite("IdCitizenship", roleName)
    Else
      AuthorizationRules.DenyWrite("IdCitizenship", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdCitizenship")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsCompany") Then
      AuthorizationRules.AllowWrite("IsCompany", roleName)
    Else
      AuthorizationRules.DenyWrite("IsCompany", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsCompany")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Occupation") Then
      AuthorizationRules.AllowWrite("Occupation", roleName)
    Else
      AuthorizationRules.DenyWrite("Occupation", roleName)
    End If
    'AuthorizationRules.AllowWrite("Occupation")

    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("WorksInCompany") Then
      AuthorizationRules.AllowWrite("WorksInCompany", roleName)
    Else
      AuthorizationRules.DenyWrite("WorksInCompany", roleName)
    End If
    'AuthorizationRules.AllowWrite("WorksInCompany")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdBusinessType") Then
      AuthorizationRules.AllowWrite("IdBusinessType", roleName)
    Else
      AuthorizationRules.DenyWrite("IdBusinessType", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdBusinessType")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")

    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Email") Then
      AuthorizationRules.AllowWrite("Email", roleName)
    Else
      AuthorizationRules.DenyWrite("Email", roleName)
    End If
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PassportNumber") Then
      AuthorizationRules.AllowWrite("PassportNumber", roleName)
    Else
      AuthorizationRules.DenyWrite("PassportNumber", roleName)
    End If
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("BLK") Then
      AuthorizationRules.AllowWrite("BLK", roleName)
    Else
      AuthorizationRules.DenyWrite("BLK", roleName)
    End If
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CanSendNotifications") Then
      AuthorizationRules.AllowWrite("CanSendNotifications", roleName)
    Else
      AuthorizationRules.DenyWrite("CanSendNotifications", roleName)
    End If
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DateOfBirth") Then
      AuthorizationRules.AllowWrite("DateOfBirth", roleName)
    Else
      AuthorizationRules.DenyWrite("DateOfBirth", roleName)
    End If


  End Sub

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Customer")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Customer")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Customer")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Customer")
  End Function


#End Region ' Authorization Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewCustomer() As Customer
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a Customer")
    End If
    Return DataPortal.Create(Of Customer)()
  End Function

  Public Shared Function GetCustomer(ByVal id As Integer) As Customer
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to get a Customer")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of Customer, Integer)(id))
  End Function
  Public Shared Function GetCustomerByMB(ByVal mb As String) As Customer
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to get a Customer")
    End If
    Return DataPortal.Fetch(New filterCriteria(mb))
  End Function
  Public Shared Sub DeleteCustomer(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to delete a Customer")
    End If
    DataPortal.Delete(New SingleCriteria(Of Customer, Integer)(id))
  End Sub

  Public Overrides Function Save() As Customer

    Dim result As Customer = MyBase.Save

    'OnCustomerSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
    'Return MyBase.Save()
  End Function

#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewCustomerChild() As Customer
    Return DataPortal.CreateChild(Of Customer)()
  End Function

  Friend Shared Function GetCustomer(ByVal dr As SafeDataReader) As Customer
    Return DataPortal.FetchChild(Of Customer)(dr)
  End Function

#End Region 'Child Factory Methods

#Region " Data Access "
  <Serializable()> _
   Private Class filterCriteria
    Private _inStr As String

    Public ReadOnly Property InStr() As String
      Get
        Return _inStr
      End Get
    End Property

    Public Sub New(ByVal inStr As String)
      _inStr = inStr
    End Sub
  End Class
#Region " Data Access - Create "

  <RunLocal()> _
  Private Overloads Sub DataPortal_Create()
    'proveri
    'Dim objOpcii As Options = CType(Csla.ApplicationContext.LocalContext("objOpcii"), Options)
    'LoadProperty(Of Integer)(IdCitizenshipProperty, Community.Drzavjanstvo(objOpcii.IdCommunity))
    ValidationRules.CheckRules()

  End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of Customer, Integer))
    Database.LogInfo("Customer.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of String)(MbProperty, dr.GetString("MB"))
            LoadProperty(Of String)(CustomerSurnameProperty, dr.GetString("CustomerSurname"))
            LoadProperty(Of String)(CustomerFirstNameProperty, dr.GetString("CustomerFirstName"))
            LoadProperty(Of String)(PhoneNumberProperty, dr.GetString("PhoneNumber"))
            LoadProperty(Of String)(FaxProperty, dr.GetString("Fax"))
            LoadProperty(Of Integer)(IdLivingAddressProperty, dr.GetInt32("IdLivingAddress"))
            LoadProperty(Of String)(LivingAddressNumberProperty, dr.GetString("LivingAddressNumber"))
            LoadProperty(Of Integer)(IdLivingCityProperty, dr.GetInt32("IdLivingCity"))
            LoadProperty(Of Integer)(IdBirhCityProperty, dr.GetInt32("IdBirhCity"))
            LoadProperty(Of Integer)(IdBirthAddressProperty, dr.GetInt32("IdBirthAddress"))
            LoadProperty(Of String)(BrithAddressNumberProperty, dr.GetString("BrithAddressNumber"))
            LoadProperty(Of SmartDate, Date?)(DateOfBirthProperty, dr.GetSmartDate("DateOfBirth", True))
            LoadProperty(Of Integer)(IdCitizenshipProperty, dr.GetInt32("IdCitizenship"))
            LoadProperty(Of Boolean)(IsCompanyProperty, dr.GetBoolean("IsCompany"))
            LoadProperty(Of String)(OccupationProperty, dr.GetString("Occupation"))
            LoadProperty(Of String)(WorksInCompanyProperty, dr.GetString("WorksInCompany"))
            LoadProperty(Of Integer)(IdBusinessTypeProperty, dr.GetInt32("IdBusinessType"))
            LoadProperty(Of String)(EmailProperty, dr.GetString("Email"))
            LoadProperty(Of String)(PassportNumberProperty, dr.GetString("PassportNumber"))
            LoadProperty(Of String)(BLKProperty, dr.GetString("BLK"))


            LoadProperty(Of Boolean)(CanSendNotificationsProperty, dr.GetBoolean("CanSendNotifications"))
            LoadProperty(Of String)(TaxNumberProperty, dr.GetString("TaxNumber"))
            LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
            LoadProperty(Of String)(StatusProperty, dr.GetString("Status"))

            LoadProperty(Of String)(ParentNameProperty, dr.GetString("ParentName"))
            LoadProperty(Of SmartDate, Date?)(BLKDateIssuedProperty, dr.GetSmartDate("BLKDateIssued", True))
            LoadProperty(Of Integer)(BLKIssuerProperty, dr.GetInt32("BLKIssuer"))
            LoadProperty(Of String)(DriveingLicenceNumberProperty, dr.GetString("DriveingLicenceNumber"))
            LoadProperty(Of SmartDate, Date?)(DriveingLicenceDateIssuedProperty, dr.GetSmartDate("DriveingLicenceDateIssued", True))
            LoadProperty(Of Integer)(DriveingLicenceIssuerProperty, dr.GetInt32("DriveingLicenceIssuer"))
            LoadProperty(Of SmartDate, Date?)(PassDateIssuedProperty, dr.GetSmartDate("PassDateIssued", True))
            LoadProperty(Of Integer)(PassIssuerProperty, dr.GetInt32("PassIssuer"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)


          End Using
        End Using
        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildrenContacts
          cm1.Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of CustomersContactPersons) _
            (ContactPersonsProperty, CustomersContactPersons.GetCustomersContactPersons(drc))
          End Using
        End Using

        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildrenBankAccounts
          cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of CustomerBankAccounts) _
            (BankAccountsProperty, CustomerBankAccounts.GetCustomerBankAccounts(drc))
          End Using
        End Using
      End Using
      ValidationRules.CheckRules()
    Catch ex As Exception
      Database.LogException("Customer.DataPortal_Fetch", ex)
      Throw New DbCslaException("Customer.DataPortal_Fetch", ex)
    End Try
  End Sub
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As filterCriteria)
    Database.LogInfo("Customer.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByMB
          cm.Parameters.AddWithValue("@mb", criteria.InStr)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            Try

              dr.Read()

              LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
              LoadProperty(Of String)(MbProperty, dr.GetString("MB"))
              LoadProperty(Of String)(CustomerSurnameProperty, dr.GetString("CustomerSurname"))
              LoadProperty(Of String)(CustomerFirstNameProperty, dr.GetString("CustomerFirstName"))
              LoadProperty(Of String)(PhoneNumberProperty, dr.GetString("PhoneNumber"))
              LoadProperty(Of String)(FaxProperty, dr.GetString("Fax"))
              LoadProperty(Of Integer)(IdLivingAddressProperty, dr.GetInt32("IdLivingAddress"))
              LoadProperty(Of String)(LivingAddressNumberProperty, dr.GetString("LivingAddressNumber"))
              LoadProperty(Of Integer)(IdLivingCityProperty, dr.GetInt32("IdLivingCity"))
              LoadProperty(Of Integer)(IdBirhCityProperty, dr.GetInt32("IdBirhCity"))
              LoadProperty(Of Integer)(IdBirthAddressProperty, dr.GetInt32("IdBirthAddress"))
              LoadProperty(Of String)(BrithAddressNumberProperty, dr.GetString("BrithAddressNumber"))
              LoadProperty(Of SmartDate, Date?)(DateOfBirthProperty, dr.GetSmartDate("DateOfBirth", True))
              LoadProperty(Of Integer)(IdCitizenshipProperty, dr.GetInt32("IdCitizenship"))
              LoadProperty(Of Boolean)(IsCompanyProperty, dr.GetBoolean("IsCompany"))
              LoadProperty(Of String)(OccupationProperty, dr.GetString("Occupation"))
              LoadProperty(Of String)(WorksInCompanyProperty, dr.GetString("WorksInCompany"))
              LoadProperty(Of Integer)(IdBusinessTypeProperty, dr.GetInt32("IdBusinessType"))
              LoadProperty(Of String)(EmailProperty, dr.GetString("Email"))
              LoadProperty(Of String)(PassportNumberProperty, dr.GetString("PassportNumber"))
              LoadProperty(Of String)(BLKProperty, dr.GetString("BLK"))


              LoadProperty(Of Boolean)(CanSendNotificationsProperty, dr.GetBoolean("CanSendNotifications"))
              LoadProperty(Of String)(TaxNumberProperty, dr.GetString("TaxNumber"))
              LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
              LoadProperty(Of String)(StatusProperty, dr.GetString("Status"))
              LoadProperty(Of String)(ParentNameProperty, dr.GetString("ParentName"))
              LoadProperty(Of SmartDate, Date?)(BLKDateIssuedProperty, dr.GetSmartDate("BLKDateIssued", True))
              LoadProperty(Of Integer)(BLKIssuerProperty, dr.GetInt32("BLKIssuer"))
              LoadProperty(Of String)(DriveingLicenceNumberProperty, dr.GetString("DriveingLicenceNumber"))
              LoadProperty(Of SmartDate, Date?)(DriveingLicenceDateIssuedProperty, dr.GetSmartDate("DriveingLicenceDateIssued", True))
              LoadProperty(Of Integer)(DriveingLicenceIssuerProperty, dr.GetInt32("DriveingLicenceIssuer"))
              LoadProperty(Of SmartDate, Date?)(PassDateIssuedProperty, dr.GetSmartDate("PassDateIssued", True))
              LoadProperty(Of Integer)(PassIssuerProperty, dr.GetInt32("PassIssuer"))
              dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)



            Catch ex As Exception
              Exit Sub
            End Try
          End Using
        End Using
        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildrenContacts
          cm1.Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of CustomersContactPersons) _
            (ContactPersonsProperty, CustomersContactPersons.GetCustomersContactPersons(drc))
          End Using
        End Using

        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildrenBankAccounts
          cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of CustomerBankAccounts) _
            (BankAccountsProperty, CustomerBankAccounts.GetCustomerBankAccounts(drc))
          End Using
        End Using
      End Using
      ValidationRules.CheckRules()
    Catch ex As Exception
      Database.LogException("Customer.DataPortal_Fetch", ex)
      Throw New DbCslaException("Customer.DataPortal_Fetch", ex)
    End Try
  End Sub
#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Protected Overrides Sub DataPortal_Insert()
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spAdd

            .Parameters.AddWithValue("@MB", ReadProperty(Of String)(MbProperty))
            .Parameters.AddWithValue("@CustomerSurname", ReadProperty(Of String)(CustomerSurnameProperty))
            .Parameters.AddWithValue("@CustomerFirstName", ReadProperty(Of String)(CustomerFirstNameProperty))
            .Parameters.AddWithValue("@PhoneNumber", ReadProperty(Of String)(PhoneNumberProperty))
            .Parameters.AddWithValue("@Fax", ReadProperty(Of String)(FaxProperty))
            .Parameters.AddWithValue("@IdLivingAddress", ReadProperty(Of Integer)(IdLivingAddressProperty))
            .Parameters.AddWithValue("@LivingAddressNumber", ReadProperty(Of String)(LivingAddressNumberProperty))
            .Parameters.AddWithValue("@IdLivingCity", ReadProperty(Of Integer)(IdLivingCityProperty))
            .Parameters.AddWithValue("@IdBirhCity", ReadProperty(Of Integer)(IdBirhCityProperty))
            .Parameters.AddWithValue("@IdBirthAddress", ReadProperty(Of Integer)(IdBirthAddressProperty))
            .Parameters.AddWithValue("@BrithAddressNumber", ReadProperty(Of String)(BrithAddressNumberProperty))
            .Parameters.AddWithValue("@DateOfBirth", ReadProperty(Of SmartDate)(DateOfBirthProperty).DBValue)
            .Parameters.AddWithValue("@IdCitizenship", ReadProperty(Of Integer)(IdCitizenshipProperty))
            .Parameters.AddWithValue("@IsCompany", ReadProperty(Of Boolean)(IsCompanyProperty))
            .Parameters.AddWithValue("@Occupation", ReadProperty(Of String)(OccupationProperty))
            .Parameters.AddWithValue("@WorksInCompany", ReadProperty(Of String)(WorksInCompanyProperty))
            .Parameters.AddWithValue("@IdBusinessType", ReadProperty(Of Integer)(IdBusinessTypeProperty))
            .Parameters.AddWithValue("@Email", ReadProperty(Of String)(EmailProperty))
            .Parameters.AddWithValue("@PassportNumber", ReadProperty(Of String)(PassportNumberProperty))
            .Parameters.AddWithValue("@BLK", ReadProperty(Of String)(BLKProperty))
            .Parameters.AddWithValue("@canSendNotifications", ReadProperty(Of Boolean)(CanSendNotificationsProperty))
            .Parameters.AddWithValue("@TaxNumber", ReadProperty(Of String)(TaxNumberProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
            .Parameters.AddWithValue("@Status", ReadProperty(Of String)(StatusProperty))

            .Parameters.AddWithValue("@ParentName", ReadProperty(Of String)(ParentNameProperty))
            .Parameters.AddWithValue("@BLKDateIssued", ReadProperty(Of SmartDate)(BLKDateIssuedProperty).DBValue)
            .Parameters.AddWithValue("@BLKIssuer", ReadProperty(Of Integer)(BLKIssuerProperty))
            .Parameters.AddWithValue("@DriveingLicenceNumber", ReadProperty(Of String)(DriveingLicenceNumberProperty))
            .Parameters.AddWithValue("@DriveingLicenceDateIssued", ReadProperty(Of SmartDate)(DriveingLicenceDateIssuedProperty).DBValue)
            .Parameters.AddWithValue("@DriveingLicenceIssuer", ReadProperty(Of Integer)(DriveingLicenceIssuerProperty))
            .Parameters.AddWithValue("@PassDateIssued", ReadProperty(Of SmartDate)(PassDateIssuedProperty).DBValue)
            .Parameters.AddWithValue("@PassIssuer", ReadProperty(Of Integer)(PassIssuerProperty))


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
        FieldManager.UpdateChildren(Me)

        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("Customer.DataPortal_Insert", ex)
      Throw New DbCslaException("Customer.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("Customer.DataPortal_Insert", GetHashCode())
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Protected Overrides Sub DataPortal_Update()
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
            .Parameters.AddWithValue("@MB", ReadProperty(Of String)(MbProperty))
            .Parameters.AddWithValue("@CustomerSurname", ReadProperty(Of String)(CustomerSurnameProperty))
            .Parameters.AddWithValue("@CustomerFirstName", ReadProperty(Of String)(CustomerFirstNameProperty))
            .Parameters.AddWithValue("@PhoneNumber", ReadProperty(Of String)(PhoneNumberProperty))
            .Parameters.AddWithValue("@Fax", ReadProperty(Of String)(FaxProperty))
            .Parameters.AddWithValue("@IdLivingAddress", ReadProperty(Of Integer)(IdLivingAddressProperty))
            .Parameters.AddWithValue("@LivingAddressNumber", ReadProperty(Of String)(LivingAddressNumberProperty))
            .Parameters.AddWithValue("@IdLivingCity", ReadProperty(Of Integer)(IdLivingCityProperty))
            .Parameters.AddWithValue("@IdBirhCity", ReadProperty(Of Integer)(IdBirhCityProperty))
            .Parameters.AddWithValue("@IdBirthAddress", ReadProperty(Of Integer)(IdBirthAddressProperty))
            .Parameters.AddWithValue("@BrithAddressNumber", ReadProperty(Of String)(BrithAddressNumberProperty))
            .Parameters.AddWithValue("@DateOfBirth", ReadProperty(Of SmartDate)(DateOfBirthProperty).DBValue)
            .Parameters.AddWithValue("@IdCitizenship", ReadProperty(Of Integer)(IdCitizenshipProperty))
            .Parameters.AddWithValue("@IsCompany", ReadProperty(Of Boolean)(IsCompanyProperty))
            .Parameters.AddWithValue("@Occupation", ReadProperty(Of String)(OccupationProperty))
            .Parameters.AddWithValue("@WorksInCompany", ReadProperty(Of String)(WorksInCompanyProperty))
            .Parameters.AddWithValue("@IdBusinessType", ReadProperty(Of Integer)(IdBusinessTypeProperty))
            .Parameters.AddWithValue("@Email", ReadProperty(Of String)(EmailProperty))
            .Parameters.AddWithValue("@PassportNumber", ReadProperty(Of String)(PassportNumberProperty))
            .Parameters.AddWithValue("@BLK", ReadProperty(Of String)(BLKProperty))
            .Parameters.AddWithValue("@canSendNotifications", ReadProperty(Of Boolean)(CanSendNotificationsProperty))
            .Parameters.AddWithValue("@TaxNumber", ReadProperty(Of String)(TaxNumberProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
            .Parameters.AddWithValue("@Status", ReadProperty(Of String)(StatusProperty))

            .Parameters.AddWithValue("@ParentName", ReadProperty(Of String)(ParentNameProperty))
            .Parameters.AddWithValue("@BLKDateIssued", ReadProperty(Of SmartDate)(BLKDateIssuedProperty).DBValue)
            .Parameters.AddWithValue("@BLKIssuer", ReadProperty(Of Integer)(BLKIssuerProperty))
            .Parameters.AddWithValue("@DriveingLicenceNumber", ReadProperty(Of String)(DriveingLicenceNumberProperty))
            .Parameters.AddWithValue("@DriveingLicenceDateIssued", ReadProperty(Of SmartDate)(DriveingLicenceDateIssuedProperty).DBValue)
            .Parameters.AddWithValue("@DriveingLicenceIssuer", ReadProperty(Of Integer)(DriveingLicenceIssuerProperty))
            .Parameters.AddWithValue("@PassDateIssued", ReadProperty(Of SmartDate)(PassDateIssuedProperty).DBValue)
            .Parameters.AddWithValue("@PassIssuer", ReadProperty(Of Integer)(PassIssuerProperty))

            .Parameters.AddWithValue("@lastChanged", _lastChanged)
            Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using

        'update child objects
        FieldManager.UpdateChildren(Me)


        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("Employee.DataPortal_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DBConcurrencyException("Employee.DataPortal_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Protected Overrides Sub DataPortal_DeleteSelf()
    DataPortal_Delete(New SingleCriteria(Of Customer, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of Customer, Integer))
    Database.LogInfo("Customer.DataPortal_Delete", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spDelete
            .Parameters.AddWithValue("@id", criteria.Value)
            .ExecuteNonQuery()
          End With
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("Customer.DataPortal_Delete", ex)
      Throw New DbCslaException("Customer.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "
  '  <RunLocal()> _
  'Private Overloads Child_Create()

  '    ValidationRules.CheckRules()
  '  End Sub

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("Customer.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of String)(MbProperty, dr.GetString("MB"))
      LoadProperty(Of String)(CustomerSurnameProperty, dr.GetString("CustomerSurname"))
      LoadProperty(Of String)(CustomerFirstNameProperty, dr.GetString("CustomerFirstName"))
      LoadProperty(Of String)(PhoneNumberProperty, dr.GetString("PhoneNumber"))
      LoadProperty(Of String)(FaxProperty, dr.GetString("Fax"))
      LoadProperty(Of Integer)(IdLivingAddressProperty, dr.GetInt32("IdLivingAddress"))
      LoadProperty(Of String)(LivingAddressNumberProperty, dr.GetString("LivingAddressNumber"))
      LoadProperty(Of Integer)(IdLivingCityProperty, dr.GetInt32("IdLivingCity"))
      LoadProperty(Of Integer)(IdBirhCityProperty, dr.GetInt32("IdBirhCity"))
      LoadProperty(Of Integer)(IdBirthAddressProperty, dr.GetInt32("IdBirthAddress"))
      LoadProperty(Of String)(BrithAddressNumberProperty, dr.GetString("BrithAddressNumber"))
      LoadProperty(Of SmartDate, Date?)(DateOfBirthProperty, dr.GetSmartDate("DateOfBirth", True))
      LoadProperty(Of Integer)(IdCitizenshipProperty, dr.GetInt32("IdCitizenship"))
      LoadProperty(Of Boolean)(IsCompanyProperty, dr.GetBoolean("IsCompany"))
      LoadProperty(Of String)(OccupationProperty, dr.GetString("Occupation"))
      LoadProperty(Of String)(WorksInCompanyProperty, dr.GetString("WorksInCompany"))
      LoadProperty(Of Integer)(IdBusinessTypeProperty, dr.GetInt32("IdBusinessType"))
      LoadProperty(Of String)(EmailProperty, dr.GetString("Email"))
      LoadProperty(Of String)(PassportNumberProperty, dr.GetString("PassportNumber"))
      LoadProperty(Of String)(BLKProperty, dr.GetString("BLK"))
      LoadProperty(Of Boolean)(CanSendNotificationsProperty, dr.GetBoolean("CanSendNotifications"))
      LoadProperty(Of String)(TaxNumberProperty, dr.GetString("TaxNumber"))
      LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
      LoadProperty(Of String)(StatusProperty, dr.GetString("Status"))
      LoadProperty(Of String)(ParentNameProperty, dr.GetString("ParentName"))
      LoadProperty(Of SmartDate, Date?)(BLKDateIssuedProperty, dr.GetSmartDate("BLKDateIssued", True))
      LoadProperty(Of Integer)(BLKIssuerProperty, dr.GetInt32("BLKIssuer"))
      LoadProperty(Of String)(DriveingLicenceNumberProperty, dr.GetString("DriveingLicenceNumber"))
      LoadProperty(Of SmartDate, Date?)(DriveingLicenceDateIssuedProperty, dr.GetSmartDate("DriveingLicenceDateIssued", True))
      LoadProperty(Of Integer)(DriveingLicenceIssuerProperty, dr.GetInt32("DriveingLicenceIssuer"))
      LoadProperty(Of SmartDate, Date?)(PassDateIssuedProperty, dr.GetSmartDate("PassDateIssued", True))
      LoadProperty(Of Integer)(PassIssuerProperty, dr.GetInt32("PassIssuer"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetChildrenContacts
          cm.Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm.ExecuteReader)
            LoadProperty(Of CustomersContactPersons) _
            (ContactPersonsProperty, CustomersContactPersons.GetCustomersContactPersons(drc))
          End Using
        End Using

        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildrenBankAccounts
          cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of CustomerBankAccounts) _
            (BankAccountsProperty, CustomerBankAccounts.GetCustomerBankAccounts(drc))
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("Customer.Child_Fetch", ex)
      Throw New DbCslaException("Customer.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert()
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spAdd
            .Parameters.AddWithValue("@MB", ReadProperty(Of String)(MbProperty))
            .Parameters.AddWithValue("@CustomerSurname", ReadProperty(Of String)(CustomerSurnameProperty))
            .Parameters.AddWithValue("@CustomerFirstName", ReadProperty(Of String)(CustomerFirstNameProperty))
            .Parameters.AddWithValue("@PhoneNumber", ReadProperty(Of String)(PhoneNumberProperty))
            .Parameters.AddWithValue("@Fax", ReadProperty(Of String)(FaxProperty))
            .Parameters.AddWithValue("@IdLivingAddress", ReadProperty(Of Integer)(IdLivingAddressProperty))
            .Parameters.AddWithValue("@LivingAddressNumber", ReadProperty(Of String)(LivingAddressNumberProperty))
            .Parameters.AddWithValue("@IdLivingCity", ReadProperty(Of Integer)(IdLivingCityProperty))
            .Parameters.AddWithValue("@IdBirhCity", ReadProperty(Of Integer)(IdBirhCityProperty))
            .Parameters.AddWithValue("@IdBirthAddress", ReadProperty(Of Integer)(IdBirthAddressProperty))
            .Parameters.AddWithValue("@BrithAddressNumber", ReadProperty(Of String)(BrithAddressNumberProperty))
            .Parameters.AddWithValue("@DateOfBirth", ReadProperty(Of SmartDate)(DateOfBirthProperty).DBValue)
            .Parameters.AddWithValue("@IdCitizenship", ReadProperty(Of Integer)(IdCitizenshipProperty))
            .Parameters.AddWithValue("@IsCompany", ReadProperty(Of Boolean)(IsCompanyProperty))
            .Parameters.AddWithValue("@Occupation", ReadProperty(Of String)(OccupationProperty))
            .Parameters.AddWithValue("@WorksInCompany", ReadProperty(Of String)(WorksInCompanyProperty))
            .Parameters.AddWithValue("@IdBusinessType", ReadProperty(Of Integer)(IdBusinessTypeProperty))
            .Parameters.AddWithValue("@Email", ReadProperty(Of String)(EmailProperty))
            .Parameters.AddWithValue("@PassportNumber", ReadProperty(Of String)(PassportNumberProperty))
            .Parameters.AddWithValue("@BLK", ReadProperty(Of String)(BLKProperty))
            .Parameters.AddWithValue("@canSendNotifications", ReadProperty(Of Boolean)(CanSendNotificationsProperty))
            .Parameters.AddWithValue("@TaxNumber", ReadProperty(Of String)(TaxNumberProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
            .Parameters.AddWithValue("@Status", ReadProperty(Of String)(StatusProperty))

            .Parameters.AddWithValue("@ParentName", ReadProperty(Of String)(ParentNameProperty))
            .Parameters.AddWithValue("@BLKDateIssued", ReadProperty(Of SmartDate)(BLKDateIssuedProperty).DBValue)
            .Parameters.AddWithValue("@BLKIssuer", ReadProperty(Of Integer)(BLKIssuerProperty))
            .Parameters.AddWithValue("@DriveingLicenceNumber", ReadProperty(Of String)(DriveingLicenceNumberProperty))
            .Parameters.AddWithValue("@DriveingLicenceDateIssued", ReadProperty(Of SmartDate)(DriveingLicenceDateIssuedProperty).DBValue)
            .Parameters.AddWithValue("@DriveingLicenceIssuer", ReadProperty(Of Integer)(DriveingLicenceIssuerProperty))
            .Parameters.AddWithValue("@PassDateIssued", ReadProperty(Of SmartDate)(PassDateIssuedProperty).DBValue)
            .Parameters.AddWithValue("@PassIssuer", ReadProperty(Of Integer)(PassIssuerProperty))

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
        FieldManager.UpdateChildren(Me)

        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("Customer.Child_Insert", ex)
      Throw New DbCslaException("Customer.Child_Insert", ex)
    Finally
      Database.LogInfo("Customer.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("Customer.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext.Remove("cn")
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
            .Parameters.AddWithValue("@MB", ReadProperty(Of String)(MbProperty))
            .Parameters.AddWithValue("@CustomerSurname", ReadProperty(Of String)(CustomerSurnameProperty))
            .Parameters.AddWithValue("@CustomerFirstName", ReadProperty(Of String)(CustomerFirstNameProperty))
            .Parameters.AddWithValue("@PhoneNumber", ReadProperty(Of String)(PhoneNumberProperty))
            .Parameters.AddWithValue("@Fax", ReadProperty(Of String)(FaxProperty))
            .Parameters.AddWithValue("@IdLivingAddress", ReadProperty(Of Integer)(IdLivingAddressProperty))
            .Parameters.AddWithValue("@LivingAddressNumber", ReadProperty(Of String)(LivingAddressNumberProperty))
            .Parameters.AddWithValue("@IdLivingCity", ReadProperty(Of Integer)(IdLivingCityProperty))
            .Parameters.AddWithValue("@IdBirhCity", ReadProperty(Of Integer)(IdBirhCityProperty))
            .Parameters.AddWithValue("@IdBirthAddress", ReadProperty(Of Integer)(IdBirthAddressProperty))
            .Parameters.AddWithValue("@BrithAddressNumber", ReadProperty(Of String)(BrithAddressNumberProperty))
            .Parameters.AddWithValue("@DateOfBirth", ReadProperty(Of SmartDate)(DateOfBirthProperty).DBValue)
            .Parameters.AddWithValue("@IdCitizenship", ReadProperty(Of Integer)(IdCitizenshipProperty))
            .Parameters.AddWithValue("@IsCompany", ReadProperty(Of Boolean)(IsCompanyProperty))
            .Parameters.AddWithValue("@Occupation", ReadProperty(Of String)(OccupationProperty))
            .Parameters.AddWithValue("@WorksInCompany", ReadProperty(Of String)(WorksInCompanyProperty))
            .Parameters.AddWithValue("@IdBusinessType", ReadProperty(Of Integer)(IdBusinessTypeProperty))
            .Parameters.AddWithValue("@Email", ReadProperty(Of String)(EmailProperty))
            .Parameters.AddWithValue("@PassportNumber", ReadProperty(Of String)(PassportNumberProperty))
            .Parameters.AddWithValue("@BLK", ReadProperty(Of String)(BLKProperty))
            .Parameters.AddWithValue("@canSendNotifications", ReadProperty(Of Boolean)(CanSendNotificationsProperty))
            .Parameters.AddWithValue("@TaxNumber", ReadProperty(Of String)(TaxNumberProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
            .Parameters.AddWithValue("@Status", ReadProperty(Of String)(StatusProperty))

            .Parameters.AddWithValue("@ParentName", ReadProperty(Of String)(ParentNameProperty))
            .Parameters.AddWithValue("@BLKDateIssued", ReadProperty(Of SmartDate)(BLKDateIssuedProperty).DBValue)
            .Parameters.AddWithValue("@BLKIssuer", ReadProperty(Of Integer)(BLKIssuerProperty))
            .Parameters.AddWithValue("@DriveingLicenceNumber", ReadProperty(Of String)(DriveingLicenceNumberProperty))
            .Parameters.AddWithValue("@DriveingLicenceDateIssued", ReadProperty(Of SmartDate)(DriveingLicenceDateIssuedProperty).DBValue)
            .Parameters.AddWithValue("@DriveingLicenceIssuer", ReadProperty(Of Integer)(DriveingLicenceIssuerProperty))
            .Parameters.AddWithValue("@PassDateIssued", ReadProperty(Of SmartDate)(PassDateIssuedProperty).DBValue)
            .Parameters.AddWithValue("@PassIssuer", ReadProperty(Of Integer)(PassIssuerProperty))

            .Parameters.AddWithValue("@lastChanged", _lastChanged)

            Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
        FieldManager.UpdateChildren(Me)

        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("Customer.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("Customer.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("Customer.Child_DeleteSelf", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
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
      Database.LogException("Customer.Child_Fetch", ex)
      Throw New DbCslaException("Customer.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

#Region " Exists "

  Public Shared Function Exists(ByVal Name As String) As Long

    Dim result As ExistsCommand
    result = DataPortal.Execute(Of ExistsCommand)(New ExistsCommand(Name))
    Return result.Exists

  End Function

  <Serializable()> _
  Private Class ExistsCommand
    Inherits CommandBase

    Private _mb As String
    Private _Exists As Long

    Public ReadOnly Property Exists() As Long
      Get
        Return _Exists
      End Get
    End Property

    Public Sub New(ByVal Mb As String)
      _mb = Mb
    End Sub

    Protected Overrides Sub DataPortal_Execute()
      Dim count As Long = 0

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.Text
          cm.CommandText = "select Id from Customers where MB=@mb"
          cm.Parameters.AddWithValue("@mb", _mb)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            If dr.Read() Then
              count = dr.GetInt64("Id")
            End If
          End Using
          _Exists = count
        End Using
      End Using

    End Sub

  End Class

#End Region

#Region " MB Exists "

  Public Shared Function MBExists(ByVal strMB As String, ByVal idCustomer As Long) As Boolean

    Return MBExistsCommand.MBExists(strMB, idCustomer)

  End Function

  <Serializable()> _
  Private Class MBExistsCommand
    Inherits CommandBase
    Private _mb As String
    Private _idCustomer As Long
    Private _existsMB As Boolean
    Public ReadOnly Property ExistsMB() As Boolean
      Get
        Return _existsMB
      End Get
    End Property

    Public Shared Function MBExists(ByVal strMB As String, ByVal idCustomer As Long) As Boolean

      Dim result As MBExistsCommand
      result = DataPortal.Execute(Of MBExistsCommand)(New MBExistsCommand(strMB, idCustomer))
      Return result.ExistsMB

    End Function

    Private Sub New(ByVal strMB As String, ByVal idCustomer As Long)
      _mb = strMB
      _idCustomer = idCustomer
      _existsMB = False
    End Sub

    Protected Overrides Sub DataPortal_Execute()
      Dim pom As Integer = 0
      Using cn As SqlConnection = Database.VTE_SqlConnection
        'ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "NumOfCustomerMBExists"
          cm.Parameters.AddWithValue("@mb", _mb)
          cm.Parameters.AddWithValue("@idCustomer", _idCustomer)
          pom = cm.ExecuteScalar
          If pom = 0 Then
            _existsMB = False
          Else
            _existsMB = True
          End If
        End Using
      End Using
    End Sub

  End Class

#End Region

  '#Region " Readonlylist refresh "
  '  Public Shared Event CustomerSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  '  Protected Shared Sub OnCustomerSaved(ByVal sender As Customer, ByVal e As Csla.Core.SavedEventArgs)
  '    RaiseEvent CustomerSaved(sender, e)
  '  End Sub
  '#End Region

End Class
