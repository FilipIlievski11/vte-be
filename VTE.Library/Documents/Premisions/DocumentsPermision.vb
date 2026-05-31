Imports Csla.Validation.CommonRules

<Serializable()> _
Public Class DocumentsPermision
    Inherits Csla.BusinessBase(Of DocumentsPermision)


#Region " Stored Procedures Names "
    Private Const spGetByID As String = "GetDocumentsPermisionByID"
    Private Const spGetAll As String = "GetDocumentsPermisions"
    Private Const spUpdate As String = "updateDocumentsPermision"
    Private Const spAdd As String = "addDocumentsPermision"
    Private Const spDelete As String = "deleteDocumentsPermision"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
    'register properties
    Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsPermision), New PropertyInfo(Of Long)("Id"))
    Private Shared IdCustomerVehicleRelationProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsPermision), New PropertyInfo(Of Long)("IdCustomerVehicleRelation"))
    Private Shared IdCustomerVehicleRelationOwnerProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsPermision), New PropertyInfo(Of Long)("IdCustomerVehicleRelationOwner"))
    Private Shared IdOperatorCreatedProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsPermision), New PropertyInfo(Of Integer)("IdOperatorCreated"))
    Private Shared IdIssuerProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsPermision), New PropertyInfo(Of Integer)("IdIssuer"))
    Private Shared IdCityOfIssuingProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsPermision), New PropertyInfo(Of Integer)("IdCityOfIssuing"))
    Private Shared TrafficLicenceNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsPermision), New PropertyInfo(Of String)("TrafficLicenceNumber"))
    Private Shared PermissionNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsPermision), New PropertyInfo(Of String)("PermissionNumber"))
    Private Shared TriptiqueNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsPermision), New PropertyInfo(Of String)("TriptiqueNumber"))
    Private Shared DateCreatedProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(DocumentsPermision), New PropertyInfo(Of SmartDate)("DateCreated", "DateCreated", New SmartDate(DateTime.Today, True)))
    Private Shared ValidTillDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(DocumentsPermision), New PropertyInfo(Of SmartDate)("ValidTillDate", "ValidTillDate", New SmartDate(DateTime.Today, True)))
    Private Shared DateStartProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(DocumentsPermision), New PropertyInfo(Of SmartDate)("DateStart", "DateStart", New SmartDate(DateTime.Today, True)))

    Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsPermision), New PropertyInfo(Of String)("Note"))
    Private Shared IdOrganisationProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsPermision), New PropertyInfo(Of Integer)("IdOrganisation"))

    Private _lastChanged(7) As Byte

    <System.ComponentModel.DataObjectField(True, True)> _
    Public ReadOnly Property Id() As Long
        Get
            Return GetProperty(Of Long)(IdProperty)
        End Get
    End Property
    Public Property IdCustomerVehicleRelation() As Long
        Get
            Return GetProperty(Of Long)(IdCustomerVehicleRelationProperty)
        End Get
        Set(ByVal value As Long)
            SetProperty(Of Long)(IdCustomerVehicleRelationProperty, value)
        End Set
    End Property
    Public Property IdCustomerVehicleRelationOwner() As Long
        Get
            Return GetProperty(Of Long)(IdCustomerVehicleRelationOwnerProperty)
        End Get
        Set(ByVal value As Long)
            SetProperty(Of Long)(IdCustomerVehicleRelationOwnerProperty, value)
        End Set
    End Property
    Public Property IdOperatorCreated() As Integer
        Get
            Return GetProperty(Of Integer)(IdOperatorCreatedProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdOperatorCreatedProperty, value)
        End Set
    End Property
    Public Property IdIssuer() As Integer
        Get
            Return GetProperty(Of Integer)(IdIssuerProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdIssuerProperty, value)
        End Set
    End Property
    Public Property IdCityOfIssuing() As Integer
        Get
            Return GetProperty(Of Integer)(IdCityOfIssuingProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdCityOfIssuingProperty, value)
        End Set
    End Property

    Public Property PermissionNumber() As String
        Get
            Return GetProperty(Of String)(PermissionNumberProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(PermissionNumberProperty, value)
        End Set
    End Property
    Public Property TrafficLicenceNumber() As String
        Get
            Return GetProperty(Of String)(TrafficLicenceNumberProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(TrafficLicenceNumberProperty, value)
        End Set
    End Property
    Public Property TriptiqueNumber() As String
        Get
            Return GetProperty(Of String)(TriptiqueNumberProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(TriptiqueNumberProperty, value)
        End Set
    End Property
    Public Property DateCreated() As Date
        Get
            Return GetProperty(Of SmartDate, Date)(DateCreatedProperty)
        End Get
        Set(ByVal value As Date)
            SetProperty(Of SmartDate, Date)(DateCreatedProperty, value)
            SetProperty(Of SmartDate, Date)(ValidTillDateProperty, value.AddYears(1))
        End Set
    End Property
    Public Property ValidTillDate() As Date
        Get
            Return GetProperty(Of SmartDate, Date)(ValidTillDateProperty)
        End Get
        Set(ByVal value As Date)
            SetProperty(Of SmartDate, Date)(ValidTillDateProperty, value)
        End Set
    End Property
    Public Property DateStart() As Date
        Get
            Return GetProperty(Of SmartDate, Date)(DateStartProperty)
        End Get
        Set(ByVal value As Date)
            SetProperty(Of SmartDate, Date)(DateStartProperty, value)
        End Set
    End Property


    Public Property Note() As String
        Get
            Return GetProperty(Of String)(NoteProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(NoteProperty, value)
        End Set
    End Property
    Public Property IdOrganisation() As Integer
        Get
            Return GetProperty(Of Integer)(IdOrganisationProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdOrganisationProperty, value)
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Id.ToString
    End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCustomerVehicleRelation") Then
            AuthorizationRules.AllowWrite("IdCustomerVehicleRelation", roleName)
        Else
            AuthorizationRules.DenyWrite("IdCustomerVehicleRelation", roleName)
        End If
        'AuthorizationRules.AllowWrite("IdCustomerVehicleRelation")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCustomerVehicleRelationOwner") Then
            AuthorizationRules.AllowWrite("IdCustomerVehicleRelationOwner", roleName)
        Else
            AuthorizationRules.DenyWrite("IdCustomerVehicleRelationOwner", roleName)
        End If
        'AuthorizationRules.AllowWrite("IdCustomerVehicleRelationOwner")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdOperatorCreated") Then
            AuthorizationRules.AllowWrite("IdOperatorCreated", roleName)
        Else
            AuthorizationRules.DenyWrite("IdOperatorCreated", roleName)
        End If
        'AuthorizationRules.AllowWrite("IdOperatorCreated")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DateCreated") Then
            AuthorizationRules.AllowWrite("DateCreated", roleName)
        Else
            AuthorizationRules.DenyWrite("DateCreated", roleName)
        End If
        'AuthorizationRules.AllowWrite("DateCreated")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ValidTillDate") Then
            AuthorizationRules.AllowWrite("ValidTillDate", roleName)
        Else
            AuthorizationRules.DenyWrite("ValidTillDate", roleName)
        End If
        'AuthorizationRules.AllowWrite("ValidTillDate")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Note") Then
            AuthorizationRules.AllowWrite("Note", roleName)
        Else
            AuthorizationRules.DenyWrite("Note", roleName)
        End If
        'AuthorizationRules.AllowWrite("Note")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
            AuthorizationRules.AllowWrite("Active", roleName)
        Else
            AuthorizationRules.DenyWrite("Active", roleName)
        End If
        'AuthorizationRules.AllowWrite("Active")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdIssuer") Then
            AuthorizationRules.AllowWrite("IdIssuer", roleName)
        Else
            AuthorizationRules.DenyWrite("IdIssuer", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCityOfIssuing") Then
            AuthorizationRules.AllowWrite("IdCityOfIssuing", roleName)
        Else
            AuthorizationRules.DenyWrite("IdCityOfIssuing", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TrafficLicenceNumber") Then
            AuthorizationRules.AllowWrite("TrafficLicenceNumber", roleName)
        Else
            AuthorizationRules.DenyWrite("TrafficLicenceNumber", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PermissionNumber") Then
            AuthorizationRules.AllowWrite("PermissionNumber", roleName)
        Else
            AuthorizationRules.DenyWrite("PermissionNumber", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TriptiqueNumber") Then
            AuthorizationRules.AllowWrite("TriptiqueNumber", roleName)
        Else
            AuthorizationRules.DenyWrite("TriptiqueNumber", roleName)
        End If

    End Sub



    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsPermision")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsPermision")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsPermision")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsPermision")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
    Protected Overrides Sub AddBusinessRules()
        ' DateCreatedProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, DateCreatedProperty)
        ' ValidTillDateProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ValidTillDateProperty)
        ' NoteProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, _
                                New Csla.Validation.CommonRules.MaxLengthRuleArgs(NoteProperty, 250))

        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, TrafficLicenceNumberProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, _
                                New Csla.Validation.CommonRules.MaxLengthRuleArgs(TrafficLicenceNumberProperty, 50))

        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, _
                                New Csla.Validation.CommonRules.MaxLengthRuleArgs(TriptiqueNumberProperty, 50))

        'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
        '                        New Csla.Validation.IntegerMinValueRuleArgs(IdCustomerVehicleRelationProperty, 1))
        ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                                New IntegerMinValueRuleArgs(IdCustomerVehicleRelationOwnerProperty, 1))
        ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                                New IntegerMinValueRuleArgs(IdIssuerProperty, 1))
        ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                                New IntegerMinValueRuleArgs(IdCityOfIssuingProperty, 1))

        ValidationRules.AddRule(Of DocumentsPermision)(AddressOf IdRelation, IdCustomerVehicleRelationProperty)
        ValidationRules.AddRule(Of DocumentsPermision)(AddressOf IdRelation, IdCustomerVehicleRelationOwnerProperty)

        ValidationRules.AddRule(Of DocumentsPermision)(AddressOf PremisionUnique, IdCustomerVehicleRelationProperty)
        ValidationRules.AddRule(Of DocumentsPermision)(AddressOf PremisionUnique, IdCustomerVehicleRelationOwnerProperty)

        ValidationRules.AddDependentProperty(IdCustomerVehicleRelationProperty, IdCustomerVehicleRelationOwnerProperty, True)


        ValidationRules.AddRule(Of DocumentsPermision)(AddressOf DateCreatedAndValid, DateCreatedProperty)
        ValidationRules.AddRule(Of DocumentsPermision)(AddressOf DateCreatedAndValid, ValidTillDateProperty)
        ValidationRules.AddDependentProperty(DateCreatedProperty, ValidTillDateProperty, True)

    End Sub

    Private Shared Function PremisionUnique(Of T As DocumentsPermision)( _
      ByVal target As T, ByVal e As Csla.Validation.RuleArgs) As Boolean
        If (target.IdCustomerVehicleRelation = 0) Or (target.IdCustomerVehicleRelationOwner = 0) Then Return True
        If DocumentsPermision.ExistsPremision(target.IdCustomerVehicleRelation, target.IdCustomerVehicleRelationOwner) Then
            e.Description = "Одобрението постои"
            Return False
        Else
            Return True
        End If
    End Function

    Private Shared Function IdRelation(Of T As DocumentsPermision)(ByVal target As T, _
        ByVal e As Csla.Validation.RuleArgs) As Boolean
        If (target.IdCustomerVehicleRelation = 0) Or (target.IdCustomerVehicleRelationOwner = 0) Then Return True
        'If target.IdCustomerVehicleRelation = target.IdCustomerVehicleRelationOwner Then
        '  e.Description = "Клиентот е сопственик на возилото, не е потрбно полномошно"
        '  Return False
        'End If
        Dim relacija As CustomerVehiclesRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(target.IdCustomerVehicleRelationOwner)
        If (relacija.IdRelationType = 1) AndAlso (relacija.IdCustomer = target.IdCustomerVehicleRelation) Then
            e.Description = "Клиентот е сопственик на возилото, не е потрбно полномошно"
            Return False
        End If

        Return True
    End Function
    Private Shared Function DateCreatedAndValid(Of T As DocumentsPermision)(ByVal target As T, _
        ByVal e As Csla.Validation.RuleArgs) As Boolean
        If target.DateCreated >= target.ValidTillDate Then
            e.Description = "Одберете датум до кој е валидно полномошното"
            Return False
        Else
            Return True
        End If
    End Function
