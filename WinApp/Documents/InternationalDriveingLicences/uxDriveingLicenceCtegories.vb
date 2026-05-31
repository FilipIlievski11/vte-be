Public Class uxDriveingLicenceCtegories

  Private WithEvents _driveingLicenceCtegories As DriveingLicenceCtegories

#Region " KeyPress "

  Private Sub uxDriveingLicenceCtegories_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxDriveingLicenceCtegories

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxDriveingLicenceCtegories

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.DriveingLicenceCtegories.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.DriveingLicenceCtegories.CanEditObject
    DriveingLicenceCtegoriesGridControl.Enabled = VTE.Library.DriveingLicenceCtegories.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.DriveingLicenceCtegories.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.DriveingLicenceCtegories.CanDeleteObject


  End Sub

  Private Sub uxDriveingLicenceCtegories_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _driveingLicenceCtegories = DriveingLicenceCtegories.GetDriveingLicenceCtegories
      If _driveingLicenceCtegories IsNot Nothing Then
        Me.DriveingLicenceCtegoriesBindingSource.DataSource = _driveingLicenceCtegories
        Me.UxKopcinja1.cmdSave.Enabled = _driveingLicenceCtegories.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _driveingLicenceCtegories.IsDirty
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


  Private Sub uxDriveingLicenceCtegories_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.DriveingLicenceCtegoriesBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _driveingLicenceCtegories.Count <> 0 Then
          _driveingLicenceCtegories.RemoveAt(Me.DriveingLicenceCtegoriesBindingSource.Position)
        End If
      Case "cmdExit"
        If _driveingLicenceCtegories.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If DriveingLicenceCtegories.CanEditObject Then
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
    Me.DriveingLicenceCtegoriesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.DriveingLicenceCtegoriesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _driveingLicenceCtegories = _driveingLicenceCtegories.Save
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
        _driveingLicenceCtegories = Nothing
        Try
          _driveingLicenceCtegories = DriveingLicenceCtegories.GetDriveingLicenceCtegories
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
      Me.DriveingLicenceCtegoriesBindingSource.DataSource = _driveingLicenceCtegories

      Me.DriveingLicenceCtegoriesBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.DriveingLicenceCtegoriesBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub DriveingLicenceCtegoriesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DriveingLicenceCtegoriesBindingSource.CurrentItemChanged
    Try
      If _driveingLicenceCtegories.IsSavable Then
        UxKopcinja1.cmdSave.Enabled = True
      Else
        UxKopcinja1.cmdSave.Enabled = False
      End If
      If _driveingLicenceCtegories.IsDirty Then
        UxKopcinja1.cmdCancel.Enabled = True
      Else
        UxKopcinja1.cmdCancel.Enabled = False
      End If
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As DriveingLicenceCtegory In _driveingLicenceCtegories
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

 'Private Sub DriveingLicenceCtegoriesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DriveingLicenceCtegoriesGridControl.ProcessGridKey

 '  If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
 '    If _driveingLicenceCtegories.IsSavable Then
 '      GridView1.AddNewRow()
 '      GridView1.FocusedColumn = colCode
 '    Else
 '      e.SuppressKeyPress = True
 '    End If
 '  End If

 'End Sub

  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    If e.FocusedColumn Is colCode Then
      System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    Else
      System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
    End If
  End Sub
End Class
