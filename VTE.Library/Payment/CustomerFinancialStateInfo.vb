
<Serializable()> _
Public Class CustomerFinancialStateInfo
  Inherits ReadOnlyBase(Of CustomerFinancialStateInfo)

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
  Private _idDocument As Long
  Public ReadOnly Property IdDocument() As Long
    Get
      Return _idDocument
    End Get
  End Property
  Private _idDocumentTehnicalExam As Long
  Public ReadOnly Property IdDocumentTehnicalExam() As Long
    Get
      Return _idDocumentTehnicalExam
    End Get
  End Property
  Private _idDocumentsTrafficLicences As Long
  Public ReadOnly Property IdDocumentsTrafficLicences() As Long
    Get
      Return _idDocumentsTrafficLicences
    End Get
  End Property
  Private _idDocumentIternationalDriveingLicence As Long
  Public ReadOnly Property IdDocumentIternationalDriveingLicence() As Long
    Get
      Return _idDocumentIternationalDriveingLicence
    End Get
  End Property
  Private _idDocumentPermisions As Long
  Public ReadOnly Property IdDocumentPermisions() As Long
    Get
      Return _idDocumentPermisions
    End Get
  End Property
  Private _idPriceCatalog As Integer
  Public ReadOnly Property IdPriceCatalog() As Integer
    Get
      Return _idPriceCatalog
    End Get
  End Property
  Private _note As String
  Public ReadOnly Property Note() As String
    Get
      Return _note
    End Get
  End Property
  Private _price As Decimal
  Public ReadOnly Property Price() As Decimal
    Get
      Return _price
    End Get
  End Property
  Private _payed As Boolean
  Public ReadOnly Property Payed() As Boolean
    Get
      Return _payed
    End Get
  End Property
  Private _shellNumber As String
  Public ReadOnly Property ShellNumber() As String
    Get
      Return _shellNumber
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
  Public ReadOnly Property CustomerName() As String
    Get
      Return _customerFirstName & " " & _customerSurname
    End Get
  End Property
  Private _name As String
  Public ReadOnly Property Name() As String
    Get
      Return _name
    End Get
  End Property
  Private _idDDVCatalog As Integer
  Public ReadOnly Property IdDDVCatalog() As Integer
    Get
      Return _idDDVCatalog
    End Get
  End Property
  Private _dDVName As String
  Public ReadOnly Property DDVName() As String
    Get
      Return _dDVName
    End Get
  End Property
  Private _dDVValue As Decimal
  Public ReadOnly Property DDVValue() As Decimal
    Get
      Return _dDVValue
    End Get
  End Property
  Public ReadOnly Property DDVPriceValue() As Decimal
    Get
      Return Math.Round(_price * (_dDVValue / 100), 0)
    End Get
  End Property
  Public ReadOnly Property PriceWithDDV() As Decimal
    Get
      Return Math.Round(_price * (1 + _dDVValue / 100), 0)
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
        Try

            _id = dr.GetInt64("Id")
            _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
            _idDocument = dr.GetInt64("IdDocument")
            _idDocumentTehnicalExam = dr.GetInt64("IdDocumentTehnicalExam")
            _idDocumentsTrafficLicences = dr.GetInt64("IdDocumentsTrafficLicences")
            _idDocumentIternationalDriveingLicence = dr.GetInt64("IdDocumentIternationalDriveingLicence")
            _idDocumentPermisions = dr.GetInt64("IdDocumentPermisions")
            _idPriceCatalog = dr.GetInt32("IdPriceCatalog")
            _note = dr.GetString("Note")
            _price = dr.GetDecimal("Price")
            _payed = dr.GetBoolean("Payed")
            _shellNumber = dr.GetString("ShellNumber")
            _customerSurname = dr.GetString("CustomerSurname")
            _customerFirstName = dr.GetString("CustomerFirstName")

            'ime na PriceItem
            Dim pInfo As PaymentCataologInfo = _
              CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
              PaymentCataologList).GetInfo(_idPriceCatalog)

            _name = pInfo.PaymentName

            _idDDVCatalog = pInfo.IdDDV
            Dim ddvL As DDVList = CType(Csla.ApplicationContext.LocalContext.Item("objDDVList"), DDVList)
            _dDVName = ddvL.GetInfo(pInfo.IdDDV).DDVName
            _dDVValue = ddvL.GetInfo(pInfo.IdDDV).DDVValue

        Catch ex As Exception

        End Try
    End Sub

End Class