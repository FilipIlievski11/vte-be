<Serializable()> _
Public Class PrintPlavInfo
  Inherits ReadOnlyBase(Of PrintPlavInfo)

#Region " JoinList "
  Private _currentOwner As PrintCustomerList = PrintCustomerList.EmptyList
  Public ReadOnly Property CurrentOwner() As PrintCustomerList
    Get
      Return _currentOwner
    End Get
  End Property

  Private _newOwner As PrintCustomerList = PrintCustomerList.EmptyList
  Public ReadOnly Property NewOwner() As PrintCustomerList
    Get
      Return _newOwner
    End Get
  End Property

  Private _vehicle As PrintVehcileList = PrintVehcileList.EmptyList
  Public ReadOnly Property CurrentVehicle() As PrintVehcileList
    Get
      Return _vehicle
    End Get
  End Property

    'Private _toOrganization As String
    'Public ReadOnly Property ToOrganization() As String
    '  Get
    '    Return _toOrganization
    '  End Get
    'End Property

  Private _paymentProofs As String = String.Empty
  Public ReadOnly Property PaymentProofs() As String
    Get
      Return UCase(_paymentProofs)
    End Get
  End Property

  Private _ownershipProofs As String = String.Empty
  Public ReadOnly Property OwnershipProofs() As String
    Get
      Return UCase(_ownershipProofs)
    End Get
  End Property

