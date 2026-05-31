Imports Csla.Validation.CommonRules

<Serializable()> _
Public Class PaymentDocumentsDetail
    Inherits Csla.BusinessBase(Of PaymentDocumentsDetail)

#Region " Stored Procedures Names "
    Private Const spGetByID As String = "GetPaymentDocumentsDetailByID"
    Private Const spGetAll As String = "GetPaymentDocumentsDetails"
    Private Const spUpdate As String = "updatePaymentDocumentsDetail"
    Private Const spAdd As String = "addPaymentDocumentsDetail"
    Private Const spDelete As String = "deletePaymentDocumentsDetail"

#End Region 'Stored Procedures Names

#Region " Calculated Properties "
    Public ReadOnly Property Total() As Double
        Get
            Return ReadProperty(Of Decimal)(PriceProperty) * (1 - ReadProperty(Of Single)(DiscountProperty) / 100)
        End Get
    End Property
    Public ReadOnly Property VisibleOrder() As Integer
        Get
            If IdPriceCatalog <> 0 Then
                Dim ord As Integer = _
                CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
                PaymentCataologList).GetInfoByIdPaymentParametar(IdPriceCatalog).VisibleOrder
                Return ord
            Else
                Return 0
            End If
        End Get
    End Property
#End Region

#Region " Business Properties and Methods "
    'register properties
    Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(PaymentDocumentsDetail), New PropertyInfo(Of Long)("Id"))
    Private Shared IdPaymentDocumentsProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(PaymentDocumentsDetail), New PropertyInfo(Of Long)("IdPaymentDocuments"))
    Private Shared IdPriceCatalogProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentDocumentsDetail), New PropertyInfo(Of Integer)("IdPriceCatalog"))
    Private Shared PriceProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(PaymentDocumentsDetail), New PropertyInfo(Of Decimal)("Price"))
    Private Shared DdvProperty As PropertyInfo(Of Single) = RegisterProperty(Of Single)(GetType(PaymentDocumentsDetail), New PropertyInfo(Of Single)("Ddv"))
    Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(PaymentDocumentsDetail), New PropertyInfo(Of String)("Note"))
    Private Shared PrePayedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentDocumentsDetail), New PropertyInfo(Of Boolean)("PrePayed"))
    Private Shared NotePrePayedProperty As PropertyInfo(Of String) = _
    RegisterProperty(Of String)(GetType(PaymentDocumentsDetail), New PropertyInfo(Of String)("NotePrePayed"))
    Private Shared DiscountProperty As PropertyInfo(Of Single) = _
    RegisterProperty(Of Single)(GetType(PaymentDocumentsDetail), New PropertyInfo(Of Single)("Discount"))
    Private Shared IdCustomerFinancialStateProperty As PropertyInfo(Of Long) = _
    RegisterProperty(Of Long)(GetType(PaymentDocumentsDetail), New PropertyInfo(Of Long)("IdCustomerFinancialState"))
    Private _lastChanged(7) As Byte

    <System.ComponentModel.DataObjectField(True, True)> _
    Public ReadOnly Property Id() As Long
        Get
            Return GetProperty(Of Long)(IdProperty)
        End Get
    End Property
    Public Property IdPaymentDocuments() As Long
        Get
            Return GetProperty(Of Long)(IdPaymentDocumentsProperty)
        End Get
        Set(ByVal value As Long)
            SetProperty(Of Long)(IdPaymentDocumentsProperty, value)
        End Set
    End Property
    Public Property IdPriceCatalog() As Integer
        Get

            Return GetProperty(Of Integer)(IdPriceCatalogProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdPriceCatalogProperty, value)
        End Set
    End Property
    Public Property Price() As Decimal
        Get
            Return FicalRound(GetProperty(Of Decimal)(PriceProperty))
        End Get
        Set(ByVal value As Decimal)
            SetProperty(Of Decimal)(PriceProperty, FicalRound(value))
        End Set
    End Property
    Public Property PriceWithoutTax() As Decimal
        Get
            Dim DDV As Decimal = GetProperty(Of Single)(DdvProperty)
            Return FicalRound(DDVPresmetki.DanocnaOsnovica(GetProperty(Of Decimal)(PriceProperty), DDV, 1)) '(Math.Round(DDVPresmetki.DanocnaOsnovica(GetProperty(Of Decimal)(PriceProperty), DDV, 1), 2))
        End Get
        Set(ByVal value As Decimal)
            Dim DDV As Decimal = GetProperty(Of Single)(DdvProperty)
            Dim vrednost As Decimal = FicalRound(value * (1 + DDV / 100)) 'Math.Round(value * (1 + DDV / 100), 2)
            SetProperty(Of Decimal)(PriceProperty, FicalRound(vrednost))
        End Set
    End Property

    Public Property Ddv() As Single
        Get
            Return GetProperty(Of Single)(DdvProperty)
        End Get
        Set(ByVal value As Single)
            SetProperty(Of Single)(DdvProperty, value)
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
    Public Property PrePayed() As Boolean
        Get
            Return GetProperty(Of Boolean)(PrePayedProperty)
        End Get
        Set(ByVal value As Boolean)
            SetProperty(Of Boolean)(PrePayedProperty, value)
        End Set
    End Property
    Public Property NotePrePayed() As String
        Get
            Return GetProperty(Of String)(NotePrePayedProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(NotePrePayedProperty, value)
        End Set
    End Property

    Public Property Discount() As Single
        Get
            Return GetProperty(Of Single)(DiscountProperty)
        End Get
        Set(ByVal value As Single)
            Dim pom As PaymentCataologInfo = PaymentCataologList.GetPaymentCataologList.GetInfoByIdPaymentParametar(IdPriceCatalog)
            If Not pom.AllowDiscount Then
                SetProperty(Of Single)(DiscountProperty, 0)
            Else
                SetProperty(Of Single)(DiscountProperty, value)
            End If

        End Set
    End Property
    Public Property IdCustomerFinancialState() As Long
        Get
            Return GetProperty(Of Long)(IdCustomerFinancialStateProperty)
        End Get
        Set(ByVal value As Long)
            SetProperty(Of Long)(IdCustomerFinancialStateProperty, value)
        End Set
    End Property
    Public Overrides Function ToString() As String
        Return Id.ToString
    End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdPaymentDocuments") Then
            AuthorizationRules.AllowWrite("IdPaymentDocuments", roleName)
        Else
            AuthorizationRules.DenyWrite("IdPaymentDocuments", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdPriceCatalog") Then
            AuthorizationRules.AllowWrite("IdPriceCatalog", roleName)
        Else
            AuthorizationRules.DenyWrite("IdPriceCatalog", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Price") Then
            AuthorizationRules.AllowWrite("Price", roleName)
        Else
            AuthorizationRules.DenyWrite("Price", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Ddv") Then
            AuthorizationRules.AllowWrite("Ddv", roleName)
        Else
            AuthorizationRules.DenyWrite("Ddv", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Note") Then
            AuthorizationRules.AllowWrite("Note", roleName)
        Else
            AuthorizationRules.DenyWrite("Note", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PrePayed") Then
            AuthorizationRules.AllowWrite("PrePayed", roleName)
        Else
            AuthorizationRules.DenyWrite("PrePayed", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NotePrePayed") Then
            AuthorizationRules.AllowWrite("NotePrePayed", roleName)
        Else
            AuthorizationRules.DenyWrite("NotePrePayed", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Discount") Then
            AuthorizationRules.AllowWrite("Discount", roleName)
        Else
            AuthorizationRules.DenyWrite("Discount", roleName)
        End If

    End Sub

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("PaymentDocumentsDetail")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("PaymentDocumentsDetail")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("PaymentDocumentsDetail")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("PaymentDocumentsDetail")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
    Protected Overrides Sub AddBusinessRules()
        ' NoteProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NoteProperty, 150))
        ' NotePrePayedProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NotePrePayedProperty, 150))

        'ValidationRules.AddRule(Of PaymentDocumentsDetail)(AddressOf NoteRequired, NotePrePayedProperty)
        'ValidationRules.AddDependentProperty(NotePrePayedProperty, PrePayedProperty, True)


        ValidationRules.AddRule(Of PaymentDocumentsDetail)(AddressOf MaxDiscount, DiscountProperty)

        ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                                New IntegerMinValueRuleArgs(IdPriceCatalogProperty, 1))

    End Sub

    Private Shared Function NoteRequired(Of T As PaymentDocumentsDetail)(ByVal target As T, _
      ByVal e As Csla.Validation.RuleArgs) As Boolean
        If target.PrePayed AndAlso (target.NotePrePayed = String.Empty Or target.NotePrePayed = "") Then
            e.Description = "Внесете доказ за плаќањето"
            Return False
        Else
            Return True
        End If
    End Function


    Private Shared Function MaxDiscount(Of T As PaymentDocumentsDetail)(ByVal target As T, _
      ByVal e As Csla.Validation.RuleArgs) As Boolean
        If target.Discount >= 100 Then
            e.Description = "Процентот на попустот мора да е помал од 100"
            Return False
        Else
            Return True
        End If
    End Function
#End Region ' Validation Rules

#Region " Factory Methods "

    Friend Shared Function NewPaymentDocumentsDetailChild() As PaymentDocumentsDetail
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a PaymentDocumentsDetail")
        'End If
        Return DataPortal.CreateChild(Of PaymentDocumentsDetail)()
    End Function

    Friend Shared Function GetPaymentDocumentsDetail(ByVal dr As SafeDataReader) As PaymentDocumentsDetail
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a PaymentDocumentsDetail")
        'End If
        Return DataPortal.FetchChild(Of PaymentDocumentsDetail)(dr)
    End Function

    Public Shared Sub DeletePaymentCategorie(ByVal id As Integer)
        'If Not CanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a PaymentDocumentsDetail")
        'End If
        DataPortal.Delete(New SingleCriteria(Of PaymentDocumentsDetail, Integer)(id))
    End Sub

    Public Overrides Function Save() As PaymentDocumentsDetail
        'If IsDeleted AndAlso Not CanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a PaymentDocumentsDetail")
        'ElseIf IsNew AndAlso Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a PaymentDocumentsDetail")
        'ElseIf Not CanEditObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to update a PaymentDocumentsDetail")
        'End If
        Return MyBase.Save()
    End Function

    Private Sub New()
    End Sub

#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Create "
    <RunLocal()> _
    Protected Overloads Sub Child_Create()
        ValidationRules.CheckRules()
    End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

    Private Sub Child_Fetch(ByVal dr As SafeDataReader)
        Database.LogInfo("PaymentDocumentsDetail.Child_Fetch", GetHashCode())
        Try

            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of Long)(IdPaymentDocumentsProperty, dr.GetInt64("IdPaymentDocuments"))
            LoadProperty(Of Integer)(IdPriceCatalogProperty, dr.GetInt32("IdPriceCatalog"))
            LoadProperty(Of Decimal)(PriceProperty, dr.GetDecimal("Price"))
            LoadProperty(Of Single)(DdvProperty, dr.GetValue("DDV"))
            LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
            LoadProperty(Of Boolean)(PrePayedProperty, dr.GetBoolean("PrePayed"))
            LoadProperty(Of String)(NotePrePayedProperty, dr.GetString("NotePrePayed"))
            LoadProperty(Of Single)(DiscountProperty, dr.GetValue("Discount"))
            LoadProperty(Of Long)(IdCustomerFinancialStateProperty, dr.GetInt64("IdCustomerFinancialState"))
            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

            ValidationRules.CheckRules()

        Catch ex As Exception
            Database.LogException("PaymentDocumentsDetail.Child_Fetch", ex)
            Throw New DbCslaException("PaymentDocumentsDetail.Child_Fetch", ex)
        End Try

    End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

    Private Sub Child_Insert(ByVal parent As PaymentDocument)
        Try
            Using cn As SqlConnection = ApplicationContext.LocalContext("cn")
                If cn.State = ConnectionState.Closed Then
                    cn.ConnectionString = Database.VTEConnection
                    cn.Open()
                End If

                'razdolzi ja kasata na customerot za ovoj detal ako go ima
                If Not parent.Storno Then


                    Dim priceInfo As PaymentCataologInfo = _
                    CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"), PaymentCataologList) _
                    .GetInfo(ReadProperty(Of Integer)(IdPriceCatalogProperty))
                    'provrti dali e aritkl so koj se zadolzuva komitentot
                    If (priceInfo.TrigerdByInternationalDrivierLicence Or _
                       priceInfo.TrigerdByPremisionForVehicle Or _
                       priceInfo.TrigerdByRequest Or _
                       priceInfo.TrigerdByTechnicalExam Or _
                       priceInfo.TrigerdByTrafficLicence) Or _
                       priceInfo.TrigerdByIrregularTechnicalExam Then
                        '  And _
                        '(Not ReadProperty(Of Boolean)(PrePayedProperty)) Then

                        Using cm As SqlCommand = cn.CreateCommand

                            Try
                                cm.CommandType = CommandType.StoredProcedure
                                cm.CommandText = "addPaymentDocumentsDetailFinace"
                                cm.Parameters.AddWithValue("@idPriceCatalog", ReadProperty(Of Integer)(IdPriceCatalogProperty))
                                cm.Parameters.AddWithValue("@idCustomerVehicleRelation", parent.IdCustomerVehicleRelation)
                                cm.ExecuteNonQuery()
                            Catch ex As Exception

                            End Try

                        End Using
                    End If

                End If

                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = spAdd
                        'Smeni go Id so Parent.Id
                        .Parameters.AddWithValue("@IdPaymentDocuments", parent.Id)
                        .Parameters.AddWithValue("@IdPriceCatalog", ReadProperty(Of Integer)(IdPriceCatalogProperty))
                        .Parameters.AddWithValue("@Price", ReadProperty(Of Decimal)(PriceProperty))
                        .Parameters.AddWithValue("@DDV", ReadProperty(Of Single)(DdvProperty))
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
                        .Parameters.AddWithValue("@PrePayed", ReadProperty(Of Boolean)(PrePayedProperty))
                        .Parameters.AddWithValue("@NotePrePayed", ReadProperty(Of String)(NotePrePayedProperty))
                        .Parameters.AddWithValue("Discount", ReadProperty(Of Single)(DiscountProperty))
                        .Parameters.AddWithValue("IdCustomerFinancialState", ReadProperty(Of Long)(IdCustomerFinancialStateProperty))

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
            Database.LogException("PaymentDocumentsDetail.Child_Insert", ex)
            Throw New DbCslaException("PaymentDocumentsDetail.Child_Insert", ex)
        Finally
            Database.LogInfo("PaymentDocumentsDetail.Child_Insert", GetHashCode)
        End Try

    End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

    Private Sub Child_Update(ByVal parent As PaymentDocument)
        Database.LogInfo("PaymentDocumentsDetail.Child_Update", GetHashCode)
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
                        .Parameters.AddWithValue("@IdPaymentDocuments", parent.Id)
                        .Parameters.AddWithValue("@IdPriceCatalog", ReadProperty(Of Integer)(IdPriceCatalogProperty))
                        .Parameters.AddWithValue("@Price", ReadProperty(Of Decimal)(PriceProperty))
                        .Parameters.AddWithValue("@DDV", ReadProperty(Of Single)(DdvProperty))
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
                        .Parameters.AddWithValue("@PrePayed", ReadProperty(Of Boolean)(PrePayedProperty))
                        .Parameters.AddWithValue("@NotePrePayed", ReadProperty(Of String)(NotePrePayedProperty))
                        .Parameters.AddWithValue("Discount", ReadProperty(Of Single)(DiscountProperty))
                        .Parameters.AddWithValue("IdCustomerFinancialState", ReadProperty(Of Long)(IdCustomerFinancialStateProperty))
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
            Database.LogException("PaymentDocumentsDetail.Child_Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DbCslaException("PaymentDocumentsDetail.Child_Update", ex)
            End If
        End Try
    End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

    Private Sub Child_DeleteSelf()

        Database.LogInfo("PaymentDocumentsDetail.Child_DeleteSelf", GetHashCode)
        Try
            Using cn As SqlConnection = Csla.ApplicationContext.LocalContext("cn")
                If cn.State = ConnectionState.Closed Then
                    cn.ConnectionString = Database.VTEConnection
                    cn.Open()
                End If

                Dim priceInfo As PaymentCataologInfo = _
                CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"), PaymentCataologList) _
                .GetInfo(ReadProperty(Of Integer)(IdPriceCatalogProperty))
                'provrti dali e aritkl so koj se zadolzuva komitentot
                If (priceInfo.TrigerdByInternationalDrivierLicence Or _
                   priceInfo.TrigerdByPremisionForVehicle Or _
                   priceInfo.TrigerdByRequest Or _
                   priceInfo.TrigerdByTechnicalExam Or _
                   priceInfo.TrigerdByTrafficLicence) Then
                    'And _
                    '(Not ReadProperty(Of Boolean)(PrePayedProperty)) Then

                    'zadolzi ja kasata na customerot za ovoj detal ako go ima
                    Using cm As SqlCommand = cn.CreateCommand
                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = "updatePaymentDocumentsDetailFinace"
                        cm.Parameters.AddWithValue("@idPriceCatalog", ReadProperty(Of Integer)(IdPriceCatalogProperty))
                        cm.Parameters.AddWithValue("@idCustomerVehicleRelation", ApplicationContext.LocalContext("tmpIdCustomerVehicleRelation"))
                        cm.ExecuteNonQuery()
                    End Using

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
            Database.LogException("PaymentDocumentsDetail.Child_Fetch", ex)
            Throw New DbCslaException("PaymentDocumentsDetail.Child_Fetch", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access
#Region "ReturnFinancialState"
    Public Sub VratiZadolzi()
        Using cn As SqlConnection = Database.VTE_SqlConnection
            ApplicationContext.LocalContext("cn") = cn
            Using cm As SqlCommand = cn.CreateCommand
                With cm
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "VratiFinansiskiZadolzuvanja"
                    .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdCustomerFinancialStateProperty))
                    .ExecuteNonQuery()
                End With
            End Using
        End Using

    End Sub
#End Region


End Class
