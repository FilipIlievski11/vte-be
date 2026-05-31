Imports Csla.Validation.CommonRules

<Serializable()> _
Public Class City
    Inherits Csla.BusinessBase(Of City)


#Region " Stored Procedures Names "
    Private Const spGetByID As String = "GetCitieByID"
    Private Const spGetAll As String = "GetCities"
    Private Const spUpdate As String = "updateCitie"
    Private Const spAdd As String = "addCitie"
    Private Const spDelete As String = "deleteCitie"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
    'register properties
    Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(City), New PropertyInfo(Of Integer)("Id"))
    Private Shared IdCommunityCodeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(City), New PropertyInfo(Of Integer)("IdCommunityCode"))
    Private Shared CityNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(City), New PropertyInfo(Of String)("CityName"))
    Private Shared CityZipProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(City), New PropertyInfo(Of Integer)("CityZip"))
    Private Shared IdCountryProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(City), New PropertyInfo(Of Integer)("IdCountry"))

    Private _lastChanged(7) As Byte

    <System.ComponentModel.DataObjectField(True, True)> _
    Public ReadOnly Property Id() As Integer
        Get
            Return GetProperty(Of Integer)(IdProperty)
        End Get
    End Property

    Public Property IdCommunityCode() As Integer
        Get
            Return GetProperty(Of Integer)(IdCommunityCodeProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdCommunityCodeProperty, value)

        End Set
    End Property
    Public Property CityName() As String
        Get
            Return GetProperty(Of String)(CityNameProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(CityNameProperty, value)
        End Set
    End Property
    Public Property CityZip() As Integer
        Get
            Return GetProperty(Of Integer)(CityZipProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(CityZipProperty, value)
        End Set
    End Property
    Public Property IdCountry() As Integer
        Get
            Return GetProperty(Of Integer)(IdCountryProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdCountryProperty, value)
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
            AuthorizationRules.AllowWrite("IdCommunityCode", roleName)
        Else
            AuthorizationRules.DenyWrite("IdCommunityCode", roleName)
        End If
        'AuthorizationRules.AllowWrite("IdCommunityCode")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CityName") Then
            AuthorizationRules.AllowWrite("CityName", roleName)
        Else
            AuthorizationRules.DenyWrite("CityName", roleName)
        End If
        'AuthorizationRules.AllowWrite("CityName")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CityZip") Then
            AuthorizationRules.AllowWrite("CityZip", roleName)
        Else
            AuthorizationRules.DenyWrite("CityZip", roleName)
        End If
        'AuthorizationRules.AllowWrite("CityZip")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCountry") Then
            AuthorizationRules.AllowWrite("IdCountry", roleName)
        Else
            AuthorizationRules.DenyWrite("IdCountry", roleName)
        End If
        'AuthorizationRules.AllowWrite("IdCountry")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
            AuthorizationRules.AllowWrite("Active", roleName)
        Else
            AuthorizationRules.DenyWrite("Active", roleName)
        End If
        'AuthorizationRules.AllowWrite("Active")
    End Sub


    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Cities")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Cities")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Cities")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Cities")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
    Protected Overrides Sub AddBusinessRules()
        ' CityNameProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CityNameProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CityNameProperty, 50))

        'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CityZipProperty)
        'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CityNameProperty, 50))
        ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                                New IntegerMinValueRuleArgs(IdCommunityCodeProperty, 1))
        ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                                New IntegerMinValueRuleArgs(IdCountryProperty, 1))

        'ValidationRules.AddRule(AddressOf Csla.Validation.IntegerMinValue, _
        '                        New Csla.Validation.IntegerMinValueRuleArgs(IdCommunityCodeProperty, 1))
    End Sub



#End Region ' Validation Rules

#Region " Factory Methods "

    Private Sub New()
        ' require use of factory method 
    End Sub

    Public Shared Function NewCity() As City
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a City")
        End If
        Return DataPortal.Create(Of City)()
    End Function

    Public Shared Function GetCity(ByVal id As Integer) As City
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a City")
        End If
        Return DataPortal.Fetch(New SingleCriteria(Of City, Integer)(id))
    End Function

    Public Shared Sub DeleteCity(ByVal id As Integer)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a City")
        End If
        DataPortal.Delete(New SingleCriteria(Of City, Integer)(id))
    End Sub

#End Region ' Factory Methods

