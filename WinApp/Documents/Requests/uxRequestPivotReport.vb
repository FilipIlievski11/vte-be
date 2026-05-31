Public Class uxRequestPivotReport
    Private WithEvents _requestPivotReport As RequestPivotReportList

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub uxRequestPivotReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
           
            DateEditStart.EditValue = Now
            DateEditEnd.EditValue = Now
            _requestPivotReport = RequestPivotReportList.GetRequestPivotReportListByDate(Today.Date, Today.Date)
            Me.RequestPivotReportListBindingSource.DataSource = _requestPivotReport
        Catch ex As Exception

        End Try
    End Sub

#Region " WinPart Code "

    Protected Overrides Function GetIdValue() As Object

        Return My.Resources.uxRequestPivotReport

    End Function

    Public Overrides Function ToString() As String

        Return My.Resources.uxRequestPivotReport

    End Function

    Private Sub uxRequestPivotReport_CurrentPrincipalChanged( _
      ByVal sender As Object, _
      ByVal e As System.EventArgs) _
      Handles Me.CurrentPrincipalChanged
    End Sub


#End Region

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        PrintableComponentLink1.CreateDocument()
        PrintableComponentLink1.ShowPreview()
    End Sub

    Private Sub btnShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShow.Click
        _requestPivotReport = RequestPivotReportList.GetRequestPivotReportListByDate(DateEditStart.EditValue, DateEditEnd.EditValue)
        Me.RequestPivotReportListBindingSource.DataSource = _requestPivotReport
    End Sub
End Class
