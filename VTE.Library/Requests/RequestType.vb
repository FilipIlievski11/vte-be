
<Serializable()> _
Public Class RequestType
  Inherits Csla.BusinessBase(Of RequestType)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetRequestTypeByID"
  Private Const spGetAll As String = "GetRequestTypes"
  Private Const spUpdate As String = "updateRequestType"
  Private Const spAdd As String = "addRequestType"
  Private Const spDelete As String = "deleteRequestType"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(RequestType), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdRequestTypeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(RequestType), New PropertyInfo(Of Integer)("IdRequestType"))
  Private Shared IdDocumentPrintProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(RequestType), New PropertyInfo(Of Integer)("IdDocumentPrint"))
    Private Shared IsTehnicalExamRequiredProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(RequestType), New PropertyInfo(Of Integer)("IsTehnicalExamRequired"))
  Private Shared IsPayRequiredProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(RequestType), New PropertyInfo(Of Boolean)("IsPayRequired"))
  Private Shared IsNewRegistrationProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(RequestType), New PropertyInfo(Of Boolean)("IsNewRegistration"))
  Private Shared IsRelationDeletedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(RequestType), New PropertyInfo(Of Boolean)("IsRelationDeleted"))
  Private Shared IsVehicleDeletedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(RequestType), New PropertyInfo(Of Boolean)("IsVehicleDeleted"))
  Private Shared IsNewCustomerProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(RequestType), New PropertyInfo(Of Boolean)("IsNewCustomer"))
  Private Shared IsVehicleChangedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(RequestType), New PropertyInfo(Of Boolean)("IsVehicleChanged"))
  Private Shared IsCustomerChangedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(RequestType), New PropertyInfo(Of Boolean)("IsCustomerChanged"))
  Private Shared IsSufficientProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(RequestType), New PropertyInfo(Of Boolean)("IsSufficient"))
  Private Shared IsPreviosRegistrationReqiredProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(RequestType), New PropertyInfo(Of Boolean)("IsPreviosRegistrationReqired"))

  Private Shared TypeNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(RequestType), New PropertyInfo(Of String)("TypeName"))
  Private Shared TypeDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(RequestType), New PropertyInfo(Of String)("TypeDescription"))
  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Dim result = GetProperty(Of Integer)(IdProperty)
      If result = 0 Then
        result = GetMax() + 1
        LoadProperty(Of Integer)(IdProperty, result)
      End If
      Return result
    End Get
  End Property

  Private Function GetMax() As Integer

    ' generate a default id value
    Dim parent As RequestTypes = CType(Me.Parent, RequestTypes)
    Dim max As Integer = 0
    For Each item As RequestType In parent
      If Not ReferenceEquals(item, Me) AndAlso item.Id > max Then
        max = item.Id
      End If
    Next
    Return max

  End Function

  Public Property IdRequestType() As Integer
    Get
      Return GetProperty(Of Integer)(IdRequestTypeProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdRequestTypeProperty, value)
    End Set
  End Property
  Public Property IdDocumentPrint() As Integer
    Get
      Return GetProperty(Of Integer)(IdDocumentPrintProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdDocumentPrintProperty, value)
    End Set
  End Property
    Public Property IsTehnicalExamRequired() As Integer
        Get
            Return GetProperty(Of Integer)(IsTehnicalExamRequiredProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IsTehnicalExamRequiredProperty, value)
        End Set
    End Property
  Public Property IsPayRequired() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsPayRequiredProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsPayRequiredProperty, value)
    End Set
  End Property
  Public Property IsNewRegistration() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsNewRegistrationProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsNewRegistrationProperty, value)
    End Set
  End Property
  Public Property IsRelationDeleted() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsRelationDeletedProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsRelationDeletedProperty, value)
    End Set
  End Property
  Public Property IsVehicleDeleted() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsVehicleDeletedProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsVehicleDeletedProperty, value)
    End Set
  End Property
  Public Property IsNewCustomer() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsNewCustomerProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsNewCustomerProperty, value)
    End Set
  End Property
  Public Property IsVehicleChanged() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsVehicleChangedProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsVehicleChangedProperty, value)
    End Set
  End Property
  Public Property IsCustomerChanged() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsCustomerChangedProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsCustomerChangedProperty, value)
    End Set
  End Property
  Public Property IsPreviosRegistrationReqired() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsPreviosRegistrationReqiredProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsPreviosRegistrationReqiredProperty, value)
    End Set
  End Property

  Public Property TypeName() As String
    Get
      Return GetProperty(Of String)(TypeNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(TypeNameProperty, value)
    End Set
  End Property
  Public Property TypeDescription() As String
    Get
      Return GetProperty(Of String)(TypeDescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(TypeDescriptionProperty, value)
    End Set
  End Property
  Public Property IsSufficient() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsSufficientProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsSufficientProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdRequestType") Then
      AuthorizationRules.AllowWrite("IdRequestType", roleName)
    Else
      AuthorizationRules.DenyWrite("IdRequestType", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdRequestType")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdDocumentPrint") Then
      AuthorizationRules.AllowWrite("IdDocumentPrint", roleName)
    Else
      AuthorizationRules.DenyWrite("IdDocumentPrint", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdDocumentPrint")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsTehnicalExamRequired") Then
      AuthorizationRules.AllowWrite("IsTehnicalExamRequired", roleName)
    Else
      AuthorizationRules.DenyWrite("IsTehnicalExamRequired", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsTehnicalExamRequired")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsPayRequired") Then
      AuthorizationRules.AllowWrite("IsPayRequired", roleName)
    Else
      AuthorizationRules.DenyWrite("IsPayRequired", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsPayRequired")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsNewRegistration") Then
      AuthorizationRules.AllowWrite("IsNewRegistration", roleName)
    Else
      AuthorizationRules.DenyWrite("IsNewRegistration", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsNewRegistration")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsRelationDeleted") Then
      AuthorizationRules.AllowWrite("IsRelationDeleted", roleName)
    Else
      AuthorizationRules.DenyWrite("IsRelationDeleted", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsRelationDeleted")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsVehicleDeleted") Then
      AuthorizationRules.AllowWrite("IsVehicleDeleted", roleName)
    Else
      AuthorizationRules.DenyWrite("IsVehicleDeleted", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsVehicleDeleted")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsNewCustomer") Then
      AuthorizationRules.AllowWrite("IsNewCustomer", roleName)
    Else
      AuthorizationRules.DenyWrite("IsNewCustomer", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsNewCustomer")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsVehicleChanged") Then
      AuthorizationRules.AllowWrite("IsVehicleChanged", roleName)
    Else
      AuthorizationRules.DenyWrite("IsVehicleChanged", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsVehicleChanged")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsCustomerChanged") Then
      AuthorizationRules.AllowWrite("IsCustomerChanged", roleName)
    Else
      AuthorizationRules.DenyWrite("IsCustomerChanged", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsCustomerChanged")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TypeName") Then
      AuthorizationRules.AllowWrite("TypeName", roleName)
    Else
      AuthorizationRules.DenyWrite("TypeName", roleName)
    End If
    'AuthorizationRules.AllowWrite("TypeName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TypeDescription") Then
      AuthorizationRules.AllowWrite("TypeDescription", roleName)
    Else
      AuthorizationRules.DenyWrite("TypeDescription", roleName)
    End If
    'AuthorizationRules.AllowWrite("TypeDescription")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("RequestType")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("RequestType")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("RequestType")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("RequestType")
    End Function
#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' TypeNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, TypeNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(TypeNameProperty, 250))
    ' TypeDescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(TypeDescriptionProperty, 250))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

    Public Shared Function NewRequestType() As RequestType
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a RequestType")
        End If
        Return DataPortal.Create(Of RequestType)()
    End Function

    Public Shared Function GetRequestType(ByVal id As Integer) As RequestType
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a RequestType")
        End If
        Return DataPortal.Fetch(New SingleCriteria(Of RequestType, Integer)(Id))
    End Function

    Public Shared Sub DeleteRequestType(ByVal id As Integer)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a RequestType")
        End If
        DataPortal.Delete(New SingleCriteria(Of RequestType, Integer)(Id))
    End Sub

  Public Overrides Function Save() As RequestType
        If IsDeleted AndAlso Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a RequestType")
        ElseIf IsNew AndAlso Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a RequestType")
        ElseIf Not CanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a RequestType")
        End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewRequestTypeChild() As RequestType
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a RequestType")
        End If
        Return DataPortal.CreateChild(Of RequestType)()
  End Function

  Friend Shared Function GetRequestType(ByVal dr As SafeDataReader) As RequestType
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a RequestType")
        End If
        Return DataPortal.FetchChild(Of RequestType)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of RequestType, Integer))
    Database.LogInfo("RequestType.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of Integer)(IdRequestTypeProperty, dr.GetInt32("IdRequestType"))
            LoadProperty(Of Integer)(IdDocumentPrintProperty, dr.GetInt32("IdDocumentPrint"))
                        LoadProperty(Of Integer)(IsTehnicalExamRequiredProperty, dr.GetInt32("IsTehnicalExamRequired"))
            LoadProperty(Of Boolean)(IsPayRequiredProperty, dr.GetBoolean("IsPayRequired"))
            LoadProperty(Of Boolean)(IsNewRegistrationProperty, dr.GetBoolean("IsNewRegistration"))
            LoadProperty(Of Boolean)(IsRelationDeletedProperty, dr.GetBoolean("IsRelationDeleted"))
            LoadProperty(Of Boolean)(IsVehicleDeletedProperty, dr.GetBoolean("IsVehicleDeleted"))
            LoadProperty(Of Boolean)(IsNewCustomerProperty, dr.GetBoolean("IsNewCustomer"))
            LoadProperty(Of Boolean)(IsVehicleChangedProperty, dr.GetBoolean("IsVehicleChanged"))
            LoadProperty(Of Boolean)(IsCustomerChangedProperty, dr.GetBoolean("IsCustomerChanged"))
            LoadProperty(Of Boolean)(IsSufficientProperty, dr.GetBoolean("IsSufficient"))
            LoadProperty(Of Boolean)(IsPreviosRegistrationReqiredProperty, dr.GetBoolean("IsPreviosRegistrationReqired"))
            LoadProperty(Of String)(TypeNameProperty, dr.GetString("TypeName"))
            LoadProperty(Of String)(TypeDescriptionProperty, dr.GetString("TypeDescription"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("RequestType.DataPortal_Fetch", ex)
      Throw New DbCslaException("RequestType.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@IdRequestType", ReadProperty(Of Integer)(IdRequestTypeProperty))
            .Parameters.AddWithValue("@IdDocumentPrint", ReadProperty(Of Integer)(IdDocumentPrintProperty))
                        .Parameters.AddWithValue("@IsTehnicalExamRequired", ReadProperty(Of Integer)(IsTehnicalExamRequiredProperty))
            .Parameters.AddWithValue("@IsPayRequired", ReadProperty(Of Boolean)(IsPayRequiredProperty))
            .Parameters.AddWithValue("@IsNewRegistration", ReadProperty(Of Boolean)(IsNewRegistrationProperty))
            .Parameters.AddWithValue("@IsRelationDeleted", ReadProperty(Of Boolean)(IsRelationDeletedProperty))
            .Parameters.AddWithValue("@IsVehicleDeleted", ReadProperty(Of Boolean)(IsVehicleDeletedProperty))
            .Parameters.AddWithValue("@IsNewCustomer", ReadProperty(Of Boolean)(IsNewCustomerProperty))
            .Parameters.AddWithValue("@IsVehicleChanged", ReadProperty(Of Boolean)(IsVehicleChangedProperty))
            .Parameters.AddWithValue("@IsCustomerChanged", ReadProperty(Of Boolean)(IsCustomerChangedProperty))
            .Parameters.AddWithValue("@IsSufficient", ReadProperty(Of Boolean)(IsSufficientProperty))
            .Parameters.AddWithValue("@IsPreviosRegistrationReqired", ReadProperty(Of Boolean)(IsPreviosRegistrationReqiredProperty))
            .Parameters.AddWithValue("@TypeName", ReadProperty(Of String)(TypeNameProperty))
            .Parameters.AddWithValue("@TypeDescription", ReadProperty(Of String)(TypeDescriptionProperty))

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
      Database.LogException("RequestType.DataPortal_Insert", ex)
      Throw New DbCslaException("RequestType.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("RequestType.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@IdRequestType", ReadProperty(Of Integer)(IdRequestTypeProperty))
            .Parameters.AddWithValue("@IdDocumentPrint", ReadProperty(Of Integer)(IdDocumentPrintProperty))
                        .Parameters.AddWithValue("@IsTehnicalExamRequired", ReadProperty(Of Integer)(IsTehnicalExamRequiredProperty))
            .Parameters.AddWithValue("@IsPayRequired", ReadProperty(Of Boolean)(IsPayRequiredProperty))
            .Parameters.AddWithValue("@IsNewRegistration", ReadProperty(Of Boolean)(IsNewRegistrationProperty))
            .Parameters.AddWithValue("@IsRelationDeleted", ReadProperty(Of Boolean)(IsRelationDeletedProperty))
            .Parameters.AddWithValue("@IsVehicleDeleted", ReadProperty(Of Boolean)(IsVehicleDeletedProperty))
            .Parameters.AddWithValue("@IsNewCustomer", ReadProperty(Of Boolean)(IsNewCustomerProperty))
            .Parameters.AddWithValue("@IsVehicleChanged", ReadProperty(Of Boolean)(IsVehicleChangedProperty))
            .Parameters.AddWithValue("@IsCustomerChanged", ReadProperty(Of Boolean)(IsCustomerChangedProperty))
            .Parameters.AddWithValue("@IsSufficient", ReadProperty(Of Boolean)(IsSufficientProperty))
            .Parameters.AddWithValue("@IsPreviosRegistrationReqired", ReadProperty(Of Boolean)(IsPreviosRegistrationReqiredProperty))
            .Parameters.AddWithValue("@TypeName", ReadProperty(Of String)(TypeNameProperty))
            .Parameters.AddWithValue("@TypeDescription", ReadProperty(Of String)(TypeDescriptionProperty))
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
    DataPortal_Delete(New SingleCriteria(Of RequestType, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of RequestType, Integer))
    Database.LogInfo("RequestType.DataPortal_Delete", GetHashCode())
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
      Database.LogException("RequestType.DataPortal_Delete", ex)
      Throw New DbCslaException("RequestType.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "
  '<RunLocal()> _
  'Private Overloads Sub Child_Create()
  '  ValidationRules.CheckRules()
  'End Sub

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("RequestType.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdRequestTypeProperty, dr.GetInt32("IdRequestType"))
      LoadProperty(Of Integer)(IdDocumentPrintProperty, dr.GetInt32("IdDocumentPrint"))
            LoadProperty(Of Integer)(IsTehnicalExamRequiredProperty, dr.GetInt32("IsTehnicalExamRequired"))
      LoadProperty(Of Boolean)(IsPayRequiredProperty, dr.GetBoolean("IsPayRequired"))
      LoadProperty(Of Boolean)(IsNewRegistrationProperty, dr.GetBoolean("IsNewRegistration"))
      LoadProperty(Of Boolean)(IsRelationDeletedProperty, dr.GetBoolean("IsRelationDeleted"))
      LoadProperty(Of Boolean)(IsVehicleDeletedProperty, dr.GetBoolean("IsVehicleDeleted"))
      LoadProperty(Of Boolean)(IsNewCustomerProperty, dr.GetBoolean("IsNewCustomer"))
      LoadProperty(Of Boolean)(IsVehicleChangedProperty, dr.GetBoolean("IsVehicleChanged"))
      LoadProperty(Of Boolean)(IsCustomerChangedProperty, dr.GetBoolean("IsCustomerChanged"))
      LoadProperty(Of Boolean)(IsSufficientProperty, dr.GetBoolean("IsSufficient"))
      LoadProperty(Of Boolean)(IsPreviosRegistrationReqiredProperty, dr.GetBoolean("IsPreviosRegistrationReqired"))
      LoadProperty(Of String)(TypeNameProperty, dr.GetString("TypeName"))
      LoadProperty(Of String)(TypeDescriptionProperty, dr.GetString("TypeDescription"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("RequestType.Child_Fetch", ex)
      Throw New DbCslaException("RequestType.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@IdRequestType", ReadProperty(Of Integer)(IdRequestTypeProperty))
            .Parameters.AddWithValue("@IdDocumentPrint", ReadProperty(Of Integer)(IdDocumentPrintProperty))
                        .Parameters.AddWithValue("@IsTehnicalExamRequired", ReadProperty(Of Integer)(IsTehnicalExamRequiredProperty))
            .Parameters.AddWithValue("@IsPayRequired", ReadProperty(Of Boolean)(IsPayRequiredProperty))
            .Parameters.AddWithValue("@IsNewRegistration", ReadProperty(Of Boolean)(IsNewRegistrationProperty))
            .Parameters.AddWithValue("@IsRelationDeleted", ReadProperty(Of Boolean)(IsRelationDeletedProperty))
            .Parameters.AddWithValue("@IsVehicleDeleted", ReadProperty(Of Boolean)(IsVehicleDeletedProperty))
            .Parameters.AddWithValue("@IsNewCustomer", ReadProperty(Of Boolean)(IsNewCustomerProperty))
            .Parameters.AddWithValue("@IsVehicleChanged", ReadProperty(Of Boolean)(IsVehicleChangedProperty))
            .Parameters.AddWithValue("@IsCustomerChanged", ReadProperty(Of Boolean)(IsCustomerChangedProperty))
            .Parameters.AddWithValue("@IsSufficient", ReadProperty(Of Boolean)(IsSufficientProperty))
            .Parameters.AddWithValue("@IsPreviosRegistrationReqired", ReadProperty(Of Boolean)(IsPreviosRegistrationReqiredProperty))
            .Parameters.AddWithValue("@TypeName", ReadProperty(Of String)(TypeNameProperty))
            .Parameters.AddWithValue("@TypeDescription", ReadProperty(Of String)(TypeDescriptionProperty))

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
        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("RequestType.Child_Insert", ex)
      Throw New DbCslaException("RequestType.Child_Insert", ex)
    Finally
      Database.LogInfo("RequestType.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("RequestType.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext.Remove("cn")
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@IdRequestType", ReadProperty(Of Integer)(IdRequestTypeProperty))
            .Parameters.AddWithValue("@IdDocumentPrint", ReadProperty(Of Integer)(IdDocumentPrintProperty))
                        .Parameters.AddWithValue("@IsTehnicalExamRequired", ReadProperty(Of Integer)(IsTehnicalExamRequiredProperty))
            .Parameters.AddWithValue("@IsPayRequired", ReadProperty(Of Boolean)(IsPayRequiredProperty))
            .Parameters.AddWithValue("@IsNewRegistration", ReadProperty(Of Boolean)(IsNewRegistrationProperty))
            .Parameters.AddWithValue("@IsRelationDeleted", ReadProperty(Of Boolean)(IsRelationDeletedProperty))
            .Parameters.AddWithValue("@IsVehicleDeleted", ReadProperty(Of Boolean)(IsVehicleDeletedProperty))
            .Parameters.AddWithValue("@IsNewCustomer", ReadProperty(Of Boolean)(IsNewCustomerProperty))
            .Parameters.AddWithValue("@IsVehicleChanged", ReadProperty(Of Boolean)(IsVehicleChangedProperty))
            .Parameters.AddWithValue("@IsCustomerChanged", ReadProperty(Of Boolean)(IsCustomerChangedProperty))
            .Parameters.AddWithValue("@IsPreviosRegistrationReqired", ReadProperty(Of Boolean)(IsPreviosRegistrationReqiredProperty))
            .Parameters.AddWithValue("@IsSufficient", ReadProperty(Of Boolean)(IsSufficientProperty))
            .Parameters.AddWithValue("@TypeName", ReadProperty(Of String)(TypeNameProperty))
            .Parameters.AddWithValue("@TypeDescription", ReadProperty(Of String)(TypeDescriptionProperty))
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
      Database.LogException("RequestType.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("RequestType.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("RequestType.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("RequestType.Child_Fetch", ex)
      Throw New DbCslaException("RequestType.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
