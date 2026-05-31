
<Serializable()> _
Public Class Vehicle
  Inherits Csla.BusinessBase(Of Vehicle)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleByID"
  Private Const spGetAll As String = "GetVehicles"
  Private Const spUpdate As String = "updateVehicle"
  Private Const spAdd As String = "addVehicle"
  Private Const spDelete As String = "deleteVehicle"
  Private Const spGetChildAxes As String = "getVehicleAxiByIdVehicle"
  Private Const spGetByShellNumber As String = "getVehicleByShellNum"
  Private Const spGetChildAxesDestination As String = "getVehicleBetweenAxesDestinationByIdVehicle"
  Private Const spGetChildFirstRegistration As String = "getVehicleFirstRegistrationByIdVehicle"
  Private Const spGetChildrenTires As String = "getVehicleTireTypeByIdVehicleModel"
  Private Const spGetChildrenPersonalTires As String = "getVehicleTyreByIdVehicle"
  Private Const spGetChildLastTehnicalExam As String = "getDocumentsActiveTehnicalExamsReportByIdVehicle"
  Private Const spGetChildRegistrations As String = "GetVehicleRegistrationByIdVehicle"
#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Vehicle), New PropertyInfo(Of Long)("Id"))
  Private Shared IdVehicleBodyTypeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdVehicleBodyType", "Каросерија", 0))
  Private Shared IdVehicleCategoriesProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdVehicleCategories"))
  Private Shared IdVehicleUseProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdVehicleUse"))
  Private Shared IdEngineTypeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdEngineType"))
  Private Shared IdEnginePowerSourceProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdEnginePowerSource", "IdEnginePowerSource", 0))
  Private Shared IdEngineSecondPowerSourceProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdEngineSecondPowerSource", "IdEngineSecondPowerSource", 0))
  Private Shared IdGearBoxProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdGearBox"))
  Private Shared IdBreakesProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdBreakes"))
  Private Shared IdSupportingProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdSupporting"))
  Private Shared IdVehicleModelProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdVehicleModel"))
  Private Shared IdVehicleCategoryForPaymentsProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdVehicleCategoryForPayments"))
  Private Shared IdEngineEcoProgramProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdEngineEcoProgram"))
  Private Shared IdMadeCountryProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdMadeCountry"))
  Private Shared IdPrimaryColorProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdPrimaryColor"))
  Private Shared IdSecondaryColorProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdSecondaryColor"))

  Private Shared ColorCodeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("ColorCode"))

  Private Shared FirstRegistrationNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("FirstRegistrationNumber"))
  Private Shared LastRegistratinNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("LastRegistratinNumber"))
  ' Private Shared FirstRegistrationCommunityProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("FirstRegistrationCommunity"))
  ' Private Shared LastRegistrationCommunityProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("LastRegistrationCommunity"))
  Private Shared FirstRegistrationMakeDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(Vehicle), New PropertyInfo(Of SmartDate)("FirstRegistrationMakeDate", "FirstRegistrationMakeDate", New SmartDate(DateTime.Today, True)))
  Private Shared FirstRegistrationValidTillProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(Vehicle), New PropertyInfo(Of SmartDate)("FirstRegistrationValidTill", "FirstRegistrationValidTill", New SmartDate(DateTime.Today, True)))
  Private Shared LastRegistrationMakeDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(Vehicle), New PropertyInfo(Of SmartDate)("LastRegistrationMakeDate", "LastRegistrationMakeDate", New SmartDate(DateTime.Today, True)))
  Private Shared LastRegistrationValidTillProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(Vehicle), New PropertyInfo(Of SmartDate)("LastRegistrationValidTill", "LastRegistrationValidTill", New SmartDate(DateTime.Today, True)))
  Private Shared FirstIdRegistrationIssuerProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdFirstRegistrationIssuer", "IdFirstRegistrationIssuer", 1))
  Private Shared LastIdRegistrationIssuerProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("IdLastRegistrationIssuer", "IdLastRegistrationIssuer", 1))

  Private Shared EngineNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("EngineNumber"))
  Private Shared EnginePowerProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("EnginePower"))
  Private Shared EngineTorqueProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("EngineTorque"))
  Private Shared EngineTorqueUnderGassProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("EngineTorqueUnderGass"))
  Private Shared EngineWorkingCapacityProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("EngineWorkingCapacity"))
  Private Shared EnginePowerOutPutProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("EnginePowerOutPut"))
  Private Shared ShellNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("ShellNumber"))
  Private Shared MakeDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(Vehicle), New PropertyInfo(Of SmartDate)("MakeDate", "MakeDate", New SmartDate(DateTime.Today, True)))
  Private Shared NumberOfDoorsProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("NumberOfDoors"))
  Private Shared NumberOfSeatsProperty As PropertyInfo(Of Short) = RegisterProperty(Of Short)(GetType(Vehicle), New PropertyInfo(Of Short)("NumberOfSeats"))
  Private Shared NumberOfStandingSeatsProperty As PropertyInfo(Of Short) = RegisterProperty(Of Short)(GetType(Vehicle), New PropertyInfo(Of Short)("NumberOfStandingSeats"))
  Private Shared NumberOfLieingSeatsProperty As PropertyInfo(Of Short) = RegisterProperty(Of Short)(GetType(Vehicle), New PropertyInfo(Of Short)("NumberOfLieingSeats"))
  Private Shared EmptyWaightProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("EmptyWaight"))
  Private Shared MaximunAllowedWaightProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("MaximunAllowedWaight"))
  Private Shared TrailerWaightWithBreakProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("TrailerWaightWithBreak"))
  Private Shared TrailerWaightWithoutBreakProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("TrailerWaightWithoutBreak"))
  Private Shared NumberOfAxisProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("NumberOfAxis"))
  Private Shared PropulsionAxisProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("PropulsionAxis"))
  Private Shared NumberOfWheelsProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("NumberOfWheels"))
  Private Shared NumberOfPropulsionWheelsProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("NumberOfPropulsionWheels"))
  Private Shared VehicleSizeHightProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("VehicleSizeHight"))
  Private Shared VehicleSizeWidthProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("VehicleSizeWidth"))
  Private Shared VehicleSizeLengthProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("VehicleSizeLength"))
  Private Shared SuffocationProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(Vehicle), New PropertyInfo(Of Boolean)("Suffocation"))
  Private Shared HookProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(Vehicle), New PropertyInfo(Of Boolean)("Hook"))
  Private Shared VitloProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(Vehicle), New PropertyInfo(Of Boolean)("Vitlo"))
  Private Shared VerticalBurdenOnTheSeatProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(Vehicle), New PropertyInfo(Of Boolean)("VerticalBurdenOnTheSeat"))
  Private Shared VerticalBurdenOnTheSeatNoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("VerticalBurdenOnTheSeatNote"))
  Private Shared HologationSertificateNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("HologationSertificateNumber"))
  Private Shared NoiseStaticProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("NoiseStatic"))
  Private Shared NoiseMovmentProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("NoiseMovment"))
  Private Shared CoProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("Co"))
  Private Shared HcProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("Hc"))
  Private Shared NOxProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("NOx"))
  Private Shared HCNOxProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("HCNOx"))
  Private Shared BlackeningProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("Blackening"))
  Private Shared PinpointsProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("Pinpoints"))
  Private Shared Co2Property As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("Co2"))
  Private Shared FuelConsumptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("FuelConsumption"))
  Private Shared CapacityFuelTankProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("CapacityFuelTank"))
  Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("Note"))
  Private Shared IsSocialNotPrivateProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(Vehicle), New PropertyInfo(Of Boolean)("IsSocialNotPrivate"))
  Private Shared ForPrivateTransportNotPublicProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(Vehicle), New PropertyInfo(Of Boolean)("ForPrivateTransportNotPublic"))
  Private Shared TNGProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(Vehicle), New PropertyInfo(Of Boolean)("TNG"))
  Private Shared VehicleModelAddingProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("VehicleModelAdding"))
  Private Shared MaxSpeedProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("MaxSpeed"))
  Private Shared TempOfEngineOilProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("TempOfEngineOil"))
    Private Shared IdentifikacijaNaMotorMestoMetodProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("IdentifikacijaNaMotorMestoMetod"))
    Private Shared MasaPoOska1Property As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("MasaPoOska1"))
    Private Shared MasaPoOska2Property As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("MasaPoOska2"))
    Private Shared MasaPoOska3Property As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("MasaPoOska3"))
    Private Shared MasaPoOska4Property As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("MasaPoOska4"))
    Private Shared MasaPoOska5Property As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("MasaPoOska5"))
    Private Shared MasaPoOskaPriklucnaProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("MasaPoOskaPriklucna"))
    Private Shared MaxKonstOptovaruvanjeVoPriklucokProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("MaxKonstOptovaruvanjeVoPriklucok"))
    Private Shared MaxKonstVkMasaKocnaPrikolkaProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("MaxKonstVkMasaKocnaPrikolka"))
    Private Shared MaxKonstVkMasaNeKocnaPrikolkaProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("MaxKonstVkMasaNeKocnaPrikolka"))
    Private Shared MaxLegVkMasaProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("MaxLegVkMasa"))
    Private Shared MaxLegVkMasaGrupaProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("MaxLegVkMasaGrupa"))
    Private Shared NoiseTechnicalSpecProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("NoiseTechnicalSpec"))
    Private Shared OdnosKwCcmProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("OdnosKwCcm"))
    Private Shared OsnoOptovaruvanje1Property As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("OsnoOptovaruvanje1"))
    Private Shared OsnoOptovaruvanje2Property As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("OsnoOptovaruvanje2"))
    Private Shared OsnoOptovaruvanje3Property As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("OsnoOptovaruvanje3"))
    Private Shared OsnoOptovaruvanje4Property As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("OsnoOptovaruvanje4"))
    Private Shared OsnoOptovaruvanje5Property As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("OsnoOptovaruvanje5"))
    Private Shared OsnoOptovaruvanjePriklucnaProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("OsnoOptovaruvanjePriklucna"))
    Private Shared OznakaNaOdobrenieProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("OznakaNaOdobrenie"))
    Private Shared OznakaNaOdobrenieZaPriklucUredProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("OznakaNaOdobrenieZaPriklucUred"))
    Private Shared TBrOdobrenieMehanPriklucokProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("TBrOdobrenieMehanPriklucok"))
    Private Shared TipProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("Tip"))
    Private Shared TMarkaMehanPriklucokProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("TMarkaMehanPriklucok"))
    Private Shared TMaxHorVerOptovaruvanjePriklucokProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("TMaxHorVerOptovaruvanjePriklucok"))
    Private Shared TMaxKonstVkMasaNaKombinacijaProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("TMaxKonstVkMasaNaKombinacija"))
    Private Shared TMaxKonstVkMasaPoluprikolkaProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("TMaxKonstVkMasaPoluprikolka"))
    Private Shared TMaxKonstVkMasaPrikolkaProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("TMaxKonstVkMasaPrikolka"))
    Private Shared TMaxKonstVkMasaPrikolkaSoCenOskaProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("TMaxKonstVkMasaPrikolkaSoCenOska"))
    Private Shared TMaxKonstVkMasaPrikolkaStoMozePrikluciProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("TMaxKonstVkMasaPrikolkaStoMozePrikluci"))
    Private Shared TMinMasaProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("TMinMasa"))
    Private Shared TTipMehanPriklucokProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("TTipMehanPriklucok"))
    Private Shared TZastitnaKabinaProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("TZastitnaKabina"))
    Private Shared TZastitnaRamkaProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("TZastitnaRamka"))
    Private Shared VarijantaIzvedbaProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("VarijantaIzvedba"))
    Private Shared BrojEUPotvrdaProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Vehicle), New PropertyInfo(Of String)("BrojEUPotvrda"))
    Private Shared MaxKonstVkMasaProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(Vehicle), New PropertyInfo(Of Decimal)("MaxKonstVkMasa"))
    Private Shared BrojNaVrteziProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Vehicle), New PropertyInfo(Of Integer)("BrojNaVrtezi"))

  Private _lastChanged(7) As Byte
  Private _requiredField As VehicleFieldList = VehicleFieldList.EmptyList

  Private Shared VehicleAxesProperty As PropertyInfo(Of VehicleAxes) = _
 RegisterProperty(Of VehicleAxes)(GetType(Vehicle), New PropertyInfo(Of VehicleAxes)("VehicleAxes"))

  Private Shared VehiclePersonalTiresProperty As PropertyInfo(Of VehicleTyres) = _
 RegisterProperty(Of VehicleTyres)(GetType(Vehicle), New PropertyInfo(Of VehicleTyres)("VehiclePersonalTires"))

  '  Private Shared VehicleLastTehnicalExamsProperty As PropertyInfo(Of VehicleLastTehnicalExams) = _
  'RegisterProperty(Of VehicleLastTehnicalExams)(GetType(Vehicle), New PropertyInfo(Of VehicleLastTehnicalExams)("VehicleLastTehnicalExams"))

  '  Private Shared VehicleTiresProperty As PropertyInfo(Of TireTypes) = _
  'RegisterProperty(Of TireTypes)(GetType(Vehicle), New PropertyInfo(Of TireTypes)("VehicleTires"))

  '  Private Shared VehicleRegistrationsProperty As PropertyInfo(Of VehicleRegistrations) = _
  'RegisterProperty(Of VehicleRegistrations)(GetType(Vehicle), New PropertyInfo(Of VehicleRegistrations)("Registrations"))

  Private Shared VehicleAxesDestinationProperty As PropertyInfo(Of VehicleBetweenAxesDestinations) = _
 RegisterProperty(Of VehicleBetweenAxesDestinations)(GetType(Vehicle), New PropertyInfo(Of VehicleBetweenAxesDestinations)("VehicleAxesDestination"))


  'Public ReadOnly Property FirstRegistation() As String
  '    Get
  '        Return Registrations.GetFirstRegistration()
  '    End Get
  'End Property

  'Public ReadOnly Property LastRegistation() As String
  '    Get
  '        Return Registrations.GetLastRegistration()
  '    End Get
  'End Property

  Public ReadOnly Property VehicleTrafficLicenceNumber() As String
    Get
      Return TrafficLicenceNumber.GetTrafficLicenceNumber(Me.Id)
    End Get
  End Property


  'Public ReadOnly Property Registrations() As VehicleRegistrations
  '    Get
  '        If Not FieldManager.FieldExists(VehicleRegistrationsProperty) Then
  '            SetProperty(Of VehicleRegistrations) _
  '            (VehicleRegistrationsProperty, VehicleRegistrations.NewVehicleRegistrations)
  '        End If
  '        Return GetProperty(Of VehicleRegistrations)(VehicleRegistrationsProperty)
  '    End Get
  'End Property

  'Public ReadOnly Property VehicleLastTehnicalExams() As VehicleLastTehnicalExams
  '    Get
  '        If Not FieldManager.FieldExists(VehicleLastTehnicalExamsProperty) Then
  '            SetProperty(Of VehicleLastTehnicalExams) _
  '            (VehicleLastTehnicalExamsProperty, VehicleLastTehnicalExams.NewVehicleLastTehnicalExams)
  '        End If
  '        Return GetProperty(Of VehicleLastTehnicalExams)(VehicleLastTehnicalExamsProperty)
  '    End Get
  'End Property
  'Public ReadOnly Property VehicleTires() As TireTypes
  '    Get
  '        If Not FieldManager.FieldExists(VehicleTiresProperty) Then
  '            SetProperty(Of TireTypes) _
  '            (VehicleTiresProperty, TireTypes.NewTireTypes)
  '        End If
  '        Return GetProperty(Of TireTypes)(VehicleTiresProperty)
  '    End Get
  'End Property
  Public ReadOnly Property VehiclePersonalTires() As VehicleTyres
    Get
      If Not FieldManager.FieldExists(VehiclePersonalTiresProperty) Then
        SetProperty(Of VehicleTyres) _
        (VehiclePersonalTiresProperty, VehicleTyres.NewVehicleTyres)
      End If
      Return GetProperty(Of VehicleTyres)(VehiclePersonalTiresProperty)
    End Get
  End Property

  Public ReadOnly Property VehicleAxesDestinations() As VehicleBetweenAxesDestinations
    Get
      If Not FieldManager.FieldExists(VehicleAxesDestinationProperty) Then
        SetProperty(Of VehicleBetweenAxesDestinations) _
        (VehicleAxesDestinationProperty, VehicleBetweenAxesDestinations.NewVehicleBetweenAxesDestinations)
      End If
      Return GetProperty(Of VehicleBetweenAxesDestinations)(VehicleAxesDestinationProperty)
    End Get
  End Property

  Public ReadOnly Property VehicleAxes() As VehicleAxes
    Get
      If Not FieldManager.FieldExists(VehicleAxesProperty) Then
        SetProperty(Of VehicleAxes) _
        (VehicleAxesProperty, VehicleAxes.NewVehicleAxes)
      End If
      Return GetProperty(Of VehicleAxes)(VehicleAxesProperty)
    End Get
  End Property

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Long
    Get
      Return GetProperty(Of Long)(IdProperty)
    End Get
  End Property
  Public Property IdVehicleBodyType() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleBodyTypeProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleBodyTypeProperty, value)
    End Set
  End Property
  Public Property IdVehicleCategories() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleCategoriesProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleCategoriesProperty, value)
    End Set
  End Property
  Public Property IdVehicleUse() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleUseProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleUseProperty, value)
    End Set
  End Property
  Public Property IdEngineType() As Integer
    Get
      Return GetProperty(Of Integer)(IdEngineTypeProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdEngineTypeProperty, value)
    End Set
  End Property
  Public Property IdEnginePowerSource() As Integer
    Get
      Return GetProperty(Of Integer)(IdEnginePowerSourceProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdEnginePowerSourceProperty, value)
    End Set
  End Property
  Public Property IdEngineSecondPowerSource() As Integer
    Get
      Return GetProperty(Of Integer)(IdEngineSecondPowerSourceProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdEngineSecondPowerSourceProperty, value)
    End Set
  End Property

  Public Property IdVehicleCategoryForPayments() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty, value)
    End Set
  End Property

  'Public Property IdEngineMark() As Integer
  '  Get
  '    Return GetProperty(Of Integer)(IdEngineMarkProperty)
  '  End Get
  '  Set(ByVal value As Integer)
  '    SetProperty(Of Integer)(IdEngineMarkProperty, value)
  '  End Set
  'End Property
  Public Property IdGearBox() As Integer
    Get
      Return GetProperty(Of Integer)(IdGearBoxProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdGearBoxProperty, value)
    End Set
  End Property
  Public Property IdBreakes() As Integer
    Get
      Return GetProperty(Of Integer)(IdBreakesProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdBreakesProperty, value)
    End Set
  End Property
  Public Property IdSupporting() As Integer
    Get
      Return GetProperty(Of Integer)(IdSupportingProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdSupportingProperty, value)
    End Set
  End Property
  Public Property IdVehicleModel() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleModelProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleModelProperty, value)
    End Set
  End Property

  Public Property IdEngineEcoProgram() As Integer
    Get
      Return GetProperty(Of Integer)(IdEngineEcoProgramProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdEngineEcoProgramProperty, value)
    End Set
  End Property

  Public Property IdMadeCountry() As Integer
    Get
      Return GetProperty(Of Integer)(IdMadeCountryProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdMadeCountryProperty, value)
    End Set
  End Property

  Public Property ColorCode() As String
    Get
      Return GetProperty(Of String)(ColorCodeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(ColorCodeProperty, value)
    End Set
  End Property

  Public Property LastRegistratinNumber() As String
    Get
      'Dim pom As String = CommunitiesList.GetCommunitiesList.GetCommunitiesListById(Options.GetOptions.IdCommunity).RegistrationCode
      'If GetProperty(Of String)(LastRegistratinNumberProperty) = pom & "-000-AA" Then
      '    Return pom & "-"
      'Else
      Return GetProperty(Of String)(LastRegistratinNumberProperty)
      'End If
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(LastRegistratinNumberProperty, UCase(value))
    End Set
  End Property

  Public Property FirstRegistrationNumber() As String
    Get
      Return GetProperty(Of String)(FirstRegistrationNumberProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(FirstRegistrationNumberProperty, UCase(value))
    End Set
  End Property

  'Public Property LastRegistrationCommunity() As String
  '    Get
  '        Return GetProperty(Of String)(LastRegistrationCommunityProperty)
  '    End Get
  '    Set(ByVal value As String)
  '        SetProperty(Of String)(LastRegistrationCommunityProperty, value)
  '    End Set
  'End Property

  Public Property FirstIdRegistrationIssuer() As Integer
    Get
      Return GetProperty(Of Integer)(FirstIdRegistrationIssuerProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(FirstIdRegistrationIssuerProperty, value)
    End Set
  End Property

  Public Property LastIdRegistrationIssuer() As Integer
    Get
      Return GetProperty(Of Integer)(LastIdRegistrationIssuerProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(LastIdRegistrationIssuerProperty, value)
    End Set
  End Property

  'Public Property FirstRegistrationCommunity() As String
  '    Get
  '        Return GetProperty(Of String)(FirstRegistrationCommunityProperty)
  '    End Get
  '    Set(ByVal value As String)
  '        SetProperty(Of String)(FirstRegistrationCommunityProperty, value)
  '    End Set
  'End Property

  Public Property FirstRegistrationMakeDate() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(FirstRegistrationMakeDateProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(FirstRegistrationMakeDateProperty, value)
    End Set
  End Property

  Public Property FirstRegistrationValidTill() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(FirstRegistrationValidTillProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(FirstRegistrationValidTillProperty, value)
    End Set
  End Property

  Public Property LastRegistrationMakeDate() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(LastRegistrationMakeDateProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(LastRegistrationMakeDateProperty, value)
    End Set
  End Property

  Public Property LastRegistrationValidTill() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(LastRegistrationValidTillProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(LastRegistrationValidTillProperty, value)
    End Set
  End Property

  Public Property VehicleModelAdding() As String
    Get
      Return GetProperty(Of String)(VehicleModelAddingProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(VehicleModelAddingProperty, value)
    End Set
  End Property
  Public Property EngineNumber() As String
    Get
      Return GetProperty(Of String)(EngineNumberProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(EngineNumberProperty, value)
    End Set
  End Property
  Public Property EnginePower() As Decimal
    Get
      Return GetProperty(Of Decimal)(EnginePowerProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(EnginePowerProperty, value)
    End Set
  End Property
  Public Property EngineTorque() As String
    Get
      Return GetProperty(Of String)(EngineTorqueProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(EngineTorqueProperty, value)
    End Set
  End Property
  Public Property EngineTorqueUnderGass() As Decimal
    Get
      Return GetProperty(Of Decimal)(EngineTorqueUnderGassProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(EngineTorqueUnderGassProperty, value)
    End Set
  End Property

  Public Property EngineWorkingCapacity() As Decimal
    Get
      Return GetProperty(Of Decimal)(EngineWorkingCapacityProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(EngineWorkingCapacityProperty, value)
    End Set
  End Property
  Public Property VerticalBurdenOnTheSeat() As Boolean
    Get
      Return GetProperty(Of Boolean)(VerticalBurdenOnTheSeatProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(VerticalBurdenOnTheSeatProperty, value)
    End Set
  End Property
  Public Property VerticalBurdenOnTheSeatNote() As String
    Get
      Return GetProperty(Of String)(VerticalBurdenOnTheSeatNoteProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(VerticalBurdenOnTheSeatNoteProperty, value)
    End Set
  End Property
  Public Property EnginePowerOutPut() As Decimal
    Get
      Return GetProperty(Of Decimal)(EnginePowerOutPutProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(EnginePowerOutPutProperty, value)
    End Set
  End Property
  Public Property ShellNumber() As String
    Get
      Return GetProperty(Of String)(ShellNumberProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(ShellNumberProperty, value)
    End Set
  End Property

  Public Property MakeDate() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(MakeDateProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(MakeDateProperty, value)
    End Set
  End Property

  Public Property NumberOfDoors() As Integer
    Get
      Return GetProperty(Of Integer)(NumberOfDoorsProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(NumberOfDoorsProperty, value)
    End Set
  End Property
  Public Property NumberOfSeats() As Short
    Get
      Return GetProperty(Of Short)(NumberOfSeatsProperty)
    End Get
    Set(ByVal value As Short)
      SetProperty(Of Short)(NumberOfSeatsProperty, value)
    End Set
  End Property
  Public Property NumberOfStandingSeats() As Short
    Get
      Return GetProperty(Of Short)(NumberOfStandingSeatsProperty)
    End Get
    Set(ByVal value As Short)
      SetProperty(Of Short)(NumberOfStandingSeatsProperty, value)
    End Set
  End Property
  Public Property NumberOfLieingSeats() As Short
    Get
      Return GetProperty(Of Short)(NumberOfLieingSeatsProperty)
    End Get
    Set(ByVal value As Short)
      SetProperty(Of Short)(NumberOfLieingSeatsProperty, value)
    End Set
  End Property
  Public Property EmptyWaight() As Decimal
    Get
      Return GetProperty(Of Decimal)(EmptyWaightProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(EmptyWaightProperty, value)
    End Set
  End Property

  Public Property MaximunAllowedWaight() As Decimal 'e nosivost
    Get
      Return GetProperty(Of Decimal)(MaximunAllowedWaightProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(MaximunAllowedWaightProperty, value)
    End Set
  End Property
  'Public ReadOnly Property CarringCapacity() As decimal
  '  Get
  '    Return (MaximunAllowedWaight - EmptyWaight)
  '  End Get
  'End Property
  Public ReadOnly Property TotalWaight() As Decimal
    Get
      Return (MaximunAllowedWaight + EmptyWaight)
    End Get
  End Property
  Public Property TrailerWaightWithBreak() As String
    Get
      Return GetProperty(Of String)(TrailerWaightWithBreakProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(TrailerWaightWithBreakProperty, value)
    End Set
  End Property
  Public Property TrailerWaightWithoutBreak() As String
    Get
      Return GetProperty(Of String)(TrailerWaightWithoutBreakProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(TrailerWaightWithoutBreakProperty, value)
    End Set
  End Property
  Public Property NumberOfAxis() As Integer
    Get
      Return GetProperty(Of Integer)(NumberOfAxisProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(NumberOfAxisProperty, value)
    End Set
  End Property
  Public Property PropulsionAxis() As Integer
    Get
      Return GetProperty(Of Integer)(PropulsionAxisProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(PropulsionAxisProperty, value)
    End Set
  End Property
  Public Property NumberOfWheels() As Integer
    Get
      Return GetProperty(Of Integer)(NumberOfWheelsProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(NumberOfWheelsProperty, value)
    End Set
  End Property
  Public Property NumberOfPropulsionWheels() As Integer
    Get
      Return GetProperty(Of Integer)(NumberOfPropulsionWheelsProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(NumberOfPropulsionWheelsProperty, value)
    End Set
  End Property
  Public Property VehicleSizeHight() As Decimal
    Get
      Return GetProperty(Of Decimal)(VehicleSizeHightProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(VehicleSizeHightProperty, value)
    End Set
  End Property
  Public Property VehicleSizeWidth() As Decimal
    Get
      Return GetProperty(Of Decimal)(VehicleSizeWidthProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(VehicleSizeWidthProperty, value)
    End Set
  End Property
  Public Property VehicleSizeLength() As Decimal
    Get
      Return GetProperty(Of Decimal)(VehicleSizeLengthProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(VehicleSizeLengthProperty, value)
    End Set
  End Property
  Public Property Suffocation() As Boolean
    Get
      Return GetProperty(Of Boolean)(SuffocationProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(SuffocationProperty, value)
    End Set
  End Property
  Public Property Hook() As Boolean
    Get
      Return GetProperty(Of Boolean)(HookProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(HookProperty, value)
    End Set
  End Property
  Public Property Vitlo() As Boolean
    Get
      Return GetProperty(Of Boolean)(VitloProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(VitloProperty, value)
    End Set
  End Property
  Public Property IdPrimaryColor() As Integer
    Get
      Return GetProperty(Of Integer)(IdPrimaryColorProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdPrimaryColorProperty, value)
    End Set
  End Property
  Public Property IdSecondaryColor() As Integer
    Get
      Return GetProperty(Of Integer)(IdSecondaryColorProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdSecondaryColorProperty, value)
    End Set
  End Property
  Public Property HologationSertificateNumber() As String
    Get
      Return GetProperty(Of String)(HologationSertificateNumberProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(HologationSertificateNumberProperty, value)
    End Set
  End Property
  Public Property NoiseStatic() As Decimal
    Get
      Return GetProperty(Of Decimal)(NoiseStaticProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(NoiseStaticProperty, value)
    End Set
  End Property
  Public Property NoiseMovment() As Decimal
    Get
      Return GetProperty(Of Decimal)(NoiseMovmentProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(NoiseMovmentProperty, value)
    End Set
  End Property
  Public Property CO() As Decimal
    Get
      Return GetProperty(Of Decimal)(CoProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(CoProperty, value)
    End Set
  End Property
  Public Property HC() As Decimal
    Get
      Return GetProperty(Of Decimal)(HcProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(HcProperty, value)
    End Set
  End Property
  Public Property NOx() As Decimal
    Get
      Return GetProperty(Of Decimal)(NOxProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(NOxProperty, value)
    End Set
  End Property
  Public Property HCNOx() As Decimal
    Get
      Return GetProperty(Of Decimal)(HCNOxProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(HCNOxProperty, value)
    End Set
  End Property
  Public Property Blackening() As String
    Get
      Return GetProperty(Of String)(BlackeningProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(BlackeningProperty, value)
    End Set
  End Property
  Public Property Pinpoints() As String
    Get
      Return GetProperty(Of String)(PinpointsProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(PinpointsProperty, value)
    End Set
  End Property
  Public Property CO2() As Decimal
    Get
      Return GetProperty(Of Decimal)(Co2Property)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(Co2Property, value)
    End Set
  End Property
  Public Property FuelConsumption() As String
    Get
      Return GetProperty(Of String)(FuelConsumptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(FuelConsumptionProperty, value)
    End Set
  End Property
  Public Property CapacityFuelTank() As Decimal
    Get
      Return GetProperty(Of Decimal)(CapacityFuelTankProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(CapacityFuelTankProperty, value)
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

  Public Property IsSocialNotPrivate() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsSocialNotPrivateProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsSocialNotPrivateProperty, value)
    End Set
  End Property
  Public Property ForPrivateTransportNotPublic() As Boolean
    Get
      Return GetProperty(Of Boolean)(ForPrivateTransportNotPublicProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(ForPrivateTransportNotPublicProperty, value)
    End Set
  End Property

  Public Property TNG() As Boolean
    Get
      Return GetProperty(Of Boolean)(TNGProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(TNGProperty, value)
    End Set
  End Property

  Public Property MaxSpeed() As Decimal
    Get
      Return GetProperty(Of Decimal)(MaxSpeedProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(MaxSpeedProperty, value)
    End Set
  End Property

    Public Property TempOfEngineOil() As Decimal
        Get
            Return GetProperty(Of Decimal)(TempOfEngineOilProperty)
        End Get
        Set(ByVal value As Decimal)
            SetProperty(Of Decimal)(TempOfEngineOilProperty, value)
        End Set
    End Property
    Public Property MasaPoOska1() As Integer
        Get
            Return GetProperty(Of Integer)(MasaPoOska1Property)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MasaPoOska1Property, value)
        End Set
    End Property
    Public Property MasaPoOska2() As Integer
        Get
            Return GetProperty(Of Integer)(MasaPoOska2Property)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MasaPoOska2Property, value)
        End Set
    End Property
    Public Property MasaPoOska3() As Integer
        Get
            Return GetProperty(Of Integer)(MasaPoOska3Property)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MasaPoOska3Property, value)
        End Set
    End Property
    Public Property MasaPoOska4() As Integer
        Get
            Return GetProperty(Of Integer)(MasaPoOska4PRoperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MasaPoOska4PRoperty, value)
        End Set
    End Property
    Public Property MasaPoOska5() As Integer
        Get
            Return GetProperty(Of Integer)(MasaPoOska5PRoperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MasaPoOska5PRoperty, value)
        End Set
    End Property
    Public Property IdentifikacijaNaMotorMestoMetod() As String
        Get
            Return GetProperty(Of String)(IdentifikacijaNaMotorMestoMetodProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(IdentifikacijaNaMotorMestoMetodProperty, value)
        End Set
    End Property
    Public Property MasaPoOskaPriklucna() As Integer
        Get
            Return GetProperty(Of Integer)(MasaPoOskaPriklucnaProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MasaPoOskaPriklucnaProperty, value)
        End Set
    End Property
    Public Property MaxKonstOptovaruvanjeVoPriklucok() As Integer
        Get
            Return GetProperty(Of Integer)(MaxKonstOptovaruvanjeVoPriklucokProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MaxKonstOptovaruvanjeVoPriklucokProperty, value)
        End Set
    End Property
    Public Property MaxKonstVkMasaKocnaPrikolka() As Integer
        Get
            Return GetProperty(Of Integer)(MaxKonstVkMasaKocnaPrikolkaProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MaxKonstVkMasaKocnaPrikolkaProperty, value)
        End Set
    End Property
    Public Property MaxKonstVkMasaNeKocnaPrikolka() As Integer
        Get
            Return GetProperty(Of Integer)(MaxKonstVkMasaNeKocnaPrikolkaProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MaxKonstVkMasaNeKocnaPrikolkaProperty, value)
        End Set
    End Property
    Public Property MaxLegVkMasa() As Decimal
        Get
            Return GetProperty(Of Decimal)(MaxLegVkMasaProperty)
        End Get
        Set(ByVal value As Decimal)
            SetProperty(Of Decimal)(MaxLegVkMasaProperty, value)
        End Set
    End Property
    Public Property MaxLegVkMasaGrupa() As Decimal
        Get
            Return GetProperty(Of Decimal)(MaxLegVkMasaGrupaProperty)
        End Get
        Set(ByVal value As Decimal)
            SetProperty(Of Decimal)(MaxLegVkMasaGrupaProperty, value)
        End Set
    End Property
    Public Property NoiseTechnicalSpec() As String
        Get
            Return GetProperty(Of String)(NoiseTechnicalSpecProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(NoiseTechnicalSpecProperty, value)
        End Set
    End Property
    Public Property OdnosKwCcm() As String
        Get
            Return GetProperty(Of String)(OdnosKwCcmProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(OdnosKwCcmProperty, value)
        End Set
    End Property
    Public Property OsnoOptovaruvanje1() As Integer
        Get
            Return GetProperty(Of Integer)(OsnoOptovaruvanje1Property)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(OsnoOptovaruvanje1Property, value)
        End Set
    End Property
    Public Property OsnoOptovaruvanje2() As Integer
        Get
            Return GetProperty(Of Integer)(OsnoOptovaruvanje2Property)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(OsnoOptovaruvanje2Property, value)
        End Set
    End Property
    Public Property OsnoOptovaruvanje3() As Integer
        Get
            Return GetProperty(Of Integer)(OsnoOptovaruvanje3Property)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(OsnoOptovaruvanje3Property, value)
        End Set
    End Property
    Public Property OsnoOptovaruvanje4() As Integer
        Get
            Return GetProperty(Of Integer)(OsnoOptovaruvanje4Property)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(OsnoOptovaruvanje4Property, value)
        End Set
    End Property
    Public Property OsnoOptovaruvanje5() As Integer
        Get
            Return GetProperty(Of Integer)(OsnoOptovaruvanje5Property)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(OsnoOptovaruvanje5Property, value)
        End Set
    End Property



    Public Property OsnoOptovaruvanjePriklucna() As Integer
        Get
            Return GetProperty(Of Integer)(OsnoOptovaruvanjePriklucnaPRoperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(OsnoOptovaruvanjePriklucnaPRoperty, value)
        End Set
    End Property
    Public Property OznakaNaOdobrenie() As String
        Get
            Return GetProperty(Of String)(OznakaNaOdobrenieProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(OznakaNaOdobrenieProperty, value)
        End Set
    End Property
    Public Property OznakaNaOdobrenieZaPriklucUred() As String
        Get
            Return GetProperty(Of String)(OznakaNaOdobrenieZaPriklucUredProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(OznakaNaOdobrenieZaPriklucUredProperty, value)
        End Set
    End Property
    Public Property Tip() As String
        Get
            Return GetProperty(Of String)(TipProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(TipProperty, value)
        End Set
    End Property
    Public Property TBrOdobrenieMehanPriklucok() As String
        Get
            Return GetProperty(Of String)(TBrOdobrenieMehanPriklucokProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(TBrOdobrenieMehanPriklucokProperty, value)
        End Set
    End Property
    Public Property TMarkaMehanPriklucok() As String
        Get
            Return GetProperty(Of String)(TMarkaMehanPriklucokProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(TMarkaMehanPriklucokProperty, value)
        End Set
    End Property
    Public Property TMaxHorVerOptovaruvanjePriklucok() As Integer
        Get
            Return GetProperty(Of Integer)(TMaxHorVerOptovaruvanjePriklucokProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(TMaxHorVerOptovaruvanjePriklucokProperty, value)
        End Set
    End Property
    Public Property TMaxKonstVkMasaNaKombinacija() As Integer
        Get
            Return GetProperty(Of Integer)(TMaxKonstVkMasaNaKombinacijaProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(TMaxKonstVkMasaNaKombinacijaProperty, value)
        End Set
    End Property
    Public Property TMaxKonstVkMasaPoluprikolka() As Integer
        Get
            Return GetProperty(Of Integer)(TMaxKonstVkMasaPoluprikolkaProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(TMaxKonstVkMasaPoluprikolkaProperty, value)
        End Set
    End Property
    Public Property TMaxKonstVkMasaPrikolka() As Integer
        Get
            Return GetProperty(Of Integer)(TMaxKonstVkMasaPrikolkaProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(TMaxKonstVkMasaPrikolkaProperty, value)
        End Set
    End Property
    Public Property TMaxKonstVkMasaPrikolkaSoCenOska() As Integer
        Get
            Return GetProperty(Of Integer)(TMaxKonstVkMasaPrikolkaSoCenOskaProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(TMaxKonstVkMasaPrikolkaSoCenOskaProperty, value)
        End Set
    End Property
    Public Property TMaxKonstVkMasaPrikolkaStoMozePrikluci() As Integer
        Get
            Return GetProperty(Of Integer)(TMaxKonstVkMasaPrikolkaStoMozePrikluciProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(TMaxKonstVkMasaPrikolkaStoMozePrikluciProperty, value)
        End Set
    End Property
    Public Property TMinMasa() As Integer
        Get
            Return GetProperty(Of Integer)(TMinMasaProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(TMinMasaProperty, value)
        End Set
    End Property
    Public Property TTipMehanPriklucok() As String
        Get
            Return GetProperty(Of String)(TTipMehanPriklucokProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(TTipMehanPriklucokProperty, value)
        End Set
    End Property
    Public Property TZastitnaKabina() As String
        Get
            Return GetProperty(Of String)(TZastitnaKabinaProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(TZastitnaKabinaProperty, value)
        End Set
    End Property
    Public Property TZastitnaRamka() As String
        Get
            Return GetProperty(Of String)(TZastitnaRamkaProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(TZastitnaRamkaProperty, value)
        End Set
    End Property
    Public Property VarijantaIzvedba() As String
        Get
            Return GetProperty(Of String)(VarijantaIzvedbaProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(VarijantaIzvedbaProperty, value)
        End Set
    End Property
    Public Property BrojNaVrtezi() As Integer
        Get
            Return GetProperty(Of Integer)(BrojNaVrteziProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(BrojNaVrteziProperty, value)
        End Set
    End Property
    Public Property MaxKonstVkMasa() As Decimal
        Get
            Return GetProperty(Of Decimal)(MaxKonstVkMasaProperty)
        End Get
        Set(ByVal value As Decimal)
            SetProperty(Of Decimal)(MaxKonstVkMasaProperty, value)
        End Set
    End Property
    Public Property BrojEUPotvrda() As String
        Get
            Return GetProperty(Of String)(BrojEUPotvrdaProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(BrojEUPotvrdaProperty, value)
        End Set
    End Property
  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function


#End Region 'Business Properties and Methods

#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicleBodyType") Then
      AuthorizationRules.AllowWrite("IdVehicleBodyType", roleName)
    Else
      AuthorizationRules.DenyWrite("IdVehicleBodyType", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdVehicleBodyType")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdEngineType") Then
      AuthorizationRules.AllowWrite("IdEngineType", roleName)
    Else
      AuthorizationRules.DenyWrite("IdEngineType", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdEngineType")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdEnginePowerSource") Then
      AuthorizationRules.AllowWrite("IdEnginePowerSource", roleName)
    Else
      AuthorizationRules.DenyWrite("IdEnginePowerSource", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdEnginePowerSource")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdEngineMark") Then
      AuthorizationRules.AllowWrite("IdEngineMark", roleName)
    Else
      AuthorizationRules.DenyWrite("IdEngineMark", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdEngineMark")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdGearBox") Then
      AuthorizationRules.AllowWrite("IdGearBox", roleName)
    Else
      AuthorizationRules.DenyWrite("IdGearBox", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdGearBox")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdBreakes") Then
      AuthorizationRules.AllowWrite("IdBreakes", roleName)
    Else
      AuthorizationRules.DenyWrite("IdBreakes", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdBreakes")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdSupporting") Then
      AuthorizationRules.AllowWrite("IdSupporting", roleName)
    Else
      AuthorizationRules.DenyWrite("IdSupporting", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdSupporting")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicleModel") Then
      AuthorizationRules.AllowWrite("IdVehicleModel", roleName)
    Else
      AuthorizationRules.DenyWrite("IdVehicleModel", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdVehicleModel")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("EngineNumber") Then
      AuthorizationRules.AllowWrite("EngineNumber", roleName)
    Else
      AuthorizationRules.DenyWrite("EngineNumber", roleName)
    End If
    'AuthorizationRules.AllowWrite("EngineNumber")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("EnginePower") Then
      AuthorizationRules.AllowWrite("EnginePower", roleName)
    Else
      AuthorizationRules.DenyWrite("EnginePower", roleName)
    End If
    'AuthorizationRules.AllowWrite("EnginePower")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("EngineTorque") Then
      AuthorizationRules.AllowWrite("EngineTorque", roleName)
    Else
      AuthorizationRules.DenyWrite("EngineTorque", roleName)
    End If
    'AuthorizationRules.AllowWrite("EngineTorque")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("EngineWorkingCapacity") Then
      AuthorizationRules.AllowWrite("EngineWorkingCapacity", roleName)
    Else
      AuthorizationRules.DenyWrite("EngineWorkingCapacity", roleName)
    End If
    'AuthorizationRules.AllowWrite("EngineWorkingCapacity")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("EnginePowerOutPut") Then
      AuthorizationRules.AllowWrite("EnginePowerOutPut", roleName)
    Else
      AuthorizationRules.DenyWrite("EnginePowerOutPut", roleName)
    End If
    'AuthorizationRules.AllowWrite("EnginePowerOutPut")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ShellNumber") Then
      AuthorizationRules.AllowWrite("ShellNumber", roleName)
    Else
      AuthorizationRules.DenyWrite("ShellNumber", roleName)
    End If
    'AuthorizationRules.AllowWrite("ShellNumber")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("RegistrationNumber") Then
      AuthorizationRules.AllowWrite("RegistrationNumber", roleName)
    Else
      AuthorizationRules.DenyWrite("RegistrationNumber", roleName)
    End If
    'AuthorizationRules.AllowWrite("RegistrationNumber")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DateOfLastRegistrationt") Then
      AuthorizationRules.AllowWrite("DateOfLastRegistrationt", roleName)
    Else
      AuthorizationRules.DenyWrite("DateOfLastRegistrationt", roleName)
    End If
    'AuthorizationRules.AllowWrite("DateOfLastRegistrationt")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("MakeDate") Then
      AuthorizationRules.AllowWrite("MakeDate", roleName)
    Else
      AuthorizationRules.DenyWrite("MakeDate", roleName)
    End If
    'AuthorizationRules.AllowWrite("MakeDate")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NumberOfDoors") Then
      AuthorizationRules.AllowWrite("NumberOfDoors", roleName)
    Else
      AuthorizationRules.DenyWrite("NumberOfDoors", roleName)
    End If
    'AuthorizationRules.AllowWrite("NumberOfDoors")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NumberOfSeats") Then
      AuthorizationRules.AllowWrite("NumberOfSeats", roleName)
    Else
      AuthorizationRules.DenyWrite("NumberOfSeats", roleName)
    End If
    'AuthorizationRules.AllowWrite("NumberOfSeats")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NumberOfStandingSeats") Then
      AuthorizationRules.AllowWrite("NumberOfStandingSeats", roleName)
    Else
      AuthorizationRules.DenyWrite("NumberOfStandingSeats", roleName)
    End If
    'AuthorizationRules.AllowWrite("NumberOfStandingSeats")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NumberOfLieingSeats") Then
      AuthorizationRules.AllowWrite("NumberOfLieingSeats", roleName)
    Else
      AuthorizationRules.DenyWrite("NumberOfLieingSeats", roleName)
    End If
    'AuthorizationRules.AllowWrite("NumberOfLieingSeats")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("EmptyWaight") Then
      AuthorizationRules.AllowWrite("EmptyWaight", roleName)
    Else
      AuthorizationRules.DenyWrite("EmptyWaight", roleName)
    End If
    'AuthorizationRules.AllowWrite("EmptyWaight")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("MaximunAllowedWaight") Then
      AuthorizationRules.AllowWrite("MaximunAllowedWaight", roleName)
    Else
      AuthorizationRules.DenyWrite("MaximunAllowedWaight", roleName)
    End If
    'AuthorizationRules.AllowWrite("MaximunAllowedWaight")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TrailerWaightWithBreak") Then
      AuthorizationRules.AllowWrite("TrailerWaightWithBreak", roleName)
    Else
      AuthorizationRules.DenyWrite("TrailerWaightWithBreak", roleName)
    End If
    'AuthorizationRules.AllowWrite("TrailerWaightWithBreak")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TrailerWaightWithoutBreak") Then
      AuthorizationRules.AllowWrite("TrailerWaightWithoutBreak", roleName)
    Else
      AuthorizationRules.DenyWrite("TrailerWaightWithoutBreak", roleName)
    End If
    'AuthorizationRules.AllowWrite("TrailerWaightWithoutBreak")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NumberOfAxis") Then
      AuthorizationRules.AllowWrite("NumberOfAxis", roleName)
    Else
      AuthorizationRules.DenyWrite("NumberOfAxis", roleName)
    End If
    'AuthorizationRules.AllowWrite("NumberOfAxis")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PropulsionAxis") Then
      AuthorizationRules.AllowWrite("PropulsionAxis", roleName)
    Else
      AuthorizationRules.DenyWrite("PropulsionAxis", roleName)
    End If
    'AuthorizationRules.AllowWrite("PropulsionAxis")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NumberOfWheels") Then
      AuthorizationRules.AllowWrite("NumberOfWheels", roleName)
    Else
      AuthorizationRules.DenyWrite("NumberOfWheels", roleName)
    End If
    'AuthorizationRules.AllowWrite("NumberOfWheels")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NumberOfPropulsionWheels") Then
      AuthorizationRules.AllowWrite("NumberOfPropulsionWheels", roleName)
    Else
      AuthorizationRules.DenyWrite("NumberOfPropulsionWheels", roleName)
    End If
    'AuthorizationRules.AllowWrite("NumberOfPropulsionWheels")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("VehicleSizeHight") Then
      AuthorizationRules.AllowWrite("VehicleSizeHight", roleName)
    Else
      AuthorizationRules.DenyWrite("VehicleSizeHight", roleName)
    End If
    'AuthorizationRules.AllowWrite("VehicleSizeHight")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("VehicleSizeWidth") Then
      AuthorizationRules.AllowWrite("VehicleSizeWidth", roleName)
    Else
      AuthorizationRules.DenyWrite("VehicleSizeWidth", roleName)
    End If
    'AuthorizationRules.AllowWrite("VehicleSizeWidth")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("VehicleSizeLength") Then
      AuthorizationRules.AllowWrite("VehicleSizeLength", roleName)
    Else
      AuthorizationRules.DenyWrite("VehicleSizeLength", roleName)
    End If
    'AuthorizationRules.AllowWrite("VehicleSizeLength")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Suffocation") Then
      AuthorizationRules.AllowWrite("Suffocation", roleName)
    Else
      AuthorizationRules.DenyWrite("Suffocation", roleName)
    End If
    'AuthorizationRules.AllowWrite("Suffocation")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Hook") Then
      AuthorizationRules.AllowWrite("Hook", roleName)
    Else
      AuthorizationRules.DenyWrite("Hook", roleName)
    End If
    'AuthorizationRules.AllowWrite("Hook")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Vitlo") Then
      AuthorizationRules.AllowWrite("Vitlo", roleName)
    Else
      AuthorizationRules.DenyWrite("Vitlo", roleName)
    End If
    'AuthorizationRules.AllowWrite("Vitlo")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdPrimaryColor") Then
      AuthorizationRules.AllowWrite("IdPrimaryColor", roleName)
    Else
      AuthorizationRules.DenyWrite("IdPrimaryColor", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdPrimaryColor")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdSecondaryColor") Then
      AuthorizationRules.AllowWrite("IdSecondaryColor", roleName)
    Else
      AuthorizationRules.DenyWrite("IdSecondaryColor", roleName)
    End If
    'AuthorizationRules.AllowWrite("IdSecondaryColor")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("HologationSertificateNumber") Then
      AuthorizationRules.AllowWrite("HologationSertificateNumber", roleName)
    Else
      AuthorizationRules.DenyWrite("HologationSertificateNumber", roleName)
    End If
    'AuthorizationRules.AllowWrite("HologationSertificateNumber")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NoiseStatic") Then
      AuthorizationRules.AllowWrite("NoiseStatic", roleName)
    Else
      AuthorizationRules.DenyWrite("NoiseStatic", roleName)
    End If
    'AuthorizationRules.AllowWrite("NoiseStatic")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NoiseMovment") Then
      AuthorizationRules.AllowWrite("NoiseMovment", roleName)
    Else
      AuthorizationRules.DenyWrite("NoiseMovment", roleName)
    End If
    'AuthorizationRules.AllowWrite("NoiseMovment")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Co") Then
      AuthorizationRules.AllowWrite("Co", roleName)
    Else
      AuthorizationRules.DenyWrite("Co", roleName)
    End If
    'AuthorizationRules.AllowWrite("Co")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Hc") Then
      AuthorizationRules.AllowWrite("Hc", roleName)
    Else
      AuthorizationRules.DenyWrite("Hc", roleName)
    End If
    'AuthorizationRules.AllowWrite("Hc")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("NOx") Then
      AuthorizationRules.AllowWrite("NOx", roleName)
    Else
      AuthorizationRules.DenyWrite("NOx", roleName)
    End If
    'AuthorizationRules.AllowWrite("NOx")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("HCNOx") Then
      AuthorizationRules.AllowWrite("HCNOx", roleName)
    Else
      AuthorizationRules.DenyWrite("HCNOx", roleName)
    End If
    'AuthorizationRules.AllowWrite("HCNOx")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Blackening") Then
      AuthorizationRules.AllowWrite("Blackening", roleName)
    Else
      AuthorizationRules.DenyWrite("Blackening", roleName)
    End If
    'AuthorizationRules.AllowWrite("Blackening")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Pinpoints") Then
      AuthorizationRules.AllowWrite("Pinpoints", roleName)
    Else
      AuthorizationRules.DenyWrite("Pinpoints", roleName)
    End If
    'AuthorizationRules.AllowWrite("Pinpoints")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Co2") Then
      AuthorizationRules.AllowWrite("Co2", roleName)
    Else
      AuthorizationRules.DenyWrite("Co2", roleName)
    End If
    'AuthorizationRules.AllowWrite("Co2")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("FuelConsumption") Then
      AuthorizationRules.AllowWrite("FuelConsumption", roleName)
    Else
      AuthorizationRules.DenyWrite("FuelConsumption", roleName)
    End If
    'AuthorizationRules.AllowWrite("FuelConsumption")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CapacityFuelTank") Then
      AuthorizationRules.AllowWrite("CapacityFuelTank", roleName)
    Else
      AuthorizationRules.DenyWrite("CapacityFuelTank", roleName)
        End If
        'AuthorizationRules.AllowWrite("CapacityFuelTank")
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdentifikacijaNaMotorMestoMetod") Then
            AuthorizationRules.AllowWrite("IdentifikacijaNaMotorMestoMetod", roleName)
        Else
            AuthorizationRules.DenyWrite("IdentifikacijaNaMotorMestoMetod", roleName)
        End If

        'AuthorizationRules.AllowWrite("IdentifikacijaNaMotorMestoMetod")
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
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Vehicle")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Vehicle")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Vehicle")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Vehicle")
  End Function

#End Region ' Authorization Rules

#Region " Validation Rules "

  Private Sub Vehicle_ChildChanged(ByVal sender As Object, ByVal e As Csla.Core.ChildChangedEventArgs) Handles Me.ChildChanged
    Try

      'Console.WriteLine(e.ChildObject.GetType)
      'If TypeOf (e.ChildObject) Is VehicleRegistrations Then
      '    ValidationRules.CheckRules(VehicleRegistrationsProperty)
      'End If

    Catch ex As Exception

    End Try

  End Sub

  Private Sub Vehicle_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
    Try
      If e.PropertyName = "IdVehicleCategories" Then
        If ReadProperty(Of Integer)(IdVehicleCategoriesProperty) <> 0 Then
          _requiredField = Nothing
          _requiredField = VehicleFieldList.GetListByIdCategory(ReadProperty(Of Integer)(IdVehicleCategoriesProperty))
          'Me.BrokenRulesCollection.Clear()
          ValidationRules.CheckRules()
          'For i As Integer = 0 To _requiredField.Count - 1
          '  If _requiredField(i).Key <> "IdVehicleCategories" Then
          '    PropertyHasChanged(_requiredField(i).Key)
          '    'ValidationRules.CheckRules(_requiredField(i).Key)
          '  End If
          'Next
        End If
      End If
    Catch ex As Exception

    End Try
  End Sub

  Private Function StRequired(Of T As Vehicle)(ByVal target As T, _
    ByVal e As Csla.Validation.RuleArgs) As Boolean
    If (_requiredField IsNot Nothing) AndAlso _requiredField.ContainsKey(e.PropertyName) Then
      Dim value As String = CStr(CallByName(target, e.PropertyName, CallType.Get))
      If Strings.Trim(value) = String.Empty Then
        e.Description = "Полето е задолжително"
        Return False
      Else
        Return True
      End If
    End If
    Return True
  End Function

  Private Function DecRequired(Of T As Vehicle)(ByVal target As T, _
  ByVal e As Csla.Validation.RuleArgs) As Boolean
    If (_requiredField IsNot Nothing) AndAlso _requiredField.ContainsKey(e.PropertyName) Then
      Dim value As Double = CDbl(CallByName(target, e.PropertyName, CallType.Get))
      If value <= 0 Then
        e.Description = "Полето е задолжително"
        Return False
      Else
        Return True
      End If
    End If
    Return True
  End Function

  Protected Overrides Sub AddInstanceBusinessRules()
    For Each prop As System.Reflection.PropertyInfo In GetType(Vehicle).GetProperties
      If prop.CanWrite Then
        'za stringovi
        Select Case prop.PropertyType.Name
          Case GetType(String).Name
            ValidationRules.AddInstanceRule(Of Vehicle)(AddressOf StRequired, prop.Name)
          Case GetType(System.Int16).Name, GetType(System.Int32).Name, _
            GetType(System.Int64).Name, GetType(System.Single).Name, _
            GetType(System.Double).Name
            ValidationRules.AddInstanceRule(Of Vehicle)(AddressOf DecRequired, prop.Name)
        End Select


      End If
    Next
    MyBase.AddBusinessRules()
  End Sub


  Protected Overrides Sub AddBusinessRules()
    'klucevi
    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                            New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdVehicleCategoriesProperty, 1))

    'IdVehicleBodyType rules
    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect _
                            , New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdVehicleBodyTypeProperty, 1))
    ''IdEngineTypeProperty rules
    'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    '                        New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdEngineTypeProperty, 1))
    ''IdEnginePowerSourceProperty rules
    'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    '                        New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdEnginePowerSourceProperty, 1))
    'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    '                    New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdEngineSecondPowerSourceProperty, 0))

    ''IdGearBoxProperty rules
    ''ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    ''                        New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdGearBoxProperty, 1))
    ' ''IdBreakesProperty rules
    ''ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    ''                        New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdBreakesProperty, 1))
    ' ''IdSupportingProperty rules
    ''ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    ''                        New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdSupportingProperty, 1))
    ''IdVehicleModelProperty rules
    'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    '                        New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdVehicleModelProperty, 1))
    ''IdVehicleModelProperty rules

    ''IdPrimaryColorProperty rules
    'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    '                        New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdPrimaryColorProperty, 1))
    ''ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
    ''                        New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(IdSecondaryColorProperty, 0))



    ' ShellNumberProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ShellNumberProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, FirstRegistrationNumberProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, LastRegistratinNumberProperty)
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, FirstRegistrationCommunityProperty)
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, LastRegistrationCommunityProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(ShellNumberProperty, 17))
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMinLength, New Csla.Validation.CommonRules.MinLengthRuleArgs(ShellNumberProperty, 4))
    ValidationRules.AddRule(Of Vehicle)(AddressOf NoDuplicates, ShellNumberProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, _
                            New Csla.Validation.CommonRules.MaxLengthRuleArgs(FirstRegistrationNumberProperty, 50))
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, _
                            New Csla.Validation.CommonRules.MaxLengthRuleArgs(LastRegistratinNumberProperty, 50))


    '' EngineNumberProperty rules

    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, EngineNumberProperty)
    '' EnginePowerProperty rules
    'ValidationRules.AddRule(Of Vehicle)(AddressOf MinEnginePower, EnginePowerProperty)
    ''NumberOfDoorsProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.IntegerMinValue, New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(NumberOfDoorsProperty, 0))
    ''NumberOfSeatsProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.IntegerMinValue, New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(NumberOfSeatsProperty, 0))
    ''EmptyWaightProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.IntegerMinValue, New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(EmptyWaightProperty, 0))
    ''MaximunAllowedWaightProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.IntegerMinValue, New Csla.Validation.CommonRules.IntegerMinValueRuleArgs(MaximunAllowedWaightProperty, 0))


    '' MakeDateProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, MakeDateProperty)
    '' TrailerWaightWithBreakProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(TrailerWaightWithBreakProperty, 20))
    '' TrailerWaightWithoutBreakProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(TrailerWaightWithoutBreakProperty, 20))
    '' HologationSertificateNumberProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(HologationSertificateNumberProperty, 100))
    '' BlackeningProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BlackeningProperty, 20))
    '' PinpointsProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(PinpointsProperty, 20))
    '' FuelConsumptionProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(FuelConsumptionProperty, 20))
    '' NoteProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NoteProperty, 500))

    ' NumberOfAxisProperty and PropulsionAxisProperty rules
    ValidationRules.AddRule(Of Vehicle)(AddressOf NumOfAxes, NumberOfAxisProperty)
    ValidationRules.AddRule(Of Vehicle)(AddressOf NumOfAxes, PropulsionAxisProperty)
    ValidationRules.AddDependentProperty(NumberOfAxisProperty, PropulsionAxisProperty, True)

    ' NumberOfWheelsProperty and NumberOfPropulsionWheelsProperty rules
    ValidationRules.AddRule(Of Vehicle)(AddressOf NumOfWheels, NumberOfWheelsProperty)
    ValidationRules.AddRule(Of Vehicle)(AddressOf NumOfWheels, NumberOfPropulsionWheelsProperty)
    ValidationRules.AddDependentProperty(NumberOfWheelsProperty, NumberOfPropulsionWheelsProperty, True)

    '' VehicleSizeHightProperty rules
    'ValidationRules.AddRule(Of Vehicle)(AddressOf SizeHight, VehicleSizeHightProperty)
    '' VehicleSizeLengthProperty rules
    'ValidationRules.AddRule(Of Vehicle)(AddressOf SizeLength, VehicleSizeLengthProperty)
    '' VehicleSizeWidthProperty rules
    'ValidationRules.AddRule(Of Vehicle)(AddressOf SizeWidth, VehicleSizeWidthProperty)

    'ValidationRules.AddRule(Of Vehicle)(AddressOf EmptyGTMaxWeight, EmptyWaightProperty)
    'ValidationRules.AddRule(Of Vehicle)(AddressOf EmptyGTMaxWeight, MaximunAllowedWaightProperty)
    'ValidationRules.AddDependentProperty(EmptyWaightProperty, MaximunAllowedWaightProperty, True)

    'ValidationRules.AddRule(Of Vehicle)(AddressOf RegistationRequired, VehicleRegistrationsProperty)

    'ValidationRules.AddRule(Of VehicleRegistration) _
    '(AddressOf DateCreatedAndValid, FirstRegistrationMakeDateProperty)
    'ValidationRules.AddRule(Of VehicleRegistration) _
    '(AddressOf DateCreatedAndValid, FirstRegistrationValidTillProperty)
    'ValidationRules.AddDependentProperty(FirstRegistrationMakeDateProperty, FirstRegistrationValidTillProperty, True)
    'ValidationRules.AddRule(Of VehicleRegistration) _
    '(AddressOf DateCreatedAndValid, LastRegistrationMakeDateProperty)
    'ValidationRules.AddRule(Of VehicleRegistration) _
    '(AddressOf DateCreatedAndValid, LastRegistrationValidTillProperty)
    'ValidationRules.AddDependentProperty(LastRegistrationMakeDateProperty, LastRegistrationValidTillProperty, True)

  End Sub

  Private Shared Function DateCreatedAndValid(Of T As VehicleRegistration)(ByVal target As T, _
     ByVal e As Csla.Validation.RuleArgs) As Boolean
    If target.DateOfRegistration >= target.DateRegistrationValidTill Then
      e.Description = "Одберете датум до кој е валидна регистрацијата"
      Return False
    Else
      Return True
    End If
  End Function

  Private Shared Function RegistationRequired(Of T As Vehicle)(ByVal target As T, _
    ByVal e As Csla.Validation.RuleArgs) As Boolean

    'If target.Registrations.Count = 0 Then
    '    e.Description = "Мора да биде внесена барем една регистација која ке биде и прва"
    '    Return False
    'Else
    '    Return True
    'End If

  End Function


  Private Shared Function EmptyGTMaxWeight(Of T As Vehicle)(ByVal target As T, _
    ByVal e As Csla.Validation.RuleArgs) As Boolean
    If target.EmptyWaight > target.MaximunAllowedWaight Then
      e.Description = "Неправилно поплнети тежини"
      Return False
    Else
      Return True
    End If
  End Function

  Private Shared Function NumOfAxes(Of T As Vehicle)(ByVal target As T, _
      ByVal e As Csla.Validation.RuleArgs) As Boolean
    If target.NumberOfAxis < target.PropulsionAxis Then
      e.Description = "Број на оски мора да биде поголем или еднаков на број на носечки оски"
      Return False
    Else
      Return True
    End If
  End Function

  Private Shared Function SizeHight(Of T As Vehicle)(ByVal target As T, _
      ByVal e As Csla.Validation.RuleArgs) As Boolean
    If target.VehicleSizeHight <= 0 Then
      e.Description = "Висината на возилото мора да е >=0"
      Return False
    Else
      Return True
    End If
  End Function
  Private Shared Function SizeLength(Of T As Vehicle)(ByVal target As T, _
     ByVal e As Csla.Validation.RuleArgs) As Boolean
    If target.VehicleSizeLength <= 0 Then
      e.Description = "Должината на возилото мора да е >=0"
      Return False
    Else
      Return True
    End If
  End Function
  Private Shared Function SizeWidth(Of T As Vehicle)(ByVal target As T, _
     ByVal e As Csla.Validation.RuleArgs) As Boolean
    If target.VehicleSizeWidth <= 0 Then
      e.Description = "Ширината на возилото мора да е >=0"
      Return False
    Else
      Return True
    End If
  End Function
  Private Shared Function MinEnginePower(Of T As Vehicle)(ByVal target As T, _
      ByVal e As Csla.Validation.RuleArgs) As Boolean
    If target.EnginePower <= 0 Then
      e.Description = "Снагата на моторот мора да е позитивна вредност"
      Return False
    Else
      Return True
    End If
  End Function
  Private Shared Function NumOfWheels(Of T As Vehicle)(ByVal target As T, _
     ByVal e As Csla.Validation.RuleArgs) As Boolean
    If target.NumberOfWheels < target.NumberOfPropulsionWheels Then
      e.Description = "Број на тркала мора да биде поголем или еднаков на број на носечки тркала"
      Return False
    Else
      Return True
    End If
  End Function

  Private Shared Function NoDuplicates(Of T As Vehicle)(ByVal target As T, _
      ByVal e As Csla.Validation.RuleArgs) As Boolean
    If Vehicle.ShellExists(target.ShellNumber, target.Id) Then
      e.Description = "Шасијата мора да биде единствена"
      Return False
    Else
      Return True
    End If
  End Function

  'Private Shared Function NoDuplicatesRegistrationNumber(Of T As Vehicle)(ByVal target As T, _
  '  ByVal e As Csla.Validation.RuleArgs) As Boolean
  '  If Vehicle.RegistrationNumberExists(target.RegistrationNumber, target.Id) Then
  '    e.Description = "Регистрацијата мора да биде единствена"
  '    Return False
  '  Else
  '    Return True
  '  End If
  'End Function


#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewVehicle() As Vehicle
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a Vehicle")
    End If
    Return DataPortal.Create(Of Vehicle)()
  End Function

  Public Shared Function GetVehicle(ByVal id As Integer) As Vehicle
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a Vehicle")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of Vehicle, Integer)(id))
  End Function
  Public Shared Function GetVehicleByShellNum(ByVal shellNum As String) As Vehicle
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a Vehicle")
    End If
    Return DataPortal.Fetch(New filterCriteria(shellNum))
  End Function
  Public Shared Function GetVehicleNotActive(ByVal id As Integer) As Vehicle
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a Vehicle")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of Vehicle, Integer)(id))
  End Function

  Public Shared Sub DeleteVehicle(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a Vehicle")
    End If
    DataPortal.Delete(New SingleCriteria(Of Vehicle, Integer)(id))
  End Sub

  Public Overrides Function Save() As Vehicle
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a Vehicle")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a Vehicle")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a Vehicle")
    End If

    Dim result As Vehicle = MyBase.Save

    'OnVehicleSaved(Me, New Csla.Core.SavedEventArgs(result))
    'CustomerVehiclesRelation.OnVehicleSaved(Me, New Csla.Core.SavedEventArgs(result))
    Return result
    'Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleChild() As Vehicle
    Return DataPortal.CreateChild(Of Vehicle)()
  End Function

  Friend Shared Function GetVehicle(ByVal dr As SafeDataReader) As Vehicle
    Return DataPortal.FetchChild(Of Vehicle)(dr)
  End Function

#End Region 'Child Factory Methods

#Region " Data Access "
  <Serializable()> _
 Private Class filterCriteria
    Private _inNum As String

    Public ReadOnly Property InNum() As String
      Get
        Return _inNum
      End Get
    End Property

    Public Sub New(ByVal inNum As String)
      _inNum = inNum
    End Sub
  End Class
#Region " Data Access - Create "

  <RunLocal()> _
  Private Overloads Sub DataPortal_Create()
    'Dim tmpCom As Community = Community.GetCommunity(CType(Csla.ApplicationContext.LocalContext("objOpcii"), Options).IdCommunity)
    'Dim newReg As VehicleRegistration = Registrations.AddNew
    'newReg.RegistrationNumber = tmpCom.RegistrationCode & "-000-AA"
    'newReg.DateOfRegistration = Now.Date
    'newReg.DateRegistrationValidTill = Now.AddYears(1).Date
    'newReg.PlaceOfRegistration = tmpCom.CommunityName
    'FirstRegistrationNumber = tmpCom.RegistrationCode & "-000-AA"
    'LastRegistratinNumber = tmpCom.RegistrationCode & "-000-AA"
    'FirstRegistrationCommunity = tmpCom.CommunityName
    'LastRegistrationCommunity = tmpCom.CommunityName
    'FirstRegistrationMakeDate = Now.Date
    'FirstRegistrationValidTill = Now.AddYears(1).Date
    'LastRegistrationMakeDate = Now.Date
    'LastRegistrationValidTill = Now.AddYears(1).Date
    ValidationRules.CheckRules()
  End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of Vehicle, Integer))
    Database.LogInfo("Vehicle.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          ' cm.Connection.ConnectionTimeout(120)
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of Integer)(IdVehicleBodyTypeProperty, dr.GetInt32("IdVehicleBodyType"))
            LoadProperty(Of Integer)(IdVehicleCategoriesProperty, dr.GetInt32("IdVehicleCategories"))
            LoadProperty(Of Integer)(IdVehicleUseProperty, dr.GetInt32("IdVehicleUse"))
            LoadProperty(Of Integer)(IdEngineTypeProperty, dr.GetInt32("IdEngineType"))
            LoadProperty(Of Integer)(IdEnginePowerSourceProperty, dr.GetInt32("IdEnginePowerSource"))
            LoadProperty(Of Integer)(IdEngineSecondPowerSourceProperty, dr.GetInt32("IdEngineSecondPowerSource"))
            LoadProperty(Of Integer)(IdGearBoxProperty, dr.GetInt32("IdGearBox"))
            LoadProperty(Of Integer)(IdBreakesProperty, dr.GetInt32("IdBreakes"))
            LoadProperty(Of Integer)(IdSupportingProperty, dr.GetInt32("IdSupporting"))
            LoadProperty(Of Integer)(IdVehicleModelProperty, dr.GetInt32("IdVehicleModel"))
            LoadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty, dr.GetInt32("IdVehicleCategoryForPayments"))
            LoadProperty(Of Integer)(IdEngineEcoProgramProperty, dr.GetInt32("IdEngineEcoProgram"))
            LoadProperty(Of Integer)(IdMadeCountryProperty, dr.GetInt32("IdMadeCountry"))
            LoadProperty(Of String)(ColorCodeProperty, dr.GetString("ColorCode"))
            LoadProperty(Of Integer)(FirstIdRegistrationIssuerProperty, dr.GetInt32("IdFirstRegistrationIssuer"))
            LoadProperty(Of Integer)(LastIdRegistrationIssuerProperty, dr.GetInt32("IdLastRegistrationIssuer"))
            LoadProperty(Of String)(FirstRegistrationNumberProperty, dr.GetString("FirstRegistrationNumber"))
            LoadProperty(Of String)(LastRegistratinNumberProperty, dr.GetString("LastRegistratinNumber"))
            ' LoadProperty(Of String)(FirstRegistrationCommunityProperty, dr.GetString("FirstRegistrationCommunity"))
            '  LoadProperty(Of String)(LastRegistrationCommunityProperty, dr.GetString("LastRegistrationCommunity"))
            LoadProperty(Of SmartDate, Date?)(FirstRegistrationMakeDateProperty, dr.GetSmartDate("FirstRegistrationMakeDate", True))
            LoadProperty(Of SmartDate, Date?)(FirstRegistrationValidTillProperty, dr.GetSmartDate("FirstRegistrationValidTill", True))
            LoadProperty(Of SmartDate, Date?)(LastRegistrationMakeDateProperty, dr.GetSmartDate("LastRegistrationMakeDate", True))
            LoadProperty(Of SmartDate, Date?)(LastRegistrationValidTillProperty, dr.GetSmartDate("LastRegistrationValidTill", True))

            LoadProperty(Of String)(EngineNumberProperty, dr.GetString("EngineNumber"))
            LoadProperty(Of Decimal)(EnginePowerProperty, dr.GetValue("EnginePower"))
            LoadProperty(Of String)(EngineTorqueProperty, dr.GetString("EngineTorque"))
            LoadProperty(Of Decimal)(EngineTorqueUnderGassProperty, dr.GetValue("EngineTorqueUnderGass"))
            LoadProperty(Of Decimal)(EngineWorkingCapacityProperty, dr.GetValue("EngineWorkingCapacity"))
            LoadProperty(Of Decimal)(EnginePowerOutPutProperty, dr.GetValue("EnginePowerOutPut"))
            LoadProperty(Of String)(ShellNumberProperty, dr.GetString("ShellNumber"))
            LoadProperty(Of SmartDate, Date?)(MakeDateProperty, dr.GetSmartDate("MakeDate", True))
            LoadProperty(Of Integer)(NumberOfDoorsProperty, dr.GetInt32("NumberOfDoors"))
            LoadProperty(Of Short)(NumberOfSeatsProperty, dr.GetInt16("NumberOfSeats"))
            LoadProperty(Of Short)(NumberOfStandingSeatsProperty, dr.GetInt16("NumberOfStandingSeats"))
            LoadProperty(Of Short)(NumberOfLieingSeatsProperty, dr.GetInt16("NumberOfLieingSeats"))
            LoadProperty(Of Decimal)(EmptyWaightProperty, dr.GetValue("EmptyWaight"))
            LoadProperty(Of Decimal)(MaximunAllowedWaightProperty, dr.GetValue("MaximunAllowedWaight"))
            LoadProperty(Of String)(TrailerWaightWithBreakProperty, dr.GetString("TrailerWaightWithBreak"))
            LoadProperty(Of String)(TrailerWaightWithoutBreakProperty, dr.GetString("TrailerWaightWithoutBreak"))
            LoadProperty(Of Integer)(NumberOfAxisProperty, dr.GetInt32("NumberOfAxis"))
            LoadProperty(Of Integer)(PropulsionAxisProperty, dr.GetInt32("PropulsionAxis"))
            LoadProperty(Of Integer)(NumberOfWheelsProperty, dr.GetInt32("NumberOfWheels"))
            LoadProperty(Of Integer)(NumberOfPropulsionWheelsProperty, dr.GetInt32("NumberOfPropulsionWheels"))
            LoadProperty(Of Decimal)(VehicleSizeHightProperty, dr.GetValue("VehicleSizeHight"))
            LoadProperty(Of Decimal)(VehicleSizeWidthProperty, dr.GetValue("VehicleSizeWidth"))
            LoadProperty(Of Decimal)(VehicleSizeLengthProperty, dr.GetValue("VehicleSizeLength"))
            LoadProperty(Of Boolean)(SuffocationProperty, dr.GetBoolean("Suffocation"))
            LoadProperty(Of Boolean)(HookProperty, dr.GetBoolean("Hook"))
            LoadProperty(Of Boolean)(VitloProperty, dr.GetBoolean("Vitlo"))
            LoadProperty(Of Boolean)(VerticalBurdenOnTheSeatProperty, dr.GetBoolean("VerticalBurdenOnTheSeat"))
            LoadProperty(Of String)(VerticalBurdenOnTheSeatNoteProperty, dr.GetString("VerticalBurdenOnTheSeatNote"))
            LoadProperty(Of Integer)(IdPrimaryColorProperty, dr.GetInt32("IdPrimaryColor"))
            LoadProperty(Of Integer)(IdSecondaryColorProperty, dr.GetInt32("IdSecondaryColor"))
            LoadProperty(Of String)(HologationSertificateNumberProperty, dr.GetString("HologationSertificateNumber"))
            LoadProperty(Of Decimal)(NoiseStaticProperty, dr.GetValue("NoiseStatic"))
            LoadProperty(Of Decimal)(NoiseMovmentProperty, dr.GetValue("NoiseMovment"))
            LoadProperty(Of Decimal)(CoProperty, dr.GetValue("CO"))
            LoadProperty(Of Decimal)(HcProperty, dr.GetValue("HC"))
            LoadProperty(Of Decimal)(NOxProperty, dr.GetValue("NOx"))
            LoadProperty(Of Decimal)(HCNOxProperty, dr.GetValue("HCNOx"))
            LoadProperty(Of String)(BlackeningProperty, dr.GetString("Blackening"))
            LoadProperty(Of String)(PinpointsProperty, dr.GetString("Pinpoints"))
            LoadProperty(Of Decimal)(Co2Property, dr.GetValue("CO2"))
            LoadProperty(Of String)(FuelConsumptionProperty, dr.GetString("FuelConsumption"))
            LoadProperty(Of Decimal)(CapacityFuelTankProperty, dr.GetValue("CapacityFuelTank"))
            LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
            LoadProperty(Of Boolean)(IsSocialNotPrivateProperty, dr.GetBoolean("IsSocialNotPrivate"))
            LoadProperty(Of Boolean)(ForPrivateTransportNotPublicProperty, dr.GetBoolean("ForPrivateTransportNotPublic"))
            LoadProperty(Of Boolean)(TNGProperty, dr.GetBoolean("TNG"))
            LoadProperty(Of String)(VehicleModelAddingProperty, dr.GetString("VehicleModelAdding"))
            LoadProperty(Of Decimal)(MaxSpeedProperty, dr.GetValue("MaxSpeed"))
                        LoadProperty(Of Decimal)(TempOfEngineOilProperty, dr.GetValue("TempOfEngineOil"))
                        LoadProperty(Of String)(IdentifikacijaNaMotorMestoMetodProperty, dr.GetString("IdentifikacijaNaMotorMestoMetod"))
                        LoadProperty(Of Integer)(MasaPoOska1Property, dr.GetInt32("MasaPoOska1"))
                        LoadProperty(Of Integer)(MasaPoOska2Property, dr.GetInt32("MasaPoOska2"))
                        LoadProperty(Of Integer)(MasaPoOska3Property, dr.GetInt32("MasaPoOska3"))
                        LoadProperty(Of Integer)(MasaPoOska4Property, dr.GetInt32("MasaPoOska4"))
                        LoadProperty(Of Integer)(MasaPoOska5Property, dr.GetInt32("MasaPoOska5"))
                        LoadProperty(Of Integer)(MasaPoOskaPriklucnaProperty, dr.GetInt32("MasaPoOskaPriklucna"))
                        LoadProperty(Of Integer)(MaxKonstOptovaruvanjeVoPriklucokProperty, dr.GetInt32("MaxKonstOptovaruvanjeVoPriklucok"))
                        LoadProperty(Of Integer)(MaxKonstVkMasaKocnaPrikolkaProperty, dr.GetInt32("MaxKonstVkMasaKocnaPrikolka"))
                        LoadProperty(Of Integer)(MaxKonstVkMasaNeKocnaPrikolkaProperty, dr.GetInt32("MaxKonstVkMasaNeKocnaPrikolka"))
                        LoadProperty(Of Decimal)(MaxLegVkMasaProperty, dr.GetValue("MaxLegVkMasa"))
                        LoadProperty(Of Decimal)(MaxLegVkMasaGrupaProperty, dr.GetValue("MaxLegVkMasaGrupa"))
                        LoadProperty(Of String)(NoiseTechnicalSpecProperty, dr.GetString("NoiseTechnicalSpec"))
                        LoadProperty(Of String)(OdnosKwCcmProperty, dr.GetString("OdnosKwCcm"))
                        LoadProperty(Of Integer)(OsnoOptovaruvanje1Property, dr.GetInt32("OsnoOptovaruvanje1"))
                        LoadProperty(Of Integer)(OsnoOptovaruvanje2Property, dr.GetInt32("OsnoOptovaruvanje2"))
                        LoadProperty(Of Integer)(OsnoOptovaruvanje3Property, dr.GetInt32("OsnoOptovaruvanje3"))
                        LoadProperty(Of Integer)(OsnoOptovaruvanje4Property, dr.GetInt32("OsnoOptovaruvanje4"))
                        LoadProperty(Of Integer)(OsnoOptovaruvanje5Property, dr.GetInt32("OsnoOptovaruvanje5"))
                        LoadProperty(Of Integer)(OsnoOptovaruvanjePriklucnaProperty, dr.GetInt32("OsnoOptovaruvanjePriklucna"))
                        LoadProperty(Of String)(OznakaNaOdobrenieProperty, dr.GetString("OznakaNaOdobrenie"))
                        LoadProperty(Of String)(OznakaNaOdobrenieZaPriklucUredProperty, dr.GetString("OznakaNaOdobrenieZaPriklucUred"))
                        LoadProperty(Of String)(TBrOdobrenieMehanPriklucokProperty, dr.GetString("TBrOdobrenieMehanPriklucok"))
                        LoadProperty(Of String)(TipProperty, dr.GetString("Tip"))
                        LoadProperty(Of String)(TMarkaMehanPriklucokProperty, dr.GetString("TMarkaMehanPriklucok"))
                        LoadProperty(Of Integer)(TMaxHorVerOptovaruvanjePriklucokProperty, dr.GetInt32("TMaxHorVerOptovaruvanjePriklucok"))
                        LoadProperty(Of Integer)(TMaxKonstVkMasaNaKombinacijaProperty, dr.GetInt32("TMaxKonstVkMasaNaKombinacija"))
                        LoadProperty(Of Integer)(TMaxKonstVkMasaPoluprikolkaProperty, dr.GetInt32("TMaxKonstVkMasaPoluprikolka"))
                        LoadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaProperty, dr.GetInt32("TMaxKonstVkMasaPrikolka"))
                        LoadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaSoCenOskaProperty, dr.GetInt32("TMaxKonstVkMasaPrikolkaSoCenOska"))
                        LoadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaStoMozePrikluciProperty, dr.GetInt32("TMaxKonstVkMasaPrikolkaStoMozePrikluci"))
                        LoadProperty(Of Integer)(TMinMasaProperty, dr.GetInt32("TMinMasa"))
                        LoadProperty(Of String)(TTipMehanPriklucokProperty, dr.GetString("TTipMehanPriklucok"))
                        LoadProperty(Of String)(TZastitnaKabinaProperty, dr.GetString("TZastitnaKabina"))
                        LoadProperty(Of String)(TZastitnaRamkaProperty, dr.GetString("TZastitnaRamka"))
                        LoadProperty(Of String)(VarijantaIzvedbaProperty, dr.GetString("VarijantaIzvedba"))
                        LoadProperty(Of Integer)(BrojNaVrteziProperty, dr.GetInt32("BrojNaVrtezi"))
                        LoadProperty(Of Decimal)(MaxKonstVkMasaProperty, dr.GetValue("MaxKonstVkMasa"))
                        LoadProperty(Of String)(BrojEUPotvrdaProperty, dr.GetString("BrojEUPotvrda"))
            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using

        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildAxes
          cm1.Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleAxes) _
            (VehicleAxesProperty, VehicleAxes.GetVehicleAxes(drc))
          End Using
        End Using

        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildAxesDestination
          cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleBetweenAxesDestinations) _
            (VehicleAxesDestinationProperty, VehicleBetweenAxesDestinations.GetVehicleBetweenAxesDestinations(drc))
          End Using
        End Using

        'Using cm1 As SqlCommand = cn.CreateCommand
        '    cm1.CommandType = CommandType.StoredProcedure
        '    cm1.CommandText = spGetChildLastTehnicalExam
        '    cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
        '    Using drc As New SafeDataReader(cm1.ExecuteReader)
        '        LoadProperty(Of VehicleLastTehnicalExams) _
        '        (VehicleLastTehnicalExamsProperty, VehicleLastTehnicalExams.GetVehicleLastTehnicalExams(drc))
        '    End Using
        'End Using

        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildrenPersonalTires
          cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleTyres) _
            (VehiclePersonalTiresProperty, VehicleTyres.GetVehicleTyres(drc))
          End Using
        End Using
        '?
        'Using cm1 As SqlCommand = cn.CreateCommand
        '    cm1.CommandType = CommandType.StoredProcedure
        '    cm1.CommandText = spGetChildrenTires
        '    cm1.Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))
        '    Using drc As New SafeDataReader(cm1.ExecuteReader)
        '        LoadProperty(Of TireTypes) _
        '        (VehicleTiresProperty, TireTypes.GetTireTypes(drc))
        '    End Using
        'End Using
        'registracii
        'Using cm1 As SqlCommand = cn.CreateCommand
        '    cm1.CommandType = CommandType.StoredProcedure
        '    cm1.CommandText = spGetChildRegistrations
        '    cm1.Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
        '    Using drc As New SafeDataReader(cm1.ExecuteReader)
        '        LoadProperty(Of VehicleRegistrations) _
        '        (VehicleRegistrationsProperty, VehicleRegistrations.GetVehicleRegistrations(drc))
        '    End Using
        'End Using
      End Using
    Catch ex As Exception

      Database.LogException("Vehicle.DataPortal_Fetch", ex)
      Throw New DbCslaException("Vehicle.DataPortal_Fetch", ex)
    End Try

  End Sub
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As filterCriteria)
    Database.LogInfo("Vehicle.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          ' cm.Connection.ConnectionTimeout(120)
          cm.CommandText = spGetByShellNumber
          cm.Parameters.AddWithValue("@shellNum", criteria.InNum)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            Try


              dr.Read()

              LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
              LoadProperty(Of Integer)(IdVehicleBodyTypeProperty, dr.GetInt32("IdVehicleBodyType"))
              LoadProperty(Of Integer)(IdVehicleCategoriesProperty, dr.GetInt32("IdVehicleCategories"))
              LoadProperty(Of Integer)(IdVehicleUseProperty, dr.GetInt32("IdVehicleUse"))
              LoadProperty(Of Integer)(IdEngineTypeProperty, dr.GetInt32("IdEngineType"))
              LoadProperty(Of Integer)(IdEnginePowerSourceProperty, dr.GetInt32("IdEnginePowerSource"))
              LoadProperty(Of Integer)(IdEngineSecondPowerSourceProperty, dr.GetInt32("IdEngineSecondPowerSource"))
              LoadProperty(Of Integer)(IdGearBoxProperty, dr.GetInt32("IdGearBox"))
              LoadProperty(Of Integer)(IdBreakesProperty, dr.GetInt32("IdBreakes"))
              LoadProperty(Of Integer)(IdSupportingProperty, dr.GetInt32("IdSupporting"))
              LoadProperty(Of Integer)(IdVehicleModelProperty, dr.GetInt32("IdVehicleModel"))
              LoadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty, dr.GetInt32("IdVehicleCategoryForPayments"))
              LoadProperty(Of Integer)(IdEngineEcoProgramProperty, dr.GetInt32("IdEngineEcoProgram"))
              LoadProperty(Of Integer)(IdMadeCountryProperty, dr.GetInt32("IdMadeCountry"))
              LoadProperty(Of String)(ColorCodeProperty, dr.GetString("ColorCode"))
              LoadProperty(Of Integer)(FirstIdRegistrationIssuerProperty, dr.GetInt32("IdFirstRegistrationIssuer"))
              LoadProperty(Of Integer)(LastIdRegistrationIssuerProperty, dr.GetInt32("IdLastRegistrationIssuer"))
              LoadProperty(Of String)(FirstRegistrationNumberProperty, dr.GetString("FirstRegistrationNumber"))
              LoadProperty(Of String)(LastRegistratinNumberProperty, dr.GetString("LastRegistratinNumber"))
              ' LoadProperty(Of String)(FirstRegistrationCommunityProperty, dr.GetString("FirstRegistrationCommunity"))
              '  LoadProperty(Of String)(LastRegistrationCommunityProperty, dr.GetString("LastRegistrationCommunity"))
              LoadProperty(Of SmartDate, Date?)(FirstRegistrationMakeDateProperty, dr.GetSmartDate("FirstRegistrationMakeDate", True))
              LoadProperty(Of SmartDate, Date?)(FirstRegistrationValidTillProperty, dr.GetSmartDate("FirstRegistrationValidTill", True))
              LoadProperty(Of SmartDate, Date?)(LastRegistrationMakeDateProperty, dr.GetSmartDate("LastRegistrationMakeDate", True))
              LoadProperty(Of SmartDate, Date?)(LastRegistrationValidTillProperty, dr.GetSmartDate("LastRegistrationValidTill", True))

              LoadProperty(Of String)(EngineNumberProperty, dr.GetString("EngineNumber"))
              LoadProperty(Of Decimal)(EnginePowerProperty, dr.GetValue("EnginePower"))
              LoadProperty(Of String)(EngineTorqueProperty, dr.GetString("EngineTorque"))
              LoadProperty(Of Decimal)(EngineTorqueUnderGassProperty, dr.GetValue("EngineTorqueUnderGass"))
              LoadProperty(Of Decimal)(EngineWorkingCapacityProperty, dr.GetValue("EngineWorkingCapacity"))
              LoadProperty(Of Decimal)(EnginePowerOutPutProperty, dr.GetValue("EnginePowerOutPut"))
              LoadProperty(Of String)(ShellNumberProperty, dr.GetString("ShellNumber"))
              LoadProperty(Of SmartDate, Date?)(MakeDateProperty, dr.GetSmartDate("MakeDate", True))
              LoadProperty(Of Integer)(NumberOfDoorsProperty, dr.GetInt32("NumberOfDoors"))
              LoadProperty(Of Short)(NumberOfSeatsProperty, dr.GetInt16("NumberOfSeats"))
              LoadProperty(Of Short)(NumberOfStandingSeatsProperty, dr.GetInt16("NumberOfStandingSeats"))
              LoadProperty(Of Short)(NumberOfLieingSeatsProperty, dr.GetInt16("NumberOfLieingSeats"))
              LoadProperty(Of Decimal)(EmptyWaightProperty, dr.GetValue("EmptyWaight"))
              LoadProperty(Of Decimal)(MaximunAllowedWaightProperty, dr.GetValue("MaximunAllowedWaight"))
              LoadProperty(Of String)(TrailerWaightWithBreakProperty, dr.GetString("TrailerWaightWithBreak"))
              LoadProperty(Of String)(TrailerWaightWithoutBreakProperty, dr.GetString("TrailerWaightWithoutBreak"))
              LoadProperty(Of Integer)(NumberOfAxisProperty, dr.GetInt32("NumberOfAxis"))
              LoadProperty(Of Integer)(PropulsionAxisProperty, dr.GetInt32("PropulsionAxis"))
              LoadProperty(Of Integer)(NumberOfWheelsProperty, dr.GetInt32("NumberOfWheels"))
              LoadProperty(Of Integer)(NumberOfPropulsionWheelsProperty, dr.GetInt32("NumberOfPropulsionWheels"))
              LoadProperty(Of Decimal)(VehicleSizeHightProperty, dr.GetValue("VehicleSizeHight"))
              LoadProperty(Of Decimal)(VehicleSizeWidthProperty, dr.GetValue("VehicleSizeWidth"))
              LoadProperty(Of Decimal)(VehicleSizeLengthProperty, dr.GetValue("VehicleSizeLength"))
              LoadProperty(Of Boolean)(SuffocationProperty, dr.GetBoolean("Suffocation"))
              LoadProperty(Of Boolean)(HookProperty, dr.GetBoolean("Hook"))
              LoadProperty(Of Boolean)(VitloProperty, dr.GetBoolean("Vitlo"))
              LoadProperty(Of Boolean)(VerticalBurdenOnTheSeatProperty, dr.GetBoolean("VerticalBurdenOnTheSeat"))
              LoadProperty(Of String)(VerticalBurdenOnTheSeatNoteProperty, dr.GetString("VerticalBurdenOnTheSeatNote"))
              LoadProperty(Of Integer)(IdPrimaryColorProperty, dr.GetInt32("IdPrimaryColor"))
              LoadProperty(Of Integer)(IdSecondaryColorProperty, dr.GetInt32("IdSecondaryColor"))
              LoadProperty(Of String)(HologationSertificateNumberProperty, dr.GetString("HologationSertificateNumber"))
              LoadProperty(Of Decimal)(NoiseStaticProperty, dr.GetValue("NoiseStatic"))
              LoadProperty(Of Decimal)(NoiseMovmentProperty, dr.GetValue("NoiseMovment"))
              LoadProperty(Of Decimal)(CoProperty, dr.GetValue("CO"))
              LoadProperty(Of Decimal)(HcProperty, dr.GetValue("HC"))
              LoadProperty(Of Decimal)(NOxProperty, dr.GetValue("NOx"))
              LoadProperty(Of Decimal)(HCNOxProperty, dr.GetValue("HCNOx"))
              LoadProperty(Of String)(BlackeningProperty, dr.GetString("Blackening"))
              LoadProperty(Of String)(PinpointsProperty, dr.GetString("Pinpoints"))
              LoadProperty(Of Decimal)(Co2Property, dr.GetValue("CO2"))
              LoadProperty(Of String)(FuelConsumptionProperty, dr.GetString("FuelConsumption"))
              LoadProperty(Of Decimal)(CapacityFuelTankProperty, dr.GetValue("CapacityFuelTank"))
              LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
              LoadProperty(Of Boolean)(IsSocialNotPrivateProperty, dr.GetBoolean("IsSocialNotPrivate"))
              LoadProperty(Of Boolean)(ForPrivateTransportNotPublicProperty, dr.GetBoolean("ForPrivateTransportNotPublic"))
              LoadProperty(Of Boolean)(TNGProperty, dr.GetBoolean("TNG"))
              LoadProperty(Of String)(VehicleModelAddingProperty, dr.GetString("VehicleModelAdding"))
              LoadProperty(Of Decimal)(MaxSpeedProperty, dr.GetValue("MaxSpeed"))
                            LoadProperty(Of Decimal)(TempOfEngineOilProperty, dr.GetValue("TempOfEngineOil"))
                            LoadProperty(Of String)(IdentifikacijaNaMotorMestoMetodProperty, dr.GetString("IdentifikacijaNaMotorMestoMetod"))
                            LoadProperty(Of Integer)(MasaPoOska1Property, dr.GetInt32("MasaPoOska1"))
                            LoadProperty(Of Integer)(MasaPoOska2Property, dr.GetInt32("MasaPoOska2"))
                            LoadProperty(Of Integer)(MasaPoOska3Property, dr.GetInt32("MasaPoOska3"))
                            LoadProperty(Of Integer)(MasaPoOska4Property, dr.GetInt32("MasaPoOska4"))
                            LoadProperty(Of Integer)(MasaPoOska5Property, dr.GetInt32("MasaPoOska5"))
                            LoadProperty(Of Integer)(MasaPoOskaPriklucnaProperty, dr.GetInt32("MasaPoOskaPriklucna"))
                            LoadProperty(Of Integer)(MaxKonstOptovaruvanjeVoPriklucokProperty, dr.GetInt32("MaxKonstOptovaruvanjeVoPriklucok"))
                            LoadProperty(Of Integer)(MaxKonstVkMasaKocnaPrikolkaProperty, dr.GetInt32("MaxKonstVkMasaKocnaPrikolka"))
                            LoadProperty(Of Integer)(MaxKonstVkMasaNeKocnaPrikolkaProperty, dr.GetInt32("MaxKonstVkMasaNeKocnaPrikolka"))
                            LoadProperty(Of Decimal)(MaxLegVkMasaProperty, dr.GetValue("MaxLegVkMasa"))
                            LoadProperty(Of Decimal)(MaxLegVkMasaGrupaProperty, dr.GetValue("MaxLegVkMasaGrupa"))
                            LoadProperty(Of String)(NoiseTechnicalSpecProperty, dr.GetString("NoiseTechnicalSpec"))
                            LoadProperty(Of String)(OdnosKwCcmProperty, dr.GetString("OdnosKwCcm"))
                            LoadProperty(Of Integer)(OsnoOptovaruvanje1Property, dr.GetInt32("OsnoOptovaruvanje1"))
                            LoadProperty(Of Integer)(OsnoOptovaruvanje2Property, dr.GetInt32("OsnoOptovaruvanje2"))
                            LoadProperty(Of Integer)(OsnoOptovaruvanje3Property, dr.GetInt32("OsnoOptovaruvanje3"))
                            LoadProperty(Of Integer)(OsnoOptovaruvanje4Property, dr.GetInt32("OsnoOptovaruvanje4"))
                            LoadProperty(Of Integer)(OsnoOptovaruvanje5Property, dr.GetInt32("OsnoOptovaruvanje5"))
                            LoadProperty(Of Integer)(OsnoOptovaruvanjePriklucnaProperty, dr.GetInt32("OsnoOptovaruvanjePriklucna"))
                            LoadProperty(Of String)(OznakaNaOdobrenieProperty, dr.GetString("OznakaNaOdobrenie"))
                            LoadProperty(Of String)(OznakaNaOdobrenieZaPriklucUredProperty, dr.GetString("OznakaNaOdobrenieZaPriklucUred"))
                            LoadProperty(Of String)(TBrOdobrenieMehanPriklucokProperty, dr.GetString("TBrOdobrenieMehanPriklucok"))
                            LoadProperty(Of String)(TipProperty, dr.GetString("Tip"))
                            LoadProperty(Of String)(TMarkaMehanPriklucokProperty, dr.GetString("TMarkaMehanPriklucok"))
                            LoadProperty(Of Integer)(TMaxHorVerOptovaruvanjePriklucokProperty, dr.GetInt32("TMaxHorVerOptovaruvanjePriklucok"))
                            LoadProperty(Of Integer)(TMaxKonstVkMasaNaKombinacijaProperty, dr.GetInt32("TMaxKonstVkMasaNaKombinacija"))
                            LoadProperty(Of Integer)(TMaxKonstVkMasaPoluprikolkaProperty, dr.GetInt32("TMaxKonstVkMasaPoluprikolka"))
                            LoadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaProperty, dr.GetInt32("TMaxKonstVkMasaPrikolka"))
                            LoadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaSoCenOskaProperty, dr.GetInt32("TMaxKonstVkMasaPrikolkaSoCenOska"))
                            LoadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaStoMozePrikluciProperty, dr.GetInt32("TMaxKonstVkMasaPrikolkaStoMozePrikluci"))
                            LoadProperty(Of Integer)(TMinMasaProperty, dr.GetInt32("TMinMasa"))
                            LoadProperty(Of String)(TTipMehanPriklucokProperty, dr.GetString("TTipMehanPriklucok"))
                            LoadProperty(Of String)(TZastitnaKabinaProperty, dr.GetString("TZastitnaKabina"))
                            LoadProperty(Of String)(TZastitnaRamkaProperty, dr.GetString("TZastitnaRamka"))
                            LoadProperty(Of String)(VarijantaIzvedbaProperty, dr.GetString("VarijantaIzvedba"))
                            LoadProperty(Of Integer)(BrojNaVrteziProperty, dr.GetInt32("BrojNaVrtezi"))
                            LoadProperty(Of Decimal)(MaxKonstVkMasaProperty, dr.GetValue("MaxKonstVkMasa"))
                            LoadProperty(Of String)(BrojEUPotvrdaProperty, dr.GetString("BrojEUPotvrda"))
              'LoadProperty(Of Integer)(IdVehicleAdaptationProperty, dr.GetInt32("IdVehicleAdaptation"))
              ' LoadProperty(Of Integer)(IdGarageTypeProperty, dr.GetInt32("IdVehicleGarageType"))
              'LoadProperty(Of Boolean)(RadioStationProperty, dr.GetBoolean("RadioStation"))
              dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
            Catch ex As Exception
              Exit Sub
            End Try
          End Using

        End Using

        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildAxes
          cm1.Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleAxes) _
            (VehicleAxesProperty, VehicleAxes.GetVehicleAxes(drc))
          End Using
        End Using

        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildAxesDestination
          cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleBetweenAxesDestinations) _
            (VehicleAxesDestinationProperty, VehicleBetweenAxesDestinations.GetVehicleBetweenAxesDestinations(drc))
          End Using
        End Using

        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildrenPersonalTires
          cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleTyres) _
            (VehiclePersonalTiresProperty, VehicleTyres.GetVehicleTyres(drc))
          End Using
        End Using

      End Using
    Catch ex As Exception

      Database.LogException("Vehicle.DataPortal_Fetch", ex)
      Throw New DbCslaException("Vehicle.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@IdVehicleBodyType", ReadProperty(Of Integer)(IdVehicleBodyTypeProperty))
            .Parameters.AddWithValue("@IdVehicleCategories", ReadProperty(Of Integer)(IdVehicleCategoriesProperty))
            .Parameters.AddWithValue("@IdVehicleUse", ReadProperty(Of Integer)(IdVehicleUseProperty))
            .Parameters.AddWithValue("@IdEngineType", ReadProperty(Of Integer)(IdEngineTypeProperty))
            .Parameters.AddWithValue("@IdEnginePowerSource", ReadProperty(Of Integer)(IdEnginePowerSourceProperty))
            .Parameters.AddWithValue("@IdEngineSecondPowerSource", ReadProperty(Of Integer)(IdEngineSecondPowerSourceProperty))
            ' .Parameters.AddWithValue("@IdEngineMark", ReadProperty(Of Integer)(IdEngineMarkProperty))
            .Parameters.AddWithValue("@IdGearBox", ReadProperty(Of Integer)(IdGearBoxProperty))
            .Parameters.AddWithValue("@IdBreakes", ReadProperty(Of Integer)(IdBreakesProperty))
            .Parameters.AddWithValue("@IdSupporting", ReadProperty(Of Integer)(IdSupportingProperty))
            .Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))
            .Parameters.AddWithValue("@IdVehicleCategoryForPayments", ReadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty))
            .Parameters.AddWithValue("@IdEngineEcoProgram", ReadProperty(Of Integer)(IdEngineEcoProgramProperty))
            .Parameters.AddWithValue("@IdMadeCountry", ReadProperty(Of Integer)(IdMadeCountryProperty))
            .Parameters.AddWithValue("@ColorCode", ReadProperty(Of String)(ColorCodeProperty))

            .Parameters.AddWithValue("@FirstRegistrationNumber", ReadProperty(Of String)(FirstRegistrationNumberProperty))
            .Parameters.AddWithValue("@LastRegistratinNumber", ReadProperty(Of String)(LastRegistratinNumberProperty))
            '  .Parameters.AddWithValue("@FirstRegistrationCommunity", ReadProperty(Of String)(FirstRegistrationCommunityProperty))
            '  .Parameters.AddWithValue("@LastRegistrationCommunity", ReadProperty(Of String)(LastRegistrationCommunityProperty))
            .Parameters.AddWithValue("@FirstRegistrationMakeDate", ReadProperty(Of SmartDate)(FirstRegistrationMakeDateProperty).DBValue)
            .Parameters.AddWithValue("@FirstRegistrationValidTill", ReadProperty(Of SmartDate)(FirstRegistrationValidTillProperty).DBValue)
            .Parameters.AddWithValue("@LastRegistrationMakeDate", ReadProperty(Of SmartDate)(LastRegistrationMakeDateProperty).DBValue)
            .Parameters.AddWithValue("@LastRegistrationValidTill", ReadProperty(Of SmartDate)(LastRegistrationValidTillProperty).DBValue)
            .Parameters.AddWithValue("@FirstIdRegistrationIssuer", ReadProperty(Of Integer)(FirstIdRegistrationIssuerProperty))
            .Parameters.AddWithValue("@IdLastRegistrationIssuer", ReadProperty(Of Integer)(LastIdRegistrationIssuerProperty))

            .Parameters.AddWithValue("@EngineNumber", ReadProperty(Of String)(EngineNumberProperty))
            .Parameters.AddWithValue("@EnginePower", ReadProperty(Of Decimal)(EnginePowerProperty))
            .Parameters.AddWithValue("@EngineTorque", ReadProperty(Of String)(EngineTorqueProperty))
            .Parameters.AddWithValue("@EngineTorqueUnderGass", ReadProperty(Of Decimal)(EngineTorqueUnderGassProperty))
            .Parameters.AddWithValue("@EngineWorkingCapacity", ReadProperty(Of Decimal)(EngineWorkingCapacityProperty))
            .Parameters.AddWithValue("@EnginePowerOutPut", ReadProperty(Of Decimal)(EnginePowerOutPutProperty))
            .Parameters.AddWithValue("@ShellNumber", ReadProperty(Of String)(ShellNumberProperty))
            .Parameters.AddWithValue("@MakeDate", ReadProperty(Of SmartDate)(MakeDateProperty).DBValue)
            .Parameters.AddWithValue("@NumberOfDoors", ReadProperty(Of Integer)(NumberOfDoorsProperty))
            .Parameters.AddWithValue("@NumberOfSeats", ReadProperty(Of Short)(NumberOfSeatsProperty))
            .Parameters.AddWithValue("@NumberOfStandingSeats", ReadProperty(Of Short)(NumberOfStandingSeatsProperty))
            .Parameters.AddWithValue("@NumberOfLieingSeats", ReadProperty(Of Short)(NumberOfLieingSeatsProperty))
            .Parameters.AddWithValue("@EmptyWaight", ReadProperty(Of Decimal)(EmptyWaightProperty))
            .Parameters.AddWithValue("@MaximunAllowedWaight", ReadProperty(Of Decimal)(MaximunAllowedWaightProperty))
            .Parameters.AddWithValue("@TrailerWaightWithBreak", ReadProperty(Of String)(TrailerWaightWithBreakProperty))
            .Parameters.AddWithValue("@TrailerWaightWithoutBreak", ReadProperty(Of String)(TrailerWaightWithoutBreakProperty))
            .Parameters.AddWithValue("@NumberOfAxis", ReadProperty(Of Integer)(NumberOfAxisProperty))
            .Parameters.AddWithValue("@PropulsionAxis", ReadProperty(Of Integer)(PropulsionAxisProperty))
            .Parameters.AddWithValue("@NumberOfWheels", ReadProperty(Of Integer)(NumberOfWheelsProperty))
            .Parameters.AddWithValue("@NumberOfPropulsionWheels", ReadProperty(Of Integer)(NumberOfPropulsionWheelsProperty))
            .Parameters.AddWithValue("@VehicleSizeHight", ReadProperty(Of Decimal)(VehicleSizeHightProperty))
            .Parameters.AddWithValue("@VehicleSizeWidth", ReadProperty(Of Decimal)(VehicleSizeWidthProperty))
            .Parameters.AddWithValue("@VehicleSizeLength", ReadProperty(Of Decimal)(VehicleSizeLengthProperty))
            .Parameters.AddWithValue("@Suffocation", ReadProperty(Of Boolean)(SuffocationProperty))
            .Parameters.AddWithValue("@Hook", ReadProperty(Of Boolean)(HookProperty))
            .Parameters.AddWithValue("@Vitlo", ReadProperty(Of Boolean)(VitloProperty))
            .Parameters.AddWithValue("@VerticalBurdenOnTheSeat", ReadProperty(Of Boolean)(VerticalBurdenOnTheSeatProperty))
            .Parameters.AddWithValue("@VerticalBurdenOnTheSeatNote", ReadProperty(Of String)(VerticalBurdenOnTheSeatNoteProperty))
            .Parameters.AddWithValue("@IdPrimaryColor", ReadProperty(Of Integer)(IdPrimaryColorProperty))
            .Parameters.AddWithValue("@IdSecondaryColor", ReadProperty(Of Integer)(IdSecondaryColorProperty))
            .Parameters.AddWithValue("@HologationSertificateNumber", ReadProperty(Of String)(HologationSertificateNumberProperty))
            .Parameters.AddWithValue("@NoiseStatic", ReadProperty(Of Decimal)(NoiseStaticProperty))
            .Parameters.AddWithValue("@NoiseMovment", ReadProperty(Of Decimal)(NoiseMovmentProperty))
            .Parameters.AddWithValue("@CO", ReadProperty(Of Decimal)(CoProperty))
            .Parameters.AddWithValue("@HC", ReadProperty(Of Decimal)(HcProperty))
            .Parameters.AddWithValue("@NOx", ReadProperty(Of Decimal)(NOxProperty))
            .Parameters.AddWithValue("@HCNOx", ReadProperty(Of Decimal)(HCNOxProperty))
            .Parameters.AddWithValue("@Blackening", ReadProperty(Of String)(BlackeningProperty))
            .Parameters.AddWithValue("@Pinpoints", ReadProperty(Of String)(PinpointsProperty))
            .Parameters.AddWithValue("@CO2", ReadProperty(Of Decimal)(Co2Property))
            .Parameters.AddWithValue("@FuelConsumption", ReadProperty(Of String)(FuelConsumptionProperty))
            .Parameters.AddWithValue("@CapacityFuelTank", ReadProperty(Of Decimal)(CapacityFuelTankProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
            .Parameters.AddWithValue("@IsSocialNotPrivate", ReadProperty(Of Boolean)(IsSocialNotPrivateProperty))
            .Parameters.AddWithValue("@ForPrivateTransportNotPublic", ReadProperty(Of Boolean)(ForPrivateTransportNotPublicProperty))
            .Parameters.AddWithValue("@TNG", ReadProperty(Of Boolean)(TNGProperty))
            .Parameters.AddWithValue("@VehicleModelAdding", ReadProperty(Of String)(VehicleModelAddingProperty))
            .Parameters.AddWithValue("@MaxSpeed", ReadProperty(Of Decimal)(MaxSpeedProperty))
            .Parameters.AddWithValue("@TempOfEngineOil", ReadProperty(Of Decimal)(TempOfEngineOilProperty))
                        .Parameters.AddWithValue("@IdentifikacijaNaMotorMestoMetod", ReadProperty(Of String)(IdentifikacijaNaMotorMestoMetodProperty))
                        .Parameters.AddWithValue("@MasaPoOska1", ReadProperty(Of Integer)(MasaPoOska1Property))
                        .Parameters.AddWithValue("@MasaPoOska2", ReadProperty(Of Integer)(MasaPoOska2Property))
                        .Parameters.AddWithValue("@MasaPoOska3", ReadProperty(Of Integer)(MasaPoOska3Property))
                        .Parameters.AddWithValue("@MasaPoOska4", ReadProperty(Of Integer)(MasaPoOska4Property))
                        .Parameters.AddWithValue("@MasaPoOska5", ReadProperty(Of Integer)(MasaPoOska5Property))
                        .Parameters.AddWithValue("@MasaPoOskaPriklucna", ReadProperty(Of Integer)(MasaPoOskaPriklucnaProperty))

                        .Parameters.AddWithValue("@MaxKonstOptovaruvanjeVoPriklucok", ReadProperty(Of Integer)(MaxKonstOptovaruvanjeVoPriklucokProperty))
                        .Parameters.AddWithValue("@MaxKonstVkMasaKocnaPrikolka", ReadProperty(Of Integer)(MaxKonstVkMasaKocnaPrikolkaProperty))
                        .Parameters.AddWithValue("@MaxKonstVkMasaNeKocnaPrikolka", ReadProperty(Of Integer)(MaxKonstVkMasaNeKocnaPrikolkaProperty))
                        .Parameters.AddWithValue("@MaxLegVkMasa", ReadProperty(Of Decimal)(MaxLegVkMasaProperty))
                        .Parameters.AddWithValue("@MaxLegVkMasaGrupa", ReadProperty(Of Decimal)(MaxLegVkMasaGrupaProperty))
                        .Parameters.AddWithValue("@NoiseTechnicalSpec", ReadProperty(Of String)(NoiseTechnicalSpecProperty))
                        .Parameters.AddWithValue("@OdnosKwCcm", ReadProperty(Of String)(OdnosKwCcmProperty))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje1", ReadProperty(Of Integer)(OsnoOptovaruvanje1Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje2", ReadProperty(Of Integer)(OsnoOptovaruvanje2Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje3", ReadProperty(Of Integer)(OsnoOptovaruvanje3Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje4", ReadProperty(Of Integer)(OsnoOptovaruvanje4Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje5", ReadProperty(Of Integer)(OsnoOptovaruvanje5Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanjePriklucna", ReadProperty(Of Integer)(OsnoOptovaruvanjePriklucnaProperty))
                        .Parameters.AddWithValue("@OznakaNaOdobrenie", ReadProperty(Of String)(OznakaNaOdobrenieProperty))
                        .Parameters.AddWithValue("@OznakaNaOdobrenieZaPriklucUred", ReadProperty(Of String)(OznakaNaOdobrenieZaPriklucUredProperty))
                        .Parameters.AddWithValue("@TBrOdobrenieMehanPriklucok", ReadProperty(Of String)(TBrOdobrenieMehanPriklucokProperty))
                        .Parameters.AddWithValue("@Tip", ReadProperty(Of String)(TipProperty))
                        .Parameters.AddWithValue("@TMarkaMehanPriklucok", ReadProperty(Of String)(TMarkaMehanPriklucokProperty))
                        .Parameters.AddWithValue("@TMaxHorVerOptovaruvanjePriklucok", ReadProperty(Of Integer)(TMaxHorVerOptovaruvanjePriklucokProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaNaKombinacija", ReadProperty(Of Integer)(TMaxKonstVkMasaNaKombinacijaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPoluprikolka", ReadProperty(Of Integer)(TMaxKonstVkMasaPoluprikolkaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPrikolka", ReadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPrikolkaSoCenOska", ReadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaSoCenOskaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPrikolkaStoMozePrikluci", ReadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaStoMozePrikluciProperty))
                        .Parameters.AddWithValue("@TMinMasa", ReadProperty(Of Integer)(TMinMasaProperty))
                        .Parameters.AddWithValue("@TTipMehanPriklucok", ReadProperty(Of String)(TTipMehanPriklucokProperty))
                        .Parameters.AddWithValue("@TZastitnaKabina", ReadProperty(Of String)(TZastitnaKabinaProperty))
                        .Parameters.AddWithValue("@TZastitnaRamka", ReadProperty(Of String)(TZastitnaRamkaProperty))
                        .Parameters.AddWithValue("@VarijantaIzvedba", ReadProperty(Of String)(VarijantaIzvedbaProperty))
                        .Parameters.AddWithValue("@BrojNaVrtezi", ReadProperty(Of Integer)(BrojNaVrteziProperty))
                        .Parameters.AddWithValue("@MaxKonstVkMasa", ReadProperty(Of Decimal)(MaxKonstVkMasaProperty))
                        .Parameters.AddWithValue("@BrojEUPotvrda", ReadProperty(Of String)(BrojEUPotvrdaProperty))
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
      Database.LogException("Vehicle.DataPortal_Insert", ex)
      Throw New DbCslaException("Vehicle.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("Vehicle.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@IdVehicleBodyType", ReadProperty(Of Integer)(IdVehicleBodyTypeProperty))
            .Parameters.AddWithValue("@IdVehicleCategories", ReadProperty(Of Integer)(IdVehicleCategoriesProperty))
            .Parameters.AddWithValue("@IdVehicleUse", ReadProperty(Of Integer)(IdVehicleUseProperty))
            .Parameters.AddWithValue("@IdEngineType", ReadProperty(Of Integer)(IdEngineTypeProperty))
            .Parameters.AddWithValue("@IdEnginePowerSource", ReadProperty(Of Integer)(IdEnginePowerSourceProperty))
            .Parameters.AddWithValue("@IdEngineSecondPowerSource", ReadProperty(Of Integer)(IdEngineSecondPowerSourceProperty))
            '.Parameters.AddWithValue("@IdEngineMark", ReadProperty(Of Integer)(IdEngineMarkProperty))
            .Parameters.AddWithValue("@IdGearBox", ReadProperty(Of Integer)(IdGearBoxProperty))
            .Parameters.AddWithValue("@IdBreakes", ReadProperty(Of Integer)(IdBreakesProperty))
            .Parameters.AddWithValue("@IdSupporting", ReadProperty(Of Integer)(IdSupportingProperty))
            .Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))
            .Parameters.AddWithValue("@IdVehicleCategoryForPayments", ReadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty))
            .Parameters.AddWithValue("@IdEngineEcoProgram", ReadProperty(Of Integer)(IdEngineEcoProgramProperty))
            .Parameters.AddWithValue("@IdMadeCountry", ReadProperty(Of Integer)(IdMadeCountryProperty))
            .Parameters.AddWithValue("@ColorCode", ReadProperty(Of String)(ColorCodeProperty))

            .Parameters.AddWithValue("@FirstRegistrationNumber", ReadProperty(Of String)(FirstRegistrationNumberProperty))
            .Parameters.AddWithValue("@LastRegistratinNumber", ReadProperty(Of String)(LastRegistratinNumberProperty))
            ' .Parameters.AddWithValue("@FirstRegistrationCommunity", ReadProperty(Of String)(FirstRegistrationCommunityProperty))
            '  .Parameters.AddWithValue("@LastRegistrationCommunity", ReadProperty(Of String)(LastRegistrationCommunityProperty))
            .Parameters.AddWithValue("@FirstRegistrationMakeDate", ReadProperty(Of SmartDate)(FirstRegistrationMakeDateProperty).DBValue)
            .Parameters.AddWithValue("@FirstRegistrationValidTill", ReadProperty(Of SmartDate)(FirstRegistrationValidTillProperty).DBValue)
            .Parameters.AddWithValue("@LastRegistrationMakeDate", ReadProperty(Of SmartDate)(LastRegistrationMakeDateProperty).DBValue)
            .Parameters.AddWithValue("@LastRegistrationValidTill", ReadProperty(Of SmartDate)(LastRegistrationValidTillProperty).DBValue)
            .Parameters.AddWithValue("@FirstIdRegistrationIssuer", ReadProperty(Of Integer)(FirstIdRegistrationIssuerProperty))
            .Parameters.AddWithValue("@IdLastRegistrationIssuer", ReadProperty(Of Integer)(LastIdRegistrationIssuerProperty))

            .Parameters.AddWithValue("@EngineNumber", ReadProperty(Of String)(EngineNumberProperty))
            .Parameters.AddWithValue("@EnginePower", ReadProperty(Of Decimal)(EnginePowerProperty))
            .Parameters.AddWithValue("@EngineTorque", ReadProperty(Of String)(EngineTorqueProperty))
            .Parameters.AddWithValue("@EngineTorqueUnderGass", ReadProperty(Of Decimal)(EngineTorqueUnderGassProperty))
            .Parameters.AddWithValue("@EngineWorkingCapacity", ReadProperty(Of Decimal)(EngineWorkingCapacityProperty))
            .Parameters.AddWithValue("@EnginePowerOutPut", ReadProperty(Of Decimal)(EnginePowerOutPutProperty))
            .Parameters.AddWithValue("@ShellNumber", ReadProperty(Of String)(ShellNumberProperty))
            .Parameters.AddWithValue("@MakeDate", ReadProperty(Of SmartDate)(MakeDateProperty).DBValue)
            .Parameters.AddWithValue("@NumberOfDoors", ReadProperty(Of Integer)(NumberOfDoorsProperty))
            .Parameters.AddWithValue("@NumberOfSeats", ReadProperty(Of Short)(NumberOfSeatsProperty))
            .Parameters.AddWithValue("@NumberOfStandingSeats", ReadProperty(Of Short)(NumberOfStandingSeatsProperty))
            .Parameters.AddWithValue("@NumberOfLieingSeats", ReadProperty(Of Short)(NumberOfLieingSeatsProperty))
            .Parameters.AddWithValue("@EmptyWaight", ReadProperty(Of Decimal)(EmptyWaightProperty))
            .Parameters.AddWithValue("@MaximunAllowedWaight", ReadProperty(Of Decimal)(MaximunAllowedWaightProperty))
            .Parameters.AddWithValue("@TrailerWaightWithBreak", ReadProperty(Of String)(TrailerWaightWithBreakProperty))
            .Parameters.AddWithValue("@TrailerWaightWithoutBreak", ReadProperty(Of String)(TrailerWaightWithoutBreakProperty))
            .Parameters.AddWithValue("@NumberOfAxis", ReadProperty(Of Integer)(NumberOfAxisProperty))
            .Parameters.AddWithValue("@PropulsionAxis", ReadProperty(Of Integer)(PropulsionAxisProperty))
            .Parameters.AddWithValue("@NumberOfWheels", ReadProperty(Of Integer)(NumberOfWheelsProperty))
            .Parameters.AddWithValue("@NumberOfPropulsionWheels", ReadProperty(Of Integer)(NumberOfPropulsionWheelsProperty))
            .Parameters.AddWithValue("@VehicleSizeHight", ReadProperty(Of Decimal)(VehicleSizeHightProperty))
            .Parameters.AddWithValue("@VehicleSizeWidth", ReadProperty(Of Decimal)(VehicleSizeWidthProperty))
            .Parameters.AddWithValue("@VehicleSizeLength", ReadProperty(Of Decimal)(VehicleSizeLengthProperty))
            .Parameters.AddWithValue("@Suffocation", ReadProperty(Of Boolean)(SuffocationProperty))
            .Parameters.AddWithValue("@Hook", ReadProperty(Of Boolean)(HookProperty))
            .Parameters.AddWithValue("@Vitlo", ReadProperty(Of Boolean)(VitloProperty))
            .Parameters.AddWithValue("@VerticalBurdenOnTheSeat", ReadProperty(Of Boolean)(VerticalBurdenOnTheSeatProperty))
            .Parameters.AddWithValue("@VerticalBurdenOnTheSeatNote", ReadProperty(Of String)(VerticalBurdenOnTheSeatNoteProperty))
            .Parameters.AddWithValue("@IdPrimaryColor", ReadProperty(Of Integer)(IdPrimaryColorProperty))
            .Parameters.AddWithValue("@IdSecondaryColor", ReadProperty(Of Integer)(IdSecondaryColorProperty))
            .Parameters.AddWithValue("@HologationSertificateNumber", ReadProperty(Of String)(HologationSertificateNumberProperty))
            .Parameters.AddWithValue("@NoiseStatic", ReadProperty(Of Decimal)(NoiseStaticProperty))
            .Parameters.AddWithValue("@NoiseMovment", ReadProperty(Of Decimal)(NoiseMovmentProperty))
            .Parameters.AddWithValue("@CO", ReadProperty(Of Decimal)(CoProperty))
            .Parameters.AddWithValue("@HC", ReadProperty(Of Decimal)(HcProperty))
            .Parameters.AddWithValue("@NOx", ReadProperty(Of Decimal)(NOxProperty))
            .Parameters.AddWithValue("@HCNOx", ReadProperty(Of Decimal)(HCNOxProperty))
            .Parameters.AddWithValue("@Blackening", ReadProperty(Of String)(BlackeningProperty))
            .Parameters.AddWithValue("@Pinpoints", ReadProperty(Of String)(PinpointsProperty))
            .Parameters.AddWithValue("@CO2", ReadProperty(Of Decimal)(Co2Property))
            .Parameters.AddWithValue("@FuelConsumption", ReadProperty(Of String)(FuelConsumptionProperty))
            .Parameters.AddWithValue("@CapacityFuelTank", ReadProperty(Of Decimal)(CapacityFuelTankProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
            .Parameters.AddWithValue("@IsSocialNotPrivate", ReadProperty(Of Boolean)(IsSocialNotPrivateProperty))
            .Parameters.AddWithValue("@ForPrivateTransportNotPublic", ReadProperty(Of Boolean)(ForPrivateTransportNotPublicProperty))
            .Parameters.AddWithValue("@TNG", ReadProperty(Of Boolean)(TNGProperty))
            .Parameters.AddWithValue("@VehicleModelAdding", ReadProperty(Of String)(VehicleModelAddingProperty))
            .Parameters.AddWithValue("@MaxSpeed", ReadProperty(Of Decimal)(MaxSpeedProperty))
            .Parameters.AddWithValue("@TempOfEngineOil", ReadProperty(Of Decimal)(TempOfEngineOilProperty))
                        .Parameters.AddWithValue("@IdentifikacijaNaMotorMestoMetod", ReadProperty(Of String)(IdentifikacijaNaMotorMestoMetodProperty))
                        .Parameters.AddWithValue("@MasaPoOska1", ReadProperty(Of Integer)(MasaPoOska1Property))
                        .Parameters.AddWithValue("@MasaPoOska2", ReadProperty(Of Integer)(MasaPoOska2Property))
                        .Parameters.AddWithValue("@MasaPoOska3", ReadProperty(Of Integer)(MasaPoOska3Property))
                        .Parameters.AddWithValue("@MasaPoOska4", ReadProperty(Of Integer)(MasaPoOska4Property))
                        .Parameters.AddWithValue("@MasaPoOska5", ReadProperty(Of Integer)(MasaPoOska5Property))
                        .Parameters.AddWithValue("@MasaPoOskaPriklucna", ReadProperty(Of Integer)(MasaPoOskaPriklucnaProperty))
                        .Parameters.AddWithValue("@MaxKonstOptovaruvanjeVoPriklucok", ReadProperty(Of Integer)(MaxKonstOptovaruvanjeVoPriklucokProperty))
                        .Parameters.AddWithValue("@MaxKonstVkMasaKocnaPrikolka", ReadProperty(Of Integer)(MaxKonstVkMasaKocnaPrikolkaProperty))
                        .Parameters.AddWithValue("@MaxKonstVkMasaNeKocnaPrikolka", ReadProperty(Of Integer)(MaxKonstVkMasaNeKocnaPrikolkaProperty))
                        .Parameters.AddWithValue("@MaxLegVkMasa", ReadProperty(Of Decimal)(MaxLegVkMasaProperty))
                        .Parameters.AddWithValue("@MaxLegVkMasaGrupa", ReadProperty(Of Decimal)(MaxLegVkMasaGrupaProperty))
                        .Parameters.AddWithValue("@NoiseTechnicalSpec", ReadProperty(Of String)(NoiseTechnicalSpecProperty))
                        .Parameters.AddWithValue("@OdnosKwCcm", ReadProperty(Of String)(OdnosKwCcmProperty))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje1", ReadProperty(Of Integer)(OsnoOptovaruvanje1Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje2", ReadProperty(Of Integer)(OsnoOptovaruvanje2Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje3", ReadProperty(Of Integer)(OsnoOptovaruvanje3Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje4", ReadProperty(Of Integer)(OsnoOptovaruvanje4Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje5", ReadProperty(Of Integer)(OsnoOptovaruvanje5Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanjePriklucna", ReadProperty(Of Integer)(OsnoOptovaruvanjePriklucnaProperty))
                        .Parameters.AddWithValue("@OznakaNaOdobrenie", ReadProperty(Of String)(OznakaNaOdobrenieProperty))
                        .Parameters.AddWithValue("@OznakaNaOdobrenieZaPriklucUred", ReadProperty(Of String)(OznakaNaOdobrenieZaPriklucUredProperty))
                        .Parameters.AddWithValue("@TBrOdobrenieMehanPriklucok", ReadProperty(Of String)(TBrOdobrenieMehanPriklucokProperty))
                        .Parameters.AddWithValue("@Tip", ReadProperty(Of String)(TipProperty))
                        .Parameters.AddWithValue("@TMarkaMehanPriklucok", ReadProperty(Of String)(TMarkaMehanPriklucokProperty))
                        .Parameters.AddWithValue("@TMaxHorVerOptovaruvanjePriklucok", ReadProperty(Of Integer)(TMaxHorVerOptovaruvanjePriklucokProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaNaKombinacija", ReadProperty(Of Integer)(TMaxKonstVkMasaNaKombinacijaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPoluprikolka", ReadProperty(Of Integer)(TMaxKonstVkMasaPoluprikolkaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPrikolka", ReadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPrikolkaSoCenOska", ReadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaSoCenOskaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPrikolkaStoMozePrikluci", ReadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaStoMozePrikluciProperty))
                        .Parameters.AddWithValue("@TMinMasa", ReadProperty(Of Integer)(TMinMasaProperty))
                        .Parameters.AddWithValue("@TTipMehanPriklucok", ReadProperty(Of String)(TTipMehanPriklucokProperty))
                        .Parameters.AddWithValue("@TZastitnaKabina", ReadProperty(Of String)(TZastitnaKabinaProperty))
                        .Parameters.AddWithValue("@TZastitnaRamka", ReadProperty(Of String)(TZastitnaRamkaProperty))
                        .Parameters.AddWithValue("@VarijantaIzvedba", ReadProperty(Of String)(VarijantaIzvedbaProperty))
                        .Parameters.AddWithValue("@BrojNaVrtezi", ReadProperty(Of Integer)(BrojNaVrteziProperty))
                        .Parameters.AddWithValue("@MaxKonstVkMasa", ReadProperty(Of Decimal)(MaxKonstVkMasaProperty))
                        .Parameters.AddWithValue("@BrojEUPotvrda", ReadProperty(Of String)(BrojEUPotvrdaProperty))
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
      Database.LogException("Vehicle.DataPortal_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DBConcurrencyException("Vehicle.DataPortal_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Protected Overrides Sub DataPortal_DeleteSelf()
    DataPortal_Delete(New SingleCriteria(Of Vehicle, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of Vehicle, Integer))
    Database.LogInfo("Vehicle.DataPortal_Delete", GetHashCode())
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
      Database.LogException("Vehicle.DataPortal_Delete", ex)
      Throw New DbCslaException("Vehicle.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("Vehicle.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Long)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdVehicleBodyTypeProperty, dr.GetInt32("IdVehicleBodyType"))
      LoadProperty(Of Integer)(IdVehicleCategoriesProperty, dr.GetInt32("IdVehicleCategories"))
      LoadProperty(Of Integer)(IdVehicleUseProperty, dr.GetInt32("IdVehicleUse"))
      LoadProperty(Of Integer)(IdEngineTypeProperty, dr.GetInt32("IdEngineType"))
      LoadProperty(Of Integer)(IdEnginePowerSourceProperty, dr.GetInt32("IdEnginePowerSource"))
      LoadProperty(Of Integer)(IdEngineSecondPowerSourceProperty, dr.GetInt32("IdEngineSecondPowerSource"))
      LoadProperty(Of Integer)(IdGearBoxProperty, dr.GetInt32("IdGearBox"))
      LoadProperty(Of Integer)(IdBreakesProperty, dr.GetInt32("IdBreakes"))
      LoadProperty(Of Integer)(IdSupportingProperty, dr.GetInt32("IdSupporting"))
      LoadProperty(Of Integer)(IdVehicleModelProperty, dr.GetInt32("IdVehicleModel"))
      LoadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty, dr.GetInt32("IdVehicleCategoryForPayments"))
      LoadProperty(Of Integer)(IdEngineEcoProgramProperty, dr.GetInt32("IdEngineEcoProgram"))
      LoadProperty(Of Integer)(IdMadeCountryProperty, dr.GetInt32("IdMadeCountry"))
      LoadProperty(Of String)(ColorCodeProperty, dr.GetString("ColorCode"))
      LoadProperty(Of String)(EngineNumberProperty, dr.GetString("EngineNumber"))
      LoadProperty(Of Decimal)(EnginePowerProperty, dr.GetValue("EnginePower"))
      LoadProperty(Of String)(EngineTorqueProperty, dr.GetString("EngineTorque"))
      LoadProperty(Of Decimal)(EngineTorqueUnderGassProperty, dr.GetValue("EngineTorqueUnderGass"))
      LoadProperty(Of Decimal)(EngineWorkingCapacityProperty, dr.GetValue("EngineWorkingCapacity"))
      LoadProperty(Of Decimal)(EnginePowerOutPutProperty, dr.GetValue("EnginePowerOutPut"))
      LoadProperty(Of String)(ShellNumberProperty, dr.GetString("ShellNumber"))
      LoadProperty(Of SmartDate, Date?)(MakeDateProperty, dr.GetSmartDate("MakeDate", True))
      LoadProperty(Of Integer)(NumberOfDoorsProperty, dr.GetInt32("NumberOfDoors"))
      LoadProperty(Of Short)(NumberOfSeatsProperty, dr.GetInt16("NumberOfSeats"))
      LoadProperty(Of Short)(NumberOfStandingSeatsProperty, dr.GetInt16("NumberOfStandingSeats"))
      LoadProperty(Of Short)(NumberOfLieingSeatsProperty, dr.GetInt16("NumberOfLieingSeats"))
      LoadProperty(Of Decimal)(EmptyWaightProperty, dr.GetValue("EmptyWaight"))
      LoadProperty(Of Decimal)(MaximunAllowedWaightProperty, dr.GetValue("MaximunAllowedWaight"))
      LoadProperty(Of String)(TrailerWaightWithBreakProperty, dr.GetString("TrailerWaightWithBreak"))
      LoadProperty(Of String)(TrailerWaightWithoutBreakProperty, dr.GetString("TrailerWaightWithoutBreak"))
      LoadProperty(Of Integer)(NumberOfAxisProperty, dr.GetInt32("NumberOfAxis"))
      LoadProperty(Of Integer)(PropulsionAxisProperty, dr.GetInt32("PropulsionAxis"))
      LoadProperty(Of Integer)(NumberOfWheelsProperty, dr.GetInt32("NumberOfWheels"))
      LoadProperty(Of Integer)(NumberOfPropulsionWheelsProperty, dr.GetInt32("NumberOfPropulsionWheels"))
      LoadProperty(Of Decimal)(VehicleSizeHightProperty, dr.GetValue("VehicleSizeHight"))
      LoadProperty(Of Decimal)(VehicleSizeWidthProperty, dr.GetValue("VehicleSizeWidth"))
      LoadProperty(Of Decimal)(VehicleSizeLengthProperty, dr.GetValue("VehicleSizeLength"))
      LoadProperty(Of Boolean)(SuffocationProperty, dr.GetBoolean("Suffocation"))
      LoadProperty(Of Boolean)(HookProperty, dr.GetBoolean("Hook"))
      LoadProperty(Of Boolean)(VitloProperty, dr.GetBoolean("Vitlo"))
      LoadProperty(Of Boolean)(VerticalBurdenOnTheSeatProperty, dr.GetBoolean("VerticalBurdenOnTheSeat"))
      LoadProperty(Of String)(VerticalBurdenOnTheSeatNoteProperty, dr.GetString("VerticalBurdenOnTheSeatNote"))
      LoadProperty(Of Integer)(IdPrimaryColorProperty, dr.GetInt32("IdPrimaryColor"))
      LoadProperty(Of Integer)(IdSecondaryColorProperty, dr.GetInt32("IdSecondaryColor"))
      LoadProperty(Of String)(HologationSertificateNumberProperty, dr.GetString("HologationSertificateNumber"))
      LoadProperty(Of Decimal)(NoiseStaticProperty, dr.GetValue("NoiseStatic"))
      LoadProperty(Of Decimal)(NoiseMovmentProperty, dr.GetValue("NoiseMovment"))
      LoadProperty(Of Decimal)(CoProperty, dr.GetValue("CO"))
      LoadProperty(Of Decimal)(HcProperty, dr.GetValue("HC"))
      LoadProperty(Of Decimal)(NOxProperty, dr.GetValue("NOx"))
      LoadProperty(Of Decimal)(HCNOxProperty, dr.GetValue("HCNOx"))
      LoadProperty(Of String)(BlackeningProperty, dr.GetString("Blackening"))
      LoadProperty(Of String)(PinpointsProperty, dr.GetString("Pinpoints"))
      LoadProperty(Of Decimal)(Co2Property, dr.GetValue("CO2"))
      LoadProperty(Of String)(FuelConsumptionProperty, dr.GetString("FuelConsumption"))
      LoadProperty(Of Decimal)(CapacityFuelTankProperty, dr.GetValue("CapacityFuelTank"))
      LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
      LoadProperty(Of Boolean)(IsSocialNotPrivateProperty, dr.GetBoolean("IsSocialNotPrivate"))
      LoadProperty(Of Boolean)(ForPrivateTransportNotPublicProperty, dr.GetBoolean("ForPrivateTransportNotPublic"))
      LoadProperty(Of Boolean)(TNGProperty, dr.GetBoolean("TNG"))
      LoadProperty(Of String)(VehicleModelAddingProperty, dr.GetString("VehicleModelAdding"))
      LoadProperty(Of Decimal)(MaxSpeedProperty, dr.GetValue("MaxSpeed"))
      LoadProperty(Of Decimal)(TempOfEngineOilProperty, dr.GetValue("TempOfEngineOil"))
      LoadProperty(Of Integer)(FirstIdRegistrationIssuerProperty, dr.GetInt32("IdFirstRegistrationIssuer"))
      LoadProperty(Of Integer)(LastIdRegistrationIssuerProperty, dr.GetInt32("IdLastRegistrationIssuer"))
      LoadProperty(Of String)(FirstRegistrationNumberProperty, dr.GetString("FirstRegistrationNumber"))
            LoadProperty(Of String)(LastRegistratinNumberProperty, dr.GetString("LastRegistratinNumber"))
            LoadProperty(Of String)(IdentifikacijaNaMotorMestoMetodProperty, dr.GetString("IdentifikacijaNaMotorMestoMetod"))
            LoadProperty(Of Integer)(MasaPoOska1Property, dr.GetInt32("MasaPoOska1"))
            LoadProperty(Of Integer)(MasaPoOska2Property, dr.GetInt32("MasaPoOska2"))
            LoadProperty(Of Integer)(MasaPoOska3Property, dr.GetInt32("MasaPoOska3"))
            LoadProperty(Of Integer)(MasaPoOska4Property, dr.GetInt32("MasaPoOska4"))
            LoadProperty(Of Integer)(MasaPoOska5Property, dr.GetInt32("MasaPoOska5"))
            LoadProperty(Of Integer)(MasaPoOskaPriklucnaProperty, dr.GetInt32("MasaPoOskaPriklucna"))
            LoadProperty(Of Integer)(MaxKonstOptovaruvanjeVoPriklucokProperty, dr.GetInt32("MaxKonstOptovaruvanjeVoPriklucok"))
            LoadProperty(Of Integer)(MaxKonstVkMasaKocnaPrikolkaProperty, dr.GetInt32("MaxKonstVkMasaKocnaPrikolka"))
            LoadProperty(Of Integer)(MaxKonstVkMasaNeKocnaPrikolkaProperty, dr.GetInt32("MaxKonstVkMasaNeKocnaPrikolka"))
            LoadProperty(Of Decimal)(MaxLegVkMasaProperty, dr.GetValue("MaxLegVkMasa"))
            LoadProperty(Of Decimal)(MaxLegVkMasaGrupaProperty, dr.GetValue("MaxLegVkMasaGrupa"))
            LoadProperty(Of String)(NoiseTechnicalSpecProperty, dr.GetString("NoiseTechnicalSpec"))
            LoadProperty(Of String)(OdnosKwCcmProperty, dr.GetString("OdnosKwCcm"))
            LoadProperty(Of Integer)(OsnoOptovaruvanje1Property, dr.GetInt32("OsnoOptovaruvanje1"))
            LoadProperty(Of Integer)(OsnoOptovaruvanje2Property, dr.GetInt32("OsnoOptovaruvanje2"))
            LoadProperty(Of Integer)(OsnoOptovaruvanje3Property, dr.GetInt32("OsnoOptovaruvanje3"))
            LoadProperty(Of Integer)(OsnoOptovaruvanje4Property, dr.GetInt32("OsnoOptovaruvanje4"))
            LoadProperty(Of Integer)(OsnoOptovaruvanje5Property, dr.GetInt32("OsnoOptovaruvanje5"))
            LoadProperty(Of Integer)(OsnoOptovaruvanjePriklucnaProperty, dr.GetInt32("OsnoOptovaruvanjePriklucna"))
            LoadProperty(Of String)(OznakaNaOdobrenieProperty, dr.GetString("OznakaNaOdobrenie"))
            LoadProperty(Of String)(OznakaNaOdobrenieZaPriklucUredProperty, dr.GetString("OznakaNaOdobrenieZaPriklucUred"))
            LoadProperty(Of String)(TBrOdobrenieMehanPriklucokProperty, dr.GetString("TBrOdobrenieMehanPriklucok"))
            LoadProperty(Of String)(TipProperty, dr.GetString("Tip"))
            LoadProperty(Of String)(TMarkaMehanPriklucokProperty, dr.GetString("TMarkaMehanPriklucok"))
            LoadProperty(Of Integer)(TMaxHorVerOptovaruvanjePriklucokProperty, dr.GetInt32("TMaxHorVerOptovaruvanjePriklucok"))
            LoadProperty(Of Integer)(TMaxKonstVkMasaNaKombinacijaProperty, dr.GetInt32("TMaxKonstVkMasaNaKombinacija"))
            LoadProperty(Of Integer)(TMaxKonstVkMasaPoluprikolkaProperty, dr.GetInt32("TMaxKonstVkMasaPoluprikolka"))
            LoadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaProperty, dr.GetInt32("TMaxKonstVkMasaPrikolka"))
            LoadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaSoCenOskaProperty, dr.GetInt32("TMaxKonstVkMasaPrikolkaSoCenOska"))
            LoadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaStoMozePrikluciProperty, dr.GetInt32("TMaxKonstVkMasaPrikolkaStoMozePrikluci"))
            LoadProperty(Of Integer)(TMinMasaProperty, dr.GetInt32("TMinMasa"))
            LoadProperty(Of String)(TTipMehanPriklucokProperty, dr.GetString("TTipMehanPriklucok"))
            LoadProperty(Of String)(TZastitnaKabinaProperty, dr.GetString("TZastitnaKabina"))
            LoadProperty(Of String)(TZastitnaRamkaProperty, dr.GetString("TZastitnaRamka"))
            LoadProperty(Of String)(VarijantaIzvedbaProperty, dr.GetString("VarijantaIzvedba"))
            LoadProperty(Of Integer)(BrojNaVrteziProperty, dr.GetInt32("BrojNaVrtezi"))
            LoadProperty(Of Decimal)(MaxKonstVkMasaProperty, dr.GetValue("MaxKonstVkMasa"))
            LoadProperty(Of String)(BrojEUPotvrdaProperty, dr.GetString("BrojEUPotvrda"))
      'LoadProperty(Of String)(FirstRegistrationCommunityProperty, dr.GetString("FirstRegistrationCommunity"))
      'LoadProperty(Of String)(LastRegistrationCommunityProperty, dr.GetString("LastRegistrationCommunity"))
      LoadProperty(Of SmartDate, Date?)(FirstRegistrationMakeDateProperty, dr.GetSmartDate("FirstRegistrationMakeDate", True))
      LoadProperty(Of SmartDate, Date?)(FirstRegistrationValidTillProperty, dr.GetSmartDate("FirstRegistrationValidTill", True))
      LoadProperty(Of SmartDate, Date?)(LastRegistrationMakeDateProperty, dr.GetSmartDate("LastRegistrationMakeDate", True))
      LoadProperty(Of SmartDate, Date?)(LastRegistrationValidTillProperty, dr.GetSmartDate("LastRegistrationValidTill", True))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      Using cn As SqlConnection = Database.VTE_SqlConnection

        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetChildAxes
          cm.Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm.ExecuteReader)
            LoadProperty(Of VehicleAxes) _
            (VehicleAxesProperty, VehicleAxes.GetVehicleAxes(drc))
          End Using
        End Using

        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildAxesDestination
          cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleBetweenAxesDestinations) _
            (VehicleAxesDestinationProperty, VehicleBetweenAxesDestinations.GetVehicleBetweenAxesDestinations(drc))
          End Using
        End Using
        'Using cm1 As SqlCommand = cn.CreateCommand
        '    cm1.CommandType = CommandType.StoredProcedure
        '    cm1.CommandText = spGetChildLastTehnicalExam
        '    cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
        '    Using drc As New SafeDataReader(cm1.ExecuteReader)
        '        LoadProperty(Of VehicleLastTehnicalExams) _
        '        (VehicleLastTehnicalExamsProperty, VehicleLastTehnicalExams.GetVehicleLastTehnicalExams(drc))
        '    End Using
        'End Using

        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildrenPersonalTires
          cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleTyres) _
            (VehiclePersonalTiresProperty, VehicleTyres.GetVehicleTyres(drc))
          End Using
        End Using
        'Using cm1 As SqlCommand = cn.CreateCommand
        '    cm1.CommandType = CommandType.StoredProcedure
        '    cm1.CommandText = spGetChildrenTires
        '    cm1.Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))
        '    Using drc As New SafeDataReader(cm1.ExecuteReader)
        '        LoadProperty(Of TireTypes) _
        '        (VehicleTiresProperty, TireTypes.GetTireTypes(drc))
        '    End Using
        'End Using
        'registracii
        'Using cm1 As SqlCommand = cn.CreateCommand
        '    cm1.CommandType = CommandType.StoredProcedure
        '    cm1.CommandText = spGetChildRegistrations
        '    cm1.Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
        '    Using drc As New SafeDataReader(cm1.ExecuteReader)
        '        LoadProperty(Of VehicleRegistrations) _
        '        (VehicleRegistrationsProperty, VehicleRegistrations.GetVehicleRegistrations(drc))
        '    End Using
        'End Using
      End Using


    Catch ex As Exception
      Database.LogException("Vehicle.Child_Fetch", ex)
      Throw New DbCslaException("Vehicle.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@IdVehicleBodyType", ReadProperty(Of Integer)(IdVehicleBodyTypeProperty))
            .Parameters.AddWithValue("@IdVehicleCategories", ReadProperty(Of Integer)(IdVehicleCategoriesProperty))
            .Parameters.AddWithValue("@IdVehicleUse", ReadProperty(Of Integer)(IdVehicleUseProperty))
            .Parameters.AddWithValue("@IdEngineType", ReadProperty(Of Integer)(IdEngineTypeProperty))
            .Parameters.AddWithValue("@IdEnginePowerSource", ReadProperty(Of Integer)(IdEnginePowerSourceProperty))
            .Parameters.AddWithValue("@IdEngineSecondPowerSource", ReadProperty(Of Integer)(IdEngineSecondPowerSourceProperty))
            '.Parameters.AddWithValue("@IdEngineMark", ReadProperty(Of Integer)(IdEngineMarkProperty))
            .Parameters.AddWithValue("@IdGearBox", ReadProperty(Of Integer)(IdGearBoxProperty))
            .Parameters.AddWithValue("@IdBreakes", ReadProperty(Of Integer)(IdBreakesProperty))
            .Parameters.AddWithValue("@IdSupporting", ReadProperty(Of Integer)(IdSupportingProperty))
            .Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))
            .Parameters.AddWithValue("@IdVehicleCategoryForPayments", ReadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty))
            .Parameters.AddWithValue("@IdEngineEcoProgram", ReadProperty(Of Integer)(IdEngineEcoProgramProperty))
            .Parameters.AddWithValue("@IdMadeCountry", ReadProperty(Of Integer)(IdMadeCountryProperty))
            .Parameters.AddWithValue("@ColorCode", ReadProperty(Of String)(ColorCodeProperty))
            .Parameters.AddWithValue("@EngineNumber", ReadProperty(Of String)(EngineNumberProperty))
            .Parameters.AddWithValue("@EnginePower", ReadProperty(Of Decimal)(EnginePowerProperty))
            .Parameters.AddWithValue("@EngineTorque", ReadProperty(Of String)(EngineTorqueProperty))
            .Parameters.AddWithValue("@EngineTorqueUnderGass", ReadProperty(Of Decimal)(EngineTorqueUnderGassProperty))
            .Parameters.AddWithValue("@EngineWorkingCapacity", ReadProperty(Of Decimal)(EngineWorkingCapacityProperty))
            .Parameters.AddWithValue("@EnginePowerOutPut", ReadProperty(Of Decimal)(EnginePowerOutPutProperty))
            .Parameters.AddWithValue("@ShellNumber", ReadProperty(Of String)(ShellNumberProperty))
            .Parameters.AddWithValue("@MakeDate", ReadProperty(Of SmartDate)(MakeDateProperty).DBValue)
            .Parameters.AddWithValue("@NumberOfDoors", ReadProperty(Of Integer)(NumberOfDoorsProperty))
            .Parameters.AddWithValue("@NumberOfSeats", ReadProperty(Of Short)(NumberOfSeatsProperty))
            .Parameters.AddWithValue("@NumberOfStandingSeats", ReadProperty(Of Short)(NumberOfStandingSeatsProperty))
            .Parameters.AddWithValue("@NumberOfLieingSeats", ReadProperty(Of Short)(NumberOfLieingSeatsProperty))
            .Parameters.AddWithValue("@EmptyWaight", ReadProperty(Of Decimal)(EmptyWaightProperty))
            .Parameters.AddWithValue("@MaximunAllowedWaight", ReadProperty(Of Decimal)(MaximunAllowedWaightProperty))
            .Parameters.AddWithValue("@TrailerWaightWithBreak", ReadProperty(Of String)(TrailerWaightWithBreakProperty))
            .Parameters.AddWithValue("@TrailerWaightWithoutBreak", ReadProperty(Of String)(TrailerWaightWithoutBreakProperty))
            .Parameters.AddWithValue("@NumberOfAxis", ReadProperty(Of Integer)(NumberOfAxisProperty))
            .Parameters.AddWithValue("@PropulsionAxis", ReadProperty(Of Integer)(PropulsionAxisProperty))
            .Parameters.AddWithValue("@NumberOfWheels", ReadProperty(Of Integer)(NumberOfWheelsProperty))
            .Parameters.AddWithValue("@NumberOfPropulsionWheels", ReadProperty(Of Integer)(NumberOfPropulsionWheelsProperty))
            .Parameters.AddWithValue("@VehicleSizeHight", ReadProperty(Of Decimal)(VehicleSizeHightProperty))
            .Parameters.AddWithValue("@VehicleSizeWidth", ReadProperty(Of Decimal)(VehicleSizeWidthProperty))
            .Parameters.AddWithValue("@VehicleSizeLength", ReadProperty(Of Decimal)(VehicleSizeLengthProperty))
            .Parameters.AddWithValue("@Suffocation", ReadProperty(Of Boolean)(SuffocationProperty))
            .Parameters.AddWithValue("@Hook", ReadProperty(Of Boolean)(HookProperty))
            .Parameters.AddWithValue("@Vitlo", ReadProperty(Of Boolean)(VitloProperty))
            .Parameters.AddWithValue("@VerticalBurdenOnTheSeat", ReadProperty(Of Boolean)(VerticalBurdenOnTheSeatProperty))
            .Parameters.AddWithValue("@VerticalBurdenOnTheSeatNote", ReadProperty(Of String)(VerticalBurdenOnTheSeatNoteProperty))
            .Parameters.AddWithValue("@IdPrimaryColor", ReadProperty(Of Integer)(IdPrimaryColorProperty))
            .Parameters.AddWithValue("@IdSecondaryColor", ReadProperty(Of Integer)(IdSecondaryColorProperty))
            .Parameters.AddWithValue("@HologationSertificateNumber", ReadProperty(Of String)(HologationSertificateNumberProperty))
            .Parameters.AddWithValue("@NoiseStatic", ReadProperty(Of Decimal)(NoiseStaticProperty))
            .Parameters.AddWithValue("@NoiseMovment", ReadProperty(Of Decimal)(NoiseMovmentProperty))
            .Parameters.AddWithValue("@CO", ReadProperty(Of Decimal)(CoProperty))
            .Parameters.AddWithValue("@HC", ReadProperty(Of Decimal)(HcProperty))
            .Parameters.AddWithValue("@NOx", ReadProperty(Of Decimal)(NOxProperty))
            .Parameters.AddWithValue("@HCNOx", ReadProperty(Of Decimal)(HCNOxProperty))
            .Parameters.AddWithValue("@Blackening", ReadProperty(Of String)(BlackeningProperty))
            .Parameters.AddWithValue("@Pinpoints", ReadProperty(Of String)(PinpointsProperty))
            .Parameters.AddWithValue("@CO2", ReadProperty(Of Decimal)(Co2Property))
            .Parameters.AddWithValue("@FuelConsumption", ReadProperty(Of String)(FuelConsumptionProperty))
            .Parameters.AddWithValue("@CapacityFuelTank", ReadProperty(Of Decimal)(CapacityFuelTankProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
            .Parameters.AddWithValue("@IsSocialNotPrivate", ReadProperty(Of Boolean)(IsSocialNotPrivateProperty))
            .Parameters.AddWithValue("@ForPrivateTransportNotPublic", ReadProperty(Of Boolean)(ForPrivateTransportNotPublicProperty))
            .Parameters.AddWithValue("@TNG", ReadProperty(Of Boolean)(TNGProperty))
            .Parameters.AddWithValue("@VehicleModelAdding", ReadProperty(Of String)(VehicleModelAddingProperty))
            .Parameters.AddWithValue("@MaxSpeed", ReadProperty(Of Decimal)(MaxSpeedProperty))
                        .Parameters.AddWithValue("@TempOfEngineOil", ReadProperty(Of Decimal)(TempOfEngineOilProperty))
                        .Parameters.AddWithValue("@IdentifikacijaNaMotorMestoMetod", ReadProperty(Of String)(IdentifikacijaNaMotorMestoMetodProperty))
                        .Parameters.AddWithValue("@MasaPoOska1", ReadProperty(Of Integer)(MasaPoOska1Property))
                        .Parameters.AddWithValue("@MasaPoOska2", ReadProperty(Of Integer)(MasaPoOska2Property))
                        .Parameters.AddWithValue("@MasaPoOska3", ReadProperty(Of Integer)(MasaPoOska3Property))
                        .Parameters.AddWithValue("@MasaPoOska4", ReadProperty(Of Integer)(MasaPoOska4Property))
                        .Parameters.AddWithValue("@MasaPoOska5", ReadProperty(Of Integer)(MasaPoOska5Property))
                        .Parameters.AddWithValue("@MasaPoOskaPriklucna", ReadProperty(Of Integer)(MasaPoOskaPriklucnaProperty))
                        .Parameters.AddWithValue("@MaxKonstOptovaruvanjeVoPriklucok", ReadProperty(Of Integer)(MaxKonstOptovaruvanjeVoPriklucokProperty))
                        .Parameters.AddWithValue("@MaxKonstVkMasaKocnaPrikolka", ReadProperty(Of Integer)(MaxKonstVkMasaKocnaPrikolkaProperty))
                        .Parameters.AddWithValue("@MaxKonstVkMasaNeKocnaPrikolka", ReadProperty(Of Integer)(MaxKonstVkMasaNeKocnaPrikolkaProperty))
                        .Parameters.AddWithValue("@MaxLegVkMasa", ReadProperty(Of Decimal)(MaxLegVkMasaProperty))
                        .Parameters.AddWithValue("@MaxLegVkMasaGrupa", ReadProperty(Of Decimal)(MaxLegVkMasaGrupaProperty))
                        .Parameters.AddWithValue("@NoiseTechnicalSpec", ReadProperty(Of String)(NoiseTechnicalSpecProperty))
                        .Parameters.AddWithValue("@OdnosKwCcm", ReadProperty(Of String)(OdnosKwCcmProperty))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje1", ReadProperty(Of Integer)(OsnoOptovaruvanje1Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje2", ReadProperty(Of Integer)(OsnoOptovaruvanje2Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje3", ReadProperty(Of Integer)(OsnoOptovaruvanje3Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje4", ReadProperty(Of Integer)(OsnoOptovaruvanje4Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje5", ReadProperty(Of Integer)(OsnoOptovaruvanje5Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanjePriklucna", ReadProperty(Of Integer)(OsnoOptovaruvanjePriklucnaProperty))
                        .Parameters.AddWithValue("@OznakaNaOdobrenie", ReadProperty(Of String)(OznakaNaOdobrenieProperty))
                        .Parameters.AddWithValue("@OznakaNaOdobrenieZaPriklucUred", ReadProperty(Of String)(OznakaNaOdobrenieZaPriklucUredProperty))
                        .Parameters.AddWithValue("@TBrOdobrenieMehanPriklucok", ReadProperty(Of String)(TBrOdobrenieMehanPriklucokProperty))
                        .Parameters.AddWithValue("@Tip", ReadProperty(Of String)(TipProperty))
                        .Parameters.AddWithValue("@TMarkaMehanPriklucok", ReadProperty(Of String)(TMarkaMehanPriklucokProperty))
                        .Parameters.AddWithValue("@TMaxHorVerOptovaruvanjePriklucok", ReadProperty(Of Integer)(TMaxHorVerOptovaruvanjePriklucokProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaNaKombinacija", ReadProperty(Of Integer)(TMaxKonstVkMasaNaKombinacijaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPoluprikolka", ReadProperty(Of Integer)(TMaxKonstVkMasaPoluprikolkaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPrikolka", ReadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPrikolkaSoCenOska", ReadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaSoCenOskaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPrikolkaStoMozePrikluci", ReadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaStoMozePrikluciProperty))
                        .Parameters.AddWithValue("@TMinMasa", ReadProperty(Of Integer)(TMinMasaProperty))
                        .Parameters.AddWithValue("@TTipMehanPriklucok", ReadProperty(Of String)(TTipMehanPriklucokProperty))
                        .Parameters.AddWithValue("@TZastitnaKabina", ReadProperty(Of String)(TZastitnaKabinaProperty))
                        .Parameters.AddWithValue("@TZastitnaRamka", ReadProperty(Of String)(TZastitnaRamkaProperty))
                        .Parameters.AddWithValue("@VarijantaIzvedba", ReadProperty(Of String)(VarijantaIzvedbaProperty))
                        .Parameters.AddWithValue("@BrojNaVrtezi", ReadProperty(Of Integer)(BrojNaVrteziProperty))
                        .Parameters.AddWithValue("@MaxKonstVkMasa", ReadProperty(Of Decimal)(MaxKonstVkMasaProperty))
                        .Parameters.AddWithValue("@BrojEUPotvrda", ReadProperty(Of String)(BrojEUPotvrdaProperty))
                        .Parameters.AddWithValue("@FirstRegistrationNumber", ReadProperty(Of String)(FirstRegistrationNumberProperty))
            .Parameters.AddWithValue("@LastRegistratinNumber", ReadProperty(Of String)(LastRegistratinNumberProperty))
            '.Parameters.AddWithValue("@FirstRegistrationCommunity", ReadProperty(Of String)(FirstRegistrationCommunityProperty))
            '.Parameters.AddWithValue("@LastRegistrationCommunity", ReadProperty(Of String)(LastRegistrationCommunityProperty))
            .Parameters.AddWithValue("@FirstRegistrationMakeDate", ReadProperty(Of SmartDate)(FirstRegistrationMakeDateProperty).DBValue)
            .Parameters.AddWithValue("@FirstRegistrationValidTill", ReadProperty(Of SmartDate)(FirstRegistrationValidTillProperty).DBValue)
            .Parameters.AddWithValue("@LastRegistrationMakeDate", ReadProperty(Of SmartDate)(LastRegistrationMakeDateProperty).DBValue)
            .Parameters.AddWithValue("@LastRegistrationValidTill", ReadProperty(Of SmartDate)(LastRegistrationValidTillProperty).DBValue)
            .Parameters.AddWithValue("@FirstIdRegistrationIssuer", ReadProperty(Of Integer)(FirstIdRegistrationIssuerProperty))
            .Parameters.AddWithValue("@IdLastRegistrationIssuer", ReadProperty(Of Integer)(LastIdRegistrationIssuerProperty))

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
      Database.LogException("Vehicle.Child_Insert", ex)
      Throw New DbCslaException("Vehicle.Child_Insert", ex)
    Finally
      Database.LogInfo("Vehicle.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("Vehicle.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
            .Parameters.AddWithValue("@IdVehicleBodyType", ReadProperty(Of Integer)(IdVehicleBodyTypeProperty))
            .Parameters.AddWithValue("@IdVehicleCategories", ReadProperty(Of Integer)(IdVehicleCategoriesProperty))
            .Parameters.AddWithValue("@IdVehicleUse", ReadProperty(Of Integer)(IdVehicleUseProperty))
            .Parameters.AddWithValue("@IdEngineType", ReadProperty(Of Integer)(IdEngineTypeProperty))
            .Parameters.AddWithValue("@IdEnginePowerSource", ReadProperty(Of Integer)(IdEnginePowerSourceProperty))
            .Parameters.AddWithValue("@IdEngineSecondPowerSource", ReadProperty(Of Integer)(IdEngineSecondPowerSourceProperty))
            ' .Parameters.AddWithValue("@IdEngineMark", ReadProperty(Of Integer)(IdEngineMarkProperty))
            .Parameters.AddWithValue("@IdGearBox", ReadProperty(Of Integer)(IdGearBoxProperty))
            .Parameters.AddWithValue("@IdBreakes", ReadProperty(Of Integer)(IdBreakesProperty))
            .Parameters.AddWithValue("@IdSupporting", ReadProperty(Of Integer)(IdSupportingProperty))
            .Parameters.AddWithValue("@IdVehicleModel", ReadProperty(Of Integer)(IdVehicleModelProperty))
            .Parameters.AddWithValue("@IdVehicleCategoryForPayments", ReadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty))
            .Parameters.AddWithValue("@IdEngineEcoProgram", ReadProperty(Of Integer)(IdEngineEcoProgramProperty))
            .Parameters.AddWithValue("@IdMadeCountry", ReadProperty(Of Integer)(IdMadeCountryProperty))
            .Parameters.AddWithValue("@ColorCode", ReadProperty(Of String)(ColorCodeProperty))
            .Parameters.AddWithValue("@EngineNumber", ReadProperty(Of String)(EngineNumberProperty))
            .Parameters.AddWithValue("@EnginePower", ReadProperty(Of Decimal)(EnginePowerProperty))
            .Parameters.AddWithValue("@EngineTorque", ReadProperty(Of String)(EngineTorqueProperty))
            .Parameters.AddWithValue("@EngineTorqueUnderGass", ReadProperty(Of Decimal)(EngineTorqueUnderGassProperty))
            .Parameters.AddWithValue("@EngineWorkingCapacity", ReadProperty(Of Decimal)(EngineWorkingCapacityProperty))
            .Parameters.AddWithValue("@EnginePowerOutPut", ReadProperty(Of Decimal)(EnginePowerOutPutProperty))
            .Parameters.AddWithValue("@ShellNumber", ReadProperty(Of String)(ShellNumberProperty))
            .Parameters.AddWithValue("@MakeDate", ReadProperty(Of SmartDate)(MakeDateProperty).DBValue)
            .Parameters.AddWithValue("@NumberOfDoors", ReadProperty(Of Integer)(NumberOfDoorsProperty))
            .Parameters.AddWithValue("@NumberOfSeats", ReadProperty(Of Short)(NumberOfSeatsProperty))
            .Parameters.AddWithValue("@NumberOfStandingSeats", ReadProperty(Of Short)(NumberOfStandingSeatsProperty))
            .Parameters.AddWithValue("@NumberOfLieingSeats", ReadProperty(Of Short)(NumberOfLieingSeatsProperty))
            .Parameters.AddWithValue("@EmptyWaight", ReadProperty(Of Decimal)(EmptyWaightProperty))
            .Parameters.AddWithValue("@MaximunAllowedWaight", ReadProperty(Of Decimal)(MaximunAllowedWaightProperty))
            .Parameters.AddWithValue("@TrailerWaightWithBreak", ReadProperty(Of String)(TrailerWaightWithBreakProperty))
            .Parameters.AddWithValue("@TrailerWaightWithoutBreak", ReadProperty(Of String)(TrailerWaightWithoutBreakProperty))
            .Parameters.AddWithValue("@NumberOfAxis", ReadProperty(Of Integer)(NumberOfAxisProperty))
            .Parameters.AddWithValue("@PropulsionAxis", ReadProperty(Of Integer)(PropulsionAxisProperty))
            .Parameters.AddWithValue("@NumberOfWheels", ReadProperty(Of Integer)(NumberOfWheelsProperty))
            .Parameters.AddWithValue("@NumberOfPropulsionWheels", ReadProperty(Of Integer)(NumberOfPropulsionWheelsProperty))
            .Parameters.AddWithValue("@VehicleSizeHight", ReadProperty(Of Decimal)(VehicleSizeHightProperty))
            .Parameters.AddWithValue("@VehicleSizeWidth", ReadProperty(Of Decimal)(VehicleSizeWidthProperty))
            .Parameters.AddWithValue("@VehicleSizeLength", ReadProperty(Of Decimal)(VehicleSizeLengthProperty))
            .Parameters.AddWithValue("@Suffocation", ReadProperty(Of Boolean)(SuffocationProperty))
            .Parameters.AddWithValue("@Hook", ReadProperty(Of Boolean)(HookProperty))
            .Parameters.AddWithValue("@Vitlo", ReadProperty(Of Boolean)(VitloProperty))
            .Parameters.AddWithValue("@VerticalBurdenOnTheSeat", ReadProperty(Of Boolean)(VerticalBurdenOnTheSeatProperty))
            .Parameters.AddWithValue("@VerticalBurdenOnTheSeatNote", ReadProperty(Of String)(VerticalBurdenOnTheSeatNoteProperty))
            .Parameters.AddWithValue("@IdPrimaryColor", ReadProperty(Of Integer)(IdPrimaryColorProperty))
            .Parameters.AddWithValue("@IdSecondaryColor", ReadProperty(Of Integer)(IdSecondaryColorProperty))
            .Parameters.AddWithValue("@HologationSertificateNumber", ReadProperty(Of String)(HologationSertificateNumberProperty))
            .Parameters.AddWithValue("@NoiseStatic", ReadProperty(Of Decimal)(NoiseStaticProperty))
            .Parameters.AddWithValue("@NoiseMovment", ReadProperty(Of Decimal)(NoiseMovmentProperty))
            .Parameters.AddWithValue("@CO", ReadProperty(Of Decimal)(CoProperty))
            .Parameters.AddWithValue("@HC", ReadProperty(Of Decimal)(HcProperty))
            .Parameters.AddWithValue("@NOx", ReadProperty(Of Decimal)(NOxProperty))
            .Parameters.AddWithValue("@HCNOx", ReadProperty(Of Decimal)(HCNOxProperty))
            .Parameters.AddWithValue("@Blackening", ReadProperty(Of String)(BlackeningProperty))
            .Parameters.AddWithValue("@Pinpoints", ReadProperty(Of String)(PinpointsProperty))
            .Parameters.AddWithValue("@CO2", ReadProperty(Of Decimal)(Co2Property))
            .Parameters.AddWithValue("@FuelConsumption", ReadProperty(Of String)(FuelConsumptionProperty))
            .Parameters.AddWithValue("@CapacityFuelTank", ReadProperty(Of Decimal)(CapacityFuelTankProperty))
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
            .Parameters.AddWithValue("@IsSocialNotPrivate", ReadProperty(Of Boolean)(IsSocialNotPrivateProperty))
            .Parameters.AddWithValue("@ForPrivateTransportNotPublic", ReadProperty(Of Boolean)(ForPrivateTransportNotPublicProperty))
            .Parameters.AddWithValue("@TNG", ReadProperty(Of Boolean)(TNGProperty))
            .Parameters.AddWithValue("@VehicleModelAdding", ReadProperty(Of String)(VehicleModelAddingProperty))
            .Parameters.AddWithValue("@MaxSpeed", ReadProperty(Of Decimal)(MaxSpeedProperty))
                        .Parameters.AddWithValue("@TempOfEngineOil", ReadProperty(Of Decimal)(TempOfEngineOilProperty))
                        .Parameters.AddWithValue("@IdentifikacijaNaMotorMestoMetod", ReadProperty(Of String)(IdentifikacijaNaMotorMestoMetodProperty))
                        .Parameters.AddWithValue("@MasaPoOska1", ReadProperty(Of Integer)(MasaPoOska1Property))
                        .Parameters.AddWithValue("@MasaPoOska2", ReadProperty(Of Integer)(MasaPoOska2Property))
                        .Parameters.AddWithValue("@MasaPoOska3", ReadProperty(Of Integer)(MasaPoOska3Property))
                        .Parameters.AddWithValue("@MasaPoOska4", ReadProperty(Of Integer)(MasaPoOska4Property))
                        .Parameters.AddWithValue("@MasaPoOska5", ReadProperty(Of Integer)(MasaPoOska5Property))
                        .Parameters.AddWithValue("@MasaPoOskaPriklucna", ReadProperty(Of Integer)(MasaPoOskaPriklucnaProperty))
                        .Parameters.AddWithValue("@MaxKonstOptovaruvanjeVoPriklucok", ReadProperty(Of Integer)(MaxKonstOptovaruvanjeVoPriklucokProperty))
                        .Parameters.AddWithValue("@MaxKonstVkMasaKocnaPrikolka", ReadProperty(Of Integer)(MaxKonstVkMasaKocnaPrikolkaProperty))
                        .Parameters.AddWithValue("@MaxKonstVkMasaNeKocnaPrikolka", ReadProperty(Of Integer)(MaxKonstVkMasaNeKocnaPrikolkaProperty))
                        .Parameters.AddWithValue("@MaxLegVkMasa", ReadProperty(Of Decimal)(MaxLegVkMasaProperty))
                        .Parameters.AddWithValue("@MaxLegVkMasaGrupa", ReadProperty(Of Decimal)(MaxLegVkMasaGrupaProperty))
                        .Parameters.AddWithValue("@NoiseTechnicalSpec", ReadProperty(Of String)(NoiseTechnicalSpecProperty))
                        .Parameters.AddWithValue("@OdnosKwCcm", ReadProperty(Of String)(OdnosKwCcmProperty))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje1", ReadProperty(Of Integer)(OsnoOptovaruvanje1Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje2", ReadProperty(Of Integer)(OsnoOptovaruvanje2Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje3", ReadProperty(Of Integer)(OsnoOptovaruvanje3Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje4", ReadProperty(Of Integer)(OsnoOptovaruvanje4Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanje5", ReadProperty(Of Integer)(OsnoOptovaruvanje5Property))
                        .Parameters.AddWithValue("@OsnoOptovaruvanjePriklucna", ReadProperty(Of Integer)(OsnoOptovaruvanjePriklucnaProperty))
                        .Parameters.AddWithValue("@OznakaNaOdobrenie", ReadProperty(Of String)(OznakaNaOdobrenieProperty))
                        .Parameters.AddWithValue("@OznakaNaOdobrenieZaPriklucUred", ReadProperty(Of String)(OznakaNaOdobrenieZaPriklucUredProperty))
                        .Parameters.AddWithValue("@TBrOdobrenieMehanPriklucok", ReadProperty(Of String)(TBrOdobrenieMehanPriklucokProperty))
                        .Parameters.AddWithValue("@Tip", ReadProperty(Of String)(TipProperty))
                        .Parameters.AddWithValue("@TMarkaMehanPriklucok", ReadProperty(Of String)(TMarkaMehanPriklucokProperty))
                        .Parameters.AddWithValue("@TMaxHorVerOptovaruvanjePriklucok", ReadProperty(Of Integer)(TMaxHorVerOptovaruvanjePriklucokProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaNaKombinacija", ReadProperty(Of Integer)(TMaxKonstVkMasaNaKombinacijaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPoluprikolka", ReadProperty(Of Integer)(TMaxKonstVkMasaPoluprikolkaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPrikolka", ReadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPrikolkaSoCenOska", ReadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaSoCenOskaProperty))
                        .Parameters.AddWithValue("@TMaxKonstVkMasaPrikolkaStoMozePrikluci", ReadProperty(Of Integer)(TMaxKonstVkMasaPrikolkaStoMozePrikluciProperty))
                        .Parameters.AddWithValue("@TMinMasa", ReadProperty(Of Integer)(TMinMasaProperty))
                        .Parameters.AddWithValue("@TTipMehanPriklucok", ReadProperty(Of String)(TTipMehanPriklucokProperty))
                        .Parameters.AddWithValue("@TZastitnaKabina", ReadProperty(Of String)(TZastitnaKabinaProperty))
                        .Parameters.AddWithValue("@TZastitnaRamka", ReadProperty(Of String)(TZastitnaRamkaProperty))
                        .Parameters.AddWithValue("@VarijantaIzvedba", ReadProperty(Of String)(VarijantaIzvedbaProperty))
                        .Parameters.AddWithValue("@BrojNaVrtezi", ReadProperty(Of Integer)(BrojNaVrteziProperty))
                        .Parameters.AddWithValue("@MaxKonstVkMasa", ReadProperty(Of Decimal)(MaxKonstVkMasaProperty))
                        .Parameters.AddWithValue("@BrojEUPotvrda", ReadProperty(Of String)(BrojEUPotvrdaProperty))
                        .Parameters.AddWithValue("@FirstRegistrationNumber", ReadProperty(Of String)(FirstRegistrationNumberProperty))
            .Parameters.AddWithValue("@LastRegistratinNumber", ReadProperty(Of String)(LastRegistratinNumberProperty))
            ' .Parameters.AddWithValue("@FirstRegistrationCommunity", ReadProperty(Of String)(FirstRegistrationCommunityProperty))
            ' .Parameters.AddWithValue("@LastRegistrationCommunity", ReadProperty(Of String)(LastRegistrationCommunityProperty))
            .Parameters.AddWithValue("@FirstRegistrationMakeDate", ReadProperty(Of SmartDate)(FirstRegistrationMakeDateProperty).DBValue)
            .Parameters.AddWithValue("@FirstRegistrationValidTill", ReadProperty(Of SmartDate)(FirstRegistrationValidTillProperty).DBValue)
            .Parameters.AddWithValue("@LastRegistrationMakeDate", ReadProperty(Of SmartDate)(LastRegistrationMakeDateProperty).DBValue)
            .Parameters.AddWithValue("@LastRegistrationValidTill", ReadProperty(Of SmartDate)(LastRegistrationValidTillProperty).DBValue)
            .Parameters.AddWithValue("@FirstIdRegistrationIssuer", ReadProperty(Of Integer)(FirstIdRegistrationIssuerProperty))
            .Parameters.AddWithValue("@IdLastRegistrationIssuer", ReadProperty(Of Integer)(LastIdRegistrationIssuerProperty))

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
      Database.LogException("Vehicle.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("Vehicle.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("Vehicle.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("Vehicle.Child_Fetch", ex)
      Throw New DbCslaException("Vehicle.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

#Region " NumOfShellExists "

  Public Shared Function ShellExists(ByVal strShell As String, ByVal idVehic As Long) As Boolean

    Return ShellExistsCommand.ShellExists(strShell, idVehic)

  End Function

  <Serializable()> _
  Private Class ShellExistsCommand
    Inherits CommandBase
    Private _shell As String
    Private _idVehic As Long
    Private _shellExists As Boolean
    Public ReadOnly Property ExistsShell() As Boolean
      Get
        Return _shellExists
      End Get
    End Property

    Public Shared Function ShellExists(ByVal sifra As String, ByVal idVehic As Long) As Boolean

      Dim result As ShellExistsCommand
      result = DataPortal.Execute(Of ShellExistsCommand)(New ShellExistsCommand(sifra, idVehic))
      Return result.ExistsShell

    End Function

    Private Sub New(ByVal strShell As String, ByVal idVehic As Long)
      _shell = strShell
      _idVehic = idVehic
      _shellExists = False
    End Sub

    Protected Overrides Sub DataPortal_Execute()
      Dim pom As Integer = 0
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "NumOfVehiclesShellExists"
          cm.Parameters.AddWithValue("@sifra", _shell)
          cm.Parameters.AddWithValue("@idVehicle", _idVehic)
          pom = cm.ExecuteScalar
          If pom = 0 Then
            _shellExists = False
          Else
            _shellExists = True
          End If
        End Using
      End Using
    End Sub

  End Class

#End Region

  '#Region " VehicleFirstRegistrations "

  '    Public Shared Function GetVehicleFirstRegistration(ByVal idVehic As Long) As registrationBasicInfo

  '        Dim result As registrationBasicInfo
  '        result = FirstRegistration.GetFirstRegistration(idVehic)
  '        If result Is Nothing Then
  '            Return New registrationBasicInfo("непозната", New Date(1, 1, 1), "", New Date(1, 1, 1), 1)
  '        Else
  '            Return result

  '        End If

  '    End Function


  '    <Serializable()> _
  '    Private Class FirstRegistration
  '        Inherits CommandBase

  '        Private _idVehic As Long
  '        Private _firstRegistration As registrationBasicInfo
  '        Public ReadOnly Property FirstRegistration() As registrationBasicInfo
  '            Get
  '                Return _firstRegistration
  '            End Get
  '        End Property

  '        Public Shared Function GetFirstRegistration(ByVal idVehic As Long) As registrationBasicInfo
  '            Dim result As FirstRegistration
  '            result = DataPortal.Execute(Of FirstRegistration)(New FirstRegistration(idVehic))
  '            Return result.FirstRegistration
  '        End Function

  '        Private Sub New(ByVal idVehic As Long)
  '            _idVehic = idVehic
  '        End Sub


  '        Protected Overrides Sub DataPortal_Execute()
  '            Using cn As SqlConnection = Database.VTE_SqlConnection
  '                Using cm As SqlCommand = cn.CreateCommand
  '                    cm.CommandType = CommandType.StoredProcedure
  '                    cm.CommandText = "getVehicleFirstRegistration"
  '                    cm.Parameters.AddWithValue("@idVehicle", _idVehic)
  '                    Using dr As New SafeDataReader(cm.ExecuteReader)
  '                        If dr.Read Then
  '                            _firstRegistration = _
  '                              New registrationBasicInfo( _
  '                              dr.GetString("RegistrationNumber"), _
  '                              dr.GetDateTime("DateOfRegistration"), _
  '                              dr.GetString("PlaceOfRegistration"), _
  '                              dr.GetDateTime("DateRegistrationValidTill"), _
  '                              dr.GetInt32("IdRegistrationIssuer"))

  '                        End If
  '                    End Using

  '                End Using
  '            End Using
  '        End Sub

  '    End Class


  '#End Region

#Region " VehicleTrafficLicenceNumber "

  Public Shared Function GetTrafficLicenceNumber(ByVal idVehic As Long) As String

    Dim result As String
    result = TrafficLicenceNumber.GetTrafficLicenceNumber(idVehic)
    If result Is Nothing Then
      Return (My.Resources.Nema)
    Else
      Return result

    End If

  End Function


  <Serializable()> _
  Private Class TrafficLicenceNumber
    Inherits CommandBase

    Private _idVehic As Long
    Private _trafficLicenceNumber As String
    Public ReadOnly Property TrafficLicenceNumber() As String
      Get
        Return _trafficLicenceNumber
      End Get
    End Property

    Public Shared Function GetTrafficLicenceNumber(ByVal idVehic As Long) As String
      Dim result As TrafficLicenceNumber
      result = DataPortal.Execute(Of TrafficLicenceNumber)(New TrafficLicenceNumber(idVehic))
      Return result.TrafficLicenceNumber
    End Function

    Private Sub New(ByVal idVehic As Long)
      _idVehic = idVehic
    End Sub


    Protected Overrides Sub DataPortal_Execute()
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getDocumentsTrafficLicencesNumberByIdVehicle"
          cm.Parameters.AddWithValue("@idVehicle", _idVehic)
          _trafficLicenceNumber = cm.ExecuteScalar
        End Using
      End Using
    End Sub

  End Class


#End Region

  '#Region " VehicleLastRegistrations "


  '    Public Shared Function GetVehicleLastRegistration(ByVal idVehic As Long) As registrationBasicInfo
  '        Dim result As registrationBasicInfo
  '        result = LastRegistration.GetLastRegistration(idVehic)
  '        If result Is Nothing Then
  '            Return New registrationBasicInfo("нема", New Date(1, 1, 1), "", New Date(1, 1, 1), 1)
  '        Else
  '            Return result

  '        End If

  '    End Function

  '    <Serializable()> _
  '    Private Class LastRegistration
  '        Inherits CommandBase

  '        Private _idVehic As Long
  '        Private _LastRegistration As registrationBasicInfo
  '        Public ReadOnly Property LastRegistration() As registrationBasicInfo
  '            Get
  '                Return _LastRegistration
  '            End Get
  '        End Property

  '        Public Shared Function GetLastRegistration(ByVal idVehic As Long) As registrationBasicInfo
  '            Dim result As LastRegistration
  '            result = DataPortal.Execute(Of LastRegistration)(New LastRegistration(idVehic))
  '            Return result.LastRegistration
  '        End Function

  '        Private Sub New(ByVal idVehic As Long)
  '            _idVehic = idVehic
  '        End Sub

  '        Protected Overrides Sub DataPortal_Execute()
  '            Using cn As SqlConnection = Database.VTE_SqlConnection
  '                Using cm As SqlCommand = cn.CreateCommand
  '                    cm.CommandType = CommandType.StoredProcedure
  '                    cm.CommandText = "getVehicleLastRegistration"
  '                    cm.Parameters.AddWithValue("@idVehicle", _idVehic)
  '                    Using dr As New SafeDataReader(cm.ExecuteReader)
  '                        If dr.Read Then
  '                            _LastRegistration = _
  '                              New registrationBasicInfo( _
  '                              dr.GetString("RegistrationNumber"), _
  '                              dr.GetDateTime("DateOfRegistration"), _
  '                              dr.GetString("PlaceOfRegistration"), _
  '                              dr.GetDateTime("DateRegistrationValidTill"), _
  '                              dr.GetInt32("IdRegistrationIssuer"))

  '                        End If
  '                    End Using

  '                End Using
  '            End Using
  '        End Sub

  '    End Class

  '#End Region

#Region " VehicleLastOwner "


  Public Shared Function GetVehicleLastOwner(ByVal idVehic As Long) As ownerBasicInfo
    Dim result As ownerBasicInfo
    result = LastOwner.GetLastOwner(idVehic)
    If result Is Nothing Then
      Return New ownerBasicInfo(0, "", "", "")
    Else
      Return result

    End If

  End Function

  <Serializable()> _
  Private Class LastOwner
    Inherits CommandBase

    Private _idVehic As Long
    Private _ownerId As ownerBasicInfo
    Public ReadOnly Property LastOwner() As ownerBasicInfo
      Get
        Return _ownerId
      End Get
    End Property

    Public Shared Function GetLastOwner(ByVal idVehic As Long) As ownerBasicInfo
      Dim result As LastOwner
      result = DataPortal.Execute(Of LastOwner)(New LastOwner(idVehic))
      Return result.LastOwner
    End Function

    Private Sub New(ByVal idVehic As Long)
      _idVehic = idVehic
    End Sub

    Protected Overrides Sub DataPortal_Execute()
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getVehicleLastOwner"
          cm.Parameters.AddWithValue("@idVehicle", _idVehic)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            If dr.Read Then
              _ownerId = _
                New ownerBasicInfo(dr.GetInt64("IdCustomer"), _
                                   dr.GetString("CustomerFirstName"), _
                                   dr.GetString("CustomerSurname"), _
                                   dr.GetString("MB"))
            End If
          End Using

        End Using
      End Using
    End Sub

  End Class

#End Region

#Region " VehicleLastOwner "


  Public Shared Function GetVehicleCurrentOwner(ByVal idVehic As Long) As ownerBasicInfo
    Dim result As ownerBasicInfo
    result = CurrentOwnerCommand.GetCurrentOwner(idVehic)
    If result Is Nothing Then
      Return New ownerBasicInfo(0, "", "", "")
    Else
      Return result

    End If

  End Function

  <Serializable()> _
  Private Class CurrentOwnerCommand
    Inherits CommandBase

    Private _idVehic As Long
    Private _ownerId As ownerBasicInfo
    Public ReadOnly Property CurrentOwner() As ownerBasicInfo
      Get
        Return _ownerId
      End Get
    End Property

    Public Shared Function GetCurrentOwner(ByVal idVehic As Long) As ownerBasicInfo
      Dim result As CurrentOwnerCommand
      result = DataPortal.Execute(Of CurrentOwnerCommand)(New CurrentOwnerCommand(idVehic))
      Return result.CurrentOwner
    End Function

    Private Sub New(ByVal idVehic As Long)
      _idVehic = idVehic
    End Sub

    Protected Overrides Sub DataPortal_Execute()
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getVehicleCurrentOwner"
          cm.Parameters.AddWithValue("@idVehicle", _idVehic)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            If dr.Read Then
              _ownerId = _
                New ownerBasicInfo(dr.GetInt64("IdCustomer"), _
                                   dr.GetString("CustomerFirstName"), _
                                   dr.GetString("CustomerSurname"), _
                                   dr.GetString("MB"))
            End If
          End Using

        End Using
      End Using
    End Sub

  End Class

#End Region


  '#Region " Readonlylist refresh "
  '    Public Shared Event VehicleSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  '    Protected Shared Sub OnVehicleSaved(ByVal sender As Vehicle, ByVal e As Csla.Core.SavedEventArgs)
  '        RaiseEvent VehicleSaved(sender, e)
  '    End Sub
  '#End Region

End Class

Public Class registrationBasicInfo
  Private _registrationNumber As String
  Public Property RegistrationNumber() As String
    Get
      Return _registrationNumber
    End Get
    Set(ByVal value As String)
      _registrationNumber = value
    End Set
  End Property

  Private _registrationDate As Date
  Public Property RegistrationDate() As Date
    Get
      Return _registrationDate
    End Get
    Set(ByVal value As Date)
      _registrationDate = value
    End Set
  End Property
  Private _registrationPlace As String
  Public Property RegistrationPlace() As String
    Get
      Return _registrationPlace
    End Get
    Set(ByVal value As String)
      _registrationPlace = value
    End Set
  End Property
  Private _dateRegistrationValidTill As Date
  Public Property DateRegistrationValidTill() As Date
    Get
      Return _dateRegistrationValidTill
    End Get
    Set(ByVal value As Date)
      _dateRegistrationValidTill = value
    End Set
  End Property
  Private _idIssuer As Integer
  Public Property IDIssuer() As Integer
    Get
      Return _idIssuer
    End Get
    Set(ByVal value As Integer)
      _idIssuer = value
    End Set
  End Property

  Public Sub New(ByVal regNumber As String, ByVal regDate As Date, ByVal regPlace As String, ByVal regDateValid As Date, ByVal issuer As Integer)
    _registrationNumber = regNumber
    _registrationDate = regDate
    _registrationPlace = regPlace
    _dateRegistrationValidTill = regDateValid
    _idIssuer = issuer
  End Sub

End Class

Public Class ownerBasicInfo
  Private _ownerId As Long
  Public Property OwnerId() As Long
    Get
      Return _ownerId
    End Get
    Set(ByVal value As Long)
      _ownerId = value
    End Set
  End Property

  Private _ownerName As String
  Public Property OwnerName() As String
    Get
      Return _ownerName
    End Get
    Set(ByVal value As String)
      _ownerName = value
    End Set
  End Property

  Private _ownerMB As String
  Public Property OwnerMB() As String
    Get
      Return _ownerMB
    End Get
    Set(ByVal value As String)
      _ownerMB = value
    End Set
  End Property


  Public Sub New(ByVal ownerId As Long, ByVal ownerName As String, ByVal ownerSurname As String, ByVal ownerMB As String)
    _ownerId = ownerId
    If ownerSurname = String.Empty Then
      _ownerName = ownerName
    Else
      _ownerName = ownerSurname & " " & ownerName
    End If
    _ownerMB = ownerMB
  End Sub

End Class