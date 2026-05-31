Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils
Public Class uxDashboard
  Private WithEvents _finances As CustumerFinanceList 'CustumerFinanceDepList
  Private WithEvents _incorectTehnicalExams As IncorectTehnicalExamsList
  Private WithEvents _activeDocuments As ActiveDocumentList 'ActiveDocumentsDepList

  Public Sub New()
    Try


      ' This call is required by the Windows Form Designer.
      'DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = True
      Control.CheckForIllegalCrossThreadCalls = False
      InitializeComponent()

      ' Add any initialization after the InitializeComponent() call.
      ApplyAuthorizationRules()
    Catch ex As Exception

    End Try
  End Sub

  Private Sub ApplyAuthorizationRules()
    Try
      Me.PaymentCataologListBindingSource.DataSource = objPaymentCatalogList
      Me.UsersListBindingSource.DataSource = objUsersList
      If Customers.CanEditObject Then
        _finances = CustumerFinanceList.GetCustumerFinanceList 'CustumerFinanceDepList.GetCustumerFinanceDepList
        Me.CustumerFinanceDepListBindingSource.DataSource = _finances
      End If
      If DocumentsTehnicalExamsReport.CanEditObject Then
        _incorectTehnicalExams = IncorectTehnicalExamsList.GetIncorectTehnicalExamsList
        Me.IncorectTehnicalExamsDepListBindingSource.DataSource = _incorectTehnicalExams
        LayoutControlGroup3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
      Else
        LayoutControlGroup3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
      End If
      If Document.CanEditObject Then
        _activeDocuments = ActiveDocumentList.GetActiveDocumentList(True, False) 'ActiveDocumentsDepList.GetActiveDocumentsDepList
        Me.ActiveDocumentsDepListBindingSource.DataSource = _activeDocuments
      End If
      If Request.CanGetObject And Request.CanEditObject Then
        LayoutControlGroup4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        LayoutControlItem14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        LayoutControlItem19.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        LayoutControlItem20.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
      Else
        LayoutControlGroup4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        LayoutControlItem14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        LayoutControlItem19.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        LayoutControlItem20.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
      End If
      If PaymentDocument.CanAddObject Then
        LayoutControlGroup2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
      Else
        LayoutControlGroup2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
      End If
      If LayoutControlItem14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never _
      AndAlso LayoutControlGroup4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never Then

        SplitterItem1.X = 0
      End If



      Me.RequestTypeListBindingSource.DataSource = objRequestTypeList
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

  Private Sub uxDashboard_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'ExpandLevel(GridView1, 0)
    Me.RequestTypeListTreeList.ForceInitialize()
    Me.RequestTypeListTreeList.Nodes(0).Expanded = True
    Me.RequestTypeListTreeList.Nodes(1).Expanded = True
    Me.RequestTypeListTreeList.Nodes(2).Expanded = True

    Me.IsShowingBrokenRules = False
  End Sub

#Region " WinPart "

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxDashboard

  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxDashboard
  End Function

#End Region

