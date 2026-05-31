
<Serializable()> _
Public Class PaymentDocumentFiscalPrintInfo
  Inherits ReadOnlyBase(Of PaymentDocumentFiscalPrintInfo)

  Private _idPaymentDocument As Long
  Public ReadOnly Property IdPaymentDocument() As Long
    Get
      Return _idPaymentDocument
    End Get
  End Property
  Private _datePay As Date
  Public ReadOnly Property DatePay() As Date
    Get
      Return _datePay
    End Get
  End Property
  Private _idPaymentType As Integer
  Public ReadOnly Property IdPaymentType() As Integer
    Get
      Return _idPaymentType
    End Get
  End Property
  Private _paymentType As String
  Public ReadOnly Property PaymentType() As String
    Get
      Return _paymentType
    End Get
  End Property
  Private _idDocumentDetal As Long
  Public ReadOnly Property IdDocumentDetal() As Long
    Get
      Return _idDocumentDetal
    End Get
  End Property
  Private _idPriceCatalog As Integer
  Public ReadOnly Property IdPriceCatalog() As Integer
    Get
      Return _idPriceCatalog
    End Get
  End Property
  Private _prametarName As String
  Public ReadOnly Property PrametarName() As String
    Get
      Return _prametarName
    End Get
  End Property
  Private _price As Decimal
  Public ReadOnly Property Price() As Decimal
    Get
            Return FicalRound(_price)
    End Get
  End Property
  Private _dDV As Single
  Public ReadOnly Property DDV() As Single
    Get
      Return _dDV
    End Get
  End Property
  Private _discount As Single
  Public ReadOnly Property Discount() As Single
    Get
      Return _discount
    End Get
  End Property

  Private _storno As Boolean
  Public ReadOnly Property Storno() As Boolean
    Get
      Return _storno
    End Get
  End Property

  Private _CategoryName As String
  Public ReadOnly Property CategoryName() As String
    Get
      Return _CategoryName
    End Get
  End Property

  Protected Overrides Function GetIdValue() As Object
    Return _idpaymentdocument
  End Function

  Public Overrides Function ToString() As String
    Return _idpaymentdocument
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _idpaymentdocument = dr.GetInt64("IdPaymentDocument")
    _datepay = dr.GetDateTime("DatePay")
    _idpaymenttype = dr.GetInt32("IdPaymentType")
    _paymenttype = dr.GetString("PaymentType")
    _iddocumentdetal = dr.GetInt64("IdDocumentDetal")
    _idpricecatalog = dr.GetInt32("IdPriceCatalog")
    _prametarname = dr.GetString("PrametarName")
    _price = dr.GetDecimal("Price")
    _dDV = dr.GetValue("DDV")
    _discount = dr.GetValue("Discount")
    _storno = dr.GetValue("Storno")
    _CategoryName = dr.GetString("CategoryName")
  End Sub

  Public Sub AddPrice(ByVal value As Decimal)
    _price += value
  End Sub


End Class