
<Serializable()> _
Public Class Request
 Inherits Csla.BusinessBase(Of Request)

#Region " Stored Procedures Names "
 Private Const spGetByID As String = "GetRequestByID"
 Private Const spGetAll As String = "GetRequests"
 Private Const spUpdate As String = "updateRequest"
 Private Const spAdd As String = "addRequest"
 Private Const spDelete As String = "deleteRequest"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
 'register properties
 Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Request), New PropertyInfo(Of Long)("Id"))
 Private Shared IdRequestTypeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Request), New PropertyInfo(Of Integer)("IdRequestType"))
 Private Shared IdCustomerVehicleRelationProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Request), New PropertyInfo(Of Long)("IdCustomerVehicleRelation"))
 Private Shared IdCustomerVehicleRelationNewProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Request), New PropertyInfo(Of Long)("IdCustomerVehicleRelationNew"))
 Private Shared IdOperatorCreatedProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Request), New PropertyInfo(Of Integer)("IdOperatorCreated"))
 Private Shared IdOperatorModifiedProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Request), New PropertyInfo(Of Integer)("IdOperatorModified"))
 Private Shared IdOperatorEndedProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Request), New PropertyInfo(Of Integer)("IdOperatorEnded"))
 Private Shared IdTechnicalExamReportProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Request), New PropertyInfo(Of Long)("IdTechnicalExamReport"))
 Private Shared IdPreviousRegistrationProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Request), New PropertyInfo(Of Integer)("IdPreviousRegistration"))
 Private Shared DateCreatedProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(Request), New PropertyInfo(Of SmartDate)("DateCreated", New SmartDate(DateTime.Today, True)))
 Private Shared DateModifiedProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(Request), New PropertyInfo(Of SmartDate)("DateModified", New SmartDate(True)))
 Private Shared DateEndedProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(Request), New PropertyInfo(Of SmartDate)("DateEnded", New SmartDate(True)))
 Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Request), New PropertyInfo(Of String)("Note"))
 Private Shared IsCustomerChangedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(Request), New PropertyInfo(Of Boolean)("IsCustomerChanged"))
 Private Shared IsVehicleChangedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(Request), New PropertyInfo(Of Boolean)("IsVehicleChanged"))
 Private Shared IdOrganizationProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Request), New PropertyInfo(Of Integer)("IdOrganization"))
 'deca
 Private Shared OwnershipProofsProperty As PropertyInfo(Of RequestVehicleOwnershipProofs) = RegisterProperty(Of RequestVehicleOwnershipProofs)(GetType(Request), New PropertyInfo(Of RequestVehicleOwnershipProofs)("OwnershipProofs"))

 Private Shared AttachmentsProperty As PropertyInfo(Of RequestAttachments) = RegisterProperty(New PropertyInfo(Of RequestAttachments)("Attachments", "Attachments"))
 ''' <Summary>
 ''' Gets and sets the Attachments value.
 ''' </Summary>
 Public ReadOnly Property Attachments() As RequestAttachments
  Get
   If Not FieldManager.FieldExists(AttachmentsProperty) Then
    SetProperty(Of RequestAttachments) _
    (AttachmentsProperty, RequestAttachments.NewRequestAttachments())
   End If
   Return GetProperty(Of RequestAttachments)(AttachmentsProperty)
  End Get
 End Property

 Public ReadOnly Property OwnershipProofs() As RequestVehicleOwnershipProofs
  Get
   If Not FieldManager.FieldExists(OwnershipProofsProperty) Then
    SetProperty(Of RequestVehicleOwnershipProofs)(OwnershipProofsProperty, RequestVehicleOwnershipProofs.NewRequestVehicleOwnershipProofs)
   End If
   Return GetProperty(Of RequestVehicleOwnershipProofs)(OwnershipProofsProperty)
  End Get
 End Property
 Private Shared PaymentProofsProperty As PropertyInfo(Of RequestPaymentProofs) = RegisterProperty(Of RequestPaymentProofs)(GetType(Request), New PropertyInfo(Of RequestPaymentProofs)("PaymentProofs"))
 Public ReadOnly Property PaymentProofs() As RequestPaymentProofs
  Get
   If Not FieldManager.FieldExists(PaymentProofsProperty) Then
    SetProperty(Of RequestPaymentProofs)(PaymentProofsProperty, RequestPaymentProofs.NewRequestPaymentProofs)
   End If
   Return GetProperty(Of RequestPaymentProofs)(PaymentProofsProperty)
  End Get
 End Property

 Private _lastChanged(7) As Byte

 <System.ComponentModel.DataObjectField(True, True)> _
 Public ReadOnly Property Id() As Long
  Get
   Return GetProperty(Of Long)(IdProperty)
  End Get
 End Property
 Public Property IdRequestType() As Integer
  Get
   Return GetProperty(Of Integer)(IdRequestTypeProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdRequestTypeProperty, value)
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
 Public Property IdCustomerVehicleRelationNew() As Long
  Get
   Return GetProperty(Of Long)(IdCustomerVehicleRelationNewProperty)
  End Get
  Set(ByVal value As Long)
   SetProperty(Of Long)(IdCustomerVehicleRelationNewProperty, value)
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
 Public Property IdOperatorModified() As Integer
  Get
   Return GetProperty(Of Integer)(IdOperatorModifiedProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdOperatorModifiedProperty, value)
  End Set
 End Property
 Public Property IdOperatorEnded() As Integer
  Get
   Return GetProperty(Of Integer)(IdOperatorEndedProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdOperatorEndedProperty, value)
  End Set
 End Property
 Public Property IdTechnicalExamReport() As Long
  Get
   Return GetProperty(Of Long)(IdTechnicalExamReportProperty)
  End Get
  Set(ByVal value As Long)
   SetProperty(Of Long)(IdTechnicalExamReportProperty, value)
  End Set
 End Property
 Public Property IdPreviousRegistration() As Integer
  Get
   Return GetProperty(Of Integer)(IdPreviousRegistrationProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdPreviousRegistrationProperty, value)
  End Set
 End Property
 Public Property DateCreated() As Date
  Get
   Return GetProperty(Of SmartDate, Date)(DateCreatedProperty)
  End Get
  Set(ByVal value As Date)
   SetProperty(Of SmartDate, Date)(DateCreatedProperty, value)
  End Set
 End Property
 Public Property DateModified() As Date
  Get
   Return GetProperty(Of SmartDate, Date)(DateModifiedProperty)
  End Get
  Set(ByVal value As Date)
   SetProperty(Of SmartDate, Date)(DateModifiedProperty, value)
  End Set
 End Property
 Public Property DateEnded() As Date
  Get
   Return GetProperty(Of SmartDate, Date)(DateEndedProperty)
  End Get
  Set(ByVal value As Date)
   SetProperty(Of SmartDate, Date)(DateEndedProperty, value)
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
 Public Property IsCustomerChanged() As Boolean
  Get
   Return GetProperty(Of Boolean)(IsCustomerChangedProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(IsCustomerChangedProperty, value)
  End Set
 End Property
 Public Property IsVehicleChanged() As Boolean
  Get
   Return GetProperty(Of Boolean)(IsVehicleChangedProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(IsVehicleChangedProperty, value)
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


 Public Overrides Function ToString() As String
  Return Id.ToString
 End Function
#End Region 'Business Properties and Methods

#Region " Public Methods "
 Public Sub CloseRequest()
  Dim reqestType As RequestTypeInfo = _
    CType(Csla.ApplicationContext.LocalContext("objRequestTypeList"),  _
    RequestTypeList).getInfoById(Me.IdRequestType)
  Dim curRel As CustomerVehiclesRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(Me.IdCustomerVehicleRelation)
  'stavi nova relacija

  If reqestType.IsRelationDeleted Then
   If curRel.IdCustomer = Me.IdCustomerVehicleRelationNew Then
    curRel.TerminationNote = "Избришана па продолжена по барање бр." & Me.Id
    curRel.Save()
   Else
    curRel.EndDate = Now.Date
    curRel.TerminationNote = "Избришана по барање бр." & Me.Id
    curRel.Save()
    curRel.Delete()
    'curRel.Save()
   End If

  End If
  'proverki za tehnicki
  If reqestType.IsTehnicalExamRequired Then
   If Me.IdTechnicalExamReport = 0 Then
    MsgBox("Потребно е да се изврши техички преглед," & vbCrLf _
           & "пред да се затвори барањето", , "Грешка")
    Exit Sub
   Else
    Dim tehnicalExam As DocumentsTehnicalExamsReport = _
    DocumentsTehnicalExamsReport.GetDocumentsTehnicalExamsReport(Me.IdTechnicalExamReport)
    If Not (tehnicalExam.VehicleIsRight AndAlso tehnicalExam.IdSecondControler > 0) Then
     'Dim objOpcii = CType(Csla.ApplicationContext.LocalContext("objOpcii"), Options)
     ' If objOpcii.ApproveRequestAutomate Then
     'If Not tehnicalExam.IdSecondControler > 0 Then
     '    tehnicalExam.IdSecondControler = Csla.ApplicationContext.LocalContext("EmployeeID")
     '    tehnicalExam.Save()

     'End If
     ' If Not tehnicalExam.VehicleIsRight Then
     MsgBox("Техничкиот преглед за возилото е невалиден," & vbCrLf _
       & "не може да се затвори барањето", , "Грешка")
     Exit Sub
     'End If
     'Else
     'MsgBox("Техничкиот преглед за возилото е невалиден," & vbCrLf _
     '       & "не може да се затвори барањето", , "Грешка")
     'Exit Sub

     'End If
    End If
   End If
  End If
  If reqestType.IsVehicleDeleted Then
   Dim veh As Vehicle = Vehicle.GetVehicle(curRel.IdVehicle)
   veh.Delete()
   veh.Save()
  End If
  curRel.Save()
  Me.DateEnded = Now.Date
  Me.IdOperatorEnded = Csla.ApplicationContext.LocalContext("EmployeeID")
  Me.Save()
 End Sub
#End Region

#Region " Authorization Rules "

 Protected Overrides Sub AddAuthorizationRules()
  Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdRequestType") Then
   AuthorizationRules.AllowWrite("IdRequestType", roleName)
  Else
   AuthorizationRules.DenyWrite("IdRequestType", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdRequestType")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCustomerVehicleRelation") Then
   AuthorizationRules.AllowWrite("IdCustomerVehicleRelation", roleName)
  Else
   AuthorizationRules.DenyWrite("IdCustomerVehicleRelation", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdCustomerVehicleRelation")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCustomerVehicleRelationNew") Then
   AuthorizationRules.AllowWrite("IdCustomerVehicleRelationNew", roleName)
  Else
   AuthorizationRules.DenyWrite("IdCustomerVehicleRelationNew", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdCustomerVehicleRelationNew")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdOperatorCreated") Then
   AuthorizationRules.AllowWrite("IdOperatorCreated", roleName)
  Else
   AuthorizationRules.DenyWrite("IdOperatorCreated", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdOperatorCreated")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdOperatorModified") Then
   AuthorizationRules.AllowWrite("IdOperatorModified", roleName)
  Else
   AuthorizationRules.DenyWrite("IdOperatorModified", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdOperatorModified")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdOperatorEnded") Then
   AuthorizationRules.AllowWrite("IdOperatorEnded", roleName)
  Else
   AuthorizationRules.DenyWrite("IdOperatorEnded", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdOperatorEnded")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdTechnicalExamReport") Then
   AuthorizationRules.AllowWrite("IdTechnicalExamReport", roleName)
  Else
   AuthorizationRules.DenyWrite("IdTechnicalExamReport", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdTechnicalExamReport")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdPreviousRegistration") Then
   AuthorizationRules.AllowWrite("IdPreviousRegistration", roleName)
  Else
   AuthorizationRules.DenyWrite("IdPreviousRegistration", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdPreviousRegistration")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DateCreated") Then
   AuthorizationRules.AllowWrite("DateCreated", roleName)
  Else
   AuthorizationRules.DenyWrite("DateCreated", roleName)
  End If
  'AuthorizationRules.AllowWrite("DateCreated")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DateModified") Then
   AuthorizationRules.AllowWrite("DateModified", roleName)
  Else
   AuthorizationRules.DenyWrite("DateModified", roleName)
  End If
  'AuthorizationRules.AllowWrite("DateModified")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DateEnded") Then
   AuthorizationRules.AllowWrite("DateEnded", roleName)
  Else
   AuthorizationRules.DenyWrite("DateEnded", roleName)
  End If
  'AuthorizationRules.AllowWrite("DateEnded")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Note") Then
   AuthorizationRules.AllowWrite("Note", roleName)
  Else
   AuthorizationRules.DenyWrite("Note", roleName)
  End If
  'AuthorizationRules.AllowWrite("Note")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsCustomerChanged") Then
   AuthorizationRules.AllowWrite("IsCustomerChanged", roleName)
  Else
   AuthorizationRules.DenyWrite("IsCustomerChanged", roleName)
  End If
  'AuthorizationRules.AllowWrite("IsCustomerChanged")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsVehicleChanged") Then
   AuthorizationRules.AllowWrite("IsVehicleChanged", roleName)
  Else
   AuthorizationRules.DenyWrite("IsVehicleChanged", roleName)
  End If
  'AuthorizationRules.AllowWrite("IsVehicleChanged")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
   AuthorizationRules.AllowWrite("Active", roleName)
  Else
   AuthorizationRules.DenyWrite("Active", roleName)
  End If
  'AuthorizationRules.AllowWrite("Active")
 End Sub


#End Region ' Authorization Rules

#Region " Validation Rules "
 Protected Overrides Sub AddBusinessRules()
  ' DateCreatedProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, DateCreatedProperty)
  ' NoteProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NoteProperty, 250))

  ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                          New Validation.IntegerMinValueRuleArgs(IdRequestTypeProperty, 1))
  ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                          New Validation.IntegerMinValueRuleArgs(IdCustomerVehicleRelationProperty, 1))

  ValidationRules.AddRule(Of Request)(AddressOf NewCustomerRequired, IdCustomerVehicleRelationNewProperty)
  ValidationRules.AddRule(Of Request)(AddressOf NewCustomerRequired, IdRequestTypeProperty)
  ValidationRules.AddDependentProperty(IdCustomerVehicleRelationNewProperty, IdRequestTypeProperty, True)

  ' ValidationRules.AddRule(Of Request)(AddressOf PreviosRegistrationRequired, IdPreviousRegistrationProperty)
  'ValidationRules.AddRule(Of Request)(AddressOf PreviosRegistrationRequired, IdRequestTypeProperty)
  ' ValidationRules.AddDependentProperty(IdPreviousRegistrationProperty, IdRequestTypeProperty, True)
 End Sub

 Private Shared Function RequestTypeIsSufficent(Of T As Request)(ByVal target As T, _
 ByVal e As Csla.Validation.RuleArgs) As Boolean
  Dim requestType As RequestTypeInfo = CType(Csla.ApplicationContext.LocalContext("objRequestTypeList"), RequestTypeList).getInfoById(target.IdRequestType)
  If (requestType Is Nothing) AndAlso (Not requestType.IsSufficient) Then
   e.Description = "Одбраната релација не е доволна, изберете понатаму"
   Return False
  End If
  Return True
 End Function

 Private Shared Function NewCustomerRequired(Of T As Request)(ByVal target As T, _
 ByVal e As Csla.Validation.RuleArgs) As Boolean
  Dim requestType As RequestTypeInfo = CType(Csla.ApplicationContext.LocalContext("objRequestTypeList"), RequestTypeList).getInfoById(target.IdRequestType)
  If (requestType IsNot Nothing) AndAlso (requestType.IsNewCustomer) Then
   If Not target.IdCustomerVehicleRelationNew >= 1 Then
    e.Description = "Потребно е да се внесе новиот сопственик"
    Return False
   End If
  End If
  Return True
 End Function

 Private Shared Function PreviosRegistrationRequired(Of T As Request)(ByVal target As T, _
ByVal e As Csla.Validation.RuleArgs) As Boolean
  Dim requestType As RequestTypeInfo = CType(Csla.ApplicationContext.LocalContext("objRequestTypeList"), RequestTypeList).getInfoById(target.IdRequestType)
  If (requestType IsNot Nothing) AndAlso (requestType.IsPreviosRegistrationReqired) Then
   If Not target.IdPreviousRegistration >= 1 Then
    e.Description = "Потребно е да се избере претходна регистрација"
    Return False
   End If
  End If
  Return True
 End Function

#End Region ' Validation Rules

#Region " Factory Methods "

 Private Sub New()
  ' require use of factory method 
 End Sub

 Public Shared Function NewRequest() As Request
  If Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a Request")
  End If
  Return DataPortal.Create(Of Request)()
 End Function

 Public Shared Function GetRequest(ByVal id As Long) As Request
  If Not CanGetObject() Then
   Throw New System.Security.SecurityException("User not authorized to view a Request")
  End If
  Return DataPortal.Fetch(New SingleCriteria(Of Request, Integer)(id))
 End Function

 Public Shared Function GetRequestByIdTehEx(ByVal idTehEx As Long, ByVal inpom As Integer) As Request
  If Not CanGetObject() Then
   Throw New System.Security.SecurityException("User not authorized to view a Request")
  End If
  Return DataPortal.Fetch(New criteriaByTehExaId(idTehEx, inpom))
 End Function

 Public Shared Sub DeleteRequest(ByVal id As Long)
  If Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a Request")
  End If
  DataPortal.Delete(New SingleCriteria(Of Request, Integer)(id))
 End Sub


 Public Overrides Function Save() As Request
  If IsDeleted AndAlso Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a Request")
  ElseIf IsNew AndAlso Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a Request")
  ElseIf Not CanEditObject() Then
   Throw New System.Security.SecurityException("User not authorized to update a Request")
  End If

  Dim result As Request = MyBase.Save

  OnRequestSaved(Me, New Csla.Core.SavedEventArgs(result))

  Return result
 End Function
 Public Shared Function CanGetObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Request")
 End Function

 Public Shared Function CanAddObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Request")
 End Function

 Public Shared Function CanEditObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Request")
 End Function

 Public Shared Function CanDeleteObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Request")
 End Function

#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Create "

 <RunLocal()> _
 Private Overloads Sub DataPortal_Create()
  DateCreated = Now.Date
  IdOperatorCreated = Csla.ApplicationContext.LocalContext("EmployeeID")
  ValidationRules.CheckRules()
  Dim paymentproof As RequestPaymentProof = PaymentProofs.AddNew()
  paymentproof.IdPaymentProof = 1
  Dim ownershipproof As RequestVehicleOwnershipProof = OwnershipProofs.AddNew()
  ownershipproof.IdVehicleOwnershipProof = 7
 End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "
 <Serializable()> _
Private Class criteriaByTehExaId
  Private _idIn As Long
  Private _pom As Integer

  Public ReadOnly Property IdIn() As Long
   Get
    Return _idIn
   End Get
  End Property

  Public ReadOnly Property pom() As Integer
   Get
    Return _pom
   End Get
  End Property
  Public Sub New(ByVal idIn As Long, ByVal pom As Integer)
   _idIn = idIn
   _pom = pom
  End Sub
 End Class
 Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of Request, Integer))
  Database.LogInfo("Request.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = spGetByID
     cm.Parameters.AddWithValue("@Id", criteria.Value)
     Using dr As New SafeDataReader(cm.ExecuteReader)
      dr.Read()

      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Integer)(IdRequestTypeProperty, dr.GetInt32("IdRequestType"))
      LoadProperty(Of Long)(IdCustomerVehicleRelationProperty, dr.GetInt64("IdCustomerVehicleRelation"))
      LoadProperty(Of Long)(IdCustomerVehicleRelationNewProperty, dr.GetInt64("IdCustomerVehicleRelationNew"))
      LoadProperty(Of Integer)(IdOperatorCreatedProperty, dr.GetInt32("IdOperatorCreated"))
      LoadProperty(Of Integer)(IdOperatorModifiedProperty, dr.GetInt32("IdOperatorModified"))
      LoadProperty(Of Integer)(IdOperatorEndedProperty, dr.GetInt32("IdOperatorEnded"))
      LoadProperty(Of Long)(IdTechnicalExamReportProperty, dr.GetInt64("IdTechnicalExamReport"))
      LoadProperty(Of Integer)(IdPreviousRegistrationProperty, dr.GetInt32("IdPreviousRegistration"))
      LoadProperty(Of SmartDate, Date?)(DateCreatedProperty, dr.GetSmartDate("DateCreated", True))
      LoadProperty(Of SmartDate, Date?)(DateModifiedProperty, dr.GetSmartDate("DateModified", True))
      LoadProperty(Of SmartDate, Date?)(DateEndedProperty, dr.GetSmartDate("DateEnded", True))
      LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
      LoadProperty(Of Boolean)(IsCustomerChangedProperty, dr.GetBoolean("IsCustomerChanged"))
      LoadProperty(Of Boolean)(IsVehicleChangedProperty, dr.GetBoolean("IsVehicleChanged"))
      LoadProperty(Of Integer)(IdOrganizationProperty, dr.GetInt32("IdOrganisation"))
      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

     End Using
    End Using
    'deca
    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "getRequestVehicleOwnershipProofByIdRequest"
     cm.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
     Using dr As New SafeDataReader(cm.ExecuteReader)
      LoadProperty(Of RequestVehicleOwnershipProofs) _
      (OwnershipProofsProperty, RequestVehicleOwnershipProofs.GetRequestVehicleOwnershipProofs(dr))
     End Using
    End Using
    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "getRequestPaymentProoByIdRequest"
     cm.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
     Using dr As New SafeDataReader(cm.ExecuteReader)
      LoadProperty(Of RequestPaymentProofs) _
      (PaymentProofsProperty, RequestPaymentProofs.GetRequestPaymentProofs(dr))
     End Using
    End Using
    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "getDocumentAttachmentByIdDocument"
     cm.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
     Using dr As New SafeDataReader(cm.ExecuteReader)
      LoadProperty(Of RequestAttachments) _
      (AttachmentsProperty, RequestAttachments.GetRequestAttachments(dr))
     End Using
    End Using
   End Using

  Catch ex As Exception
   Database.LogException("Request.DataPortal_Fetch", ex)
   Throw New DbCslaException("Request.DataPortal_Fetch", ex)
  End Try

 End Sub

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As criteriaByTehExaId)
  Database.LogInfo("Request.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "getRequestByIdTechnicalExamReport"
     cm.Parameters.AddWithValue("@IdTechnicalExamReport", criteria.IdIn)
     Using dr As New SafeDataReader(cm.ExecuteReader)
      dr.Read()

      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Integer)(IdRequestTypeProperty, dr.GetInt32("IdRequestType"))
      LoadProperty(Of Long)(IdCustomerVehicleRelationProperty, dr.GetInt64("IdCustomerVehicleRelation"))
      LoadProperty(Of Long)(IdCustomerVehicleRelationNewProperty, dr.GetInt64("IdCustomerVehicleRelationNew"))
      LoadProperty(Of Integer)(IdOperatorCreatedProperty, dr.GetInt32("IdOperatorCreated"))
      LoadProperty(Of Integer)(IdOperatorModifiedProperty, dr.GetInt32("IdOperatorModified"))
      LoadProperty(Of Integer)(IdOperatorEndedProperty, dr.GetInt32("IdOperatorEnded"))
      LoadProperty(Of Long)(IdTechnicalExamReportProperty, dr.GetInt64("IdTechnicalExamReport"))
      LoadProperty(Of Integer)(IdPreviousRegistrationProperty, dr.GetInt32("IdPreviousRegistration"))
      LoadProperty(Of SmartDate, Date?)(DateCreatedProperty, dr.GetSmartDate("DateCreated", True))
      LoadProperty(Of SmartDate, Date?)(DateModifiedProperty, dr.GetSmartDate("DateModified", True))
      LoadProperty(Of SmartDate, Date?)(DateEndedProperty, dr.GetSmartDate("DateEnded", True))
      LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
      LoadProperty(Of Boolean)(IsCustomerChangedProperty, dr.GetBoolean("IsCustomerChanged"))
      LoadProperty(Of Boolean)(IsVehicleChangedProperty, dr.GetBoolean("IsVehicleChanged"))
      LoadProperty(Of Integer)(IdOrganizationProperty, dr.GetInt32("IdOrganisation"))
      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
     End Using
    End Using
    'deca

    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "getRequestVehicleOwnershipProofByIdRequest"
     cm.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
     Using dr As New SafeDataReader(cm.ExecuteReader)
      LoadProperty(Of RequestVehicleOwnershipProofs) _
      (OwnershipProofsProperty, RequestVehicleOwnershipProofs.GetRequestVehicleOwnershipProofs(dr))
     End Using
    End Using
    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "getRequestPaymentProoByIdRequest"
     cm.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
     Using dr As New SafeDataReader(cm.ExecuteReader)
      LoadProperty(Of RequestPaymentProofs) _
      (PaymentProofsProperty, RequestPaymentProofs.GetRequestPaymentProofs(dr))
     End Using
    End Using
    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "getDocumentAttachmentByIdDocument"
     cm.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
     Using dr As New SafeDataReader(cm.ExecuteReader)
      LoadProperty(Of RequestAttachments) _
      (AttachmentsProperty, RequestAttachments.GetRequestAttachments(dr))
     End Using
    End Using

   End Using

  Catch ex As Exception
   Database.LogException("Request.DataPortal_Fetch", ex)

   'Throw New DbCslaException("Request.DataPortal_Fetch", ex)
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

      .Parameters.AddWithValue("@IdRequestType", ReadProperty(Of Integer)(IdRequestTypeProperty))
      .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
      .Parameters.AddWithValue("@IdCustomerVehicleRelationNew", ReadProperty(Of Long)(IdCustomerVehicleRelationNewProperty))
      .Parameters.AddWithValue("@IdOperatorCreated", Csla.ApplicationContext.LocalContext("EmployeeID"))
      .Parameters.AddWithValue("@IdOperatorModified", Csla.ApplicationContext.LocalContext("EmployeeID"))

      .Parameters.AddWithValue("@IdTechnicalExamReport", ReadProperty(Of Long)(IdTechnicalExamReportProperty))
      .Parameters.AddWithValue("@IdPreviousRegistration", ReadProperty(Of Integer)(IdPreviousRegistrationProperty))
      .Parameters.AddWithValue("@DateCreated", ReadProperty(Of SmartDate)(DateCreatedProperty).DBValue)
      .Parameters.AddWithValue("@DateModified", ReadProperty(Of SmartDate)(DateModifiedProperty).DBValue)
      Dim objCurentTechExamOrganizationInfo As TehnicalExamOrganizationsInfo = _
CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)
      If objCurentTechExamOrganizationInfo.ApproveRequestAutomate Then
       .Parameters.AddWithValue("@DateEnded", Now.Date)
       .Parameters.AddWithValue("@IdOperatorEnded", Csla.ApplicationContext.LocalContext("EmployeeID"))
      Else
       .Parameters.AddWithValue("@IdOperatorEnded", ReadProperty(Of Integer)(IdOperatorEndedProperty))
       .Parameters.AddWithValue("@DateEnded", ReadProperty(Of SmartDate)(DateEndedProperty).DBValue)
      End If

      .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
      .Parameters.AddWithValue("@IsCustomerChanged", ReadProperty(Of Boolean)(IsCustomerChangedProperty))
      .Parameters.AddWithValue("@IsVehicleChanged", ReadProperty(Of Boolean)(IsVehicleChangedProperty))
            .Parameters.AddWithValue("@IdOrganisation", objCurentTechExamOrganizationInfo.Id)
      Dim param As New SqlParameter("@newId", SqlDbType.Int)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)
      param = New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)

      .ExecuteNonQuery()

      LoadProperty(Of Long)(IdProperty, CLng(.Parameters("@newId").Value))
      _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
     End With
    End Using
    'update child objects
    FieldManager.UpdateChildren(Me)
    AddDeptsToCustomer(cn)
    If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
     ApplicationContext.LocalContext.Remove("cn")
    End If
   End Using
  Catch ex As Exception
   Database.LogException("Request.DataPortal_Insert", ex)
   Throw New DbCslaException("Request.DataPortal_Insert", ex)
  Finally
   Database.LogInfo("Request.DataPortal_Insert", GetHashCode())
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
      .Parameters.AddWithValue("@IdRequestType", ReadProperty(Of Integer)(IdRequestTypeProperty))
      .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
      .Parameters.AddWithValue("@IdCustomerVehicleRelationNew", ReadProperty(Of Long)(IdCustomerVehicleRelationNewProperty))
      .Parameters.AddWithValue("@IdOperatorCreated", ReadProperty(Of Integer)(IdOperatorCreatedProperty))
      .Parameters.AddWithValue("@IdOperatorModified", Csla.ApplicationContext.LocalContext("EmployeeID"))
      .Parameters.AddWithValue("@IdOperatorEnded", ReadProperty(Of Integer)(IdOperatorEndedProperty))
      .Parameters.AddWithValue("@IdTechnicalExamReport", ReadProperty(Of Long)(IdTechnicalExamReportProperty))
      .Parameters.AddWithValue("@IdPreviousRegistration", ReadProperty(Of Integer)(IdPreviousRegistrationProperty))
      .Parameters.AddWithValue("@DateCreated", ReadProperty(Of SmartDate)(DateCreatedProperty).DBValue)
      .Parameters.AddWithValue("@DateModified", ReadProperty(Of SmartDate)(DateModifiedProperty).DBValue)
      .Parameters.AddWithValue("@DateEnded", ReadProperty(Of SmartDate)(DateEndedProperty).DBValue)
      .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
      .Parameters.AddWithValue("@IsCustomerChanged", ReadProperty(Of Boolean)(IsCustomerChangedProperty))
      .Parameters.AddWithValue("@IsVehicleChanged", ReadProperty(Of Boolean)(IsVehicleChangedProperty))
      .Parameters.AddWithValue("@IdOrganisation", ReadProperty(Of Integer)(IdOrganizationProperty))
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
   Database.LogException("Request.DataPortal_Update", ex)
   If Not ex.Message.EndsWith("drug korisnik") Then
    Throw New DBConcurrencyException("Request.DataPortal_Update", ex)
   End If
  End Try
 End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

 Protected Overrides Sub DataPortal_DeleteSelf()
  DataPortal_Delete(New SingleCriteria(Of Request, Integer)(Id))
 End Sub

 Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of Request, Integer))
  Database.LogInfo("Request.DataPortal_Delete", GetHashCode())
  Try
   Dim reqestType As RequestTypeInfo = _
     CType(Csla.ApplicationContext.LocalContext("objRequestTypeList"),  _
     RequestTypeList).getInfoById(Me.IdRequestType)
   'izbrisi nova relacija
   If reqestType.IsNewCustomer Then
    Dim tmpVehicleId As Integer = _
      CustomerVehiclesRelation.GetCustomerVehiclesRelation( _
      ReadProperty(Of Long)(IdCustomerVehicleRelationProperty)).IdVehicle

    Dim newRel As CustomerVehiclesRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsListByCustomerVehicleAndType(Me.IdCustomerVehicleRelationNew, tmpVehicleId, 1).Item(0).Id)


    If reqestType.IsRelationDeleted Then
     newRel.EndDate = Now.Date
     newRel.TerminationNote = "Избришана по откажување на барање бр." & Me.Id

     newRel.Delete()
     newRel.Save()
    End If
   End If


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
   Database.LogException("Request.DataPortal_Delete", ex)
   Throw New DbCslaException("Request.DataPortal_Delete", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Payment "
 Private Sub AddDeptsToCustomer(ByVal cn As SqlConnection)
  Try
   'zemi go voziloto od relacijata
   'Dim tmpVehicleId As Integer = _
   '  CustomerVehiclesRelation.GetCustomerVehiclesRelation( _
   '  ReadProperty(Of Long)(IdCustomerVehicleRelationProperty)).IdVehicle
   ' Dim objRlationList As CustomerVehiclesRelationsList = Csla.ApplicationContext.LocalContext("objRlationList")
   Dim tmpVehicleId As Integer = _
   CustomerVehiclesRelation.GetCustomerVehiclesRelation( _
  ReadProperty(Of Long)(IdCustomerVehicleRelationProperty)).IdVehicle

   'zemi relacija
   Dim IdRelation As Long
   Dim objRequestTypeList = Csla.ApplicationContext.LocalContext("objRequestTypeList")


   If objRequestTypeList.getInfoById(Me.IdRequestType).IsNewCustomer Then
    'IdRelation = objRlationList.GetInfoRelationById(Me.IdCustomerVehicleRelationNew).Id
    IdRelation = CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsListByCustomerVehicleAndType(Me.IdCustomerVehicleRelationNew, tmpVehicleId, 1).Item(0).Id
   Else
    IdRelation = ReadProperty(Of Long)(IdCustomerVehicleRelationProperty)
   End If
   '
   Dim tmpRelacija As CustomerVehiclesRelation = _
   CustomerVehiclesRelation.GetCustomerVehiclesRelation(IdRelation)
   Dim vInfo As Vehicle = Vehicle.GetVehicle(tmpVehicleId)
   Dim objIsuerList As RegistrationIssuerList
   Dim idCommunityLastReg As Integer = 0
   Try
    objIsuerList = CType(Csla.ApplicationContext.LocalContext.Item("objRegistrationIssuerList"), RegistrationIssuerList)
    Dim CitiesI As CityInfo = _
CType(Csla.ApplicationContext.LocalContext.Item("objCityList"), CityList). _
GetCityListById(Customer.GetCustomer(tmpRelacija.IdCustomer).IdLivingCity)
    'objIsuerList = CType(Csla.ApplicationContext.LocalContext.Item("objRegistrationIssuerList"), RegistrationIssuerList)
    idCommunityLastReg = (CitiesI.IdCommunity)
   Catch ex As Exception
    objIsuerList = RegistrationIssuerList.GetRegistrationIssuerList 'CType(Csla.ApplicationContext.LocalContext.Item("objRegistrationIssuerList"), RegistrationIssuerList)
    idCommunityLastReg = 0 'objIsuerList.GetRegistrationIssuerInfo(vInfo.LastIdRegistrationIssuer).IdCommunity
   End Try
   'filtriraj go katalogot so paramaetri:
   '1. TrigerdBy? (Request)
   '2. IdVehicleCategoryForPayments (od vozilito)
   '3. prebaraj dali konkretnoto vozilo spaga vo nekoi od tie
   Dim pCatalog As PaymentCataologList = _
     CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
     PaymentCataologList).GetPaymentForDepts("TrigerdByRequest", vInfo)
   'zapamti gi site plakanje vo listata
   For Each pInfo As PaymentCataologInfo In pCatalog
    'vo sluaj poedinecno(so formula)
    If (pInfo.ParametarFrom = 0) AndAlso (pInfo.ParametarTo = 0) Then
     If pInfo.IdCommunity > 0 Then
      If idCommunityLastReg = pInfo.IdCommunity Then
       Using cm As SqlCommand = cn.CreateCommand
        cm.CommandType = CommandType.StoredProcedure
        cm.CommandText = "addCustomerFinancialStatFromDocument"

        cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", IdRelation)
        cm.Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdProperty))
        cm.Parameters.AddWithValue("@note", "по барање бр." & ReadProperty(Of Long)(IdProperty))
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
      End If
     Else
      Using cm As SqlCommand = cn.CreateCommand
       cm.CommandType = CommandType.StoredProcedure
       cm.CommandText = "addCustomerFinancialStatFromDocument"

       cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", IdRelation)
       cm.Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdProperty))
       cm.Parameters.AddWithValue("@note", "по барање бр." & ReadProperty(Of Long)(IdProperty))
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
     End If

    Else
     If pInfo.IdCommunity > 0 Then
      If idCommunityLastReg = pInfo.IdCommunity Then
       Using cm As SqlCommand = cn.CreateCommand
        cm.CommandType = CommandType.StoredProcedure
        cm.CommandText = "addCustomerFinancialStatFromDocument"
        cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", IdRelation)
        cm.Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdProperty))
        cm.Parameters.AddWithValue("@note", "по барање бр." & ReadProperty(Of Long)(IdProperty))
        cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)
        cm.Parameters.AddWithValue("@Price", pInfo.Price)
        cm.Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
        cm.ExecuteNonQuery()
       End Using
      End If
     Else
      Using cm As SqlCommand = cn.CreateCommand
       cm.CommandType = CommandType.StoredProcedure
       cm.CommandText = "addCustomerFinancialStatFromDocument"
       cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", IdRelation)
       cm.Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdProperty))
       cm.Parameters.AddWithValue("@note", "по барање бр." & ReadProperty(Of Long)(IdProperty))
       cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)
       cm.Parameters.AddWithValue("@Price", pInfo.Price)
       cm.Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
       cm.ExecuteNonQuery()
      End Using
     End If

    End If
   Next

   OnCustomerFinanceSaved(Me, New Csla.Core.SavedEventArgs(Me))

   'MsgBox("da")
  Catch ex As Exception
   MsgBox(ex.Message)
  End Try

 End Sub
#End Region

#Region " Readonlylist refresh "
 Public Shared Event RequestSaved As EventHandler(Of Csla.Core.SavedEventArgs)
 Protected Shared Sub OnRequestSaved(ByVal sender As Request, ByVal e As Csla.Core.SavedEventArgs)
  RaiseEvent RequestSaved(sender, e)
 End Sub
 Public Shared Event CustomerFinanceSaved As EventHandler(Of Csla.Core.SavedEventArgs)
 Protected Shared Sub OnCustomerFinanceSaved(ByVal sender As Request, ByVal e As Csla.Core.SavedEventArgs)
  RaiseEvent CustomerFinanceSaved(sender, e)
 End Sub
#End Region
End Class
