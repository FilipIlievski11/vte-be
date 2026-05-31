Public Class uxVehicle

  Private WithEvents _vehicleModelList As VehicleModelList
  ' Private WithEvents _vehicleBodytypeList As VehicleBodytypeList
  '  Private WithEvents _VehicleCategoryList As VehicleCategoryList
  '   Private WithEvents _ColorsList As ColorsList
  Private WithEvents _VehicleEngineTypeList As VehicleEngineTypeList
  '  Private WithEvents _CityList As CityList
  '   Private WithEvents _CountriesList As CountriesList
  ' Private WithEvents _VehicleEnginePowerSourceTypeList As VehicleEnginePowerSourceTypeList
  'Private WithEvents _VehicleEngineMarkList As VehicleEngineMarkList
  '  Private WithEvents _VehicleBrakesList As VehicleBrakesList
  '  Private WithEvents _VehicleGearBoxList As VehicleGearBoxList
  '  Private WithEvents _VehicleSupportingList As VehicleSupportingList
  Private WithEvents _engineTypeModelRelation As EngineTypeModelRelation
  Private WithEvents _tireTypeList As VehicleTireTypeList
  Private WithEvents _useList As VehicleUseList
  '  Private WithEvents _vehicleCategoryForPaymentList As VehicleCategoryForPaymentsList
  '  Private WithEvents _vehicleEngineEcoProgramList As VehicleEngineEcoProgramList
  ' Private WithEvents _registrationIssuerList As RegistrationIssuerList
  Private WithEvents _bodyTypePayment As VehicleBodytpePaymentList
  Private _lastRegChanged As Boolean
  Private _firstRegChanged As Boolean
  Private _firstRegNo As String
  ' Private _firstRegCom As String
  Private _firstRegMake As DateTime
  Private _firstRegValid As DateTime
  Private _firstRegIdIssuer As Integer
  Private _lastRegNo As String
  '  Private _lastRegCom As String
  Private _lastRegMake As DateTime
  Private _lastRegValid As DateTime
  Private _lastRegIdIssuer As Integer

  Private WithEvents _vehicle As Vehicle
  Public ReadOnly Property Vehicle() As Vehicle
    Get
      Return _vehicle
    End Get
  End Property

  Public Sub New(ByVal vehicle As Vehicle)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    ' Add any initialization after the InitializeComponent() call.

    _vehicle = vehicle

    LoadList()

    If _vehicle.IsNew Then
      ' _vehicle.IdEnginePowerSource = 0
      _firstRegChanged = True
      _lastRegChanged = True 'False
      TraffLicenceNumInsertTextEdit.Text = ""
      LayoutTrafficLicNumInsert.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
      LayoutTrafficLicNumOld.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
      Dim pom As String = objCommunityList.GetCommunitiesListById(objCurentTehExamOrganization.IdCommunity).RegistrationCode
      _vehicle.FirstIdRegistrationIssuer = objCurentTehExamOrganization.IdDefaultRegistrationIssuer    '.RegistrationNumber = pom '& "-000-"
      '_vehicle.FirstRegistrationCommunity = objCommunityList.GetCommunitiesListById(objCurentTehExamOrganization.IdCommunity).CommunityName
      _vehicle.FirstRegistrationValidTill = Date.Now.AddYears(1)
      _vehicle.FirstRegistrationMakeDate = Date.Now
      _vehicle.FirstRegistrationNumber = pom & "-000-AA"
      _vehicle.LastIdRegistrationIssuer = objCurentTehExamOrganization.IdDefaultRegistrationIssuer
      _lastRegIdIssuer = _vehicle.LastIdRegistrationIssuer
      _vehicle.LastRegistratinNumber = _vehicle.FirstRegistrationNumber
      _lastRegNo = _vehicle.LastRegistratinNumber
      ' _vehicle.LastRegistrationCommunity = _vehicle.FirstRegistrationCommunity
      '  _lastRegCom = _vehicle.LastRegistrationCommunity
      _vehicle.LastRegistrationValidTill = Date.Now.AddYears(1)
      _lastRegValid = _vehicle.LastRegistrationValidTill
      _vehicle.LastRegistrationMakeDate = Date.Now
      _lastRegMake = _vehicle.LastRegistrationMakeDate
      _firstRegIdIssuer = -1
      ' _firstRegCom = ""
      _firstRegMake = Date.MinValue
      _firstRegNo = ""
      _firstRegValid = Date.MinValue
    Else
      _firstRegIdIssuer = _vehicle.FirstIdRegistrationIssuer
      '   _firstRegCom = _vehicle.FirstRegistrationCommunity
      _firstRegMake = _vehicle.FirstRegistrationMakeDate
      _firstRegNo = _vehicle.FirstRegistrationNumber
      _firstRegValid = _vehicle.FirstRegistrationValidTill

      _lastRegIdIssuer = _vehicle.LastIdRegistrationIssuer
      _lastRegNo = _vehicle.LastRegistratinNumber
      '  _lastRegCom = _vehicle.LastRegistrationCommunity
      _lastRegMake = _vehicle.LastRegistrationMakeDate
      _lastRegValid = _vehicle.LastRegistrationValidTill

      LayoutTrafficLicNumInsert.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
      LayoutTrafficLicNumOld.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    End If
    BindUI()

    'If vehicle.IsNew Then
    '  Dim pom As String = CommunitiesList.GetCommunitiesList.GetCommunitiesListById(Options.GetOptions.IdCommunity).RegistrationCode
    '  _vehicle.RegistrationNumber = pom '& "-000-"

    'End If

    ApplyAuthorizationRules()
    If _vehicle.IsNew Then
      Me.MakeDateDateEdit.Properties.NullDate = New Csla.SmartDate(True).Date
      Me.MakeDateDateEdit.Properties.NullText = String.Empty
    End If
    ' If Not _vehicle.IsNew Then
    ' ''Me.RepositoryItemTextEdit2.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx
    ' ''Me.RepositoryItemTextEdit2.Mask.EditMask = _useList.GetUseIById(_vehicle.IdVehicleUse).RegistrationMask

    'Me.LastRegistratinNumberTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx
    'Me.LastRegistratinNumberTextEdit.Properties.Mask.EditMask = objVehicleUseList.GetUseIById(_vehicle.IdVehicleUse).RegistrationMask
    '' Me.FirstRegistrationNumberTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx
    ' Me.FirstRegistrationNumberTextEdit.Properties.Mask.EditMask = _useList.GetUseIById(_vehicle.IdVehicleUse).RegistrationMask
    ' End If


    'Dim listanavozila As VehiclesListShortListAll = VehiclesListShortListAll.GetVehiclesListShortListAll()
    'Dim listabrojki As List(Of Integer) = New List(Of Integer)
    'For Each vozilo As VehiclesListShortInfoAll In listanavozila
    '    listabrojki.Add(vozilo.Id)

    'Next
    'For Each broj As Integer In listabrojki
    '    Dim smenetovozilo As VTE.Library.Vehicle = VTE.Library.Vehicle.GetVehicle(broj)
    '    Dim prvareg As registrationBasicInfo = VTE.Library.Vehicle.GetVehicleFirstRegistration(broj)
    '    Dim poslednareg As registrationBasicInfo = VTE.Library.Vehicle.GetVehicleLastRegistration(broj)

    '    smenetovozilo.LastIdRegistrationIssuer = poslednareg.IDIssuer
    '    smenetovozilo.LastRegistratinNumber = poslednareg.RegistrationNumber
    '    smenetovozilo.LastRegistrationCommunity = poslednareg.RegistrationPlace
    '    smenetovozilo.LastRegistrationMakeDate = poslednareg.RegistrationDate
    '    smenetovozilo.LastRegistrationValidTill = poslednareg.DateRegistrationValidTill

    '    smenetovozilo.FirstIdRegistrationIssuer = prvareg.IDIssuer
    '    smenetovozilo.FirstRegistrationCommunity = prvareg.RegistrationPlace
    '    smenetovozilo.FirstRegistrationMakeDate = prvareg.RegistrationDate
    '    smenetovozilo.FirstRegistrationNumber = prvareg.RegistrationNumber
    '    smenetovozilo.FirstRegistrationValidTill = prvareg.DateRegistrationValidTill
    '    Try
    '        smenetovozilo = smenetovozilo.Save
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try

    'Next

  End Sub

  Private Sub ApplyAuthorizationRules()

  End Sub

  Private Sub uxVehicle_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    ' Me.RegistrationsGridControl.ForceInitialize()
    Me.ShellNumberTextEdit.Focus()
  End Sub


