Public Class uxTehnicalExamOrganizations

 Private WithEvents _TEOrganizations As TehnicalExamOrganizations

#Region " KeyPress "

 Private Sub uxTehnicalExamOrganizations_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
  Select Case Asc(e.KeyChar)
   Case 13
    SendKeys.Send("{TAB}")
  End Select
 End Sub

#End Region

#Region " WinPart Code "

 Protected Overrides Function GetIdValue() As Object

  Return My.Resources.uxTehnicalExamOrganizations

 End Function

 Public Overrides Function ToString() As String

  Return My.Resources.uxTehnicalExamOrganizations

 End Function
#End Region

 Private Sub ApplyAuthorizationRules()

  'kontroli eanble/disable
  Me.ReadWriteAuthorization1.ResetControlAuthorization()

  If Not VTE.Library.TehnicalExamOrganizations.CanGetObject Then
   Me.Close()
  End If

  Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.TehnicalExamOrganizations.CanEditObject
  TehnicalExamOrganizationsGridControl.Enabled = VTE.Library.TehnicalExamOrganizations.CanEditObject

  Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.TehnicalExamOrganizations.CanAddObject

  Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.TehnicalExamOrganizations.CanDeleteObject

 End Sub

 Private Sub uxStreets_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

  ApplyAuthorizationRules()
  Me.CityListBindingSource.DataSource = CityList.GetCityList
  _TEOrganizations = TehnicalExamOrganizations.GetTehnicalExamOrganizations
  If _TEOrganizations IsNot Nothing Then
   Me.TehnicalExamOrganizationsBindingSource.DataSource = _TEOrganizations
  End If
  Me.CompanyListBindingSource.DataSource = objCompanyList
  UxKopcinja1.cmdAdd.Focus()

 End Sub

 Private Sub uxTehnicalExamOrganizations_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
  ApplyAuthorizationRules()
 End Sub

 Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
  Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

   Case "cmdAdd"
    Try
     Me.TehnicalExamOrganizationsBindingSource.AddNew()
     GridView1.Focus()
    Catch ex As Exception
     MsgBox(ex.Message)
    End Try
    '_cities.Item(Me.CitiesBindingSource.Position).CommunityCode = 8
   Case "cmdSave"
    RebindUI(True, True)
    UxKopcinja1.cmdAdd.Focus()
   Case "cmdDelete"
    Me.TehnicalExamOrganizationsBindingSource.RemoveCurrent()
   Case "cmdCancel"
    RebindUI(False, True)
   Case "cmdExit"
    If _TEOrganizations.IsDirty Then
     Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
      Case MsgBoxResult.Yes
       If TehnicalExamOrganizations.CanEditObject Then
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

  End Select
 End Sub

 Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
  ' stop the flow of events
  Me.TehnicalExamOrganizationsBindingSource.RaiseListChangedEvents = False
  ' commit edits in memory
  UnbindBindingSource(Me.TehnicalExamOrganizationsBindingSource, saveObject, True)
  Try
   ' save or cancel changes
   If saveObject Then
    Try
     _TEOrganizations = _TEOrganizations.Save
    Catch ex As Csla.Validation.ValidationException
     MsgBox(My.Resources.ValidationError)
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
    _TEOrganizations = Nothing
    Try
     _TEOrganizations = TehnicalExamOrganizations.GetTehnicalExamOrganizations
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
   Me.TehnicalExamOrganizationsBindingSource.DataSource = _TEOrganizations

   Me.TehnicalExamOrganizationsBindingSource.RaiseListChangedEvents = True
   Me.TehnicalExamOrganizationsBindingSource.ResetBindings(False)
  End Try
 End Sub

 Private Sub BindUI()
  _TEOrganizations.BeginEdit()
  Me.TehnicalExamOrganizationsBindingSource.DataSource = _TEOrganizations
 End Sub

 Private Sub TehnicalExamOrganizationsBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TehnicalExamOrganizationsBindingSource.CurrentItemChanged
  If _TEOrganizations.IsSavable Then
   UxKopcinja1.cmdSave.Enabled = True
  Else
   UxKopcinja1.cmdSave.Enabled = False
  End If
  If _TEOrganizations.IsDirty Then
   UxKopcinja1.cmdCancel.Enabled = True
  Else
   UxKopcinja1.cmdCancel.Enabled = False
  End If
  Dim message As New System.Text.StringBuilder
  message.AppendFormat("{0}" + vbCrLf, "")
  For Each child As TehnicalExamOrganization In _TEOrganizations
   For Each rule As Csla.Validation.BrokenRule In _
         child.BrokenRulesCollection
    message.AppendFormat( _
      "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
   Next
  Next
  ShowBrokenRules(message.ToString)
 End Sub

 Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
  Dim det As TehnicalExamOrganization = _TEOrganizations.Item(Me.TehnicalExamOrganizationsBindingSource.Position)
  For Each column As DevExpress.XtraGrid.Columns.GridColumn In GridView1.Columns
   column.OptionsColumn.ReadOnly = Not det.CanWriteProperty(column.FieldName)
  Next
 End Sub


 Private Sub GridView1_InitNewRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles GridView1.InitNewRow
  Dim det As TehnicalExamOrganization = _TEOrganizations.Item(Me.TehnicalExamOrganizationsBindingSource.Position)
  For Each column As DevExpress.XtraGrid.Columns.GridColumn In GridView1.Columns
   column.OptionsColumn.ReadOnly = Not det.CanWriteProperty(column.FieldName)
  Next
 End Sub

 Private AllowShowEditor As Boolean = True

 Private Sub GridView1_FocusedRowChanged1(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
  AllowShowEditor = False
  BeginInvoke(New MethodInvoker(AddressOf tr))
 End Sub
 Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
  AllowShowEditor = False
  BeginInvoke(New MethodInvoker(AddressOf tr))
 End Sub
 Private Sub GridView1_ShowingEditor1(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles GridView1.ShowingEditor
  e.Cancel = Not AllowShowEditor
 End Sub

 Private Sub tr()
  AllowShowEditor = True
 End Sub

 'Private Sub TehnicalExamOrganizationsGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TehnicalExamOrganizationsGridControl.ProcessGridKey

 '  If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
 '    If _TEOrganizations.IsSavable Then
 '      GridView1.AddNewRow()
 '      GridView1.FocusedColumn = colOrganizationName
 '    Else
 '      e.SuppressKeyPress = True
 '    End If
 '  End If

 'End Sub


 Private Sub LookUpEditCity_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditCity.ButtonPressed
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
End Class
