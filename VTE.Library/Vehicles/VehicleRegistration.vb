
<Serializable()> _
Public Class VehicleRegistration
    Inherits Csla.BusinessBase(Of VehicleRegistration)

#Region " Stored Procedures Names "
    Private Const spGetByID As String = "GetVehicleRegistrationByID"
    Private Const spGetAll As String = "GetVehicleRegistrations"
    Private Const spUpdate As String = "updateVehicleRegistration"
    Private Const spAdd As String = "addVehicleRegistration"
    Private Const spDelete As String = "deleteVehicleRegistration"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
    'register properties
    Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(VehicleRegistration), New PropertyInfo(Of Long)("Id"))
    Private Shared IdVehicleProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(VehicleRegistration), New PropertyInfo(Of Long)("IdVehicle", "IdVehicle", 1))
    Private Shared IdRegistrationIssuerProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleRegistration), New PropertyInfo(Of Integer)("IdRegistrationIssuer", "IdRegistrationIssuer", 1))
    Private Shared RegistrationNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleRegistration), New PropertyInfo(Of String)("RegistrationNumber"))
    Private Shared DateOfRegistrationProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(VehicleRegistration), New PropertyInfo(Of SmartDate)("DateOfRegistration", "DateOfRegistration", New SmartDate(DateTime.Today, True)))
    Private Shared DateRegistrationValidTillProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(VehicleRegistration), New PropertyInfo(Of SmartDate)("DateRegistrationValidTill", "DateRegistrationValidTill", New SmartDate(DateTime.Today, True)))
    ' Private Shared PlaceOfRegistrationProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleRegistration), New PropertyInfo(Of String)("PlaceOfRegistration"))
    Private Shared IsFirstRegistrationProperty As PropertyInfo(Of Boolean) = RegisterProperty(New PropertyInfo(Of Boolean)("IsFirstRegistration", "IsFirstRegistration", False))

    Private _lastChanged(7) As Byte

    <System.ComponentModel.DataObjectField(True, True)> _
    Public ReadOnly Property Id() As Long
        Get
            Return GetProperty(Of Long)(IdProperty)
        End Get
    End Property
    Public Property IdVehicle() As Long
        Get
            Return GetProperty(Of Long)(IdVehicleProperty)
        End Get
        Set(ByVal value As Long)
            SetProperty(Of Long)(IdVehicleProperty, value)
        End Set
    End Property
    Public Property IdRegistrationIssuer() As Integer
        Get
            Return GetProperty(Of Integer)(IdRegistrationIssuerProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdRegistrationIssuerProperty, value)
        End Set
    End Property
    Public Property RegistrationNumber() As String
        Get
            Return GetProperty(Of String)(RegistrationNumberProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(RegistrationNumberProperty, value)
        End Set
    End Property
    Public Property DateOfRegistration() As Date
        Get
            Return GetProperty(Of SmartDate, Date)(DateOfRegistrationProperty)
        End Get
        Set(ByVal value As Date)
            SetProperty(Of SmartDate, Date)(DateOfRegistrationProperty, value)
            'stavi go istiot datum plus 1 godina za registracjiata
            SetProperty(Of SmartDate, Date)(DateRegistrationValidTillProperty, value.AddYears(1))
        End Set
    End Property
    Public Property DateRegistrationValidTill() As Date
        Get
            Return GetProperty(Of SmartDate, Date)(DateRegistrationValidTillProperty)
        End Get
        Set(ByVal value As Date)
            SetProperty(Of SmartDate, Date)(DateRegistrationValidTillProperty, value)
        End Set
    End Property
    'Public Property PlaceOfRegistration() As String
    '    Get
    '        Return GetProperty(Of String)(PlaceOfRegistrationProperty)
    '    End Get
    '    Set(ByVal value As String)
    '        SetProperty(Of String)(PlaceOfRegistrationProperty, value)
    '    End Set
    'End Property
    Public Property IsFirstRegistration() As Boolean
        Get
            Return GetProperty(Of Boolean)(IsFirstRegistrationProperty)
        End Get
        Set(ByVal value As Boolean)
            SetProperty(Of Boolean)(IsFirstRegistrationProperty, value)
        End Set
    End Property


    Public Overrides Function ToString() As String
        Return Id.ToString
    End Function
#End Region 'Business Properties and Methods

#Region " Validation Rules "


    Protected Overrides Sub AddBusinessRules()
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, _
                                RegistrationNumberProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, _
                                New Csla.Validation.CommonRules.MaxLengthRuleArgs(RegistrationNumberProperty, 10))
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, _
                                DateOfRegistrationProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, _
                                DateRegistrationValidTillProperty)
        'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, _
        '                        PlaceOfRegistrationProperty)
        'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, _
        '                        New Csla.Validation.CommonRules.MaxLengthRuleArgs(PlaceOfRegistrationProperty, 150))
        'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
        '                        New Csla.Validation.IntegerMinValueRuleArgs(IdRegistrationIssuerProperty, 1))

        '' '' ''ValidationRules.AddRule(Of VehicleRegistration)( _
        '' '' ''  AddressOf NoDuplicatesRegistrationNumber, RegistrationNumberProperty)
        ValidationRules.AddRule(Of VehicleRegistration) _
        (AddressOf DateCreatedAndValid, DateOfRegistrationProperty)
        ValidationRules.AddRule(Of VehicleRegistration) _
        (AddressOf DateCreatedAndValid, DateRegistrationValidTillProperty)
        ValidationRules.AddDependentProperty(DateOfRegistrationProperty, DateRegistrationValidTillProperty, True)
        ' '' ''ValidationRules.AddRule(Of VehicleRegistration)( _
        ' '' ''  AddressOf FirstRegistrationOnlyOne, IsFirstRegistrationProperty)
    End Sub

    Private Shared Function FirstRegistrationOnlyOne(Of T As VehicleRegistration)(ByVal target As T, _
      ByVal e As Csla.Validation.RuleArgs) As Boolean
        'izlezi ako e false, nema potreba da proveruva
        If target.IsFirstRegistration = False Then Return True

        Dim parent As VehicleRegistrations = CType(target.Parent, VehicleRegistrations)
        If parent IsNot Nothing Then
            For Each item As VehicleRegistration In parent
                If (item.IsFirstRegistration = True) AndAlso (Not ReferenceEquals(item, target)) Then
                    e.Description = "Може да има само една прва регистрација"
                    Return False
                End If
            Next
        End If
        Return True
    End Function

    Private Shared Function DateCreatedAndValid(Of T As VehicleRegistration)(ByVal target As T, _
       ByVal e As Csla.Validation.RuleArgs) As Boolean
        If target.DateOfRegistration > target.DateRegistrationValidTill Then
            e.Description = "Одберете датум до кој е валидна регистрацијата"
            Return False
        Else
            Return True
        End If
    End Function

    Private Shared Function NoDuplicatesRegistrationNumber(Of T As VehicleRegistration)(ByVal target As T, _
      ByVal e As Csla.Validation.RuleArgs) As Boolean
        Dim objCurentTehExamStation As TehnicalExamOrganizationsInfo = CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)

        Dim communitList As CommunitiesList = Csla.ApplicationContext.LocalContext("objCommunityList")
        Dim pom As String = communitList.GetCommunitiesListById(objCurentTehExamStation.IdCommunity).RegistrationCode
        Dim defaultRegistration As String = pom & "-000-AA"

        If (target.RegistrationNumber <> defaultRegistration) AndAlso target.IsFirstRegistration = False AndAlso VehicleRegistration.RegistrationNumberExists(target.RegistrationNumber, target.Id) Then
            e.Description = "Регистрацијата мора да биде единствена"
            Return False
        Else
            Return True
        End If
    End Function

