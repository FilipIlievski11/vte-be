Imports System.Drawing
Public Class uxCustomers

 Private WithEvents _customer As Customer
 Private WithEvents _countriesList As CountriesList

 Public ReadOnly Property Customer() As Customer
  Get
   Return _customer
  End Get
 End Property
 Public Sub New(ByVal Customer As Customer)

  ' This call is required by the Windows Form Designer.
  InitializeComponent()

  _customer = Customer
  LoadList()


  BindUI()
  CustomLookUpEditBusinesstype.Enabled = False
  ' AttachmentsTab.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden

  ' Add any initialization after the InitializeComponent() call.
  ApplyAuthorizationRules()
 End Sub


 Private Sub ApplyAuthorizationRules()

  'kontroli eanble/disable
  Me.ReadWriteAuthorization1.ResetControlAuthorization()

  If Not VTE.Library.Customers.CanGetObject Then
   Me.Close()
  End If

  Me.UxKopcinja1.cmdSave.Visible = VTE.Library.Customers.CanEditObject

  Me.UxKopcinja1.cmdAdd.Visible = VTE.Library.Customers.CanAddObject

  Me.UxKopcinja1.cmdDelete.Visible = VTE.Library.Customers.CanDeleteObject


 End Sub

 Private Sub uxCustomer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  IsCompanyCheckEdit_CheckStateChanged(sender, e)
  MbTextEdit.Focus()
  If Not _customer.IsNew AndAlso _customer.IsCompany Then
   'CustomLookUpEditBirthAddress.Enabled = False
   LookUpEditBirthCity.Enabled = False
   'BirthAddressTextEdit.Enabled = False
  Else
   ' _customer .IdCitizenship =
  End If
  If _customer.Status <> String.Empty Then
   ColorEdit1.Color = Color.FromArgb(CType(_customer.Status, Integer))
  End If
 End Sub

#Region " KeyPress "

 Private Sub uxCustomers_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
  Select Case Asc(e.KeyChar)
   Case 13
    SendKeys.Send("{TAB}")
  End Select
 End Sub

#End Region

#Region " WinPart Code "

 Protected Overrides Function GetIdValue() As Object

  Return My.Resources.uxCities

 End Function

 Public Overrides Function ToString() As String

  Return My.Resources.uxCustomers

 End Function

 Private Sub uxCustomer_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
  ApplyAuthorizationRules()
 End Sub

#End Region

#Region " Bindings "

 Private Sub LoadList()
  Me.CityListBindingSource.DataSource = CityList.GetCityList
  Me.BusinessTypeListBindingSource.DataSource = BusinessTypeList.GetBusinessTypeList
  Me.StreetsListBindingSource.DataSource = StreetsList.GetStreetsList
  _countriesList = CountriesList.GetCountriesList
  Me.CountriesListBindingSource.DataSource = _countriesList
  Me.RegistrationIssuerListBindingSource.DataSource = objRegistrationIssuerList
 End Sub

 Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
  ' disable events
  Me.ContactPersonsBindingSource.RaiseListChangedEvents = False
  Me.CustomerBindingSource.RaiseListChangedEvents = False
  Me.BankAccountsBindingSource.RaiseListChangedEvents = False
  Try
   UnbindBindingSource(Me.BankAccountsBindingSource, saveObject, False)
   UnbindBindingSource(Me.ContactPersonsBindingSource, saveObject, False)
   UnbindBindingSource(Me.CustomerBindingSource, saveObject, True)

   ' save or cancel changes
   If saveObject Then
    _customer.ApplyEdit()
    Try
     'Dim tmp As Customer = _customer.Clone
     _customer = _customer.Save 'tmp.Save
    Catch ex As Csla.Validation.ValidationException
     MsgBox(My.Resources.ValidationError) '("Some validation errors has occurred," + vbCrLf + " please check Broken rules collection on bottom for details")

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
    _customer.CancelEdit()
   End If
  Finally
   ' rebind UI if requested
   If rebind Then
    BindUI()
   End If

   ' restore events
   Me.CustomerBindingSource.RaiseListChangedEvents = True
   Me.ContactPersonsBindingSource.RaiseListChangedEvents = True
   Me.BankAccountsBindingSource.RaiseListChangedEvents = True
   If rebind Then
    ' refresh the UI if rebinding
    Me.CustomerBindingSource.ResetBindings(False)
    Me.ContactPersonsBindingSource.ResetBindings(False)
    Me.BankAccountsBindingSource.ResetBindings(False)
   End If

  End Try
 End Sub

 Private Sub BindUI()
  _customer.BeginEdit()
  Me.CustomerBindingSource.DataSource = _customer
 End Sub

 Private Sub CustomerBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
 Handles CustomerBindingSource.CurrentItemChanged, ContactPersonsBindingSource.CurrentItemChanged

  UxKopcinja1.cmdSave.Enabled = _customer.IsSavable
  UxKopcinja1.cmdCancel.Enabled = _customer.IsDirty

  Dim message As New System.Text.StringBuilder
  message.AppendFormat("{0}" + vbCrLf, "")
  For Each rule As Csla.Validation.BrokenRule In _
          _customer.BrokenRulesCollection
   message.AppendFormat( _
     "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
  Next

  ShowBrokenRules(message.ToString)
 End Sub

#End Region

#Region " Kopcinja "

 Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
  Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

   Case "cmdAdd"
    Try
     RebindUI(True, True)
     _customer = Nothing
     _customer = Customer.NewCustomer
     BindUI()
     CustomerFirstNameTextEdit.Focus()
    Catch ex As Exception
     MsgBox(ex.Message)
    End Try

   Case "cmdSave"
    Using busy As New Splash(My.Resources.textSaveing)
     Try
      RebindUI(True, True)
     Catch ex As Exception
      MessageBox.Show("FDGDFASGS")
     End Try

    End Using
    UxKopcinja1.cmdAdd.Focus()

   Case "cmdDelete"
    Me.CustomerBindingSource.RemoveCurrent()

   Case "cmdCancel"
    RebindUI(False, True)
    'Me.CustomerBindingSource.CancelEdit()
    'LoadList()

   Case "cmdExit"
    If _customer.IsDirty Then
     Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, My.Resources.exitNote)
      Case MsgBoxResult.Yes
       If Customers.CanEditObject Then
        RebindUI(True, False)
        Me.Close()
       Else
        If MsgBox(My.Resources.NoPermisionExit, _
          MsgBoxStyle.YesNo, My.Resources.exitNote) = MsgBoxResult.Yes Then
         Me.Close()
        End If
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

  End Select
 End Sub

