
<Serializable()> _
Public Class VehicleEngineType
  Inherits Csla.BusinessBase(Of VehicleEngineType)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleEngineTypeByID"
  Private Const spGetAll As String = "GetVehicleEngineTypes"
  Private Const spUpdate As String = "updateVehicleEngineType"
  Private Const spAdd As String = "addVehicleEngineType"
  Private Const spDelete As String = "deleteVehicleEngineType"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleEngineType), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdVehicleMakerProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleEngineType), New PropertyInfo(Of Integer)("IdVehicleMaker"))
  Private Shared EngineTypeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleEngineType), New PropertyInfo(Of String)("EngineType"))
  Private Shared TechincalDescriptionEcoProgramProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleEngineType), New PropertyInfo(Of String)("TechincalDescriptionEcoProgram"))
  Private Shared IdDefaultPowerSourceProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleEngineType), New PropertyInfo(Of Integer)("IdDefaultPowerSource"))
    Private Shared DefaultPowerProperty As PropertyInfo(Of Single) = RegisterProperty(Of Single)(GetType(VehicleEngineType), New PropertyInfo(Of Single)("DefaultPower")) 'e za rabotna zafatnina
    Private Shared DefaultPowerOutPutProperty As PropertyInfo(Of Single) = RegisterProperty(Of Single)(GetType(VehicleEngineType), New PropertyInfo(Of Single)("DefaultPowerOutPut"))
    Private Shared DefaultTorqueProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleEngineType), New PropertyInfo(Of String)("DefaultTorque"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdVehicleMaker() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleMakerProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleMakerProperty, value)
    End Set
  End Property
  Public Property EngineType() As String
    Get
      Return GetProperty(Of String)(EngineTypeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(EngineTypeProperty, value)
    End Set
  End Property
  Public Property TechincalDescriptionEcoProgram() As String
    Get
      Return GetProperty(Of String)(TechincalDescriptionEcoProgramProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(TechincalDescriptionEcoProgramProperty, value)
    End Set
  End Property
  Public Property IdDefaultPowerSource() As Integer
    Get
      Return GetProperty(Of Integer)(IdDefaultPowerSourceProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdDefaultPowerSourceProperty, value)
    End Set
  End Property
  Public Property DefaultPower() As Single
    Get
      Return GetProperty(Of Single)(DefaultPowerProperty)
    End Get
    Set(ByVal value As Single)
      SetProperty(Of Single)(DefaultPowerProperty, value)
    End Set
  End Property
  Public Property DefaultPowerOutPut() As Single
    Get
      Return GetProperty(Of Single)(DefaultPowerOutPutProperty)
    End Get
    Set(ByVal value As Single)
      SetProperty(Of Single)(DefaultPowerOutPutProperty, value)
    End Set
  End Property
    Public Property DefaultTorque() As String
        Get
            Return GetProperty(Of String)(DefaultTorqueProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(DefaultTorqueProperty, value)
        End Set
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
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("EngineType") Then
      AuthorizationRules.AllowWrite("EngineType", roleName)
    Else
      AuthorizationRules.DenyWrite("EngineType", roleName)
    End If
    'AuthorizationRules.AllowWrite("EngineType")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TechincalDescriptionEcoProgram") Then
      AuthorizationRules.AllowWrite("TechincalDescriptionEcoProgram", roleName)
    Else
      AuthorizationRules.DenyWrite("TechincalDescriptionEcoProgram", roleName)
    End If
    'AuthorizationRules.AllowWrite("TechincalDescriptionEcoProgram")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdDefaultPowerSource") Then
      AuthorizationRules.AllowWrite("IdDefaultPowerSource", roleName)
    Else
      AuthorizationRules.DenyWrite("IdDefaultPowerSource", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdDefaultPowerSource")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DefaultPower") Then
      AuthorizationRules.AllowWrite("DefaultPower", roleName)
    Else
      AuthorizationRules.DenyWrite("DefaultPower", roleName)
    End If
    'AuthorizationRules.AllowWrite("DefaultPower")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DefaultPowerOutOut") Then
      AuthorizationRules.AllowWrite("DefaultPowerOutOut", roleName)
    Else
      AuthorizationRules.DenyWrite("DefaultPowerOutOut", roleName)
    End If
    'AuthorizationRules.AllowWrite("DefaultPowerOutOut")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DefaultTorque") Then
      AuthorizationRules.AllowWrite("DefaultTorque", roleName)
    Else
      AuthorizationRules.DenyWrite("DefaultTorque", roleName)
    End If
    'AuthorizationRules.AllowWrite("DefaultTorque")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleEngineType")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleEngineType")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleEngineType")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleEngineType")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' EngineTypeProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, EngineTypeProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(EngineTypeProperty, 250))
    ' TechincalDescriptionEcoProgramProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, TechincalDescriptionEcoProgramProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(TechincalDescriptionEcoProgramProperty, 250))
        ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                               New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdVehicleMakerProperty, 1))
        ValidationRules.AddRule(Of VehicleEngineType)(AddressOf NoDuplicates, EngineTypeProperty)
    End Sub

    Private Shared Function NoDuplicates(Of T As VehicleEngineType)(ByVal target As T, _
     ByVal e As Csla.Validation.RuleArgs) As Boolean
        If VehicleEngineType.Exists(target.EngineType, target.IdVehicleMaker) > 0 AndAlso VehicleEngineType.Exists(target.EngineType, target.IdVehicleMaker) <> target.Id Then
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

  Public Shared Function NewVehicleEngineType() As VehicleEngineType
        'If Not CanAddObject() Then
        '  Throw New System.Security.SecurityException("User not authorized to add a VehicleEngineType")
        'End If
    Return DataPortal.Create(Of VehicleEngineType)()
  End Function

  Public Shared Function GetVehicleEngineType(ByVal id As Integer) As VehicleEngineType
        'If Not CanGetObject() Then
        '  Throw New System.Security.SecurityException("User not authorized to view a VehicleEngineType")
        'End If
    Return DataPortal.Fetch(New SingleCriteria(Of VehicleEngineType, Integer)(Id))
  End Function

  Public Shared Sub DeleteVehicleEngineType(ByVal id As Integer)
        'If Not CanDeleteObject() Then
        '  Throw New System.Security.SecurityException("User not authorized to remove a VehicleEngineType")
        'End If
    DataPortal.Delete(New SingleCriteria(Of VehicleEngineType, Integer)(Id))
  End Sub

  Public Overrides Function Save() As VehicleEngineType
        'If IsDeleted AndAlso Not CanDeleteObject() Then
        '  Throw New System.Security.SecurityException("User not authorized to remove a VehicleEngineType")
        'ElseIf IsNew AndAlso Not CanAddObject() Then
        '  Throw New System.Security.SecurityException("User not authorized to add a VehicleEngineType")
        'ElseIf Not CanEditObject() Then
        '  Throw New System.Security.SecurityException("User not authorized to update a VehicleEngineType")
        'End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleEngineTypeChild() As VehicleEngineType
    Return DataPortal.CreateChild(Of VehicleEngineType)()
  End Function

  Friend Shared Function GetVehicleEngineType(ByVal dr As SafeDataReader) As VehicleEngineType
        Return DataPortal.FetchChild(Of VehicleEngineType)(dr)

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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleEngineType, Integer))
    Database.LogInfo("VehicleEngineType.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of Integer)(IdVehicleMakerProperty, dr.GetInt32("IdVehicleMaker"))
            LoadProperty(Of String)(EngineTypeProperty, dr.GetString("EngineTypeCode"))
            LoadProperty(Of String)(TechincalDescriptionEcoProgramProperty, dr.GetString("TechincalDescription"))
            LoadProperty(Of Integer)(IdDefaultPowerSourceProperty, dr.GetInt32("IdDefaultPowerSource"))
            LoadProperty(Of Single)(DefaultPowerProperty, dr.GetValue("DefaultPower"))
            LoadProperty(Of Single)(DefaultPowerOutPutProperty, dr.GetValue("DefaultPowerOutPut"))
                        LoadProperty(Of String)(DefaultTorqueProperty, dr.GetString("DefaultTorque"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleEngineType.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleEngineType.DataPortal_Fetch", ex)
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
            .Parameters.AddWithValue("@EngineTypeCode", ReadProperty(Of String)(EngineTypeProperty))
            .Parameters.AddWithValue("@TechincalDescription", ReadProperty(Of String)(TechincalDescriptionEcoProgramProperty))
            .Parameters.AddWithValue("@IdDefaultPowerSource", ReadProperty(Of Integer)(IdDefaultPowerSourceProperty))
            .Parameters.AddWithValue("@DefaultPower", ReadProperty(Of Single)(DefaultPowerProperty))
            .Parameters.AddWithValue("@DefaultPowerOutPut", ReadProperty(Of Single)(DefaultPowerOutPutProperty))
                        .Parameters.AddWithValue("@DefaultTorque", ReadProperty(Of String)(DefaultTorqueProperty))

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
      Database.LogException("VehicleEngineType.DataPortal_Insert", ex)
      Throw New DbCslaException("VehicleEngineType.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("VehicleEngineType.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@EngineTypeCode", ReadProperty(Of String)(EngineTypeProperty))
            .Parameters.AddWithValue("@TechincalDescription", ReadProperty(Of String)(TechincalDescriptionEcoProgramProperty))
            .Parameters.AddWithValue("@IdDefaultPowerSource", ReadProperty(Of Integer)(IdDefaultPowerSourceProperty))
            .Parameters.AddWithValue("@DefaultPower", ReadProperty(Of Single)(DefaultPowerProperty))
            .Parameters.AddWithValue("@DefaultPowerOutPut", ReadProperty(Of Single)(DefaultPowerOutPutProperty))
                        .Parameters.AddWithValue("@DefaultTorque", ReadProperty(Of String)(DefaultTorqueProperty))
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
    DataPortal_Delete(New SingleCriteria(Of VehicleEngineType, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleEngineType, Integer))
    Database.LogInfo("VehicleEngineType.DataPortal_Delete", GetHashCode())
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
      Database.LogException("VehicleEngineType.DataPortal_Delete", ex)
      Throw New DbCslaException("VehicleEngineType.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleEngineType.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdVehicleMakerProperty, dr.GetInt32("IdVehicleMaker"))
      LoadProperty(Of String)(EngineTypeProperty, dr.GetString("EngineTypeCode"))
      LoadProperty(Of String)(TechincalDescriptionEcoProgramProperty, dr.GetString("TechincalDescription"))
      LoadProperty(Of Integer)(IdDefaultPowerSourceProperty, dr.GetInt32("IdDefaultPowerSource"))
      LoadProperty(Of Single)(DefaultPowerProperty, dr.GetValue("DefaultPower"))
      LoadProperty(Of Single)(DefaultPowerOutPutProperty, dr.GetValue("DefaultPowerOutPut"))
            LoadProperty(Of String)(DefaultTorqueProperty, dr.GetString("DefaultTorque"))


            'LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            'LoadProperty(Of Integer)(IdVehicleMakerProperty, dr.GetInt32("IdVehicleMaker"))
            'LoadProperty(Of String)(EngineTypeProperty, dr.GetString("EngineTypeCode"))
            'LoadProperty(Of String)(TechincalDescriptionEcoProgramProperty, dr.GetString("TechincalDescription"))
            'LoadProperty(Of Integer)(IdDefaultPowerSourceProperty, dr.GetInt32("IdDefaultPowerSource"))
            'LoadProperty(Of Single)(DefaultPowerProperty, dr.GetValue("DefaultPower"))
            'LoadProperty(Of Single)(DefaultPowerOutPutProperty, dr.GetValue("DefaultPowerOutPut"))
            'LoadProperty(Of Single)(DefaultTorqueProperty, dr.GetValue("DefaultTorque"))
            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
            'dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("VehicleEngineType.Child_Fetch", ex)
      Throw New DbCslaException("VehicleEngineType.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@EngineTypeCode", ReadProperty(Of String)(EngineTypeProperty))
            .Parameters.AddWithValue("@TechincalDescription", ReadProperty(Of String)(TechincalDescriptionEcoProgramProperty))
            .Parameters.AddWithValue("@IdDefaultPowerSource", ReadProperty(Of Integer)(IdDefaultPowerSourceProperty))
            .Parameters.AddWithValue("@DefaultPower", ReadProperty(Of Single)(DefaultPowerProperty))
            .Parameters.AddWithValue("@DefaultPowerOutPut", ReadProperty(Of Single)(DefaultPowerOutPutProperty))
                        .Parameters.AddWithValue("@DefaultTorque", ReadProperty(Of String)(DefaultTorqueProperty))

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
      Database.LogException("VehicleEngineType.Child_Insert", ex)
      Throw New DbCslaException("VehicleEngineType.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleEngineType.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("VehicleEngineType.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@IdVehicleMaker", ReadProperty(Of Integer)(IdVehicleMakerProperty))
            .Parameters.AddWithValue("@EngineTypeCode", ReadProperty(Of String)(EngineTypeProperty))
            .Parameters.AddWithValue("@TechincalDescription", ReadProperty(Of String)(TechincalDescriptionEcoProgramProperty))
            .Parameters.AddWithValue("@IdDefaultPowerSource", ReadProperty(Of Integer)(IdDefaultPowerSourceProperty))
            .Parameters.AddWithValue("@DefaultPower", ReadProperty(Of Single)(DefaultPowerProperty))
            .Parameters.AddWithValue("@DefaultPowerOutPut", ReadProperty(Of Single)(DefaultPowerOutPutProperty))
                        .Parameters.AddWithValue("@DefaultTorque", ReadProperty(Of String)(DefaultTorqueProperty))
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
      Database.LogException("VehicleEngineType.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleEngineType.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleEngineType.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleEngineType.Child_Fetch", ex)
      Throw New DbCslaException("VehicleEngineType.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

#Region " EngineType Exists "

    Public Shared Function Exists(ByVal strEngineType As String, ByVal idMaker As Long) As Long

        Return ExistsCommand.EngineTypeExists(strEngineType, idMaker)

    End Function

    <Serializable()> _
    Private Class ExistsCommand
        Inherits CommandBase
        Private _EngineType As String
        Private _idMaker As Long
        Private _existsEngineType As Long
        Public ReadOnly Property Exists() As Long
            Get
                Return _existsEngineType
            End Get
        End Property

        Public Shared Function EngineTypeExists(ByVal strEngineType As String, ByVal idModel As Long) As Long

            Dim result As ExistsCommand
            result = DataPortal.Execute(Of ExistsCommand)(New ExistsCommand(strEngineType, idModel))
            Return result.Exists

        End Function

        Private Sub New(ByVal strEngineType As String, ByVal idMaker As Long)
            _EngineType = strEngineType
            _idMaker = idMaker
            _existsEngineType = 0
        End Sub

        Protected Overrides Sub DataPortal_Execute()
            Dim pom As Integer = 0
            Using cn As SqlConnection = Database.VTE_SqlConnection
                'ApplicationContext.LocalContext("cn") = cn
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.Text
                    cm.CommandText = "SELECT Id FROM [VehicleEngineTypes]  WHERE EngineTypeCode = @EngineType AND IdVehicleMaker = @idMAKER and Active = 1"
                    cm.Parameters.AddWithValue("@EngineType", _EngineType)
                    cm.Parameters.AddWithValue("@idMAKER", _idMaker)
                    pom = cm.ExecuteScalar
                    If pom > 0 Then
                        _existsEngineType = pom
                    Else
                        _existsEngineType = 0
                    End If
                End Using
            End Using
        End Sub

    End Class

#End Region
End Class
