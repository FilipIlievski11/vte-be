
<Serializable()> _
Public Class Color
 Inherits Csla.BusinessBase(Of Color)


#Region " Stored Procedures Names "
 Private Const spGetByID As String = "GetColorByID"
 Private Const spGetAll As String = "GetColors"
 Private Const spUpdate As String = "updateColor"
 Private Const spAdd As String = "addColor"
 Private Const spDelete As String = "deleteColor"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
 'register properties
 Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Color), New PropertyInfo(Of Integer)("Id"))
 Private Shared ColorCodeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Color), New PropertyInfo(Of String)("ColorCode"))
 Private Shared ColorDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Color), New PropertyInfo(Of String)("ColorDescription"))
 Private Shared NewColorEffectsProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Color), New PropertyInfo(Of String)("NewColorEffects"))
 Private Shared NewColorCodeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Color), New PropertyInfo(Of Integer)("NewColorCode"))
 Private Shared NewColorDarknessProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Color), New PropertyInfo(Of String)("NewColorDarkness"))

 Private _lastChanged(7) As Byte

 <System.ComponentModel.DataObjectField(True, True)> _
 Public ReadOnly Property Id() As Integer
  Get
   Return GetProperty(Of Integer)(IdProperty)
  End Get
 End Property
 Public Property ColorCode() As String
  Get
   Return GetProperty(Of String)(ColorCodeProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(ColorCodeProperty, value)
  End Set
 End Property
 Public Property ColorDescription() As String
  Get
   Return GetProperty(Of String)(ColorDescriptionProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(ColorDescriptionProperty, value)
  End Set
 End Property
 Public Property NewColorEffects() As String
  Get
   Return GetProperty(Of String)(NewColorEffectsProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(NewColorEffectsProperty, value)
  End Set
 End Property

 Public ReadOnly Property Color() As String
  Get
   Return ColorCode & "-" & ColorDescription
  End Get
 End Property
 Public Property NewColorCode() As Integer
  Get
   Return GetProperty(Of Integer)(NewColorCodeProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(NewColorCodeProperty, value)
  End Set
 End Property
 Public Property NewColorDarkness() As String
  Get
   Return GetProperty(Of String)(NewColorDarknessProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(NewColorDarknessProperty, value)
  End Set
 End Property

 Public Overrides Function ToString() As String
  Return Id.ToString
 End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

 Protected Overrides Sub AddAuthorizationRules()
  Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ColorCode") Then
   AuthorizationRules.AllowWrite("ColorCode", roleName)
  Else
   AuthorizationRules.DenyWrite("ColorCode", roleName)
  End If
  'AuthorizationRules.AllowWrite("ColorCode")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ColorDescription") Then
   AuthorizationRules.AllowWrite("ColorDescription", roleName)
  Else
   AuthorizationRules.DenyWrite("ColorDescription", roleName)
  End If
  'AuthorizationRules.AllowWrite("ColorDescription")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NewColorEffects") Then
   AuthorizationRules.AllowWrite("NewColorEffects", roleName)
  Else
   AuthorizationRules.DenyWrite("NewColorEffects", roleName)
  End If
  'AuthorizationRules.AllowWrite("NewColorEffects")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NewColorCode") Then
   AuthorizationRules.AllowWrite("NewColorCode", roleName)
  Else
   AuthorizationRules.DenyWrite("NewColorCode", roleName)
  End If
  'AuthorizationRules.AllowWrite("NewColorCode")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NewColorDarkness") Then
   AuthorizationRules.AllowWrite("NewColorDarkness", roleName)
  Else
   AuthorizationRules.DenyWrite("NewColorDarkness", roleName)
  End If
  'AuthorizationRules.AllowWrite("NewColorDarkness")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
   AuthorizationRules.AllowWrite("Active", roleName)
  Else
   AuthorizationRules.DenyWrite("Active", roleName)
  End If
  'AuthorizationRules.AllowWrite("Active")
 End Sub



 Public Shared Function CanGetObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Color")
 End Function

 Public Shared Function CanAddObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Color")
 End Function

 Public Shared Function CanEditObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Color")
 End Function

 Public Shared Function CanDeleteObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Color")
 End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
 Protected Overrides Sub AddBusinessRules()
  ' ColorCodeProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ColorCodeProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(ColorCodeProperty, 50))
  ' ColorDescriptionProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ColorDescriptionProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(ColorDescriptionProperty, 250))
  ' NewColorEffectsProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NewColorEffectsProperty, 1))
  ' NewColorDarknessProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NewColorDarknessProperty, 1))
 End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

 Private Sub New()
  ' require use of factory method 
 End Sub

 Public Shared Function NewColor() As Color
  If Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a Color")
  End If
  Return DataPortal.Create(Of Color)()
 End Function

 Public Shared Function GetColor(ByVal id As Integer) As Color
  If Not CanGetObject() Then
   Throw New System.Security.SecurityException("User not authorized to view a Color")
  End If
  Return DataPortal.Fetch(New SingleCriteria(Of Color, Integer)(id))
 End Function

 Public Shared Sub DeleteColor(ByVal id As Integer)
  If Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a Color")
  End If
  DataPortal.Delete(New SingleCriteria(Of Color, Integer)(id))
 End Sub

 Public Overrides Function Save() As Color
  If IsDeleted AndAlso Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a Color")
  ElseIf IsNew AndAlso Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a Color")
  ElseIf Not CanEditObject() Then
   Throw New System.Security.SecurityException("User not authorized to update a Color")
  End If
  Return MyBase.Save()
 End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

 Friend Shared Function NewColorChild() As Color
  Return DataPortal.CreateChild(Of Color)()
 End Function

 Friend Shared Function GetColor(ByVal dr As SafeDataReader) As Color
  Return DataPortal.FetchChild(Of Color)(dr)
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

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of Color, Integer))
  Database.LogInfo("Color.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = spGetByID
     cm.Parameters.AddWithValue("@Id", criteria.Value)
     Using dr As New SafeDataReader(cm.ExecuteReader)
      dr.Read()

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(ColorCodeProperty, dr.GetString("ColorCode"))
      LoadProperty(Of String)(ColorDescriptionProperty, dr.GetString("ColorDescription"))
      LoadProperty(Of String)(NewColorEffectsProperty, dr.GetString("NewColorEffects"))
      LoadProperty(Of Integer)(NewColorCodeProperty, dr.GetInt32("NewColorCode"))
      LoadProperty(Of String)(NewColorDarknessProperty, dr.GetString("NewColorDarkness"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
     End Using
    End Using
   End Using

  Catch ex As Exception
   Database.LogException("Color.DataPortal_Fetch", ex)
   Throw New DbCslaException("Color.DataPortal_Fetch", ex)
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

      .Parameters.AddWithValue("@ColorCode", ReadProperty(Of String)(ColorCodeProperty))
      .Parameters.AddWithValue("@ColorDescription", ReadProperty(Of String)(ColorDescriptionProperty))
      .Parameters.AddWithValue("@NewColorEffects", ReadProperty(Of String)(NewColorEffectsProperty))
      .Parameters.AddWithValue("@NewColorCode", ReadProperty(Of Integer)(NewColorCodeProperty))
      .Parameters.AddWithValue("@NewColorDarkness", ReadProperty(Of String)(NewColorDarknessProperty))

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
   Database.LogException("Color.DataPortal_Insert", ex)
   Throw New DbCslaException("Color.DataPortal_Insert", ex)
  Finally
   Database.LogInfo("Color.DataPortal_Insert", GetHashCode())
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
      .Parameters.AddWithValue("@ColorCode", ReadProperty(Of String)(ColorCodeProperty))
      .Parameters.AddWithValue("@ColorDescription", ReadProperty(Of String)(ColorDescriptionProperty))
      .Parameters.AddWithValue("@NewColorEffects", ReadProperty(Of String)(NewColorEffectsProperty))
      .Parameters.AddWithValue("@NewColorCode", ReadProperty(Of Integer)(NewColorCodeProperty))
      .Parameters.AddWithValue("@NewColorDarkness", ReadProperty(Of String)(NewColorDarknessProperty))
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
  DataPortal_Delete(New SingleCriteria(Of Color, Integer)(Id))
 End Sub

 Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of Color, Integer))
  Database.LogInfo("Color.DataPortal_Delete", GetHashCode())
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
   Database.LogException("Color.DataPortal_Delete", ex)
   Throw New DbCslaException("Color.DataPortal_Delete", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

 Private Sub Child_Fetch(ByVal dr As SafeDataReader)
  Database.LogInfo("Color.Child_Fetch", GetHashCode())
  Try
   LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
   LoadProperty(Of String)(ColorCodeProperty, dr.GetString("ColorCode"))
   LoadProperty(Of String)(ColorDescriptionProperty, dr.GetString("ColorDescription"))
   LoadProperty(Of String)(NewColorEffectsProperty, dr.GetString("NewColorEffects"))
   LoadProperty(Of Integer)(NewColorCodeProperty, dr.GetInt32("NewColorCode"))
   LoadProperty(Of String)(NewColorDarknessProperty, dr.GetString("NewColorDarkness"))

   dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
  Catch ex As Exception
   Database.LogException("Color.Child_Fetch", ex)
   Throw New DbCslaException("Color.Child_Fetch", ex)
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
      .Parameters.AddWithValue("@ColorCode", ReadProperty(Of String)(ColorCodeProperty))
      .Parameters.AddWithValue("@ColorDescription", ReadProperty(Of String)(ColorDescriptionProperty))
      .Parameters.AddWithValue("@NewColorEffects", ReadProperty(Of String)(NewColorEffectsProperty))
      .Parameters.AddWithValue("@NewColorCode", ReadProperty(Of Integer)(NewColorCodeProperty))
      .Parameters.AddWithValue("@NewColorDarkness", ReadProperty(Of String)(NewColorDarknessProperty))

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
   Database.LogException("Color.Child_Insert", ex)
   Throw New DbCslaException("Color.Child_Insert", ex)
  Finally
   Database.LogInfo("Color.Child_Insert", GetHashCode)
  End Try

 End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

 Private Sub Child_Update()
  Database.LogInfo("Color.Child_Update", GetHashCode)
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    ApplicationContext.LocalContext.Remove("cn")
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spUpdate

      .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
      .Parameters.AddWithValue("@ColorCode", ReadProperty(Of String)(ColorCodeProperty))
      .Parameters.AddWithValue("@ColorDescription", ReadProperty(Of String)(ColorDescriptionProperty))
      .Parameters.AddWithValue("@NewColorEffects", ReadProperty(Of String)(NewColorEffectsProperty))
      .Parameters.AddWithValue("@NewColorCode", ReadProperty(Of Integer)(NewColorCodeProperty))
      .Parameters.AddWithValue("@NewColorDarkness", ReadProperty(Of String)(NewColorDarknessProperty))
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
   Database.LogException("Color.Child_Update", ex)
   If Not ex.Message.EndsWith("drug korisnik") Then
    Throw New DbCslaException("Color.Child_Update", ex)
   End If
  End Try
 End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

 Private Sub Child_DeleteSelf()

  Database.LogInfo("Color.Child_DeleteSelf", GetHashCode)
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
   Database.LogException("Color.Child_Fetch", ex)
   Throw New DbCslaException("Color.Child_Fetch", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
