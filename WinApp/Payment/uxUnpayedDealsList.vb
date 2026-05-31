Public Class uxUnpayedDealsList
 Private WithEvents _unpayedDealsList As UnpayedDealsList
 Private _payed As Boolean


 Public Sub New(ByVal payed As Boolean)

  ' This call is required by the Windows Form Designer.
  InitializeComponent()
  _payed = payed
  ' Add any initialization after the InitializeComponent() call.

 End Sub

#Region " WinPart Code "

 Protected Overrides Function GetIdValue() As Object
  If _payed Then
   Return My.Resources.uxClosedDealsList
  Else
   Return My.Resources.uxOpenDealsList
  End If
 End Function

 Public Overrides Function ToString() As String
  If _payed Then
   Return My.Resources.uxClosedDealsList
  Else
   Return My.Resources.uxOpenDealsList
  End If
 End Function

 Private Sub uxUnpayedDealsList_CurrentPrincipalChanged( _
   ByVal sender As Object, _
   ByVal e As System.EventArgs) _
   Handles Me.CurrentPrincipalChanged
 End Sub


#End Region

 Private Sub uxUnpayedDealsList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  _unpayedDealsList = UnpayedDealsList.GetUnpayedDealsList(_payed)
  Me.UnpayedDealsListBindingSource.DataSource = _unpayedDealsList
  If _payed Then
   btnEdit.Enabled = False
  Else
   btnEdit.Enabled = True
  End If
 End Sub

 Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
  Me.Close()
 End Sub



 Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click, HyperLinkCustomer.DoubleClick

  Dim doc As PaymentDocument = PaymentDocument.GetPaymentDocument _
          (_unpayedDealsList.Item(Me.UnpayedDealsListBindingSource.Position).Id)
  Dim par As MainForm = Me.ParentForm
  par.AddWinPart(New uxPaymentDocument(doc))
 End Sub

 Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
  Me.PrintableComponentLink1.ShowPreviewDialog(Me)
 End Sub

  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    If GridView1.FocusedColumn IsNot Nothing Then
      Select Case GridView1.FocusedColumn.Name
        Case "colShellnumber"
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
        Case Else
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
      End Select
    End If
  End Sub
End Class
