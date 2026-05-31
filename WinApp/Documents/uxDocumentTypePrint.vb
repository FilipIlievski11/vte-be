Public Class uxDocumentTypePrint

  Private WithEvents _documentTypePrins As DocumentTypePrints

#Region " KeyPress "

  Private Sub uxDocumentTypePrint_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxDocumentTypePrint

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxDocumentTypePrint

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.DocumentTypePrints.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.DocumentTypePrints.CanEditObject
    DocumentTypePrintsGridControl.Enabled = VTE.Library.DocumentTypePrints.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.DocumentTypePrints.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.DocumentTypePrints.CanDeleteObject


  End Sub

  Private Sub uxDocumentTypePrints_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _documentTypePrins = DocumentTypePrints.GetDocumentTypePrints
      If _documentTypePrins IsNot Nothing Then
        Me.DocumentTypePrintsBindingSource.DataSource = _documentTypePrins
        Me.UxKopcinja1.cmdSave.Enabled = _documentTypePrins.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _documentTypePrins.IsDirty
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


  Private Sub uxDocumentTypePrints_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.DocumentTypePrintsBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _documentTypePrins.Count <> 0 Then
          _documentTypePrins.RemoveAt(Me.DocumentTypePrintsBindingSource.Position)
        End If
      Case "cmdExit"
        If _documentTypePrins.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If DocumentTypePrints.CanEditObject Then
                RebindUI(True, False)
              End If
              Me.Close()
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
    Me.DocumentTypePrintsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.DocumentTypePrintsBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _documentTypePrins = _documentTypePrins.Save
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
        _documentTypePrins = Nothing
        Try
          _documentTypePrins = DocumentTypePrints.GetDocumentTypePrints
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
      Me.DocumentTypePrintsBindingSource.DataSource = _documentTypePrins

      Me.DocumentTypePrintsBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.DocumentTypePrintsBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub DocumentTypePrintsBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DocumentTypePrintsBindingSource.CurrentItemChanged
    Try
      UxKopcinja1.cmdSave.Enabled = _documentTypePrins.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _documentTypePrins.IsDirty
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As DocumentTypePrint In _documentTypePrins
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

  Private Sub DocumentTypePrintsGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DocumentTypePrintsGridControl.ProcessGridKey

    If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
      If _documentTypePrins.IsSavable Then
        GridView1.AddNewRow()
      Else
        e.SuppressKeyPress = True
      End If
    End If

  End Sub


End Class
