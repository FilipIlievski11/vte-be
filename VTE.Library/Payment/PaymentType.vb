
<Serializable()> _
Public Class PaymentType
 Inherits Csla.BusinessBase(Of PaymentType)


#Region " Stored Procedures Names "
 Private Const spGetByID As String = "GetPaymentTypeByID"
 Private Const spGetAll As String = "GetPaymentTypes"
 Private Const spUpdate As String = "updatePaymentType"
 Private Const spAdd As String = "addPaymentType"
 Private Const spDelete As String = "deletePaymentType"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
 'register properties
 Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentType), New PropertyInfo(Of Integer)("Id"))
 Private Shared IdCompanyProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentType), New PropertyInfo(Of Integer)("IdCompany"))
 Private Shared NameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(PaymentType), New PropertyInfo(Of String)("Name"))
 Private Shared FiskalnaKesProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentType), New PropertyInfo(Of Boolean)("FiskalnaKes"))
 Private Shared FiskalnaKartickaProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentType), New PropertyInfo(Of Boolean)("FiskalnaKarticka"))
 Private Shared RatiProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentType), New PropertyInfo(Of Boolean)("Rati"))
 Private Shared SmetkaProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentType), New PropertyInfo(Of Boolean)("Smetka"))
 Private Shared FakturaProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentType), New PropertyInfo(Of Boolean)("Faktura"))
 Private Shared PrintTextProperty As PropertyInfo(Of String) = _
   RegisterProperty(Of String)(GetType(PaymentType), New PropertyInfo(Of String)("PrintText"))
 Private Shared PrefixProperty As PropertyInfo(Of String) = _
   RegisterProperty(Of String)(GetType(PaymentType), New PropertyInfo(Of String)("Prefix"))
 Private Shared PayedAmountProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentType), New PropertyInfo(Of Boolean)("PayedAmount"))


 Private _lastChanged(7) As Byte

 <System.ComponentModel.DataObjectField(True, True)> _
 Public ReadOnly Property Id() As Integer
  Get
   Return GetProperty(Of Integer)(IdProperty)
  End Get
 End Property

 Public Property IdCompany() As Integer
  Get
   Return GetProperty(Of Integer)(IdCompanyProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdCompanyProperty, value)
  End Set
 End Property
 Public Property Name() As String
  Get
   Return GetProperty(Of String)(NameProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(NameProperty, value)
  End Set
 End Property
 Public Property FiskalnaKes() As Boolean
  Get
   Return GetProperty(Of Boolean)(FiskalnaKesProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(FiskalnaKesProperty, value)
  End Set
 End Property
 Public Property FiskalnaKarticka() As Boolean
  Get
   Return GetProperty(Of Boolean)(FiskalnaKartickaProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(FiskalnaKartickaProperty, value)
  End Set
 End Property
 Public Property Rati() As Boolean
  Get
   Return GetProperty(Of Boolean)(RatiProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(RatiProperty, value)
  End Set
 End Property
 Public Property Smetka() As Boolean
  Get
   Return GetProperty(Of Boolean)(SmetkaProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(SmetkaProperty, value)
  End Set
 End Property
 Public Property Faktura() As Boolean
  Get
   Return GetProperty(Of Boolean)(FakturaProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(FakturaProperty, value)
  End Set
 End Property
 Public Property PrintText() As String
  Get
   Return GetProperty(Of String)(PrintTextProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(PrintTextProperty, value)
  End Set
 End Property
 Public Property Prefix() As String
  Get
   Return GetProperty(Of String)(PrefixProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(PrefixProperty, value)
  End Set
 End Property
 Public Property PayedAmount() As Boolean
  Get
   Return GetProperty(Of Boolean)(PayedAmountProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(PayedAmountProperty, value)
  End Set
 End Property

 Public Overrides Function ToString() As String
  Return Id.ToString
 End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

 Protected Overrides Sub AddAuthorizationRules()
  Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Name") Then
   AuthorizationRules.AllowWrite("Name", roleName)
  Else
   AuthorizationRules.DenyWrite("Name", roleName)
  End If
  'AuthorizationRules.AllowWrite("Name")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("FiskalnaKes") Then
   AuthorizationRules.AllowWrite("FiskalnaKes", roleName)
  Else
   AuthorizationRules.DenyWrite("FiskalnaKes", roleName)
  End If
  'AuthorizationRules.AllowWrite("FiskalnaKes")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("FiskalnaKarticka") Then
   AuthorizationRules.AllowWrite("FiskalnaKarticka", roleName)
  Else
   AuthorizationRules.DenyWrite("FiskalnaKarticka", roleName)
  End If
  'AuthorizationRules.AllowWrite("FiskalnaKarticka")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Rati") Then
   AuthorizationRules.AllowWrite("Rati", roleName)
  Else
   AuthorizationRules.DenyWrite("Rati", roleName)
  End If
  'AuthorizationRules.AllowWrite("Rati")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Smetka") Then
   AuthorizationRules.AllowWrite("Smetka", roleName)
  Else
   AuthorizationRules.DenyWrite("Smetka", roleName)
  End If
  'AuthorizationRules.AllowWrite("Smetka")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Faktura") Then
   AuthorizationRules.AllowWrite("Faktura", roleName)
  Else
   AuthorizationRules.DenyWrite("Faktura", roleName)
  End If
  'AuthorizationRules.AllowWrite("Faktura")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
   AuthorizationRules.AllowWrite("Active", roleName)
  Else
   AuthorizationRules.DenyWrite("Active", roleName)
  End If
  'AuthorizationRules.AllowWrite("Active")
 End Sub



 Public Shared Function CanGetObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("PaymentType")
 End Function

 Public Shared Function CanAddObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("PaymentType")
 End Function

 Public Shared Function CanEditObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("PaymentType")
 End Function

 Public Shared Function CanDeleteObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("PaymentType")
 End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
 Protected Overrides Sub AddBusinessRules()
  ' NameProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, NameProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NameProperty, 50))

  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(PrefixProperty, 15))
 End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

 Private Sub New()
  ' require use of factory method 
 End Sub

 Public Shared Function NewPaymentType() As PaymentType
  If Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a PaymentType")
  End If
  Return DataPortal.Create(Of PaymentType)()
 End Function

 Public Shared Function GetPaymentType(ByVal id As Integer) As PaymentType
  If Not CanGetObject() Then
   Throw New System.Security.SecurityException("User not authorized to view a PaymentType")
  End If
  Return DataPortal.Fetch(New SingleCriteria(Of PaymentType, Integer)(id))
 End Function

 Public Shared Sub DeletePaymentType(ByVal id As Integer)
  If Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a PaymentType")
  End If
  DataPortal.Delete(New SingleCriteria(Of PaymentType, Integer)(id))
 End Sub

 Public Overrides Function Save() As PaymentType
  If IsDeleted AndAlso Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a PaymentType")
  ElseIf IsNew AndAlso Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a PaymentType")
  ElseIf Not CanEditObject() Then
   Throw New System.Security.SecurityException("User not authorized to update a PaymentType")
  End If
  Return MyBase.Save()
 End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

 Friend Shared Function NewPaymentTypeChild() As PaymentType
  Return DataPortal.CreateChild(Of PaymentType)()
 End Function

 Friend Shared Function GetPaymentType(ByVal dr As SafeDataReader) As PaymentType
  Return DataPortal.FetchChild(Of PaymentType)(dr)
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

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of PaymentType, Integer))
  Database.LogInfo("PaymentType.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = spGetByID
     cm.Parameters.AddWithValue("@Id", criteria.Value)
     Using dr As New SafeDataReader(cm.ExecuteReader)
      dr.Read()

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdCompanyProperty, dr.GetInt32("IdCompany"))
      LoadProperty(Of String)(NameProperty, dr.GetString("Name"))
      LoadProperty(Of Boolean)(FiskalnaKesProperty, dr.GetBoolean("Fiskalna_kes"))
      LoadProperty(Of Boolean)(FiskalnaKartickaProperty, dr.GetBoolean("Fiskalna_karticka"))
      LoadProperty(Of Boolean)(RatiProperty, dr.GetBoolean("Rati"))
      LoadProperty(Of Boolean)(SmetkaProperty, dr.GetBoolean("Smetka"))
      LoadProperty(Of Boolean)(FakturaProperty, dr.GetBoolean("Faktura"))
      LoadProperty(Of String)(PrintTextProperty, dr.GetString("PrintText"))
      LoadProperty(Of String)(PrefixProperty, dr.GetString("Prefix"))
      LoadProperty(Of Boolean)(PayedAmountProperty, dr.GetBoolean("PayedAmount"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
     End Using
    End Using
   End Using

  Catch ex As Exception
   Database.LogException("PaymentType.DataPortal_Fetch", ex)
   Throw New DbCslaException("PaymentType.DataPortal_Fetch", ex)
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
      Dim tehOrg As TehnicalExamOrganizationsInfo = Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization")

      .Parameters.AddWithValue("@IdCompany", tehOrg.IdCompany)
      .Parameters.AddWithValue("@Name", ReadProperty(Of String)(NameProperty))
      .Parameters.AddWithValue("@Fiskalna_kes", ReadProperty(Of Boolean)(FiskalnaKesProperty))
      .Parameters.AddWithValue("@Fiskalna_karticka", ReadProperty(Of Boolean)(FiskalnaKartickaProperty))
      .Parameters.AddWithValue("@Rati", ReadProperty(Of Boolean)(RatiProperty))
      .Parameters.AddWithValue("@Smetka", ReadProperty(Of Boolean)(SmetkaProperty))
      .Parameters.AddWithValue("@Faktura", ReadProperty(Of Boolean)(FakturaProperty))
      .Parameters.AddWithValue("@PrintText", ReadProperty(Of String)(PrintTextProperty))
      .Parameters.AddWithValue("@Prefix", ReadProperty(Of String)(PrefixProperty))
      .Parameters.AddWithValue("@PayedAmount", ReadProperty(Of Boolean)(PayedAmountProperty))

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
   Database.LogException("PaymentType.DataPortal_Insert", ex)
   Throw New DbCslaException("PaymentType.DataPortal_Insert", ex)
  Finally
   Database.LogInfo("PaymentType.DataPortal_Insert", GetHashCode())
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
      .Parameters.AddWithValue("@IdCompany", ReadProperty(Of Integer)(IdCompanyProperty))
      .Parameters.AddWithValue("@Name", ReadProperty(Of String)(NameProperty))
      .Parameters.AddWithValue("@Fiskalna_kes", ReadProperty(Of Boolean)(FiskalnaKesProperty))
      .Parameters.AddWithValue("@Fiskalna_karticka", ReadProperty(Of Boolean)(FiskalnaKartickaProperty))
      .Parameters.AddWithValue("@Rati", ReadProperty(Of Boolean)(RatiProperty))
      .Parameters.AddWithValue("@Smetka", ReadProperty(Of Boolean)(SmetkaProperty))
      .Parameters.AddWithValue("@Faktura", ReadProperty(Of Boolean)(FakturaProperty))
      .Parameters.AddWithValue("@PrintText", ReadProperty(Of String)(PrintTextProperty))
      .Parameters.AddWithValue("@Prefix", ReadProperty(Of String)(PrefixProperty))
      .Parameters.AddWithValue("@PayedAmount", ReadProperty(Of Boolean)(PayedAmountProperty))
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
  DataPortal_Delete(New SingleCriteria(Of PaymentType, Integer)(Id))
 End Sub

 Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of PaymentType, Integer))
  Database.LogInfo("PaymentType.DataPortal_Delete", GetHashCode())
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
   Database.LogException("PaymentType.DataPortal_Delete", ex)
   Throw New DbCslaException("PaymentType.DataPortal_Delete", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

 Private Sub Child_Fetch(ByVal dr As SafeDataReader)
  Database.LogInfo("PaymentType.Child_Fetch", GetHashCode())
  Try
   LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
   LoadProperty(Of Integer)(IdCompanyProperty, dr.GetInt32("IdCompany"))
   LoadProperty(Of String)(NameProperty, dr.GetString("Name"))
   LoadProperty(Of Boolean)(FiskalnaKesProperty, dr.GetBoolean("Fiskalna_kes"))
   LoadProperty(Of Boolean)(FiskalnaKartickaProperty, dr.GetBoolean("Fiskalna_karticka"))
   LoadProperty(Of Boolean)(RatiProperty, dr.GetBoolean("Rati"))
   LoadProperty(Of Boolean)(SmetkaProperty, dr.GetBoolean("Smetka"))
   LoadProperty(Of Boolean)(FakturaProperty, dr.GetBoolean("Faktura"))
   LoadProperty(Of String)(PrintTextProperty, dr.GetString("PrintText"))
   LoadProperty(Of String)(PrefixProperty, dr.GetString("Prefix"))
   LoadProperty(Of Boolean)(PayedAmountProperty, dr.GetBoolean("PayedAmount"))
   dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
  Catch ex As Exception
   Database.LogException("PaymentType.Child_Fetch", ex)
   Throw New DbCslaException("PaymentType.Child_Fetch", ex)
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
      Dim tehOrg As TehnicalExamOrganizationsInfo = Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization")

      .Parameters.AddWithValue("@IdCompany", tehOrg.IdCompany)
      .Parameters.AddWithValue("@Name", ReadProperty(Of String)(NameProperty))
      .Parameters.AddWithValue("@Fiskalna_kes", ReadProperty(Of Boolean)(FiskalnaKesProperty))
      .Parameters.AddWithValue("@Fiskalna_karticka", ReadProperty(Of Boolean)(FiskalnaKartickaProperty))
      .Parameters.AddWithValue("@Rati", ReadProperty(Of Boolean)(RatiProperty))
      .Parameters.AddWithValue("@Smetka", ReadProperty(Of Boolean)(SmetkaProperty))
      .Parameters.AddWithValue("@Faktura", ReadProperty(Of Boolean)(FakturaProperty))
      .Parameters.AddWithValue("@PrintText", ReadProperty(Of String)(PrintTextProperty))
      .Parameters.AddWithValue("@Prefix", ReadProperty(Of String)(PrefixProperty))
      .Parameters.AddWithValue("@PayedAmount", ReadProperty(Of Boolean)(PayedAmountProperty))
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
   Database.LogException("PaymentType.Child_Insert", ex)
   Throw New DbCslaException("PaymentType.Child_Insert", ex)
  Finally
   Database.LogInfo("PaymentType.Child_Insert", GetHashCode)
  End Try

 End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

 Private Sub Child_Update()
  Database.LogInfo("PaymentType.Child_Update", GetHashCode)
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spUpdate

      .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
      .Parameters.AddWithValue("@IdCompany", ReadProperty(Of Integer)(IdCompanyProperty))
      .Parameters.AddWithValue("@Name", ReadProperty(Of String)(NameProperty))
      .Parameters.AddWithValue("@Fiskalna_kes", ReadProperty(Of Boolean)(FiskalnaKesProperty))
      .Parameters.AddWithValue("@Fiskalna_karticka", ReadProperty(Of Boolean)(FiskalnaKartickaProperty))
      .Parameters.AddWithValue("@Rati", ReadProperty(Of Boolean)(RatiProperty))
      .Parameters.AddWithValue("@Smetka", ReadProperty(Of Boolean)(SmetkaProperty))
      .Parameters.AddWithValue("@Faktura", ReadProperty(Of Boolean)(FakturaProperty))
      .Parameters.AddWithValue("@PrintText", ReadProperty(Of String)(PrintTextProperty))
      .Parameters.AddWithValue("@Prefix", ReadProperty(Of String)(PrefixProperty))
      .Parameters.AddWithValue("@PayedAmount", ReadProperty(Of Boolean)(PayedAmountProperty))
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
   Database.LogException("PaymentType.Child_Update", ex)
   If Not ex.Message.EndsWith("drug korisnik") Then
    Throw New DbCslaException("PaymentType.Child_Update", ex)
   End If
  End Try
 End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

 Private Sub Child_DeleteSelf()

  Database.LogInfo("PaymentType.Child_DeleteSelf", GetHashCode)
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
   Database.LogException("PaymentType.Child_Fetch", ex)
   Throw New DbCslaException("PaymentType.Child_Fetch", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
