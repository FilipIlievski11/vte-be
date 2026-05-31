Public Class rptPaymentDocumentDogovor
 Private WithEvents _dogovor As PaymentDocumentRataFiscalPrintList
 Private WithEvents _paymentDocument As PrintPaymentDocumetnByIdDocumetnList
 Public Sub New(ByVal inId As Long)

  ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
  'lblSecretar.Text = objCurentTehExamOrganization.Sekretar
  _dogovor = PaymentDocumentRataFiscalPrintList.GetPaymentDocumentRataFiscalPrintList(inId, False)
  BindingSource1.DataSource = _dogovor

  _paymentDocument = PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList _
  (_dogovor(0).Idpaymentdocument)
  Dim _dogRati As PaymentRatiDogovor = PaymentRatiDogovor.GetPaymentRatiDogovor(_dogovor.Item(0).IdDogovor)
  BindingSourceDogovor.DataSource = _dogRati
  lblOstanato.Text = _dogovor(_dogovor.Count - 1).OstanataZaPlakanjeSuma
  lblIznos.Text = _dogovor(0).VkupnaSuma 'PaymentDocument.GetPaymentDocument(_dogovor(0).Idpaymentdocument).ProveriVkupno
  ' Add any initialization after the InitializeComponent() call.
  Try
   PictureBoxLogo.ImageUrl = objCurentTehExamOrganization.LogoPath
   PictureBoxLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage
  Catch ex As Exception

  End Try
  Dim pomRegBr As String = ""
  If (_paymentDocument.Item(0).IdVehicle > 0) Then
   pomRegBr = _paymentDocument.Item(0).PoslednaRegZaSmetka()
   If pomRegBr = "" Then
    pomRegBr = _paymentDocument.Item(0).RegistrationNumber
   End If
  Else
   pomRegBr = ""
  End If
  Dim _opstina As String = objCommunityList.GetCommunitiesListById(objCurentTehExamOrganization.IdCommunity).CommunityName
  XrLabel15.Text = "1. " & UCase(objCurentTehExamOrganization.OrganizationName) & " претставувано од " _
  & objCurentTehExamOrganization.Sekretar & " од една страна како давател на услуги"
  lblOpstina.Text = _opstina
  lblKorisnik.Text = "2. Корисникот на услуга " & _dogovor.Item(0).CustomerDisplayName & " со адреса на живеење " & _dogovor.Item(0).AddressDisplay & _
  " и ЕМБР: " & _dogovor.Item(0).MB & " Сопственик на возило со рег.број " & pomRegBr '_paymentDocument(0).PoslednaRegZaSmetka()
  lblGarant.Text = "Гарантот на корисникот што ги превзема обврските за отплата на надоместок " & _dogRati.GarantNaziv & _
  " " & _dogRati.GarantAdresa & " со ЕМБР: " & _dogRati.GartEMB
  ' lblOpstina2.Text = _opstina
  'lblVehicleRegistration.Text = _paymentDocument(0).PoslednaRegZaSmetka() 'Vehicle.GetVehicle(_dogovor.Item(0).IdVehicle).LastRegistratinNumber
  'Dim tabelaU As DevExpress.XtraReports.UI.XRTable = New DevExpress.XtraReports.UI.XRTable
  Try

  
   For i As Integer = 1 To _paymentDocument.Count '- 1
    Dim detal As PrintPaymentDocumetnByIdDocumetnInfo = _paymentDocument.Item(i - 1)
    Dim pomIma As Integer = -1
    If Not detal.PrePayed Then
     Dim red As DevExpress.XtraReports.UI.XRTableRow = New DevExpress.XtraReports.UI.XRTableRow
     red.Borders = DevExpress.XtraPrinting.BorderSide.None
     red.Cells.Add(New DevExpress.XtraReports.UI.XRTableCell)
     red.Cells.Add(New DevExpress.XtraReports.UI.XRTableCell)
     red.Cells.Add(New DevExpress.XtraReports.UI.XRTableCell)
     red.Borders = DevExpress.XtraPrinting.BorderSide.None


     If i > 1 Then

      For j As Integer = 0 To i - 2
       If TableUslugi.Rows(j).Cells(0).Text = detal.PaymentCategory Then
        pomIma = j ' - 1
        Exit For
       End If
      Next
     End If
     If pomIma >= 0 Then
      TableUslugi.Rows(pomIma).Cells(1).Text = CType(TableUslugi.Rows(pomIma).Cells(1).Text, Integer) + detal.CenaBezDDV
      TableUslugi.Rows(0).Cells(2).Text = CType(TableUslugi.Rows(0).Cells(2).Text, Integer) + detal.DDVIznos
     Else


      TableUslugi.InsertRowBelow(red)
      TableUslugi.Rows(0).Cells(0).Text = detal.PaymentCategory
      TableUslugi.Rows(0).Cells(0).TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
      TableUslugi.Rows(0).Cells(1).Text = detal.CenaBezDDV
      TableUslugi.Rows(0).Cells(2).Text = detal.DDVIznos

     End If

    End If
   Next
  Catch ex As Exception

  End Try
  XrTableCell7.Width = TableUslugi.Rows(0).Cells(0).Width
  XrTableCell8.Width = TableUslugi.Rows(0).Cells(1).Width
  'XrTableCell9.Width = TableUslugi.Rows(0).Cells(0).Width

 End Sub
End Class