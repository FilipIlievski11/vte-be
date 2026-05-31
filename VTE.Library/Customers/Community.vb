
<Serializable()> _
Public Class Community
  Inherits Csla.BusinessBase(Of Community)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCommunitieByID"
  Private Const spGetAll As String = "GetCommunities"
  Private Const spUpdate As String = "updateCommunitie"
  Private Const spAdd As String = "addCommunitie"
  Private Const spDelete As String = "deleteCommunitie"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Community), New PropertyInfo(Of Integer)("Id"))
  Private Shared CommunityCodeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Community), New PropertyInfo(Of String)("CommunityCode"))
  Private Shared CommunityNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Community), New PropertyInfo(Of String)("CommunityName"))
  Private Shared RegistrationCodeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Community), New PropertyInfo(Of String)("RegistrationCode"))
  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property CommunityCode() As String
    Get
      Return GetProperty(Of String)(CommunityCodeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CommunityCodeProperty, value)
    End Set
  End Property
  Public Property CommunityName() As String
    Get
      Return GetProperty(Of String)(CommunityNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CommunityNameProperty, value)
    End Set
  End Property
  Public Property RegistrationCode() As String
    Get
      Return GetProperty(Of String)(RegistrationCodeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(RegistrationCodeProperty, value)
    End Set
  End Property
  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CommunityCode") Then
      AuthorizationRules.AllowWrite("CommunityCode", roleName)
    Else
      AuthorizationRules.DenyWrite("CommunityCode", roleName)
    End If
    'AuthorizationRules.AllowWrite("CommunityCode")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CommunityName") Then
      AuthorizationRules.AllowWrite("CommunityName", roleName)
    Else
      AuthorizationRules.DenyWrite("CommunityName", roleName)
    End If
    'AuthorizationRules.AllowWrite("CommunityName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Community")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Community")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Community")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Community")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' CommunityCodeProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CommunityCodeProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CommunityCodeProperty, 50))
    ' CommunityNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CommunityNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CommunityNameProperty, 50))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewCommunity() As Community
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a Community")
    End If
    Return DataPortal.Create(Of Community)()
  End Function

  Public Shared Function GetCommunity(ByVal id As Integer) As Community
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a Community")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of Community, Integer)(Id))
  End Function

  Public Shared Sub DeleteCommunity(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a Community")
    End If
    DataPortal.Delete(New SingleCriteria(Of Community, Integer)(Id))
  End Sub

  Public Overrides Function Save() As Community
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a Community")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a Community")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a Community")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewCommunityChild() As Community
    Return DataPortal.CreateChild(Of Community)()
  End Function

  Friend Shared Function GetCommunity(ByVal dr As SafeDataReader) As Community
    Return DataPortal.FetchChild(Of Community)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of Community, Integer))
    Database.LogInfo("Community.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(CommunityCodeProperty, dr.GetString("CommunityCode"))
            LoadProperty(Of String)(CommunityNameProperty, dr.GetString("CommunityName"))
            LoadProperty(Of String)(RegistrationCodeProperty, dr.GetString("RegistrationCode"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("Community.DataPortal_Fetch", ex)
      Throw New DbCslaException("Community.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@CommunityCode", ReadProperty(Of String)(CommunityCodeProperty))
            .Parameters.AddWithValue("@CommunityName", ReadProperty(Of String)(CommunityNameProperty))
            .Parameters.AddWithValue("@RegistrationCode", ReadProperty(Of String)(RegistrationCodeProperty))

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
      Database.LogException("Community.DataPortal_Insert", ex)
      Throw New DbCslaException("Community.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("Community.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@CommunityCode", ReadProperty(Of String)(CommunityCodeProperty))
            .Parameters.AddWithValue("@CommunityName", ReadProperty(Of String)(CommunityNameProperty))
            .Parameters.AddWithValue("@RegistrationCode", ReadProperty(Of String)(RegistrationCodeProperty))
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
    DataPortal_Delete(New SingleCriteria(Of Community, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of Community, Integer))
    Database.LogInfo("Community.DataPortal_Delete", GetHashCode())
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
      Database.LogException("Community.DataPortal_Delete", ex)
      Throw New DbCslaException("Community.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("Community.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(CommunityCodeProperty, dr.GetString("CommunityCode"))
      LoadProperty(Of String)(CommunityNameProperty, dr.GetString("CommunityName"))
      LoadProperty(Of String)(RegistrationCodeProperty, dr.GetString("RegistrationCode"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("Community.Child_Fetch", ex)
      Throw New DbCslaException("Community.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@CommunityCode", ReadProperty(Of String)(CommunityCodeProperty))
            .Parameters.AddWithValue("@CommunityName", ReadProperty(Of String)(CommunityNameProperty))
            .Parameters.AddWithValue("@RegistrationCode", ReadProperty(Of String)(RegistrationCodeProperty))

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
      Database.LogException("Community.Child_Insert", ex)
      Throw New DbCslaException("Community.Child_Insert", ex)
    Finally
      Database.LogInfo("Community.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("Community.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@CommunityCode", ReadProperty(Of String)(CommunityCodeProperty))
            .Parameters.AddWithValue("@CommunityName", ReadProperty(Of String)(CommunityNameProperty))
            .Parameters.AddWithValue("@RegistrationCode", ReadProperty(Of String)(RegistrationCodeProperty))
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
      Database.LogException("Community.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("Community.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("Community.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("Community.Child_Fetch", ex)
      Throw New DbCslaException("Community.Child_Fetch", ex)
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
          cm.CommandText = "SELECT Id FROM [Communities] WHERE CommunityName=@naziv"
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

#Region "Drzavjanstvo"
  Public Shared Function Drzavjanstvo(ByVal IdCommunity As Integer) As Integer

    Dim result As ExistsCountry
    result = DataPortal.Execute(Of ExistsCountry)(New ExistsCountry(IdCommunity))
    Return result.IdCountry

  End Function

  <Serializable()> _
  Private Class ExistsCountry
    Inherits CommandBase

    Private _IdCommunity As Integer
    Private _IdCountry As Integer

    Public ReadOnly Property IdCountry() As Integer
      Get
        Return _IdCountry
      End Get
    End Property

    Public Sub New(ByVal IdCommunity As Integer)
      _IdCommunity = IdCommunity
    End Sub

    Protected Overrides Sub DataPortal_Execute()
      Dim count As Integer = 0

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.Text
          cm.CommandText = _
          "SELECT top (1) dbo.Countries.id FROM dbo.Cities INNER JOIN dbo.Countries ON dbo.Cities.IdCountry = dbo.Countries.Id INNER JOIN dbo.Communities ON dbo.Cities.IdCommunityCode = dbo.Communities.Id where dbo.Communities.Id = @IdCommunity"
          cm.Parameters.AddWithValue("@IdCommunity", _IdCommunity)
          _IdCountry = cm.ExecuteScalar
          'Using dr As New SafeDataReader(cm.ExecuteReader)
          '  If dr.Read() Then
          '    count = dr.GetInt32("Id")
          '  End If
          'End Using
          '_Exists = count
        End Using
      End Using

    End Sub

  End Class

#End Region
End Class
