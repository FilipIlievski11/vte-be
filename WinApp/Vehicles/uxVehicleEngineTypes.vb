Public Class uxVehicleEngineTypes

  Private WithEvents _VehicleEngineType As VehicleEngineTypes
  Private WithEvents _vehicleMakersList As VehicleMakerList
  Private WithEvents _enginePowerSources As VehicleEnginePowerSourceTypeList

#Region "PritisnatoKopce"

  Private Sub uxVehicleEngineTypes_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")

    End Select
  End Sub
#End Region

#Region "WinPart"

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxVehicleEngineTypes
  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxVehicleEngineTypes
  End Function

#End Region

  Private Sub ApplyAuthorizationRules()
    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.VehicleEngineTypes.CanAddObject
    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.VehicleEngineTypes.CanDeleteObject
    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.VehicleEngineTypes.CanEditObject
  End Sub

  Private Sub uxVehicleEngineTypes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _VehicleEngineType = VehicleEngineTypes.GetVehicleEngineTypes
      _vehicleMakersList = VehicleMakerList.GetVehicleMakerList
      _enginePowerSources = VehicleEnginePowerSourceTypeList.GetVehicleEnginePowerSourceTypeList
      Me.VehicleEnginePowerSourceTypeListBindingSource.DataSource = _enginePowerSources
      Me.VehicleMakerListBindingSource.DataSource = _vehicleMakersList
      If _VehicleEngineType IsNot Nothing Then
        Me.VehicleEngineTypesBindingSource.DataSource = _VehicleEngineType
        Me.UxKopcinja1.cmdSave.Enabled = _VehicleEngineType.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _VehicleEngineType.IsDirty
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

  Private Sub uxVehicleEngineTypes_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name
      Case "cmdAdd"
        _VehicleEngineType.AddNew()
        GridView1.Focus()

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdDelete"
        If _VehicleEngineType.Count > 0 Then
          _VehicleEngineType.RemoveAt(VehicleEngineTypesBindingSource.Position)
        End If

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdExit"
        If _VehicleEngineType.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VTE.Library.VehicleEngineTypes.CanEditObject Then
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
    Me.VehicleEngineTypesBindingSource.RaiseListChangedEvents = False

    ' commit edits in memory
    UnbindBindingSource(Me.VehicleEngineTypesBindingSource, saveObject, True)

    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _VehicleEngineType = _VehicleEngineType.Save
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
        _VehicleEngineType = Nothing
        Try
          _VehicleEngineType = VehicleEngineTypes.GetVehicleEngineTypes
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
      Me.VehicleEngineTypesBindingSource.DataSource = _VehicleEngineType
      Me.VehicleEngineTypesBindingSource.RaiseListChangedEvents = True

      If rebind Then
        Me.VehicleEngineTypesBindingSource.ResetBindings(False)

      End If

    End Try
  End Sub


  'Private Sub GridControl1_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.ProcessGridKey
  '  Select Case e.KeyCode
  '    Case Keys.Add, Keys.Oemplus
  '      VehicleEngineTypesBindingSource.AddNew()
  '      GridView1.Focus()
  '  End Select
  'End Sub

  Private Sub VehicleEngineTypesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleEngineTypesBindingSource.CurrentItemChanged
    Try
      If _VehicleEngineType.IsSavable Then
        UxKopcinja1.cmdSave.Enabled = True
      Else
        UxKopcinja1.cmdSave.Enabled = False
      End If
      If _VehicleEngineType.IsDirty Then
        UxKopcinja1.cmdCancel.Enabled = True
      Else
        UxKopcinja1.cmdCancel.Enabled = False
      End If
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VTE.Library.VehicleEngineType In _VehicleEngineType
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

  Private Sub LookUpEditPowerSource_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditPowerSource.ButtonPressed
    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxVehicleEnginePowerSourceTypes) Then
            par.ShowWinPart(CType(ctl, uxVehicleEnginePowerSourceTypes))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxVehicleEnginePowerSourceTypes)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If
  End Sub

  'Private Sub LookUpEditMaker_ProcessNewValue(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles LookUpEditMaker.ProcessNewValue
  '  If e.DisplayValue.ToString = String.Empty Then
  '    e.Handled = False
  '    Exit Sub
  '  End If

  '  Try
  '    'Console.WriteLine("da")
  '    If VehicleMaker.Exists(e.DisplayValue.ToString) = 0 Then
  '      If MsgBox(e.DisplayValue.ToString & " " & My.Resources.nePostoi, MsgBoxStyle.YesNo, "Зачувај?") = MsgBoxResult.Yes Then

  '        Dim newC As VehicleMaker = VehicleMaker.NewVehicleMaker
  '        newC.CompanyName = e.DisplayValue.ToString()
  '        newC.ApplyEdit()
  '        newC = newC.Save

  '        Me.VehicleMakerListBindingSource.RaiseListChangedEvents = False

  '        Me.VehicleMakerListBindingSource.DataSource = VehicleMakerList.GetVehicleMakerList()
  '        Me.VehicleMakerListBindingSource.RaiseListChangedEvents = True
  '        Me.VehicleMakerListBindingSource.ResetBindings(False)
  '      End If
  '      e.Handled = True
  '    End If
  '  Catch ex As Exception
  '    MsgBox(ex.Message)
  '  End Try

  '  e.Handled = True
  'End Sub
  Private AllowShowEditor As Boolean = True
  Private Sub GridView1_FocusedRowChanged1(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
    AllowShowEditor = False
    BeginInvoke(New MethodInvoker(AddressOf tr))
  End Sub
  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    AllowShowEditor = False
    BeginInvoke(New MethodInvoker(AddressOf tr))
  End Sub
  Private Sub GridView1_ShowingEditor1(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles GridView1.ShowingEditor
    e.Cancel = Not AllowShowEditor
  End Sub

  Private Sub tr()
    AllowShowEditor = True
  End Sub

  Private Sub LookUpEditPowerSource_ProcessNewValue(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles LookUpEditPowerSource.ProcessNewValue
    If e.DisplayValue.ToString = String.Empty Then
      e.Handled = False
      Exit Sub
    End If

    Try
      'Console.WriteLine("da")
      If VehicleEnginePowerSourceType.Exists(e.DisplayValue.ToString) = 0 Then
    If MsgBox(e.DisplayValue.ToString & " " & My.Resources.nePostoi, MsgBoxStyle.YesNo, My.Resources.Zacuvaj & "?") = MsgBoxResult.Yes Then

     Dim newC As VehicleEnginePowerSourceType = VehicleEnginePowerSourceType.NewVehicleEnginePowerSourceType
     newC.PowerSourceName = e.DisplayValue.ToString()
     newC.ApplyEdit()
     newC = newC.Save

     Me.VehicleEnginePowerSourceTypeListBindingSource.RaiseListChangedEvents = False

     Me.VehicleEnginePowerSourceTypeListBindingSource.DataSource = VehicleEnginePowerSourceTypeList.GetVehicleEnginePowerSourceTypeList()
     Me.VehicleEnginePowerSourceTypeListBindingSource.RaiseListChangedEvents = True
     Me.VehicleEnginePowerSourceTypeListBindingSource.ResetBindings(False)
    End If
        e.Handled = True
      End If
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

    e.Handled = True
  End Sub

  Private Sub LookUpEditMaker_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditMaker.ButtonClick
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

  Private Sub GridView1_ColumnChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.FocusedColumnChanged
    If (GridView1.FocusedColumn Is colIdVehicleMaker) Or (GridView1.FocusedColumn Is colEngineType) Then
      System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    Else
      System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
    End If
  End Sub
End Class
