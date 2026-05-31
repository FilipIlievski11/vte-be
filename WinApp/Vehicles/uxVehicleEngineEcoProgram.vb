Public Class uxVehicleEngineEcoProgram

  Private WithEvents _vehicleEngineEcoPrograms As VehicleEngineEcoPrograms

#Region " KeyPress "

  Private Sub uxVehicleEngineEcoProgram_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxVehicleEngineEcoProgram

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxVehicleEngineEcoProgram

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.VehicleEngineEcoPrograms.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.VehicleEngineEcoPrograms.CanEditObject
    Me.UxKopcinja1.cmdCancel.Enabled = VTE.Library.VehicleEngineEcoPrograms.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.VehicleEngineEcoPrograms.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.VehicleEngineEcoPrograms.CanDeleteObject


  End Sub

  Private Sub uxVehicleEngineEcoProgram_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _vehicleEngineEcoPrograms = VehicleEngineEcoPrograms.GetVehicleEngineEcoPrograms
      If _vehicleEngineEcoPrograms IsNot Nothing Then
        Me.VehicleEngineEcoProgramsBindingSource.DataSource = _vehicleEngineEcoPrograms
        Me.UxKopcinja1.cmdSave.Enabled = _vehicleEngineEcoPrograms.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _vehicleEngineEcoPrograms.IsDirty
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


  Private Sub uxVehicleEngineEcoProgram_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.VehicleEngineEcoProgramsBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _vehicleEngineEcoPrograms.Count <> 0 Then
          _vehicleEngineEcoPrograms.RemoveAt(Me.VehicleEngineEcoProgramsBindingSource.Position)
        End If
      Case "cmdExit"
        If _vehicleEngineEcoPrograms.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VehicleEngineEcoPrograms.CanEditObject Then
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
    Me.VehicleEngineEcoProgramsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.VehicleEngineEcoProgramsBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _vehicleEngineEcoPrograms = _vehicleEngineEcoPrograms.Save

          objVehicleEngineEcoProgramList = VehicleEngineEcoProgramList.GetVehicleEngineEcoProgramList
          If Csla.ApplicationContext.LocalContext.Contains("objVehicleEngineEcoProgramList") Then
            Csla.ApplicationContext.LocalContext.Remove("objVehicleEngineEcoProgramList")
          End If
          Csla.ApplicationContext.LocalContext.Add("objVehicleEngineEcoProgramList", objVehicleEngineEcoProgramList)
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
        _vehicleEngineEcoPrograms = Nothing
        Try
          _vehicleEngineEcoPrograms = VehicleEngineEcoPrograms.GetVehicleEngineEcoPrograms
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
      Me.VehicleEngineEcoProgramsBindingSource.DataSource = _vehicleEngineEcoPrograms

      Me.VehicleEngineEcoProgramsBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.VehicleEngineEcoProgramsBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub VehicleEngineEcoProgramsBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleEngineEcoProgramsBindingSource.CurrentItemChanged
    Try
      If _vehicleEngineEcoPrograms.IsSavable Then
        UxKopcinja1.cmdSave.Enabled = True
      Else
        UxKopcinja1.cmdSave.Enabled = False
      End If
      If _vehicleEngineEcoPrograms.IsDirty Then
        UxKopcinja1.cmdCancel.Enabled = True
      Else
        UxKopcinja1.cmdCancel.Enabled = False
      End If
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VehicleEngineEcoProgram In _vehicleEngineEcoPrograms
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

  'Private Sub VehicleEngineEcoProgramsGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles VehicleEngineEcoProgramsGridControl.ProcessGridKey

  '  If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
  '    If _vehicleEngineEcoPrograms.IsSavable Then
  '      GridView1.AddNewRow()
  '    Else
  '      e.SuppressKeyPress = True
  '    End If
  '  End If

  'End Sub

  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    If GridView1.FocusedColumn Is colEcoProgram Then
      System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    Else
      System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
    End If
  End Sub
End Class