#End Region ' Validation Rules

#Region " Factory Methods "

    Public Shared Function NewRegistration() As VehicleRegistration
        Return DataPortal.Create(Of VehicleRegistration)()
    End Function

    Public Overrides Function Save() As VehicleRegistration
        Dim result As VehicleRegistration = MyBase.Save
        Return result
    End Function

    Friend Shared Function NewVehicleRegistrationChild() As VehicleRegistration
        Return DataPortal.Create(Of VehicleRegistration)()
    End Function

    Friend Shared Function GetVehicleRegistration(ByVal dr As SafeDataReader) As VehicleRegistration
        Return DataPortal.Fetch(Of VehicleRegistration)(dr)
    End Function

    Private Sub New()
    End Sub

#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Create "
    Protected Overloads Sub DataPortal_Create()
        Dim objCurentTehExamStation As TehnicalExamOrganizationsInfo = CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)

        Dim communitList As CommunitiesList = Csla.ApplicationContext.LocalContext("objCommunityList")
        Dim pom As CommunitiesInfo = communitList.GetCommunitiesListById(objCurentTehExamStation.IdCommunity)
        Me.RegistrationNumber = pom.RegistrationCode & "-000-AA"
        '  Me.PlaceOfRegistration = pom.CommunityName

        Me.DateOfRegistration = Now.Date
        Me.DateRegistrationValidTill = Now.AddYears(1).Date

        ValidationRules.CheckRules()
    End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

    Private Overloads Sub DataPortal_Fetch(ByVal dr As SafeDataReader)
        Database.LogInfo("VehicleRegistration.Fetch", GetHashCode())
        Try

            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of Long)(IdVehicleProperty, dr.GetInt64("IdVehicle"))
            LoadProperty(Of Integer)(IdRegistrationIssuerProperty, dr.GetInt32("IdRegistrationIssuer"))
            LoadProperty(Of String)(RegistrationNumberProperty, dr.GetString("RegistrationNumber"))
            LoadProperty(Of SmartDate, Date?)(DateOfRegistrationProperty, dr.GetSmartDate("DateOfRegistration", True))
            LoadProperty(Of SmartDate, Date?)(DateRegistrationValidTillProperty, dr.GetSmartDate("DateRegistrationValidTill", True))
            '  LoadProperty(Of String)(PlaceOfRegistrationProperty, dr.GetString("PlaceOfRegistration"))
            LoadProperty(Of Boolean)(IsFirstRegistrationProperty, dr.GetBoolean("IsFirstRegistration"))
            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

            ValidationRules.CheckRules()

        Catch ex As Exception
            Database.LogException("VehicleRegistration.Fetch", ex)
            Throw New DbCslaException("VehicleRegistration.Fetch", ex)
        End Try

    End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

    Private Overloads Sub DataPortal_Insert()
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                '' ''Using cm1 As SqlCommand = cn.CreateCommand
                '' ''    cm1.CommandType = CommandType.StoredProcedure
                '' ''    cm1.CommandText = spAdd
                '' ''    cm1.Parameters.AddWithValue("@IdVehicle", IdVehicleProperty)
                '' ''    cm1.ExecuteNonQuery()
                '' ''End Using
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = spAdd
                        'Smeni go Id so Parent.Id
                        .Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdVehicleProperty))
                        .Parameters.AddWithValue("@IdRegistrationIssuer", ReadProperty(Of Integer)(IdRegistrationIssuerProperty))
                        .Parameters.AddWithValue("@RegistrationNumber", ReadProperty(Of String)(RegistrationNumberProperty))
                        .Parameters.AddWithValue("@DateOfRegistration", ReadProperty(Of SmartDate)(DateOfRegistrationProperty).DBValue)
                        .Parameters.AddWithValue("@DateRegistrationValidTill", ReadProperty(Of SmartDate)(DateRegistrationValidTillProperty).DBValue)
                        ' .Parameters.AddWithValue("@PlaceOfRegistration", ReadProperty(Of String)(PlaceOfRegistrationProperty))
                        .Parameters.AddWithValue("@IsFirstRegistration", ReadProperty(Of Boolean)(IsFirstRegistrationProperty))
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
            End Using
        Catch ex As Exception
            Database.LogException("VehicleRegistration.Insert", ex)
            Throw New DbCslaException("VehicleRegistration.Insert", ex)
        Finally
            Database.LogInfo("VehicleRegistration.Insert", GetHashCode)
        End Try

    End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

    Private Overloads Sub DataPortal_Update()
        Database.LogInfo("VehicleRegistration.Update", GetHashCode)
        Try
            Using cn As SqlConnection = ApplicationContext.LocalContext("cn")
                If cn.State = ConnectionState.Closed Then
                    cn.ConnectionString = Database.VTEConnection
                    cn.Open()
                End If
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = spUpdate

                        .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
                        .Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdVehicleProperty))
                        .Parameters.AddWithValue("@IdRegistrationIssuer", ReadProperty(Of Integer)(IdRegistrationIssuerProperty))
                        .Parameters.AddWithValue("@RegistrationNumber", ReadProperty(Of String)(RegistrationNumberProperty))
                        .Parameters.AddWithValue("@DateOfRegistration", ReadProperty(Of SmartDate)(DateOfRegistrationProperty).DBValue)
                        .Parameters.AddWithValue("@DateRegistrationValidTill", ReadProperty(Of SmartDate)(DateRegistrationValidTillProperty).DBValue)
                        '  .Parameters.AddWithValue("@PlaceOfRegistration", ReadProperty(Of String)(PlaceOfRegistrationProperty))
                        .Parameters.AddWithValue("@IsFirstRegistration", ReadProperty(Of Boolean)(IsFirstRegistrationProperty))
                        .Parameters.AddWithValue("@lastChanged", _lastChanged)
                        Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
                        param.Direction = ParameterDirection.Output
                        .Parameters.Add(param)

                        .ExecuteNonQuery()

                        _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
                    End With
                End Using
                'update child objects
            End Using
        Catch ex As Exception
            Database.LogException("VehicleRegistration.Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DbCslaException("VehicleRegistration.Update", ex)
            End If
        End Try
    End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

    Private Overloads Sub DataPortal_Delete()

        Database.LogInfo("VehicleRegistration.DeleteSelf", GetHashCode)
        Try
            Using cn As SqlConnection = Csla.ApplicationContext.LocalContext("cn")
                If cn.State = ConnectionState.Closed Then
                    cn.ConnectionString = Database.VTEConnection
                    cn.Open()
                End If
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
            Database.LogException("VehicleRegistration.Fetch", ex)
            Throw New DbCslaException("VehicleRegistration.Fetch", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access


#Region " RegistrationNumberExists "

    Public Shared Function RegistrationNumberExists(ByVal strRegistrationNumber As String, ByVal idVehic As Long) As Boolean

        Return RegistrationNumberExistsCommand.RegistrationNumberExists(strRegistrationNumber, idVehic)

    End Function

    <Serializable()> _
    Private Class RegistrationNumberExistsCommand
        Inherits CommandBase
        Private _registrationNumber As String
        Private _idVehic As Long
        Private _registrationNumberExists As Boolean
        Public ReadOnly Property RegistrationExists() As Boolean
            Get
                Return _registrationNumberExists
            End Get
        End Property

        Public Shared Function RegistrationNumberExists(ByVal strRegistrationNumber As String, ByVal idVehic As Long) As Boolean

            Dim result As RegistrationNumberExistsCommand
            result = DataPortal.Execute(Of RegistrationNumberExistsCommand)(New RegistrationNumberExistsCommand(strRegistrationNumber, idVehic))
            Return result.RegistrationExists

        End Function

        Private Sub New(ByVal strRegistrationNumber As String, ByVal idVehic As Long)
            _registrationNumber = strRegistrationNumber
            _idVehic = idVehic
            _registrationNumberExists = False
        End Sub

        Protected Overrides Sub DataPortal_Execute()
            Dim pom As Integer = 0
            Using cn As SqlConnection = Database.VTE_SqlConnection
                ApplicationContext.LocalContext("cn") = cn
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "NumOfVehiclesRegistrationNumberExists"
                    cm.Parameters.AddWithValue("@sifra", _registrationNumber)
                    cm.Parameters.AddWithValue("@idVehicle", _idVehic)
                    pom = cm.ExecuteScalar
                    If pom = 0 Then
                        _registrationNumberExists = False
                    Else
                        _registrationNumberExists = True
                    End If
                End Using
            End Using
        End Sub

    End Class

#End Region

End Class
