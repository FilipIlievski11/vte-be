
<Serializable()> _
Public Class PaymentDocumentsInfo
  Inherits ReadOnlyBase(Of PaymentDocumentsInfo)
    

    Private _id As Long
    Public ReadOnly Property Id() As Long
        Get
            Return _id
        End Get
    End Property
    Private _idPaymentType As Integer
    Public ReadOnly Property IdPaymentType() As Integer
        Get
            Return _idPaymentType
        End Get
    End Property
    Private _idCustomerVehicleRelation As Long
    Public ReadOnly Property IdCustomerVehicleRelation() As Long
        Get
            Return _idCustomerVehicleRelation
        End Get
    End Property
    Private _idOperator As Integer
    Public ReadOnly Property IdOperator() As Integer
        Get
            Return _idOperator
        End Get
    End Property
    Private _datePay As Date
    Public ReadOnly Property DatePay() As Date
        Get
            Return _datePay
        End Get
    End Property
    Private _dateRequired As Date
    Public ReadOnly Property DateRequired() As Date
        Get
            Return _dateRequired
        End Get
    End Property
    Private _discount As Single
    Public ReadOnly Property Discount() As Single
        Get
            Return _discount
        End Get
    End Property
    Private _payed As Boolean
    Public ReadOnly Property Payed() As Boolean
        Get
            Return _payed
        End Get
    End Property
    Private _note As String
    Public ReadOnly Property Note() As String
        Get
            Return _note
        End Get
    End Property
    Private _documentNumber As String
    Public ReadOnly Property DocumentNumber() As String
        Get
            Return _documentNumber
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
        _idPaymentType = dr.GetInt32("IdPaymentType")
        _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
        _idOperator = dr.GetInt32("IdOperator")
        _datePay = dr.GetDateTime("DatePay")
        _dateRequired = dr.GetDateTime("DateRequired")
        _discount = dr.GetValue("Discount")
        _payed = dr.GetBoolean("Payed")
        _note = dr.GetString("Note")
        _documentNumber = dr.GetString("DocumentNumber")
    End Sub

End Class