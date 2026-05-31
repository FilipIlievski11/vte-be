Public Class dijFildsPrivilege 


    Private WithEvents _CSLAObjectsList As CSLAObjectsList
    Private WithEvents _roolList As RoolList
    Private WithEvents _filedsPrivileges As FieldsPrivileges

    Private Sub ApplyAuthorizationRules()

        ' kontroli(eanble / disable)
        Me.ReadWriteAuthorization1.ResetControlAuthorization()

    End Sub

    Private Sub uxCSLAObjects_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ApplyAuthorizationRules()
            Me.Focus()
            GridView1.Focus()
            'GridView1.FocusedColumn = colCSLAObjectName
            System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))

            _CSLAObjectsList = CSLAObjectsList.GetCSLAObjectsList
            _roolList = RoolList.GetRoolList
            _filedsPrivileges = FieldsPrivileges.GetFieldsPrivileges
            Me.CSLAObjectsListBindingSource.DataSource = _CSLAObjectsList
            Me.RoolListBindingSource.DataSource = _roolList
            If _filedsPrivileges IsNot Nothing Then
                Me.FieldsPrivilegesBindingSource.DataSource = _filedsPrivileges
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


    Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
        Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

            Case "cmdSave"
                RebindUI(True, True)
                UxKopcinja1.cmdAdd.Focus()

            Case "cmdAdd"
                _filedsPrivileges.AddNew()
                GridView1.Focus()

            Case "cmdCancel"
                RebindUI(False, True)

            Case "cmdDelete"
                If _filedsPrivileges.Count <> 0 Then
                    _filedsPrivileges.RemoveAt(Me.FieldsPrivilegesBindingSource.Position)
                End If

            Case "cmdExit"
                If _filedsPrivileges.IsDirty Then
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
        Me.FieldsPrivilegesBindingSource.RaiseListChangedEvents = False
        ' commit edits in memory
        UnbindBindingSource(Me.FieldsPrivilegesBindingSource, saveObject, True)

        Try
            ' save or cancel changes
            If saveObject Then
                Try
                    _filedsPrivileges = _filedsPrivileges.Save
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
                _filedsPrivileges = Nothing
                Try
                    _filedsPrivileges = FieldsPrivileges.GetFieldsPrivileges
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
            Me.FieldsPrivilegesBindingSource.DataSource = _filedsPrivileges

            Me.FieldsPrivilegesBindingSource.RaiseListChangedEvents = True

            If rebind Then
                Me.FieldsPrivilegesBindingSource.ResetBindings(False)
            End If

        End Try
    End Sub


    Private Sub FieldsPrivilegesGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles FieldsPrivilegesGridControl.ProcessGridKey

        If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
            If _filedsPrivileges.IsSavable Then
                GridView1.AddNewRow()
            Else
                e.SuppressKeyPress = True
            End If
        End If

    End Sub

End Class