#Region " KeyPress "


  Private Sub uxVehicle_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")

    End Select

  End Sub
#End Region

#Region " WinPart "

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxVehicle
  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxVehicle
  End Function

#End Region

#Region " Bindings "

  Private Sub LoadList()




    _tireTypeList = VehicleTireTypeList.GetVehicleTireTypeList
    Me.VehicleTireTypeListBindingSource.DataSource = _tireTypeList

    '_vehicleBodytypeList = VehicleBodytypeList.GetVehicleBodytypeList
    Me.VehicleBodytypeListBindingSource.DataSource = objVehicleBodytypeList

    ' _VehicleCategoryList = VehicleCategoryList.GetVehicleCategoryList
    Me.VehicleCategoryListBindingSource.DataSource = objVehicleCategoryList

    ' _ColorsList = ColorsList.GetColorsList
    Me.ColorsListBindingSource.DataSource = objColorsList

    _vehicleModelList = VehicleModelList.GetVehicleModelList
    Me.VehicleModelListBindingSource.DataSource = _vehicleModelList

    _VehicleEngineTypeList = VehicleEngineTypeList.GetVehicleEngineTypeList
    Me.VehicleEngineTypeListBindingSource.DataSource = _VehicleEngineTypeList

    ' _CityList = CityList.GetCityList
    Me.CityListBindingSource.DataSource = objCityList

    '_CountriesList = CountriesList.GetCountriesList
    Me.CountriesListBindingSource.DataSource = objCountriesList

    '  _VehicleEnginePowerSourceTypeList = VehicleEnginePowerSourceTypeList.GetVehicleEnginePowerSourceTypeList
    Me.VehicleEnginePowerSourceTypeListBindingSource.DataSource = objVehicleEnginePowerSourceTypeList

    '   _VehicleBrakesList = VehicleBrakesList.GetVehicleBrakesList
    Me.VehicleBrakesListBindingSource.DataSource = objVehicleBrakesList

    ' _VehicleGearBoxList = VehicleGearBoxList.GetVehicleGearBoxList
    Me.VehicleGearBoxListBindingSource.DataSource = objVehicleGearBoxList

    '_VehicleSupportingList = VehicleSupportingList.GetVehicleSupportingList
    Me.VehicleSupportingListBindingSource.DataSource = objVehicleSupportingList

    ' _vehicleEngineEcoProgramList = VehicleEngineEcoProgramList.GetVehicleEngineEcoProgramList
    Me.VehicleEngineEcoProgramListBindingSource.DataSource = objVehicleEngineEcoProgramList

    ' _vehicleCategoryForPaymentList = VehicleCategoryForPaymentsList.GetVehicleCategoryForPaymentsList
    Me.VehicleCategoryForPaymentsListBindingSource.DataSource = objVehicleCategoryForPaymentsList

    '   _registrationIssuerList = RegistrationIssuerList.GetRegistrationIssuerList()
    Me.RegistrationIssuerListBindingSource.DataSource = objRegistrationIssuerList

    '_useList = VehicleUseList.GetVehicleUseList
    Me.VehicleUseListBindingSource.DataSource = objVehicleUseList

  End Sub

  Private Sub BindUI()
    _vehicle.BeginEdit()
    Me.VehicleBindingSource.DataSource = _vehicle
  End Sub

  Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
    ' stop the flow of events
    Me.VehicleBindingSource.RaiseListChangedEvents = False
    Me.VehicleAxesBindingSource.RaiseListChangedEvents = False
    Me.VehicleAxesDestinationsBindingSource.RaiseListChangedEvents = False
    Me.VehiclePersonalTiresBindingSource.RaiseListChangedEvents = False
    'Me.RegistrationsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    'UnbindBindingSource(Me.RegistrationsBindingSource, saveObject, False)
    UnbindBindingSource(Me.VehicleAxesDestinationsBindingSource, saveObject, False)
    UnbindBindingSource(Me.VehiclePersonalTiresBindingSource, saveObject, False)
    UnbindBindingSource(Me.VehicleAxesBindingSource, saveObject, False)
    UnbindBindingSource(Me.VehicleBindingSource, saveObject, True)

    Me.VehicleAxesBindingSource.DataSource = Me.VehicleBindingSource
    Me.VehiclePersonalTiresBindingSource.DataSource = Me.VehicleBindingSource
    'Me.RegistrationsBindingSource.DataSource = Me.VehicleBindingSource
    Me.VehicleAxesDestinationsBindingSource.DataSource = Me.VehicleBindingSource
    Try
      ' save or cancel changes
      If saveObject Then

        _vehicle.ApplyEdit()
        Try
          _vehicle = _vehicle.Save
          If _firstRegChanged Then
            'If _firstRegCom <> _vehicle.FirstRegistrationCommunity Or _
            If _firstRegIdIssuer <> _vehicle.FirstIdRegistrationIssuer Or _
            _firstRegMake <> _vehicle.FirstRegistrationMakeDate Or _
            _firstRegNo <> _vehicle.FirstRegistrationNumber Or _
            _firstRegValid <> _vehicle.FirstRegistrationValidTill Then
              Dim newreg As VehicleRegistration = VehicleRegistration.NewRegistration()
              newreg.DateOfRegistration = _vehicle.FirstRegistrationMakeDate
              newreg.DateRegistrationValidTill = _vehicle.FirstRegistrationMakeDate.AddYears(1)
              newreg.IdRegistrationIssuer = _vehicle.FirstIdRegistrationIssuer
              newreg.IdVehicle = _vehicle.Id
              newreg.IsFirstRegistration = True
              ' newreg.PlaceOfRegistration = _vehicle.FirstRegistrationCommunity
              newreg.RegistrationNumber = _vehicle.FirstRegistrationNumber
              newreg = newreg.Save()
              _firstRegIdIssuer = _vehicle.FirstIdRegistrationIssuer
              ' _firstRegCom = _vehicle.FirstRegistrationCommunity
              _firstRegMake = _vehicle.FirstRegistrationMakeDate
              _firstRegNo = _vehicle.FirstRegistrationNumber
              _firstRegValid = _vehicle.FirstRegistrationValidTill
            End If
          End If
          If _lastRegChanged Then
            '  If _lastRegCom <> _vehicle.LastRegistrationCommunity Or _
            If _lastRegIdIssuer <> _vehicle.LastIdRegistrationIssuer Or _
            _lastRegMake <> _vehicle.LastRegistrationMakeDate Or _
            _lastRegNo <> _vehicle.LastRegistratinNumber Or _
            _lastRegValid <> _vehicle.LastRegistrationValidTill Then
              If _vehicle.FirstIdRegistrationIssuer <> _vehicle.LastIdRegistrationIssuer Or _
                                       _vehicle.FirstRegistrationMakeDate <> _vehicle.LastRegistrationMakeDate Or _
              _vehicle.FirstRegistrationNumber <> _vehicle.LastRegistratinNumber Or _
              _vehicle.FirstRegistrationValidTill <> _vehicle.LastRegistrationValidTill Then
                '  _vehicle.FirstRegistrationCommunity <> _vehicle.LastRegistrationCommunity Or _
                Dim newreg As VehicleRegistration = VehicleRegistration.NewRegistration()
                newreg.DateOfRegistration = _vehicle.LastRegistrationMakeDate
                newreg.DateRegistrationValidTill = _vehicle.LastRegistrationValidTill
                newreg.IdRegistrationIssuer = _vehicle.LastIdRegistrationIssuer
                newreg.IdVehicle = _vehicle.Id
                newreg.IsFirstRegistration = False
                ' newreg.PlaceOfRegistration = _vehicle.LastRegistrationCommunity
                newreg.RegistrationNumber = _vehicle.LastRegistratinNumber
                newreg = newreg.Save()
                _lastRegIdIssuer = _vehicle.LastIdRegistrationIssuer
                _lastRegNo = _vehicle.LastRegistratinNumber
                ' _lastRegCom = _vehicle.LastRegistrationCommunity
                _lastRegMake = _vehicle.LastRegistrationMakeDate
                _lastRegValid = _vehicle.LastRegistrationValidTill
              End If
            End If
          End If

          If LayoutTrafficLicNumInsert.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always _
          AndAlso TraffLicenceNumInsertTextEdit.Text IsNot Nothing _
          AndAlso TraffLicenceNumInsertTextEdit.Text <> "" Then

            Dim relacija As CustomerVehiclesRelation = CustomerVehiclesRelation.NewCustomerVehiclesRelation
            relacija.IdCustomer = 0
            relacija.IdVehicle = _vehicle.Id
            relacija.IdRelationType = 1
            relacija.StartDate = Now.Date
            relacija.EndDate = Now.Date.AddYears(1)

            Dim soobrakajna As DocumentsTrafficLicence = DocumentsTrafficLicence.NewDocumentsTrafficLicence
            soobrakajna.IdCustomerVehicleRelation = (relacija.Save()).Id
            soobrakajna.TrafficLicenceNumber = TraffLicenceNumInsertTextEdit.Text
            soobrakajna.MadeDate = Now
            soobrakajna.IdTehnicalExamOrganizationsIssuedBy = 0

            'soobrakajna.EndDate = Now.AddMonths(objOpcii.TrafficLicenceVlidNumOfMonths)
            soobrakajna.Save()
          End If

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
      Me.VehicleAxesBindingSource.RaiseListChangedEvents = True
      Me.VehiclePersonalTiresBindingSource.RaiseListChangedEvents = True
      Me.VehicleAxesDestinationsBindingSource.RaiseListChangedEvents = True
      'Me.RegistrationsBindingSource.RaiseListChangedEvents = True
      If rebind Then
        ' refresh the UI if rebinding
        Me.VehicleBindingSource.ResetBindings(False)
        Me.VehiclePersonalTiresBindingSource.ResetBindings(False)
        Me.VehicleAxesDestinationsBindingSource.ResetBindings(False)
        Me.VehicleAxesBindingSource.ResetBindings(False)
        'Me.RegistrationsBindingSource.ResetBindings(False)
      End If
    End Try

  End Sub

  Private Sub BindingSources_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
    VehicleBindingSource.CurrentChanged, _
    VehicleAxesBindingSource.CurrentItemChanged, _
    VehiclePersonalTiresBindingSource.CurrentItemChanged ''', '_
    'RegistrationsBindingSource.CurrentItemChanged()

    ApplyVallidation()

    btnSave.Enabled = _vehicle.IsSavable
    btnCancel.Enabled = _vehicle.IsDirty

  End Sub


  Private Sub ApplyVallidation()
    Me.btnSave.Enabled = _vehicle.IsSavable
    Me.btnAddNew.Enabled = _vehicle.IsValid
    Dim message As New System.Text.StringBuilder
    message.AppendFormat("{0}" + vbCrLf, "")
    For Each rule As Csla.Validation.BrokenRule In _vehicle.BrokenRulesCollection
      message.AppendFormat( _
        "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
    Next
    ''Tires
    'For Each child As vehi In _vehicle.VehiclePersonalTires
    '    For Each rule As Csla.Validation.BrokenRule In child.BrokenRulesCollection
    '        message.AppendFormat( _
    '          "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
    '    Next
    'Next
    'axes
    For Each child As VehicleAxis In _vehicle.VehicleAxes
      For Each rule As Csla.Validation.BrokenRule In child.BrokenRulesCollection
        message.AppendFormat( _
          "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
      Next
    Next
    'registrations
    'For Each child As VehicleRegistration In _vehicle.Registrations
    '    For Each rule As Csla.Validation.BrokenRule In child.BrokenRulesCollection
    '        message.AppendFormat( _
    '          "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
    '    Next
    'Next
    ShowBrokenRules(message.ToString, True)
  End Sub

#End Region

#Region " Buttons "

  Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
    Using busy As New Splash(My.Resources.textSaveing)

      Try
        If _engineTypeModelRelation.IsNew Then

          _engineTypeModelRelation.Save()
        End If
      Catch ex As Exception

      End Try

      RebindUI(True, True)
    End Using
    Me.btnAddNew.Focus()

  End Sub

  Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    RebindUI(False, True)
  End Sub

  Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
    Try
      RebindUI(True, True)
    Catch ex As Exception
      Exit Sub
    End Try

    _vehicle = Nothing
    _vehicle = Vehicle.NewVehicle

    BindUI()
    Me.ShellNumberTextEdit.Focus()
  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    If _vehicle.IsDirty Then
      Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, My.Resources.exitNote)
        Case MsgBoxResult.Yes
          If Vehicle.CanEditObject Then
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
  End Sub

#End Region

  'Private Sub ModelLookUpEdit_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ModelLookUpEdit.EditValueChanged
  '      '_VehicleEngineTypeList = VehicleEngineTypeList.GetVehicleEngineTypeList(ModelLookUpEdit.EditValue)
  '      _engineTypeModelRelation = Nothing
  'End Sub

  Private Sub NumberOfAxisSpinEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NumberOfAxisSpinEdit.GotFocus
    NumberOfAxisSpinEdit.Select()
  End Sub

  Private Sub NumberOfAxisSpinEdit_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) _
    Handles NumberOfAxisSpinEdit.MouseUp, NumberOfDoorsSpinEdit.MouseUp, NumberOfLieingSeatsSpinEdit.MouseUp, _
   NumberOfPropulsionWheelsSpinEdit.MouseUp, NumberOfSeatsSpinEdit.MouseUp, _
   NumberOfStandingSeatsSpinEdit.MouseUp, EnginePowerSpinEdit.MouseUp, _
   EngineWorkingCapacitySpinEdit1.MouseUp, VehicleSizeHightSpinEdit.MouseUp, _
   VehicleSizeLengthSpinEdit.MouseUp, VehicleSizeWidthSpinEdit.MouseUp, EmptyWaightSpinEdit.MouseUp, _
   MaximunAllowedWaightSpinEdit.MouseUp, NumberOfWheelsSpinEdit.MouseUp, PropulsionAxisSpinEdit.MouseUp

    CType(sender, DevExpress.XtraEditors.SpinEdit).SelectionStart = 0
    CType(sender, DevExpress.XtraEditors.SpinEdit).SelectionLength = CType(sender, DevExpress.XtraEditors.SpinEdit).Text.Length

  End Sub


  Private Sub NumberOfAxisSpinEdit_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles NumberOfAxisSpinEdit.Validated

    While GridView4.RowCount > 0
      GridView4.MoveFirst()
      GridView4.DeleteSelectedRows()
    End While
    While GridView6.RowCount > 0
      GridView6.MoveFirst()
      GridView6.DeleteSelectedRows()
    End While

    If NumberOfAxisSpinEdit.Value > 1 Then
      For i As Integer = 1 To (NumberOfAxisSpinEdit.Value) 'CType(NumberOfAxisSpinEdit.Text, Integer)
        Dim pomAxis As VehicleAxis = _vehicle.VehicleAxes.AddNew
        pomAxis.IdVehicle = _vehicle.Id
        pomAxis.AxisNumber = i
        Dim pomAxisDestination As VehicleBetweenAxesDestination = _vehicle.VehicleAxesDestinations.AddNew
        pomAxisDestination.IdVehicle = _vehicle.Id
        pomAxisDestination.FromTo = i - 1 & "-" & i ' i & "-" & i + 1 '
      Next
      'Dim pomAxis1 As VehicleAxis = _vehicle.VehicleAxes.AddNew
      'pomAxis1.IdVehicle = _vehicle.Id
      'pomAxis1.AxisNumber = NumberOfAxisSpinEdit.Value
    End If
    PropulsionAxisSpinEdit.Focus()
  End Sub




  Private Sub CustomLookUpEngineType_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomLookUpEngineType.Validated
    If _vehicle.IdEngineType > 0 Then
      Dim Info As VehicleEngineTypeInfo = _VehicleEngineTypeList.GetVehicleEngineTypeById(_vehicle.IdEngineType)
      If Info IsNot Nothing Then
        _vehicle.EngineTorque = Info.DefaultTorque
        '_vehicle.EnginePower = Info.DefaultPower
        _vehicle.EnginePowerOutPut = Info.DefaultPowerOutPut
        _vehicle.IdEnginePowerSource = Info.IdDefaultPowerSource
        _vehicle.EngineWorkingCapacity = Info.DefaultPower
      End If
    End If
    Try
      If _engineTypeModelRelation.IsNew Then
        _engineTypeModelRelation.IdEngineType = CustomLookUpEngineType.EditValue
      End If
    Catch ex As Exception

    End Try

  End Sub




  Private Sub LookUpEditVehicleType_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditVehicleType.EditValueChanged
    If LookUpEditVehicleType.EditValue > 0 Then
      _bodyTypePayment = VehicleBodytpePaymentList.GetVehicleBodytpePaymentListByCategory(LookUpEditVehicleType.EditValue)
      '_vehicleBodytypeList = VehicleBodytypeList.GetVehicleBodytypeListByCategory(LookUpEditVehicleType.EditValue)
      BodyTypeCodeLookUpEdit.Properties.ReadOnly = False
      'If _vehicleBodytypeList.Count > 0 Then
      '    Me.VehicleBodytypeListBindingSource.DataSource = _vehicleBodytypeList
      'End If
      If _bodyTypePayment.Count > 0 Then
        Me.VehicleBodytpePaymentListBindingSource.DataSource = _bodyTypePayment
      End If
    End If
  End Sub

  Private Sub LookUpEditVehicleType_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditVehicleType.Validated
    If LookUpEditVehicleType.EditValue > 0 Then
      Dim cat As VehicleCategorie = VehicleCategorie.GetVehicleCategorie(CInt(LookUpEditVehicleType.EditValue))
      DisableFields(cat, Me)
    End If

  End Sub

  Private Sub DisableFields(ByVal cat As VehicleCategorie, ByVal paren As Control)

    'pomini gi site kontroli vo formata i disable ako gi ima vo listata
    For Each ctr As Control In paren.Controls
      'textBox
      If TypeOf ctr Is DevExpress.XtraEditors.TextEdit Then
        For Each binding As Binding In ctr.DataBindings
          ' get the BindingSource if appropriate
          If TypeOf binding.DataSource Is BindingSource Then
            If cat.DisabledFields.ContainsField(binding.BindingMemberInfo.BindingField) Then
              ctr.Enabled = False
            Else
              ctr.Enabled = True
            End If
          End If
        Next
      End If
      'otidi vo rekurzija ako ima deca
      If ctr.HasChildren Then
        DisableFields(cat, ctr)
      End If
    Next


  End Sub

  Private Sub CustomLookUpEnginePowerSource_ProcessNewValue(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) _
  Handles CustomLookUpEnginePowerSource.ProcessNewValue, CustomLookUpEngineSecondPowerSource.ProcessNewValue
    If e.DisplayValue.ToString = String.Empty Then
      e.Handled = False
      Exit Sub
    End If

    Try
      'Console.WriteLine("da")
      If VehicleEnginePowerSourceType.Exists(e.DisplayValue.ToString) = 0 Then
        If MsgBox(e.DisplayValue.ToString & My.Resources.nePostoi, MsgBoxStyle.YesNo, My.Resources.Zacuvaj & "?") = MsgBoxResult.Yes Then

          Dim newSt As VehicleEnginePowerSourceType = VehicleEnginePowerSourceType.NewVehicleEnginePowerSourceType
          newSt.PowerSourceName = e.DisplayValue.ToString()
          newSt.ApplyEdit()
          newSt = newSt.Save

          Me.VehicleEnginePowerSourceTypeListBindingSource.RaiseListChangedEvents = False

          Me.VehicleEnginePowerSourceTypeListBindingSource.DataSource = VehicleEnginePowerSourceTypeList.GetVehicleEnginePowerSourceTypeList()
          Me.VehicleEnginePowerSourceTypeListBindingSource.RaiseListChangedEvents = True
          Me.VehicleEnginePowerSourceTypeListBindingSource.ResetBindings(False)
        End If
        e.Handled = True
      End If
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

    e.Handled = True
  End Sub



  Private Sub NumberOfPropulsionWheelsSpinEdit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles NumberOfPropulsionWheelsSpinEdit.LostFocus
    If GridView6.RowCount > 0 Then
      GridView6.Focus()
      GridView6.FocusedRowHandle = 0
    Else
      'If GridView5.RowCount > 0 Then
      '    GridView5.Focus()
      'Else
      '    'FirstRegistrationNumberTextEdit.Focus()
      '    System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
      'End If
    End If
  End Sub

  'Private Sub GridView5_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView5.CellValueChanged

  '    ' '' ''Kod za da se smeni registracionoto podracnje ama momentalno ne moze da se napravi posto

  '    ' '' ''If GridView5.FocusedColumn Is colRegistrationNumber Then
  '    ' '' ''    GridView5.SetRowCellValue(GridView5.FocusedRowHandle, colIdRegistrationIssuer.FieldName, 
  '    ' '' ''End If
  'End Sub

  'Private Sub GridView5_ColumnChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView5.ColumnChanged

  'End Sub


  'Private Sub GridView5_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView5.FocusedColumnChanged
  '    If GridView5.FocusedColumn IsNot Nothing Then
  '        If (GridView5.FocusedColumn Is colRegistrationNumber) Or (GridView5.FocusedColumn Is colDateOfRegistration) Or (GridView5.FocusedColumn Is colDateRegistrationValidTill) Then
  '            System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
  '        Else
  '            System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
  '        End If
  '    End If

  'End Sub
  'Private Sub GridView5_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView5.GotFocus
  '    GridView5.FocusedColumn = colRegistrationNumber
  'End Sub

  Private Sub VehicleAxesDestinationsGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles VehicleAxesDestinationsGridControl.ProcessGridKey
    If e.KeyCode = Keys.Enter Then
      If GridView4.FocusedRowHandle = GridView4.RowCount - 1 Then
        e.Handled = True
        GridView3.Focus()
        GridView3.FocusedRowHandle = 0
      End If
    End If
  End Sub

  Private Sub VehicleAxesGridControl1_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles VehicleAxesGridControl1.ProcessGridKey
    If e.KeyCode = Keys.Enter Then
      If GridView6.FocusedRowHandle = GridView6.RowCount - 1 Then
        e.Handled = True
        GridView4.Focus()
        GridView4.FocusedRowHandle = 0
      End If
    End If
  End Sub







  Private Sub VerticalBurdenOnTheSeatCheckEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VerticalBurdenOnTheSeatCheckEdit.CheckedChanged
    If VerticalBurdenOnTheSeatCheckEdit.Checked Then
      VerticalBurdenOnTheSeatNoteTextEdit.Enabled = True
    Else
      VerticalBurdenOnTheSeatNoteTextEdit.Enabled = False
      _vehicle.VerticalBurdenOnTheSeatNote = String.Empty
    End If
  End Sub

#Region " LookUpEdit Validate events "
  Private Sub BodyTypeCodeLookUpEdit_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles BodyTypeCodeLookUpEdit.Validated
    If BodyTypeCodeLookUpEdit.EditValue > 0 Then
      'tuka stavi payment categorija
      Try
        _vehicle.IdVehicleCategoryForPayments = VehicleCategoryForPaymentsList.GetVehicleCategoryForPaymentsListByCategory(LookUpEditVehicleType.EditValue, BodyTypeCodeLookUpEdit.EditValue).Item(0).Id

      Catch ex As Exception
        '_vehicle.IdVehicleCategoryForPayments = 0
        MsgBox(ex.Message)
      End Try
      Try
        _useList = VehicleUseList.GetVehicleUseListByCategoryAndBodytype(LookUpEditVehicleType.EditValue, BodyTypeCodeLookUpEdit.EditValue)
        Me.VehicleUseListBindingSource.DataSource = _useList
      Catch ex As Exception

      End Try
      If _useList.Count > 0 Then
        Dim categoryPom As VehicleCategorie = VehicleCategorie.GetVehicleCategorie(_vehicle.IdVehicleCategories)
        For Each child As VehicleCategoriesRelation In categoryPom.Relations
          If child.IdBodytype = _vehicle.IdVehicleBodyType Then
            _vehicle.IdVehicleUse = child.IdUse
            Dim useInfo As VehicleUseInfo = VehicleUseList.GetVehicleUseList.GetUseIById(child.IdUse)
            If (useInfo.RegistrationMask <> String.Empty) Then
              'resetiraj registracii
              'RepositoryItemTextEdit2.Mask.EditMask = DevExpress.XtraEditors.Mask.MaskType.None
              'For Each reg As VehicleRegistration In _vehicle.Registrations
              '  reg.RegistrationNumber = String.Empty
              'Next
              Try
                RepositoryItemTextEdit2.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx
                RepositoryItemTextEdit2.Mask.EditMask = useInfo.RegistrationMask
                'RepositoryItemTextEdit2.Mask.UseMaskAsDisplayFormat = True
              Catch ex As Exception
                RepositoryItemTextEdit2.Mask.EditMask = DevExpress.XtraEditors.Mask.MaskType.None
                MsgBox(ex.Message)
              End Try
            End If
            Exit Sub
          End If
        Next
      End If
    End If
  End Sub
#End Region

#Region " language change "

  'Private Sub GridView3_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView3.FocusedRowChanged
  '    If GridView3.FocusedRowHandle = -2147483647 Then

  '    End If
  'End Sub


  Private Sub ChangeLanguageOnGotFocus(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles EngineNumberTextEdit.GotFocus, ShellNumberTextEdit.GotFocus, _
    CustomLookUpBrakes.GotFocus, GridView3.GotFocus, BodyTypeCodeLookUpEdit.GotFocus, _
    ModelLookUpEdit.GotFocus, PrimaryColorCustomLookUpEdit.GotFocus, SecondaryColorLookUpEdit.GotFocus, _
    RepositoryItemTextEdit2.Enter, tmpVehicleModelTextEdit.GotFocus


    System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
    System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
  End Sub





  'Private Sub ChangeLanguageOnLostFocus(ByVal sender As Object, ByVal e As System.EventArgs) _
  '  Handles EngineNumberTextEdit.LostFocus, ShellNumberTextEdit.LostFocus, _
  '  CustomLookUpBrakes.LostFocus, GridView3.LostFocus, BodyTypeCodeLookUpEdit.LostFocus, _
  '  ModelLookUpEdit.LostFocus, PrimaryColorCustomLookUpEdit.LostFocus, SecondaryColorLookUpEdit.LostFocus, _
  '  RepositoryItemTextEdit2.Leave, tmpVehicleModelTextEdit.LostFocus

  '    System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
  '    System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
  'End Sub

#End Region

#Region " Links "

  Private Sub LookUpEditVehicleType_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) _
    Handles LookUpEditVehicleType.ButtonPressed

    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxVehicleCategories) Then
            par.ShowWinPart(CType(ctl, uxVehicleCategories))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxVehicleCategories)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If

  End Sub

  Private Sub ModelLookUpEdit_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) _
    Handles ModelLookUpEdit.ButtonPressed
    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxVehicleModels) Then

            par.ShowWinPart(CType(ctl, uxVehicleModels))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxVehicleModels)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If
  End Sub

  Private Sub BodyTypeCodeLookUpEdit_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) _
    Handles BodyTypeCodeLookUpEdit.ButtonPressed
    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxVehicleBodytypes) Then
            par.ShowWinPart(CType(ctl, uxVehicleBodytypes))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxVehicleBodytypes)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If
  End Sub

  Private Sub ColorCustomLookUpEdit_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) _
     Handles PrimaryColorCustomLookUpEdit.ButtonPressed, SecondaryColorLookUpEdit.ButtonPressed

    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxColors) Then
            par.ShowWinPart(CType(ctl, uxColors))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxColors)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If
  End Sub


  Private Sub CustomLookUpEngineType_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) _
    Handles CustomLookUpEngineType.ButtonPressed

    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxVehicleEngineTypes) Then
            par.ShowWinPart(CType(ctl, uxVehicleEngineTypes))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxVehicleEngineTypes)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    Else
      If e.Button.Index = 2 Then
        _VehicleEngineTypeList = VehicleEngineTypeList.GetVehicleEngineTypeList
        Me.VehicleEngineTypeListBindingSource.DataSource = _VehicleEngineTypeList
        _engineTypeModelRelation = EngineTypeModelRelation.NewEngineTypeModelRelation
        _engineTypeModelRelation.IdVehicleModel = _vehicle.IdVehicleModel
      End If
    End If
  End Sub


  Private Sub CustomLookUpBrakes_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) _
    Handles CustomLookUpBrakes.ButtonPressed

    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxBrakes) Then
            par.ShowWinPart(CType(ctl, uxBrakes))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxBrakes)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If
  End Sub

  Private Sub CustomLookUpGearBox_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) _
    Handles CustomLookUpGearBox.ButtonPressed

    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxVehicleGearBoxes) Then
            par.ShowWinPart(CType(ctl, uxVehicleGearBoxes))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxVehicleGearBoxes)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If
  End Sub

  Private Sub CustomLookUpSupporting_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) _
    Handles CustomLookUpSupporting.ButtonPressed

    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxVehicleSupporting) Then
            par.ShowWinPart(CType(ctl, uxVehicleSupporting))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxVehicleSupporting)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If

  End Sub

  Private Sub CountryCustomLookUpEdit_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) _
    Handles CountryCustomLookUpEdit.ButtonPressed

    If e.Button.Index = 1 Then
      Dim par As MainForm = Me.ParentForm
      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        For Each ctl As Control In page.Controls
          If (TypeOf ctl Is uxCountries) Then
            par.ShowWinPart(CType(ctl, uxCountries))
            Exit Sub
          End If
        Next
      Next
      Using cekaj As New StatusBusy(My.Resources.txtLoading)
        Try
          par.AddWinPart(New uxCountries)
        Catch ex As Exception
          MsgBox(ex.Message)
        End Try
      End Using
    End If

  End Sub

#End Region


  'Private Sub tmpVehicleModelTextEdit_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmpVehicleModelTextEdit.Validated
  '    If tmpVehicleModelTextEdit.Text <> String.Empty Then

  '        If _vehicleModelList(Me.VehicleModelListBindingSource.Position) IsNot Nothing Then
  '            'stavi nov model vo baza
  '            Dim newModel As VehicleModel = VehicleModel.NewVehicleModel
  '            newModel.IdVehicleMaker = _vehicleModelList.GetVehicleModelInfoById(Me.ModelLookUpEdit.EditValue).IdVehicleMaker '.IdVehicleMaker
  '            newModel.ModelCode = ""
  '            newModel.ModelName = _vehicleModelList.GetVehicleModelInfoById(Me.ModelLookUpEdit.EditValue).ModelName & " " & tmpVehicleModelTextEdit.Text
  '            Me.VehicleModelListBindingSource.RaiseListChangedEvents = False
  '            newModel = newModel.Save
  '            _vehicleModelList = VehicleModelList.GetVehicleModelList()
  '            Me.VehicleModelListBindingSource.DataSource = _vehicleModelList
  '            Me.VehicleModelListBindingSource.RaiseListChangedEvents = True
  '            Me.VehicleModelListBindingSource.ResetBindings(False)
  '            Vehicle.IdVehicleModel = newModel.Id
  '            GetInfoFromModel()
  '        End If

  '        tmpVehicleModelTextEdit.Text = String.Empty
  '    End If
  'End Sub

  Private Sub ModelLookUpEdit_Validated(ByVal sender As Object, ByVal e As System.EventArgs) _
   Handles ModelLookUpEdit.Validated
    GetInfoFromModel()

  End Sub

  Private Sub GetInfoFromModel()
    If (ModelLookUpEdit.EditValue > 0) AndAlso (_vehicle.IsNew) Then
      Dim modInfo As VehicleModelInfo = _vehicleModelList.GetInfo(Me.ModelLookUpEdit.EditValue)
      'zemja na proizvodstov

            _vehicle.IdMadeCountry = modInfo.IdCountry
            '_vehicle.Tip = modInfo.ModelName
      'godina na proizvodstvo
      If modInfo.YearOfBeginingProduction > New Csla.SmartDate(True).Date AndAlso MakeDateDateEdit.DateTime.Date = Now.Date Then
        _vehicle.MakeDate = modInfo.YearOfBeginingProduction
      End If
      'rebind choices
      Me.VehicleEngineTypeListBindingSource.RaiseListChangedEvents = False
      Dim makerId As Integer = _vehicleModelList.GetVehicleModelInfoById(ModelLookUpEdit.EditValue).IdVehicleMaker
      _VehicleEngineTypeList = VehicleEngineTypeList.GetVehicleEngineTypeList(makerId) '(ModelLookUpEdit.EditValue)
      Me.VehicleEngineTypeListBindingSource.DataSource = _VehicleEngineTypeList
      Me.VehicleEngineTypeListBindingSource.RaiseListChangedEvents = True
      Me.VehicleEngineTypeListBindingSource.ResetBindings(False)
    End If
  End Sub



  Private Sub VehiclePersonalTiresGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles VehiclePersonalTiresGridControl.ProcessGridKey
    If GridView3.FocusedRowHandle = GridView3.RowCount - 1 Then
      e.Handled = True
      TrailerWaightWithBreakTextEdit1.Focus()
    End If
  End Sub

  'Private Sub VehiclePersonalTiresGridControl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VehiclePersonalTiresGridControl.Click

  'End Sub

  'Private Sub BodyTypeCodeLookUpEdit_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BodyTypeCodeLookUpEdit.EditValueChanged

  'End Sub

  Private Sub CountryCustomLookUpEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles CountryCustomLookUpEdit.GotFocus
    System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
  End Sub

  Private Sub CustomRepositoryItemLookupEdit1_ProcessNewValue(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles CustomRepositoryItemLookupEdit1.ProcessNewValue
    If e.DisplayValue.ToString = String.Empty Then
      e.Handled = False
      Exit Sub
    End If

    Try
      'Console.WriteLine("da")
      If VehicleTireType.Exists(e.DisplayValue.ToString) = 0 Then
        If MsgBox(e.DisplayValue.ToString & My.Resources.nePostoi, MsgBoxStyle.YesNo, My.Resources.Zacuvaj & "?") = MsgBoxResult.Yes Then

          Dim newTire As VehicleTireType = VehicleTireType.NewVehicleTireType
          newTire.TireType = e.DisplayValue.ToString()
          newTire.Seria = (e.DisplayValue.ToString()).Substring(4, 2)
          newTire.Dimenzions = (e.DisplayValue.ToString()).Substring(7, 2)
          newTire.ApplyEdit()
          newTire = newTire.Save

          Me.VehicleTireTypeListBindingSource.RaiseListChangedEvents = False

          Me.VehicleTireTypeListBindingSource.DataSource = VehicleTireTypeList.GetVehicleTireTypeList
          Me.VehicleTireTypeListBindingSource.RaiseListChangedEvents = True
          Me.VehicleTireTypeListBindingSource.ResetBindings(False)
        End If
        e.Handled = True
      End If
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

    e.Handled = True
  End Sub

  Private Sub GridView3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridView3.KeyDown
    If e.KeyCode = Keys.Tab Then
      TrailerWaightWithBreakTextEdit1.Focus()
    End If
  End Sub


  Private Sub LookUpEditVehicleType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditVehicleType.GotFocus
    System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
  End Sub

#Region " Scan "
  Private _attachments As Attachments
  Private _attachmentsByVehicle As Attachments
  Private _attachmentsTypeList As AttachmentTypeList
  Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
    If _vehicle.Id > 0 Then
      AttachmentTab.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible
      _attachments = Attachments.GetAttachments
      _attachmentsByVehicle = Attachments.GetAttachmentByIdCustomer(_vehicle.Id, False)
      _attachmentsTypeList = AttachmentTypeList.GetAttachmentTypeList
      AttachmentsBindingSource.DataSource = _attachmentsByVehicle
      AttachmentTypeListBindingSource.DataSource = _attachmentsTypeList
    Else
      AttachmentTab.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden
      MsgBox("Возилото мора прво да биде зачувано")
    End If
  End Sub

  Private Sub AttachmentsBindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles AttachmentsBindingSource.ListChanged
    If _attachmentsByVehicle IsNot Nothing Then
      Dim tmp As Boolean = (_attachmentsByVehicle.Count > 0)
      Me.btnDelete.Enabled = tmp
      Me.btnEdit.Enabled = tmp
    End If
  End Sub

  Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click

    Dim att As Attachment = _attachments.AddNew
    att.IdVehicle = _vehicle.Id
    Dim dijT As New dijTwain(att)

    If dijT.ShowDialog(Me) = DialogResult.OK Then
      _attachments.Save()
      _attachmentsByVehicle = Attachments.GetAttachmentByIdCustomer(_vehicle.Id, False)
      AttachmentsBindingSource.DataSource = _attachmentsByVehicle
    Else
      'cancel edit na att
      '_attachments.Remove(att)
    End If

  End Sub

  Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
    Dim att As Attachment = (CType(Me.AttachmentsBindingSource.Current, Attachment))
    Dim i As Integer = 0
    For Each attach As Attachment In _attachmentsByVehicle
      If att.Id = attach.Id Then
        'UnbindBindingSource (AttachmentsBindingSource ,True ,True )
        AttachmentsBindingSource.DataSource = Nothing
        _attachmentsByVehicle.Item(i).IdVehicle = 0
        _attachmentsByVehicle.Save()
        Exit For
      Else
        i += 1
      End If
    Next
    AttachmentsBindingSource.DataSource = Attachments.GetAttachmentByIdCustomer(_vehicle.Id, False)


    'Me.AttachmentsBindingSource.RemoveCurrent()

  End Sub

  Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click, RepositoryItemPictureEdit1.DoubleClick

    Try
      Dim att As Attachment
      Using bus As New Splash(My.Resources.LoadingData)
        att = _attachmentsByVehicle(Me.AttachmentsBindingSource.Position)
      End Using


      Dim dijT As New dijTwain(att)
      dijT.ShowDialog()
    Catch ex As Exception

    End Try
  End Sub

#End Region

  'Private Sub CustomLookUpEditUse_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomLookUpEditUse.EditValueChanged
  '    Dim mask As String = objVehicleUseList.GetUseIById(_vehicle.IdVehicleUse).RegistrationMask
  '    Me.LastRegistratinNumberTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx
  '    'Me.FirstRegistrationNumberTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx
  '    If mask <> "" Then
  '        Me.LastRegistratinNumberTextEdit.Properties.Mask.EditMask = mask
  '        Me.FirstRegistrationNumberTextEdit.Properties.Mask.EditMask = mask
  '    Else
  '        Me.LastRegistratinNumberTextEdit.Properties.Mask.EditMask = "([A-Z0-9^Ž^Đ^Š^Č][A-Z0-9^Ž^Đ^Š^Č]?[A-Z0-9^Ž^Đ^Š^Č])-([0-9][0-9][0-9])-([[A-Z^Ž^Đ^Š^Č]][[A-Z^Ž^Đ^Š^Č]]?[[A-Z^Ž^Đ^Š^Č]])"
  '        '  Me.FirstRegistrationNumberTextEdit.Properties.Mask.EditMask = "([A-Z0-9][A-Z0-9]?[A-Z0-9])-([0-9][0-9][0-9])-([A-Z0-9][A-Z0-9]?[A-Z0-9])"
  '    End If

  'End Sub

  Private Sub LastRegistrationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LastRegistratinNumberTextEdit.EditValueChanged, _
   LastRegistrationMakeDateDateEdit.EditValueChanged, LastRegistrationValidTillDateEdit.EditValueChanged
    If _vehicle Is Nothing Then
      _lastRegChanged = False
    Else
      _lastRegChanged = True
    End If
  End Sub

  Private Sub FirstRegistrationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
  FirstRegistrationMakeDateDateEdit.EditValueChanged, FirstRegistrationNumberTextEdit.EditValueChanged, FirstRegistrationValidTillDateEdit.EditValueChanged
    If _vehicle Is Nothing Then
      _firstRegChanged = False
    Else
      _firstRegChanged = True
    End If

  End Sub

  ' Private Sub LookUpEdit1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEdit1.LostFocus
  'LastRegistratinNumberTextEdit.Focus()
  ' End Sub

  Private Sub btnAllRegs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllRegs.Click
    Dim forma As uxVehicleRegList = New uxVehicleRegList(_vehicle.Id)
    forma.ShowDialog()

  End Sub

  Private Sub FirstRegistrationNumberTextEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles FirstRegistrationNumberTextEdit.GotFocus, LastRegistratinNumberTextEdit.GotFocus, _
  FirstRegistrationMakeDateDateEdit.GotFocus, FirstRegistrationValidTillDateEdit.GotFocus, _
  LastRegistrationMakeDateDateEdit.GotFocus, LastRegistrationValidTillDateEdit.GotFocus
    System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
  End Sub


  Private Sub FirstRegistrationNumberTextEdit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles FirstRegistrationNumberTextEdit.LostFocus, LastRegistratinNumberTextEdit.LostFocus, _
  FirstRegistrationMakeDateDateEdit.LostFocus, FirstRegistrationValidTillDateEdit.LostFocus, _
  LastRegistrationMakeDateDateEdit.LostFocus, LastRegistrationValidTillDateEdit.LostFocus
    System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
  End Sub
  'Private Sub FirstRegistrationMakeDateDateEditEditValueChanged() Handles _vehicle.PropertyChanged 'FirstRegistrationMakeDateDateEdit.EditValueChanged
  '    Try

  '        If FirstRegistrationMakeDateDateEdit.EditValue IsNot Nothing Then
  '            _vehicle.FirstRegistrationValidTill = CType(FirstRegistrationMakeDateDateEdit.EditValue, Date).AddYears(1)
  '        End If
  '    Catch ex As Exception

  '    End Try

  '    'FirstRegistrationValidTillDateEdit.EditValue = CType(FirstRegistrationMakeDateDateEdit.EditValue, Date).AddYears(1)
  'End Sub

  Private Sub _vehicle_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _vehicle.PropertyChanged
    Select Case e.PropertyName
      Case "FirstRegistrationMakeDate"
        _vehicle.FirstRegistrationValidTill = CType(_vehicle.FirstRegistrationMakeDate, Date).AddYears(1)
      Case "LastRegistrationMakeDate"
        _vehicle.LastRegistrationValidTill = CType(_vehicle.LastRegistrationMakeDate, Date).AddYears(1)
    End Select

  End Sub


  Private Sub LookUpEdit2_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEdit2.Validated
    _vehicle.LastIdRegistrationIssuer = LookUpEdit2.EditValue
  End Sub

  Private Sub ShellNumberTextEdit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ShellNumberTextEdit.KeyDown
    If e.KeyData = Keys.Q Or e.KeyData = Keys.O Or e.KeyData = Keys.I Then
      e.SuppressKeyPress = True
    End If
  End Sub
  Private Sub ShellNumberTextEdit_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShellNumberTextEdit.Validated
    If ShellNumberTextEdit.Text <> String.Empty AndAlso _vehicle.IsNew Then
      Dim pom As Vehicle
      Dim pomString As String = ShellNumberTextEdit.Text
      Try
        pom = Vehicle.GetVehicleByShellNum(ShellNumberTextEdit.Text)
      Catch ex As Exception
        pom = Nothing
      End Try

      If pom IsNot Nothing AndAlso pom.Id > 0 Then
        _vehicle = pom

      Else
        ' _vehicle = Vehicle.NewVehicle
        _vehicle.ShellNumber = pomString
        FirstRegistrationNumberTextEdit.Text = ""
        LastRegistratinNumberTextEdit.Text = ""
      End If
      Me.VehicleBindingSource.DataSource = _vehicle
    End If
  End Sub

  
End Class
