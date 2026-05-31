Imports DevExpress.XtraTreeList
Imports DevExpress.XtraTreeList.Nodes
Public Class uxDocumentTehnicalExamReports

    Private WithEvents _documentTehnicalExamReport As DocumentsTehnicalExamsReport
    Private WithEvents _TehnicalExamTypes As TehnicalExamsTypesList
 Private WithEvents _customerVehiclesRelationsList As CustomerVehiclesRelationsSearchList
    Private WithEvents _tehnicalExamVehiclePartsList As TehnicalExamVehiclePartsList
    Private WithEvents _tehnicalExamOrganizationsList As TehnicalExamOrganizationsList
    Private WithEvents _statusList As DocumentsTehnicalExamsReportsDetailsStatusList
    Private WithEvents _vehicle As Vehicle = Nothing
    Private pomRelacijaVozilo As String = ""
    Private pomRelacijaCustomer As String = ""
    Public Sub New(ByVal InTehExamReport As DocumentsTehnicalExamsReport)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        _documentTehnicalExamReport = InTehExamReport

        LoadList()

        BindUI()
        If _documentTehnicalExamReport.IsNew Then
            _documentTehnicalExamReport.IdOrganizationForTehnicalExam = objCurentTehExamOrganization.Id
        End If
        LookUpEditVehicleShellNum.Enabled = _documentTehnicalExamReport.IsNew
        LookUpEditCustomer.Enabled = _documentTehnicalExamReport.IsNew
        'If _documentTehnicalExamReport.IdCustomerVehicleRelation > 0 Then
        '    LookUpEditCustomer.Properties.ReadOnly = True
        '    LookUpEditVehicleShellNum.Properties.ReadOnly = True
        '    LookUpEditVehicleShellNum.Enabled = True
        '    btnEditVehicle.Enabled = True
        'Else
        '    LookUpEditCustomer.Properties.ReadOnly = False
        '    LookUpEditVehicleShellNum.Properties.ReadOnly = False
        '    LookUpEditVehicleShellNum.Enabled = False
        '    btnEditVehicle.Enabled = False
        'End If

        ' Add any initialization after the InitializeComponent() call.

        ApplyAuthorizationRules()
        GridView1.Columns("IdStatus").FilterInfo = New DevExpress.XtraGrid.Columns.ColumnFilterInfo("([IdStatus] = 1) or ([IdStatus] = 3)")
        ' LookUpEditOrganization.EditValue = objOpcii.Company
    End Sub


    Private Sub ApplyAuthorizationRules()

        ' kontroli(eanble / disable)
        Me.ReadWriteAuthorization1.ResetControlAuthorization()
        If Not VTE.Library.DocumentsTehnicalExamsReport.CanGetObject Then
            Me.Close()
        End If
        Me.btnSave.Enabled = VTE.Library.DocumentsTehnicalExamsReport.CanEditObject
        Me.btnNew.Enabled = VTE.Library.DocumentsTehnicalExamsReport.CanAddObject
        Me.btnSave.Enabled = _documentTehnicalExamReport.IsSavable
        Me.btnNew.Enabled = _documentTehnicalExamReport.IsValid
    End Sub

    Private Sub uxDocumentTehnicalExamReports_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'LookUpEditOrganization.EditValue = objOpcii.Company
        LookUpEditVehicleShellNum.Focus()
    End Sub

#Region " KeyPress "

    Private Sub uxuxDocumentTehnicalExamReports_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
        Select Case Asc(e.KeyChar)
            Case 13
                SendKeys.Send("{TAB}")
        End Select
    End Sub

#End Region

#Region " WinPart Code "

    Protected Overrides Function GetIdValue() As Object

        Return My.Resources.uxDocumentTehnicalExamReports

    End Function

    Public Overrides Function ToString() As String

        Return My.Resources.uxDocumentTehnicalExamReports

    End Function

    Private Sub uxDocumentTehnicalExamReports_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
        ApplyAuthorizationRules()
    End Sub

