
<Serializable()> _
Public Class PrintPaymentDocumetnByIdDocumetnInfo
    Inherits ReadOnlyBase(Of PrintPaymentDocumetnByIdDocumetnInfo)

#Region " Calculated Fields "
    Public ReadOnly Property AddressDisplay() As String
        Get
            If (_livingAddressNumber IsNot Nothing) AndAlso (_streetName IsNot Nothing) Then
                Return UCase("ул. " & _streetName & " бр." & _livingAddressNumber)
            Else
                Return _streetName
            End If
        End Get
    End Property

    Public ReadOnly Property DokumentNaslov() As String
        Get
            Return " бр." & _DocumentNumber & "/" & _DocumentDate.Year
        End Get
    End Property
    Public ReadOnly Property VehicleMakerModel() As String
        Get
            Return _companyName & ", " & _modelName
        End Get
    End Property
    Public ReadOnly Property VehicleMakerModelAddingTng() As String
        Get
            If _TNG Then
                Return _companyName & ", " & _modelName & " " & _VehicleModelAdding & " TNG"
            Else
                Return _companyName & ", " & _modelName & " " & _VehicleModelAdding
            End If
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
                Return -FicalRound(DDVPresmetki.EdinicnaCenaBezDDV(soPopust, _dDV)) '((Math.Round(DDVPresmetki.EdinicnaCenaBezDDV(soPopust, _dDV), 2)))
            Else
                Return FicalRound(DDVPresmetki.EdinicnaCenaBezDDV(soPopust, _dDV)) '(Math.Round(DDVPresmetki.EdinicnaCenaBezDDV(soPopust, _dDV), 2))
            End If
        End Get
    End Property
    Public ReadOnly Property DDVIznos() As Single
        Get
            Dim soPopust As Double = (_price - (_price * _discount / 100))
            If _storno Then
                Return -FicalRound(DDVPresmetki.DDVIsnos(soPopust, _dDV, 1)) '((Math.Round(DDVPresmetki.DDVIsnos(soPopust, _dDV, 1), 2)))
            Else
                Return FicalRound(DDVPresmetki.DDVIsnos(soPopust, _dDV, 1)) '(Math.Round(DDVPresmetki.DDVIsnos(soPopust, _dDV, 1), 2))
            End If
        End Get
    End Property

    Public ReadOnly Property VkupnoZaRed() As Double
        Get
            Dim soPopust As Double = (_price - (_price * _discount / 100))
            If _storno Then
                Return -FicalRound(soPopust) '((Math.Round(soPopust, 2)))
            Else
                Return FicalRound(soPopust) '(Math.Round(soPopust, 2))
            End If
        End Get
    End Property
    Private _rata As Boolean
    Public ReadOnly Property Rata() As Boolean
        Get
            Return _rata
        End Get
    End Property

    Public ReadOnly Property DiscountValue() As Double
        Get
            Dim Popust As Double = (_price * _discount / 100)
            If Storno Then
                Return -FicalRound(Popust) '((Math.Round(Popust, 2)))
            Else
                Return FicalRound(Popust) '(Math.Round(Popust, 2))
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
    Private _PlatenaSuma As Double
    Public ReadOnly Property PlatenaSuma() As Double
        Get
            'Dim suma As Double = 0
            'Dim payDocument As PaymentDocument = PaymentDocument.GetPaymentDocument(DocumentID)
            'Dim rati As PaymentDocumentsRati = payDocument.PaymentDocumentRati
            'For Each rata As PaymentDocumentsRata In rati
            '    If rata.Payed Then
            '        suma += rata.Price
            '    End If
            'Next
            Return _PlatenaSuma
        End Get
    End Property
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
    Private _RequiredDate As Date
    Public ReadOnly Property RequiredDate() As Date
        Get
            Return _RequiredDate
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
    Private _printText As String
    Public ReadOnly Property PrintText() As String
        Get
            Return _printText
        End Get
    End Property
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

    Private _discount As Decimal
    Public ReadOnly Property Discount() As Decimal
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
    Public ReadOnly Property DDVProcent() As String
        Get
            Return _dDV & "%"
        End Get
    End Property

    Private _note As String
    Public ReadOnly Property Note() As String
        Get
            If _note <> "Автоматска пресметка" Then
                Return _note
            Else
                Return ""
            End If
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
    Private _isCompany As Boolean
    Public ReadOnly Property IsCompany() As Boolean
        Get
            Return _isCompany
        End Get
    End Property
    Private _idLivingAddress As Integer
    Public ReadOnly Property IdLivingAddress() As Integer
        Get
            Return _idLivingAddress
        End Get
    End Property
    Private _streetName As String
    Public ReadOnly Property StreetName() As String
        Get
            Return UCase(_streetName)
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
    Private _livingAddressNumber As String
    Public ReadOnly Property LivingAddressNumber() As String
        Get
            Return _livingAddressNumber
        End Get
    End Property

    Private _shellNumber As String
    Public ReadOnly Property ShellNumber() As String
        Get
            Return _shellNumber
        End Get
    End Property
    Private _modelName As String
    Public ReadOnly Property ModelName() As String
        Get
            Return _modelName
        End Get
    End Property
    Private _companyName As String
    Public ReadOnly Property CompanyName() As String
        Get
            Return _companyName
        End Get
    End Property
    Private _companyTrademark As String
    Public ReadOnly Property CompanyTrademark() As String
        Get
            Return _companyTrademark
        End Get
    End Property

    Private _IdCustomerFaktura As Integer
    Public ReadOnly Property IdCustomerFaktura() As Integer
        Get
            Return _IdCustomerFaktura
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
    Private _operatorNameAndStation As String
    Public ReadOnly Property OperatorNameAndStation() As String
        Get
            Return _operatorNameAndStation
        End Get
    End Property
    Private _documentNote As String = String.Empty
    Public ReadOnly Property DocumentNote() As String
        Get
            Return _documentNote
        End Get
    End Property
    Private _VisibleOrder As Integer
    Public ReadOnly Property VisibleOrder() As Integer
        Get
            Return _VisibleOrder
        End Get
    End Property
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

    Private _categoryCode As String
    Public ReadOnly Property CategoryCode() As String
        Get
            Return _categoryCode
        End Get
    End Property

    Private _categoryName As String
    Public ReadOnly Property CategoryName() As String
        Get
            Return _categoryName
        End Get
    End Property
    Public ReadOnly Property Category() As String
        Get
            Return _categoryCode & "-" & _categoryName
        End Get
    End Property
    Private _categoryForPaymentsCode As String
    Public ReadOnly Property CategoryForPaymentsCode() As String
        Get
            Return _categoryForPaymentsCode
        End Get
    End Property

    Private _CategoryForPaymentsName As String
    Public ReadOnly Property CategoryForPaymentsName() As String
        Get
            Return _CategoryForPaymentsName
        End Get
    End Property
    Private _VehicleModelAdding As String
    Public ReadOnly Property VehicleModelAdding() As String
        Get
            Return _VehicleModelAdding
        End Get
    End Property
    Private _TNG As Boolean
    Public ReadOnly Property TNG() As Boolean
        Get
            Return _TNG
        End Get
    End Property

    Public ReadOnly Property CategoryForPayments() As String
        Get
            Return _categoryForPaymentsCode & "-" & _CategoryForPaymentsName
        End Get
    End Property
    Private _BLK As String
    Public ReadOnly Property BLK() As String
        Get
            Return _BLK
        End Get
    End Property

    Private _PrePayed As Boolean
    Public ReadOnly Property PrePayed() As Boolean
        Get
            Return _PrePayed
        End Get
    End Property
    Private _OstanatoZaNaplata
    Public ReadOnly Property OstanatoZaNaplata() As Double
        Get
            Return _OstanatoZaNaplata
        End Get
    End Property
    Private _paymentCategoryCommunity As Integer
    Public ReadOnly Property PaymentCategoryCommunity() As String
        Get
            If _paymentCategoryCommunity > 0 Then
                Dim objCommunityList As CommunitiesList = Csla.ApplicationContext.LocalContext.Item("objCommunityList")
                Dim commu As String = objCommunityList.GetCommunitiesListById(_paymentCategoryCommunity).CommunityCode
                Return _paymanetCategory & " (" & commu & ")"
            Else
                Return _paymanetCategory
            End If

        End Get
    End Property

    Private _TehnicalExamsType As String
    Public ReadOnly Property TehnicalExamsType() As String
        Get
            Return _TehnicalExamsType
        End Get
    End Property

    Public ReadOnly Property PaymentCategoryNote() As String
        Get
            Return _paymanetCategory & " " & Note
        End Get
    End Property

    Protected Overrides Function GetIdValue() As Object
        Return _documentID
    End Function

    Public Overrides Function ToString() As String
        Return _documentID
    End Function

    Friend Sub New(ByVal dr As SafeDataReader)
        Try
            _documentID = dr.GetInt64("DocumentID")
            _DocumentDate = dr.GetDateTime("DocumentDate")
            _RequiredDate = dr.GetDateTime("RequiredDate")
            _documentNote = dr.GetString("DocumentNote")
            _idPaymentType = dr.GetInt32("IdPaymentType")
            _paymentType = dr.GetString("PaymentType")
            _printText = dr.GetString("PrintText")
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
            _isCompany = dr.GetBoolean("IsCompany")
            _idLivingAddress = dr.GetInt32("IdLivingAddress")
            _streetName = dr.GetString("StreetName")
            _cityName = dr.GetString("CityName")
            _cityZip = dr.GetInt32("CityZip")
            _livingAddressNumber = dr.GetString("LivingAddressNumber")
            _shellNumber = dr.GetString("ShellNumber")
            _modelName = dr.GetString("ModelName")
            _companyName = dr.GetString("CompanyName")
            _companyTrademark = dr.GetString("CompanyTrademark")
            _discount = dr.GetValue("Discount")
            _storno = dr.GetValue("Storno")
            _communityName = dr.GetString("CommunityName")
            _PlatenaSuma = dr.GetValue("SumRatiPayed")
            _categoryCode = dr.GetString("CategoryCode")
            _categoryName = dr.GetString("CategoryName")
            _categoryForPaymentsCode = dr.GetString("CategoryForPaymentsCode")
            _CategoryForPaymentsName = dr.GetString("CategoryForPaymentsName")
            _Payed = dr.GetBoolean("Payed")
            _OstanatoZaNaplata = dr.GetValue("OstanatoZaPlakanje")
            If _OstanatoZaNaplata < 1 And _OstanatoZaNaplata > -1 Then
                _OstanatoZaNaplata = 0
            Else
                _OstanatoZaNaplata = Math.Round(_OstanatoZaNaplata)
            End If
            _registrationNumber = dr.GetString("LastRegistratinNumber") 'regInfo.RegistrationNumber
            _VehicleModelAdding = dr.GetString("VehicleModelAdding")
            _TNG = dr.GetBoolean("TNG")
            _PrePayed = dr.GetBoolean("PrePayed")
            _BLK = dr.GetString("BLK")
            If _idPriceCatalog > 0 Then
                ''Dim pInfo As PaymentCataologInfo = _
                ''CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
                ''PaymentCataologList).GetInfo(_idPriceCatalog)
                _paymanetCategory = dr.GetString("paymanetCategory")
                _priceName = _paymanetCategory & " " & dr.GetString("ItemName") & " " & _
            String.Format(dr.GetString("PrametarName"), dr.GetValue("ParametarFrom").ToString, dr.GetValue("ParametarTo").ToString) ''pInfo.PaymentName
                '' pInfo.CategoryName
                _VisibleOrder = dr.GetInt32("VisibleOrder")
                _paymentCategoryCommunity = dr.GetInt32("IdCommunity")
            Else
                _priceName = My.Resources.Rata
                _paymanetCategory = My.Resources.Rata
                _VisibleOrder = 100
                _paymentCategoryCommunity = 0
            End If
            Dim objUsersInfo As UsersInfo = _
            CType(Csla.ApplicationContext.LocalContext("objUsersList"),  _
            UsersList).getInfoById(dr.GetInt32("IdOperator"))
            _operatorName = objUsersInfo.FullName
            _operatorNameAndStation = objUsersInfo.FullNameAndStation
            Dim payType As PaymentTypeInfo = CType(Csla.ApplicationContext.LocalContext("objPaymentTypeList"),  _
            PaymentTypeList).GetPaymentTypeInfoById(_idPaymentType)
            _rata = payType.Rati
            _TehnicalExamsType = dr.GetString("TehnicalExamsType")
            _IdCustomerFaktura = dr.GetInt32("IdFakturiraNa")
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
    End Sub
    Public Function PoslednaRegZaSmetka() As String
        Try
            Dim isNewReg As Boolean = False
            Dim regNum As String = ""
            Dim regCode As String = ""
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "getLastRegNumByREquestType"
                    cm.Parameters.AddWithValue("@IdPaymentDocument", _documentID)
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            isNewReg = dr.GetBoolean("IsNewRegistration")
                            regNum = dr.GetString("LastRegistratinNumber")
                            regCode = dr.GetString("RegistrationCode")
                        End While

                    End Using
                End Using
            End Using
            Dim objCurentTehExamStation As TehnicalExamOrganizationsInfo = CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)
            If regNum = "" Then
                Return regNum
            Else
                If isNewReg Then
                    Return regCode & "-"
                Else
                    Return regNum
                End If
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Class