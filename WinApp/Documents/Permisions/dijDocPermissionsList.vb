Public Class dijDocPermissionsList
    Private WithEvents _documenPermission As DocumentsPermision
    Private WithEvents _documenPermissionList As DocumentsPermisionsList

    Public ReadOnly Property DocumenPermision() As DocumentsPermision
        Get
            Return _documenPermission
        End Get
    End Property
    Public ReadOnly Property DocumenPermisionList() As DocumentsPermisionsList
        Get
            Return _documenPermissionList
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
        Try
            _documenPermissionList = DocumentsPermisionsList.GetDocumentsPermisionsListFromTo(Today.Date, Today.Date)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub dijDocPermissionsList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.DocumentsPermisionsListBindingSource.DataSource = _documenPermissionList
        DocumentsPermisionsListGridControl.ForceInitialize()
        ApplyAuthorizationRules()
        DocumentsPermisionsListGridControl.Focus()
        DateEdit1.EditValue = Today.Date
        DateEdit2.EditValue = Today.Date
        '
        Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
        Me.GridView1.FocusedColumn = colId
    End Sub
    Private Sub ApplyAuthorizationRules()
        btnDelete.Visible = VTE.Library.DocumentsPermision.CanDeleteObject
        btnDelete.Enabled = VTE.Library.DocumentsPermision.CanDeleteObject
        If VTE.Library.DocumentsPermision.CanDeleteObject Then
            LayoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        Else
            LayoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
    End Sub
    Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
        If GridView1.FocusedColumn IsNot Nothing Then
            Select Case GridView1.FocusedColumn.Name
                Case "colShellNumber", "colModelName", "colCompanyName", "colLastRegistrationNumber"
                    System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
                    System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
                Case Else
                    System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
                    System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
            End Select
        End If
    End Sub

    Private Sub dij_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.F1
                Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
        End Select
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click ', HyperLinkEditIdDocumentPermission.DoubleClick
        If Me.DocumentsPermisionsListBindingSource.Position >= 0 Then
            _documenPermission = DocumentsPermision.GetDocumentsPermision(_documenPermissionList.Item(Me.DocumentsPermisionsListBindingSource.Position).Id)
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Me.DialogResult = Windows.Forms.DialogResult.Yes
        Me.Close()
        ' Me.PrintableComponentLink1.ShowPreviewDialog(Me)
    End Sub


    Private Sub RepositoryItemButtonEdit1_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles RepositoryItemButtonEdit1.ButtonPressed
        Select Case e.Button.Index
            Case 0
                'pecati
                If Me.DocumentsPermisionsListBindingSource.Position >= 0 Then
                    Dim rptPermissionForV As rptOdobrenieZaTugoV = New rptOdobrenieZaTugoV(_documenPermissionList.Item(Me.DocumentsPermisionsListBindingSource.Position).Id)
                    'Dim ux As New uxPrint(rptPermissionForV)
                    'CType(Me.ParentForm.ParentForm, MainForm).AddWinPart(ux)
                    rptPermissionForV.ShowPreviewDialog()
                End If
        End Select
    End Sub


    Private Sub btnBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBill.Click
        Try
            Using busy As New Splash("Вчитувам")

                Dim selectedRelationId As Long = _
                 _documenPermissionList.Item _
                (Me.DocumentsPermisionsListBindingSource.Position).IdCustomerVehicleRelationOwner

                Dim dokNov As PaymentDocument = PaymentDocument.NewPaymentDocument
                dokNov.IdCustomerVehicleRelation = selectedRelationId
                Dim par As MainForm = Me.Owner
                par.AddWinPart(New uxPaymentDocument(dokNov))
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        _documenPermissionList = DocumentsPermisionsList.GetDocumentsPermisionsListFromTo(DateEdit1.DateTime, DateEdit2.DateTime)
        Me.DocumentsPermisionsListBindingSource.DataSource = _documenPermissionList
    End Sub

    Private Sub HyperLinkEditIdDocumentPermission_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles HyperLinkEditIdDocumentPermission.DoubleClick
        If Me.DocumentsPermisionsListBindingSource.Position >= 0 Then
            _documenPermission = DocumentsPermision.GetDocumentsPermision(_documenPermissionList.Item(Me.DocumentsPermisionsListBindingSource.Position).Id)
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            If Me.DocumentsPermisionsListBindingSource.Position >= 0 Then
                _documenPermission = DocumentsPermision.GetDocumentsPermision(_documenPermissionList.Item(Me.DocumentsPermisionsListBindingSource.Position).Id)
                _documenPermission.Delete()
                _documenPermission.Save()
                _documenPermissionList = DocumentsPermisionsList.GetDocumentsPermisionsListFromTo(DateEdit1.DateTime, DateEdit2.DateTime)
                Me.DocumentsPermisionsListBindingSource.DataSource = _documenPermissionList
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class