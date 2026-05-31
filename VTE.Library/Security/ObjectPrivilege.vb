
<Serializable()> _
Public Class ObjectPrivilege
  Inherits Csla.BusinessBase(Of ObjectPrivilege)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetObjectPrivilegeByID"
  Private Const spGetAll As String = "GetObjectPrivileges"
  Private Const spUpdate As String = "updateObjectPrivilege"
  Private Const spAdd As String = "addObjectPrivilege"
  Private Const spDelete As String = "deleteObjectPrivilege"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(ObjectPrivilege), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdRoleProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(ObjectPrivilege), New PropertyInfo(Of Integer)("IdRole"))
  Private Shared IdCSLAObjectProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(ObjectPrivilege), New PropertyInfo(Of Integer)("IdCSLAObject"))
  Private Shared CanAddObjectProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(ObjectPrivilege), New PropertyInfo(Of Boolean)("CanAddObject"))
  Private Shared CanGetObjectProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(ObjectPrivilege), New PropertyInfo(Of Boolean)("CanGetObject"))
  Private Shared CanDeleteObjectProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(ObjectPrivilege), New PropertyInfo(Of Boolean)("CanDeleteObject"))
  Private Shared CanEditObjectProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(ObjectPrivilege), New PropertyInfo(Of Boolean)("CanEditObject"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdRole() As Integer
    Get
      Return GetProperty(Of Integer)(IdRoleProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdRoleProperty, value)
    End Set
  End Property
  Public Property IdCSLAObject() As Integer
    Get
      Return GetProperty(Of Integer)(IdCSLAObjectProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdCSLAObjectProperty, value)
    End Set
  End Property
  Public Property CanAddObject() As Boolean
    Get
      Return GetProperty(Of Boolean)(CanAddObjectProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(CanAddObjectProperty, value)
    End Set
  End Property
  Public Property CanGetObject() As Boolean
    Get
      Return GetProperty(Of Boolean)(CanGetObjectProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(CanGetObjectProperty, value)
    End Set
  End Property
  Public Property CanDeleteObject() As Boolean
    Get
      Return GetProperty(Of Boolean)(CanDeleteObjectProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(CanDeleteObjectProperty, value)
    End Set
  End Property
  Public Property CanEditObject() As Boolean
    Get
      Return GetProperty(Of Boolean)(CanEditObjectProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(CanEditObjectProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdRole") Then
            AuthorizationRules.AllowWrite("IdRole", roleName)
        Else
            AuthorizationRules.DenyWrite("IdRole", roleName)
        End If
        'AuthorizationRules.AllowWrite("IdRole")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCSLAObject") Then
            AuthorizationRules.AllowWrite("IdCSLAObject", roleName)
        Else
            AuthorizationRules.DenyWrite("IdCSLAObject", roleName)
        End If
        'AuthorizationRules.AllowWrite("IdCSLAObject")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CanAddObject") Then
            AuthorizationRules.AllowWrite("CanAddObject", roleName)
        Else
            AuthorizationRules.DenyWrite("CanAddObject", roleName)
        End If
        'AuthorizationRules.AllowWrite("CanAddObject")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CanGetObject") Then
            AuthorizationRules.AllowWrite("CanGetObject", roleName)
        Else
            AuthorizationRules.DenyWrite("CanGetObject", roleName)
        End If
        'AuthorizationRules.AllowWrite("CanGetObject")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CanDeleteObject") Then
            AuthorizationRules.AllowWrite("CanDeleteObject", roleName)
        Else
            AuthorizationRules.DenyWrite("CanDeleteObject", roleName)
        End If
        'AuthorizationRules.AllowWrite("CanDeleteObject")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CanEditObject") Then
            AuthorizationRules.AllowWrite("CanEditObject", roleName)
        Else
            AuthorizationRules.DenyWrite("CanEditObject", roleName)
        End If
        'AuthorizationRules.AllowWrite("CanEditObject")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
            AuthorizationRules.AllowWrite("Active", roleName)
        Else
            AuthorizationRules.DenyWrite("Active", roleName)
        End If
        'AuthorizationRules.AllowWrite("Active")
    End Sub


    Public Shared Function AvtorizacijaCanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("ObjectPrivilege")
    End Function

    Public Shared Function AvtorizacijaCanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("ObjectPrivilege")
    End Function

    Public Shared Function AvtorizacijaCanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("ObjectPrivilege")
    End Function

    Public Shared Function AvtorizacijaCanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("ObjectPrivilege")
    End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()

  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewObjectPrivilege() As ObjectPrivilege
        If Not AvtorizacijaCanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a ObjectPrivilege")
        End If
    Return DataPortal.Create(Of ObjectPrivilege)()
  End Function

  Public Shared Function GetObjectPrivilege(ByVal id As Integer) As ObjectPrivilege
        If Not AvtorizacijaCanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a ObjectPrivilege")
        End If
    Return DataPortal.Fetch(New SingleCriteria(Of ObjectPrivilege, Integer)(Id))
  End Function

  Public Shared Sub DeleteObjectPrivilege(ByVal id As Integer)
        If Not AvtorizacijaCanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a ObjectPrivilege")
        End If
    DataPortal.Delete(New SingleCriteria(Of ObjectPrivilege, Integer)(Id))
  End Sub

  Public Overrides Function Save() As ObjectPrivilege
        If IsDeleted AndAlso Not AvtorizacijaCanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a ObjectPrivilege")
        ElseIf IsNew AndAlso Not AvtorizacijaCanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a ObjectPrivilege")
        ElseIf Not AvtorizacijaCanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a ObjectPrivilege")
        End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewObjectPrivilegeChild() As ObjectPrivilege
    Return DataPortal.CreateChild(Of ObjectPrivilege)()
  End Function

  Friend Shared Function GetObjectPrivilege(ByVal dr As SafeDataReader) As ObjectPrivilege
    Return DataPortal.FetchChild(Of ObjectPrivilege)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of ObjectPrivilege, Integer))
    Database.LogInfo("ObjectPrivilege.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of Integer)(IdRoleProperty, dr.GetInt32("IdRole"))
            LoadProperty(Of Integer)(IdCSLAObjectProperty, dr.GetInt32("IdCSLAObject"))
            LoadProperty(Of Boolean)(CanAddObjectProperty, dr.GetBoolean("CanAddObject"))
            LoadProperty(Of Boolean)(CanGetObjectProperty, dr.GetBoolean("CanGetObject"))
            LoadProperty(Of Boolean)(CanDeleteObjectProperty, dr.GetBoolean("CanDeleteObject"))
            LoadProperty(Of Boolean)(CanEditObjectProperty, dr.GetBoolean("CanEditObject"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("ObjectPrivilege.DataPortal_Fetch", ex)
      Throw New DbCslaException("ObjectPrivilege.DataPortal_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Protected Overrides Sub DataPortal_Insert()
    Try
      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spAdd

            .Parameters.AddWithValue("@IdRole", ReadProperty(Of Integer)(IdRoleProperty))
            .Parameters.AddWithValue("@IdCSLAObject", ReadProperty(Of Integer)(IdCSLAObjectProperty))
            .Parameters.AddWithValue("@CanAddObject", ReadProperty(Of Boolean)(CanAddObjectProperty))
            .Parameters.AddWithValue("@CanGetObject", ReadProperty(Of Boolean)(CanGetObjectProperty))
            .Parameters.AddWithValue("@CanDeleteObject", ReadProperty(Of Boolean)(CanDeleteObjectProperty))
            .Parameters.AddWithValue("@CanEditObject", ReadProperty(Of Boolean)(CanEditObjectProperty))

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
      Database.LogException("ObjectPrivilege.DataPortal_Insert", ex)
      Throw New DbCslaException("ObjectPrivilege.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("ObjectPrivilege.DataPortal_Insert", GetHashCode())
    End Try
  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Protected Overrides Sub DataPortal_Update()
    Try
      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@IdRole", ReadProperty(Of Integer)(IdRoleProperty))
            .Parameters.AddWithValue("@IdCSLAObject", ReadProperty(Of Integer)(IdCSLAObjectProperty))
            .Parameters.AddWithValue("@CanAddObject", ReadProperty(Of Boolean)(CanAddObjectProperty))
            .Parameters.AddWithValue("@CanGetObject", ReadProperty(Of Boolean)(CanGetObjectProperty))
            .Parameters.AddWithValue("@CanDeleteObject", ReadProperty(Of Boolean)(CanDeleteObjectProperty))
            .Parameters.AddWithValue("@CanEditObject", ReadProperty(Of Boolean)(CanEditObjectProperty))
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
    DataPortal_Delete(New SingleCriteria(Of ObjectPrivilege, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of ObjectPrivilege, Integer))
    Database.LogInfo("ObjectPrivilege.DataPortal_Delete", GetHashCode())
    Try
      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
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
      Database.LogException("ObjectPrivilege.DataPortal_Delete", ex)
      Throw New DbCslaException("ObjectPrivilege.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("ObjectPrivilege.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdRoleProperty, dr.GetInt32("IdRole"))
      LoadProperty(Of Integer)(IdCSLAObjectProperty, dr.GetInt32("IdCSLAObject"))
      LoadProperty(Of Boolean)(CanAddObjectProperty, dr.GetBoolean("CanAddObject"))
      LoadProperty(Of Boolean)(CanGetObjectProperty, dr.GetBoolean("CanGetObject"))
      LoadProperty(Of Boolean)(CanDeleteObjectProperty, dr.GetBoolean("CanDeleteObject"))
      LoadProperty(Of Boolean)(CanEditObjectProperty, dr.GetBoolean("CanEditObject"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("ObjectPrivilege.Child_Fetch", ex)
      Throw New DbCslaException("ObjectPrivilege.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert()
    Try
      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spAdd
            .Parameters.AddWithValue("@IdRole", ReadProperty(Of Integer)(IdRoleProperty))
            .Parameters.AddWithValue("@IdCSLAObject", ReadProperty(Of Integer)(IdCSLAObjectProperty))
            .Parameters.AddWithValue("@CanAddObject", ReadProperty(Of Boolean)(CanAddObjectProperty))
            .Parameters.AddWithValue("@CanGetObject", ReadProperty(Of Boolean)(CanGetObjectProperty))
            .Parameters.AddWithValue("@CanDeleteObject", ReadProperty(Of Boolean)(CanDeleteObjectProperty))
            .Parameters.AddWithValue("@CanEditObject", ReadProperty(Of Boolean)(CanEditObjectProperty))

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
      Database.LogException("ObjectPrivilege.Child_Insert", ex)
      Throw New DbCslaException("ObjectPrivilege.Child_Insert", ex)
    Finally
      Database.LogInfo("ObjectPrivilege.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("ObjectPrivilege.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        ApplicationContext.LocalContext.Remove("cn")
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@IdRole", ReadProperty(Of Integer)(IdRoleProperty))
            .Parameters.AddWithValue("@IdCSLAObject", ReadProperty(Of Integer)(IdCSLAObjectProperty))
            .Parameters.AddWithValue("@CanAddObject", ReadProperty(Of Boolean)(CanAddObjectProperty))
            .Parameters.AddWithValue("@CanGetObject", ReadProperty(Of Boolean)(CanGetObjectProperty))
            .Parameters.AddWithValue("@CanDeleteObject", ReadProperty(Of Boolean)(CanDeleteObjectProperty))
            .Parameters.AddWithValue("@CanEditObject", ReadProperty(Of Boolean)(CanEditObjectProperty))
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
      Database.LogException("ObjectPrivilege.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("ObjectPrivilege.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("ObjectPrivilege.Child_DeleteSelf", GetHashCode)
    Try
      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
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
      Database.LogException("ObjectPrivilege.Child_Fetch", ex)
      Throw New DbCslaException("ObjectPrivilege.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
