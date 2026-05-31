Public Class rptPaymentDocumentByID
  Private WithEvents _dokument As PrintPaymentDocumetnByIdDocumetnList

  Public Sub New(ByVal inDokumetn As PrintPaymentDocumetnByIdDocumetnList)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    ' Add any initialization after the InitializeComponent() call.
    _dokument = inDokumetn
    Me.PaymentDocumentBindingSource.DataSource = _dokument
        XrLabel33.Text = UCase(objCurentTehExamOrganization.OrganizationAndStationName)
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
End Class