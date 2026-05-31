
<Serializable()> _
Public Class PrintVehcileInfo
    Inherits ReadOnlyBase(Of PrintVehcileInfo)

    Private _id As Long
    Public ReadOnly Property Id() As Long
        Get
            Return _id
        End Get
    End Property
    Private _idVehicleBodyType As Integer
    Public ReadOnly Property IdVehicleBodyType() As Integer
        Get
            Return _idVehicleBodyType
        End Get
    End Property
    Private _idVehicleCategories As Integer
    Public ReadOnly Property IdVehicleCategories() As Integer
        Get
            Return _idVehicleCategories
        End Get
    End Property
    Private _categoryCode As String
    Public ReadOnly Property CategoryCode() As String
        Get
            Return _categoryCode
        End Get
    End Property
    Private _categoryName As String
    Public ReadOnly Property CategoryName() As String
        Get
            Return UCase(_categoryName)
        End Get
    End Property
    Private _bodytypeCode As String
    Public ReadOnly Property BodytypeCode() As String
        Get
            Return _bodytypeCode
        End Get
    End Property
    Private _bodytypeDescriprion As String
    Public ReadOnly Property BodytypeDescriprion() As String
        Get
            Return _bodytypeDescriprion
        End Get
    End Property
    Private _oldBodytypeDescription As String
    Public ReadOnly Property OldBodytypeDescription() As String
        Get
            Return _oldBodytypeDescription
        End Get
    End Property
    Private _oldCategoryName As String
    Public ReadOnly Property OldCategoryName() As String
        Get
            Return _oldCategoryName
        End Get
    End Property
    Private _idVehicleCategoryForPayments As Integer
    Public ReadOnly Property IdVehicleCategoryForPayments() As Integer
        Get
            Return _idVehicleCategoryForPayments
        End Get
    End Property
    Private _categoryForPaymentsCode As String
    Public ReadOnly Property CategoryForPaymentsCode() As String
        Get
            Return _categoryForPaymentsCode
        End Get
    End Property
    Private _categoryForPaymentsName As String
    Public ReadOnly Property CategoryForPaymentsName() As String
        Get
            Return _categoryForPaymentsName
        End Get
    End Property
    Private _idVehicleUse As Integer
    Public ReadOnly Property IdVehicleUse() As Integer
        Get
            Return _idVehicleUse
        End Get
    End Property
    Private _useDescription As String
    Public ReadOnly Property UseDescription() As String
        Get
            Return _useDescription
        End Get
    End Property
    Private _idVehicleModel As Integer
    Public ReadOnly Property IdVehicleModel() As Integer
        Get
            Return _idVehicleModel
        End Get
    End Property
    Private _modelCode As String
    Public ReadOnly Property ModelCode() As String
        Get
            Return _modelCode
        End Get
    End Property
    Private _modelName As String
    Public ReadOnly Property ModelName() As String
        Get
            Return _modelName
        End Get
    End Property
    Private _idVehicleMaker As Integer
    Public ReadOnly Property IdVehicleMaker() As Integer
        Get
            Return _idVehicleMaker
        End Get
    End Property
    Private _vehicleMaker As String
    Public ReadOnly Property VehicleMaker() As String
        Get
            Return _vehicleMaker
        End Get
    End Property
    Private _VehicleModelAdding As String
    Public ReadOnly Property VehicleModelAdding() As String
        Get
            Return _VehicleModelAdding
        End Get
    End Property
    Public ReadOnly Property VehicleModelAndModelAdding() As String
        Get
            If _tng Then
                Return _modelName & " " & _VehicleModelAdding & " TNG"
            Else
                Return _modelName & " " & _VehicleModelAdding
            End If

        End Get
    End Property
    Private _idPrimaryColor As Integer
    Public ReadOnly Property IdPrimaryColor() As Integer
        Get
            Return _idPrimaryColor
        End Get
    End Property
    Private _idSecondaryColor As Integer
    Public ReadOnly Property IdSecondaryColor() As Integer
        Get
            Return _idSecondaryColor
        End Get
    End Property
    Private _colorCode As String
    Public ReadOnly Property ColorCode() As String
        Get
            Return _colorCode
        End Get
    End Property
    Public ReadOnly Property Color() As String
        Get
            If _idPrimaryColor > 0 Then
                If _idSecondaryColor > 0 Then
                    Return _colorCode & "-" & _colorDescription & "; " _
                    & _colorCodeSecondary & "-" & _colorDescriptionSecondary
                Else
                    Return _colorCode & "-" & _colorDescription
                End If
            Else
                Return ""
            End If
        End Get
    End Property
    Private _colorDescription As String
    Public ReadOnly Property ColorDescription() As String
        Get
            Return _colorDescription
        End Get
    End Property
    Private _colorCodeSecondary As String
    Public ReadOnly Property ColorCodeSecondary() As String
        Get
            Return _colorCodeSecondary
        End Get
    End Property
    Private _colorDescriptionSecondary As String
    Public ReadOnly Property ColorDescriptionSecondary() As String
        Get
            Return _colorDescriptionSecondary
        End Get
    End Property
    Private _idEngineType As Integer
    Public ReadOnly Property IdEngineType() As Integer
        Get
            Return _idEngineType
        End Get
    End Property
    Private _engineTypeCode As String
    Public ReadOnly Property EngineTypeCode() As String
        Get
            Return _engineTypeCode
        End Get
    End Property
    Public ReadOnly Property EngineTypeCodeAndNumber() As String
        Get
            Return _engineTypeCode & " / " & _engineNumber
        End Get
    End Property
    Private _vehicleEngineTypeTechincalDescription As String
    Public ReadOnly Property VehicleEngineTypeTechincalDescription() As String
        Get
            Return _vehicleEngineTypeTechincalDescription
        End Get
    End Property
    Private _engineNumber As String
    Public ReadOnly Property EngineNumber() As String
        Get
            Return _engineNumber
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
    Private _engineTorque As String
    Public ReadOnly Property EngineTorque() As String
        Get
            Return _engineTorque
        End Get
    End Property
    Private _shellNumber As String
    Public ReadOnly Property ShellNumber() As String
        Get
            Return _shellNumber
        End Get
    End Property
    Private _makeDate As Date
    Public ReadOnly Property MakeDate() As Date
        Get
            Return _makeDate
        End Get
    End Property
    Private _numberOfDoors As Integer
    Public ReadOnly Property NumberOfDoors() As Integer
        Get
            Return _numberOfDoors
        End Get
    End Property
    Private _numberOfSeats As Short
    Public ReadOnly Property NumberOfSeats() As Short
        Get
            Return _numberOfSeats
        End Get
    End Property
    Private _numberOfStandingSeats As Short
    Public ReadOnly Property NumberOfStandingSeats() As Short
        Get
            Return _numberOfStandingSeats
        End Get
    End Property
    Private _numberOfLieingSeats As Short
    Public ReadOnly Property NumberOfLieingSeats() As Short
        Get
            Return _numberOfLieingSeats
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
    Private _numberOfAxis As Integer
    Public ReadOnly Property NumberOfAxis() As Integer
        Get
            Return _numberOfAxis
        End Get
    End Property
    Private _propulsionAxis As Integer
    Public ReadOnly Property PropulsionAxis() As Integer
        Get
            Return _propulsionAxis
        End Get
    End Property
    Private _numberOfWheels As Integer
    Public ReadOnly Property NumberOfWheels() As Integer
        Get
            Return _numberOfWheels
        End Get
    End Property
    Private _numberOfPropulsionWheels As Integer
    Public ReadOnly Property NumberOfPropulsionWheels() As Integer
        Get
            Return _numberOfPropulsionWheels
        End Get
    End Property
    Private _vehicleSizeHight As Single
    Public ReadOnly Property VehicleSizeHight() As Single
        Get
            Return _vehicleSizeHight
        End Get
    End Property
    Private _vehicleSizeWidth As Single
    Public ReadOnly Property VehicleSizeWidth() As Single
        Get
            Return _vehicleSizeWidth
        End Get
    End Property
    Private _vehicleSizeLength As Single
    Public ReadOnly Property VehicleSizeLength() As Single
        Get
            Return _vehicleSizeLength
        End Get
    End Property

    Private _lastRegistrationNumber As String
    Public ReadOnly Property LastRegistration() As String
        Get
            Dim objCurentTehExamStation As TehnicalExamOrganizationsInfo = CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)

            Dim communitList As CommunitiesList = Csla.ApplicationContext.LocalContext("objCommunityList")
            Dim pom As String = ""
            'If _LastRegIdCommunity > 0 Then
            ' pom = communitList.GetCommunitiesListById(_LastRegIdCommunity).RegistrationCode
            'Else
            pom = communitList.GetCommunitiesListById(objCurentTehExamStation.IdCommunity).RegistrationCode
            'End If

            '

            If _lastRegistrationNumber = pom & "-000-AA" Then
                Return pom & "-"
            Else
                Return _lastRegistrationNumber
            End If
        End Get
    End Property

    'Private _lastRegistrationPlace As String
    Public ReadOnly Property LastRegistrationPlace() As String
        Get
            Dim communityList As CommunitiesList = Csla.ApplicationContext.LocalContext.Item("objCommunityList")
            Dim place As String
            If _LastRegIdCommunity > 0 Then
                place = communityList.GetCommunitiesListById(_LastRegIdCommunity).CommunityName
            Else
                place = ""
            End If

            Return place
        End Get
    End Property
    Private _firstRegistrationNumber As String
    Public ReadOnly Property FirstRegistration() As String
        Get
            Return _firstRegistrationNumber
        End Get
    End Property
    Private _firstRegistrationDate As Date
    Public ReadOnly Property FirstRegistrationDate() As Date
        Get
            Try
                If _firstRegistrationDate.Date > CType("01/01/1753", Date) Then
                    Return _firstRegistrationDate.Date
                Else
                    Return ""
                End If
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    'Private _firstRegistrationPlace As String
    Public ReadOnly Property FirstRegistrationPlace() As String
        Get
            Dim communityList As CommunitiesList = Csla.ApplicationContext.LocalContext.Item("objCommunityList")
            Dim place As String
            If _FirstRegIdCommunity > 0 Then
                place = communityList.GetCommunitiesListById(_FirstRegIdCommunity).CommunityName
            Else
                place = ""
            End If

            Return place
        End Get
    End Property
    Private _dateOfLastRegistration As Date
    Public ReadOnly Property DateOfLastRegistrationa() As Date
        Get
            Return _dateOfLastRegistration
        End Get
    End Property
    Private _lastRegistrationValidTill As Date
    Public ReadOnly Property LastRegistrationValidTill() As Date
        Get
            Return _lastRegistrationValidTill
        End Get
    End Property
    Private _firstRegistrationValidTill As Date
    Public ReadOnly Property FirstRegistrationValidTill() As Date
        Get
            Return _firstRegistrationValidTill
        End Get
    End Property
    Private _EngineWorkingCapacity As Double
    Public ReadOnly Property EngineWorkingCapacity() As Double
        Get
            Return Math.Round(_EngineWorkingCapacity, 1)
        End Get
    End Property
    Public ReadOnly Property LastRegistrationDisplay() As String
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
    Private _IdMadeCountry As Integer
    Public ReadOnly Property IdMadeCountry() As Integer
        Get
            Return _IdMadeCountry
        End Get
    End Property
    Private _MadeCountry As String
    Public ReadOnly Property MadeCountry() As String
        Get
            Return _MadeCountry
        End Get
    End Property
    Private _IdEnginePowerSource As Integer
    Public ReadOnly Property IdEnginePowerSource() As Integer
        Get
            Return _IdEnginePowerSource
        End Get
    End Property
    Private _PowerSourceName As String
    Public ReadOnly Property PowerSourceName() As String
        Get
            Return UCase(_PowerSourceName)
        End Get
    End Property

    Public ReadOnly Property TotalWaight() As Decimal
        Get
            Return _maximunAllowedWaight + _emptyWaight
        End Get
    End Property
    Private _hook As Boolean
    Public ReadOnly Property Hook() As Boolean
        Get
            Return _hook
        End Get
    End Property
    Private _tng As Boolean
    Public ReadOnly Property TNG() As Boolean
        Get
            Return _tng
        End Get
    End Property
    Private _vitlo As Boolean
    Public ReadOnly Property Vitlo() As Boolean
        Get
            Return _vitlo
        End Get
    End Property
    Private _IdEngineSecondPowerSource As Integer
    Public ReadOnly Property IdEngineSecondPowerSource() As Integer
        Get
            Return _IdEngineSecondPowerSource
        End Get
    End Property
    Private _PowerSourceNameSecond As String
    Public ReadOnly Property PowerSourceNameSecond() As String
        Get
            Return UCase(_PowerSourceNameSecond)
        End Get
    End Property


    Private _FirstRegIssuer As String
    Public ReadOnly Property FirstRegIssuer() As String
        Get
            Return UCase(_FirstRegIssuer)
        End Get
    End Property

    Private _LastRegIssuer As String
    Public ReadOnly Property LastRegIssuer() As String
        Get
            Return UCase(_LastRegIssuer)
        End Get
    End Property
    Private _FirstRegIdCommunity As Integer
    Public ReadOnly Property FirstRegIdCommunity() As Integer
        Get
            Return UCase(_FirstRegIdCommunity)
        End Get
    End Property
    Private _LastRegIdCommunity As Integer
    Public ReadOnly Property LastRegIdCommunity() As Integer
        Get
            Return UCase(_LastRegIdCommunity)
        End Get
    End Property
    Private _MaxSpeed As Integer
    Public ReadOnly Property MaxSpeed() As Integer
        Get
            Return UCase(_MaxSpeed)
        End Get
    End Property
    Private _MasaPoOska1 As Integer
    Public ReadOnly Property MasaPoOska1() As Integer
        Get
            Return UCase(_MasaPoOska1)
        End Get
    End Property
    Private _MasaPoOska2 As Integer
    Public ReadOnly Property MasaPoOska2() As Integer
        Get
            Return UCase(_MasaPoOska2)
        End Get
    End Property
    Private _MasaPoOska3 As Integer
    Public ReadOnly Property MasaPoOska3() As Integer
        Get
            Return UCase(_MasaPoOska3)
        End Get
    End Property
    Private _MasaPoOska4 As Integer
    Public ReadOnly Property MasaPoOska4() As Integer
        Get
            Return UCase(_MasaPoOska4)
        End Get
    End Property
    Private _MasaPoOska5 As Integer
    Public ReadOnly Property MasaPoOska5() As Integer
        Get
            Return UCase(_MasaPoOska5)
        End Get
    End Property
    Private _OsnoOptovaruvanje1 As Integer
    Public ReadOnly Property OsnoOptovaruvanje1() As Integer
        Get
            Return UCase(_OsnoOptovaruvanje1)
        End Get
    End Property
    Private _OsnoOptovaruvanje2 As Integer
    Public ReadOnly Property OsnoOptovaruvanje2() As Integer
        Get
            Return UCase(_OsnoOptovaruvanje2)
        End Get
    End Property
    Private _OsnoOptovaruvanje3 As Integer
    Public ReadOnly Property OsnoOptovaruvanje3() As Integer
        Get
            Return UCase(_OsnoOptovaruvanje3)
        End Get
    End Property
    Private _OsnoOptovaruvanje4 As Integer
    Public ReadOnly Property OsnoOptovaruvanje4() As Integer
        Get
            Return UCase(_OsnoOptovaruvanje4)
        End Get
    End Property
    Private _OsnoOptovaruvanje5 As Integer
    Public ReadOnly Property OsnoOptovaruvanje5() As Integer
        Get
            Return UCase(_OsnoOptovaruvanje5)
        End Get
    End Property

    Private _MaxKonstVkMasaKocnaPrikolka As Integer
    Public ReadOnly Property MaxKonstVkMasaKocnaPrikolka() As Integer
        Get
            Return UCase(_MaxKonstVkMasaKocnaPrikolka)
        End Get
    End Property
    Private _MaxKonstVkMasaNeKocnaPrikolka As Integer
    Public ReadOnly Property MaxKonstVkMasaNeKocnaPrikolka() As Integer
        Get
            Return UCase(_MaxKonstVkMasaNeKocnaPrikolka)
        End Get
    End Property
    Private _OznakaNaOdobrenieZaPriklucUred As String
    Public ReadOnly Property OznakaNaOdobrenieZaPriklucUred() As String
        Get
            Return UCase(_OznakaNaOdobrenieZaPriklucUred)
        End Get
    End Property
    Private _NoiseTechnicalSpec As String
    Public ReadOnly Property NoiseTechnicalSpec() As String
        Get
            Return UCase(_NoiseTechnicalSpec)
        End Get
    End Property
    Private _BrojEUPotvrda As String
    Public ReadOnly Property BrojEUPotvrda() As String
        Get
            Return UCase(_BrojEUPotvrda)
        End Get
    End Property
    Private _OznakaNaOdobrenie As String
    Public ReadOnly Property OznakaNaOdobrenie() As String
        Get
            Return UCase(_OznakaNaOdobrenie)
        End Get
    End Property
    Private _MaxKonstOptovaruvanjeVoPriklucok As Integer
    Public ReadOnly Property MaxKonstOptovaruvanjeVoPriklucok() As Integer
        Get
            Return UCase(_MaxKonstOptovaruvanjeVoPriklucok)
        End Get
    End Property
    Private _CO2 As Decimal
    Public ReadOnly Property CO2() As Decimal
        Get
            Return UCase(_CO2)
        End Get
    End Property
    Private _BrojNaVrtezi As Integer
    Public ReadOnly Property BrojNaVrtezi() As Integer
        Get
            Return UCase(_BrojNaVrtezi)
        End Get
    End Property
    Private _MaxKonstVkMasa As Decimal
    Public ReadOnly Property MaxKonstVkMasa() As Decimal
        Get
            Return UCase(_MaxKonstVkMasa)
        End Get
    End Property
    Private _MaxLegVkMasa As Decimal
    Public ReadOnly Property MaxLegVkMasa() As Decimal
        Get
            Return UCase(_MaxLegVkMasa)
        End Get
    End Property
    Private _MaxLegVkMasaGrupa As Decimal
    Public ReadOnly Property MaxLegVkMasaGrupa() As Decimal
        Get
            Return UCase(_MaxLegVkMasaGrupa)
        End Get
    End Property
    Private _OdnosKwCcm As String
    Public ReadOnly Property OdnosKwCcm() As String
        Get
            Return UCase(_OdnosKwCcm)
        End Get
    End Property
    Private _Tip As String
    Public ReadOnly Property Tip() As String
        Get
            Return UCase(_Tip)
        End Get
    End Property
    Private _IdentifikacijaNaMotorMestoMetod As String
    Public ReadOnly Property IdentifikacijaNaMotorMestoMetod() As String
        Get
            Return UCase(_IdentifikacijaNaMotorMestoMetod)
        End Get
    End Property

    Private _TMinMasa As Integer
    Public ReadOnly Property TMinMasa() As Integer
        Get
            Return UCase(_TMinMasa)
        End Get
    End Property
    Private _TMaxKonstVkMasaPrikolka As Integer
    Public ReadOnly Property TMaxKonstVkMasaPrikolka() As Integer
        Get
            Return UCase(_TMaxKonstVkMasaPrikolka)
        End Get
    End Property
    Private _TMaxKonstVkMasaPoluprikolka As Integer
    Public ReadOnly Property TMaxKonstVkMasaPoluprikolka() As Integer
        Get
            Return UCase(_TMaxKonstVkMasaPoluprikolka)
        End Get
    End Property
    Private _TMaxKonstVkMasaPrikolkaSoCenOska As Integer
    Public ReadOnly Property TMaxKonstVkMasaPrikolkaSoCenOska() As Integer
        Get
            Return UCase(_TMaxKonstVkMasaPrikolkaSoCenOska)
        End Get
    End Property
    Private _TMaxKonstVkMasaNaKombinacija As Integer
    Public ReadOnly Property TMaxKonstVkMasaNaKombinacija() As Integer
        Get
            Return UCase(_TMaxKonstVkMasaNaKombinacija)
        End Get
    End Property
    Private _TMaxKonstVkMasaPrikolkaStoMozePrikluci As Integer
    Public ReadOnly Property TMaxKonstVkMasaPrikolkaStoMozePrikluci() As Integer
        Get
            Return UCase(_TMaxKonstVkMasaPrikolkaStoMozePrikluci)
        End Get
    End Property
    Private _TZastitnaRamka As String
    Public ReadOnly Property TZastitnaRamka() As String
        Get
            Return UCase(_TZastitnaRamka)
        End Get
    End Property
    Private _NoiseStatic As Decimal
    Public ReadOnly Property NoiseStatic() As Decimal
        Get
            Return UCase(_NoiseStatic)
        End Get
    End Property
    Private _TZastitnaKabina As String
    Public ReadOnly Property TZastitnaKabina() As String
        Get
            Return UCase(_TZastitnaKabina)
        End Get
    End Property

    Private _TMarkaMehanPriklucok As String
    Public ReadOnly Property TMarkaMehanPriklucok() As String
        Get
            Return UCase(_TMarkaMehanPriklucok)
        End Get
    End Property
    Private _TTipMehanPriklucok As String
    Public ReadOnly Property TTipMehanPriklucok() As String
        Get
            Return UCase(_TTipMehanPriklucok)
        End Get
    End Property
    Private _TBrOdobrenieMehanPriklucok As String
    Public ReadOnly Property TBrOdobrenieMehanPriklucok() As String
        Get
            Return UCase(_TBrOdobrenieMehanPriklucok)
        End Get
    End Property
    Private _Tyre As String
    Public ReadOnly Property Tyre() As String
        Get
            Return UCase(_Tyre)
        End Get
    End Property

    Private _TMaxHorVerOptovaruvanjePriklucok As Integer
    Public ReadOnly Property TMaxHorVerOptovaruvanjePriklucok() As Integer
        Get
            Return UCase(_TMaxHorVerOptovaruvanjePriklucok)
        End Get
    End Property
    Protected Overrides Function GetIdValue() As Object
        Return _id
    End Function

    Public Overrides Function ToString() As String
        Return _id
    End Function

    Friend Sub New(ByVal dr As SafeDataReader)
        _id = dr.GetInt64("Id")
        _idVehicleBodyType = dr.GetInt32("IdVehicleBodyType")
        _idVehicleCategories = dr.GetInt32("IdVehicleCategories")
        _categoryCode = dr.GetString("CategoryCode")
        _categoryName = dr.GetString("CategoryName")
        _bodytypeCode = dr.GetString("BodytypeCode")
        _bodytypeDescriprion = dr.GetString("BodytypeDescriprion")
        _oldBodytypeDescription = dr.GetString("OldBodytypeDescription")
        _oldCategoryName = dr.GetString("OldCategoryName")
        _idVehicleCategoryForPayments = dr.GetInt32("IdVehicleCategoryForPayments")
        _categoryForPaymentsCode = dr.GetString("CategoryForPaymentsCode")
        _categoryForPaymentsName = dr.GetString("CategoryForPaymentsName")
        _idVehicleUse = dr.GetInt32("IdVehicleUse")
        _useDescription = dr.GetString("UseDescription")
        _idVehicleModel = dr.GetInt32("IdVehicleModel")
        _modelCode = dr.GetString("ModelCode")
        _modelName = dr.GetString("ModelName")
        _idVehicleMaker = dr.GetInt32("IdVehicleMaker")
        _vehicleMaker = dr.GetString("VehicleMaker")
        _idPrimaryColor = dr.GetInt32("IdPrimaryColor")
        _idSecondaryColor = dr.GetInt32("IdSecondaryColor")
        _colorCode = dr.GetString("ColorCode")
        _colorDescription = dr.GetString("ColorDescription")
        _colorCodeSecondary = dr.GetString("ColorCodeSecondary")
        _colorDescriptionSecondary = dr.GetString("ColorDescriptionSecondary")
        _idEngineType = dr.GetInt32("IdEngineType")
        _engineTypeCode = dr.GetString("EngineTypeCode")
        _vehicleEngineTypeTechincalDescription = dr.GetString("VehicleEngineTypeTechincalDescription")
        _engineNumber = dr.GetString("EngineNumber")
        _enginePower = dr.GetValue("EnginePower")
        _enginePowerOutPut = dr.GetValue("EnginePowerOutPut")
        _engineTorque = dr.GetString("EngineTorque")
        _shellNumber = dr.GetString("ShellNumber")
        _makeDate = dr.GetDateTime("MakeDate")
        _numberOfDoors = dr.GetInt32("NumberOfDoors")
        _numberOfSeats = dr.GetInt16("NumberOfSeats")
        _numberOfStandingSeats = dr.GetInt16("NumberOfStandingSeats")
        _numberOfLieingSeats = dr.GetInt16("NumberOfLieingSeats")
        _emptyWaight = dr.GetValue("EmptyWaight")
        _maximunAllowedWaight = dr.GetValue("MaximunAllowedWaight")
        _numberOfAxis = dr.GetInt32("NumberOfAxis")
        _propulsionAxis = dr.GetInt32("PropulsionAxis")
        _numberOfWheels = dr.GetInt32("NumberOfWheels")
        _numberOfPropulsionWheels = dr.GetInt32("NumberOfPropulsionWheels")
        _vehicleSizeHight = dr.GetValue("VehicleSizeHight")
        _vehicleSizeWidth = dr.GetValue("VehicleSizeWidth")
        _vehicleSizeLength = dr.GetValue("VehicleSizeLength")
        _EngineWorkingCapacity = dr.GetValue("EngineWorkingCapacity")
        _IdMadeCountry = dr.GetValue("IdMadeCountry")
        _MadeCountry = dr.GetString("MadeCountry")
        _IdEnginePowerSource = dr.GetInt32("IdEnginePowerSource")
        _PowerSourceName = dr.GetString("PowerSourceName")
        _hook = dr.GetBoolean("Hook")
        _vitlo = dr.GetBoolean("Vitlo")
        _IdEngineSecondPowerSource = dr.GetInt32("IdEngineSecondPowerSource")
        _PowerSourceNameSecond = dr.GetString("PowerSourceNameSecond")
        _VehicleModelAdding = dr.GetString("VehicleModelAdding")
        _tng = dr.GetBoolean("TNG")
        'Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_id)
        _lastRegistrationNumber = dr.GetString("LastRegistratinNumber") 'regInfo.RegistrationNumber
        _dateOfLastRegistration = dr.GetDateTime("LastRegistrationMakeDate") 'regInfo.RegistrationDate
        '_lastRegistrationPlace = dr.GetString("LastRegistrationCommunity") 'regInfo.RegistrationPlace
        _lastRegistrationValidTill = dr.GetDateTime("LastRegistrationValidTill") 'regInfo.DateRegistrationValidTill
        'Dim firstRegInfo As registrationBasicInfo = Vehicle.GetVehicleFirstRegistration(_id)
        _firstRegistrationNumber = dr.GetString("FirstRegistrationNumber") 'firstRegInfo.RegistrationNumber
        _firstRegistrationDate = dr.GetDateTime("FirstRegistrationMakeDate") 'firstRegInfo.RegistrationDate
        '_firstRegistrationPlace = dr.GetString("FirstRegistrationCommunity") 'firstRegInfo.RegistrationPlace
        _firstRegistrationValidTill = dr.GetDateTime("FirstRegistrationValidTill") 'firstRegInfo.DateRegistrationValidTill
        _FirstRegIssuer = dr.GetString("FirstRegIssuer")
        _LastRegIssuer = dr.GetString("LastRegIssuer")
        _FirstRegIdCommunity = dr.GetInt32("FirstRegIdCommunity")
        _LastRegIdCommunity = dr.GetInt32("LastRegIdCommunity")
        _Tip = dr.GetString("Tip")
        _IdentifikacijaNaMotorMestoMetod = dr.GetString("IdentifikacijaNaMotorMestoMetod")
        _TMinMasa = dr.GetInt32("TMinMasa")
        _TMaxKonstVkMasaPrikolka = dr.GetInt32("TMaxKonstVkMasaPrikolka")
        _TMaxKonstVkMasaPoluprikolka = dr.GetInt32("TMaxKonstVkMasaPoluprikolka")
        _TMaxKonstVkMasaPrikolkaSoCenOska = dr.GetInt32("TMaxKonstVkMasaPrikolkaSoCenOska")
        _TMaxKonstVkMasaNaKombinacija = dr.GetInt32("TMaxKonstVkMasaNaKombinacija")
        _TMaxKonstVkMasaPrikolkaStoMozePrikluci = dr.GetInt32("TMaxKonstVkMasaPrikolkaStoMozePrikluci")
        _NoiseStatic = dr.GetValue("NoiseStatic")
        _TZastitnaKabina = dr.GetString("TZastitnaKabina")
        _TZastitnaRamka = dr.GetString("TZastitnaRamka")
        _TMarkaMehanPriklucok = dr.GetString("TMarkaMehanPriklucok")
        _TTipMehanPriklucok = dr.GetString("TTipMehanPriklucok")
        _TBrOdobrenieMehanPriklucok = dr.GetString("TBrOdobrenieMehanPriklucok")
        _TMaxHorVerOptovaruvanjePriklucok = dr.GetInt32("TMaxHorVerOptovaruvanjePriklucok")
        _BrojNaVrtezi = dr.GetInt32("BrojNaVrtezi")
        _CO2 = dr.GetValue("CO2")
        _MaxKonstVkMasa = dr.GetValue("MaxKonstVkMasa")
        _MaxLegVkMasa = dr.GetValue("MaxLegVkMasa")
        _MaxLegVkMasaGrupa = dr.GetValue("MaxLegVkMasaGrupa")
        _MaxSpeed = dr.GetValue("MaxSpeed")
        _MasaPoOska1 = dr.GetInt32("MasaPoOska1")
        _MasaPoOska2 = dr.GetInt32("MasaPoOska2")
        _MasaPoOska3 = dr.GetInt32("MasaPoOska3")
        _MasaPoOska4 = dr.GetInt32("MasaPoOska4")
        _MasaPoOska5 = dr.GetInt32("MasaPoOska5")
        _OsnoOptovaruvanje1 = dr.GetInt32("OsnoOptovaruvanje1")
        _OsnoOptovaruvanje2 = dr.GetInt32("OsnoOptovaruvanje2")
        _OsnoOptovaruvanje3 = dr.GetInt32("OsnoOptovaruvanje3")
        _OsnoOptovaruvanje4 = dr.GetInt32("OsnoOptovaruvanje4")
        _OsnoOptovaruvanje5 = dr.GetInt32("OsnoOptovaruvanje5")
        _MaxKonstOptovaruvanjeVoPriklucok = dr.GetInt32("MaxKonstOptovaruvanjeVoPriklucok")
        _NoiseTechnicalSpec = dr.GetString("NoiseTechnicalSpec")
        _OznakaNaOdobrenie = dr.GetString("OznakaNaOdobrenie")
        _BrojEUPotvrda = dr.GetString("BrojEUPotvrda")
        _OdnosKwCcm = dr.GetString("OdnosKwCcm")
        _MaxKonstVkMasaKocnaPrikolka = dr.GetInt32("MaxKonstVkMasaKocnaPrikolka")
        _MaxKonstVkMasaNeKocnaPrikolka = dr.GetInt32("MaxKonstVkMasaNeKocnaPrikolka")
        _OznakaNaOdobrenieZaPriklucUred = dr.GetString("OznakaNaOdobrenieZaPriklucUred")
        _Tyre = dr.GetString("Tyre")



    End Sub

End Class