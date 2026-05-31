Public Class uxRequestEdit

 Private WithEvents _request As Request
 Public ReadOnly Property request() As Request
  Get
   Return _request
  End Get
 End Property
 Private WithEvents _vehicle As Vehicle
 Private WithEvents _customer As Customer
 'readonly list
 Private WithEvents _customerList As CustomersSearchList
 Private WithEvents _customerInfo As CustomersSearchInfo
 Private WithEvents _customerVehicleRelationList As CustomerVehiclesRelationsSearchList
 Private WithEvents _issuerList As RegistrationIssuerList
 Private WithEvents _cityList As CityList
 Private WithEvents _ownershpProofsList As DocumentVehicleOwnershipProofList
 Private WithEvents _paymentProofsList As DocumentPaymentProofList
 ' Private WithEvents _attachmentTypeList As AttachmentTypeList
 Private WithEvents _newTehnicalExam As DocumentsTehnicalExamsReport = Nothing
 Private pomRelacija As String = ""
 Private pomRelacijaCustomer As String = ""
 Private pom As String = ""
 Public Sub New(ByVal request As Request)

  ' This call is required by the Windows Form Designer.
  InitializeComponent()

  ' Add any initialization after the InitializeComponent() call.
  _request = request
  LoadList()
  BindUI()
  ApplyAuthorizationRules()

 End Sub

 Private Sub ApplyAuthorizationRules()

 End Sub

 Private Sub uxRequestEdit_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  If _request.IsNew Then
   Dim reqType As RequestTypeInfo = _
   CType(Csla.ApplicationContext.LocalContext("objRequestTypeList"),  _
   RequestTypeList).getInfoById(_request.IdRequestType)
   If reqType.IsSufficient Then
    Me.UxRequestTypeChooser1.SelectType(_request.IdRequestType)
    Me.LookUpEditRelation.Focus()
   Else
    Me.UxRequestTypeChooser1.InitControl(_request.IdRequestType)
    Me.UxRequestTypeChooser1.Focus()
   End If
  Else
   Me.UxRequestTypeChooser1.SelectType(_request.IdRequestType)
   'LoadDependantList()
   Me.LookUpEditRelation.Focus()
  End If
 End Sub

 Private Sub _request_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _request.PropertyChanged
  Select Case e.PropertyName
   'Case "IdCustomerVehicleRelation"
   '  If _request.IdCustomerVehicleRelation > 0 Then
   '    LoadDependantList()
   '  End If
   Case "IdRequestType"
    SetupTypeDepend()
  End Select
 End Sub

 Private Sub SetupTypeDepend()
  'enable/disable groups
  If Me.UxRequestTypeChooser1.CurrentRequestTypeInfo.IsNewCustomer Then
   Me.NewOwnerLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
  Else
   Me.NewOwnerLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
  End If
  'If Me.UxRequestTypeChooser1.CurrentRequestTypeInfo.IsPreviosRegistrationReqired Then
  '  Me.PreviosRegistrationLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
  'Else
  '  Me.PreviosRegistrationLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
  'End If
  _request.IsCustomerChanged = Me.UxRequestTypeChooser1.CurrentRequestTypeInfo.IsCustomerChanged
  _request.IsVehicleChanged = Me.UxRequestTypeChooser1.CurrentRequestTypeInfo.IsVehicleChanged
  'disable vehicleEdit
  'Me.LookUpEditRelation.Properties.Buttons(2).Enabled = Me.UxRequestTypeChooser1.CurrentRequestTypeInfo.IsVehicleChanged
  'diable customer edit
  'Me.LookUpEditRelation.Properties.Buttons(3).Enabled = Me.UxRequestTypeChooser1.CurrentRequestTypeInfo.IsVehicleChanged
 End Sub

