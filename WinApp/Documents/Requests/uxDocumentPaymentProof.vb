Public Class uxDocumentPaymentProof

  Private WithEvents _DocumentPaymentProof As DocumentPaymentProofes

#Region " KeyPress "

  Private Sub uxDocumentPaymentProof_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxDocumentPaymentProof

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxDocumentPaymentProof

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.DocumentPaymentProofes.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.DocumentPaymentProofes.CanEditObject
    DocumentPaymentProofesGridControl.Enabled = VTE.Library.DocumentPaymentProofes.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.DocumentPaymentProofes.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.DocumentPaymentProofes.CanDeleteObject


  End Sub

  Private Sub uxDocumentPaymentProofes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _DocumentPaymentProof = DocumentPaymentProofes.GetDocumentPaymentProofes
      If _DocumentPaymentProof IsNot Nothing Then
        Me.DocumentPaymentProofesBindingSource.DataSource = _DocumentPaymentProof
        Me.UxKopcinja1.cmdSave.Enabled = _DocumentPaymentProof.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _DocumentPaymentProof.IsDirty
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


  Private Sub uxDocumentPaymentProofes_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.DocumentPaymentProofesBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _DocumentPaymentProof.Count <> 0 Then
          _DocumentPaymentProof.RemoveAt(Me.DocumentPaymentProofesBindingSource.Position)
        End If
      Case "cmdExit"
        If _DocumentPaymentProof.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If DocumentPaymentProofes.CanEditObject Then
                RebindUI(True, False)
                Me.Close()
              Else
                'If MsgBox("Немате дозвола го запамтите објектот, дали сакате да излезете", _
                '  MsgBoxStyle.YesNo, "излез") = MsgBoxResult.Yes Then
                Me.Close()
                'End If
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
    Me.DocumentPaymentProofesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.DocumentPaymentProofesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _DocumentPaymentProof = _DocumentPaymentProof.Save
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
        _DocumentPaymentProof = Nothing
        Try
          _DocumentPaymentProof = DocumentPaymentProofes.GetDocumentPaymentProofes
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
      Me.DocumentPaymentProofesBindingSource.DataSource = _DocumentPaymentProof

      Me.DocumentPaymentProofesBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.DocumentPaymentProofesBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub DocumentPaymentProofesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DocumentPaymentProofesBindingSource.CurrentItemChanged
    Try
      If _DocumentPaymentProof.IsSavable Then
        UxKopcinja1.cmdSave.Enabled = True
      Else
        UxKopcinja1.cmdSave.Enabled = False
      End If
      If _DocumentPaymentProof.IsDirty Then
        UxKopcinja1.cmdCancel.Enabled = True
      Else
        UxKopcinja1.cmdCancel.Enabled = False
      End If
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As DocumentPaymentProof In _DocumentPaymentProof
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

  Private Sub DocumentPaymentProofesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DocumentPaymentProofesGridControl.ProcessGridKey

    If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
      If _DocumentPaymentProof.IsSavable Then
        GridView1.AddNewRow()
      Else
        e.SuppressKeyPress = True
      End If
    End If

  End Sub


End Class
