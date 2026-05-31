Public Class uxVehicleUse

  Private WithEvents _vehicleUses As VehicleUses
#Region "PritisnatoKopce"

  Private Sub uxVehicleUses_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")

    End Select
  End Sub
#End Region

#Region "WinPart"

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxVehicleUses
  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxVehicleUses
  End Function

#End Region

  Private Sub ApplyAuthorizationRules()
    If Not VTE.Library.VehicleUses.CanGetObject Then
      Me.Close()
    End If
    Me.UxKopcinja2.cmdAdd.Enabled = VTE.Library.VehicleUses.CanAddObject
    Me.UxKopcinja2.cmdDelete.Enabled = VTE.Library.VehicleUses.CanDeleteObject
    Me.UxKopcinja2.cmdSave.Enabled = VTE.Library.VehicleUses.CanEditObject
  End Sub

  Private Sub uxVehicleUses_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
      _vehicleUses = VehicleUses.GetVehicleUses
      If _vehicleUses IsNot Nothing Then
        Me.VehicleUsesBindingSource1.DataSource = _vehicleUses
        Me.UxKopcinja2.cmdSave.Enabled = _vehicleUses.IsSavable
        Me.UxKopcinja2.cmdCancel.Enabled = _vehicleUses.IsDirty
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

  Private Sub uxVehicleUses_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja2_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja2.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name
      Case "cmdAdd"
        _vehicleUses.AddNew()
        GridView3.Focus()

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja2.cmdAdd.Focus()

      Case "cmdDelete"
        If _vehicleUses.Count > 0 Then
          _vehicleUses.RemoveAt(VehicleUsesBindingSource1.Position)
        End If

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdExit"
        If _vehicleUses.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VTE.Library.VehicleUses.CanEditObject Then
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
    Me.VehicleUsesBindingSource1.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.VehicleUsesBindingSource1, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _vehicleUses = _vehicleUses.Save
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
        _vehicleUses = Nothing
        Try
          _vehicleUses = VehicleUses.GetVehicleUses
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
      Me.VehicleUsesBindingSource1.DataSource = _vehicleUses
      Me.VehicleUsesBindingSource1.RaiseListChangedEvents = True
      If rebind Then
        Me.VehicleUsesBindingSource1.ResetBindings(False)
      End If

    End Try
  End Sub


  'Private Sub VehicleUsesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles VehicleUsesGridControl1.ProcessGridKey
  '  Select Case e.KeyCode
  '    Case Keys.Add, Keys.Oemplus
  '      VehicleUsesBindingSource1.AddNew()
  '      GridView3.Focus()
  '  End Select
  'End Sub

  Private Sub VehicleSupportsBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleUsesBindingSource1.CurrentItemChanged
    Try

      UxKopcinja2.cmdSave.Enabled = _vehicleUses.IsSavable
      UxKopcinja2.cmdCancel.Enabled = _vehicleUses.IsDirty
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VTE.Library.VehicleUse In _vehicleUses
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
