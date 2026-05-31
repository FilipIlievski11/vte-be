Public Class uxFakturiList
  Private WithEvents _payDocList As PayDocList

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxClosedDealsList
  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxOpenDealsList
  End Function

  Private Sub uxUnpayedDealsList_CurrentPrincipalChanged( _
    ByVal sender As Object, _
    ByVal e As System.EventArgs) _
    Handles Me.CurrentPrincipalChanged
  End Sub


#End Region

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    DateEdit1.DateTime = Now
    DateEdit2.DateTime = Now
    RefreshData()
    Me.PaymentTypeListBindingSource.DataSource = PaymentTypeList.GetPaymentTypeList
    ' Add any initialization after the InitializeComponent() call.

  End Sub
  Private Sub btnShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShow.Click
    RefreshData()
  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
  End Sub

  Private Sub btnPay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPay.Click
    Dim doc As PaymentDocument = PaymentDocument.GetPaymentDocument _
            (_payDocList.Item(Me.PayDocListBindingSource.Position).Id)
    doc.Payed = True
    doc.Save()
    RefreshData()
  End Sub
  Private Sub RefreshData()
    _payDocList = PayDocList.GetPayDocList(CustomLookUpEdit1.EditValue, DateEdit1.EditValue, DateEdit2.EditValue)
    Me.PayDocListBindingSource.DataSource = _payDocList
  End Sub

  Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    Me.PrintableComponentLink1.ShowPreviewDialog(Me)
  End Sub

  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    If GridView1.FocusedColumn IsNot Nothing Then
      Select Case GridView1.FocusedColumn.Name
        Case "colShellnumber", "colLastRegistratinNumber"
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
        Case Else
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
      End Select
    End If
  End Sub
End Class
