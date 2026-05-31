Public Class rptPaymentDocumentByIDCompactDouble
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

    '_dokument = From p In inDokumetn Order By p.VisibleOrder Ascending
    CellCompany.Text = UCase(objCurentTehExamOrganization.OrganizationAndStationName)
    CellAdd.Text = objCurentTehExamOrganization.StationAddress
    CellTel.Text = "тел: " & objCurentTehExamOrganization.Tel
    CellEDB.Text = objCurentTehExamOrganization.EDB
    CellCompany2.Text = UCase(objCurentTehExamOrganization.OrganizationAndStationName)
    CellAdd2.Text = objCurentTehExamOrganization.StationAddress
    CellTel2.Text = "тел: " & objCurentTehExamOrganization.Tel
    CellEDB2.Text = objCurentTehExamOrganization.EDB

    _vehiclePrint = PrintVehcileList.GetPrintVehcileList(_dokument(0).IdVehicle)
    Me.PaymentDocumentBindingSource.DataSource = _dokument
    Me.VehicleBindingSource.DataSource = _vehiclePrint
    If (_dokument.Item(0).IdVehicle > 0) Then
      Dim pomRegBr As String = _dokument.Item(0).PoslednaRegZaSmetka()
      If pomRegBr <> "" Then
        lblRegNum1.Text = pomRegBr
        lblRegNum2.Text = pomRegBr
      Else
        lblRegNum1.Text = _dokument.Item(0).RegistrationNumber
        lblRegNum2.Text = _dokument.Item(0).RegistrationNumber
      End If
    Else
      lblRegNum1.Text = ""
      lblRegNum2.Text = ""
    End If
    'lblRegNum1.Text = _dokument(0).PoslednaRegZaSmetka()
    'lblRegNum2.Text = _dokument(0).PoslednaRegZaSmetka()
    'For i As Integer = 1 To if((mod(_dokument.Count / 2)>0),_dokument .Count /2+1,_dokument.Count /2)
    '    For j As Integer = 0 To _dokument.Count
    '        If i <> j Then
    '            If _dokument.Item(i - 1).PaymentCategory.ToString = _dokument.Item(j).PaymentCategory.ToString Then
    '                _dokument.Item(i - 1).Price += _dokument.Item(j).Price
    '                _dokument.Item(j).
    '            End If
    '        End If
    '    Next
    'Next
    ' cellWithCap1.Visible = objCurentTehExamOrganization.PriceWithTax
    cellWith1.Visible = objCurentTehExamOrganization.PriceWithTax
    cellWithout1.Visible = objCurentTehExamOrganization.PriceWithoutTax
    cellDDV1.Visible = (objCurentTehExamOrganization.PriceWithTax And objCurentTehExamOrganization.PriceWithoutTax)
    cellDDV2.Visible = (objCurentTehExamOrganization.PriceWithTax And objCurentTehExamOrganization.PriceWithoutTax)
    ' cellWithoutCap1.Visible = objCurentTehExamOrganization.PriceWithoutTax
    ' SumWith1.Visible = objCurentTehExamOrganization.PriceWithTax
    ' SumWithout1.Visible = objCurentTehExamOrganization.PriceWithoutTax
    'cellWithCap2.Visible = objCurentTehExamOrganization.PriceWithTax
    cellWith2.Visible = objCurentTehExamOrganization.PriceWithTax
    cellWithout2.Visible = objCurentTehExamOrganization.PriceWithoutTax
    '  cellWithoutCap2.Visible = objCurentTehExamOrganization.PriceWithoutTax
    ' SumWith2.Visible = objCurentTehExamOrganization.PriceWithTax
    ' SumWithout2.Visible = objCurentTehExamOrganization.PriceWithoutTax
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
    cellredbr2.Text = redenBr
    redenBr += 1
  End Sub

  Private Sub rptPaymentDocumentByIDCompact_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles Me.BeforePrint

  End Sub
End Class