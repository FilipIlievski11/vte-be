
<Serializable()> _
Public Class TehnicalExamOrganization
 Inherits Csla.BusinessBase(Of TehnicalExamOrganization)


#Region " Stored Procedures Names "
 Private Const spGetByID As String = "GetTehnicalExamOrganizationByID"
 Private Const spGetAll As String = "GetTehnicalExamOrganizations"
 Private Const spUpdate As String = "updateTehnicalExamOrganization"
 Private Const spAdd As String = "addTehnicalExamOrganization"
 Private Const spDelete As String = "deleteTehnicalExamOrganization"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
 'register properties
 Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Integer)("Id"))
 Private Shared IdCompanyProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Integer)("IdCompany"))
 Private Shared StationProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(TehnicalExamOrganization), New PropertyInfo(Of String)("Station"))
 Private Shared IdCityProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Integer)("IdCity"))
 Private Shared CodeProperty As PropertyInfo(Of String) = _
 RegisterProperty(Of String)(GetType(TehnicalExamOrganization), New PropertyInfo(Of String)("Code"))

 Private Shared ApproveRequestAutomateProperty As PropertyInfo(Of Boolean) = _
 RegisterProperty(Of Boolean)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Boolean)("ApproveRequestAutomate"))

 Private Shared NewTechnicalExamReportProperty As PropertyInfo(Of Boolean) = _
RegisterProperty(Of Boolean)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Boolean)("NewTechnicalExamReportProperty"))

 Private Shared IdCommunityProperty As PropertyInfo(Of Integer) _
 = RegisterProperty(Of Integer)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Integer)("IdCommunity"))

 Private Shared TrafficLicenceVlidNumOfMonthsProperty As PropertyInfo(Of Integer) _
= RegisterProperty(Of Integer)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Integer)("TrafficLicenceVlidNumOfMonths"))

 Private Shared RegistrationVlidNumOfMonthsProperty As PropertyInfo(Of Integer) _
= RegisterProperty(Of Integer)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Integer)("RegistrationVlidNumOfMonths"))


 Private Shared OdgovorenOrganProperty As PropertyInfo(Of String) _
= RegisterProperty(Of String)(GetType(TehnicalExamOrganization), New PropertyInfo(Of String)("OdgovorenOrgan"))

 Private Shared SekretarProperty As PropertyInfo(Of String) _
= RegisterProperty(Of String)(GetType(TehnicalExamOrganization), New PropertyInfo(Of String)("Sekretar"))

 Private Shared IdPaymentPrintOptionProperty As PropertyInfo(Of Integer) _
   = RegisterProperty(Of Integer)(GetType(TehnicalExamOrganization), _
   New PropertyInfo(Of Integer)("IdPaymentPrintOption"))

 Private Shared IdDefaultRegistrationIssuerProperty As PropertyInfo(Of Integer) = _
RegisterProperty(Of Integer)(GetType(TehnicalExamOrganization), _
New PropertyInfo(Of Integer)("IdDefaultRegistrationIssuer"))

 Private Shared IdDefaultCityProperty As PropertyInfo(Of Integer) = _
   RegisterProperty(Of Integer)(GetType(TehnicalExamOrganization), _
   New PropertyInfo(Of Integer)("IdDefaultCity"))

 Private Shared AutmateProcesesProperty As PropertyInfo(Of Boolean) _
   = RegisterProperty(Of Boolean)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Boolean)("AutmateProceses"))

 Private Shared ZiroSmetkaProperty As PropertyInfo(Of String) _
= RegisterProperty(Of String)(GetType(TehnicalExamOrganization), _
New PropertyInfo(Of String)("ZiroSmetka"))

 Private Shared DeponentProperty As PropertyInfo(Of String) _
= RegisterProperty(Of String)(GetType(TehnicalExamOrganization), _
New PropertyInfo(Of String)("Deponent"))

 Private Shared EDBProperty As PropertyInfo(Of String) _
= RegisterProperty(Of String)(GetType(TehnicalExamOrganization), _
New PropertyInfo(Of String)("EDB"))

 Private Shared StationAddressProperty As PropertyInfo(Of String) _
= RegisterProperty(Of String)(GetType(TehnicalExamOrganization), _
New PropertyInfo(Of String)("StationAddress"))

 Private Shared TelProperty As PropertyInfo(Of String) _
= RegisterProperty(Of String)(GetType(TehnicalExamOrganization), _
New PropertyInfo(Of String)("Tel"))

 Private Shared FaxProperty As PropertyInfo(Of String) _
= RegisterProperty(Of String)(GetType(TehnicalExamOrganization), _
New PropertyInfo(Of String)("Fax"))

 Private Shared ValutaProperty As PropertyInfo(Of Integer) _
= RegisterProperty(Of Integer)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Integer)("Valuta"))


 Private Shared PictureServerPathProperty As PropertyInfo(Of String) _
   = RegisterProperty(Of String)(GetType(TehnicalExamOrganization), _
   New PropertyInfo(Of String)("PictureServerPath"))
 Private Shared LogoPathProperty As PropertyInfo(Of String) _
