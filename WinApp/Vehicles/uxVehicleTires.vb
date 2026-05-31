Public Class uxVehicleTires


  Private WithEvents _VehicleTires As VehicleTireTypes

#Region " KeyPress "

  Private Sub uxVehicleTires_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxVehicleTireTypes

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxVehicleTireTypes

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.VehicleTireTypes.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.VehicleTireTypes.CanEditObject
    VehicleTireTypesGridControl.Enabled = VTE.Library.VehicleTireTypes.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.VehicleTireTypes.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.VehicleTireTypes.CanDeleteObject


  End Sub

  Private Sub uxBrakes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _VehicleTires = VehicleTireTypes.GetVehicleTireTypes
      If _VehicleTires IsNot Nothing Then
        Me.VehicleTireTypesBindingSource.DataSource = _VehicleTires
        Me.UxKopcinja1.cmdSave.Enabled = _VehicleTires.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _VehicleTires.IsDirty
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


  Private Sub uxBrakes_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.VehicleTireTypesBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _VehicleTires.Count <> 0 Then
          _VehicleTires.RemoveAt(Me.VehicleTireTypesBindingSource.Position)
        End If
      Case "cmdExit"
        If _VehicleTires.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VehicleTireTypes.CanEditObject Then
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
    Me.VehicleTireTypesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.VehicleTireTypesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _VehicleTires = _VehicleTires.Save
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
        _VehicleTires = Nothing
        Try
          _VehicleTires = VehicleTireTypes.GetVehicleTireTypes
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
      Me.VehicleTireTypesBindingSource.DataSource = _VehicleTires

      Me.VehicleTireTypesBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.VehicleTireTypesBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub VehicleTireTypesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleTireTypesBindingSource.CurrentItemChanged
    Try
      If _VehicleTires.IsSavable Then
        UxKopcinja1.cmdSave.Enabled = True
      Else
        UxKopcinja1.cmdSave.Enabled = False
      End If
      If _VehicleTires.IsDirty Then
        UxKopcinja1.cmdCancel.Enabled = True
      Else
        UxKopcinja1.cmdCancel.Enabled = False
      End If
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VehicleTireType In _VehicleTires
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
