Imports DevExpress.XtraPrinting
Public Class uxReportByPaymentCategory
    Private WithEvents _reportList As ReportByCategoryForPaymentList
    Private WithEvents _categoryList As PaymentCategoryList
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        DateEditEnd.EditValue = Now
        DateEditStart.EditValue = Now
        ' Add any initialization after the InitializeComponent() call.

    End Sub
#Region " WinPart Code "

    Protected Overrides Function GetIdValue() As Object

        Return My.Resources.uxReportByPaymentCategory

    End Function

    Public Overrides Function ToString() As String

        Return My.Resources.uxReportByPaymentCategory

    End Function
#End Region
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShow.Click
  _reportList = ReportByCategoryForPaymentList.GetReportByCategoryForPaymentList _
  (LookUpEditCategory.EditValue, DateEditStart.DateTime, DateEditEnd.DateTime)
        Me.ReportByCategoryForPaymentListBindingSource.DataSource = _reportList
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Me.PrintableComponentLink1.CreateDocument()
        Dim phf As PageHeaderFooter = _
         TryCast(PrintableComponentLink1.PageHeaderFooter, PageHeaderFooter)
        phf.Header.Content.Clear()

        ' Add custom information to the link's header.
        phf.Header.Content.AddRange(New String() _
            {("ПРЕГЛЕД ЗА НАПЛАТА ЗА: " & LookUpEditCategory.Text), (""), ("Датум од: " & DateEditStart.Text & "  до: " & DateEditEnd.Text)})

        'phf.Header.Content.AddRange(New String() _
        '  {0, 1, 2})
        'phf.Header.Content(1) = ("Датум од: " & deStartDate.Text & "  до: " & deEndDate.Text)
        phf.Header.LineAlignment = BrickAlignment.Center

        Me.PrintableComponentLink1.ShowPreview()

    End Sub

    Private Sub uxReportByPaymentCategory_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            _categoryList = PaymentCategoryList.GetPaymentCategoryList
            Me.PaymentCategoryListBindingSource.DataSource = _categoryList
            LookUpEditCategory.EditValue = _categoryList.Item(0).Id
   _reportList = ReportByCategoryForPaymentList.GetReportByCategoryForPaymentList _
(LookUpEditCategory.EditValue, DateEditStart.DateTime, DateEditEnd.DateTime)
            Me.ReportByCategoryForPaymentListBindingSource.DataSource = _reportList

        Catch ex As Exception

        End Try
    End Sub
End Class
