Public Class uxRelationsTypes

  Private WithEvents _relationsTypes As CustomerVehicleRelationTypes

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

        Return My.Resources.uxRelationTypes

  End Function

  Public Overrides Function ToString() As String

        Return My.Resources.uxRelationTypes

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.CustomerVehicleRelationTypes.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.CustomerVehicleRelationTypes.CanEditObject
    CustomerVehicleRelationTypesGridControl.Enabled = _
    VTE.Library.CustomerVehicleRelationTypes.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.CustomerVehicleRelationTypes.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.CustomerVehicleRelationTypes.CanDeleteObject


  End Sub

  Private Sub uxRelationsTypes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _relationsTypes = CustomerVehicleRelationTypes.GetCustomerVehicleRelationTypes
      If _relationsTypes IsNot Nothing Then
        Me.CustomerVehicleRelationTypesBindingSource.DataSource = _relationsTypes
        Me.UxKopcinja1.cmdSave.Enabled = _relationsTypes.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _relationsTypes.IsDirty
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


  Private Sub uxRelationsTypes_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.CustomerVehicleRelationTypesBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _relationsTypes.Count <> 0 Then
          _relationsTypes.RemoveAt(Me.CustomerVehicleRelationTypesBindingSource.Position)
        End If
      Case "cmdExit"
        If _relationsTypes.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If CustomerVehicleRelationTypes.CanEditObject Then
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
    Me.CustomerVehicleRelationTypesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.CustomerVehicleRelationTypesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _relationsTypes = _relationsTypes.Save
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
        _relationsTypes = Nothing
        Try
          _relationsTypes = CustomerVehicleRelationTypes.GetCustomerVehicleRelationTypes
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
      Me.CustomerVehicleRelationTypesBindingSource.DataSource = _relationsTypes

      Me.CustomerVehicleRelationTypesBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.CustomerVehicleRelationTypesBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub CustomerVehicleRelationTypesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerVehicleRelationTypesBindingSource.CurrentItemChanged
    Try
      UxKopcinja1.cmdSave.Enabled = _relationsTypes.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _relationsTypes.IsDirty

      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As CustomerVehicleRelationType In _relationsTypes
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

  Private Sub CustomerVehicleRelationTypesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CustomerVehicleRelationTypesGridControl.ProcessGridKey

    If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
      If _relationsTypes.IsSavable Then
        GridView1.AddNewRow()
        'GridView1.FocusedColumn = colBodytypeCode
      Else
        e.SuppressKeyPress = True
      End If
    End If

  End Sub

End Class
