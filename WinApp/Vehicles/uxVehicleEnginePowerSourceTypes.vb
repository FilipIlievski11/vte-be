Public Class uxVehicleEnginePowerSourceTypes


  Private WithEvents _VehicleEnginePowerSourceType As VehicleEnginePowerSourceTypes
#Region "PritisnatoKopce"

  Private Sub uxVehicleEnginePowerSourceType_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")

    End Select
  End Sub
#End Region

#Region "WinPart"

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxVehicleEnginePowerSourceTypes
  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxVehicleEnginePowerSourceTypes
  End Function

#End Region

  Private Sub ApplyAuthorizationRules()
    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.VehicleEnginePowerSourceTypes.CanAddObject
    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.VehicleEnginePowerSourceTypes.CanDeleteObject
    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.VehicleEnginePowerSourceTypes.CanEditObject
  End Sub

  Private Sub uxVehicleEnginePowerSourceTypes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _VehicleEnginePowerSourceType = VehicleEnginePowerSourceTypes.GetVehicleEnginePowerSourceTypes
      If _VehicleEnginePowerSourceType IsNot Nothing Then
        Me.VehicleEnginePowerSourceTypesBindingSource.DataSource = _VehicleEnginePowerSourceType
        Me.UxKopcinja1.cmdSave.Enabled = _VehicleEnginePowerSourceType.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _VehicleEnginePowerSourceType.IsDirty
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

  Private Sub uxVehicleEnginePowerSourceTypes_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name
      Case "cmdAdd"
        _VehicleEnginePowerSourceType.AddNew()
        GridView1.Focus()

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdDelete"
        If _VehicleEnginePowerSourceType.Count > 0 Then
          _VehicleEnginePowerSourceType.RemoveAt(VehicleEnginePowerSourceTypesBindingSource.Position)
        End If

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdExit"
        If _VehicleEnginePowerSourceType.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VTE.Library.VehicleEnginePowerSourceTypes.CanEditObject Then
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
    Me.VehicleEnginePowerSourceTypesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.VehicleEnginePowerSourceTypesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _VehicleEnginePowerSourceType = _VehicleEnginePowerSourceType.Save
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
        _VehicleEnginePowerSourceType = Nothing
        Try
          _VehicleEnginePowerSourceType = VehicleEnginePowerSourceTypes.GetVehicleEnginePowerSourceTypes
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
      Me.VehicleEnginePowerSourceTypesBindingSource.DataSource = _VehicleEnginePowerSourceType
      Me.VehicleEnginePowerSourceTypesBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.VehicleEnginePowerSourceTypesBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub


  'Private Sub GridControl1_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.ProcessGridKey
  '  Select Case e.KeyCode
  '    Case Keys.Add, Keys.Oemplus
  '      VehicleEnginePowerSourceTypesBindingSource.AddNew()
  '      GridView1.Focus()
  '  End Select
  'End Sub

  Private Sub VehicleEnginePowerSourceTypesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleEnginePowerSourceTypesBindingSource.CurrentItemChanged
    Try
      UxKopcinja1.cmdSave.Enabled = _VehicleEnginePowerSourceType.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _VehicleEnginePowerSourceType.IsDirty
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VTE.Library.VehicleEnginePowerSourceType In _VehicleEnginePowerSourceType
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
