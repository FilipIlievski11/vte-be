
<Serializable()> _
Public Class PaymentDocument
    Inherits Csla.BusinessBase(Of PaymentDocument)


#Region " Stored Procedures Names "
    Private Const spGetByID As String = "GetPaymentDocumentByID"
    Private Const spGetAll As String = "GetPaymentDocuments"
    Private Const spUpdate As String = "updatePaymentDocument"
    Private Const spAdd As String = "addPaymentDocument"
    Private Const spDelete As String = "deletePaymentDocument"
    Private Const spGetChildDetails As String = "getPaymentDocumentsDetailByIdPaymentDocuments"
    Private Const spGetChildRati As String = "getPaymentDocumentsRatByIdPaymentDocument"
    Private Const SpGetNumberForPaymentDocument As String = "getPaymentDocumentsNumberByIdStationAndIdTypeOfPayment"
#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
    'register properties
    Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(PaymentDocument), New PropertyInfo(Of Long)("Id"))
    Private Shared IdPaymentTypeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentDocument), New PropertyInfo(Of Integer)("IdPaymentType"))
    Private Shared IdCustomerVehicleRelationProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(PaymentDocument), New PropertyInfo(Of Long)("IdCustomerVehicleRelation"))
    Private Shared IdOperatorProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentDocument), New PropertyInfo(Of Integer)("IdOperator"))
    Private Shared DocumentNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(PaymentDocument), New PropertyInfo(Of String)("DocumentNumber"))
    Private Shared DatePayProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(PaymentDocument), New PropertyInfo(Of SmartDate)("DatePay", "DatePay", New SmartDate(DateTime.Now, True)))
    Private Shared DateRequiredProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(PaymentDocument), New PropertyInfo(Of SmartDate)("DateRequired", "DateRequired", New SmartDate(Now.Date, True)))
    Private Shared DiscountProperty As PropertyInfo(Of Single) = RegisterProperty(Of Single)(GetType(PaymentDocument), New PropertyInfo(Of Single)("Discount"))
    Private Shared PayedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentDocument), New PropertyInfo(Of Boolean)("Payed", "Payed", False))
    Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(PaymentDocument), New PropertyInfo(Of String)("Note"))
    Private Shared StornoProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentDocument), New PropertyInfo(Of Boolean)("Storno", "Storno", False))
    Private Shared IdDogovorProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(PaymentDocument), New PropertyInfo(Of Long)("IdDogovor"))
    Private Shared IdOrganizationProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentDocument), New PropertyInfo(Of Integer)("IdOrganization"))
    Private Shared PolisaProperty As PropertyInfo(Of Double) = RegisterProperty(Of Double)(GetType(PaymentDocument), New PropertyInfo(Of Double)("Polisa"))
    Private Shared IdFakturiraNaProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentDocument), New PropertyInfo(Of Integer)("IdFakturiraNa"))
    Private _lastChanged(7) As Byte


    Private Shared PaymentDocumentDetailsProperty As PropertyInfo(Of PaymentDocumentsDetails) = _
   RegisterProperty(Of PaymentDocumentsDetails)(GetType(PaymentDocument), New PropertyInfo(Of PaymentDocumentsDetails)("PaymentDocumentDetails"))

    Private Shared PaymentDocumentRatiProperty As PropertyInfo(Of PaymentDocumentsRati) = _
   RegisterProperty(Of PaymentDocumentsRati)(GetType(PaymentDocument), New PropertyInfo(Of PaymentDocumentsRati)("PaymentDocumentRati"))


    <System.ComponentModel.DataObjectField(True, True)> _
   Public ReadOnly Property Id() As Long
        Get
            Return GetProperty(Of Long)(IdProperty)
        End Get
    End Property
    Public Property DocumentNumber() As String
        Get
            Return GetProperty(Of String)(DocumentNumberProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(DocumentNumberProperty, value)
        End Set
    End Property
    Public ReadOnly Property PaymentDocumentDetails() As PaymentDocumentsDetails
        Get
            If Not FieldManager.FieldExists(PaymentDocumentDetailsProperty) Then
                SetProperty(Of PaymentDocumentsDetails) _
                (PaymentDocumentDetailsProperty, PaymentDocumentsDetails.NewPaymentDocumentsDetails)
            End If
            Return GetProperty(Of PaymentDocumentsDetails)(PaymentDocumentDetailsProperty)
        End Get
    End Property
    Public ReadOnly Property PaymentDocumentRati() As PaymentDocumentsRati
        Get
            If Not FieldManager.FieldExists(PaymentDocumentRatiProperty) Then
                SetProperty(Of PaymentDocumentsRati) _
                (PaymentDocumentRatiProperty, PaymentDocumentsRati.NewPaymentDocumentsRati)
            End If
            Return GetProperty(Of PaymentDocumentsRati)(PaymentDocumentRatiProperty)
        End Get
    End Property
    Public Property IdPaymentType() As Integer
        Get
            Return GetProperty(Of Integer)(IdPaymentTypeProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdPaymentTypeProperty, value)
        End Set
    End Property
    Public Property IdCustomerVehicleRelation() As Long
        Get
            Return GetProperty(Of Long)(IdCustomerVehicleRelationProperty)
        End Get
        Set(ByVal value As Long)
            SetProperty(Of Long)(IdCustomerVehicleRelationProperty, value)
        End Set
    End Property
    Public Property IdDogovor() As Long
        Get
            Return GetProperty(Of Long)(IdDogovorProperty)
        End Get
        Set(ByVal value As Long)
            SetProperty(Of Long)(IdDogovorProperty, value)
        End Set
    End Property

    Public Property IdOperator() As Integer
        Get
            Return GetProperty(Of Integer)(IdOperatorProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdOperatorProperty, value)
        End Set
    End Property
    Public Property DatePay() As Date
        Get
            Return GetProperty(Of SmartDate, Date)(DatePayProperty)
        End Get
        Set(ByVal value As Date)
            SetProperty(Of SmartDate, Date)(DatePayProperty, value)
        End Set
    End Property
    Public Property DateRequired() As Date
        Get
            Return GetProperty(Of SmartDate, Date)(DateRequiredProperty)
        End Get
        Set(ByVal value As Date)
            SetProperty(Of SmartDate, Date)(DateRequiredProperty, value)
        End Set
    End Property
    Public Property Discount() As Single
        Get
            Return GetProperty(Of Single)(DiscountProperty)
        End Get
        Set(ByVal value As Single)
            SetProperty(Of Single)(DiscountProperty, value)
        End Set
    End Property
    Public Property Payed() As Boolean
        Get
            Return GetProperty(Of Boolean)(PayedProperty)
        End Get
        Set(ByVal value As Boolean)
            SetProperty(Of Boolean)(PayedProperty, value)
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
    Public Property Storno() As Boolean
        Get
            Return GetProperty(Of Boolean)(StornoProperty)
        End Get
        Set(ByVal value As Boolean)
            SetProperty(Of Boolean)(StornoProperty, value)
        End Set
    End Property

    Public Property IdOrganization() As Integer
        Get
            Return GetProperty(Of Integer)(IdOrganizationProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdOrganizationProperty, value)
        End Set
    End Property

    Public Property Polisa() As Double
        Get
            Return GetProperty(Of Double)(PolisaProperty)
        End Get
        Set(ByVal value As Double)
            SetProperty(Of Double)(PolisaProperty, value)
            '  Me.PaymentDocumentDetails.RaiseListChangedEvents = True
        End Set
    End Property
    Public Property IdFakturiraNa() As Integer
        Get
            Return GetProperty(Of Integer)(IdFakturiraNaProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdFakturiraNaProperty, value)
        End Set
    End Property

    Private _vkupno As Decimal = 0
    Public ReadOnly Property Vkupno() As Decimal
        Get
            Return _vkupno
        End Get
    End Property
    Public ReadOnly Property ProveriVkupno() As Decimal
        Get
            Dim sumDetailPrices As Decimal = 0
            For Each item As PaymentDocumentsDetail In Me.PaymentDocumentDetails
                If item.PrePayed = False Then
                    sumDetailPrices += item.Price '+ item.Ddv * item.Price / 100
                End If
            Next
            If Discount > 0 Then
                _vkupno = FicalRound(sumDetailPrices * (1 - Discount / 100))
            Else
                _vkupno = FicalRound(sumDetailPrices)
            End If
            Return _vkupno
        End Get
    End Property
    Private Sub PaymentDocument_ChildChanged(ByVal sender As Object, ByVal e As Csla.Core.ChildChangedEventArgs) Handles Me.ChildChanged

        Dim sumDetailPrices As Decimal = 0
        For Each item As PaymentDocumentsDetail In Me.PaymentDocumentDetails
            If item.PrePayed = False Then
                sumDetailPrices += item.Price * (1 - item.Discount / 100)
            End If
        Next
        If Discount > 0 Then
            _vkupno = FicalRound(sumDetailPrices * (1 - Discount / 100))
        Else
            _vkupno = FicalRound(sumDetailPrices)
        End If
        Try
            PropertyHasChanged("Vkupno")
        Catch ex As Exception

        End Try


        If e.ChildObject Is PaymentDocumentDetails AndAlso Me.IsNew Then
            Dim rata As PaymentDocumentsRata
            If Me.PaymentDocumentRati.Count > 0 Then
                rata = Me.PaymentDocumentRati.Item(0)
                rata.Price = _vkupno
                rata.DatePayed = Now
            Else
                rata = Me.PaymentDocumentRati.AddNew
                rata.Price = _vkupno
                rata.DatePayed = Now
            End If

        End If
        'If e.ChildObject Is PaymentDocumentRati Then
        '    Dim plateniRati As Decimal = 0
        '    Try
        '        For Each rat As PaymentDocumentsRata In Me.PaymentDocumentRati
        '            If rat.Payed Then
        '                plateniRati += rat.Price
        '            End If
        '        Next
        '    Catch ex As Exception
        '    End Try

        '    If Me.PaymentDocumentRati.Count > 1 AndAlso Vkupno <= plateniRati Then
        '        Me.Payed = True
        '    Else
        '        Me.Payed = False

        '    End If

        'End If
        ' ''If PaymentTypeList.GetPaymentTypeList.GetPaymentTypeInfoById(Me.IdPaymentType).Faktura Then
        ' ''  Me.Payed = False
        ' ''End If
    End Sub


    Public Overrides Function ToString() As String
        Return Id.ToString
    End Function

    Public Sub CreateDetails(ByVal intIdRelation As Integer)
        'iscisti stari detali
        Me.PaymentDocumentDetails.Clear()

        Dim customerFinasii As CustomerFinancialStateList = _
        CustomerFinancialStateList.GetCustomerFinancialStateListByIdCustomerVehicleRelation(intIdRelation)

        For Each childIn As CustomerFinancialStateInfo In customerFinasii
            If Not childIn.Payed Then
                Dim childDetal As PaymentDocumentsDetail = Me.PaymentDocumentDetails.AddNew

                With childDetal

                    .Ddv = childIn.DDVValue
                    .IdPriceCatalog = childIn.IdPriceCatalog
                    .Price = childIn.Price
                    .IdCustomerFinancialState = childIn.Id

                End With
            End If
        Next


    End Sub

#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdPaymentType") Then
            AuthorizationRules.AllowWrite("IdPaymentType", roleName)
        Else
            AuthorizationRules.DenyWrite("IdPaymentType", roleName)
        End If
        'AuthorizationRules.AllowWrite("IdPaymentType")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCustomerVehicleRelation") Then
            AuthorizationRules.AllowWrite("IdCustomerVehicleRelation", roleName)
        Else
            AuthorizationRules.DenyWrite("IdCustomerVehicleRelation", roleName)
        End If
        'AuthorizationRules.AllowWrite("IdCustomerVehicleRelation")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdOperator") Then
            AuthorizationRules.AllowWrite("IdOperator", roleName)
        Else
            AuthorizationRules.DenyWrite("IdOperator", roleName)
        End If
        'AuthorizationRules.AllowWrite("IdOperator")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DatePay") Then
            AuthorizationRules.AllowWrite("DatePay", roleName)
        Else
            AuthorizationRules.DenyWrite("DatePay", roleName)
        End If
        'AuthorizationRules.AllowWrite("DatePay")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DateRequired") Then
            AuthorizationRules.AllowWrite("DateRequired", roleName)
        Else
            AuthorizationRules.DenyWrite("DateRequired", roleName)
        End If
        'AuthorizationRules.AllowWrite("DateRequired")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Discount") Then
            AuthorizationRules.AllowWrite("Discount", roleName)
        Else
            AuthorizationRules.DenyWrite("Discount", roleName)
        End If
        'AuthorizationRules.AllowWrite("Discount")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Payed") Then
            AuthorizationRules.AllowWrite("Payed", roleName)
        Else
            AuthorizationRules.DenyWrite("Payed", roleName)
        End If
        'AuthorizationRules.AllowWrite("Payed")
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
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("PaymentDocument")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("PaymentDocument")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("PaymentDocument")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("PaymentDocument")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
    Protected Overrides Sub AddBusinessRules()
        ' DatePayProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, DatePayProperty)
        ' DateRequiredProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, DateRequiredProperty)
        ' NoteProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NoteProperty, 150))

        ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                                New Csla.Validation.IntegerMinValueRuleArgs(IdCustomerVehicleRelationProperty, 1))
        ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                                New Csla.Validation.IntegerMinValueRuleArgs(IdPaymentTypeProperty, 1))
        ValidationRules.AddRule(Of PaymentDocument)(AddressOf CheckChild, PaymentDocumentDetailsProperty)
        ValidationRules.AddRule(Of PaymentDocument)(AddressOf CheckChild, PaymentDocumentRatiProperty)
    End Sub
    Private Shared Function CheckChild(Of T As PaymentDocument)(ByVal target As T, _
   ByVal e As Csla.Validation.RuleArgs) As Boolean
        Dim vkupnoRati As Decimal = 0
        Dim vkupnoDetali As Decimal = 0
        For Each rata As PaymentDocumentsRata In target.PaymentDocumentRati
            vkupnoRati += rata.Price
        Next
        For Each detal As PaymentDocumentsDetail In target.PaymentDocumentDetails
            vkupnoDetali += detal.Price
        Next
        If vkupnoRati > vkupnoDetali Then
            e.Description = "Збирот на ратите не смее да биде поголем од вкупната сметка"
            Return False
        End If
        Return True
    End Function
