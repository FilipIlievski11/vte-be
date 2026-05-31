Public Class rptTehnicalExamsRegistar

  Public Sub New(ByVal DatumOd As Date, ByVal DatumDo As Date)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    lblOdDo.Text = "Îä: " & DatumOd.Date & " Äî: " & DatumDo.Date
    ' Add any initialization after the InitializeComponent() call.
    Me.DocumentsTehnicalExamsReportListBindingSource.DataSource = _
    NovRegistarList.GetNovRegistarList(DatumOd.Date, DatumDo.Date)
  End Sub

  'Private Sub XrTable2_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTable2.BeforePrint
  '  XrTable2.CanGrow = True
  '  XrTable2.Height = XrTableRow2.Height
  '  Detail.Height = XrTable2.Height
  'End Sub

  'Private Sub XrTableRow2_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableRow2.BeforePrint
  '  XrTable2.CanGrow = True
  '  XrTable2.Height = XrTableRow2.Height
  '  Detail.Height = XrTable2.Height
  'End Sub
End Class