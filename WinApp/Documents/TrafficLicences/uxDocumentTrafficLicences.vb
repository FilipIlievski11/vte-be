Public Class uxDocumentTrafficLicences

 Private WithEvents _trafficLicence As DocumentsTrafficLicence
 Private WithEvents _customerVehicleRelation As CustomerVehiclesRelationsSearchList = Nothing
 Private WithEvents _registrationIssuerList As RegistrationIssuerList
 Private pomRelacijaCustomer As String = ""
 Private pomRelacijaVehicle As String = ""
 Public Sub New()

  ' This call is required by the Windows Form Designer.
  InitializeComponent()
  Try
   _trafficLicence = DocumentsTrafficLicence.NewDocumentsTrafficLicence
   Me.DocumentsTrafficLcenceBindingSource.DataSource = _trafficLicence
  Catch ex As Exception

  End Try
  LoadList()
  ApplyAuthorizationRules()
  ' Add any initialization after the InitializeComponent() call.

 End Sub
 Public Sub New(ByVal InTraffLicence As DocumentsTrafficLicence)

  ' This call is required by the Windows Form Designer.
  InitializeComponent()
  ' Add any initialization after the InitializeComponent() call.
  Try
   _trafficLicence = InTraffLicence
   Me.DocumentsTrafficLcenceBindingSource.DataSource = _trafficLicence
  Catch ex As Exception

  End Try
  LoadList()
  ApplyAuthorizationRules()

 End Sub

 Private Sub uxDocumentTrafficLicences_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  If _trafficLicence.IsNew Then
   _trafficLicence.IdTehnicalExamOrganizationsIssuedBy = objCurentTehExamOrganization.IdDefaultRegistrationIssuer
  End If

  RelationVehicle.Focus()

 End Sub

 Private Sub ApplyAuthorizationRules()

  ' kontroli(eanble / disable)
  Me.ReadWriteAuthorization1.ResetControlAuthorization()
  If Not VTE.Library.DocumentsTrafficLicence.CanGetObject Then
   Me.Close()
  End If
  Me.btnSave.Enabled = VTE.Library.DocumentsTrafficLicence.CanEditObject
  Me.btnNew.Enabled = VTE.Library.DocumentsTrafficLicence.CanAddObject
  Me.btnSave.Enabled = _trafficLicence.IsSavable 'AndAlso _trafficLicence.IsValid
  Me.btnPrint.Enabled = Not _trafficLicence.IsNew AndAlso _trafficLicence.IsValid 'Not _trafficLicence.IsDirty 

 End Sub

#Region " KeyPress "

 Private Sub uxDocumentTehnicalExamReports_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
  Select Case Asc(e.KeyChar)
   Case 13
    SendKeys.Send("{TAB}")
  End Select
 End Sub

#End Region

#Region " WinPart Code "

 Protected Overrides Function GetIdValue() As Object

  Return My.Resources.uxDocumentTrafficLicences

 End Function

 Public Overrides Function ToString() As String

  Return My.Resources.uxDocumentTrafficLicences

 End Function
#End Region

