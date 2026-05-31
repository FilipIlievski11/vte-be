<Serializable()> _
Public Class CalculationInfo
  Inherits ReadOnlyBase(Of CalculationInfo)

  Private _calculationItem As String
  Public ReadOnly Property CalculationItem() As String
    Get
      Return _calculationItem
    End Get
  End Property

  Private _bankAccount As String
  Public ReadOnly Property BankAccount() As String
    Get
      Return _bankAccount
    End Get
  End Property

  Private _bank As String
  Public ReadOnly Property Bank() As String
    Get
      Return _bank
    End Get
  End Property

  Private _form As String
  Public ReadOnly Property Form() As String
    Get
      Return _form
    End Get
  End Property

  Private _amount As Double
  Public ReadOnly Property Amount() As Double
    Get
      Return _amount
    End Get
  End Property

  Friend Sub New(ByVal calculationItem As String, _
                 ByVal bankAccount As String, _
                 ByVal bank As String, _
                 ByVal form As String, _
                 ByVal amount As Double)
    _calculationItem = calculationItem
    _bankAccount = bankAccount
    _bank = bank
    _form = form
    _amount = amount
  End Sub

End Class
