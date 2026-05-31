
<Serializable()> _
Public Class PrintVehicleByIdWhiteRequestInfo
  Inherits ReadOnlyBase(Of PrintVehicleByIdWhiteRequestInfo)

  Private _id As Long
  Public ReadOnly Property Id() As Long
    Get
      Return _id
    End Get
  End Property
  Private _idVehicle As Long
  Public ReadOnly Property IdVehicle() As Long
    Get
      Return _idVehicle
    End Get
  End Property
  Private _modelName As String
  Public ReadOnly Property ModelName() As String
    Get
      Return _modelName
    End Get
  End Property
  Private _makeDate As Date
  Public ReadOnly Property MakeDate() As Date
    Get
      Return _makeDate
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
  Private _oldCategoryName As String
  Public ReadOnly Property OldCategoryName() As String
    Get
      Return _oldCategoryName
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
      Return _categoryName
    End Get
  End Property
  Private _mKSJUS As String
  Public ReadOnly Property MKSJUS() As String
    Get
      Return _mKSJUS
    End Get
  End Property
  Private _iSO As String
  Public ReadOnly Property ISO() As String
    Get
      Return _iSO
    End Get
  End Property
  Private _mKSJUSDescription As String
  Public ReadOnly Property MKSJUSDescription() As String
    Get
      Return _mKSJUSDescription
    End Get
  End Property
  Private _idCountry As Integer
  Public ReadOnly Property IdCountry() As Integer
    Get
      Return _idCountry
    End Get
  End Property
  Private _engineTypeCode As String
  Public ReadOnly Property EngineTypeCode() As String
    Get
      Return _engineTypeCode
    End Get
  End Property
  Private _techincalDescriptionEcoProgram As String
  Public ReadOnly Property TechincalDescriptionEcoProgram() As String
    Get
      Return _techincalDescriptionEcoProgram
    End Get
  End Property
  Private _shellNumber As String
  Public ReadOnly Property ShellNumber() As String
    Get
      Return _shellNumber
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

  Public ReadOnly Property CarringCapasity() As Single
    Get
      Return (_maximunAllowedWaight - _emptyWaight)
    End Get
  End Property
  Private _idEnginePowerSource As Integer
  Public ReadOnly Property IdEnginePowerSource() As Integer
    Get
      Return _idEnginePowerSource
    End Get
  End Property
  Private _idEngineSecondPowerSource As Integer
  Public ReadOnly Property IdEngineSecondPowerSource() As Integer
    Get
      Return _idEngineSecondPowerSource
    End Get
  End Property
  Private _useDescription As String
  Public ReadOnly Property UseDescription() As String
    Get
      Return _useDescription
    End Get
  End Property

  Public ReadOnly Property CompanyNameAndTrademark() As String
    Get
      Return _companyName & "-" & _companyTrademark
    End Get
  End Property

  Private _lastRegistrationNumber As String
  Public ReadOnly Property LastRegistration() As String
    Get
      Return _lastRegistrationNumber
    End Get
  End Property
    'Private _lastRegistrationPlace As String
    'Public ReadOnly Property LastRegistrationPlace() As String
    '  Get
    '    Return _lastRegistrationPlace
    '  End Get
    'End Property
  Private _firstRegistrationNumber As String
  Public ReadOnly Property FirstRegistration() As String
    Get
      Return _firstRegistrationNumber
    End Get
  End Property
  Private _firstRegistrationDate As Date
  Public ReadOnly Property FirstRegistrationDate() As Date
    Get
      Return _firstRegistrationDate
    End Get
  End Property
    'Private _firstRegistrationPlace As String
    'Public ReadOnly Property FirstRegistrationPlace() As String
    '  Get
    '    Return _firstRegistrationPlace
    '  End Get
    'End Property
  Private _dateOfLastRegistration As Date
  Public ReadOnly Property DateOfLastRegistrationa() As Date
    Get
      Return _dateOfLastRegistration
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
    _idvehicle = dr.GetInt64("IdVehicle")
    _modelname = dr.GetString("ModelName")
    _makedate = dr.GetDateTime("MakeDate")
    _companyname = dr.GetString("CompanyName")
    _companytrademark = dr.GetString("CompanyTrademark")
    _oldcategoryname = dr.GetString("OldCategoryName")
    _categorycode = dr.GetString("CategoryCode")
    _categoryname = dr.GetString("CategoryName")
    _mksjus = dr.GetString("MKSJUS")
    _iso = dr.GetString("ISO")
    _mksjusdescription = dr.GetString("MKSJUSDescription")
    _idcountry = dr.GetInt32("IdCountry")
    _enginetypecode = dr.GetString("EngineTypeCode")
    _techincaldescriptionecoprogram = dr.GetString("TechincalDescriptionEcoProgram")
    _shellnumber = dr.GetString("ShellNumber")
    _enginenumber = dr.GetString("EngineNumber")
    _enginePower = dr.GetValue("EnginePower")
    _enginePowerOutPut = dr.GetValue("EnginePowerOutPut")
    _engineWorkingCapacity = dr.GetValue("EngineWorkingCapacity")
    _emptyWaight = dr.GetValue("EmptyWaight")
    _maximunAllowedWaight = dr.GetValue("MaximunAllowedWaight")
    _idenginepowersource = dr.GetInt32("IdEnginePowerSource")
    _idenginesecondpowersource = dr.GetInt32("IdEngineSecondPowerSource")
    _useDescription = dr.GetString("UseDescription")
        'Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_id)
        _lastRegistrationNumber = dr.GetString("LastRegistratinNumber") 'regInfo.RegistrationNumber
        _dateOfLastRegistration = dr.GetDateTime("LastRegistrationMakeDate") 'regInfo.RegistrationDate
        '_lastRegistrationPlace = dr.GetString("LastRegistrationCommunity") 'regInfo.RegistrationPlace
        'Dim firstRegInfo As registrationBasicInfo = Vehicle.GetVehicleFirstRegistration(_id)
        _firstRegistrationNumber = dr.GetString("FirstRegistrationNumber") 'firstRegInfo.RegistrationNumber
        _firstRegistrationDate = dr.GetDateTime("FirstRegistrationMakeDate") 'firstRegInfo.RegistrationDate
        ' _firstRegistrationPlace = dr.GetString("FirstRegistrationCommunity") 'firstRegInfo.RegistrationPlace
  End Sub

End Class
