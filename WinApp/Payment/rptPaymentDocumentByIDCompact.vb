Public Class rptPaymentDocumentByIDCompact
  Private WithEvents _dokument As Csla.SortedBindingList(Of PrintPaymentDocumetnByIdDocumetnInfo)
  Private WithEvents _vehiclePrint As PrintVehcileList
  Private redenBr As Integer = 0
  Public Sub New(ByVal inDokumetn As PrintPaymentDocumetnByIdDocumetnList)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    redenBr = 1
    ' Add any initialization after the InitializeComponent() call.
    _dokument = New Csla.SortedBindingList(Of PrintPaymentDocumetnByIdDocumetnInfo)(inDokumetn)
    _dokument.ApplySort("VisibleOrder", System.ComponentModel.ListSortDirection.Ascending)
    CellCompany.Text = UCase(objCurentTehExamOrganization.OrganizationAndStationName)
    CellAdd.Text = objCurentTehExamOrganization.StationAddress
    CellTel.Text = "тел: " & objCurentTehExamOrganization.Tel
    CellEDB.Text = objCurentTehExamOrganization.EDB
    '_dokument = From p In inDokumetn Order By p.VisibleOrder Ascending


    _vehiclePrint = PrintVehcileList.GetPrintVehcileList(_dokument(0).IdVehicle)
    Me.PaymentDocumentBindingSource.DataSource = _dokument
    Me.VehicleBindingSource.DataSource = _vehiclePrint
    'lblRegNum.Text = _dokument(0).PoslednaRegZaSmetka()
    If (_dokument.Item(0).IdVehicle > 0) Then
      Dim pomRegBr As String = _dokument.Item(0).PoslednaRegZaSmetka()
      If pomRegBr <> "" Then
        lblRegNum.Text = pomRegBr
      Else
        lblRegNum.Text = _dokument.Item(0).RegistrationNumber
      End If
    Else
      lblRegNum.Text = ""
    End If
    'For i As Integer = 1 To if((mod(_dokument.Count / 2)>0),_dokument .Count /2+1,_dokument.Count /2)
    '    For j As Integer = 0 To _dokument.Count
    'If i <> j Then
    '            If _dokument.Item(i - 1).PaymentCategory.ToString = _dokument.Item(j).PaymentCategory.ToString Then
    '                _dokument.Item(i - 1).Price += _dokument.Item(j).Price
    '                _dokument.Item(j).
    '            End If
    '        End If
    '    Next
    'Next
    ' cellWithCaption.Visible = objCurentTehExamOrganization.PriceWithTax
    cellWith.Visible = objCurentTehExamOrganization.PriceWithTax
    cellWithout.Visible = objCurentTehExamOrganization.PriceWithoutTax
    cellDDV.Visible = (objCurentTehExamOrganization.PriceWithTax And objCurentTehExamOrganization.PriceWithoutTax)
    ' cellWithoutCaption.Visible = objCurentTehExamOrganization.PriceWithoutTax
    ' SumWith.Visible = objCurentTehExamOrganization.PriceWithTax
    ' SumWithout.Visible = objCurentTehExamOrganization.PriceWithoutTax
  End Sub

  'Private Sub lblCenaBezDdv_SummaryCalculated(ByVal sender As Object, ByVal e As DevExpress.XtraReports.UI.TextFormatEventArgs) Handles lblCenaBezDdv.SummaryCalculated
  '  If _dokument.Item(0).Discount > 0 Then
  '    lblSoPopust.Visible = True
  '    lblCenaBezDDvSoPopust.Visible = True
  '    lblCenaBezDDvSoPopust.Text = (Math.Round(e.Value * (1 - _dokument.Item(0).Discount / 100), 0)).ToString
  '  End If
  'End Sub

  'Private Sub lblDDV_SummaryCalculated(ByVal sender As Object, ByVal e As DevExpress.XtraReports.UI.TextFormatEventArgs) Handles lblDDV.SummaryCalculated
  '  If _dokument.Item(0).Discount > 0 Then
  '    lblSoPopust.Visible = True
  '    lblDDVsoPopust.Visible = True
  '    lblDDVsoPopust.Text = (Math.Round(e.Value * (1 - _dokument.Item(0).Discount / 100), 0)).ToString
  '  End If
  'End Sub

  'Private Sub lblVkupno_SummaryCalculated(ByVal sender As Object, ByVal e As DevExpress.XtraReports.UI.TextFormatEventArgs) Handles lblVkupno.SummaryCalculated
  '  If _dokument.Item(0).Discount > 0 Then
  '    lblSoPopust.Visible = True
  '    lblVkupnoSoPopust.Visible = True
  '    lblVkupnoSoPopust.Text = (Math.Round(e.Value * (1 - _dokument.Item(0).Discount / 100), 0)).ToString
  '  End If
  'End Sub




  Private Sub cellredBr_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles cellredBr.BeforePrint
    cellredBr.Text = redenBr
    redenBr += 1
  End Sub
End Class