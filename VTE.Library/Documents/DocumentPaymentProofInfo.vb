
<Serializable()> _
Public Class DocumentPaymentProofInfo
  Inherits ReadOnlyBase(Of DocumentPaymentProofInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _paymentProofName As String
  Public ReadOnly Property PaymentProofName() As String
    Get
      Return _paymentProofName
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal intId As Integer, ByVal strPaymentProofName As String)
    _id = intId
    _paymentProofName = strPaymentProofName
  End Sub

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("Id")
    _paymentproofname = dr.GetString("PaymentProofName")
  End Sub

End Class