#End Region

#Region " Bindings "

    Private Sub LoadList()
        If Not _documentTehnicalExamReport.IsNew Then
   _customerVehiclesRelationsList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(_documentTehnicalExamReport.IdCustomerVehicleRelation)
        Else
            _customerVehiclesRelationsList = Nothing
        End If
  Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehiclesRelationsList
  'Me.TreeList1.Nodes(0).ExpandAll()
  _TehnicalExamTypes = TehnicalExamsTypesList.GetTehnicalExamsTypesList
  Me.TehnicalExamsTypesListBindingSource.DataSource = _TehnicalExamTypes




  _tehnicalExamVehiclePartsList = TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsList
  Me.TehnicalExamVehiclePartsListBindingSource.DataSource = _tehnicalExamVehiclePartsList

  _tehnicalExamOrganizationsList = TehnicalExamOrganizationsList.GetTehnicalExamOrganizationsList
  Me.TehnicalExamOrganizationsListBindingSource.DataSource = _tehnicalExamOrganizationsList

  _statusList = DocumentsTehnicalExamsReportsDetailsStatusList.GetDocumentsTehnicalExamsReportsDetailsStatusList
  Me.DocumentsTehnicalExamsReportsDetailsStatusListBindingSource.DataSource = _statusList

  Me.UsersListBindingSource.DataSource = objUsersList
  Me.TehnicalExamVehiclePartsListBindingSource2.DataSource = _tehnicalExamVehiclePartsList
  'CreateColumns(TreeList2)
  TreeList1.Visible = False
  CreateNodes(TreeList2, 0)
  TreeList2.Columns(0).Caption = My.Resources.IncorectParts
  'Me.TreeList1.ForceInitialize()
  'For Each node As T In ASPxTreeList1.GetSelectedNodes()
  '    ASPxTreeList1.DeleteNode(node.Key)
  'Next node

 End Sub







 Private Sub BindUI()
  _documentTehnicalExamReport.BeginEdit()
  Me.DocumentsTehnicalExamsReportBindingSource.DataSource = _documentTehnicalExamReport


 End Sub

 Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
  ' stop the flow of events
  Me.DocumentsTehnicalExamsReportBindingSource.RaiseListChangedEvents = False
  Me.DetailsBindingSource.RaiseListChangedEvents = False
  Me.VisualErrorsBindingSource.RaiseListChangedEvents = False
  UnbindBindingSource(Me.VisualErrorsBindingSource, saveObject, False)
  UnbindBindingSource(Me.DetailsBindingSource, saveObject, False)
  UnbindBindingSource(Me.DocumentsTehnicalExamsReportBindingSource, saveObject, True)

  Me.DetailsBindingSource.DataSource = Me.DocumentsTehnicalExamsReportBindingSource
  Me.VisualErrorsBindingSource.DataSource = Me.DocumentsTehnicalExamsReportBindingSource
  Try
   ' save or cancel changes
   If saveObject Then
    _documentTehnicalExamReport.ApplyEdit()

    Try
     _documentTehnicalExamReport = _documentTehnicalExamReport.Save

    Catch ex As Csla.Validation.ValidationException
     MsgBox("Some validation errors has occurred," + vbCrLf + " please check Broken rules collection on bottom for details")

    Catch ex As Csla.DataPortalException
     MessageBox.Show(ex.BusinessException.ToString, _
       "Error saving", MessageBoxButtons.OK, _
       MessageBoxIcon.Exclamation)

    Catch ex As Exception
     MessageBox.Show(ex.ToString, _
       "Error saving", MessageBoxButtons.OK, _
       MessageBoxIcon.Exclamation)
    End Try

   Else
    _documentTehnicalExamReport.CancelEdit()
   End If

  Finally
   ' rebind UI if requested
   If rebind Then
    BindUI()
   End If
   Me.DocumentsTehnicalExamsReportBindingSource.RaiseListChangedEvents = True
   Me.DetailsBindingSource.RaiseListChangedEvents = True
   Me.VisualErrorsBindingSource.RaiseListChangedEvents = True
   If rebind Then
    Me.DocumentsTehnicalExamsReportBindingSource.ResetBindings(False)
    Me.DetailsBindingSource.ResetBindings(False)
    Me.VisualErrorsBindingSource.ResetBindings(False)
   End If

  End Try
 End Sub

 Private Sub BindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
   DocumentsTehnicalExamsReportBindingSource.CurrentItemChanged, _
   DetailsBindingSource.CurrentItemChanged, VisualErrorsBindingSource.CurrentItemChanged

  Me.btnSave.Enabled = _documentTehnicalExamReport.IsSavable
  Me.btnNew.Enabled = _documentTehnicalExamReport.IsValid
  Me.btnPrint.Enabled = Not _documentTehnicalExamReport.IsDirty

  Me.LookUpEditCustomer.Enabled = _documentTehnicalExamReport.IsNew

  Dim message As New System.Text.StringBuilder

  message.AppendFormat("{0}" + vbCrLf, "")
  For Each rule As Csla.Validation.BrokenRule In _documentTehnicalExamReport.BrokenRulesCollection
   message.AppendFormat( _
     "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
  Next
  'detali
  For Each child As DocumentsTehnicalExamsReportsDetail In _documentTehnicalExamReport.Details
   For Each rule As Csla.Validation.BrokenRule In child.BrokenRulesCollection
    message.AppendFormat( _
      "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
   Next
  Next
  For Each child As DocumentsTehnicalExamsReportsVisualError In _documentTehnicalExamReport.VisualErrors
   For Each rule As Csla.Validation.BrokenRule In child.BrokenRulesCollection
    message.AppendFormat( _
      "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
   Next
  Next
  ' For Each child As DocumentsTehnicalExamsReportsMeasuredValues In _documentTehnicalExamReport.MeasuredValues
  '  Next
  ShowBrokenRules(message.ToString, True)

 End Sub

 'Private Sub DetaliBindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _
 '  DetailsBindingSource.ListChanged
 '  If (Me.DetailsBindingSource IsNot Nothing) AndAlso (e.ListChangedType = System.ComponentModel.ListChangedType.ItemDeleted) Then
 '    Me.btnSave.Enabled = _documentTehnicalExamReport.IsSavable 'IsValid

 '  End If
 'End Sub

#End Region

#Region " Kopcinja "

 Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
  Me.Close()
 End Sub

 Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
  ' proveri dali ima validen tehnicki pregled za relacijata(Vozilo), osven ako ne e istiot
  ' ako ima stavim ValidTillDate = now
  Using busy As New Splash("Запамтувам...")
   'If _documentTehnicalExamReport.IsNew Then
   '    Dim proverkaNaPostoeckiPregled As DocumentsTehnicalExamsReport
   '    Try
   '        proverkaNaPostoeckiPregled = DocumentsTehnicalExamsReports. _
   '        GetDocumentTehnicalExamReportByIdRelation(_documentTehnicalExamReport.IdCustomerVehicleRelation)
   '    Catch ex As Exception
   '        proverkaNaPostoeckiPregled = Nothing
   '    End Try

   '    If proverkaNaPostoeckiPregled IsNot Nothing AndAlso _
   '    proverkaNaPostoeckiPregled.Id > 0 Then
   '        DocumentsTehnicalExamsReport.ZatvoriValidnostNaTehnicki(proverkaNaPostoeckiPregled.Id)
   '    End If


   'End If
   RebindUI(True, True)
  End Using
 End Sub

 Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click

  Try
   RebindUI(True, True)
   'tuka otvori pecati

  Catch ex As Exception
   Exit Sub
  End Try

  _documentTehnicalExamReport = Nothing
  _documentTehnicalExamReport = DocumentsTehnicalExamsReport.NewDocumentsTehnicalExamsReport
  BindUI()

  LookUpEditCustomer.Properties.ReadOnly = False

  Me.LookUpEditCustomer.Focus()
 End Sub

 Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
  RebindUI(False, True)
 End Sub

 Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    If objCurentTehExamOrganization.NewTechnicalExamReport Then
      Dim rptNewTehnicalExamReport As rptNewTehnicalExamReport = New rptNewTehnicalExamReport(_documentTehnicalExamReport.IdCustomerVehicleRelation, _documentTehnicalExamReport.Id)
      'rptNewTehnicalExamReport.ShowPreviewDialog()
      Dim ux1 As New uxPrint(rptNewTehnicalExamReport)
      CType(Me.ParentForm, MainForm).AddWinPart(ux1)
    Else
      Dim rptTehnicalExamReport As rptTehnickiPregledZapisnik = New rptTehnickiPregledZapisnik(_documentTehnicalExamReport.Id)
      ''------------------
      If rptTehnicalExamReport.IsRight Then
        Dim rptPotvrdaTehnicalExamReport As rptPotvrdaZaTehnickaIspravnost = _
  New rptPotvrdaZaTehnickaIspravnost(_documentTehnicalExamReport.Id)
        Dim ux As New uxPrint(rptPotvrdaTehnicalExamReport)
        CType(Me.ParentForm, MainForm).AddWinPart(ux)
      End If

      Dim ux2 As New uxPrint(rptTehnicalExamReport)
      CType(Me.ParentForm, MainForm).AddWinPart(ux2)
    End If

    ''Dim rptPotvrdaTehnicalExamReport As rptPotvrdaZaTehnickaIspravnost = New rptPotvrdaZaTehnickaIspravnost(_documentTehnicalExamReport.Id)
    ' '' rptPotvrdaTehnicalExamReport.ShowPreviewDialog()
    ''Dim ux As New uxPrint(rptPotvrdaTehnicalExamReport)
    ''CType(Me.ParentForm, MainForm).AddWinPart(ux)
 End Sub

#End Region

#Region " Grid Events "

 'Private Sub VehicleBrakesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DetailsGridControl.ProcessGridKey

 '  If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
 '    If _documentTehnicalExamReport.IsSavable Then
 '      GridView1.AddNewRow()
 '    Else
 '      e.SuppressKeyPress = True
 '    End If
 '  End If

 'End Sub

 'Private Sub LookUpEditVehicleParts_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditVehicleParts.ButtonPressed
 '  If e.Button.Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Delete Then
 '    Me.DetailsBindingSource.RemoveCurrent()
 '  End If
 'End Sub

 Private Sub GridView1_ValidateRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs) Handles GridView1.ValidateRow
  Dim detal As DocumentsTehnicalExamsReportsDetail = CType(e.Row, DocumentsTehnicalExamsReportsDetail)
  If detal.IsNew AndAlso detal.IdStatus = 2 Then
   e.ErrorText = "Може да е исправен или неисправен"
   e.Valid = False
  End If
  If e.Valid Then
   For Each detall As DocumentsTehnicalExamsReportsDetail In _documentTehnicalExamReport.Details
    If (detall.IdTehnicalExamVehivlePart = detal.IdTehnicalExamVehivlePart) _
        AndAlso (Not ReferenceEquals(detall, detal)) AndAlso _
        (detall.IdStatus = 3) Then
     detall.IdStatus = 2
     'detal.IdStatus = 3
    End If
   Next
  End If

 End Sub

#End Region

#Region " Focus "
 Private Sub DetaliGridControl_Enter(ByVal sender As Object, ByVal e As System.EventArgs) _
  Handles DetailsGridControl.Enter

  DetailsGridControl.FocusedView = GridView1
  If GridView1.RowCount = 0 Then
   GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle

  End If
  Me.GridView1.FocusedColumn = colIdTehnicalExamVehivlePart
 End Sub

 Private Sub DetaliGridControl_Leave(ByVal sender As Object, ByVal e As System.EventArgs) _
  Handles DetailsGridControl.Leave
  GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle
 End Sub
#End Region


 Private Sub LookUpEdit2Controler_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEdit2Controler.ButtonPressed, LookUpEdit1Controler.ButtonPressed
  If e.Button.Index = 1 Then
   Select Case sender.name
    Case "LookUpEdit2Controler"
     Dim dij As New Login(True)
     If dij.ShowDialog(Me) = DialogResult.OK Then
      _documentTehnicalExamReport.IdSecondControler = dij.VerifyIdentityEmployeeId
     End If
    Case "LookUpEdit1Controler"
     Dim dij As New Login(True)
     If dij.ShowDialog(Me) = DialogResult.OK Then
      _documentTehnicalExamReport.IdFirsControler = dij.VerifyIdentityEmployeeId
     End If
   End Select
   'If sender.name = "LookUpEdit2Controler" Then

   '    Dim dij As New Login(True)
   '    If dij.ShowDialog(Me) = DialogResult.OK Then
   '        _documentTehnicalExamReport.IdSecondControler = dij.VerifyIdentityEmployeeId
   '    End If
   'End If
  End If
 End Sub


 Private Sub LookUpEditVehicleShellNum_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditCustomer.ButtonPressed
  Select Case e.Button.Index

   'Case 1

   '    Dim dij As New dijVehicleCustomerList()
   '    If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
   '        _documentTehnicalExamReport.IdCustomerVehicleRelation = dij.SelectedCustomerVehicles.Id
   '    End If

   Case 1
    If DockManager1.Panels.Count <= 2 Then
     Dim panel As DevExpress.XtraBars.Docking.DockPanel = Me.DockManager1.AddPanel(DevExpress.XtraBars.Docking.DockingStyle.Bottom)
     panel.Name = "addRelationPanel"
     panel.Text = "Додавње на нова релација"
     Dim ux As New uxAddNewRelation()
     AddHandler ux.Disposed, AddressOf ux_dispose
     panel.Size = New Size(600, 150)
     panel.Controls.Add(ux)
     ux.Dock = DockStyle.Fill
     ux.BringToFront()
     panel.Top = True
     panel.Options.AllowDockBottom = True
     panel.Options.AllowDockFill = False
     panel.Options.AllowDockLeft = False
     panel.Options.AllowDockRight = False
     panel.Options.ShowCloseButton = False
     panel.Options.ShowAutoHideButton = False
     panel.BringToFront()

     panel.Show()

    Else
     DockManager1.Panels.Item(DockManager1.Panels.Count - 1).Focus()
    End If
  End Select
 End Sub
 Private Sub LookUpEditCustomer_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditVehicleShellNum.ButtonPressed
  Select Case e.Button.Index

   'Case 1

   '    Dim dij As New dijVehicleCustomerList()
   '    If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
   '        _documentTehnicalExamReport.IdCustomerVehicleRelation = dij.SelectedCustomerVehicles.Id
   '    End If

   Case 1
    If DockManager1.Panels.Count <= 2 Then
     Dim panel As DevExpress.XtraBars.Docking.DockPanel = Me.DockManager1.AddPanel(DevExpress.XtraBars.Docking.DockingStyle.Bottom)
     panel.Name = "addRelationPanel"
     panel.Text = "Додавње на нова релација"
     Dim ux As New uxAddNewRelation()
     AddHandler ux.Disposed, AddressOf ux_dispose
     panel.Size = New Size(600, 150)
     panel.Controls.Add(ux)
     ux.Dock = DockStyle.Fill
     ux.BringToFront()
     panel.Top = True
     panel.Options.AllowDockBottom = True
     panel.Options.AllowDockFill = False
     panel.Options.AllowDockLeft = False
     panel.Options.AllowDockRight = False
     panel.Options.ShowCloseButton = False
     panel.Options.ShowAutoHideButton = False
     panel.BringToFront()

     panel.Show()

    Else
     DockManager1.Panels.Item(DockManager1.Panels.Count - 1).Focus()
    End If
  End Select
 End Sub
 Private Sub ux_dispose(ByVal sender As Object, ByVal e As System.EventArgs)
  If CType(sender, uxAddNewRelation).SelectedRelationId <> 0 Then
   Me.CustomerVehiclesRelationsSearchListBindingSource.RaiseListChangedEvents = False
   _customerVehiclesRelationsList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(CType(sender, uxAddNewRelation).SelectedRelationId)
   Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehiclesRelationsList
   Me.CustomerVehiclesRelationsSearchListBindingSource.RaiseListChangedEvents = True
   Me.CustomerVehiclesRelationsSearchListBindingSource.ResetBindings(False)
   _documentTehnicalExamReport.IdCustomerVehicleRelation = CType(sender, uxAddNewRelation).SelectedRelationId
   LookUpEditCustomer.EditValue = CType(sender, uxAddNewRelation).SelectedRelationId
  End If
  Me.DockManager1.RemovePanel(Me.DockManager1.Panels("addRelationPanel"))
 End Sub
 Private Sub leTehnicalExamsTypes_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles leTehnicalExamsTypes.Validated
  Try
   _documentTehnicalExamReport.ValidTillDate = _documentTehnicalExamReport.MadeDate.AddDays(_TehnicalExamTypes.GetInfoById(leTehnicalExamsTypes.EditValue).ValidNumOfDays)
  Catch ex As Exception

  End Try

 End Sub

 Private Sub btnEditVehicle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditVehicle.Click
  Dim par As MainForm = Me.ParentForm
  Using cekaj As New StatusBusy(My.Resources.txtLoading)
   Dim vehicleID As Integer = _
       Me.LookUpEditCustomer.GetColumnValue("IdVehicle")
   _vehicle = Vehicle.GetVehicle(vehicleID)
   par.AddWinPart(New uxVehicle(_vehicle))
  End Using
 End Sub

 Private Sub LookUpEditVehicleShellNum_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditCustomer.EditValueChanged, LookUpEditVehicleShellNum.EditValueChanged
  If LookUpEditCustomer.EditValue > 0 Or LookUpEditVehicleShellNum.EditValue > 0 Then
   btnEditVehicle.Enabled = True
  Else
   btnEditVehicle.Enabled = False
  End If
 End Sub




 'Private Sub RepositoryItemHyperLinkNeispravnosti_OpenLink(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles RepositoryItemHyperLinkNeispravnosti.OpenLink

 '    Try
 '        If _documentTehnicalExamReport.Details.Count > 0 Then
 '            For Each detal As DocumentsTehnicalExamsReportsDetail In _documentTehnicalExamReport.Details
 '                If detal.IdTehnicalExamVehivlePart = _tehnicalExamVehiclePartsList.Item(Me.TehnicalExamVehiclePartsListBindingSource2.Position).Id Then

 '                    Exit Sub

 '                End If
 '            Next
 '        End If
 '        Dim tehDetail As DocumentsTehnicalExamsReportsDetail = _documentTehnicalExamReport.Details.AddNew
 '        tehDetail.IdTehnicalExamVehivlePart = _tehnicalExamVehiclePartsList.Item(Me.TehnicalExamVehiclePartsListBindingSource2.Position).Id

 '    Catch ex As Exception

 '    End Try
 'End Sub
#Region "TreeList panels"
 'Private Sub CreateColumns(ByVal tl As TreeList)
 '    ' Create three columns.
 '    tl.BeginUpdate()
 '    tl.Columns.Add()
 '    tl.Columns(0).Caption = ""
 '    tl.Columns(0).VisibleIndex = 0
 '    tl.Columns(0).ColumnEdit = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
 '    tl.Columns(0).ColumnEditName = "HyperLink" & tl.Name
 '    tl.Columns.Add()
 '    tl.Columns(1).Caption = ""
 '    tl.Columns(1).VisibleIndex = 1
 '    tl.Columns(1).Visible = False

 '    tl.EndUpdate()
 'End Sub

 Private Sub CreateNodes(ByVal tl As TreeList, ByVal inId As Integer)
  tl.BeginUnboundLoad()
  ' Create a root node .

  For Each item As TehnicalExamVehiclePartsInfo In _tehnicalExamVehiclePartsList
   If item.IdCategoryVehicleParts = inId Then
    Dim parentForRootNodes As TreeListNode = Nothing
    Dim rootNode As TreeListNode = tl.AppendNode(New Object() {item.Description, item.Id}, parentForRootNodes)
   End If
  Next

  tl.EndUnboundLoad()
  'tl.Visible = True
 End Sub
 Private Sub HyperLinkTreeList2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HyperLinkTreeList2.Click
  TreeList4.ClearNodes()
  TreeList4.Visible = False
  LayoutControlTreeList4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
  TreeList3.ClearNodes()
  CreateNodes(TreeList3, TreeList2.FocusedNode.Item(1)) 'e.Node.Item(1))
  TreeList3.Columns(0).Caption = TreeList2.FocusedNode.Item(0) ' e.Node.Item(0)

 End Sub

 Private Sub HyperLinkTreeList3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HyperLinkTreeList3.Click
  TreeList4.ClearNodes()

  TreeList4.Visible = True
  LayoutControlTreeList4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
  CreateNodes(TreeList4, TreeList3.FocusedNode.Item(1)) 'e.Node.Item(1))
  TreeList4.Columns(0).Caption = TreeList3.FocusedNode.Item(0) ' e.Node.Item(0)

 End Sub
#End Region



 Private Sub HyperLinkTreeList2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
 HyperLinkTreeList2.DoubleClick, HyperLinkTreeList3.DoubleClick, HyperLinkTreeList4.DoubleClick, HyperLinkTreeList5.DoubleClick
  Try
   Dim pomId As Integer = 0
   Select Case sender.parent.name
    Case "TreeList2"
     pomId = TreeList2.FocusedNode.Item(1)
    Case "TreeList3"
     pomId = TreeList3.FocusedNode.Item(1)
    Case "TreeList4"
     pomId = TreeList4.FocusedNode.Item(1)
    Case "TreeList5"
     pomId = TreeList5.FocusedNode.Item(1)
   End Select
   If _documentTehnicalExamReport.Details.Count > 0 Then

    For Each detal As DocumentsTehnicalExamsReportsDetail In _documentTehnicalExamReport.Details
     If detal.IdTehnicalExamVehivlePart = pomId Then '_tehnicalExamVehiclePartsList.Item(Me.TehnicalExamVehiclePartsListBindingSource2.Position).Id Then
      Exit Sub
     End If
    Next
   End If
   Dim tehDetail As DocumentsTehnicalExamsReportsDetail = _documentTehnicalExamReport.Details.AddNew
   tehDetail.IdTehnicalExamVehivlePart = pomId

  Catch ex As Exception

  End Try
 End Sub

 Private Sub LookUpEditCustomer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditCustomer.GotFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo(uSettings.Culture))
 End Sub

 Private Sub LookUpEditCustomer_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LookUpEditCustomer.KeyUp

  If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
  e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
  AndAlso e.KeyData <> Keys.Tab Then
   pomRelacijaCustomer = LookUpEditCustomer.Text
  Else
   Exit Sub
  End If
  If pomRelacijaCustomer <> String.Empty AndAlso pomRelacijaCustomer.Length >= 7 Then
   Dim par As MainForm = Me.ParentForm
   'Dim pomTekst As String = LookUpEditCustomer.Text
   Using cekaj As New StatusBusy(My.Resources.txtLoading)

    Try
     _customerVehiclesRelationsList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListByString(pomRelacijaCustomer, False)
    Catch ex As Exception
     _customerVehiclesRelationsList = Nothing
    End Try

    Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehiclesRelationsList
    If _customerVehiclesRelationsList.Count > 0 Then

     LookUpEditCustomer.ClosePopup()
     LookUpEditCustomer.ShowPopup()
     LookUpEditCustomer.Text = pomRelacijaCustomer

    Else
     If DockManager1.Panels.Count <= 2 Then

      pomRelacijaCustomer = ""
      LookUpEditCustomer.Text = pomRelacijaCustomer
      Dim panel As DevExpress.XtraBars.Docking.DockPanel = Me.DockManager1.AddPanel(DevExpress.XtraBars.Docking.DockingStyle.Bottom)
      panel.Name = "addRelationPanel"
      panel.Text = "Додавње на нова релација"
      Dim ux As New uxAddNewRelation()
      AddHandler ux.Disposed, AddressOf ux_dispose
      panel.Size = New Size(600, 150)
      panel.Controls.Add(ux)
      ux.Dock = DockStyle.Fill
      ux.BringToFront()

      panel.Top = True
      panel.Options.AllowDockBottom = True
      panel.Options.AllowDockFill = False
      panel.Options.AllowDockLeft = False
      panel.Options.AllowDockRight = False
      panel.Options.AllowFloating = False
      panel.Options.ShowCloseButton = False
      panel.Options.ShowAutoHideButton = False
      panel.BringToFront()

      panel.Show()

     Else
      DockManager1.Panels.Item(DockManager1.Panels.Count - 1).Focus()
     End If
    End If

   End Using
  End If
 End Sub

 Private Sub LookUpEditVehicleShellNum_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditVehicleShellNum.GotFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
 End Sub


 Private Sub LookUpEditVehicleShellNum_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LookUpEditVehicleShellNum.KeyUp

  If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
  e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
  AndAlso e.KeyData <> Keys.Tab Then
   pomRelacijaVozilo = LookUpEditVehicleShellNum.Text
  Else
   Exit Sub
  End If
  If pomRelacijaVozilo <> String.Empty AndAlso pomRelacijaVozilo.Length >= 4 Then
   Dim par As MainForm = Me.ParentForm
   'Dim pomTekst As String = LookUpEditCustomer.Text
   Using cekaj As New StatusBusy(My.Resources.txtLoading)

    Try
     _customerVehiclesRelationsList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListByString(pomRelacijaVozilo, True)
    Catch ex As Exception
     _customerVehiclesRelationsList = Nothing
    End Try

    Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehiclesRelationsList
    If _customerVehiclesRelationsList.Count > 0 Then

     LookUpEditVehicleShellNum.ClosePopup()
     LookUpEditVehicleShellNum.ShowPopup()
     LookUpEditVehicleShellNum.Text = pomRelacijaVozilo

    Else
     If DockManager1.Panels.Count <= 2 Then

      pomRelacijaCustomer = ""
      LookUpEditVehicleShellNum.Text = pomRelacijaVozilo
      Dim panel As DevExpress.XtraBars.Docking.DockPanel = Me.DockManager1.AddPanel(DevExpress.XtraBars.Docking.DockingStyle.Bottom)
      panel.Name = "addRelationPanel"
      panel.Text = "Додавње на нова релација"
      Dim ux As New uxAddNewRelation()
      AddHandler ux.Disposed, AddressOf ux_dispose
      panel.Size = New Size(600, 150)
      panel.Controls.Add(ux)
      ux.Dock = DockStyle.Fill
      ux.BringToFront()

      panel.Top = True
      panel.Options.AllowDockBottom = True
      panel.Options.AllowDockFill = False
      panel.Options.AllowDockLeft = False
      panel.Options.AllowDockRight = False
      panel.Options.AllowFloating = False
      panel.Options.ShowCloseButton = False
      panel.Options.ShowAutoHideButton = False
      panel.BringToFront()

      panel.Show()

     Else
      DockManager1.Panels.Item(DockManager1.Panels.Count - 1).Focus()
     End If
    End If

   End Using
  End If
 End Sub
End Class
