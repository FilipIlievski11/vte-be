
<Serializable()> _
Public Class VehicleMaker
  Inherits Csla.BusinessBase(Of VehicleMaker)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleMakerByID"
  Private Const spGetAll As String = "GetVehicleMakers"
  Private Const spUpdate As String = "updateVehicleMaker"
  Private Const spAdd As String = "addVehicleMaker"
  Private Const spDelete As String = "deleteVehicleMaker"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleMaker), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdCountryProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleMaker), New PropertyInfo(Of Integer)("IdCountry"))
  Private Shared CompanyNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleMaker), New PropertyInfo(Of String)("CompanyName"))
  Private Shared CompanyTrademarkProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleMaker), New PropertyInfo(Of String)("CompanyTrademark"))

  Private _lastChanged(7) As Byte

  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdCountry() As Integer
    Get
      Return GetProperty(Of Integer)(IdCountryProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdCountryProperty, value)
    End Set
  End Property
  Public Property CompanyName() As String
    Get
      Return GetProperty(Of String)(CompanyNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CompanyNameProperty, value)
    End Set
  End Property
  Public Property CompanyTrademark() As String
    Get
      Return GetProperty(Of String)(CompanyTrademarkProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CompanyTrademarkProperty, value)
    End Set
  End Property

  Public ReadOnly Property CompanyNameAndTrademark() As String
    Get
      Return (CompanyName & ": " & CompanyTrademark)
    End Get

  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCountry") Then
      AuthorizationRules.AllowWrite("IdCountry", roleName)
    Else
      AuthorizationRules.DenyWrite("IdCountry", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdCountry")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CompanyName") Then
      AuthorizationRules.AllowWrite("CompanyName", roleName)
    Else
      AuthorizationRules.DenyWrite("CompanyName", roleName)
    End If
    'AuthorizationRules.AllowWrite("CompanyName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CompanyTrademark") Then
      AuthorizationRules.AllowWrite("CompanyTrademark", roleName)
    Else
      AuthorizationRules.DenyWrite("CompanyTrademark", roleName)
    End If
    'AuthorizationRules.AllowWrite("CompanyTrademark")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleMaker")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleMaker")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleMaker")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleMaker")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' CompanyNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CompanyNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CompanyNameProperty, 250))
    ' CompanyTrademarkProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CompanyTrademarkProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CompanyTrademarkProperty, 250))
        ValidationRules.AddRule(Of VehicleMaker)(AddressOf NoDuplicates, CompanyNameProperty)
        ValidationRules.AddRule(Of VehicleMaker)(AddressOf NoDuplicates, IdCountryProperty)
    End Sub

    Private Shared Function NoDuplicates(Of T As VehicleMaker)(ByVal target As T, _
     ByVal e As Csla.Validation.RuleArgs) As Boolean
        If VehicleMaker.Exists(target.CompanyName) > 0 AndAlso VehicleMaker.Exists(target.CompanyName) <> target.Id Then
            Dim maker As VehicleMakerInfo = VehicleMakerList.GetVehicleMakerList.GetVehicleMakerListById _
    (VehicleMaker.Exists(target.CompanyName))
            If target.IdCountry = maker.IdCountry Then
                e.Description = "Марката веќе е внесена"
                Return False
            Else
                Return True
            End If
        Else
            Return True
        End If
    End Function
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewVehicleMaker() As VehicleMaker
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleMaker")
    End If
    Return DataPortal.Create(Of VehicleMaker)()
  End Function

  Public Shared Function GetVehicleMaker(ByVal id As Integer) As VehicleMaker
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a VehicleMaker")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of VehicleMaker, Integer)(Id))
  End Function

  Public Shared Sub DeleteVehicleMaker(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleMaker")
    End If
    DataPortal.Delete(New SingleCriteria(Of VehicleMaker, Integer)(Id))
  End Sub

  Public Overrides Function Save() As VehicleMaker
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleMaker")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleMaker")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a VehicleMaker")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleMakerChild() As VehicleMaker
    Return DataPortal.CreateChild(Of VehicleMaker)()
  End Function

  Friend Shared Function GetVehicleMaker(ByVal dr As SafeDataReader) As VehicleMaker
    Return DataPortal.FetchChild(Of VehicleMaker)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleMaker, Integer))
    Database.LogInfo("VehicleMaker.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of Integer)(IdCountryProperty, dr.GetInt32("IdCountry"))
            LoadProperty(Of String)(CompanyNameProperty, dr.GetString("CompanyName"))
            LoadProperty(Of String)(CompanyTrademarkProperty, dr.GetString("CompanyTrademark"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleMaker.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleMaker.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@IdCountry", ReadProperty(Of Integer)(IdCountryProperty))
            .Parameters.AddWithValue("@CompanyName", ReadProperty(Of String)(CompanyNameProperty))
            .Parameters.AddWithValue("@CompanyTrademark", ReadProperty(Of String)(CompanyTrademarkProperty))

            Dim param As New SqlParameter("@newId", SqlDbType.Int)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)
            param = New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            LoadProperty(Of Integer)(IdProperty, CInt(.Parameters("@newId").Value))
            Console.WriteLine(GetProperty(Of Integer)(IdProperty))
            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("VehicleMaker.DataPortal_Insert", ex)
      Throw New DbCslaException("VehicleMaker.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("VehicleMaker.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@IdCountry", ReadProperty(Of Integer)(IdCountryProperty))
            .Parameters.AddWithValue("@CompanyName", ReadProperty(Of String)(CompanyNameProperty))
            .Parameters.AddWithValue("@CompanyTrademark", ReadProperty(Of String)(CompanyTrademarkProperty))
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
    DataPortal_Delete(New SingleCriteria(Of VehicleMaker, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleMaker, Integer))
    Database.LogInfo("VehicleMaker.DataPortal_Delete", GetHashCode())
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
      Database.LogException("VehicleMaker.DataPortal_Delete", ex)
      Throw New DbCslaException("VehicleMaker.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleMaker.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdCountryProperty, dr.GetInt32("IdCountry"))
      LoadProperty(Of String)(CompanyNameProperty, dr.GetString("CompanyName"))
      LoadProperty(Of String)(CompanyTrademarkProperty, dr.GetString("CompanyTrademark"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("VehicleMaker.Child_Fetch", ex)
      Throw New DbCslaException("VehicleMaker.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@IdCountry", ReadProperty(Of Integer)(IdCountryProperty))
            .Parameters.AddWithValue("@CompanyName", ReadProperty(Of String)(CompanyNameProperty))
            .Parameters.AddWithValue("@CompanyTrademark", ReadProperty(Of String)(CompanyTrademarkProperty))

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
      Database.LogException("VehicleMaker.Child_Insert", ex)
      Throw New DbCslaException("VehicleMaker.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleMaker.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("VehicleMaker.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@IdCountry", ReadProperty(Of Integer)(IdCountryProperty))
            .Parameters.AddWithValue("@CompanyName", ReadProperty(Of String)(CompanyNameProperty))
            .Parameters.AddWithValue("@CompanyTrademark", ReadProperty(Of String)(CompanyTrademarkProperty))
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
      Database.LogException("VehicleMaker.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleMaker.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleMaker.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleMaker.Child_Fetch", ex)
      Throw New DbCslaException("VehicleMaker.Child_Fetch", ex)
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
                    cm.CommandText = "SELECT Id FROM [VehicleMakers] WHERE CompanyName=@naziv and Active = 1"
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
