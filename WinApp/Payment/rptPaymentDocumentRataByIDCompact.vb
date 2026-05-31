Public Class rptPaymentDocumentRataByIDCompact
  Private WithEvents _dokument As PaymentDocumentRataFiscalPrintList
  Private WithEvents _vehiclePrint As PrintVehcileList
  Public Sub New(ByVal inId As Long)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    ' Add any initialization after the InitializeComponent() call.
    _dokument = PaymentDocumentRataFiscalPrintList.GetPaymentDocumentRataFiscalPrintList(inId, True)
    _vehiclePrint = PrintVehcileList.GetPrintVehcileList(_dokument(0).IdVehicle)
    Me.BindingSourcePaymentDocument.DataSource = _dokument
    Me.BindingSourceVehicle.DataSource = _vehiclePrint
    lblOperator.Text = Csla.ApplicationContext.LocalContext("EmployeeFullName")
    CellCompany.Text = UCase(objCurentTehExamOrganization.OrganizationAndStationName)
    CellAdd.Text = objCurentTehExamOrganization.StationAddress
    CellTel.Text = "тел: " & objCurentTehExamOrganization.Tel
    CellEDB.Text = objCurentTehExamOrganization.EDB
  End Sub
End Class