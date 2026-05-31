
<Serializable()> _
Public Class CSLAObject
  Inherits Csla.BusinessBase(Of CSLAObject)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCSLAObjectByID"
  Private Const spGetAll As String = "GetCSLAObjects"
  Private Const spUpdate As String = "updateCSLAObject"
  Private Const spAdd As String = "addCSLAObject"
  Private Const spDelete As String = "deleteCSLAObject"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(CSLAObject), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdCSLAObjectProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(CSLAObject), New PropertyInfo(Of Integer)("IdCSLAObject"))
  Private Shared CSLAObjectNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CSLAObject), New PropertyInfo(Of String)("CSLAObjectName"))
  Private Shared CSLAObjectTypeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CSLAObject), New PropertyInfo(Of String)("CSLAObjectType"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdCSLAObject() As Integer
    Get
      Return GetProperty(Of Integer)(IdCSLAObjectProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdCSLAObjectProperty, value)
    End Set
  End Property
  Public Property CSLAObjectName() As String
    Get
      Return GetProperty(Of String)(CSLAObjectNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CSLAObjectNameProperty, value)
    End Set
  End Property
  Public Property CSLAObjectType() As String
    Get
      Return GetProperty(Of String)(CSLAObjectTypeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CSLAObjectTypeProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCSLAObject") Then
            AuthorizationRules.AllowWrite("IdCSLAObject", roleName)
        Else
            AuthorizationRules.DenyWrite("IdCSLAObject", roleName)
        End If
        'AuthorizationRules.AllowWrite("IdCSLAObject")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CSLAObjectName") Then
            AuthorizationRules.AllowWrite("CSLAObjectName", roleName)
        Else
            AuthorizationRules.DenyWrite("CSLAObjectName", roleName)
        End If
        'AuthorizationRules.AllowWrite("CSLAObjectName")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CSLAObjectType") Then
            AuthorizationRules.AllowWrite("CSLAObjectType", roleName)
        Else
            AuthorizationRules.DenyWrite("CSLAObjectType", roleName)
        End If
        'AuthorizationRules.AllowWrite("CSLAObjectType")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
            AuthorizationRules.AllowWrite("Active", roleName)
        Else
            AuthorizationRules.DenyWrite("Active", roleName)
        End If
        'AuthorizationRules.AllowWrite("Active")
    End Sub


    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("CSLAObject")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("CSLAObject")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("CSLAObject")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("CSLAObject")
    End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' CSLAObjectNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CSLAObjectNameProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CSLAObjectNameProperty, 100))
    ' CSLAObjectTypeProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CSLAObjectTypeProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CSLAObjectTypeProperty, 100))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewCSLAObject() As CSLAObject
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a CSLAObject")
        End If
    Return DataPortal.Create(Of CSLAObject)()
  End Function

  Public Shared Function GetCSLAObject(ByVal id As Integer) As CSLAObject
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a CSLAObject")
        End If
    Return DataPortal.Fetch(New SingleCriteria(Of CSLAObject, Integer)(Id))
  End Function

  Public Shared Sub DeleteCSLAObject(ByVal id As Integer)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a CSLAObject")
        End If
    DataPortal.Delete(New SingleCriteria(Of CSLAObject, Integer)(Id))
  End Sub

    Public Overrides Function Save() As CSLAObject
        If IsDeleted AndAlso Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a CSLAObject")
        ElseIf IsNew AndAlso Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a CSLAObject")
        ElseIf Not CanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a CSLAObject")
        End If
        Return MyBase.Save()
    End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewCSLAObjectChild() As CSLAObject
    Return DataPortal.CreateChild(Of CSLAObject)()
  End Function

  Friend Shared Function GetCSLAObject(ByVal dr As SafeDataReader) As CSLAObject
    Return DataPortal.FetchChild(Of CSLAObject)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of CSLAObject, Integer))
    Database.LogInfo("CSLAObject.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of Integer)(IdCSLAObjectProperty, dr.GetInt32("IdCSLAObject"))
            LoadProperty(Of String)(CSLAObjectNameProperty, dr.GetString("CSLAObjectName"))
            LoadProperty(Of String)(CSLAObjectTypeProperty, dr.GetString("CSLAObjectType"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("CSLAObject.DataPortal_Fetch", ex)
      Throw New DbCslaException("CSLAObject.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@IdCSLAObject", ReadProperty(Of Integer)(IdCSLAObjectProperty))
            .Parameters.AddWithValue("@CSLAObjectName", ReadProperty(Of String)(CSLAObjectNameProperty))
            .Parameters.AddWithValue("@CSLAObjectType", ReadProperty(Of String)(CSLAObjectTypeProperty))

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
      Database.LogException("CSLAObject.DataPortal_Insert", ex)
      Throw New DbCslaException("CSLAObject.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("CSLAObject.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@IdCSLAObject", ReadProperty(Of Integer)(IdCSLAObjectProperty))
            .Parameters.AddWithValue("@CSLAObjectName", ReadProperty(Of String)(CSLAObjectNameProperty))
            .Parameters.AddWithValue("@CSLAObjectType", ReadProperty(Of String)(CSLAObjectTypeProperty))
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
    DataPortal_Delete(New SingleCriteria(Of CSLAObject, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of CSLAObject, Integer))
    Database.LogInfo("CSLAObject.DataPortal_Delete", GetHashCode())
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
      Database.LogException("CSLAObject.DataPortal_Delete", ex)
      Throw New DbCslaException("CSLAObject.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("CSLAObject.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdCSLAObjectProperty, dr.GetInt32("IdCSLAObject"))
      LoadProperty(Of String)(CSLAObjectNameProperty, dr.GetString("CSLAObjectName"))
      LoadProperty(Of String)(CSLAObjectTypeProperty, dr.GetString("CSLAObjectType"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("CSLAObject.Child_Fetch", ex)
      Throw New DbCslaException("CSLAObject.Child_Fetch", ex)
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
                        .Parameters.AddWithValue("@IdCSLAObject", ReadProperty(Of Integer)(IdCSLAObjectProperty))
                        .Parameters.AddWithValue("@CSLAObjectName", ReadProperty(Of String)(CSLAObjectNameProperty))
                        .Parameters.AddWithValue("@CSLAObjectType", ReadProperty(Of String)(CSLAObjectTypeProperty))

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
      Database.LogException("CSLAObject.Child_Insert", ex)
      Throw New DbCslaException("CSLAObject.Child_Insert", ex)
    Finally
      Database.LogInfo("CSLAObject.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("CSLAObject.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        ApplicationContext.LocalContext.Remove("cn")
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@IdCSLAObject", ReadProperty(Of Integer)(IdCSLAObjectProperty))
            .Parameters.AddWithValue("@CSLAObjectName", ReadProperty(Of String)(CSLAObjectNameProperty))
            .Parameters.AddWithValue("@CSLAObjectType", ReadProperty(Of String)(CSLAObjectTypeProperty))
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
      Database.LogException("CSLAObject.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("CSLAObject.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("CSLAObject.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("CSLAObject.Child_Fetch", ex)
      Throw New DbCslaException("CSLAObject.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
