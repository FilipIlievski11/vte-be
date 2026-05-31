Public Class uxPrivileges 

    Private WithEvents _privileges As ObjectPrivileges
    Private WithEvents _cslaList As CSLAObjectsList
    Private WithEvents _roolList As RoolList
    Private Sub ApplyAuthorizationRules()

        ' kontroli(eanble / disable)
        Me.ReadWriteAuthorization1.ResetControlAuthorization()

        If Not VTE.Library.ObjectPrivileges.CanGetObject Then
            Me.Close()
        End If

        Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.ObjectPrivileges.CanEditObject
        ObjectPrivilegesGridControl.Enabled = VTE.Library.ObjectPrivileges.CanEditObject

        Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.ObjectPrivileges.CanAddObject

        Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.ObjectPrivileges.CanDeleteObject


    End Sub

    Private Sub uxPrivileges_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ApplyAuthorizationRules()
            Me.Focus()
            GridView1.Focus()
            GridView1.FocusedColumn = colIdRole
            '            System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))

            _privileges = ObjectPrivileges.GetObjectPrivileges
            _cslaList = CSLAObjectsList.GetCSLAObjectsList
            _roolList = RoolList.GetRoolList
            If _privileges IsNot Nothing Then
                Me.ObjectPrivilegesBindingSource.DataSource = _privileges
            End If
            Me.CSLAObjectsListBindingSource.DataSource = _cslaList
            Me.RoolListBindingSource.DataSource = _roolList
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
                _privileges.AddNew()
                GridView1.Focus()

            Case "cmdCancel"
                RebindUI(False, True)

            Case "cmdDelete"
                If _privileges.Count <> 0 Then
                    _privileges.RemoveAt(Me.ObjectPrivilegesBindingSource.Position)
                End If

            Case "cmdExit"
                If _privileges.IsDirty Then
                    Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
                        Case MsgBoxResult.Yes
                            'If CSLAObjects.CanEditObject Then
                            RebindUI(True, False)
                            Me.Close()
                            'Else
                            'Me.Close()
                            'End If

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
        Me.ObjectPrivilegesBindingSource.RaiseListChangedEvents = False
        ' commit edits in memory
        UnbindBindingSource(Me.ObjectPrivilegesBindingSource, saveObject, True)

        Try
            ' save or cancel changes
            If saveObject Then
                Try
                    _privileges = _privileges.Save
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
                _privileges = Nothing
                Try
                    _privileges = ObjectPrivileges.GetObjectPrivileges
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
            Me.ObjectPrivilegesBindingSource.DataSource = _privileges

            Me.ObjectPrivilegesBindingSource.RaiseListChangedEvents = True

            If rebind Then
                Me.ObjectPrivilegesBindingSource.ResetBindings(False)
            End If

        End Try
    End Sub


    Private Sub GridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ObjectPrivilegesGridControl.ProcessGridKey

        If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
            If _privileges.IsSavable Then
                GridView1.AddNewRow()
            Else
                e.SuppressKeyPress = True
            End If
        End If

    End Sub

End Class