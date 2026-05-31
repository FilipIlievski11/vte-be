
<Serializable()> _
Public Class PaymentCategorie
 Inherits Csla.BusinessBase(Of PaymentCategorie)

#Region " Stored Procedures Names "
 Private Const spGetByID As String = "GetPaymentCategorieByID"
 Private Const spGetAll As String = "GetPaymentCategories"
 Private Const spUpdate As String = "updatePaymentCategorie"
 Private Const spAdd As String = "addPaymentCategorie"
 Private Const spDelete As String = "deletePaymentCategorie"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
 'register properties
 Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentCategorie), New PropertyInfo(Of Integer)("Id"))
 Private Shared IdDDVProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentCategorie), New PropertyInfo(Of Integer)("IdDDV"))
 Private Shared IdCalculationItemProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentCategorie), New PropertyInfo(Of Integer)("IdCalculationItem"))
 Private Shared IdCommunityProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentCategorie), New PropertyInfo(Of Integer)("IdCommunity"))
 Private Shared IdCompanyProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentCategorie), New PropertyInfo(Of Integer)("IdCompany"))
 Private Shared CategoryNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(PaymentCategorie), New PropertyInfo(Of String)("CategoryName"))
 Private Shared AllowDiscountProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentCategorie), New PropertyInfo(Of Boolean)("AllowDiscount"))
 Private Shared TrigerdByRequestProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentCategorie), New PropertyInfo(Of Boolean)("TrigerdByRequest"))
 Private Shared TrigerdByTechnicalExamProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentCategorie), New PropertyInfo(Of Boolean)("TrigerdByTechnicalExam"))
 Private Shared TrigerdByTrafficLicenceProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentCategorie), New PropertyInfo(Of Boolean)("TrigerdByTrafficLicence"))
 Private Shared TrigerdByPremisionForVehicleProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentCategorie), New PropertyInfo(Of Boolean)("TrigerdByPremisionForVehicle"))
 Private Shared TrigerdByInternationalDrivierLicenceProperty As PropertyInfo(Of Boolean) = _
                              RegisterProperty(Of Boolean)(GetType(PaymentCategorie), _
                              New PropertyInfo(Of Boolean)("TrigerdByInternationalDrivierLicence"))
 Private Shared PaymentItemsProperty As PropertyInfo(Of PaymentItems) = _
                              RegisterProperty(Of PaymentItems)(GetType(PaymentCategorie), _
                              New PropertyInfo(Of PaymentItems)("PaymentsItems"))

 Private Shared TrigerdByIrregularTechnicalExamProperty As PropertyInfo(Of Boolean) = _
                              RegisterProperty(Of Boolean)(GetType(PaymentCategorie), _
                              New PropertyInfo(Of Boolean)("TrigerdByIrregularTechnicalExam"))
 Private Shared VisibleOrderProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentCategorie), New PropertyInfo(Of Integer)("VisibleOrder"))
 Private _lastChanged(7) As Byte

 <System.ComponentModel.DataObjectField(True, True)> _
 Public ReadOnly Property Id() As Integer
  Get
   Return GetProperty(Of Integer)(IdProperty)
  End Get
 End Property
 Public Property IdDDV() As Integer
  Get
   If Csla.ApplicationContext.LocalContext.Contains("CategorieDDV") Then
    Csla.ApplicationContext.LocalContext.Remove("CategorieDDV")
   End If
   Csla.ApplicationContext.LocalContext.Add("CategorieDDV", GetProperty(Of Integer)(IdDDVProperty))
   Return GetProperty(Of Integer)(IdDDVProperty)

  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdDDVProperty, value)
   If Csla.ApplicationContext.LocalContext.Contains("CategorieDDV") Then
    Csla.ApplicationContext.LocalContext.Remove("CategorieDDV")
   End If
   Csla.ApplicationContext.LocalContext.Add("CategorieDDV", value)
  End Set
 End Property
 Public Property IdCalculationItem() As Integer
  Get
   Return GetProperty(Of Integer)(IdCalculationItemProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdCalculationItemProperty, value)
  End Set
 End Property
 Public Property IdCommunity() As Integer
  Get
   Return GetProperty(Of Integer)(IdCommunityProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdCommunityProperty, value)
  End Set
 End Property

 Public Property IdCompany() As Integer
  Get
   Return GetProperty(Of Integer)(IdCompanyProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdCompanyProperty, value)
  End Set
 End Property

 Public Property CategoryName() As String
  Get
   Return GetProperty(Of String)(CategoryNameProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(CategoryNameProperty, value)
  End Set
 End Property

 Public Property AllowDiscount() As Boolean
  Get
   Return GetProperty(Of Boolean)(AllowDiscountProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(AllowDiscountProperty, value)
  End Set
 End Property
 Public Property TrigerdByRequest() As Boolean
  Get
   Return GetProperty(Of Boolean)(TrigerdByRequestProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(TrigerdByRequestProperty, value)
  End Set
 End Property
 Public Property TrigerdByTechnicalExam() As Boolean
  Get
   Return GetProperty(Of Boolean)(TrigerdByTechnicalExamProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(TrigerdByTechnicalExamProperty, value)
  End Set
 End Property
 Public Property TrigerdByTrafficLicence() As Boolean
  Get
   Return GetProperty(Of Boolean)(TrigerdByTrafficLicenceProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(TrigerdByTrafficLicenceProperty, value)
  End Set
 End Property
 Public Property TrigerdByPremisionForVehicle() As Boolean
  Get
   Return GetProperty(Of Boolean)(TrigerdByPremisionForVehicleProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(TrigerdByPremisionForVehicleProperty, value)
  End Set
 End Property
 Public Property TrigerdByInternationalDrivierLicence() As Boolean
  Get
   Return GetProperty(Of Boolean)(TrigerdByInternationalDrivierLicenceProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(TrigerdByInternationalDrivierLicenceProperty, value)
  End Set
 End Property
 Public Property TrigerdByIrregularTechnicalExam() As Boolean
  Get
   Return GetProperty(Of Boolean)(TrigerdByIrregularTechnicalExamProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(TrigerdByIrregularTechnicalExamProperty, value)
  End Set
 End Property
 Public Property VisibleOrder() As Integer
  Get
   Return GetProperty(Of Integer)(VisibleOrderProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(VisibleOrderProperty, value)
  End Set
 End Property

 Public ReadOnly Property PaymentsItems() As PaymentItems
  Get
   If Not FieldManager.FieldExists(PaymentItemsProperty) Then
    SetProperty(Of PaymentItems) _
    (PaymentItemsProperty, PaymentItems.NewPaymentItems)
   End If
   Return GetProperty(Of PaymentItems)(PaymentItemsProperty)
  End Get
 End Property


 Public Overrides Function ToString() As String
  Return Id.ToString
 End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

 Protected Overrides Sub AddAuthorizationRules()
  Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdDDV") Then
   AuthorizationRules.AllowWrite("IdDDV", roleName)
  Else
   AuthorizationRules.DenyWrite("IdDDV", roleName)
  End If
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCalculationItem") Then
   AuthorizationRules.AllowWrite("IdCalculationItem", roleName)
  Else
   AuthorizationRules.DenyWrite("IdCalculationItem", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdDDV")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CategoryName") Then
   AuthorizationRules.AllowWrite("CategoryName", roleName)
  Else
   AuthorizationRules.DenyWrite("CategoryName", roleName)
  End If
  'AuthorizationRules.AllowWrite("CategoryName")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TrigerdByRequest") Then
   AuthorizationRules.AllowWrite("TrigerdByRequest", roleName)
  Else
   AuthorizationRules.DenyWrite("TrigerdByRequest", roleName)
  End If
  'AuthorizationRules.AllowWrite("TrigerdByRequest")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TrigerdByTechnicalExam") Then
   AuthorizationRules.AllowWrite("TrigerdByTechnicalExam", roleName)
  Else
   AuthorizationRules.DenyWrite("TrigerdByTechnicalExam", roleName)
  End If
  'AuthorizationRules.AllowWrite("TrigerdByTechnicalExam")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TrigerdByTrafficLicence") Then
   AuthorizationRules.AllowWrite("TrigerdByTrafficLicence", roleName)
  Else
   AuthorizationRules.DenyWrite("TrigerdByTrafficLicence", roleName)
  End If
  'AuthorizationRules.AllowWrite("TrigerdByTrafficLicence")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TrigerdByPremisionForVehicle") Then
   AuthorizationRules.AllowWrite("TrigerdByPremisionForVehicle", roleName)
  Else
   AuthorizationRules.DenyWrite("TrigerdByPremisionForVehicle", roleName)
  End If
  'AuthorizationRules.AllowWrite("TrigerdByPremisionForVehicle")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TrigerdByInternationalDrivierLicence") Then
   AuthorizationRules.AllowWrite("TrigerdByInternationalDrivierLicence", roleName)
  Else
   AuthorizationRules.DenyWrite("TrigerdByInternationalDrivierLicence", roleName)
  End If
  'TrigerdByIrregularTechnicalExamProperty
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TrigerdByIrregularTechnicalExamProperty") Then
   AuthorizationRules.AllowWrite("TrigerdByIrregularTechnicalExamProperty", roleName)
  Else
   AuthorizationRules.DenyWrite("TrigerdByIrregularTechnicalExamProperty", roleName)
  End If
  'AuthorizationRules.AllowWrite("TrigerdByInternationalDrivierLicence")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
   AuthorizationRules.AllowWrite("Active", roleName)
  Else
   AuthorizationRules.DenyWrite("Active", roleName)
  End If
  'AuthorizationRules.AllowWrite("Active")
 End Sub



 Public Shared Function CanGetObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("PaymentCategorie")
 End Function

 Public Shared Function CanAddObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("PaymentCategorie")
 End Function

 Public Shared Function CanEditObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("PaymentCategorie")
 End Function

 Public Shared Function CanDeleteObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("PaymentCategorie")
 End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
 Protected Overrides Sub AddBusinessRules()
  ' CategoryNameProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CategoryNameProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CategoryNameProperty, 250))
  ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                          New Csla.Validation.IntegerMinValueRuleArgs(IdCalculationItemProperty, 0))
  ValidationRules.AddRule(AddressOf Csla.Validation.IntegerMinValue, _
                          New Validation.IntegerMinValueRuleArgs(IdDDVProperty, 0))
  ValidationRules.AddRule(AddressOf Csla.Validation.IntegerMinValue, _
                          New Validation.IntegerMinValueRuleArgs(VisibleOrderProperty, 0))
 End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

 Private Sub New()
  ' require use of factory method 
 End Sub

 Public Shared Function NewPaymentCategorie() As PaymentCategorie
  If Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a PaymentCategorie")
  End If
  Return DataPortal.Create(Of PaymentCategorie)()
 End Function

 Public Shared Function GetPaymentCategorie(ByVal id As Integer) As PaymentCategorie
  If Not CanGetObject() Then
   Throw New System.Security.SecurityException("User not authorized to view a PaymentCategorie")
  End If
  Return DataPortal.Fetch(New SingleCriteria(Of PaymentCategorie, Integer)(id))
 End Function

 Public Shared Sub DeletePaymentCategorie(ByVal id As Integer)
  If Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a PaymentCategorie")
  End If
  DataPortal.Delete(New SingleCriteria(Of PaymentCategorie, Integer)(id))
 End Sub

 Public Overrides Function Save() As PaymentCategorie
  If IsDeleted AndAlso Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a PaymentCategorie")
  ElseIf IsNew AndAlso Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a PaymentCategorie")
  ElseIf Not CanEditObject() Then
   Throw New System.Security.SecurityException("User not authorized to update a PaymentCategorie")
  End If
  Return MyBase.Save()
 End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

 Friend Shared Function NewPaymentCategorieChild() As PaymentCategorie
  Return DataPortal.CreateChild(Of PaymentCategorie)()
 End Function

 Friend Shared Function GetPaymentCategorie(ByVal dr As SafeDataReader) As PaymentCategorie
  Return DataPortal.FetchChild(Of PaymentCategorie)(dr)
 End Function

#End Region 'Child Factory Methods

#Region " Data Access "

#Region " Data Access - Create "

 <RunLocal()> _
 Private Overloads Sub DataPortal_Create()
  ValidationRules.CheckRules()
 End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of PaymentCategorie, Integer))
  Database.LogInfo("PaymentCategorie.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = spGetByID
     cm.Parameters.AddWithValue("@Id", criteria.Value)
     Using dr As New SafeDataReader(cm.ExecuteReader)
      dr.Read()

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdDDVProperty, dr.GetInt32("IdDDV"))
      LoadProperty(Of Integer)(IdCalculationItemProperty, dr.GetInt32("IdCalculationItem"))
      LoadProperty(Of Integer)(IdCommunityProperty, dr.GetInt32("IdCommunity"))
      LoadProperty(Of Integer)(IdCompanyProperty, dr.GetInt32("IdCompany"))
      LoadProperty(Of String)(CategoryNameProperty, dr.GetString("CategoryName"))
      LoadProperty(Of Boolean)(AllowDiscountProperty, dr.GetBoolean("AllowDiscount"))
      LoadProperty(Of Boolean)(TrigerdByRequestProperty, dr.GetBoolean("TrigerdByRequest"))
      LoadProperty(Of Boolean)(TrigerdByTechnicalExamProperty, dr.GetBoolean("TrigerdByTechnicalExam"))
      LoadProperty(Of Boolean)(TrigerdByTrafficLicenceProperty, dr.GetBoolean("TrigerdByTrafficLicence"))
      LoadProperty(Of Boolean)(TrigerdByPremisionForVehicleProperty, dr.GetBoolean("TrigerdByPremisionForVehicle"))
      LoadProperty(Of Boolean)(TrigerdByInternationalDrivierLicenceProperty, dr.GetBoolean("TrigerdByInternationalDrivierLicence"))
      LoadProperty(Of Boolean)(TrigerdByIrregularTechnicalExamProperty, dr.GetBoolean("TrigerdByIrregularTechnicalExam"))
      LoadProperty(Of Integer)(VisibleOrderProperty, dr.GetInt32("VisibleOrder"))
      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
     End Using
    End Using
   End Using

  Catch ex As Exception
   Database.LogException("PaymentCategorie.DataPortal_Fetch", ex)
   Throw New DbCslaException("PaymentCategorie.DataPortal_Fetch", ex)
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

      .Parameters.AddWithValue("@IdDDV", ReadProperty(Of Integer)(IdDDVProperty))
      .Parameters.AddWithValue("@IdCalculationItem", ReadProperty(Of Integer)(IdCalculationItemProperty))
      .Parameters.AddWithValue("@IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty))
      Dim tehOrg As TehnicalExamOrganizationsInfo = Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization")

      .Parameters.AddWithValue("@IdCompany", tehOrg.IdCompany) 'ReadProperty(Of Integer)(IdCompanyProperty))
      .Parameters.AddWithValue("@CategoryName", ReadProperty(Of String)(CategoryNameProperty))
      .Parameters.AddWithValue("@AllowDiscount", ReadProperty(Of Boolean)(AllowDiscountProperty))
      .Parameters.AddWithValue("@TrigerdByRequest", ReadProperty(Of Boolean)(TrigerdByRequestProperty))
      .Parameters.AddWithValue("@TrigerdByTechnicalExam", ReadProperty(Of Boolean)(TrigerdByTechnicalExamProperty))
      .Parameters.AddWithValue("@TrigerdByTrafficLicence", ReadProperty(Of Boolean)(TrigerdByTrafficLicenceProperty))
      .Parameters.AddWithValue("@TrigerdByPremisionForVehicle", ReadProperty(Of Boolean)(TrigerdByPremisionForVehicleProperty))
      .Parameters.AddWithValue("@TrigerdByInternationalDrivierLicence", ReadProperty(Of Boolean)(TrigerdByInternationalDrivierLicenceProperty))
      .Parameters.AddWithValue("@TrigerdByIrregularTechnicalExam", ReadProperty(Of Boolean)(TrigerdByIrregularTechnicalExamProperty))
      .Parameters.AddWithValue("@VisibleOrder", ReadProperty(Of Integer)(VisibleOrderProperty))
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
    If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
     ApplicationContext.LocalContext.Remove("cn")
    End If
   End Using
  Catch ex As Exception
   Database.LogException("PaymentCategorie.DataPortal_Insert", ex)
   Throw New DbCslaException("PaymentCategorie.DataPortal_Insert", ex)
  Finally
   Database.LogInfo("PaymentCategorie.DataPortal_Insert", GetHashCode())
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

      .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
      .Parameters.AddWithValue("@IdDDV", ReadProperty(Of Integer)(IdDDVProperty))
      .Parameters.AddWithValue("@IdCalculationItem", ReadProperty(Of Integer)(IdCalculationItemProperty))
      .Parameters.AddWithValue("@IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty))
      'Dim tehOrg As TehnicalExamOrganizationsInfo = Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization")

      .Parameters.AddWithValue("@IdCompany", ReadProperty(Of Integer)(IdCompanyProperty))
      .Parameters.AddWithValue("@CategoryName", ReadProperty(Of String)(CategoryNameProperty))
      .Parameters.AddWithValue("@AllowDiscount", ReadProperty(Of Boolean)(AllowDiscountProperty))
      .Parameters.AddWithValue("@TrigerdByRequest", ReadProperty(Of Boolean)(TrigerdByRequestProperty))
      .Parameters.AddWithValue("@TrigerdByTechnicalExam", ReadProperty(Of Boolean)(TrigerdByTechnicalExamProperty))
      .Parameters.AddWithValue("@TrigerdByTrafficLicence", ReadProperty(Of Boolean)(TrigerdByTrafficLicenceProperty))
      .Parameters.AddWithValue("@TrigerdByPremisionForVehicle", ReadProperty(Of Boolean)(TrigerdByPremisionForVehicleProperty))
      .Parameters.AddWithValue("@TrigerdByInternationalDrivierLicence", ReadProperty(Of Boolean)(TrigerdByInternationalDrivierLicenceProperty))
      .Parameters.AddWithValue("@TrigerdByIrregularTechnicalExam", ReadProperty(Of Boolean)(TrigerdByIrregularTechnicalExamProperty))
      .Parameters.AddWithValue("@VisibleOrder", ReadProperty(Of Integer)(VisibleOrderProperty))

      .Parameters.AddWithValue("@lastChanged", _lastChanged)
      Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)

      .ExecuteNonQuery()

      _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
     End With
    End Using
    'update child objects
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
  DataPortal_Delete(New SingleCriteria(Of PaymentCategorie, Integer)(Id))
 End Sub

 Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of PaymentCategorie, Integer))
  Database.LogInfo("PaymentCategorie.DataPortal_Delete", GetHashCode())
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
   Database.LogException("PaymentCategorie.DataPortal_Delete", ex)
   Throw New DbCslaException("PaymentCategorie.DataPortal_Delete", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

 Private Sub Child_Fetch(ByVal dr As SafeDataReader)
  Database.LogInfo("PaymentCategorie.Child_Fetch", GetHashCode())
  Try
   LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
   LoadProperty(Of Integer)(IdDDVProperty, dr.GetInt32("IdDDV"))
   LoadProperty(Of Integer)(IdCalculationItemProperty, dr.GetInt32("IdCalculationItem"))
   LoadProperty(Of Integer)(IdCommunityProperty, dr.GetInt32("IdCommunity"))
   LoadProperty(Of Integer)(IdCompanyProperty, dr.GetInt32("IdCompany"))
   LoadProperty(Of String)(CategoryNameProperty, dr.GetString("CategoryName"))
   LoadProperty(Of Boolean)(AllowDiscountProperty, dr.GetBoolean("AllowDiscount"))
   LoadProperty(Of Boolean)(TrigerdByRequestProperty, dr.GetBoolean("TrigerdByRequest"))
   LoadProperty(Of Boolean)(TrigerdByTechnicalExamProperty, dr.GetBoolean("TrigerdByTechnicalExam"))
   LoadProperty(Of Boolean)(TrigerdByTrafficLicenceProperty, dr.GetBoolean("TrigerdByTrafficLicence"))
   LoadProperty(Of Boolean)(TrigerdByPremisionForVehicleProperty, dr.GetBoolean("TrigerdByPremisionForVehicle"))
   LoadProperty(Of Boolean)(TrigerdByInternationalDrivierLicenceProperty, dr.GetBoolean("TrigerdByInternationalDrivierLicence"))
   LoadProperty(Of Boolean)(TrigerdByIrregularTechnicalExamProperty, dr.GetBoolean("TrigerdByIrregularTechnicalExam"))
   LoadProperty(Of Integer)(VisibleOrderProperty, dr.GetInt32("VisibleOrder"))

   dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "getPaymentItemByIdPymentCategory"
     cm.Parameters.AddWithValue("@id", ReadProperty(Of Integer)(IdProperty))
     Using drc As New SafeDataReader(cm.ExecuteReader)
      LoadProperty(Of PaymentItems)(PaymentItemsProperty, PaymentItems.GetPaymentItems(drc))
     End Using
    End Using

   End Using

  Catch ex As Exception
   Database.LogException("PaymentCategorie.Child_Fetch", ex)
   Throw New DbCslaException("PaymentCategorie.Child_Fetch", ex)
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
      .Parameters.AddWithValue("@IdDDV", ReadProperty(Of Integer)(IdDDVProperty))
      .Parameters.AddWithValue("@IdCalculationItem", ReadProperty(Of Integer)(IdCalculationItemProperty))
      .Parameters.AddWithValue("@IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty))
      Dim tehOrg As TehnicalExamOrganizationsInfo = Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization")

      .Parameters.AddWithValue("@IdCompany", tehOrg.IdCompany)
      .Parameters.AddWithValue("@CategoryName", ReadProperty(Of String)(CategoryNameProperty))
      .Parameters.AddWithValue("@AllowDiscount", ReadProperty(Of Boolean)(AllowDiscountProperty))
      .Parameters.AddWithValue("@TrigerdByRequest", ReadProperty(Of Boolean)(TrigerdByRequestProperty))
      .Parameters.AddWithValue("@TrigerdByTechnicalExam", ReadProperty(Of Boolean)(TrigerdByTechnicalExamProperty))
      .Parameters.AddWithValue("@TrigerdByTrafficLicence", ReadProperty(Of Boolean)(TrigerdByTrafficLicenceProperty))
      .Parameters.AddWithValue("@TrigerdByPremisionForVehicle", ReadProperty(Of Boolean)(TrigerdByPremisionForVehicleProperty))
      .Parameters.AddWithValue("@TrigerdByInternationalDrivierLicence", ReadProperty(Of Boolean)(TrigerdByInternationalDrivierLicenceProperty))
      .Parameters.AddWithValue("@TrigerdByIrregularTechnicalExam", ReadProperty(Of Boolean)(TrigerdByIrregularTechnicalExamProperty))
      .Parameters.AddWithValue("@VisibleOrder", ReadProperty(Of Integer)(VisibleOrderProperty))

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
    'deca
    FieldManager.UpdateChildren(Me)
    If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
     ApplicationContext.LocalContext.Remove("cn")
    End If
   End Using
  Catch ex As Exception
   Database.LogException("PaymentCategorie.Child_Insert", ex)
   Throw New DbCslaException("PaymentCategorie.Child_Insert", ex)
  Finally
   Database.LogInfo("PaymentCategorie.Child_Insert", GetHashCode)
  End Try

 End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

 Private Sub Child_Update()
  Database.LogInfo("PaymentCategorie.Child_Update", GetHashCode)
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spUpdate

      .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
      .Parameters.AddWithValue("@IdDDV", ReadProperty(Of Integer)(IdDDVProperty))
      .Parameters.AddWithValue("@IdCalculationItem", ReadProperty(Of Integer)(IdCalculationItemProperty))
      .Parameters.AddWithValue("@IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty))
      'Dim tehOrg As TehnicalExamOrganizationsInfo = Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization")
      .Parameters.AddWithValue("@IdCompany", ReadProperty(Of Integer)(IdCompanyProperty))
      .Parameters.AddWithValue("@CategoryName", ReadProperty(Of String)(CategoryNameProperty))
      .Parameters.AddWithValue("@AllowDiscount", ReadProperty(Of Boolean)(AllowDiscountProperty))
      .Parameters.AddWithValue("@TrigerdByRequest", ReadProperty(Of Boolean)(TrigerdByRequestProperty))
      .Parameters.AddWithValue("@TrigerdByTechnicalExam", ReadProperty(Of Boolean)(TrigerdByTechnicalExamProperty))
      .Parameters.AddWithValue("@TrigerdByTrafficLicence", ReadProperty(Of Boolean)(TrigerdByTrafficLicenceProperty))
      .Parameters.AddWithValue("@TrigerdByPremisionForVehicle", ReadProperty(Of Boolean)(TrigerdByPremisionForVehicleProperty))
      .Parameters.AddWithValue("@TrigerdByInternationalDrivierLicence", ReadProperty(Of Boolean)(TrigerdByInternationalDrivierLicenceProperty))
      .Parameters.AddWithValue("@TrigerdByIrregularTechnicalExam", ReadProperty(Of Boolean)(TrigerdByIrregularTechnicalExamProperty))
      .Parameters.AddWithValue("@lastChanged", _lastChanged)
      .Parameters.AddWithValue("@VisibleOrder", ReadProperty(Of Integer)(VisibleOrderProperty))

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
   Database.LogException("PaymentCategorie.Child_Update", ex)
   If Not ex.Message.EndsWith("drug korisnik") Then
    Throw New DbCslaException("PaymentCategorie.Child_Update", ex)
   End If
  End Try
 End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

 Private Sub Child_DeleteSelf()

  Database.LogInfo("PaymentCategorie.Child_DeleteSelf", GetHashCode)
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
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
   Database.LogException("PaymentCategorie.Child_Fetch", ex)
   Throw New DbCslaException("PaymentCategorie.Child_Fetch", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
