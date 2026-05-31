Public Class dijOptions
 'Private WithEvents _options As Options
 Private WithEvents _curentTehExamOrganization As TehnicalExamOrganization
 Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
  Dim dirInfo As New System.IO.DirectoryInfo(My.Application.Info.DirectoryPath)

  For Each fInfo As System.IO.FileInfo In dirInfo.GetFiles()
   Console.WriteLine(fInfo.Name)
   If (fInfo.Extension = ".xml") AndAlso (fInfo.Name.Contains("GridControl") Or fInfo.Name.Contains("LayoutControl") Or fInfo.Name.Contains("DataLayoutControl")) Then
    Console.WriteLine(fInfo.Name)
    fInfo.Delete()
   End If
  Next
  Me.Close()
    End Sub

 Private Sub ApplyAuthorizationRules()
 End Sub
 Private Sub dijOptions_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  Try
   ApplyAuthorizationRules()
   Me.UserSettingsBindingSource.DataSource = uSettings
   _curentTehExamOrganization = TehnicalExamOrganization.GetTehnicalExamOrganization(objCurentTehExamOrganization.Id)
   Me.TehnicalExamOrganizationBindingSource.DataSource = _curentTehExamOrganization
   Me.CommunitiesListBindingSource.DataSource = CommunitiesList.GetCommunitiesList
   Me.PaymentPrintOptionListBindingSource.DataSource = PaymentPrintOptionList.GetPaymentPrintOptionList
   Me.RegistrationIssuerListBindingSource.DataSource = RegistrationIssuerList.GetRegistrationIssuerList
   Me.CityListBindingSource.DataSource = CityList.GetCityList()
   Me.PaymentCategoryListBindingSource.DataSource = PaymentCategoryList.GetPaymentCategoryList
   Me.CompanyListBindingSource.DataSource = objCompanyList
   Me.OptionsBindingSource.DataSource = objOpcii
   PercentOfTehSpinEdit.Enabled = _curentTehExamOrganization.CalculatePercentOfTeh
   CustomLookUpEdit1.Enabled = _curentTehExamOrganization.CalculatePercentOfTeh
   CustomLookUpEdit2.Enabled = _curentTehExamOrganization.CalculatePercentOfTeh
  Catch ex As Csla.DataPortalException
   MessageBox.Show(ex.BusinessException.ToString, _
     "Error loading", MessageBoxButtons.OK, _
     MessageBoxIcon.Exclamation)
  Catch ex As Exception
   MessageBox.Show(ex.ToString, _
     "Error loading", MessageBoxButtons.OK, _
     MessageBoxIcon.Exclamation)
  End Try
 End Sub

 Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
  Me.Close()

 End Sub

 Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
  'objOpcii.ApplyEdit()
  RebindUI(True, True)
  'objOpcii.Save()
  Me.Close()
 End Sub
 Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
  ' stop the flow of events
  Me.OptionsBindingSource.RaiseListChangedEvents = False
  Me.UserSettingsBindingSource.RaiseListChangedEvents = False
  Me.TehnicalExamOrganizationBindingSource.RaiseListChangedEvents = False
  ' commit edits in memory
  UnbindBindingSource(Me.OptionsBindingSource, saveObject, True)
  UnbindBindingSource(Me.UserSettingsBindingSource, saveObject, True)
  UnbindBindingSource(Me.TehnicalExamOrganizationBindingSource, saveObject, True)
  Try
   ' save or cancel changes
   If saveObject Then
    Try
     objOpcii = objOpcii.Save
     uSettings = uSettings.Save
     _curentTehExamOrganization = _curentTehExamOrganization.Save
     If Csla.ApplicationContext.LocalContext.Contains("uSettings") Then
      Csla.ApplicationContext.LocalContext.Remove("uSettings")
     End If
          Csla.ApplicationContext.LocalContext.Add("uSettings", uSettings)

     If Csla.ApplicationContext.LocalContext.Contains("objOpcii") Then
      Csla.ApplicationContext.LocalContext.Remove("objOpcii")
     End If
     Csla.ApplicationContext.LocalContext.Add("objOpcii", objOpcii)

     If Csla.ApplicationContext.LocalContext.Contains("objCurentTehExamOrganization") Then
      Csla.ApplicationContext.LocalContext.Remove("objCurentTehExamOrganization")
          End If

     objCurentTehExamOrganization = TehnicalExamOrganizationsList.GetTehnicalExamOrganizationsList. _
     GetTehnicalExamOrganizationsInfoById(_curentTehExamOrganization.Id)
     Csla.ApplicationContext.LocalContext.Add("objCurentTehExamOrganization", objCurentTehExamOrganization)

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
    objOpcii = Nothing
    Try
     objOpcii = Options.GetOptions()
     objCurentTehExamOrganization = objTehExamOrganizations.GetTehnicalExamOrganizationsInfoById(_curentTehExamOrganization.Id)
    Catch ex As Csla.DataPortalException
     MessageBox.Show(ex.BusinessException.ToString, _
       "Error saving", MessageBoxButtons.OK, _
       MessageBoxIcon.Exclamation)

    Catch ex As Exception
     MessageBox.Show(ex.ToString, _
       "Error saving", MessageBoxButtons.OK, _
       MessageBoxIcon.Exclamation)
    End Try

   End If


  Finally
   Me.OptionsBindingSource.DataSource = objOpcii
   Me.UserSettingsBindingSource.DataSource = uSettings
   Me.TehnicalExamOrganizationBindingSource.DataSource = _curentTehExamOrganization
   Me.OptionsBindingSource.RaiseListChangedEvents = True
   Me.UserSettingsBindingSource.RaiseListChangedEvents = True
   Me.TehnicalExamOrganizationBindingSource.RaiseListChangedEvents = True
   Me.OptionsBindingSource.ResetBindings(False)
   Me.UserSettingsBindingSource.ResetBindings(False)
   Me.TehnicalExamOrganizationBindingSource.ResetBindings(False)
  End Try
 End Sub

#Region " Data binding helpers "

 Protected Sub UnbindBindingSource( _
   ByVal source As BindingSource, ByVal apply As Boolean, ByVal isRoot As Boolean)

  Dim current As System.ComponentModel.IEditableObject = _
          TryCast(source.Current, System.ComponentModel.IEditableObject)
  If isRoot Then
   source.DataSource = Nothing
  End If
  If current IsNot Nothing Then
   If apply Then
    current.EndEdit()
   Else
    current.CancelEdit()
   End If
  End If

 End Sub

#End Region

 Private Sub cboPictureServerPath_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles cboPictureServerPath.ButtonClick
  If FolderBrowserDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
   cboPictureServerPath.EditValue = FolderBrowserDialog1.SelectedPath
  End If
 End Sub

 Private Sub cboFiscalPath_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles cboFiscalPath.ButtonClick
  If FolderBrowserDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
   cboFiscalPath.EditValue = FolderBrowserDialog1.SelectedPath
  End If
 End Sub


 Private Sub ComboBoxEditLogo_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles ComboBoxEditLogo.ButtonClick
  If OpenFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
   ComboBoxEditLogo.EditValue = OpenFileDialog1.FileName
  End If
 End Sub

 Private Sub CalculatePercentOfTehCheckEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CalculatePercentOfTehCheckEdit.CheckedChanged
  PercentOfTehSpinEdit.Enabled = CalculatePercentOfTehCheckEdit.Checked
  CustomLookUpEdit1.Enabled = CalculatePercentOfTehCheckEdit.Checked
  CustomLookUpEdit2.Enabled = CalculatePercentOfTehCheckEdit.Checked
 End Sub


End Class