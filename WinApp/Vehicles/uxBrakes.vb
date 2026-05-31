Public Class uxBrakes

  Private WithEvents _brakes As VehicleBrakes

#Region " KeyPress "

  Private Sub uxCountries_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxBrakes

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxBrakes

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.VehicleBrakes.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.VehicleBrakes.CanEditObject
    VehicleBrakesGridControl.Enabled = VTE.Library.VehicleBrakes.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.VehicleBrakes.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.VehicleBrakes.CanDeleteObject


  End Sub

  Private Sub uxBrakes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _brakes = VehicleBrakes.GetVehicleBrakes
      If _brakes IsNot Nothing Then
        Me.VehicleBrakesBindingSource.DataSource = _brakes
        Me.UxKopcinja1.cmdSave.Enabled = _brakes.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _brakes.IsDirty
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
        Me.VehicleBrakesBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _brakes.Count <> 0 Then
          _brakes.RemoveAt(Me.VehicleBrakesBindingSource.Position)
        End If
      Case "cmdExit"
        If _brakes.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VehicleBrakes.CanEditObject Then
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
    Me.VehicleBrakesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.VehicleBrakesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
                    _brakes = _brakes.Save
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
        _brakes = Nothing
        Try
          _brakes = VehicleBrakes.GetVehicleBrakes
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
      Me.VehicleBrakesBindingSource.DataSource = _brakes

      Me.VehicleBrakesBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.VehicleBrakesBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub VehicleBrakesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleBrakesBindingSource.CurrentItemChanged
    Try
      If _brakes.IsSavable Then
        UxKopcinja1.cmdSave.Enabled = True
      Else
        UxKopcinja1.cmdSave.Enabled = False
      End If
      If _brakes.IsDirty Then
        UxKopcinja1.cmdCancel.Enabled = True
      Else
        UxKopcinja1.cmdCancel.Enabled = False
      End If
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VehicleBrake In _brakes
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

  'Private Sub VehicleBrakesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles VehicleBrakesGridControl.ProcessGridKey

  '  If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
  '    If _brakes.IsSavable Then
  '      GridView1.AddNewRow()
  '      GridView1.FocusedColumn = colBreakesCode
  '    Else
  '      e.SuppressKeyPress = True
  '    End If
  '  End If

  'End Sub

  'Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
  '  If _brakes.Count > 0 Then
  '    Dim det As VehicleBrake = _brakes.Item(Me.VehicleBrakesBindingSource.Position)
  '    For Each column As DevExpress.XtraGrid.Columns.GridColumn In GridView1.Columns
  '      column.OptionsColumn.ReadOnly = Not det.CanWriteProperty(column.FieldName)
  '    Next
  '  End If
  'End Sub
End Class

