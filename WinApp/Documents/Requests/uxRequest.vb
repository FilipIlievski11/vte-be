Public Class uxRequest

    '  Private WithEvents _documentTypes As DocumentType
    '  Private WithEvents _customer As Customer
    '  Private WithEvents _vehicle As Vehicle
    '  Private WithEvents _document As Document
    '  Private WithEvents _documentTypesList As DocumentTypesList
    '  Private WithEvents _documentTypeOption As DocumentTypesOptionsInfo
    '  Private WithEvents _documentTypeOptionDetail As DocumentTypesOptionsDetailsInfo
    '  Private WithEvents _customerVehicleRelationsList As CustomerVehiclesRelationsList
    '  Private WithEvents _options As DocumentTypesOptions
    '  Private WithEvents _oldCustomerVehicleRelation As CustomerVehiclesRelation
    '  Private WithEvents _newCustomerVehicleRelation As CustomerVehiclesRelation = Nothing
    '  Private WithEvents _businessTypeList As BusinessTypeList
    '  Private WithEvents _relationList As CustomerVehiclesRelationsList
    '    Private WithEvents _vehicleList As VehicleListShort
    '  Private WithEvents _attachmentTypeList As AttachmentTypeList
    '  Private WithEvents _tehExamOrganizations As TehnicalExamOrganizationsList
    '  Private WithEvents _docVehicleOwnershipProofList As DocumentVehicleOwnershipProofList
    '  Private WithEvents _docPaymentProofList As DocumentPaymentProofList
    '  Private _promenetaTehnickaSostojba As Boolean = False
    '  '  Private WithEvents _newTehExam As DocumentsTehnicalExamsReport = DocumentsTehnicalExamsReport.NewDocumentsTehnicalExamsReport

    '#Region "PritisnatoKopce"


    '  Private Sub uxVehicleCategorie_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    '    Select Case Asc(e.KeyChar)
    '      Case 13
    '        SendKeys.Send("{TAB}")
    '    End Select
    '  End Sub
    '#End Region

    '#Region "WinPart"

    '  Protected Overrides Function GetIdValue() As Object
    '    Return My.Resources.uxRequests
    '  End Function

    '  Public Overrides Function ToString() As String
    '    Return My.Resources.uxRequests
    '  End Function

    '#End Region


    '  Public Sub New(ByVal docTypeIn As DocumentType, ByVal docIn As Document)

    '    ' This call is required by the Windows Form Designer.
    '    InitializeComponent()


    '    _documentTypes = docTypeIn
    '    _document = docIn

    '    LoadList()
    '    BindUICustomer()
    '    BindUIVehicle()
    '    BindUIDocument()

    '    _document.IdDocumentType = _documentTypes.Id

    '    Me.DocumentBindingSource.DataSource = _document
    '    If Not _document.IsNew Then
    '      If _document.IdCustomerVehicleRelationHistory > 0 Then
    '        _oldCustomerVehicleRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(_document.IdCustomerVehicleRelationHistory)
    '        _newCustomerVehicleRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(_document.IdCustomerVehicleRelation)
    '        _vehicle = Vehicle.GetVehicle(_oldCustomerVehicleRelation.IdVehicle)
    '        _customer = Customer.GetCustomer(_oldCustomerVehicleRelation.IdCustomer)
    '        LookUpEditNewOwner.EditValue = _newCustomerVehicleRelation.IdCustomer
    '        ControlGroupLastTehnicalExam.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    '      Else
    '        _oldCustomerVehicleRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(_document.IdCustomerVehicleRelation)
    '        _vehicle = Vehicle.GetVehicle(_oldCustomerVehicleRelation.IdVehicle)
    '        _customer = Customer.GetCustomer(_oldCustomerVehicleRelation.IdCustomer)
    '      End If

    '    End If
    '    ' Add any initialization after the InitializeComponent() call.
    '    RadioGroup1.Focus()

    '  End Sub
    '  Public ReadOnly Property Document() As Document
    '    Get
    '      Return _document
    '    End Get
    '  End Property

    '  Private Sub LoadList()
    '    _relationList = CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsList

    '    Me.CustomersListBindingSource.DataSource = CustomersList.GetCustomersList
    '        _vehicleList = objVehicleListShort 'VehicleList.GetVehicleList
    '    Me.VehicleListBindingSource.DataSource = _vehicleList
    '    _documentTypesList = DocumentTypesList.GetDocumentTypesList
    '    Me.DocumentTypesListBindingSource.DataSource = _documentTypesList
    '    Dim pomBrTipovi As Integer = DocumentTypesList.GetDocumentTypesList.Count
    '    If _documentTypes.Id = (DocumentTypes.GetDocumentTypes.Item(pomBrTipovi - 1).Id) Then
    '      _promenetaTehnickaSostojba = True
    '    Else
    '      _promenetaTehnickaSostojba = False
    '    End If
    '    Me.CityListBindingSource.DataSource = CityList.GetCityList
    '    _customerVehicleRelationsList = CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsList
    '    Me.CustomerVehiclesRelationsListBindingSource.DataSource = _customerVehicleRelationsList
    '    Me.StreetsListBindingSource.DataSource = StreetsList.GetStreetsList
    '    _tehExamOrganizations = TehnicalExamOrganizationsList.GetTehnicalExamOrganizationsListBezMomentalna(objOpcii.Company)
    '    Me.TehnicalExamOrganizationsListBindingSource.DataSource = _tehExamOrganizations 'TehnicalExamOrganizationsList.GetTehnicalExamOrganizationsList
    '    _businessTypeList = BusinessTypeList.GetBusinessTypeList
    '    Me.BusinessTypeListBindingSource.DataSource = _businessTypeList

    '    Me.VehicleCategoryListBindingSource.DataSource = VehicleCategoryList.GetVehicleCategoryList
    '    Me.VehicleEnginePowerSourceTypeListBindingSource.DataSource = VehicleEnginePowerSourceTypeList.GetVehicleEnginePowerSourceTypeList

    '    _docVehicleOwnershipProofList = DocumentVehicleOwnershipProofList.GetDocumentVehicleOwnershipProofList
    '    Me.DocumentVehicleOwnershipProofListBindingSource.DataSource = _docVehicleOwnershipProofList
    '    _docPaymentProofList = DocumentPaymentProofList.GetDocumentPaymentProofList
    '    Me.DocumentPaymentProofListBindingSource.DataSource = _docPaymentProofList

    '    _attachmentTypeList = AttachmentTypeList.GetAttachmentTypeList
    '    Me.AttachmentTypeListBindingSource.DataSource = _attachmentTypeList

    '  End Sub

    '  Private Sub LoadDependantList()
    '    'inicijalizacija na zavisni listi
    '    If (_document.IdCustomerVehicleRelation > 0) AndAlso (LookUpEditVehicle.EditValue IsNot Nothing) AndAlso (LookUpEditVehicle.EditValue > 0) Then
    '      Me.VehicleRegistrationListBindingSource.DataSource = _
    '      VehicleRegistrationList.GetVehicleRegistrationList(LookUpEditVehicle.EditValue)
    '    End If

    '  End Sub

    '  Private Sub uxRequest_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    LookUpEditDocumentType.EditValue = _documentTypes.Id
    '    If _documentTypes.Id = DocumentTypesList.GetDocumentTypesList.Item(2).Id AndAlso _document.IsNew Then
    '      _vehicleList = VehicleList.GetVehicleListOdjaveniVozila(100)
    '      Me.VehicleListBindingSource.DataSource = _vehicleList
    '    End If

    '    _options = DocumentTypes.GetDocumentTypes.GetDocumentTypesById _
    '(LookUpEditDocumentType.EditValue).DocumentTypesOptions

    '    LayoutControlItemRadioGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    '    LayoutRadioGroup2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    '    For i = 0 To _options.Count - 1
    '      Dim pom As DevExpress.XtraEditors.Controls.RadioGroupItem = _
    '      New DevExpress.XtraEditors.Controls.RadioGroupItem
    '      pom.Description = _options.Item(i).OptionName
    '      pom.Value = False 'CheckState.Unchecked
    '      Me.RadioGroup1.Properties.Items.Add(pom)
    '    Next
    '    If Not _document.IsNew Or _document.IdDocumentTypeOption > 0 Then
    '      If Not _document.IsNew Then

    '        Dim pom As CustomerVehiclesRelation = _oldCustomerVehicleRelation 'CustomerVehiclesRelation.GetCustomerVehiclesRelation(_document.IdCustomerVehicleRelation)
    '        LookUpEditCustomer.EditValue = pom.IdCustomer
    '        LookUpEditVehicle.EditValue = pom.IdVehicle
    '        _customer = Customer.GetCustomer(pom.IdCustomer)
    '        _vehicle = Vehicle.GetVehicle(pom.IdVehicle)
    '        Me.CustomerBindingSource.DataSource = _customer
    '        Me.VehicleBindingSource.DataSource = _vehicle
    '        _documentTypeOption = DocumentTypesOptionsList.GetDocumentTypesOptionsList.GetDocumentTypesOptionsListById _
    '     (_document.IdDocumentTypeOption)
    '        If _documentTypeOption.IsTehnicalExamRquired Then
    '          If Not _vehicle.VehicleLastTehnicalExams.Count > 0 Then
    '            btnPrint.Enabled = False
    '          Else
    '            btnPrint.Enabled = True
    '          End If
    '        Else
    '          btnPrint.Enabled = True
    '        End If
    '        LookUpEditVehicle.Focus()
    '      End If
    '      If _document.IdDocumentTypeOption > 0 Then
    '        LayoutControlItemRadioGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    '        Dim optionName As String = DocumentTypesOptionsList.GetDocumentTypesOptionsList. _
    '        GetDocumentTypesOptionsListById(_document.IdDocumentTypeOption).OptionName
    '        Dim i As Integer = 0
    '        For Each item As DevExpress.XtraEditors.Controls.RadioGroupItem In RadioGroup1.Properties.Items
    '          If item.Description = optionName Then
    '            item.Value = True
    '            RadioGroup1.SelectedIndex = i
    '            Exit For
    '          End If
    '          i += 1
    '        Next
    '        RadioGroup1.Properties.ReadOnly = True
    '        If RadioGroup2.Visible Then
    '          RadioGroup2.SelectedIndex = 0
    '          RadioGroup2.Focus()
    '        Else
    '          LookUpEditVehicle.Focus()
    '        End If
    '      End If
    '    Else
    '      RadioGroup1.Focus()
    '      RadioGroup1.SelectedIndex = 0
    '    End If


    '    LoadDependantList()
    '  End Sub

    '  Private Sub RadioGroup1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.Leave
    '    If RadioGroup2.Visible Then
    '      RadioGroup2.Focus()
    '      RadioGroup2.SelectedIndex = 0
    '    Else
    '      LookUpEditVehicle.Focus()
    '    End If
    '  End Sub

    '  'Private Sub RadioGroup1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.LostFocus
    '  '  'If Me.Focused Then
    '  '  If RadioGroup2.Visible Then
    '  '    RadioGroup2.Focus()
    '  '    RadioGroup2.SelectedIndex = 0
    '  '  Else
    '  '    LookUpEditVehicle.Focus()
    '  '  End If
    '  '  'Else
    '  '  'Exit Sub
    '  '  'End If
    '  'End Sub



    '  Private Sub RadioGroup1_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
    '    If RadioGroup1.SelectedIndex = 1 AndAlso (_documentTypes.Id = _documentTypesList.Item(1).Id) Then
    '      PreviousRegistrationLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    '    Else
    '      PreviousRegistrationLayoutControlGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    '    End If
    '    If _promenetaTehnickaSostojba AndAlso RadioGroup1.SelectedIndex = 1 Then
    '      GroupVehicle.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    '    Else
    '      GroupVehicle.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    '    End If
    '    If _document.IsNew AndAlso _documentTypes.Id = DocumentTypes.GetDocumentTypes(1).Id Then '7 Then
    '      _vehicleList = VehicleList.GetVehicleList
    '      Me.VehicleListBindingSource.DataSource = _vehicleList
    '      Select Case RadioGroup1.SelectedIndex
    '        'Case 0
    '        '  '_vehicleList = VehicleList.GetVehicleList
    '        '  'Me.VehicleListBindingSource.DataSource = _vehicleList
    '        '  Dim par As MainForm = Me.ParentForm
    '        '  Using cekaj As New StatusBusy(My.Resources.txtLoading)
    '        '    Try
    '        '      par.AddWinPart(New uxVehicle(Vehicle.NewVehicle))
    '        '    Catch ex As Exception
    '        '      MsgBox(ex.Message)
    '        '    End Try
    '        '  End Using
    '        Case 1
    '          '_vehicleList = VehicleList.GetVehicleList
    '          'Me.VehicleListBindingSource.DataSource = _vehicleList
    '          _vehicleList = VehicleList.GetVehicleListOdjaveniVozila(1)
    '          Me.VehicleListBindingSource.DataSource = _vehicleList

    '        Case 0, 2
    '          _vehicleList = VehicleList.GetVehicleList
    '          Me.VehicleListBindingSource.DataSource = _vehicleList
    '      End Select
    '    End If
    '    ControlGroupCurrentOwnership.Enabled = True
    '    ControlGroupLastTehnicalExam.Enabled = True
    '    Dim docTypeOption As DocumentTypesOption = _documentTypes.DocumentTypesOptions.Item(RadioGroup1.SelectedIndex)
    '    _documentTypeOption = DocumentTypesOptionsList.GetDocumentTypesOptionsList.GetDocumentTypesOptionsListById _
    '     (docTypeOption.Id)
    '    ClearOptions()

    '    'Dim dokOption As DocumentTypes = DocumentTypes.GetDocumentTypes
    '    'If dokOption(0).Id = _document.IdDocumentType Then

    '    'End If
    '    Select Case _documentTypeOption.Id
    '      Case 1
    '        Me.GroupVehicle.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    '        'vo detali
    '      Case 4, 7, 2
    '        RegistrationNumberTextEdit.Enabled = True
    '      Case 8
    '        '?

    '      Case 9
    '        LookUpEditAddress.Enabled = True
    '        LivingAddressNumberTextEdit.Enabled = True
    '        LookUpEditCity.Enabled = True
    '    End Select
    '    If _documentTypeOption.IsTehnicalExamRquired AndAlso _document.IsNew Then
    '      ControlGroupLastTehnicalExam.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    '    Else
    '      'ova
    '      ControlGroupLastTehnicalExam.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    '    End If
    '    RadioGroup2.Properties.Items.Clear()
    '    Dim optionsDetails As DocumentTypesOptionDetails = docTypeOption.Details
    '    If optionsDetails.Count = 0 Then
    '      LayoutRadioGroup2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    '      'If _documentTypeOption.RelationDeleted Then
    '      '  ControlGroupNewOwner.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    '      '  ControlGroupNewOwner.Enabled = True
    '      'Else
    '      If _document.IdDocumentType = DocumentTypes.GetDocumentTypes(1).Id AndAlso _
    '      RadioGroup1.SelectedIndex = 1 Then '_document.IdDocumentTypeOption = _documentTypes.DocumentTypesOptions(1).Id 
    '        ControlGroupNewOwner.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    '        ControlGroupNewOwner.Enabled = True
    '      Else
    '        ControlGroupNewOwner.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    '      End If

    '      'End If

    '    Else
    '      LayoutRadioGroup2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    '      ControlGroupNewOwner.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    '      For i = 0 To optionsDetails.Count - 1
    '        Dim pom As DevExpress.XtraEditors.Controls.RadioGroupItem = _
    '        New DevExpress.XtraEditors.Controls.RadioGroupItem
    '        pom.Description = optionsDetails.Item(i).Name
    '        pom.Value = False
    '        Me.RadioGroup2.Properties.Items.Add(pom)
    '      Next
    '    End If
    '    If Not _document.IsNew Then
    '      If _document.IdDocumentTypeOptionDetail > 0 Then
    '        LayoutRadioGroup2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    '        Dim optionDetailName As String = DocumentTypesOptionsDetailsList.GetDocumentTypesOptionsDetailsList. _
    '       GetDocumentTypesOptionsDetailById(_document.IdDocumentTypeOptionDetail).Name
    '        Dim i As Integer = 0
    '        For Each item As DevExpress.XtraEditors.Controls.RadioGroupItem In RadioGroup2.Properties.Items
    '          If item.Description = optionDetailName Then
    '            item.Value = True
    '            RadioGroup2.SelectedIndex = i
    '            Exit For
    '          End If
    '          i += 1
    '        Next
    '      End If
    '    Else
    '      'If _document.IdDocumentTypeOptionDetail > 0 Then
    '      '  RadioGroup2.Focus()
    '      'End If
    '    End If
    '  End Sub




    '  Private Sub LookUpEditCustomer_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditCustomer.ButtonPressed
    '    If e.Button.Index = 1 Then
    '      Dim par As MainForm = Me.ParentForm
    '      Using cekaj As New StatusBusy(My.Resources.txtLoading)
    '        Try
    '          par.AddWinPart(New uxCustomers(Customer.NewCustomer))
    '        Catch ex As Exception
    '          MsgBox(ex.Message)
    '        End Try
    '      End Using
    '    Else
    '      If e.Button.Index = 2 Then
    '        Dim dij As New dijCustomersList()
    '        If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '          LookUpEditCustomer.EditValue = dij.Customer.Id
    '        End If
    '      End If
    '    End If
    '  End Sub





    '  Private Sub RebindUIVehicle(ByVal saveObject As Boolean, ByVal rebind As Boolean)
    '    ' stop the flow of events
    '    Me.VehicleBindingSource.RaiseListChangedEvents = False
    '    Me.VehicleLastTehnicalExamBindingSource.RaiseListChangedEvents = False
    '    ' commit edits in memory
    '    UnbindBindingSource(Me.VehicleLastTehnicalExamBindingSource, saveObject, False)
    '    UnbindBindingSource(Me.VehicleBindingSource, saveObject, True)

    '    Me.VehicleLastTehnicalExamBindingSource.DataSource = Me.VehicleBindingSource
    '    Try
    '      ' save or cancel changes
    '      If saveObject Then
    '        _vehicle.ApplyEdit()
    '        Try
    '          _vehicle = _vehicle.Save

    '        Catch ex As Csla.DataPortalException
    '          MessageBox.Show(ex.BusinessException.ToString(), _
    '            "Error saving", MessageBoxButtons.OK, _
    '            MessageBoxIcon.Exclamation)

    '        Catch ex As Exception
    '          MessageBox.Show(ex.ToString(), _
    '            "Error Saving", MessageBoxButtons.OK, _
    '            MessageBoxIcon.Exclamation)
    '        End Try
    '      Else
    '        _vehicle.CancelEdit()
    '      End If
    '    Finally
    '      'rebind UI if requested
    '      If rebind Then
    '        BindUIVehicle()
    '      End If

    '      ' restore events
    '      Me.VehicleBindingSource.RaiseListChangedEvents = True
    '      Me.VehicleLastTehnicalExamBindingSource.RaiseListChangedEvents = True

    '      If rebind Then
    '        ' refresh the UI if rebinding
    '        Me.VehicleBindingSource.ResetBindings(False)
    '        Me.VehicleLastTehnicalExamBindingSource.ResetBindings(False)
    '      End If
    '    End Try

    '  End Sub
    '  Private Sub RebindUICustomer(ByVal saveObject As Boolean, ByVal rebind As Boolean)
    '    ' stop the flow of events

    '    Me.CustomerBindingSource.RaiseListChangedEvents = False
    '    ' commit edits in memory
    '    UnbindBindingSource(Me.CustomerBindingSource, saveObject, True)
    '    Try
    '      ' save or cancel changes
    '      If saveObject Then
    '        _customer.ApplyEdit()

    '        Try
    '          _customer = _customer.Save
    '        Catch ex As Csla.DataPortalException
    '          MessageBox.Show(ex.BusinessException.ToString(), _
    '            "Error saving", MessageBoxButtons.OK, _
    '            MessageBoxIcon.Exclamation)

    '        Catch ex As Exception
    '          MessageBox.Show(ex.ToString(), _
    '            "Error Saving", MessageBoxButtons.OK, _
    '            MessageBoxIcon.Exclamation)
    '        End Try
    '      Else
    '        _customer.CancelEdit()
    '      End If
    '    Finally
    '      'rebind UI if requested
    '      If rebind Then
    '        BindUICustomer()
    '      End If

    '      ' restore events

    '      Me.CustomerBindingSource.RaiseListChangedEvents = True

    '      If rebind Then
    '        ' refresh the UI if rebinding
    '        Me.CustomerBindingSource.ResetBindings(False)

    '      End If
    '    End Try

    '  End Sub

    '  Private Sub BindUICustomer()
    '    Try
    '      _customer.BeginEdit()
    '      Me.CustomerBindingSource.DataSource = _customer
    '    Catch
    '    End Try
    '  End Sub
    '  Private Sub BindUIVehicle()
    '    Try
    '      _vehicle.BeginEdit()
    '      Me.VehicleBindingSource.DataSource = _vehicle
    '    Catch
    '    End Try
    '  End Sub
    '  Private Sub BindUIDocument()
    '    Try
    '      _document.BeginEdit()
    '      Me.DocumentBindingSource.DataSource = _document
    '    Catch ex As Exception
    '    End Try
    '  End Sub

    '  Private Sub RebindUIDocument(ByVal saveObject As Boolean, ByVal rebind As Boolean)
    '    ' stop the flow of events
    '    Me.DocumentBindingSource.RaiseListChangedEvents = False
    '    Me.AttachmentsBindingSource.RaiseListChangedEvents = False
    '    ' commit edits in memory
    '    UnbindBindingSource(Me.AttachmentsBindingSource, saveObject, False)
    '    UnbindBindingSource(Me.DocumentBindingSource, saveObject, True)
    '    Me.AttachmentsBindingSource.DataSource = Me.Document
    '    Try
    '      ' save or cancel changes
    '      If saveObject Then






    '        'If LookUpEditNewRelation.EditValue > 0 Then
    '        '  _document.IdCustomerVehicleRelationHistory = LookUpEditRelation.EditValue
    '        '  _document.IdCustomerVehicleRelation = LookUpEditNewRelation.EditValue
    '        'Else
    '        '  _document.IdCustomerVehicleRelation = LookUpEditRelation.EditValue
    '        'End If
    '        _document.IdDocumentType = LookUpEditDocumentType.EditValue

    '        If DocumentTypes.GetDocumentTypes.GetDocumentTypesById(LookUpEditDocumentType.EditValue).DocumentTypesOptions.Count > 0 Then
    '          _document.IdDocumentTypeOption = DocumentTypes.GetDocumentTypes. _
    '          GetDocumentTypesById(LookUpEditDocumentType.EditValue).DocumentTypesOptions. _
    '          Item(RadioGroup1.SelectedIndex).Id
    '        End If
    '        If DocumentTypes.GetDocumentTypes. _
    '               GetDocumentTypesById(LookUpEditDocumentType.EditValue).DocumentTypesOptions. _
    '               Item(RadioGroup1.SelectedIndex).Details.Count > 0 Then
    '          Try
    '            _document.IdDocumentTypeOptionDetail = DocumentTypes.GetDocumentTypes. _
    '                  GetDocumentTypesById(LookUpEditDocumentType.EditValue).DocumentTypesOptions. _
    '                  Item(RadioGroup1.SelectedIndex).Details.Item(RadioGroup2.SelectedIndex).Id
    '          Catch ex As Exception
    '            MsgBox("Изберете вид на промена")
    '            Exit Sub
    '          End Try

    '        End If
    '        _document.ApplyEdit()
    '        Try
    '          '_document.IdCustomerVehicleRelation = _oldCustomerVehicleRelation.Id
    '          If _newCustomerVehicleRelation IsNot Nothing Then
    '            Dim pomNewRel As CustomerVehiclesRelation = _newCustomerVehicleRelation.Save()
    '            _document.IdCustomerVehicleRelationHistory = _document.IdCustomerVehicleRelation
    '            _document.IdCustomerVehicleRelation = pomNewRel.Id
    '            LookUpEditNewRelation.EditValue = pomNewRel.Id
    '          End If
    '          _document = _document.Save
    '        Catch ex As Csla.DataPortalException
    '          MessageBox.Show(ex.BusinessException.ToString(), _
    '            "Error saving", MessageBoxButtons.OK, _
    '            MessageBoxIcon.Exclamation)
    '        Catch ex As Exception
    '          MessageBox.Show(ex.ToString(), _
    '            "Error Saving", MessageBoxButtons.OK, _
    '            MessageBoxIcon.Exclamation)
    '        End Try
    '      Else
    '        _document.CancelEdit()
    '      End If
    '    Finally
    '      'rebind UI if requested
    '      If rebind Then
    '        BindUIDocument()
    '      End If

    '      ' restore events
    '      Me.DocumentBindingSource.RaiseListChangedEvents = True
    '      Me.AttachmentsBindingSource.RaiseListChangedEvents = True
    '      If rebind Then
    '        ' refresh the UI if rebinding
    '        Me.DocumentBindingSource.ResetBindings(False)
    '        Me.AttachmentsBindingSource.ResetBindings(False)
    '      End If
    '    End Try

    '  End Sub



    '  Private Sub LookUpEditOrganization_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditOrganization.ButtonPressed
    '    If e.Button.Index = 1 Then
    '      Dim par As MainForm = Me.ParentForm
    '      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
    '        For Each ctl As Control In page.Controls
    '          If (TypeOf ctl Is uxTehnicalExamOrganizations) Then
    '            par.ShowWinPart(CType(ctl, uxTehnicalExamOrganizations))
    '            Exit Sub
    '          End If
    '        Next
    '      Next
    '      Using cekaj As New StatusBusy(My.Resources.txtLoading)
    '        Try
    '          par.AddWinPart(New uxTehnicalExamOrganizations)
    '        Catch ex As Exception
    '          MsgBox(ex.Message)
    '        End Try
    '      End Using
    '    End If
    '  End Sub

    '  Private Sub LookUpEditNewOwner_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditNewOwner.ButtonPressed
    '    If e.Button.Index = 1 Then
    '      Dim par As MainForm = Me.ParentForm
    '      Using cekaj As New StatusBusy(My.Resources.txtLoading)
    '        Try
    '          par.AddWinPart(New uxCustomers(Customer.NewCustomer))
    '        Catch ex As Exception
    '          MsgBox(ex.Message)
    '        End Try
    '      End Using
    '    Else
    '      If e.Button.Index = 2 Then
    '        Dim dij As New dijCustomersList()
    '        If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '          LookUpEditNewOwner.EditValue = dij.Customer.Id
    '        End If
    '      End If
    '    End If
    '  End Sub

    '  'Private Sub _customer_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _customer.PropertyChanged, _vehicle.PropertyChanged
    '  '  btnSave.Enabled = _customer.IsSavable Or _vehicle.IsSavable

    '  'End Sub





    '  Private Sub LookUpEditNewOwner_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditNewOwner.Validated
    '    If LookUpEditNewOwner.EditValue > 0 Then
    '      'Dim pom As CustomerVehicleRelationType = CustomerVehicleRelationType.GetCustomerVehicleRelationType(1)
    '      'Dim pomRelation As Long = pom.ZemiRelationId(LookUpEditNewOwner.EditValue, LookUpEditVehicle.EditValue)
    '      'If pomRelation > 0 Then
    '      '  LookUpEditNewRelation.EditValue = pomRelation
    '      '  _newCustomerVehicleRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(pomRelation)
    '      'Else
    '      If LookUpEditCustomer.EditValue = LookUpEditNewOwner.EditValue Then
    '        MsgBox("Изберете нов сопственик")
    '        LookUpEditNewOwner.EditValue = Nothing
    '        LookUpEditNewOwner.Focus()
    '        Exit Sub
    '      End If
    '      _newCustomerVehicleRelation = CustomerVehiclesRelation.NewCustomerVehiclesRelation
    '      _newCustomerVehicleRelation.StartDate = Now.AddYears(10)
    '      _newCustomerVehicleRelation.IdCustomer = LookUpEditNewOwner.EditValue
    '      _newCustomerVehicleRelation.IdVehicle = LookUpEditVehicle.EditValue
    '      _newCustomerVehicleRelation.IdRelationType = 1
    '      'End If

    '    End If
    '  End Sub


    '#Region "Kopcinja"

    '  Private Sub UxDokumentsRelations1_btnPrint_Pressed(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Dim dokType As DocumentType = DocumentTypes.GetDocumentTypes.GetDocumentTypesById _
    '    (LookUpEditDocumentType.EditValue)

    '    If _document.Id = 0 Then
    '      MsgBox("Пред да го печатите документот, притиснете запамти")
    '      Exit Sub
    '    End If

    '    Dim dokOptionsIndex As Integer = RadioGroup1.SelectedIndex
    '    Dim dokDetailsIndex As Integer = RadioGroup2.SelectedIndex
    '    Dim pomVehicleInfo As VehicleInfo = VehicleList.GetVehicleList.GetVehicleListById(_vehicle.Id)
    '    If pomVehicleInfo Is Nothing Then
    '      pomVehicleInfo = VehicleList.GetVehicleList().GetVehicleListById(_vehicle.Id)
    '    End If
    '    Dim pomCustomerInfo As CustomersInfo = CustomersList.GetCustomersList.GetCustomersListById(_customer.Id)
    '    objOpcii = Options.GetOptions
    '    Select Case dokType.IdDocumentTypePrint
    '      Case 1
    '        Dim rptBaranjeZelen As rptZelen = _
    '        New rptZelen(dokOptionsIndex, dokDetailsIndex, pomVehicleInfo, pomCustomerInfo, _document)
    '        rptBaranjeZelen.Margins.Top = objOpcii.ZelenTopMargin
    '        rptBaranjeZelen.Margins.Left = objOpcii.ZelenLeftMargin
    '        rptBaranjeZelen.Margins.Right = objOpcii.ZelenRightMargin
    '        rptBaranjeZelen.Margins.Bottom = objOpcii.ZelenButtonMargin

    '        rptBaranjeZelen.ShowPreviewDialog()
    '        'rptBaranjeZelen.Print()
    '      Case 2
    '        Dim rptBaranjePlav As rptPlav = New rptPlav(dokOptionsIndex, pomVehicleInfo, pomCustomerInfo, _document)
    '        rptBaranjePlav.Margins.Top = objOpcii.PlavTopMargin
    '        rptBaranjePlav.Margins.Left = objOpcii.PlavLeftMargin
    '        rptBaranjePlav.Margins.Right = objOpcii.PlavRightMargin
    '        rptBaranjePlav.Margins.Bottom = objOpcii.PlavButtonMargin
    '        rptBaranjePlav.ShowPreviewDialog()
    '        'rptBaranjePlav.Print()
    '      Case 3
    '        Dim rptBaranjeBel As rptBel = New rptBel(dokOptionsIndex, _vehicle.Id, _customer.Id, _document)
    '        rptBaranjeBel.Margins.Top = objOpcii.BelTopMargin
    '        rptBaranjeBel.Margins.Left = objOpcii.BelLeftMargin
    '        rptBaranjeBel.Margins.Right = objOpcii.BelRightmargin
    '        rptBaranjeBel.Margins.Bottom = objOpcii.BelButtonMargin
    '        rptBaranjeBel.ShowPreviewDialog()
    '        'rptBaranjeBel.Print()
    '    End Select
    '  End Sub

    '  Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
    '    'Dim pomRel As CustomerVehiclesRelation
    '    'If _oldCustomerVehicleRelation.IsNew Then
    '    '  pomRel = _oldCustomerVehicleRelation.Save()
    '    'Else
    '    '  pomRel = _oldCustomerVehicleRelation
    '    'End If
    '    'LookUpEditRelation.EditValue = pomRel.Id
    '    '_document.IdCustomerVehicleRelation = pomRel.Id
    '    Try
    '      If _documentTypeOption.IsTehnicalExamRquired AndAlso Not _vehicle.VehicleLastTehnicalExams.Count > 0 Then
    '        Dim _newTehExam As DocumentsTehnicalExamsReport = DocumentsTehnicalExamsReport.NewDocumentsTehnicalExamsReport
    '        _newTehExam.IdCustomerVehicleRelation = _document.IdCustomerVehicleRelation 'pomRel.Id
    '        _newTehExam.MadeDate = DateFromDateEdit.DateTime
    '        _newTehExam.IdOrganizationForTehnicalExam = LookUpEditOrganization.EditValue
    '        _newTehExam.ValidTillDate = DateFromDateEdit.DateTime.AddMonths(objOpcii.TrafficLicenceVlidNumOfMonths)
    '        _newTehExam.VehicleIsRight = True
    '        _newTehExam.IdFirsControler = 1
    '        _newTehExam.IdSecondControler = 2
    '        _newTehExam.IdTypeOfTehnicalExam = 1
    '        _newTehExam.Save()
    '      End If

    '    Catch ex As Exception
    '      btnPrint.Enabled = False
    '      MsgBox("Потребен е технички преглед за барањето")
    '    End Try

    '    If _vehicle.IsDirty Then
    '      RebindUIVehicle(True, True)
    '    End If
    '    If _customer.IsDirty Then
    '      RebindUICustomer(True, True)
    '    End If

    '    RebindUIDocument(True, True)
    '  End Sub

    '  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    '    On Error Resume Next
    '    Me.Close()
    '  End Sub

    '#End Region


    '  Private Sub RadioGroup2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup2.SelectedIndexChanged
    '    _documentTypeOptionDetail = DocumentTypesOptionsDetailsList.GetDocumentTypesOptionsDetailsList. _
    '    GetDocumentTypesOptionsDetailById(_options.Item(RadioGroup1.SelectedIndex).Details. _
    '                                      Item(RadioGroup2.SelectedIndex).Id)
    '    If _documentTypeOptionDetail.IsNewCustomer Then
    '      ControlGroupNewOwner.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    '      ControlGroupNewOwner.Enabled = True
    '    Else
    '      ControlGroupNewOwner.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    '    End If
    '    ClearOptions()
    '    Select Case _documentTypeOptionDetail.Id

    '      Case 2
    '        CustomerSurnameTextEdit.Enabled = True
    '        CustomerFirstNameTextEdit.Enabled = True
    '        PassportNumberTextEdit.Enabled = True
    '      Case 3
    '        WorksInCompanyTextEdit.Enabled = True
    '        LivingAddressNumberTextEdit.Enabled = True
    '        LookUpEditAddress.Enabled = True
    '        LookUpEditCity.Enabled = True
    '      Case 4
    '        LookUpEditBusinessType.Enabled = True
    '        OccupationTextEdit.Enabled = True

    '    End Select
    '  End Sub

    '  Private Sub ClearOptions()
    '    CustomerSurnameTextEdit.Enabled = False
    '    CustomerFirstNameTextEdit.Enabled = False
    '    LivingAddressNumberTextEdit.Enabled = False
    '    LookUpEditAddress.Enabled = False
    '    LookUpEditCity.Enabled = False
    '    OccupationTextEdit.Enabled = False
    '    WorksInCompanyTextEdit.Enabled = False
    '    LookUpEditBusinessType.Enabled = False
    '    PassportNumberTextEdit.Enabled = False
    '    ShellNumberTextEdit.Enabled = False
    '    RegistrationNumberTextEdit.Enabled = False
    '  End Sub

    '  Private Sub DocumentBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
    '  DocumentBindingSource.CurrentItemChanged, _
    '  VehicleLastTehnicalExamBindingSource.CurrentItemChanged, _
    '  VehicleBindingSource.CurrentItemChanged, _
    '  AttachmentsBindingSource.CurrentItemChanged

    '    Me.btnSave.Enabled = _document.IsSavable
    '    Me.btnCancel.Enabled = _document.IsDirty
    '    'Me.btnPrint.Enabled = LookUpEditOrganization.Visible And LookUpEditOrganization.Text <> String.Empty
    '    Try
    '      Dim message As New System.Text.StringBuilder
    '      message.AppendFormat("{0}" + vbCrLf, "")
    '      ''VehicleLastTehnicalExams
    '      'For Each child As VehicleLastTehnicalExam In _vehicle.VehicleLastTehnicalExams
    '      '  For Each rule As Csla.Validation.BrokenRule In child.BrokenRulesCollection
    '      '    message.AppendFormat( _
    '      '      "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
    '      '  Next
    '      'Next
    '      'For Each rule As Csla.Validation.BrokenRule In _newTehExam.BrokenRulesCollection
    '      '  message.AppendFormat( _
    '      '    "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
    '      'Next
    '      For Each rule As Csla.Validation.BrokenRule In _document.BrokenRulesCollection
    '        message.AppendFormat( _
    '          "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
    '      Next

    '      ShowBrokenRules(message.ToString, True)

    '    Catch ex As Exception

    '    End Try
    '  End Sub

    '  Private Sub CustomerBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerBindingSource.CurrentItemChanged
    '    Try
    '      If RadioGroup2.SelectedIndex = 3 Then
    '        If _customer.IsCompany Then
    '          OccupationTextEdit.Enabled = False
    '          LookUpEditBusinessType.Enabled = True
    '        Else
    '          OccupationTextEdit.Enabled = True
    '          LookUpEditBusinessType.Enabled = False
    '        End If
    '      End If
    '    Catch ex As Exception

    '    End Try

    '  End Sub

    '  Private Sub LookUpEditCity_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditCity.ButtonPressed
    '    If e.Button.Index = 1 Then
    '      Dim par As MainForm = Me.ParentForm
    '      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
    '        For Each ctl As Control In page.Controls
    '          If (TypeOf ctl Is uxCities) Then
    '            par.ShowWinPart(CType(ctl, uxCities))
    '            Exit Sub
    '          End If
    '        Next
    '      Next
    '      Using cekaj As New StatusBusy(My.Resources.txtLoading)
    '        Try
    '          par.AddWinPart(New uxCities)
    '        Catch ex As Exception
    '          MsgBox(ex.Message)
    '        End Try
    '      End Using
    '    End If
    '  End Sub

    '  Private Sub LookUpEditBusinessType_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditBusinessType.ButtonPressed
    '    If e.Button.Index = 1 Then
    '      Dim par As MainForm = Me.ParentForm
    '      For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
    '        For Each ctl As Control In page.Controls
    '          If (TypeOf ctl Is uxBusinessTypes) Then
    '            par.ShowWinPart(CType(ctl, uxBusinessTypes))
    '            Exit Sub
    '          End If
    '        Next
    '      Next
    '      Using cekaj As New StatusBusy(My.Resources.txtLoading)
    '        Try
    '          par.AddWinPart(New uxBusinessTypes)
    '        Catch ex As Exception
    '          MsgBox(ex.Message)
    '        End Try
    '      End Using
    '    End If
    '  End Sub

    '  Private Sub LookUpEditAddress_ProcessNewValue(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles LookUpEditAddress.ProcessNewValue
    '    If e.DisplayValue.ToString = String.Empty Then
    '      e.Handled = False
    '      Exit Sub
    '    End If

    '    Try
    '      'Console.WriteLine("da")
    '      If Street.Exists(e.DisplayValue.ToString) = 0 Then
    '        If MsgBox(e.DisplayValue.ToString & My.Resources.nePostoi, MsgBoxStyle.YesNo, "Зачувај?") = MsgBoxResult.Yes Then

    '          Dim newSt As Street = Street.NewStreet
    '          newSt.StreetName = e.DisplayValue.ToString()
    '          newSt.ApplyEdit()
    '          newSt = newSt.Save

    '          Me.StreetsListBindingSource.RaiseListChangedEvents = False

    '          Me.StreetsListBindingSource.DataSource = StreetsList.GetStreetsList()
    '          Me.StreetsListBindingSource.RaiseListChangedEvents = True
    '          Me.StreetsListBindingSource.ResetBindings(False)
    '        End If
    '        e.Handled = True
    '      End If
    '    Catch ex As Exception
    '      MsgBox(ex.Message)
    '    End Try

    '    e.Handled = True
    '  End Sub

    '  Private Sub LookUpEditEnginePowerSource_ProcessNewValue(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles LookUpEditEnginePowerSource.ProcessNewValue
    '    If e.DisplayValue.ToString = String.Empty Then
    '      e.Handled = False
    '      Exit Sub
    '    End If

    '    Try
    '      'Console.WriteLine("da")
    '      If VehicleEnginePowerSourceType.Exists(e.DisplayValue.ToString) = 0 Then
    '        If MsgBox(e.DisplayValue.ToString & My.Resources.nePostoi, MsgBoxStyle.YesNo, "Зачувај?") = MsgBoxResult.Yes Then

    '          Dim newSt As VehicleEnginePowerSourceType = VehicleEnginePowerSourceType.NewVehicleEnginePowerSourceType
    '          newSt.PowerSourceName = e.DisplayValue.ToString()
    '          newSt.ApplyEdit()
    '          newSt = newSt.Save

    '          Me.VehicleEnginePowerSourceTypeListBindingSource.RaiseListChangedEvents = False

    '          Me.VehicleEnginePowerSourceTypeListBindingSource.DataSource = VehicleEnginePowerSourceTypeList.GetVehicleEnginePowerSourceTypeList()
    '          Me.VehicleEnginePowerSourceTypeListBindingSource.RaiseListChangedEvents = True
    '          Me.VehicleEnginePowerSourceTypeListBindingSource.ResetBindings(False)
    '        End If
    '        e.Handled = True
    '      End If
    '    Catch ex As Exception
    '      MsgBox(ex.Message)
    '    End Try

    '    e.Handled = True
    '  End Sub

    '#Region " Language change "
    '  Private Sub LookUpEditVehicle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditVehicle.GotFocus, RegistrationNumberTextEdit.GotFocus
    '    System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    '  End Sub

    '  Private Sub LookUpEditVehicle_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditVehicle.LostFocus, RegistrationNumberTextEdit.LostFocus
    '    System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
    '  End Sub
    '#End Region

    '    '#Region " Scan "

    '    '  Private Sub AttachmentsBindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles AttachmentsBindingSource.ListChanged
    '    '    If _document IsNot Nothing Then
    '    '      Dim tmp As Boolean = (_document.Attachments.Count > 0)
    '    '      Me.btnDeleteAttachment.Enabled = tmp
    '    '      Me.btnEditAttachment.Enabled = tmp
    '    '    End If
    '    '  End Sub

    '    '  Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
    '    '        Dim att As RequestAttachment = _document.Attachments.AddNew

    '    '        Dim dijT As New dijTwain(att)

    '    '        If dijT.ShowDialog(Me) = DialogResult.OK Then
    '    '        Else
    '    '            'cancel edit na att
    '    '            _document.Attachments.Remove(att)
    '    '        End If

    '    '  End Sub

    '    '  Private Sub btnDeleteAttachment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteAttachment.Click
    '    '    Me.AttachmentsBindingSource.RemoveCurrent()

    '    '  End Sub

    '    '  Private Sub btnEditAttachment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditAttachment.Click
    '    '        'Try
    '    '        '  Dim att As DocumentAttachment
    '    '        '  Using bus As New Splash(My.Resources.LoadingData)
    '    '        '    att = _document.Attachments(Me.AttachmentsBindingSource.Position)
    '    '        '  End Using


    '    '        '  Dim dijT As New dijTwain(att)
    '    '        '  dijT.ShowDialog()
    '    '        'Catch ex As Exception

    '    '        'End Try
    '    '  End Sub

    '    '#End Region

    '  Private Sub VehicleOwnershipProofLookUpEdit_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles VehicleOwnershipProofLookUpEdit.Validated
    '    If VehicleOwnershipProofLookUpEdit.EditValue = 0 Then
    '      Me._document.VehicleOwnershipProof = String.Empty
    '      Me.VehicleOwnershipProofTextEdit.Properties.ReadOnly = True
    '    Else
    '      Me.VehicleOwnershipProofTextEdit.Properties.ReadOnly = False
    '    End If
    '  End Sub

    '  Private Sub PaymentProofLookUpEdit_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles PaymentProofLookUpEdit.Validated
    '    If Me.PaymentProofLookUpEdit.EditValue = 0 Then
    '      Me._document.PaymentProof = String.Empty
    '      Me.PaymentProofTextEdit.Properties.ReadOnly = True
    '    Else
    '      Me.PaymentProofTextEdit.Properties.ReadOnly = False
    '    End If
    '  End Sub

    '  'Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
    '  '  Dim dij As New dijVehicleCustomerList()
    '  '  If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '  '    LookUpEditVehicle.EditValue = dij.Vehicle.IdVehicle
    '  '    ' _vehicle.Id = dij.Vehicle.IdVehicle
    '  '    LookUpEditCustomer.EditValue = dij.Vehicle.IdCustomer
    '  '    VehicleOwnershipProofLookUpEdit.Focus()
    '  '  End If
    '  'End Sub





    '#Region " Prebaruvanje "

    '  Private Sub LookUpEditVehicle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LookUpEditVehicle.KeyDown
    '    Select Case e.KeyData
    '      Case Keys.F1
    '        LookUpEditVehicle_ButtonPressed(Me.LookUpEditVehicle, _
    '        New DevExpress.XtraEditors.Controls.ButtonPressedEventArgs( _
    '        Me.LookUpEditVehicle.Properties.Buttons.Item(2)))
    '    End Select
    '  End Sub

    '  Private Sub LookUpEditVehicle_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditVehicle.ButtonPressed
    '    If e.Button.Index = 1 Then
    '      Dim par As MainForm = Me.ParentForm
    '      Using cekaj As New StatusBusy(My.Resources.txtLoading)
    '        Try
    '          par.AddWinPart(New uxVehicle(Vehicle.NewVehicle))
    '        Catch ex As Exception
    '          MsgBox(ex.Message)
    '        End Try
    '      End Using
    '    Else
    '      If e.Button.Index = 2 Then
    '        Dim dij As New dijVehicleList(_vehicleList)
    '        If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '          LookUpEditVehicle.EditValue = dij.Vehicle.Id
    '          If dij.Vehicle.IdCurrentOwner <> 0 Then
    '            _customer = Customer.GetCustomer(dij.Vehicle.IdCurrentOwner) ' LookUpEditCustomer.EditValue = dij.Vehicle.IdCurrentOwner
    '            LookUpEditCustomer.EditValue = _customer.Id
    '            LookUpEditCustomer_Validated(Me.LookUpEditCustomer, New System.EventArgs)
    '            VehicleOwnershipProofLookUpEdit.Focus()
    '          End If
    '        End If
    '      End If
    '    End If
    '  End Sub

    '  Private Sub LookUpEditVehicle_Validated(ByVal sender As Object, ByVal e As System.EventArgs) _
    '    Handles LookUpEditVehicle.Validated 'LookUpEditRelation.GotFocus

    '    If (Not IsDBNull(LookUpEditVehicle.EditValue)) AndAlso LookUpEditVehicle.EditValue > 0 Then
    '      _vehicle = Vehicle.GetVehicle(LookUpEditVehicle.EditValue)
    '      Me.VehicleBindingSource.DataSource = _vehicle
    '      If _vehicle.VehicleLastTehnicalExams.Count > 0 Then
    '        ControlGroupLastTehnicalExam.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    '      End If
    '      'ako ima relacija, stavi ja
    '      If (Not IsDBNull(LookUpEditCustomer.EditValue)) AndAlso (LookUpEditCustomer.EditValue > 0) Then
    '        Dim pom As CustomerVehicleRelationType = CustomerVehicleRelationType.GetCustomerVehicleRelationType(1)
    '        Dim pomRelation As Long = pom.ZemiRelationId(LookUpEditCustomer.EditValue, LookUpEditVehicle.EditValue)
    '        If pomRelation > 0 Then
    '          LookUpEditRelation.EditValue = pomRelation
    '          _oldCustomerVehicleRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(pomRelation)
    '        Else

    '          Dim postoiSopstvenikDrug As Long = pom.ProveriRelacijaSoVozilo(LookUpEditVehicle.EditValue)
    '          If postoiSopstvenikDrug = 0 Then
    '            _oldCustomerVehicleRelation = CustomerVehiclesRelation.NewCustomerVehiclesRelation
    '            _oldCustomerVehicleRelation.IdCustomer = LookUpEditCustomer.EditValue
    '            _oldCustomerVehicleRelation.IdVehicle = LookUpEditVehicle.EditValue
    '            _oldCustomerVehicleRelation.IdRelationType = 1 'LookUpEditRelation.EditValue

    '          Else
    '            MsgBox("Возилото е регистрирано на сопственикот: " & Customer.GetCustomer(postoiSopstvenikDrug).CustomerFirstName)
    '            LookUpEditCustomer.EditValue = Nothing
    '            LookUpEditCustomer.Focus()
    '          End If

    '          '  Dim pomRel As CustomerVehiclesRelation = _customerVehicleRelation.Save()
    '          '  LookUpEditRelation.EditValue = pomRel.Id

    '        End If
    '        '_document.IdCustomerVehicleRelation = LookUpEditRelation.EditValue

    '      Else
    '        'najdi relacija i stavi
    '        Dim info As CustomerVehiclesRelationsInfo = _relationList.getInfoByIdVehicle(LookUpEditVehicle.EditValue)
    '        If info IsNot Nothing Then
    '          _document.IdCustomerVehicleRelation = info.Id

    '          Me.LookUpEditCustomer.EditValue = info.IdCustomer

    '        Else
    '          'dodadi sopstvenik
    '          If LookUpEditCustomer.EditValue > 0 Then
    '            If MsgBox("Дали сакате да го додаете како сопсвеник", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
    '              _oldCustomerVehicleRelation = Nothing
    '              _oldCustomerVehicleRelation = CustomerVehiclesRelation.NewCustomerVehiclesRelation
    '              _oldCustomerVehicleRelation.IdCustomer = LookUpEditCustomer.EditValue
    '              _oldCustomerVehicleRelation.IdVehicle = LookUpEditVehicle.EditValue
    '              _oldCustomerVehicleRelation.IdRelationType = 1 'kako sopstvenik
    '            End If
    '          End If
    '        End If
    '      End If

    '    End If
    '  End Sub

    '  Private Sub LookUpEditCustomer_Validated(ByVal sender As Object, ByVal e As System.EventArgs) _
    '    Handles LookUpEditCustomer.Validated
    '    If (Not IsDBNull(LookUpEditCustomer.EditValue)) AndAlso (LookUpEditCustomer.EditValue > 0) Then

    '      If (Not IsDBNull(LookUpEditVehicle.EditValue)) AndAlso (LookUpEditVehicle.EditValue > 0) Then
    '        'proveri dali postoi relacijata
    '        Dim info As CustomerVehiclesRelationsInfo = _
    '          _relationList.getInfoByIdVehicleAndIdCostomer(LookUpEditVehicle.EditValue, LookUpEditCustomer.EditValue)
    '        If (info IsNot Nothing) Then
    '          _document.IdCustomerVehicleRelation = info.Id
    '          'Me.LookUpEditCustomer.EditValue = info.IdCustomer
    '          Try
    '            _oldCustomerVehicleRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(info.Id)

    '          Catch ex As Exception
    '            MsgBox(ex.Message)
    '          End Try
    '          _customer = Customer.GetCustomer(LookUpEditCustomer.EditValue)
    '          Me.CustomerBindingSource.DataSource = _customer
    '        Else
    '          If (_relationList.getInfoByIdVehicle(Me.LookUpEditVehicle.EditValue) Is Nothing) Then
    '            If MsgBox("Дали сакате да го додадете како сопсвеник", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
    '              _oldCustomerVehicleRelation = Nothing
    '              _oldCustomerVehicleRelation = CustomerVehiclesRelation.NewCustomerVehiclesRelation
    '              _oldCustomerVehicleRelation.IdCustomer = LookUpEditCustomer.EditValue
    '              _oldCustomerVehicleRelation.IdVehicle = LookUpEditVehicle.EditValue
    '              _oldCustomerVehicleRelation.IdRelationType = 1 'kako sopstvenik

    '              _document.IdCustomerVehicleRelation = _oldCustomerVehicleRelation.Save().Id
    '            End If
    '            _customer = Customer.GetCustomer(LookUpEditCustomer.EditValue)
    '            Me.CustomerBindingSource.DataSource = _customer
    '          Else
    '            MsgBox("Возилото има друг сопственик")
    '            Me.LookUpEditVehicle.EditValue = Nothing
    '            Me.LookUpEditCustomer.EditValue = Nothing
    '            _document.IdCustomerVehicleRelation = 0
    '            LookUpEditVehicle.Focus()
    '          End If

    '          'MsgBox("Возилото има друг сопственик")

    '          'Me.LookUpEditVehicle.EditValue = Nothing
    '          'Me.LookUpEditCustomer.EditValue = Nothing
    '          '_document.IdCustomerVehicleRelation = 0
    '          'LookUpEditVehicle.Focus()
    '        End If

    '      Else
    '        MsgBox("Одберете возило")
    '      End If

    '    End If
    '  End Sub


    '#End Region

    '#Region " Previous Registration "
    '  Private Sub _document_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _document.PropertyChanged
    '    Select Case e.PropertyName
    '      Case "IdCustomerVehicleRelation"
    '        LoadDependantList()
    '    End Select

    '  End Sub

    '  Private Sub PreviousRegistrationCustomLookUpEdit_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles PreviousRegistrationCustomLookUpEdit.ButtonPressed
    '    If e.Button.Index = 1 Then
    '      'dijAddNewRegistration
    '      If Me.LookUpEditVehicle.EditValue > 0 Then
    '        Dim dij As New dijAddNewRegistration(Vehicle.GetVehicle(Me.LookUpEditVehicle.EditValue))
    '        If dij.ShowDialog = DialogResult.OK Then
    '          'vcitaj nanovo registrationList
    '          LoadDependantList()
    '          _document.IdPreviousRegistration = dij.SelectedRegistration.Id
    '        End If
    '      Else
    '        MsgBox("Одберете возило")
    '      End If


    '    End If
    '  End Sub
    '#End Region

    '  Private Sub PreviousRegistrationCustomLookUpEdit_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PreviousRegistrationCustomLookUpEdit.EditValueChanged

    '  End Sub
End Class
