Public Class uxVehicleModels

  Private WithEvents _vehicleModels As VehicleModels

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

    Return My.Resources.uxVehicleModels

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxVehicleModels

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.VehicleModels.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.VehicleModels.CanEditObject
    GridControl1.Enabled = VTE.Library.VehicleModels.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.VehicleModels.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.VehicleModels.CanDeleteObject


  End Sub
   
  Private Sub uxVehicleModels_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try


      _vehicleModels = VehicleModels.GetVehicleModels
      If _vehicleModels IsNot Nothing Then
        Me.VehicleModelsBindingSource.DataSource = _vehicleModels
        'Me.VehicleTireTypesBindingSource.DataSource = _vehicleModels
        Me.UxKopcinja1.cmdSave.Enabled = _vehicleModels.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _vehicleModels.IsDirty
      End If
      Me.VehicleMakerListBindingSource.DataSource = VehicleMakerList.GetVehicleMakerList
      Me.VehicleBodytypeListBindingSource.DataSource = VehicleBodytypeList.GetVehicleBodytypeList
    Catch ex As Csla.DataPortalException
      MessageBox.Show(ex.BusinessException.ToString, _
        "Error loading", MessageBoxButtons.OK, _
        MessageBoxIcon.Exclamation)

    Catch ex As Exception
      MessageBox.Show(ex.ToString, _
        "Error loading", MessageBoxButtons.OK, _
        MessageBoxIcon.Exclamation)
    End Try

        System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    ApplyAuthorizationRules()

  End Sub


  Private Sub uxVehicleModels_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.VehicleModelsBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _vehicleModels.Count <> 0 Then
          _vehicleModels.RemoveAt(Me.VehicleModelsBindingSource.Position)
        End If
      Case "cmdExit"
        If _vehicleModels.IsDirty Then
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

    Me.VehicleModelsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory

    UnbindBindingSource(Me.VehicleModelsBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _vehicleModels = _vehicleModels.Save
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
        _vehicleModels = Nothing
        Try
          _vehicleModels = VehicleModels.GetVehicleModels
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
      Me.VehicleModelsBindingSource.DataSource = _vehicleModels

      Me.VehicleModelsBindingSource.RaiseListChangedEvents = True

      If rebind Then
        Me.VehicleModelsBindingSource.ResetBindings(False)

      End If

    End Try
  End Sub

  Private Sub VehicleModelsBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
  Handles VehicleModelsBindingSource.CurrentItemChanged
    Try
      UxKopcinja1.cmdSave.Enabled = _vehicleModels.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _vehicleModels.IsDirty

      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VehicleModel In _vehicleModels
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

  'Private Sub VehicleModelsGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.ProcessGridKey

  '  If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
  '    If _vehicleModels.IsSavable Then
  '      GridView1.AddNewRow()
  '      'GridView1.FocusedColumn = colBodytypeCode
  '    Else
  '      e.SuppressKeyPress = True
  '    End If
  '  End If

  'End Sub


  Private Sub LookUpEditBodytype_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditBodytype.ButtonPressed
    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxVehicleBodytypes) Then
            par.ShowWinPart(CType(ctl, uxVehicleBodytypes))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxVehicleBodytypes)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If
  End Sub
  Private AllowShowEditor As Boolean = True

  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
        AllowShowEditor = False
        BeginInvoke(New MethodInvoker(AddressOf tr))

            System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    End Sub

  Private Sub GridView1_FocusedRowChanged1(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        AllowShowEditor = False
        BeginInvoke(New MethodInvoker(AddressOf tr))
    End Sub



  Private Sub GridView1_ShowingEditor1(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles GridView1.ShowingEditor
        e.Cancel = Not AllowShowEditor
  End Sub

  Private Sub tr()
    AllowShowEditor = True
  End Sub



  Private Sub LookUpEditVehicleMakers_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditVehicleMakers.ButtonPressed
    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxVehicleMakers) Then
            par.ShowWinPart(CType(ctl, uxVehicleMakers))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxVehicleMakers)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If
  End Sub

   
End Class