#Region " Child Factory Methods "

    Friend Shared Function NewCityChild() As City
        Return DataPortal.CreateChild(Of City)()
    End Function

    Friend Shared Function GetCity(ByVal dr As SafeDataReader) As City
        Return DataPortal.FetchChild(Of City)(dr)
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

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of City, Integer))
        Database.LogInfo("City.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = spGetByID
                    cm.Parameters.AddWithValue("@Id", criteria.Value)
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        dr.Read()

                        LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
                        LoadProperty(Of Integer)(IdCommunityCodeProperty, dr.GetInt32("IdCommunityCode"))
                        LoadProperty(Of String)(CityNameProperty, dr.GetString("CityName"))
                        LoadProperty(Of Integer)(CityZipProperty, dr.GetInt32("CityZip"))
                        LoadProperty(Of Integer)(IdCountryProperty, dr.GetInt32("IdCountry"))

                        dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("City.DataPortal_Fetch", ex)
            Throw New DbCslaException("City.DataPortal_Fetch", ex)
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

                        .Parameters.AddWithValue("@IdCommunityCode", ReadProperty(Of Integer)(IdCommunityCodeProperty))
                        .Parameters.AddWithValue("@CityName", ReadProperty(Of String)(CityNameProperty))
                        .Parameters.AddWithValue("@CityZip", ReadProperty(Of Integer)(CityZipProperty))
                        .Parameters.AddWithValue("@IdCountry", ReadProperty(Of Integer)(IdCountryProperty))

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
            Database.LogException("City.DataPortal_Insert", ex)
            Throw New DbCslaException("City.DataPortal_Insert", ex)
        Finally
            Database.LogInfo("City.DataPortal_Insert", GetHashCode())
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
                        .Parameters.AddWithValue("@IdCommunityCode", ReadProperty(Of Integer)(IdCommunityCodeProperty))
                        .Parameters.AddWithValue("@CityName", ReadProperty(Of String)(CityNameProperty))
                        .Parameters.AddWithValue("@CityZip", ReadProperty(Of Integer)(CityZipProperty))
                        .Parameters.AddWithValue("@IdCountry", ReadProperty(Of Integer)(IdCountryProperty))
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
        DataPortal_Delete(New SingleCriteria(Of City, Integer)(Id))
    End Sub

    Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of City, Integer))
        Database.LogInfo("City.DataPortal_Delete", GetHashCode())
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
            Database.LogException("City.DataPortal_Delete", ex)
            Throw New DbCslaException("City.DataPortal_Delete", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

    Private Sub Child_Fetch(ByVal dr As SafeDataReader)
        Database.LogInfo("City.Child_Fetch", GetHashCode())
        Try
            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of Integer)(IdCommunityCodeProperty, dr.GetInt32("IdCommunityCode"))
            LoadProperty(Of String)(CityNameProperty, dr.GetString("CityName"))
            LoadProperty(Of Integer)(CityZipProperty, dr.GetInt32("CityZip"))
            LoadProperty(Of Integer)(IdCountryProperty, dr.GetInt32("IdCountry"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
        Catch ex As Exception
            Database.LogException("City.Child_Fetch", ex)
            Throw New DbCslaException("City.Child_Fetch", ex)
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
                        .Parameters.AddWithValue("@IdCommunityCode", ReadProperty(Of Integer)(IdCommunityCodeProperty))
                        .Parameters.AddWithValue("@CityName", ReadProperty(Of String)(CityNameProperty))
                        .Parameters.AddWithValue("@CityZip", ReadProperty(Of Integer)(CityZipProperty))
                        .Parameters.AddWithValue("@IdCountry", ReadProperty(Of Integer)(IdCountryProperty))
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
            Database.LogException("City.Child_Insert", ex)
            Throw New DbCslaException("City.Child_Insert", ex)
        Finally
            Database.LogInfo("City.Child_Insert", GetHashCode)
        End Try

    End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

    Private Sub Child_Update()
        Database.LogInfo("City.Child_Update", GetHashCode)
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                ApplicationContext.LocalContext("cn") = cn
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = spUpdate

                        .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
                        .Parameters.AddWithValue("@IdCommunityCode", ReadProperty(Of Integer)(IdCommunityCodeProperty))
                        .Parameters.AddWithValue("@CityName", ReadProperty(Of String)(CityNameProperty))
                        .Parameters.AddWithValue("@CityZip", ReadProperty(Of Integer)(CityZipProperty))
                        .Parameters.AddWithValue("@IdCountry", ReadProperty(Of Integer)(IdCountryProperty))
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
            Database.LogException("City.Child_Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DbCslaException("City.Child_Update", ex)
            End If
        End Try
    End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

    Private Sub Child_DeleteSelf()

        Database.LogInfo("City.Child_DeleteSelf", GetHashCode)
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
            Database.LogException("City.Child_Fetch", ex)
            Throw New DbCslaException("City.Child_Fetch", ex)
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
                    cm.CommandText = "SELECT Id FROM [Cities] WHERE CityName=@naziv"
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

