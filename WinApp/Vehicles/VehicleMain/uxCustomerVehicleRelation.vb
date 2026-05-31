Public Class uxCustomerVehicleRelation
    Private WithEvents _customerVehicleRelations As CustomerVehiclesRelationsList
#Region "WinPart"

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxCustomerVehicleRelation
  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxCustomerVehicleRelation
  End Function

#End Region

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.

  End Sub

  Private Sub uxCustomerVehicleRelation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      Me.CustomersListBindingSource.DataSource = CustomersList.GetCustomersList
            Me.VehicleListBindingSource.DataSource = VehicleListShort.GetVehicleListShort  'VehicleList.GetVehicleList
      Me.CustomerVehicleRelationTypeListBindingSource.DataSource = CustomerVehicleRelationTypeList.GetCustomerVehicleRelationTypeList
            _customerVehicleRelations = CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsList
      Me.CustomerVehiclesRelationsBindingSource.DataSource = _customerVehicleRelations

    Catch ex As Exception
      MsgBox(ex.Message)

    End Try

   
  End Sub


  Private AllowShowEditor As Boolean = True

  Private Sub GridView1_FocusedRowChanged1(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
    AllowShowEditor = False
    BeginInvoke(New MethodInvoker(AddressOf tr))
  End Sub
  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    AllowShowEditor = False
    BeginInvoke(New MethodInvoker(AddressOf tr))
  End Sub
  Private Sub GridView1_ShowingEditor1(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles GridView1.ShowingEditor
    e.Cancel = Not AllowShowEditor
  End Sub

  Private Sub tr()
    AllowShowEditor = True
  End Sub

    'Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
    '    ' stop the flow of events
    '    Me.CustomerVehiclesRelationsBindingSource.RaiseListChangedEvents = False
    '    ' commit edits in memory
    '    UnbindBindingSource(Me.CustomerVehiclesRelationsBindingSource, saveObject, True)
    '    Try
    '        ' save or cancel changes
    '        If saveObject Then
    '            Try
    '                _customerVehicleRelations = _customerVehicleRelations.Save
    '            Catch ex As Csla.DataPortalException
    '                MessageBox.Show(ex.BusinessException.ToString, _
    '                  "Error saving", MessageBoxButtons.OK, _
    '                  MessageBoxIcon.Exclamation)

    '            Catch ex As Exception
    '                MessageBox.Show(ex.ToString, _
    '                  "Error saving", MessageBoxButtons.OK, _
    '                  MessageBoxIcon.Exclamation)
    '            End Try

    '        Else
    '            _customerVehicleRelations = Nothing
    '            Try
    '                _customerVehicleRelations = CustomerVehiclesRelations.GetCustomerVehiclesRelations
    '            Catch ex As Csla.DataPortalException
    '                MessageBox.Show(ex.BusinessException.ToString, _
    '                  "Error saving", MessageBoxButtons.OK, _
    '                  MessageBoxIcon.Exclamation)

    '            Catch ex As Exception
    '                MessageBox.Show(ex.ToString, _
    '                  "Error saving", MessageBoxButtons.OK, _
    '                  MessageBoxIcon.Exclamation)

    '            End Try

    '        End If


    '    Finally
    '        Me.CustomerVehiclesRelationsBindingSource.DataSource = _customerVehicleRelations

    '        Me.CustomerVehiclesRelationsBindingSource.RaiseListChangedEvents = True
    '        If rebind Then
    '            Me.CustomerVehiclesRelationsBindingSource.ResetBindings(False)
    '        End If

    '    End Try
    'End Sub
    'Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    '    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

    '        Case "cmdAdd"
    '            Try
    '                Me.CustomerVehiclesRelationsBindingSource.AddNew()
    '                GridView1.Focus()
    '            Catch ex As Exception
    '                MsgBox(ex.Message)
    '            End Try
    '            '_cities.Item(Me.CitiesBindingSource.Position).CommunityCode = 8
    '        Case "cmdSave"
    '            RebindUI(True, True)
    '            UxKopcinja1.cmdAdd.Focus()
    '        Case "cmdDelete"
    '            Me.CustomerVehiclesRelationsBindingSource.RemoveCurrent()
    '        Case "cmdCancel"
    '            RebindUI(False, True)
    '        Case "cmdExit"
    '            If _customerVehicleRelations.IsDirty Then
    '                Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
    '                    Case MsgBoxResult.Yes
    '                        If CustomerVehiclesRelations.CanEditObject Then
    '                            RebindUI(True, False)
    '                            Me.Close()
    '                        Else

    '                            Me.Close()
    '                        End If
    '                    Case MsgBoxResult.No
    '                        RebindUI(False, False)
    '                        Me.Close()
    '                    Case MsgBoxResult.Cancel
    '                        Exit Sub
    '                End Select
    '            Else
    '                Me.Close()
    '            End If

    '    End Select
    'End Sub

  Private Sub LookUpEditCustomers_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditCustomers.ButtonPressed
    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxCustomers) Then
            par.ShowWinPart(CType(ctl, uxCustomers))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxCustomers(Customer.NewCustomer))
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If

  End Sub

  Private Sub LookUpEditVehicles_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditVehicles.ButtonPressed
    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxVehicle) Then
            par.ShowWinPart(CType(ctl, uxVehicle))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxVehicle(Vehicle.NewVehicle))
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If

  End Sub

  Private Sub LookUpEditRelationType_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditRelationType.ButtonPressed
    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxRelationsTypes) Then
            par.ShowWinPart(CType(ctl, uxRelationsTypes))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxRelationsTypes)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If

  End Sub

  
End Class
