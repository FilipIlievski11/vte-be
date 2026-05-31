
<Serializable()> _
Public Class BusinessType
  Inherits Csla.BusinessBase(Of BusinessType)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetBusinessTypeByID"
  Private Const spGetAll As String = "GetBusinessTypes"
  Private Const spUpdate As String = "updateBusinessType"
  Private Const spAdd As String = "addBusinessType"
  Private Const spDelete As String = "deleteBusinessType"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(BusinessType), New PropertyInfo(Of Integer)("Id"))
  Private Shared BusinessTypeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(BusinessType), New PropertyInfo(Of String)("BusinessType"))
  Private Shared BusinessTypeDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(BusinessType), New PropertyInfo(Of String)("BusinessTypeDescription"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property BusinessType() As String
    Get
      Return GetProperty(Of String)(BusinessTypeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(BusinessTypeProperty, value)
    End Set
  End Property
  Public Property BusinessTypeDescription() As String
    Get
      Return GetProperty(Of String)(BusinessTypeDescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(BusinessTypeDescriptionProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' BusinessTypeProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, BusinessTypeProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BusinessTypeProperty, 5))
    ' BusinessTypeDescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, BusinessTypeDescriptionProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BusinessTypeDescriptionProperty, 250))
  End Sub
#End Region ' Validation Rules

#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("BusinessType") Then
      AuthorizationRules.AllowWrite("BusinessType", roleName)
    Else
      AuthorizationRules.DenyWrite("BusinessType", roleName)
    End If
    'AuthorizationRules.AllowWrite("BusinessType")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("BusinessTypeDescription") Then
      AuthorizationRules.AllowWrite("BusinessTypeDescription", roleName)
    Else
      AuthorizationRules.DenyWrite("BusinessTypeDescription", roleName)
    End If
    'AuthorizationRules.AllowWrite("BusinessTypeDescription")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub
    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User,  _
               VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("BusinessTypes")
    End Function

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User,  _
               VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("BusinessTypes")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User,  _
                VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("BusinessTypes")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User,  _
                VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("BusinessTypes")
    End Function


#End Region ' Authorization Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

    Public Shared Function NewBusinessType() As BusinessType
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a BusinessType")
        End If
        Return DataPortal.Create(Of BusinessType)()
    End Function

    Public Shared Function GetBusinessType(ByVal id As Integer) As BusinessType
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a BusinessType")
        End If
        Return DataPortal.Fetch(New SingleCriteria(Of BusinessType, Integer)(id))
    End Function

    Public Shared Sub DeleteBusinessType(ByVal id As Integer)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a BusinessType")
        End If
        DataPortal.Delete(New SingleCriteria(Of BusinessType, Integer)(id))
    End Sub

#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewBusinessTypeChild() As BusinessType
    Return DataPortal.CreateChild(Of BusinessType)()
  End Function

  Friend Shared Function GetBusinessType(ByVal dr As SafeDataReader) As BusinessType
    Return DataPortal.FetchChild(Of BusinessType)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of BusinessType, Integer))
    Database.LogInfo("BusinessType.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(BusinessTypeProperty, dr.GetString("BusinessTypeCode"))
            LoadProperty(Of String)(BusinessTypeDescriptionProperty, dr.GetString("BusinessTypeDescription"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("BusinessType.DataPortal_Fetch", ex)
      Throw New DbCslaException("BusinessType.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@BusinessTypeCode", ReadProperty(Of String)(BusinessTypeProperty))
            .Parameters.AddWithValue("@BusinessTypeDescription", ReadProperty(Of String)(BusinessTypeDescriptionProperty))

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
      Database.LogException("BusinessType.DataPortal_Insert", ex)
      Throw New DbCslaException("BusinessType.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("BusinessType.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@BusinessTypeCode", ReadProperty(Of String)(BusinessTypeProperty))
            .Parameters.AddWithValue("@BusinessTypeDescription", ReadProperty(Of String)(BusinessTypeDescriptionProperty))
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
    DataPortal_Delete(New SingleCriteria(Of BusinessType, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of BusinessType, Integer))
    Database.LogInfo("BusinessType.DataPortal_Delete", GetHashCode())
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
      Database.LogException("BusinessType.DataPortal_Delete", ex)
      Throw New DbCslaException("BusinessType.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("BusinessType.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(BusinessTypeProperty, dr.GetString("BusinessTypeCode"))
      LoadProperty(Of String)(BusinessTypeDescriptionProperty, dr.GetString("BusinessTypeDescription"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("BusinessType.Child_Fetch", ex)
      Throw New DbCslaException("BusinessType.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@BusinessTypeCode", ReadProperty(Of String)(BusinessTypeProperty))
            .Parameters.AddWithValue("@BusinessTypeDescription", ReadProperty(Of String)(BusinessTypeDescriptionProperty))

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
      Database.LogException("BusinessType.Child_Insert", ex)
      Throw New DbCslaException("BusinessType.Child_Insert", ex)
    Finally
      Database.LogInfo("BusinessType.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("BusinessType.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@BusinessTypeCode", ReadProperty(Of String)(BusinessTypeProperty))
            .Parameters.AddWithValue("@BusinessTypeDescription", ReadProperty(Of String)(BusinessTypeDescriptionProperty))
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
      Database.LogException("BusinessType.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("BusinessType.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("BusinessType.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("BusinessType.Child_Fetch", ex)
      Throw New DbCslaException("BusinessType.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

#Region " Exists "

  Public Shared Function Exists(ByVal Naziv As String) As Integer

    Dim result As ExistsCommand
    result = DataPortal.Execute(Of ExistsCommand)(New ExistsCommand(Naziv))
    Return result.Exists

  End Function

  <Serializable()> _
  Private Class ExistsCommand
    Inherits CommandBase

    Private _naziv As String
    Private _Exists As Integer

    Public ReadOnly Property Exists() As Integer
      Get
        Return _Exists
      End Get
    End Property

    Public Sub New(ByVal Naziv As String)
      _naziv = Naziv
    End Sub

    Protected Overrides Sub DataPortal_Execute()
      Dim count As Integer = 0

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.Text
          cm.CommandText = "SELECT Id FROM [BusinessTypes] WHERE BusinessTypeDescription=@naziv"
          cm.Parameters.AddWithValue("@naziv", _naziv)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            If dr.Read() Then
              count = dr.GetInt32("Id")
            End If
          End Using
          _Exists = count
        End Using
      End Using

    End Sub

  End Class

#End Region

End Class
