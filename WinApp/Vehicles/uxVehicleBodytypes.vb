Public Class uxVehicleBodytypes

  Private WithEvents _vehicleBodytypes As VehicleBodytypes

#Region " KeyPress "

  Private Sub uxVehicleBodytypes_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

        Return My.Resources.uxVehicleBodytypes

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxVehicleBodytypes

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()
    System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    If Not VTE.Library.VehicleBodytypes.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.VehicleBodytypes.CanEditObject
    GridControl1.Enabled = VTE.Library.VehicleBodytypes.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.VehicleBodytypes.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.VehicleBodytypes.CanDeleteObject


  End Sub

  Private Sub uxuxVehicleBodytypes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _vehicleBodytypes = VehicleBodytypes.GetVehicleBodytypes
      If _vehicleBodytypes IsNot Nothing Then
        Me.VehicleBodytypesBindingSource.DataSource = _vehicleBodytypes
        Me.UxKopcinja1.cmdSave.Enabled = _vehicleBodytypes.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _vehicleBodytypes.IsDirty
      End If

      Me.VehicleCategoryListBindingSource.DataSource = VehicleCategoryList.GetVehicleCategoryList
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


  Private Sub uxVehicleBodytypes_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.VehicleBodytypesBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _vehicleBodytypes.Count <> 0 Then
          _vehicleBodytypes.RemoveAt(Me.VehicleBodytypesBindingSource.Position)
        End If
      Case "cmdExit"
        If _vehicleBodytypes.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VehicleBrakes.CanEditObject Then
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
    Me.VehicleBodytypesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.VehicleBodytypesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _vehicleBodytypes = _vehicleBodytypes.Save
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
        _vehicleBodytypes = Nothing
        Try
          _vehicleBodytypes = VehicleBodytypes.GetVehicleBodytypes
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
      Me.VehicleBodytypesBindingSource.DataSource = _vehicleBodytypes

      Me.VehicleBodytypesBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.VehicleBodytypesBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub VehicleCategoriesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleBodytypesBindingSource.CurrentItemChanged
    Try
      UxKopcinja1.cmdSave.Enabled = _vehicleBodytypes.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _vehicleBodytypes.IsDirty
      
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VehicleBodytype In _vehicleBodytypes
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

  Private Sub VehicleBrakesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.ProcessGridKey

    If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
      If _vehicleBodytypes.IsSavable Then
        GridView1.AddNewRow()
        'GridView1.FocusedColumn = colBodytypeCode
      Else
        e.SuppressKeyPress = True
      End If
    End If

  End Sub
  Private Sub LookUpEditCategory_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditCategory.ButtonPressed
    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxVehicleCategories) Then
            par.ShowWinPart(CType(ctl, uxVehicleCategories))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxVehicleCategories)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If
  End Sub
End Class

