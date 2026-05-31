
<Serializable()> _
Public Class PrintDocumentsTehnicalExamsReportsInfo
  Inherits ReadOnlyBase(Of PrintDocumentsTehnicalExamsReportsInfo)

  Private _id As Long
  Public ReadOnly Property Id() As Long
    Get
      Return _id
    End Get
  End Property


  Private _IdVehicle As Long
  Public ReadOnly Property IdVehicle() As Long
    Get
      Return _IdVehicle
    End Get
  End Property
  Private _madeDate As Date
  Public ReadOnly Property MadeDate() As Date
    Get
      Return _madeDate
    End Get
  End Property
  Private _validTillDate As Date
  Public ReadOnly Property ValidTillDate() As Date
    Get
      Return _validTillDate
    End Get
  End Property
  Private _idOrganizationForTehnicalExam As Integer
  Public ReadOnly Property IdOrganizationForTehnicalExam() As Integer
    Get
      Return _idOrganizationForTehnicalExam
    End Get
  End Property
  Private _organizationName As String
  Public ReadOnly Property OrganizationName() As String
    Get
      Return _organizationName
    End Get
    End Property
    Private _EngineTypeCode As String
    Public ReadOnly Property EngineTypeCode() As String
        Get
            Return _EngineTypeCode
        End Get
    End Property

  Private _station As String
  Public ReadOnly Property Station() As String
    Get
      Return _station
    End Get
  End Property
  Private _idFirsControler As Integer
  Public ReadOnly Property IdFirsControler() As Integer
    Get
      Return _idFirsControler
    End Get
  End Property
  Private _idSecondControler As Integer
  Public ReadOnly Property IdSecondControler() As Integer
    Get
      Return _idSecondControler
    End Get
  End Property
  Private _vehicleIsRight As Boolean
  Public ReadOnly Property VehicleIsRight() As Boolean
    Get
      Return _vehicleIsRight
    End Get
  End Property
  Private _customerSurname As String
  Public ReadOnly Property CustomerSurname() As String
    Get
      Return _customerSurname
    End Get
  End Property
  Private _customerFirstName As String
  Public ReadOnly Property CustomerFirstName() As String
    Get
      Return _customerFirstName
    End Get
  End Property
  Private _idLivingAddress As Integer
  Public ReadOnly Property IdLivingAddress() As Integer
    Get
      Return _idLivingAddress
    End Get
  End Property
  Private _livingAddressNumber As String
  Public ReadOnly Property LivingAddressNumber() As String
    Get
      Return _livingAddressNumber
    End Get
  End Property
  Private _idLivingCity As Integer
  Public ReadOnly Property IdLivingCity() As Integer
    Get
      Return _idLivingCity
    End Get
  End Property
  Private _streetName As String
  Public ReadOnly Property StreetName() As String
    Get
      Return _streetName
    End Get
  End Property
  Private _cityName As String
  Public ReadOnly Property CityName() As String
    Get
      Return UCase(_cityName)
    End Get
  End Property
  Private _cityZip As Integer
  Public ReadOnly Property CityZip() As Integer
    Get
      Return _cityZip
    End Get
  End Property
  Private _countryName As String
  Public ReadOnly Property CountryName() As String
    Get
      Return _countryName
    End Get
  End Property
  Private _communityName As String
  Public ReadOnly Property CommunityName() As String
    Get
      Return _communityName
    End Get
  End Property
  Private _modelName As String
  Public ReadOnly Property ModelName() As String
    Get
      Return _modelName
    End Get
    End Property
    Private _modelAdding As String
    Public ReadOnly Property ModelAdding() As String
        Get
            Return _modelAdding
        End Get
    End Property
    Private _tng As Boolean
    Public ReadOnly Property TNG() As Boolean
        Get
            Return _tng
        End Get
    End Property
    Private _tip As String
    Public ReadOnly Property Tip() As String
        Get
            Return _tip
        End Get
    End Property
    Public ReadOnly Property ModelNameAddingTng() As String
        Get
            If _tng Then
                Return _modelName & " " & _modelAdding & " TNG"
            Else
                Return _modelName & " " & _modelAdding
            End If

        End Get
    End Property
  Private _makeDate As Date
  Public ReadOnly Property MakeDate() As Date
    Get
      Return _makeDate
    End Get
  End Property
  Private _numberOfAxis As Integer
  Public ReadOnly Property NumberOfAxis() As Integer
    Get
      Return _numberOfAxis
    End Get
  End Property
  Private _engineNumber As String
  Public ReadOnly Property EngineNumber() As String
    Get
      Return _engineNumber
    End Get
  End Property
  Private _engineWorkingCapacity As Single
  Public ReadOnly Property EngineWorkingCapacity() As Single
    Get
      Return _engineWorkingCapacity
    End Get
  End Property
  Private _emptyWaight As Single
  Public ReadOnly Property EmptyWaight() As Single
    Get
      Return _emptyWaight
    End Get
  End Property
  Private _maximunAllowedWaight As Single
  Public ReadOnly Property MaximunAllowedWaight() As Single
    Get
      Return _maximunAllowedWaight
    End Get
  End Property
  Private _idCountryOfProduction As Integer
  Public ReadOnly Property IdCountryOfProduction() As Integer
    Get
      Return _idCountryOfProduction
    End Get
  End Property
  Private _companyName As String
  Public ReadOnly Property CompanyName() As String
    Get
      Return _companyName
    End Get
  End Property
  Private _companyTrademark As String
  Public ReadOnly Property CompanyTrademark() As String
    Get
      Return _companyTrademark
    End Get
  End Property
  Private _propulsionAxis As Integer
  Public ReadOnly Property PropulsionAxis() As Integer
    Get
      Return _propulsionAxis
    End Get
  End Property
  Private _shellNumber As String
  Public ReadOnly Property ShellNumber() As String
    Get
      Return _shellNumber
    End Get
  End Property
  Private _enginePower As Single
  Public ReadOnly Property EnginePower() As Single
    Get
      Return _enginePower
    End Get
    End Property
    Private _enginePowerOutPut As Single
    Public ReadOnly Property EnginePowerOutPut() As Single
        Get
            Return _enginePowerOutPut
        End Get
    End Property
  Private _numberOfSeats As Short
  Public ReadOnly Property NumberOfSeats() As Short
    Get
      Return _numberOfSeats
    End Get
  End Property
  Private _explanationNote As String
  Public ReadOnly Property ExplanationNote() As String
    Get
      Return _explanationNote
    End Get
  End Property
  Private _driversWarning As String
  Public ReadOnly Property DriversWarning() As String
    Get
      Return _driversWarning
    End Get
  End Property
  Public ReadOnly Property VehicleType() As String
    Get
      Return _companyName & " " & _companyTrademark
    End Get
  End Property
  Private _isSocialNotPrivate As Boolean
  Public ReadOnly Property IsSocialNotPrivate() As Boolean
    Get
      Return _isSocialNotPrivate
    End Get
  End Property
  Public ReadOnly Property CarringCapacity() As Single
    Get
      Return (_maximunAllowedWaight - _emptyWaight)
    End Get
  End Property
  Public ReadOnly Property OrganizationAndStation() As String
    Get
      If _station IsNot Nothing Then
        Return UCase(_organizationName & "-" & _station)
      Else
        Return UCase(_station)
      End If
    End Get
  End Property
  Public ReadOnly Property LivingAddress() As String
        Get
            Dim pom As String
            If _communityName <> _cityName Then
                pom = _cityName & ", " & _communityName
            Else
                pom = _cityName
            End If
            If Not (_streetName = String.Empty) Then
                If Not (Trim(_livingAddressNumber) = String.Empty) Then
                    Return UCase(_streetName & " бр." & _livingAddressNumber) & " " & pom
                Else
                    Return UCase(_streetName) & " " & pom
                End If
            Else
                Return pom
            End If
        End Get
  End Property
  Public ReadOnly Property CustomerName() As String
    Get
      Return _customerFirstName & " " & _customerSurname
    End Get
  End Property
  Private _forPrivateTransportNotPublic As Boolean
  Public ReadOnly Property ForPrivateTransportNotPublic() As Boolean
    Get
      Return _forPrivateTransportNotPublic
    End Get
  End Property
  Private _idTypeOfTehnicalExam As Integer
  Public ReadOnly Property IdTypeOfTehnicalExam() As Integer
    Get
      Return _idTypeOfTehnicalExam
    End Get
  End Property
  Private _idCustomerVehicleRelation As Integer
  Public ReadOnly Property IdCustomerVehicleRelation() As Integer
    Get
      Return _idCustomerVehicleRelation
    End Get
    End Property
    Private _colorCode As String
    Public ReadOnly Property ColorCode() As String
        Get
            Return _colorCode
        End Get
    End Property
    Private _colorDescription As String
    Public ReadOnly Property ColorDscription() As String
        Get
            Return _colorDescription
        End Get
    End Property
    Private _colorCode2 As String
    Public ReadOnly Property ColorCode2() As String
        Get
            Return _colorCode2
        End Get
    End Property
    Private _colorDescription2 As String
    Public ReadOnly Property ColorDscription2() As String
        Get
            Return _colorDescription2
        End Get
    End Property
    Private _IsNewCustomer As Boolean
    Public ReadOnly Property IsNewCustomer() As Boolean
        Get
            Return _IsNewCustomer
        End Get
    End Property
    Private _IsNewRegistration As Boolean
    Public ReadOnly Property IsNewRegistration() As Boolean
        Get
            Return _IsNewRegistration
        End Get
    End Property
  Private _CategoryName As String
  Public ReadOnly Property CategoryName() As String
    Get
      Return _CategoryName
    End Get
  End Property
    Public ReadOnly Property ColorFull()
        Get
            If ColorCode2 IsNot Nothing AndAlso ColorCode2 <> "" AndAlso ColorCode2 <> String.Empty Then
                Return _colorCode & "-" & _colorDescription & "; " & _colorCode2 & "-" & _colorDescription2
            Else
                Return _colorCode & "-" & _colorDescription
            End If
        End Get
    End Property
  Private _lastRegistrationNumber As String
  Public ReadOnly Property LastRegistration() As String
        Get
            Dim objCurentTehExamStation As TehnicalExamOrganizationsInfo = CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)
           
            Dim communitList As CommunitiesList = Csla.ApplicationContext.LocalContext("objCommunityList")


            Dim pom As String = communitList.GetCommunitiesListById(objCurentTehExamStation.IdCommunity).RegistrationCode

            If _lastRegistrationNumber = pom & "-000-AA" Then
                Return pom & "-"
            Else
                Return _lastRegistrationNumber

            End If

        End Get
    End Property

    Private _idLastRegIssuer As Integer
    Public ReadOnly Property IdLastRegIssuer() As Integer
        Get
            Return _idLastRegIssuer
        End Get
    End Property
    Private _communityId As Integer
    Public ReadOnly Property communityId() As Integer
        Get
            Return _communityId
        End Get
    End Property

  Public ReadOnly Property LastRegistrationPlace() As String
        Get
            Dim issuerList As RegistrationIssuerList = Csla.ApplicationContext.LocalContext.Item("objRegistrationIssuerList")

            Dim communityList As CommunitiesList = Csla.ApplicationContext.LocalContext.Item("objCommunityList")
            Dim place As String
            Dim _LastRegIdCommunity As Integer = issuerList.GetRegistrationIssuerInfo(_idLastRegIssuer).IdCommunity
            If _LastRegIdCommunity > 0 Then
                place = communityList.GetCommunitiesListById(_LastRegIdCommunity).CommunityName
            Else
                place = ""
            End If

            Return place
        End Get
    End Property
    Public ReadOnly Property EngineTypeAndNum() As String
        Get
            Return _EngineTypeCode & "/" & _engineNumber
        End Get
    End Property
    Private _regNumber As String
    Public ReadOnly Property RegNumber() As String
        Get
            Return _regNumber
        End Get
    End Property
    'Public ReadOnly Property RegNumberDolg() As String
    '    Get
    '        Dim opcii = CType(Csla.ApplicationContext.LocalContext("objOpcii"), Options)
    '        'Dim operatorBr As String = String.Empty
    '        Dim stanici As TehnicalExamOrganizationsList = Csla.ApplicationContext.LocalContext("objTehExamOrganizations")
    '        'If Csla.ApplicationContext.LocalContext("EmployeeID") < 10 Then
    '        '    operatorBr = "0" & Csla.ApplicationContext.LocalContext("EmployeeID").ToString
    '        'Else
    '        '    operatorBr = Csla.ApplicationContext.LocalContext("EmployeeID").ToString
    '        'End If
    '        Return stanici.GetTehnicalExamOrganizationsInfoById(_idOrganizationForTehnicalExam).Code & RegNumber & "/" & IdFirsControler & IdSecondControler & "/" & MadeDate.Year
    '    End Get
    'End Property
  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function
   
  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt64("Id")
    _madeDate = dr.GetDateTime("MadeDate")
    _validTillDate = dr.GetDateTime("ValidTillDate")
    _idOrganizationForTehnicalExam = dr.GetInt32("IdOrganizationForTehnicalExam")
    _organizationName = dr.GetString("OrganizationName")
    _station = dr.GetString("Station")
    _idFirsControler = dr.GetInt32("IdFirsControler")
    _idSecondControler = dr.GetInt32("IdSecondControler")
    _vehicleIsRight = dr.GetBoolean("VehicleIsRight")
    _customerSurname = dr.GetString("CustomerSurname")
    _customerFirstName = dr.GetString("CustomerFirstName")
    _idLivingAddress = dr.GetInt32("IdLivingAddress")
    _livingAddressNumber = dr.GetString("LivingAddressNumber")
    _idLivingCity = dr.GetInt32("IdLivingCity")
    _streetName = dr.GetString("StreetName")
    _cityName = dr.GetString("CityName")
    _cityZip = dr.GetInt32("CityZip")
    _countryName = dr.GetString("CountryName")
    _communityName = dr.GetString("CommunityName")
    _modelName = dr.GetString("ModelName")
    _makeDate = dr.GetDateTime("MakeDate")
    _numberOfAxis = dr.GetInt32("NumberOfAxis")
    _engineNumber = dr.GetString("EngineNumber")
    _engineWorkingCapacity = dr.GetValue("EngineWorkingCapacity")
    _emptyWaight = dr.GetValue("EmptyWaight")
    _maximunAllowedWaight = dr.GetValue("MaximunAllowedWaight")
    _idCountryOfProduction = dr.GetInt32("IdCountryOfProduction")
    _companyName = dr.GetString("CompanyName")
    _companyTrademark = dr.GetString("CompanyTrademark")
    _propulsionAxis = dr.GetInt32("PropulsionAxis")
    _shellNumber = dr.GetString("ShellNumber")
    _enginePower = dr.GetValue("EnginePower")
    _numberOfSeats = dr.GetInt16("NumberOfSeats")
    _explanationNote = dr.GetString("ExplanationNote")
    _driversWarning = dr.GetString("DriversWarning")
    _isSocialNotPrivate = dr.GetBoolean("IsSocialNotPrivate")
    _forPrivateTransportNotPublic = dr.GetBoolean("ForPrivateTransportNotPublic")
    _idTypeOfTehnicalExam = dr.GetInt32("IdTypeOfTehnicalExam")
    _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
        _IdVehicle = dr.GetInt64("IdVehicle")
        _enginePowerOutPut = dr.GetValue("EnginePowerOutPut")
        _colorCode = dr.GetString("ColorCode")
        _colorDescription = dr.GetString("ColorDescription")
        _colorCode2 = dr.GetString("ColorCode2")
        _colorDescription2 = dr.GetString("ColorDescription2")
        _EngineTypeCode = dr.GetString("EngineTypeCode")
        _regNumber = dr.GetString("RegNumber")
        _tip = dr.GetString("Tip")

        ' Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_IdVehicle)

        _lastRegistrationNumber = dr.GetString("LastRegistratinNumber") 'regInfo.RegistrationNumber
        ' _lastRegistrationPlace = dr.GetString("LastRegistrationCommunity") 'regInfo.RegistrationPlace
        _idLastRegIssuer = dr.GetInt32("IdLastRegistrationIssuer")
        _modelAdding = dr.GetString("VehicleModelAdding")
        _tng = dr.GetBoolean("TNG")
        Try
            _IsNewCustomer = dr.GetBoolean("IsNewCustomer")
        Catch ex As Exception
            _IsNewCustomer = False
        End Try
        Try
            _IsNewRegistration = dr.GetBoolean("IsNewRegistration")
        Catch ex As Exception
            _IsNewRegistration = False
        End Try

        _communityId = dr.GetInt32("communityId")
    _CategoryName = dr.GetString("CategoryName")
  End Sub

End Class