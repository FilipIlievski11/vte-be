
<Serializable()> _
Public Class RegistrationIssuer
  Inherits Csla.BusinessBase(Of RegistrationIssuer)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetRegistrationIssuerByID"
  Private Const spGetAll As String = "GetRegistrationIssuers"
  Private Const spUpdate As String = "updateRegistrationIssuer"
  Private Const spAdd As String = "addRegistrationIssuer"
  Private Const spDelete As String = "deleteRegistrationIssuer"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(RegistrationIssuer), New PropertyInfo(Of Integer)("Id"))
  Private Shared IssuerNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(RegistrationIssuer), New PropertyInfo(Of String)("IssuerName"))
    Private Shared IdCommunityProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(RegistrationIssuer), New PropertyInfo(Of Integer)("IdCommunity"))
  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IssuerName() As String
    Get
      Return GetProperty(Of String)(IssuerNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(IssuerNameProperty, value)
    End Set
    End Property
    Public Property IdCommunity() As Integer
        Get
            Return GetProperty(Of Integer)(IdCommunityProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdCommunityProperty, value)
        End Set
    End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "


  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IssuerName") Then
      AuthorizationRules.AllowWrite("IssuerName", roleName)
    Else
      AuthorizationRules.DenyWrite("IssuerName", roleName)
    End If
    'AuthorizationRules.AllowWrite("IssuerName")
  End Sub
  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("RegistrationIssuers")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("RegistrationIssuers")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("RegistrationIssuers")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("RegistrationIssuers")
  End Function
#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' IssuerNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, IssuerNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(IssuerNameProperty, 50))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewRegistrationIssuer() As RegistrationIssuer
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a RegistrationIssuer")
    End If
    Return DataPortal.Create(Of RegistrationIssuer)()
  End Function

  Public Shared Function GetRegistrationIssuer(ByVal id As Integer) As RegistrationIssuer
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a RegistrationIssuer")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of RegistrationIssuer, Integer)(Id))
  End Function

  Public Shared Sub DeleteRegistrationIssuer(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a RegistrationIssuer")
    End If
    DataPortal.Delete(New SingleCriteria(Of RegistrationIssuer, Integer)(Id))
  End Sub

  Public Overrides Function Save() As RegistrationIssuer
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a RegistrationIssuer")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a RegistrationIssuer")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a RegistrationIssuer")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewRegistrationIssuerChild() As RegistrationIssuer
    Return DataPortal.CreateChild(Of RegistrationIssuer)()
  End Function

  Friend Shared Function GetRegistrationIssuer(ByVal dr As SafeDataReader) As RegistrationIssuer
    Return DataPortal.FetchChild(Of RegistrationIssuer)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of RegistrationIssuer, Integer))
    Database.LogInfo("RegistrationIssuer.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(IssuerNameProperty, dr.GetString("IssuerName"))
                        LoadProperty(Of Integer)(IdCommunityProperty, dr.GetInt32("IdCommunity"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("RegistrationIssuer.DataPortal_Fetch", ex)
      Throw New DbCslaException("RegistrationIssuer.DataPortal_Fetch", ex)
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

                        .Parameters.AddWithValue("@IssuerName", ReadProperty(Of String)(IssuerNameProperty))
                        .Parameters.AddWithValue("@IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty))

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
      Database.LogException("RegistrationIssuer.DataPortal_Insert", ex)
      Throw New DbCslaException("RegistrationIssuer.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("RegistrationIssuer.DataPortal_Insert", GetHashCode())
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
                        .Parameters.AddWithValue("@IssuerName", ReadProperty(Of String)(IssuerNameProperty))
                        .Parameters.AddWithValue("@IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty))
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
    DataPortal_Delete(New SingleCriteria(Of RegistrationIssuer, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of RegistrationIssuer, Integer))
    Database.LogInfo("RegistrationIssuer.DataPortal_Delete", GetHashCode())
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
      Database.LogException("RegistrationIssuer.DataPortal_Delete", ex)
      Throw New DbCslaException("RegistrationIssuer.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("RegistrationIssuer.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(IssuerNameProperty, dr.GetString("IssuerName"))
            LoadProperty(Of Integer)(IdCommunityProperty, dr.GetInt32("IdCommunity"))
      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("RegistrationIssuer.Child_Fetch", ex)
      Throw New DbCslaException("RegistrationIssuer.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@IssuerName", ReadProperty(Of String)(IssuerNameProperty))
                        .Parameters.AddWithValue("@IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty))
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
      Database.LogException("RegistrationIssuer.Child_Insert", ex)
      Throw New DbCslaException("RegistrationIssuer.Child_Insert", ex)
    Finally
      Database.LogInfo("RegistrationIssuer.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("RegistrationIssuer.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext.Remove("cn")
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
                        .Parameters.AddWithValue("@IssuerName", ReadProperty(Of String)(IssuerNameProperty))
                        .Parameters.AddWithValue("@IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty))
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
      Database.LogException("RegistrationIssuer.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("RegistrationIssuer.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("RegistrationIssuer.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("RegistrationIssuer.Child_Fetch", ex)
      Throw New DbCslaException("RegistrationIssuer.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
