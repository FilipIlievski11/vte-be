Imports Csla.Validation.CommonRules

<Serializable()> _
Public Class DocumentsTrafficLicence
    Inherits Csla.BusinessBase(Of DocumentsTrafficLicence)

#Region " Stored Procedures Names "
    Private Const spGetByID As String = "GetDocumentsTrafficLicenceByID"
    Private Const spGetAll As String = "GetDocumentsTrafficLicences"
    Private Const spUpdate As String = "updateDocumentsTrafficLicence"
    Private Const spAdd As String = "addDocumentsTrafficLicence"
    Private Const spDelete As String = "deleteDocumentsTrafficLicence"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
    'register properties
    Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsTrafficLicence), New PropertyInfo(Of Long)("Id"))
    Private Shared IdCustomerVehicleRelationProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsTrafficLicence), New PropertyInfo(Of Long)("IdCustomerVehicleRelation"))
    Private Shared IdTehnicalExamOrganizationsIssuedByProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsTrafficLicence), New PropertyInfo(Of Integer)("IdTehnicalExamOrganizationsIssuedBy"))
    Private Shared TrafficLicenceNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsTrafficLicence), New PropertyInfo(Of String)("TrafficLicenceNumber"))
    Private Shared MadeDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(DocumentsTrafficLicence), New PropertyInfo(Of SmartDate)("MadeDate", "MadeDate", New SmartDate(DateTime.Today, True)))
    Private Shared EndDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(DocumentsTrafficLicence), New PropertyInfo(Of SmartDate)("EndDate"))
    Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsTrafficLicence), New PropertyInfo(Of String)("Note"))

    Private _lastChanged(7) As Byte
    Private Shared ExtensionsProperty As PropertyInfo(Of TrafficLicencesExtensions) = _
      RegisterProperty(Of TrafficLicencesExtensions)(GetType(DocumentsTrafficLicence), _
      New PropertyInfo(Of TrafficLicencesExtensions)("Extensions"))

    Public ReadOnly Property Extensions() As TrafficLicencesExtensions
        Get
            If Not FieldManager.FieldExists(ExtensionsProperty) Then
                SetProperty(Of TrafficLicencesExtensions)(ExtensionsProperty, _
                  TrafficLicencesExtensions.NewTrafficLicencesExtensions)
            End If
            Return GetProperty(Of TrafficLicencesExtensions)(ExtensionsProperty)
        End Get
    End Property

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
    Public Property IdTehnicalExamOrganizationsIssuedBy() As Integer
        Get
            Return GetProperty(Of Integer)(IdTehnicalExamOrganizationsIssuedByProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdTehnicalExamOrganizationsIssuedByProperty, value)
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
    Public Property MadeDate() As Date
        Get
            Return GetProperty(Of SmartDate, Date)(MadeDateProperty)
        End Get
        Set(ByVal value As Date)
            SetProperty(Of SmartDate, Date)(MadeDateProperty, value)

        End Set
    End Property

    Public Property EndDate() As Date
        Get
            Return GetProperty(Of SmartDate, Date)(EndDateProperty)
        End Get
        Set(ByVal value As Date)
            SetProperty(Of SmartDate, Date)(EndDateProperty, value)
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
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdTehnicalExamOrganizationsIssuedBy") Then
            AuthorizationRules.AllowWrite("IdTehnicalExamOrganizationsIssuedBy", roleName)
        Else
            AuthorizationRules.DenyWrite("IdTehnicalExamOrganizationsIssuedBy", roleName)
        End If
        'AuthorizationRules.AllowWrite("IdTehnicalExamOrganizationsIssuedBy")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TrafficLicenceNumber") Then
            AuthorizationRules.AllowWrite("TrafficLicenceNumber", roleName)
        Else
            AuthorizationRules.DenyWrite("TrafficLicenceNumber", roleName)
        End If
        'AuthorizationRules.AllowWrite("TrafficLicenceNumber")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("MadeDate") Then
            AuthorizationRules.AllowWrite("MadeDate", roleName)
        Else
            AuthorizationRules.DenyWrite("MadeDate", roleName)
        End If
        'AuthorizationRules.AllowWrite("MadeDate")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("EndDate") Then
            AuthorizationRules.AllowWrite("EndDate", roleName)
        Else
            AuthorizationRules.DenyWrite("EndDate", roleName)
        End If
        'AuthorizationRules.AllowWrite("EndDate")
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
    End Sub



    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsTrafficLicence")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsTrafficLicence")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsTrafficLicence")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsTrafficLicence")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
    Protected Overrides Sub AddBusinessRules()
        ' TrafficLicenceNumberProperty rules
        'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, TrafficLicenceNumberProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(TrafficLicenceNumberProperty, 50))
        ' MadeDateProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, MadeDateProperty)
        ' EndDateProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, EndDateProperty)
        ' NoteProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NoteProperty, 250))
        ' ValidationRules.AddRule(Of DocumentsTrafficLicence)(AddressOf NumberUnicat, TrafficLicenceNumberProperty)

        ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                               New IntegerMinValueRuleArgs(IdCustomerVehicleRelationProperty, 1))
        'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
        '                       New Validation.IntegerMinValueRuleArgs(IdTehnicalExamOrganizationsIssuedByProperty, 1))

    End Sub

    Private Shared Function NumberUnicat(Of T As DocumentsTrafficLicence)(ByVal target As T, _
         ByVal e As Csla.Validation.RuleArgs) As Boolean
        If DocumentsTrafficLicences.TrafficLicenceExists(target.TrafficLicenceNumber, target.Id) Then
            e.Description = "Бројот на сообраќајната мора да биде единствен"
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

    Public Shared Function NewDocumentsTrafficLicence() As DocumentsTrafficLicence
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a DocumentsTrafficLicence")
        End If
        Return DataPortal.Create(Of DocumentsTrafficLicence)()
    End Function

    Public Shared Function GetDocumentsTrafficLicence(ByVal id As Long) As DocumentsTrafficLicence
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a DocumentsTrafficLicence")
        End If
        Return DataPortal.Fetch(New SingleCriteria(Of DocumentsTrafficLicence, Integer)(id))
    End Function

    Public Shared Sub DeleteDocumentsTrafficLicence(ByVal id As Long)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a DocumentsTrafficLicence")
        End If
        DataPortal.Delete(New SingleCriteria(Of DocumentsTrafficLicence, Integer)(id))
    End Sub

    Public Overrides Function Save() As DocumentsTrafficLicence
        If IsDeleted AndAlso Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a DocumentsTrafficLicence")
        ElseIf IsNew AndAlso Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a DocumentsTrafficLicence")
        ElseIf Not CanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a DocumentsTrafficLicence")
        End If
        Return MyBase.Save()
    End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

    Friend Shared Function NewDocumentsTrafficLicenceChild() As DocumentsTrafficLicence
        Return DataPortal.CreateChild(Of DocumentsTrafficLicence)()
    End Function

    Friend Shared Function GetDocumentsTrafficLicence(ByVal dr As SafeDataReader) As DocumentsTrafficLicence
        Return DataPortal.FetchChild(Of DocumentsTrafficLicence)(dr)
    End Function

