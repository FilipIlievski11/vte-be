Public Class uxCompany

 Private WithEvents _companies As Company

#Region "PritisnatoKopce"

 Private Sub uxCompanies_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
  Select Case Asc(e.KeyChar)
   Case 13
    SendKeys.Send("{TAB}")
  End Select
 End Sub
#End Region

#Region "WinPart"

 Protected Overrides Function GetIdValue() As Object
  Return My.Resources.uxCompanies
 End Function

 Public Overrides Function ToString() As String
  Return My.Resources.uxCompanies
 End Function

#End Region

 'Private Sub ApplyAuthorizationRules()
 ' Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.Companies.CanAddObject
 ' Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.Companies.CanDeleteObject
 ' Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.Companies.CanEditObject
 'End Sub

 Private Sub uxCompanies_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  Try
   UxKopcinja1.cmdAdd.Enabled = False
   UxKopcinja1.cmdDelete.Enabled = False
   'ApplyAuthorizationRules()
   System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo(uSettings.Culture))
   _companies = Company.GetCompany(CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)

   If _companies IsNot Nothing Then
    Me.CompaniesBindingSource.DataSource = _companies
    Me.UxKopcinja1.cmdSave.Enabled = _companies.IsSavable
    Me.UxKopcinja1.cmdCancel.Enabled = _companies.IsDirty
   End If
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


 Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
  Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name
   'Case "cmdAdd"
   ' _companies.AddNew()
   ' GridView1.Focus()

   Case "cmdSave"
    'RebindUI(True, True)
    _companies.Save()
    UxKopcinja1.cmdAdd.Focus()

    'Case "cmdDelete"
    ' If _companies IsNot Nothing Then
    '  _companies.Delete()

    ' End If

   Case "cmdCancel"
    _companies = Company.GetCompany(CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
    Me.CompaniesBindingSource.DataSource = _companies
    '    RebindUI(False, True)

   Case "cmdExit"
    If _companies.IsDirty Then
     Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, My.Resources.exitNote)
      Case MsgBoxResult.Yes
       _companies.Save()
       Me.CompaniesBindingSource.DataSource = _companies

       'RebindUI(True, False)
       Me.Close()

      Case MsgBoxResult.No
       _companies = Company.GetCompany(CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
       Me.CompaniesBindingSource.DataSource = _companies

       'RebindUI(False, False)
       Me.Close()
      Case MsgBoxResult.Cancel
       Exit Sub
     End Select
    Else
     Me.Close()
    End If

  End Select
 End Sub

 'Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
 ' ' stop the flow of events
 ' Me.CompaniesBindingSource.RaiseListChangedEvents = False
 ' ' commit edits in memory
 ' UnbindBindingSource(Me.CompaniesBindingSource, saveObject, True)
 ' Try
 '  ' save or cancel changes
 '  If saveObject Then
 '   Try
 '    _companies = _companies.Save
 '   Catch ex As Csla.Validation.ValidationException
 '    MsgBox(My.Resources.ValidationError)
 '   Catch ex As Csla.DataPortalException
 '    MessageBox.Show(ex.BusinessException.ToString, _
 '      "Error saving", MessageBoxButtons.OK, _
 '      MessageBoxIcon.Exclamation)

 '   Catch ex As Exception
 '    MessageBox.Show(ex.ToString, _
 '      "Error saving", MessageBoxButtons.OK, _
 '      MessageBoxIcon.Exclamation)
 '   End Try
 '  Else
 '   _companies = Nothing
 '   Try
 '    _companies = Companies.GetCompanies
 '   Catch ex As Csla.DataPortalException
 '    MessageBox.Show(ex.BusinessException.ToString, _
 '      "Error saving", MessageBoxButtons.OK, _
 '      MessageBoxIcon.Exclamation)

 '   Catch ex As Exception
 '    MessageBox.Show(ex.ToString, _
 '      "Error saving", MessageBoxButtons.OK, _
 '      MessageBoxIcon.Exclamation)
 '   End Try
 '  End If
 ' Finally
 '  Me.CompaniesBindingSource.DataSource = _companies
 '  Me.CompaniesBindingSource.RaiseListChangedEvents = True
 '  If rebind Then
 '   Me.CompaniesBindingSource.ResetBindings(False)
 '  End If

 ' End Try
 'End Sub

 Private Sub CompaniesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CompaniesBindingSource.CurrentItemChanged
  Try
   If _companies.IsSavable Then
    UxKopcinja1.cmdSave.Enabled = True
   Else
    UxKopcinja1.cmdSave.Enabled = False
   End If
   If _companies.IsDirty Then
    UxKopcinja1.cmdCancel.Enabled = True
   Else
    UxKopcinja1.cmdCancel.Enabled = False
   End If
   Dim message As New System.Text.StringBuilder
   message.AppendFormat("{0}" + vbCrLf, "")
   'For Each child As VTE.Library.Company In _companies
   For Each rule As Csla.Validation.BrokenRule In _
         _companies.BrokenRulesCollection
    message.AppendFormat( _
      "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
   Next
   ' Next
   ShowBrokenRules(message.ToString)
  Catch
  End Try
 End Sub

End Class

