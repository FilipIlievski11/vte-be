Public Class uxVehicleCategoryForPayments
  Private WithEvents _vehicleCategoryForPayments As VehicleCategoryForPayments
#Region " KeyPress "

  Private Sub uxVehicleCategoryForPayments_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxBrakes

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxVehicleCategoryForPayments

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.VehicleCategoryForPayments.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.VehicleCategoryForPayments.CanEditObject
    VehicleCategoryForPaymentsGridControl.Enabled = VTE.Library.VehicleCategoryForPayments.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.VehicleCategoryForPayments.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.VehicleCategoryForPayments.CanDeleteObject


  End Sub

  Private Sub uxVehicleCategoryForPayments_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _vehicleCategoryForPayments = VehicleCategoryForPayments.GetVehicleCategoryForPayments
      If _vehicleCategoryForPayments IsNot Nothing Then
        Me.VehicleCategoryForPaymentsBindingSource.DataSource = _vehicleCategoryForPayments
        Me.UxKopcinja1.cmdSave.Enabled = _vehicleCategoryForPayments.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _vehicleCategoryForPayments.IsDirty
      End If
        Catch ex As Csla.Validation.ValidationException
            MsgBox(My.Resources.ValidationError)
            'Me.VehicleCategoryForPaymentsBindingSource.DataSource = VehicleCategoryForPayments.GetVehicleCategoryForPayments
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


  Private Sub uxVehicleCategoryForPayments_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.VehicleCategoryForPaymentsBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _vehicleCategoryForPayments.Count <> 0 Then
          _vehicleCategoryForPayments.RemoveAt(Me.VehicleCategoryForPaymentsBindingSource.Position)
        End If
      Case "cmdExit"
        If _vehicleCategoryForPayments.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VehicleCategoryForPayments.CanEditObject Then
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
    Me.VehicleCategoryForPaymentsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.VehicleCategoryForPaymentsBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _vehicleCategoryForPayments = _vehicleCategoryForPayments.Save
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
        _vehicleCategoryForPayments = Nothing
        Try
          _vehicleCategoryForPayments = VehicleCategoryForPayments.GetVehicleCategoryForPayments
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
      Me.VehicleCategoryForPaymentsBindingSource.DataSource = _vehicleCategoryForPayments

      Me.VehicleCategoryForPaymentsBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.VehicleCategoryForPaymentsBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub VehicleCategoriesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleCategoryForPaymentsBindingSource.CurrentItemChanged
    Try

      UxKopcinja1.cmdSave.Enabled = _vehicleCategoryForPayments.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _vehicleCategoryForPayments.IsDirty

      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VehicleCategoryForPayment In _vehicleCategoryForPayments
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

  'Private Sub VehicleBrakesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles VehicleCategoryForPaymentsGridControl.ProcessGridKey

  '  If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
  '    If _vehicleCategoryForPayments.IsSavable Then
  '      GridView1.AddNewRow()
  '      'GridView1.FocusedColumn = colBodytypeCode
  '    Else
  '      e.SuppressKeyPress = True
  '    End If
  '  End If

  'End Sub
  
End Class
