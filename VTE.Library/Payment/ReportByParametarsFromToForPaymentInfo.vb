
<Serializable()> _
Public Class ReportByParametarsFromToForPaymentInfo
  Inherits ReadOnlyBase(Of ReportByParametarsFromToForPaymentInfo)

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

  Private _LastRegistratinNumber As String
  Public ReadOnly Property LastRegistratinNumber() As String
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _LastRegistratinNumber
    End Get
  End Property

  Private _LastRegistrationMakeDate As Date
  Public ReadOnly Property LastRegistrationMakeDate() As String
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Try
        Dim minDatum As DateTime = "1900-01-01 00:00:00.000"
        If _LastRegistrationMakeDate <> Date.MinValue AndAlso _LastRegistrationMakeDate > minDatum Then
          Return _LastRegistrationMakeDate
        Else
          Return ""
        End If

      Catch ex As Exception
        Return ""
      End Try
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

    
  Private _discount As Single
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

  Private _vehicleid As Long
  Public ReadOnly Property Vehicleid() As Long
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _vehicleid
    End Get
  End Property

  Private _prametarname As String
  Public ReadOnly Property Prametarname() As String
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _prametarname
    End Get
  End Property

  Public ReadOnly Property ParametarsFromTo()
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Dim pom As String = _prametarname
      If pom.Contains("{0}") Then
        pom = pom.Replace("{0}", _parametarfrom.ToString)
        pom = pom.Replace("{1}", _parametarto.ToString)
      End If
      Return pom
    End Get
  End Property

  Private _parametarfrom As Single
  Public ReadOnly Property Parametarfrom() As Decimal
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _parametarfrom
    End Get
  End Property

  Private _parametarto As Single
  Public ReadOnly Property Parametarto() As Decimal
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _parametarto
    End Get
  End Property

  Private _parametarsprice As Decimal
  Public ReadOnly Property Parametarsprice() As Decimal
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _parametarsprice
    End Get
  End Property

  Protected Overrides Function GetIdValue() As Object
    Return _vehicleid
  End Function

  Public ReadOnly Property ProverenIznos() As Decimal
    Get
      If Not _payedamount Or _prepayed Then
        Return FicalRound(_price - _price * _discount / 100)
      Else
        Return 0
      End If
    End Get
  End Property
  Public ReadOnly Property NaplatenIznos() As Decimal
    Get
      If _payedamount AndAlso Not _prepayed Then
        Return FicalRound(_price - _price * _discount / 100)
      Else
        Return 0
      End If
    End Get
  End Property

  Public ReadOnly Property BrNaVozilaZaProverenIznos() As Integer
    Get
      If _vehicleid > 0 Then
        If (Not _payedamount Or _prepayed) AndAlso _price > 0 Then
          Return 1
        Else
          Return 0
        End If
      Else
        Return 0
      End If
    End Get
  End Property
  Public ReadOnly Property BrNaVozilaNaplatenIznos() As Integer
    Get
      If _vehicleid > 0 Then
        If _payedamount AndAlso Not _prepayed AndAlso _price > 0 Then
          Return 1 '_vehicleid
        Else
          Return 0
        End If
      Else
        Return 0
      End If
    End Get
  End Property
  Private _Station As String
  Public ReadOnly Property Station() As String
    <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Get
      Return _Station
    End Get
  End Property
  Public ReadOnly Property SumAmount() As Decimal
    Get
      Return Math.Round(ProverenIznos + NaplatenIznos, 1)
    End Get
  End Property
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

  Friend Shared Function GetReportByParametarsFromToForPaymentInfo(ByVal dr As SafeDataReader) As ReportByParametarsFromToForPaymentInfo
    Return New ReportByParametarsFromToForPaymentInfo(dr)
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
        _paymentcategoryid = dr.GetInt32("PaymentCategoryId")
        _vehicleid = dr.GetInt64("VehicleId")
        _prametarname = dr.GetString("PrametarName")
        _parametarfrom = dr.GetValue("ParametarFrom")
        _parametarto = dr.GetValue("ParametarTo")
        _parametarsprice = dr.GetDecimal("ParametarsPrice")
        _LastRegistratinNumber = dr.GetString("LastRegistratinNumber")
        _LastRegistrationMakeDate = dr.GetDateTime("LastRegistrationMakeDate")
        _Station = dr.GetString("Station")
    End Sub

#End Region

End Class