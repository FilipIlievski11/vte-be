Public Class uxTehnicalWxamPivotReportBR
  Private WithEvents _tehExamPivotReportList As TechnicalExamPivotReportWithoutNList

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    DateEditFrom.EditValue = Now
    DateEditTo.EditValue = Now
    ' Add any initialization after the InitializeComponent() call.

  End Sub

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxTechnicalExamPivotReport

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxTechnicalExamPivotReport

  End Function

  Private Sub uxTechnicalExamPivotReport_CurrentPrincipalChanged( _
    ByVal sender As Object, _
    ByVal e As System.EventArgs) _
    Handles Me.CurrentPrincipalChanged
  End Sub


#End Region

  Private Sub uxTechnicalExamPivotReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    _tehExamPivotReportList = TechnicalExamPivotReportWithoutNList.GetTechnicalExamPivotReportWithoutNList(DateEditFrom.EditValue, DateEditTo.EditValue)
    Me.TechnicalExamPivotReportWithoutNListBindingSource.DataSource = _tehExamPivotReportList
  End Sub
  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
  End Sub

  Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    PrintableComponentLink1.CreateDocument()
    PrintableComponentLink1.ShowPreview()
  End Sub

  Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
    _tehExamPivotReportList = TechnicalExamPivotReportWithoutNList.GetTechnicalExamPivotReportWithoutNList(DateEditFrom.EditValue, DateEditTo.EditValue)
    Me.TechnicalExamPivotReportWithoutNListBindingSource.DataSource = _tehExamPivotReportList
  End Sub
End Class
