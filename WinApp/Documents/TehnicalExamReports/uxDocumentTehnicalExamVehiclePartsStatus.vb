Public Class uxDocumentTehnicalExamVehiclePartsStatus


  Private WithEvents _statuses As DocumentsTehnicalExamsReportsDetailsStatuses
#Region "PritisnatoKopce"

  Private Sub uxDocumentTehnicalExamVehiclePartsStatus_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")

    End Select
  End Sub
#End Region

#Region "WinPart"

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxDocumentTehnicalExamVehiclePartsStatus
  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxDocumentTehnicalExamVehiclePartsStatus
  End Function

#End Region

  Private Sub ApplyAuthorizationRules()
    If Not VTE.Library.DocumentsTehnicalExamsReportsDetailsStatuses.CanGetObject Then
      Me.Close()
    End If
    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.DocumentsTehnicalExamsReportsDetailsStatuses.CanAddObject
    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.DocumentsTehnicalExamsReportsDetailsStatuses.CanDeleteObject
    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.DocumentsTehnicalExamsReportsDetailsStatuses.CanEditObject
  End Sub

  Private Sub uxDocumentTehnicalExamVehiclePartsStatus_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()

      _statuses = DocumentsTehnicalExamsReportsDetailsStatuses.GetDocumentsTehnicalExamsReportsDetailsStatuses
      If _statuses IsNot Nothing Then
        Me.DocumentsTehnicalExamsReportsDetailsStatusesBindingSource.DataSource = _statuses
        Me.UxKopcinja1.cmdSave.Enabled = _statuses.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _statuses.IsDirty
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

  Private Sub uxDDVCatalog_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name
      Case "cmdAdd"
        _statuses.AddNew()
        GridView1.Focus()

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdDelete"
        If _statuses.Count > 0 Then
          _statuses.RemoveAt(DocumentsTehnicalExamsReportsDetailsStatusesBindingSource.Position)
        End If

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdExit"
        If _statuses.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VTE.Library.DocumentsTehnicalExamsReportsDetailsStatuses.CanEditObject Then
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
    Me.DocumentsTehnicalExamsReportsDetailsStatusesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.DocumentsTehnicalExamsReportsDetailsStatusesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _statuses = _statuses.Save
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
        _statuses = Nothing
        Try
          _statuses = DocumentsTehnicalExamsReportsDetailsStatuses.GetDocumentsTehnicalExamsReportsDetailsStatuses
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
      Me.DocumentsTehnicalExamsReportsDetailsStatusesBindingSource.DataSource = _statuses
      Me.DocumentsTehnicalExamsReportsDetailsStatusesBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.DocumentsTehnicalExamsReportsDetailsStatusesBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub


  Private Sub GridControl1_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DocumentsTehnicalExamsReportsDetailsStatusesGridControl.ProcessGridKey
    Select Case e.KeyCode
      Case Keys.Add, Keys.Oemplus
        DocumentsTehnicalExamsReportsDetailsStatusesBindingSource.AddNew()
        GridView1.Focus()
    End Select
  End Sub

  Private Sub DocumentsTehnicalExamsReportsDetailsStatusesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DocumentsTehnicalExamsReportsDetailsStatusesBindingSource.CurrentItemChanged
    Try

      UxKopcinja1.cmdSave.Enabled = _statuses.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _statuses.IsDirty
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VTE.Library.DocumentsTehnicalExamsReportsDetailsStatus In _statuses
        For Each rule As Csla.Validation.BrokenRule In _
              child.BrokenRulesCollection
          message.AppendFormat( _
            "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
        Next
      Next
      ShowBrokenRules(message.ToString)
    Catch
    End Try
  End Sub

End Class


