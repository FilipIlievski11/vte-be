
<Serializable()> _
Public Class Country
  Inherits Csla.BusinessBase(Of Country)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCountrieByID"
  Private Const spGetAll As String = "GetCountries"
  Private Const spUpdate As String = "updateCountrie"
  Private Const spAdd As String = "addCountrie"
  Private Const spDelete As String = "deleteCountrie"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Country), New PropertyInfo(Of Integer)("Id"))
  Private Shared CountryNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Country), New PropertyInfo(Of String)("CountryName"))
  Private Shared CountryShortNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Country), New PropertyInfo(Of String)("CountryShortName"))
  Private Shared CitizenshipProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Country), New PropertyInfo(Of String)("Citizenship"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property CountryName() As String
    Get
      Return GetProperty(Of String)(CountryNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CountryNameProperty, value)
    End Set
  End Property
  Public Property CountryShortName() As String
    Get
      Return GetProperty(Of String)(CountryShortNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CountryShortNameProperty, value)
    End Set
  End Property
  Public Property Citizenship() As String
    Get
      Return GetProperty(Of String)(CitizenshipProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CitizenshipProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' CountryNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CountryNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CountryNameProperty, 150))
        'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CountryShortNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CountryShortNameProperty, 3))
        'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMinLength, New Csla.Validation.CommonRules.MinLengthRuleArgs(CountryShortNameProperty, 2))
    ValidationRules.AddRule(Of Country)(AddressOf NoDuplicatesCountryShortName, CountryShortNameProperty)
    ValidationRules.AddRule(Of Country)(AddressOf NoDuplicatesCountryName, CountryNameProperty)

  End Sub

  Private Shared Function NoDuplicatesCountryShortName(Of T As Country)(ByVal target As T, _
  ByVal e As Csla.Validation.RuleArgs) As Boolean
    If Country.CountryShortNameExists(target.CountryShortName, target.Id) Then
      e.Description = "Кодот на државата мора да биде единствен"
      Return False
    Else
      Return True
    End If
  End Function

  Private Shared Function NoDuplicatesCountryName(Of T As Country)(ByVal target As T, _
ByVal e As Csla.Validation.RuleArgs) As Boolean
    If Country.CountryNameExists(target.CountryName, target.Id) Then
      e.Description = "Името на државата мора да биде единствен"
      Return False
    Else
      Return True
    End If
  End Function

#End Region ' Validation Rules

#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CountryName") Then
      AuthorizationRules.AllowWrite("CountryName", roleName)
    Else
      AuthorizationRules.DenyWrite("CountryName", roleName)
    End If
    'AuthorizationRules.AllowWrite("CountryName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub


    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Countries")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Countries")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Countries")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Countries")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

    Public Shared Function NewCountry() As Country
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a Country")
        End If
        Return DataPortal.Create(Of Country)()
    End Function

  Public Shared Function GetCountry(ByVal id As Integer) As Country
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a Country")
        End If
        Return DataPortal.Fetch(New SingleCriteria(Of Country, Integer)(id))
  End Function

    Public Shared Sub DeleteCountry(ByVal id As Integer)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a Country")
        End If
        DataPortal.Delete(New SingleCriteria(Of Country, Integer)(id))
    End Sub

