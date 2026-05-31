Public Class uxTehnicalExamTypes

  Private WithEvents _tehnicalExamTypes As TehnicalExamsTypes

#Region " KeyPress "

  Private Sub uxTehnicalExamTypes_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxTehnicalExamTypes

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxTehnicalExamTypes

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.TehnicalExamsTypes.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.TehnicalExamsTypes.CanEditObject
    TehnicalExamsTypesGridControl.Enabled = VTE.Library.TehnicalExamsTypes.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.TehnicalExamsTypes.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.TehnicalExamsTypes.CanDeleteObject


  End Sub

  Private Sub uxTehnicalExamsTypes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _tehnicalExamTypes = TehnicalExamsTypes.GetTehnicalExamsTypes
      If _tehnicalExamTypes IsNot Nothing Then
        Me.TehnicalExamsTypesBindingSource.DataSource = _tehnicalExamTypes
        Me.UxKopcinja1.cmdSave.Enabled = _tehnicalExamTypes.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _tehnicalExamTypes.IsDirty
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


  Private Sub uxTehnicalExamsTypes_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.TehnicalExamsTypesBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _tehnicalExamTypes.Count <> 0 Then
          _tehnicalExamTypes.RemoveAt(Me.TehnicalExamsTypesBindingSource.Position)
        End If
      Case "cmdExit"
        If _tehnicalExamTypes.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If TehnicalExamsTypes.CanEditObject Then
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
    Me.TehnicalExamsTypesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.TehnicalExamsTypesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _tehnicalExamTypes = _tehnicalExamTypes.Save
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
        _tehnicalExamTypes = Nothing
        Try
          _tehnicalExamTypes = TehnicalExamsTypes.GetTehnicalExamsTypes
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
      Me.TehnicalExamsTypesBindingSource.DataSource = _tehnicalExamTypes

      Me.TehnicalExamsTypesBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.TehnicalExamsTypesBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub TehnicalExamsTypesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TehnicalExamsTypesBindingSource.CurrentItemChanged
    Try

      UxKopcinja1.cmdSave.Enabled = _tehnicalExamTypes.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _tehnicalExamTypes.IsDirty

      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As TehnicalExamsType In _tehnicalExamTypes
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

  Private Sub TehnicalExamVehiclePartsCategoriesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TehnicalExamsTypesGridControl.ProcessGridKey

    If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
      If _tehnicalExamTypes.IsSavable Then
        GridView1.AddNewRow()
      Else
        e.SuppressKeyPress = True
      End If
    End If

  End Sub

End Class

