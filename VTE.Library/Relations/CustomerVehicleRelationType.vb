
<Serializable()> _
Public Class CustomerVehicleRelationType
  Inherits Csla.BusinessBase(Of CustomerVehicleRelationType)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCustomerVehiclesRelationTypeByID"
  Private Const spGetAll As String = "GetCustomerVehiclesRelationTypes"
  Private Const spUpdate As String = "updateCustomerVehiclesRelationType"
  Private Const spAdd As String = "addCustomerVehiclesRelationType"
  Private Const spDelete As String = "deleteCustomerVehiclesRelationType"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(CustomerVehicleRelationType), New PropertyInfo(Of Integer)("Id"))
  Private Shared RelationTypeNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomerVehicleRelationType), New PropertyInfo(Of String)("RelationTypeName"))
  Private Shared IsCustomerOnlyProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(CustomerVehicleRelationType), New PropertyInfo(Of Boolean)("IsCustomerOnly"))
  Private Shared IsOwnerProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(CustomerVehicleRelationType), New PropertyInfo(Of Boolean)("IsOwner"))
  Private Shared IsAuthorizedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(CustomerVehicleRelationType), New PropertyInfo(Of Boolean)("IsAuthorized"))
  Private Shared RelationDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomerVehicleRelationType), New PropertyInfo(Of String)("RelationDescription"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property RelationTypeName() As String
    Get
      Return GetProperty(Of String)(RelationTypeNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(RelationTypeNameProperty, value)
    End Set
  End Property
  Public Property IsCustomerOnly() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsCustomerOnlyProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsCustomerOnlyProperty, value)
    End Set
  End Property
  Public Property IsOwner() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsOwnerProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsOwnerProperty, value)
    End Set
  End Property
  Public Property IsAuthorized() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsAuthorizedProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsAuthorizedProperty, value)
    End Set
  End Property
  Public Property RelationDescription() As String
    Get
      Return GetProperty(Of String)(RelationDescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(RelationDescriptionProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("RelationTypeName") Then
      AuthorizationRules.AllowWrite("RelationTypeName", roleName)
    Else
      AuthorizationRules.DenyWrite("RelationTypeName", roleName)
    End If
    'AuthorizationRules.AllowWrite("RelationTypeName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsCustomerOnly") Then
      AuthorizationRules.AllowWrite("IsCustomerOnly", roleName)
    Else
      AuthorizationRules.DenyWrite("IsCustomerOnly", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsCustomerOnly")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsOwner") Then
      AuthorizationRules.AllowWrite("IsOwner", roleName)
    Else
      AuthorizationRules.DenyWrite("IsOwner", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsOwner")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsAuthorized") Then
      AuthorizationRules.AllowWrite("IsAuthorized", roleName)
    Else
      AuthorizationRules.DenyWrite("IsAuthorized", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsAuthorized")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("RelationDescription") Then
      AuthorizationRules.AllowWrite("RelationDescription", roleName)
    Else
      AuthorizationRules.DenyWrite("RelationDescription", roleName)
    End If
    'AuthorizationRules.AllowWrite("RelationDescription")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("CustomerVehicleRelationType")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("CustomerVehicleRelationType")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("CustomerVehicleRelationType")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("CustomerVehicleRelationType")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' RelationTypeNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, RelationTypeNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(RelationTypeNameProperty, 50))
    ' RelationDescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, RelationDescriptionProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(RelationDescriptionProperty, 250))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewCustomerVehicleRelationType() As CustomerVehicleRelationType
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a CustomerVehicleRelationType")
    End If
    Return DataPortal.Create(Of CustomerVehicleRelationType)()
  End Function

  Public Shared Function GetCustomerVehicleRelationType(ByVal id As Integer) As CustomerVehicleRelationType
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a CustomerVehicleRelationType")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of CustomerVehicleRelationType, Integer)(Id))
  End Function
 
  Public Shared Sub DeleteCustomerVehicleRelationType(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a CustomerVehicleRelationType")
    End If
    DataPortal.Delete(New SingleCriteria(Of CustomerVehicleRelationType, Integer)(Id))
  End Sub

  Public Overrides Function Save() As CustomerVehicleRelationType
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a CustomerVehicleRelationType")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a CustomerVehicleRelationType")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a CustomerVehicleRelationType")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewCustomerVehicleRelationTypeChild() As CustomerVehicleRelationType
    Return DataPortal.CreateChild(Of CustomerVehicleRelationType)()
  End Function

  Friend Shared Function GetCustomerVehicleRelationType(ByVal dr As SafeDataReader) As CustomerVehicleRelationType
    Return DataPortal.FetchChild(Of CustomerVehicleRelationType)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of CustomerVehicleRelationType, Integer))
    Database.LogInfo("CustomerVehicleRelationType.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(RelationTypeNameProperty, dr.GetString("RelationTypeName"))
            LoadProperty(Of Boolean)(IsCustomerOnlyProperty, dr.GetBoolean("IsCustomerOnly"))
            LoadProperty(Of Boolean)(IsOwnerProperty, dr.GetBoolean("IsOwner"))
            LoadProperty(Of Boolean)(IsAuthorizedProperty, dr.GetBoolean("IsAuthorized"))
            LoadProperty(Of String)(RelationDescriptionProperty, dr.GetString("RelationDescription"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("CustomerVehicleRelationType.DataPortal_Fetch", ex)
      Throw New DbCslaException("CustomerVehicleRelationType.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@RelationTypeName", ReadProperty(Of String)(RelationTypeNameProperty))
            .Parameters.AddWithValue("@IsCustomerOnly", ReadProperty(Of Boolean)(IsCustomerOnlyProperty))
            .Parameters.AddWithValue("@IsOwner", ReadProperty(Of Boolean)(IsOwnerProperty))
            .Parameters.AddWithValue("@IsAuthorized", ReadProperty(Of Boolean)(IsAuthorizedProperty))
            .Parameters.AddWithValue("@RelationDescription", ReadProperty(Of String)(RelationDescriptionProperty))

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
      Database.LogException("CustomerVehicleRelationType.DataPortal_Insert", ex)
      Throw New DbCslaException("CustomerVehicleRelationType.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("CustomerVehicleRelationType.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@RelationTypeName", ReadProperty(Of String)(RelationTypeNameProperty))
            .Parameters.AddWithValue("@IsCustomerOnly", ReadProperty(Of Boolean)(IsCustomerOnlyProperty))
            .Parameters.AddWithValue("@IsOwner", ReadProperty(Of Boolean)(IsOwnerProperty))
            .Parameters.AddWithValue("@IsAuthorized", ReadProperty(Of Boolean)(IsAuthorizedProperty))
            .Parameters.AddWithValue("@RelationDescription", ReadProperty(Of String)(RelationDescriptionProperty))
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
    DataPortal_Delete(New SingleCriteria(Of CustomerVehicleRelationType, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of CustomerVehicleRelationType, Integer))
    Database.LogInfo("CustomerVehicleRelationType.DataPortal_Delete", GetHashCode())
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
      Database.LogException("CustomerVehicleRelationType.DataPortal_Delete", ex)
      Throw New DbCslaException("CustomerVehicleRelationType.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("CustomerVehicleRelationType.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(RelationTypeNameProperty, dr.GetString("RelationTypeName"))
      LoadProperty(Of Boolean)(IsCustomerOnlyProperty, dr.GetBoolean("IsCustomerOnly"))
      LoadProperty(Of Boolean)(IsOwnerProperty, dr.GetBoolean("IsOwner"))
      LoadProperty(Of Boolean)(IsAuthorizedProperty, dr.GetBoolean("IsAuthorized"))
      LoadProperty(Of String)(RelationDescriptionProperty, dr.GetString("RelationDescription"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("CustomerVehicleRelationType.Child_Fetch", ex)
      Throw New DbCslaException("CustomerVehicleRelationType.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@RelationTypeName", ReadProperty(Of String)(RelationTypeNameProperty))
            .Parameters.AddWithValue("@IsCustomerOnly", ReadProperty(Of Boolean)(IsCustomerOnlyProperty))
            .Parameters.AddWithValue("@IsOwner", ReadProperty(Of Boolean)(IsOwnerProperty))
            .Parameters.AddWithValue("@IsAuthorized", ReadProperty(Of Boolean)(IsAuthorizedProperty))
            .Parameters.AddWithValue("@RelationDescription", ReadProperty(Of String)(RelationDescriptionProperty))

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
      Database.LogException("CustomerVehicleRelationType.Child_Insert", ex)
      Throw New DbCslaException("CustomerVehicleRelationType.Child_Insert", ex)
    Finally
      Database.LogInfo("CustomerVehicleRelationType.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("CustomerVehicleRelationType.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@RelationTypeName", ReadProperty(Of String)(RelationTypeNameProperty))
            .Parameters.AddWithValue("@IsCustomerOnly", ReadProperty(Of Boolean)(IsCustomerOnlyProperty))
            .Parameters.AddWithValue("@IsOwner", ReadProperty(Of Boolean)(IsOwnerProperty))
            .Parameters.AddWithValue("@IsAuthorized", ReadProperty(Of Boolean)(IsAuthorizedProperty))
            .Parameters.AddWithValue("@RelationDescription", ReadProperty(Of String)(RelationDescriptionProperty))
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
      Database.LogException("CustomerVehicleRelationType.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("CustomerVehicleRelationType.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("CustomerVehicleRelationType.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("CustomerVehicleRelationType.Child_Fetch", ex)
      Throw New DbCslaException("CustomerVehicleRelationType.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access


  Public Function ZemiRelationType(ByVal idCustomer As Integer, ByVal idVehicle As Integer) As Integer
    Database.LogInfo("CustomerVehicleRelationType.DataPortal_Fetch", GetHashCode())
    Try
      If idVehicle = Nothing Then
        idVehicle = 0
      End If
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getCustomerVehiclesRelationTypeByCustomerAndVehicle"
          cm.Parameters.AddWithValue("@idCustomer", idCustomer)
          cm.Parameters.AddWithValue("@idVehicle", idVehicle)
          Return cm.ExecuteScalar()
        End Using
      End Using
    Catch ex As Exception
      Return 0
    End Try
  End Function

    Public Function ZemiRelationId(ByVal idCustomer As Long, ByVal idVehicle As Long) As Long
        Database.LogInfo("CustomerVehicleRelationType.DataPortal_Fetch", GetHashCode())
        Try
            If idVehicle = Nothing Then
                idVehicle = 0
            End If
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "getCustomerVehiclesRelationIdByCustomerAndVehicle"
                    cm.Parameters.AddWithValue("@idCustomer", idCustomer)
                    cm.Parameters.AddWithValue("@idVehicle", idVehicle)
                    Return cm.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return 0
        End Try
    End Function
  Public Function ProveriRelacijaSoVozilo(ByVal idVehicle As Long) As Long
    Database.LogInfo("CustomerVehicleRelationType.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "ProveriRelacijaSoVozilo"
          cm.Parameters.AddWithValue("@idVehicle", idVehicle)
          Dim customerId As Long
          Try
            customerId = cm.ExecuteScalar
          Catch ex As Exception
            customerId = 0
          End Try
          Return customerId
        End Using
      End Using
    Catch ex As Exception
      Return 0
    End Try
  End Function
End Class
