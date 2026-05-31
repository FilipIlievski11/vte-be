Public Class uxDocumentPermisionRequest

  Private WithEvents _permision As DocumentsPermision
 Private WithEvents _customerList As CustomersSearchList
 Private WithEvents _customerVehicleRelationList As CustomerVehiclesRelationsSearchList
    ' Private WithEvents _relaciiList As CustomerVehiclesRelationsList
  Private WithEvents _issuerList As RegistrationIssuerList
    Private WithEvents _cityList As CityList
    Private pomRelacijaCustomer As String = ""
    Private pomRelacijaVehicle As String = ""
    Private pomCustomer As String = ""

  Public Sub New(ByVal docPermision As DocumentsPermision)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    ' Add any initialization after the InitializeComponent() call.
        _permision = docPermision

        LoadList()
        If Not docPermision.IsNew Then
            LookUpEditCustomer.EditValue = _customerList.Item(0).Id  ' _relaciiList.GetInfoRelationById(_permision.IdCustomerVehicleRelation).IdCustomer
            LookUpEditRelationVehicle.Enabled = False
            LookUpEditCustomer.Enabled = False
            LookUpEditRelationCustomer.Enabled = False
        Else
            LookUpEditRelationVehicle.Enabled = True
            LookUpEditCustomer.Enabled = True
            LookUpEditRelationCustomer.Enabled = True
        End If
        BindUI()
        ApplyAuthorizationRules()
    End Sub

    Private Sub ApplyAuthorizationRules()

    End Sub

    Private Sub uxDocumentPermisionRequest_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    ValidTillDateDateEdit.Focus()
  End Sub


#Region " KeyPress "

    Private Sub uxVehicle_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
        Select Case Asc(e.KeyChar)
            Case 13
                SendKeys.Send("{TAB}")

        End Select
    End Sub
#End Region

#Region " WinPart "

    Protected Overrides Function GetIdValue() As Object
        Return My.Resources.uxDocumentPermisionRequest
    End Function

    Public Overrides Function ToString() As String
        Return My.Resources.uxDocumentPermisionRequest
    End Function

#End Region

