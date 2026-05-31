
<Serializable()> _
Public Class DocumentsInternationalDriveingLicence
 Inherits Csla.BusinessBase(Of DocumentsInternationalDriveingLicence)

#Region " Stored Procedures Names "
 Private Const spGetByID As String = "GetDocumentsInternationalDriveingLicenceByID"
 Private Const spGetAll As String = "GetDocumentsInternationalDriveingLicences"
 Private Const spUpdate As String = "updateDocumentsInternationalDriveingLicence"
 Private Const spAdd As String = "addDocumentsInternationalDriveingLicence"
 Private Const spDelete As String = "deleteDocumentsInternationalDriveingLicence"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
 'register properties
 Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsInternationalDriveingLicence), New PropertyInfo(Of Long)("Id"))
 Private Shared IdCustomerProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsInternationalDriveingLicence), New PropertyInfo(Of Long)("IdCustomer"))
 Private Shared IdOperatorCreatedProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsInternationalDriveingLicence), New PropertyInfo(Of Integer)("IdOperatorCreated"))
 Private Shared IdIssuerProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsInternationalDriveingLicence), New PropertyInfo(Of Integer)("IdIssuer"))
 Private Shared NumberOfLicenceProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsInternationalDriveingLicence), New PropertyInfo(Of String)("NumberOfLicence"))
 Private Shared NumberOfNationalLicenceProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsInternationalDriveingLicence), New PropertyInfo(Of String)("NumberOfNationalLicence"))
 Private Shared DateCreatedProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(DocumentsInternationalDriveingLicence), New PropertyInfo(Of SmartDate)("DateCreated", New SmartDate(DateTime.Today, True)))
 Private Shared ValidTillDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(DocumentsInternationalDriveingLicence), New PropertyInfo(Of SmartDate)("ValidTillDate", New SmartDate(DateTime.Today, True)))
 Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsInternationalDriveingLicence), New PropertyInfo(Of String)("Note"))
 Private Shared ValidForCategoriesProperty As  _
 PropertyInfo(Of DocumentsInternationalDriveingLicenceValidForCategories) _
 = RegisterProperty(Of DocumentsInternationalDriveingLicenceValidForCategories) _
 (GetType(DocumentsInternationalDriveingLicence), New PropertyInfo(Of  _
 DocumentsInternationalDriveingLicenceValidForCategories)("ValidForCategories"))
 Private _lastChanged(7) As Byte

 <System.ComponentModel.DataObjectField(True, True)> _
 Public ReadOnly Property Id() As Long
  Get
   Return GetProperty(Of Long)(IdProperty)
  End Get
 End Property
 Public Property IdCustomer() As Long
  Get
   Return GetProperty(Of Long)(IdCustomerProperty)
  End Get
  Set(ByVal value As Long)
   SetProperty(Of Long)(IdCustomerProperty, value)
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
 Public Property NumberOfLicence() As String
  Get
   Return GetProperty(Of String)(NumberOfLicenceProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(NumberOfLicenceProperty, value)
  End Set
 End Property
 Public Property NumberOfNationalLicence() As String
  Get
   Return GetProperty(Of String)(NumberOfNationalLicenceProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(NumberOfNationalLicenceProperty, value)
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
 Public Property ValidTillDate() As Date
  Get
   Return GetProperty(Of SmartDate, Date)(ValidTillDateProperty)
  End Get
  Set(ByVal value As Date)
   SetProperty(Of SmartDate, Date)(ValidTillDateProperty, value)
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

 Public Property ValidForCategories() As DocumentsInternationalDriveingLicenceValidForCategories
  Get
   If Not FieldManager.FieldExists(ValidForCategoriesProperty) Then
    SetProperty(Of DocumentsInternationalDriveingLicenceValidForCategories) _
    (ValidForCategoriesProperty, DocumentsInternationalDriveingLicenceValidForCategories.NewDocumentsInternationalDriveingLicenceValidForCategories)
   End If
   Return GetProperty(Of DocumentsInternationalDriveingLicenceValidForCategories)(ValidForCategoriesProperty)
  End Get
  Set(ByVal value As DocumentsInternationalDriveingLicenceValidForCategories)
   SetProperty(Of DocumentsInternationalDriveingLicenceValidForCategories)(ValidForCategoriesProperty, value)
  End Set
 End Property
 Public Overrides ReadOnly Property IsValid() As Boolean
  Get
   Return MyBase.IsValid AndAlso Me.ValidForCategories.IsValid
  End Get
 End Property
 Public Overrides ReadOnly Property IsDirty() As Boolean
  Get
   Return MyBase.IsDirty OrElse Me.ValidForCategories.IsDirty
  End Get
 End Property
 Public Overrides Function ToString() As String
  Return Id.ToString
 End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

 Protected Overrides Sub AddAuthorizationRules()
  Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCustomer") Then
   AuthorizationRules.AllowWrite("IdCustomer", roleName)
  Else
   AuthorizationRules.DenyWrite("IdCustomer", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdCustomer")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdOperatorCreated") Then
   AuthorizationRules.AllowWrite("IdOperatorCreated", roleName)
  Else
   AuthorizationRules.DenyWrite("IdOperatorCreated", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdOperatorCreated")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdIssuer") Then
   AuthorizationRules.AllowWrite("IdIssuer", roleName)
  Else
   AuthorizationRules.DenyWrite("IdIssuer", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdIssuer")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NumberOfLicence") Then
   AuthorizationRules.AllowWrite("NumberOfLicence", roleName)
  Else
   AuthorizationRules.DenyWrite("NumberOfLicence", roleName)
  End If
  'AuthorizationRules.AllowWrite("NumberOfLicence")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NumberOfNationalLicence") Then
   AuthorizationRules.AllowWrite("NumberOfNationalLicence", roleName)
  Else
   AuthorizationRules.DenyWrite("NumberOfNationalLicence", roleName)
  End If
  'AuthorizationRules.AllowWrite("NumberOfNationalLicence")
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

 End Sub


 Public Shared Function CanGetObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsInternationalDriveingLicence")
 End Function

 Public Shared Function CanAddObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsInternationalDriveingLicence")
 End Function

 Public Shared Function CanEditObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsInternationalDriveingLicence")
 End Function

 Public Shared Function CanDeleteObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsInternationalDriveingLicence")
 End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
 Private Sub DocumentsInternationalDriveingLicence_ChildChanged(ByVal sender As Object, ByVal e As Csla.Core.ChildChangedEventArgs) Handles Me.ChildChanged
  Try

   'Console.WriteLine(e.ChildObject.GetType)
   If TypeOf (e.ChildObject) Is DocumentsInternationalDriveingLicenceValidForCategories Then
    ValidationRules.CheckRules(ValidForCategoriesProperty)
   End If

  Catch ex As Exception
   MsgBox(ex.Message())
  End Try

 End Sub
 Protected Overrides Sub AddBusinessRules()
  ' NumberOfLicenceProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, NumberOfLicenceProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NumberOfLicenceProperty, 50))
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, NumberOfNationalLicenceProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NumberOfNationalLicenceProperty, 50))
  ' DateCreatedProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, DateCreatedProperty)
  ' ValidTillDateProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ValidTillDateProperty)

  ' NoteProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NoteProperty, 250))

  ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                          New Validation.IntegerMinValueRuleArgs(IdCustomerProperty, 1))
  ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                          New Validation.IntegerMinValueRuleArgs(IdIssuerProperty, 1))
  ValidationRules.AddRule(Of DocumentsInternationalDriveingLicence)(AddressOf Dates, DateCreatedProperty)
  ValidationRules.AddRule(Of DocumentsInternationalDriveingLicence)(AddressOf Dates, ValidTillDateProperty)
  ValidationRules.AddDependentProperty(DateCreatedProperty, ValidTillDateProperty, True)

  ValidationRules.AddRule(Of DocumentsInternationalDriveingLicence)(AddressOf NoDuplicates, NumberOfLicenceProperty)

  ValidationRules.AddRule(Of DocumentsInternationalDriveingLicence)(AddressOf ProveriDeca, ValidForCategoriesProperty)
 End Sub

 Private Shared Function NoDuplicates(Of T As DocumentsInternationalDriveingLicence)(ByVal target As T, _
  ByVal e As Csla.Validation.RuleArgs) As Boolean
  If DocumentsInternationalDriveingLicence.InternationLicenceExists(target.NumberOfLicence, target.Id) Then
   e.Description = "Бројот на дозволата мора да биде единствен"
   Return False
  Else
   Return True
  End If
 End Function

 Private Shared Function ProveriDeca(Of T As DocumentsInternationalDriveingLicence)(ByVal target As T, _
ByVal e As Csla.Validation.RuleArgs) As Boolean
  Dim chekiranoBaremEdno As Boolean = False
  For Each child As DocumentsInternationalDriveingLicenceValidForCategorie In target.ValidForCategories
   If child.IsCheck Then
    chekiranoBaremEdno = True
    Exit For
   End If
  Next
  If chekiranoBaremEdno Then
   Return True
  Else
   e.Description = "Изберете барем една категорија"
   Return False
  End If
 End Function
 Private Shared Function Dates(Of T As DocumentsInternationalDriveingLicence)(ByVal target As T, _
  ByVal e As Csla.Validation.RuleArgs) As Boolean
  If target.DateCreated > target.ValidTillDate Then
   e.Description = "Проверете ја важноста на дозволата (до кој датум е)"
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

 Public Shared Function NewDocumentsInternationalDriveingLicence() As DocumentsInternationalDriveingLicence
  If Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a DocumentsInternationalDriveingLicence")
  End If
  Return DataPortal.Create(Of DocumentsInternationalDriveingLicence)()
 End Function

 Public Shared Function GetDocumentsInternationalDriveingLicence(ByVal id As Long) As DocumentsInternationalDriveingLicence
  If Not CanGetObject() Then
   Throw New System.Security.SecurityException("User not authorized to view a DocumentsInternationalDriveingLicence")
  End If
  Return DataPortal.Fetch(New SingleCriteria(Of DocumentsInternationalDriveingLicence, Integer)(id))
 End Function

 Public Shared Sub DeleteDocumentsInternationalDriveingLicence(ByVal id As Long)
  If Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a DocumentsInternationalDriveingLicence")
  End If
  DataPortal.Delete(New SingleCriteria(Of DocumentsInternationalDriveingLicence, Integer)(id))
 End Sub

 Public Overrides Function Save() As DocumentsInternationalDriveingLicence
  If IsDeleted AndAlso Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a DocumentsInternationalDriveingLicence")
  ElseIf IsNew AndAlso Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a DocumentsInternationalDriveingLicence")
  ElseIf Not CanEditObject() Then
   Throw New System.Security.SecurityException("User not authorized to update a DocumentsInternationalDriveingLicence")
  End If
  Return MyBase.Save()
 End Function
