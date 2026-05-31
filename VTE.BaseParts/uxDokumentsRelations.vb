Imports VTE.Library
Imports System.ComponentModel

Public Class uxDokumentsRelations

  Private WithEvents _document As Document
  'Private WithEvents _relation As CustomerVehiclesRelation = Nothing
  Public Event select_relation(ByVal sender As System.Object, ByVal e As System.EventArgs)
  Public Event select_customer(ByVal sender As System.Object, ByVal e As System.EventArgs)
  Public Event select_vehicle(ByVal sender As System.Object, ByVal e As System.EventArgs)
  Public Event buttonNewPressed_vehicle(ByVal sender As System.Object, ByVal e As System.EventArgs)
  Public Event buttonNewPressed_customer(ByVal sender As System.Object, ByVal e As System.EventArgs)
  Public Event buttonGetPressed_vehicle(ByVal sender As System.Object, ByVal e As System.EventArgs)
  Public Event buttonGetPressed_customer(ByVal sender As System.Object, ByVal e As System.EventArgs)
  Public Event btnExit_Pressed(ByVal sender As System.Object, ByVal e As System.EventArgs)
  Public Event btnPrint_Pressed(ByVal sender As System.Object, ByVal e As System.EventArgs)
  Public Event radioGroup_valueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
  ' Public WithEvents _documentTypeOption As DocumentTypesOptionsInfo


  Public Sub LoadList(ByVal docIn As Document)
    'btnOK.Enabled = True
    Me.CustomersListBindingSource.DataSource = CustomersList.GetCustomersList
    Me.VehicleListBindingSource.DataSource = VehicleList.GetVehicleList
    Me.DocumentTypesListBindingSource.DataSource = DocumentTypesList.GetDocumentTypesList
    Me.CustomerVehicleRelationTypeListBindingSource.DataSource = CustomerVehicleRelationTypeList.GetCustomerVehicleRelationTypeList
    _document = docIn
    If Not _document.IsNew Then
      Dim pom As CustomerVehiclesRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(_document.IdCustomerVehicleRelation)
      LookUpEditCustomer.EditValue = pom.IdCustomer
      LookUpEditVehicle.EditValue = pom.IdVehicle
      LookUpEditRelation.EditValue = pom.IdRelationType
    End If
    '_documentTypeOption = DocumentTypesOptionsList.GetDocumentTypesOptionsList.GetDocumentTypesOptionsListById _
    ' (_document.IdDocumentTypeOption)

  End Sub

  Public Sub RadioGroup()
    Dim options As DocumentTypesOptions = DocumentTypes.GetDocumentTypes.GetDocumentTypesById _
   (LookUpEditDocumentType.EditValue).DocumentTypesOptions
    If options.Count = 0 Then
      LayoutControlItemRadioGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    Else
      LayoutControlItemRadioGroup.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
      For i = 0 To options.Count - 1
        Dim pom As DevExpress.XtraEditors.Controls.RadioGroupItem = _
        New DevExpress.XtraEditors.Controls.RadioGroupItem
        pom.Description = options.Item(i).OptionName
        pom.Value = False 'CheckState.Unchecked
        Me.RadioGroup1.Properties.Items.Add(pom)
      Next
    End If
    LayoutRadioGroup2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
  End Sub
  Public ReadOnly Property Document() As Document
    Get
      Return _document
    End Get
  End Property
  Private Sub BindUI()
    _document.BeginEdit()
    Me.DocumentBindingSource.DataSource = _document
  End Sub

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    '  btnSave.Enabled = False
    ' btnPrint.Enabled = False
    ' Add any initialization after the InitializeComponent() call.
    'Me.CustomLookUpEdit2.Properties.ReadOnly = Not IsCustomerOnly

  End Sub

  Private Sub relation_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LookUpEditRelation.EditValueChanged
    If LookUpEditRelation.EditValue > 0 Then
      btnOK.Enabled = True
      RaiseEvent select_relation(sender, e)
    End If
  End Sub

  Private Sub LookUpEditCustomer_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditCustomer.ButtonPressed
    If e.Button.Index = 1 Then
      RaiseEvent buttonNewPressed_customer(sender, e)
    Else
      If e.Button.Index = 2 Then
        RaiseEvent buttonGetPressed_customer(sender, e)
      End If
    End If
  End Sub

  Private Sub LookUpEditCustomer_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LookUpEditCustomer.EditValueChanged
    If LookUpEditCustomer.EditValue > 0 Then
      RaiseEvent select_customer(sender, e)
    End If
    ProveriDocument()
  End Sub

  Private Sub LookUpEditVehicle_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditVehicle.ButtonPressed
    If e.Button.Index = 1 Then
      RaiseEvent buttonNewPressed_vehicle(sender, e)
    Else
      If e.Button.Index = 2 Then
        RaiseEvent buttonGetPressed_vehicle(sender, e)
      End If
    End If
  End Sub

  Private Sub LookUpEditVehicle_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LookUpEditVehicle.EditValueChanged
    If LookUpEditVehicle.EditValue > 0 Then
      RaiseEvent select_vehicle(sender, e)
    End If
    If LookUpEditCustomer.EditValue > 0 Then
      If LookUpEditVehicle.EditValue <= 0 Then
        LookUpEditVehicle.EditValue = 0
      End If
      Dim pom As CustomerVehicleRelationType = CustomerVehicleRelationType.GetCustomerVehicleRelationType(1)
      Dim RelationTypePostoecka As Integer
      RelationTypePostoecka = pom.ZemiRelationType(LookUpEditCustomer.EditValue, LookUpEditVehicle.EditValue)
      If RelationTypePostoecka <> 0 Then
        LookUpEditRelation.EditValue = RelationTypePostoecka
        'CustomerVehicleRelationTypeList.GetCustomerVehicleRelationTypeList.GetCustomerVehicleRelationTypeListById _
      Else
        lblText.Text = "Изберете релација и притеснете на копчето"
        LookUpEditRelation.Properties.ReadOnly = False
        LookUpEditRelation.Focus()
        ProveriDocument()
        Exit Sub
      End If
    Else
      lblText.Text = "Изберете клиент"
      LookUpEditCustomer.Focus()
    End If
    ProveriDocument()
  End Sub

  Private Sub LookUpEditCustomer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditCustomer.GotFocus
    Me.ToolTipController1.ShowHint(LookUpEditCustomer.ToolTip)
    LookUpEditVehicle.Properties.ReadOnly = _
    Not (DocumentTypesList.GetDocumentTypesList. _
   GetDocumentTypesListById(LookUpEditDocumentType.EditValue).IsVehiceRequired)
  End Sub

  Private Sub LookUpEditVehicle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditVehicle.GotFocus
    Me.ToolTipController1.ShowHint(LookUpEditVehicle.ToolTip)
  End Sub

  Private Sub LookUpEditRelation_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpEditRelation.GotFocus
    Dim pom As Boolean = DocumentTypesList.GetDocumentTypesList. _
   GetDocumentTypesListById(LookUpEditDocumentType.EditValue).IsVehiceRequired

    Me.CustomerVehicleRelationTypeListBindingSource.DataSource = _
    CustomerVehicleRelationTypeList.GetCustomerVehicleRelationTypeListBYIsCustomerOnly(Not pom)
  End Sub

  Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
    If LookUpEditCustomer.EditValue > 0 Then
      Dim pom As CustomerVehicleRelationType = CustomerVehicleRelationType.GetCustomerVehicleRelationType(1)
      If pom.ZemiRelationType(LookUpEditCustomer.EditValue, LookUpEditVehicle.EditValue) <> 0 Then
        LookUpEditRelation.EditValue = pom.ZemiRelationType(LookUpEditCustomer.EditValue, LookUpEditVehicle.EditValue)
        LookUpEditRelation.Properties.ReadOnly = True
      Else
        LookUpEditRelation.Properties.ReadOnly = False
        Dim _customerVehicleRelation As CustomerVehiclesRelation = CustomerVehiclesRelation.NewCustomerVehiclesRelation
        _customerVehicleRelation.IdCustomer = LookUpEditCustomer.EditValue
        If LookUpEditVehicle.EditValue = 0 Then
          LookUpEditVehicle.EditValue = Nothing
        End If
        _customerVehicleRelation.IdVehicle = LookUpEditVehicle.EditValue
        _customerVehicleRelation.IdRelationType = LookUpEditRelation.EditValue
        _customerVehicleRelation.Save()
        lblText.Text = "Внесено"
      End If
    End If
  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    RaiseEvent btnExit_Pressed(sender, e)
  End Sub

  Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
    RebindUI(True, True)
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
    Me.DocumentBindingSource.RaiseListChangedEvents = False

    ' commit edits in memory
    UnbindBindingSource(Me.DocumentBindingSource, saveObject, True)

    Try
      ' save or cancel changes
      If saveObject Then

        _document.ApplyEdit()
        _document.IdDocumentType = LookUpEditDocumentType.EditValue
        _document.IdCustomerVehicleRelation = _
        CustomerVehicleRelationType.GetCustomerVehicleRelationType(1). _
        ZemiRelationId(LookUpEditCustomer.EditValue, LookUpEditVehicle.EditValue)
        If DocumentTypes.GetDocumentTypes.GetDocumentTypesById(LookUpEditDocumentType.EditValue).DocumentTypesOptions.Count > 0 Then
          _document.IdDocumentTypeOption = DocumentTypes.GetDocumentTypes. _
          GetDocumentTypesById(LookUpEditDocumentType.EditValue).DocumentTypesOptions. _
          Item(RadioGroup1.SelectedIndex).Id
        End If
        If DocumentTypes.GetDocumentTypes. _
               GetDocumentTypesById(LookUpEditDocumentType.EditValue).DocumentTypesOptions. _
               Item(RadioGroup1.SelectedIndex).Details.Count > 0 Then
          _document.IdDocumentTypeOptionDetail = DocumentTypes.GetDocumentTypes. _
                  GetDocumentTypesById(LookUpEditDocumentType.EditValue).DocumentTypesOptions. _
                  Item(RadioGroup1.SelectedIndex).Details.Item(RadioGroup2.SelectedIndex).Id
        End If
        Try
          _document = _document.Save
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
        _document.CancelEdit()
      End If
    Finally
      'rebind UI if requested
      If rebind Then
        BindUI()
      End If

      ' restore events
      Me.DocumentBindingSource.RaiseListChangedEvents = True

      If rebind Then
        ' refresh the UI if rebinding
        Me.DocumentBindingSource.ResetBindings(False)

      End If
    End Try

  End Sub

  Private Sub RadioGroup1_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroup1.SelectedIndexChanged
    RadioGroup2.Properties.Items.Clear()
    Dim optionsDetails As DocumentTypesOptionDetails = DocumentTypes.GetDocumentTypes.GetDocumentTypesById _
     (LookUpEditDocumentType.EditValue).DocumentTypesOptions.Item(RadioGroup1.SelectedIndex).Details
    If optionsDetails.Count = 0 Then
      LayoutRadioGroup2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    Else
      LayoutRadioGroup2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
      For i = 0 To optionsDetails.Count - 1
        Dim pom As DevExpress.XtraEditors.Controls.RadioGroupItem = _
        New DevExpress.XtraEditors.Controls.RadioGroupItem
        pom.Description = optionsDetails.Item(i).Name
        pom.Value = False
        Me.RadioGroup2.Properties.Items.Add(pom)
      Next
    End If
    RaiseEvent radioGroup_valueChanged(sender, e)
  End Sub

  Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    RaiseEvent btnPrint_Pressed(sender, e)
  End Sub

  'Private Sub _document_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _document.PropertyChanged
  '  Me.btnSave.Enabled = Not (Not _document.IsNew AndAlso Not _document.IsDirty)
  '  Me.btnPrint.Enabled = Not _document.IsDirty
  'End Sub
  Private Sub ProveriDocument()
    If LookUpEditVehicle.EditValue > 0 AndAlso LookUpEditCustomer.EditValue > 0 Then
      Try
        Dim pom As DocumentActiveInfo = DocumentActiveList.GetDocumentList _
        (LookUpEditCustomer.EditValue, LookUpEditVehicle.EditValue, LookUpEditDocumentType.EditValue).Item(0)
        _document = Document.GetDocument(pom.Id)
      Catch ex As Exception
        _document = Document.NewDocument
      End Try
      BindUI()
    End If

  End Sub
End Class