#Region " Bindings "
 Private Sub LoadList()
  If Not _trafficLicence.IsNew Then
   _customerVehicleRelation = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById _
            (_trafficLicence.IdCustomerVehicleRelation)
   'Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehicleRelation
  End If
  '  _customerVehicleRelation = CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsListByTypeOfRelation(1)
  Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehicleRelation

  _registrationIssuerList = RegistrationIssuerList.GetRegistrationIssuerList
  Me.RegistrationIssuerListBindingSource.DataSource = _registrationIssuerList
 End Sub


 Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
  ' stop the flow of events
  Me.DocumentsTrafficLcenceBindingSource.RaiseListChangedEvents = False
  Me.ExtensionsBindingSource.RaiseListChangedEvents = False
  ' commit edits in memory
  UnbindBindingSource(Me.DocumentsTrafficLcenceBindingSource, saveObject, True)
  UnbindBindingSource(Me.ExtensionsBindingSource, saveObject, False)
  Me.ExtensionsBindingSource.DataSource = Me.DocumentsTrafficLcenceBindingSource
  Try
   ' save or cancel changes
   If saveObject Then
    _trafficLicence.ApplyEdit()
    Try
     _trafficLicence = _trafficLicence.Save
    Catch ex As Csla.Validation.ValidationException
     MsgBox(My.Resources.ValidationError)
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
    _trafficLicence.CancelEdit()
   End If
  Finally
   'rebind UI if requested
   If rebind Then
    Me.DocumentsTrafficLcenceBindingSource.DataSource = _trafficLicence
   End If
   ' restore events
   Me.DocumentsTrafficLcenceBindingSource.RaiseListChangedEvents = True
   Me.ExtensionsBindingSource.RaiseListChangedEvents = True
   If rebind Then
    ' refresh the UI if rebinding
    Me.DocumentsTrafficLcenceBindingSource.ResetBindings(False)
    Me.ExtensionsBindingSource.ResetBindings(False)
   End If
  End Try
 End Sub

 Private Sub DocumentsTrafficLcenceBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
     DocumentsTrafficLcenceBindingSource.CurrentItemChanged, ExtensionsBindingSource.CurrentItemChanged
  Me.btnSave.Enabled = _trafficLicence.IsSavable 'AndAlso _trafficLicence.IsValid
  Me.btnPrint.Enabled = Not _trafficLicence.IsNew AndAlso _trafficLicence.IsValid 'Not _trafficLicence.IsDirty 
  Me.btnNew.Enabled = Not _trafficLicence.IsDirty
 End Sub

#End Region

#Region " Buttons "

 Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click


  Dim vehicleID As Integer = _
   Me.RelationVehicle.GetColumnValue("IdVehicle")
  Dim _vehicle As VehicleInfo
  _vehicle = VehicleList.GetVehicleById(vehicleID)
  'GetVehicleList.GetVehicleListById(vehicleID)
        'Dim dij As New dijKratkaDolga
        Dim dij As New dijKratkaDolga1234
  If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
            'Select Case dij.Dolga
            ' Case 0
            '  Dim stampaTrafficLicence As rptTrafficLicence = New rptTrafficLicence(_trafficLicence.Id)
            '  Dim ux As New uxPrint(stampaTrafficLicence)
            '  CType(Me.ParentForm, MainForm).AddWinPart(ux)
            ' Case 1
            '  Dim stampaTrafficLicence As rptTrafficLicenceZaTraktori = New rptTrafficLicenceZaTraktori(_trafficLicence.Id)
            '  Dim ux As New uxPrint(stampaTrafficLicence)
            '  CType(Me.ParentForm, MainForm).AddWinPart(ux)
            ' Case 2
            '  Dim stampaTrafficLicence As rptTrafficLicenceZaTraktoriZemjodelski = New rptTrafficLicenceZaTraktoriZemjodelski(_trafficLicence.Id)
            '  Dim ux As New uxPrint(stampaTrafficLicence)
            '  CType(Me.ParentForm, MainForm).AddWinPart(ux)
            '         End Select
            'nov select za novite soobrakajni - prethodniot da se trgne
            Select Case dij.Dolga
                Case 0
                    Dim stampaTrafficLicence As rptTrafficLicence1 = New rptTrafficLicence1(_trafficLicence.Id)
                    Dim ux As New uxPrint(stampaTrafficLicence)
                    CType(Me.ParentForm, MainForm).AddWinPart(ux)
                Case 1
                    Dim stampaTrafficLicence As rptTrafficLicence2 = New rptTrafficLicence2(_trafficLicence.Id)
                    Dim ux As New uxPrint(stampaTrafficLicence)
                    CType(Me.ParentForm, MainForm).AddWinPart(ux)
                Case 2
                    Dim stampaTrafficLicence As rptTrafficLicence3 = New rptTrafficLicence3(_trafficLicence.Id)
                    Dim ux As New uxPrint(stampaTrafficLicence)
                    CType(Me.ParentForm, MainForm).AddWinPart(ux)
                Case 3
                    Dim stampaTrafficLicence As rptTrafficLicence4 = New rptTrafficLicence4(_trafficLicence.Id)
                    Dim ux As New uxPrint(stampaTrafficLicence)
                    CType(Me.ParentForm, MainForm).AddWinPart(ux)
            End Select
  End If

 End Sub

 Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
  RebindUI(False, True)
 End Sub

 Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
  RebindUI(True, True)
 End Sub

 Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
  If _trafficLicence.IsDirty Then
   Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
    Case MsgBoxResult.Yes
     If Vehicle.CanEditObject Then
      RebindUI(True, False)
      Me.Close()
     Else
      Me.Close()
     End If

    Case MsgBoxResult.No
     RebindUI(False, False)
     Me.Close()
    Case MsgBoxResult.Cancel
     Exit Sub
   End Select
  Else
   Me.Close()
  End If
 End Sub

 Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
  Try
   RebindUI(True, True)
   'tuka otvori pecati
  Catch ex As Exception
   Exit Sub
  End Try
  _trafficLicence = Nothing
  _trafficLicence = DocumentsTrafficLicence.NewDocumentsTrafficLicence
  Me.DocumentsTrafficLcenceBindingSource.DataSource = _trafficLicence
  Me.RelationVehicle.Focus()
 End Sub

