Public Class dijInternationalDriveingLicencesList
    Private WithEvents _documentInternationalLicenceList As DocumentsInternationalDriveingLicenceList
    Private WithEvents _documentInternationalLicence As DocumentsInternationalDriveingLicence

    Public ReadOnly Property DocumenInternationalLicence() As DocumentsInternationalDriveingLicence
        Get
            Return _documentInternationalLicence
        End Get
    End Property
    Public ReadOnly Property DocInterLicenceList() As DocumentsInternationalDriveingLicenceList
        Get
            Return _documentInternationalLicenceList
        End Get
    End Property
    Public ReadOnly Property Od() As String
        Get
            Return Format(DateEdit1.EditValue, "dd.MM.yyyy") '.ToString
        End Get
    End Property
    Public ReadOnly Property DoDat() As String
        Get
            Return Format(DateEdit2.EditValue, "dd.MM.yyyy")
        End Get
    End Property

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Try
            _documentInternationalLicenceList = DocumentsInternationalDriveingLicenceList. _
            GetDocumentsInternationalDriveingLicenceListFromTo(Today.Date, Today.Date)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub ApplyAuthorizationRules()
        btnDelete.Visible = VTE.Library.DocumentsInternationalDriveingLicence.CanDeleteObject
        btnDelete.Enabled = VTE.Library.DocumentsInternationalDriveingLicence.CanDeleteObject
        If VTE.Library.DocumentsInternationalDriveingLicence.CanDeleteObject Then
            LayoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        Else
            LayoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
    End Sub
    Private Sub dijInternationalDriveingLicencesList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.DocumentsInternationalDriveingLicenceListBindingSource.DataSource = _documentInternationalLicenceList
        DocumentsInternationalDriveingLicenceListGridControl.ForceInitialize()
        ApplyAuthorizationRules()
        DocumentsInternationalDriveingLicenceListGridControl.Focus()
        '
        DateEdit1.EditValue = Today.Date
        DateEdit2.EditValue = Today.Date
        Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
        Me.GridView1.FocusedColumn = colId
    End Sub

    'Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    '  If GridView1.FocusedColumn IsNot Nothing Then
    '    Select Case GridView1.FocusedColumn.Name
    '      Case "colShellNumber", "colModelName", "colCompanyName", "colLastRegistrationNumber"
    '        System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
    '        System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    '      Case Else
    '        System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
    '        System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
    '    End Select
    '  End If
    'End Sub

    Private Sub dij_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.F1
                Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
        End Select
    End Sub

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click, HyperLinkEditId.DoubleClick
        If Me.DocumentsInternationalDriveingLicenceListBindingSource.Position >= 0 Then
            _documentInternationalLicence = DocumentsInternationalDriveingLicence. _
            GetDocumentsInternationalDriveingLicence(_documentInternationalLicenceList.Item _
                                        (Me.DocumentsInternationalDriveingLicenceListBindingSource.Position).Id)
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub



    Private Sub RepositoryItemButtonEdit1_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles RepositoryItemButtonEdit1.ButtonPressed
        Select Case e.Button.Index
            Case 0
                'pecati
                If Me.DocumentsInternationalDriveingLicenceListBindingSource.Position >= 0 Then
                    Dim rpt As New rptInternationalDriveingLicence( _
                    _documentInternationalLicenceList.Item( _
                    Me.DocumentsInternationalDriveingLicenceListBindingSource.Position).Id)
                    rpt.ShowPreviewDialog()
                End If
        End Select
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Me.DialogResult = Windows.Forms.DialogResult.Yes
        Me.Close()

        'Me.PrintableComponentLink1.ShowPreviewDialog(Me)
    End Sub

    Private Sub btnCreatePayment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreatePayment.Click
        Try
            Using busy As New Splash("Вчитувам")
                Dim selectedRelationId As Long = _
                CustomerVehiclesRelation.ExistsCustomerOnly( _
                _documentInternationalLicenceList.Item(Me.DocumentsInternationalDriveingLicenceListBindingSource.Position).IdCustomer)
                Dim par As MainForm = Me.Owner
                Dim dok As PaymentDocument = PaymentDocument.NewPaymentDocument
                dok.IdCustomerVehicleRelation = selectedRelationId
                par.AddWinPart(New uxPaymentDocument(dok))
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        _documentInternationalLicenceList = DocumentsInternationalDriveingLicenceList. _
        GetDocumentsInternationalDriveingLicenceListFromTo(DateEdit1.DateTime, DateEdit2.DateTime)
        Me.DocumentsInternationalDriveingLicenceListBindingSource.DataSource = _documentInternationalLicenceList
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            If Me.DocumentsInternationalDriveingLicenceListBindingSource.Position >= 0 Then
                _documentInternationalLicence = DocumentsInternationalDriveingLicence. _
                GetDocumentsInternationalDriveingLicence(_documentInternationalLicenceList.Item _
                                            (Me.DocumentsInternationalDriveingLicenceListBindingSource.Position).Id)
                _documentInternationalLicence.Delete()
                _documentInternationalLicence.Save()

                _documentInternationalLicenceList = DocumentsInternationalDriveingLicenceList. _
        GetDocumentsInternationalDriveingLicenceListFromTo(DateEdit1.DateTime, DateEdit2.DateTime)
                Me.DocumentsInternationalDriveingLicenceListBindingSource.DataSource = _documentInternationalLicenceList

            End If
        Catch ex As Exception

        End Try

    End Sub
End Class