#End Region ' Validation Rules

#Region " Factory Methods "

    Private Sub New()
        ' require use of factory method 
    End Sub

    Public Shared Function NewPaymentDocument() As PaymentDocument
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a PaymentDocument")
        End If
        Return DataPortal.Create(Of PaymentDocument)()
    End Function

    Public Shared Function GetPaymentDocument(ByVal id As Long) As PaymentDocument
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a PaymentDocument")
        End If
        Return DataPortal.Fetch(New SingleCriteria(Of PaymentDocument, Integer)(id))
    End Function
    Public Shared Function GetPaymentDocumentByDocNum(ByVal inDocNum As String) As PaymentDocument
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a PaymentDocument")
        End If
        Return DataPortal.Fetch(New CriteriaByDocNum(inDocNum))
    End Function

    Public Shared Sub DeletePaymentDocument(ByVal id As Long)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a PaymentDocument")
        End If
        DataPortal.Delete(New SingleCriteria(Of PaymentDocument, Integer)(id))
    End Sub

    Public Overrides Function Save() As PaymentDocument
        If IsDeleted AndAlso Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a PaymentDocument")
        ElseIf IsNew AndAlso Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a PaymentDocument")
        ElseIf Not CanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a PaymentDocument")
        End If
        Dim result As PaymentDocument = MyBase.Save

        OnPaymentDocumentSaved(Me, New Csla.Core.SavedEventArgs(result))

        Return result
    End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

    Friend Shared Function NewPaymentDocumentChild() As PaymentDocument
        Return DataPortal.CreateChild(Of PaymentDocument)()
    End Function

    Friend Shared Function GetPaymentDocument(ByVal dr As SafeDataReader) As PaymentDocument
        Return DataPortal.FetchChild(Of PaymentDocument)(dr)
    End Function