= RegisterProperty(Of String)(GetType(TehnicalExamOrganization), _
New PropertyInfo(Of String)("LogoPath"))

 Private Shared PriceWithTaxProperty As PropertyInfo(Of Boolean) _
  = RegisterProperty(Of Boolean)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Boolean)("PriceWithTax"))
 Private Shared PriceWithoutTaxProperty As PropertyInfo(Of Boolean) _
 = RegisterProperty(Of Boolean)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Boolean)("PriceWithoutTax"))
 Private Shared CalculatePercentOfTehProperty As PropertyInfo(Of Boolean) _
= RegisterProperty(Of Boolean)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Boolean)("CalculatePercentOfTeh"))
 Private Shared PercentOfTehProperty As PropertyInfo(Of Double) _
= RegisterProperty(Of Double)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Double)("PercentOfTeh"))
 Private Shared idTehnicalExamProperty As PropertyInfo(Of Integer) _
= RegisterProperty(Of Integer)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Integer)("idTehnicalExam"))
 Private Shared idStavkaZavisnaOdTehnicalExamProperty As PropertyInfo(Of Integer) _
= RegisterProperty(Of Integer)(GetType(TehnicalExamOrganization), New PropertyInfo(Of Integer)("idStavkaZavisnaOdTehnicalExam"))

 Private _lastChanged(7) As Byte

 <System.ComponentModel.DataObjectField(True, True)> _
 Public ReadOnly Property Id() As Integer
  Get
   Return GetProperty(Of Integer)(IdProperty)
  End Get
 End Property
 Public Property IdCompany() As Integer
  Get
   Return GetProperty(Of Integer)(IdCompanyProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdCompanyProperty, value)
  End Set
 End Property
 Public Property Station() As String
  Get
   Return GetProperty(Of String)(StationProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(StationProperty, value)
  End Set
 End Property
 Public Property IdCity() As Integer
  Get
   Return GetProperty(Of Integer)(IdCityProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdCityProperty, value)
  End Set
 End Property
 Public Property Code() As String
  Get
   Return GetProperty(Of String)(CodeProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(CodeProperty, value)
  End Set
 End Property


 Public Property ApproveRequestAutomate() As Boolean
  Get
   Return GetProperty(Of Boolean)(ApproveRequestAutomateProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(ApproveRequestAutomateProperty, value)
  End Set
 End Property
 Public Property NewTechnicalExamReport() As Boolean
  Get
   Return GetProperty(Of Boolean)(NewTechnicalExamReportProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(NewTechnicalExamReportProperty, value)
  End Set
 End Property
 Public Property IdCommunity() As Integer
  Get
   Return GetProperty(Of Integer)(IdCommunityProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdCommunityProperty, value)
  End Set
 End Property
 Public Property TrafficLicenceVlidNumOfMonths() As Integer
  Get
   Return GetProperty(Of Integer)(TrafficLicenceVlidNumOfMonthsProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(TrafficLicenceVlidNumOfMonthsProperty, value)
  End Set
 End Property
 Public Property RegistrationVlidNumOfMonths() As Integer
  Get
   Return GetProperty(Of Integer)(RegistrationVlidNumOfMonthsProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(RegistrationVlidNumOfMonthsProperty, value)
  End Set
 End Property

 Public Property OdgovorenOrgan() As String
  Get
   Return GetProperty(Of String)(OdgovorenOrganProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(OdgovorenOrganProperty, value)
  End Set
 End Property

 Public Property Sekretar() As String
  Get
   Return GetProperty(Of String)(SekretarProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(SekretarProperty, value)
  End Set
 End Property

 Public Property IdPaymentPrintOption() As Integer
  Get
   Return GetProperty(Of Integer)(IdPaymentPrintOptionProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdPaymentPrintOptionProperty, value)
  End Set
 End Property

 Public Property IdDefaultRegistrationIssuer() As Integer
  Get
   Return GetProperty(Of Integer)(IdDefaultRegistrationIssuerProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdDefaultRegistrationIssuerProperty, value)
  End Set
 End Property

 Public Property IdDefaultCityIssuer() As Integer
  Get
   Return GetProperty(Of Integer)(IdDefaultCityProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdDefaultCityProperty, value)
  End Set
 End Property

 Public Property AutmateProceses() As Boolean
  Get
   Return GetProperty(Of Boolean)(AutmateProcesesProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(AutmateProcesesProperty, value)
  End Set
 End Property

 Public Property ZiroSmetka() As String
  Get
   Return GetProperty(Of String)(ZiroSmetkaProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(ZiroSmetkaProperty, value)
  End Set
 End Property

 Public Property Deponent() As String
  Get
   Return GetProperty(Of String)(DeponentProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(DeponentProperty, value)
  End Set
 End Property

 Public Property EDB() As String
  Get
   Return GetProperty(Of String)(EDBProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(EDBProperty, value)
  End Set
 End Property

 Public Property StationAddress() As String
  Get
   Return GetProperty(Of String)(StationAddressProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(StationAddressProperty, value)
  End Set
 End Property

 Public Property Tel() As String
  Get
   Return GetProperty(Of String)(TelProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(TelProperty, value)
  End Set
 End Property

 Public Property Fax() As String
  Get
   Return GetProperty(Of String)(FaxProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(FaxProperty, value)
  End Set
 End Property

 Public Property Valuta() As Integer
  Get
   Return GetProperty(Of Integer)(ValutaProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(ValutaProperty, value)
  End Set
 End Property

 Public Property PictureServerPath() As String
  Get
   Return GetProperty(Of String)(PictureServerPathProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(PictureServerPathProperty, value)
  End Set
 End Property

 Public Property LogoPath() As String
  Get
   Return GetProperty(Of String)(LogoPathProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(LogoPathProperty, value)
  End Set
 End Property

 Public Property PriceWithTax() As Boolean
  Get
   Return GetProperty(Of Boolean)(PriceWithTaxProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(PriceWithTaxProperty, value)
  End Set
 End Property

 Public Property PriceWithoutTax() As Boolean
  Get
   Return GetProperty(Of Boolean)(PriceWithoutTaxProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(PriceWithoutTaxProperty, value)
  End Set
 End Property

 Public Property CalculatePercentOfTeh() As Boolean
  Get
   Return GetProperty(Of Boolean)(CalculatePercentOfTehProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(CalculatePercentOfTehProperty, value)
  End Set
 End Property
 Public Property PercentOfTeh() As Double
  Get
   Return GetProperty(Of Double)(PercentOfTehProperty)
  End Get
  Set(ByVal value As Double)
   SetProperty(Of Double)(PercentOfTehProperty, value)
  End Set
 End Property
 Public Property idTehnicalExam() As Integer
  Get
   Return GetProperty(Of Integer)(idTehnicalExamProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(idTehnicalExamProperty, value)
  End Set
 End Property
 Public Property idStavkaZavisnaOdTehnicalExam() As Integer
  Get
   Return GetProperty(Of Integer)(idStavkaZavisnaOdTehnicalExamProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(idStavkaZavisnaOdTehnicalExamProperty, value)
  End Set
 End Property

 Public Overrides Function ToString() As String
  Return Id.ToString
 End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

 Protected Overrides Sub AddAuthorizationRules()
  Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("OrganizationName") Then
   AuthorizationRules.AllowWrite("OrganizationName", roleName)
  Else
   AuthorizationRules.DenyWrite("OrganizationName", roleName)
  End If
  'AuthorizationRules.AllowWrite("OrganizationName")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Station") Then
   AuthorizationRules.AllowWrite("Station", roleName)
  Else
   AuthorizationRules.DenyWrite("Station", roleName)
  End If
  'AuthorizationRules.AllowWrite("Station")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCity") Then
   AuthorizationRules.AllowWrite("IdCity", roleName)
  Else
   AuthorizationRules.DenyWrite("IdCity", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdCity")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
   AuthorizationRules.AllowWrite("Active", roleName)
  Else
   AuthorizationRules.DenyWrite("Active", roleName)
  End If
  'AuthorizationRules.AllowWrite("Active")
 End Sub



 Public Shared Function CanGetObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("TehnicalExamOrganization")
 End Function

 Public Shared Function CanAddObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("TehnicalExamOrganization")
 End Function

 Public Shared Function CanEditObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("TehnicalExamOrganization")
 End Function

 Public Shared Function CanDeleteObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("TehnicalExamOrganization")
 End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
 Protected Overrides Sub AddBusinessRules()
  ' OrganizationNameProperty rules
  'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
  '                        New Validation.IntegerMinValueRuleArgs(IdCompanyProperty, 1))
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CodeProperty, 20))
  ' StationProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, StationProperty)
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(StationProperty, 150))
 End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

 Private Sub New()
  ' require use of factory method 
 End Sub

 Public Shared Function NewTehnicalExamOrganization() As TehnicalExamOrganization
  If Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a TehnicalExamOrganization")
  End If
  Return DataPortal.Create(Of TehnicalExamOrganization)()
 End Function

 Public Shared Function GetTehnicalExamOrganization(ByVal id As Integer) As TehnicalExamOrganization
  If Not CanGetObject() Then
   Throw New System.Security.SecurityException("User not authorized to view a TehnicalExamOrganization")
  End If
  Return DataPortal.Fetch(New SingleCriteria(Of TehnicalExamOrganization, Integer)(id))
 End Function

 Public Shared Sub DeleteTehnicalExamOrganization(ByVal id As Integer)
  If Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a TehnicalExamOrganization")
  End If
  DataPortal.Delete(New SingleCriteria(Of TehnicalExamOrganization, Integer)(id))
 End Sub

 Public Overrides Function Save() As TehnicalExamOrganization
  If IsDeleted AndAlso Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a TehnicalExamOrganization")
  ElseIf IsNew AndAlso Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a TehnicalExamOrganization")
  ElseIf Not CanEditObject() Then
   Throw New System.Security.SecurityException("User not authorized to update a TehnicalExamOrganization")
  End If
  Return MyBase.Save()
 End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

 Friend Shared Function NewTehnicalExamOrganizationChild() As TehnicalExamOrganization
  Return DataPortal.CreateChild(Of TehnicalExamOrganization)()
 End Function

 Friend Shared Function GetTehnicalExamOrganization(ByVal dr As SafeDataReader) As TehnicalExamOrganization
  Return DataPortal.FetchChild(Of TehnicalExamOrganization)(dr)
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

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of TehnicalExamOrganization, Integer))
  Database.LogInfo("TehnicalExamOrganization.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = spGetByID
     cm.Parameters.AddWithValue("@Id", criteria.Value)
     Using dr As New SafeDataReader(cm.ExecuteReader)
      dr.Read()

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdCompanyProperty, dr.GetInt32("IdCompany"))
      LoadProperty(Of String)(StationProperty, dr.GetString("Station"))
      LoadProperty(Of Integer)(IdCityProperty, dr.GetInt32("IdCity"))
      LoadProperty(Of String)(CodeProperty, dr.GetString("Code"))
      LoadProperty(Of Integer)(TrafficLicenceVlidNumOfMonthsProperty, dr.GetInt32("TrafficLicenceVlidNumOfMonths"))
      LoadProperty(Of Integer)(RegistrationVlidNumOfMonthsProperty, dr.GetInt32("RegistrationVlidNumOfMonths"))
      LoadProperty(Of Integer)(IdCommunityProperty, dr.GetInt32("IdCommunity"))

      LoadProperty(Of String)(OdgovorenOrganProperty, dr.GetString("OdgovorenOrgan"))
      LoadProperty(Of String)(SekretarProperty, dr.GetString("Sekretar"))
      LoadProperty(Of Integer)(IdPaymentPrintOptionProperty, dr.GetInt32("IdPaymentPrintOption"))
      LoadProperty(Of Integer)(IdDefaultRegistrationIssuerProperty, dr.GetInt32("IdDefaultRegistrationIssuer"))
      LoadProperty(Of Integer)(IdDefaultCityProperty, dr.GetInt32("IdDefaultCity"))
      LoadProperty(Of Boolean)(AutmateProcesesProperty, dr.GetBoolean("AutmateProceses"))
      ' podatoci z astanicata (za pecatenje fakturi)
      LoadProperty(Of String)(ZiroSmetkaProperty, dr.GetString("ZiroSmetka"))
      LoadProperty(Of String)(DeponentProperty, dr.GetString("Deponent"))
      LoadProperty(Of String)(StationAddressProperty, dr.GetString("StationAddress"))
      LoadProperty(Of String)(EDBProperty, dr.GetString("EDB"))
      LoadProperty(Of String)(TelProperty, dr.GetString("Tel"))
      LoadProperty(Of String)(FaxProperty, dr.GetString("Fax"))
      LoadProperty(Of Integer)(ValutaProperty, dr.GetInt32("Valuta"))

      LoadProperty(Of Boolean)(ApproveRequestAutomateProperty, dr.GetBoolean("ApproveRequestAutomate"))
      LoadProperty(Of Boolean)(NewTechnicalExamReportProperty, dr.GetBoolean("NewTechnicalExamReport"))
      LoadProperty(Of String)(LogoPathProperty, dr.GetString("LogoPath"))
      LoadProperty(Of String)(PictureServerPathProperty, dr.GetString("PictureServerPath"))

      LoadProperty(Of Boolean)(PriceWithTaxProperty, dr.GetBoolean("PriceWithTax"))
      LoadProperty(Of Boolean)(PriceWithoutTaxProperty, dr.GetBoolean("PriceWithoutTax"))
      LoadProperty(Of Boolean)(CalculatePercentOfTehProperty, dr.GetBoolean("CalculatePercentOfTeh"))
      LoadProperty(Of Double)(PercentOfTehProperty, dr.GetValue("PercentOfTeh"))
      LoadProperty(Of Integer)(idTehnicalExamProperty, dr.GetInt32("idTehnicalExam"))
      LoadProperty(Of Integer)(idStavkaZavisnaOdTehnicalExamProperty, dr.GetInt32("idStavkaZavisnaOdTehnicalExam"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
     End Using
    End Using
   End Using

  Catch ex As Exception
   Database.LogException("TehnicalExamOrganization.DataPortal_Fetch", ex)
   Throw New DbCslaException("TehnicalExamOrganization.DataPortal_Fetch", ex)
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

      .Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
      .Parameters.AddWithValue("@Station", ReadProperty(Of String)(StationProperty))
      .Parameters.AddWithValue("@IdCity", ReadProperty(Of Integer)(IdCityProperty))
      .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
      .Parameters.AddWithValue("@TrafficLicenceVlidNumOfMonths", ReadProperty(Of Integer)(TrafficLicenceVlidNumOfMonthsProperty))
      .Parameters.AddWithValue("@RegistrationVlidNumOfMonths", ReadProperty(Of Integer)(RegistrationVlidNumOfMonthsProperty))
      .Parameters.AddWithValue("@IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty))

      .Parameters.AddWithValue("@OdgovorenOrgan", ReadProperty(Of String)(OdgovorenOrganProperty))
      .Parameters.AddWithValue("@Sekretar", ReadProperty(Of String)(SekretarProperty))
      .Parameters.AddWithValue("@IdPaymentPrintOption", ReadProperty(Of Integer)(IdPaymentPrintOptionProperty))
      .Parameters.AddWithValue("@IdDefaultRegistrationIssuer", ReadProperty(Of Integer)(IdDefaultRegistrationIssuerProperty))
      .Parameters.AddWithValue("@IdDefaultCity", ReadProperty(Of Integer)(IdDefaultCityProperty))
      .Parameters.AddWithValue("@AutmateProceses", ReadProperty(Of Boolean)(AutmateProcesesProperty))
      .Parameters.AddWithValue("@ZiroSmetka", ReadProperty(Of String)(ZiroSmetkaProperty))
      .Parameters.AddWithValue("@Deponent", ReadProperty(Of String)(DeponentProperty))
      .Parameters.AddWithValue("@StationAddress", ReadProperty(Of String)(StationAddressProperty))
      .Parameters.AddWithValue("@EDB", ReadProperty(Of String)(EDBProperty))
      .Parameters.AddWithValue("@Tel", ReadProperty(Of String)(TelProperty))
      .Parameters.AddWithValue("@Fax", ReadProperty(Of String)(FaxProperty))
      .Parameters.AddWithValue("@Valuta", ReadProperty(Of Integer)(ValutaProperty))
      .Parameters.AddWithValue("@ApproveRequestAutomate", ReadProperty(Of Boolean)(ApproveRequestAutomateProperty))
      .Parameters.AddWithValue("@NewTechnicalExamReport", ReadProperty(Of Boolean)(NewTechnicalExamReportProperty))
      .Parameters.AddWithValue("@LogoPath", ReadProperty(Of String)(LogoPathProperty))
      .Parameters.AddWithValue("@PictureServerPath", ReadProperty(Of String)(PictureServerPathProperty))

      .Parameters.AddWithValue("@PriceWithTax", ReadProperty(Of Boolean)(PriceWithTaxProperty))
      .Parameters.AddWithValue("@PriceWithoutTax", ReadProperty(Of Boolean)(PriceWithoutTaxProperty))
      .Parameters.AddWithValue("@CalculatePercentOfTeh", ReadProperty(Of Boolean)(CalculatePercentOfTehProperty))
      .Parameters.AddWithValue("@PercentOfTeh", ReadProperty(Of Double)(PercentOfTehProperty))
      .Parameters.AddWithValue("@idTehnicalExam", ReadProperty(Of Integer)(idTehnicalExamProperty))
      .Parameters.AddWithValue("@idStavkaZavisnaOdTehnicalExam", ReadProperty(Of Integer)(idStavkaZavisnaOdTehnicalExamProperty))

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
   Database.LogException("TehnicalExamOrganization.DataPortal_Insert", ex)
   Throw New DbCslaException("TehnicalExamOrganization.DataPortal_Insert", ex)
  Finally
   Database.LogInfo("TehnicalExamOrganization.DataPortal_Insert", GetHashCode())
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
      .Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
      .Parameters.AddWithValue("@Station", ReadProperty(Of String)(StationProperty))
      .Parameters.AddWithValue("@IdCity", ReadProperty(Of Integer)(IdCityProperty))
      .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
      .Parameters.AddWithValue("@TrafficLicenceVlidNumOfMonths", ReadProperty(Of Integer)(TrafficLicenceVlidNumOfMonthsProperty))
      .Parameters.AddWithValue("@RegistrationVlidNumOfMonths", ReadProperty(Of Integer)(RegistrationVlidNumOfMonthsProperty))
      .Parameters.AddWithValue("@IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty))

      .Parameters.AddWithValue("@OdgovorenOrgan", ReadProperty(Of String)(OdgovorenOrganProperty))
      .Parameters.AddWithValue("@Sekretar", ReadProperty(Of String)(SekretarProperty))
      .Parameters.AddWithValue("@IdPaymentPrintOption", ReadProperty(Of Integer)(IdPaymentPrintOptionProperty))
      .Parameters.AddWithValue("@IdDefaultRegistrationIssuer", ReadProperty(Of Integer)(IdDefaultRegistrationIssuerProperty))
      .Parameters.AddWithValue("@IdDefaultCity", ReadProperty(Of Integer)(IdDefaultCityProperty))
      .Parameters.AddWithValue("@AutmateProceses", ReadProperty(Of Boolean)(AutmateProcesesProperty))
      .Parameters.AddWithValue("@ZiroSmetka", ReadProperty(Of String)(ZiroSmetkaProperty))
      .Parameters.AddWithValue("@Deponent", ReadProperty(Of String)(DeponentProperty))
      .Parameters.AddWithValue("@StationAddress", ReadProperty(Of String)(StationAddressProperty))
      .Parameters.AddWithValue("@EDB", ReadProperty(Of String)(EDBProperty))
      .Parameters.AddWithValue("@Tel", ReadProperty(Of String)(TelProperty))
      .Parameters.AddWithValue("@Fax", ReadProperty(Of String)(FaxProperty))
      .Parameters.AddWithValue("@Valuta", ReadProperty(Of Integer)(ValutaProperty))
      .Parameters.AddWithValue("@ApproveRequestAutomate", ReadProperty(Of Boolean)(ApproveRequestAutomateProperty))
      .Parameters.AddWithValue("@NewTechnicalExamReport", ReadProperty(Of Boolean)(NewTechnicalExamReportProperty))
      .Parameters.AddWithValue("@LogoPath", ReadProperty(Of String)(LogoPathProperty))
      .Parameters.AddWithValue("@PictureServerPath", ReadProperty(Of String)(PictureServerPathProperty))
      .Parameters.AddWithValue("@PriceWithTax", ReadProperty(Of Boolean)(PriceWithTaxProperty))
      .Parameters.AddWithValue("@PriceWithoutTax", ReadProperty(Of Boolean)(PriceWithoutTaxProperty))
      .Parameters.AddWithValue("@CalculatePercentOfTeh", ReadProperty(Of Boolean)(CalculatePercentOfTehProperty))
      .Parameters.AddWithValue("@PercentOfTeh", ReadProperty(Of Double)(PercentOfTehProperty))
      .Parameters.AddWithValue("@idTehnicalExam", ReadProperty(Of Integer)(idTehnicalExamProperty))
      .Parameters.AddWithValue("@idStavkaZavisnaOdTehnicalExam", ReadProperty(Of Integer)(idStavkaZavisnaOdTehnicalExamProperty))
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
  DataPortal_Delete(New SingleCriteria(Of TehnicalExamOrganization, Integer)(Id))
 End Sub

 Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of TehnicalExamOrganization, Integer))
  Database.LogInfo("TehnicalExamOrganization.DataPortal_Delete", GetHashCode())
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
   Database.LogException("TehnicalExamOrganization.DataPortal_Delete", ex)
   Throw New DbCslaException("TehnicalExamOrganization.DataPortal_Delete", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

 Private Sub Child_Fetch(ByVal dr As SafeDataReader)
  Database.LogInfo("TehnicalExamOrganization.Child_Fetch", GetHashCode())
  Try
   LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
   LoadProperty(Of Integer)(IdCompanyProperty, dr.GetInt32("IdCompany"))
   LoadProperty(Of String)(StationProperty, dr.GetString("Station"))
   LoadProperty(Of Integer)(IdCityProperty, dr.GetInt32("IdCity"))
   LoadProperty(Of String)(CodeProperty, dr.GetString("Code"))
   LoadProperty(Of Integer)(TrafficLicenceVlidNumOfMonthsProperty, dr.GetInt32("TrafficLicenceVlidNumOfMonths"))
   LoadProperty(Of Integer)(RegistrationVlidNumOfMonthsProperty, dr.GetInt32("RegistrationVlidNumOfMonths"))
   LoadProperty(Of Integer)(IdCommunityProperty, dr.GetInt32("IdCommunity"))

   LoadProperty(Of String)(OdgovorenOrganProperty, dr.GetString("OdgovorenOrgan"))
   LoadProperty(Of String)(SekretarProperty, dr.GetString("Sekretar"))
   LoadProperty(Of Integer)(IdPaymentPrintOptionProperty, dr.GetInt32("IdPaymentPrintOption"))
   LoadProperty(Of Integer)(IdDefaultRegistrationIssuerProperty, dr.GetInt32("IdDefaultRegistrationIssuer"))
   LoadProperty(Of Integer)(IdDefaultCityProperty, dr.GetInt32("IdDefaultCity"))
   LoadProperty(Of Boolean)(AutmateProcesesProperty, dr.GetBoolean("AutmateProceses"))
   ' podatoci z astanicata (za pecatenje fakturi)
   LoadProperty(Of String)(ZiroSmetkaProperty, dr.GetString("ZiroSmetka"))
   LoadProperty(Of String)(DeponentProperty, dr.GetString("Deponent"))
   LoadProperty(Of String)(StationAddressProperty, dr.GetString("StationAddress"))
   LoadProperty(Of String)(EDBProperty, dr.GetString("EDB"))
   LoadProperty(Of String)(TelProperty, dr.GetString("Tel"))
   LoadProperty(Of String)(FaxProperty, dr.GetString("Fax"))
   LoadProperty(Of Integer)(ValutaProperty, dr.GetInt32("Valuta"))

   LoadProperty(Of Boolean)(ApproveRequestAutomateProperty, dr.GetBoolean("ApproveRequestAutomate"))
   LoadProperty(Of Boolean)(NewTechnicalExamReportProperty, dr.GetBoolean("NewTechnicalExamReport"))
   LoadProperty(Of String)(LogoPathProperty, dr.GetString("LogoPath"))
   LoadProperty(Of String)(PictureServerPathProperty, dr.GetString("PictureServerPath"))
   LoadProperty(Of Boolean)(PriceWithTaxProperty, dr.GetBoolean("PriceWithTax"))
   LoadProperty(Of Boolean)(PriceWithoutTaxProperty, dr.GetBoolean("PriceWithoutTax"))
   LoadProperty(Of Boolean)(CalculatePercentOfTehProperty, dr.GetBoolean("CalculatePercentOfTeh"))
   LoadProperty(Of Double)(PercentOfTehProperty, dr.GetValue("PercentOfTeh"))
   LoadProperty(Of Integer)(idTehnicalExamProperty, dr.GetInt32("idTehnicalExam"))
   LoadProperty(Of Integer)(idStavkaZavisnaOdTehnicalExamProperty, dr.GetInt32("idStavkaZavisnaOdTehnicalExam"))
   dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
  Catch ex As Exception
   Database.LogException("TehnicalExamOrganization.Child_Fetch", ex)
   Throw New DbCslaException("TehnicalExamOrganization.Child_Fetch", ex)
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
      .Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
      .Parameters.AddWithValue("@Station", ReadProperty(Of String)(StationProperty))
      .Parameters.AddWithValue("@IdCity", ReadProperty(Of Integer)(IdCityProperty))
      .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
      .Parameters.AddWithValue("@TrafficLicenceVlidNumOfMonths", ReadProperty(Of Integer)(TrafficLicenceVlidNumOfMonthsProperty))
      .Parameters.AddWithValue("@RegistrationVlidNumOfMonths", ReadProperty(Of Integer)(RegistrationVlidNumOfMonthsProperty))
      .Parameters.AddWithValue("@IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty))

      .Parameters.AddWithValue("@OdgovorenOrgan", ReadProperty(Of String)(OdgovorenOrganProperty))
      .Parameters.AddWithValue("@Sekretar", ReadProperty(Of String)(SekretarProperty))
      .Parameters.AddWithValue("@IdPaymentPrintOption", ReadProperty(Of Integer)(IdPaymentPrintOptionProperty))
      .Parameters.AddWithValue("@IdDefaultRegistrationIssuer", ReadProperty(Of Integer)(IdDefaultRegistrationIssuerProperty))
      .Parameters.AddWithValue("@IdDefaultCity", ReadProperty(Of Integer)(IdDefaultCityProperty))
      .Parameters.AddWithValue("@AutmateProceses", ReadProperty(Of Boolean)(AutmateProcesesProperty))
      .Parameters.AddWithValue("@ZiroSmetka", ReadProperty(Of String)(ZiroSmetkaProperty))
      .Parameters.AddWithValue("@Deponent", ReadProperty(Of String)(DeponentProperty))
      .Parameters.AddWithValue("@StationAddress", ReadProperty(Of String)(StationAddressProperty))
      .Parameters.AddWithValue("@EDB", ReadProperty(Of String)(EDBProperty))
      .Parameters.AddWithValue("@Tel", ReadProperty(Of String)(TelProperty))
      .Parameters.AddWithValue("@Fax", ReadProperty(Of String)(FaxProperty))
      .Parameters.AddWithValue("@Valuta", ReadProperty(Of Integer)(ValutaProperty))
      .Parameters.AddWithValue("@ApproveRequestAutomate", ReadProperty(Of Boolean)(ApproveRequestAutomateProperty))
      .Parameters.AddWithValue("@NewTechnicalExamReport", ReadProperty(Of Boolean)(NewTechnicalExamReportProperty))
      .Parameters.AddWithValue("@LogoPath", ReadProperty(Of String)(LogoPathProperty))
      .Parameters.AddWithValue("@PictureServerPath", ReadProperty(Of String)(PictureServerPathProperty))
      .Parameters.AddWithValue("@PriceWithTax", ReadProperty(Of Boolean)(PriceWithTaxProperty))
      .Parameters.AddWithValue("@PriceWithoutTax", ReadProperty(Of Boolean)(PriceWithoutTaxProperty))
      .Parameters.AddWithValue("@CalculatePercentOfTeh", ReadProperty(Of Boolean)(CalculatePercentOfTehProperty))
      .Parameters.AddWithValue("@PercentOfTeh", ReadProperty(Of Double)(PercentOfTehProperty))
      .Parameters.AddWithValue("@idTehnicalExam", ReadProperty(Of Integer)(idTehnicalExamProperty))
      .Parameters.AddWithValue("@idStavkaZavisnaOdTehnicalExam", ReadProperty(Of Integer)(idStavkaZavisnaOdTehnicalExamProperty))
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
   Database.LogException("TehnicalExamOrganization.Child_Insert", ex)
   Throw New DbCslaException("TehnicalExamOrganization.Child_Insert", ex)
  Finally
   Database.LogInfo("TehnicalExamOrganization.Child_Insert", GetHashCode)
  End Try

 End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

 Private Sub Child_Update()
  Database.LogInfo("TehnicalExamOrganization.Child_Update", GetHashCode)
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spUpdate

      .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
      .Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
      .Parameters.AddWithValue("@Station", ReadProperty(Of String)(StationProperty))
      .Parameters.AddWithValue("@IdCity", ReadProperty(Of Integer)(IdCityProperty))
      .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
      .Parameters.AddWithValue("@TrafficLicenceVlidNumOfMonths", ReadProperty(Of Integer)(TrafficLicenceVlidNumOfMonthsProperty))
      .Parameters.AddWithValue("@RegistrationVlidNumOfMonths", ReadProperty(Of Integer)(RegistrationVlidNumOfMonthsProperty))
      .Parameters.AddWithValue("@IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty))
      .Parameters.AddWithValue("@OdgovorenOrgan", ReadProperty(Of String)(OdgovorenOrganProperty))
      .Parameters.AddWithValue("@Sekretar", ReadProperty(Of String)(SekretarProperty))
      .Parameters.AddWithValue("@IdPaymentPrintOption", ReadProperty(Of Integer)(IdPaymentPrintOptionProperty))
      .Parameters.AddWithValue("@IdDefaultRegistrationIssuer", ReadProperty(Of Integer)(IdDefaultRegistrationIssuerProperty))
      .Parameters.AddWithValue("@IdDefaultCity", ReadProperty(Of Integer)(IdDefaultCityProperty))
      .Parameters.AddWithValue("@AutmateProceses", ReadProperty(Of Boolean)(AutmateProcesesProperty))
      .Parameters.AddWithValue("@ZiroSmetka", ReadProperty(Of String)(ZiroSmetkaProperty))
      .Parameters.AddWithValue("@Deponent", ReadProperty(Of String)(DeponentProperty))
      .Parameters.AddWithValue("@StationAddress", ReadProperty(Of String)(StationAddressProperty))
      .Parameters.AddWithValue("@EDB", ReadProperty(Of String)(EDBProperty))
      .Parameters.AddWithValue("@Tel", ReadProperty(Of String)(TelProperty))
      .Parameters.AddWithValue("@Fax", ReadProperty(Of String)(FaxProperty))
      .Parameters.AddWithValue("@Valuta", ReadProperty(Of Integer)(ValutaProperty))
      .Parameters.AddWithValue("@ApproveRequestAutomate", ReadProperty(Of Boolean)(ApproveRequestAutomateProperty))
      .Parameters.AddWithValue("@NewTechnicalExamReport", ReadProperty(Of Boolean)(NewTechnicalExamReportProperty))
      .Parameters.AddWithValue("@LogoPath", ReadProperty(Of String)(LogoPathProperty))
      .Parameters.AddWithValue("@PictureServerPath", ReadProperty(Of String)(PictureServerPathProperty))
      .Parameters.AddWithValue("@PriceWithTax", ReadProperty(Of Boolean)(PriceWithTaxProperty))
      .Parameters.AddWithValue("@PriceWithoutTax", ReadProperty(Of Boolean)(PriceWithoutTaxProperty))
      .Parameters.AddWithValue("@CalculatePercentOfTeh", ReadProperty(Of Boolean)(CalculatePercentOfTehProperty))
      .Parameters.AddWithValue("@PercentOfTeh", ReadProperty(Of Double)(PercentOfTehProperty))
      .Parameters.AddWithValue("@idTehnicalExam", ReadProperty(Of Integer)(idTehnicalExamProperty))
      .Parameters.AddWithValue("@idStavkaZavisnaOdTehnicalExam", ReadProperty(Of Integer)(idStavkaZavisnaOdTehnicalExamProperty))
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
   Database.LogException("TehnicalExamOrganization.Child_Update", ex)
   If Not ex.Message.EndsWith("drug korisnik") Then
    Throw New DbCslaException("TehnicalExamOrganization.Child_Update", ex)
   End If
  End Try
 End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

 Private Sub Child_DeleteSelf()

  Database.LogInfo("TehnicalExamOrganization.Child_DeleteSelf", GetHashCode)
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
   Database.LogException("TehnicalExamOrganization.Child_Fetch", ex)
   Throw New DbCslaException("TehnicalExamOrganization.Child_Fetch", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
