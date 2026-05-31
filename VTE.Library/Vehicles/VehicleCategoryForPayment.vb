
<Serializable()> _
Public Class VehicleCategoryForPayment
 Inherits Csla.BusinessBase(Of VehicleCategoryForPayment)

#Region " Stored Procedures Names "
 Private Const spGetByID As String = "GetVehicleCategoryForPaymentByID"
 Private Const spGetAll As String = "GetVehicleCategoryForPayments"
 Private Const spUpdate As String = "updateVehicleCategoryForPayment"
 Private Const spAdd As String = "addVehicleCategoryForPayment"
 Private Const spDelete As String = "deleteVehicleCategoryForPayment"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
 'register properties
 Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleCategoryForPayment), New PropertyInfo(Of Integer)("Id"))
 Private Shared CodeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleCategoryForPayment), New PropertyInfo(Of String)("Code"))
 Private Shared NameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleCategoryForPayment), New PropertyInfo(Of String)("Name"))
 Private Shared ZelenMapProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleCategoryForPayment), New PropertyInfo(Of Integer)("ZelenMap"))
 Private Shared BelMapProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleCategoryForPayment), New PropertyInfo(Of Integer)("BelMap"))
 Private _lastChanged(7) As Byte

 <System.ComponentModel.DataObjectField(True, True)> _
 Public ReadOnly Property Id() As Integer
  Get
   Return GetProperty(Of Integer)(IdProperty)
  End Get
 End Property
 Public Property Code() As String
  Get
   Return GetProperty(Of String)(CodeProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(CodeProperty, value)
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
 Public Property ZelenMap() As Integer
  Get
   Return GetProperty(Of Integer)(ZelenMapProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(ZelenMapProperty, value)
  End Set
 End Property
 Public Property BelMap() As Integer
  Get
   Return GetProperty(Of Integer)(BelMapProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(BelMapProperty, value)
  End Set
 End Property

 Public Overrides Function ToString() As String
  Return Id.ToString
 End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

 Protected Overrides Sub AddAuthorizationRules()
  Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Code") Then
   AuthorizationRules.AllowWrite("Code", roleName)
  Else
   AuthorizationRules.DenyWrite("Code", roleName)
  End If
  'AuthorizationRules.AllowWrite("Code")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Name") Then
   AuthorizationRules.AllowWrite("Name", roleName)
  Else
   AuthorizationRules.DenyWrite("Name", roleName)
  End If
  'AuthorizationRules.AllowWrite("Name")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
   AuthorizationRules.AllowWrite("Active", roleName)
  Else
   AuthorizationRules.DenyWrite("Active", roleName)
  End If
  'AuthorizationRules.AllowWrite("Active")
 End Sub



 Public Shared Function CanGetObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleCategoryForPayments")
 End Function

 Public Shared Function CanAddObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleCategoryForPayments")
 End Function

 Public Shared Function CanEditObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleCategoryForPayments")
 End Function

 Public Shared Function CanDeleteObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleCategoryForPayments")
 End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
 Protected Overrides Sub AddBusinessRules()
  ' CodeProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CodeProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CodeProperty, 3))
  ' NameProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, NameProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NameProperty, 50))
 End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

 Private Sub New()
  ' require use of factory method 
 End Sub

 Public Shared Function NewVehicleCategoryForPayment() As VehicleCategoryForPayment
  If Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a VehicleCategoryForPayment")
  End If
  Return DataPortal.Create(Of VehicleCategoryForPayment)()
 End Function

 Public Shared Function GetVehicleCategoryForPayment(ByVal id As Integer) As VehicleCategoryForPayment
  If Not CanGetObject() Then
   Throw New System.Security.SecurityException("User not authorized to view a VehicleCategoryForPayment")
  End If
  Return DataPortal.Fetch(New SingleCriteria(Of VehicleCategoryForPayment, Integer)(id))
 End Function

 Public Shared Sub DeleteVehicleCategoryForPayment(ByVal id As Integer)
  If Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a VehicleCategoryForPayment")
  End If
  DataPortal.Delete(New SingleCriteria(Of VehicleCategoryForPayment, Integer)(id))
 End Sub

 Public Overrides Function Save() As VehicleCategoryForPayment
  If IsDeleted AndAlso Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a VehicleCategoryForPayment")
  ElseIf IsNew AndAlso Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a VehicleCategoryForPayment")
  ElseIf Not CanEditObject() Then
   Throw New System.Security.SecurityException("User not authorized to update a VehicleCategoryForPayment")
  End If
  Return MyBase.Save()
 End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

 Friend Shared Function NewVehicleCategoryForPaymentChild() As VehicleCategoryForPayment
  Return DataPortal.CreateChild(Of VehicleCategoryForPayment)()
 End Function

 Friend Shared Function GetVehicleCategoryForPayment(ByVal dr As SafeDataReader) As VehicleCategoryForPayment
  Return DataPortal.FetchChild(Of VehicleCategoryForPayment)(dr)
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

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleCategoryForPayment, Integer))
  Database.LogInfo("VehicleCategoryForPayment.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = spGetByID
     cm.Parameters.AddWithValue("@Id", criteria.Value)
     Using dr As New SafeDataReader(cm.ExecuteReader)
      dr.Read()

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(CodeProperty, dr.GetString("Code"))
      LoadProperty(Of String)(NameProperty, dr.GetString("Name"))
      LoadProperty(Of Integer)(ZelenMapProperty, dr.GetInt32("ZelenMap"))
      LoadProperty(Of Integer)(BelMapProperty, dr.GetInt32("BelMap"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
     End Using
    End Using
   End Using

  Catch ex As Exception
   Database.LogException("VehicleCategoryForPayment.DataPortal_Fetch", ex)
   Throw New DbCslaException("VehicleCategoryForPayment.DataPortal_Fetch", ex)
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

      .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
      .Parameters.AddWithValue("@Name", ReadProperty(Of String)(NameProperty))
      .Parameters.AddWithValue("@ZelenMap", ReadProperty(Of Integer)(ZelenMapProperty))
      .Parameters.AddWithValue("@BelMap", ReadProperty(Of Integer)(BelMapProperty))

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
   Database.LogException("VehicleCategoryForPayment.DataPortal_Insert", ex)
   Throw New DbCslaException("VehicleCategoryForPayment.DataPortal_Insert", ex)
  Finally
   Database.LogInfo("VehicleCategoryForPayment.DataPortal_Insert", GetHashCode())
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
      .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
      .Parameters.AddWithValue("@Name", ReadProperty(Of String)(NameProperty))
      .Parameters.AddWithValue("@ZelenMap", ReadProperty(Of Integer)(ZelenMapProperty))
      .Parameters.AddWithValue("@BelMap", ReadProperty(Of Integer)(BelMapProperty))
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
  DataPortal_Delete(New SingleCriteria(Of VehicleCategoryForPayment, Integer)(Id))
 End Sub

 Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleCategoryForPayment, Integer))
  Database.LogInfo("VehicleCategoryForPayment.DataPortal_Delete", GetHashCode())
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
   Database.LogException("VehicleCategoryForPayment.DataPortal_Delete", ex)
   Throw New DbCslaException("VehicleCategoryForPayment.DataPortal_Delete", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

 Private Sub Child_Fetch(ByVal dr As SafeDataReader)
  Database.LogInfo("VehicleCategoryForPayment.Child_Fetch", GetHashCode())
  Try
   LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
   LoadProperty(Of String)(CodeProperty, dr.GetString("Code"))
   LoadProperty(Of String)(NameProperty, dr.GetString("Name"))
   LoadProperty(Of Integer)(ZelenMapProperty, dr.GetInt32("ZelenMap"))
   LoadProperty(Of Integer)(BelMapProperty, dr.GetInt32("BelMap"))
   dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
  Catch ex As Exception
   Database.LogException("VehicleCategoryForPayment.Child_Fetch", ex)
   Throw New DbCslaException("VehicleCategoryForPayment.Child_Fetch", ex)
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
      .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
      .Parameters.AddWithValue("@Name", ReadProperty(Of String)(NameProperty))
      .Parameters.AddWithValue("@ZelenMap", ReadProperty(Of Integer)(ZelenMapProperty))
      .Parameters.AddWithValue("@BelMap", ReadProperty(Of Integer)(BelMapProperty))

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
   Database.LogException("VehicleCategoryForPayment.Child_Insert", ex)
   Throw New DbCslaException("VehicleCategoryForPayment.Child_Insert", ex)
  Finally
   Database.LogInfo("VehicleCategoryForPayment.Child_Insert", GetHashCode)
  End Try

 End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

 Private Sub Child_Update()
  Database.LogInfo("VehicleCategoryForPayment.Child_Update", GetHashCode)
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spUpdate

      .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
      .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
      .Parameters.AddWithValue("@Name", ReadProperty(Of String)(NameProperty))
      .Parameters.AddWithValue("@ZelenMap", ReadProperty(Of Integer)(ZelenMapProperty))
      .Parameters.AddWithValue("@BelMap", ReadProperty(Of Integer)(BelMapProperty))
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
   Database.LogException("VehicleCategoryForPayment.Child_Update", ex)
   If Not ex.Message.EndsWith("drug korisnik") Then
    Throw New DbCslaException("VehicleCategoryForPayment.Child_Update", ex)
   End If
  End Try
 End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

 Private Sub Child_DeleteSelf()

  Database.LogInfo("VehicleCategoryForPayment.Child_DeleteSelf", GetHashCode)
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
   Database.LogException("VehicleCategoryForPayment.Child_Fetch", ex)
   Throw New DbCslaException("VehicleCategoryForPayment.Child_Fetch", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
