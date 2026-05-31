
<Serializable()> _
Public Class UnpayedDealsInfo
 Inherits ReadOnlyBase(Of UnpayedDealsInfo)

#Region " Business Properties and Methods "

 Private _rati As Boolean
 Public ReadOnly Property Rati() As Boolean
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _rati
  End Get
 End Property

 Private _id As Long
 Public ReadOnly Property Id() As Long
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _id
  End Get
 End Property

 Private _documentnumber As String
 Public ReadOnly Property Documentnumber() As String
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _documentnumber
  End Get
 End Property

 Private _payed As Boolean
 Public ReadOnly Property Payed() As Boolean
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _payed
  End Get
 End Property

 Private _customersurname As String
 Public ReadOnly Property Customersurname() As String
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _customersurname
  End Get
 End Property

 Private _customerfirstname As String
 Public ReadOnly Property Customerfirstname() As String
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _customerfirstname
  End Get
 End Property
 Public ReadOnly Property Customer() As String
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _customerfirstname & " " & _customersurname
  End Get
 End Property

 Private _mb As String
 Public ReadOnly Property Mb() As String
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _mb
  End Get
 End Property

 Private _shellnumber As String
 Public ReadOnly Property Shellnumber() As String
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _shellnumber
  End Get
 End Property

 Private _OstanatoZaPlakanje As Double
 Public ReadOnly Property OstanatoZaPlakanje() As Double
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _OstanatoZaPlakanje
  End Get
 End Property
 Private _DatePay As Date
 Public ReadOnly Property DatePay() As Date
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _DatePay.Date
  End Get
 End Property


 Private _PhoneNumber As String
 Public ReadOnly Property PhoneNumber() As String
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _PhoneNumber
  End Get
 End Property

 Protected Overrides Function GetIdValue() As Object
  Return _id
 End Function



#End Region

#Region " Factory Methods "

 Friend Shared Function GetUnpayedDealsInfo(ByVal dr As SafeDataReader) As UnpayedDealsInfo
  Return New UnpayedDealsInfo(dr)
 End Function

 Private Sub New(ByVal dr As SafeDataReader)
  Fetch(dr)
 End Sub


#End Region

#Region " Data Access - Fetch "

 Private Sub Fetch(ByVal dr As SafeDataReader)
  _rati = dr.GetBoolean("Rati")
  _id = dr.GetInt64("Id")
  _documentnumber = dr.GetString("DocumentNumber")
  _payed = dr.GetBoolean("Payed")
  _customersurname = dr.GetString("CustomerSurname")
  _customerfirstname = dr.GetString("CustomerFirstName")
  _mb = dr.GetString("MB")
  _shellnumber = dr.GetString("ShellNumber")
  _OstanatoZaPlakanje = dr.GetValue("OstanatoZaPlakanje")
  _DatePay = dr.GetDateTime("DatePay")
  _PhoneNumber = dr.GetString("PhoneNumber")
 End Sub

#End Region

End Class