#Region " Finances "

  Private Sub CustumerFinanceDepListBindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles CustumerFinanceDepListBindingSource.ListChanged
    Try

      If _finances IsNot Nothing Then
        Dim tmp As Boolean = (_finances.Count > 0)
        Me.btnFinanceCreateBill.Enabled = tmp
        Me.btnCreateCalculation.Enabled = tmp
      End If

    Catch ex As Exception

    End Try
  End Sub

  Private Sub btnNewBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewBill.Click
    Try
      Dim par As MainForm = Me.ParentForm
      Dim dok As PaymentDocument = PaymentDocument.NewPaymentDocument
      par.AddWinPart(New uxPaymentDocument(dok))
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

  Private Sub btnFinanceCreateBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFinanceCreateBill.Click
    Try
      Using busy As New Splash("Вчитувам")
        Dim selectedRelationId As Long = _finances.Item(Me.CustumerFinanceDepListBindingSource.Position).IdCustomerVehicleRelation
        Dim par As MainForm = Me.ParentForm
        Dim dok As PaymentDocument = PaymentDocument.NewPaymentDocument
        dok.IdCustomerVehicleRelation = selectedRelationId
        par.AddWinPart(New uxPaymentDocument(dok))
      End Using
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

  Private Sub btnFinancePaymentReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFinancePaymentReport.Click
    Try
      Using busy As New Splash(My.Resources.LoadingData)
        Dim par As MainForm = Me.ParentForm
        par.AddWinPart(New uxPaymentPivotReport)
      End Using
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

  Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
    Try
      Using busy As New Splash(My.Resources.LoadingData)
        Dim par As MainForm = Me.ParentForm
        par.AddWinPart(New uxPaymentPivotShort)
      End Using
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

  Private Sub btnCreateCalculation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateCalculation.Click
    'otkako ke se napravi CreateDetails
    'napavi presmetkaList i stavi ja vo Pivot za printanje
    Try
      Using busy As New Splash(My.Resources.LoadingData)
        Dim selectedRelationId As Long = _finances.Item(Me.CustumerFinanceDepListBindingSource.Position).IdCustomerVehicleRelation
        Dim cust As String = _finances.Item(Me.CustumerFinanceDepListBindingSource.Position).CustomerDisplay
        Dim vehic As String = _finances.Item(Me.CustumerFinanceDepListBindingSource.Position).VehicleDisplay
        Dim par As MainForm = Me.ParentForm
        Dim dok As PaymentDocument = PaymentDocument.NewPaymentDocument
        dok.IdCustomerVehicleRelation = selectedRelationId
        dok.CreateDetails(selectedRelationId)
        Dim calc As CalculationList = CalculationList.GetCalculationList(dok)

        par.AddWinPart(New uxCaclulationPivot(calc, cust, vehic))
      End Using
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub
  Private Sub btnNewCalculation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewCalculation.Click
    Try
      Using busy As New Splash(My.Resources.LoadingData)
        Dim par As MainForm = Me.ParentForm
        par.AddWinPart(New uxCaclulationPivot())
      End Using
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub
#End Region

