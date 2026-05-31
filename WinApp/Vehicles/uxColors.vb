Public Class uxColors
  Private WithEvents _colors As Colors
  
#Region "PritisnatoKopce"

  Private Sub uxColors_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")

    End Select
  End Sub
#End Region

#Region "WinPart"

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxColors
  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxColors
  End Function

#End Region

  Private Sub ApplyAuthorizationRules()
    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.Colors.CanAddObject
    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.Colors.CanDeleteObject
    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.Colors.CanEditObject
  End Sub

  Private Sub uxColors_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
      _colors = Colors.GetColors

      If _colors IsNot Nothing Then
        Me.ColorsBindingSource.DataSource = _colors
        Me.UxKopcinja1.cmdSave.Enabled = _colors.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _colors.IsDirty
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

  Private Sub uxColors_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name
      Case "cmdAdd"
        _colors.AddNew()
        GridView1.Focus()

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdDelete"
        If _colors.Count > 0 Then
          _colors.RemoveAt(ColorsBindingSource.Position)
        End If

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdExit"
        If _colors.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VTE.Library.Colors.CanEditObject Then
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
    Me.ColorsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.ColorsBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
                    _colors = _colors.Save
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
        _colors = Nothing
        Try
          _colors = Colors.GetColors
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
      Me.ColorsBindingSource.DataSource = _colors
      Me.ColorsBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.ColorsBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

 
  'Private Sub GridControl1_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.ProcessGridKey
  '  Select Case e.KeyCode
  '    Case Keys.Add, Keys.Oemplus
  '      ColorsBindingSource.AddNew()
  '      GridView1.Focus()
  '  End Select
  'End Sub

  Private Sub ColorsBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ColorsBindingSource.CurrentItemChanged
    Try
      If _colors.IsSavable Then
        UxKopcinja1.cmdSave.Enabled = True
      Else
        UxKopcinja1.cmdSave.Enabled = False
      End If
      If _colors.IsDirty Then
        UxKopcinja1.cmdCancel.Enabled = True
      Else
        UxKopcinja1.cmdCancel.Enabled = False
      End If
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VTE.Library.Color In _colors
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