#End Region

#Region " Links "
 Private Sub RelationVehicle_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelationVehicle.EditValueChanged
  Me.RelationVehicle.Properties.Buttons(3).Enabled = (Me.RelationVehicle.EditValue > 0)
  Me.RelationVehicle.Properties.Buttons(2).Enabled = (Me.RelationVehicle.EditValue > 0)

 End Sub
 Private Sub RelationCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelationCustomer.EditValueChanged
  Me.RelationCustomer.Properties.Buttons(3).Enabled = (Me.RelationCustomer.EditValue > 0)
  Me.RelationCustomer.Properties.Buttons(2).Enabled = (Me.RelationCustomer.EditValue > 0)
 End Sub



 Private Sub ux_dispose(ByVal sender As Object, ByVal e As System.EventArgs)
  If CType(sender, uxAddNewRelation).SelectedRelationId <> 0 Then
   Me.CustomerVehiclesRelationsSearchListBindingSource.RaiseListChangedEvents = False
   _customerVehicleRelation = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(CType(sender, uxAddNewRelation).SelectedRelationId)
   Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehicleRelation
   Me.CustomerVehiclesRelationsSearchListBindingSource.RaiseListChangedEvents = True
   Me.CustomerVehiclesRelationsSearchListBindingSource.ResetBindings(False)
   _trafficLicence.IdCustomerVehicleRelation = CType(sender, uxAddNewRelation).SelectedRelationId
   RelationVehicle.EditValue = CType(sender, uxAddNewRelation).SelectedRelationId
  End If
  Me.DockManager1.RemovePanel(Me.DockManager1.Panels("addRelationPanel"))
 End Sub



#End Region

#Region " Language change "
 Private Sub TrafficLicenceNumberTextEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) _
Handles TrafficLicenceNumberTextEdit.GotFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
 End Sub

 Private Sub TrafficLicenceNumberTextEdit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) _
 Handles TrafficLicenceNumberTextEdit.LostFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
 End Sub
#End Region


 Private Sub RepositoryItemButtonEdit1_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles RepositoryItemButtonEdit1.ButtonPressed
  Select Case e.Button.Index
   Case 0
    Dim uxStam As New rptTrafficLicenceExtension( _
    _trafficLicence.Extensions(Me.ExtensionsBindingSource.Position).ValidTill, _
    Me.ExtensionsBindingSource.Position, _
    _trafficLicence.Extensions(Me.ExtensionsBindingSource.Position).Note)
    uxStam.ShowPreviewDialog()

  End Select
 End Sub


 Private Sub LookUpEditRelationCustomer_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles RelationCustomer.ButtonPressed
  Select Case e.Button.Index
   Case 3
    Dim par As MainForm = Me.ParentForm
    Using cekaj As New StatusBusy(My.Resources.txtLoading)
     Try
      Dim vehicleID As Integer = _
      Me.RelationCustomer.GetColumnValue("IdVehicle")
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
      Me.RelationCustomer.GetColumnValue("IdCustomer")
      par.AddWinPart(New uxCustomers(Customer.GetCustomer(customerId)))
     Catch ex As Exception
      MsgBox(ex.Message)
     End Try
    End Using
    'Case 4
    '    Dim dij As New dijVehicleCustomerList
    '    If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '        _trafficLicence.IdCustomerVehicleRelation = dij.SelectedCustomerVehicles.Id
    '    End If
   Case 1
    If DockManager1.Panels.Count <= 0 Then
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

 Private Sub LookUpEditRelationCustomer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles RelationCustomer.GotFocus
  System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo(uSettings.Culture))
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo(uSettings.Culture))
  ' RelationCustomer.Text = pomRelacijaCustomer
 End Sub

 Private Sub LookUpEditRelationCustomer_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RelationCustomer.KeyUp
  If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
