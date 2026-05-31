
<Serializable()> _
Public Class VehicleUse
  Inherits Csla.BusinessBase(Of VehicleUse)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleUsByID"
  Private Const spGetAll As String = "GetVehicleUse"
  Private Const spUpdate As String = "updateVehicleUs"
  Private Const spAdd As String = "addVehicleUs"
  Private Const spDelete As String = "deleteVehicleUs"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleUse), New PropertyInfo(Of Integer)("Id"))
  Private Shared UseDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleUse), New PropertyInfo(Of String)("UseDescription"))
    Private Shared RegistrationMaskProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleUse), New PropertyInfo(Of String)("RegistrationMask")) ', "RegistrationMask", "([A-Z0-9^Ž^Đ^Š^Č][A-Z0-9^Ž^Đ^Š^Č]?[A-Z0-9^Ž^Đ^Š^Č])-([0-9][0-9][0-9])-([A-Z^Ž^Đ^Š^Č][A-Z^Ž^Đ^Š^Č]?[A-Z^Ž^Đ^Š^Č])"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property UseDescription() As String
    Get
      Return GetProperty(Of String)(UseDescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(UseDescriptionProperty, value)
    End Set
  End Property

  Public Property RegistrationMask() As String
    Get
      Return GetProperty(Of String)(RegistrationMaskProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(RegistrationMaskProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("UseDescription") Then
      AuthorizationRules.AllowWrite("UseDescription", roleName)
    Else
      AuthorizationRules.DenyWrite("UseDescription", roleName)
    End If
    'AuthorizationRules.AllowWrite("UseDescription")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("RegistrationMask") Then
      AuthorizationRules.AllowWrite("RegistrationMask", roleName)
    Else
      AuthorizationRules.DenyWrite("RegistrationMask", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleUse")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleUse")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleUse")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleUse")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' UseDescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, UseDescriptionProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(UseDescriptionProperty, 250))
    ValidationRules.AddRule(Of VehicleUse)(AddressOf _
      UseDescriptionUnique(Of VehicleUse), UseDescriptionProperty)

  End Sub

  Private Shared Function UseDescriptionUnique(Of T As VehicleUse) _
(ByVal target As T, ByVal e As Csla.Validation.RuleArgs)

    Dim parent As VehicleUses = CType(target.Parent, VehicleUses)
    If parent IsNot Nothing Then
      For Each child As VehicleUse In parent
        If Not ReferenceEquals(target, child) Then
          If child.GetProperty(Of String)(UseDescriptionProperty) = target.GetProperty(Of String)(UseDescriptionProperty) Then
            e.Description = "Таква намена постои"
            'e.StopProcessing = True
            Return False
          End If
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

  Public Shared Function NewVehicleUse() As VehicleUse
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleUse")
    End If
    Return DataPortal.Create(Of VehicleUse)()
  End Function

  Public Shared Function GetVehicleUse(ByVal id As Integer) As VehicleUse
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a VehicleUse")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of VehicleUse, Integer)(Id))
  End Function

  Public Shared Sub DeleteVehicleUse(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleUse")
    End If
    DataPortal.Delete(New SingleCriteria(Of VehicleUse, Integer)(Id))
  End Sub

  Public Overrides Function Save() As VehicleUse
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleUse")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleUse")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a VehicleUse")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleUseChild() As VehicleUse
    Return DataPortal.CreateChild(Of VehicleUse)()
  End Function

  Friend Shared Function GetVehicleUse(ByVal dr As SafeDataReader) As VehicleUse
    Return DataPortal.FetchChild(Of VehicleUse)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleUse, Integer))
    Database.LogInfo("VehicleUse.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(UseDescriptionProperty, dr.GetString("UseDescription"))
            LoadProperty(Of String)(RegistrationMaskProperty, dr.GetString("RegistrationMask"))
            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleUse.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleUse.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@UseDescription", ReadProperty(Of String)(UseDescriptionProperty))
            .Parameters.AddWithValue("@RegistrationMask", ReadProperty(Of String)(RegistrationMaskProperty))
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
      Database.LogException("VehicleUse.DataPortal_Insert", ex)
      Throw New DbCslaException("VehicleUse.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("VehicleUse.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@UseDescription", ReadProperty(Of String)(UseDescriptionProperty))
            .Parameters.AddWithValue("@RegistrationMask", ReadProperty(Of String)(RegistrationMaskProperty))
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
    DataPortal_Delete(New SingleCriteria(Of VehicleUse, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleUse, Integer))
    Database.LogInfo("VehicleUse.DataPortal_Delete", GetHashCode())
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
      Database.LogException("VehicleUse.DataPortal_Delete", ex)
      Throw New DbCslaException("VehicleUse.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleUse.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(UseDescriptionProperty, dr.GetString("UseDescription"))
      LoadProperty(Of String)(RegistrationMaskProperty, dr.GetString("RegistrationMask"))
      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("VehicleUse.Child_Fetch", ex)
      Throw New DbCslaException("VehicleUse.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@UseDescription", ReadProperty(Of String)(UseDescriptionProperty))
            .Parameters.AddWithValue("@RegistrationMask", ReadProperty(Of String)(RegistrationMaskProperty))
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
      Database.LogException("VehicleUse.Child_Insert", ex)
      Throw New DbCslaException("VehicleUse.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleUse.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("VehicleUse.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext.Remove("cn")
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@UseDescription", ReadProperty(Of String)(UseDescriptionProperty))
            .Parameters.AddWithValue("@RegistrationMask", ReadProperty(Of String)(RegistrationMaskProperty))
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
      Database.LogException("VehicleUse.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleUse.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleUse.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleUse.Child_Fetch", ex)
      Throw New DbCslaException("VehicleUse.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
