Public Class uxAddNewRelation

 Private WithEvents _relation As CustomerVehiclesRelation

 Private WithEvents _vehicleList As VehicleListShort
 Private WithEvents _customerList As CustomersSearchList
 Private WithEvents _relationTypeList As CustomerVehicleRelationTypeList
 Private _seletedRelationId As Integer
 Private pomCustomer As String = ""
 Private pomVehicle As String = ""
 Public ReadOnly Property SelectedRelationId() As Integer
  Get
   Return _seletedRelationId
  End Get
 End Property


 Public Sub New()

  ' This call is required by the Windows Form Designer.
  InitializeComponent()

  ' Add any initialization after the InitializeComponent() call.
  LoadList()
  _relation = CustomerVehiclesRelation.NewCustomerVehiclesRelation
  _relation.IdRelationType = 1
  BindUI()
  ApplyAuthorizationRules()

 End Sub
 ''Public Sub New(ByVal strCustomer As String, ByVal strVehicle As String)

 '' ' This call is required by the Windows Form Designer.
 '' InitializeComponent()

 '' ' Add any initialization after the InitializeComponent() call.
 '' LoadList()
 '' _relation = CustomerVehiclesRelation.NewCustomerVehiclesRelation
 '' _relation.IdRelationType = 1

 '' BindUI()
 '' ApplyAuthorizationRules()
 '' If strVehicle <> "" Then
 ''  VehicleCustomLookUpEdit.Text = strVehicle
 ''  VehicleCustomLookUpEdit.Focus()
 '' Else
 ''  CustomerCustomLookUpEdit.Text = strCustomer
 ''  CustomerCustomLookUpEdit.Focus()
 '' End If



 ''End Sub

 Private Sub LoadList()
  _vehicleList = Nothing
  'Dim sortedList = From p In _vehicleList Order By p.Id Descending
  Me.VehicleListBindingSource.DataSource = _vehicleList 'sortedList
  _customerList = Nothing
  'Dim sortedListC = From p In _customerList Order By p.Id Descending
  Me.CustomersSearchListBindingSource.DataSource = _customerList 'sortedListC
  _relationTypeList = CustomerVehicleRelationTypeList.GetCustomerVehicleRelationTypeList
  Me.CustomerVehicleRelationTypeListBindingSource.DataSource = _relationTypeList

 End Sub

 Private Sub BindUI()
  _relation.BeginEdit()
  Me.CustomerVehiclesRelationBindingSource.DataSource = _relation
 End Sub

 Private Sub ApplyAuthorizationRules()
  btnOk.Enabled = _relation.IsSavable
 End Sub

 Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
  Me.CustomerVehiclesRelationBindingSource.RaiseListChangedEvents = False
  UnbindBindingSource(Me.CustomerVehiclesRelationBindingSource, saveObject, True)
  Try
   ' save or cancel changes
   If saveObject Then
    _relation.ApplyEdit()
    If _relation.IdVehicle = 0 Then
     _relation.IdRelationType = 3
    End If
    '
    Try
     _relation = _relation.Save
    Catch ex As Csla.DataPortalException
     MessageBox.Show(ex.BusinessException.ToString(), _
       "Error saving", MessageBoxButtons.OK, _
       MessageBoxIcon.Exclamation)
    Catch ex As Exception
     MessageBox.Show(ex.ToString(), _
       "Error Saving", MessageBoxButtons.OK, _
       MessageBoxIcon.Exclamation)
    End Try
   Else
    _relation.CancelEdit()
   End If
  Finally
   'rebind UI if requested
   If rebind Then
    BindUI()
   End If
   ' restore events
   Me.CustomerVehiclesRelationBindingSource.RaiseListChangedEvents = True
   If rebind Then
    ' refresh the UI if rebinding
    Me.CustomerVehiclesRelationBindingSource.ResetBindings(False)
   End If
  End Try

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

 Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
  If Not _relation.IsValid Then
   MsgBox("Релацијата не може да биде запамтена поради:" & _
          vbCrLf & _relation.BrokenRulesCollection.Item(0).Description)
   Exit Sub
  End If
  Try
   RebindUI(True, True)
  Catch ex As Exception
   Exit Sub
  End Try
  Me._seletedRelationId = _relation.Id
  Me.Dispose()
 End Sub

 Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
  Me._seletedRelationId = 0
  Me.Dispose()
 End Sub

 Private Sub VehicleCustomLookUpEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleCustomLookUpEdit.GotFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
    System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
  VehicleCustomLookUpEdit.Text = pomVehicle
 End Sub

 Private Sub VehicleCustomLookUpEdit_lostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleCustomLookUpEdit.LostFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
    System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
 End Sub

 Private Sub VehicleCustomLookUpEdit_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles VehicleCustomLookUpEdit.ButtonPressed
  Select Case e.Button.Index
   Case 1
    Dim par As MainForm = Me.ParentForm
    Using cekaj As New StatusBusy(My.Resources.txtLoading)
     Try
      par.AddWinPart(New uxVehicle(Vehicle.NewVehicle))
     Catch ex As Exception
      MsgBox(ex.Message)
     End Try
    End Using
    'Case 2
    '  Dim dij As New dijVehicleList(_vehicleList)
    '  If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '    _relation.IdVehicle = dij.Vehicle.Id
    '  End If
  End Select
 End Sub

 Private Sub CustomerCustomLookUpEdit_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles CustomerCustomLookUpEdit.ButtonPressed
  Select Case e.Button.Index
   Case 1
    Dim par As MainForm = Me.ParentForm
    Using cekaj As New StatusBusy(My.Resources.txtLoading)
     Try
      par.AddWinPart(New uxCustomers(Customer.NewCustomer))
     Catch ex As Exception
      MsgBox(ex.Message)
     End Try
    End Using
   Case 2
    Dim dij As New dijCustomersList()
    If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
     _relation.IdCustomer = dij.Customer.Id
    End If
  End Select
 End Sub



 Private Sub VehicleCustomLookUpEdit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles VehicleCustomLookUpEdit.KeyUp
  If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
