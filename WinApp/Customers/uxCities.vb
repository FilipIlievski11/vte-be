Public Class uxCities
  Private WithEvents _cities As Cities

#Region " KeyPress "



  Private Sub uxCities_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxCities

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxCities

  End Function
#End Region

  Private Sub LoadList()
    _cities = Cities.GetCities
    If _cities IsNot Nothing Then
      Me.CitiesBindingSource.DataSource = _cities
    End If
    Me.CountriesListBindingSource.DataSource = CountriesList.GetCountriesList
    Me.CommunitiesListBindingSource.DataSource = CommunitiesList.GetCommunitiesList
  End Sub

  Private Sub ApplyAuthorizationRules()

    'kontroli eanble/disable
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.Cities.CanGetObject Then
      Me.Close()
    End If

    If VTE.Library.Cities.CanEditObject Then
      Me.UxKopcinja1.cmdSave.Enabled = True
      CitiesGridControl.Enabled = True
    Else
      Me.UxKopcinja1.cmdSave.Enabled = False
      CitiesGridControl.Enabled = False
    End If
    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.Cities.CanAddObject


    If VTE.Library.Cities.CanDeleteObject Then
      Me.UxKopcinja1.cmdDelete.Enabled = True
    Else
      Me.UxKopcinja1.cmdDelete.Enabled = False
    End If

  End Sub

  Private Sub uxCities_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    ApplyAuthorizationRules()
    LoadList()
    UxKopcinja1.cmdAdd.Focus()

  End Sub

  Private Sub uxCities_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdAdd"
        Try
          Me.CitiesBindingSource.AddNew()
          GridView1.Focus()
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
        '_cities.Item(Me.CitiesBindingSource.Position).CommunityCode = 8
      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()
      Case "cmdDelete"
        Me.CitiesBindingSource.RemoveCurrent()
      Case "cmdCancel"
        RebindUI(False, True)
      Case "cmdExit"
        If _cities.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If Cities.CanEditObject Then
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
    Me.CitiesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.CitiesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _cities = _cities.Save
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
        _cities = Nothing
        Try
          _cities = Cities.GetCities
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
      Me.CitiesBindingSource.DataSource = _cities

      Me.CitiesBindingSource.RaiseListChangedEvents = True
      Me.CitiesBindingSource.ResetBindings(False)
    End Try
  End Sub

  Private Sub BindUI()
    _cities.BeginEdit()
    Me.CitiesBindingSource.DataSource = _cities
  End Sub

  Private Sub CitiesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CitiesBindingSource.CurrentItemChanged
    If _cities.IsSavable Then
      UxKopcinja1.cmdSave.Enabled = True
    Else
      UxKopcinja1.cmdSave.Enabled = False
    End If
    If _cities.IsDirty Then
      UxKopcinja1.cmdCancel.Enabled = True
    Else
      UxKopcinja1.cmdCancel.Enabled = False
    End If
    Dim message As New System.Text.StringBuilder
    message.AppendFormat("{0}" + vbCrLf, "")
    For Each child As City In _cities
      For Each rule As Csla.Validation.BrokenRule In _
            child.BrokenRulesCollection
        message.AppendFormat( _
          "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
      Next
    Next
    ShowBrokenRules(message.ToString)
  End Sub
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


  Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
    If _cities.Count > 0 Then
      Dim det As City = _cities.Item(Me.CitiesBindingSource.Position)
      For Each column As DevExpress.XtraGrid.Columns.GridColumn In GridView1.Columns
        column.OptionsColumn.ReadOnly = Not det.CanWriteProperty(column.FieldName)
      Next
    End If
  End Sub

  Private Sub GridView1_InitNewRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles GridView1.InitNewRow
    Dim det As City = _cities.Item(Me.CitiesBindingSource.Position)
    For Each column As DevExpress.XtraGrid.Columns.GridColumn In GridView1.Columns
      column.OptionsColumn.ReadOnly = Not det.CanWriteProperty(column.FieldName)
    Next
  End Sub
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

  Private Sub LookUpEditCummunity_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles _
   LookUpEditCummunity.ButtonPressed
    If e.Button.Index = 1 Then
      If e.Button.Enabled Then
        Dim par As MainForm = Me.ParentForm
        For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
          For Each ctl As Control In page.Controls
            If (TypeOf ctl Is uxCommunities) Then
              par.ShowWinPart(CType(ctl, uxCommunities))
              Exit Sub
            End If
          Next
        Next
        Using cekaj As New StatusBusy(My.Resources.txtLoading)
          Try
            par.AddWinPart(New uxCommunities)
          Catch ex As Exception
            MsgBox(ex.Message)
          End Try
        End Using
      End If
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
