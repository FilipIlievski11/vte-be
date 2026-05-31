
<Serializable()> _
Public Class VehicleModel
  Inherits Csla.BusinessBase(Of VehicleModel)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleModeByID"
  Private Const spGetAll As String = "GetVehicleModel"
  Private Const spUpdate As String = "updateVehicleMode"
  Private Const spAdd As String = "addVehicleMode"
  Private Const spDelete As String = "deleteVehicleMode"
  Private Const spGetChildrenTires As String = "getVehicleTireTypeByIdVehicleModel"
#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleModel), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdVehicleMakerProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleModel), New PropertyInfo(Of Integer)("IdVehicleMaker"))
  'Private Shared IdBodytypeDefaultProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleModel), New PropertyInfo(Of Integer)("IdBodytypeDefault"))

  Private Shared ModelCodeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleModel), New PropertyInfo(Of String)("ModelCode"))
  Private Shared ModelNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleModel), New PropertyInfo(Of String)("ModelName"))
  Private Shared YearOfBeginingProductionProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(VehicleModel), New PropertyInfo(Of SmartDate)("YearOfBeginingProduction", New SmartDate(DateTime.MinValue, True)))
  Private Shared YearOfEndingProductionProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(VehicleModel), New PropertyInfo(Of SmartDate)("YearOfEndingProduction", New SmartDate(DateTime.MinValue, True)))

  Private _lastChanged(7) As Byte

  '  Private Shared VehicleTireTypesProperty As PropertyInfo(Of VehicleTireTypes) = _
  'RegisterProperty(Of VehicleTireTypes)(GetType(VehicleModel), New PropertyInfo(Of VehicleTireTypes)("VehicleTireTypes"))

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  'Public ReadOnly Property VehicleTireTypes() As VehicleTireTypes
  '  Get
  '    If Not FieldManager.FieldExists(VehicleTireTypesProperty) Then
  '      SetProperty(Of VehicleTireTypes) _
  '      (VehicleTireTypesProperty, VehicleTireTypes.NewVehicleTireTypes)
  '    End If
  '    Return GetProperty(Of VehicleTireTypes)(VehicleTireTypesProperty)
  '  End Get
  'End Property
  Public Property IdVehicleMaker() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleMakerProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleMakerProperty, value)
    End Set
  End Property
  'Public Property IdBodytypeDefault() As Integer
  '  Get
  '    Return GetProperty(Of Integer)(IdBodytypeDefaultProperty)
  '  End Get
  '  Set(ByVal value As Integer)
  '    SetProperty(Of Integer)(IdBodytypeDefaultProperty, value)
  '  End Set
  'End Property

  Public Property ModelCode() As String
    Get
      Return GetProperty(Of String)(ModelCodeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(ModelCodeProperty, value)
    End Set
  End Property
  Public Property ModelName() As String
    Get
      Return GetProperty(Of String)(ModelNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(ModelNameProperty, value)
    End Set
  End Property
  Public Property YearOfBeginingProduction() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(YearOfBeginingProductionProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(YearOfBeginingProductionProperty, value)
    End Set
  End Property
  Public Property YearOfBeginingProductionString() As String
    Get
      If YearOfBeginingProduction = DateTime.MinValue Then
        Return ""
      Else
        Return GetProperty(Of SmartDate, Date)(YearOfBeginingProductionProperty).Year
      End If
    End Get
    Set(ByVal value As String)
      If value = "" Or value = String.Empty Then
        SetProperty(Of SmartDate, Date)(YearOfBeginingProductionProperty, DateTime.MinValue)
      Else
        SetProperty(Of SmartDate, Date)(YearOfBeginingProductionProperty, CType("01.01." & value, DateTime))
      End If

    End Set
  End Property
  Public Property YearOfEndingProductionString() As String
    Get
      If YearOfEndingProduction = DateTime.MinValue Then
        Return ""
      Else
        Return GetProperty(Of SmartDate, Date)(YearOfEndingProductionProperty).Year
      End If
    End Get
    Set(ByVal value As String)
      If value = "" Or value = String.Empty Then
        SetProperty(Of SmartDate, Date)(YearOfEndingProductionProperty, DateTime.MinValue)
      Else
        SetProperty(Of SmartDate, Date)(YearOfEndingProductionProperty, CType("01.01." & value, DateTime))
      End If

    End Set
  End Property
  Public Property YearOfEndingProduction() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(YearOfEndingProductionProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(YearOfEndingProductionProperty, value)
    End Set
  End Property

  Public ReadOnly Property YearOfProduction() As String
    Get
      Return YearOfBeginingProduction.Year & "-" & YearOfEndingProduction.Year
    End Get
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicleMaker") Then
      AuthorizationRules.AllowWrite("IdVehicleMaker", roleName)
    Else
      AuthorizationRules.DenyWrite("IdVehicleMaker", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdVehicleMaker")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdBodytypeDefault") Then
      AuthorizationRules.AllowWrite("IdBodytypeDefault", roleName)
    Else
      AuthorizationRules.DenyWrite("IdBodytypeDefault", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdBodytypeDefault")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ModelName") Then
      AuthorizationRules.AllowWrite("ModelName", roleName)
    Else
      AuthorizationRules.DenyWrite("ModelName", roleName)
    End If
    'AuthorizationRules.AllowWrite("ModelName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("YearOfBeginingProduction") Then
      AuthorizationRules.AllowWrite("YearOfBeginingProduction", roleName)
    Else
      AuthorizationRules.DenyWrite("YearOfBeginingProduction", roleName)
    End If
    'AuthorizationRules.AllowWrite("YearOfBeginingProduction")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("YearOfEndingProduction") Then
      AuthorizationRules.AllowWrite("YearOfEndingProduction", roleName)
    Else
      AuthorizationRules.DenyWrite("YearOfEndingProduction", roleName)
    End If
    'AuthorizationRules.AllowWrite("YearOfEndingProduction")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleModel")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleModel")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleModel")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleModel")
  End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' ModelNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ModelNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(ModelNameProperty, 250))
    ' YearOfBeginingProductionProperty rules
    ' ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, YearOfBeginingProductionProperty)
    ' YearOfEndingProductionProperty rules
    ' ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, YearOfEndingProductionProperty)
        ValidationRules.AddRule(Of VehicleModel)(AddressOf NoDuplicates, ModelNameProperty)
        ValidationRules.AddRule(Of VehicleModel)(AddressOf NoDuplicates, IdVehicleMakerProperty)
    End Sub

    Private Shared Function NoDuplicates(Of T As VehicleModel)(ByVal target As T, _
     ByVal e As Csla.Validation.RuleArgs) As Boolean
        If VehicleModel.ModelNameExists(target.ModelName, target.IdVehicleMaker) > 0 AndAlso _
        VehicleModel.ModelNameExists(target.ModelName, target.IdVehicleMaker) <> target.Id Then
            e.Description = "Типот веќе е внесен"
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

  Public Shared Function NewVehicleModel() As VehicleModel
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleModel")
    End If
    Return DataPortal.Create(Of VehicleModel)()
  End Function

  Public Shared Function GetVehicleModel(ByVal id As Integer) As VehicleModel
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a VehicleModel")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of VehicleModel, Integer)(Id))
  End Function

  Public Shared Sub DeleteVehicleModel(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleModel")
    End If
    DataPortal.Delete(New SingleCriteria(Of VehicleModel, Integer)(Id))
  End Sub

  Public Overrides Function Save() As VehicleModel
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleModel")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleModel")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a VehicleModel")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleModelChild() As VehicleModel
    Return DataPortal.CreateChild(Of VehicleModel)()
  End Function

  Friend Shared Function GetVehicleModel(ByVal dr As SafeDataReader) As VehicleModel
    Return DataPortal.FetchChild(Of VehicleModel)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleModel, Integer))
    Database.LogInfo("VehicleModel.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of Integer)(IdVehicleMakerProperty, dr.GetInt32("IdVehicleMaker"))
            'LoadProperty(Of Integer)(IdBodytypeDefaultProperty, dr.GetInt32("IdBodytypeDefault"))

            LoadProperty(Of String)(ModelCodeProperty, dr.GetString("ModelCode"))
            LoadProperty(Of String)(ModelNameProperty, dr.GetString("ModelName"))
            LoadProperty(Of SmartDate, Date?)(YearOfBeginingProductionProperty, dr.GetSmartDate("YearOfBeginingProduction", True))
            LoadProperty(Of SmartDate, Date?)(YearOfEndingProductionProperty, dr.GetSmartDate("YearOfEndingProduction", True))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
        'Using cm1 As SqlCommand = cn.CreateCommand
        '  cm1.CommandType = CommandType.StoredProcedure
        '  cm1.CommandText = spGetChildrenTires
        '  cm1.Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdProperty))
        '  Using drc As New SafeDataReader(cm1.ExecuteReader)
        '    LoadProperty(Of VehicleTireTypes) _
        '    (VehicleTireTypesProperty, VehicleTireTypes.GetVehicleTireTypes(drc))
        '  End Using
        'End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleModel.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleModel.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@IdVehicleMaker", ReadProperty(Of Integer)(IdVehicleMakerProperty))
            ' .Parameters.AddWithValue("@IdBodytypeDefault", ReadProperty(Of Integer)(IdBodytypeDefaultProperty))

            .Parameters.AddWithValue("@ModelCode", ReadProperty(Of String)(ModelCodeProperty))
            .Parameters.AddWithValue("@ModelName", ReadProperty(Of String)(ModelNameProperty))
            .Parameters.AddWithValue("@YearOfBeginingProduction", ReadProperty(Of SmartDate)(YearOfBeginingProductionProperty).DBValue)
            .Parameters.AddWithValue("@YearOfEndingProduction", ReadProperty(Of SmartDate)(YearOfEndingProductionProperty).DBValue)

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
        'FieldManager.UpdateChildren(Me)

        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("VehicleModel.DataPortal_Insert", ex)
      Throw New DbCslaException("VehicleModel.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("VehicleModel.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@IdVehicleMaker", ReadProperty(Of Integer)(IdVehicleMakerProperty))
            ' .Parameters.AddWithValue("@IdBodytypeDefault", ReadProperty(Of Integer)(IdBodytypeDefaultProperty))
            .Parameters.AddWithValue("@ModelCode", ReadProperty(Of String)(ModelCodeProperty))
            .Parameters.AddWithValue("@ModelName", ReadProperty(Of String)(ModelNameProperty))
            .Parameters.AddWithValue("@YearOfBeginingProduction", ReadProperty(Of SmartDate)(YearOfBeginingProductionProperty).DBValue)
            .Parameters.AddWithValue("@YearOfEndingProduction", ReadProperty(Of SmartDate)(YearOfEndingProductionProperty).DBValue)
            .Parameters.AddWithValue("@lastChanged", _lastChanged)
            Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
        'FieldManager.UpdateChildren(Me)

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
    DataPortal_Delete(New SingleCriteria(Of VehicleModel, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleModel, Integer))
    Database.LogInfo("VehicleModel.DataPortal_Delete", GetHashCode())
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
      Database.LogException("VehicleModel.DataPortal_Delete", ex)
      Throw New DbCslaException("VehicleModel.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleModel.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdVehicleMakerProperty, dr.GetInt32("IdVehicleMaker"))
      'LoadProperty(Of Integer)(IdBodytypeDefaultProperty, dr.GetInt32("IdBodytypeDefault"))

      LoadProperty(Of String)(ModelCodeProperty, dr.GetString("ModelCode"))
      LoadProperty(Of String)(ModelNameProperty, dr.GetString("ModelName"))
      LoadProperty(Of SmartDate, Date?)(YearOfBeginingProductionProperty, dr.GetSmartDate("YearOfBeginingProduction", True))
      LoadProperty(Of SmartDate, Date?)(YearOfEndingProductionProperty, dr.GetSmartDate("YearOfEndingProduction", True))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
      'Using cn As SqlConnection = Database.VTE_SqlConnection
      '  Using cm1 As SqlCommand = cn.CreateCommand
      '    cm1.CommandType = CommandType.StoredProcedure
      '    cm1.CommandText = spGetChildrenTires
      '    cm1.Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdProperty))
      '    Using drc As New SafeDataReader(cm1.ExecuteReader)
      '      LoadProperty(Of VehicleTireTypes) _
      '      (VehicleTireTypesProperty, VehicleTireTypes.GetVehicleTireTypes(drc))
      '    End Using
      '  End Using
      'End Using
    Catch ex As Exception
      Database.LogException("VehicleModel.Child_Fetch", ex)
      Throw New DbCslaException("VehicleModel.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@IdVehicleMaker", ReadProperty(Of Integer)(IdVehicleMakerProperty))
            '.Parameters.AddWithValue("@IdBodytypeDefault", ReadProperty(Of Integer)(IdBodytypeDefaultProperty))
            .Parameters.AddWithValue("@ModelCode", ReadProperty(Of String)(ModelCodeProperty))
            .Parameters.AddWithValue("@ModelName", ReadProperty(Of String)(ModelNameProperty))
            .Parameters.AddWithValue("@YearOfBeginingProduction", ReadProperty(Of SmartDate)(YearOfBeginingProductionProperty).DBValue)
            .Parameters.AddWithValue("@YearOfEndingProduction", ReadProperty(Of SmartDate)(YearOfEndingProductionProperty).DBValue)

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
        'FieldManager.UpdateChildren(Me)

        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("VehicleModel.Child_Insert", ex)
      Throw New DbCslaException("VehicleModel.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleModel.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("VehicleModel.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@IdVehicleMaker", ReadProperty(Of Integer)(IdVehicleMakerProperty))
            '.Parameters.AddWithValue("@IdBodytypeDefault", ReadProperty(Of Integer)(IdBodytypeDefaultProperty))
            .Parameters.AddWithValue("@ModelCode", ReadProperty(Of String)(ModelCodeProperty))
            .Parameters.AddWithValue("@ModelName", ReadProperty(Of String)(ModelNameProperty))
            .Parameters.AddWithValue("@YearOfBeginingProduction", ReadProperty(Of SmartDate)(YearOfBeginingProductionProperty).DBValue)
            .Parameters.AddWithValue("@YearOfEndingProduction", ReadProperty(Of SmartDate)(YearOfEndingProductionProperty).DBValue)
            .Parameters.AddWithValue("@lastChanged", _lastChanged)
            Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
        ' FieldManager.UpdateChildren(Me)

        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("VehicleModel.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleModel.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleModel.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleModel.Child_Fetch", ex)
      Throw New DbCslaException("VehicleModel.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

#Region " ModelName Exists "

    Public Shared Function ModelNameExists(ByVal strModelName As String, ByVal idMaker As Long) As Long

        Return ModelNameExistsCommand.ModelNameExists(strModelName, idMaker)

    End Function

  <Serializable()> _
  Private Class ModelNameExistsCommand
    Inherits CommandBase
    Private _ModelName As String
        Private _idMaker As Long
        Private _existsModelName As Long
        Public ReadOnly Property ExistsModelName() As Long
            Get
                Return _existsModelName
            End Get
        End Property

        Public Shared Function ModelNameExists(ByVal strModelName As String, ByVal idModel As Long) As Long

            Dim result As ModelNameExistsCommand
            result = DataPortal.Execute(Of ModelNameExistsCommand)(New ModelNameExistsCommand(strModelName, idModel))
            Return result.ExistsModelName

        End Function

        Private Sub New(ByVal strModelName As String, ByVal idMaker As Long)
            _ModelName = strModelName
            _idMaker = idMaker
            _existsModelName = 0
        End Sub

    Protected Overrides Sub DataPortal_Execute()
      Dim pom As Integer = 0
      Using cn As SqlConnection = Database.VTE_SqlConnection
        'ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.Text
                    cm.CommandText = "SELECT Id FROM [VehicleModel]  WHERE ModelName = @ModelName AND IdVehicleMaker = @idMAKER and Active = 1"
          cm.Parameters.AddWithValue("@ModelName", _ModelName)
                    cm.Parameters.AddWithValue("@idMAKER", _idMaker)
          pom = cm.ExecuteScalar
                    If pom > 0 Then
                        _existsModelName = pom
                    Else
                        _existsModelName = 0
                    End If
        End Using
      End Using
    End Sub

  End Class

#End Region

End Class