AndAlso e.KeyData <> Keys.Tab Then
   pomVehicle = VehicleCustomLookUpEdit.Text
  Else
   Exit Sub
  End If
  If pomVehicle <> String.Empty AndAlso pomVehicle.Length >= 4 Then
   Dim par As MainForm = Me.ParentForm
   'Dim pomTekst As String = LookUpEditCustomer.Text
   Using cekaj As New StatusBusy(My.Resources.txtLoading)

    Try
     _vehicleList = VehicleListShort.GetVehicleByShellOrReg(pomVehicle) '(LookUpEditCustomer.Text)
    Catch ex As Exception
     _vehicleList = Nothing
    End Try

    Me.VehicleListBindingSource.DataSource = _vehicleList

    If _vehicleList.Count > 0 Then
     'If _customerList.Count = 1 Then

     VehicleCustomLookUpEdit.ClosePopup()
     VehicleCustomLookUpEdit.ShowPopup()
     VehicleCustomLookUpEdit.Text = pomVehicle
     '    ' e.Handled = True

     'End If
    Else
     If MsgBox(My.Resources.nePostoi, MsgBoxStyle.YesNo, My.Resources.Zacuvaj & "?") = MsgBoxResult.Yes Then
      Dim novo As Vehicle = Vehicle.NewVehicle
      novo.ShellNumber = pomVehicle
      par.AddWinPart(New uxVehicle(novo))
     Else
      pomVehicle = ""
      VehicleCustomLookUpEdit.Text = ""
     End If
    End If

   End Using
  End If
 End Sub

 Private Sub CustomerCustomLookUpEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerCustomLookUpEdit.GotFocus
  System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo(uSettings.Culture))
  CustomerCustomLookUpEdit.Text = pomCustomer
 End Sub

 Private Sub CustomerCustomLookUpEdit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CustomerCustomLookUpEdit.KeyUp
  If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
AndAlso e.KeyData <> Keys.Tab Then
   pomCustomer = CustomerCustomLookUpEdit.Text
  Else
   Exit Sub
  End If
  If pomCustomer <> String.Empty AndAlso pomCustomer.Length >= 7 Then
   Dim par As MainForm = Me.ParentForm
   'Dim pomTekst As String = LookUpEditCustomer.Text
   Using cekaj As New StatusBusy(My.Resources.txtLoading)

    Try
     _customerList = CustomersSearchList.GetCustomersListShortByString(pomCustomer) '(LookUpEditCustomer.Text)
    Catch ex As Exception
     _customerList = Nothing
    End Try

    Me.CustomersSearchListBindingSource.DataSource = _customerList

    If _customerList.Count > 0 Then
     'If _customerList.Count = 1 Then

     CustomerCustomLookUpEdit.ClosePopup()
     CustomerCustomLookUpEdit.ShowPopup()
     CustomerCustomLookUpEdit.Text = pomCustomer
     '    ' e.Handled = True

     'End If
    Else
     If MsgBox(My.Resources.nePostoi, MsgBoxStyle.YesNo, My.Resources.Zacuvaj & "?") = MsgBoxResult.Yes Then
      Dim novo As Customer = Customer.NewCustomer
      novo.Mb = pomCustomer
      par.AddWinPart(New uxCustomers(novo))
     Else
      pomCustomer = ""
      CustomerCustomLookUpEdit.Text = ""
     End If
    End If

   End Using
  End If
 End Sub


 Private Sub CustomerVehiclesRelationBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerVehiclesRelationBindingSource.CurrentItemChanged
  btnOk.Enabled = _relation.IsSavable
 End Sub
End Class
