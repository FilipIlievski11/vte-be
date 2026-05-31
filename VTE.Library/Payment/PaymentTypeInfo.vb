
<Serializable()> _
Public Class PaymentTypeInfo
 Inherits ReadOnlyBase(Of PaymentTypeInfo)

 Private _id As Integer
 Public ReadOnly Property Id() As Integer
  Get
   Return _id
  End Get
 End Property
 Private _idCompany As Integer
 Public ReadOnly Property IdCompany() As Integer
  Get
   Return _idCompany
  End Get
 End Property
 Private _name As String
 Public ReadOnly Property Name() As String
  Get
   Return _name
  End Get
 End Property
 Private _fiskalnaKes As Boolean
 Public ReadOnly Property FiskalnaKes() As Boolean
  Get
   Return _fiskalnaKes
  End Get
 End Property
 Private _fiskalnaKarticka As Boolean
 Public ReadOnly Property FiskalnaKarticka() As Boolean
  Get
   Return _fiskalnaKarticka
  End Get
 End Property
 Private _rati As Boolean
 Public ReadOnly Property Rati() As Boolean
  Get
   Return _rati
  End Get
 End Property
 Private _smetka As Boolean
 Public ReadOnly Property Smetka() As Boolean
  Get
   Return _smetka
  End Get
 End Property
 Private _faktura As Boolean
 Public ReadOnly Property Faktura() As Boolean
  Get
   Return _faktura
  End Get
 End Property
 Private _printText As String
 Public ReadOnly Property PrintText() As String
  Get
   Return _printText
  End Get
 End Property
 Private _prefix As String
 Public ReadOnly Property Prefix() As String
  Get
   Return _prefix
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
  _idCompany = dr.GetInt32("IdCompany")
  _name = dr.GetString("Name")
  _fiskalnaKes = dr.GetBoolean("Fiskalna_kes")
  _fiskalnaKarticka = dr.GetBoolean("Fiskalna_karticka")
  _rati = dr.GetBoolean("Rati")
  _smetka = dr.GetBoolean("Smetka")
  _faktura = dr.GetBoolean("Faktura")
  _printText = dr.GetString("PrintText")
  _prefix = dr.GetString("Prefix")
 End Sub

End Class