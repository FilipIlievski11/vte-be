

<Serializable()> _
Public Class PayDocInfo
  Inherits ReadOnlyBase(Of PayDocInfo)

#Region " Business Properties and Methods "

  Private _Name As String
  Public ReadOnly Property Name() As String
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _Name
    End Get
  End Property

  Private _DocumentNumber As String
  Public ReadOnly Property DocumentNumber() As String
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _DocumentNumber
    End Get
  End Property

  Private _payed As Boolean
  Public ReadOnly Property Payed() As Boolean
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _payed
    End Get
  End Property

  Private _customer As String
  Public ReadOnly Property Customer() As String
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _customer
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

  Private _LastRegistratinNumber As String
  Public ReadOnly Property LastRegistratinNumber() As String
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _LastRegistratinNumber
    End Get
  End Property

  Private _DatePay As Date
  Public ReadOnly Property DatePay() As Date
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _DatePay.Date
    End Get
  End Property

  Private _DateRequired As Date
  Public ReadOnly Property DateRequired() As Date
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _DateRequired.Date
    End Get
  End Property
  Private _id As Long
  Public ReadOnly Property Id() As Long
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _id
    End Get
  End Property

  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function



#End Region

#Region " Factory Methods "

  Friend Shared Function GetPayDocInfo(ByVal dr As SafeDataReader) As PayDocInfo
    Return New PayDocInfo(dr)
  End Function

  Private Sub New(ByVal dr As SafeDataReader)
    Fetch(dr)
  End Sub


#End Region

#Region " Data Access - Fetch "

  Private Sub Fetch(ByVal dr As SafeDataReader)
    _DocumentNumber = dr.GetString("DocumentNumber")
    _payed = dr.GetBoolean("Payed")
    _customer = dr.GetString("Customer")
    _mb = dr.GetString("MB")
    _shellnumber = dr.GetString("ShellNumber")
    _LastRegistratinNumber = dr.GetString("LastRegistratinNumber")
    _DatePay = dr.GetDateTime("DatePay")
    _DateRequired = dr.GetDateTime("DateRequired")
    _id = dr.GetInt64("Id")
  End Sub

#End Region

End Class

