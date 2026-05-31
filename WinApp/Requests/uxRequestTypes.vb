Public Class uxRequestTypes
  Private WithEvents _requestTypes As RequestTypes
  Private WithEvents _requestTypesList As RequestTypeList
  Private WithEvents _printTypeList As DocumentTypePrintsList
  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.


  End Sub

  Private Sub ApplyAuthorizationRules()
    If Not VTE.Library.RequestTypes.CanGetObject Then
      Me.Close()
    End If
    Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.DDVCatalogs.CanAddObject
    Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.DDVCatalogs.CanDeleteObject
    Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.DDVCatalogs.CanEditObject
  End Sub

  Private Sub uxRequestTypes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      _requestTypes = RequestTypes.GetRequestTypes
      Me.RequestTypesBindingSource.DataSource = _requestTypes
      _requestTypesList = RequestTypeList.GetRequestTypeList
      Me.RequestTypeListBindingSource.DataSource = _requestTypesList
      _printTypeList = DocumentTypePrintsList.GetDocumentTypePrintsList
            Me.DocumentTypePrintsListBindingSource.DataSource = _printTypeList
            Me.TehnicalExamsTypesListBindingSource.DataSource = TehnicalExamsTypesList.GetTehnicalExamsTypesList
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

  End Sub

#Region "PritisnatoKopce"
  Private Sub ux_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")

    End Select
  End Sub
#End Region

#Region "WinPart"

  Protected Overrides Function GetIdValue() As Object
    Return "Типови на барања"
  End Function

  Public Overrides Function ToString() As String
    Return "Типови на барања"
  End Function

#End Region


  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name
      Case "cmdAdd"
        If Me.RequestTypesBindingSource.Position >= 0 Then
          Dim parItem As RequestType = _requestTypes.Item(Me.RequestTypesBindingSource.Position)
          Dim newItem As RequestType = _requestTypes.AddNew()
          Dim arr As String() = {"Id", "TypeName", "Note"}
          Csla.Data.Map(parItem, newItem, arr)
          Me.PropertyGridControl1.Refresh()
        Else
          Dim newItem As RequestType = _requestTypes.AddNew()
        End If
        GridView1.Focus()

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdDelete"
        If _requestTypes.Count > 0 Then
          _requestTypes.RemoveAt(RequestTypesBindingSource.Position)
        End If

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdExit"
        If _requestTypes.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VTE.Library.RequestTypes.CanEditObject Then
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
    Me.RequestTypesBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.RequestTypesBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _requestTypes = _requestTypes.Save

          objRequestTypeList = RequestTypeList.GetRequestTypeList
          If Csla.ApplicationContext.LocalContext.Contains("objRequestTypeList") Then
            Csla.ApplicationContext.LocalContext.Remove("objRequestTypeList")
          End If
          Csla.ApplicationContext.LocalContext.Add("objRequestTypeList", objRequestTypeList)

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
        _requestTypes = Nothing
        Try
          _requestTypes = RequestTypes.GetRequestTypes
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
      Me.RequestTypesBindingSource.DataSource = _requestTypes
      Me.RequestTypesBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.RequestTypesBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub TreeList1_FocusedNodeChanged1(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles RequestTypeListTreeList.FocusedNodeChanged
    PropertyGridControl1.SelectedObject = RequestTypeListTreeList.GetDataRecordByNode(e.Node)
  End Sub

  Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    'Me.PropertyGridControl1.RowsCustomization()
    'Me.PropertyGridControl1.CustomizationForm.Show()
  End Sub
End Class
