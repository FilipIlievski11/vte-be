
<Serializable()> _
Public Class PrintTrafficLicenceInfo
    Inherits ReadOnlyBase(Of PrintTrafficLicenceInfo)

#Region " Calculated Fields "
    Private _customerCity As String
    Public ReadOnly Property LivingAddressDisplay() As String
        Get
            If _streetName = String.Empty Or _streetName = " " Then
                Return _customerCity
            Else
                If _customerCity <> String.Empty AndAlso UCase(ToLat(_customerCity)) <> UCase(ToLat(_communityCountry)) Then
                    Return UCase(ToLat(_streetName & " бр." & _livingAddressNumber)) & ", " & UCase(ToLat(_customerCity))
                Else
                    Return UCase(ToLat(_streetName & " бр." & _livingAddressNumber))
                End If
            End If

        End Get
    End Property
    Public ReadOnly Property ModelPlusTNG() As String
        Get
            If _TNG Then
                Return (UCase(ToLat(_modelName)) & " " & (UCase(ToLat(_VehicleModelAdding))) & " " & "TNG")
            Else
                Return UCase(ToLat(_modelName)) & " " & (UCase(ToLat(_VehicleModelAdding)))
            End If
        End Get
    End Property
    Public ReadOnly Property EnginePowerDisplay() As String
        Get
            If _enginePower <> 0 Then
                Return UCase(ToLat(_enginePowerOutPut & "(" & _enginePower & ")"))
            Else
                Return UCase(ToLat(_enginePowerOutPut))
            End If
        End Get
    End Property

    Public ReadOnly Property BodiTypeUseDisplay() As String
        Get
            Return UCase(ToLat(_bodytypeDescriprion & ", " & _useDescription))
        End Get
    End Property
    Public ReadOnly Property RegNumberShort() As String
        Get
            If _lastRegistrationNumber <> String.Empty Then
                Return UCase(_lastRegistrationNumber.Substring(3))
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property FirstColor() As String
        Get
            Return (_colorCode & " " & _colorDescription)
        End Get
    End Property
    Public ReadOnly Property SecondColor() As String
        Get
            Return (_secondaryColorCode & " " & _secondaryColorDescription)
        End Get
    End Property
    'Public ReadOnly Property ColorDisplay() As String
    '    Get
    '        If _secondaryColorCode = String.Empty Then
    '            If Not _colorCode = String.Empty Then
    '                Return UCase(ToLat(_colorCode & " " & _colorDescription))
    '            Else
    '                Return ""
    '            End If
    '        Else
    '            'If _colorCode = String.Empty Then
    '            Return UCase(ToLat(_colorCode & " " & _colorDescription & ", " & _
    '            _secondaryColorCode & " " & _secondaryColorDescription))
    '            '    Else
    '            '    Return ""
    '            'End If
    '        End If
    '    End Get
    'End Property

