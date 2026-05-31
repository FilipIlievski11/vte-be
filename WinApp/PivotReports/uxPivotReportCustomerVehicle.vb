Public Class uxPivotReportCustomerVehicle
  Private _CustomerVehiclePivorReportList As PrintCustomerVehiclePivotReportList

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

        Return My.Resources.uxPivotReportCustomerVehicle

  End Function

  Public Overrides Function ToString() As String

        Return My.Resources.uxPivotReportCustomerVehicle
        'Return "Извештај за комитенти и возила"

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

  Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    PrintableComponentLink1.CreateDocument()
    PrintableComponentLink1.ShowPreview()
  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
  End Sub

  Private Sub RadioGroup1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
    Me.PrintCustomerVehiclePivotReportListBindingSource.RaiseListChangedEvents = False
    Select Case RadioGroup1.SelectedIndex
      Case 0
                _CustomerVehiclePivorReportList = PrintCustomerVehiclePivotReportList.GetPrintCustomerVehiclePivotReportList(DateStart.EditValue, DateEnd.EditValue)
        'Me.PrintCustomerVehiclePivotReportListBindingSource.DataSource = _CustomerVehiclePivorReportList
      Case 1
                _CustomerVehiclePivorReportList = PrintCustomerVehiclePivotReportList.GetPrintCustomerVehiclePivotReportListCurrentOwners(DateStart.EditValue, DateEnd.EditValue)
        'Me.PrintCustomerVehiclePivotReportListBindingSource.DataSource = _CustomerVehiclePivorReportList
      Case 2
                _CustomerVehiclePivorReportList = PrintCustomerVehiclePivotReportList.GetPrintCustomerVehiclePivotReportListPrevOwners(DateStart.EditValue, DateEnd.EditValue)
        'Me.PrintCustomerVehiclePivotReportListBindingSource.DataSource = _CustomerVehiclePivorReportList

    End Select
    Me.PrintCustomerVehiclePivotReportListBindingSource.DataSource = _CustomerVehiclePivorReportList
    Me.PrintCustomerVehiclePivotReportListBindingSource.RaiseListChangedEvents = True
    Me.PrintCustomerVehiclePivotReportListBindingSource.ResetBindings(False)
  End Sub

  Private Sub uxPivotReportCustomerVehicle_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _CustomerVehiclePivorReportList = PrintCustomerVehiclePivotReportList.GetPrintCustomerVehiclePivotReportList(Now, Now)
    Me.PrintCustomerVehiclePivotReportListBindingSource.DataSource = _CustomerVehiclePivorReportList
  End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        _CustomerVehiclePivorReportList = PrintCustomerVehiclePivotReportList.GetPrintCustomerVehiclePivotReportList(DateStart.EditValue, DateEnd.EditValue)
        Me.PrintCustomerVehiclePivotReportListBindingSource.DataSource = _CustomerVehiclePivorReportList
        Me.PrintCustomerVehiclePivotReportListBindingSource.RaiseListChangedEvents = True
        Me.PrintCustomerVehiclePivotReportListBindingSource.ResetBindings(False)
    End Sub
End Class
