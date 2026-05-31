Public Class uxVehicleGearBoxes

  Private WithEvents _GearBoxes As VehicleGearBoxes

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

    Return My.Resources.uxVehicleGearBoxes

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxVehicleGearBoxes

  End Function
#End Region

  Private Sub LoadList()
    _GearBoxes = VehicleGearBoxes.GetVehicleGearBoxes
  End Sub

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.VehicleGearBoxes.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.VehicleGearBoxes.CanEditObject
    VehicleGearBoxesGridControl.Enabled = VTE.Library.VehicleGearBoxes.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.VehicleGearBoxes.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.VehicleGearBoxes.CanDeleteObject


  End Sub

  Private Sub uxVehicleGearBoxes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
            System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
      _GearBoxes = VehicleGearBoxes.GetVehicleGearBoxes
      If _GearBoxes IsNot Nothing Then
        Me.VehicleGearBoxesBindingSource.DataSource = _GearBoxes
        Me.UxKopcinja1.cmdSave.Enabled = _GearBoxes.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _GearBoxes.IsDirty
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


  Private Sub uxVehicleGearBoxes_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.VehicleGearBoxesBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, False)

      Case "cmdDelete"
        If _GearBoxes.Count <> 0 Then
          _GearBoxes.RemoveAt(Me.VehicleGearBoxesBindingSource.Position)
        End If
      Case "cmdExit"
        If _GearBoxes.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VehicleGearBoxes.CanEditObject Then
                RebindUI(True, False)
                Me.Close()
              Else
                If MsgBox("Немате дозвола го запамтите објектот, дали сакате да излезете", _
                  MsgBoxStyle.YesNo, "излез") = MsgBoxResult.Yes Then
                  Me.Close()
                End If
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
    Me.VehicleGearBoxesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.VehicleGearBoxesBindingSource, True, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _GearBoxes = _GearBoxes.Save
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
        _GearBoxes = Nothing
        _GearBoxes = VehicleGearBoxes.GetVehicleGearBoxes
      End If


    Finally
      Me.VehicleGearBoxesBindingSource.DataSource = _GearBoxes

      Me.VehicleGearBoxesBindingSource.RaiseListChangedEvents = True
      Me.VehicleGearBoxesBindingSource.ResetBindings(False)
    End Try
  End Sub

  Private Sub VehicleGearBoxesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleGearBoxesBindingSource.CurrentItemChanged
    Try

      UxKopcinja1.cmdSave.Enabled = _GearBoxes.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _GearBoxes.IsDirty
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VehicleGearBox In _GearBoxes
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

  Private Sub GridView1_InitNewRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles GridView1.InitNewRow
    Dim det As VehicleGearBox = _GearBoxes.Item(Me.VehicleGearBoxesBindingSource.Position)
    For Each column As DevExpress.XtraGrid.Columns.GridColumn In GridView1.Columns
      column.OptionsColumn.ReadOnly = Not det.CanWriteProperty(column.FieldName)
    Next
  End Sub

  'Private Sub VehicleBrakesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles VehicleGearBoxesGridControl.ProcessGridKey

  '  If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
  '    If _GearBoxes.IsSavable Then
  '      GridView1.AddNewRow()
  '      GridView1.FocusedColumn = colGearBoxCode
  '    Else
  '      e.SuppressKeyPress = True
  '    End If
  '  End If

  'End Sub

  Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
    If _GearBoxes.Count > 0 Then
      Dim det As VehicleGearBox = _GearBoxes.Item(Me.VehicleGearBoxesBindingSource.Position)
      For Each column As DevExpress.XtraGrid.Columns.GridColumn In GridView1.Columns
        column.OptionsColumn.ReadOnly = Not det.CanWriteProperty(column.FieldName)
      Next
    End If
  End Sub
End Class

