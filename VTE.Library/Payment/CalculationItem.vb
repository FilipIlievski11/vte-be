
<Serializable()> _
Public Class CalculationItem
  Inherits Csla.BusinessBase(Of CalculationItem)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCalculationItemByID"
  Private Const spGetAll As String = "GetCalculationItems"
  Private Const spUpdate As String = "updateCalculationItem"
  Private Const spAdd As String = "addCalculationItem"
  Private Const spDelete As String = "deleteCalculationItem"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(CalculationItem), New PropertyInfo(Of Integer)("Id"))
  Private Shared ItemNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CalculationItem), New PropertyInfo(Of String)("ItemName"))
  Private Shared BankAccountProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CalculationItem), New PropertyInfo(Of String)("BankAccount"))
  Private Shared BankProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CalculationItem), New PropertyInfo(Of String)("Bank"))
  Private Shared FormProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(CalculationItem), New PropertyInfo(Of String)("Form"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property ItemName() As String
    Get
      Return GetProperty(Of String)(ItemNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(ItemNameProperty, value)
    End Set
  End Property
  Public Property BankAccount() As String
    Get
      Return GetProperty(Of String)(BankAccountProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(BankAccountProperty, value)
    End Set
  End Property
  Public Property Bank() As String
    Get
      Return GetProperty(Of String)(BankProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(BankProperty, value)
    End Set
  End Property
  Public Property Form() As String
    Get
      Return GetProperty(Of String)(FormProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(FormProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ItemName") Then
      AuthorizationRules.AllowWrite("ItemName", roleName)
    Else
      AuthorizationRules.DenyWrite("ItemName", roleName)
    End If
    'AuthorizationRules.AllowWrite("ItemName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("BankAccount") Then
      AuthorizationRules.AllowWrite("BankAccount", roleName)
    Else
      AuthorizationRules.DenyWrite("BankAccount", roleName)
    End If
    'AuthorizationRules.AllowWrite("BankAccount")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Bank") Then
      AuthorizationRules.AllowWrite("Bank", roleName)
    Else
      AuthorizationRules.DenyWrite("Bank", roleName)
    End If
    'AuthorizationRules.AllowWrite("Bank")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Form") Then
      AuthorizationRules.AllowWrite("Form", roleName)
    Else
      AuthorizationRules.DenyWrite("Form", roleName)
    End If
    'AuthorizationRules.AllowWrite("Form")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub


  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("CalculationItems")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("CalculationItems")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("CalculationItems")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("CalculationItems")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' ItemNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ItemNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(ItemNameProperty, 150))
    ' BankAccountProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, BankAccountProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BankAccountProperty, 50))
    ' BankProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, BankProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BankProperty, 150))
    ' FormProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, FormProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(FormProperty, 50))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewCalculationItem() As CalculationItem
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a CalculationItem")
    End If
    Return DataPortal.Create(Of CalculationItem)()
  End Function

  Public Shared Function GetCalculationItem(ByVal id As Integer) As CalculationItem
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a CalculationItem")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of CalculationItem, Integer)(Id))
  End Function

  Public Shared Sub DeleteCalculationItem(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a CalculationItem")
    End If
    DataPortal.Delete(New SingleCriteria(Of CalculationItem, Integer)(Id))
  End Sub

  Public Overrides Function Save() As CalculationItem
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a CalculationItem")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a CalculationItem")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a CalculationItem")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewCalculationItemChild() As CalculationItem
    Return DataPortal.CreateChild(Of CalculationItem)()
  End Function

  Friend Shared Function GetCalculationItem(ByVal dr As SafeDataReader) As CalculationItem
    Return DataPortal.FetchChild(Of CalculationItem)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of CalculationItem, Integer))
    Database.LogInfo("CalculationItem.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(ItemNameProperty, dr.GetString("ItemName"))
            LoadProperty(Of String)(BankAccountProperty, dr.GetString("BankAccount"))
            LoadProperty(Of String)(BankProperty, dr.GetString("Bank"))
            LoadProperty(Of String)(FormProperty, dr.GetString("Form"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("CalculationItem.DataPortal_Fetch", ex)
      Throw New DbCslaException("CalculationItem.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@ItemName", ReadProperty(Of String)(ItemNameProperty))
            .Parameters.AddWithValue("@BankAccount", ReadProperty(Of String)(BankAccountProperty))
            .Parameters.AddWithValue("@Bank", ReadProperty(Of String)(BankProperty))
            .Parameters.AddWithValue("@Form", ReadProperty(Of String)(FormProperty))

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
      Database.LogException("CalculationItem.DataPortal_Insert", ex)
      Throw New DbCslaException("CalculationItem.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("CalculationItem.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@ItemName", ReadProperty(Of String)(ItemNameProperty))
            .Parameters.AddWithValue("@BankAccount", ReadProperty(Of String)(BankAccountProperty))
            .Parameters.AddWithValue("@Bank", ReadProperty(Of String)(BankProperty))
            .Parameters.AddWithValue("@Form", ReadProperty(Of String)(FormProperty))
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
    DataPortal_Delete(New SingleCriteria(Of CalculationItem, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of CalculationItem, Integer))
    Database.LogInfo("CalculationItem.DataPortal_Delete", GetHashCode())
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
      Database.LogException("CalculationItem.DataPortal_Delete", ex)
      Throw New DbCslaException("CalculationItem.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("CalculationItem.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(ItemNameProperty, dr.GetString("ItemName"))
      LoadProperty(Of String)(BankAccountProperty, dr.GetString("BankAccount"))
      LoadProperty(Of String)(BankProperty, dr.GetString("Bank"))
      LoadProperty(Of String)(FormProperty, dr.GetString("Form"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("CalculationItem.Child_Fetch", ex)
      Throw New DbCslaException("CalculationItem.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@ItemName", ReadProperty(Of String)(ItemNameProperty))
            .Parameters.AddWithValue("@BankAccount", ReadProperty(Of String)(BankAccountProperty))
            .Parameters.AddWithValue("@Bank", ReadProperty(Of String)(BankProperty))
            .Parameters.AddWithValue("@Form", ReadProperty(Of String)(FormProperty))

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
      Database.LogException("CalculationItem.Child_Insert", ex)
      Throw New DbCslaException("CalculationItem.Child_Insert", ex)
    Finally
      Database.LogInfo("CalculationItem.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("CalculationItem.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext.Remove("cn")
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@ItemName", ReadProperty(Of String)(ItemNameProperty))
            .Parameters.AddWithValue("@BankAccount", ReadProperty(Of String)(BankAccountProperty))
            .Parameters.AddWithValue("@Bank", ReadProperty(Of String)(BankProperty))
            .Parameters.AddWithValue("@Form", ReadProperty(Of String)(FormProperty))
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
      Database.LogException("CalculationItem.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("CalculationItem.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("CalculationItem.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("CalculationItem.Child_Fetch", ex)
      Throw New DbCslaException("CalculationItem.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
