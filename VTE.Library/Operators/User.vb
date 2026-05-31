
<Serializable()> _
Public Class User
 Inherits Csla.BusinessBase(Of User)


#Region " Stored Procedures Names "
 Private Const spGetByID As String = "GetUserByID"
 Private Const spGetAll As String = "GetUsers"
 Private Const spUpdate As String = "updateUser"
 Private Const spAdd As String = "addUser"
 Private Const spDelete As String = "deleteUser"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
 'register properties
 Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(User), New PropertyInfo(Of Long)("Id"))
 Private Shared IdRoleProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(User), New PropertyInfo(Of Integer)("IdRole"))
 Private Shared IdDataBaseProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(User), New PropertyInfo(Of Integer)("IdDataBase"))
 Private Shared IdStationProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(User), New PropertyInfo(Of Integer)("IdStation"))
 Private Shared UserFullNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(User), New PropertyInfo(Of String)("UserFullName"))
 Private Shared UserNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(User), New PropertyInfo(Of String)("UserName"))
 Private Shared UserPassProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(User), New PropertyInfo(Of String)("UserPass"))
 Private Shared FirstNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(User), New PropertyInfo(Of String)("FirstName"))
 Private Shared SureNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(User), New PropertyInfo(Of String)("SureName"))
 Private Shared AddressProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(User), New PropertyInfo(Of String)("Address"))
 Private Shared EmbgProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(User), New PropertyInfo(Of String)("Embg"))
 Private Shared BlkProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(User), New PropertyInfo(Of String)("Blk"))
 Private Shared DateOfBirthProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(User), New PropertyInfo(Of SmartDate)("DateOfBirth", New SmartDate(True)))
 Private Shared DateOfHireingProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(User), New PropertyInfo(Of SmartDate)("DateOfHireing", New SmartDate(True)))
 Private Shared RfidProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(User), New PropertyInfo(Of String)("Rfid"))

 Private _lastChanged(7) As Byte

 <System.ComponentModel.DataObjectField(True, True)> _
 Public ReadOnly Property Id() As Long
  Get
   Return GetProperty(Of Long)(IdProperty)
  End Get
 End Property
 Public Property IdRole() As Integer
  Get
   Return GetProperty(Of Integer)(IdRoleProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdRoleProperty, value)
  End Set
 End Property
 Public Property IdDataBase() As Integer
  Get
   Return GetProperty(Of Integer)(IdDataBaseProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdDataBaseProperty, value)
  End Set
 End Property
 Public Property IdStation() As Integer
  Get
   Return GetProperty(Of Integer)(IdStationProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdStationProperty, value)
  End Set
 End Property
 Public Property UserFullName() As String
  Get
   Return GetProperty(Of String)(UserFullNameProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(UserFullNameProperty, value)
  End Set
 End Property
 Public Property UserName() As String
  Get
   Return GetProperty(Of String)(UserNameProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(UserNameProperty, value)
  End Set
 End Property
 Public Property UserPass() As String
  Get
   Return GetProperty(Of String)(UserPassProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(UserPassProperty, value)
  End Set
 End Property
 Public Property FirstName() As String
  Get
   Return GetProperty(Of String)(FirstNameProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(FirstNameProperty, value)
  End Set
 End Property
 Public Property SureName() As String
  Get
   Return GetProperty(Of String)(SureNameProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(SureNameProperty, value)
  End Set
 End Property
 Public Property Address() As String
  Get
   Return GetProperty(Of String)(AddressProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(AddressProperty, value)
  End Set
 End Property
 Public Property Embg() As String
  Get
   Return GetProperty(Of String)(EmbgProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(EmbgProperty, value)
  End Set
 End Property
 Public Property Blk() As String
  Get
   Return GetProperty(Of String)(BlkProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(BlkProperty, value)
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
 Public Property DateOfHireing() As Date
  Get
   Return GetProperty(Of SmartDate, Date)(DateOfHireingProperty)
  End Get
  Set(ByVal value As Date)
   SetProperty(Of SmartDate, Date)(DateOfHireingProperty, value)
  End Set
 End Property
 Public Property Rfid() As String
  Get
   Return GetProperty(Of String)(RfidProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(RfidProperty, value)
  End Set
 End Property

 Public Overrides Function ToString() As String
  Return Id.ToString
 End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

 Protected Overrides Sub AddAuthorizationRules()
  Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdRole") Then
   AuthorizationRules.AllowWrite("IdRole", roleName)
  Else
   AuthorizationRules.DenyWrite("IdRole", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdRole")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdDataBase") Then
   AuthorizationRules.AllowWrite("IdDataBase", roleName)
  Else
   AuthorizationRules.DenyWrite("IdDataBase", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdDataBase")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("UserFullName") Then
   AuthorizationRules.AllowWrite("UserFullName", roleName)
  Else
   AuthorizationRules.DenyWrite("UserFullName", roleName)
  End If
  'AuthorizationRules.AllowWrite("UserFullName")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("UserName") Then
   AuthorizationRules.AllowWrite("UserName", roleName)
  Else
   AuthorizationRules.DenyWrite("UserName", roleName)
  End If
  'AuthorizationRules.AllowWrite("UserName")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("UserPass") Then
   AuthorizationRules.AllowWrite("UserPass", roleName)
  Else
   AuthorizationRules.DenyWrite("UserPass", roleName)
  End If
  'AuthorizationRules.AllowWrite("UserPass")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("FirstName") Then
   AuthorizationRules.AllowWrite("FirstName", roleName)
  Else
   AuthorizationRules.DenyWrite("FirstName", roleName)
  End If
  'AuthorizationRules.AllowWrite("FirstName")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("SureName") Then
   AuthorizationRules.AllowWrite("SureName", roleName)
  Else
   AuthorizationRules.DenyWrite("SureName", roleName)
  End If
  'AuthorizationRules.AllowWrite("SureName")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Address") Then
   AuthorizationRules.AllowWrite("Address", roleName)
  Else
   AuthorizationRules.DenyWrite("Address", roleName)
  End If
  'AuthorizationRules.AllowWrite("Address")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Embg") Then
   AuthorizationRules.AllowWrite("Embg", roleName)
  Else
   AuthorizationRules.DenyWrite("Embg", roleName)
  End If
  'AuthorizationRules.AllowWrite("Embg")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Blk") Then
   AuthorizationRules.AllowWrite("Blk", roleName)
  Else
   AuthorizationRules.DenyWrite("Blk", roleName)
  End If
  'AuthorizationRules.AllowWrite("Blk")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DateOfBirth") Then
   AuthorizationRules.AllowWrite("DateOfBirth", roleName)
  Else
   AuthorizationRules.DenyWrite("DateOfBirth", roleName)
  End If
  'AuthorizationRules.AllowWrite("DateOfBirth")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DateOfHireing") Then
   AuthorizationRules.AllowWrite("DateOfHireing", roleName)
  Else
   AuthorizationRules.DenyWrite("DateOfHireing", roleName)
  End If
  'AuthorizationRules.AllowWrite("DateOfHireing")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Rfid") Then
   AuthorizationRules.AllowWrite("Rfid", roleName)
  Else
   AuthorizationRules.DenyWrite("Rfid", roleName)
  End If
  'AuthorizationRules.AllowWrite("Rfid")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
   AuthorizationRules.AllowWrite("Active", roleName)
  Else
   AuthorizationRules.DenyWrite("Active", roleName)
  End If
  'AuthorizationRules.AllowWrite("Active")
 End Sub



 Public Shared Function CanGetObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("User")
 End Function

 Public Shared Function CanAddObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("User")
 End Function

 Public Shared Function CanEditObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("User")
 End Function

 Public Shared Function CanDeleteObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("User")
 End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
 Protected Overrides Sub AddBusinessRules()
  ' UserFullNameProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(UserFullNameProperty, 50))
  ' UserNameProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, UserNameProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMinLength, New Csla.Validation.CommonRules.MinLengthRuleArgs(UserNameProperty, 3))
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(UserNameProperty, 50))
  ' UserPassProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, UserPassProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMinLength, New Csla.Validation.CommonRules.MinLengthRuleArgs(UserPassProperty, 7))
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(UserPassProperty, 50))
  ValidationRules.AddRule(Of User)(AddressOf PWDCheck, UserPassProperty)
  ' FirstNameProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(FirstNameProperty, 50))
  ' SureNameProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(SureNameProperty, 50))
  ' AddressProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(AddressProperty, 200))
  ' EmbgProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(EmbgProperty, 13))
  ' BlkProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BlkProperty, 10))
  ' RfidProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(RfidProperty, 50))
  ValidationRules.AddRule(Of User)(AddressOf NoDuplicates, UserNameProperty)
  ValidationRules.AddRule(Of User)(AddressOf NoDuplicates, UserPassProperty)
  ValidationRules.AddDependentProperty(UserNameProperty, UserPassProperty, True)
    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect _
                           , New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdStationProperty, 1))
 End Sub

 Private Shared Function PWDCheck(Of T As User)(ByVal target As T, _
     ByVal e As Csla.Validation.RuleArgs) As Boolean
  If target.UserPass.Contains(".") Or target.UserPass.Contains("!") Or _
  target.UserPass.Contains("@") Or target.UserPass.Contains("#") Or _
  target.UserPass.Contains("$") Or target.UserPass.Contains("%") Or _
  target.UserPass.Contains("^") Or target.UserPass.Contains("&") Or _
  target.UserPass.Contains("*") Or target.UserPass.Contains("(") Or _
  target.UserPass.Contains(")") Or target.UserPass.Contains("[") Or _
  target.UserPass.Contains("]") Or target.UserPass.Contains("{") Or _
  target.UserPass.Contains("}") Then

   Return True
  Else
   e.Description = My.Resources.SpecialChars
   Return False
  End If
 End Function
 Private Shared Function NoDuplicates(Of T As User)(ByVal target As T, _
    ByVal e As Csla.Validation.RuleArgs) As Boolean
  If User.Exists(target.UserName, target.UserPass, target.Id) Then
   e.Description = "Таков корисник веќе постои"
   Return False
  Else
   Return True
  End If
 End Function
#End Region ' Validation Rules

#Region " Factory Methods "

 Private Sub New()
  ' require use of factory method 
 End Sub

 Public Shared Function NewUser() As User
  If Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a User")
  End If
  Return DataPortal.Create(Of User)()
 End Function

 Public Shared Function GetUser(ByVal id As Long) As User
  If Not CanGetObject() Then
   Throw New System.Security.SecurityException("User not authorized to view a User")
  End If
  Return DataPortal.Fetch(New SingleCriteria(Of User, Integer)(id))
 End Function

 Public Shared Sub DeleteUser(ByVal id As Long)
  If Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a User")
  End If
  DataPortal.Delete(New SingleCriteria(Of User, Integer)(id))
 End Sub

 Public Overrides Function Save() As User
  If IsDeleted AndAlso Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a User")
  ElseIf IsNew AndAlso Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a User")
  ElseIf Not CanEditObject() Then
   Throw New System.Security.SecurityException("User not authorized to update a User")
  End If
  Return MyBase.Save()
 End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

 Friend Shared Function NewUserChild() As User
  Return DataPortal.CreateChild(Of User)()
 End Function

 Friend Shared Function GetUser(ByVal dr As SafeDataReader) As User
  Return DataPortal.FetchChild(Of User)(dr)
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

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of User, Integer))
  Database.LogInfo("User.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = spGetByID
     cm.Parameters.AddWithValue("@Id", criteria.Value)
     Using dr As New SafeDataReader(cm.ExecuteReader)
      dr.Read()

      LoadProperty(Of Long)(IdProperty, dr.GetInt64("ID"))
      LoadProperty(Of Integer)(IdRoleProperty, dr.GetInt32("IdRole"))
      LoadProperty(Of Integer)(IdDataBaseProperty, dr.GetInt32("IdDataBase"))
      LoadProperty(Of Integer)(IdStationProperty, dr.GetInt32("IdStation"))
      LoadProperty(Of String)(UserFullNameProperty, dr.GetString("UserFullName"))
      LoadProperty(Of String)(UserNameProperty, dr.GetString("UserName"))
      LoadProperty(Of String)(UserPassProperty, dr.GetString("UserPass"))
      LoadProperty(Of String)(FirstNameProperty, dr.GetString("FirstName"))
      LoadProperty(Of String)(SureNameProperty, dr.GetString("SureName"))
      LoadProperty(Of String)(AddressProperty, dr.GetString("Address"))
      LoadProperty(Of String)(EmbgProperty, dr.GetString("EMBG"))
      LoadProperty(Of String)(BlkProperty, dr.GetString("BLK"))
      LoadProperty(Of SmartDate, Date?)(DateOfBirthProperty, dr.GetSmartDate("DateOfBirth", True))
      LoadProperty(Of SmartDate, Date?)(DateOfHireingProperty, dr.GetSmartDate("DateOfHireing", True))
      LoadProperty(Of String)(RfidProperty, dr.GetString("RFID"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
     End Using
    End Using
   End Using

  Catch ex As Exception
   Database.LogException("User.DataPortal_Fetch", ex)
   Throw New DbCslaException("User.DataPortal_Fetch", ex)
  End Try

 End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

 Protected Overrides Sub DataPortal_Insert()
  Try
   Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spAdd

      .Parameters.AddWithValue("@IdRole", ReadProperty(Of Integer)(IdRoleProperty))
      .Parameters.AddWithValue("@IdDataBase", CType(Csla.ApplicationContext.LocalContext.Item("objCurentUser"), UsersInfo).IdDataBase)
      .Parameters.AddWithValue("@IdStation", ReadProperty(Of Integer)(IdStationProperty))
      .Parameters.AddWithValue("@UserFullName", ReadProperty(Of String)(UserFullNameProperty))
      .Parameters.AddWithValue("@UserName", ReadProperty(Of String)(UserNameProperty))
      .Parameters.AddWithValue("@UserPass", ReadProperty(Of String)(UserPassProperty))
      .Parameters.AddWithValue("@FirstName", ReadProperty(Of String)(FirstNameProperty))
      .Parameters.AddWithValue("@SureName", ReadProperty(Of String)(SureNameProperty))
      .Parameters.AddWithValue("@Address", ReadProperty(Of String)(AddressProperty))
      .Parameters.AddWithValue("@EMBG", ReadProperty(Of String)(EmbgProperty))
      .Parameters.AddWithValue("@BLK", ReadProperty(Of String)(BlkProperty))
      .Parameters.AddWithValue("@DateOfBirth", ReadProperty(Of SmartDate)(DateOfBirthProperty).DBValue)
      .Parameters.AddWithValue("@DateOfHireing", ReadProperty(Of SmartDate)(DateOfHireingProperty).DBValue)
      .Parameters.AddWithValue("@RFID", ReadProperty(Of String)(RfidProperty))

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
    If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
     ApplicationContext.LocalContext.Remove("cn")
    End If
   End Using
  Catch ex As Exception
   Database.LogException("User.DataPortal_Insert", ex)
   Throw New DbCslaException("User.DataPortal_Insert", ex)
  Finally
   Database.LogInfo("User.DataPortal_Insert", GetHashCode())
  End Try
 End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

 Protected Overrides Sub DataPortal_Update()
  Try
   Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spUpdate

      .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
      .Parameters.AddWithValue("@IdRole", ReadProperty(Of Integer)(IdRoleProperty))
      .Parameters.AddWithValue("@IdDataBase", ReadProperty(Of Integer)(IdDataBaseProperty))
      .Parameters.AddWithValue("@IdStation", ReadProperty(Of Integer)(IdStationProperty))
      .Parameters.AddWithValue("@UserFullName", ReadProperty(Of String)(UserFullNameProperty))
      .Parameters.AddWithValue("@UserName", ReadProperty(Of String)(UserNameProperty))
      .Parameters.AddWithValue("@UserPass", ReadProperty(Of String)(UserPassProperty))
      .Parameters.AddWithValue("@FirstName", ReadProperty(Of String)(FirstNameProperty))
      .Parameters.AddWithValue("@SureName", ReadProperty(Of String)(SureNameProperty))
      .Parameters.AddWithValue("@Address", ReadProperty(Of String)(AddressProperty))
      .Parameters.AddWithValue("@EMBG", ReadProperty(Of String)(EmbgProperty))
      .Parameters.AddWithValue("@BLK", ReadProperty(Of String)(BlkProperty))
      .Parameters.AddWithValue("@DateOfBirth", ReadProperty(Of SmartDate)(DateOfBirthProperty).DBValue)
      .Parameters.AddWithValue("@DateOfHireing", ReadProperty(Of SmartDate)(DateOfHireingProperty).DBValue)
      .Parameters.AddWithValue("@RFID", ReadProperty(Of String)(RfidProperty))
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
  DataPortal_Delete(New SingleCriteria(Of User, Integer)(Id))
 End Sub

 Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of User, Integer))
  Database.LogInfo("User.DataPortal_Delete", GetHashCode())
  Try
   Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
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
   Database.LogException("User.DataPortal_Delete", ex)
   Throw New DbCslaException("User.DataPortal_Delete", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

 Private Sub Child_Fetch(ByVal dr As SafeDataReader)
  Database.LogInfo("User.Child_Fetch", GetHashCode())
  Try
      LoadProperty(Of Long)(IdProperty, dr.GetInt64("ID"))
      LoadProperty(Of Integer)(IdRoleProperty, dr.GetInt32("IdRole"))
      LoadProperty(Of Integer)(IdDataBaseProperty, dr.GetInt32("IdDataBase"))
      LoadProperty(Of Integer)(IdStationProperty, dr.GetInt32("IdStation"))
      LoadProperty(Of String)(UserFullNameProperty, dr.GetString("UserFullName"))
      LoadProperty(Of String)(UserNameProperty, dr.GetString("UserName"))
      LoadProperty(Of String)(UserPassProperty, dr.GetString("UserPass"))
      LoadProperty(Of String)(FirstNameProperty, dr.GetString("FirstName"))
      LoadProperty(Of String)(SureNameProperty, dr.GetString("SureName"))
      LoadProperty(Of String)(AddressProperty, dr.GetString("Address"))
      LoadProperty(Of String)(EmbgProperty, dr.GetString("EMBG"))
      LoadProperty(Of String)(BlkProperty, dr.GetString("BLK"))
      LoadProperty(Of SmartDate, Date?)(DateOfBirthProperty, dr.GetSmartDate("DateOfBirth", True))
      LoadProperty(Of SmartDate, Date?)(DateOfHireingProperty, dr.GetSmartDate("DateOfHireing", True))
      LoadProperty(Of String)(RfidProperty, dr.GetString("RFID"))

      ' dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
  Catch ex As Exception
   Database.LogException("User.Child_Fetch", ex)
   Throw New DbCslaException("User.Child_Fetch", ex)
  End Try

 End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

 Private Sub Child_Insert()
  Try
   Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spAdd
      .Parameters.AddWithValue("@IdRole", ReadProperty(Of Integer)(IdRoleProperty))
      .Parameters.AddWithValue("@IdDataBase", CType(Csla.ApplicationContext.LocalContext.Item("objCurentUser"), UsersInfo).IdDataBase)
      .Parameters.AddWithValue("@IdStation", ReadProperty(Of Integer)(IdStationProperty))
      .Parameters.AddWithValue("@UserFullName", ReadProperty(Of String)(UserFullNameProperty))
      .Parameters.AddWithValue("@UserName", ReadProperty(Of String)(UserNameProperty))
      .Parameters.AddWithValue("@UserPass", ReadProperty(Of String)(UserPassProperty))
      .Parameters.AddWithValue("@FirstName", ReadProperty(Of String)(FirstNameProperty))
      .Parameters.AddWithValue("@SureName", ReadProperty(Of String)(SureNameProperty))
      .Parameters.AddWithValue("@Address", ReadProperty(Of String)(AddressProperty))
      .Parameters.AddWithValue("@EMBG", ReadProperty(Of String)(EmbgProperty))
      .Parameters.AddWithValue("@BLK", ReadProperty(Of String)(BlkProperty))
      .Parameters.AddWithValue("@DateOfBirth", ReadProperty(Of SmartDate)(DateOfBirthProperty).DBValue)
      .Parameters.AddWithValue("@DateOfHireing", ReadProperty(Of SmartDate)(DateOfHireingProperty).DBValue)
      .Parameters.AddWithValue("@RFID", ReadProperty(Of String)(RfidProperty))

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
    If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
     ApplicationContext.LocalContext.Remove("cn")
    End If
   End Using
  Catch ex As Exception
   Database.LogException("User.Child_Insert", ex)
   Throw New DbCslaException("User.Child_Insert", ex)
  Finally
   Database.LogInfo("User.Child_Insert", GetHashCode)
  End Try

 End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

 Private Sub Child_Update()
  Database.LogInfo("User.Child_Update", GetHashCode)
  Try
   Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spUpdate

      .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
      .Parameters.AddWithValue("@IdRole", ReadProperty(Of Integer)(IdRoleProperty))
      .Parameters.AddWithValue("@IdDataBase", ReadProperty(Of Integer)(IdDataBaseProperty))
      .Parameters.AddWithValue("@IdStation", ReadProperty(Of Integer)(IdStationProperty))
      .Parameters.AddWithValue("@UserFullName", ReadProperty(Of String)(UserFullNameProperty))
      .Parameters.AddWithValue("@UserName", ReadProperty(Of String)(UserNameProperty))
      .Parameters.AddWithValue("@UserPass", ReadProperty(Of String)(UserPassProperty))
      .Parameters.AddWithValue("@FirstName", ReadProperty(Of String)(FirstNameProperty))
      .Parameters.AddWithValue("@SureName", ReadProperty(Of String)(SureNameProperty))
      .Parameters.AddWithValue("@Address", ReadProperty(Of String)(AddressProperty))
      .Parameters.AddWithValue("@EMBG", ReadProperty(Of String)(EmbgProperty))
      .Parameters.AddWithValue("@BLK", ReadProperty(Of String)(BlkProperty))
      .Parameters.AddWithValue("@DateOfBirth", ReadProperty(Of SmartDate)(DateOfBirthProperty).DBValue)
      .Parameters.AddWithValue("@DateOfHireing", ReadProperty(Of SmartDate)(DateOfHireingProperty).DBValue)
      .Parameters.AddWithValue("@RFID", ReadProperty(Of String)(RfidProperty))
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
   Database.LogException("User.Child_Update", ex)
   If Not ex.Message.EndsWith("drug korisnik") Then
    Throw New DbCslaException("User.Child_Update", ex)
   End If
  End Try
 End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

 Private Sub Child_DeleteSelf()

  Database.LogInfo("User.Child_DeleteSelf", GetHashCode)
  Try
   Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
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
   Database.LogException("User.Child_Fetch", ex)
   Throw New DbCslaException("User.Child_Fetch", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access
#Region " Exists "

 Public Shared Function Exists(ByVal strUserName As String, ByVal strPwd As String, ByVal id As Long) As Boolean

  Return ExistsCommand.Exists(strUserName, strPwd, id)

 End Function

 <Serializable()> _
 Private Class ExistsCommand
  Inherits CommandBase
  Private _userName As String
  Private _userPwd As String
  Private _id As Long
  Private _Exists As Boolean
  Public ReadOnly Property ExistsUser() As Boolean
   Get
    Return _Exists
   End Get
  End Property

  Public Shared Function Exists(ByVal strUserName As String, ByVal strPwd As String, ByVal id As Long) As Boolean
   If strUserName <> String.Empty AndAlso strPwd <> String.Empty Then
    Dim result As ExistsCommand
    result = DataPortal.Execute(Of ExistsCommand)(New ExistsCommand(strUserName, strPwd, id))
    Return result.ExistsUser
   Else
    Return True
   End If
  End Function

  Private Sub New(ByVal strUserName As String, ByVal strPwd As String, ByVal id As Integer)
   _userName = strUserName
   _userPwd = strPwd
   _id = id
   _Exists = False
  End Sub

  Protected Overrides Sub DataPortal_Execute()
   Dim pom As Integer = 0
   Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "CheckUserNamePwd"
     cm.Parameters.AddWithValue("@name", _userName)
     cm.Parameters.AddWithValue("@pwd", _userPwd)
     cm.Parameters.AddWithValue("@id", _id)
     pom = cm.ExecuteScalar
     If pom = 0 Then
      _Exists = False
     Else
      _Exists = True
     End If
    End Using
   End Using
  End Sub

 End Class

#End Region
End Class
