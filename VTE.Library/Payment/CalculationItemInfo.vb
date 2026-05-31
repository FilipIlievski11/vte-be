
<Serializable()> _
Public Class CalculationItemInfo
  Inherits ReadOnlyBase(Of CalculationItemInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _itemName As String
  Public ReadOnly Property ItemName() As String
    Get
      Return _itemName
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
  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal id As Integer, ByVal itemName As String)
    _id = id
    _itemName = itemName
  End Sub

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("Id")
    _itemname = dr.GetString("ItemName")
    _bankaccount = dr.GetString("BankAccount")
    _bank = dr.GetString("Bank")
    _form = dr.GetString("Form")
  End Sub

End Class