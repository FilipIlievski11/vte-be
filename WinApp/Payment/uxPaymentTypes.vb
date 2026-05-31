Public Class uxPaymentTypes

 Private WithEvents _paymentTypes As PaymentTypes = PaymentTypes.GetPaymentTypes

#Region " KeyPress "

 Private Sub uxPaymentTypes_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
  Select Case Asc(e.KeyChar)
   Case 13
    SendKeys.Send("{TAB}")
  End Select
 End Sub

#End Region

#Region " WinPart Code "

 Protected Overrides Function GetIdValue() As Object

  Return My.Resources.uxPaymentTypes

 End Function

 Public Overrides Function ToString() As String

  Return My.Resources.uxPaymentTypes

 End Function
#End Region

 Private Sub ApplyAuthorizationRules()

  ' kontroli(eanble / disable)
  Me.ReadWriteAuthorization1.ResetControlAuthorization()

  If Not VTE.Library.PaymentTypes.CanGetObject Then
   Me.Close()
  End If

  Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.PaymentTypes.CanEditObject
  PaymentTypesGridControl.Enabled = VTE.Library.PaymentTypes.CanEditObject

  Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.PaymentTypes.CanAddObject

  Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.PaymentTypes.CanDeleteObject


 End Sub

 Private Sub uxPaymentTypes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  Try
   ApplyAuthorizationRules()
   If _paymentTypes IsNot Nothing Then
    Me.PaymentTypesBindingSource.DataSource = _paymentTypes
    Me.UxKopcinja1.cmdSave.Enabled = _paymentTypes.IsSavable
    Me.UxKopcinja1.cmdCancel.Enabled = _paymentTypes.IsDirty
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


 Private Sub uxPaymentTypes_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
  ApplyAuthorizationRules()
 End Sub

 Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
  Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

   Case "cmdSave"
    RebindUI(True, True)
    UxKopcinja1.cmdAdd.Focus()

   Case "cmdAdd"
    Me.PaymentTypesBindingSource.AddNew()
    GridView1.Focus()

   Case "cmdCancel"
    RebindUI(False, True)

   Case "cmdDelete"
    If _paymentTypes.Count <> 0 Then
     _paymentTypes.RemoveAt(Me.PaymentTypesBindingSource.Position)
    End If
   Case "cmdExit"
    If _paymentTypes.IsDirty Then
     Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
      Case MsgBoxResult.Yes
       If PaymentTypes.CanEditObject Then
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
  Me.PaymentTypesBindingSource.RaiseListChangedEvents = False
  ' commit edits in memory
  UnbindBindingSource(Me.PaymentTypesBindingSource, saveObject, True)
  Try
   ' save or cancel changes
   If saveObject Then
    Try
     _paymentTypes = _paymentTypes.Save
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
    _paymentTypes = Nothing
    Try
     _paymentTypes = PaymentTypes.GetPaymentTypes
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
   Me.PaymentTypesBindingSource.DataSource = _paymentTypes

   Me.PaymentTypesBindingSource.RaiseListChangedEvents = True
   If rebind Then
    Me.PaymentTypesBindingSource.ResetBindings(False)
   End If

  End Try
 End Sub

 Private Sub PaymentTypesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PaymentTypesBindingSource.CurrentItemChanged
  Try
   UxKopcinja1.cmdSave.Enabled = _paymentTypes.IsSavable
   UxKopcinja1.cmdCancel.Enabled = _paymentTypes.IsDirty

   Dim message As New System.Text.StringBuilder
   message.AppendFormat("{0}" + vbCrLf, "")
   For Each child As PaymentType In _paymentTypes
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

 Private Sub PaymentTypesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles PaymentTypesGridControl.ProcessGridKey

  If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
   If _paymentTypes.IsSavable Then
    GridView1.AddNewRow()
   Else
    e.SuppressKeyPress = True
   End If
  End If

 End Sub

 Private Sub btnNuliranje_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuliranje.Click
  Try


   Dim idStanica As Integer = objCurentTehExamOrganization.Id
   Dim idtip As Integer = _paymentTypes.Item(Me.PaymentTypesBindingSource.Position).Id
   _paymentTypes.NuliranjePoTip(idStanica, idtip)
   MsgBox(My.Resources.UspesnoNuliranjNa & _paymentTypes.Item(Me.PaymentTypesBindingSource.Position).Name)
  Catch ex As Exception
   MsgBox(My.Resources.GreskaNuliranje & _paymentTypes.Item(Me.PaymentTypesBindingSource.Position).Name)
  End Try
 End Sub

 Private Sub btnNuliranjeZapisnici_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuliranjeZapisnici.Click
  Try
   _paymentTypes.NuliranjeZapisni(objCurentTehExamOrganization.Id)
   MsgBox(My.Resources.UspesnoNuliranjNa)
  Catch ex As Exception
   MsgBox(My.Resources.GreskaNuliranje)
  End Try
 End Sub
End Class

