

<Serializable()> _
Public Class PrintPaymentPivotInfo
    Inherits ReadOnlyBase(Of PrintPaymentPivotInfo)

#Region " Calculated Fields "
   

    Public ReadOnly Property DokumentNaslov() As String
        Get
            Return " бр." & _DocumentNumber & "/" & _DocumentDate.Year
        End Get
    End Property

    Public ReadOnly Property CustomerDisplayName() As String
        Get
            If _customerSurname = String.Empty Then
                Return _customerFirstName
            Else
                Return _customerFirstName & " " & _customerSurname
            End If
        End Get
    End Property

    Private _price As Decimal
    Public ReadOnly Property Price() As Decimal
        Get
            If _storno Then
                Return -(_price)
            Else
                Return _price
            End If
        End Get
    End Property

    Public ReadOnly Property CenaBezDDV() As Double
        Get
            Dim soPopust As Double = (_price - (_price * _discount / 100))
            If _storno Then
                Return -FicalRound((Math.Round(DDVPresmetki.EdinicnaCenaBezDDV(soPopust, _dDV), 2)))
            Else
                Return FicalRound(Math.Round(DDVPresmetki.EdinicnaCenaBezDDV(soPopust, _dDV), 2))
            End If
        End Get
    End Property
    Public ReadOnly Property DDVIznos() As Single
        Get
            Dim soPopust As Double = (_price - (_price * _discount / 100))
            If _storno Then
                Return -FicalRound((Math.Round(DDVPresmetki.DDVIsnos(soPopust, _dDV, 1), 2)))
            Else
                Return FicalRound(Math.Round(DDVPresmetki.DDVIsnos(soPopust, _dDV, 1), 2))
            End If
        End Get
    End Property

    Public ReadOnly Property VkupnoZaRed() As Double
        Get
            Dim soPopust As Double = (_price - (_price * _discount / 100))
            If _storno Then
                Return -FicalRound((Math.Round(soPopust, 2)))
            Else
                Return FicalRound(Math.Round(soPopust, 2))
            End If
        End Get
    End Property

    Public ReadOnly Property DiscountValue() As Double
        Get
            Dim Popust As Double = (_price * _discount / 100)
            If Storno Then
                Return -FicalRound((Math.Round(Popust, 2)))
            Else
                Return FicalRound(Math.Round(Popust, 2))
            End If
        End Get
    End Property

    'Public ReadOnly Property OstanataSumaZaPlakanje() As Double
    '    Get
    '        Dim sumaNaDetali As Double = 0
    '        Dim suma As Double = 0
    '        Dim payDocument As PaymentDocument = PaymentDocument.GetPaymentDocument(DocumentID)
    '        Dim rati As PaymentDocumentsRati = payDocument.PaymentDocumentRati
    '        Dim detali As PaymentDocumentsDetails = payDocument.PaymentDocumentDetails
    '        For Each detal As PaymentDocumentsDetail In detali
    '            If Not detal.PrePayed Then
    '                sumaNaDetali += detal.Price
    '            End If
    '        Next
    '        For Each rata As PaymentDocumentsRata In rati
    '            If rata.Payed Then
    '                suma += rata.Price
    '            End If
    '        Next
    '        suma = sumaNaDetali - suma
    '        If suma < 1 Then
    '            suma = 0
    '        End If
    '        Return suma
    '    End Get
    'End Property
    'Public ReadOnly Property PlatenaSuma() As Double
    '    Get
    '        Dim suma As Double = 0
    '        Dim payDocument As PaymentDocument = PaymentDocument.GetPaymentDocument(DocumentID)
    '        Dim rati As PaymentDocumentsRati = payDocument.PaymentDocumentRati
    '        For Each rata As PaymentDocumentsRata In rati
    '            If rata.Payed Then
    '                suma += rata.Price
    '            End If
    '        Next
    '        Return suma
    '    End Get
    'End Property
