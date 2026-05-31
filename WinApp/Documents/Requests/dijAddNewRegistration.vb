Public Class dijAddNewRegistration 
  Private WithEvents _vehicle As Vehicle
  Public ReadOnly Property vehicle() As Vehicle
    Get
      Return _vehicle
    End Get
  End Property
  Private _selectedRegistration As VehicleRegistration
  Public ReadOnly Property SelectedRegistration() As VehicleRegistration
    Get
      Return _selectedRegistration
    End Get
  End Property

  Private WithEvents _registrationIssuerList As RegistrationIssuerList

  Public Sub New(ByVal inVehicle As Vehicle)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
    _vehicle = inVehicle
    BindUI()
    Me.VehicleBindingSource.DataSource = _vehicle
    _registrationIssuerList = RegistrationIssuerList.GetRegistrationIssuerList()
    Me.RegistrationIssuerListBindingSource.DataSource = _registrationIssuerList
    'stavi maska na registracija
    RepositoryItemTextEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx
    RepositoryItemTextEdit1.Mask.EditMask = VehicleUseList.GetVehicleUseList().GetUseIById(_vehicle.IdVehicleUse).RegistrationMask

  End Sub

  Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
    RebindUI(True, False)
        'Me._selectedRegistration = vehicle.Registrations(Me.RegistrationsBindingSource.Position)
    Me.DialogResult = Windows.Forms.DialogResult.OK
    Me.Close()
  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
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
    Me.VehicleBindingSource.RaiseListChangedEvents = False
    Me.RegistrationsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.RegistrationsBindingSource, saveObject, False)
    UnbindBindingSource(Me.VehicleBindingSource, saveObject, True)

    Me.RegistrationsBindingSource.DataSource = Me.VehicleBindingSource
    Try
      ' save or cancel changes
      If saveObject Then
        _vehicle.ApplyEdit()
        Try
          _vehicle = _vehicle.Save

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
        _vehicle.CancelEdit()
      End If
    Finally
      'rebind UI if requested
      If rebind Then
        BindUI()
      End If

      ' restore events
      Me.VehicleBindingSource.RaiseListChangedEvents = True
      Me.RegistrationsBindingSource.RaiseListChangedEvents = True

      If rebind Then
        ' refresh the UI if rebinding
        Me.VehicleBindingSource.ResetBindings(False)
        Me.RegistrationsBindingSource.ResetBindings(False)
      End If
    End Try

  End Sub

  Private Sub BindUI()
    _vehicle.BeginEdit()
    Me.VehicleBindingSource.DataSource = _vehicle
  End Sub

  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    If GridView1.FocusedColumn IsNot Nothing Then
      If (GridView1.FocusedColumn Is colRegistrationNumber) Or _
         (GridView1.FocusedColumn Is colDateOfRegistration) Or _
         (GridView1.FocusedColumn Is colDateRegistrationValidTill) Then
        System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
      Else
        System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
      End If
    End If
  End Sub
End Class