Public Class uxBusinessTypes

  Private WithEvents _businessTypes As BusinessTypes

#Region " KeyPress "



  Private Sub uxBusinessTypes_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
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

    Return My.Resources.uxBusinessTypes

  End Function
#End Region

  Private Sub LoadList()

    _businessTypes = BusinessTypes.GetBusinessTypes
    If _businessTypes IsNot Nothing Then
      Me.BusinessTypesBindingSource.DataSource = _businessTypes
    End If
  End Sub

  Private Sub ApplyAuthorizationRules()

    'kontroli eanble/disable
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.BusinessTypes.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.BusinessTypes.CanEditObject
    BusinessTypesGridControl.Enabled = VTE.Library.BusinessTypes.CanEditObject
   
    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.BusinessTypes.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.BusinessTypes.CanDeleteObject
 
  End Sub

  Private Sub uxBusinessTypes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    ApplyAuthorizationRules()
    LoadList()
    UxKopcinja1.cmdAdd.Focus()

  End Sub

  Private Sub uxBusinessTypes_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdAdd"
        Try
          Me.BusinessTypesBindingSource.AddNew()
          GridView1.Focus()
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdDelete"
        Me.BusinessTypesBindingSource.RemoveCurrent()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdExit"
        If _businessTypes.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If BusinessTypes.CanEditObject Then
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
    Me.BusinessTypesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.BusinessTypesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _businessTypes = _businessTypes.Save
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
        _businessTypes = Nothing
        Try
          _businessTypes = BusinessTypes.GetBusinessTypes
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
      Me.BusinessTypesBindingSource.DataSource = _businessTypes

      Me.BusinessTypesBindingSource.RaiseListChangedEvents = True
      Me.BusinessTypesBindingSource.ResetBindings(False)
    End Try
  End Sub

  Private Sub BindUI()
    _businessTypes.BeginEdit()
    Me.BusinessTypesBindingSource.DataSource = _businessTypes
  End Sub

  Private Sub BusinessTypesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles BusinessTypesBindingSource.CurrentItemChanged
    If _businessTypes.IsSavable Then
      UxKopcinja1.cmdSave.Enabled = True
    Else
      UxKopcinja1.cmdSave.Enabled = False
    End If
    Dim message As New System.Text.StringBuilder
    message.AppendFormat("{0}" + vbCrLf, "")
    For Each child As BusinessType In _businessTypes
      For Each rule As Csla.Validation.BrokenRule In _
            child.BrokenRulesCollection
        message.AppendFormat( _
          "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
      Next
    Next
    ShowBrokenRules(message.ToString)
  End Sub

End Class