#End Region

    Private _documentID As Long
    Public ReadOnly Property DocumentID() As Long
        Get
            Return _documentID
        End Get
    End Property

    Private _DocumentDate As Date
    Public ReadOnly Property DocumentDate() As Date
        Get
            Return _DocumentDate
        End Get
    End Property
  
    Private _idPaymentType As Integer
    Public ReadOnly Property IdPaymentType() As Integer
        Get
            Return _idPaymentType
        End Get
    End Property
    Private _paymentType As String
    Public ReadOnly Property PaymentType() As String
        Get
            Return _paymentType
        End Get
    End Property
    'Private _printText As String
    'Public ReadOnly Property PrintText() As String
    '    Get
    '        Return _printText
    '    End Get
    'End Property
    Private _idCustomer As Long
    Public ReadOnly Property IdCustomer() As Long
        Get
            Return _idCustomer
        End Get
    End Property
    Private _idVehicle As Long
    Public ReadOnly Property IdVehicle() As Long
        Get
            Return _idVehicle
        End Get
    End Property
    Private _idDetal As Long
    Public ReadOnly Property IdDetal() As Long
        Get
            Return _idDetal
        End Get
    End Property
    Private _idPriceCatalog As Integer
    Public ReadOnly Property IdPriceCatalog() As Integer
        Get
            Return _idPriceCatalog
        End Get
    End Property
    Private _priceName As String
    Public ReadOnly Property PriceName() As String
        Get
            Return _priceName
        End Get
    End Property

    Private _discount As Single
    Public ReadOnly Property Discount() As Single
        Get
            Return _discount
        End Get
    End Property
    Private _dDV As Single
    Public ReadOnly Property DDV() As Single
        Get
            Return _dDV
        End Get
    End Property

    Private _note As String
    Public ReadOnly Property Note() As String
        Get
            Return _note
        End Get
    End Property
    Private _customerSurname As String
    Public ReadOnly Property CustomerSurname() As String
        Get
            Return _customerSurname
        End Get
    End Property
    Private _customerFirstName As String
    Public ReadOnly Property CustomerFirstName() As String
        Get
            Return _customerFirstName
        End Get
    End Property
  
    Private _cityName As String
    Public ReadOnly Property CityName() As String
        Get
            Return UCase(_cityName)
        End Get
    End Property
    Private _cityZip As Integer
    Public ReadOnly Property CityZip() As Integer
        Get
            Return _cityZip
        End Get
    End Property

    Private _shellNumber As String
    Public ReadOnly Property ShellNumber() As String
        Get
            Return _shellNumber
        End Get
    End Property
 
    Private _registrationNumber As String
    Public ReadOnly Property RegistrationNumber() As String
        Get
            Dim objCurentTehExamStation As TehnicalExamOrganizationsInfo = CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)

            Dim communitList As CommunitiesList = Csla.ApplicationContext.LocalContext("objCommunityList")
            Dim pom As String = communitList.GetCommunitiesListById(objCurentTehExamStation.IdCommunity).RegistrationCode
            If _registrationNumber = pom & "-000-AA" Then
                Return pom & "-"
            Else
                Return _registrationNumber
            End If
        End Get
    End Property

    Private _storno As Boolean
    Public ReadOnly Property Storno() As Boolean
        Get
            Return _storno
        End Get
    End Property
    Public ReadOnly Property StornoMk() As String
        Get
            If _storno Then
                Return "ДА"
            Else
                Return "НЕ"
            End If
        End Get
    End Property

    Private _Payed As Boolean
    Public ReadOnly Property Payed() As Boolean
        Get
            Return _Payed
        End Get
    End Property
    Public ReadOnly Property PayedMk() As String
        Get
            If _Payed Then
                Return "ДА"
            Else
                Return "НЕ"
            End If
        End Get
    End Property
    Private _paymanetCategory As String
    Public ReadOnly Property PaymentCategory() As String
        Get
            Return _paymanetCategory
        End Get
    End Property

    Private _operatorName As String
    Public ReadOnly Property OperatorName() As String
        Get
            Return _operatorName
        End Get
    End Property
    'Private _documentNote As String = String.Empty
    'Public ReadOnly Property DocumentNote() As String
    '    Get
    '        Return _documentNote
    '    End Get
    'End Property
  
    Private _DocumentNumber As String
    Public ReadOnly Property DocumentNumber() As String
        Get
            Return _DocumentNumber
        End Get
    End Property
    Private _communityName As String
    Public ReadOnly Property CommunityName() As String
        Get
            Return _communityName
        End Get
    End Property

   
    'Private _PrePayed As Boolean
    'Public ReadOnly Property PrePayed() As Boolean
    '    Get
    '        Return _PrePayed
    '    End Get
    'End Property
    Protected Overrides Function GetIdValue() As Object
        Return _documentID
    End Function

    Public Overrides Function ToString() As String
        Return _documentID
    End Function

    Friend Sub New(ByVal dr As SafeDataReader)
        _documentID = dr.GetInt64("DocumentID")
        _DocumentDate = dr.GetDateTime("DocumentDate")

        ' _documentNote = dr.GetString("DocumentNote")
        _idPaymentType = dr.GetInt32("IdPaymentType")
        _paymentType = dr.GetString("PaymentType")
        ' _printText = dr.GetString("PrintText")
        _idCustomer = dr.GetInt64("IdCustomer")
        _idVehicle = dr.GetInt64("IdVehicle")
        _idDetal = dr.GetInt64("IdDetal")
        _idPriceCatalog = dr.GetInt32("IdPriceCatalog")
        _DocumentNumber = dr.GetString("DocumentNumber")
        _price = dr.GetDecimal("Price")
        _dDV = dr.GetValue("DDV")
        _note = dr.GetString("Note")
        _customerSurname = dr.GetString("CustomerSurname")
        _customerFirstName = dr.GetString("CustomerFirstName")
        _cityName = dr.GetString("CityName")
        _cityZip = dr.GetInt32("CityZip")
        _shellNumber = dr.GetString("ShellNumber")
        _discount = dr.GetValue("Discount")
        _storno = dr.GetValue("Storno")
        _communityName = dr.GetString("CommunityName")
        _Payed = dr.GetBoolean("Payed")
        _registrationNumber = dr.GetString("LastRegistratinNumber")

        'Dim pInfo As PaymentCataologInfo = _
        'CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
        'PaymentCataologList).GetInfo(_idPriceCatalog)
        '_priceName = pInfo.PaymentName
        '_paymanetCategory = pInfo.CategoryName
        'Dim objUsersInfo As UsersInfo = _
        'CType(Csla.ApplicationContext.LocalContext("objUsersList"),  _
        'UsersList).getInfoById(dr.GetInt32("IdOperator"))
        '_operatorName = objUsersInfo.FullName
        

    End Sub


End Class
