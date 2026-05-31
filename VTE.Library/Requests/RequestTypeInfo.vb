
<Serializable()> _
Public Class RequestTypeInfo
  Inherits ReadOnlyBase(Of RequestTypeInfo)



  Private _id As Integer
  Public ReadOnly Property Id() As Integer
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
  Private _idDocumentPrint As Integer
  Public ReadOnly Property IdDocumentPrint() As Integer
    Get
      Return _idDocumentPrint
    End Get
  End Property
    Private _isTehnicalExamRequired As Integer
    Public ReadOnly Property IsTehnicalExamRequired() As Integer
        Get
            Return _isTehnicalExamRequired
        End Get
    End Property
  Private _isPayRequired As Boolean
  Public ReadOnly Property IsPayRequired() As Boolean
    Get
      Return _isPayRequired
    End Get
  End Property
  Private _isNewRegistration As Boolean
  Public ReadOnly Property IsNewRegistration() As Boolean
    Get
      Return _isNewRegistration
    End Get
  End Property
  Private _isRelationDeleted As Boolean
  Public ReadOnly Property IsRelationDeleted() As Boolean
    Get
      Return _isRelationDeleted
    End Get
  End Property
  Private _isVehicleDeleted As Boolean
  Public ReadOnly Property IsVehicleDeleted() As Boolean
    Get
      Return _isVehicleDeleted
    End Get
  End Property
  Private _isNewCustomer As Boolean
  Public ReadOnly Property IsNewCustomer() As Boolean
    Get
      Return _isNewCustomer
    End Get
  End Property
  Private _isVehicleChanged As Boolean
  Public ReadOnly Property IsVehicleChanged() As Boolean
    Get
      Return _isVehicleChanged
    End Get
  End Property
  Private _isCustomerChanged As Boolean
  Public ReadOnly Property IsCustomerChanged() As Boolean
    Get
      Return _isCustomerChanged
    End Get
  End Property
  Private _IsSufficient As Boolean
  Public ReadOnly Property IsSufficient() As Boolean
    Get
      Return _IsSufficient
    End Get
  End Property

  Private _typeName As String
  Public ReadOnly Property TypeName() As String
    Get
      Return _typeName
    End Get
  End Property
  Private _typeDescription As String
  Public ReadOnly Property TypeDescription() As String
    Get
      Return _typeDescription
    End Get
  End Property

  Private _IsPreviosRegistrationReqired As Boolean
  Public ReadOnly Property IsPreviosRegistrationReqired() As Boolean
    Get
      Return _IsPreviosRegistrationReqired
    End Get
  End Property

  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("Id")
    _idRequestType = dr.GetInt32("IdRequestType")
    _idDocumentPrint = dr.GetInt32("IdDocumentPrint")
        _isTehnicalExamRequired = dr.GetInt32("IsTehnicalExamRequired")
    _isPayRequired = dr.GetBoolean("IsPayRequired")
    _isNewRegistration = dr.GetBoolean("IsNewRegistration")
    _isRelationDeleted = dr.GetBoolean("IsRelationDeleted")
    _isVehicleDeleted = dr.GetBoolean("IsVehicleDeleted")
    _isNewCustomer = dr.GetBoolean("IsNewCustomer")
    _isVehicleChanged = dr.GetBoolean("IsVehicleChanged")
    _isCustomerChanged = dr.GetBoolean("IsCustomerChanged")
    _IsSufficient = dr.GetBoolean("IsSufficient")
    _IsPreviosRegistrationReqired = dr.GetBoolean("IsPreviosRegistrationReqired")
    _typeName = dr.GetString("TypeName")
    _typeDescription = dr.GetString("TypeDescription")
  End Sub

End Class