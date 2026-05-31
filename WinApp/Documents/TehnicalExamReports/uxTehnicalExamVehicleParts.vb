Public Class uxTehnicalExamVehicleParts

  Private WithEvents _tehnicalExamVehicleParts As TehnicalExamVehicleParts
  Private WithEvents _tehExamVejiclePartsList As TehnicalExamVehiclePartsList

#Region " KeyPress "

  Private Sub uxTehnicalExamVehicleParts_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxTehnicalExamVehicleParts

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxTehnicalExamVehicleParts

  End Function
#End Region

  Private Sub ApplyAuthorizationRules()

    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.TehnicalExamVehicleParts.CanGetObject Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.TehnicalExamVehicleParts.CanEditObject
    TehnicalExamVehiclePartsGridControl.Enabled = VTE.Library.TehnicalExamVehicleParts.CanEditObject

    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.TehnicalExamVehicleParts.CanAddObject

    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.TehnicalExamVehicleParts.CanDeleteObject


  End Sub

  Private Sub uxTehnicalExamVehicleParts_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      _tehnicalExamVehicleParts = TehnicalExamVehicleParts.GetTehnicalExamVehicleParts
      If _tehnicalExamVehicleParts IsNot Nothing Then
        Me.TehnicalExamVehiclePartsBindingSource.DataSource = _tehnicalExamVehicleParts
        'Me.TehnicalExamVehiclePartsCategoryListBindingSource.DataSource = TehnicalExamVehiclePartsCategoryList.GetTehnicalExamVehiclePartsCategoryList
        Me.UxKopcinja1.cmdSave.Enabled = _tehnicalExamVehicleParts.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _tehnicalExamVehicleParts.IsDirty
      End If
      _tehExamVejiclePartsList = TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsList
      Me.TehnicalExamVehiclePartsListBindingSource.DataSource = _tehExamVejiclePartsList
    Catch ex As Csla.DataPortalException
      MessageBox.Show(ex.BusinessException.ToString, _
        "Error loading", MessageBoxButtons.OK, _
        MessageBoxIcon.Exclamation)

    Catch ex As Exception
      MessageBox.Show(ex.ToString, _
        "Error loading", MessageBoxButtons.OK, _
        MessageBoxIcon.Exclamation)
    End Try
    For Each nodeRoot As TehnicalExamVehiclePartsInfo In _tehExamVejiclePartsList
      If nodeRoot.IdCategoryVehicleParts = Nothing Then
        Dim pomNode As System.Windows.Forms.TreeNode = New TreeNode
        pomNode.Text = nodeRoot.Description
        pomNode.Name = "nodeRoot" & nodeRoot.Id
        TreeView1.Nodes.Add(pomNode)
        LoadTree(pomNode)
      End If
    Next

  End Sub

  Private Sub LoadTree(ByVal rootNode As System.Windows.Forms.TreeNode)

    Dim list As TehnicalExamVehiclePartsList = TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsListByCategory(CType(Mid(rootNode.Name, 9), Integer))
    If list.Count > 0 Then
      For Each listItem As TehnicalExamVehiclePartsInfo In list
        Dim newNode As System.Windows.Forms.TreeNode = New TreeNode
        newNode.Name = "nodeRoot" & listItem.Id
        newNode.Text = listItem.Description
        rootNode.Nodes.Add(newNode)
        LoadTree(newNode)
      Next
    End If

  End Sub

  Private Sub uxTehnicalExamVehicleParts_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.TehnicalExamVehiclePartsBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _tehnicalExamVehicleParts.Count <> 0 Then
          _tehnicalExamVehicleParts.RemoveAt(Me.TehnicalExamVehiclePartsBindingSource.Position)
        End If
      Case "cmdExit"
        If _tehnicalExamVehicleParts.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If TehnicalExamVehicleParts.CanEditObject Then
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
    Me.TehnicalExamVehiclePartsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.TehnicalExamVehiclePartsBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _tehnicalExamVehicleParts = _tehnicalExamVehicleParts.Save
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
        _tehnicalExamVehicleParts = Nothing
        Try
          _tehnicalExamVehicleParts = TehnicalExamVehicleParts.GetTehnicalExamVehicleParts
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
      Me.TehnicalExamVehiclePartsBindingSource.DataSource = _tehnicalExamVehicleParts

      Me.TehnicalExamVehiclePartsBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.TehnicalExamVehiclePartsBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub TehnicalExamVehiclePartsBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TehnicalExamVehiclePartsBindingSource.CurrentItemChanged
    Try
      If _tehnicalExamVehicleParts.IsSavable Then
        UxKopcinja1.cmdSave.Enabled = True
      Else
        UxKopcinja1.cmdSave.Enabled = False
      End If
      If _tehnicalExamVehicleParts.IsDirty Then
        UxKopcinja1.cmdCancel.Enabled = True
      Else
        UxKopcinja1.cmdCancel.Enabled = False
      End If
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As TehnicalExamVehiclePart In _tehnicalExamVehicleParts
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

  Private Sub TehnicalExamVehiclePartsGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TehnicalExamVehiclePartsGridControl.ProcessGridKey

    If (e.KeyCode = Keys.Add) Or (e.KeyCode = Keys.Oemplus) Then
      If _tehnicalExamVehicleParts.IsSavable Then
        GridView1.AddNewRow()
      Else
        e.SuppressKeyPress = True
      End If
    End If

  End Sub

  'Private Sub LookUpEditCategory_ProcessNewValue(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles LookUpEditCategory.ProcessNewValue
  '  If e.DisplayValue.ToString = String.Empty Then
  '    e.Handled = False
  '    Exit Sub
  '  End If

  '  Try
  '    'Console.WriteLine("da")
  '    If TehnicalExamVehiclePartsCategory.Exists(e.DisplayValue.ToString) = 0 Then
  '      If MsgBox(e.DisplayValue.ToString & " " & My.Resources.nePostoi, MsgBoxStyle.YesNo, "Зачувај?") = MsgBoxResult.Yes Then

  '        Dim newC As TehnicalExamVehiclePartsCategory = TehnicalExamVehiclePartsCategory.NewTehnicalExamVehiclePartsCategory
  '        newC.CategoryName = e.DisplayValue.ToString()
  '        newC.ApplyEdit()
  '        newC = newC.Save

  '        Me.TehnicalExamVehiclePartsCategoryListBindingSource.RaiseListChangedEvents = False

  '        Me.TehnicalExamVehiclePartsCategoryListBindingSource.DataSource = TehnicalExamVehiclePartsCategoryList.GetTehnicalExamVehiclePartsCategoryList()
  '        Me.TehnicalExamVehiclePartsCategoryListBindingSource.RaiseListChangedEvents = True
  '        Me.TehnicalExamVehiclePartsCategoryListBindingSource.ResetBindings(False)
  '      End If
  '      e.Handled = True
  '    End If
  '  Catch ex As Exception
  '    MsgBox(ex.Message)
  '  End Try

  '  e.Handled = True
  'End Sub
End Class

