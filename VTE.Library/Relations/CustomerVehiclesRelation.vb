
<Serializable()> _
Public Class CustomerVehiclesRelation
  Inherits Csla.BusinessBase(Of CustomerVehiclesRelation)



#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCustomerVehiclesRelationByID"
  Private Const spGetAll As String = "GetCustomerVehiclesRelations"
  Private Const spUpdate As String = "updateCustomerVehiclesRelation"
  Private Const spAdd As String = "addCustomerVehiclesRelation"
  Private Const spDelete As String = "deleteCustomerVehiclesRelation"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(CustomerVehiclesRelation), New PropertyInfo(Of Long)("Id"))
  Private Shared IdRelationTypeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(CustomerVehiclesRelation), New PropertyInfo(Of Integer)("IdRelationType"))
  Private Shared IdCustomerProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(CustomerVehiclesRelation), New PropertyInfo(Of Long)("IdCustomer", "IdCustomer", 0))
  Private Shared IdVehicleProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(CustomerVehiclesRelation), New PropertyInfo(Of Long)("IdVehicle", "IdVehicle", 0))
  Private Shared StartDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(CustomerVehiclesRelation), New PropertyInfo(Of SmartDate)("StartDate", "StartDate", New SmartDate(DateTime.Today, True)))
  Private Shared EndDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(CustomerVehiclesRelation), New PropertyInfo(Of SmartDate)("EndDate", New SmartDate(True)))
  Private Shared BeginNoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomerVehiclesRelation), New PropertyInfo(Of String)("BeginNote"))
  Private Shared TerminationNoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CustomerVehiclesRelation), New PropertyInfo(Of String)("TerminationNote"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Long
    Get
      Return GetProperty(Of Long)(IdProperty)
    End Get
  End Property
  Public Property IdRelationType() As Integer
    Get
      Return GetProperty(Of Integer)(IdRelationTypeProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdRelationTypeProperty, value)
    End Set
  End Property
  Public Property IdCustomer() As Long
    Get
      Return GetProperty(Of Long)(IdCustomerProperty)
    End Get
    Set(ByVal value As Long)
      SetProperty(Of Long)(IdCustomerProperty, value)
    End Set
  End Property
  Public Property IdVehicle() As Long
    Get
      Return GetProperty(Of Long)(IdVehicleProperty)
    End Get
    Set(ByVal value As Long)
      SetProperty(Of Long)(IdVehicleProperty, value)
    End Set
  End Property
  Public Property StartDate() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(StartDateProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(StartDateProperty, value)
    End Set
  End Property
  Public Property EndDate() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(EndDateProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(EndDateProperty, value)
    End Set
  End Property
  Public Property BeginNote() As String
    Get
      Return GetProperty(Of String)(BeginNoteProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(BeginNoteProperty, value)
    End Set
  End Property
  Public Property TerminationNote() As String
    Get
      Return GetProperty(Of String)(TerminationNoteProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(TerminationNoteProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdRelationType") Then
      AuthorizationRules.AllowWrite("IdRelationType", roleName)
    Else
      AuthorizationRules.DenyWrite("IdRelationType", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdRelationType")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCustomer") Then
      AuthorizationRules.AllowWrite("IdCustomer", roleName)
    Else
      AuthorizationRules.DenyWrite("IdCustomer", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdCustomer")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicle") Then
      AuthorizationRules.AllowWrite("IdVehicle", roleName)
    Else
      AuthorizationRules.DenyWrite("IdVehicle", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdVehicle")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("StartDate") Then
      AuthorizationRules.AllowWrite("StartDate", roleName)
    Else
      AuthorizationRules.DenyWrite("StartDate", roleName)
    End If
    'AuthorizationRules.AllowWrite("StartDate")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("EndDate") Then
      AuthorizationRules.AllowWrite("EndDate", roleName)
    Else
      AuthorizationRules.DenyWrite("EndDate", roleName)
    End If
    'AuthorizationRules.AllowWrite("EndDate")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("BeginNote") Then
      AuthorizationRules.AllowWrite("BeginNote", roleName)
    Else
      AuthorizationRules.DenyWrite("BeginNote", roleName)
    End If
    'AuthorizationRules.AllowWrite("BeginNote")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TerminationNote") Then
      AuthorizationRules.AllowWrite("TerminationNote", roleName)
    Else
      AuthorizationRules.DenyWrite("TerminationNote", roleName)
    End If
    'AuthorizationRules.AllowWrite("TerminationNote")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("CustomerVehiclesRelation")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("CustomerVehiclesRelation")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("CustomerVehiclesRelation")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("CustomerVehiclesRelation")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' StartDateProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, StartDateProperty)
    ' BeginNoteProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BeginNoteProperty, 250))
    ' TerminationNoteProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(TerminationNoteProperty, 250))
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.IntegerMinValue, New Csla.Validation.IntegerMinValueRuleArgs(IdCustomerProperty, 0))
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.IntegerMinValue, New Csla.Validation.IntegerMinValueRuleArgs(IdVehicleProperty, 0))
    '
        ValidationRules.AddRule(Of CustomerVehiclesRelation)(AddressOf RelationUnique(Of CustomerVehiclesRelation), IdCustomerProperty)
        ValidationRules.AddRule(Of CustomerVehiclesRelation)(AddressOf RelationUnique(Of CustomerVehiclesRelation), IdVehicleProperty)
    ValidationRules.AddDependentProperty(IdCustomerProperty, IdVehicleProperty, True)
  End Sub

  Private Shared Function RelationUnique(Of T As CustomerVehiclesRelation)(ByVal target As T, ByVal e As Csla.Validation.RuleArgs) As Boolean
    If (target.IdCustomer = 0) Or (target.IdVehicle = 0) Then Return True
    If CustomerVehiclesRelation.ExistsRelation(target.IdCustomer, target.IdVehicle) Then
      e.Description = "Релацијата помеѓу клиентот и возилото постои"
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

  Public Shared Function NewCustomerVehiclesRelation() As CustomerVehiclesRelation
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a CustomerVehiclesRelation")
    End If
    Return DataPortal.Create(Of CustomerVehiclesRelation)()
  End Function

  Public Shared Function GetCustomerVehiclesRelation(ByVal id As Long) As CustomerVehiclesRelation
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a CustomerVehiclesRelation")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of CustomerVehiclesRelation, Integer)(id))
  End Function

  Public Shared Sub DeleteCustomerVehiclesRelation(ByVal id As Long)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a CustomerVehiclesRelation")
    End If
    DataPortal.Delete(New SingleCriteria(Of CustomerVehiclesRelation, Integer)(id))
  End Sub

  Public Overrides Function Save() As CustomerVehiclesRelation
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a CustomerVehiclesRelation")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a CustomerVehiclesRelation")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a CustomerVehiclesRelation")
    End If

        Dim result As CustomerVehiclesRelation = MyBase.Save()

        'OnCustomerVehiclesRelationSaved(Me, New Csla.Core.SavedEventArgs(result))

        Return result
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewCustomerVehiclesRelationChild() As CustomerVehiclesRelation
    Return DataPortal.CreateChild(Of CustomerVehiclesRelation)()
  End Function

  Friend Shared Function GetCustomerVehiclesRelation(ByVal dr As SafeDataReader) As CustomerVehiclesRelation
    Return DataPortal.FetchChild(Of CustomerVehiclesRelation)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of CustomerVehiclesRelation, Integer))
    Database.LogInfo("CustomerVehiclesRelation.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of Integer)(IdRelationTypeProperty, dr.GetInt32("IdRelationType"))
            LoadProperty(Of Long)(IdCustomerProperty, dr.GetInt64("IdCustomer"))
            LoadProperty(Of Long)(IdVehicleProperty, dr.GetInt64("IdVehicle"))
            LoadProperty(Of SmartDate, Date?)(StartDateProperty, dr.GetSmartDate("StartDate", True))
            LoadProperty(Of SmartDate, Date?)(EndDateProperty, dr.GetSmartDate("EndDate", True))
            LoadProperty(Of String)(BeginNoteProperty, dr.GetString("BeginNote"))
            LoadProperty(Of String)(TerminationNoteProperty, dr.GetString("TerminationNote"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using
            'puka
    Catch ex As Exception
      Database.LogException("CustomerVehiclesRelation.DataPortal_Fetch", ex)
      Throw New DbCslaException("CustomerVehiclesRelation.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@IdRelationType", ReadProperty(Of Integer)(IdRelationTypeProperty))
            .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
            .Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdVehicleProperty))
            .Parameters.AddWithValue("@StartDate", ReadProperty(Of SmartDate)(StartDateProperty).DBValue)
            .Parameters.AddWithValue("@EndDate", ReadProperty(Of SmartDate)(EndDateProperty).DBValue)
            .Parameters.AddWithValue("@BeginNote", ReadProperty(Of String)(BeginNoteProperty))
            .Parameters.AddWithValue("@TerminationNote", ReadProperty(Of String)(TerminationNoteProperty))

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
      Database.LogException("CustomerVehiclesRelation.DataPortal_Insert", ex)
      Throw New DbCslaException("CustomerVehiclesRelation.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("CustomerVehiclesRelation.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@IdRelationType", ReadProperty(Of Integer)(IdRelationTypeProperty))
            .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
            .Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdVehicleProperty))
            .Parameters.AddWithValue("@StartDate", ReadProperty(Of SmartDate)(StartDateProperty).DBValue)
            .Parameters.AddWithValue("@EndDate", ReadProperty(Of SmartDate)(EndDateProperty).DBValue)
            .Parameters.AddWithValue("@BeginNote", ReadProperty(Of String)(BeginNoteProperty))
            .Parameters.AddWithValue("@TerminationNote", ReadProperty(Of String)(TerminationNoteProperty))
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
    DataPortal_Delete(New SingleCriteria(Of CustomerVehiclesRelation, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of CustomerVehiclesRelation, Integer))
    Database.LogInfo("CustomerVehiclesRelation.DataPortal_Delete", GetHashCode())
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
      Database.LogException("CustomerVehiclesRelation.DataPortal_Delete", ex)
      Throw New DbCslaException("CustomerVehiclesRelation.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("CustomerVehiclesRelation.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Integer)(IdRelationTypeProperty, dr.GetInt32("IdRelationType"))
      LoadProperty(Of Long)(IdCustomerProperty, dr.GetInt64("IdCustomer"))
      LoadProperty(Of Long)(IdVehicleProperty, dr.GetInt64("IdVehicle"))
      LoadProperty(Of SmartDate, Date?)(StartDateProperty, dr.GetSmartDate("StartDate", True))
      LoadProperty(Of SmartDate, Date?)(EndDateProperty, dr.GetSmartDate("EndDate", True))
      LoadProperty(Of String)(BeginNoteProperty, dr.GetString("BeginNote"))
      LoadProperty(Of String)(TerminationNoteProperty, dr.GetString("TerminationNote"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("CustomerVehiclesRelation.Child_Fetch", ex)
      Throw New DbCslaException("CustomerVehiclesRelation.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@IdRelationType", ReadProperty(Of Integer)(IdRelationTypeProperty))
            .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
            .Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdVehicleProperty))
            .Parameters.AddWithValue("@StartDate", ReadProperty(Of SmartDate)(StartDateProperty).DBValue)
            .Parameters.AddWithValue("@EndDate", ReadProperty(Of SmartDate)(EndDateProperty).DBValue)
            .Parameters.AddWithValue("@BeginNote", ReadProperty(Of String)(BeginNoteProperty))
            .Parameters.AddWithValue("@TerminationNote", ReadProperty(Of String)(TerminationNoteProperty))

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
      Database.LogException("CustomerVehiclesRelation.Child_Insert", ex)
      Throw New DbCslaException("CustomerVehiclesRelation.Child_Insert", ex)
    Finally
      Database.LogInfo("CustomerVehiclesRelation.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("CustomerVehiclesRelation.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
            .Parameters.AddWithValue("@IdRelationType", ReadProperty(Of Integer)(IdRelationTypeProperty))
            .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
            .Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdVehicleProperty))
            .Parameters.AddWithValue("@StartDate", ReadProperty(Of SmartDate)(StartDateProperty).DBValue)
            .Parameters.AddWithValue("@EndDate", ReadProperty(Of SmartDate)(EndDateProperty).DBValue)
            .Parameters.AddWithValue("@BeginNote", ReadProperty(Of String)(BeginNoteProperty))
            .Parameters.AddWithValue("@TerminationNote", ReadProperty(Of String)(TerminationNoteProperty))
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
      Database.LogException("CustomerVehiclesRelation.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("CustomerVehiclesRelation.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("CustomerVehiclesRelation.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("CustomerVehiclesRelation.Child_Fetch", ex)
      Throw New DbCslaException("CustomerVehiclesRelation.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

#Region " Exists "

  Public Shared Function Exists(ByVal MB As String) As Long

    Dim result As ExistsCommand
    result = DataPortal.Execute(Of ExistsCommand)(New ExistsCommand(MB))
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

    Public Sub New(ByVal MB As String)
      _mb = MB
    End Sub

    Protected Overrides Sub DataPortal_Execute()
      Dim count As Long = 0

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.Text
          cm.CommandText = _
  "Select dbo.CustomerVehiclesRelations.Id FROM dbo.CustomerVehiclesRelations INNER JOIN dbo.Customers ON dbo.Customers.Id = dbo.CustomerVehiclesRelations.IdCustomer where dbo.CustomerVehiclesRelations.active=1 and dbo.Customers.MB=@mb and (dbo.CustomerVehiclesRelations.IdVehicle = 0 or isnull(dbo.CustomerVehiclesRelations.IdVehicle,-5)=-5) and dbo.Customers.IsCompany=0"
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

#Region " Exists Relation without vehicle"

    Public Shared Function ExistsVehicleZero(ByVal idCustomer As Long) As Long

        Dim result As ExistsZeroCommand
        result = DataPortal.Execute(Of ExistsZeroCommand)(New ExistsZeroCommand(idCustomer))
        Return result.ExistsZero

    End Function

    <Serializable()> _
    Private Class ExistsZeroCommand
        Inherits CommandBase

        Private _idCustomer As Long
        Private _Exists As Long

        Public ReadOnly Property ExistsZero() As Long
            Get
                Return _Exists
            End Get
        End Property

        Public Sub New(ByVal idCustomer As Long)
            _idCustomer = idCustomer
        End Sub

        Protected Overrides Sub DataPortal_Execute()
            Dim count As Long = 0

            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.Text
                    cm.CommandText = _
            "Select dbo.CustomerVehiclesRelations.Id FROM dbo.CustomerVehiclesRelations Where(dbo.CustomerVehiclesRelations.active = 1 and dbo.CustomerVehiclesRelations.IdCustomer= @idCustomer and dbo.CustomerVehiclesRelations.IdVehicle = 0 and (dbo.CustomerVehiclesRelations.EndDate>getdate() or isnull(dbo.CustomerVehiclesRelations.EndDate,-2)=-2)"
                    cm.Parameters.AddWithValue("@idCustomer", _idCustomer)
                    Try
                        Using dr As New SafeDataReader(cm.ExecuteReader)
                            If dr.Read() Then
                                count = dr.GetInt64("Id")
                            End If
                        End Using
                        _Exists = count
                    Catch ex As Exception
                        _Exists = 0
                    End Try

                End Using
            End Using

        End Sub

    End Class

#End Region
    

#Region " Validation commands "


  Public Shared Function ExistsRelation(ByVal idCustomer As Long, ByVal idVehicle As Long) As Boolean

    Return ExistsRelationCommand.Exists(idCustomer, idVehicle)

    End Function
    Public Shared Function ExistsRelationId(ByVal idCustomer As Long, ByVal idVehicle As Long) As Long

        Return ExistsRelationCommand.ExistsId(idCustomer, idVehicle)

    End Function

  <Serializable()> _
  Private Class ExistsRelationCommand
    Inherits CommandBase

    Private _idCustomer As Long
    Private _idVehicle As Long

        Private _exists As Boolean
        Private _existId As Long
    Public ReadOnly Property RelationExists() As Boolean
      Get
        Return _exists
      End Get
        End Property
        Public ReadOnly Property RelationExistsId() As Long
            Get
                Return _existId
            End Get
        End Property

    Public Sub New(ByVal idCustomer As Long, ByVal idVehicle As Long)
      _idCustomer = idCustomer
      _idVehicle = idVehicle
    End Sub

    Public Shared Function Exists(ByVal idCustomer As Long, ByVal idVehicle As Long) As Boolean

      Dim result As ExistsRelationCommand
      result = DataPortal.Execute(Of ExistsRelationCommand)(New ExistsRelationCommand(idCustomer, idVehicle))
      Return result.RelationExists

        End Function
        Public Shared Function ExistsId(ByVal idCustomer As Long, ByVal idVehicle As Long) As Long

            Dim result As ExistsRelationCommand
            result = DataPortal.Execute(Of ExistsRelationCommand)(New ExistsRelationCommand(idCustomer, idVehicle))
            Return result.RelationExistsId

        End Function

    Protected Overrides Sub DataPortal_Execute()
      Dim count As Long = 0

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "existsCustomerVehicleRelation"
          cm.Parameters.AddWithValue("@idCustomer", _idCustomer)
          cm.Parameters.AddWithValue("@idVehicle", _idVehicle)
          Dim result As Integer = CInt(cm.ExecuteScalar)
                    _exists = (result > 0)
                    If _exists Then
                        _existId = result
                    Else
                        _existId = 0
                    End If
        End Using
      End Using

    End Sub

  End Class

#End Region

#Region " Exists CustomerOnly "
  Public Shared Function ExistsCustomerOnly(ByVal idCustomer As Long) As Long

    Return ExistsCustomerOnlyCommand.Exists(idCustomer)

  End Function

  <Serializable()> _
  Private Class ExistsCustomerOnlyCommand
    Inherits CommandBase

    Private _idCustomer As Long

    Private _exists As Long
    Public ReadOnly Property RelationExists() As Long
      Get
        Return _exists
      End Get
    End Property

    Public Sub New(ByVal idCustomer As Long)
      _idCustomer = idCustomer
    End Sub

    Public Shared Function Exists(ByVal idCustomer As Long) As Long

      Dim result As ExistsCustomerOnlyCommand
      result = DataPortal.Execute(Of ExistsCustomerOnlyCommand)(New ExistsCustomerOnlyCommand(idCustomer))
      Return result.RelationExists

    End Function

    Protected Overrides Sub DataPortal_Execute()
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "ExistsRelationCustomerOnly"
          cm.Parameters.AddWithValue("@idCustomer", _idCustomer)
          Dim result As Long = 0
          result = CLng(cm.ExecuteScalar)
          _exists = result
        End Using
      End Using

    End Sub

  End Class
#End Region

    '#Region " Readonlylist refresh "
    '    Public Shared Event CustomerVehiclesRelationSaved As EventHandler(Of Csla.Core.SavedEventArgs)
    '    Protected Shared Sub OnCustomerVehiclesRelationSaved(ByVal sender As CustomerVehiclesRelation, ByVal e As Csla.Core.SavedEventArgs)
    '        RaiseEvent CustomerVehiclesRelationSaved(sender, e)
    '    End Sub

    '#End Region

End Class
