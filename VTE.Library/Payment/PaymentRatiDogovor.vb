
<Serializable()> _
Public Class PaymentRatiDogovor
    Inherits Csla.BusinessBase(Of PaymentRatiDogovor)


#Region " Stored Procedures Names "
    Private Const spGetByID As String = "getDogovorZaRatById"
    Private Const spGetAll As String = "getDogovorZaRati"
    Private Const spUpdate As String = "updateDogovorZaRat"
    Private Const spAdd As String = "addDogovorZaRat"
    Private Const spDelete As String = "deleteDogovorZaRat"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
    'register properties
    Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(PaymentRatiDogovor), New PropertyInfo(Of Long)("Id"))
    Private Shared BrojProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(PaymentRatiDogovor), New PropertyInfo(Of String)("Broj"))
    Private Shared DatumProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(PaymentRatiDogovor), New PropertyInfo(Of SmartDate)("Datum", "Datum", New SmartDate(DateTime.Today, True)))
    Private Shared GarantNazivProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(PaymentRatiDogovor), New PropertyInfo(Of String)("GarantNaziv"))
    Private Shared GarantAdresaProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(PaymentRatiDogovor), New PropertyInfo(Of String)("GarantAdresa"))
    Private Shared GartEMBProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(PaymentRatiDogovor), New PropertyInfo(Of String)("GartEMB"))
    Private Shared BrNaRatiProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentRatiDogovor), New PropertyInfo(Of Integer)("BrNaRati"))

    Private _lastChanged(7) As Byte

    <System.ComponentModel.DataObjectField(True, True)> _
    Public ReadOnly Property Id() As Long
        Get
            Return GetProperty(Of Long)(IdProperty)
        End Get
    End Property
    Public Property Broj() As String
        Get
            Return GetProperty(Of String)(BrojProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(BrojProperty, value)
        End Set
    End Property
    Public Property Datum() As Date
        Get
            Return GetProperty(Of SmartDate, Date)(DatumProperty)
        End Get
        Set(ByVal value As Date)
            SetProperty(Of SmartDate, Date)(DatumProperty, value)
        End Set
    End Property
    Public Property GarantNaziv() As String
        Get
            Return GetProperty(Of String)(GarantNazivProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(GarantNazivProperty, value)
        End Set
    End Property
    Public Property GarantAdresa() As String
        Get
            Return GetProperty(Of String)(GarantAdresaProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(GarantAdresaProperty, value)
        End Set
    End Property
    Public Property GartEMB() As String
        Get
            Return GetProperty(Of String)(GartEMBProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(GartEMBProperty, value)
        End Set
    End Property
    Public Property BrNaRati() As Integer
        Get
            Return GetProperty(Of Integer)(BrNaRatiProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(BrNaRatiProperty, value)
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Id.ToString
    End Function
#End Region 'Business Properties and Methods

#Region " Validation Rules "
    Protected Overrides Sub AddBusinessRules()

        'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, BrojProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BrojProperty, 50))

        'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, GarantNazivProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(GarantNazivProperty, 50))

        ' ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, GarantAdresaProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(GarantAdresaProperty, 250))

        '  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, GartEMBProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(GartEMBProperty, 20))

    End Sub

#End Region ' Validation Rules

#Region " Authorization Rules "

    'Protected Overrides Sub AddAuthorizationRules()
    '    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    '    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CountryName") Then
    '        AuthorizationRules.AllowWrite("CountryName", roleName)
    '    Else
    '        AuthorizationRules.DenyWrite("CountryName", roleName)
    '    End If
    '    'AuthorizationRules.AllowWrite("CountryName")
    '    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
    '        AuthorizationRules.AllowWrite("Active", roleName)
    '    Else
    '        AuthorizationRules.DenyWrite("Active", roleName)
    '    End If
    '    'AuthorizationRules.AllowWrite("Active")
    'End Sub


    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("PaymentRatiDogovor")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("PaymentRatiDogovor")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("PaymentRatiDogovor")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("PaymentRatiDogovor")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

    Private Sub New()
        ' require use of factory method 
    End Sub

    Public Shared Function NewPaymentRatiDogovor() As PaymentRatiDogovor
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a PaymentRatiDogovor")
        End If
        Return DataPortal.Create(Of PaymentRatiDogovor)()
    End Function

    Public Shared Function GetPaymentRatiDogovor(ByVal id As Integer) As PaymentRatiDogovor
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a PaymentRatiDogovor")
        End If
        Return DataPortal.Fetch(New SingleCriteria(Of PaymentRatiDogovor, Integer)(id))
    End Function

    Public Shared Sub DeletePaymentRatiDogovor(ByVal id As Integer)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a PaymentRatiDogovor")
        End If
        DataPortal.Delete(New SingleCriteria(Of PaymentRatiDogovor, Integer)(id))
    End Sub

#End Region ' Factory Methods

#Region " Child Factory Methods "

    Friend Shared Function NewPaymentRatiDogovorChild() As PaymentRatiDogovor
        Return DataPortal.CreateChild(Of PaymentRatiDogovor)()
    End Function

    Friend Shared Function GetPaymentRatiDogovor(ByVal dr As SafeDataReader) As PaymentRatiDogovor
        Return DataPortal.FetchChild(Of PaymentRatiDogovor)(dr)
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

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of PaymentRatiDogovor, Integer))
        Database.LogInfo("PaymentRatiDogovor.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = spGetByID
                    cm.Parameters.AddWithValue("@Id", criteria.Value)
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        dr.Read()

                        LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
                        LoadProperty(Of String)(BrojProperty, dr.GetString("Broj"))
                        LoadProperty(Of String)(GarantNazivProperty, dr.GetString("GarantNaziv"))
                        LoadProperty(Of String)(GarantAdresaProperty, dr.GetString("GarantAdresa"))
                        LoadProperty(Of String)(GartEMBProperty, dr.GetString("GartEMB"))
                        LoadProperty(Of Integer)(BrNaRatiProperty, dr.GetInt32("BrNaRati"))

                        dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("PaymentRatiDogovor.DataPortal_Fetch", ex)
            Throw New DbCslaException("PaymentRatiDogovor.DataPortal_Fetch", ex)
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

                        .Parameters.AddWithValue("@Broj", ReadProperty(Of String)(BrojProperty))
                        .Parameters.AddWithValue("@Datum", ReadProperty(Of SmartDate)(DatumProperty).DBValue)
                        .Parameters.AddWithValue("@GarantNaziv", ReadProperty(Of String)(GarantNazivProperty))
                        .Parameters.AddWithValue("@GarantAdresa", ReadProperty(Of String)(GarantAdresaProperty))
                        .Parameters.AddWithValue("@GartEMB", ReadProperty(Of String)(GartEMBProperty))
                        .Parameters.AddWithValue("@BrNaRati", ReadProperty(Of Integer)(BrNaRatiProperty))

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
                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("PaymentRatiDogovor.DataPortal_Insert", ex)
            Throw New DbCslaException("PaymentRatiDogovor.DataPortal_Insert", ex)
        Finally
            Database.LogInfo("PaymentRatiDogovor.DataPortal_Insert", GetHashCode())
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

                        .Parameters.AddWithValue("@Broj", ReadProperty(Of String)(BrojProperty))
                        .Parameters.AddWithValue("@Datum", ReadProperty(Of SmartDate)(DatumProperty).DBValue)
                        .Parameters.AddWithValue("@GarantNaziv", ReadProperty(Of String)(GarantNazivProperty))
                        .Parameters.AddWithValue("@GarantAdresa", ReadProperty(Of String)(GarantAdresaProperty))
                        .Parameters.AddWithValue("@GartEMB", ReadProperty(Of String)(GartEMBProperty))
                        .Parameters.AddWithValue("@BrNaRati", ReadProperty(Of Integer)(BrNaRatiProperty))
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
            Database.LogException("PaymentRatiDogovor.DataPortal_Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DBConcurrencyException("PaymentRatiDogovor.DataPortal_Update", ex)
            End If
        End Try
    End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

    Protected Overrides Sub DataPortal_DeleteSelf()
        DataPortal_Delete(New SingleCriteria(Of PaymentRatiDogovor, Integer)(Id))
    End Sub

    Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of PaymentRatiDogovor, Integer))
        Database.LogInfo("PaymentRatiDogovor.DataPortal_Delete", GetHashCode())
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
            Database.LogException("PaymentRatiDogovor.DataPortal_Delete", ex)
            Throw New DbCslaException("PaymentRatiDogovor.DataPortal_Delete", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

    Private Sub Child_Fetch(ByVal dr As SafeDataReader)
        Database.LogInfo("PaymentRatiDogovor.Child_Fetch", GetHashCode())
        Try

            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of String)(BrojProperty, dr.GetString("Broj"))
            LoadProperty(Of String)(GarantNazivProperty, dr.GetString("GarantNaziv"))
            LoadProperty(Of String)(GarantAdresaProperty, dr.GetString("GarantAdresa"))
            LoadProperty(Of String)(GartEMBProperty, dr.GetString("GartEMB"))
            LoadProperty(Of Integer)(BrNaRatiProperty, dr.GetInt32("BrNaRati"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
        Catch ex As Exception
            Database.LogException("PaymentRatiDogovor.Child_Fetch", ex)
            Throw New DbCslaException("PaymentRatiDogovor.Child_Fetch", ex)
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
                        .Parameters.AddWithValue("@Broj", ReadProperty(Of String)(BrojProperty))
                        .Parameters.AddWithValue("@Datum", ReadProperty(Of SmartDate)(DatumProperty).DBValue)
                        .Parameters.AddWithValue("@GarantNaziv", ReadProperty(Of String)(GarantNazivProperty))
                        .Parameters.AddWithValue("@GarantAdresa", ReadProperty(Of String)(GarantAdresaProperty))
                        .Parameters.AddWithValue("@GartEMB", ReadProperty(Of String)(GartEMBProperty))
                        .Parameters.AddWithValue("@BrNaRati", ReadProperty(Of Integer)(BrNaRatiProperty))
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
                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("PaymentRatiDogovor.Child_Insert", ex)
            Throw New DbCslaException("PaymentRatiDogovor.Child_Insert", ex)
        Finally
            Database.LogInfo("PaymentRatiDogovor.Child_Insert", GetHashCode)
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

                        .Parameters.AddWithValue("@Broj", ReadProperty(Of String)(BrojProperty))
                        .Parameters.AddWithValue("@Datum", ReadProperty(Of SmartDate)(DatumProperty).DBValue)
                        .Parameters.AddWithValue("@GarantNaziv", ReadProperty(Of String)(GarantNazivProperty))
                        .Parameters.AddWithValue("@GarantAdresa", ReadProperty(Of String)(GarantAdresaProperty))
                        .Parameters.AddWithValue("@GartEMB", ReadProperty(Of String)(GartEMBProperty))
                        .Parameters.AddWithValue("@BrNaRati", ReadProperty(Of Integer)(BrNaRatiProperty))
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
            Database.LogException("PaymentRatiDogovor.Child_Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DbCslaException("PaymentRatiDogovor.Child_Update", ex)
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
                        .Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
                        .ExecuteNonQuery()
                    End With
                End Using

            End Using
        Catch ex As Exception
            Database.LogException("PaymentRatiDogovor.Child_Fetch", ex)
            Throw New DbCslaException("PaymentRatiDogovor.Child_Fetch", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access
End Class