#Region " Technical Exams "

  Private Sub IncorectTehnicalExamsDepListBindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles IncorectTehnicalExamsDepListBindingSource.ListChanged
    Try
      If _incorectTehnicalExams IsNot Nothing Then
        Dim tmp As Boolean = (_incorectTehnicalExams.Count > 0)
        Me.btnTechnicalExamEdit.Enabled = tmp
        Me.btnTehnicalExamDismis.Enabled = tmp
        Me.btnTehnicalExamPrint.Enabled = tmp
      End If
    Catch ex As Exception

    End Try

  End Sub

  Private Sub btnTehnicalExamPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTehnicalExamPrint.Click
    Try
      Dim isOK As Boolean = False
      If objCurentTehExamOrganization.NewTechnicalExamReport Then
        Dim rptNewTehnicalExamReport As rptNewTehnicalExamReport = New rptNewTehnicalExamReport(_incorectTehnicalExams.Item(Me.IncorectTehnicalExamsDepListBindingSource.Position).IdCustomerVehicleRelation, _incorectTehnicalExams.Item(Me.IncorectTehnicalExamsDepListBindingSource.Position).IdTehnicalExam)
        'rptNewTehnicalExamReport.ShowPreviewDialog()

        Dim ux1 As New uxPrint(rptNewTehnicalExamReport)
        CType(Me.ParentForm, MainForm).AddWinPart(ux1)
      Else
        Dim rptTehnicalExamReport As rptTehnickiPregledZapisnik = New rptTehnickiPregledZapisnik(_incorectTehnicalExams.Item(Me.IncorectTehnicalExamsDepListBindingSource.Position).IdTehnicalExam)
        ''---------
        If rptTehnicalExamReport.IsRight Then
          Dim rptPotvrdaTehnicalExamReport As rptPotvrdaZaTehnickaIspravnost = _
    New rptPotvrdaZaTehnickaIspravnost(_incorectTehnicalExams.Item(Me.IncorectTehnicalExamsDepListBindingSource.Position).IdTehnicalExam)
          Dim ux As New uxPrint(rptPotvrdaTehnicalExamReport)
          CType(Me.ParentForm, MainForm).AddWinPart(ux)
        End If
        ''---------
        ' rptTehnicalExamReport.ShowPreviewDialog()
        Dim ux2 As New uxPrint(rptTehnicalExamReport)
        CType(Me.ParentForm, MainForm).AddWinPart(ux2)
      End If

    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

  Private Sub btnTehnicalExamDismis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTehnicalExamDismis.Click
    If MsgBox("Дали сакате да го избришете техничкиот преглед", MsgBoxStyle.YesNoCancel, "Внимание") = MsgBoxResult.Yes Then
      Try
        Using busy As New Splash("Бришам...")
          Dim idTehExam As Long
          idTehExam = _incorectTehnicalExams.Item(Me.IncorectTehnicalExamsDepListBindingSource.Position).IdTehnicalExam

          Dim requestPom As Request
          Try

            requestPom = Request.GetRequestByIdTehEx(idTehExam, 2)
            If requestPom.Id > 0 Then
              requestPom.IdTechnicalExamReport = 0
              requestPom.Save()
            Else
              requestPom = Nothing
            End If
          Catch ex As Exception
            requestPom = Nothing
            Exit Try
          End Try

          Dim dok As DocumentsTehnicalExamsReport = _
                 DocumentsTehnicalExamsReport.GetDocumentsTehnicalExamsReport(idTehExam)
          dok.Details.Clear()
          dok.Save()
          dok.Delete()
          dok.Save()
        End Using
      Catch ex As Exception
        MsgBox(ex.Message)
      End Try
    End If
  End Sub

  Private Sub btnTechnicalExamEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTechnicalExamEdit.Click
    Try
      Using busy As New Splash(My.Resources.LoadingData)
        Dim par As MainForm = Me.ParentForm
        Dim dok As DocumentsTehnicalExamsReport = _
                   DocumentsTehnicalExamsReport.GetDocumentsTehnicalExamsReport( _
                   _incorectTehnicalExams.Item(Me.IncorectTehnicalExamsDepListBindingSource.Position).IdTehnicalExam)
        par.AddWinPart(New uxDocumentTehnicalExamReports(dok))
      End Using
    Catch ex As Exception

    End Try
  End Sub

  Private Sub btnTechnicalExamNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTechnicalExamNew.Click
    Try
      Dim par As MainForm = Me.ParentForm
      par.AddWinPart(New uxDocumentTehnicalExamReports( _
                     DocumentsTehnicalExamsReport.NewDocumentsTehnicalExamsReport))

    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

#End Region

