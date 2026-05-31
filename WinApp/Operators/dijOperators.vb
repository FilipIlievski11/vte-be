Public Class dijOperators

  Private WithEvents _operators As Users 'Employes

  Private Sub ApplyAuthorizationRules()
    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()
    If Not (VTE.Library.Users.CanGetObject Or VTE.Library.User.CanGetObject) Then
      Me.Close()
    End If
    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.Users.CanEditObject
    UsersGridControl.Enabled = VTE.Library.Users.CanEditObject
    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.Users.CanAddObject
    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.Users.CanDeleteObject
  End Sub

  Private Sub dijOperators_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      GridView1.Focus()
      ApplyAuthorizationRules()
      Me.RoolListBindingSource.DataSource = RoolList.GetRoolList
      Me.DataBasesListBindingSource.DataSource = DataBasesList.GetDataBasesList
      Dim idComp As Integer = CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany
      Me.TehnicalExamOrganizationsListBindingSource.DataSource = objTehExamOrganizations.GetStationsForCompany 'TehnicalExamOrganizationsList.GetTehnicalExamOrganizationsList
      _operators = Users.GetUsersByCompany(idComp, objCurentUser.IdDataBase) 'Employes.GetEmployes
      If _operators IsNot Nothing Then
        Me.UsersBindingSource.DataSource = _operators
      End If
      'Dim _user As User
      '_user = User.GetUser(Csla.ApplicationContext.LocalContext.Item("EmployeeID"))

      'Dim str As String = String.Format("[IdDataBase]='{0}'", _user.IdDataBase)
      'GridView1.Columns("IdDataBase").FilterInfo = New DevExpress.XtraGrid.Columns.ColumnFilterInfo(str)

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


  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Try
          _operators.AddNew()
        Catch ex As Exception

        End Try
      
        GridView1.Focus()
      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _operators.Count <> 0 Then
          _operators.RemoveAt(Me.UsersBindingSource.Position)
        End If

      Case "cmdExit"
        If _operators.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              RebindUI(True, False)
              Me.Close()
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


  Protected Sub UnbindBindingSource( _
    ByVal source As BindingSource, ByVal apply As Boolean, ByVal isRoot As Boolean)

    Dim current As System.ComponentModel.IEditableObject = _
            TryCast(source.Current, System.ComponentModel.IEditableObject)
    If isRoot Then
      source.DataSource = Nothing
    End If
    If current IsNot Nothing Then
      If apply Then
        current.EndEdit()
      Else
        current.CancelEdit()
      End If
    End If

  End Sub

  Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
    ' stop the flow of events

    Me.UsersBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.UsersBindingSource, saveObject, True)

    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _operators = _operators.Save
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
        _operators = Nothing
        Try
          _operators = Users.GetUsers
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
      Me.UsersBindingSource.DataSource = _operators

      Me.UsersBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.UsersBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub


  Private Sub EmployesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles UsersGridControl.ProcessGridKey

    If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
      If _operators.IsSavable Then
        GridView1.AddNewRow()
      Else
        e.SuppressKeyPress = True
      End If
    End If

  End Sub

  Private Sub UsersBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UsersBindingSource.CurrentItemChanged
    Try
      Me.UxKopcinja1.cmdSave.Enabled = _operators.IsSavable
      Me.UxKopcinja1.cmdCancel.Enabled = _operators.IsDirty
    Catch ex As Exception

    End Try

  End Sub

End Class


