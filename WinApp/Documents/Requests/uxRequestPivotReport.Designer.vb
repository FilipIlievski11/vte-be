<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxRequestPivotReport
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxRequestPivotReport))
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.btnShow = New DevExpress.XtraEditors.SimpleButton
  Me.DateEditEnd = New DevExpress.XtraEditors.DateEdit
  Me.DateEditStart = New DevExpress.XtraEditors.DateEdit
  Me.btnExit = New DevExpress.XtraEditors.SimpleButton
  Me.btnPrint = New DevExpress.XtraEditors.SimpleButton
  Me.PivotGridControl1 = New DevExpress.XtraPivotGrid.PivotGridControl
  Me.RequestPivotReportListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.fieldIdVehicle = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldShellNumber = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldCategoryCode = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldCategoryName = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldCategory = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldDateCreated = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldDateEnded = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldRequestId = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldNote = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldTypeName = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldStatus = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldRegNumber = New DevExpress.XtraPivotGrid.PivotGridField
  Me.fieldCustomer = New DevExpress.XtraPivotGrid.PivotGridField
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
  CType(Me.DateEditEnd.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DateEditEnd.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DateEditStart.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DateEditStart.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.PivotGridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.RequestPivotReportListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
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
  Me.LayoutControl1.Controls.Add(Me.btnShow)
  Me.LayoutControl1.Controls.Add(Me.DateEditEnd)
  Me.LayoutControl1.Controls.Add(Me.DateEditStart)
  Me.LayoutControl1.Controls.Add(Me.btnExit)
  Me.LayoutControl1.Controls.Add(Me.btnPrint)
  Me.LayoutControl1.Controls.Add(Me.PivotGridControl1)
  Me.LayoutControl1.Font = Nothing
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
  '
  'btnShow
  '
  Me.btnShow.AccessibleDescription = Nothing
  Me.btnShow.AccessibleName = Nothing
  resources.ApplyResources(Me.btnShow, "btnShow")
  Me.btnShow.BackgroundImage = Nothing
  Me.btnShow.MinimumSize = New System.Drawing.Size(0, 40)
  Me.btnShow.Name = "btnShow"
  Me.btnShow.StyleController = Me.LayoutControl1
  '
  'DateEditEnd
  '
  resources.ApplyResources(Me.DateEditEnd, "DateEditEnd")
  Me.DateEditEnd.BackgroundImage = Nothing
  Me.DateEditEnd.EditValue = Nothing
  Me.DateEditEnd.Name = "DateEditEnd"
  Me.DateEditEnd.Properties.AccessibleDescription = Nothing
  Me.DateEditEnd.Properties.AccessibleName = Nothing
  Me.DateEditEnd.Properties.AutoHeight = CType(resources.GetObject("DateEditEnd.Properties.AutoHeight"), Boolean)
  Me.DateEditEnd.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("DateEditEnd.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
  Me.DateEditEnd.Properties.Mask.AutoComplete = CType(resources.GetObject("DateEditEnd.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.DateEditEnd.Properties.Mask.BeepOnError = CType(resources.GetObject("DateEditEnd.Properties.Mask.BeepOnError"), Boolean)
  Me.DateEditEnd.Properties.Mask.EditMask = resources.GetString("DateEditEnd.Properties.Mask.EditMask")
  Me.DateEditEnd.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DateEditEnd.Properties.Mask.IgnoreMaskBlank"), Boolean)
  Me.DateEditEnd.Properties.Mask.MaskType = CType(resources.GetObject("DateEditEnd.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.DateEditEnd.Properties.Mask.PlaceHolder = CType(resources.GetObject("DateEditEnd.Properties.Mask.PlaceHolder"), Char)
  Me.DateEditEnd.Properties.Mask.SaveLiteral = CType(resources.GetObject("DateEditEnd.Properties.Mask.SaveLiteral"), Boolean)
  Me.DateEditEnd.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("DateEditEnd.Properties.Mask.ShowPlaceHolders"), Boolean)
  Me.DateEditEnd.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("DateEditEnd.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.DateEditEnd.Properties.VistaTimeProperties.AccessibleDescription = Nothing
  Me.DateEditEnd.Properties.VistaTimeProperties.AccessibleName = Nothing
  Me.DateEditEnd.Properties.VistaTimeProperties.AutoHeight = CType(resources.GetObject("DateEditEnd.Properties.VistaTimeProperties.AutoHeight"), Boolean)
  Me.DateEditEnd.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
  Me.DateEditEnd.Properties.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("DateEditEnd.Properties.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.DateEditEnd.Properties.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("DateEditEnd.Properties.VistaTimeProperties.Mask.BeepOnError"), Boolean)
  Me.DateEditEnd.Properties.VistaTimeProperties.Mask.EditMask = resources.GetString("DateEditEnd.Properties.VistaTimeProperties.Mask.EditMask")
  Me.DateEditEnd.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DateEditEnd.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
  Me.DateEditEnd.Properties.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("DateEditEnd.Properties.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.DateEditEnd.Properties.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("DateEditEnd.Properties.VistaTimeProperties.Mask.PlaceHolder"), Char)
  Me.DateEditEnd.Properties.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("DateEditEnd.Properties.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
  Me.DateEditEnd.Properties.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("DateEditEnd.Properties.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
  Me.DateEditEnd.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("DateEditEnd.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.DateEditEnd.StyleController = Me.LayoutControl1
  '
  'DateEditStart
  '
  resources.ApplyResources(Me.DateEditStart, "DateEditStart")
  Me.DateEditStart.BackgroundImage = Nothing
  Me.DateEditStart.EditValue = Nothing
  Me.DateEditStart.Name = "DateEditStart"
  Me.DateEditStart.Properties.AccessibleDescription = Nothing
  Me.DateEditStart.Properties.AccessibleName = Nothing
  Me.DateEditStart.Properties.AutoHeight = CType(resources.GetObject("DateEditStart.Properties.AutoHeight"), Boolean)
  Me.DateEditStart.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("DateEditStart.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
  Me.DateEditStart.Properties.Mask.AutoComplete = CType(resources.GetObject("DateEditStart.Properties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.DateEditStart.Properties.Mask.BeepOnError = CType(resources.GetObject("DateEditStart.Properties.Mask.BeepOnError"), Boolean)
  Me.DateEditStart.Properties.Mask.EditMask = resources.GetString("DateEditStart.Properties.Mask.EditMask")
  Me.DateEditStart.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DateEditStart.Properties.Mask.IgnoreMaskBlank"), Boolean)
  Me.DateEditStart.Properties.Mask.MaskType = CType(resources.GetObject("DateEditStart.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.DateEditStart.Properties.Mask.PlaceHolder = CType(resources.GetObject("DateEditStart.Properties.Mask.PlaceHolder"), Char)
  Me.DateEditStart.Properties.Mask.SaveLiteral = CType(resources.GetObject("DateEditStart.Properties.Mask.SaveLiteral"), Boolean)
  Me.DateEditStart.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("DateEditStart.Properties.Mask.ShowPlaceHolders"), Boolean)
  Me.DateEditStart.Properties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("DateEditStart.Properties.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.DateEditStart.Properties.VistaTimeProperties.AccessibleDescription = Nothing
  Me.DateEditStart.Properties.VistaTimeProperties.AccessibleName = Nothing
  Me.DateEditStart.Properties.VistaTimeProperties.AutoHeight = CType(resources.GetObject("DateEditStart.Properties.VistaTimeProperties.AutoHeight"), Boolean)
  Me.DateEditStart.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
  Me.DateEditStart.Properties.VistaTimeProperties.Mask.AutoComplete = CType(resources.GetObject("DateEditStart.Properties.VistaTimeProperties.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.DateEditStart.Properties.VistaTimeProperties.Mask.BeepOnError = CType(resources.GetObject("DateEditStart.Properties.VistaTimeProperties.Mask.BeepOnError"), Boolean)
  Me.DateEditStart.Properties.VistaTimeProperties.Mask.EditMask = resources.GetString("DateEditStart.Properties.VistaTimeProperties.Mask.EditMask")
  Me.DateEditStart.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DateEditStart.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
  Me.DateEditStart.Properties.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("DateEditStart.Properties.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.DateEditStart.Properties.VistaTimeProperties.Mask.PlaceHolder = CType(resources.GetObject("DateEditStart.Properties.VistaTimeProperties.Mask.PlaceHolder"), Char)
  Me.DateEditStart.Properties.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("DateEditStart.Properties.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
  Me.DateEditStart.Properties.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("DateEditStart.Properties.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
  Me.DateEditStart.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("DateEditStart.Properties.VistaTimeProperties.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.DateEditStart.StyleController = Me.LayoutControl1
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
  Me.PivotGridControl1.DataSource = Me.RequestPivotReportListBindingSource
  Me.PivotGridControl1.Fields.AddRange(New DevExpress.XtraPivotGrid.PivotGridField() {Me.fieldIdVehicle, Me.fieldShellNumber, Me.fieldCategoryCode, Me.fieldCategoryName, Me.fieldCategory, Me.fieldDateCreated, Me.fieldDateEnded, Me.fieldRequestId, Me.fieldNote, Me.fieldTypeName, Me.fieldStatus, Me.fieldRegNumber, Me.fieldCustomer})
  Me.PivotGridControl1.Name = "PivotGridControl1"
  Me.PivotGridControl1.OLAPConnectionString = Nothing
  '
  'RequestPivotReportListBindingSource
  '
  Me.RequestPivotReportListBindingSource.DataSource = GetType(VTE.Library.RequestPivotReportList)
  '
  'fieldIdVehicle
  '
  Me.fieldIdVehicle.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldIdVehicle.AreaIndex = 0
  resources.ApplyResources(Me.fieldIdVehicle, "fieldIdVehicle")
  Me.fieldIdVehicle.Name = "fieldIdVehicle"
  Me.fieldIdVehicle.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Count
  '
  'fieldShellNumber
  '
  Me.fieldShellNumber.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldShellNumber.AreaIndex = 1
  resources.ApplyResources(Me.fieldShellNumber, "fieldShellNumber")
  Me.fieldShellNumber.Name = "fieldShellNumber"
  '
  'fieldCategoryCode
  '
  Me.fieldCategoryCode.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldCategoryCode.AreaIndex = 1
  resources.ApplyResources(Me.fieldCategoryCode, "fieldCategoryCode")
  Me.fieldCategoryCode.Name = "fieldCategoryCode"
  Me.fieldCategoryCode.Visible = False
  '
  'fieldCategoryName
  '
  Me.fieldCategoryName.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
  Me.fieldCategoryName.AreaIndex = 2
  resources.ApplyResources(Me.fieldCategoryName, "fieldCategoryName")
  Me.fieldCategoryName.Name = "fieldCategoryName"
  Me.fieldCategoryName.Visible = False
  '
  'fieldCategory
  '
  Me.fieldCategory.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldCategory.AreaIndex = 2
  resources.ApplyResources(Me.fieldCategory, "fieldCategory")
  Me.fieldCategory.Name = "fieldCategory"
  '
  'fieldDateCreated
  '
  Me.fieldDateCreated.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldDateCreated.AreaIndex = 0
  resources.ApplyResources(Me.fieldDateCreated, "fieldDateCreated")
  Me.fieldDateCreated.Name = "fieldDateCreated"
  '
  'fieldDateEnded
  '
  Me.fieldDateEnded.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldDateEnded.AreaIndex = 4
  resources.ApplyResources(Me.fieldDateEnded, "fieldDateEnded")
  Me.fieldDateEnded.Name = "fieldDateEnded"
  Me.fieldDateEnded.Visible = False
  '
  'fieldRequestId
  '
  Me.fieldRequestId.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea
  Me.fieldRequestId.AreaIndex = 0
  resources.ApplyResources(Me.fieldRequestId, "fieldRequestId")
  Me.fieldRequestId.Name = "fieldRequestId"
  Me.fieldRequestId.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Count
  '
  'fieldNote
  '
  Me.fieldNote.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldNote.AreaIndex = 3
  resources.ApplyResources(Me.fieldNote, "fieldNote")
  Me.fieldNote.Name = "fieldNote"
  '
  'fieldTypeName
  '
  Me.fieldTypeName.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea
  Me.fieldTypeName.AreaIndex = 0
  resources.ApplyResources(Me.fieldTypeName, "fieldTypeName")
  Me.fieldTypeName.Name = "fieldTypeName"
  '
  'fieldStatus
  '
  Me.fieldStatus.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldStatus.AreaIndex = 6
  resources.ApplyResources(Me.fieldStatus, "fieldStatus")
  Me.fieldStatus.Name = "fieldStatus"
  '
  'fieldRegNumber
  '
  Me.fieldRegNumber.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldRegNumber.AreaIndex = 4
  resources.ApplyResources(Me.fieldRegNumber, "fieldRegNumber")
  Me.fieldRegNumber.Name = "fieldRegNumber"
  '
  'fieldCustomer
  '
  Me.fieldCustomer.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
  Me.fieldCustomer.AreaIndex = 5
  resources.ApplyResources(Me.fieldCustomer, "fieldCustomer")
  Me.fieldCustomer.Name = "fieldCustomer"
  '
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem6, Me.LayoutControlGroup2})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(843, 428)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.PivotGridControl1
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 55)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(839, 369)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem1.TextToControlDistance = 0
  Me.LayoutControlItem1.TextVisible = False
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.btnPrint
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(502, 0)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(152, 55)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem2.TextToControlDistance = 0
  Me.LayoutControlItem2.TextVisible = False
  '
  'LayoutControlItem3
  '
  Me.LayoutControlItem3.Control = Me.btnExit
  resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
  Me.LayoutControlItem3.Location = New System.Drawing.Point(654, 0)
  Me.LayoutControlItem3.Name = "LayoutControlItem3"
  Me.LayoutControlItem3.Size = New System.Drawing.Size(185, 55)
  Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem3.TextToControlDistance = 0
  Me.LayoutControlItem3.TextVisible = False
  '
  'LayoutControlItem6
  '
  Me.LayoutControlItem6.Control = Me.btnShow
  resources.ApplyResources(Me.LayoutControlItem6, "LayoutControlItem6")
  Me.LayoutControlItem6.Location = New System.Drawing.Point(366, 0)
  Me.LayoutControlItem6.Name = "LayoutControlItem6"
  Me.LayoutControlItem6.Size = New System.Drawing.Size(136, 55)
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
  Me.LayoutControlGroup2.Size = New System.Drawing.Size(366, 55)
  Me.LayoutControlGroup2.Tag = "Interim"
  '
  'LayoutControlItem4
  '
  Me.LayoutControlItem4.Control = Me.DateEditStart
  resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
  Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem4.Name = "LayoutControlItem4"
  Me.LayoutControlItem4.Size = New System.Drawing.Size(180, 31)
  Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem4.TextSize = New System.Drawing.Size(19, 20)
  '
  'LayoutControlItem5
  '
  Me.LayoutControlItem5.Control = Me.DateEditEnd
  resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
  Me.LayoutControlItem5.Location = New System.Drawing.Point(180, 0)
  Me.LayoutControlItem5.Name = "LayoutControlItem5"
  Me.LayoutControlItem5.Size = New System.Drawing.Size(180, 31)
  Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem5.TextSize = New System.Drawing.Size(19, 20)
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
  Me.PrintableComponentLink1.Margins = New System.Drawing.Printing.Margins(50, 50, 50, 50)
  Me.PrintableComponentLink1.PaperKind = System.Drawing.Printing.PaperKind.A4
  Me.PrintableComponentLink1.PrintingSystem = Me.PrintingSystem1
  Me.PrintableComponentLink1.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart
  '
  'uxRequestPivotReport
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.LayoutControl1)
  Me.Name = "uxRequestPivotReport"
  Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.DateEditEnd.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DateEditEnd.Properties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DateEditStart.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DateEditStart.Properties, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.PivotGridControl1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.RequestPivotReportListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents PivotGridControl1 As DevExpress.XtraPivotGrid.PivotGridControl
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnPrint As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents RequestPivotReportListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents fieldIdVehicle As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldShellNumber As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCategoryCode As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCategoryName As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCategory As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldDateCreated As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldDateEnded As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldRequestId As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldNote As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldTypeName As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents PrintingSystem1 As DevExpress.XtraPrinting.PrintingSystem
    Friend WithEvents PrintableComponentLink1 As DevExpress.XtraPrinting.PrintableComponentLink
    Friend WithEvents fieldStatus As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldRegNumber As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldCustomer As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents DateEditEnd As DevExpress.XtraEditors.DateEdit
    Friend WithEvents DateEditStart As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnShow As DevExpress.XtraEditors.SimpleButton
 Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
 Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup

End Class