#End Region

  Private _id As Long
  Public ReadOnly Property Id() As Long
    Get
      Return _id
    End Get
  End Property
  Private _idRequestType As Integer
  Public ReadOnly Property IdRequestType() As Integer
    Get
      Return _idRequestType
    End Get
  End Property
  Private _idCustomerVehicleRelation As Long
  Public ReadOnly Property IdCustomerVehicleRelation() As Long
    Get
      Return _idCustomerVehicleRelation
    End Get
  End Property
  Private _idCustomerVehicleRelationNew As Long
  Public ReadOnly Property IdCustomerVehicleRelationNew() As Long
    Get
      Return _idCustomerVehicleRelationNew
    End Get
  End Property
  Private _isCustomerChanged As Boolean
  Public ReadOnly Property IsCustomerChanged() As Boolean
    Get
      Return _isCustomerChanged
    End Get
  End Property
  Private _isVehicleChanged As Boolean
  Public ReadOnly Property IsVehicleChanged() As Boolean
    Get
      Return _isVehicleChanged
    End Get
  End Property
  Private _note As String
  Public ReadOnly Property Note() As String
    Get
      Return _note
    End Get
  End Property
  Private _idCustomer As Long
  Public ReadOnly Property IdCustomer() As Long
    Get
      Return _idCustomer
    End Get
  End Property
  Private _idVehicle As Long
  Public ReadOnly Property IdVehicle() As Long
    Get
      Return _idVehicle
    End Get
  End Property
  Private _idPreviousRegistration As Integer
  Public ReadOnly Property IdPreviousRegistration() As Integer
    Get
      Return _idPreviousRegistration
    End Get
  End Property
    'Private _registrationNumberPrevios As String
    'Public ReadOnly Property RegistrationNumberPrevios() As String
    '  Get
    '    Return _registrationNumberPrevios
    '  End Get
    'End Property
    'Private _dateOfRegistration As Date
    'Public ReadOnly Property DateOfRegistration() As Date
    '  Get
    '    Return _dateOfRegistration
    '  End Get
    'End Property
  Private _dateCreated As Date
  Public ReadOnly Property DateCreated() As Date
    Get
      Return _dateCreated
    End Get
  End Property
  Private _dateModified As Date
  Public ReadOnly Property DateModified() As Date
    Get
      Return _dateModified
    End Get
  End Property
  Private _dateEnded As Date
  Public ReadOnly Property DateEnded() As Date
    Get
      Return _dateEnded
    End Get
  End Property
  Private _idTechnicalExamReport As Long
  Public ReadOnly Property IdTechnicalExamReport() As Long
    Get
      Return _idTechnicalExamReport
    End Get
  End Property
  Private _madeDateTehnicalExam As Date
  Public ReadOnly Property MadeDateTehnicalExam() As Date
    Get
      Return _madeDateTehnicalExam
    End Get
  End Property
  Private _validTillDateTehnicalExam As Date
  Public ReadOnly Property ValidTillDateTehnicalExam() As Date
    Get
      Return _validTillDateTehnicalExam
    End Get
  End Property
  Private _vehicleIsRight As Boolean
  Public ReadOnly Property VehicleIsRight() As Boolean
    Get
      Return _vehicleIsRight
    End Get
  End Property
  Private _idOrganizationForTehnicalExam As Integer
  Public ReadOnly Property IdOrganizationForTehnicalExam() As Integer
    Get
      Return _idOrganizationForTehnicalExam
    End Get
    End Property
    Private _regNumber As String
    Public ReadOnly Property RegNumber() As String
        Get
            Return (_regNumber)
        End Get
    End Property
  Private _organizationName As String
  Public ReadOnly Property OrganizationName() As String
    Get
      Return UCase(_organizationName)
    End Get
  End Property
  Private _station As String
  Public ReadOnly Property Station() As String
    Get
      Return UCase(_station)
    End Get
  End Property
  Private _idCity As Integer
  Public ReadOnly Property IdCity() As Integer
    Get
      Return _idCity
    End Get
  End Property
  Private _cityNameTehnicalOrganization As String
  Public ReadOnly Property CityNameTehnicalOrganization() As String
    Get
      Return _cityNameTehnicalOrganization
    End Get
  End Property
    'Private _IdRegistrationIssuer As Integer
    'Public ReadOnly Property IdRegistrationIssuer() As Integer
    '  Get
    '    Return _IdRegistrationIssuer
    '  End Get
    '  End Property
    Public ReadOnly Property RegNumberDolg() As String
        Get
            Dim opcii = CType(Csla.ApplicationContext.LocalContext("objOpcii"), Options)
            Dim operatorBr As String = String.Empty
            Dim stanica As TehnicalExamOrganizationsInfo = Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization")
            If Csla.ApplicationContext.LocalContext("EmployeeID") < 1 Then
                operatorBr = "0" & Csla.ApplicationContext.LocalContext("EmployeeID").ToString
            Else
                operatorBr = Csla.ApplicationContext.LocalContext("EmployeeID").ToString
            End If
            Try
                Return stanica.Code & IdTechnicalExamReport & operatorBr & "/" & DateCreated.Year
            Catch ex As Exception
                Return IdTechnicalExamReport & operatorBr & "/" & DateCreated.Year
            End Try

        End Get
    End Property

    'Private _IssuerName As String
    'Public ReadOnly Property IssuerName() As String
    '  Get
    '    Return _IssuerName
    '  End Get
    'End Property

    'Private _DateRegistrationValidTill As Date
    'Public ReadOnly Property DateRegistrationValidTill() As Date
    '  Get
    '    Return _DateRegistrationValidTill
    '  End Get
    'End Property

  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt64("Id")
    _idRequestType = dr.GetInt32("IdRequestType")
    _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
    _idCustomerVehicleRelationNew = dr.GetInt64("IdCustomerVehicleRelationNew")
    _isCustomerChanged = dr.GetBoolean("IsCustomerChanged")
    _isVehicleChanged = dr.GetBoolean("IsVehicleChanged")
    _note = dr.GetString("Note")
    _idCustomer = dr.GetInt64("IdCustomer")
    _idVehicle = dr.GetInt64("IdVehicle")
    _idPreviousRegistration = dr.GetInt32("IdPreviousRegistration")
        ' _registrationNumberPrevios = dr.GetString("RegistrationNumberPrevios")
        ' _dateOfRegistration = dr.GetDateTime("DateOfRegistration")
    _dateCreated = dr.GetDateTime("DateCreated")
    _dateModified = dr.GetDateTime("DateModified")
    _dateEnded = dr.GetDateTime("DateEnded")
    _idTechnicalExamReport = dr.GetInt64("IdTechnicalExamReport")
    _madeDateTehnicalExam = dr.GetDateTime("MadeDateTehnicalExam")
    _validTillDateTehnicalExam = dr.GetDateTime("ValidTillDateTehnicalExam")
    _vehicleIsRight = dr.GetBoolean("VehicleIsRight")
        _idOrganizationForTehnicalExam = dr.GetInt32("IdOrganizationForTehnicalExam")
        _regNumber = dr.GetString("RegNumber")
    _organizationName = dr.GetString("OrganizationName")
    _station = dr.GetString("Station")
    _idCity = dr.GetInt32("IdCity")
    _cityNameTehnicalOrganization = dr.GetString("CityNameTehnicalOrganization")
        ' _IdRegistrationIssuer = dr.GetInt32("IdRegistrationIssuer")

        ' _IssuerName = dr.GetString("IssuerName")
        ' _DateRegistrationValidTill = dr.GetDateTime("DateRegistrationValidTill")

        'Dim objRegIssuers As RegistrationIssuerList = CType(Csla.ApplicationContext.LocalContext("objRegistrationIssuerList"), RegistrationIssuerList)
        'Dim regIssuer As RegistrationIssuerInfo = objRegIssuers.GetRegistrationIssuerInfo(CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdDefaultRegistrationIssuer)
        '_toOrganization = regIssuer.IssuerName

    'deca
    _vehicle = PrintVehcileList.GetPrintVehcileList(_idVehicle)
    'proveri dali ima promena na sopstvenist
    Dim reqTypeList As RequestTypeList = _
      CType(Csla.ApplicationContext.LocalContext("objRequestTypeList"),  _
      RequestTypeList)

    Dim reqType As RequestTypeInfo = reqTypeList.getInfoById(_idRequestType)
    If reqType.IsNewCustomer Then
      _newOwner = PrintCustomerList.GetPrintCustomerList(_idCustomerVehicleRelationNew)
      _currentOwner = PrintCustomerList.GetPrintCustomerList(_idCustomer)
    Else
      _currentOwner = PrintCustomerList.GetPrintCustomerList(_idCustomer)
      _newOwner = _currentOwner
    End If
    'listi
    dr.NextResult()
    While dr.Read
      If _paymentProofs <> String.Empty Then
        _paymentProofs &= "; " & vbCrLf
      End If
            _paymentProofs &= dr.GetString("PaymentProofType") & " " & dr.GetString("PaymentProofNumber")
    End While
    dr.NextResult()
    While dr.Read
      If _ownershipProofs <> String.Empty Then
        _ownershipProofs &= "; " & vbCrLf
      End If
            _ownershipProofs &= dr.GetString("VehicleOwnershipProofType") & " " & dr.GetString("VehicleOwnershipProofNumber")
    End While
  End Sub

End Class
