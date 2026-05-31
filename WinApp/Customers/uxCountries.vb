Public Class uxCountries

  Private WithEvents _countries As Countries

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

    Return My.Resources.uxCountries

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxCountries

  End Function
#End Region



  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.Countries.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.Countries.CanEditObject
    CountriesGridControl.Enabled = VTE.Library.Countries.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.Countries.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.Countries.CanDeleteObject


  End Sub

  Private Sub uxCountries_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _countries = Countries.GetCountries
      If _countries IsNot Nothing Then
        Me.CountriesBindingSource.DataSource = _countries
        Me.UxKopcinja1.cmdSave.Enabled = _countries.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _countries.IsDirty
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


  Private Sub uxCountries_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.CountriesBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _countries.Count <> 0 Then
          _countries.RemoveAt(Me.CountriesBindingSource.Position)
        End If
      Case "cmdExit"
        If _countries.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If Countries.CanEditObject Then
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
    Me.CountriesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.CountriesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _countries = _countries.Save
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
        _countries = Nothing
        Try
          _countries = Countries.GetCountries
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
      Me.CountriesBindingSource.DataSource = _countries

      Me.CountriesBindingSource.RaiseListChangedEvents = True
      Me.CountriesBindingSource.ResetBindings(False)
    End Try
  End Sub

  Private Sub CountriesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CountriesBindingSource.CurrentItemChanged
    Try
      If _countries.IsSavable Then
        UxKopcinja1.cmdSave.Enabled = True
      Else
        UxKopcinja1.cmdSave.Enabled = False
      End If
      If _countries.IsDirty Then
        UxKopcinja1.cmdCancel.Enabled = True
      Else
        UxKopcinja1.cmdCancel.Enabled = False
      End If
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As Country In _countries
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

  Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)
    If _countries.Count > 0 Then
      Dim det As Country = _countries.Item(Me.CountriesBindingSource.Position)
      For Each column As DevExpress.XtraGrid.Columns.GridColumn In GridView1.Columns
        column.OptionsColumn.ReadOnly = Not det.CanWriteProperty(column.FieldName)
      Next
    End If
  End Sub


  Private Sub GridView1_InitNewRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs)
    Dim det As Country = _countries.Item(Me.CountriesBindingSource.Position)
    For Each column As DevExpress.XtraGrid.Columns.GridColumn In GridView1.Columns
      column.OptionsColumn.ReadOnly = Not det.CanWriteProperty(column.FieldName)
    Next
  End Sub

  Private Sub CountriesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

    If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
      If _countries.IsSavable Then
        GridView1.AddNewRow()
        GridView1.FocusedColumn = colCountryName
      Else
        e.SuppressKeyPress = True
      End If
    End If

  End Sub

  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    If GridView1.FocusedColumn IsNot Nothing Then
      If GridView1.FocusedColumn.Name = colCountryShortName.Name Then
        System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
        System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
      Else
        System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
        System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
      End If
    End If
  End Sub
End Class