#Region " Bindings "

 'Private Sub LoadDependantList()
 '  'inicijalizacija na zavisni listi
 '  Me.VehicleRegistrationListBindingSource.DataSource = _
 '    VehicleRegistrationList.GetVehicleRegistrationList(_customerVehicleRelationList.GetInfoRelationById(_request.IdCustomerVehicleRelation).IdVehicle)

 'End Sub

 Public Sub LoadList()
  _customerList = Nothing 'objCustomersListShort
  'Me.CustomersListBindingSource.DataSource = _customerList
  '    _customerVehicleRelationList = objRlationList  'CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsListByTypeOfRelation(1)
  'Me.CustomerVehiclesRelationsListBindingSource.DataSource = _customerVehicleRelationList
  _issuerList = objRegistrationIssuerList 'RegistrationIssuerList.GetRegistrationIssuerList()
  If Not _request.IsNew Then
   _customerVehicleRelationList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(_request.IdCustomerVehicleRelation)
   Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehicleRelationList
   If _request.IdCustomerVehicleRelationNew > 0 Then
    _customerInfo = CustomersSearchList.GetCustomersListShortById(_request.IdCustomerVehicleRelationNew).Item(0)
    Me.CustomersSearchListBindingSource.DataSource = _customerInfo
   End If

  End If
  Me.RegistrationIssuerListBindingSource.DataSource = _issuerList
  _cityList = objCityList 'CityList.GetCityList
  Me.CityListBindingSource.DataSource = _cityList
  _ownershpProofsList = objOwnershipProofList  'DocumentVehicleOwnershipProofList.GetDocumentVehicleOwnershipProofList
  Me.DocumentVehicleOwnershipProofListBindingSource.DataSource = _ownershpProofsList
  _paymentProofsList = objPaymentProofList 'DocumentPaymentProofList.GetDocumentPaymentProofList
  Me.DocumentPaymentProofListBindingSource.DataSource = _paymentProofsList
  '_attachmentTypeList = AttachmentTypeList.GetAttachmentTypeList
  ' Me.AttachmentTypeListBindingSource.DataSource = _attachmentTypeList
 End Sub

 Private Sub BindUI()
  Try
   _request.BeginEdit()
   Me.RequestBindingSource.DataSource = _request
  Catch ex As Exception
   MsgBox(ex.Message)
  End Try
 End Sub

 Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
  ' stop the flow of events
  Me.RequestBindingSource.RaiseListChangedEvents = False
  Me.OwnershipProofsBindingSource.RaiseListChangedEvents = False
  Me.PaymentProofsBindingSource.RaiseListChangedEvents = False
  'Me.AttachmentsBindingSource.RaiseListChangedEvents = False
  ' commit edits in memory
  UnbindBindingSource(Me.OwnershipProofsBindingSource, saveObject, False)
  UnbindBindingSource(Me.PaymentProofsBindingSource, saveObject, False)
  ' UnbindBindingSource(Me.AttachmentsBindingSource, saveObject, False)
  UnbindBindingSource(Me.RequestBindingSource, saveObject, True)

  Me.OwnershipProofsBindingSource.DataSource = Me.RequestBindingSource
  Me.PaymentProofsBindingSource.DataSource = Me.RequestBindingSource
  '  Me.AttachmentsBindingSource.DataSource = Me.RequestBindingSource
  Try
   ' save or cancel changes
   If saveObject Then

    _request.ApplyEdit()

    Try
     'avtomatski da pravi tehnicki
     'stavi nova relacija
     Dim newRel As CustomerVehiclesRelation = Nothing
     If Me.UxRequestTypeChooser1.CurrentRequestTypeInfo.IsNewCustomer Then
      If CustomerVehiclesRelation.ExistsRelation(_request.IdCustomerVehicleRelationNew, CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(_request.IdCustomerVehicleRelation).Item(0).IdVehicle) Then
       newRel = CustomerVehiclesRelation.GetCustomerVehiclesRelation(CustomerVehiclesRelation.ExistsRelationId(_request.IdCustomerVehicleRelationNew, CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(_request.IdCustomerVehicleRelation).Item(0).IdVehicle))
       'CustomerVehiclesRelation.GetCustomerVehiclesRelation(CustomerVehiclesRelation.ExistsRelationId(_request.IdCustomerVehicleRelationNew, _customerVehicleRelationList.GetInfoRelationById(_request.IdCustomerVehicleRelation).IdVehicle))
      Else
       newRel = CustomerVehiclesRelation.NewCustomerVehiclesRelation
       newRel.IdCustomer = _request.IdCustomerVehicleRelationNew 'Me.IdCustomerVehicleRelationNew
       newRel.IdVehicle = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(_request.IdCustomerVehicleRelation).Item(0).IdVehicle 'curRel.IdVehicle
       newRel.IdRelationType = 1
       newRel = newRel.Save()
      End If
     End If
     'Dim newTehnicalExam As DocumentsTehnicalExamsReport = Nothing
     If objCurentTehExamOrganization.AutmateProceses AndAlso _
     (_request.IdTechnicalExamReport = 0) AndAlso _
     (Me.UxRequestTypeChooser1.CurrentRequestTypeInfo.IsTehnicalExamRequired > 0) Then
    
      _newTehnicalExam = DocumentsTehnicalExamsReport.NewDocumentsTehnicalExamsReport
      '  Dim pomRelation As Long
      'pomRelation = _customerVehicleRelationList.GetCustomerVehiclesRelationsListById (_request.IdCustomerVehicleRelation).Id
      If _request.IdCustomerVehicleRelationNew > 0 Then
       _newTehnicalExam.IdCustomerVehicleRelation = newRel.Id
      Else
       _newTehnicalExam.IdCustomerVehicleRelation = _request.IdCustomerVehicleRelation
      End If
      _newTehnicalExam.IdTypeOfTehnicalExam = Me.UxRequestTypeChooser1.CurrentRequestTypeInfo.IsTehnicalExamRequired '1
      _newTehnicalExam.MadeDate = Now.Date
      _newTehnicalExam.ValidTillDate = Now.AddYears(1).Date
      '_newTehnicalExam.IdCustomerVehicleRelation = _customerVehicleRelationList.GetInfoRelationById(_request.IdCustomerVehicleRelation).Id '_request.IdCustomerVehicleRelation
      _newTehnicalExam = _newTehnicalExam.Save
      ' End If


      _request.IdTechnicalExamReport = _newTehnicalExam.Id

     End If

     _request = _request.Save
     'If _newTehnicalExam Is Nothing Then
     '    _newTehnicalExam = DocumentsTehnicalExamsReports.GetDocumentsTehnicalExamsReports. _
     '    GetTechnicalExamReportById(_request.IdTechnicalExamReport)
     'End If

    Catch ex As Csla.DataPortalException
     MessageBox.Show(ex.BusinessException.ToString(), _
       "Error saving", MessageBoxButtons.OK, _
       MessageBoxIcon.Exclamation)
    Catch ex As Exception
     MessageBox.Show(ex.ToString(), _
       "Error Saving", MessageBoxButtons.OK, _
       MessageBoxIcon.Exclamation)
    End Try
   Else
    _request.CancelEdit()
   End If
  Finally
   'rebind UI if requested
   If rebind Then
    BindUI()
   End If

   ' restore events
   Me.RequestBindingSource.RaiseListChangedEvents = True
   Me.OwnershipProofsBindingSource.RaiseListChangedEvents = True
   Me.PaymentProofsBindingSource.RaiseListChangedEvents = True
   ' Me.AttachmentsBindingSource.RaiseListChangedEvents = True
   If rebind Then
    ' refresh the UI if rebinding
    Me.RequestBindingSource.ResetBindings(False)
    Me.OwnershipProofsBindingSource.ResetBindings(False)
    Me.PaymentProofsBindingSource.ResetBindings(False)

    Me.tPage.Text = Me.ToString
   End If

  End Try

 End Sub

 Private Sub BindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
   RequestBindingSource.CurrentItemChanged, _
   PaymentProofsBindingSource.CurrentItemChanged, _
   OwnershipProofsBindingSource.CurrentItemChanged

  Me.btnSave.Enabled = _request.IsSavable
  Me.btnPrint.Enabled = Not _request.IsDirty

  Dim message As New System.Text.StringBuilder

  message.AppendFormat("{0}" + vbCrLf, "")
  For Each rule As Csla.Validation.BrokenRule In _request.BrokenRulesCollection
   message.AppendFormat( _
     "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
  Next
  'detali
  For Each child As RequestPaymentProof In _request.PaymentProofs
   For Each rule As Csla.Validation.BrokenRule In child.BrokenRulesCollection
    message.AppendFormat( _
      "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
   Next
  Next
  For Each child As RequestVehicleOwnershipProof In _request.OwnershipProofs
   For Each rule As Csla.Validation.BrokenRule In child.BrokenRulesCollection
    message.AppendFormat( _
      "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
   Next
  Next
  ShowBrokenRules(message.ToString, True)

 End Sub

 Private Sub DetaliBindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _
     PaymentProofsBindingSource.ListChanged, _
     OwnershipProofsBindingSource.ListChanged

  If (Me.PaymentProofsBindingSource IsNot Nothing) AndAlso (e.ListChangedType = System.ComponentModel.ListChangedType.ItemDeleted) Then
   Me.btnSave.Enabled = _request.IsValid
  End If
  If (Me.OwnershipProofsBindingSource IsNot Nothing) AndAlso (e.ListChangedType = System.ComponentModel.ListChangedType.ItemDeleted) Then
   Me.btnSave.Enabled = _request.IsValid
  End If
 End Sub

#End Region

#Region "PritisnatoKopce"


 Private Sub ux_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
  Select Case Asc(e.KeyChar)
   Case 13
        SendKeys.Send("{TAB}")

  End Select
 End Sub
#End Region

#Region "WinPart"

 Protected Overrides Function GetIdValue() As Object
  If _request IsNot Nothing Then
   Return "Барање бр." & _request.Id
  Else
   Return "Барање бр." & 0
  End If
 End Function

 Public Overrides Function ToString() As String
  If _request IsNot Nothing Then
   Return "Барање бр." & _request.Id
  Else
   Return "Барање бр." & 0
  End If
 End Function

#End Region

#Region " Request Types "
 Private Sub UxRequestTypeChooser1_CurrentRequestTypeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxRequestTypeChooser1.CurrentRequestTypeChanged
  _request.IdRequestType = UxRequestTypeChooser1.CurrentRequestTypeInfo.Id
  SetupTypeDepend()
  Me.LookUpEditRelation.Focus()
 End Sub
#End Region

#Region " Links "

#Region " CustomerVehicleLinks "

 Private Sub LookUpEditRelation_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LookUpEditRelation.EditValueChanged

  Me.LookUpEditRelation.Properties.Buttons(2).Enabled = (Me.LookUpEditRelation.EditValue > 0)
 End Sub

 Private Sub LookUpEditRelation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LookUpEditRelation.KeyDown
  Select Case e.KeyData
   Case Keys.F1
    LookUpEditRelation_ButtonPressed(Me.LookUpEditRelation, _
    New DevExpress.XtraEditors.Controls.ButtonPressedEventArgs( _
    Me.LookUpEditRelation.Properties.Buttons.Item(4)))
  End Select
 End Sub

 Private Sub LookUpEditRelation_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditRelation.ButtonPressed
  Select Case e.Button.Index
   Case 2
    'izmeni vehicle
    Dim par As MainForm = Me.ParentForm
    Using cekaj As New StatusBusy(My.Resources.txtLoading)
     Try
      Dim vehicleID As Integer = _
      Me.LookUpEditRelation.GetColumnValue("IdVehicle")
      _vehicle = Vehicle.GetVehicle(vehicleID)
      par.AddWinPart(New uxVehicle(_vehicle))
     Catch ex As Exception
      MsgBox(ex.Message)
     End Try
    End Using

   Case 1
    If DockManager1.Panels.Count <= 1 Then

     'stavi za dodavanje na nova relacija
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
   _customerVehicleRelationList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(CType(sender, uxAddNewRelation).SelectedRelationId)
   Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehicleRelationList
   Me.CustomerVehiclesRelationsSearchListBindingSource.RaiseListChangedEvents = True
   Me.CustomerVehiclesRelationsSearchListBindingSource.ResetBindings(False)
   _request.IdCustomerVehicleRelation = CType(sender, uxAddNewRelation).SelectedRelationId
   LookUpEditRelation.EditValue = CType(sender, uxAddNewRelation).SelectedRelationId
  End If
  Me.DockManager1.RemovePanel(Me.DockManager1.Panels("addRelationPanel"))
 End Sub

#End Region

#Region " New owner "

 Private Sub LookUpEditCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LookUpEditCustomer.EditValueChanged
  Me.LookUpEditCustomer.Properties.Buttons(3).Enabled = (Me.LookUpEditCustomer.EditValue > 0)
 End Sub

 Private Sub LookUpEditCustomer_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditCustomer.ButtonPressed
  Select Case e.Button.Index
   Case 1
    Dim par As MainForm = Me.ParentForm
    Using cekaj As New StatusBusy(My.Resources.txtLoading)
     Try
      par.AddWinPart(New uxCustomers(Customer.NewCustomer))
     Catch ex As Exception
      MsgBox(ex.Message)
     End Try
    End Using
   Case 2
    Dim dij As New dijCustomersList()
    If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
     _request.IdCustomerVehicleRelationNew = dij.Customer.Id
    End If
   Case 3
    Dim par As MainForm = Me.ParentForm
    Using cekaj As New StatusBusy(My.Resources.txtLoading)
     Try
      par.AddWinPart(New uxCustomers(Customer.GetCustomer(Me.LookUpEditCustomer.EditValue)))
     Catch ex As Exception
      MsgBox(ex.Message)
     End Try
    End Using
  End Select
 End Sub

 Private Sub LookUpEditCustomer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LookUpEditCustomer.KeyDown
  Select Case e.KeyData
   Case Keys.F1
    LookUpEditCustomer_ButtonPressed(Me.LookUpEditCustomer, _
    New DevExpress.XtraEditors.Controls.ButtonPressedEventArgs( _
    Me.LookUpEditCustomer.Properties.Buttons.Item(2)))
  End Select
 End Sub

#End Region

#Region " Previous Registration "
 Private Sub PreviousRegistrationCustomLookUpEdit_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles PreviousRegistrationCustomLookUpEdit.ButtonPressed

  'If e.Button.Index = 1 Then
  '  'dijAddNewRegistration
  '  If _request.IdCustomerVehicleRelation > 0 Then
  '    Dim vehicleId As Long = _customerVehicleRelationList.GetInfoRelationById(_request.IdCustomerVehicleRelation).IdVehicle
  '    Dim dij As New dijAddNewRegistration(Vehicle.GetVehicle(vehicleId))
  '    If dij.ShowDialog = DialogResult.OK Then
  '      'vcitaj nanovo registrationList
  '      LoadDependantList()
  '      _request.IdPreviousRegistration = dij.SelectedRegistration.Id
  '    End If
  '  Else
  '    MsgBox("Одберете возило")
  '  End If
  'End If
 End Sub

 Private Sub PreviousRegistrationCustomLookUpEdit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles PreviousRegistrationCustomLookUpEdit.KeyDown
  Select Case e.KeyData
   Case Keys.F1
    PreviousRegistrationCustomLookUpEdit_ButtonPressed(Me.PreviousRegistrationCustomLookUpEdit, _
    New DevExpress.XtraEditors.Controls.ButtonPressedEventArgs( _
    Me.PreviousRegistrationCustomLookUpEdit.Properties.Buttons.Item(1)))
  End Select
 End Sub
#End Region

#End Region

#Region " Buttons "

 Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
  Dim rpt As DevExpress.XtraReports.UI.XtraReport
  Select Case Me.UxRequestTypeChooser1.CurrentRequestTypeInfo.IdDocumentPrint
   Case 1
    Dim print As PrintZelenList = PrintZelenList.GetPrintZelenList(_request.Id)
    rpt = New printZelen(print)
   Case 2
    Dim print As PrintPlavList = PrintPlavList.GetPrintPlavList(_request.Id)
    rpt = New PrintPlav(print)
   Case Else
    Dim print As printBelList = printBelList.GetprintBelList(_request.Id)
    rpt = New printBel(print)

  End Select


  Dim ux As New uxPrint(rpt)
  'ux.Show()
  CType(Me.ParentForm, MainForm).AddWinPart(ux)
 End Sub

 Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

    ''If _request.IsNew Then
    ''  Dim proveriBaranje As ActiveDocumentsDepInfo = ActiveDocumentList.GetActiveDocumentList(True, False). _
    ''  GetActivListByRelationAndType(_request.IdCustomerVehicleRelation, _request.IdRequestType)
    ''  If proveriBaranje IsNot Nothing Then
    ''    _request = request.GetRequest(proveriBaranje.Id)
    ''  End If
    ''End If
  Using busy As New Splash("Запамтувам...")
   RebindUI(True, True)
  End Using
  'If objOpcii.ApproveRequestAutomate Then

  '    ProveriOdobriBaranje()
  'End If
 End Sub

 Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
  RebindUI(False, True)
 End Sub

 Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
  RebindUI(False, False)
  Me.Close()
 End Sub

#End Region


 Private Sub Grid_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
   GridControl1.GotFocus, GridControl2.GotFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
 End Sub

 'Private Sub Grid_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
 '  GridControl1.LostFocus, GridControl2.LostFocus

 'End Sub

 '#Region " Scan "

 '    Private Sub AttachmentsBindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles AttachmentsBindingSource.ListChanged
 '        If _request IsNot Nothing Then
 '            Dim tmp As Boolean = (_request.Attachments.Count > 0)
 '            Me.btnDeleteAttachment.Enabled = tmp
 '            Me.btnEditAttachment.Enabled = tmp
 '        End If
 '    End Sub

 '    Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
 '        Dim att As RequestAttachment = _request.Attachments.AddNew

 '        Dim dijT As New dijTwain(att)

 '        If dijT.ShowDialog(Me) = DialogResult.OK Then
 '        Else
 '            'cancel edit na att
 '            _request.Attachments.Remove(att)
 '        End If

 '    End Sub

 '    Private Sub btnDeleteAttachment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteAttachment.Click
 '        Me.AttachmentsBindingSource.RemoveCurrent()

 '    End Sub

 '    Private Sub btnEditAttachment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditAttachment.Click
 '        Try
 '            Dim att As RequestAttachment
 '            Using bus As New Splash(My.Resources.LoadingData)
 '                att = _request.Attachments(Me.AttachmentsBindingSource.Position)
 '            End Using


 '            Dim dijT As New dijTwain(att)
 '            dijT.ShowDialog()
 '        Catch ex As Exception

 '        End Try
 '    End Sub

 '#End Region

 Private Sub LookUpEditCustomer_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LookUpEditCustomer.KeyUp
  If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
 e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
AndAlso e.KeyData <> Keys.Tab Then
   pom = LookUpEditCustomer.Text
  Else
   Exit Sub
  End If
  If pom <> String.Empty AndAlso pom.Length >= 7 Then
   Dim par As MainForm = Me.ParentForm
   'Dim pomTekst As String = LookUpEditCustomer.Text
   Using cekaj As New StatusBusy(My.Resources.txtLoading)

    Try
     _customerList = CustomersSearchList.GetCustomersListShortByString(pom) '(LookUpEditCustomer.Text)
    Catch ex As Exception
     _customerList = Nothing
    End Try

    Me.CustomersSearchListBindingSource.DataSource = _customerList

    If _customerList.Count > 0 Then
     'If _customerList.Count = 1 Then

     LookUpEditCustomer.ClosePopup()
     LookUpEditCustomer.ShowPopup()
     LookUpEditCustomer.Text = pom
     '    ' e.Handled = True

     'End If
    Else
     If MsgBox(My.Resources.nePostoi, MsgBoxStyle.YesNo, My.Resources.Zacuvaj & "?") = MsgBoxResult.Yes Then
      Dim novo As Customer = Customer.NewCustomer
      novo.Mb = pom
      par.AddWinPart(New uxCustomers(novo))
     Else
      pom = ""
      LookUpEditCustomer.Text = ""
     End If
    End If

   End Using
  End If
 End Sub

 Private Sub LookUpEditRelation_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LookUpEditRelation.KeyUp

  If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
  e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
  AndAlso e.KeyData <> Keys.Tab Then
   pomRelacija = LookUpEditRelation.Text
  Else
   Exit Sub
  End If
  If pomRelacija <> String.Empty AndAlso pomRelacija.Length >= 4 Then
   Dim par As MainForm = Me.ParentForm
   'Dim pomTekst As String = LookUpEditCustomer.Text
   Using cekaj As New StatusBusy(My.Resources.txtLoading)

    Try
     _customerVehicleRelationList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListByString(pomRelacija, True)
    Catch ex As Exception
     _customerVehicleRelationList = Nothing
    End Try

    Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehicleRelationList
    If _customerVehicleRelationList.Count > 0 Then

     LookUpEditRelation.ClosePopup()
     LookUpEditRelation.ShowPopup()
     LookUpEditRelation.Text = pomRelacija

    Else
     If DockManager1.Panels.Count <= 1 Then
    
      Dim panel As DevExpress.XtraBars.Docking.DockPanel = Me.DockManager1.AddPanel(DevExpress.XtraBars.Docking.DockingStyle.Bottom)
      panel.Name = "addRelationPanel"
      panel.Text = "Додавње на нова релација"
      Dim ux As New uxAddNewRelation() '("", pomRelacija)
      AddHandler ux.Disposed, AddressOf ux_dispose
      panel.Size = New Size(600, 150)
      panel.Controls.Add(ux)
      ux.Dock = DockStyle.Fill
      ux.BringToFront()
      pomRelacija = ""
      LookUpEditRelation.Text = pomRelacija
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

 Private Sub LookUpEditCustomer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditCustomer.GotFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo(uSettings.Culture))
  LookUpEditCustomer.Text = pom
 End Sub

 Private Sub CustomLookUpEditRelationCustomer_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles CustomLookUpEditRelationCustomer.ButtonPressed
  Select Case e.Button.Index

   Case 2
    'izmeni customer
    Dim par As MainForm = Me.ParentForm
    Using cekaj As New StatusBusy(My.Resources.txtLoading)
     Try
      Dim customerId As Integer = _
      Me.CustomLookUpEditRelationCustomer.GetColumnValue("IdCustomer")
      _customer = Customer.GetCustomer(customerId)
      par.AddWinPart(New uxCustomers(_customer))
     Catch ex As Exception
      MsgBox(ex.Message)
     End Try
    End Using

   Case 1
    If DockManager1.Panels.Count <= 1 Then

     'stavi za dodavanje na nova relacija
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

 Private Sub CustomLookUpEditRelationCustomer_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomLookUpEditRelationCustomer.EditValueChanged
  Me.CustomLookUpEditRelationCustomer.Properties.Buttons(2).Enabled = (Me.CustomLookUpEditRelationCustomer.EditValue > 0)
 End Sub

 Private Sub CustomLookUpEditRelationCustomer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomLookUpEditRelationCustomer.GotFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo(uSettings.Culture))
 End Sub

 Private Sub CustomLookUpEditRelationCustomer_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CustomLookUpEditRelationCustomer.KeyUp

  If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
  e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
  AndAlso e.KeyData <> Keys.Tab Then
   pomRelacijaCustomer = CustomLookUpEditRelationCustomer.Text
  Else
   Exit Sub
  End If
  If pomRelacijaCustomer <> String.Empty AndAlso pomRelacijaCustomer.Length >= 7 Then
   Dim par As MainForm = Me.ParentForm
   'Dim pomTekst As String = LookUpEditCustomer.Text
   Using cekaj As New StatusBusy(My.Resources.txtLoading)

    Try
     _customerVehicleRelationList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListByString(pomRelacijaCustomer, False)
    Catch ex As Exception
     _customerVehicleRelationList = Nothing
    End Try

    Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehicleRelationList
    If _customerVehicleRelationList.Count > 0 Then

     CustomLookUpEditRelationCustomer.ClosePopup()
     CustomLookUpEditRelationCustomer.ShowPopup()
     CustomLookUpEditRelationCustomer.Text = pomRelacijaCustomer

    Else
     If DockManager1.Panels.Count <= 1 Then
      pomRelacijaCustomer = ""
      LookUpEditRelation.Text = ""
      Dim panel As DevExpress.XtraBars.Docking.DockPanel = Me.DockManager1.AddPanel(DevExpress.XtraBars.Docking.DockingStyle.Bottom)
      panel.Name = "addRelationPanel"
      panel.Text = "Додавње на нова релација"
      Dim ux As New uxAddNewRelation() 'pomRelacijaCustomer, "")
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

 Private Sub LookUpEditRelation_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditRelation.GotFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
 End Sub
End Class