#End Region


 Private Sub IsCompanyCheckEdit_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles IsCompanyCheckEdit.CheckStateChanged
  If IsCompanyCheckEdit.Checked Then
   _customer.IdBusinessType = 0
   ContactPersonsGridControl.Enabled = True
   OccupationTextEdit.Enabled = False
   CustomLookUpEditBusinesstype.Enabled = True
   LayoutControlItem23.Text = My.Resources.layoutCustomerDateOfBirthCompany
   LookUpEditBirthCity.Enabled = False
   LookUpEdit1.Enabled = False
   PassportNumberTextEdit.Enabled = False
   BLKTextEdit.Enabled = False

   WorksInCompanyTextEdit.Enabled = False
   TaxNumberTextEdit.Enabled = True
   'BirthAddressTextEdit.Enabled = False
   'CustomLookUpEditBirthAddress.Enabled = False
   'LookUpEditBirthCity.Enabled = False
   DriveingLicenceNumberTextEdit.Enabled = False
   DriveingLicenceDateIssuedDateEdit.Enabled = False
   DriveingLicenceIssuerTextEdit.Enabled = False
   BLKDateIssuedDateEdit.Enabled = False
   BLKIssuerTextEdit.Enabled = False
   PassIssuerTextEdit.Enabled = False
   PassDateIssuedDateEdit1.Enabled = False
  Else
   LookUpEditBirthCity.Enabled = True
   LookUpEdit1.Enabled = True
   ContactPersonsGridControl.Enabled = False
   OccupationTextEdit.Enabled = True
   CustomLookUpEditBusinesstype.Enabled = False
   LayoutControlItem23.Text = My.Resources.layoutCustomerDateOfBirth
   PassportNumberTextEdit.Enabled = True
   BLKTextEdit.Enabled = True
   WorksInCompanyTextEdit.Enabled = True
   TaxNumberTextEdit.Enabled = False
   'CustomLookUpEditBirthAddress.Enabled = True
   'BirthAddressTextEdit.Enabled = True
   'LookUpEditBirthCity.Enabled = True
   DriveingLicenceNumberTextEdit.Enabled = True
   DriveingLicenceDateIssuedDateEdit.Enabled = True
   DriveingLicenceIssuerTextEdit.Enabled = True
   BLKDateIssuedDateEdit.Enabled = True
   BLKIssuerTextEdit.Enabled = True
   PassIssuerTextEdit.Enabled = True
   PassDateIssuedDateEdit1.Enabled = True
  End If
 End Sub

 Private Sub CustomLookUpEditBusinesstype_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles CustomLookUpEditBusinesstype.ButtonPressed
  If e.Button.Index = 1 Then
   Dim par As MainForm = Me.ParentForm
   For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
    For Each ctl As Control In page.Controls
     If (TypeOf ctl Is uxBusinessTypes) Then
      par.ShowWinPart(CType(ctl, uxBusinessTypes))
      Exit Sub
     End If
    Next
   Next
   Using cekaj As New StatusBusy(My.Resources.txtLoading)
    Try
     par.AddWinPart(New uxBusinessTypes)
    Catch ex As Exception
     MsgBox(ex.Message)
    End Try
   End Using
  End If
 End Sub

 Private Sub LookUpEditBirthCity_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) _
 Handles LookUpEditBirthCity.ButtonPressed, LookUpEditLivingCity.ButtonPressed
  If e.Button.Index = 1 Then
   Dim par As MainForm = Me.ParentForm
   For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
    For Each ctl As Control In page.Controls
     If (TypeOf ctl Is uxCities) Then
      par.ShowWinPart(CType(ctl, uxCities))
      Exit Sub
     End If
    Next
   Next
   Using cekaj As New StatusBusy(My.Resources.txtLoading)
    Try
     par.AddWinPart(New uxCities)
    Catch ex As Exception
     MsgBox(ex.Message)
    End Try
   End Using
  End If
 End Sub


 Private Sub CustomLookUpEditBirthAddress_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles CustomLookUpEditLivingAddress.ButtonPressed
  If e.Button.Index = 1 Then
   Dim par As MainForm = Me.ParentForm
   For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
    For Each ctl As Control In page.Controls
     If (TypeOf ctl Is uxStreets) Then
      par.ShowWinPart(CType(ctl, uxStreets))
      Exit Sub
     End If
    Next
   Next
   Using cekaj As New StatusBusy(My.Resources.txtLoading)
    Try
     par.AddWinPart(New uxStreets)
    Catch ex As Exception
     MsgBox(ex.Message)
    End Try
   End Using
  End If
 End Sub

 Private Sub EmailTextEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles EmailTextEdit.GotFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
 End Sub

 Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
  If GridView1.FocusedColumn Is colEmail Then
   System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
  Else
   System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
  End If
 End Sub

 Private Sub EmailTextEdit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles EmailTextEdit.LostFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
 End Sub

 Private Sub CustomLookUpEditLivingAddress_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomLookUpEditLivingAddress.GotFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
 End Sub

 Private Sub CustomLookUpEditBirthAddress_ProcessNewValue(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) _
 Handles CustomLookUpEditLivingAddress.ProcessNewValue
  If e.DisplayValue.ToString = String.Empty Then
   e.Handled = False
   Exit Sub
  End If

  Try
   'Console.WriteLine("da")
   If Street.Exists(e.DisplayValue.ToString) = 0 Then
    If MsgBox(e.DisplayValue.ToString & My.Resources.nePostoi, MsgBoxStyle.YesNo, My.Resources.Zacuvaj & "?") = MsgBoxResult.Yes Then

     Dim newSt As Street = Street.NewStreet
     newSt.StreetName = e.DisplayValue.ToString()
     newSt.ApplyEdit()
     newSt = newSt.Save

     Me.StreetsListBindingSource.RaiseListChangedEvents = False

     Me.StreetsListBindingSource.DataSource = StreetsList.GetStreetsList()
     Me.StreetsListBindingSource.RaiseListChangedEvents = True
     Me.StreetsListBindingSource.ResetBindings(False)
    End If
    e.Handled = True
   End If
  Catch ex As Exception
   MsgBox(ex.Message)
  End Try

  e.Handled = True

 End Sub

 'Private Sub LookUpEditBirthCity_ProcessNewValue(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles LookUpEditBirthCity.ProcessNewValue, LookUpEditLivingCity.ProcessNewValue
 '  If e.DisplayValue.ToString = String.Empty Then
 '    e.Handled = False
 '    Exit Sub
 '  End If

 '  Try
 '    'Console.WriteLine("da")
 '    If City.Exists(e.DisplayValue.ToString) = 0 Then
 '      If MsgBox(e.DisplayValue.ToString & " " & My.Resources.nePostoi, MsgBoxStyle.YesNo, "Зачувај?") = MsgBoxResult.Yes Then

 '        Dim newC As City = City.NewCity
 '        newC.CityName = e.DisplayValue.ToString()
 '        newC.IdCountry = 1
 '        newC.ApplyEdit()
 '        newC = newC.Save

 '        Me.CityListBindingSource.RaiseListChangedEvents = False

 '        Me.CityListBindingSource.DataSource = CityList.GetCityList()
 '        Me.CityListBindingSource.RaiseListChangedEvents = True
 '        Me.CityListBindingSource.ResetBindings(False)
 '      End If
 '      e.Handled = True
 '    End If
 '  Catch ex As Exception
 '    MsgBox(ex.Message)
 '  End Try

 '  e.Handled = True
 'End Sub

 'Private Sub CustomLookUpEditBusinesstype_ProcessNewValue(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles CustomLookUpEditBusinesstype.ProcessNewValue
 '  If e.DisplayValue.ToString = String.Empty Then
 '    e.Handled = False
 '    Exit Sub
 '  End If

 '  Try
 '    'Console.WriteLine("da")
 '    If BusinessType.Exists(e.DisplayValue.ToString) = 0 Then
 '      If MsgBox(e.DisplayValue.ToString & " " & My.Resources.nePostoi, MsgBoxStyle.YesNo, "Зачувај?") = MsgBoxResult.Yes Then

 '        Dim newC As BusinessType = BusinessType.NewBusinessType
 '        newC.BusinessTypeDescription = e.DisplayValue.ToString()
 '        newC.BusinessType = e.DisplayValue.ToString()
 '        newC.ApplyEdit()
 '        newC = newC.Save

 '        Me.BusinessTypeListBindingSource.RaiseListChangedEvents = False

 '        Me.BusinessTypeListBindingSource.DataSource = BusinessTypeList.GetBusinessTypeList()
 '        Me.BusinessTypeListBindingSource.RaiseListChangedEvents = True
 '        Me.BusinessTypeListBindingSource.ResetBindings(False)
 '      End If
 '      e.Handled = True
 '    End If
 '  Catch ex As Exception
 '    MsgBox(ex.Message)
 '  End Try

 '  e.Handled = True
 'End Sub