#Region " Requests "

  Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    Using busy As New Splash(My.Resources.LoadingData)
      Try
        Dim doc As ActiveDocumentsDepInfo = _activeDocuments.Item(Me.ActiveDocumentsDepListBindingSource.Position)
        Dim rpt As DevExpress.XtraReports.UI.XtraReport
        Select Case doc.IdDocumentPrint
          Case 1
            Dim print As PrintZelenList = PrintZelenList.GetPrintZelenList(doc.Id)
            rpt = New printZelen(print)
          Case 2
            Dim print As PrintPlavList = PrintPlavList.GetPrintPlavList(doc.Id)
            rpt = New PrintPlav(print)
          Case Else
            Dim print As printBelList = printBelList.GetprintBelList(doc.Id)
            rpt = New printBel(print)
        End Select

        Dim ux As New uxPrint(rpt)
        CType(Me.ParentForm, MainForm).AddWinPart(ux)
      Catch ex As Exception
        MsgBox(ex.Message)
      End Try
    End Using
  End Sub

  Private Sub btnEditDocument_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditDocument.Click

    Try
      Using bus As New Splash(My.Resources.LoadingData)
        Dim _request As Request = Request.GetRequest(Me._activeDocuments.Item(Me.ActiveDocumentsDepListBindingSource.Position).Id)
        Dim par As MainForm = Me.ParentForm
        For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
          For Each ctl As Control In page.Controls
            If (TypeOf ctl Is uxRequestEdit) AndAlso (CType(ctl, uxRequestEdit).request.Id = _request.Id) Then
              par.ShowWinPart(CType(ctl, uxRequestEdit))
              Exit Sub
            End If
          Next
        Next
        par.AddWinPart(New uxRequestEdit(_request))

      End Using
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub
  Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    If MsgBox("Дали сакате да го откажете барањето", MsgBoxStyle.YesNoCancel, "Внимание") = MsgBoxResult.Yes Then
      Using busy As New Splash("Бришам...")
        Try
          Dim _request As Request = Request.GetRequest(Me._activeDocuments.Item(Me.ActiveDocumentsDepListBindingSource.Position).Id)
          'priveri dali e nova relacija i izbrisi ja taa

          _request.Delete()
          _request.Save()
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If
  End Sub
  Private Sub ActiveDocumentsDepListBindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles ActiveDocumentsDepListBindingSource.ListChanged
    Try

      If _activeDocuments IsNot Nothing Then
        Dim tmp As Boolean = (_activeDocuments.Count > 0)
        'Me.btnRefreshRequests.Enabled = tmp
        Me.btnApproveRequest.Enabled = tmp
        Me.btnEditDocument.Enabled = tmp
        Me.btnCancel.Enabled = tmp
        Me.btnPrint.Enabled = tmp
        ' Me.btnDogovori.Enabled = tmp
      End If

    Catch ex As Exception

    End Try
  End Sub

  Private appZelen As AppearanceDefault = New AppearanceDefault(System.Drawing.Color.LightGreen)
  Private appPlav As AppearanceDefault = New AppearanceDefault(System.Drawing.Color.LightBlue)
  Private appBel As AppearanceDefault = New AppearanceDefault(System.Drawing.Color.White)

  Private Sub GridView3_RowStyle(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles GridView3.RowStyle
    Dim row As ActiveDocumentsDepInfo = CType(GridView3.GetRow(e.RowHandle), ActiveDocumentsDepInfo)
    If row IsNot Nothing Then

      Select Case row.RequestPrintName
        Case "Зелен"
          AppearanceHelper.Apply(e.Appearance, appZelen)
        Case "Плав"
          AppearanceHelper.Apply(e.Appearance, appPlav)
        Case "Бел"
          AppearanceHelper.Apply(e.Appearance, appBel)
      End Select

    End If

  End Sub

  Private Sub RequestTypeListTreeList_NodeCellStyle(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.GetCustomNodeCellStyleEventArgs) Handles RequestTypeListTreeList.NodeCellStyle
    If e.Node.Item(colIdDocumentPrint1) IsNot Nothing Then
      Select Case e.Node.Item(colIdDocumentPrint1)
        Case 1
          AppearanceHelper.Apply(e.Appearance, appZelen)
        Case 2
          AppearanceHelper.Apply(e.Appearance, appPlav)
        Case 3
          AppearanceHelper.Apply(e.Appearance, appBel)
      End Select
    End If
  End Sub
  'Private Sub btnCreateTehnicalExam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateTehnicalExam.Click
  '    'Using busy As New Splash("Креирање на технички преглед...")
  '    Try

  '        Dim activeRequest As ActiveDocumentsDepInfo = _
  '          Me._activeDocuments(Me.ActiveDocumentsDepListBindingSource.Position)
  '        Dim reqType As RequestTypeInfo = _
  '          CType(Csla.ApplicationContext.LocalContext("objRequestTypeList"), RequestTypeList).getInfoById(activeRequest.IdRequestType)
  '        'proveri dali e potrebno da se izvrsi teh
  '        If Not reqType.IsTehnicalExamRequired Then
  '            MsgBox("За овој тип на барање не е потребно технички преглед")
  '            Exit Sub
  '        End If
  '        'proveri dali postoi tehnicki pregled
  '        If activeRequest.IdTechnicalExamReport > 0 Then
  '            MsgBox("За барањето е направен технички преглед")
  '            Exit Sub
  '        End If

  '        Using busy As New Splash(My.Resources.LoadingData)
  '            Dim par As MainForm = Me.ParentForm
  '            Dim tehnicalExam As DocumentsTehnicalExamsReport = DocumentsTehnicalExamsReport.NewDocumentsTehnicalExamsReport
  '            tehnicalExam.IdCustomerVehicleRelation = activeRequest.IdCustomerVehicleRelation
  '            par.AddWinPart(New uxDocumentTehnicalExamReports(tehnicalExam))
  '        End Using


  '    Catch ex As Exception
  '        MsgBox(ex.Message)
  '    End Try
  'End Sub

  'Private Sub btnAddNewRegistration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewRegistration.Click
  '    Dim activeRequest As ActiveDocumentsDepInfo = _
  '      Me._activeDocuments(Me.ActiveDocumentsDepListBindingSource.Position)
  '    'proveri dali treba da se stavi/izmeni registracija
  '    Dim _request As Request = Request.GetRequest(activeRequest.Id)
  '    Dim reqType As RequestTypeInfo = _
  '       CType(Csla.ApplicationContext.LocalContext("objRequestTypeList"), RequestTypeList).getInfoById(activeRequest.IdRequestType)

  '    If reqType.IsNewRegistration Then
  '        Dim curRel As CustomerVehiclesRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(_request.IdCustomerVehicleRelation)
  '        Dim _vehicle = Vehicle.GetVehicle(curRel.IdVehicle)
  '        'Dim dij As New dijAddNewRegistration(_vehicle)
  '        'dij.ShowDialog(Me.ParentForm)

  '    End If
  'End Sub

  Private Sub btnApproveRequest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApproveRequest.Click
    Dim activeRequest As ActiveDocumentsDepInfo = _
       Me._activeDocuments(Me.ActiveDocumentsDepListBindingSource.Position)
    'proveri dali treba da se stavi/izmeni registracija
    Dim _request As Request = Request.GetRequest(activeRequest.Id)
    Dim reqType As RequestTypeInfo = _
       CType(Csla.ApplicationContext.LocalContext("objRequestTypeList"), RequestTypeList).getInfoById(activeRequest.IdRequestType)
    Try
      'If reqType.IsNewRegistration Then

      '    Dim curRel As CustomerVehiclesRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(_request.IdCustomerVehicleRelation)
      '    Dim _vehicle = Vehicle.GetVehicle(curRel.IdVehicle)
      '    Dim dij As New dijNewRegistration(_vehicle)
      '    dij.ShowDialog(Me.ParentForm)
      '    'Dim dij As New dijAddNewRegistration(_vehicle)
      '    'dij.ShowDialog(Me.ParentForm)
      '    If dij.DialogResult = DialogResult.OK Then
      '        _request.CloseRequest()
      '    End If
      'Else
      _request.CloseRequest()
      'End If




    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

  Private Sub btnNewRequest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      Using busy As New Splash(My.Resources.LoadingData)
        Dim par As MainForm = Me.ParentForm

        par.AddWinPart(New uxRequestEdit(Request.NewRequest))
      End Using
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

  Private Sub RequestTypeRepositoryItemHyperLinkEdit_OpenLink(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles RequestTypeRepositoryItemHyperLinkEdit.OpenLink
    Try
      Using busy As New Splash(My.Resources.LoadingData)
        Dim par As MainForm = Me.ParentForm
        Dim _reqest As Request = Request.NewRequest
        Dim reqType As RequestTypeInfo = _
        objRequestTypeList.Item(Me.RequestTypeListBindingSource.Position)
        'da dobie lista
        'If reqType.IsSufficient Then
        '  _reqest.IdRequestType = reqType.IdRequestType
        'Else
        '  _reqest.IdRequestType = reqType.Id
        'End If
        _reqest.IdRequestType = reqType.Id
        par.AddWinPart(New uxRequestEdit(_reqest))
      End Using
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

#End Region

  Private Sub ExpandLevel(ByVal view As DevExpress.XtraGrid.Views.Grid.GridView, ByVal level As Integer)
    GridView1.CollapseAllGroups()

    For i As Integer = -1 To -99999 Step -1
      If Not (view.IsValidRowHandle(i)) Then Exit For
      If (view.GetRowLevel(i) = level) Then
        view.SetRowExpanded(i, True)
      End If
    Next
  End Sub

#Region "Odobrenija i Megunarodni"

  Private Sub btnNewPermission_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewPermission.Click
    Using busy As New Splash(My.Resources.txtLoading)
      Dim par As MainForm = Me.ParentForm
      Try
        par.AddWinPart(New uxDocumentPermisionRequest(DocumentsPermision.NewDocumentsPermision))
      Catch ex As Csla.DataPortalException
        MessageBox.Show(ex.BusinessException.ToString, _
          "Error loading", MessageBoxButtons.OK, _
          MessageBoxIcon.Exclamation)
      Catch ex As Exception
        MessageBox.Show(ex.ToString, _
          "Error loading", MessageBoxButtons.OK, _
          MessageBoxIcon.Exclamation)
      End Try
    End Using
  End Sub

  Private Sub btnNewInternationalDriLic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewInternationalDriLic.Click
    Using busy As New Splash(My.Resources.txtLoading)
      Dim par As MainForm = Me.ParentForm
      Try
        par.AddWinPart(New uxInternationalDriveingLicences _
                       (DocumentsInternationalDriveingLicence.NewDocumentsInternationalDriveingLicence))
      Catch ex As Csla.DataPortalException
        MessageBox.Show(ex.BusinessException.ToString, _
          "Error loading", MessageBoxButtons.OK, _
          MessageBoxIcon.Exclamation)
      Catch ex As Exception
        MessageBox.Show(ex.ToString, _
          "Error loading", MessageBoxButtons.OK, _
          MessageBoxIcon.Exclamation)
      End Try
    End Using
  End Sub

#End Region



  Private Sub btnDogovori_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDogovori.Click
    Using busy As New Splash(My.Resources.txtLoading)
      Try
        Dim par As MainForm = Me.ParentForm
        par.AddWinPart(New uxUnpayedDealsList(False))
      Catch ex As Csla.DataPortalException
        MessageBox.Show(ex.BusinessException.ToString, _
          "Error loading", MessageBoxButtons.OK, _
          MessageBoxIcon.Exclamation)
      Catch ex As Exception
        MessageBox.Show(ex.ToString, _
          "Error loading", MessageBoxButtons.OK, _
          MessageBoxIcon.Exclamation)
      End Try
    End Using
  End Sub

  Private Sub btnRefreshRequests_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshRequests.Click
    _activeDocuments = ActiveDocumentList.GetActiveDocumentList(True, False) 'ActiveDocumentsDepList.GetActiveDocumentsDepList
    Me.ActiveDocumentsDepListBindingSource.DataSource = _activeDocuments
  End Sub

  Private Sub btnRefreshFinances_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshFinances.Click
    _finances = CustumerFinanceList.GetCustumerFinanceList 'CustumerFinanceDepList.GetCustumerFinanceDepList
    Me.CustumerFinanceDepListBindingSource.DataSource = _finances
  End Sub

  Private Sub btnRefreshTechReports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshTechReports.Click
    _incorectTehnicalExams = IncorectTehnicalExamsList.GetIncorectTehnicalExamsList
    Me.IncorectTehnicalExamsDepListBindingSource.DataSource = _incorectTehnicalExams
  End Sub
End Class