#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewCountryChild() As Country
    Return DataPortal.CreateChild(Of Country)()
  End Function

  Friend Shared Function GetCountry(ByVal dr As SafeDataReader) As Country
    Return DataPortal.FetchChild(Of Country)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of Country, Integer))
    Database.LogInfo("Country.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(CountryNameProperty, dr.GetString("CountryName"))
            LoadProperty(Of String)(CountryShortNameProperty, dr.GetString("CountryShortName"))
            LoadProperty(Of String)(CitizenshipProperty, dr.GetString("Citizenship"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("Country.DataPortal_Fetch", ex)
      Throw New DbCslaException("Country.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@CountryName", ReadProperty(Of String)(CountryNameProperty))
            .Parameters.AddWithValue("@CountryShortName", ReadProperty(Of String)(CountryShortNameProperty))
            .Parameters.AddWithValue("@Citizenship", ReadProperty(Of String)(CitizenshipProperty))

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
      Database.LogException("Country.DataPortal_Insert", ex)
      Throw New DbCslaException("Country.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("Country.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@CountryName", ReadProperty(Of String)(CountryNameProperty))
            .Parameters.AddWithValue("@CountryShortName", ReadProperty(Of String)(CountryShortNameProperty))
            .Parameters.AddWithValue("@Citizenship", ReadProperty(Of String)(CitizenshipProperty))
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
    DataPortal_Delete(New SingleCriteria(Of Country, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of Country, Integer))
    Database.LogInfo("Country.DataPortal_Delete", GetHashCode())
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
      Database.LogException("Country.DataPortal_Delete", ex)
      Throw New DbCslaException("Country.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("Country.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(CountryNameProperty, dr.GetString("CountryName"))
      LoadProperty(Of String)(CountryShortNameProperty, dr.GetString("CountryShortName"))
      LoadProperty(Of String)(CitizenshipProperty, dr.GetString("Citizenship"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("Country.Child_Fetch", ex)
      Throw New DbCslaException("Country.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@CountryName", ReadProperty(Of String)(CountryNameProperty))
            .Parameters.AddWithValue("@CountryShortName", ReadProperty(Of String)(CountryShortNameProperty))
            .Parameters.AddWithValue("@Citizenship", ReadProperty(Of String)(CitizenshipProperty))
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
      Database.LogException("Country.Child_Insert", ex)
      Throw New DbCslaException("Country.Child_Insert", ex)
    Finally
      Database.LogInfo("Country.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("Country.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@CountryName", ReadProperty(Of String)(CountryNameProperty))
            .Parameters.AddWithValue("@CountryShortName", ReadProperty(Of String)(CountryShortNameProperty))
            .Parameters.AddWithValue("@Citizenship", ReadProperty(Of String)(CitizenshipProperty))
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
      Database.LogException("Country.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("Country.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("Country.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("Country.Child_Fetch", ex)
      Throw New DbCslaException("Country.Child_Fetch", ex)
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
          cm.CommandText = "SELECT Id FROM [Countries] WHERE CountryName=@naziv"
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

#Region " CountyShortName Exists "

  Public Shared Function CountryShortNameExists(ByVal strCountryShortName As String, ByVal idCountry As Long) As Boolean

    Return CountryShortNameExistsCommand.CountryShortNameExists(strCountryShortName, idCountry)

  End Function

  <Serializable()> _
  Private Class CountryShortNameExistsCommand
    Inherits CommandBase
    Private _CountryShortName As String
    Private _idCountry As Long
    Private _existsCountryShortName As Boolean
    Public ReadOnly Property ExistsCountryShortName() As Boolean
      Get
        Return _existsCountryShortName
      End Get
    End Property

    Public Shared Function CountryShortNameExists(ByVal strCountryShortName As String, ByVal idCountry As Long) As Boolean

      Dim result As CountryShortNameExistsCommand
      result = DataPortal.Execute(Of CountryShortNameExistsCommand)(New CountryShortNameExistsCommand(strCountryShortName, idCountry))
      Return result.ExistsCountryShortName

    End Function

    Private Sub New(ByVal strCountryShortName As String, ByVal idCountry As Long)
      _CountryShortName = strCountryShortName
      _idCountry = idCountry
      _existsCountryShortName = False
    End Sub

    Protected Overrides Sub DataPortal_Execute()
      Dim pom As Integer = 0
      Using cn As SqlConnection = Database.VTE_SqlConnection
        'ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "NumOfCustomerCountryShortNameExists"
          cm.Parameters.AddWithValue("@CountryShortName", _CountryShortName)
          cm.Parameters.AddWithValue("@idCountry", _idCountry)
          pom = cm.ExecuteScalar
          If pom = 0 Then
            _existsCountryShortName = False
          Else
            _existsCountryShortName = True
          End If
        End Using
      End Using
    End Sub

  End Class

#End Region

#Region " CountryName Exists "

  Public Shared Function CountryNameExists(ByVal strCountryName As String, ByVal idCountry As Long) As Boolean

    Return CountryNameExistsCommand.CountryNameExists(strCountryName, idCountry)

  End Function

  <Serializable()> _
  Private Class CountryNameExistsCommand
    Inherits CommandBase
    Private _CountryName As String
    Private _idCountry As Long
    Private _existsCountryName As Boolean
    Public ReadOnly Property ExistsCountryName() As Boolean
      Get
        Return _existsCountryName
      End Get
    End Property

    Public Shared Function CountryNameExists(ByVal strCountryName As String, ByVal idCountry As Long) As Boolean

      Dim result As CountryNameExistsCommand
      result = DataPortal.Execute(Of CountryNameExistsCommand)(New CountryNameExistsCommand(strCountryName, idCountry))
      Return result.ExistsCountryName

    End Function

    Private Sub New(ByVal strCountryName As String, ByVal idCountry As Long)
      _CountryName = strCountryName
      _idCountry = idCountry
      _existsCountryName = False
    End Sub

    Protected Overrides Sub DataPortal_Execute()
      Dim pom As Integer = 0
      Using cn As SqlConnection = Database.VTE_SqlConnection
        'ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "NumOfCustomerCountryNameExists"
          cm.Parameters.AddWithValue("@CountryName", _CountryName)
          cm.Parameters.AddWithValue("@idCountry", _idCountry)
          pom = cm.ExecuteScalar
          If pom = 0 Then
            _existsCountryName = False
          Else
            _existsCountryName = True
          End If
        End Using
      End Using
    End Sub

  End Class

#End Region

End Class
