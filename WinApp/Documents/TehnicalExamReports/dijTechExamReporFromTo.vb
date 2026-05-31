Public Class dijTechExamReportFromTo
    Private datOd As Date
    Private datDo As Date
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        DateEdit1.EditValue = Today
        DateEdit2.EditValue = Today

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public ReadOnly Property DatuOd()
        Get
            Return datOd
        End Get
    End Property
    Public ReadOnly Property DatumDo()
        Get
            Return datDo
        End Get
    End Property

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        datOd = DateEdit1.EditValue
        datDo = DateEdit2.EditValue
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        datOd = Nothing
        datDo = Nothing
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class