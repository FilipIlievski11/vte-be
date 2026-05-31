
<Serializable()> _
Public Class Document
  Inherits Csla.BusinessBase(Of Document)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentByID"
  Private Const spGetAll As String = "GetDocuments"
  Private Const spUpdate As String = "updateDocument"
  Private Const spAdd As String = "addDocument"
  Private Const spDelete As String = "deleteDocument"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Document), New PropertyInfo(Of Long)("Id"))
  Private Shared IdDocumentTypeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Document), New PropertyInfo(Of Integer)("IdDocumentType"))
  Private Shared IdDocumentTypeOptionProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Document), New PropertyInfo(Of Integer)("IdDocumentTypeOption"))
  Private Shared IdDocumentTypeOptionDetailProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Document), New PropertyInfo(Of Integer)("IdDocumentTypeOptionDetail"))
  Private Shared IdCustomerVehicleRelationProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Document), New PropertyInfo(Of Long)("IdCustomerVehicleRelation"))
  Private Shared IdCustomerVehicleRelationHistoryProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Document), New PropertyInfo(Of Long)("IdCustomerVehicleRelationHistory"))
  Private Shared IdOperatorCreatedProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Document), New PropertyInfo(Of Integer)("IdOperatorCreated"))
  Private Shared IdOperatorModifiedProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Document), New PropertyInfo(Of Integer)("IdOperatorModified"))
  Private Shared IdOperatorEndedProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Document), New PropertyInfo(Of Integer)("IdOperatorEnded"))
  Private Shared IdTechnicalExamReportProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Document), New PropertyInfo(Of Long)("IdTechnicalExamReport"))
  Private Shared IdPreviousRegistrationProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Document), New PropertyInfo(Of Integer)("IdPreviousRegistration"))
  Private Shared DateCreatedProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(Document), New PropertyInfo(Of SmartDate)("DateCreated", "DateCreated", New SmartDate(DateTime.Today, True)))
  Private Shared DateModifiedProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(Document), New PropertyInfo(Of SmartDate)("DateModified", New SmartDate(True)))
  Private Shared DateEndedProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(Document), New PropertyInfo(Of SmartDate)("DateEnded", New SmartDate(True)))
  Private Shared IdVehicleOwnershipProofProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Document), New PropertyInfo(Of Integer)("IdVehicleOwnershipProof"))
  Private Shared IdPaymentProofProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Document), New PropertyInfo(Of Integer)("IdPaymentProof"))

  Private Shared VehicleOwnershipProofProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Document), New PropertyInfo(Of String)("VehicleOwnershipProof"))
  Private Shared PaymentProofProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Document), New PropertyInfo(Of String)("PaymentProof"))

  Private Shared IdPayAttachmentProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Document), New PropertyInfo(Of Long)("IdPayAttachment"))
  Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Document), New PropertyInfo(Of String)("Note"))

  Private Shared AttachmentsProperty As PropertyInfo(Of DocumentAttachments) = RegisterProperty(New PropertyInfo(Of DocumentAttachments)("Attachments", "Attachments"))
  ''' <Summary>
  ''' Gets and sets the Attachments value.
  ''' </Summary>
  Public ReadOnly Property Attachments() As DocumentAttachments
    Get
      If Not FieldManager.FieldExists(AttachmentsProperty) Then
        SetProperty(Of DocumentAttachments) _
        (AttachmentsProperty, DocumentAttachments.NewDocumentAttachments())
      End If
      Return GetProperty(Of DocumentAttachments)(AttachmentsProperty)
    End Get
  End Property

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Long
    Get
      Return GetProperty(Of Long)(IdProperty)
    End Get
  End Property
  Public Property IdDocumentType() As Integer
    Get
      Return GetProperty(Of Integer)(IdDocumentTypeProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdDocumentTypeProperty, value)
    End Set
  End Property
  Public Property IdDocumentTypeOption() As Integer
    Get
      Return GetProperty(Of Integer)(IdDocumentTypeOptionProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdDocumentTypeOptionProperty, value)
    End Set
  End Property
  Public Property IdDocumentTypeOptionDetail() As Integer
    Get
      Return GetProperty(Of Integer)(IdDocumentTypeOptionDetailProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdDocumentTypeOptionDetailProperty, value)
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
  Public Property IdCustomerVehicleRelationHistory() As Long
    Get
      Return GetProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty)
    End Get
    Set(ByVal value As Long)
      SetProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty, value)
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
  Public Property VehicleOwnershipProof() As String
    Get
      Return GetProperty(Of String)(VehicleOwnershipProofProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(VehicleOwnershipProofProperty, value)
    End Set
  End Property
  Public Property PaymentProof() As String
    Get
      Return GetProperty(Of String)(PaymentProofProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(PaymentProofProperty, value)
    End Set
  End Property

  Public Property IdPaymentProof() As Integer
    Get
      Return GetProperty(Of Integer)(IdPaymentProofProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdPaymentProofProperty, value)
    End Set
  End Property
  Public Property IdVehicleOwnershipProof() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleOwnershipProofProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleOwnershipProofProperty, value)
    End Set
  End Property
  Public Property IdPayAttachment() As Long
    Get
      Return GetProperty(Of Long)(IdPayAttachmentProperty)
    End Get
    Set(ByVal value As Long)
      SetProperty(Of Long)(IdPayAttachmentProperty, value)
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
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdDocumentType") Then
      AuthorizationRules.AllowWrite("IdDocumentType", roleName)
    Else
      AuthorizationRules.DenyWrite("IdDocumentType", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdDocumentType")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdDocumentTypeOption") Then
      AuthorizationRules.AllowWrite("IdDocumentTypeOption", roleName)
    Else
      AuthorizationRules.DenyWrite("IdDocumentTypeOption", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdDocumentTypeOption")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdDocumentTypeOptionDetail") Then
      AuthorizationRules.AllowWrite("IdDocumentTypeOptionDetail", roleName)
    Else
      AuthorizationRules.DenyWrite("IdDocumentTypeOptionDetail", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdDocumentTypeOptionDetail")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCustomerVehicleRelation") Then
      AuthorizationRules.AllowWrite("IdCustomerVehicleRelation", roleName)
    Else
      AuthorizationRules.DenyWrite("IdCustomerVehicleRelation", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdCustomerVehicleRelation")
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
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdTechnicalOperator") Then
      AuthorizationRules.AllowWrite("IdTechnicalOperator", roleName)
    Else
      AuthorizationRules.DenyWrite("IdTechnicalOperator", roleName)
    End If
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdPreviousRegistration") Then
      AuthorizationRules.AllowWrite("IdPreviousRegistration", roleName)
    Else
      AuthorizationRules.DenyWrite("IdPreviousRegistration", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdTechnicalOperator")
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
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("VehicleOwnershipProof") Then
      AuthorizationRules.AllowWrite("VehicleOwnershipProof", roleName)
    Else
      AuthorizationRules.DenyWrite("VehicleOwnershipProof", roleName)
    End If
    'AuthorizationRules.AllowWrite("VehicleOwnershipProof")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdPayAttachment") Then
      AuthorizationRules.AllowWrite("IdPayAttachment", roleName)
    Else
      AuthorizationRules.DenyWrite("IdPayAttachment", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdPayAttachment")
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
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Document")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Document")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Document")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Document")
  End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' DateCreatedProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, DateCreatedProperty)
    ' VehicleOwnershipProofProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(VehicleOwnershipProofProperty, 250))
    ' NoteProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NoteProperty, 250))
    'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    '                     New Validation.IntegerMinValueRuleArgs(IdDocumentType, 0))
    'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    '                     New Validation.IntegerMinValueRuleArgs(IdCustomerVehicleRelationProperty, 0))
    'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    '                     New Validation.IntegerMinValueRuleArgs(IdOperatorCreatedProperty, 0))
    ValidationRules.AddRule(Of Document)(AddressOf IdGraterThan, IdDocumentTypeProperty)
    ValidationRules.AddRule(Of Document)(AddressOf IdGraterThanRelation, IdCustomerVehicleRelationProperty)

    'ValidationRules.AddRule(Of Document)(AddressOf CompareRelations, IdCustomerVehicleRelationProperty)
    'ValidationRules.AddRule(Of Document)(AddressOf CompareRelations, IdCustomerVehicleRelationHistoryProperty)
    'ValidationRules.AddDependentProperty(IdCustomerVehicleRelationProperty, IdCustomerVehicleRelationHistoryProperty, True)
    ValidationRules.AddRule(Of Document)(AddressOf PreviousRegistrationRequired, IdPreviousRegistrationProperty)
    ValidationRules.AddRule(Of Document)(AddressOf PreviousRegistrationRequired, IdDocumentTypeOptionProperty)
    ValidationRules.AddDependentProperty(IdPreviousRegistrationProperty, IdDocumentTypeOptionProperty, True)
  End Sub

  Private Shared Function PreviousRegistrationRequired(Of T As Document) _
    (ByVal target As T, ByVal e As Csla.Validation.RuleArgs) As Boolean
    If (target.IdDocumentTypeOption = 5) AndAlso (target.IdPreviousRegistration = 0) Then
      e.Description = "Одберете претходна регистрација"
      Return False
    End If
    Return True
  End Function

  Private Shared Function CompareRelations(Of T As Document)(ByVal target As T, _
   ByVal e As Csla.Validation.RuleArgs) As Boolean
    If target.IdCustomerVehicleRelation = target.IdCustomerVehicleRelationHistory Then
      e.Description = "Изберете нов сопственик"
      Return False
    Else
      Return True
    End If
  End Function
  Private Shared Function IdGraterThan(Of T As Document)(ByVal target As T, _
   ByVal e As Csla.Validation.RuleArgs) As Boolean
    If target.IdDocumentType < 1 Then
      e.Description = "Изберете тип"
      Return False
    Else
      Return True
    End If
  End Function
  Private Shared Function IdGraterThanRelation(Of T As Document)(ByVal target As T, _
   ByVal e As Csla.Validation.RuleArgs) As Boolean
    If target.IdCustomerVehicleRelation < 1 Then
      e.Description = "Изберете релација"
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

  Public Shared Function NewDocument() As Document
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a Document")
    End If
    Return DataPortal.Create(Of Document)()
  End Function

  Public Shared Function GetDocument(ByVal id As Long) As Document
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a Document")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of Document, Integer)(id))
  End Function

  Public Shared Sub DeleteDocument(ByVal id As Long)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a Document")
    End If
    DataPortal.Delete(New SingleCriteria(Of Document, Integer)(id))
  End Sub

  Public Overrides Function Save() As Document
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a Document")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a Document")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a Document")
    End If
    'Dim result As Document = MyBase.Save()

    'OnDocumentSaved(Me, New Csla.Core.SavedEventArgs(result))

    'Return result
    Return MyBase.Save()
  End Function

#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewDocumentChild() As Document
    Return DataPortal.CreateChild(Of Document)()
  End Function

  Friend Shared Function GetDocument(ByVal dr As SafeDataReader) As Document
    Return DataPortal.FetchChild(Of Document)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of Document, Integer))
    Database.LogInfo("Document.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of Integer)(IdDocumentTypeProperty, dr.GetInt32("IdDocumentType"))
            LoadProperty(Of Integer)(IdDocumentTypeOptionProperty, dr.GetInt32("IdDocumentTypeOption"))
            LoadProperty(Of Integer)(IdDocumentTypeOptionDetailProperty, dr.GetInt32("IdDocumentTypeOptionDetail"))
            LoadProperty(Of Long)(IdCustomerVehicleRelationProperty, dr.GetInt64("IdCustomerVehicleRelation"))
            LoadProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty, dr.GetInt64("IdCustomerVehicleRelationHistory"))
            LoadProperty(Of Integer)(IdOperatorCreatedProperty, dr.GetInt32("IdOperatorCreated"))
            LoadProperty(Of Integer)(IdOperatorModifiedProperty, dr.GetInt32("IdOperatorModified"))
            LoadProperty(Of Integer)(IdOperatorEndedProperty, dr.GetInt32("IdOperatorEnded"))
            LoadProperty(Of Long)(IdTechnicalExamReportProperty, dr.GetInt64("IdTechnicalExamReport"))
            LoadProperty(Of Integer)(IdPreviousRegistrationProperty, dr.GetInt32("IdPreviousRegistration"))
            LoadProperty(Of SmartDate, Date?)(DateCreatedProperty, dr.GetSmartDate("DateCreated", True))
            LoadProperty(Of SmartDate, Date?)(DateModifiedProperty, dr.GetSmartDate("DateModified", True))
            LoadProperty(Of SmartDate, Date?)(DateEndedProperty, dr.GetSmartDate("DateEnded", True))
            LoadProperty(Of Integer)(IdVehicleOwnershipProofProperty, dr.GetInt32("IdVehicleOwnershipProof"))
            LoadProperty(Of Integer)(IdPaymentProofProperty, dr.GetInt32("IdPaymentProof"))

            LoadProperty(Of String)(VehicleOwnershipProofProperty, dr.GetString("VehicleOwnershipProof"))
            LoadProperty(Of String)(PaymentProofProperty, dr.GetString("PaymentProof"))
            LoadProperty(Of Long)(IdPayAttachmentProperty, dr.GetInt64("IdPayAttachment"))
            LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

            dr.NextResult()

            LoadProperty(Of DocumentAttachments)(AttachmentsProperty, DocumentAttachments.GetDocumentAttachments(dr))
          End Using
        End Using

      End Using

    Catch ex As Exception
      Database.LogException("Document.DataPortal_Fetch", ex)
      Throw New DbCslaException("Document.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@IdDocumentType", ReadProperty(Of Integer)(IdDocumentTypeProperty))
            .Parameters.AddWithValue("@IdDocumentTypeOption", ReadProperty(Of Integer)(IdDocumentTypeOptionProperty))
            .Parameters.AddWithValue("@IdDocumentTypeOptionDetail", ReadProperty(Of Integer)(IdDocumentTypeOptionDetailProperty))
            .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
            .Parameters.AddWithValue("@IdCustomerVehicleRelationHistory", ReadProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty))
            .Parameters.AddWithValue("@IdOperatorCreated", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
            .Parameters.AddWithValue("@IdOperatorModified", ReadProperty(Of Integer)(IdOperatorModifiedProperty))
            .Parameters.AddWithValue("@IdOperatorEnded", ReadProperty(Of Integer)(IdOperatorEndedProperty))
            .Parameters.AddWithValue("@IdTechnicalExamReport", ReadProperty(Of Long)(IdTechnicalExamReportProperty))
            .Parameters.AddWithValue("@IdPreviousRegistration", ReadProperty(Of Integer)(IdPreviousRegistrationProperty))
            .Parameters.AddWithValue("@DateCreated", ReadProperty(Of SmartDate)(DateCreatedProperty).DBValue)
            .Parameters.AddWithValue("@DateModified", ReadProperty(Of SmartDate)(DateModifiedProperty).DBValue)
            .Parameters.AddWithValue("@DateEnded", ReadProperty(Of SmartDate)(DateEndedProperty).DBValue)
            .Parameters.AddWithValue("@IdVehicleOwnershipProof", ReadProperty(Of Integer)(IdVehicleOwnershipProofProperty))
            .Parameters.AddWithValue("@IdPaymentProof", ReadProperty(Of Integer)(IdPaymentProofProperty))
            .Parameters.AddWithValue("@VehicleOwnershipProof", ReadProperty(Of String)(VehicleOwnershipProofProperty))
            .Parameters.AddWithValue("@PaymentProof", ReadProperty(Of String)(PaymentProofProperty))
            .Parameters.AddWithValue("@IdPayAttachment", ReadProperty(Of Long)(IdPayAttachmentProperty))
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

        AddDeptsToCustomer(cn)

        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If

      End Using
    Catch ex As Exception
      Database.LogException("Document.DataPortal_Insert", ex)
      Throw New DbCslaException("Document.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("Document.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@IdDocumentType", ReadProperty(Of Integer)(IdDocumentTypeProperty))
            .Parameters.AddWithValue("@IdDocumentTypeOption", ReadProperty(Of Integer)(IdDocumentTypeOptionProperty))
            .Parameters.AddWithValue("@IdDocumentTypeOptionDetail", ReadProperty(Of Integer)(IdDocumentTypeOptionDetailProperty))
            .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
            .Parameters.AddWithValue("@IdCustomerVehicleRelationHistory", ReadProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty))
            .Parameters.AddWithValue("@IdOperatorCreated", ReadProperty(Of Integer)(IdOperatorCreatedProperty))
            .Parameters.AddWithValue("@IdOperatorModified", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
            .Parameters.AddWithValue("@IdOperatorEnded", ReadProperty(Of Integer)(IdOperatorEndedProperty))
            .Parameters.AddWithValue("@IdTechnicalExamReport", ReadProperty(Of Long)(IdTechnicalExamReportProperty))
            .Parameters.AddWithValue("@IdPreviousRegistration", ReadProperty(Of Integer)(IdPreviousRegistrationProperty))
            .Parameters.AddWithValue("@DateCreated", ReadProperty(Of SmartDate)(DateCreatedProperty).DBValue)
            .Parameters.AddWithValue("@DateModified", DateTime.Now)
            If ReadProperty(Of Integer)(IdOperatorEndedProperty) > 0 Then
              .Parameters.AddWithValue("@DateEnded", Now)
            Else
              .Parameters.AddWithValue("@DateEnded", ReadProperty(Of SmartDate)(DateEndedProperty).DBValue)
            End If
            .Parameters.AddWithValue("@IdVehicleOwnershipProof", ReadProperty(Of Integer)(IdVehicleOwnershipProofProperty))
            .Parameters.AddWithValue("@IdPaymentProof", ReadProperty(Of Integer)(IdPaymentProofProperty))
            .Parameters.AddWithValue("@VehicleOwnershipProof", ReadProperty(Of String)(VehicleOwnershipProofProperty))
            .Parameters.AddWithValue("@PaymentProof", ReadProperty(Of String)(PaymentProofProperty))
            .Parameters.AddWithValue("@IdPayAttachment", ReadProperty(Of Long)(IdPayAttachmentProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
            .Parameters.AddWithValue("@lastChanged", _lastChanged)
            Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        If ReadProperty(Of Integer)(IdOperatorEndedProperty) > 0 Then
          If ReadProperty(Of Integer)(IdDocumentTypeOptionDetailProperty) > 0 Then
            Using cm1 As SqlCommand = cn.CreateCommand
              cm1.CommandType = CommandType.StoredProcedure
              cm1.CommandText = "updateCustomerVehiclesRelationsByDocumentTypeOptionDetail"
              cm1.Parameters.AddWithValue("@docTypeOptionDetail", ReadProperty(Of Integer)(IdDocumentTypeOptionDetailProperty))
              cm1.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
              cm1.Parameters.AddWithValue("@IdCustomerVehicleRelationHistory", ReadProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty))
              cm1.Parameters.AddWithValue("@TerminationNote", _
              DocumentTypesOptionsDetailsList.GetDocumentTypesOptionsDetailsList.GetDocumentTypesOptionsDetailById(ReadProperty(Of Integer)(IdDocumentTypeOptionDetailProperty)).Name)

              cm1.ExecuteNonQuery()
            End Using
          Else
            If ReadProperty(Of Integer)(IdDocumentTypeOptionProperty) > 0 Then
              Using cm1 As SqlCommand = cn.CreateCommand
                cm1.CommandType = CommandType.StoredProcedure
                cm1.CommandText = "updateCustomerVehiclesRelationsByDocumentTypeOption"
                cm1.Parameters.AddWithValue("@docTypeOption", ReadProperty(Of Integer)(IdDocumentTypeOptionProperty))
                cm1.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
                cm1.Parameters.AddWithValue("@IdCustomerVehicleRelationHistory", ReadProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty))
                cm1.Parameters.AddWithValue("@TerminationNote", _
               DocumentTypesOptionsList.GetDocumentTypesOptionsList.GetDocumentTypesOptionsListById(ReadProperty(Of Integer)(IdDocumentTypeOptionProperty)).OptionName)

                cm1.ExecuteNonQuery()
              End Using
            End If
          End If
          Using cm1 As SqlCommand = cn.CreateCommand
            cm1.CommandType = CommandType.StoredProcedure
            cm1.CommandText = "updateDocumentsTrafficLicenceSetEndDtae"
            If (ReadProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty) > 0) Then
              cm1.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty))
            Else
              cm1.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
            End If
            cm1.Parameters.AddWithValue("@docTypeOption", ReadProperty(Of Integer)(IdDocumentTypeOptionProperty))
            cm1.ExecuteNonQuery()
          End Using
        End If
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
    DataPortal_Delete(New SingleCriteria(Of Document, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of Document, Integer))
    Database.LogInfo("Document.DataPortal_Delete", GetHashCode())
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
        'izbrisi go zadolzuvanjeto na kom
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = "deleteCustomerDept"
            .Parameters.AddWithValue("@IdDocument", criteria.Value)
            .ExecuteNonQuery()
          End With
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("Document.DataPortal_Delete", ex)
      Throw New DbCslaException("Document.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("Document.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Integer)(IdDocumentTypeProperty, dr.GetInt32("IdDocumentType"))
      LoadProperty(Of Integer)(IdDocumentTypeOptionProperty, dr.GetInt32("IdDocumentTypeOption"))
      LoadProperty(Of Integer)(IdDocumentTypeOptionDetailProperty, dr.GetInt32("IdDocumentTypeOptionDetail"))
      LoadProperty(Of Long)(IdCustomerVehicleRelationProperty, dr.GetInt64("IdCustomerVehicleRelation"))
      LoadProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty, dr.GetInt64("IdCustomerVehicleRelationHistory"))
      LoadProperty(Of Integer)(IdOperatorCreatedProperty, dr.GetInt32("IdOperatorCreated"))
      LoadProperty(Of Integer)(IdOperatorModifiedProperty, dr.GetInt32("IdOperatorModified"))
      LoadProperty(Of Integer)(IdOperatorEndedProperty, dr.GetInt32("IdOperatorEnded"))
      LoadProperty(Of Long)(IdTechnicalExamReportProperty, dr.GetInt32("IdTechnicalExamReport"))
      LoadProperty(Of Integer)(IdPreviousRegistrationProperty, dr.GetInt32("IdPreviosRegistration"))
      LoadProperty(Of SmartDate, Date?)(DateCreatedProperty, dr.GetSmartDate("DateCreated", True))
      LoadProperty(Of SmartDate, Date?)(DateModifiedProperty, dr.GetSmartDate("DateModified", True))
      LoadProperty(Of SmartDate, Date?)(DateEndedProperty, dr.GetSmartDate("DateEnded", True))
      LoadProperty(Of Integer)(IdVehicleOwnershipProofProperty, dr.GetInt32("IdVehicleOwnershipProof"))
      LoadProperty(Of Integer)(IdPaymentProofProperty, dr.GetInt32("IdPaymentProof"))

      LoadProperty(Of String)(VehicleOwnershipProofProperty, dr.GetString("VehicleOwnershipProof"))
      LoadProperty(Of String)(PaymentProofProperty, dr.GetString("PaymentProof"))
      LoadProperty(Of Long)(IdPayAttachmentProperty, dr.GetInt64("IdPayAttachment"))
      LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("Document.Child_Fetch", ex)
      Throw New DbCslaException("Document.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@IdDocumentType", ReadProperty(Of Integer)(IdDocumentTypeProperty))
            .Parameters.AddWithValue("@IdDocumentTypeOption", ReadProperty(Of Integer)(IdDocumentTypeOptionProperty))
            .Parameters.AddWithValue("@IdDocumentTypeOptionDetail", ReadProperty(Of Integer)(IdDocumentTypeOptionDetailProperty))
            .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
            .Parameters.AddWithValue("@IdCustomerVehicleRelationHistory", ReadProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty))
            .Parameters.AddWithValue("@IdOperatorCreated", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
            .Parameters.AddWithValue("@IdOperatorModified", ReadProperty(Of Integer)(IdOperatorModifiedProperty))
            .Parameters.AddWithValue("@IdOperatorEnded", ReadProperty(Of Integer)(IdOperatorEndedProperty))
            .Parameters.AddWithValue("@IdTechnicalExamReport", ReadProperty(Of Long)(IdTechnicalExamReportProperty))
            .Parameters.AddWithValue("@IdPreviousRegistration", ReadProperty(Of Integer)(IdPreviousRegistrationProperty))
            .Parameters.AddWithValue("@DateCreated", ReadProperty(Of SmartDate)(DateCreatedProperty).DBValue)
            .Parameters.AddWithValue("@DateModified", ReadProperty(Of SmartDate)(DateModifiedProperty).DBValue)
            .Parameters.AddWithValue("@DateEnded", ReadProperty(Of SmartDate)(DateEndedProperty).DBValue)
            .Parameters.AddWithValue("@IdVehicleOwnershipProof", ReadProperty(Of Integer)(IdVehicleOwnershipProofProperty))
            .Parameters.AddWithValue("@IdPaymentProof", ReadProperty(Of Integer)(IdPaymentProofProperty))
            .Parameters.AddWithValue("@VehicleOwnershipProof", ReadProperty(Of String)(VehicleOwnershipProofProperty))
            .Parameters.AddWithValue("@PaymentProof", ReadProperty(Of String)(PaymentProofProperty))
            .Parameters.AddWithValue("@IdPayAttachment", ReadProperty(Of Long)(IdPayAttachmentProperty))
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

        'zadolzuvanje po izvrsena usluga, po IdCustomerVehicle
        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = "insertFinancialStatePriceCatalogForRequests"
          cm1.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
          cm1.Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdProperty))
          cm1.Parameters.AddWithValue("@note", "по барање бр." & ReadProperty(Of Long)(IdProperty))
          cm1.ExecuteNonQuery()
        End Using

        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("Document.Child_Insert", ex)
      Throw New DbCslaException("Document.Child_Insert", ex)
    Finally
      Database.LogInfo("Document.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("Document.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
            .Parameters.AddWithValue("@IdDocumentType", ReadProperty(Of Integer)(IdDocumentTypeProperty))
            .Parameters.AddWithValue("@IdDocumentTypeOption", ReadProperty(Of Integer)(IdDocumentTypeOptionProperty))
            .Parameters.AddWithValue("@IdDocumentTypeOptionDetail", ReadProperty(Of Integer)(IdDocumentTypeOptionDetailProperty))
            .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
            .Parameters.AddWithValue("@IdCustomerVehicleRelationHistory", ReadProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty))
            .Parameters.AddWithValue("@IdOperatorCreated", ReadProperty(Of Integer)(IdOperatorCreatedProperty))
            .Parameters.AddWithValue("@IdOperatorModified", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
            .Parameters.AddWithValue("@IdOperatorEnded", ReadProperty(Of Integer)(IdOperatorEndedProperty))
            .Parameters.AddWithValue("@IdTechnicalExamReport", ReadProperty(Of Long)(IdTechnicalExamReportProperty))
            .Parameters.AddWithValue("@IdPreviousRegistration", ReadProperty(Of Integer)(IdPreviousRegistrationProperty))
            .Parameters.AddWithValue("@DateCreated", ReadProperty(Of SmartDate)(DateCreatedProperty).DBValue)
            .Parameters.AddWithValue("@DateModified", DateTime.Now)
            If ReadProperty(Of Integer)(IdOperatorEndedProperty) > 0 Then
              .Parameters.AddWithValue("@DateEnded", Now)
            Else
              .Parameters.AddWithValue("@DateEnded", ReadProperty(Of SmartDate)(DateEndedProperty).DBValue)
            End If
            .Parameters.AddWithValue("@IdVehicleOwnershipProof", ReadProperty(Of Integer)(IdVehicleOwnershipProofProperty))
            .Parameters.AddWithValue("@IdPaymentProof", ReadProperty(Of Integer)(IdPaymentProofProperty))
            .Parameters.AddWithValue("@VehicleOwnershipProof", ReadProperty(Of String)(VehicleOwnershipProofProperty))
            .Parameters.AddWithValue("@PaymentProof", ReadProperty(Of String)(PaymentProofProperty))
            .Parameters.AddWithValue("@IdPayAttachment", ReadProperty(Of Long)(IdPayAttachmentProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
            .Parameters.AddWithValue("@lastChanged", _lastChanged)
            Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using

        If ReadProperty(Of Integer)(IdOperatorEndedProperty) > 0 Then
          If ReadProperty(Of Integer)(IdDocumentTypeOptionDetailProperty) > 0 Then
            Using cm1 As SqlCommand = cn.CreateCommand
              cm1.CommandType = CommandType.StoredProcedure
              cm1.CommandText = "updateCustomerVehiclesRelationsByDocumentTypeOptionDetail"
              cm1.Parameters.AddWithValue("@docTypeOptionDetail", ReadProperty(Of Integer)(IdDocumentTypeOptionDetailProperty))
              cm1.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
              cm1.Parameters.AddWithValue("@IdCustomerVehicleRelationHistory", ReadProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty))
              cm1.Parameters.AddWithValue("@TerminationNote", _
              DocumentTypesOptionsDetailsList.GetDocumentTypesOptionsDetailsList.GetDocumentTypesOptionsDetailById(ReadProperty(Of Integer)(IdDocumentTypeOptionDetailProperty)).Name)

              cm1.ExecuteNonQuery()
            End Using
          Else
            Using cm1 As SqlCommand = cn.CreateCommand
              cm1.CommandType = CommandType.StoredProcedure
              cm1.CommandText = "updateCustomerVehiclesRelationsByDocumentTypeOption"
              cm1.Parameters.AddWithValue("@docTypeOption", ReadProperty(Of Integer)(IdDocumentTypeOptionProperty))
              cm1.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
              cm1.Parameters.AddWithValue("@IdCustomerVehicleRelationHistory", ReadProperty(Of Long)(IdCustomerVehicleRelationHistoryProperty))
              cm1.Parameters.AddWithValue("@TerminationNote", _
             DocumentTypesOptionsList.GetDocumentTypesOptionsList.GetDocumentTypesOptionsListById(ReadProperty(Of Integer)(IdDocumentTypeOptionProperty)).OptionName)

              cm1.ExecuteNonQuery()
            End Using
          End If
        End If

        'update child objects
        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("Document.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("Document.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("Document.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("Document.Child_Fetch", ex)
      Throw New DbCslaException("Document.Child_Fetch", ex)
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
          Using cm As SqlCommand = cn.CreateCommand
            cm.CommandType = CommandType.StoredProcedure
            cm.CommandText = "addCustomerFinancialStatFromDocument"
            cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
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
            cm.ExecuteNonQuery()
          End Using
        Else
          Using cm As SqlCommand = cn.CreateCommand
            cm.CommandType = CommandType.StoredProcedure
            cm.CommandText = "addCustomerFinancialStatFromDocument"
            cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
            cm.Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdProperty))
            cm.Parameters.AddWithValue("@note", "по барање бр." & ReadProperty(Of Long)(IdProperty))
            cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)
            cm.Parameters.AddWithValue("@Price", pInfo.Price)
            cm.ExecuteNonQuery()
          End Using

        End If
      Next


      'MsgBox("da")
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

  End Sub
#End Region

End Class