#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Create "

 <RunLocal()> _
 Private Overloads Sub DataPortal_Create()
  Dim catList As DriveingLicenceCtegoryList = DriveingLicenceCtegoryList.GetDriveingLicenceCtegoryList
  For Each catInfo As DriveingLicenceCtegoryInfo In catList
   Dim ch As DocumentsInternationalDriveingLicenceValidForCategorie = Me.ValidForCategories.AddNew
   ch.IdLicenceCategorie = catInfo.Id
  Next

  Me.DateCreated = Now.Date
  Me.ValidTillDate = Now.AddYears(1).Date
  Me.IdIssuer = CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id
  ValidationRules.CheckRules()
 End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of DocumentsInternationalDriveingLicence, Integer))
  Database.LogInfo("DocumentsInternationalDriveingLicence.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = spGetByID
     cm.Parameters.AddWithValue("@Id", criteria.Value)
     Using dr As New SafeDataReader(cm.ExecuteReader)
      dr.Read()

      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Long)(IdCustomerProperty, dr.GetInt64("IdCustomer"))
      LoadProperty(Of Integer)(IdOperatorCreatedProperty, dr.GetInt32("IdOperatorCreated"))
      LoadProperty(Of Integer)(IdIssuerProperty, dr.GetInt32("IdIssuer"))
      LoadProperty(Of String)(NumberOfLicenceProperty, dr.GetString("NumberOfLicence"))
      LoadProperty(Of String)(NumberOfNationalLicenceProperty, dr.GetString("NumberOfNationalLicence"))
      LoadProperty(Of SmartDate, Date?)(DateCreatedProperty, dr.GetSmartDate("DateCreated", True))
      LoadProperty(Of SmartDate, Date?)(ValidTillDateProperty, dr.GetSmartDate("ValidTillDate", True))
      LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
     End Using
    End Using
    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "getDocumentsInternationalDriveingLicencesValidForCategorieByIdInternationalDrivingLicence"
     cm.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
     Using drc As New SafeDataReader(cm.ExecuteReader)
      LoadProperty(Of DocumentsInternationalDriveingLicenceValidForCategories) _
      (ValidForCategoriesProperty, DocumentsInternationalDriveingLicenceValidForCategories.GetDocumentsInternationalDriveingLicenceValidForCategories(drc))
     End Using
    End Using
   End Using

  Catch ex As Exception
   Database.LogException("DocumentsInternationalDriveingLicence.DataPortal_Fetch", ex)
   Throw New DbCslaException("DocumentsInternationalDriveingLicence.DataPortal_Fetch", ex)
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

      .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
      .Parameters.AddWithValue("@IdOperatorCreated", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
      .Parameters.AddWithValue("@IdIssuer", ReadProperty(Of Integer)(IdIssuerProperty))
      .Parameters.AddWithValue("@NumberOfLicence", ReadProperty(Of String)(NumberOfLicenceProperty))
      .Parameters.AddWithValue("@NumberOfNationalLicence", ReadProperty(Of String)(NumberOfNationalLicenceProperty))
      .Parameters.AddWithValue("@DateCreated", ReadProperty(Of SmartDate)(DateCreatedProperty).DBValue)
      .Parameters.AddWithValue("@ValidTillDate", ReadProperty(Of SmartDate)(ValidTillDateProperty).DBValue)
      .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))

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
    'tuka stavi relacija ako nema
    AddDeptsToCustomer(ReadProperty(Of Long)(IdCustomerProperty), cn)
    'update child objects
    FieldManager.UpdateChildren(Me)
    If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
     ApplicationContext.LocalContext.Remove("cn")
    End If
   End Using
  Catch ex As Exception
   Database.LogException("DocumentsInternationalDriveingLicence.DataPortal_Insert", ex)
   Throw New DbCslaException("DocumentsInternationalDriveingLicence.DataPortal_Insert", ex)
  Finally
   Database.LogInfo("DocumentsInternationalDriveingLicence.DataPortal_Insert", GetHashCode())
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
      .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
      .Parameters.AddWithValue("@IdOperatorCreated", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
      .Parameters.AddWithValue("@IdIssuer", ReadProperty(Of Integer)(IdIssuerProperty))
      .Parameters.AddWithValue("@NumberOfLicence", ReadProperty(Of String)(NumberOfLicenceProperty))
      .Parameters.AddWithValue("@NumberOfNationalLicence", ReadProperty(Of String)(NumberOfNationalLicenceProperty))
      .Parameters.AddWithValue("@DateCreated", ReadProperty(Of SmartDate)(DateCreatedProperty).DBValue)
      .Parameters.AddWithValue("@ValidTillDate", ReadProperty(Of SmartDate)(ValidTillDateProperty).DBValue)
      .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
      .Parameters.AddWithValue("@lastChanged", _lastChanged)
      Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)

      .ExecuteNonQuery()

      _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
     End With
    End Using
    'tuka stavi relacija ako nema
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
  DataPortal_Delete(New SingleCriteria(Of DocumentsInternationalDriveingLicence, Integer)(Id))
 End Sub

 Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of DocumentsInternationalDriveingLicence, Integer))
  Database.LogInfo("DocumentsInternationalDriveingLicence.DataPortal_Delete", GetHashCode())
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
   Database.LogException("DocumentsInternationalDriveingLicence.DataPortal_Delete", ex)
   Throw New DbCslaException("DocumentsInternationalDriveingLicence.DataPortal_Delete", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " InternationLicence Exists "

 Public Shared Function InternationLicenceExists(ByVal strLicence As String, ByVal id As Long) As Boolean

  Return InternationLicenceExistsCommand.InternationLicenceExists(strLicence, id)

 End Function

 <Serializable()> _
 Private Class InternationLicenceExistsCommand
  Inherits CommandBase
  Private _licenceNumber As String
  Private _idLicence As Long
  Private _existsInternationLicence As Boolean
  Public ReadOnly Property ExistsInternationLicence() As Boolean
   Get
    Return _existsInternationLicence
   End Get
  End Property

  Public Shared Function InternationLicenceExists(ByVal strLicence As String, ByVal id As Long) As Boolean

   Dim result As InternationLicenceExistsCommand
   result = DataPortal.Execute(Of InternationLicenceExistsCommand)(New InternationLicenceExistsCommand(strLicence, id))
   Return result.ExistsInternationLicence

  End Function

  Private Sub New(ByVal strLicence As String, ByVal id As Long)
   _licenceNumber = strLicence
   _licenceNumber = id
   _existsInternationLicence = False
  End Sub

  Protected Overrides Sub DataPortal_Execute()
   Dim pom As Integer = 0
   Using cn As SqlConnection = Database.VTE_SqlConnection
    'ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "NumOfInternationLicenceExists"
     cm.Parameters.AddWithValue("@licenceNumber", _licenceNumber)
     cm.Parameters.AddWithValue("@idLicence", _idLicence)
     pom = cm.ExecuteScalar
     If pom = 0 Then
      _existsInternationLicence = False
     Else
      _existsInternationLicence = True
     End If
    End Using
   End Using
  End Sub

 End Class

#End Region

#Region " Payment "

 Private Sub AddDeptsToCustomer(ByVal idCustomer As Integer, ByVal cn As SqlConnection)
  Dim rel As Long = CustomerVehiclesRelation.ExistsCustomerOnly(idCustomer)
  If rel = 0 Then
   Dim newRelation As CustomerVehiclesRelation = CustomerVehiclesRelation.NewCustomerVehiclesRelation
   newRelation.IdCustomer = idCustomer
   newRelation.IdVehicle = 0
   newRelation.IdRelationType = 3
   newRelation = newRelation.Save
   rel = newRelation.Id
  End If

  Try

   Dim pCatalog As PaymentCataologList = _
    CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
    PaymentCataologList).GetPaymentForDepts("TrigerdByInternationalDrivierLicence")

   For Each pInfo As PaymentCataologInfo In pCatalog
    'vo sluaj poedinecno(so formula)
    If (pInfo.ParametarFrom = 0) AndAlso (pInfo.ParametarTo = 0) Then


     Using cm As SqlCommand = cn.CreateCommand
      cm.CommandType = CommandType.StoredProcedure
      cm.CommandText = "insertFinancialStatePriceCatalogForIternationalDriveingLicences"
      cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", rel)
      cm.Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdProperty))
      cm.Parameters.AddWithValue("@note", "по меѓународна бр." & ReadProperty(Of Long)(IdProperty))
      cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)

      If pInfo.VehicleField = "Null" Then
       'fiksno
       cm.Parameters.AddWithValue("@Price", pInfo.Price)
       cm.Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
      Else
       'presmetlivo, ne se slucuva vamu
       'cm.Parameters.AddWithValue("@Price", pInfo.Price * CInt(CallByName(vInfo, pInfo.VehicleField, CallType.Get)))
      End If
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
