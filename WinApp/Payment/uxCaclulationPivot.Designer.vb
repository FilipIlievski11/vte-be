<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxCaclulationPivot
    Inherits VTE.BaseParts.uxWinPart

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
  Me.components = New System.ComponentModel.Container
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxCaclulationPivot))
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.LookUpReqType = New DevExpress.XtraEditors.LookUpEdit
  Me.RequestTypeListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.btnCalculate = New DevExpress.XtraEditors.SimpleButton
  Me.LookUpVehicle = New DevExpress.XtraEditors.LookUpEdit
  Me.VehicleListShortBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.VehicleTextEdit = New DevExpress.XtraEditors.TextEdit
  Me.btnPrint = New DevExpress.XtraEditors.SimpleButton
  Me.btnExit = New DevExpress.XtraEditors.SimpleButton
  Me.CustomerTextEdit = New DevExpress.XtraEditors.TextEdit
  Me.PivotGridControl1 = New DevExpress.XtraPivotGrid.PivotGridControl
  Me.CalculationListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.fieldCalculationItem = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldBankAccount = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldBank = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldForm = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldAmount = New DevExpress.XtraPivotGrid.PivotGridField
  Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutVehicle = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutBtnCalculate = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
  Me.PrintingSystem1 = New DevExpress.XtraPrinting.PrintingSystem(Me.components)
  Me.PrintableComponentLink1 = New DevExpress.XtraPrinting.PrintableComponentLink(Me.components)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.LayoutControl1.SuspendLayout()
  CType(Me.LookUpReqType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.RequestTypeListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LookUpVehicle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.VehicleListShortBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.VehicleTextEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CustomerTextEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.PivotGridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CalculationListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutVehicle, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutBtnCalculate, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'LayoutControl1
  '
  Me.LayoutControl1.AccessibleDescription = Nothing
  Me.LayoutControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
  Me.LayoutControl1.BackgroundImage = Nothing
  Me.LayoutControl1.Controls.Add(Me.LookUpReqType)
  Me.LayoutControl1.Controls.Add(Me.btnCalculate)
  Me.LayoutControl1.Controls.Add(Me.LookUpVehicle)
  Me.LayoutControl1.Controls.Add(Me.VehicleTextEdit)
  Me.LayoutControl1.Controls.Add(Me.btnPrint)
  Me.LayoutControl1.Controls.Add(Me.btnExit)
  Me.LayoutControl1.Controls.Add(Me.CustomerTextEdit)
  Me.LayoutControl1.Controls.Add(Me.PivotGridControl1)
  Me.LayoutControl1.Font = Nothing
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
  '
  'LookUpReqType
  '
  resources.ApplyResources(Me.LookUpReqType, "LookUpReqType")
  Me.LookUpReqType.BackgroundImage = Nothing
  Me.LookUpReqType.EditValue = Nothing
  Me.LookUpReqType.Name = "LookUpReqType"
  Me.LookUpReqType.Properties.AccessibleDescription = Nothing
  Me.LookUpReqType.Properties.AccessibleName = Nothing
  Me.LookUpReqType.Properties.AutoHeight = CType(resources.GetObject("LookUpReqType.Properties.AutoHeight"), Boolean)
  Me.LookUpReqType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpReqType.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
  Me.LookUpReqType.Properties.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdRequestType", "IdRequestType", 80, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdDocumentPrint", "IdDocumentPrint", 86, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsTehnicalExamRequired", "IsTehnicalExamRequired", 123, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsPayRequired", "IsPayRequired", 76, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsNewRegistration", "IsNewRegistration", 94, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsRelationDeleted", "IsRelationDeleted", 91, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsVehicleDeleted", "IsVehicleDeleted", 85, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsNewCustomer", "IsNewCustomer", 82, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsVehicleChanged", "IsVehicleChanged", 91, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsCustomerChanged", "IsCustomerChanged", 104, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsSufficient", "IsSufficient", 60, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("TypeName", "TypeName", 57, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("TypeDescription", "TypeDescription", 83, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsPreviosRegistrationReqired", "IsPreviosRegistrationReqired", 145, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
  Me.LookUpReqType.Properties.DataSource = Me.RequestTypeListBindingSource
  Me.LookUpReqType.Properties.DisplayMember = "TypeName"
  Me.LookUpReqType.Properties.ValueMember = "IdRequestType"
  Me.LookUpReqType.StyleController = Me.LayoutControl1
  '
  'RequestTypeListBindingSource
  '
  Me.RequestTypeListBindingSource.DataSource = GetType(VTE.Library.RequestTypeList)
  '
  'btnCalculate
  '
  Me.btnCalculate.AccessibleDescription = Nothing
  Me.btnCalculate.AccessibleName = Nothing
  resources.ApplyResources(Me.btnCalculate, "btnCalculate")
  Me.btnCalculate.BackgroundImage = Nothing
  Me.btnCalculate.Name = "btnCalculate"
  Me.btnCalculate.StyleController = Me.LayoutControl1
  '
  'LookUpVehicle
  '
  resources.ApplyResources(Me.LookUpVehicle, "LookUpVehicle")
  Me.LookUpVehicle.BackgroundImage = Nothing
  Me.LookUpVehicle.EditValue = Nothing
  Me.LookUpVehicle.Name = "LookUpVehicle"
  Me.LookUpVehicle.Properties.AccessibleDescription = Nothing
  Me.LookUpVehicle.Properties.AccessibleName = Nothing
  Me.LookUpVehicle.Properties.AutoHeight = CType(resources.GetObject("LookUpVehicle.Properties.AutoHeight"), Boolean)
  Me.LookUpVehicle.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpVehicle.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpVehicle.Properties.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpVehicle.Properties.Buttons2"), CType(resources.GetObject("LookUpVehicle.Properties.Buttons3"), Integer), CType(resources.GetObject("LookUpVehicle.Properties.Buttons4"), Boolean), CType(resources.GetObject("LookUpVehicle.Properties.Buttons5"), Boolean), CType(resources.GetObject("LookUpVehicle.Properties.Buttons6"), Boolean), CType(resources.GetObject("LookUpVehicle.Properties.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing)})
  Me.LookUpVehicle.Properties.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ShellNumber", "ShellNumber", 65, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ModelName", "ModelName", 61, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("VehiceMaker", "VehiceMaker", 66, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
  Me.LookUpVehicle.Properties.DataSource = Me.VehicleListShortBindingSource
  Me.LookUpVehicle.Properties.DisplayMember = "ShellNumber"
  Me.LookUpVehicle.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
  Me.LookUpVehicle.Properties.ValueMember = "Id"
  Me.LookUpVehicle.StyleController = Me.LayoutControl1
  '
  'VehicleListShortBindingSource
  '
  Me.VehicleListShortBindingSource.DataSource = GetType(VTE.Library.VehicleListShort)
  '
  'VehicleTextEdit
  '
  resources.ApplyResources(Me.VehicleTextEdit, "VehicleTextEdit")
  Me.VehicleTextEdit.BackgroundImage = Nothing
  Me.VehicleTextEdit.EditValue = Nothing
  Me.VehicleTextEdit.Name = "VehicleTextEdit"
  Me.VehicleTextEdit.Properties.AccessibleDescription = Nothing
  Me.VehicleTextEdit.Properties.AccessibleName = Nothing
  Me.VehicleTextEdit.Properties.AutoHeight = CType(resources.GetObject("VehicleTextEdit.Properties.AutoHeight"), Boolean)
  Me.VehicleTextEdit.Properties.Mask.AutoComplete = CType(resources.GetObject("VehicleTextEdit.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.VehicleTextEdit.Properties.Mask.BeepOnError = CType(resources.GetObject("VehicleTextEdit.Properties.Mask.BeepOnError"), Boolean)
  Me.VehicleTextEdit.Properties.Mask.EditMask = resources.GetString("VehicleTextEdit.Properties.Mask.EditMask")
  Me.VehicleTextEdit.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("VehicleTextEdit.Properties.Mask.IgnoreMaskBlank"), Boolean)
  Me.VehicleTextEdit.Properties.Mask.MaskType = CType(resources.GetObject("VehicleTextEdit.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.VehicleTextEdit.Properties.Mask.PlaceHolder = CType(resources.GetObject("VehicleTextEdit.Properties.Mask.PlaceHolder"), Char)
  Me.VehicleTextEdit.Properties.Mask.SaveLiteral = CType(resources.GetObject("VehicleTextEdit.Properties.Mask.SaveLiteral"), Boolean)
  Me.VehicleTextEdit.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("VehicleTextEdit.Properties.Mask.ShowPlaceHolders"), Boolean)
  Me.VehicleTextEdit.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("VehicleTextEdit.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.VehicleTextEdit.Properties.ReadOnly = True
  Me.VehicleTextEdit.StyleController = Me.LayoutControl1
  '
  'btnPrint
  '
  Me.btnPrint.AccessibleDescription = Nothing
  Me.btnPrint.AccessibleName = Nothing
  resources.ApplyResources(Me.btnPrint, "btnPrint")
  Me.btnPrint.BackgroundImage = Nothing
  Me.btnPrint.Name = "btnPrint"
  Me.btnPrint.StyleController = Me.LayoutControl1
  '
  'btnExit
  '
  Me.btnExit.AccessibleDescription = Nothing
  Me.btnExit.AccessibleName = Nothing
  resources.ApplyResources(Me.btnExit, "btnExit")
  Me.btnExit.BackgroundImage = Nothing
  Me.btnExit.Name = "btnExit"
  Me.btnExit.StyleController = Me.LayoutControl1
  '
  'CustomerTextEdit
  '
  resources.ApplyResources(Me.CustomerTextEdit, "CustomerTextEdit")
  Me.CustomerTextEdit.BackgroundImage = Nothing
  Me.CustomerTextEdit.EditValue = Nothing
  Me.CustomerTextEdit.Name = "CustomerTextEdit"
  Me.CustomerTextEdit.Properties.AccessibleDescription = Nothing
  Me.CustomerTextEdit.Properties.AccessibleName = Nothing
  Me.CustomerTextEdit.Properties.AutoHeight = CType(resources.GetObject("CustomerTextEdit.Properties.AutoHeight"), Boolean)
  Me.CustomerTextEdit.Properties.Mask.AutoComplete = CType(resources.GetObject("CustomerTextEdit.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.CustomerTextEdit.Properties.Mask.BeepOnError = CType(resources.GetObject("CustomerTextEdit.Properties.Mask.BeepOnError"), Boolean)
  Me.CustomerTextEdit.Properties.Mask.EditMask = resources.GetString("CustomerTextEdit.Properties.Mask.EditMask")
  Me.CustomerTextEdit.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("CustomerTextEdit.Properties.Mask.IgnoreMaskBlank"), Boolean)
  Me.CustomerTextEdit.Properties.Mask.MaskType = CType(resources.GetObject("CustomerTextEdit.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.CustomerTextEdit.Properties.Mask.PlaceHolder = CType(resources.GetObject("CustomerTextEdit.Properties.Mask.PlaceHolder"), Char)
  Me.CustomerTextEdit.Properties.Mask.SaveLiteral = CType(resources.GetObject("CustomerTextEdit.Properties.Mask.SaveLiteral"), Boolean)
  Me.CustomerTextEdit.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("CustomerTextEdit.Properties.Mask.ShowPlaceHolders"), Boolean)
  Me.CustomerTextEdit.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("CustomerTextEdit.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.CustomerTextEdit.Properties.ReadOnly = True
  Me.CustomerTextEdit.StyleController = Me.LayoutControl1
  '
  'PivotGridControl1
  '
  Me.PivotGridControl1.AccessibleDescription = Nothing
  Me.PivotGridControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.PivotGridControl1, "PivotGridControl1")
  Me.PivotGridControl1.Appearance.Cell.Options.UseTextOptions = True
  Me.PivotGridControl1.Appearance.Cell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.PivotGridControl1.AppearancePrint.Cell.Options.UseTextOptions = True
  Me.PivotGridControl1.AppearancePrint.Cell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisWord
  Me.PivotGridControl1.AppearancePrint.Cell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.PivotGridControl1.AppearancePrint.FieldHeader.Options.UseTextOptions = True
  Me.PivotGridControl1.AppearancePrint.FieldHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.PivotGridControl1.AppearancePrint.FieldValue.Options.UseTextOptions = True
  Me.PivotGridControl1.AppearancePrint.FieldValue.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.PivotGridControl1.AppearancePrint.FilterSeparator.Options.UseTextOptions = True
  Me.PivotGridControl1.AppearancePrint.FilterSeparator.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.PivotGridControl1.BackgroundImage = Nothing
  Me.PivotGridControl1.Cursor = System.Windows.Forms.Cursors.Default
  Me.PivotGridControl1.DataSource = Me.CalculationListBindingSource
  Me.PivotGridControl1.Fields.AddRange(New DevExpress.XtraPivotGrid.PivotGridField() {Me.fieldCalculationItem, Me.fieldBankAccount, Me.fieldBank, Me.fieldForm, Me.fieldAmount})
  Me.PivotGridControl1.Name = "PivotGridControl1"
  Me.PivotGridControl1.OLAPConnectionString = Nothing
  Me.PivotGridControl1.OptionsPrint.PrintColumnHeaders = DevExpress.Utils.DefaultBoolean.[False]
  Me.PivotGridControl1.OptionsPrint.PrintDataHeaders = DevExpress.Utils.DefaultBoolean.[False]
  Me.PivotGridControl1.OptionsPrint.PrintFilterHeaders = DevExpress.Utils.DefaultBoolean.[False]
  Me.PivotGridControl1.OptionsPrint.PrintRowHeaders = DevExpress.Utils.DefaultBoolean.[True]
  Me.PivotGridControl1.OptionsPrint.UsePrintAppearance = True
  '
  'CalculationListBindingSource
  '
  Me.CalculationListBindingSource.DataSource = GetType(VTE.Library.CalculationList)
  '
  'fieldCalculationItem
  '
  Me.fieldCalculationItem.Appearance.Value.Options.UseTextOptions = True
  Me.fieldCalculationItem.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
  Me.fieldCalculationItem.Appearance.Value.TextOptions.Trimming = DevExpress.Utils.Trimming.Word
  Me.fieldCalculationItem.Appearance.Value.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  Me.fieldCalculationItem.Appearance.Value.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.fieldCalculationItem.Appearance.ValueTotal.Options.UseTextOptions = True
  Me.fieldCalculationItem.Appearance.ValueTotal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.fieldCalculationItem.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldCalculationItem.AreaIndex = 0
  resources.ApplyResources(Me.fieldCalculationItem, "fieldCalculationItem")
  Me.fieldCalculationItem.Name = "fieldCalculationItem"
  Me.fieldCalculationItem.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.[True]
  '
  'fieldBankAccount
  '
  Me.fieldBankAccount.Appearance.Value.Options.UseTextOptions = True
  Me.fieldBankAccount.Appearance.Value.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.fieldBankAccount.Appearance.ValueTotal.Options.UseTextOptions = True
  Me.fieldBankAccount.Appearance.ValueTotal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.fieldBankAccount.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldBankAccount.AreaIndex = 1
  resources.ApplyResources(Me.fieldBankAccount, "fieldBankAccount")
  Me.fieldBankAccount.Name = "fieldBankAccount"
  Me.fieldBankAccount.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.[True]
  '
  'fieldBank
  '
  Me.fieldBank.Appearance.Value.Options.UseTextOptions = True
  Me.fieldBank.Appearance.Value.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.fieldBank.Appearance.ValueTotal.Options.UseTextOptions = True
  Me.fieldBank.Appearance.ValueTotal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.fieldBank.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldBank.AreaIndex = 2
  resources.ApplyResources(Me.fieldBank, "fieldBank")
  Me.fieldBank.Name = "fieldBank"
  Me.fieldBank.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.[True]
  '
  'fieldForm
  '
  Me.fieldForm.Appearance.Value.Options.UseTextOptions = True
  Me.fieldForm.Appearance.Value.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.fieldForm.Appearance.ValueTotal.Options.UseTextOptions = True
  Me.fieldForm.Appearance.ValueTotal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.fieldForm.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldForm.AreaIndex = 3
  resources.ApplyResources(Me.fieldForm, "fieldForm")
  Me.fieldForm.Name = "fieldForm"
  Me.fieldForm.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.[True]
  '
  'fieldAmount
  '
  Me.fieldAmount.Appearance.Value.Options.UseTextOptions = True
  Me.fieldAmount.Appearance.Value.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.fieldAmount.Appearance.ValueTotal.Options.UseTextOptions = True
  Me.fieldAmount.Appearance.ValueTotal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  Me.fieldAmount.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea
  Me.fieldAmount.AreaIndex = 0
  resources.ApplyResources(Me.fieldAmount, "fieldAmount")
  Me.fieldAmount.Name = "fieldAmount"
  '
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem5, Me.LayoutVehicle, Me.LayoutBtnCalculate, Me.LayoutControlItem6})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(843, 577)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.PivotGridControl1
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 95)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(841, 480)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem1.TextToControlDistance = 0
  Me.LayoutControlItem1.TextVisible = False
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.CustomerTextEdit
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(657, 31)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(55, 20)
  '
  'LayoutControlItem3
  '
  Me.LayoutControlItem3.Control = Me.btnExit
  resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
  Me.LayoutControlItem3.Location = New System.Drawing.Point(749, 0)
  Me.LayoutControlItem3.Name = "LayoutControlItem3"
  Me.LayoutControlItem3.Size = New System.Drawing.Size(92, 62)
  Me.LayoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
  Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem3.TextToControlDistance = 0
  Me.LayoutControlItem3.TextVisible = False
  '
  'LayoutControlItem4
  '
  Me.LayoutControlItem4.Control = Me.btnPrint
  resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
  Me.LayoutControlItem4.Location = New System.Drawing.Point(657, 0)
  Me.LayoutControlItem4.Name = "LayoutControlItem4"
  Me.LayoutControlItem4.Size = New System.Drawing.Size(92, 62)
  Me.LayoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
  Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem4.TextToControlDistance = 0
  Me.LayoutControlItem4.TextVisible = False
  '
  'LayoutControlItem5
  '
  Me.LayoutControlItem5.Control = Me.VehicleTextEdit
  resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
  Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 31)
  Me.LayoutControlItem5.Name = "LayoutControlItem5"
  Me.LayoutControlItem5.Size = New System.Drawing.Size(657, 31)
  Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem5.TextSize = New System.Drawing.Size(55, 20)
  '
  'LayoutVehicle
  '
  Me.LayoutVehicle.Control = Me.LookUpVehicle
  resources.ApplyResources(Me.LayoutVehicle, "LayoutVehicle")
  Me.LayoutVehicle.Location = New System.Drawing.Point(0, 62)
  Me.LayoutVehicle.Name = "LayoutVehicle"
  Me.LayoutVehicle.Size = New System.Drawing.Size(283, 33)
  Me.LayoutVehicle.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
  Me.LayoutVehicle.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutVehicle.TextSize = New System.Drawing.Size(35, 20)
  '
  'LayoutBtnCalculate
  '
  Me.LayoutBtnCalculate.Control = Me.btnCalculate
  resources.ApplyResources(Me.LayoutBtnCalculate, "LayoutBtnCalculate")
  Me.LayoutBtnCalculate.Location = New System.Drawing.Point(582, 62)
  Me.LayoutBtnCalculate.Name = "LayoutBtnCalculate"
  Me.LayoutBtnCalculate.Size = New System.Drawing.Size(259, 33)
  Me.LayoutBtnCalculate.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutBtnCalculate.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutBtnCalculate.TextToControlDistance = 0
  Me.LayoutBtnCalculate.TextVisible = False
  '
  'LayoutControlItem6
  '
  Me.LayoutControlItem6.Control = Me.LookUpReqType
  resources.ApplyResources(Me.LayoutControlItem6, "LayoutControlItem6")
  Me.LayoutControlItem6.Location = New System.Drawing.Point(283, 62)
  Me.LayoutControlItem6.Name = "LayoutControlItem6"
  Me.LayoutControlItem6.Size = New System.Drawing.Size(299, 33)
  Me.LayoutControlItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
  Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem6.TextSize = New System.Drawing.Size(75, 20)
  '
  'PrintingSystem1
  '
  Me.PrintingSystem1.ExportOptions.Csv.EncodingType = CType(resources.GetObject("PrintingSystem1.ExportOptions.Csv.EncodingType"), DevExpress.XtraPrinting.EncodingType)
  Me.PrintingSystem1.ExportOptions.Html.CharacterSet = resources.GetString("PrintingSystem1.ExportOptions.Html.CharacterSet")
  Me.PrintingSystem1.ExportOptions.Html.Title = resources.GetString("PrintingSystem1.ExportOptions.Html.Title")
  Me.PrintingSystem1.ExportOptions.Mht.CharacterSet = resources.GetString("PrintingSystem1.ExportOptions.Mht.CharacterSet")
  Me.PrintingSystem1.ExportOptions.Mht.Title = resources.GetString("PrintingSystem1.ExportOptions.Mht.Title")
  Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Application = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Application")
  Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Author = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Author")
  Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Keywords = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Keywords")
  Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Subject = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Subject")
  Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Title = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Title")
  Me.PrintingSystem1.ExportOptions.Text.EncodingType = CType(resources.GetObject("PrintingSystem1.ExportOptions.Text.EncodingType"), DevExpress.XtraPrinting.EncodingType)
  Me.PrintingSystem1.ExportOptions.Xls.SheetName = resources.GetString("PrintingSystem1.ExportOptions.Xls.SheetName")
  Me.PrintingSystem1.Links.AddRange(New Object() {Me.PrintableComponentLink1})
  '
  'PrintableComponentLink1
  '
  Me.PrintableComponentLink1.Component = Me.PivotGridControl1
  Me.PrintableComponentLink1.CustomPaperSize = New System.Drawing.Size(0, 0)
  Me.PrintableComponentLink1.ImageStream = CType(resources.GetObject("PrintableComponentLink1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
  Me.PrintableComponentLink1.Landscape = True
  Me.PrintableComponentLink1.Margins = New System.Drawing.Printing.Margins(20, 20, 30, 30)
  '  Me.PrintableComponentLink1.PageHeaderFooter = New DevExpress.XtraPrinting.PageHeaderFooter(New DevExpress.XtraPrinting.PageHeaderArea(New String() {Global.WinApp.My.Resources.Resources.String1, "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "УПЛАТНИ СМЕТКИ", Global.WinApp.My.Resources.Resources.String1}, New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte)), DevExpress.XtraPrinting.BrickAlignment.Center), New DevExpress.XtraPrinting.PageFooterArea(New String() {"" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Датум: [Date Printed]", "Ознака на општината: -", "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Референт: [User Name]"}, New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte)), DevExpress.XtraPrinting.BrickAlignment.Far))
  Me.PrintableComponentLink1.PaperKind = System.Drawing.Printing.PaperKind.A4
  Me.PrintableComponentLink1.PrintingSystem = Me.PrintingSystem1
  Me.PrintableComponentLink1.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart
  '
  'uxCaclulationPivot
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.LayoutControl1)
  Me.Name = "uxCaclulationPivot"
  Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.LookUpReqType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.RequestTypeListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LookUpVehicle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.VehicleListShortBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.VehicleTextEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CustomerTextEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.PivotGridControl1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CalculationListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutVehicle, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutBtnCalculate, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents CustomerTextEdit As DevExpress.XtraEditors.TextEdit
    Friend WithEvents PivotGridControl1 As DevExpress.XtraPivotGrid.PivotGridControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CalculationListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents fieldCalculationItem As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldBankAccount As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldBank As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldForm As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldAmount As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents btnPrint As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents PrintingSystem1 As DevExpress.XtraPrinting.PrintingSystem
    Friend WithEvents PrintableComponentLink1 As DevExpress.XtraPrinting.PrintableComponentLink
    Friend WithEvents VehicleTextEdit As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnCalculate As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LookUpVehicle As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutVehicle As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutBtnCalculate As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents VehicleListShortBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents LookUpReqType As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents RequestTypeListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem

End Class
