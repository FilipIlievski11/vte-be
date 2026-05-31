
<Serializable()> _
Public Class VehicleEnginePowerSourceType
  Inherits Csla.BusinessBase(Of VehicleEnginePowerSourceType)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleEnginePowerSourceTypeByID"
  Private Const spGetAll As String = "GetVehicleEnginePowerSourceTypes"
  Private Const spUpdate As String = "updateVehicleEnginePowerSourceType"
  Private Const spAdd As String = "addVehicleEnginePowerSourceType"
  Private Const spDelete As String = "deleteVehicleEnginePowerSourceType"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleEnginePowerSourceType), New PropertyInfo(Of Integer)("Id"))
  Private Shared PowerSourceNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleEnginePowerSourceType), New PropertyInfo(Of String)("PowerSourceName"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property PowerSourceName() As String
    Get
      Return GetProperty(Of String)(PowerSourceNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(PowerSourceNameProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PowerSourceName") Then
      AuthorizationRules.AllowWrite("PowerSourceName", roleName)
    Else
      AuthorizationRules.DenyWrite("PowerSourceName", roleName)
    End If
    'AuthorizationRules.AllowWrite("PowerSourceName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleEnginePowerSourceType")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleEnginePowerSourceType")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleEnginePowerSourceType")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleEnginePowerSourceType")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' PowerSourceNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, PowerSourceNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(PowerSourceNameProperty, 50))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewVehicleEnginePowerSourceType() As VehicleEnginePowerSourceType
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleEnginePowerSourceType")
    End If
    Return DataPortal.Create(Of VehicleEnginePowerSourceType)()
  End Function

  Public Shared Function GetVehicleEnginePowerSourceType(ByVal id As Integer) As VehicleEnginePowerSourceType
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a VehicleEnginePowerSourceType")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of VehicleEnginePowerSourceType, Integer)(Id))
  End Function

  Public Shared Sub DeleteVehicleEnginePowerSourceType(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleEnginePowerSourceType")
    End If
    DataPortal.Delete(New SingleCriteria(Of VehicleEnginePowerSourceType, Integer)(Id))
  End Sub

  Public Overrides Function Save() As VehicleEnginePowerSourceType
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleEnginePowerSourceType")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleEnginePowerSourceType")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a VehicleEnginePowerSourceType")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleEnginePowerSourceTypeChild() As VehicleEnginePowerSourceType
    Return DataPortal.CreateChild(Of VehicleEnginePowerSourceType)()
  End Function

  Friend Shared Function GetVehicleEnginePowerSourceType(ByVal dr As SafeDataReader) As VehicleEnginePowerSourceType
    Return DataPortal.FetchChild(Of VehicleEnginePowerSourceType)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleEnginePowerSourceType, Integer))
    Database.LogInfo("VehicleEnginePowerSourceType.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(PowerSourceNameProperty, dr.GetString("PowerSourceName"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleEnginePowerSourceType.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleEnginePowerSourceType.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@PowerSourceName", ReadProperty(Of String)(PowerSourceNameProperty))

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
      Database.LogException("VehicleEnginePowerSourceType.DataPortal_Insert", ex)
      Throw New DbCslaException("VehicleEnginePowerSourceType.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("VehicleEnginePowerSourceType.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@PowerSourceName", ReadProperty(Of String)(PowerSourceNameProperty))
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
    DataPortal_Delete(New SingleCriteria(Of VehicleEnginePowerSourceType, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleEnginePowerSourceType, Integer))
    Database.LogInfo("VehicleEnginePowerSourceType.DataPortal_Delete", GetHashCode())
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
      Database.LogException("VehicleEnginePowerSourceType.DataPortal_Delete", ex)
      Throw New DbCslaException("VehicleEnginePowerSourceType.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleEnginePowerSourceType.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(PowerSourceNameProperty, dr.GetString("PowerSourceName"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("VehicleEnginePowerSourceType.Child_Fetch", ex)
      Throw New DbCslaException("VehicleEnginePowerSourceType.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@PowerSourceName", ReadProperty(Of String)(PowerSourceNameProperty))

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
      Database.LogException("VehicleEnginePowerSourceType.Child_Insert", ex)
      Throw New DbCslaException("VehicleEnginePowerSourceType.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleEnginePowerSourceType.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("VehicleEnginePowerSourceType.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@PowerSourceName", ReadProperty(Of String)(PowerSourceNameProperty))
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
      Database.LogException("VehicleEnginePowerSourceType.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleEnginePowerSourceType.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleEnginePowerSourceType.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleEnginePowerSourceType.Child_Fetch", ex)
      Throw New DbCslaException("VehicleEnginePowerSourceType.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

#Region " Exists "

  Public Shared Function Exists(ByVal Naziv As String) As Integer

    Dim result As ExistsCommand
    result = DataPortal.Execute(Of ExistsCommand)(New ExistsCommand(Naziv))
    Return result.Exists

  End Function

  <Serializable()> _
  Private Class ExistsCommand
    Inherits CommandBase

    Private _naziv As String
    Private _Exists As Integer

    Public ReadOnly Property Exists() As Integer
      Get
        Return _Exists
      End Get
    End Property

    Public Sub New(ByVal Naziv As String)
      _naziv = Naziv
    End Sub

    Protected Overrides Sub DataPortal_Execute()
      Dim count As Integer = 0

            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.Text
                    cm.CommandText = "SELECT Id FROM [VehicleEnginePowerSourceTypes] WHERE PowerSourceName=@naziv"
                    cm.Parameters.AddWithValue("@naziv", _naziv)
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        If dr.Read() Then
                            count = dr.GetInt32("Id")
                        End If
                    End Using
                    _Exists = count
                End Using
            End Using

    End Sub

  End Class

#End Region

End Class

