Public Class uxVehicleMakers

  Private WithEvents _vehicleMakers As VehicleMakers

#Region " KeyPress "

  Private Sub uxVehicleMakers_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxVehicleMakers

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxVehicleMakers

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.VehicleMakers.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.VehicleMakers.CanEditObject
    GridControl1.Enabled = VTE.Library.VehicleMakers.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.VehicleMakers.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.VehicleMakers.CanDeleteObject


  End Sub

  Private Sub uxVehicleMakers_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
            ApplyAuthorizationRules()

            _vehicleMakers = VehicleMakers.GetVehicleMakers
            If _vehicleMakers IsNot Nothing Then
                Me.VehicleMakersBindingSource.DataSource = _vehicleMakers
                Me.UxKopcinja1.cmdSave.Enabled = _vehicleMakers.IsSavable
                Me.UxKopcinja1.cmdCancel.Enabled = _vehicleMakers.IsDirty
            End If
            Me.CountriesListBindingSource.DataSource = CountriesList.GetCountriesList
       
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


  Private Sub uxVehicleMakers_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.VehicleMakersBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _vehicleMakers.Count <> 0 Then
          _vehicleMakers.RemoveAt(Me.VehicleMakersBindingSource.Position)
        End If
      Case "cmdExit"
        If _vehicleMakers.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VehicleMakers.CanEditObject Then
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
    Me.VehicleMakersBindingSource.RaiseListChangedEvents = False

    ' commit edits in memory
    UnbindBindingSource(Me.VehicleMakersBindingSource, saveObject, True)

    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _vehicleMakers = _vehicleMakers.Save
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
        _vehicleMakers = Nothing
        Try
          _vehicleMakers = VehicleMakers.GetVehicleMakers
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
      Me.VehicleMakersBindingSource.DataSource = _vehicleMakers

      Me.VehicleMakersBindingSource.RaiseListChangedEvents = True

      If rebind Then
        Me.VehicleMakersBindingSource.ResetBindings(False)

      End If

    End Try
  End Sub

  Private Sub VehicleMakersBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleMakersBindingSource.CurrentItemChanged
    Try
      UxKopcinja1.cmdSave.Enabled = _vehicleMakers.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _vehicleMakers.IsDirty

      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VehicleMaker In _vehicleMakers
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

  'Private Sub VehicleMakersGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.ProcessGridKey

  '  If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
  '    If _vehicleMakers.IsSavable Then
  '      GridView1.AddNewRow()
  '    Else
  '      e.SuppressKeyPress = True
  '    End If
  '  End If

  'End Sub


  Private Sub LookUpEditCountry_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditCountry.ButtonPressed
    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxCountries) Then
            par.ShowWinPart(CType(ctl, uxCountries))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxCountries)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If
  End Sub

  Private Sub GridView1_ColumnChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.FocusedColumnChanged
    If (GridView1.FocusedColumn Is colCompanyName) Or (GridView1.FocusedColumn Is colCompanyTrademark) Then
      System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    Else
      System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
    End If
  End Sub

  Private Sub LookUpEditCountry_ProcessNewValue(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles LookUpEditCountry.ProcessNewValue
            If e.DisplayValue.ToString = String.Empty Then
      e.Handled = False
      Exit Sub
    End If

    Try
      'Console.WriteLine("da")
      If Country.Exists(e.DisplayValue.ToString) = 0 Then
    If MsgBox(e.DisplayValue.ToString & " " & My.Resources.nePostoi, MsgBoxStyle.YesNo, My.Resources.Zacuvaj & "?") = MsgBoxResult.Yes Then

     Dim newC As Country = Country.NewCountry
     newC.CountryName = e.DisplayValue.ToString()
     newC.Citizenship = "непознато"
     newC.CountryShortName = (e.DisplayValue.ToString()).Substring(0, 3)
     newC.ApplyEdit()
     newC = newC.Save

     Me.CountriesListBindingSource.RaiseListChangedEvents = False

     Me.CountriesListBindingSource.DataSource = CountriesList.GetCountriesList()
     Me.CountriesListBindingSource.RaiseListChangedEvents = True
     Me.CountriesListBindingSource.ResetBindings(False)
    End If
        e.Handled = True
      End If
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

    e.Handled = True

  End Sub

End Class

