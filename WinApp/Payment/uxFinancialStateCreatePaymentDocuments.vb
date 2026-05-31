Public Class uxFinancialStateCreatePaymentDocuments
  Private WithEvents _customerFinancialStateList As CustomerFinancialStateList
#Region "PritisnatoKopce"

  Private Sub uxListOfPaymentDocuments_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub
#End Region

#Region "WinPart"

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxListOfPaymentDocuments
  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxListOfPaymentDocuments
  End Function

#End Region

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Try
      _customerFinancialStateList = CustomerFinancialStateList.GetCustomerFinancialStateList()
      Me.CustomerFinancialStateListBindingSource.DataSource = _customerFinancialStateList
    Catch ex As Exception

    End Try
    ' Add any initialization after the InitializeComponent() call.

  End Sub

  Private Sub btnCreateBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateBill.Click

    Dim selectedRelationId As Long = _customerFinancialStateList.Item(Me.CustomerFinancialStateListBindingSource.Position).IdCustomerVehicleRelation
    Dim par As MainForm = Me.ParentForm
    Dim dok As PaymentDocument = PaymentDocument.NewPaymentDocument
    dok.IdCustomerVehicleRelation = selectedRelationId
    par.AddWinPart(New uxPaymentDocument(dok))


  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
  End Sub
End Class
