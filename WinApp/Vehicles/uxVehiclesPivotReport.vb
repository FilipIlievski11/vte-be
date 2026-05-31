Public Class uxVehiclesPivotReport
  Private _vehiclePivorReportList As printVehiclePivotReportList

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object
        Return My.Resources.uxVehiclesPivotReport
        'Return "Извештај за возило"

  End Function

  Public Overrides Function ToString() As String
        Return My.Resources.uxVehiclesPivotReport
        'Return "Извештај за возило"

  End Function

  Private Sub uxCustomerPivotReport_CurrentPrincipalChanged( _
    ByVal sender As Object, _
    ByVal e As System.EventArgs) _
    Handles Me.CurrentPrincipalChanged
  End Sub


#End Region

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
        DateStart.EditValue = Now
        DateEnd.EditValue = Now

    ' Add any initialization after the InitializeComponent() call.

  End Sub


  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
  End Sub

  Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    PrintableComponentLink1.CreateDocument()
    PrintableComponentLink1.ShowPreview()
  End Sub

  Private Sub uxVehiclesPivotReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _vehiclePivorReportList = printVehiclePivotReportList.GetprintVehiclePivotReportList(Now, Now)
    Me.PrintVehiclePivotReportListBindingSource.DataSource = _vehiclePivorReportList
  End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        _vehiclePivorReportList = printVehiclePivotReportList.GetprintVehiclePivotReportList(DateStart.EditValue, DateEnd.EditValue)
        Me.PrintVehiclePivotReportListBindingSource.DataSource = _vehiclePivorReportList
    End Sub
End Class
