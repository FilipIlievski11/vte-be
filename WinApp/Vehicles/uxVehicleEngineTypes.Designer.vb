<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxVehicleEngineTypes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxVehicleEngineTypes))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.VehicleEngineTypesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdVehicleMaker = New DevExpress.XtraGrid.Columns.GridColumn
        Me.LookUpEditMaker = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.VehicleMakerListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.colEngineType = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colTechincalDescriptionEcoProgram = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdDefaultPowerSource = New DevExpress.XtraGrid.Columns.GridColumn
        Me.LookUpEditPowerSource = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.VehicleEnginePowerSourceTypeListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.colDefaultPower = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDefaultPowerOutPut = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDefaultTorque = New DevExpress.XtraGrid.Columns.GridColumn
        Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleEngineTypesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LookUpEditMaker, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleMakerListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LookUpEditPowerSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleEnginePowerSourceTypeListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.AccessibleDescription = Nothing
        Me.LayoutControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
        Me.LayoutControl1.BackgroundImage = Nothing
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
        Me.LayoutControl1.Font = Nothing
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        '
        'GridControl1
        '
        Me.GridControl1.AccessibleDescription = Nothing
        Me.GridControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.GridControl1, "GridControl1")
        Me.GridControl1.BackgroundImage = Nothing
        Me.GridControl1.DataSource = Me.VehicleEngineTypesBindingSource
        Me.GridControl1.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.GridControl1.EmbeddedNavigator.AccessibleName = Nothing
        Me.GridControl1.EmbeddedNavigator.Anchor = CType(resources.GetObject("GridControl1.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.GridControl1.EmbeddedNavigator.BackgroundImage = Nothing
        Me.GridControl1.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("GridControl1.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.GridControl1.EmbeddedNavigator.ImeMode = CType(resources.GetObject("GridControl1.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.GridControl1.EmbeddedNavigator.TextLocation = CType(resources.GetObject("GridControl1.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.GridControl1.EmbeddedNavigator.ToolTip = resources.GetString("GridControl1.EmbeddedNavigator.ToolTip")
        Me.GridControl1.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("GridControl1.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.GridControl1.EmbeddedNavigator.ToolTipTitle = resources.GetString("GridControl1.EmbeddedNavigator.ToolTipTitle")
        Me.GridControl1.Font = Nothing
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.LookUpEditPowerSource, Me.LookUpEditMaker})
        Me.GridControl1.UseEmbeddedNavigator = True
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'VehicleEngineTypesBindingSource
        '
        Me.VehicleEngineTypesBindingSource.DataSource = GetType(VTE.Library.VehicleEngineTypes)
        '
        'GridView1
        '
        Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
        Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView1.Appearance.Row.Options.UseTextOptions = True
        Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        resources.ApplyResources(Me.GridView1, "GridView1")
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdVehicleMaker, Me.colEngineType, Me.colTechincalDescriptionEcoProgram, Me.colIdDefaultPowerSource, Me.colDefaultPower, Me.colDefaultPowerOutPut, Me.colDefaultTorque})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsNavigation.AutoFocusNewRow = True
        Me.GridView1.OptionsNavigation.EnterMoveNextColumn = True
        Me.GridView1.OptionsView.ShowAutoFilterRow = True
        Me.GridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways
        '
        'colId
        '
        resources.ApplyResources(Me.colId, "colId")
        Me.colId.FieldName = "Id"
        Me.colId.Name = "colId"
        Me.colId.OptionsColumn.ReadOnly = True
        '
        'colIdVehicleMaker
        '
        resources.ApplyResources(Me.colIdVehicleMaker, "colIdVehicleMaker")
        Me.colIdVehicleMaker.ColumnEdit = Me.LookUpEditMaker
        Me.colIdVehicleMaker.FieldName = "IdVehicleMaker"
        Me.colIdVehicleMaker.Name = "colIdVehicleMaker"
        '
        'LookUpEditMaker
        '
        Me.LookUpEditMaker.AccessibleDescription = Nothing
        Me.LookUpEditMaker.AccessibleName = Nothing
        resources.ApplyResources(Me.LookUpEditMaker, "LookUpEditMaker")
        Me.LookUpEditMaker.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditMaker.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditMaker.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditMaker.Buttons2"), CType(resources.GetObject("LookUpEditMaker.Buttons3"), Integer), CType(resources.GetObject("LookUpEditMaker.Buttons4"), Boolean), CType(resources.GetObject("LookUpEditMaker.Buttons5"), Boolean), CType(resources.GetObject("LookUpEditMaker.Buttons6"), Boolean), CType(resources.GetObject("LookUpEditMaker.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing)})
        Me.LookUpEditMaker.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, "", False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdCountry", "IdCountry", 55, DevExpress.Utils.FormatType.Numeric, "", False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CompanyName", "CompanyName", 78, DevExpress.Utils.FormatType.None, "", True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CompanyTrademark", "CompanyTrademark", 102, DevExpress.Utils.FormatType.None, "", False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CompanyNameAndTrademark", "CompanyNameAndTrademark", 148, DevExpress.Utils.FormatType.None, "", False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
        Me.LookUpEditMaker.DataSource = Me.VehicleMakerListBindingSource
        Me.LookUpEditMaker.DisplayMember = "CompanyName"
        Me.LookUpEditMaker.Name = "LookUpEditMaker"
        Me.LookUpEditMaker.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.LookUpEditMaker.ValueMember = "Id"
        '
        'VehicleMakerListBindingSource
        '
        Me.VehicleMakerListBindingSource.DataSource = GetType(VTE.Library.VehicleMakerList)
        '
        'colEngineType
        '
        resources.ApplyResources(Me.colEngineType, "colEngineType")
        Me.colEngineType.FieldName = "EngineType"
        Me.colEngineType.Name = "colEngineType"
        '
        'colTechincalDescriptionEcoProgram
        '
        resources.ApplyResources(Me.colTechincalDescriptionEcoProgram, "colTechincalDescriptionEcoProgram")
        Me.colTechincalDescriptionEcoProgram.FieldName = "TechincalDescriptionEcoProgram"
        Me.colTechincalDescriptionEcoProgram.Name = "colTechincalDescriptionEcoProgram"
        '
        'colIdDefaultPowerSource
        '
        resources.ApplyResources(Me.colIdDefaultPowerSource, "colIdDefaultPowerSource")
        Me.colIdDefaultPowerSource.ColumnEdit = Me.LookUpEditPowerSource
        Me.colIdDefaultPowerSource.FieldName = "IdDefaultPowerSource"
        Me.colIdDefaultPowerSource.Name = "colIdDefaultPowerSource"
        '
        'LookUpEditPowerSource
        '
        Me.LookUpEditPowerSource.AccessibleDescription = Nothing
        Me.LookUpEditPowerSource.AccessibleName = Nothing
        resources.ApplyResources(Me.LookUpEditPowerSource, "LookUpEditPowerSource")
        Me.LookUpEditPowerSource.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditPowerSource.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditPowerSource.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditPowerSource.Buttons2"), CType(resources.GetObject("LookUpEditPowerSource.Buttons3"), Integer), CType(resources.GetObject("LookUpEditPowerSource.Buttons4"), Boolean), CType(resources.GetObject("LookUpEditPowerSource.Buttons5"), Boolean), CType(resources.GetObject("LookUpEditPowerSource.Buttons6"), Boolean), CType(resources.GetObject("LookUpEditPowerSource.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing)})
        Me.LookUpEditPowerSource.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, "", False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("PowerSourceName", "PowerSource", 96, DevExpress.Utils.FormatType.None, "", True, DevExpress.Utils.HorzAlignment.Near)})
        Me.LookUpEditPowerSource.DataSource = Me.VehicleEnginePowerSourceTypeListBindingSource
        Me.LookUpEditPowerSource.DisplayMember = "PowerSourceName"
        Me.LookUpEditPowerSource.Name = "LookUpEditPowerSource"
        Me.LookUpEditPowerSource.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.LookUpEditPowerSource.ValueMember = "Id"
        '
        'VehicleEnginePowerSourceTypeListBindingSource
        '
        Me.VehicleEnginePowerSourceTypeListBindingSource.DataSource = GetType(VTE.Library.VehicleEnginePowerSourceTypeList)
        '
        'colDefaultPower
        '
        resources.ApplyResources(Me.colDefaultPower, "colDefaultPower")
        Me.colDefaultPower.FieldName = "DefaultPower"
        Me.colDefaultPower.Name = "colDefaultPower"
        '
        'colDefaultPowerOutPut
        '
        resources.ApplyResources(Me.colDefaultPowerOutPut, "colDefaultPowerOutPut")
        Me.colDefaultPowerOutPut.FieldName = "DefaultPowerOutPut"
        Me.colDefaultPowerOutPut.Name = "colDefaultPowerOutPut"
        '
        'colDefaultTorque
        '
        resources.ApplyResources(Me.colDefaultTorque, "colDefaultTorque")
        Me.colDefaultTorque.FieldName = "DefaultTorque"
        Me.colDefaultTorque.Name = "colDefaultTorque"
        '
        'UxKopcinja1
        '
        Me.UxKopcinja1.AccessibleDescription = Nothing
        Me.UxKopcinja1.AccessibleName = Nothing
        resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
        Me.UxKopcinja1.BackgroundImage = Nothing
        Me.UxKopcinja1.Name = "UxKopcinja1"
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.LayoutControlItem1, Me.SplitterItem1})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(813, 426)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.GridControl1
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 82)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(811, 342)
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.UxKopcinja1
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.MaxSize = New System.Drawing.Size(0, 76)
        Me.LayoutControlItem1.MinSize = New System.Drawing.Size(111, 76)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(811, 76)
        Me.LayoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'SplitterItem1
        '
        resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
        Me.SplitterItem1.Location = New System.Drawing.Point(0, 76)
        Me.SplitterItem1.Name = "SplitterItem1"
        Me.SplitterItem1.Size = New System.Drawing.Size(811, 6)
        '
        'uxVehicleEngineTypes
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "uxVehicleEngineTypes"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleEngineTypesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LookUpEditMaker, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleMakerListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LookUpEditPowerSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleEnginePowerSourceTypeListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
  Friend WithEvents VehicleEngineTypesBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LookUpEditPowerSource As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents VehicleEnginePowerSourceTypeListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdVehicleMaker As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents LookUpEditMaker As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents colEngineType As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colTechincalDescriptionEcoProgram As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdDefaultPowerSource As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDefaultPower As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDefaultPowerOutPut As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDefaultTorque As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents VehicleMakerListBindingSource As System.Windows.Forms.BindingSource

End Class
