
<Serializable()> _
Public Class VehicleTireType
  Inherits Csla.BusinessBase(Of VehicleTireType)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleTireTypeByID"
  Private Const spGetAll As String = "GetVehicleTireTypes"
  Private Const spUpdate As String = "updateVehicleTireType"
  Private Const spAdd As String = "addVehicleTireType"
  Private Const spDelete As String = "deleteVehicleTireType"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleTireType), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdVehicleModelProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleTireType), New PropertyInfo(Of Integer)("IdVehicleModel"))
  Private Shared SeriaProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleTireType), New PropertyInfo(Of String)("Seria"))
  Private Shared TireTypeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleTireType), New PropertyInfo(Of String)("TireType"))
  Private Shared DimenzionsProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(VehicleTireType), New PropertyInfo(Of Decimal)("Dimenzions"))
  Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleTireType), New PropertyInfo(Of String)("Note"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdVehicleModel() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleModelProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleModelProperty, value)
    End Set
  End Property
  Public Property Seria() As String
    Get
      Return GetProperty(Of String)(SeriaProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(SeriaProperty, value)
    End Set
  End Property
  Public Property TireType() As String
    Get
      Return GetProperty(Of String)(TireTypeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(TireTypeProperty, value)
    End Set
  End Property
  Public Property Dimenzions() As Decimal
    Get
      Return GetProperty(Of Decimal)(DimenzionsProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(DimenzionsProperty, value)
    End Set
  End Property
  Public Property Note() As String
    Get
      Return GetProperty(Of String)(NoteProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(NoteProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicleModel") Then
      AuthorizationRules.AllowWrite("IdVehicleModel", roleName)
    Else
      AuthorizationRules.DenyWrite("IdVehicleModel", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdVehicleModel")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Seria") Then
      AuthorizationRules.AllowWrite("Seria", roleName)
    Else
      AuthorizationRules.DenyWrite("Seria", roleName)
    End If
    'AuthorizationRules.AllowWrite("Seria")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TireType") Then
      AuthorizationRules.AllowWrite("TireType", roleName)
    Else
      AuthorizationRules.DenyWrite("TireType", roleName)
    End If
    'AuthorizationRules.AllowWrite("TireType")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Dimenzions") Then
      AuthorizationRules.AllowWrite("Dimenzions", roleName)
    Else
      AuthorizationRules.DenyWrite("Dimenzions", roleName)
    End If
    'AuthorizationRules.AllowWrite("Dimenzions")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Note") Then
      AuthorizationRules.AllowWrite("Note", roleName)
    Else
      AuthorizationRules.DenyWrite("Note", roleName)
    End If
    'AuthorizationRules.AllowWrite("Note")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleTireType")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleTireType")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleTireType")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleTireType")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' SeriaProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, SeriaProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(SeriaProperty, 50))
    ' TireTypeProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, TireTypeProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(TireTypeProperty, 150))
    ' NoteProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NoteProperty, 50))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewVehicleTireType() As VehicleTireType
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleTireType")
    End If
    Return DataPortal.Create(Of VehicleTireType)()
  End Function

  Public Shared Function GetVehicleTireType(ByVal id As Integer) As VehicleTireType
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a VehicleTireType")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of VehicleTireType, Integer)(Id))
  End Function

  Public Shared Sub DeleteVehicleTireType(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleTireType")
    End If
    DataPortal.Delete(New SingleCriteria(Of VehicleTireType, Integer)(Id))
  End Sub

  Public Overrides Function Save() As VehicleTireType
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleTireType")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleTireType")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a VehicleTireType")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleTireTypeChild() As VehicleTireType
    Return DataPortal.CreateChild(Of VehicleTireType)()
  End Function

  Friend Shared Function GetVehicleTireType(ByVal dr As SafeDataReader) As VehicleTireType
    Return DataPortal.FetchChild(Of VehicleTireType)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleTireType, Integer))
    Database.LogInfo("VehicleTireType.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of Integer)(IdVehicleModelProperty, dr.GetInt32("IdVehicleModel"))
            LoadProperty(Of String)(SeriaProperty, dr.GetString("Seria"))
            LoadProperty(Of String)(TireTypeProperty, dr.GetString("TireType"))
            LoadProperty(Of Decimal)(DimenzionsProperty, dr.GetDecimal("Dimenzions"))
            LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleTireType.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleTireType.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))
            .Parameters.AddWithValue("@Seria", ReadProperty(Of String)(SeriaProperty))
            .Parameters.AddWithValue("@TireType", ReadProperty(Of String)(TireTypeProperty))
            .Parameters.AddWithValue("@Dimenzions", ReadProperty(Of Decimal)(DimenzionsProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))

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
      Database.LogException("VehicleTireType.DataPortal_Insert", ex)
      Throw New DbCslaException("VehicleTireType.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("VehicleTireType.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))
            .Parameters.AddWithValue("@Seria", ReadProperty(Of String)(SeriaProperty))
            .Parameters.AddWithValue("@TireType", ReadProperty(Of String)(TireTypeProperty))
            .Parameters.AddWithValue("@Dimenzions", ReadProperty(Of Decimal)(DimenzionsProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
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
    DataPortal_Delete(New SingleCriteria(Of VehicleTireType, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleTireType, Integer))
    Database.LogInfo("VehicleTireType.DataPortal_Delete", GetHashCode())
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
      Database.LogException("VehicleTireType.DataPortal_Delete", ex)
      Throw New DbCslaException("VehicleTireType.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleTireType.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdVehicleModelProperty, dr.GetInt32("IdVehicleModel"))
      LoadProperty(Of String)(SeriaProperty, dr.GetString("Seria"))
      LoadProperty(Of String)(TireTypeProperty, dr.GetString("TireType"))
      LoadProperty(Of Decimal)(DimenzionsProperty, dr.GetDecimal("Dimenzions"))
      LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("VehicleTireType.Child_Fetch", ex)
      Throw New DbCslaException("VehicleTireType.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))
            .Parameters.AddWithValue("@Seria", ReadProperty(Of String)(SeriaProperty))
            .Parameters.AddWithValue("@TireType", ReadProperty(Of String)(TireTypeProperty))
            .Parameters.AddWithValue("@Dimenzions", ReadProperty(Of Decimal)(DimenzionsProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))

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
      Database.LogException("VehicleTireType.Child_Insert", ex)
      Throw New DbCslaException("VehicleTireType.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleTireType.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("VehicleTireType.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))
            .Parameters.AddWithValue("@Seria", ReadProperty(Of String)(SeriaProperty))
            .Parameters.AddWithValue("@TireType", ReadProperty(Of String)(TireTypeProperty))
            .Parameters.AddWithValue("@Dimenzions", ReadProperty(Of Decimal)(DimenzionsProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
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
      Database.LogException("VehicleTireType.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleTireType.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleTireType.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleTireType.Child_Fetch", ex)
      Throw New DbCslaException("VehicleTireType.Child_Fetch", ex)
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
                    cm.CommandText = "SELECT Id FROM [VehicleTireTypes] WHERE TireType=@naziv"
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
