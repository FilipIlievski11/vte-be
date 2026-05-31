Public Class uxVehicleSupporting

  Private WithEvents _VehicleSupporting As VehicleSupportings
#Region "PritisnatoKopce"

  Private Sub uxVehicleSupporting_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")

    End Select
  End Sub
#End Region

#Region "WinPart"

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxVehicleSupporting
  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxVehicleSupporting
  End Function

#End Region

  Private Sub ApplyAuthorizationRules()
    If Not VTE.Library.VehicleSupportings.CanGetObject Then
      Me.Close()
    End If
    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.VehicleSupportings.CanAddObject
    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.VehicleSupportings.CanDeleteObject
    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.VehicleSupportings.CanEditObject
  End Sub

  Private Sub uxColors_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()

      _VehicleSupporting = VehicleSupportings.GetVehicleSupportings
      If _VehicleSupporting IsNot Nothing Then
        Me.VehicleSupportingsBindingSource.DataSource = _VehicleSupporting
        Me.UxKopcinja1.cmdSave.Enabled = _VehicleSupporting.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _VehicleSupporting.IsDirty
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

  Private Sub uxVehicleSupporting_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name
      Case "cmdAdd"
        _VehicleSupporting.AddNew()
        GridView1.Focus()

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdDelete"
        If _VehicleSupporting.Count > 0 Then
          _VehicleSupporting.RemoveAt(VehicleSupportingsBindingSource.Position)
        End If

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdExit"
        If _VehicleSupporting.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VTE.Library.VehicleSupportings.CanEditObject Then
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
    Me.VehicleSupportingsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.VehicleSupportingsBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _VehicleSupporting = _VehicleSupporting.Save
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
        _VehicleSupporting = Nothing
        Try
          _VehicleSupporting = VehicleSupportings.GetVehicleSupportings
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
      Me.VehicleSupportingsBindingSource.DataSource = _VehicleSupporting
      Me.VehicleSupportingsBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.VehicleSupportingsBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub


  'Private Sub GridControl1_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.ProcessGridKey
  '  Select Case e.KeyCode
  '    Case Keys.Add, Keys.Oemplus
  '      VehicleSupportingsBindingSource.AddNew()
  '      GridView1.Focus()
  '  End Select
  'End Sub

  Private Sub VehicleSupportsBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleSupportingsBindingSource.CurrentItemChanged
    Try

      UxKopcinja1.cmdSave.Enabled = _VehicleSupporting.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _VehicleSupporting.IsDirty
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VTE.Library.VehicleSupporting In _VehicleSupporting
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