#End Region 'Child Factory Methods

#Region " Data Access "

#Region " Data Access - Create "

    <RunLocal()> _
    Private Overloads Sub DataPortal_Create()
        Me.MadeDate = Now.Date
        Dim curenttehOrganization As TehnicalExamOrganizationsInfo = Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization")
        SetProperty(Of Integer)(IdTehnicalExamOrganizationsIssuedByProperty, curenttehOrganization.IdDefaultRegistrationIssuer)

        SetProperty(Of SmartDate, Date)(EndDateProperty, MadeDate.AddMonths(curenttehOrganization.TrafficLicenceVlidNumOfMonths))
        ValidationRules.CheckRules()
    End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of DocumentsTrafficLicence, Integer))
        Database.LogInfo("DocumentsTrafficLicence.DataPortal_Fetch", GetHashCode())
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
                        LoadProperty(Of Integer)(IdTehnicalExamOrganizationsIssuedByProperty, dr.GetInt32("IdTehnicalExamOrganizationsIssuedBy"))
                        LoadProperty(Of String)(TrafficLicenceNumberProperty, dr.GetString("TrafficLicenceNumber"))
                        LoadProperty(Of SmartDate, Date?)(MadeDateProperty, dr.GetSmartDate("MadeDate", True))
                        LoadProperty(Of SmartDate, Date?)(EndDateProperty, dr.GetSmartDate("EndDate", True))
                        LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))

                        dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
                    End Using
                End Using
                'load children
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "getDocumentsTrafficLicencesExtensionByIdTrafficLicence"
                    cm.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
                    Using drc As New SafeDataReader(cm.ExecuteReader)
                        LoadProperty(Of TrafficLicencesExtensions)(ExtensionsProperty, _
                          TrafficLicencesExtensions.GetTrafficLicencesExtensions(drc))
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("DocumentsTrafficLicence.DataPortal_Fetch", ex)
            Throw New DbCslaException("DocumentsTrafficLicence.DataPortal_Fetch", ex)
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
                        .Parameters.AddWithValue("@IdTehnicalExamOrganizationsIssuedBy", ReadProperty(Of Integer)(IdTehnicalExamOrganizationsIssuedByProperty))
                        .Parameters.AddWithValue("@TrafficLicenceNumber", ReadProperty(Of String)(TrafficLicenceNumberProperty))
                        .Parameters.AddWithValue("@MadeDate", ReadProperty(Of SmartDate)(MadeDateProperty).DBValue)
                        .Parameters.AddWithValue("@EndDate", ReadProperty(Of SmartDate)(EndDateProperty).DBValue)
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))

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
                Me.FieldManager.UpdateChildren(Me)
                'zadolzuvanje po izvrsena usluga, по IdCustomerVehicle
                Dim objCurentTehExamOrganization = CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)

                If objCurentTehExamOrganization.IdDefaultRegistrationIssuer = IdTehnicalExamOrganizationsIssuedBy Then
                    AddDeptsToCustomer(cn)
                End If

                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("DocumentsTrafficLicence.DataPortal_Insert", ex)
            Throw New DbCslaException("DocumentsTrafficLicence.DataPortal_Insert", ex)
        Finally
            Database.LogInfo("DocumentsTrafficLicence.DataPortal_Insert", GetHashCode())
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
                        .Parameters.AddWithValue("@IdTehnicalExamOrganizationsIssuedBy", ReadProperty(Of Integer)(IdTehnicalExamOrganizationsIssuedByProperty))
                        .Parameters.AddWithValue("@TrafficLicenceNumber", ReadProperty(Of String)(TrafficLicenceNumberProperty))
                        .Parameters.AddWithValue("@MadeDate", ReadProperty(Of SmartDate)(MadeDateProperty).DBValue)
                        .Parameters.AddWithValue("@EndDate", ReadProperty(Of SmartDate)(EndDateProperty).DBValue)
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
                        .Parameters.AddWithValue("@lastChanged", _lastChanged)
                        Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
                        param.Direction = ParameterDirection.Output
                        .Parameters.Add(param)

                        .ExecuteNonQuery()

                        _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
                    End With
                End Using
                'update child objects
                Me.FieldManager.UpdateChildren(Me)
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
        DataPortal_Delete(New SingleCriteria(Of DocumentsTrafficLicence, Integer)(Id))
    End Sub

    Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of DocumentsTrafficLicence, Integer))
        Database.LogInfo("DocumentsTrafficLicence.DataPortal_Delete", GetHashCode())
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
            Database.LogException("DocumentsTrafficLicence.DataPortal_Delete", ex)
            Throw New DbCslaException("DocumentsTrafficLicence.DataPortal_Delete", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

    Private Sub Child_Fetch(ByVal dr As SafeDataReader)
        Database.LogInfo("DocumentsTrafficLicence.Child_Fetch", GetHashCode())
        Try
            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of Long)(IdCustomerVehicleRelationProperty, dr.GetInt64("IdCustomerVehicleRelation"))
            LoadProperty(Of Integer)(IdTehnicalExamOrganizationsIssuedByProperty, dr.GetInt32("IdTehnicalExamOrganizationsIssuedBy"))
            LoadProperty(Of String)(TrafficLicenceNumberProperty, dr.GetString("TrafficLicenceNumber"))
            LoadProperty(Of SmartDate, Date?)(MadeDateProperty, dr.GetSmartDate("MadeDate", True))
            LoadProperty(Of SmartDate, Date?)(EndDateProperty, dr.GetSmartDate("EndDate", True))
            LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

            'load children
            Using cm As SqlCommand = CType(Csla.ApplicationContext.LocalContext("cn"), SqlConnection).CreateCommand
                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = "getDocumentsTrafficLicencesExtensionByIdTrafficLicence"
                cm.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
                Using drc As New SafeDataReader(cm.ExecuteReader)
                    LoadProperty(Of TrafficLicencesExtensions)(ExtensionsProperty, _
                      TrafficLicencesExtensions.GetTrafficLicencesExtensions(drc))
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("DocumentsTrafficLicence.Child_Fetch", ex)
            Throw New DbCslaException("DocumentsTrafficLicence.Child_Fetch", ex)
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
                        .Parameters.AddWithValue("@IdTehnicalExamOrganizationsIssuedBy", ReadProperty(Of Integer)(IdTehnicalExamOrganizationsIssuedByProperty))
                        .Parameters.AddWithValue("@TrafficLicenceNumber", ReadProperty(Of String)(TrafficLicenceNumberProperty))
                        .Parameters.AddWithValue("@MadeDate", ReadProperty(Of SmartDate)(MadeDateProperty).DBValue)
                        .Parameters.AddWithValue("@EndDate", ReadProperty(Of SmartDate)(EndDateProperty).DBValue)
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))

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
                Dim objCurentTehExamOrganization = CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)

                If objCurentTehExamOrganization.IdDefaultRegistrationIssuer = IdTehnicalExamOrganizationsIssuedBy Then
                    'zadolzuvanje po izvrsena usluga, po IdCustomerVehicle
                    Using cm1 As SqlCommand = cn.CreateCommand
                        cm1.CommandType = CommandType.StoredProcedure
                        cm1.CommandText = "insertFinancialStatePriceCatalogForTrafficLicences"
                        cm1.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                        cm1.Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdProperty))
                        cm1.Parameters.AddWithValue("@note", "по технички преглед бр." & ReadProperty(Of Long)(IdProperty))
                        cm1.ExecuteNonQuery()
                    End Using
                    Me.FieldManager.UpdateChildren(Me)
                End If
                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("DocumentsTrafficLicence.Child_Insert", ex)
            Throw New DbCslaException("DocumentsTrafficLicence.Child_Insert", ex)
        Finally
            Database.LogInfo("DocumentsTrafficLicence.Child_Insert", GetHashCode)
        End Try

    End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

    Private Sub Child_Update()
        Database.LogInfo("DocumentsTrafficLicence.Child_Update", GetHashCode)
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                ApplicationContext.LocalContext("cn") = cn
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = spUpdate

                        .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
                        .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                        .Parameters.AddWithValue("@IdTehnicalExamOrganizationsIssuedBy", ReadProperty(Of Integer)(IdTehnicalExamOrganizationsIssuedByProperty))
                        .Parameters.AddWithValue("@TrafficLicenceNumber", ReadProperty(Of String)(TrafficLicenceNumberProperty))
                        .Parameters.AddWithValue("@MadeDate", ReadProperty(Of SmartDate)(MadeDateProperty).DBValue)
                        .Parameters.AddWithValue("@EndDate", ReadProperty(Of SmartDate)(EndDateProperty).DBValue)
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
                        .Parameters.AddWithValue("@lastChanged", _lastChanged)
                        Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
                        param.Direction = ParameterDirection.Output
                        .Parameters.Add(param)

                        .ExecuteNonQuery()

                        _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
                    End With
                End Using
                'update child objects
                Me.FieldManager.UpdateChildren(Me)
                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("DocumentsTrafficLicence.Child_Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DbCslaException("DocumentsTrafficLicence.Child_Update", ex)
            End If
        End Try
    End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

    Private Sub Child_DeleteSelf()

        Database.LogInfo("DocumentsTrafficLicence.Child_DeleteSelf", GetHashCode)
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
            Database.LogException("DocumentsTrafficLicence.Child_Fetch", ex)
            Throw New DbCslaException("DocumentsTrafficLicence.Child_Fetch", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

#Region " Payment "

    Private Sub AddDeptsToCustomer(ByVal cn As SqlConnection)
        Try

            'zemi go voziloto od relacijata
            Dim tmpVehicleId As Integer = _
              CustomerVehiclesRelation.GetCustomerVehiclesRelation( _
              ReadProperty(Of Long)(IdCustomerVehicleRelationProperty)).IdVehicle
            Dim vInfo As Vehicle = Vehicle.GetVehicle(tmpVehicleId)
            Dim pCatalog As PaymentCataologList = _
             CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
             PaymentCataologList).GetPaymentForDepts("TrigerdByTrafficLicence", vInfo)

            For Each pInfo As PaymentCataologInfo In pCatalog
                'vo sluaj poedinecno(so formula)
                If (pInfo.ParametarFrom = 0) AndAlso (pInfo.ParametarTo = 0) Then


                    Using cm As SqlCommand = cn.CreateCommand
                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = "insertFinancialStatePriceCatalogForTrafficLicences"
                        cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                        cm.Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdProperty))
                        cm.Parameters.AddWithValue("@note", "по барање за сообраќајна дозвола бр." & ReadProperty(Of Long)(IdProperty))
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
                        cm.CommandText = "insertFinancialStatePriceCatalogForTrafficLicences"
                        cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                        cm.Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdProperty))
                        cm.Parameters.AddWithValue("@note", "барање за сообраќајна дозвола бр." & ReadProperty(Of Long)(IdProperty))
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
