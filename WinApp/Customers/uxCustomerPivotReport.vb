Public Class uxCustomerPivotReport
  Private WithEvents _customerPivotReportList As printCustomerPivotReportList

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.

  End Sub

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

        Return My.Resources.uxCustomerPivotReport

  End Function

  Public Overrides Function ToString() As String

        Return My.Resources.uxCustomerPivotReport

  End Function

  Private Sub uxCustomerPivotReport_CurrentPrincipalChanged( _
    ByVal sender As Object, _
    ByVal e As System.EventArgs) _
    Handles Me.CurrentPrincipalChanged
  End Sub


#End Region

  Private Sub uxCustomerPivotReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    _customerPivotReportList = printCustomerPivotReportList.GetprintCustomerPivotReportList
    Me.PrintCustomerPivotReportListBindingSource.DataSource = _customerPivotReportList
  End Sub

  Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    Me.Close()
  End Sub

  Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

  PrintableComponentLink1.CreateDocument()

    PrintableComponentLink1.ShowPreview()
  End Sub
End Class
