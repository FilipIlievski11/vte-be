
<Serializable()> _
Public Class Rool
    Inherits Csla.BusinessBase(Of Rool)


#Region " Stored Procedures Names "
    Private Const spGetByID As String = "GetRoleByID"
    Private Const spGetAll As String = "GetRoles"
    Private Const spUpdate As String = "updateRole"
    Private Const spAdd As String = "addRole"
    Private Const spDelete As String = "deleteRole"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
    'register properties
    Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Rool), New PropertyInfo(Of Integer)("Id"))
    Private Shared RoleNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Rool), New PropertyInfo(Of String)("RoleName"))

    Private _lastChanged(7) As Byte
    <System.ComponentModel.DataObjectField(True, True)> _
    Public ReadOnly Property Id() As Integer
        Get
            Return GetProperty(Of Integer)(IdProperty)
        End Get
    End Property
    Public Property RoleName() As String
        Get
            Return GetProperty(Of String)(RoleNameProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(RoleNameProperty, value)
        End Set
    End Property
   


    Public Overrides Function ToString() As String
        Return Id.ToString
    End Function
#End Region 'Business Properties and Methods


    '#Region " Authorization Rules "

    '    Protected Overrides Sub AddAuthorizationRules()
    '        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdRole") Then
    '            AuthorizationRules.AllowWrite("IdRole", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("IdRole", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("IdRole")
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCSLAObject") Then
    '            AuthorizationRules.AllowWrite("IdCSLAObject", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("IdCSLAObject", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("IdCSLAObject")
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CanAddObject") Then
    '            AuthorizationRules.AllowWrite("CanAddObject", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("CanAddObject", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("CanAddObject")
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CanGetObject") Then
    '            AuthorizationRules.AllowWrite("CanGetObject", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("CanGetObject", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("CanGetObject")
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CanDeleteObject") Then
    '            AuthorizationRules.AllowWrite("CanDeleteObject", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("CanDeleteObject", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("CanDeleteObject")
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CanEditObject") Then
    '            AuthorizationRules.AllowWrite("CanEditObject", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("CanEditObject", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("CanEditObject")
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
    '            AuthorizationRules.AllowWrite("Active", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("Active", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("Active")
    '    End Sub


    '    Public Shared Function AvtorizacijaCanGetObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInRoolsCanGet("Rool")
    '    End Function

    '    Public Shared Function AvtorizacijaCanAddObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInRoolsCanAdd("Rool")
    '    End Function

    '    Public Shared Function AvtorizacijaCanEditObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInRoolsCanEdit("Rool")
    '    End Function

    '    Public Shared Function AvtorizacijaCanDeleteObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInRoolsCanDelete("Rool")
    '    End Function

    '#End Region ' Authorization Rules


#Region " Validation Rules "
    Protected Overrides Sub AddBusinessRules()

    End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

    Private Sub New()
        ' require use of factory method 
    End Sub

    Public Shared Function NewRool() As Rool
        'If Not AvtorizacijaCanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a Rool")
        'End If
        Return DataPortal.Create(Of Rool)()
    End Function

    Public Shared Function GetRool(ByVal id As Integer) As Rool
        'If Not AvtorizacijaCanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a Rool")
        'End If
        Return DataPortal.Fetch(New SingleCriteria(Of Rool, Integer)(id))
    End Function

    Public Shared Sub DeleteRool(ByVal id As Integer)
        'If Not AvtorizacijaCanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a Rool")
        'End If
        DataPortal.Delete(New SingleCriteria(Of Rool, Integer)(id))
    End Sub

    Public Overrides Function Save() As Rool
        'If IsDeleted AndAlso Not AvtorizacijaCanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a Rool")
        'ElseIf IsNew AndAlso Not AvtorizacijaCanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a Rool")
        'ElseIf Not AvtorizacijaCanEditObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to update a Rool")
        'End If
        Return MyBase.Save()
    End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

    Friend Shared Function NewRoolChild() As Rool
        Return DataPortal.CreateChild(Of Rool)()
    End Function

    Friend Shared Function GetRool(ByVal dr As SafeDataReader) As Rool
        Return DataPortal.FetchChild(Of Rool)(dr)
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

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of Rool, Integer))
        Database.LogInfo("Rool.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = spGetByID
                    cm.Parameters.AddWithValue("@Id", criteria.Value)
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        dr.Read()

                        LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
                        LoadProperty(Of String)(RoleNameProperty, dr.GetString("RoleName"))
                      

                        dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("Rool.DataPortal_Fetch", ex)
            Throw New DbCslaException("Rool.DataPortal_Fetch", ex)
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

                        .Parameters.AddWithValue("@RoleName", ReadProperty(Of String)(RoleNameProperty))
                       
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
            Database.LogException("Rool.DataPortal_Insert", ex)
            Throw New DbCslaException("Rool.DataPortal_Insert", ex)
        Finally
            Database.LogInfo("Rool.DataPortal_Insert", GetHashCode())
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
                        .Parameters.AddWithValue("@RoleName", ReadProperty(Of String)(RoleNameProperty))
                        
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
        DataPortal_Delete(New SingleCriteria(Of Rool, Integer)(Id))
    End Sub

    Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of Rool, Integer))
        Database.LogInfo("Rool.DataPortal_Delete", GetHashCode())
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
            Database.LogException("Rool.DataPortal_Delete", ex)
            Throw New DbCslaException("Rool.DataPortal_Delete", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

    Private Sub Child_Fetch(ByVal dr As SafeDataReader)
        Database.LogInfo("Rool.Child_Fetch", GetHashCode())
        Try
            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(RoleNameProperty, dr.GetString("RoleName"))
            

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
        Catch ex As Exception
            Database.LogException("Rool.Child_Fetch", ex)
            Throw New DbCslaException("Rool.Child_Fetch", ex)
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
                        .Parameters.AddWithValue("@RoleName", ReadProperty(Of String)(RoleNameProperty))
                        
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
            Database.LogException("Rool.Child_Insert", ex)
            Throw New DbCslaException("Rool.Child_Insert", ex)
        Finally
            Database.LogInfo("Rool.Child_Insert", GetHashCode)
        End Try

    End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

    Private Sub Child_Update()
        Database.LogInfo("Rool.Child_Update", GetHashCode)
        Try
            Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
                ApplicationContext.LocalContext.Remove("cn")
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = spUpdate

                        .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
                            .Parameters.AddWithValue("@RoleName", ReadProperty(Of String)(RoleNameProperty))
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
            Database.LogException("Rool.Child_Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DbCslaException("Rool.Child_Update", ex)
            End If
        End Try
    End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

    Private Sub Child_DeleteSelf()

        Database.LogInfo("Rool.Child_DeleteSelf", GetHashCode)
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
            Database.LogException("Rool.Child_Fetch", ex)
            Throw New DbCslaException("Rool.Child_Fetch", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
