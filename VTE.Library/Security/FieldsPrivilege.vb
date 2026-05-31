
<Serializable()> _
Public Class FieldsPrivilege
    Inherits Csla.BusinessBase(Of FieldsPrivilege)


#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "getFieldsPrivileges"
    Private Const SpZemiPoID As String = "getFieldsPrivilegeById"
    Private Const SpSnimi As String = "updateFieldsPrivilege"
    Private Const SpDodadi As String = "addFieldsPrivilege"
    Private Const SpIzbrisi As String = "deleteFieldsPrivilege"

#End Region

#Region " Business Properties and Methods "
    'register properties
    Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(FieldsPrivilege), New PropertyInfo(Of Long)("Id"))
    Private Shared IdRoleProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(FieldsPrivilege), New PropertyInfo(Of Integer)("IdRole"))
    Private Shared IdCSLAObjectProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(FieldsPrivilege), New PropertyInfo(Of Integer)("IdCSLAObject"))
    Private Shared cSLAObjectPropertyNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(FieldsPrivilege), New PropertyInfo(Of String)("cSLAObjectPropertyName"))

    Private _lastChanged(7) As Byte

    <System.ComponentModel.DataObjectField(True, True)> _
    Public ReadOnly Property Id() As Long
        Get
            Return GetProperty(Of Long)(IdProperty)
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
    Public Property cSLAObjectPropertyName() As String
        Get
            Return GetProperty(Of String)(cSLAObjectPropertyNameProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(cSLAObjectPropertyNameProperty, value)
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
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("cSLAObjectPropertyName") Then
    '            AuthorizationRules.AllowWrite("cSLAObjectPropertyName", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("cSLAObjectPropertyName", roleName)
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
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanGet("FieldsPrivilege")
    '    End Function

    '    Public Shared Function AvtorizacijaCanAddObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanAdd("FieldsPrivilege")
    '    End Function

    '    Public Shared Function AvtorizacijaCanEditObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanEdit("FieldsPrivilege")
    '    End Function

    '    Public Shared Function AvtorizacijaCanDeleteObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanDelete("FieldsPrivilege")
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

    Public Shared Function NewFieldsPrivilege() As FieldsPrivilege
        'If Not AvtorizacijaCanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a FieldsPrivilege")
        'End If
        Return DataPortal.Create(Of FieldsPrivilege)()
    End Function

    Public Shared Function GetFieldsPrivilege(ByVal id As Integer) As FieldsPrivilege
        'If Not AvtorizacijaCanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a FieldsPrivilege")
        'End If
        Return DataPortal.Fetch(New SingleCriteria(Of FieldsPrivilege, Integer)(id))
    End Function

    Public Shared Sub DeleteFieldsPrivilege(ByVal id As Integer)
        'If Not AvtorizacijaCanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a FieldsPrivilege")
        'End If
        DataPortal.Delete(New SingleCriteria(Of FieldsPrivilege, Integer)(id))
    End Sub

    Public Overrides Function Save() As FieldsPrivilege
        'If IsDeleted AndAlso Not AvtorizacijaCanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a FieldsPrivilege")
        'ElseIf IsNew AndAlso Not AvtorizacijaCanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a FieldsPrivilege")
        'ElseIf Not AvtorizacijaCanEditObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to update a FieldsPrivilege")
        'End If
        Return MyBase.Save()
    End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

    Friend Shared Function NewFieldsPrivilegeChild() As FieldsPrivilege
        Return DataPortal.CreateChild(Of FieldsPrivilege)()
    End Function

    Friend Shared Function GetFieldsPrivilege(ByVal dr As SafeDataReader) As FieldsPrivilege
        Return DataPortal.FetchChild(Of FieldsPrivilege)(dr)
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

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of FieldsPrivilege, Integer))
        Database.LogInfo("FieldsPrivilege.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = SpZemiPoID
                    cm.Parameters.AddWithValue("@Id", criteria.Value)
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        dr.Read()

                        LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
                        LoadProperty(Of Integer)(IdRoleProperty, dr.GetInt32("IdRole"))
                        LoadProperty(Of Integer)(IdCSLAObjectProperty, dr.GetInt32("IdCSLAObject"))
                        LoadProperty(Of String)(cSLAObjectPropertyNameProperty, dr.GetString("CSLAObjectPropertyName"))
                        

                        dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("FieldsPrivilege.DataPortal_Fetch", ex)
            Throw New DbCslaException("FieldsPrivilege.DataPortal_Fetch", ex)
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
                        .CommandText = SpDodadi

                        .Parameters.AddWithValue("@IdRole", ReadProperty(Of Integer)(IdRoleProperty))
                        .Parameters.AddWithValue("@IdCSLAObject", ReadProperty(Of Integer)(IdCSLAObjectProperty))
                        .Parameters.AddWithValue("@CSLAObjectPropertyName", ReadProperty(Of String)(cSLAObjectPropertyNameProperty))
                        
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
            Database.LogException("FieldsPrivilege.DataPortal_Insert", ex)
            Throw New DbCslaException("FieldsPrivilege.DataPortal_Insert", ex)
        Finally
            Database.LogInfo("FieldsPrivilege.DataPortal_Insert", GetHashCode())
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
                        .CommandText = SpSnimi

                        .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
                        .Parameters.AddWithValue("@IdRole", ReadProperty(Of Integer)(IdRoleProperty))
                        .Parameters.AddWithValue("@IdCSLAObject", ReadProperty(Of Integer)(IdCSLAObjectProperty))
                        .Parameters.AddWithValue("@CSLAObjectPropertyName", ReadProperty(Of String)(cSLAObjectPropertyNameProperty))
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
        DataPortal_Delete(New SingleCriteria(Of FieldsPrivilege, Integer)(Id))
    End Sub

    Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of FieldsPrivilege, Integer))
        Database.LogInfo("FieldsPrivilege.DataPortal_Delete", GetHashCode())
        Try
            Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = SpIzbrisi
                        .Parameters.AddWithValue("@id", criteria.Value)
                        .ExecuteNonQuery()
                    End With
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("FieldsPrivilege.DataPortal_Delete", ex)
            Throw New DbCslaException("FieldsPrivilege.DataPortal_Delete", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

    Private Sub Child_Fetch(ByVal dr As SafeDataReader)
        Database.LogInfo("FieldsPrivilege.Child_Fetch", GetHashCode())
        Try
            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of Integer)(IdRoleProperty, dr.GetInt32("IdRole"))
            LoadProperty(Of Integer)(IdCSLAObjectProperty, dr.GetInt32("IdCSLAObject"))
            LoadProperty(Of String)(cSLAObjectPropertyNameProperty, dr.GetString("CSLAObjectPropertyName"))
       

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
        Catch ex As Exception
            Database.LogException("FieldsPrivilege.Child_Fetch", ex)
            Throw New DbCslaException("FieldsPrivilege.Child_Fetch", ex)
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
                        .CommandText = SpDodadi
                        .Parameters.AddWithValue("@IdRole", ReadProperty(Of Integer)(IdRoleProperty))
                        .Parameters.AddWithValue("@IdCSLAObject", ReadProperty(Of Integer)(IdCSLAObjectProperty))
                        .Parameters.AddWithValue("@CSLAObjectPropertyName", ReadProperty(Of String)(cSLAObjectPropertyNameProperty))
                        
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
            Database.LogException("FieldsPrivilege.Child_Insert", ex)
            Throw New DbCslaException("FieldsPrivilege.Child_Insert", ex)
        Finally
            Database.LogInfo("FieldsPrivilege.Child_Insert", GetHashCode)
        End Try

    End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

    Private Sub Child_Update()
        Database.LogInfo("FieldsPrivilege.Child_Update", GetHashCode)
        Try
            Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
                ApplicationContext.LocalContext.Remove("cn")
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = SpSnimi

                        .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
                        .Parameters.AddWithValue("@IdRole", ReadProperty(Of Integer)(IdRoleProperty))
                        .Parameters.AddWithValue("@IdCSLAObject", ReadProperty(Of Integer)(IdCSLAObjectProperty))
                        .Parameters.AddWithValue("@CSLAObjectPropertyName", ReadProperty(Of String)(cSLAObjectPropertyNameProperty))
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
            Database.LogException("FieldsPrivilege.Child_Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DbCslaException("FieldsPrivilege.Child_Update", ex)
            End If
        End Try
    End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

    Private Sub Child_DeleteSelf()

        Database.LogInfo("FieldsPrivilege.Child_DeleteSelf", GetHashCode)
        Try
            Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = SpIzbrisi
                        .Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
                        .ExecuteNonQuery()
                    End With
                End Using

            End Using
        Catch ex As Exception
            Database.LogException("FieldsPrivilege.Child_Fetch", ex)
            Throw New DbCslaException("FieldsPrivilege.Child_Fetch", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
