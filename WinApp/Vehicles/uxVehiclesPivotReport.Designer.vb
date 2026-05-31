<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxVehiclesPivotReport
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxVehiclesPivotReport))
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.btnRefresh = New DevExpress.XtraEditors.SimpleButton
    Me.DateEnd = New DevExpress.XtraEditors.DateEdit
    Me.DateStart = New DevExpress.XtraEditors.DateEdit
    Me.btnExit = New DevExpress.XtraEditors.SimpleButton
    Me.btnPrint = New DevExpress.XtraEditors.SimpleButton
    Me.PivotGridControl1 = New DevExpress.XtraPivotGrid.PivotGridControl
    Me.PrintVehiclePivotReportListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.fieldEngineNumber = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldColorCode = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldEnginePower = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldEngineTorque = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldEngineTorqueUnderGass = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldEngineWorkingCapacity = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldEnginePowerOutPut = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldShellNumber = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldMakeDate = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldNumberOfDoors = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldNumberOfSeats = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldNumberOfStandingSeats = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldNumberOfLieingSeats = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldEmptyWaight = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldMaximunAllowedWaight = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldNumberOfAxis = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldPropulsionAxis = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldNumberOfWheels = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldNumberOfPropulsionWheels = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldVehicleSizeHight = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldVehicleSizeWidth = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldVehicleSizeLength = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldBodytypeCode = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldBodytypeDescriprion = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCategoryCode = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCategoryName = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldEcoProgram = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldTechincalDescription = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldEngineTypeCode = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldEngineTechincalDescription = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldIdPrimaryColor = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldIdSecondaryColor = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldModelName = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldYearOfBeginingProduction = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldYearOfEndingProduction = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCompanyName = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCompanyTrademark = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCompanyNameAndTrademark = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldYearOfProduction = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldNameCategoryForPayments = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCodeCategoryForPayments = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCategoryCodeName = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCategoryForPaymentsCodeName = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldBodytypeCodeDescription = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldEnginePowerSource = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldEngineSecondPowerSource = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldDateOfLastRegistrationa = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldLastRegistration = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldLastRegistrationPlace = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldLastRegistrationValidTill = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldId = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldPrimaryColor = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldSecondaryColor = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldPinpoints = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCO = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldCO2 = New DevExpress.XtraPivotGrid.PivotGridField
    Me.fieldBlackening = New DevExpress.XtraPivotGrid.PivotGridField
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
    Me.PrintingSystem1 = New DevExpress.XtraPrinting.PrintingSystem(Me.components)
    Me.PrintableComponentLink1 = New DevExpress.XtraPrinting.PrintableComponentLink(Me.components)
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.DateEnd.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DateEnd.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DateStart.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DateStart.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PivotGridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PrintVehiclePivotReportListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'LayoutControl1
    '
    Me.LayoutControl1.AccessibleDescription = Nothing
    Me.LayoutControl1.AccessibleName = Nothing
    resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
    Me.LayoutControl1.BackgroundImage = Nothing
    Me.LayoutControl1.Controls.Add(Me.btnRefresh)
    Me.LayoutControl1.Controls.Add(Me.DateEnd)
    Me.LayoutControl1.Controls.Add(Me.DateStart)
    Me.LayoutControl1.Controls.Add(Me.btnExit)
    Me.LayoutControl1.Controls.Add(Me.btnPrint)
    Me.LayoutControl1.Controls.Add(Me.PivotGridControl1)
    Me.LayoutControl1.Font = Nothing
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    '
    'btnRefresh
    '
    Me.btnRefresh.AccessibleDescription = Nothing
    Me.btnRefresh.AccessibleName = Nothing
    resources.ApplyResources(Me.btnRefresh, "btnRefresh")
    Me.btnRefresh.BackgroundImage = Nothing
    Me.btnRefresh.MinimumSize = New System.Drawing.Size(0, 40)
    Me.btnRefresh.Name = "btnRefresh"
    Me.btnRefresh.StyleController = Me.LayoutControl1
    '
    'DateEnd
    '
    resources.ApplyResources(Me.DateEnd, "DateEnd")
    Me.DateEnd.BackgroundImage = Nothing
    Me.DateEnd.EditValue = Nothing
    Me.DateEnd.Name = "DateEnd"
    Me.DateEnd.Properties.AccessibleDescription = Nothing
    Me.DateEnd.Properties.AccessibleName = Nothing
    Me.DateEnd.Properties.AutoHeight = CType(resources.GetObject("DateEnd.Properties.AutoHeight"), Boolean)
    Me.DateEnd.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("DateEnd.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.DateEnd.Properties.Mask.AutoComplete = CType(resources.GetObject("DateEnd.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.DateEnd.Properties.Mask.BeepOnError = CType(resources.GetObject("DateEnd.Properties.Mask.BeepOnError"), Boolean)
    Me.DateEnd.Properties.Mask.EditMask = resources.GetString("DateEnd.Properties.Mask.EditMask")
    Me.DateEnd.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DateEnd.Properties.Mask.IgnoreMaskBlank"), Boolean)
    Me.DateEnd.Properties.Mask.MaskType = CType(resources.GetObject("DateEnd.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.DateEnd.Properties.Mask.PlaceHolder = CType(resources.GetObject("DateEnd.Properties.Mask.PlaceHolder"), Char)
    Me.DateEnd.Properties.Mask.SaveLiteral = CType(resources.GetObject("DateEnd.Properties.Mask.SaveLiteral"), Boolean)
    Me.DateEnd.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("DateEnd.Properties.Mask.ShowPlaceHolders"), Boolean)
    Me.DateEnd.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("DateEnd.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.DateEnd.Properties.VistaTimeProperties.AccessibleDescription = Nothing
    Me.DateEnd.Properties.VistaTimeProperties.AccessibleName = Nothing
    Me.DateEnd.Properties.VistaTimeProperties.AutoHeight = CType(resources.GetObject("DateEnd.Properties.VistaTimeProperties.AutoHeight"), Boolean)
    Me.DateEnd.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.DateEnd.Properties.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("DateEnd.Properties.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.DateEnd.Properties.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("DateEnd.Properties.VistaTimeProperties.Mask.BeepOnError"), Boolean)
    Me.DateEnd.Properties.VistaTimeProperties.Mask.EditMask = resources.GetString("DateEnd.Properties.VistaTimeProperties.Mask.EditMask")
    Me.DateEnd.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DateEnd.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
    Me.DateEnd.Properties.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("DateEnd.Properties.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.DateEnd.Properties.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("DateEnd.Properties.VistaTimeProperties.Mask.PlaceHolder"), Char)
    Me.DateEnd.Properties.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("DateEnd.Properties.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
    Me.DateEnd.Properties.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("DateEnd.Properties.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
    Me.DateEnd.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("DateEnd.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.DateEnd.StyleController = Me.LayoutControl1
    '
    'DateStart
    '
    resources.ApplyResources(Me.DateStart, "DateStart")
    Me.DateStart.BackgroundImage = Nothing
    Me.DateStart.EditValue = Nothing
    Me.DateStart.Name = "DateStart"
    Me.DateStart.Properties.AccessibleDescription = Nothing
    Me.DateStart.Properties.AccessibleName = Nothing
    Me.DateStart.Properties.AutoHeight = CType(resources.GetObject("DateStart.Properties.AutoHeight"), Boolean)
    Me.DateStart.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("DateStart.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.DateStart.Properties.Mask.AutoComplete = CType(resources.GetObject("DateStart.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.DateStart.Properties.Mask.BeepOnError = CType(resources.GetObject("DateStart.Properties.Mask.BeepOnError"), Boolean)
    Me.DateStart.Properties.Mask.EditMask = resources.GetString("DateStart.Properties.Mask.EditMask")
    Me.DateStart.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DateStart.Properties.Mask.IgnoreMaskBlank"), Boolean)
    Me.DateStart.Properties.Mask.MaskType = CType(resources.GetObject("DateStart.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.DateStart.Properties.Mask.PlaceHolder = CType(resources.GetObject("DateStart.Properties.Mask.PlaceHolder"), Char)
    Me.DateStart.Properties.Mask.SaveLiteral = CType(resources.GetObject("DateStart.Properties.Mask.SaveLiteral"), Boolean)
    Me.DateStart.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("DateStart.Properties.Mask.ShowPlaceHolders"), Boolean)
    Me.DateStart.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("DateStart.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.DateStart.Properties.VistaTimeProperties.AccessibleDescription = Nothing
    Me.DateStart.Properties.VistaTimeProperties.AccessibleName = Nothing
    Me.DateStart.Properties.VistaTimeProperties.AutoHeight = CType(resources.GetObject("DateStart.Properties.VistaTimeProperties.AutoHeight"), Boolean)
    Me.DateStart.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.DateStart.Properties.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("DateStart.Properties.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
    Me.DateStart.Properties.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("DateStart.Properties.VistaTimeProperties.Mask.BeepOnError"), Boolean)
    Me.DateStart.Properties.VistaTimeProperties.Mask.EditMask = resources.GetString("DateStart.Properties.VistaTimeProperties.Mask.EditMask")
    Me.DateStart.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DateStart.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
    Me.DateStart.Properties.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("DateStart.Properties.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.DateStart.Properties.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("DateStart.Properties.VistaTimeProperties.Mask.PlaceHolder"), Char)
    Me.DateStart.Properties.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("DateStart.Properties.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
    Me.DateStart.Properties.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("DateStart.Properties.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
    Me.DateStart.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("DateStart.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.DateStart.StyleController = Me.LayoutControl1
    '
    'btnExit
    '
    Me.btnExit.AccessibleDescription = Nothing
    Me.btnExit.AccessibleName = Nothing
    resources.ApplyResources(Me.btnExit, "btnExit")
    Me.btnExit.BackgroundImage = Nothing
    Me.btnExit.MinimumSize = New System.Drawing.Size(0, 40)
    Me.btnExit.Name = "btnExit"
    Me.btnExit.StyleController = Me.LayoutControl1
    '
    'btnPrint
    '
    Me.btnPrint.AccessibleDescription = Nothing
    Me.btnPrint.AccessibleName = Nothing
    resources.ApplyResources(Me.btnPrint, "btnPrint")
    Me.btnPrint.BackgroundImage = Nothing
    Me.btnPrint.MinimumSize = New System.Drawing.Size(0, 40)
    Me.btnPrint.Name = "btnPrint"
    Me.btnPrint.StyleController = Me.LayoutControl1
    '
    'PivotGridControl1
    '
    Me.PivotGridControl1.AccessibleDescription = Nothing
    Me.PivotGridControl1.AccessibleName = Nothing
    resources.ApplyResources(Me.PivotGridControl1, "PivotGridControl1")
    Me.PivotGridControl1.BackgroundImage = Nothing
    Me.PivotGridControl1.Cursor = System.Windows.Forms.Cursors.Default
    Me.PivotGridControl1.DataSource = Me.PrintVehiclePivotReportListBindingSource
    Me.PivotGridControl1.Fields.AddRange(New DevExpress.XtraPivotGrid.PivotGridField() {Me.fieldEngineNumber, Me.fieldColorCode, Me.fieldEnginePower, Me.fieldEngineTorque, Me.fieldEngineTorqueUnderGass, Me.fieldEngineWorkingCapacity, Me.fieldEnginePowerOutPut, Me.fieldShellNumber, Me.fieldMakeDate, Me.fieldNumberOfDoors, Me.fieldNumberOfSeats, Me.fieldNumberOfStandingSeats, Me.fieldNumberOfLieingSeats, Me.fieldEmptyWaight, Me.fieldMaximunAllowedWaight, Me.fieldNumberOfAxis, Me.fieldPropulsionAxis, Me.fieldNumberOfWheels, Me.fieldNumberOfPropulsionWheels, Me.fieldVehicleSizeHight, Me.fieldVehicleSizeWidth, Me.fieldVehicleSizeLength, Me.fieldBodytypeCode, Me.fieldBodytypeDescriprion, Me.fieldCategoryCode, Me.fieldCategoryName, Me.fieldEcoProgram, Me.fieldTechincalDescription, Me.fieldEngineTypeCode, Me.fieldEngineTechincalDescription, Me.fieldIdPrimaryColor, Me.fieldIdSecondaryColor, Me.fieldModelName, Me.fieldYearOfBeginingProduction, Me.fieldYearOfEndingProduction, Me.fieldCompanyName, Me.fieldCompanyTrademark, Me.fieldCompanyNameAndTrademark, Me.fieldYearOfProduction, Me.fieldNameCategoryForPayments, Me.fieldCodeCategoryForPayments, Me.fieldCategoryCodeName, Me.fieldCategoryForPaymentsCodeName, Me.fieldBodytypeCodeDescription, Me.fieldEnginePowerSource, Me.fieldEngineSecondPowerSource, Me.fieldDateOfLastRegistrationa, Me.fieldLastRegistration, Me.fieldLastRegistrationPlace, Me.fieldLastRegistrationValidTill, Me.fieldId, Me.fieldPrimaryColor, Me.fieldSecondaryColor, Me.fieldPinpoints, Me.fieldCO, Me.fieldCO2, Me.fieldBlackening})
    Me.PivotGridControl1.Name = "PivotGridControl1"
    Me.PivotGridControl1.OLAPConnectionString = Nothing
    '
    'PrintVehiclePivotReportListBindingSource
    '
    Me.PrintVehiclePivotReportListBindingSource.DataSource = GetType(VTE.Library.printVehiclePivotReportList)
    '
    'fieldEngineNumber
    '
    Me.fieldEngineNumber.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldEngineNumber.AreaIndex = 0
    resources.ApplyResources(Me.fieldEngineNumber, "fieldEngineNumber")
    Me.fieldEngineNumber.Name = "fieldEngineNumber"
    '
    'fieldColorCode
    '
    Me.fieldColorCode.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldColorCode.AreaIndex = 3
    resources.ApplyResources(Me.fieldColorCode, "fieldColorCode")
    Me.fieldColorCode.Name = "fieldColorCode"
    Me.fieldColorCode.Visible = False
    '
    'fieldEnginePower
    '
    Me.fieldEnginePower.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldEnginePower.AreaIndex = 1
    resources.ApplyResources(Me.fieldEnginePower, "fieldEnginePower")
    Me.fieldEnginePower.Name = "fieldEnginePower"
    '
    'fieldEngineTorque
    '
    Me.fieldEngineTorque.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldEngineTorque.AreaIndex = 4
    resources.ApplyResources(Me.fieldEngineTorque, "fieldEngineTorque")
    Me.fieldEngineTorque.Name = "fieldEngineTorque"
    Me.fieldEngineTorque.Visible = False
    '
    'fieldEngineTorqueUnderGass
    '
    Me.fieldEngineTorqueUnderGass.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldEngineTorqueUnderGass.AreaIndex = 4
    resources.ApplyResources(Me.fieldEngineTorqueUnderGass, "fieldEngineTorqueUnderGass")
    Me.fieldEngineTorqueUnderGass.Name = "fieldEngineTorqueUnderGass"
    Me.fieldEngineTorqueUnderGass.Visible = False
    '
    'fieldEngineWorkingCapacity
    '
    Me.fieldEngineWorkingCapacity.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldEngineWorkingCapacity.AreaIndex = 2
    resources.ApplyResources(Me.fieldEngineWorkingCapacity, "fieldEngineWorkingCapacity")
    Me.fieldEngineWorkingCapacity.Name = "fieldEngineWorkingCapacity"
    '
    'fieldEnginePowerOutPut
    '
    Me.fieldEnginePowerOutPut.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldEnginePowerOutPut.AreaIndex = 5
    resources.ApplyResources(Me.fieldEnginePowerOutPut, "fieldEnginePowerOutPut")
    Me.fieldEnginePowerOutPut.Name = "fieldEnginePowerOutPut"
    Me.fieldEnginePowerOutPut.Visible = False
    '
    'fieldShellNumber
    '
    Me.fieldShellNumber.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldShellNumber.AreaIndex = 30
    resources.ApplyResources(Me.fieldShellNumber, "fieldShellNumber")
    Me.fieldShellNumber.Name = "fieldShellNumber"
    '
    'fieldMakeDate
    '
    Me.fieldMakeDate.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldMakeDate.AreaIndex = 3
    resources.ApplyResources(Me.fieldMakeDate, "fieldMakeDate")
    Me.fieldMakeDate.Name = "fieldMakeDate"
    '
    'fieldNumberOfDoors
    '
    Me.fieldNumberOfDoors.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldNumberOfDoors.AreaIndex = 4
    resources.ApplyResources(Me.fieldNumberOfDoors, "fieldNumberOfDoors")
    Me.fieldNumberOfDoors.Name = "fieldNumberOfDoors"
    '
    'fieldNumberOfSeats
    '
    Me.fieldNumberOfSeats.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldNumberOfSeats.AreaIndex = 5
    resources.ApplyResources(Me.fieldNumberOfSeats, "fieldNumberOfSeats")
    Me.fieldNumberOfSeats.Name = "fieldNumberOfSeats"
    '
    'fieldNumberOfStandingSeats
    '
    Me.fieldNumberOfStandingSeats.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldNumberOfStandingSeats.AreaIndex = 6
    resources.ApplyResources(Me.fieldNumberOfStandingSeats, "fieldNumberOfStandingSeats")
    Me.fieldNumberOfStandingSeats.Name = "fieldNumberOfStandingSeats"
    '
    'fieldNumberOfLieingSeats
    '
    Me.fieldNumberOfLieingSeats.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldNumberOfLieingSeats.AreaIndex = 7
    resources.ApplyResources(Me.fieldNumberOfLieingSeats, "fieldNumberOfLieingSeats")
    Me.fieldNumberOfLieingSeats.Name = "fieldNumberOfLieingSeats"
    '
    'fieldEmptyWaight
    '
    Me.fieldEmptyWaight.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldEmptyWaight.AreaIndex = 8
    resources.ApplyResources(Me.fieldEmptyWaight, "fieldEmptyWaight")
    Me.fieldEmptyWaight.Name = "fieldEmptyWaight"
    '
    'fieldMaximunAllowedWaight
    '
    Me.fieldMaximunAllowedWaight.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldMaximunAllowedWaight.AreaIndex = 9
    resources.ApplyResources(Me.fieldMaximunAllowedWaight, "fieldMaximunAllowedWaight")
    Me.fieldMaximunAllowedWaight.Name = "fieldMaximunAllowedWaight"
    '
    'fieldNumberOfAxis
    '
    Me.fieldNumberOfAxis.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldNumberOfAxis.AreaIndex = 17
    resources.ApplyResources(Me.fieldNumberOfAxis, "fieldNumberOfAxis")
    Me.fieldNumberOfAxis.Name = "fieldNumberOfAxis"
    '
    'fieldPropulsionAxis
    '
    Me.fieldPropulsionAxis.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldPropulsionAxis.AreaIndex = 18
    resources.ApplyResources(Me.fieldPropulsionAxis, "fieldPropulsionAxis")
    Me.fieldPropulsionAxis.Name = "fieldPropulsionAxis"
    '
    'fieldNumberOfWheels
    '
    Me.fieldNumberOfWheels.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldNumberOfWheels.AreaIndex = 19
    resources.ApplyResources(Me.fieldNumberOfWheels, "fieldNumberOfWheels")
    Me.fieldNumberOfWheels.Name = "fieldNumberOfWheels"
    '
    'fieldNumberOfPropulsionWheels
    '
    Me.fieldNumberOfPropulsionWheels.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldNumberOfPropulsionWheels.AreaIndex = 22
    resources.ApplyResources(Me.fieldNumberOfPropulsionWheels, "fieldNumberOfPropulsionWheels")
    Me.fieldNumberOfPropulsionWheels.Name = "fieldNumberOfPropulsionWheels"
    '
    'fieldVehicleSizeHight
    '
    Me.fieldVehicleSizeHight.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldVehicleSizeHight.AreaIndex = 20
    resources.ApplyResources(Me.fieldVehicleSizeHight, "fieldVehicleSizeHight")
    Me.fieldVehicleSizeHight.Name = "fieldVehicleSizeHight"
    '
    'fieldVehicleSizeWidth
    '
    Me.fieldVehicleSizeWidth.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldVehicleSizeWidth.AreaIndex = 23
    resources.ApplyResources(Me.fieldVehicleSizeWidth, "fieldVehicleSizeWidth")
    Me.fieldVehicleSizeWidth.Name = "fieldVehicleSizeWidth"
    '
    'fieldVehicleSizeLength
    '
    Me.fieldVehicleSizeLength.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldVehicleSizeLength.AreaIndex = 21
    resources.ApplyResources(Me.fieldVehicleSizeLength, "fieldVehicleSizeLength")
    Me.fieldVehicleSizeLength.Name = "fieldVehicleSizeLength"
    '
    'fieldBodytypeCode
    '
    Me.fieldBodytypeCode.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldBodytypeCode.AreaIndex = 20
    resources.ApplyResources(Me.fieldBodytypeCode, "fieldBodytypeCode")
    Me.fieldBodytypeCode.Name = "fieldBodytypeCode"
    Me.fieldBodytypeCode.Visible = False
    '
    'fieldBodytypeDescriprion
    '
    Me.fieldBodytypeDescriprion.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldBodytypeDescriprion.AreaIndex = 20
    resources.ApplyResources(Me.fieldBodytypeDescriprion, "fieldBodytypeDescriprion")
    Me.fieldBodytypeDescriprion.Name = "fieldBodytypeDescriprion"
    Me.fieldBodytypeDescriprion.Visible = False
    '
    'fieldCategoryCode
    '
    Me.fieldCategoryCode.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCategoryCode.AreaIndex = 22
    resources.ApplyResources(Me.fieldCategoryCode, "fieldCategoryCode")
    Me.fieldCategoryCode.Name = "fieldCategoryCode"
    Me.fieldCategoryCode.Visible = False
    '
    'fieldCategoryName
    '
    Me.fieldCategoryName.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCategoryName.AreaIndex = 22
    resources.ApplyResources(Me.fieldCategoryName, "fieldCategoryName")
    Me.fieldCategoryName.Name = "fieldCategoryName"
    Me.fieldCategoryName.Visible = False
    '
    'fieldEcoProgram
    '
    Me.fieldEcoProgram.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldEcoProgram.AreaIndex = 10
    resources.ApplyResources(Me.fieldEcoProgram, "fieldEcoProgram")
    Me.fieldEcoProgram.Name = "fieldEcoProgram"
    '
    'fieldTechincalDescription
    '
    Me.fieldTechincalDescription.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldTechincalDescription.AreaIndex = 21
    resources.ApplyResources(Me.fieldTechincalDescription, "fieldTechincalDescription")
    Me.fieldTechincalDescription.Name = "fieldTechincalDescription"
    Me.fieldTechincalDescription.Visible = False
    '
    'fieldEngineTypeCode
    '
    Me.fieldEngineTypeCode.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldEngineTypeCode.AreaIndex = 11
    resources.ApplyResources(Me.fieldEngineTypeCode, "fieldEngineTypeCode")
    Me.fieldEngineTypeCode.Name = "fieldEngineTypeCode"
    '
    'fieldEngineTechincalDescription
    '
    Me.fieldEngineTechincalDescription.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldEngineTechincalDescription.AreaIndex = 22
    resources.ApplyResources(Me.fieldEngineTechincalDescription, "fieldEngineTechincalDescription")
    Me.fieldEngineTechincalDescription.Name = "fieldEngineTechincalDescription"
    Me.fieldEngineTechincalDescription.Visible = False
    '
    'fieldIdPrimaryColor
    '
    Me.fieldIdPrimaryColor.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldIdPrimaryColor.AreaIndex = 33
    resources.ApplyResources(Me.fieldIdPrimaryColor, "fieldIdPrimaryColor")
    Me.fieldIdPrimaryColor.Name = "fieldIdPrimaryColor"
    Me.fieldIdPrimaryColor.Visible = False
    '
    'fieldIdSecondaryColor
    '
    Me.fieldIdSecondaryColor.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldIdSecondaryColor.AreaIndex = 33
    resources.ApplyResources(Me.fieldIdSecondaryColor, "fieldIdSecondaryColor")
    Me.fieldIdSecondaryColor.Name = "fieldIdSecondaryColor"
    Me.fieldIdSecondaryColor.Visible = False
    '
    'fieldModelName
    '
    Me.fieldModelName.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldModelName.AreaIndex = 31
    resources.ApplyResources(Me.fieldModelName, "fieldModelName")
    Me.fieldModelName.Name = "fieldModelName"
    '
    'fieldYearOfBeginingProduction
    '
    Me.fieldYearOfBeginingProduction.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldYearOfBeginingProduction.AreaIndex = 34
    resources.ApplyResources(Me.fieldYearOfBeginingProduction, "fieldYearOfBeginingProduction")
    Me.fieldYearOfBeginingProduction.Name = "fieldYearOfBeginingProduction"
    Me.fieldYearOfBeginingProduction.Visible = False
    '
    'fieldYearOfEndingProduction
    '
    Me.fieldYearOfEndingProduction.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldYearOfEndingProduction.AreaIndex = 34
    resources.ApplyResources(Me.fieldYearOfEndingProduction, "fieldYearOfEndingProduction")
    Me.fieldYearOfEndingProduction.Name = "fieldYearOfEndingProduction"
    Me.fieldYearOfEndingProduction.Visible = False
    '
    'fieldCompanyName
    '
    Me.fieldCompanyName.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCompanyName.AreaIndex = 34
    resources.ApplyResources(Me.fieldCompanyName, "fieldCompanyName")
    Me.fieldCompanyName.Name = "fieldCompanyName"
    Me.fieldCompanyName.Visible = False
    '
    'fieldCompanyTrademark
    '
    Me.fieldCompanyTrademark.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCompanyTrademark.AreaIndex = 23
    resources.ApplyResources(Me.fieldCompanyTrademark, "fieldCompanyTrademark")
    Me.fieldCompanyTrademark.Name = "fieldCompanyTrademark"
    Me.fieldCompanyTrademark.Visible = False
    '
    'fieldCompanyNameAndTrademark
    '
    Me.fieldCompanyNameAndTrademark.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
    Me.fieldCompanyNameAndTrademark.AreaIndex = 2
    resources.ApplyResources(Me.fieldCompanyNameAndTrademark, "fieldCompanyNameAndTrademark")
    Me.fieldCompanyNameAndTrademark.Name = "fieldCompanyNameAndTrademark"
    '
    'fieldYearOfProduction
    '
    Me.fieldYearOfProduction.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
    Me.fieldYearOfProduction.AreaIndex = 3
    resources.ApplyResources(Me.fieldYearOfProduction, "fieldYearOfProduction")
    Me.fieldYearOfProduction.Name = "fieldYearOfProduction"
    '
    'fieldNameCategoryForPayments
    '
    Me.fieldNameCategoryForPayments.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldNameCategoryForPayments.AreaIndex = 31
    resources.ApplyResources(Me.fieldNameCategoryForPayments, "fieldNameCategoryForPayments")
    Me.fieldNameCategoryForPayments.Name = "fieldNameCategoryForPayments"
    Me.fieldNameCategoryForPayments.Visible = False
    '
    'fieldCodeCategoryForPayments
    '
    Me.fieldCodeCategoryForPayments.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCodeCategoryForPayments.AreaIndex = 31
    resources.ApplyResources(Me.fieldCodeCategoryForPayments, "fieldCodeCategoryForPayments")
    Me.fieldCodeCategoryForPayments.Name = "fieldCodeCategoryForPayments"
    Me.fieldCodeCategoryForPayments.Visible = False
    '
    'fieldCategoryCodeName
    '
    Me.fieldCategoryCodeName.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCategoryCodeName.AreaIndex = 32
    resources.ApplyResources(Me.fieldCategoryCodeName, "fieldCategoryCodeName")
    Me.fieldCategoryCodeName.Name = "fieldCategoryCodeName"
    Me.fieldCategoryCodeName.SortBySummaryInfo.FieldName = resources.GetString("fieldCategoryCodeName.SortBySummaryInfo.FieldName")
    Me.fieldCategoryCodeName.SortBySummaryInfo.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Count
    '
    'fieldCategoryForPaymentsCodeName
    '
    Me.fieldCategoryForPaymentsCodeName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
    Me.fieldCategoryForPaymentsCodeName.AreaIndex = 0
    resources.ApplyResources(Me.fieldCategoryForPaymentsCodeName, "fieldCategoryForPaymentsCodeName")
    Me.fieldCategoryForPaymentsCodeName.Name = "fieldCategoryForPaymentsCodeName"
    Me.fieldCategoryForPaymentsCodeName.SortBySummaryInfo.FieldName = resources.GetString("fieldCategoryForPaymentsCodeName.SortBySummaryInfo.FieldName")
    Me.fieldCategoryForPaymentsCodeName.SortBySummaryInfo.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Count
    '
    'fieldBodytypeCodeDescription
    '
    Me.fieldBodytypeCodeDescription.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
    Me.fieldBodytypeCodeDescription.AreaIndex = 1
    resources.ApplyResources(Me.fieldBodytypeCodeDescription, "fieldBodytypeCodeDescription")
    Me.fieldBodytypeCodeDescription.Name = "fieldBodytypeCodeDescription"
    '
    'fieldEnginePowerSource
    '
    Me.fieldEnginePowerSource.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldEnginePowerSource.AreaIndex = 16
    resources.ApplyResources(Me.fieldEnginePowerSource, "fieldEnginePowerSource")
    Me.fieldEnginePowerSource.Name = "fieldEnginePowerSource"
    '
    'fieldEngineSecondPowerSource
    '
    Me.fieldEngineSecondPowerSource.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldEngineSecondPowerSource.AreaIndex = 13
    resources.ApplyResources(Me.fieldEngineSecondPowerSource, "fieldEngineSecondPowerSource")
    Me.fieldEngineSecondPowerSource.Name = "fieldEngineSecondPowerSource"
    '
    'fieldDateOfLastRegistrationa
    '
    Me.fieldDateOfLastRegistrationa.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldDateOfLastRegistrationa.AreaIndex = 14
    resources.ApplyResources(Me.fieldDateOfLastRegistrationa, "fieldDateOfLastRegistrationa")
    Me.fieldDateOfLastRegistrationa.Name = "fieldDateOfLastRegistrationa"
    '
    'fieldLastRegistration
    '
    Me.fieldLastRegistration.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldLastRegistration.AreaIndex = 29
    resources.ApplyResources(Me.fieldLastRegistration, "fieldLastRegistration")
    Me.fieldLastRegistration.Name = "fieldLastRegistration"
    '
    'fieldLastRegistrationPlace
    '
    Me.fieldLastRegistrationPlace.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldLastRegistrationPlace.AreaIndex = 12
    resources.ApplyResources(Me.fieldLastRegistrationPlace, "fieldLastRegistrationPlace")
    Me.fieldLastRegistrationPlace.Name = "fieldLastRegistrationPlace"
    '
    'fieldLastRegistrationValidTill
    '
    Me.fieldLastRegistrationValidTill.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldLastRegistrationValidTill.AreaIndex = 15
    resources.ApplyResources(Me.fieldLastRegistrationValidTill, "fieldLastRegistrationValidTill")
    Me.fieldLastRegistrationValidTill.Name = "fieldLastRegistrationValidTill"
    '
    'fieldId
    '
    Me.fieldId.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea
    Me.fieldId.AreaIndex = 0
    resources.ApplyResources(Me.fieldId, "fieldId")
    Me.fieldId.Name = "fieldId"
    Me.fieldId.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Count
    '
    'fieldPrimaryColor
    '
    Me.fieldPrimaryColor.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldPrimaryColor.AreaIndex = 26
    resources.ApplyResources(Me.fieldPrimaryColor, "fieldPrimaryColor")
    Me.fieldPrimaryColor.Name = "fieldPrimaryColor"
    '
    'fieldSecondaryColor
    '
    Me.fieldSecondaryColor.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldSecondaryColor.AreaIndex = 27
    resources.ApplyResources(Me.fieldSecondaryColor, "fieldSecondaryColor")
    Me.fieldSecondaryColor.Name = "fieldSecondaryColor"
    '
    'fieldPinpoints
    '
    Me.fieldPinpoints.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldPinpoints.AreaIndex = 25
    resources.ApplyResources(Me.fieldPinpoints, "fieldPinpoints")
    Me.fieldPinpoints.Name = "fieldPinpoints"
    '
    'fieldCO
    '
    Me.fieldCO.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCO.AreaIndex = 33
    resources.ApplyResources(Me.fieldCO, "fieldCO")
    Me.fieldCO.Name = "fieldCO"
    '
    'fieldCO2
    '
    Me.fieldCO2.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldCO2.AreaIndex = 24
    resources.ApplyResources(Me.fieldCO2, "fieldCO2")
    Me.fieldCO2.Name = "fieldCO2"
    '
    'fieldBlackening
    '
    Me.fieldBlackening.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
    Me.fieldBlackening.AreaIndex = 28
    resources.ApplyResources(Me.fieldBlackening, "fieldBlackening")
    Me.fieldBlackening.Name = "fieldBlackening"
    '
    'LayoutControlGroup1
    '
    resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem6, Me.LayoutControlGroup2})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(889, 473)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.TextVisible = False
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.PivotGridControl1
    resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 55)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(887, 416)
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem1.TextToControlDistance = 0
    Me.LayoutControlItem1.TextVisible = False
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.btnPrint
    resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
    Me.LayoutControlItem2.Location = New System.Drawing.Point(475, 0)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(185, 55)
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem2.TextToControlDistance = 0
    Me.LayoutControlItem2.TextVisible = False
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.btnExit
    resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
    Me.LayoutControlItem3.Location = New System.Drawing.Point(660, 0)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(227, 55)
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem3.TextToControlDistance = 0
    Me.LayoutControlItem3.TextVisible = False
    '
    'LayoutControlItem6
    '
    Me.LayoutControlItem6.Control = Me.btnRefresh
    resources.ApplyResources(Me.LayoutControlItem6, "LayoutControlItem6")
    Me.LayoutControlItem6.Location = New System.Drawing.Point(289, 0)
    Me.LayoutControlItem6.Name = "LayoutControlItem6"
    Me.LayoutControlItem6.Size = New System.Drawing.Size(186, 55)
    Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem6.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem6.TextToControlDistance = 0
    Me.LayoutControlItem6.TextVisible = False
    '
    'LayoutControlGroup2
    '
    resources.ApplyResources(Me.LayoutControlGroup2, "LayoutControlGroup2")
    Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem4, Me.LayoutControlItem5})
    Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
    Me.LayoutControlGroup2.Size = New System.Drawing.Size(289, 55)
    '
    'LayoutControlItem4
    '
    Me.LayoutControlItem4.Control = Me.DateStart
    resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
    Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem4.Name = "LayoutControlItem4"
    Me.LayoutControlItem4.Size = New System.Drawing.Size(151, 31)
    Me.LayoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem4.TextSize = New System.Drawing.Size(19, 20)
    '
    'LayoutControlItem5
    '
    Me.LayoutControlItem5.Control = Me.DateEnd
    resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
    Me.LayoutControlItem5.Location = New System.Drawing.Point(151, 0)
    Me.LayoutControlItem5.Name = "LayoutControlItem5"
    Me.LayoutControlItem5.Size = New System.Drawing.Size(132, 31)
    Me.LayoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem5.TextSize = New System.Drawing.Size(18, 20)
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
    Me.PrintableComponentLink1.PaperKind = System.Drawing.Printing.PaperKind.A4
    Me.PrintableComponentLink1.PrintingSystem = Me.PrintingSystem1
    Me.PrintableComponentLink1.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart
    '
    'uxVehiclesPivotReport
    '
    Me.AccessibleDescription = Nothing
    Me.AccessibleName = Nothing
    resources.ApplyResources(Me, "$this")
    Me.BackgroundImage = Nothing
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "uxVehiclesPivotReport"
    Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.DateEnd.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DateEnd.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DateStart.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DateStart.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PivotGridControl1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PrintVehiclePivotReportListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnPrint As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PivotGridControl1 As DevExpress.XtraPivotGrid.PivotGridControl
    Friend WithEvents PrintVehiclePivotReportListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents fieldEngineNumber As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldColorCode As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldEnginePower As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldEngineTorque As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldEngineTorqueUnderGass As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldEngineWorkingCapacity As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldEnginePowerOutPut As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldShellNumber As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldMakeDate As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldNumberOfDoors As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldNumberOfSeats As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldNumberOfStandingSeats As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldNumberOfLieingSeats As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldEmptyWaight As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldMaximunAllowedWaight As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldNumberOfAxis As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldPropulsionAxis As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldNumberOfWheels As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldNumberOfPropulsionWheels As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldVehicleSizeHight As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldVehicleSizeWidth As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldVehicleSizeLength As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldBodytypeCode As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldBodytypeDescriprion As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCategoryCode As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCategoryName As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldEcoProgram As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldTechincalDescription As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldEngineTypeCode As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldEngineTechincalDescription As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldIdPrimaryColor As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldIdSecondaryColor As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldModelName As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldYearOfBeginingProduction As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldYearOfEndingProduction As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCompanyName As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCompanyTrademark As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents fieldCompanyNameAndTrademark As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldYearOfProduction As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldNameCategoryForPayments As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCodeCategoryForPayments As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCategoryCodeName As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCategoryForPaymentsCodeName As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldBodytypeCodeDescription As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents PrintingSystem1 As DevExpress.XtraPrinting.PrintingSystem
    Friend WithEvents PrintableComponentLink1 As DevExpress.XtraPrinting.PrintableComponentLink
    Friend WithEvents fieldEnginePowerSource As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldEngineSecondPowerSource As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldDateOfLastRegistrationa As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldLastRegistration As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldLastRegistrationPlace As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldLastRegistrationValidTill As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldId As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents btnRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents DateEnd As DevExpress.XtraEditors.DateEdit
    Friend WithEvents DateStart As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents fieldPrimaryColor As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldSecondaryColor As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldPinpoints As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCO As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCO2 As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldBlackening As DevExpress.XtraPivotGrid.PivotGridField

End Class
