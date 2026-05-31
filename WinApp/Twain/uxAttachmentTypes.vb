Public Class uxAttachmentTypes


  Private WithEvents _attachmentTypes As AttachmentTypes

#Region " KeyPress "

  Private Sub uxCommunities_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

        Return My.Resources.uxAttachmentTypes


  End Function

  Public Overrides Function ToString() As String

        Return My.Resources.uxAttachmentTypes

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.AttachmentTypes.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.AttachmentTypes.CanEditObject
    AttachmentTypesGridControl.Enabled = VTE.Library.AttachmentTypes.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.AttachmentTypes.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.AttachmentTypes.CanDeleteObject


  End Sub

  Private Sub uxAttachmentTypes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _attachmentTypes = AttachmentTypes.GetAttachmentTypes
      If _attachmentTypes IsNot Nothing Then
        Me.AttachmentTypesBindingSource.DataSource = _attachmentTypes
        Me.UxKopcinja1.cmdSave.Enabled = _attachmentTypes.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _attachmentTypes.IsDirty
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


  Private Sub uxAttachmentTypes_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.AttachmentTypesBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _attachmentTypes.Count <> 0 Then
          _attachmentTypes.RemoveAt(Me.AttachmentTypesBindingSource.Position)
        End If
      Case "cmdExit"
        If _attachmentTypes.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If AttachmentTypes.CanEditObject Then
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
    Me.AttachmentTypesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.AttachmentTypesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _attachmentTypes = _attachmentTypes.Save
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
        _attachmentTypes = Nothing
        Try
          _attachmentTypes = AttachmentTypes.GetAttachmentTypes
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
      Me.AttachmentTypesBindingSource.DataSource = _attachmentTypes

      Me.AttachmentTypesBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.AttachmentTypesBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub AttachmentTypesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AttachmentTypesBindingSource.CurrentItemChanged
    Try
      If _attachmentTypes.IsSavable Then
        UxKopcinja1.cmdSave.Enabled = True
      Else
        UxKopcinja1.cmdSave.Enabled = False
      End If
      If _attachmentTypes.IsDirty Then
        UxKopcinja1.cmdCancel.Enabled = True
      Else
        UxKopcinja1.cmdCancel.Enabled = False
      End If
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As AttachmentType In _attachmentTypes
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

  Private Sub AttachmentTypesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles AttachmentTypesGridControl.ProcessGridKey

    If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
      If _attachmentTypes.IsSavable Then
        GridView1.AddNewRow()
      Else
        e.SuppressKeyPress = True
      End If
    End If

  End Sub


End Class
