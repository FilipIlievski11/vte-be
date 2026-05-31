
<Serializable()> _
Public Class printShortPivotPaymentDocumentByDateInfo
    Inherits ReadOnlyBase(Of printShortPivotPaymentDocumentByDateInfo)

#Region " Business Properties and Methods "

    Private _id As Long
    Public ReadOnly Property Id() As Long
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _id
        End Get
    End Property

    Private _idoperator As Integer
    Public ReadOnly Property Idoperator() As Integer
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _idoperator
        End Get
    End Property
    Private _IdOperatorPoDogovor As Integer
    Public ReadOnly Property IdOperatorPoDogovor() As Integer
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _IdOperatorPoDogovor
        End Get
    End Property

    Private _documentnumber As String
    Public ReadOnly Property Documentnumber() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _documentnumber
        End Get
    End Property

    Private _datepay As Date
    Public ReadOnly Property Datepay() As Date
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _datepay
        End Get
    End Property

    Private _payed As Boolean
    Public ReadOnly Property Payed() As Boolean
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _payed
        End Get
    End Property

    Private _storno As Boolean
    Public ReadOnly Property Storno() As Boolean
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _storno
        End Get
    End Property

    Private _paydocnote As String
    Public ReadOnly Property Paydocnote() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _paydocnote
        End Get
    End Property

    Private _name As String
    Public ReadOnly Property Name() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _name
        End Get
    End Property

    Private _rati As Boolean
    Public ReadOnly Property Rati() As Boolean
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _rati
        End Get
    End Property
    Private _operatorName As String
    Public ReadOnly Property OperatorName() As String
        Get
            Return _operatorName
        End Get
    End Property
    Private _operatorNameStation As String
    Public ReadOnly Property OperatorNameStation() As String
        Get
            Return _operatorNameStation
        End Get
    End Property
    Private _operatorPoDogovorName As String
    Public ReadOnly Property OperatorPoDogovorName() As String
        Get
            Return _operatorPoDogovorName
        End Get
    End Property
    Private _rataprice As Decimal
    Public ReadOnly Property Rataprice() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _rataprice
        End Get
    End Property

    Private _ratapayed As Boolean
    Public ReadOnly Property Ratapayed() As Boolean
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _ratapayed
        End Get
    End Property

    Private _ratadatepayed As Date
    Public ReadOnly Property Ratadatepayed() As Date
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _ratadatepayed
        End Get
    End Property


    Private _CustomerSurname As String
    Public ReadOnly Property CustomerSurname() As String
        Get
            Return _CustomerSurname
        End Get
    End Property
    Private _CustomerFirstName As String
    Public ReadOnly Property CustomerFirstName() As String
        Get
            Return _CustomerFirstName
        End Get
    End Property
    Public ReadOnly Property CustomerName() As String
        Get
            Return _CustomerFirstName & " " & _CustomerSurname
        End Get
    End Property
    Private _LastRegistratinNumber As String
    Public ReadOnly Property LastRegistratinNumber() As String
        Get
            Return _LastRegistratinNumber
        End Get
    End Property

    Protected Overrides Function GetIdValue() As Object
        Return _id
    End Function

#End Region

#Region " Factory Methods "

    Friend Shared Function GetprintShortPivotPaymentDocumentByDateInfo(ByVal dr As SafeDataReader) As printShortPivotPaymentDocumentByDateInfo
        Return New printShortPivotPaymentDocumentByDateInfo(dr)
    End Function

    Private Sub New(ByVal dr As SafeDataReader)
        Fetch(dr)
    End Sub


#End Region

#Region " Data Access - Fetch "

    Private Sub Fetch(ByVal dr As SafeDataReader)
        _id = dr.GetInt64("Id")
        _idoperator = dr.GetInt32("IdOperator")
        _documentnumber = dr.GetString("DocumentNumber")
        _datepay = dr.GetDateTime("DatePay")
        _payed = dr.GetBoolean("Payed")
        _storno = dr.GetBoolean("Storno")
        _paydocnote = dr.GetString("PayDocNote")
        _name = dr.GetString("Name")
        _rati = dr.GetBoolean("Rati")
        _rataprice = dr.GetDecimal("RataPrice")
        _ratapayed = dr.GetBoolean("RataPayed")
        _ratadatepayed = dr.GetDateTime("RataDatePayed")
        _IdOperatorPoDogovor = dr.GetInt32("IdOperatorPoDogovor")
        _CustomerSurname = dr.GetString("CustomerSurname")
        _CustomerFirstName = dr.GetString("CustomerFirstName")
        _LastRegistratinNumber = dr.GetString("LastRegistratinNumber")
    Try

      Dim objUsersInfo As UsersInfo = _
      CType(Csla.ApplicationContext.LocalContext("objUsersList"),  _
      UsersList).getInfoById(dr.GetInt32("IdOperator"))
      _operatorName = objUsersInfo.FullName
      _operatorNameStation = objUsersInfo.FullNameAndStation
      Dim objUsersInfo2 As UsersInfo = _
     CType(Csla.ApplicationContext.LocalContext("objUsersList"),  _
     UsersList).getInfoById(dr.GetInt32("IdOperatorPoDogovor"))
      _operatorPoDogovorName = objUsersInfo2.FullName

    Catch ex As Exception
      _operatorName = ""
      _operatorNameStation = ""
      _operatorPoDogovorName = ""
    End Try
    End Sub

#End Region

End Class
