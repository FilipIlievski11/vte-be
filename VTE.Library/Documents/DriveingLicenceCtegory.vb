
<Serializable()> _
Public Class DriveingLicenceCtegory
  Inherits Csla.BusinessBase(Of DriveingLicenceCtegory)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDriveingLicenceCtegorieByID"
  Private Const spGetAll As String = "GetDriveingLicenceCtegories"
  Private Const spUpdate As String = "updateDriveingLicenceCtegorie"
  Private Const spAdd As String = "addDriveingLicenceCtegorie"
  Private Const spDelete As String = "deleteDriveingLicenceCtegorie"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DriveingLicenceCtegory), New PropertyInfo(Of Long)("Id"))
  Private Shared CodeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DriveingLicenceCtegory), New PropertyInfo(Of String)("Code"))
  Private Shared DescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DriveingLicenceCtegory), New PropertyInfo(Of String)("Description"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Long
    Get
      Return GetProperty(Of Long)(IdProperty)
    End Get
  End Property
  Public Property Code() As String
    Get
      Return GetProperty(Of String)(CodeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CodeProperty, value)
    End Set
  End Property
  Public Property Description() As String
    Get
      Return GetProperty(Of String)(DescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(DescriptionProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Code") Then
      AuthorizationRules.AllowWrite("Code", roleName)
    Else
      AuthorizationRules.DenyWrite("Code", roleName)
    End If
    'AuthorizationRules.AllowWrite("Code")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Description") Then
      AuthorizationRules.AllowWrite("Description", roleName)
    Else
      AuthorizationRules.DenyWrite("Description", roleName)
    End If
    'AuthorizationRules.AllowWrite("Description")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DriveingLicenceCtegory")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DriveingLicenceCtegory")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DriveingLicenceCtegory")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DriveingLicenceCtegory")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' CodeProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CodeProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CodeProperty, 10))
    ' DescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, DescriptionProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(DescriptionProperty, 250))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewDriveingLicenceCtegory() As DriveingLicenceCtegory
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a DriveingLicenceCtegory")
    End If
    Return DataPortal.Create(Of DriveingLicenceCtegory)()
  End Function

  Public Shared Function GetDriveingLicenceCtegory(ByVal id As Long) As DriveingLicenceCtegory
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a DriveingLicenceCtegory")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of DriveingLicenceCtegory, Integer)(Id))
  End Function

  Public Shared Sub DeleteDriveingLicenceCtegory(ByVal id As Long)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a DriveingLicenceCtegory")
    End If
    DataPortal.Delete(New SingleCriteria(Of DriveingLicenceCtegory, Integer)(Id))
  End Sub

  Public Overrides Function Save() As DriveingLicenceCtegory
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a DriveingLicenceCtegory")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a DriveingLicenceCtegory")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a DriveingLicenceCtegory")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewDriveingLicenceCtegoryChild() As DriveingLicenceCtegory
    Return DataPortal.CreateChild(Of DriveingLicenceCtegory)()
  End Function

  Friend Shared Function GetDriveingLicenceCtegory(ByVal dr As SafeDataReader) As DriveingLicenceCtegory
    Return DataPortal.FetchChild(Of DriveingLicenceCtegory)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of DriveingLicenceCtegory, Integer))
    Database.LogInfo("DriveingLicenceCtegory.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of String)(CodeProperty, dr.GetString("Code"))
            LoadProperty(Of String)(DescriptionProperty, dr.GetString("Description"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DriveingLicenceCtegory.DataPortal_Fetch", ex)
      Throw New DbCslaException("DriveingLicenceCtegory.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
            .Parameters.AddWithValue("@Description", ReadProperty(Of String)(DescriptionProperty))

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
      Database.LogException("DriveingLicenceCtegory.DataPortal_Insert", ex)
      Throw New DbCslaException("DriveingLicenceCtegory.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("DriveingLicenceCtegory.DataPortal_Insert", GetHashCode())
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

            .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
            .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
            .Parameters.AddWithValue("@Description", ReadProperty(Of String)(DescriptionProperty))
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
    DataPortal_Delete(New SingleCriteria(Of DriveingLicenceCtegory, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of DriveingLicenceCtegory, Integer))
    Database.LogInfo("DriveingLicenceCtegory.DataPortal_Delete", GetHashCode())
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
      Database.LogException("DriveingLicenceCtegory.DataPortal_Delete", ex)
      Throw New DbCslaException("DriveingLicenceCtegory.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("DriveingLicenceCtegory.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of String)(CodeProperty, dr.GetString("Code"))
      LoadProperty(Of String)(DescriptionProperty, dr.GetString("Description"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("DriveingLicenceCtegory.Child_Fetch", ex)
      Throw New DbCslaException("DriveingLicenceCtegory.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
            .Parameters.AddWithValue("@Description", ReadProperty(Of String)(DescriptionProperty))

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
      Database.LogException("DriveingLicenceCtegory.Child_Insert", ex)
      Throw New DbCslaException("DriveingLicenceCtegory.Child_Insert", ex)
    Finally
      Database.LogInfo("DriveingLicenceCtegory.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("DriveingLicenceCtegory.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
            .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
            .Parameters.AddWithValue("@Description", ReadProperty(Of String)(DescriptionProperty))
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
      Database.LogException("DriveingLicenceCtegory.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("DriveingLicenceCtegory.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("DriveingLicenceCtegory.Child_DeleteSelf", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spDelete
            .Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
            .ExecuteNonQuery()
          End With
        End Using

      End Using
    Catch ex As Exception
      Database.LogException("DriveingLicenceCtegory.Child_Fetch", ex)
      Throw New DbCslaException("DriveingLicenceCtegory.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