#End Region ' Validation Rules

#Region " Factory Methods "

    Private Sub New()
        ' require use of factory method 
    End Sub

    Public Shared Function NewDocumentsPermision() As DocumentsPermision
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a DocumentsPermision")
        End If
        Return DataPortal.Create(Of DocumentsPermision)()
    End Function

    Public Shared Function GetDocumentsPermision(ByVal id As Long) As DocumentsPermision
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a DocumentsPermision")
        End If
        Return DataPortal.Fetch(New SingleCriteria(Of DocumentsPermision, Integer)(id))
    End Function

    Public Shared Sub DeleteDocumentsPermision(ByVal id As Long)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a DocumentsPermision")
        End If
        DataPortal.Delete(New SingleCriteria(Of DocumentsPermision, Integer)(id))
    End Sub

    Public Overrides Function Save() As DocumentsPermision
        If IsDeleted AndAlso Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a DocumentsPermision")
        ElseIf IsNew AndAlso Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a DocumentsPermision")
        ElseIf Not CanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a DocumentsPermision")
        End If
        Return MyBase.Save()
    End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

    Friend Shared Function NewDocumentsPermisionChild() As DocumentsPermision
        Return DataPortal.CreateChild(Of DocumentsPermision)()
    End Function

    Friend Shared Function GetDocumentsPermision(ByVal dr As SafeDataReader) As DocumentsPermision
        Return DataPortal.FetchChild(Of DocumentsPermision)(dr)
    End Function

