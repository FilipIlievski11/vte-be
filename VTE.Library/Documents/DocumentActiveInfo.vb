
<Serializable()> _
Public Class DocumentActiveInfo
  Inherits ReadOnlyBase(Of DocumentActiveInfo)

  Private _id As Long
  Public ReadOnly Property Id() As Long
    Get
      Return _id
    End Get
  End Property
  Private _idDocumentType As Integer
  Public ReadOnly Property IdDocumentType() As Integer
    Get
      Return _idDocumentType
    End Get
  End Property
  Private _idCustomerVehicleRelation As Long
  Public ReadOnly Property IdCustomerVehicleRelation() As Long
    Get
      Return _idCustomerVehicleRelation
    End Get
  End Property
  Private _idOperatorCreated As Integer
  Public ReadOnly Property IdOperatorCreated() As Integer
    Get
      Return _idOperatorCreated
    End Get
  End Property
  Private _idOperatorModified As Integer
  Public ReadOnly Property IdOperatorModified() As Integer
    Get
      Return _idOperatorModified
    End Get
  End Property
  Private _idOperatorEnded As Integer
  Public ReadOnly Property IdOperatorEnded() As Integer
    Get
      Return _idOperatorEnded
    End Get
  End Property
  Private _IdTechnicalExamReport As Long
  Public ReadOnly Property IdTechnicalExamReport() As Long
    Get
      Return _IdTechnicalExamReport
    End Get
  End Property
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
  Private _vehicleOwnershipProof As String
  Public ReadOnly Property VehicleOwnershipProof() As String
    Get
      Return _vehicleOwnershipProof
    End Get
  End Property
  Private _idPayAttachment As Long
  Public ReadOnly Property IdPayAttachment() As Long
    Get
      Return _idPayAttachment
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
  Private _documentTypeName As String
  Public ReadOnly Property DocumentTypeName() As String
    Get
      Return _documentTypeName
    End Get
  End Property


  Private _customerSurname As String
  Private _customerFirstName As String
  Public ReadOnly Property CustomerName() As String
    Get
      If _customerSurname = String.Empty Then
        Return _customerFirstName
      Else
        Return _customerSurname & ", " & _customerFirstName
      End If
    End Get
  End Property

  Private _isCompany As Boolean
  Public ReadOnly Property IsCompany() As Boolean
    Get
      Return _isCompany
    End Get
  End Property

  Private _engineNumber As String
  Public ReadOnly Property EngineNumber() As String
    Get
      Return _engineNumber
    End Get
  End Property

  Private _shellNumber As String
  Private _vehicleMaker As String
  Private _vehicleModel As String
  Private _vehicleLastRegistrationNumber As String

  Public ReadOnly Property VehicleDisplay() As String
    Get
      Return _shellNumber & " (" & _vehicleMaker & ", " & _vehicleModel & ") " & _vehicleLastRegistrationNumber
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
    _iddocumenttype = dr.GetInt32("IdDocumentType")
    _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
    _idoperatorcreated = dr.GetInt32("IdOperatorCreated")
    _idoperatormodified = dr.GetInt32("IdOperatorModified")
    _idoperatorended = dr.GetInt32("IdOperatorEnded")
    _IdTechnicalExamReport = dr.GetInt64("IdTechnicalExamReport")
    _datecreated = dr.GetDateTime("DateCreated")
    _datemodified = dr.GetDateTime("DateModified")
    _dateended = dr.GetDateTime("DateEnded")
    _vehicleownershipproof = dr.GetString("VehicleOwnershipProof")
    _idpayattachment = dr.GetInt64("IdPayAttachment")
    _note = dr.GetString("Note")
    _idcustomer = dr.GetInt64("IdCustomer")
    _idvehicle = dr.GetInt64("IdVehicle")
    _documenttypename = dr.GetString("DocumentTypeName")
    _customersurname = dr.GetString("CustomerSurname")
    _customerfirstname = dr.GetString("CustomerFirstName")
    _iscompany = dr.GetBoolean("IsCompany")
    _shellnumber = dr.GetString("ShellNumber")
    _engineNumber = dr.GetString("EngineNumber")

    _vehicleModel = dr.GetString("ModelName")
    _vehicleMaker = dr.GetString("CompanyName")
        ' Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_idVehicle)
        _vehicleLastRegistrationNumber = dr.GetString("LastRegistratinNumber") 'regInfo.RegistrationNumber
  End Sub

End Class