#End Region 'Child Factory Methods

    'Private Sub PaymentDocument_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
    '    Select Case e.PropertyName
    '        Case "IdPaymentType"
    '            If Me.IdPaymentType > 0 Then
    '                SetProperty(Of String)(DocumentNumberProperty, GetDocumentNumber(Me.IdPaymentType))
    '            End If
    '        Case ""
    '    End Select
    'End Sub

#Region " Data Access "
    <Serializable()> _
   Private Class CriteriaByDocNum

        Public DocNum As String

        Public Sub New(ByVal DocNum As String)

            Me.DocNum = DocNum
        End Sub
    End Class
#Region " Data Access - Create "

    <RunLocal()> _
    Private Overloads Sub DataPortal_Create()
        LoadProperty(Of Integer)(IdOperatorProperty, CInt(Csla.ApplicationContext.LocalContext("EmployeeID")))

        'Me.IdPaymentType = 1
        ValidationRules.CheckRules()

    End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of PaymentDocument, Integer))
        Database.LogInfo("PaymentDocument.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = spGetByID
                    cm.Parameters.AddWithValue("@Id", criteria.Value)
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        dr.Read()

                        LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
                        LoadProperty(Of Integer)(IdPaymentTypeProperty, dr.GetInt32("IdPaymentType"))
                        LoadProperty(Of Long)(IdCustomerVehicleRelationProperty, dr.GetInt64("IdCustomerVehicleRelation"))
                        LoadProperty(Of Integer)(IdOperatorProperty, dr.GetInt32("IdOperator"))
                        LoadProperty(Of SmartDate, Date?)(DatePayProperty, dr.GetSmartDate("DatePay", True))
                        LoadProperty(Of SmartDate, Date?)(DateRequiredProperty, dr.GetSmartDate("DateRequired", True))
                        LoadProperty(Of Single)(DiscountProperty, dr.GetValue("Discount"))
                        LoadProperty(Of Boolean)(PayedProperty, dr.GetBoolean("Payed"))
                        LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
                        LoadProperty(Of Boolean)(StornoProperty, dr.GetBoolean("Storno"))
                        LoadProperty(Of String)(DocumentNumberProperty, dr.GetString("DocumentNumber"))
                        LoadProperty(Of Long)(IdDogovorProperty, dr.GetInt64("IdDogovor"))
                        LoadProperty(Of Integer)(IdOrganizationProperty, dr.GetInt32("IdOrganization"))
                        LoadProperty(Of Integer)(IdFakturiraNaProperty, dr.GetInt32("IdFakturiraNa"))

                        dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
                    End Using
                End Using
                Using cm1 As SqlCommand = cn.CreateCommand
                    cm1.CommandType = CommandType.StoredProcedure
                    cm1.CommandText = spGetChildDetails
                    cm1.Parameters.AddWithValue("@IdPaymentDocuments", ReadProperty(Of Long)(IdProperty))
                    Using drc As New SafeDataReader(cm1.ExecuteReader)
                        LoadProperty(Of PaymentDocumentsDetails) _
                        (PaymentDocumentDetailsProperty, PaymentDocumentsDetails.GetPaymentDocumentsDetails(drc))
                    End Using
                End Using
                Using cm1 As SqlCommand = cn.CreateCommand
                    cm1.CommandType = CommandType.StoredProcedure
                    cm1.CommandText = spGetChildRati
                    cm1.Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
                    Using drc As New SafeDataReader(cm1.ExecuteReader)
                        LoadProperty(Of PaymentDocumentsRati) _
                        (PaymentDocumentRatiProperty, PaymentDocumentsRati.GetPaymentDocumentsRati(drc))
                    End Using
                End Using

            End Using

        Catch ex As Exception
            Database.LogException("PaymentDocument.DataPortal_Fetch", ex)
            Throw New DbCslaException("PaymentDocument.DataPortal_Fetch", ex)
        End Try

    End Sub
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByDocNum)
        Database.LogInfo("PaymentDocument.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "getPaymentDocumentByDocNum"
                    cm.Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
                    cm.Parameters.AddWithValue("@dokNum", criteria.DocNum)
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        dr.Read()

                        LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
                        LoadProperty(Of Integer)(IdPaymentTypeProperty, dr.GetInt32("IdPaymentType"))
                        LoadProperty(Of Long)(IdCustomerVehicleRelationProperty, dr.GetInt64("IdCustomerVehicleRelation"))
                        LoadProperty(Of Integer)(IdOperatorProperty, dr.GetInt32("IdOperator"))
                        LoadProperty(Of SmartDate, Date?)(DatePayProperty, dr.GetSmartDate("DatePay", True))
                        LoadProperty(Of SmartDate, Date?)(DateRequiredProperty, dr.GetSmartDate("DateRequired", True))
                        LoadProperty(Of Single)(DiscountProperty, dr.GetValue("Discount"))
                        LoadProperty(Of Boolean)(PayedProperty, dr.GetBoolean("Payed"))
                        LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
                        LoadProperty(Of Boolean)(StornoProperty, dr.GetBoolean("Storno"))
                        LoadProperty(Of String)(DocumentNumberProperty, dr.GetString("DocumentNumber"))
                        LoadProperty(Of Long)(IdDogovorProperty, dr.GetInt64("IdDogovor"))
                        LoadProperty(Of Integer)(IdOrganizationProperty, dr.GetInt32("IdOrganization"))
                        LoadProperty(Of Integer)(IdFakturiraNaProperty, dr.GetInt32("IdFakturiraNa"))
                        dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
                    End Using
                End Using
                Using cm1 As SqlCommand = cn.CreateCommand
                    cm1.CommandType = CommandType.StoredProcedure
                    cm1.CommandText = spGetChildDetails
                    cm1.Parameters.AddWithValue("@IdPaymentDocuments", ReadProperty(Of Long)(IdProperty))
                    Using drc As New SafeDataReader(cm1.ExecuteReader)
                        LoadProperty(Of PaymentDocumentsDetails) _
                        (PaymentDocumentDetailsProperty, PaymentDocumentsDetails.GetPaymentDocumentsDetails(drc))
                    End Using
                End Using
                Using cm1 As SqlCommand = cn.CreateCommand
                    cm1.CommandType = CommandType.StoredProcedure
                    cm1.CommandText = spGetChildRati
                    cm1.Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
                    Using drc As New SafeDataReader(cm1.ExecuteReader)
                        LoadProperty(Of PaymentDocumentsRati) _
                        (PaymentDocumentRatiProperty, PaymentDocumentsRati.GetPaymentDocumentsRati(drc))
                    End Using
                End Using

            End Using

        Catch ex As Exception
            MsgBox(My.Resources.GresenBr)
            Database.LogException("PaymentDocument.DataPortal_Fetch", ex)
            'Throw New DbCslaException("PaymentDocument.DataPortal_Fetch", ex)
        End Try

    End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

    Protected Overrides Sub DataPortal_Insert()
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                ApplicationContext.LocalContext("cn") = cn
                Using cm1 As SqlCommand = cn.CreateCommand
                    Dim _docNum As Integer = 0
                    Dim _prefix As String = CType(Csla.ApplicationContext.LocalContext("objPaymentTypeList"), PaymentTypeList). _
                     GetPaymentTypeInfoById(ReadProperty(Of Integer)(IdPaymentTypeProperty)).Prefix
                    Dim _idStation As Integer = CType(Csla.ApplicationContext.LocalContext("objCurentUser"), UsersInfo).IdStation
                    With cm1
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = SpGetNumberForPaymentDocument

                        .Parameters.AddWithValue("@IdStation", _idStation)
                        .Parameters.AddWithValue("@IdTypeOfPayment", ReadProperty(Of Integer)(IdPaymentTypeProperty))
                        _docNum = cm1.ExecuteScalar
                    End With
                    If _prefix <> String.Empty AndAlso _prefix <> "" Then
                        Me.DocumentNumber = _prefix & "-" & _idStation & "-" & _docNum & "/" & Now.Year
                    Else
                        Me.DocumentNumber = _idStation & "-" & _docNum & "/" & Now.Year
                    End If

                End Using
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = spAdd

                        .Parameters.AddWithValue("@IdPaymentType", ReadProperty(Of Integer)(IdPaymentTypeProperty))
                        .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                        .Parameters.AddWithValue("@IdOperator", CInt(Csla.ApplicationContext.LocalContext("EmployeeID")))
                        .Parameters.AddWithValue("@DatePay", ReadProperty(Of SmartDate)(DatePayProperty).DBValue)
                        .Parameters.AddWithValue("@DateRequired", ReadProperty(Of SmartDate)(DateRequiredProperty).DBValue)
                        .Parameters.AddWithValue("@Discount", ReadProperty(Of Single)(DiscountProperty))
                        .Parameters.AddWithValue("@Payed", ReadProperty(Of Boolean)(PayedProperty))
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
                        .Parameters.AddWithValue("@Storno", ReadProperty(Of Boolean)(StornoProperty))
                        .Parameters.AddWithValue("@DocumentNumber", ReadProperty(Of String)(DocumentNumberProperty))
                        .Parameters.AddWithValue("@IdDogovor", ReadProperty(Of Long)(IdDogovorProperty))
                        .Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
                        .Parameters.AddWithValue("@IdFakturiraNa", ReadProperty(Of Integer)(IdFakturiraNaProperty))


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
                ApplicationContext.LocalContext.Add("tmpIdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                FieldManager.UpdateChildren(Me)
                ApplicationContext.LocalContext.Remove("tmpIdCustomerVehicleRelation")
                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("PaymentDocument.DataPortal_Insert", ex)
            Throw New DbCslaException("PaymentDocument.DataPortal_Insert", ex)
        Finally
            Database.LogInfo("PaymentDocument.DataPortal_Insert", GetHashCode())
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
                        .Parameters.AddWithValue("@IdPaymentType", ReadProperty(Of Integer)(IdPaymentTypeProperty))
                        .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                        .Parameters.AddWithValue("@IdOperator", CInt(Csla.ApplicationContext.LocalContext("EmployeeID")))
                        .Parameters.AddWithValue("@DatePay", ReadProperty(Of SmartDate)(DatePayProperty).DBValue)
                        .Parameters.AddWithValue("@DateRequired", ReadProperty(Of SmartDate)(DateRequiredProperty).DBValue)
                        .Parameters.AddWithValue("@Discount", ReadProperty(Of Single)(DiscountProperty))
                        .Parameters.AddWithValue("@Payed", ReadProperty(Of Boolean)(PayedProperty))
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
                        .Parameters.AddWithValue("@Storno", ReadProperty(Of Boolean)(StornoProperty))
                        .Parameters.AddWithValue("@DocumentNumber", ReadProperty(Of String)(DocumentNumberProperty)) '_docNum) 'GetDocumentNumber(Me.IdPaymentType))
                        .Parameters.AddWithValue("@IdDogovor", ReadProperty(Of Long)(IdDogovorProperty))
                        .Parameters.AddWithValue("@IdOrganization", ReadProperty(Of Integer)(IdOrganizationProperty))
                        .Parameters.AddWithValue("@IdFakturiraNa", ReadProperty(Of Integer)(IdFakturiraNaProperty))

                        .Parameters.AddWithValue("@lastChanged", _lastChanged)
                        Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
                        param.Direction = ParameterDirection.Output
                        .Parameters.Add(param)

                        .ExecuteNonQuery()

                        _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
                    End With
                End Using
                'update child objects
                FieldManager.UpdateChildren(Me)
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
        DataPortal_Delete(New SingleCriteria(Of PaymentDocument, Integer)(Id))
    End Sub

    Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of PaymentDocument, Integer))
        Database.LogInfo("PaymentDocument.DataPortal_Delete", GetHashCode())
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
            Database.LogException("PaymentDocument.DataPortal_Delete", ex)
            Throw New DbCslaException("PaymentDocument.DataPortal_Delete", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

    Private Sub Child_Fetch(ByVal dr As SafeDataReader)
        Database.LogInfo("PaymentDocument.Child_Fetch", GetHashCode())
        Try
            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of Integer)(IdPaymentTypeProperty, dr.GetInt32("IdPaymentType"))
            LoadProperty(Of Long)(IdCustomerVehicleRelationProperty, dr.GetInt64("IdCustomerVehicleRelation"))
            LoadProperty(Of Integer)(IdOperatorProperty, dr.GetInt32("IdOperator"))
            LoadProperty(Of SmartDate, Date?)(DatePayProperty, dr.GetSmartDate("DatePay", True))
            LoadProperty(Of SmartDate, Date?)(DateRequiredProperty, dr.GetSmartDate("DateRequired", True))
            LoadProperty(Of Single)(DiscountProperty, dr.GetValue("Discount"))
            LoadProperty(Of Boolean)(PayedProperty, dr.GetBoolean("Payed"))
            LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
            LoadProperty(Of Boolean)(StornoProperty, dr.GetBoolean("Storno"))
            LoadProperty(Of String)(DocumentNumberProperty, dr.GetString("DocumentNumber"))
            LoadProperty(Of Long)(IdDogovorProperty, dr.GetInt64("IdDogovor"))
            LoadProperty(Of Integer)(IdOrganizationProperty, dr.GetInt32("IdOrganization"))
            LoadProperty(Of Integer)(IdFakturiraNaProperty, dr.GetInt32("IdFakturiraNa"))


            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm1 As SqlCommand = cn.CreateCommand
                    cm1.CommandType = CommandType.StoredProcedure
                    cm1.CommandText = spGetChildDetails
                    cm1.Parameters.AddWithValue("@IdPaymentDocuments", ReadProperty(Of Long)(IdProperty))
                    Using drc As New SafeDataReader(cm1.ExecuteReader)
                        LoadProperty(Of PaymentDocumentsDetails) _
                        (PaymentDocumentDetailsProperty, PaymentDocumentsDetails.GetPaymentDocumentsDetails(drc))
                    End Using
                End Using
                Using cm1 As SqlCommand = cn.CreateCommand
                    cm1.CommandType = CommandType.StoredProcedure
                    cm1.CommandText = spGetChildRati
                    cm1.Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
                    Using drc As New SafeDataReader(cm1.ExecuteReader)
                        LoadProperty(Of PaymentDocumentsRati) _
                        (PaymentDocumentRatiProperty, PaymentDocumentsRati.GetPaymentDocumentsRati(drc))
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("PaymentDocument.Child_Fetch", ex)
            Throw New DbCslaException("PaymentDocument.Child_Fetch", ex)
        End Try

    End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

    Private Sub Child_Insert()
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                ApplicationContext.LocalContext("cn") = cn
                Using cm1 As SqlCommand = cn.CreateCommand
                    Dim _docNum As Integer = 0
                    Dim _prefix As String = CType(Csla.ApplicationContext.LocalContext("objPaymentTypeList"), PaymentTypeList). _
                     GetPaymentTypeInfoById(ReadProperty(Of Integer)(IdPaymentTypeProperty)).Prefix
                    Dim _idStation As Integer = CType(Csla.ApplicationContext.LocalContext("objCurentUser"), UsersInfo).IdStation
                    With cm1
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = SpGetNumberForPaymentDocument

                        .Parameters.AddWithValue("@IdStation", _idStation)
                        .Parameters.AddWithValue("@IdTypeOfPayment", ReadProperty(Of Integer)(IdPaymentTypeProperty))
                        _docNum = cm1.ExecuteScalar
                    End With
                    If _prefix <> String.Empty AndAlso _prefix <> "" Then
                        Me.DocumentNumber = _prefix & "-" & _idStation & "-" & _docNum & "/" & Now.Year
                    Else
                        Me.DocumentNumber = _idStation & "-" & _docNum & "/" & Now.Year
                    End If

                End Using
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = spAdd
                        .Parameters.AddWithValue("@IdPaymentType", ReadProperty(Of Integer)(IdPaymentTypeProperty))
                        .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                        .Parameters.AddWithValue("@IdOperator", CInt(Csla.ApplicationContext.LocalContext("EmployeeID")))
                        .Parameters.AddWithValue("@DatePay", ReadProperty(Of SmartDate)(DatePayProperty).DBValue)
                        .Parameters.AddWithValue("@DateRequired", ReadProperty(Of SmartDate)(DateRequiredProperty).DBValue)
                        .Parameters.AddWithValue("@Discount", ReadProperty(Of Single)(DiscountProperty))
                        .Parameters.AddWithValue("@Payed", ReadProperty(Of Boolean)(PayedProperty))
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
                        .Parameters.AddWithValue("@Storno", ReadProperty(Of Boolean)(StornoProperty))
                        .Parameters.AddWithValue("@DocumentNumber", ReadProperty(Of String)(DocumentNumberProperty))
                        .Parameters.AddWithValue("@IdDogovor", ReadProperty(Of Long)(IdDogovorProperty))
                        .Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
                        .Parameters.AddWithValue("@IdFakturiraNa", ReadProperty(Of Integer)(IdFakturiraNaProperty))
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
                FieldManager.UpdateChildren(Me)
                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("PaymentDocument.Child_Insert", ex)
            Throw New DbCslaException("PaymentDocument.Child_Insert", ex)
        Finally
            Database.LogInfo("PaymentDocument.Child_Insert", GetHashCode)
        End Try

    End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

    Private Sub Child_Update()
        Database.LogInfo("PaymentDocument.Child_Update", GetHashCode)
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                ApplicationContext.LocalContext("cn") = cn
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = spUpdate

                        .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
                        .Parameters.AddWithValue("@IdPaymentType", ReadProperty(Of Integer)(IdPaymentTypeProperty))
                        .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                        .Parameters.AddWithValue("@IdOperator", CInt(Csla.ApplicationContext.LocalContext("EmployeeID")))
                        .Parameters.AddWithValue("@DatePay", ReadProperty(Of SmartDate)(DatePayProperty).DBValue)
                        .Parameters.AddWithValue("@DateRequired", ReadProperty(Of SmartDate)(DateRequiredProperty).DBValue)
                        .Parameters.AddWithValue("@Discount", ReadProperty(Of Single)(DiscountProperty))
                        .Parameters.AddWithValue("@Payed", ReadProperty(Of Boolean)(PayedProperty))
                        .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
                        .Parameters.AddWithValue("@Storno", ReadProperty(Of Boolean)(StornoProperty))
                        .Parameters.AddWithValue("@DocumentNumber", ReadProperty(Of String)(DocumentNumberProperty))
                        .Parameters.AddWithValue("@IdDogovor", ReadProperty(Of Long)(IdDogovorProperty))
                        .Parameters.AddWithValue("@IdOrganization", ReadProperty(Of Integer)(IdOrganizationProperty))
                        .Parameters.AddWithValue("@IdFakturiraNa", ReadProperty(Of Integer)(IdFakturiraNaProperty))

                        .Parameters.AddWithValue("@lastChanged", _lastChanged)
                        Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
                        param.Direction = ParameterDirection.Output
                        .Parameters.Add(param)

                        .ExecuteNonQuery()

                        _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
                    End With
                End Using
                'update child objects
                FieldManager.UpdateChildren(Me)
                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("PaymentDocument.Child_Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DbCslaException("PaymentDocument.Child_Update", ex)
            End If
        End Try
    End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

    Private Sub Child_DeleteSelf()

        Database.LogInfo("PaymentDocument.Child_DeleteSelf", GetHashCode)
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
            Database.LogException("PaymentDocument.Child_Fetch", ex)
            Throw New DbCslaException("PaymentDocument.Child_Fetch", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

    '#Region " DocumentNumberCommand "

    '    Public Shared Function GetDocumentNumber(ByVal idPaymentType As Integer) As String

    '        Dim result As DocumentNumberCommand
    '        result = DataPortal.Execute(Of DocumentNumberCommand)(New DocumentNumberCommand(idPaymentType))
    '        Return result.DocumentNumber

    '    End Function

    '    <Serializable()> _
    '    Private Class DocumentNumberCommand
    '        Inherits CommandBase

    '        Private _idPaymentType As Integer
    '        Private _documentNumber As String

    '        Public ReadOnly Property DocumentNumber()
    '            Get
    '                Return _documentNumber
    '            End Get
    '        End Property

    '        Public Sub New(ByVal idPaymentType As Integer)
    '            _idPaymentType = idPaymentType
    '        End Sub

    '        Protected Overrides Sub DataPortal_Execute()
    '            Dim result As String = String.Empty

    '            Using cn As SqlConnection = Database.VTE_SqlConnection
    '                Using cm As SqlCommand = cn.CreateCommand
    '                    cm.CommandType = CommandType.StoredProcedure
    '                    cm.CommandText = "GetPaymentDocumentPrefix"
    '                    cm.Parameters.AddWithValue("@idPaymentType", _idPaymentType)
    '                    Using dr As New SafeDataReader(cm.ExecuteReader)
    '                        If dr.Read() Then
    '                            result = dr.GetString("Prefix")
    '                        End If
    '                    End Using
    '                End Using
    '                Using cm As SqlCommand = cn.CreateCommand
    '                    cm.CommandType = CommandType.Text
    '                    cm.CommandText = "SELECT COUNT(ID) AS Broj FROM PaymentDocuments WHERE (ACTIVE=1) AND (IdPaymentType=@idPaymentType)" ' AND (DatePay BETWEEN @curretYear AND @endYear)"
    '                    cm.Parameters.AddWithValue("@idPaymentType", _idPaymentType)
    '                    'Dim curretnYear As New SmartDate(New Date(Now.Date.Year, 1, 1), True)
    '                    'cm.Parameters.AddWithValue("@curretYear", curretnYear.DBValue)
    '                    'Dim endYear As New SmartDate(New Date(Now.Date.Year, 12, 31), True)
    '                    'cm.Parameters.AddWithValue("@endYear", curretnYear.DBValue)
    '                    Using dr As New SafeDataReader(cm.ExecuteReader)
    '                        If dr.Read() Then
    '                            'count = dr.GetInt64("Id")
    '                            result = result & "" & (dr.GetValue("Broj") + 1).ToString
    '                        Else
    '                            result = result & "1"
    '                        End If
    '                    End Using

    '                End Using
    '            End Using
    '            _documentNumber = result
    '        End Sub

    '    End Class

    '#End Region


#Region " Readonlylist refresh "
    Public Shared Event PaymentDocumentSaved As EventHandler(Of Csla.Core.SavedEventArgs)
    Protected Shared Sub OnPaymentDocumentSaved(ByVal sender As PaymentDocument, ByVal e As Csla.Core.SavedEventArgs)
        RaiseEvent PaymentDocumentSaved(sender, e)
    End Sub
#End Region

    ''Private Sub PaymentDocument_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
    ''  If e.PropertyName = PolisaProperty.Name Then
    ''    Me.PaymentDocumentDetails.RaiseListChangedEvents = True
    ''  End If
    ''End Sub
End Class
