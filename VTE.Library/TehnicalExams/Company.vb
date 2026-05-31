

<Serializable()> _
Public Class Company
 Inherits Csla.BusinessBase(Of Company)


#Region " Stored Procedures Names "
 Private Const spGetByID As String = "getCompanieById"
 Private Const spGetAll As String = "getCompanies"
 Private Const spUpdate As String = "updateCompanie"
 Private Const spAdd As String = "addCompanie"
 Private Const spDelete As String = "deleteCompanie"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
 'register properties
 Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Company), New PropertyInfo(Of Integer)("Id"))
 Private Shared CompanyNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Company), New PropertyInfo(Of String)("CompanyName"))

 Private _lastChanged(7) As Byte

 <System.ComponentModel.DataObjectField(True, True)> _
 Public ReadOnly Property Id() As Integer
  Get
   Return GetProperty(Of Integer)(IdProperty)
  End Get
 End Property

 Public Property CompanyName() As String
  Get
   Return GetProperty(Of String)(CompanyNameProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(CompanyNameProperty, value)
  End Set
 End Property

 Public Overrides Function ToString() As String
  Return Id.ToString
 End Function
#End Region 'Business Properties and Methods

 '#Region " Authorization Rules "

 ' Protected Overrides Sub AddAuthorizationRules()
 '  Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
 '  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CommunityCode") Then
 '   AuthorizationRules.AllowWrite("IdCommunityCode", roleName)
 '  Else
 '   AuthorizationRules.DenyWrite("IdCommunityCode", roleName)
 '  End If

 ' End Sub


 ' Public Shared Function CanGetObject() As Boolean
 '  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Cities")
 ' End Function

 ' Public Shared Function CanAddObject() As Boolean
 '  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Cities")
 ' End Function

 ' Public Shared Function CanEditObject() As Boolean
 '  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Cities")
 ' End Function

 ' Public Shared Function CanDeleteObject() As Boolean
 '  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Cities")
 ' End Function

 '#End Region ' Authorization Rules

#Region " Validation Rules "
 Protected Overrides Sub AddBusinessRules()
  ' CompanyNameProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CompanyNameProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CompanyNameProperty, 255))

 End Sub



#End Region ' Validation Rules

#Region " Factory Methods "

 Private Sub New()
  ' require use of factory method 
 End Sub

 Public Shared Function NewCompany() As Company
  'If Not CanAddObject() Then
  ' Throw New System.Security.SecurityException("User not authorized to add a Company")
  'End If
  Return DataPortal.Create(Of Company)()
 End Function

 Public Shared Function GetCompany(ByVal id As Integer) As Company
  'If Not CanGetObject() Then
  ' Throw New System.Security.SecurityException("User not authorized to view a Company")
  'End If
  Return DataPortal.Fetch(New SingleCriteria(Of Company, Integer)(id))
 End Function

 Public Shared Sub DeleteCompany(ByVal id As Integer)
  'If Not CanDeleteObject() Then
  ' Throw New System.Security.SecurityException("User not authorized to remove a Company")
  'End If
  DataPortal.Delete(New SingleCriteria(Of Company, Integer)(id))
 End Sub

#End Region ' Factory Methods

#Region " Child Factory Methods "

 Friend Shared Function NewCompanyChild() As Company
  Return DataPortal.CreateChild(Of Company)()
 End Function

 Friend Shared Function GetCompany(ByVal dr As SafeDataReader) As Company
  Return DataPortal.FetchChild(Of Company)(dr)
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

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of Company, Integer))
  Database.LogInfo("Company.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = spGetByID
     cm.Parameters.AddWithValue("@Id", criteria.Value)
     Using dr As New SafeDataReader(cm.ExecuteReader)
      dr.Read()

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(CompanyNameProperty, dr.GetString("CompanyName"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
     End Using
    End Using
   End Using

  Catch ex As Exception
   Database.LogException("Company.DataPortal_Fetch", ex)
   Throw New DbCslaException("Company.DataPortal_Fetch", ex)
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

      .Parameters.AddWithValue("@CompanyName", ReadProperty(Of String)(CompanyNameProperty))

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
   Database.LogException("Company.DataPortal_Insert", ex)
   Throw New DbCslaException("Company.DataPortal_Insert", ex)
  Finally
   Database.LogInfo("Company.DataPortal_Insert", GetHashCode())
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

      .Parameters.AddWithValue("@CompanyName", ReadProperty(Of String)(CompanyNameProperty))
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
   Database.LogException("Company.DataPortal_Update", ex)
   If Not ex.Message.EndsWith("drug korisnik") Then
    Throw New DBConcurrencyException("Company.DataPortal_Update", ex)
   End If
  End Try
 End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

 Protected Overrides Sub DataPortal_DeleteSelf()
  DataPortal_Delete(New SingleCriteria(Of Company, Integer)(Id))
 End Sub

 Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of Company, Integer))
  Database.LogInfo("Company.DataPortal_Delete", GetHashCode())
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
   Database.LogException("Company.DataPortal_Delete", ex)
   Throw New DbCslaException("Company.DataPortal_Delete", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

 Private Sub Child_Fetch(ByVal dr As SafeDataReader)
  Database.LogInfo("Company.Child_Fetch", GetHashCode())
  Try
   LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
   LoadProperty(Of String)(CompanyNameProperty, dr.GetString("CompanyName"))

   dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
  Catch ex As Exception
   Database.LogException("Company.Child_Fetch", ex)
   Throw New DbCslaException("Company.Child_Fetch", ex)
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

      .Parameters.AddWithValue("@CompanyName", ReadProperty(Of String)(CompanyNameProperty))
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
   Database.LogException("Company.Child_Insert", ex)
   Throw New DbCslaException("Company.Child_Insert", ex)
  Finally
   Database.LogInfo("Company.Child_Insert", GetHashCode)
  End Try

 End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

 Private Sub Child_Update()
  Database.LogInfo("Company.Child_Update", GetHashCode)
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spUpdate

      .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
      .Parameters.AddWithValue("@CompanyName", ReadProperty(Of String)(CompanyNameProperty))
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
   Database.LogException("Company.Child_Update", ex)
   If Not ex.Message.EndsWith("drug korisnik") Then
    Throw New DbCslaException("Company.Child_Update", ex)
   End If
  End Try
 End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

 Private Sub Child_DeleteSelf()

  Database.LogInfo("Company.Child_DeleteSelf", GetHashCode)
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
   Database.LogException("Company.Child_Fetch", ex)
   Throw New DbCslaException("Company.Child_Fetch", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

 '#Region " Exists "

 ' Public Shared Function Exists(ByVal Naziv As String) As Integer

 '  Dim result As ExistsCommand
 '  result = DataPortal.Execute(Of ExistsCommand)(New ExistsCommand(Naziv))
 '  Return result.Exists

 ' End Function

 ' <Serializable()> _
 ' Private Class ExistsCommand
 '  Inherits CommandBase

 '  Private _naziv As String
 '  Private _Exists As Integer

 '  Public ReadOnly Property Exists() As Integer
 '   Get
 '    Return _Exists
 '   End Get
 '  End Property

 '  Public Sub New(ByVal Naziv As String)
 '   _naziv = Naziv
 '  End Sub

 '  Protected Overrides Sub DataPortal_Execute()
 '   Dim count As Integer = 0

 '   Using cn As SqlConnection = Database.VTE_SqlConnection
 '    Using cm As SqlCommand = cn.CreateCommand
 '     cm.CommandType = CommandType.Text
 '     cm.CommandText = "SELECT Id FROM [Cities] WHERE CompanyName=@naziv"
 '     cm.Parameters.AddWithValue("@naziv", _naziv)
 '     Using dr As New SafeDataReader(cm.ExecuteReader)
 '      If dr.Read() Then
 '       count = dr.GetInt32("Id")
 '      End If
 '     End Using
 '     _Exists = count
 '    End Using
 '   End Using

 '  End Sub

 ' End Class

 '#End Region

End Class

