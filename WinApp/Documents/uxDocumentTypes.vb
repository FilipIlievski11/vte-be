Public Class uxDocumentTypes

  Private WithEvents _documentTypes As DocumentTypes

#Region " KeyPress "

  Private Sub uxDocumentTypes_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxDocumentTypes

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxDocumentTypes

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.DocumentTypes.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.DocumentTypes.CanEditObject
    DocumentTypesGridControl.Enabled = VTE.Library.DocumentTypes.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.DocumentTypes.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.DocumentTypes.CanDeleteObject


  End Sub

  Private Sub uxDocumentTypePrints_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _documentTypes = DocumentTypes.GetDocumentTypes
      If _documentTypes IsNot Nothing Then
        Me.DocumentTypesBindingSource.DataSource = _documentTypes
        Me.UxKopcinja1.cmdSave.Enabled = _documentTypes.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _documentTypes.IsDirty
      End If
      Me.DocumentTypePrintsListBindingSource.DataSource = DocumentTypePrintsList.GetDocumentTypePrintsList
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
        Me.DocumentTypesBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _documentTypes.Count <> 0 Then
          _documentTypes.RemoveAt(Me.DocumentTypesBindingSource.Position)
        End If
      Case "cmdExit"
        If _documentTypes.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If DocumentTypes.CanEditObject Then
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
    Me.DocumentTypesBindingSource.RaiseListChangedEvents = False
    Me.DocumentTypesOptionsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.DocumentTypesOptionsBindingSource, saveObject, False)
    UnbindBindingSource(Me.DocumentTypesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _documentTypes = _documentTypes.Save
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
        _documentTypes = Nothing
        Try
          _documentTypes = DocumentTypes.GetDocumentTypes
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
      Me.DocumentTypesBindingSource.DataSource = _documentTypes


      Me.DocumentTypesBindingSource.RaiseListChangedEvents = True
      Me.DocumentTypesOptionsBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.DocumentTypesBindingSource.ResetBindings(False)
        Me.DocumentTypesOptionsBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub DocumentTypesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
  Handles DocumentTypesBindingSource.CurrentItemChanged, DocumentTypesOptionsBindingSource.CurrentItemChanged
    Try
      UxKopcinja1.cmdSave.Enabled = _documentTypes.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _documentTypes.IsDirty
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As DocumentType In _documentTypes
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

  Private Sub DocumentTypePrintsGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DocumentTypesGridControl.ProcessGridKey

    If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
      If _documentTypes.IsSavable Then
        GridView1.AddNewRow()
      Else
        e.SuppressKeyPress = True
      End If
    End If

  End Sub

End Class
