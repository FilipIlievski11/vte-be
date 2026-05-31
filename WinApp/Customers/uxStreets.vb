Public Class uxStreets
  Private WithEvents _streets As Streets

  Private Sub uxStreets_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    ApplyAuthorizationRules()
    _streets = Streets.GetStreets
    If _streets IsNot Nothing Then
      Me.StreetsBindingSource.DataSource = _streets
    End If
    UxKopcinja1.cmdAdd.Focus()

  End Sub

  Private Sub ApplyAuthorizationRules()

    'kontroli eanble/disable
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.Streets.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.Streets.CanEditObject
    StreetsGridControl.Enabled = VTE.Library.Streets.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.Streets.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.Streets.CanDeleteObject

  End Sub

#Region " KeyPress "

  Private Sub uxStreets_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxStreets

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxStreets

  End Function

  Private Sub uxStreets_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

#End Region




 
  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdAdd"
        Try
          Me.StreetsBindingSource.AddNew()
          GridView1.Focus()
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
        '_cities.Item(Me.CitiesBindingSource.Position).CommunityCode = 8
      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()
      Case "cmdDelete"
        Me.StreetsBindingSource.RemoveCurrent()
      Case "cmdCancel"
        RebindUI(False, True)
      Case "cmdExit"
        If _streets.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If Streets.CanEditObject Then
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
    Me.StreetsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.StreetsBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _streets = _streets.Save
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
        _streets = Nothing
        Try
          _streets = Streets.GetStreets
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
      Me.StreetsBindingSource.DataSource = _streets

      Me.StreetsBindingSource.RaiseListChangedEvents = True
      Me.StreetsBindingSource.ResetBindings(False)
    End Try
  End Sub

  Private Sub BindUI()
    _streets.BeginEdit()
    Me.StreetsBindingSource.DataSource = _streets
  End Sub

  Private Sub StreetsBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles StreetsBindingSource.CurrentItemChanged
    If _streets.IsSavable Then
      UxKopcinja1.cmdSave.Enabled = True
    Else
      UxKopcinja1.cmdSave.Enabled = False
    End If
    If _streets.IsDirty Then
      UxKopcinja1.cmdCancel.Enabled = True
    Else
      UxKopcinja1.cmdCancel.Enabled = False
    End If
    Dim message As New System.Text.StringBuilder
    message.AppendFormat("{0}" + vbCrLf, "")
    For Each child As Street In _streets
      For Each rule As Csla.Validation.BrokenRule In _
            child.BrokenRulesCollection
        message.AppendFormat( _
          "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
      Next
    Next
    ShowBrokenRules(message.ToString)
  End Sub

  Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
    Dim det As Street = _streets.Item(Me.StreetsBindingSource.Position)
    For Each column As DevExpress.XtraGrid.Columns.GridColumn In GridView1.Columns
      column.OptionsColumn.ReadOnly = Not det.CanWriteProperty(column.FieldName)
    Next
  End Sub


  Private Sub GridView1_InitNewRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles GridView1.InitNewRow
    Dim det As Street = _streets.Item(Me.StreetsBindingSource.Position)
    For Each column As DevExpress.XtraGrid.Columns.GridColumn In GridView1.Columns
      column.OptionsColumn.ReadOnly = Not det.CanWriteProperty(column.FieldName)
    Next
  End Sub

  Private Sub StreetsGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles StreetsGridControl.ProcessGridKey

    If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
      If _streets.IsSavable Then
        GridView1.AddNewRow()
        GridView1.FocusedColumn = colStreetName
      Else
        e.SuppressKeyPress = True
      End If
    End If

  End Sub

  
End Class