AndAlso e.KeyData <> Keys.Tab Then
   pomRelacijaCustomer = RelationCustomer.Text
  Else
   Exit Sub
  End If
  If pomRelacijaCustomer <> String.Empty AndAlso pomRelacijaCustomer.Length >= 7 Then
   Dim par As MainForm = Me.ParentForm
   'Dim pomTekst As String = LookUpEditCustomer.Text
   Using cekaj As New StatusBusy(My.Resources.txtLoading)

    Try
     _customerVehicleRelation = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListByString(pomRelacijaCustomer, False)
    Catch ex As Exception
     _customerVehicleRelation = Nothing
    End Try

    Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehicleRelation
    If _customerVehicleRelation.Count > 0 Then

     RelationCustomer.ClosePopup()
     RelationCustomer.ShowPopup()
     RelationCustomer.Text = pomRelacijaCustomer

    Else
     If DockManager1.Panels.Count <= 0 Then
      pomRelacijaCustomer = ""
      RelationCustomer.Text = pomRelacijaCustomer
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

 Private Sub RelationVehicle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles RelationVehicle.GotFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
  'RelationVehicle.Text = pomRelacijaVehicle
 End Sub

 Private Sub RelationVehicle_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RelationVehicle.KeyUp
  If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
AndAlso e.KeyData <> Keys.Tab Then
   pomRelacijaVehicle = RelationVehicle.Text
  Else
   Exit Sub
  End If
  If pomRelacijaVehicle <> String.Empty AndAlso pomRelacijaVehicle.Length >= 4 Then
   Dim par As MainForm = Me.ParentForm
   'Dim pomTekst As String = LookUpEditCustomer.Text
   Using cekaj As New StatusBusy(My.Resources.txtLoading)

    Try
     _customerVehicleRelation = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListByString(pomRelacijaVehicle, True)
    Catch ex As Exception
     _customerVehicleRelation = Nothing
    End Try

    Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehicleRelation
    If _customerVehicleRelation.Count > 0 Then

     RelationVehicle.ClosePopup()
     RelationVehicle.ShowPopup()
     RelationVehicle.Text = pomRelacijaVehicle

    Else
     If DockManager1.Panels.Count <= 0 Then
      pomRelacijaVehicle = ""
      RelationCustomer.Text = pomRelacijaCustomer
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

 Private Sub RelationVehicle_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles RelationVehicle.ButtonPressed
  Select Case e.Button.Index
   Case 3
    Dim par As MainForm = Me.ParentForm
    Using cekaj As New StatusBusy(My.Resources.txtLoading)
     Try
      Dim vehicleID As Integer = _
      Me.RelationVehicle.GetColumnValue("IdVehicle")
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
      Me.RelationVehicle.GetColumnValue("IdCustomer")
      par.AddWinPart(New uxCustomers(Customer.GetCustomer(customerId)))
     Catch ex As Exception
      MsgBox(ex.Message)
     End Try
    End Using
    'Case 4
    '    Dim dij As New dijVehicleCustomerList
    '    If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '        _trafficLicence.IdCustomerVehicleRelation = dij.SelectedCustomerVehicles.Id
    '    End If
   Case 1
    If DockManager1.Panels.Count <= 0 Then
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
End Class
