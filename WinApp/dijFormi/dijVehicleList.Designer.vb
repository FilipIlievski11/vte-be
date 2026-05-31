<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijVehicleList
    Inherits DevExpress.XtraEditors.XtraForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dijVehicleList))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.btnRefresh = New DevExpress.XtraEditors.SimpleButton
        Me.btnAddNew = New DevExpress.XtraEditors.SimpleButton
        Me.btnExit = New DevExpress.XtraEditors.SimpleButton
        Me.btnSelect = New DevExpress.XtraEditors.SimpleButton
        Me.VehicleListGridControl = New DevExpress.XtraGrid.GridControl
        Me.VehicleListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colShellNumber = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colLastRegistration = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colLastRegistrationValidTill = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colModelName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colVehiceMaker = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colOwnerName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colOwnerMB = New DevExpress.XtraGrid.Columns.GridColumn
        Me.HyperLinkShellNumber = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
        Me.rihleCurrentRegistationNumber = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.VehicleListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HyperLinkShellNumber, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rihleCurrentRegistationNumber, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.AccessibleDescription = Nothing
        Me.LayoutControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
        Me.LayoutControl1.BackgroundImage = Nothing
        Me.LayoutControl1.Controls.Add(Me.btnRefresh)
        Me.LayoutControl1.Controls.Add(Me.btnAddNew)
        Me.LayoutControl1.Controls.Add(Me.btnExit)
        Me.LayoutControl1.Controls.Add(Me.btnSelect)
        Me.LayoutControl1.Controls.Add(Me.VehicleListGridControl)
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
        Me.btnRefresh.MinimumSize = New System.Drawing.Size(0, 36)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.StyleController = Me.LayoutControl1
        '
        'btnAddNew
        '
        Me.btnAddNew.AccessibleDescription = Nothing
        Me.btnAddNew.AccessibleName = Nothing
        resources.ApplyResources(Me.btnAddNew, "btnAddNew")
        Me.btnAddNew.BackgroundImage = Nothing
        Me.btnAddNew.Name = "btnAddNew"
        Me.btnAddNew.StyleController = Me.LayoutControl1
        '
        'btnExit
        '
        Me.btnExit.AccessibleDescription = Nothing
        Me.btnExit.AccessibleName = Nothing
        resources.ApplyResources(Me.btnExit, "btnExit")
        Me.btnExit.BackgroundImage = Nothing
        Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExit.Name = "btnExit"
        Me.btnExit.StyleController = Me.LayoutControl1
        '
        'btnSelect
        '
        Me.btnSelect.AccessibleDescription = Nothing
        Me.btnSelect.AccessibleName = Nothing
        resources.ApplyResources(Me.btnSelect, "btnSelect")
        Me.btnSelect.BackgroundImage = Nothing
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.StyleController = Me.LayoutControl1
        '
        'VehicleListGridControl
        '
        Me.VehicleListGridControl.AccessibleDescription = Nothing
        Me.VehicleListGridControl.AccessibleName = Nothing
        resources.ApplyResources(Me.VehicleListGridControl, "VehicleListGridControl")
        Me.VehicleListGridControl.BackgroundImage = Nothing
        Me.VehicleListGridControl.DataSource = Me.VehicleListBindingSource
        Me.VehicleListGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.VehicleListGridControl.EmbeddedNavigator.AccessibleName = Nothing
        Me.VehicleListGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("VehicleListGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.VehicleListGridControl.EmbeddedNavigator.BackgroundImage = Nothing
        Me.VehicleListGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("VehicleListGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.VehicleListGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("VehicleListGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.VehicleListGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("VehicleListGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.VehicleListGridControl.EmbeddedNavigator.ToolTip = resources.GetString("VehicleListGridControl.EmbeddedNavigator.ToolTip")
        Me.VehicleListGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("VehicleListGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.VehicleListGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("VehicleListGridControl.EmbeddedNavigator.ToolTipTitle")
        Me.VehicleListGridControl.Font = Nothing
        Me.VehicleListGridControl.MainView = Me.GridView1
        Me.VehicleListGridControl.Name = "VehicleListGridControl"
        Me.VehicleListGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.HyperLinkShellNumber, Me.rihleCurrentRegistationNumber})
        Me.VehicleListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
        '
        'VehicleListBindingSource
        '
        Me.VehicleListBindingSource.DataSource = GetType(VTE.Library.VehiclesListShortListAll)
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
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colShellNumber, Me.colLastRegistration, Me.colLastRegistrationValidTill, Me.colModelName, Me.colVehiceMaker, Me.colOwnerName, Me.colOwnerMB})
        Me.GridView1.GridControl = Me.VehicleListGridControl
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ShowAutoFilterRow = True
        Me.GridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways
        Me.GridView1.OptionsView.ShowFooter = True
        Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colId, DevExpress.Data.ColumnSortOrder.Descending)})
        '
        'colId
        '
        resources.ApplyResources(Me.colId, "colId")
        Me.colId.FieldName = "Id"
        Me.colId.Name = "colId"
        Me.colId.OptionsColumn.ReadOnly = True
        Me.colId.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value
        '
        'colShellNumber
        '
        resources.ApplyResources(Me.colShellNumber, "colShellNumber")
        Me.colShellNumber.FieldName = "ShellNumber"
        Me.colShellNumber.Name = "colShellNumber"
        Me.colShellNumber.OptionsColumn.ReadOnly = True
        '
        'colLastRegistration
        '
        resources.ApplyResources(Me.colLastRegistration, "colLastRegistration")
        Me.colLastRegistration.FieldName = "LastRegistration"
        Me.colLastRegistration.Name = "colLastRegistration"
        Me.colLastRegistration.OptionsColumn.ReadOnly = True
        '
        'colLastRegistrationValidTill
        '
        resources.ApplyResources(Me.colLastRegistrationValidTill, "colLastRegistrationValidTill")
        Me.colLastRegistrationValidTill.FieldName = "LastRegistrationValidTill"
        Me.colLastRegistrationValidTill.Name = "colLastRegistrationValidTill"
        Me.colLastRegistrationValidTill.OptionsColumn.ReadOnly = True
        '
        'colModelName
        '
        resources.ApplyResources(Me.colModelName, "colModelName")
        Me.colModelName.FieldName = "ModelName"
        Me.colModelName.Name = "colModelName"
        Me.colModelName.OptionsColumn.ReadOnly = True
        '
        'colVehiceMaker
        '
        resources.ApplyResources(Me.colVehiceMaker, "colVehiceMaker")
        Me.colVehiceMaker.FieldName = "VehiceMaker"
        Me.colVehiceMaker.Name = "colVehiceMaker"
        Me.colVehiceMaker.OptionsColumn.ReadOnly = True
        '
        'colOwnerName
        '
        resources.ApplyResources(Me.colOwnerName, "colOwnerName")
        Me.colOwnerName.FieldName = "OwnerName"
        Me.colOwnerName.Name = "colOwnerName"
        Me.colOwnerName.OptionsColumn.ReadOnly = True
        '
        'colOwnerMB
        '
        resources.ApplyResources(Me.colOwnerMB, "colOwnerMB")
        Me.colOwnerMB.FieldName = "OwnerMB"
        Me.colOwnerMB.Name = "colOwnerMB"
        Me.colOwnerMB.OptionsColumn.ReadOnly = True
        '
        'HyperLinkShellNumber
        '
        Me.HyperLinkShellNumber.AccessibleDescription = Nothing
        Me.HyperLinkShellNumber.AccessibleName = Nothing
        resources.ApplyResources(Me.HyperLinkShellNumber, "HyperLinkShellNumber")
        Me.HyperLinkShellNumber.Mask.AutoComplete = CType(resources.GetObject("HyperLinkShellNumber.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.HyperLinkShellNumber.Mask.BeepOnError = CType(resources.GetObject("HyperLinkShellNumber.Mask.BeepOnError"), Boolean)
        Me.HyperLinkShellNumber.Mask.EditMask = resources.GetString("HyperLinkShellNumber.Mask.EditMask")
        Me.HyperLinkShellNumber.Mask.IgnoreMaskBlank = CType(resources.GetObject("HyperLinkShellNumber.Mask.IgnoreMaskBlank"), Boolean)
        Me.HyperLinkShellNumber.Mask.MaskType = CType(resources.GetObject("HyperLinkShellNumber.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.HyperLinkShellNumber.Mask.PlaceHolder = CType(resources.GetObject("HyperLinkShellNumber.Mask.PlaceHolder"), Char)
        Me.HyperLinkShellNumber.Mask.SaveLiteral = CType(resources.GetObject("HyperLinkShellNumber.Mask.SaveLiteral"), Boolean)
        Me.HyperLinkShellNumber.Mask.ShowPlaceHolders = CType(resources.GetObject("HyperLinkShellNumber.Mask.ShowPlaceHolders"), Boolean)
        Me.HyperLinkShellNumber.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("HyperLinkShellNumber.Mask.UseMaskAsDisplayFormat"), Boolean)
        Me.HyperLinkShellNumber.Name = "HyperLinkShellNumber"
        '
        'rihleCurrentRegistationNumber
        '
        Me.rihleCurrentRegistationNumber.AccessibleDescription = Nothing
        Me.rihleCurrentRegistationNumber.AccessibleName = Nothing
        resources.ApplyResources(Me.rihleCurrentRegistationNumber, "rihleCurrentRegistationNumber")
        Me.rihleCurrentRegistationNumber.Mask.AutoComplete = CType(resources.GetObject("rihleCurrentRegistationNumber.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.rihleCurrentRegistationNumber.Mask.BeepOnError = CType(resources.GetObject("rihleCurrentRegistationNumber.Mask.BeepOnError"), Boolean)
        Me.rihleCurrentRegistationNumber.Mask.EditMask = resources.GetString("rihleCurrentRegistationNumber.Mask.EditMask")
        Me.rihleCurrentRegistationNumber.Mask.IgnoreMaskBlank = CType(resources.GetObject("rihleCurrentRegistationNumber.Mask.IgnoreMaskBlank"), Boolean)
        Me.rihleCurrentRegistationNumber.Mask.MaskType = CType(resources.GetObject("rihleCurrentRegistationNumber.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.rihleCurrentRegistationNumber.Mask.PlaceHolder = CType(resources.GetObject("rihleCurrentRegistationNumber.Mask.PlaceHolder"), Char)
        Me.rihleCurrentRegistationNumber.Mask.SaveLiteral = CType(resources.GetObject("rihleCurrentRegistationNumber.Mask.SaveLiteral"), Boolean)
        Me.rihleCurrentRegistationNumber.Mask.ShowPlaceHolders = CType(resources.GetObject("rihleCurrentRegistationNumber.Mask.ShowPlaceHolders"), Boolean)
        Me.rihleCurrentRegistationNumber.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("rihleCurrentRegistationNumber.Mask.UseMaskAsDisplayFormat"), Boolean)
        Me.rihleCurrentRegistationNumber.Name = "rihleCurrentRegistationNumber"
        '
        'GridView2
        '
        resources.ApplyResources(Me.GridView2, "GridView2")
        Me.GridView2.GridControl = Me.VehicleListGridControl
        Me.GridView2.Name = "GridView2"
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem3, Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem5})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(992, 666)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.VehicleListGridControl
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(990, 617)
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.btnExit
        resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
        Me.LayoutControlItem3.Location = New System.Drawing.Point(717, 617)
        Me.LayoutControlItem3.MaxSize = New System.Drawing.Size(0, 47)
        Me.LayoutControlItem3.MinSize = New System.Drawing.Size(60, 47)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(273, 47)
        Me.LayoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.btnSelect
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(459, 617)
        Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(0, 47)
        Me.LayoutControlItem2.MinSize = New System.Drawing.Size(70, 47)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(258, 47)
        Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.btnAddNew
        resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
        Me.LayoutControlItem4.Location = New System.Drawing.Point(215, 617)
        Me.LayoutControlItem4.MaxSize = New System.Drawing.Size(0, 47)
        Me.LayoutControlItem4.MinSize = New System.Drawing.Size(50, 47)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(244, 47)
        Me.LayoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextToControlDistance = 0
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.btnRefresh
        resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 617)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(215, 47)
        Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem5.TextToControlDistance = 0
        Me.LayoutControlItem5.TextVisible = False
        '
        'dijVehicleList
        '
        Me.AcceptButton = Me.btnSelect
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnExit
        Me.Controls.Add(Me.LayoutControl1)
        Me.Icon = Nothing
        Me.KeyPreview = True
        Me.Name = "dijVehicleList"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.VehicleListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HyperLinkShellNumber, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rihleCurrentRegistationNumber, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnSelect As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents VehicleListGridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents VehicleListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents HyperLinkShellNumber As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents btnAddNew As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents rihleCurrentRegistationNumber As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colShellNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLastRegistration As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLastRegistrationValidTill As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colModelName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVehiceMaker As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colOwnerName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colOwnerMB As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btnRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
End Class