#End Region 'Child Factory Methods

#Region " Data Access "

#Region " Data Access - Create "

    <RunLocal()> _
    Private Overloads Sub DataPortal_Create()
        Me.DateCreated = Now.Date
        Dim objOpcii = CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)
        Me.IdIssuer = objOpcii.IdDefaultRegistrationIssuer
        Me.IdCityOfIssuing = objOpcii.IdDefaultCity
        ValidationRules.CheckRules()
    End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of DocumentsPermision, Integer))
        Database.LogInfo("DocumentsPermision.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = spGetByID
                    cm.Parameters.AddWithValue("@Id", criteria.Value)
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        dr.Read()

                        LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
                        LoadProperty(Of Long)(IdCustomerVehicleRelationProperty, dr.GetInt64("IdCustomerVehicleRelation"))
                        LoadProperty(Of Long)(IdCustomerVehicleRelationOwnerProperty, dr.GetInt64("IdCustomerVehicleRelationOwner"))
                        LoadProperty(Of Integer)(IdOperatorCreatedProperty, dr.GetInt32("IdOperatorCreated"))
                        LoadProperty(Of Integer)(IdIssuerProperty, dr.GetInt32("IdIssuer"))
                        LoadProperty(Of Integer)(IdCityOfIssuingProperty, dr.GetInt32("IdCityOfIssuing"))

                        LoadProperty(Of String)(PermissionNumberProperty, dr.GetString("PermissionNumber"))
                        LoadProperty(Of String)(TrafficLicenceNumberProperty, dr.GetString("TrafficLicenceNumber"))
                        LoadProperty(Of String)(TriptiqueNumberProperty, dr.GetString("TriptiqueNumber"))
                        LoadProperty(Of SmartDate, Date?)(DateCreatedProperty, dr.GetSmartDate("DateCreated", True))
                        LoadProperty(Of SmartDate, Date?)(ValidTillDateProperty, dr.GetSmartDate("ValidTillDate", True))
                        LoadProperty(Of SmartDate, Date?)(DateStartProperty, dr.GetSmartDate("DateStart", True))

                        LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
                        LoadProperty(Of Integer)(IdOrganisationProperty, dr.GetInt32("IdOrganisation"))

                        dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("DocumentsPermision.DataPortal_Fetch", ex)
            Throw New DbCslaException("DocumentsPermision.DataPortal_Fetch", ex)
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

                        .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                        .Parameters.AddWithValue("@IdCustomerVehicleRelationOwner", ReadProperty(Of Long)(IdCustomerVehicleRelationOwnerProperty))
                        .Parameters.AddWithValue("@IdOperatorCreated", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
                        .Parameters.AddWithValue("@IdIssuer", ReadProperty(Of Integer)(IdIssuerProperty))
                        .Parameters.AddWithValue("@IdCityOfIssuing", ReadProperty(Of Integer)(IdCityOfIssuingProperty))
                        .Parameters.AddWithValue("@PermissionNumber", ReadProperty(Of String)(PermissionNumberProperty))
                        .Parameters.AddWithValue("@TrafficLicenceNumber", ReadProperty(Of String)(TrafficLicenceNumberProperty))
                        .Parameters.AddWithValue("@TriptiqueNumber", ReadProperty(Of String)(TriptiqueNumberProperty))
                        .Parameters.AddWithValue("@DateCreated", ReadProperty(Of SmartDate)(DateCreatedProperty).DBValue)
                        .Parameters.AddWithValue("@ValidTillDate", ReadProperty(Of SmartDate)(ValidTillDateProperty).DBValue)
                        .Parameters.AddWithValue("@DateStart", ReadProperty(Of SmartDate)(DateStartProperty).DBValue)

                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
                        .Parameters.AddWithValue("@IdOrganisation", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)


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
                'zadolzuvanje po izvrsena usluga, по IdCustomerVehicle
                AddDeptsToCustomer(cn)
                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("DocumentsPermision.DataPortal_Insert", ex)
            Throw New DbCslaException("DocumentsPermision.DataPortal_Insert", ex)
        Finally
            Database.LogInfo("DocumentsPermision.DataPortal_Insert", GetHashCode())
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

                        .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
                        .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                        .Parameters.AddWithValue("@IdCustomerVehicleRelationOwner", ReadProperty(Of Long)(IdCustomerVehicleRelationOwnerProperty))
                        .Parameters.AddWithValue("@IdOperatorCreated", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
                        .Parameters.AddWithValue("@IdIssuer", ReadProperty(Of Integer)(IdIssuerProperty))
                        .Parameters.AddWithValue("@IdCityOfIssuing", ReadProperty(Of Integer)(IdCityOfIssuingProperty))
                        .Parameters.AddWithValue("@PermissionNumber", ReadProperty(Of String)(PermissionNumberProperty))
                        .Parameters.AddWithValue("@TrafficLicenceNumber", ReadProperty(Of String)(TrafficLicenceNumberProperty))
                        .Parameters.AddWithValue("@TriptiqueNumber", ReadProperty(Of String)(TriptiqueNumberProperty))
                        .Parameters.AddWithValue("@DateCreated", ReadProperty(Of SmartDate)(DateCreatedProperty).DBValue)
                        .Parameters.AddWithValue("@ValidTillDate", ReadProperty(Of SmartDate)(ValidTillDateProperty).DBValue)
                        .Parameters.AddWithValue("@DateStart", ReadProperty(Of SmartDate)(DateStartProperty).DBValue)
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
                        .Parameters.AddWithValue("@IdOrganisation", ReadProperty(Of Integer)(IdOrganisationProperty))
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
        DataPortal_Delete(New SingleCriteria(Of DocumentsPermision, Integer)(Id))
    End Sub

    Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of DocumentsPermision, Integer))
        Database.LogInfo("DocumentsPermision.DataPortal_Delete", GetHashCode())
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
            Database.LogException("DocumentsPermision.DataPortal_Delete", ex)
            Throw New DbCslaException("DocumentsPermision.DataPortal_Delete", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

    Private Sub Child_Fetch(ByVal dr As SafeDataReader)
        Database.LogInfo("DocumentsPermision.Child_Fetch", GetHashCode())
        Try
            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of Long)(IdCustomerVehicleRelationProperty, dr.GetInt64("IdCustomerVehicleRelation"))
            LoadProperty(Of Long)(IdCustomerVehicleRelationOwnerProperty, dr.GetInt64("IdCustomerVehicleRelationOwner"))
            LoadProperty(Of Integer)(IdOperatorCreatedProperty, dr.GetInt32("IdOperatorCreated"))
            LoadProperty(Of Integer)(IdIssuerProperty, dr.GetInt32("IdIssuer"))
            LoadProperty(Of Integer)(IdCityOfIssuingProperty, dr.GetInt32("IdCityOfIssuing"))
            LoadProperty(Of String)(PermissionNumberProperty, dr.GetString("PermissionNumber"))
            LoadProperty(Of String)(TrafficLicenceNumberProperty, dr.GetString("TrafficLicenceNumber"))
            LoadProperty(Of String)(TriptiqueNumberProperty, dr.GetString("TriptiqueNumber"))
            LoadProperty(Of SmartDate, Date?)(DateCreatedProperty, dr.GetSmartDate("DateCreated", True))
            LoadProperty(Of SmartDate, Date?)(ValidTillDateProperty, dr.GetSmartDate("ValidTillDate", True))
            LoadProperty(Of SmartDate, Date?)(DateStartProperty, dr.GetSmartDate("DateStart", True))
            LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
            LoadProperty(Of Integer)(IdOrganisationProperty, dr.GetInt32("IdOrganisation"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
        Catch ex As Exception
            Database.LogException("DocumentsPermision.Child_Fetch", ex)
            Throw New DbCslaException("DocumentsPermision.Child_Fetch", ex)
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
                        .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                        .Parameters.AddWithValue("@IdCustomerVehicleRelationOwner", ReadProperty(Of Long)(IdCustomerVehicleRelationOwnerProperty))
                        .Parameters.AddWithValue("@IdOperatorCreated", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
                        .Parameters.AddWithValue("@IdIssuer", ReadProperty(Of Integer)(IdIssuerProperty))
                        .Parameters.AddWithValue("@IdCityOfIssuing", ReadProperty(Of Integer)(IdCityOfIssuingProperty))
                        .Parameters.AddWithValue("@PermissionNumber", ReadProperty(Of String)(PermissionNumberProperty))
                        .Parameters.AddWithValue("@TrafficLicenceNumber", ReadProperty(Of String)(TrafficLicenceNumberProperty))
                        .Parameters.AddWithValue("@TriptiqueNumber", ReadProperty(Of String)(TriptiqueNumberProperty))
                        .Parameters.AddWithValue("@DateCreated", ReadProperty(Of SmartDate)(DateCreatedProperty).DBValue)
                        .Parameters.AddWithValue("@ValidTillDate", ReadProperty(Of SmartDate)(ValidTillDateProperty).DBValue)
                        .Parameters.AddWithValue("@DateStart", ReadProperty(Of SmartDate)(DateStartProperty).DBValue)
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
                        .Parameters.AddWithValue("@IdOrganisation", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)

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
                'zadolzuvanje po izvrsena usluga, по IdCustomerVehicle
                AddDeptsToCustomer(cn)
                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("DocumentsPermision.Child_Insert", ex)
            Throw New DbCslaException("DocumentsPermision.Child_Insert", ex)
        Finally
            Database.LogInfo("DocumentsPermision.Child_Insert", GetHashCode)
        End Try

    End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

    Private Sub Child_Update()
        Database.LogInfo("DocumentsPermision.Child_Update", GetHashCode)
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                ApplicationContext.LocalContext("cn") = cn
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = spUpdate

                        .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
                        .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                        .Parameters.AddWithValue("@IdCustomerVehicleRelationOwner", ReadProperty(Of Long)(IdCustomerVehicleRelationOwnerProperty))
                        .Parameters.AddWithValue("@IdOperatorCreated", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
                        .Parameters.AddWithValue("@IdIssuer", ReadProperty(Of Integer)(IdIssuerProperty))
                        .Parameters.AddWithValue("@IdCityOfIssuing", ReadProperty(Of Integer)(IdCityOfIssuingProperty))
                        .Parameters.AddWithValue("@PermissionNumber", ReadProperty(Of String)(PermissionNumberProperty))
                        .Parameters.AddWithValue("@TrafficLicenceNumber", ReadProperty(Of String)(TrafficLicenceNumberProperty))
                        .Parameters.AddWithValue("@TriptiqueNumber", ReadProperty(Of String)(TriptiqueNumberProperty))
                        .Parameters.AddWithValue("@DateCreated", ReadProperty(Of SmartDate)(DateCreatedProperty).DBValue)
                        .Parameters.AddWithValue("@ValidTillDate", ReadProperty(Of SmartDate)(ValidTillDateProperty).DBValue)
                        .Parameters.AddWithValue("@DateStart", ReadProperty(Of SmartDate)(DateStartProperty).DBValue)
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
                        .Parameters.AddWithValue("@IdOrganisation", ReadProperty(Of Integer)(IdOrganisationProperty))

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
            Database.LogException("DocumentsPermision.Child_Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DbCslaException("DocumentsPermision.Child_Update", ex)
            End If
        End Try
    End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

    Private Sub Child_DeleteSelf()

        Database.LogInfo("DocumentsPermision.Child_DeleteSelf", GetHashCode)
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
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
            Database.LogException("DocumentsPermision.Child_Fetch", ex)
            Throw New DbCslaException("DocumentsPermision.Child_Fetch", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

#Region " PremisionExists "
    Public Shared Function ExistsPremision(ByVal idCustomer As Long, ByVal idOwner As Long) As Boolean

        Return ExistsPremisionCommand.Exists(idCustomer, idOwner)

    End Function

    <Serializable()> _
    Private Class ExistsPremisionCommand
        Inherits CommandBase

        Private _idCustomer As Long
        Private _idOwner As Long

        Private _exists As Boolean
        Public ReadOnly Property PremisionExists() As Boolean
            Get
                Return _exists
            End Get
        End Property

        Public Sub New(ByVal idCustomer As Long, ByVal idOwner As Long)
            _idCustomer = idCustomer
            _idOwner = idOwner
        End Sub

        Public Shared Function Exists(ByVal idCustomer As Long, ByVal idOwner As Long) As Boolean

            Dim result As ExistsPremisionCommand
            result = DataPortal.Execute(Of ExistsPremisionCommand)(New ExistsPremisionCommand(idCustomer, idOwner))
            Return result.PremisionExists

        End Function

        Protected Overrides Sub DataPortal_Execute()
            Dim count As Long = 0

            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "existsPremision"
                    cm.Parameters.AddWithValue("@idCustomer", _idCustomer)
                    cm.Parameters.AddWithValue("@idOwner", _idOwner)
                    Dim result As Integer = CInt(cm.ExecuteScalar)
                    _exists = (result > 0)
                End Using
            End Using

        End Sub

    End Class
#End Region

#Region " Payment "

    Private Sub AddDeptsToCustomer(ByVal cn As SqlConnection)
        Try

            'zemi go voziloto od relacijata
            Dim tmpVehicleId As Integer = _
              CustomerVehiclesRelation.GetCustomerVehiclesRelation( _
              ReadProperty(Of Long)(IdCustomerVehicleRelationOwnerProperty)).IdVehicle
            Dim vInfo As Vehicle
            If tmpVehicleId > 0 Then
                vInfo = Vehicle.GetVehicle(tmpVehicleId)
            Else
                vInfo = Nothing
            End If
            Dim pCatalog As PaymentCataologList = _
             CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
             PaymentCataologList).GetPaymentForDepts("TrigerdByPremisionForVehicle", vInfo)

            For Each pInfo As PaymentCataologInfo In pCatalog

                'vo sluaj poedinecno(so formula)
                If (pInfo.ParametarFrom = 0) AndAlso (pInfo.ParametarTo = 0) Then

                    Using cm As SqlCommand = cn.CreateCommand
                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = "insertFinancialStatePriceCatalogForPermisions"
                        cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationOwnerProperty)) '(IdCustomerVehicleRelationProperty))
                        cm.Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdProperty))
                        cm.Parameters.AddWithValue("@note", "по одобрение бр." & ReadProperty(Of Long)(IdProperty))
                        cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)

                        If pInfo.VehicleField = "Null" Then
                            'fiksno
                            cm.Parameters.AddWithValue("@Price", pInfo.Price)
                        Else
                            'presmetlivo
                            cm.Parameters.AddWithValue("@Price", pInfo.Price * CInt(CallByName(vInfo, pInfo.VehicleField, CallType.Get)))
                        End If

                        cm.Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
                        cm.ExecuteNonQuery()
                    End Using
                Else

                    Using cm As SqlCommand = cn.CreateCommand
                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = "insertFinancialStatePriceCatalogForPermisions"
                        cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationOwnerProperty)) '(IdCustomerVehicleRelationProperty))
                        cm.Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdProperty))
                        cm.Parameters.AddWithValue("@note", "барање одобрение бр." & ReadProperty(Of Long)(IdProperty))
                        cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)
                        cm.Parameters.AddWithValue("@Price", pInfo.Price)
                        cm.Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
                        cm.ExecuteNonQuery()
                    End Using

                End If

            Next

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#End Region

End Class
