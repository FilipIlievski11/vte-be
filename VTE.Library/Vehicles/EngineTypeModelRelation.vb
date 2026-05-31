
<Serializable()> _
Public Class EngineTypeModelRelation
  Inherits Csla.BusinessBase(Of EngineTypeModelRelation)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetEngineTypeModelRelationByID"
  Private Const spGetAll As String = "GetEngineTypeModelRelations"
  Private Const spUpdate As String = "updateEngineTypeModelRelation"
  Private Const spAdd As String = "addEngineTypeModelRelation"
  Private Const spDelete As String = "deleteEngineTypeModelRelation"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(EngineTypeModelRelation), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdEngineTypeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(EngineTypeModelRelation), New PropertyInfo(Of Integer)("IdEngineType"))
  Private Shared IdVehicleModelProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(EngineTypeModelRelation), New PropertyInfo(Of Integer)("IdVehicleModel"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdEngineType() As Integer
    Get
      Return GetProperty(Of Integer)(IdEngineTypeProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdEngineTypeProperty, value)
    End Set
  End Property
  Public Property IdVehicleModel() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleModelProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleModelProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdEngineType") Then
      AuthorizationRules.AllowWrite("IdEngineType", roleName)
    Else
      AuthorizationRules.DenyWrite("IdEngineType", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdEngineType")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicleModel") Then
      AuthorizationRules.AllowWrite("IdVehicleModel", roleName)
    Else
      AuthorizationRules.DenyWrite("IdVehicleModel", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdVehicleModel")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("EngineTypeModelRelation")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("EngineTypeModelRelation")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("EngineTypeModelRelation")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("EngineTypeModelRelation")
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

  Public Shared Function NewEngineTypeModelRelation() As EngineTypeModelRelation
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a EngineTypeModelRelation")
    End If
    Return DataPortal.Create(Of EngineTypeModelRelation)()
  End Function

  Public Shared Function GetEngineTypeModelRelation(ByVal id As Integer) As EngineTypeModelRelation
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a EngineTypeModelRelation")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of EngineTypeModelRelation, Integer)(Id))
  End Function

  Public Shared Sub DeleteEngineTypeModelRelation(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a EngineTypeModelRelation")
    End If
    DataPortal.Delete(New SingleCriteria(Of EngineTypeModelRelation, Integer)(Id))
  End Sub

  Public Overrides Function Save() As EngineTypeModelRelation
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a EngineTypeModelRelation")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a EngineTypeModelRelation")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a EngineTypeModelRelation")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewEngineTypeModelRelationChild() As EngineTypeModelRelation
    Return DataPortal.CreateChild(Of EngineTypeModelRelation)()
  End Function

  Friend Shared Function GetEngineTypeModelRelation(ByVal dr As SafeDataReader) As EngineTypeModelRelation
    Return DataPortal.FetchChild(Of EngineTypeModelRelation)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of EngineTypeModelRelation, Integer))
    Database.LogInfo("EngineTypeModelRelation.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of Integer)(IdEngineTypeProperty, dr.GetInt32("IdEngineType"))
            LoadProperty(Of Integer)(IdVehicleModelProperty, dr.GetInt32("IdVehicleModel"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("EngineTypeModelRelation.DataPortal_Fetch", ex)
      Throw New DbCslaException("EngineTypeModelRelation.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@IdEngineType", ReadProperty(Of Integer)(IdEngineTypeProperty))
            .Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))

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
      Database.LogException("EngineTypeModelRelation.DataPortal_Insert", ex)
      Throw New DbCslaException("EngineTypeModelRelation.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("EngineTypeModelRelation.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@IdEngineType", ReadProperty(Of Integer)(IdEngineTypeProperty))
            .Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))
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
    DataPortal_Delete(New SingleCriteria(Of EngineTypeModelRelation, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of EngineTypeModelRelation, Integer))
    Database.LogInfo("EngineTypeModelRelation.DataPortal_Delete", GetHashCode())
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
      Database.LogException("EngineTypeModelRelation.DataPortal_Delete", ex)
      Throw New DbCslaException("EngineTypeModelRelation.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("EngineTypeModelRelation.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdEngineTypeProperty, dr.GetInt32("IdEngineType"))
      LoadProperty(Of Integer)(IdVehicleModelProperty, dr.GetInt32("IdVehicleModel"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("EngineTypeModelRelation.Child_Fetch", ex)
      Throw New DbCslaException("EngineTypeModelRelation.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@IdEngineType", ReadProperty(Of Integer)(IdEngineTypeProperty))
            .Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))

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
      Database.LogException("EngineTypeModelRelation.Child_Insert", ex)
      Throw New DbCslaException("EngineTypeModelRelation.Child_Insert", ex)
    Finally
      Database.LogInfo("EngineTypeModelRelation.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("EngineTypeModelRelation.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@IdEngineType", ReadProperty(Of Integer)(IdEngineTypeProperty))
            .Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))
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
      Database.LogException("EngineTypeModelRelation.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("EngineTypeModelRelation.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("EngineTypeModelRelation.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("EngineTypeModelRelation.Child_Fetch", ex)
      Throw New DbCslaException("EngineTypeModelRelation.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
