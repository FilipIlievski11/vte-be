Imports VTE
<Serializable()> _
Public Class DocumentsTehnicalExamsReportInfo
  Inherits ReadOnlyBase(Of DocumentsTehnicalExamsReportInfo)

  Private _id As Long
  Public ReadOnly Property Id() As Long
    Get
      Return _id
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
 
  Private _customerSurname As String
  Public ReadOnly Property CustomerSurname() As String
    Get
      Return _customerSurname
    End Get
  End Property
  Public ReadOnly Property CustomerName() As String
    Get
      Return UCase(_customerFirstName & " " & _customerSurname)
    End Get
  End Property
  Private _customerFirstName As String
  Public ReadOnly Property CustomerFirstName() As String
    Get
      Return _customerFirstName
    End Get
  End Property
  Private _isCompany As Boolean
  Public ReadOnly Property IsCompany() As Boolean
    Get
      Return _isCompany
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
  Private _isSocialNotPrivate As Boolean
  Public ReadOnly Property IsSocialNotPrivate() As Boolean
    Get
      Return _isSocialNotPrivate
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
  Private _idCustomerVehicleRelation As Long
  Public ReadOnly Property IdCustomerVehicleRelation() As Long
    Get
      Return _idCustomerVehicleRelation
    End Get
  End Property
  Private _idVehicle As Long
  Public ReadOnly Property IdVehicle() As Long
    Get
      Return _idVehicle
    End Get
  End Property
  Private _lastRegistrationNumber As String
  Public ReadOnly Property LastRegistration() As String
    Get
      Return _lastRegistrationNumber
    End Get
  End Property

  Private _regNumber As String
  Public ReadOnly Property RegNumber() As String
    Get
      Return _regNumber
    End Get
  End Property

  Private _TehnicalExamType As String
  Public ReadOnly Property TehnicalExamType() As String
    Get
      Return _TehnicalExamType
    End Get
  End Property

  Private _VehicleModel As String
  Public ReadOnly Property VehicleModel() As String
    Get
      Return _VehicleModel
    End Get
  End Property

  Private _VahiceMaker As String
  Public ReadOnly Property VahiceMaker() As String
    Get
      Return _VahiceMaker
    End Get
  End Property
  Private _note As String
  Public ReadOnly Property Note() As String
    Get
      Return _note
    End Get
  End Property
  Private _detali As String
  Public ReadOnly Property Detali() As String
    Get
      Return _detali
    End Get
  End Property
  Public ReadOnly Property DriversWarningYesNo() As String
    Get
      Try
        If _detali.Length > 0 OrElse _driversWarning <> String.Empty Then
          Return My.Resources.Yes
        Else
          Return My.Resources.No
        End If
      Catch ex As Exception
        Return My.Resources.Yes
      End Try
     

    End Get
  End Property
  Public ReadOnly Property ZavrsenaPostapka() As String
    Get
      If Detali = String.Empty Then
        If _vehicleIsRight Then
          Return My.Resources.Yes
        Else
          Return My.Resources.No
        End If
      Else
        If _vehicleIsRight Then
          Return My.Resources.No & "/" & My.Resources.Yes
        Else
          Return My.Resources.No
        End If
      End If
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
  Public ReadOnly Property CategoryCodeName() As String
    Get
      Return _categoryCode & "-" & _categoryName
    End Get
  End Property
  Private _maxDte As Date
  Public ReadOnly Property MaxDate() As Date
    Get
      Return _maxDte
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
    _madeDate = dr.GetDateTime("MadeDate")
    _validTillDate = dr.GetDateTime("ValidTillDate")
    _idOrganizationForTehnicalExam = dr.GetInt32("IdOrganizationForTehnicalExam")
    _idFirsControler = dr.GetInt32("IdFirsControler")
    _idSecondControler = dr.GetInt32("IdSecondControler")
    _vehicleIsRight = dr.GetBoolean("VehicleIsRight")
    _explanationNote = dr.GetString("ExplanationNote")
    _driversWarning = dr.GetString("DriversWarning")
    _customerSurname = dr.GetString("CustomerSurname")
    _customerFirstName = dr.GetString("CustomerFirstName")
    _isCompany = dr.GetBoolean("IsCompany")
    _shellNumber = dr.GetString("ShellNumber")
    _engineNumber = dr.GetString("EngineNumber")
    _isSocialNotPrivate = dr.GetBoolean("IsSocialNotPrivate")
    _forPrivateTransportNotPublic = dr.GetBoolean("ForPrivateTransportNotPublic")
    _idTypeOfTehnicalExam = dr.GetInt32("IdTypeOfTehnicalExam")
    _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
    _idVehicle = dr.GetInt64("IdVehicle")

    _regNumber = dr.GetString("RegNumber")
    _VahiceMaker = dr.GetString("VahiceMaker")
    _VehicleModel = dr.GetString("VehicleModel")
    _TehnicalExamType = dr.GetString("TehnicalExamType")
    _note = dr.GetString("Note")
    _categoryCode = dr.GetString("CategoryCode")
    _categoryName = dr.GetString("CategoryName")
    Try
      Dim pomDetali As DocumentsTehnicalExamsReportsDetails = _
