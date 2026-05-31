Public Class uxInternationalDriveingLicences
  Private WithEvents _documentInternationalLicence As DocumentsInternationalDriveingLicence
 Private WithEvents _customerList As CustomersSearchList
    Private WithEvents _driveingLicenceCategories As DriveingLicenceCtegoryList
    Private WithEvents _issuersList As TehnicalExamOrganizationsList
    Private pomCustomer As String = ""

  Public Sub New(ByVal inDocInternationalLicence As DocumentsInternationalDriveingLicence)
    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    ' Add any initialization after the InitializeComponent() call.
    _documentInternationalLicence = inDocInternationalLicence
    LoadList()
    BindUI()
  End Sub


  Private Sub uxInternationalDriveingLicences_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Me.btnSave.Enabled = _documentInternationalLicence.IsSavable
    Me.btnPrint.Enabled = Not _documentInternationalLicence.IsNew AndAlso _documentInternationalLicence.IsValid

    LookUpEditCustomer.Focus()
  End Sub

#Region " KeyPress "
  Private Sub uxCities_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub
#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxInternationalDriveingLicences

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxInternationalDriveingLicences

  End Function
#End Region

#Region " Bindings "

  Private Sub LoadList()
        _issuersList = objTehExamOrganizations  'TehnicalExamOrganizationsList.GetTehnicalExamOrganizationsList
        Me.TehnicalExamOrganizationsListBindingSource.DataSource = _issuersList

        _driveingLicenceCategories = objDriveingLicenceCtegoryList 'DriveingLicenceCtegoryList.GetDriveingLicenceCtegoryList
        Me.DriveingLicenceCtegoryListBindingSource.DataSource = _driveingLicenceCategories
        If _documentInternationalLicence.IsNew Then
            _customerList = Nothing
            LookUpEditCustomer.Enabled = True
        Else
   _customerList = CustomersSearchList.GetCustomersListShortById(_documentInternationalLicence.IdCustomer)
            LookUpEditCustomer.Enabled = False
        End If
        'CustomersList.GetCustomersListIsCompany(False)
  Me.CustomersSearchListBindingSource.DataSource = _customerList
 End Sub

 Private Sub BindUI()
  _documentInternationalLicence.BeginEdit()
  Me.DocumentsInternationalDriveingLicenceBindingSource.DataSource = _documentInternationalLicence
 End Sub

 Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
  ' stop the flow of events
  Me.DocumentsInternationalDriveingLicenceBindingSource.RaiseListChangedEvents = False
  Me.ValidForCategoriesBindingSource.RaiseListChangedEvents = False
  ' commit edits in memory
  UnbindBindingSource(Me.ValidForCategoriesBindingSource, saveObject, False)
  UnbindBindingSource(Me.DocumentsInternationalDriveingLicenceBindingSource, saveObject, True)
  Try
   ' save or cancel changes
   If saveObject Then
    _documentInternationalLicence.ApplyEdit()
    Try
     _documentInternationalLicence = _documentInternationalLicence.Save
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
    _documentInternationalLicence.CancelEdit()
   End If
  Finally
   'rebind UI if requested
   If rebind Then
    BindUI()
   End If
   ' restore events
   Me.DocumentsInternationalDriveingLicenceBindingSource.RaiseListChangedEvents = True
   Me.ValidForCategoriesBindingSource.RaiseListChangedEvents = True
   If rebind Then
    ' refresh the UI if rebinding
    Me.DocumentsInternationalDriveingLicenceBindingSource.ResetBindings(False)
    Me.ValidForCategoriesBindingSource.ResetBindings(False)
   End If
  End Try

 End Sub

 Private Sub BindingSources_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
   DocumentsInternationalDriveingLicenceBindingSource.CurrentItemChanged, _
   ValidForCategoriesBindingSource.CurrentItemChanged

  Me.btnSave.Enabled = _documentInternationalLicence.IsSavable
  Me.btnBill.Enabled = (Not _documentInternationalLicence.IsNew) And (Not _documentInternationalLicence.IsDirty)
  Me.btnPrint.Enabled = Not _documentInternationalLicence.IsDirty
  Me.btnPrintRequest.Enabled = Not _documentInternationalLicence.IsDirty
  Me.btnNew.Enabled = _documentInternationalLicence.IsValid
  Dim message As New System.Text.StringBuilder
  message.AppendFormat("{0}" + vbCrLf, "")
  For Each rule As Csla.Validation.BrokenRule In _documentInternationalLicence.BrokenRulesCollection
   message.AppendFormat( _
     "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
  Next
  ShowBrokenRules(message.ToString, True)
 End Sub

#End Region

#Region "Buttons"

 Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
  RebindUI(True, True)
 End Sub

 Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
  Dim rptInternationalLicence As rptInternationalDriveingLicence = New rptInternationalDriveingLicence(_documentInternationalLicence.Id)

  Dim ux As New uxPrint(rptInternationalLicence)
  CType(Me.ParentForm, MainForm).AddWinPart(ux)
  'rptInternationalLicence.ShowPreviewDialog()
 End Sub

 Private Sub btnPrintRequest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintRequest.Click
  Dim rptInternationalLicence As rptInternationalDrivLicenceRequest = New rptInternationalDrivLicenceRequest(_documentInternationalLicence.Id)
  rptInternationalLicence.Margins.Top = 0
  rptInternationalLicence.Margins.Left = 0
  rptInternationalLicence.Margins.Right = 0
  rptInternationalLicence.Margins.Bottom = 0

  Dim ux As New uxPrint(rptInternationalLicence)
  CType(Me.ParentForm, MainForm).AddWinPart(ux)
  'rptInternationalLicence.ShowPreviewDialog()
 End Sub
 Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
  Me.Close()
 End Sub
 Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
  Try
   RebindUI(True, True)
   'tuka otvori pecati
  Catch ex As Exception
   Exit Sub
  End Try
  _documentInternationalLicence = Nothing
  _documentInternationalLicence = DocumentsInternationalDriveingLicence.NewDocumentsInternationalDriveingLicence
  Me.DocumentsInternationalDriveingLicenceBindingSource.DataSource = _documentInternationalLicence
  Me.LookUpEditCustomer.Focus()
 End Sub

 Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
  RebindUI(False, True)
 End Sub

 Private Sub btnBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBill.Click
  Try
   Using busy As New Splash("Вчитувам")
    Dim dok As PaymentDocument = PaymentDocument.NewPaymentDocument
    dok.IdCustomerVehicleRelation = CustomerVehiclesRelation.ExistsCustomerOnly(_documentInternationalLicence.IdCustomer)
    Dim par As MainForm = Me.ParentForm
    par.AddWinPart(New uxPaymentDocument(dok))
   End Using
  Catch ex As Exception
   MsgBox(ex.Message)
  End Try
 End Sub

#End Region

#Region " Links "

 Private Sub LookUpEditCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LookUpEditCustomer.EditValueChanged
  Me.LookUpEditCustomer.Properties.Buttons(2).Enabled = (Me.LookUpEditCustomer.EditValue > 0)
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
    Dim par As MainForm = Me.ParentForm
    Using cekaj As New StatusBusy(My.Resources.txtLoading)
     Try
      par.AddWinPart(New uxCustomers(Customer.GetCustomer(Me.LookUpEditCustomer.EditValue)))
     Catch ex As Exception
      MsgBox(ex.Message)
     End Try
    End Using
   Case 3
    Dim dij As New dijCustomersList()
    If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
     LookUpEditCustomer.EditValue = dij.Customer.Id
    End If

  End Select
 End Sub

 Private Sub LookUpEditCustomer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LookUpEditCustomer.KeyDown
  Select Case e.KeyData
   Case Keys.F1
    LookUpEditCustomer_ButtonPressed(Me.LookUpEditCustomer, _
    New DevExpress.XtraEditors.Controls.ButtonPressedEventArgs( _
    Me.LookUpEditCustomer.Properties.Buttons.Item(3)))
  End Select
 End Sub

#End Region

#Region " Langage change "
 Private Sub NumberOfLicenceTextEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
   NumberOfLicenceTextEdit.GotFocus

  System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
  System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
 End Sub

 Private Sub NumberOfLicenceTextEdit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
   NumberOfLicenceTextEdit.LostFocus

  System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
  System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
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

    Private Sub LookUpEditCustomer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditCustomer.GotFocus
        System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo(uSettings.Culture))
        LookUpEditCustomer.Text = pomCustomer
    End Sub
End Class
