
<Serializable()> _
Public Class PaymentDocumentRataFiscalPrintInfo
    Inherits ReadOnlyBase(Of PaymentDocumentRataFiscalPrintInfo)

#Region " Business Properties and Methods "

    Private _idpaymentdocument As Long
    Public ReadOnly Property Idpaymentdocument() As Long
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _idpaymentdocument
        End Get
    End Property

    Private _IdRata As Long
    Public ReadOnly Property IdRata() As Long
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _IdRata
        End Get
    End Property
    Private _IdVehicle As Long
    Public ReadOnly Property IdVehicle() As Long
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _IdVehicle
        End Get
    End Property
    Private _storno As Boolean
    Public ReadOnly Property Storno() As Boolean
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _storno
        End Get
    End Property

    Private _broj As String
    Public ReadOnly Property Broj() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _broj
        End Get
    End Property
    Private _CustomerFirstName As String
    Public ReadOnly Property CustomerFirstName() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _CustomerFirstName
        End Get
    End Property
    Private _CustomerSurname As String
    Public ReadOnly Property CustomerSurname() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _CustomerSurname
        End Get
    End Property
    Private _MB As String
    Public ReadOnly Property MB() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _MB
        End Get
    End Property
    Private _LivingAddressNumber As String
    Public ReadOnly Property LivingAddressNumber() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _LivingAddressNumber
        End Get
    End Property
    Private _StreetName As String
    Public ReadOnly Property StreetName() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _StreetName
        End Get
    End Property
    Private _CityName As String
    Public ReadOnly Property CityName() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _CityName
        End Get
    End Property
    Private _DocumentNumber As String
    Public ReadOnly Property DocumentNumber() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _DocumentNumber
        End Get
    End Property
    Private _datum As Date
    Public ReadOnly Property Datum() As Date
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _datum
        End Get
    End Property

    Private _price As Decimal
    Public ReadOnly Property Price() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return FicalRound(_price)
        End Get
    End Property

    Private _payed As Boolean
    Public ReadOnly Property Payed() As Boolean
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _payed
        End Get
    End Property

    Private _datepayed As Date
    Public ReadOnly Property Datepayed() As Date
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _datepayed
        End Get
    End Property

    Private _note As String
    Public ReadOnly Property Note() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _note
        End Get
    End Property

    Private _IdDogovor As Long
    Public ReadOnly Property IdDogovor() As Long
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _IdDogovor
        End Get
    End Property
    Private _IdPaymentType As Integer
    Public ReadOnly Property IdPaymentType() As Integer
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _IdPaymentType
        End Get
    End Property

    Protected Overrides Function GetIdValue() As Object
        Return _idpaymentdocument
    End Function
#Region "Calculated items"
    'Private WithEvents _doc As PaymentDocument = PaymentDocument.GetPaymentDocument(_idpaymentdocument)
    Public ReadOnly Property VkupnaSuma()
        Get
            Dim sum As Decimal = 0
            Try
                Dim _doc As PaymentDocument = PaymentDocument.GetPaymentDocument(_idpaymentdocument)
                For Each detal As PaymentDocumentsDetail In _doc.PaymentDocumentDetails
                    If Not detal.PrePayed Then
                        sum += detal.Price - (detal.Price * detal.Discount / 100) '+ detal.Ddv / 100 * detal.Price
                    End If
                Next
            Catch ex As Exception
                sum = 0
            End Try
          
            Return FicalRound(sum)
        End Get
    End Property
 
    Public ReadOnly Property OstanataZaPlakanjeSuma()
        Get
            Dim sumPlateno As Decimal = 0
            Try
                Dim _doc As PaymentDocument = PaymentDocument.GetPaymentDocument(_idpaymentdocument)

                For Each rata As PaymentDocumentsRata In _doc.PaymentDocumentRati
                    If rata.Payed AndAlso rata.Id <= _IdRata Then
                        sumPlateno += rata.Price
                    End If
                Next
                sumPlateno = VkupnaSuma - sumPlateno
            Catch ex As Exception
                sumplateno = 0
            End Try
            Return Math.Round(sumPlateno, 0)
        End Get
    End Property

    Public ReadOnly Property CustomerDisplayName()
        Get
            Return _CustomerFirstName & " " & _CustomerSurname
        End Get
    End Property

    Public ReadOnly Property AddressDisplay()
        Get
            If _StreetName IsNot Nothing AndAlso _StreetName <> "" Then
                Return _StreetName & " " & _LivingAddressNumber & "; " & _CityName
            Else
                Return _CityName
            End If
            'Return "ул." & _StreetName & " " & "бр." & _LivingAddressNumber & "; " & _CityName
        End Get
    End Property
    Public ReadOnly Property SmetkaBr() As String
        Get
            Return _prefix & _broj & "/" & _datum.Year '" бр." & _DocumentNumber & "/" & _datum.Year
        End Get
    End Property
    Public ReadOnly Property PlatenaRata() As String
        Get
            If _payed Then
                Return "ДА"
            Else
                Return "НЕ"
            End If
        End Get
    End Property
    Private _prefix As String = ""
    Public ReadOnly Property Prefix() As String
        Get
           
            Return _prefix

        End Get
    End Property
#End Region
#End Region

#Region " Factory Methods "

    Friend Shared Function GetPaymentDocumentRataFiscalPrintInfo(ByVal dr As SafeDataReader) As PaymentDocumentRataFiscalPrintInfo
        Return New PaymentDocumentRataFiscalPrintInfo(dr)
    End Function

    Private Sub New(ByVal dr As SafeDataReader)
        Fetch(dr)
    End Sub


#End Region

#Region " Data Access - Fetch "

    Private Sub Fetch(ByVal dr As SafeDataReader)
        _idpaymentdocument = dr.GetInt64("IdPaymentDocument")
        _storno = dr.GetBoolean("Storno")
        _broj = dr.GetString("Broj")
        _datum = dr.GetDateTime("Datum")
        _price = dr.GetDecimal("Price")
        _payed = dr.GetBoolean("Payed")
        _datepayed = dr.GetDateTime("DatePayed")
        _note = dr.GetString("Note")
        _IdRata = dr.GetInt64("IdRata")
        _IdDogovor = dr.GetInt64("IdDogovor")
        _CustomerFirstName = dr.GetString("CustomerFirstName")
        _CustomerSurname = dr.GetString("CustomerSurname")
        _MB = dr.GetString("MB")
        _LivingAddressNumber = dr.GetString("LivingAddressNumber")
        _StreetName = dr.GetString("StreetName")
        _CityName = dr.GetString("CityName")
        _DocumentNumber = dr.GetString("DocumentNumber")
        _IdVehicle = dr.GetInt64("IdVehicle")
        _IdPaymentType = dr.GetInt32("IdPaymentType")
        _prefix = PaymentTypeList.GetPaymentTypeList.GetPaymentTypeInfoById(_IdPaymentType).Prefix
    End Sub

#End Region

End Class