#Region " Scan "
 Private _attachments As Attachments
 Private _attachmentsByCustomer As Attachments
 Private _attachmentsTypeList As AttachmentTypeList
 Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
  If _customer.Id > 0 Then
   AttachmentsTab.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible
   _attachments = Attachments.GetAttachments
   _attachmentsByCustomer = Attachments.GetAttachmentByIdCustomer(_customer.Id, True)
   _attachmentsTypeList = AttachmentTypeList.GetAttachmentTypeList
   AttachmentsBindingSource.DataSource = _attachmentsByCustomer
   BindingSourceAttachmentTypeList.DataSource = _attachmentsTypeList
  Else
   AttachmentsTab.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden
   MsgBox("Комитентот мора прво да биде зачуван")
  End If
 End Sub

 Private Sub AttachmentsBindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles AttachmentsBindingSource.ListChanged
  If _attachmentsByCustomer IsNot Nothing Then
   Dim tmp As Boolean = (_attachmentsByCustomer.Count > 0)
   Me.btnDelete.Enabled = tmp
   Me.btnEdit.Enabled = tmp
  End If
 End Sub

 Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click

  Dim att As Attachment = _attachments.AddNew
  att.IdCustomer = _customer.Id
  Dim dijT As New dijTwain(att)

  If dijT.ShowDialog(Me) = DialogResult.OK Then
   _attachments.Save()
   _attachmentsByCustomer = Attachments.GetAttachmentByIdCustomer(_customer.Id, True)
   AttachmentsBindingSource.DataSource = _attachmentsByCustomer
  Else
   'cancel edit na att
   _attachments.Remove(att)
  End If

 End Sub

 Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
  Dim att As Attachment = (CType(Me.AttachmentsBindingSource.Current, Attachment))
  Dim i As Integer = 0
  For Each attach As Attachment In _attachments
   If att.Id = attach.Id Then
    'UnbindBindingSource (AttachmentsBindingSource ,True ,True )
    AttachmentsBindingSource.DataSource = Nothing
    _attachments.Item(i).IdCustomer = 0
    _attachments.Save()
    Exit For
   Else
    i += 1
   End If
  Next
  AttachmentsBindingSource.DataSource = Attachments.GetAttachmentByIdCustomer(_customer.Id, True)


  'Me.AttachmentsBindingSource.RemoveCurrent()
 End Sub

 Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click, RepositoryItemPictureEdit1.DoubleClick

  Try
   Dim att As Attachment
   Using bus As New Splash(My.Resources.LoadingData)
    att = _attachmentsByCustomer(Me.AttachmentsBindingSource.Position)
   End Using


   Dim dijT As New dijTwain(att)
   dijT.ShowDialog()
  Catch ex As Exception

  End Try
 End Sub

#End Region

 Private Sub ColorEdit1_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ColorEdit1.EditValueChanged
  _customer.Status = ColorEdit1.Color.ToArgb
 End Sub

 Private Sub MbTextEdit_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MbTextEdit.Validated
  If MbTextEdit.Text <> String.Empty Then
   Dim pom As Customer
   Dim pomString As String = MbTextEdit.Text
   If _customer.IsNew Then
    Try
     pom = Customer.GetCustomerByMB(MbTextEdit.Text)
    Catch ex As Exception
     pom = Nothing
    End Try


    If pom IsNot Nothing AndAlso pom.Id > 0 Then
     _customer = pom

    Else
     _customer = Customer.NewCustomer
     _customer.Mb = pomString

    End If
    Me.CustomerBindingSource.DataSource = _customer
   End If
  End If
 End Sub

End Class
