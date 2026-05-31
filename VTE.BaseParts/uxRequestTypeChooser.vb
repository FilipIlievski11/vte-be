Imports VTE.Library

Public Class uxRequestTypeChooser
  Private WithEvents _requestTypeList As RequestTypeList

  Private WithEvents _currentRequestTypeInfo As RequestTypeInfo
  Public ReadOnly Property CurrentRequestTypeInfo() As RequestTypeInfo
    Get
      Return _currentRequestTypeInfo
    End Get
  End Property

  Private Sub uxRequestTypeChooser_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '_requestTypeList = RequestTypeList.GetRequestTypeList
    'Me.RequestTypeListBindingSource.DataSource = _requestTypeList
    'InitOptions(0)
  End Sub

  Public Sub SelectType(ByVal typeId As Integer)
    _requestTypeList = RequestTypeList.GetRequestTypeList
    _currentRequestTypeInfo = _requestTypeList.getInfoById(typeId)
    CaptionEmptySpaceItem.Text = "Избран тип на барање"
    Me.MessageEmptySpaceItem.Text = _requestTypeList.getDisplayText(_currentRequestTypeInfo.Id)
    Me.MessageEmptySpaceItem.TextVisible = True
    LayoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    Me.btnNext.Enabled = False
    Me.btnBack.Enabled = False
    RaiseEvent CurrentRequestTypeChanged(Me, New System.EventArgs)
  End Sub

  Public Sub InitControl(ByVal startTypeId As Integer)
    _requestTypeList = RequestTypeList.GetRequestTypeList
    Me.RequestTypeListBindingSource.DataSource = _requestTypeList
    InitOptions(startTypeId)
  End Sub

  Private Sub InitOptions(ByVal parentId As Integer)
    Dim optList As RequestTypeList = RequestTypeList.getChildList(parentId)
    Me.RequestTypesRadioGroup.Properties.Items.Clear()
    For Each it As RequestTypeInfo In optList
      Dim opt As New DevExpress.XtraEditors.Controls.RadioGroupItem
      opt.Value = it.Id
      opt.Description = it.TypeName
      RequestTypesRadioGroup.Properties.Items.Add(opt)
    Next
    RequestTypesRadioGroup.SelectedIndex = -1
    Me.btnNext.Enabled = False
    Me.btnBack.Enabled = False
  End Sub

#Region " Events "
  Public Event CurrentRequestTypeChanged As EventHandler
  Public Overridable Sub OnCurrentRequestTypeChanged( _
  ByVal sender As Object, ByVal e As EventArgs)

    RaiseEvent CurrentRequestTypeChanged(sender, e)

  End Sub

#End Region


  Private Sub RequestTypesRadioGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RequestTypesRadioGroup.SelectedIndexChanged
    If Me.RequestTypesRadioGroup.EditValue > 0 Then
      Dim reqType As RequestTypeInfo = _requestTypeList.getInfoById(Me.RequestTypesRadioGroup.EditValue)
      _currentRequestTypeInfo = reqType

      Me.btnNext.Enabled = True
      If _currentRequestTypeInfo.IsSufficient Then
        Me.btnNext.Text = "Потврди"
      Else
        Me.btnNext.Text = "Продолжи"
      End If
      Me.btnBack.Enabled = (_currentRequestTypeInfo.IdRequestType > 0)

    End If
  End Sub

  Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
    If _currentRequestTypeInfo.IsSufficient Then
      'procedura za potvrduvanje
      SelectType(_currentRequestTypeInfo.Id)
      RaiseEvent CurrentRequestTypeChanged(Me, New System.EventArgs)
      'MsgBox("ок")
    Else
      'procedura za odenje napred
      Me.MessageEmptySpaceItem.TextVisible = False
      InitOptions(_currentRequestTypeInfo.Id)
    End If
  End Sub

  Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
    InitOptions(_requestTypeList.getParentId(_currentRequestTypeInfo.IdRequestType))
  End Sub
End Class
