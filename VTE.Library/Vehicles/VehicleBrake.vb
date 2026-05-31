
<Serializable()> _
Public Class VehicleBrake
  Inherits Csla.BusinessBase(Of VehicleBrake)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleBrakeByID"
  Private Const spGetAll As String = "GetVehicleBrakes"
  Private Const spUpdate As String = "updateVehicleBrake"
  Private Const spAdd As String = "addVehicleBrake"
  Private Const spDelete As String = "deleteVehicleBrake"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleBrake), New PropertyInfo(Of Integer)("Id"))
  Private Shared BreakesCodeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleBrake), New PropertyInfo(Of Integer)("BreakesCode"))
  Private Shared BreakesDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleBrake), New PropertyInfo(Of String)("BreakesDescription"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property BreakesCode() As Integer
    Get
      Return GetProperty(Of Integer)(BreakesCodeProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(BreakesCodeProperty, value)
    End Set
  End Property
  Public Property BreakesDescription() As String
    Get
      Return GetProperty(Of String)(BreakesDescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(BreakesDescriptionProperty, value)
    End Set
  End Property

  Public ReadOnly Property Breakes() As String
    Get
      Return BreakesCode & "-" & BreakesDescription
    End Get
    
  End Property


  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("BreakesCode") Then
      AuthorizationRules.AllowWrite("BreakesCode", roleName)
    Else
      AuthorizationRules.DenyWrite("BreakesCode", roleName)
    End If
    'AuthorizationRules.AllowWrite("BreakesCode")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("BreakesDescription") Then
      AuthorizationRules.AllowWrite("BreakesDescription", roleName)
    Else
      AuthorizationRules.DenyWrite("BreakesDescription", roleName)
    End If
    'AuthorizationRules.AllowWrite("BreakesDescription")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleBrake")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleBrake")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleBrake")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleBrake")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' BreakesDescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, BreakesDescriptionProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BreakesDescriptionProperty, 150))
    '(ValidationRules.AddRule(Of VehicleBrake)(AddressOf NoDuplicates, "BreakesCode"))
  End Sub
  Private Shared Function NoDuplicates(Of T As VehicleBrake)(ByVal target As T, _
  ByVal e As Csla.Validation.RuleArgs) As Boolean

    Dim parent As VehicleBrakes = CType(target.Parent, VehicleBrakes)
    If parent IsNot Nothing Then
      For Each item As VehicleBrake In parent
        If item.BreakesCode = target.BreakesCode AndAlso Not ReferenceEquals(item, target) Then
          e.Description = "Шифрата мора да биде единствена"
          Return False
        End If
      Next
    End If
    Return True

  End Function
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewVehicleBrake() As VehicleBrake
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleBrake")
    End If
    Return DataPortal.Create(Of VehicleBrake)()
  End Function

  Public Shared Function GetVehicleBrake(ByVal id As Integer) As VehicleBrake
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a VehicleBrake")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of VehicleBrake, Integer)(Id))
  End Function

  Public Shared Sub DeleteVehicleBrake(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleBrake")
    End If
    DataPortal.Delete(New SingleCriteria(Of VehicleBrake, Integer)(Id))
  End Sub

  Public Overrides Function Save() As VehicleBrake
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleBrake")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleBrake")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a VehicleBrake")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleBrakeChild() As VehicleBrake
    Return DataPortal.CreateChild(Of VehicleBrake)()
  End Function

  Friend Shared Function GetVehicleBrake(ByVal dr As SafeDataReader) As VehicleBrake
    Return DataPortal.FetchChild(Of VehicleBrake)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleBrake, Integer))
    Database.LogInfo("VehicleBrake.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of Integer)(BreakesCodeProperty, dr.GetInt32("BreakesCode"))
            LoadProperty(Of String)(BreakesDescriptionProperty, dr.GetString("BreakesDescription"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleBrake.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleBrake.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@BreakesCode", ReadProperty(Of Integer)(BreakesCodeProperty))
            .Parameters.AddWithValue("@BreakesDescription", ReadProperty(Of String)(BreakesDescriptionProperty))

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
      Database.LogException("VehicleBrake.DataPortal_Insert", ex)
      Throw New DbCslaException("VehicleBrake.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("VehicleBrake.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@BreakesCode", ReadProperty(Of Integer)(BreakesCodeProperty))
            .Parameters.AddWithValue("@BreakesDescription", ReadProperty(Of String)(BreakesDescriptionProperty))
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
    DataPortal_Delete(New SingleCriteria(Of VehicleBrake, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleBrake, Integer))
    Database.LogInfo("VehicleBrake.DataPortal_Delete", GetHashCode())
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
      Database.LogException("VehicleBrake.DataPortal_Delete", ex)
      Throw New DbCslaException("VehicleBrake.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleBrake.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(BreakesCodeProperty, dr.GetInt32("BreakesCode"))
      LoadProperty(Of String)(BreakesDescriptionProperty, dr.GetString("BreakesDescription"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("VehicleBrake.Child_Fetch", ex)
      Throw New DbCslaException("VehicleBrake.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@BreakesCode", ReadProperty(Of Integer)(BreakesCodeProperty))
            .Parameters.AddWithValue("@BreakesDescription", ReadProperty(Of String)(BreakesDescriptionProperty))

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
      Database.LogException("VehicleBrake.Child_Insert", ex)
      Throw New DbCslaException("VehicleBrake.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleBrake.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("VehicleBrake.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@BreakesCode", ReadProperty(Of Integer)(BreakesCodeProperty))
            .Parameters.AddWithValue("@BreakesDescription", ReadProperty(Of String)(BreakesDescriptionProperty))
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
      Database.LogException("VehicleBrake.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleBrake.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleBrake.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleBrake.Child_Fetch", ex)
      Throw New DbCslaException("VehicleBrake.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
