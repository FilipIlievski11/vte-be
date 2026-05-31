Public Class uxPaymentPivotShort
    Private WithEvents _payments As printShortPivotPaymentDocumentByDateList

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

#Region " WinPart Code "

    Protected Overrides Function GetIdValue() As Object

  Return My.Resources.uxPaymentPivotReport '"Извештај за плаќања"

    End Function

    Public Overrides Function ToString() As String

  Return My.Resources.uxPaymentPivotReport ' "Извештај за плаќања"

    End Function
#End Region

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        RefreshData()
    End Sub
    Private Sub RefreshData()
        Me.PrintShortPivotPaymentDocumentByDateListBindingSource.RaiseListChangedEvents = False
        If ceRange.Checked Then
            _payments = _
                      printShortPivotPaymentDocumentByDateList.GetprintShortPivotPaymentDocumentByDateList(Format("1000-01-01", "yyyy-MM-dd"), Now.Date)
        Else
            _payments = _
                       printShortPivotPaymentDocumentByDateList.GetprintShortPivotPaymentDocumentByDateList( _
                      Me.deStartDate.EditValue.date, Me.deEndDate.EditValue.date)
        End If
        Me.PrintShortPivotPaymentDocumentByDateListBindingSource.DataSource = _payments
        Me.PrintShortPivotPaymentDocumentByDateListBindingSource.RaiseListChangedEvents = True
        Me.PrintShortPivotPaymentDocumentByDateListBindingSource.ResetBindings(False)


    End Sub

    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Me.Close()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        PrintableComponentLink1.CreateDocument()
        PrintableComponentLink1.ShowPreview()
    End Sub

    Private Sub uxPaymentPivotShort_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.deStartDate.DateTime = DateAndTime.Now.AddHours(-Now.Hour).AddMinutes(-Now.Minute)
        Me.deEndDate.DateTime = DateAndTime.Now.AddHours(23).AddMinutes(59)

        _payments = printShortPivotPaymentDocumentByDateList.GetprintShortPivotPaymentDocumentByDateList _
        (Me.deStartDate.DateTime, Me.deEndDate.DateTime)

        Me.PrintShortPivotPaymentDocumentByDateListBindingSource.DataSource = _payments
        Me.UsersListBindingSource.DataSource = objUsersList
        ' Me.CommunitiesListBindingSource.DataSource = objCommunityList
        'Me.UsersListBindingSource.DataSource = objUsersList

    End Sub

    Private Sub btnKasov_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKasov.Click
        Dim rpt As New rptKasovIzvestaj(deStartDate.EditValue, deEndDate.EditValue, LookUpEditOperator.EditValue)
        Dim parForm As MainForm = Me.ParentForm
        parForm.AddWinPart(New uxPrint(rpt))
    End Sub
End Class