#End Region

    Private _id As Long
    Public ReadOnly Property Id() As Long
        Get
            Return _id
        End Get
    End Property
    Private _idCustomerVehicleRelation As Long
    Public ReadOnly Property IdCustomerVehicleRelation() As Long
        Get
            Return _idCustomerVehicleRelation
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
    Private _idTehnicalExamOrganizationsIssuedBy As Integer
    Public ReadOnly Property IdTehnicalExamOrganizationsIssuedBy() As Integer
        Get
            Return _idTehnicalExamOrganizationsIssuedBy
        End Get
    End Property
    Private _issuerName As String
    Public ReadOnly Property IssuerName() As String
        Get
            Return UCase(ToLat(_issuerName))
        End Get
    End Property
    Private _trafficLicenceNumber As String
    Public ReadOnly Property TrafficLicenceNumber() As String
        Get
            Return UCase(ToLat(_trafficLicenceNumber))
        End Get
    End Property
    Private _madeDate As Date
    Public ReadOnly Property MadeDate() As Date
        Get
            Return _madeDate
        End Get
    End Property
    Private _endDate As Date
    Public ReadOnly Property EndDate() As Date
        Get
            Return _endDate
        End Get
    End Property
    Private _note As String
    Public ReadOnly Property Note() As String
        Get
            Return _note
        End Get
    End Property
    Private _mb As String
    Public ReadOnly Property MB() As String
        Get
            Return _mb
        End Get
    End Property
    Private _customerSurname As String
    Public ReadOnly Property CustomerSurname() As String
        Get
            Return UCase(ToLat(_customerSurname))
        End Get
    End Property
    Private _customerFirstName As String
    Public ReadOnly Property CustomerFirstName() As String
        Get
            Return UCase(ToLat(_customerFirstName))
        End Get
    End Property
    Private _idLivingAddress As Integer
    Public ReadOnly Property IdLivingAddress() As Integer
        Get
            Return _idLivingAddress
        End Get
    End Property
    Private _streetName As String
    Public ReadOnly Property StreetName() As String
        Get
            Return UCase(ToLat(_streetName))
        End Get
    End Property
    Private _livingAddressNumber As String
    Public ReadOnly Property LivingAddressNumber() As String
        Get
            Return UCase(ToLat(_livingAddressNumber))
        End Get
    End Property
    Private _idVehicleCategoryForPayments As Integer
    Public ReadOnly Property IdVehicleCategoryForPayments() As Integer
        Get
            Return _idVehicleCategoryForPayments
        End Get
    End Property
    Private _vehicleCategoryForPaymentName As String
    Public ReadOnly Property VehicleCategoryForPaymentName() As String
        Get
            Return UCase(ToLat(_vehicleCategoryForPaymentName))
        End Get
    End Property
    Private _idVehicleModel As Integer
    Public ReadOnly Property IdVehicleModel() As Integer
        Get
            Return _idVehicleModel
        End Get
    End Property
    Private _modelName As String
    Public ReadOnly Property ModelName() As String
        Get
            Return UCase(ToLat(_modelName))
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
            Return UCase(ToLat(_vehicleMaker))
        End Get
    End Property
    Private _shellNumber As String
    Public ReadOnly Property ShellNumber() As String
        Get
            Return UCase(ToLat(_shellNumber))
        End Get
    End Property
    Private _engineNumber As String
    Public ReadOnly Property EngineNumber() As String
        Get
            If (_engineNumber IsNot Nothing) AndAlso (_engineNumber <> String.Empty) Then
                Return UCase(ToLat(_engineNumber))
            Else
                Return 0
            End If

        End Get
    End Property
    Private _makeDate As Date
    Public ReadOnly Property MakeDate() As Date
        Get
            Return _makeDate
        End Get
    End Property
    Private _enginePower As Single
    Public ReadOnly Property EnginePower() As Single
        Get
            Return _enginePower
        End Get
    End Property
    Private _engineTorque As String
    Public ReadOnly Property EngineTorque() As String
        Get
            Return _engineTorque
        End Get
    End Property
    Private _engineWorkingCapacity As Single
    Public ReadOnly Property EngineWorkingCapacity() As Single
        Get
            Return _engineWorkingCapacity
        End Get
    End Property
    Private _enginePowerOutPut As Single
    Public ReadOnly Property EnginePowerOutPut() As Single
        Get
            Return _enginePowerOutPut
        End Get
    End Property
    Private _vehcilceCategoryForPaymentCode As String
    Public ReadOnly Property VehcilceCategoryForPaymentCode() As String
        Get
            Return UCase(ToLat(_vehcilceCategoryForPaymentCode))
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
    Private _bodytypeDescriprion As String
    Public ReadOnly Property BodytypeDescriprion() As String
        Get
            Return UCase(ToLat(_bodytypeDescriprion))
        End Get
    End Property
    Private _oldBodytypeDescription As String
    Public ReadOnly Property OldBodytypeDescription() As String
        Get
            Return UCase(ToLat(_oldBodytypeDescription))
        End Get
    End Property
    Private _bodytypeCode As String
    Public ReadOnly Property BodytypeCode() As String
        Get
            Return UCase(ToLat(_bodytypeCode))
        End Get
    End Property
    Private _idVehicleBodyType As Integer
    Public ReadOnly Property IdVehicleBodyType() As Integer
        Get
            Return _idVehicleBodyType
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
            Return UCase(ToLat(_useDescription))
        End Get
    End Property
    Private _idPrimaryColor As Integer
    Public ReadOnly Property IdPrimaryColor() As Integer
        Get
            Return _idPrimaryColor
        End Get
    End Property
    Private _TNG As Boolean
    Public ReadOnly Property TNG() As Integer
        Get
            Return _TNG
        End Get
    End Property
    Private _colorCode As String
    Public ReadOnly Property ColorCode() As String
        Get
            Return UCase(ToLat(_colorCode))
        End Get
    End Property
    Private _colorDescription As String
    Public ReadOnly Property ColorDescription() As String
        Get
            Return UCase(ToLat(_colorDescription))
        End Get
    End Property
    Private _idSecondaryColor As Integer
    Public ReadOnly Property IdSecondaryColor() As Integer
        Get
            Return _idSecondaryColor
        End Get
    End Property
    Private _secondaryColorCode As String
    Public ReadOnly Property SecondaryColorCode() As String
        Get
            Return UCase(ToLat(_secondaryColorCode))
        End Get
    End Property
    Private _secondaryColorDescription As String
    Public ReadOnly Property SecondaryColorDescription() As String
        Get
            Return UCase(ToLat(_secondaryColorDescription))
        End Get
    End Property
    Private _numberOfDoors As Integer
    Public ReadOnly Property NumberOfDoors() As Integer
        Get
            Return _numberOfDoors
        End Get
    End Property
    Private _numberOfAxis As Integer
    Public ReadOnly Property NumberOfAxis() As Integer
        Get
            Return _numberOfAxis
        End Get
    End Property
    'Private _firstRegistrationPlace As String = String.Empty
    'Public ReadOnly Property FirstRegistrationPlace() As String
    '    Get
    '        Return UCase(ToLat(_firstRegistrationPlace))
    '    End Get
    'End Property
    Private _VehicleModelAdding As String = String.Empty
    Public ReadOnly Property VehicleModelAdding() As String
        Get
            Return UCase(ToLat(_VehicleModelAdding))
        End Get
    End Property

    Private _firstRegistrationDate As Date = Now
    Public ReadOnly Property FirstRegistrationDate() As Date
        Get
            Return _firstRegistrationDate
        End Get
    End Property
    Private _firstRegistrationNumber As String = String.Empty
    Public ReadOnly Property FirstRegistrationNumber() As String
        Get
            If _firstRegistrationNumber <> "непозната" AndAlso _firstRegistrationNumber <> String.Empty Then
                Return UCase(_firstRegistrationNumber)
            Else
                Return ""
            End If

        End Get
    End Property
    Private _lastRegistrationPlace As String
    Public ReadOnly Property LastRegistrationPlace() As String
        Get
            Return UCase(ToLat(_lastRegistrationPlace))
        End Get
    End Property
    Private _dateOfLastRegistration As Date
    Public ReadOnly Property DateOfLastRegistration() As Date
        Get
            Return _dateOfLastRegistration
        End Get
    End Property
    Private _lastRegistrationNumber As String
    Public ReadOnly Property LastRegistrationNumber() As String
        Get
            Return UCase(ToLat(_lastRegistrationNumber))
        End Get
    End Property
    Private _idVehicleCategory As Integer
    Public ReadOnly Property IdVehicleCategory() As Integer
        Get
            Return _idVehicleCategory
        End Get
    End Property
    Private _communityCountry As String
    Public ReadOnly Property CommunityCountry() As String
        Get
            Return UCase(ToLat(_communityCountry))
        End Get
    End Property
    Private _categoryName As String
    Public ReadOnly Property CategoryName() As String
        Get
            Return UCase(ToLat(_categoryName))
        End Get
    End Property
    Private _engineType As String
    Public ReadOnly Property EngineType() As String
        Get
            Return UCase(ToLat(_engineType))
        End Get
    End Property

    Private _IdFirstRegistrationIssuer As Integer
    Public ReadOnly Property IdFirstRegistrationIssuer() As Integer
        Get
            Return _IdFirstRegistrationIssuer
        End Get
    End Property
    Public ReadOnly Property FirstRegistrationIssuer() As String
        Get
            Dim objRegistrationIssuerList As RegistrationIssuerList = Csla.ApplicationContext.LocalContext("objRegistrationIssuerList")
            Dim firstRegIssuer As RegistrationIssuerInfo = objRegistrationIssuerList.GetRegistrationIssuerInfo(_IdFirstRegistrationIssuer)
            Return firstRegIssuer.IssuerName
        End Get
    End Property
    Private _IdLastRegistrationIssuer As Integer
    Public ReadOnly Property IdLastRegistrationIssuer() As Integer
        Get
            Return _IdLastRegistrationIssuer
        End Get
    End Property

    Private _LastRegIssuerName As String
    Public ReadOnly Property LastRegIssuerName() As String
        Get
            Return UCase(ToLat(_LastRegIssuerName))
        End Get
    End Property
    Private _firstRegIdCommunity As Integer
    Public ReadOnly Property FirstRegIdCommunity() As Integer
        Get
            Return _firstRegIdCommunity
        End Get
    End Property
    Private _lastRegIdCommunity As Integer
    Public ReadOnly Property LastRegIdCommunity() As Integer
        Get
            Return _lastRegIdCommunity
        End Get
    End Property

    Public ReadOnly Property firstRegPlace() As String
        Get
            Dim communityList As CommunitiesList = Csla.ApplicationContext.LocalContext.Item("objCommunityList")
            Dim place As String
            If _firstRegIdCommunity > 0 Then
                place = UCase(ToLat(communityList.GetCommunitiesListById(_firstRegIdCommunity).CommunityName))
            Else
                place = ""
            End If

            Return place
        End Get
        
    End Property

    Public ReadOnly Property LastRegPlace() As String
        Get
            Dim communityList As CommunitiesList = Csla.ApplicationContext.LocalContext.Item("objCommunityList")
            Dim place As String
            If _lastRegIdCommunity > 0 Then
                place = communityList.GetCommunitiesListById(_lastRegIdCommunity).CommunityName
            Else
                place = ""
            End If

            Return place
        End Get

    End Property

    Private _IsCompany As Boolean
    Public ReadOnly Property IsCompany() As Boolean
        Get
            Return _IsCompany
        End Get
    End Property

    Private _IdEnginePowerSource As Integer
    Public ReadOnly Property IdEnginePowerSource() As Integer
        Get
            Return _IdEnginePowerSource
        End Get
    End Property

    Private _IdEngineSecondPowerSource As Integer
    Public ReadOnly Property IdEngineSecondPowerSource() As Integer
        Get
            Return _IdEngineSecondPowerSource
        End Get
    End Property
    Dim objVehicleEnginePowerSourceTypeList As VehicleEnginePowerSourceTypeList = Csla.ApplicationContext.LocalContext("objVehicleEnginePowerSourceTypeList")
    Public ReadOnly Property FirstEnginePowerSource() As String
        Get
            Try
                Return objVehicleEnginePowerSourceTypeList.GetPowerSourceTypeInfo(_IdEnginePowerSource).PowerSourceName
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    Public ReadOnly Property SecondEnginePowerSource() As String
        Get
            Try
                Return objVehicleEnginePowerSourceTypeList.GetPowerSourceTypeInfo(_IdEngineSecondPowerSource).PowerSourceName
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    Public ReadOnly Property EnginePowerSource() As String
        Get
            Return (FirstEnginePowerSource() + "; " + SecondEnginePowerSource())
        End Get
    End Property

    Private _MaxSpeed As Single
    Public ReadOnly Property MaxSpeed() As Single
        Get
            Return _MaxSpeed
        End Get
    End Property

    Private _VehicleSizeHight As Single
    Public ReadOnly Property VehicleSizeHight() As Single
        Get
            Return _VehicleSizeHight
        End Get
    End Property
    Private _VehicleSizeWidth As Single
    Public ReadOnly Property VehicleSizeWidth() As Single
        Get
            Return _VehicleSizeWidth
        End Get
    End Property
    Private _VehicleSizeLength As Single
    Public ReadOnly Property VehicleSizeLength() As Single
        Get
            Return _VehicleSizeLength
        End Get
    End Property

    Private _NoiseStatic As Single
    Public ReadOnly Property NoiseStatic() As Single
        Get
            Return _NoiseStatic
        End Get
    End Property
    Private _CO2 As Single
    Public ReadOnly Property CO2() As Single
        Get
            Return _CO2
        End Get
    End Property

    Private _Tip As String
    Public ReadOnly Property Tip() As String
        Get
            Return _Tip
        End Get
    End Property

    Private _VarijantaIzvedba As String
    Public ReadOnly Property VarijantaIzvedba() As String
        Get
            Return _VarijantaIzvedba
        End Get
    End Property

    Public ReadOnly Property TipVarijantaIzvedba() As String
        Get
            Return _Tip + "/" + _VarijantaIzvedba
        End Get
    End Property

    Private _OznakaNaOdobrenie As String
    Public ReadOnly Property OznakaNaOdobrenie() As String
        Get
            Return _OznakaNaOdobrenie
        End Get
    End Property

    Private _OdnosKwCcm As String
    Public ReadOnly Property OdnosKwCcm() As String
        Get
            Return _OdnosKwCcm
        End Get
    End Property

    Private _MaxLegVkMasa As Double
    Public ReadOnly Property MaxLegVkMasa() As Double
        Get
            Return _MaxLegVkMasa
        End Get
    End Property

    Private _MaxLegVkMasaGrupa As Double
    Public ReadOnly Property MaxLegVkMasaGrupa() As Double
        Get
            Return _MaxLegVkMasaGrupa
        End Get
    End Property

    Private _MasaPoOska1 As Integer
    Public ReadOnly Property MasaPoOska1() As Integer
        Get
            Return _MasaPoOska1
        End Get
    End Property

    Private _MasaPoOska2 As Integer
    Public ReadOnly Property MasaPoOska2() As Integer
        Get
            Return _MasaPoOska2
        End Get
    End Property

    Private _MasaPoOska3 As Integer
    Public ReadOnly Property MasaPoOska3() As Integer
        Get
            Return _MasaPoOska3
        End Get
    End Property
    Public ReadOnly Property MasaPoOska123() As String
        Get
            If _MasaPoOska2 > 0 Then
                If _MasaPoOska3 > 0 Then
                    Return "Oska1=" + _MasaPoOska1 + "; Oska2=" + _MasaPoOska2 + "; Oska3=" + _MasaPoOska3
                Else
                    Return "Oska1=" + _MasaPoOska1 + "; Oska2=" + _MasaPoOska2
                End If
            Else
                If _MasaPoOska1 > 0 Then
                    Return "Oska1=" + _MasaPoOska1
                Else
                    Return ""
                End If
            End If

        End Get
    End Property

    Private _MasaPoOska4 As Integer
    Public ReadOnly Property MasaPoOska4() As Integer
        Get
            Return _MasaPoOska4
        End Get
    End Property

    Private _MasaPoOska5 As Integer
    Public ReadOnly Property MasaPoOska5() As Integer
        Get
            Return _MasaPoOska5
        End Get
    End Property
   
    Private _MasaPoOskaPriklucna As Integer
    Public ReadOnly Property MasaPoOskaPriklucna() As Integer
        Get
            Return _MasaPoOskaPriklucna
        End Get
    End Property

    Public ReadOnly Property MasaPoOska45Priklucna() As String
        Get
            If _MasaPoOska4 > 0 Then
                If _MasaPoOska5 > 0 Then
                    If _MasaPoOskaPriklucna > 0 Then
                        Return "Oska4=" + _MasaPoOska4 + "; Oska5=" + _MasaPoOska5 + "; OskaPriklucna=" + _MasaPoOskaPriklucna
                    Else
                        Return "Oska4=" + _MasaPoOska4 + "; Oska5=" + _MasaPoOska5
                    End If

                Else
                    Return "Oska4=" + _MasaPoOska4
                End If
            Else
                Return ""
            End If
        End Get
    End Property
    Private _OsnoOptovaruvanje1 As Integer
    Public ReadOnly Property OsnoOptovaruvanje1() As Integer
        Get
            Return _OsnoOptovaruvanje1
        End Get
    End Property

    Private _OsnoOptovaruvanje2 As Integer
    Public ReadOnly Property OsnoOptovaruvanje2() As Integer
        Get
            Return _OsnoOptovaruvanje2
        End Get
    End Property

    Private _OsnoOptovaruvanje3 As Integer
    Public ReadOnly Property OsnoOptovaruvanje3() As Integer
        Get
            Return _OsnoOptovaruvanje3
        End Get
    End Property
    Public ReadOnly Property OsnoOptovaruvanje123() As String
        Get
            If _OsnoOptovaruvanje2 > 0 Then
                If _OsnoOptovaruvanje3 > 0 Then
                    Return "Oska1=" + _OsnoOptovaruvanje1 + "; Oska2=" + _OsnoOptovaruvanje2 + "; Oska3=" + _OsnoOptovaruvanje3
                Else
                    Return "Oska1=" + _OsnoOptovaruvanje1 + "; Oska2=" + _OsnoOptovaruvanje2
                End If
            Else
                If _MasaPoOska1 > 0 Then
                    Return "Oska1=" + _OsnoOptovaruvanje1
                Else
                    Return ""
                End If
            End If

        End Get
    End Property
    Private _OsnoOptovaruvanje4 As Integer
    Public ReadOnly Property OsnoOptovaruvanje4() As Integer
        Get
            Return _OsnoOptovaruvanje4
        End Get
    End Property
    Private _OsnoOptovaruvanje5 As Integer
    Public ReadOnly Property OsnoOptovaruvanje5() As Integer
        Get
            Return _OsnoOptovaruvanje5
        End Get
    End Property
    Private _OsnoOptovaruvanjePriklucna As Integer
    Public ReadOnly Property OsnoOptovaruvanjePriklucna() As Integer
        Get
            Return _OsnoOptovaruvanjePriklucna
        End Get
    End Property
    Public ReadOnly Property OsnoOptovaruvanje45Priklucna() As String
        Get
            If _OsnoOptovaruvanje4 > 0 Then
                If _OsnoOptovaruvanje5 > 0 Then
                    If _OsnoOptovaruvanjePriklucna > 0 Then
                        Return "Oska4=" + _OsnoOptovaruvanje4 + "; Oska5=" + _OsnoOptovaruvanje5 + "; Priklucna=" + _OsnoOptovaruvanjePriklucna
                    Else
                        Return "Oska4=" + _OsnoOptovaruvanje4 + "; Oska5=" + _OsnoOptovaruvanje5
                    End If

                Else
                    Return "Oska4=" + _OsnoOptovaruvanje4
                End If
            Else
                Return ""
            End If
        End Get
    End Property
    Private _MaxKonstVkMasaKocnaPrikolka As Integer
    Public ReadOnly Property MaxKonstVkMasaKocnaPrikolka() As Integer
        Get
            Return _MaxKonstVkMasaKocnaPrikolka
        End Get
    End Property
    Private _MaxKonstVkMasaNeKocnaPrikolka As Integer
    Public ReadOnly Property MaxKonstVkMasaNeKocnaPrikolka() As Integer
        Get
            Return _MaxKonstVkMasaNeKocnaPrikolka
        End Get
    End Property
    Private _MaxKonstOptovaruvanjeVoPriklucok As Integer
    Public ReadOnly Property MaxKonstOptovaruvanjeVoPriklucok() As Integer
        Get
            Return _MaxKonstOptovaruvanjeVoPriklucok
        End Get
    End Property

    Private _OznakaNaOdobrenieZaPriklucUred As String
    Public ReadOnly Property OznakaNaOdobrenieZaPriklucUred() As String
        Get
            Return _OznakaNaOdobrenieZaPriklucUred
        End Get
    End Property

    Public ReadOnly Property Tires() As String
        Get
            Dim vehicleTires As VehicleTireList = VehicleTireList.GetVehicleTireTypeListByIdVehicle(_idVehicle)
            Dim tiresString As String = ""
            If vehicleTires.Count > 0 Then
                For i As Integer = 0 To vehicleTires.Count - 1
                    tiresString += vehicleTires.Item(0).TireType + "; "
                Next
            End If
            Return tiresString
        End Get
    End Property

    Private _HologationSertificateNumber As String
    Public ReadOnly Property HologationSertificateNumber() As String
        Get
            Return _HologationSertificateNumber
        End Get
    End Property

    Private _TMinMasa As Integer
    Public ReadOnly Property TMinMasa() As Integer
        Get
            Return _TMinMasa
        End Get
    End Property
    Private _NumberOfWheels As Integer
    Public ReadOnly Property NumberOfWheels() As Integer
        Get
            Return NumberOfWheels
        End Get
    End Property
    Public ReadOnly Property MasaMinMax() As String
        Get
            Return _maximunAllowedWaight + "   " + _TMinMasa
        End Get
    End Property
    Public ReadOnly Property NumAxisWeels() As String
        Get
            Return _numberOfAxis + "   " + _NumberOfWheels
        End Get
    End Property
    Private _TMaxKonstVkMasaPrikolka As Integer
    Public ReadOnly Property TMaxKonstVkMasaPrikolka() As Integer
        Get
            Return _TMaxKonstVkMasaPrikolka
        End Get
    End Property

    Private _TMaxKonstVkMasaPoluprikolka As Integer
    Public ReadOnly Property TMaxKonstVkMasaPoluprikolka() As Integer
        Get
            Return _TMaxKonstVkMasaPoluprikolka
        End Get
    End Property

    Private _TMaxKonstVkMasaPrikolkaSoCenOska As Integer
    Public ReadOnly Property TMaxKonstVkMasaPrikolkaSoCenOska() As Integer
        Get
            Return _TMaxKonstVkMasaPrikolkaSoCenOska
        End Get
    End Property
    Private _TMaxKonstVkMasaNaKombinacija As Integer
    Public ReadOnly Property TMaxKonstVkMasaNaKombinacija() As Integer
        Get
            Return _TMaxKonstVkMasaNaKombinacija
        End Get
    End Property
    Private _TMaxKonstVkMasaPrikolkaStoMozePrikluci As Integer
    Public ReadOnly Property TMaxKonstVkMasaPrikolkaStoMozePrikluci() As Integer
        Get
            Return _TMaxKonstVkMasaPrikolkaStoMozePrikluci
        End Get
    End Property

    Private _IdentifikacijaNaMotorMestoMetod As String
    Public ReadOnly Property IdentifikacijaNaMotorMestoMetod() As String
        Get
            Return _IdentifikacijaNaMotorMestoMetod
        End Get
    End Property

    Private _NoiseTechnicalSpec As String
    Public ReadOnly Property NoiseTechnicalSpec() As String
        Get
            Return _NoiseTechnicalSpec
        End Get
    End Property


    Private _TBrOdobrenieMehanPriklucok As String
    Public ReadOnly Property TBrOdobrenieMehanPriklucok() As String
        Get
            Return _TBrOdobrenieMehanPriklucok
        End Get
    End Property
    Private _TMarkaMehanPriklucok As String
    Public ReadOnly Property TMarkaMehanPriklucok() As String
        Get
            Return _TMarkaMehanPriklucok
        End Get
    End Property
    Private _TTipMehanPriklucok As String
    Public ReadOnly Property TTipMehanPriklucok() As String
        Get
            Return _TTipMehanPriklucok
        End Get
    End Property
    Private _TZastitnaRamka As String
    Public ReadOnly Property TZastitnaRamka() As String
        Get
            Return _TZastitnaRamka
        End Get
    End Property
    Private _TZastitnaKabina As String
    Public ReadOnly Property TZastitnaKabina() As String
        Get
            Return _TZastitnaKabina
        End Get
    End Property

    Private _TMaxHorVerOptovaruvanjePriklucok As Integer
    Public ReadOnly Property TMaxHorVerOptovaruvanjePriklucok() As Integer
        Get
            Return _TMaxHorVerOptovaruvanjePriklucok
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
        _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
        _idCustomer = dr.GetInt64("IdCustomer")
        _idVehicle = dr.GetInt64("IdVehicle")
        _idTehnicalExamOrganizationsIssuedBy = dr.GetInt32("IdTehnicalExamOrganizationsIssuedBy")
        _issuerName = dr.GetString("IssuerName")
        _trafficLicenceNumber = dr.GetString("TrafficLicenceNumber")
        _madeDate = dr.GetDateTime("MadeDate")
        _endDate = dr.GetDateTime("EndDate")
        _note = dr.GetString("Note")
        _mb = dr.GetString("MB")
        _customerSurname = dr.GetString("CustomerSurname")
        _customerFirstName = dr.GetString("CustomerFirstName")
        _idLivingAddress = dr.GetInt32("IdLivingAddress")
        _streetName = dr.GetString("StreetName")
        _livingAddressNumber = dr.GetString("LivingAddressNumber")
        _idVehicleCategoryForPayments = dr.GetInt32("IdVehicleCategoryForPayments")
        _vehicleCategoryForPaymentName = dr.GetString("VehicleCategoryForPaymentName")
        _idVehicleModel = dr.GetInt32("IdVehicleModel")
        _modelName = dr.GetString("ModelName")
        _idVehicleMaker = dr.GetInt32("IdVehicleMaker")
        _vehicleMaker = dr.GetString("VehicleMaker")
        _shellNumber = dr.GetString("ShellNumber")
        _engineNumber = dr.GetString("EngineNumber")
        _makeDate = dr.GetDateTime("MakeDate")
        _enginePower = dr.GetValue("EnginePower")
        _engineTorque = dr.GetString("EngineTorque")
        _engineWorkingCapacity = dr.GetValue("EngineWorkingCapacity")
        _enginePowerOutPut = dr.GetValue("EnginePowerOutPut")
        _vehcilceCategoryForPaymentCode = dr.GetString("VehcilceCategoryForPaymentCode")
        _emptyWaight = dr.GetValue("EmptyWaight")
        _maximunAllowedWaight = dr.GetValue("MaximunAllowedWaight")
        _numberOfSeats = dr.GetInt16("NumberOfSeats")
        _numberOfStandingSeats = dr.GetInt16("NumberOfStandingSeats")
        _numberOfLieingSeats = dr.GetInt16("NumberOfLieingSeats")
        _bodytypeDescriprion = dr.GetString("BodytypeDescriprion")
        _oldBodytypeDescription = dr.GetString("OldBodytypeDescription")
        _bodytypeCode = dr.GetString("BodytypeCode")
        _idVehicleBodyType = dr.GetInt32("IdVehicleBodyType")
        _idVehicleUse = dr.GetInt32("IdVehicleUse")
        _useDescription = dr.GetString("UseDescription")
        _idPrimaryColor = dr.GetInt32("IdPrimaryColor")
        _TNG = dr.GetBoolean("TNG")
        _colorCode = dr.GetString("ColorCode")
        _colorDescription = dr.GetString("ColorDescription")
        _IdEnginePowerSource = dr.GetInt32("IdEnginePowerSource")
        _IdEngineSecondPowerSource = dr.GetInt32("IdEngineSecondPowerSource")
        _idSecondaryColor = dr.GetInt32("IdSecondaryColor")
        _secondaryColorCode = dr.GetString("SecondaryColorCode")
        _secondaryColorDescription = dr.GetString("SecondaryColorDescription")
        _numberOfDoors = dr.GetInt32("NumberOfDoors")
        _numberOfAxis = dr.GetInt32("NumberOfAxis")
        _idVehicleCategory = dr.GetInt32("IdVehicleCategories")
        _engineType = dr.GetString("EngineTypeCode")
        _categoryName = dr.GetString("CategoryName")
        'Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_idVehicle)
        _lastRegistrationNumber = dr.GetString("LastRegistratinNumber")
        _dateOfLastRegistration = dr.GetDateTime("LastRegistrationMakeDate")
        '_lastRegistrationPlace = dr.GetString("LastRegistrationCommunity")
        'Dim firstRegInfo As registrationBasicInfo = Vehicle.GetVehicleFirstRegistration(_idVehicle)
        ' If _firstRegistrationNumber <> "непозната" AndAlso _firstRegistrationNumber <> String.Empty Then
        _firstRegistrationNumber = dr.GetString("FirstRegistrationNumber")
        _firstRegistrationDate = dr.GetDateTime("FirstRegistrationMakeDate")
        ' _firstRegistrationPlace = dr.GetString("FirstRegistrationCommunity")
        _VehicleModelAdding = dr.GetString("VehicleModelAdding")
        _IsCompany = dr.GetBoolean("IsCompany")
        _IdFirstRegistrationIssuer = dr.GetInt32("IdFirstRegistrationIssuer")
        _IdLastRegistrationIssuer = dr.GetInt32("IdLastRegistrationIssuer")
        _LastRegIssuerName = dr.GetString("LastRegIssuerName")
        _firstRegIdCommunity = dr.GetInt32("firstRegIdCommunity")
        _lastRegIdCommunity = dr.GetInt32("lastRegIdCommunity")

        '  End If
        Dim customer As CustomersInfo = CustomersList.GetCustomersList.GetCustomersListById(_idCustomer)
        _communityCountry = customer.CommunityName '& " " & customer.CountryName
        _customerCity = UCase(ToLat(customer.CityName))
        _MaxSpeed = dr.GetValue("MaxSpeed")
        _VehicleSizeHight = dr.GetValue("VehicleSizeHight")
        _VehicleSizeLength = dr.GetValue("VehicleSizeLength")
        _VehicleSizeWidth = dr.GetValue("VehicleSizeWidth")
        _NoiseStatic = dr.GetValue("NoiseStatic")
        _CO2 = dr.GetValue("CO2")
        _Tip = dr.GetString("Tip")
        _VarijantaIzvedba = dr.GetString("VarijantaIzvedba")
        _OznakaNaOdobrenie = dr.GetString("OznakaNaOdobrenie")
        _OdnosKwCcm = dr.GetString("OdnosKwCcm")
        _MaxLegVkMasa = dr.GetValue("MaxLegVkMasa")
        _MaxLegVkMasaGrupa = dr.GetValue("MaxLegVkMasaGrupa")
        _MasaPoOska1 = dr.GetInt32("MasaPoOska1")
        _MasaPoOska2 = dr.GetInt32("MasaPoOska2")
        _MasaPoOska3 = dr.GetInt32("MasaPoOska3")
        _MasaPoOska4 = dr.GetInt32("MasaPoOska4")
        _MasaPoOska5 = dr.GetInt32("MasaPoOska5")
        _MasaPoOskaPriklucna = dr.GetInt32("MasaPoOskaPriklucna")
        _OsnoOptovaruvanje1 = dr.GetInt32("OsnoOptovaruvanje1")
        _OsnoOptovaruvanje2 = dr.GetInt32("OsnoOptovaruvanje2")
        _OsnoOptovaruvanje3 = dr.GetInt32("OsnoOptovaruvanje3")
        _OsnoOptovaruvanje4 = dr.GetInt32("OsnoOptovaruvanje4")
        _OsnoOptovaruvanje5 = dr.GetInt32("OsnoOptovaruvanje5")
        _OsnoOptovaruvanjePriklucna = dr.GetInt32("OsnoOptovaruvanjePriklucna")
        _MaxKonstVkMasaKocnaPrikolka = dr.GetInt32("MaxKonstVkMasaKocnaPrikolka")
        _MaxKonstVkMasaNeKocnaPrikolka = dr.GetInt32("MaxKonstVkMasaNeKocnaPrikolka")
        _OznakaNaOdobrenieZaPriklucUred = dr.GetString("OznakaNaOdobrenieZaPriklucUred")
        _MaxKonstOptovaruvanjeVoPriklucok = dr.GetInt32("MaxKonstOptovaruvanjeVoPriklucok")
        _HologationSertificateNumber = dr.GetString("HologationSertificateNumber")
        _TMinMasa = dr.GetInt32("TMinMasa")
        _NumberOfWheels = dr.GetInt32("NumberOfWheels")
        _TMaxKonstVkMasaPrikolka = dr.GetInt32("TMaxKonstVkMasaPrikolka")
        _TMaxKonstVkMasaPoluprikolka = dr.GetInt32("TMaxKonstVkMasaPoluprikolka")
        _TMaxKonstVkMasaPrikolkaSoCenOska = dr.GetInt32("TMaxKonstVkMasaPrikolkaSoCenOska")
        _TMaxKonstVkMasaNaKombinacija = dr.GetInt32("TMaxKonstVkMasaNaKombinacija")
        _TMaxKonstVkMasaPrikolkaStoMozePrikluci = dr.GetInt32("TMaxKonstVkMasaPrikolkaStoMozePrikluci")
        _IdentifikacijaNaMotorMestoMetod = dr.GetString("IdentifikacijaNaMotorMestoMetod")
        _NoiseTechnicalSpec = dr.GetString("NoiseTechnicalSpec")
        _TZastitnaKabina = dr.GetString("TZastitnaKabina")
        _TZastitnaRamka = dr.GetString("TZastitnaRamka")
        _TTipMehanPriklucok = dr.GetString("TTipMehanPriklucok")
        _TMarkaMehanPriklucok = dr.GetString("TMarkaMehanPriklucok")
        _TBrOdobrenieMehanPriklucok = dr.GetString("TBrOdobrenieMehanPriklucok")
        _TMaxHorVerOptovaruvanjePriklucok = dr.GetInt32("TMaxHorVerOptovaruvanjePriklucok")
    End Sub

End Class