#Region " Bindings "
    Private Sub _permision_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _permision.PropertyChanged
        Select Case e.PropertyName
            Case "IdCustomerVehicleRelationOwner"
    Dim idVehicle As Integer = _
      _customerVehicleRelationList.Item(0).IdVehicle
                _permision.TrafficLicenceNumber = Vehicle.GetTrafficLicenceNumber(idVehicle)
        End Select

    End Sub

    Public Sub LoadList()
        If _permision.IsNew Then
            _customerList = Nothing 'CustomersList.GetCustomersListIsCompany(False)

            _customerVehicleRelationList = Nothing 'CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsListByTypeOfRelation(1)
        Else
   Dim pomRel As CustomerVehiclesRelationsSearchInfo = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(_permision.IdCustomerVehicleRelation).Item(0)
   _customerList = CustomersSearchList.GetCustomersListShortById(pomRel.IdCustomer)
   _customerVehicleRelationList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(_permision.IdCustomerVehicleRelationOwner)
        End If

        '    _relaciiList = CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsList
  Me.CustomersSearchListBindingSource.DataSource = _customerList
  Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehicleRelationList

        _issuerList = objRegistrationIssuerList 'RegistrationIssuerList.GetRegistrationIssuerList()
        Me.RegistrationIssuerListBindingSource.DataSource = _issuerList
        _cityList = objCityList 'CityList.GetCityList
        Me.CityListBindingSource.DataSource = _cityList
    End Sub

    Private Sub BindUI()
        Try
            _permision.BeginEdit()
            Me.DocumentsPermisionBindingSource.DataSource = _permision
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
        ' stop the flow of events
        Me.DocumentsPermisionBindingSource.RaiseListChangedEvents = False
        ' commit edits in memory
        UnbindBindingSource(Me.DocumentsPermisionBindingSource, saveObject, True)
        Try
            ' save or cancel changes
            If saveObject Then
                _permision.ApplyEdit()
                Try
                    Dim postoiRelacija As Long = _
                   CustomerVehiclesRelation.ExistsVehicleZero(LookUpEditCustomer.EditValue)
                    If postoiRelacija > 0 Then
                        _permision.IdCustomerVehicleRelation = postoiRelacija
                    Else
                        Dim novaRelacija As CustomerVehiclesRelation = CustomerVehiclesRelation.NewCustomerVehiclesRelation
                        novaRelacija.IdCustomer = LookUpEditCustomer.EditValue
                        novaRelacija.IdVehicle = 0
                        novaRelacija.IdRelationType = 3
                        novaRelacija.StartDate = Now.Date
                        _permision.IdCustomerVehicleRelation = (novaRelacija.Save()).Id
                    End If
                    _permision = _permision.Save
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
                _permision.CancelEdit()
            End If
        Finally
            'rebind UI if requested
            If rebind Then
                BindUI()
            End If
            ' restore events
            Me.DocumentsPermisionBindingSource.RaiseListChangedEvents = True
            If rebind Then
                ' refresh the UI if rebinding
                Me.DocumentsPermisionBindingSource.ResetBindings(False)
            End If
        End Try
    End Sub

    Private Sub BindingSources_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
      DocumentsPermisionBindingSource.CurrentItemChanged

        Me.btnSave.Enabled = _permision.IsSavable
        Me.btnNew.Enabled = _permision.IsValid
        Me.btnPrint.Enabled = Not _permision.IsDirty
        Me.btnPrintPermission.Enabled = (Not _permision.IsDirty) And (Not _permision.IsNew)
        Me.btnBill.Enabled = Not _permision.IsDirty
        Dim message As New System.Text.StringBuilder
        message.AppendFormat("{0}" + vbCrLf, "")

        For Each rule As Csla.Validation.BrokenRule In _permision.BrokenRulesCollection
            message.AppendFormat( _
              "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)

        Next
        'close all alerts
        'If message.ToString <> vbCrLf Then
        '  Dim info As New DevExpress.XtraBars.Alerter.AlertInfo("Прекршени правила", message.ToString)

        '  Me.ValidationAlertControl.Show(Me.ParentForm, info)

        'End If

        ShowBrokenRules(message.ToString, True)


    End Sub

    'Private Sub ValidationAlertControl_AlertClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.Alerter.AlertClickEventArgs) Handles ValidationAlertControl.AlertClick
    '  Dim field As String = Mid(e.AlertForm.AlertInfo.Text, e.AlertForm.AlertInfo.Text.IndexOf("*") + 2, e.AlertForm.AlertInfo.Text.IndexOf(":") - 3).Trim
    '  FocusBindingField(field, Me)
    'End Sub


    Private Sub FocusBindingField(ByVal propertyName As String, ByVal paren As Control)

        'pomini gi site kontroli vo formata i disable ako gi ima vo listata
        For Each ctr As Control In paren.Controls
            'textBox
            If TypeOf ctr Is DevExpress.XtraEditors.TextEdit Then
                For Each binding As Binding In ctr.DataBindings
                    ' get the BindingSource if appropriate
                    If TypeOf binding.DataSource Is BindingSource Then
                        If propertyName = binding.BindingMemberInfo.BindingField Then
                            ctr.Focus()
                            Exit Sub
                        End If
                    End If
                Next
            End If
            'otidi vo rekurzija ako ima deca
            If ctr.HasChildren Then
                FocusBindingField(propertyName, ctr)
            End If
        Next


    End Sub

#End Region

#Region " Links "

#Region "  Customer "
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
                    LookUpEditCustomer.EditValue = dij.Customer.Id
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

#Region "  Owner "
    Private Sub LookUpEditRelation_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LookUpEditRelationVehicle.EditValueChanged
        Me.LookUpEditRelationVehicle.Properties.Buttons(3).Enabled = (Me.LookUpEditRelationVehicle.EditValue > 0)
        Me.LookUpEditRelationVehicle.Properties.Buttons(2).Enabled = (Me.LookUpEditRelationVehicle.EditValue > 0)
    End Sub
    Private Sub LookUpEditRelationCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LookUpEditRelationCustomer.EditValueChanged
        Me.LookUpEditRelationCustomer.Properties.Buttons(3).Enabled = (Me.LookUpEditRelationCustomer.EditValue > 0)
        Me.LookUpEditRelationCustomer.Properties.Buttons(2).Enabled = (Me.LookUpEditRelationCustomer.EditValue > 0)
    End Sub
    'Private Sub LookUpEditRelation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LookUpEditRelationVehicle.KeyDown
    '    Select Case e.KeyData
    '        Case Keys.F1
    '            LookUpEditRelation_ButtonPressed(Me.LookUpEditRelationVehicle, _
    '            New DevExpress.XtraEditors.Controls.ButtonPressedEventArgs( _
    '            Me.LookUpEditRelationVehicle.Properties.Buttons.Item(4)))
    '    End Select
    'End Sub

    Private Sub LookUpEditRelation_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditRelationVehicle.ButtonPressed, LookUpEditRelationCustomer.ButtonPressed

        Select Case e.Button.Index
            Case 3
                Dim par As MainForm = Me.ParentForm
                Using cekaj As New StatusBusy(My.Resources.txtLoading)
                    Try
                        Dim vehicleID As Integer = _
                        Me.LookUpEditRelationVehicle.GetColumnValue("IdVehicle")
                        par.AddWinPart(New uxVehicle(Vehicle.GetVehicle(vehicleID)))
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                End Using
            Case 2
                Dim par As MainForm = Me.ParentForm
                Using cekaj As New StatusBusy(My.Resources.txtLoading)
                    Try
                        Dim customerId As Integer = _
                        Me.LookUpEditRelationVehicle.GetColumnValue("IdCustomer")
                        par.AddWinPart(New uxCustomers(Customer.GetCustomer(customerId)))
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                End Using
            Case 4
                Dim dij As New dijVehicleCustomerList()
                If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    LookUpEditRelationVehicle.EditValue = dij.SelectedCustomerVehicles.Id
                End If
            Case 1
                If DockManager1.Panels.Count <= 0 Then

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
   _permision.IdCustomerVehicleRelationOwner = CType(sender, uxAddNewRelation).SelectedRelationId
   LookUpEditRelationVehicle.EditValue = CType(sender, uxAddNewRelation).SelectedRelationId
  End If
  Me.DockManager1.RemovePanel(Me.DockManager1.Panels("addRelationPanel"))
 End Sub

#End Region

#End Region

#Region "Buttons"

 Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
  RebindUI(True, True)
 End Sub

 Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
  Me.Close()
 End Sub

 Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
  'tuka puka
  Dim rptPermission As rptBaranjeZaOdobrenieZaTugoVozilo = New rptBaranjeZaOdobrenieZaTugoVozilo(_permision.Id)
  Dim ux As New uxPrint(rptPermission)
  CType(Me.ParentForm, MainForm).AddWinPart(ux)
  ' rptPermission.ShowPreviewDialog()
  '  rptPermission.Print()
 End Sub
 Private Sub btnPrintPermission_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintPermission.Click
  Dim rptPermissionForV As rptOdobrenieZaTugoV = New rptOdobrenieZaTugoV(_permision.Id)
  Dim ux As New uxPrint(rptPermissionForV)
  CType(Me.ParentForm, MainForm).AddWinPart(ux)
  ' rptPermissionForV.ShowPreviewDialog()
 End Sub

 Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
  RebindUI(False, True)
 End Sub

 Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
  Try
   RebindUI(True, True)
   'tuka otvori pecati
  Catch ex As Exception
   Exit Sub
  End Try
  _permision = Nothing
  _permision = DocumentsPermision.NewDocumentsPermision
  Me.DocumentsPermisionBindingSource.DataSource = _permision
  Me.ValidTillDateDateEdit.Focus()
 End Sub


 Private Sub btnBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBill.Click
  Try
   Using busy As New Splash("Вчитувам")
    Dim dok As PaymentDocument = PaymentDocument.NewPaymentDocument
    dok.IdCustomerVehicleRelation = _permision.IdCustomerVehicleRelationOwner
    Dim par As MainForm = Me.ParentForm
    par.AddWinPart(New uxPaymentDocument(dok))
   End Using
  Catch ex As Exception
   MsgBox(ex.Message)
  End Try
 End Sub

#End Region

 Private Sub LookUpEditCustomer_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LookUpEditCustomer.KeyUp
  If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
AndAlso e.KeyData <> Keys.Tab Then
   pomCustomer = LookUpEditCustomer.Text
  Else
   Exit Sub
  End If
  If pomCustomer <> String.Empty AndAlso pomCustomer.Length >= 7 Then
   Dim par As MainForm = Me.ParentForm
   'Dim pomTekst As String = LookUpEditCustomer.Text
   Using cekaj As New StatusBusy(My.Resources.txtLoading)

    Try
     _customerList = CustomersSearchList.GetCustomersListShortByString(pomCustomer) '(LookUpEditCustomer.Text)
    Catch ex As Exception
     _customerList = Nothing
    End Try

    Me.CustomersSearchListBindingSource.DataSource = _customerList

    If _customerList.Count > 0 Then
     'If _customerList.Count = 1 Then

     LookUpEditCustomer.ClosePopup()
     LookUpEditCustomer.ShowPopup()
     LookUpEditCustomer.Text = pomCustomer
     '    ' e.Handled = True

     'End If
    Else
     If MsgBox(My.Resources.nePostoi, MsgBoxStyle.YesNo, My.Resources.Zacuvaj & "?") = MsgBoxResult.Yes Then
      Dim novo As Customer = Customer.NewCustomer
      novo.Mb = pomCustomer
      par.AddWinPart(New uxCustomers(novo))
     Else
      pomCustomer = ""
      LookUpEditCustomer.Text = ""
     End If
    End If

   End Using
  End If
 End Sub

 Private Sub LookUpEditRelation_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LookUpEditRelationVehicle.KeyUp

  If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
  e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
  AndAlso e.KeyData <> Keys.Tab Then
   pomRelacijaVehicle = LookUpEditRelationVehicle.Text
  Else
   Exit Sub
  End If
  If pomRelacijaVehicle <> String.Empty AndAlso pomRelacijaVehicle.Length >= 4 Then
   Dim par As MainForm = Me.ParentForm
   'Dim pomTekst As String = LookUpEditCustomer.Text
   Using cekaj As New StatusBusy(My.Resources.txtLoading)

    Try
     _customerVehicleRelationList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListByString(pomRelacijaVehicle, True)
    Catch ex As Exception
     _customerVehicleRelationList = Nothing
    End Try

    Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehicleRelationList
    If _customerVehicleRelationList.Count > 0 Then

     LookUpEditRelationVehicle.ClosePopup()
     LookUpEditRelationVehicle.ShowPopup()
     LookUpEditRelationVehicle.Text = pomRelacijaVehicle

    Else
     If DockManager1.Panels.Count <= 0 Then

      pomRelacijaVehicle = ""
      LookUpEditRelationVehicle.Text = pomRelacijaVehicle
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

 Private Sub LookUpEditCustomer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditCustomer.GotFocus
  LookUpEditCustomer.Text = pomCustomer
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo(uSettings.Culture))
 End Sub

 Private Sub LookUpEditRelationCustomer_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LookUpEditRelationCustomer.KeyUp
  If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
 e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
 AndAlso e.KeyData <> Keys.Tab Then
   pomRelacijaCustomer = LookUpEditRelationCustomer.Text
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

     LookUpEditRelationCustomer.ClosePopup()
     LookUpEditRelationCustomer.ShowPopup()
     LookUpEditRelationCustomer.Text = pomRelacijaCustomer

    Else
     If DockManager1.Panels.Count <= 0 Then

      pomRelacijaVehicle = ""
      LookUpEditRelationCustomer.Text = pomRelacijaCustomer
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

    Private Sub LookUpEditRelationVehicle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditRelationVehicle.GotFocus
        System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    End Sub

    Private Sub LookUpEditRelationCustomer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditRelationCustomer.GotFocus
        System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo(uSettings.Culture))
  End Sub

 
  Private Sub DateStartDateEdit_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateStartDateEdit.Validated
    'ValidTillDateDateEdit.EditValue = DateStartDateEdit.DateTime.AddYears(1)
    If _permision.IsNew Then
      _permision.ValidTillDate = _permision.DateStart.AddYears(1)
    End If

  End Sub
End Class