DocumentsTehnicalExamsReport.GetDocumentsTehnicalExamsReport(_id).Details

      Dim DetailsString As String = String.Empty
      _maxDte = _madeDate
      Dim VehicleParts As TehnicalExamVehiclePartsList = CType(Csla.ApplicationContext.LocalContext.Item("objVehicleParts"), TehnicalExamVehiclePartsList)
      For Each detal As DocumentsTehnicalExamsReportsDetail In pomDetali
        ' If detal.IdStatus <> 1 Then

        DetailsString &= VehicleParts.GetTehnicalExamVehiclePartsListById(detal.IdTehnicalExamVehivlePart).Code & "; "
        If detal.DateEnter > _madeDate Then
          _maxDte = detal.DateEnter
        End If
        'End If
      Next
      _detali = DetailsString

    Catch ex As Exception

    End Try

  
    ' Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_idVehicle)
    _lastRegistrationNumber = dr.GetString("LastRegistratinNumber") 'regInfo.RegistrationNumber
  End Sub
  Friend Sub New(ByVal dr As SafeDataReader, ByVal isRight As Boolean)
    _id = dr.GetInt64("Id")
    _madeDate = dr.GetDateTime("MadeDate")
    _validTillDate = dr.GetDateTime("ValidTillDate")
    _idOrganizationForTehnicalExam = dr.GetInt32("IdOrganizationForTehnicalExam")
    _idFirsControler = dr.GetInt32("IdFirsControler")
    _idSecondControler = dr.GetInt32("IdSecondControler")
    _vehicleIsRight = dr.GetBoolean("VehicleIsRight")
    _explanationNote = dr.GetString("ExplanationNote")
    _driversWarning = dr.GetString("DriversWarning")
    _customerSurname = dr.GetString("CustomerSurname")
    _customerFirstName = dr.GetString("CustomerFirstName")
    _isCompany = dr.GetBoolean("IsCompany")
    _shellNumber = dr.GetString("ShellNumber")
    _engineNumber = dr.GetString("EngineNumber")
    _isSocialNotPrivate = dr.GetBoolean("IsSocialNotPrivate")
    _forPrivateTransportNotPublic = dr.GetBoolean("ForPrivateTransportNotPublic")
    _idTypeOfTehnicalExam = dr.GetInt32("IdTypeOfTehnicalExam")
    _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
    _idVehicle = dr.GetInt64("IdVehicle")

    _regNumber = dr.GetString("RegNumber")
    _VahiceMaker = dr.GetString("VahiceMaker")
    _VehicleModel = dr.GetString("VehicleModel")
    _TehnicalExamType = dr.GetString("TehnicalExamType")
    _note = dr.GetString("Note")
    _categoryCode = dr.GetString("CategoryCode")
    _categoryName = dr.GetString("CategoryName")
    '    Try
    '      Dim pomDetali As DocumentsTehnicalExamsReportsDetails = _
    'DocumentsTehnicalExamsReport.GetDocumentsTehnicalExamsReport(_id).Details

    '      Dim DetailsString As String = String.Empty
    '      _maxDte = _madeDate
    '      Dim VehicleParts As TehnicalExamVehiclePartsList = CType(Csla.ApplicationContext.LocalContext.Item("objVehicleParts"), TehnicalExamVehiclePartsList)
    '      For Each detal As DocumentsTehnicalExamsReportsDetail In pomDetali
    '        ' If detal.IdStatus <> 1 Then

    '        DetailsString &= VehicleParts.GetTehnicalExamVehiclePartsListById(detal.IdTehnicalExamVehivlePart).Code & "; "
    '        If detal.DateEnter > _madeDate Then
    '          _maxDte = detal.DateEnter
    '        End If
    '        'End If
    '      Next
    '      _detali = DetailsString

    '    Catch ex As Exception

    '    End Try


    ' Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_idVehicle)
    _lastRegistrationNumber = dr.GetString("LastRegistratinNumber") 'regInfo.RegistrationNumber
  End Sub

End Class