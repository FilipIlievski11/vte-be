
<Serializable()> _
Public Class ReportByCategoryForPaymentInfo
    Inherits ReadOnlyBase(Of ReportByCategoryForPaymentInfo)

#Region " Business Properties and Methods "

    Private _communityname As String
    Public ReadOnly Property Communityname() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _communityname
        End Get
    End Property

    Private _vehiclecategorycode As String
    Public ReadOnly Property Vehiclecategorycode() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _vehiclecategorycode
        End Get
    End Property

    Private _vehiclecategoryname As String
    Public ReadOnly Property Vehiclecategoryname() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _vehiclecategoryname
        End Get
    End Property

    Private _datepay As Date
    Public ReadOnly Property Datepay() As Date
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _datepay
        End Get
    End Property

    Private _price As Decimal
    Public ReadOnly Property Price() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _price
        End Get
    End Property


    'Public ReadOnly Property Price1() As Decimal
    '    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    '    Get
    '        Return FicalRound(_price * 1 / 100)
    '    End Get
    'End Property

    'Public ReadOnly Property Price99() As Decimal
    '    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    '    Get
    '        Return _price - (FicalRound(_price * 1 / 100))
    '    End Get
    'End Property

    Private _discount As Decimal
    Public ReadOnly Property Discount() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _discount
        End Get
    End Property

    Private _prepayed As Boolean
    Public ReadOnly Property Prepayed() As Boolean
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _prepayed
        End Get
    End Property

    Private _payedamount As Boolean
    Public ReadOnly Property Payedamount() As Boolean
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _payedamount
        End Get
    End Property

    Private _paymentcategoryid As Integer
    Public ReadOnly Property Paymentcategoryid() As Integer
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _paymentcategoryid
        End Get
    End Property
    Public ReadOnly Property ProverenIznos() As Decimal
        Get
            If Not _payedamount Or _prepayed Then
                Return Math.Round((_price - _price * _discount / 100), 1)
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property NaplatenIznos() As Decimal
        Get
            If _payedamount AndAlso Not _prepayed Then
                Return Math.Round((_price - _price * _discount / 100), 1)
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property SumAmount() As Decimal
        Get
            Return Math.Round(ProverenIznos + NaplatenIznos, 1)
        End Get
    End Property
    Private _vehicleid As Long
    Public ReadOnly Property Vehicleid() As Long
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _vehicleid
        End Get
    End Property

    Protected Overrides Function GetIdValue() As Object
        Return _vehicleid
    End Function

    Public ReadOnly Property Price1Naplaten() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Dim naplaten As Decimal = NaplatenIznos()

            Return Math.Round((naplaten * 1 / 100), 2)
        End Get
    End Property

    Public ReadOnly Property Price99Naplaten() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Dim naplaten As Decimal = NaplatenIznos()
            Return Math.Round((naplaten - (naplaten * 1 / 100)), 2)
        End Get
    End Property
    Public ReadOnly Property Price1Proveren() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Dim proveren As Decimal = ProverenIznos()
            Return Math.Round((proveren * 1 / 100), 2)
        End Get
    End Property

    Public ReadOnly Property Price99Proveren() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Dim proveren As Decimal = ProverenIznos()
            Return Math.Round((proveren - (proveren * 1 / 100)), 2)
        End Get
    End Property

#End Region

#Region " Factory Methods "

    Friend Shared Function GetReportByCategoryForPaymentInfo(ByVal dr As SafeDataReader) As ReportByCategoryForPaymentInfo
        Return New ReportByCategoryForPaymentInfo(dr)
    End Function

    Private Sub New(ByVal dr As SafeDataReader)
        Fetch(dr)
    End Sub


#End Region

#Region " Data Access - Fetch "

    Private Sub Fetch(ByVal dr As SafeDataReader)
        _communityname = dr.GetString("CommunityName")
        _vehiclecategorycode = dr.GetString("VehicleCategoryCode")
        _vehiclecategoryname = dr.GetString("VehicleCategoryName")
        _datepay = dr.GetDateTime("DatePay")
        _price = dr.GetDecimal("Price")
        _discount = dr.GetValue("Discount")
        _prepayed = dr.GetBoolean("PrePayed")
        _payedamount = dr.GetBoolean("PayedAmount")
        _paymentcategoryid = dr.GetInt32("IdPymentCategory")
        _vehicleid = dr.GetInt64("VehicleId")
    End Sub

#End Region

End Class