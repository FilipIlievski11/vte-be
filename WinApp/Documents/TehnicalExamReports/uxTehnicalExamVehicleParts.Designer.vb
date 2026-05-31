<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxTehnicalExamVehicleParts
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxTehnicalExamVehicleParts))
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
  Me.TehnicalExamVehiclePartsGridControl = New DevExpress.XtraGrid.GridControl
  Me.TehnicalExamVehiclePartsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIdCategoryVehicleParts = New DevExpress.XtraGrid.Columns.GridColumn
  Me.LookUpEditCategory = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Me.TehnicalExamVehiclePartsListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.colCode = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colDescription = New DevExpress.XtraGrid.Columns.GridColumn
  Me.TreeView1 = New System.Windows.Forms.TreeView
  Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
  Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
  Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
  Me.SplitterItem2 = New DevExpress.XtraLayout.SplitterItem
  Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
  Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
  Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.LayoutControl1.SuspendLayout()
  CType(Me.TehnicalExamVehiclePartsGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.TehnicalExamVehiclePartsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LookUpEditCategory, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.TehnicalExamVehiclePartsListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.SplitterItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'LayoutControl1
  '
  Me.LayoutControl1.AccessibleDescription = Nothing
  Me.LayoutControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LayoutControl1, False)
  Me.LayoutControl1.BackgroundImage = Nothing
  Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
  Me.LayoutControl1.Controls.Add(Me.TehnicalExamVehiclePartsGridControl)
  Me.LayoutControl1.Controls.Add(Me.TreeView1)
  Me.LayoutControl1.Font = Nothing
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
  '
  'UxKopcinja1
  '
  Me.UxKopcinja1.AccessibleDescription = Nothing
  Me.UxKopcinja1.AccessibleName = Nothing
  resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UxKopcinja1, False)
  Me.UxKopcinja1.BackgroundImage = Nothing
  Me.UxKopcinja1.Name = "UxKopcinja1"
  '
  'TehnicalExamVehiclePartsGridControl
  '
  Me.TehnicalExamVehiclePartsGridControl.AccessibleDescription = Nothing
  Me.TehnicalExamVehiclePartsGridControl.AccessibleName = Nothing
  resources.ApplyResources(Me.TehnicalExamVehiclePartsGridControl, "TehnicalExamVehiclePartsGridControl")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.TehnicalExamVehiclePartsGridControl, False)
  Me.TehnicalExamVehiclePartsGridControl.BackgroundImage = Nothing
  Me.TehnicalExamVehiclePartsGridControl.DataSource = Me.TehnicalExamVehiclePartsBindingSource
  Me.TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
  Me.TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.AccessibleName = Nothing
  Me.TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
  Me.TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.BackgroundImage = Nothing
  Me.TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
  Me.TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
  Me.TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
  Me.TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.ToolTip = resources.GetString("TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.ToolTip")
  Me.TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
  Me.TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("TehnicalExamVehiclePartsGridControl.EmbeddedNavigator.ToolTipTitle")
  Me.TehnicalExamVehiclePartsGridControl.Font = Nothing
  Me.TehnicalExamVehiclePartsGridControl.MainView = Me.GridView1
  Me.TehnicalExamVehiclePartsGridControl.Name = "TehnicalExamVehiclePartsGridControl"
  Me.TehnicalExamVehiclePartsGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.LookUpEditCategory})
  Me.TehnicalExamVehiclePartsGridControl.UseEmbeddedNavigator = True
  Me.TehnicalExamVehiclePartsGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'TehnicalExamVehiclePartsBindingSource
  '
  Me.TehnicalExamVehiclePartsBindingSource.DataSource = GetType(VTE.Library.TehnicalExamVehiclePart)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.TehnicalExamVehiclePartsBindingSource, False)
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
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdCategoryVehicleParts, Me.colCode, Me.colDescription})
  Me.GridView1.GridControl = Me.TehnicalExamVehiclePartsGridControl
  Me.GridView1.Name = "GridView1"
  Me.GridView1.OptionsNavigation.AutoFocusNewRow = True
  Me.GridView1.OptionsNavigation.EnterMoveNextColumn = True
  Me.GridView1.OptionsView.ShowAutoFilterRow = True
  '
  'colId
  '
  resources.ApplyResources(Me.colId, "colId")
  Me.colId.FieldName = "Id"
  Me.colId.Name = "colId"
  Me.colId.OptionsColumn.ReadOnly = True
  '
  'colIdCategoryVehicleParts
  '
  resources.ApplyResources(Me.colIdCategoryVehicleParts, "colIdCategoryVehicleParts")
  Me.colIdCategoryVehicleParts.ColumnEdit = Me.LookUpEditCategory
  Me.colIdCategoryVehicleParts.FieldName = "IdCategoryVehicleParts"
  Me.colIdCategoryVehicleParts.Name = "colIdCategoryVehicleParts"
  '
  'LookUpEditCategory
  '
  Me.LookUpEditCategory.AccessibleDescription = Nothing
  Me.LookUpEditCategory.AccessibleName = Nothing
  resources.ApplyResources(Me.LookUpEditCategory, "LookUpEditCategory")
  Me.LookUpEditCategory.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCategory.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
  Me.LookUpEditCategory.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdCategoryVehicleParts", "IdCategoryVehicleParts", 119, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Code", "Code", 31, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Description", "Description", 59, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CodeAndDescription", "CodeAndDescription", 103, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("PicturePath", "PicturePath", 61, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
  Me.LookUpEditCategory.DataSource = Me.TehnicalExamVehiclePartsListBindingSource
  Me.LookUpEditCategory.DisplayMember = "CodeAndDescription"
  Me.LookUpEditCategory.Name = "LookUpEditCategory"
  Me.LookUpEditCategory.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
  Me.LookUpEditCategory.ValueMember = "Id"
  '
  'TehnicalExamVehiclePartsListBindingSource
  '
  Me.TehnicalExamVehiclePartsListBindingSource.DataSource = GetType(VTE.Library.TehnicalExamVehiclePartsList)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.TehnicalExamVehiclePartsListBindingSource, False)
  '
  'colCode
  '
  resources.ApplyResources(Me.colCode, "colCode")
  Me.colCode.FieldName = "Code"
  Me.colCode.Name = "colCode"
  '
  'colDescription
  '
  resources.ApplyResources(Me.colDescription, "colDescription")
  Me.colDescription.FieldName = "Description"
  Me.colDescription.Name = "colDescription"
  '
  'TreeView1
  '
  Me.TreeView1.AccessibleDescription = Nothing
  Me.TreeView1.AccessibleName = Nothing
  resources.ApplyResources(Me.TreeView1, "TreeView1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.TreeView1, False)
  Me.TreeView1.BackgroundImage = Nothing
  Me.TreeView1.Font = Nothing
  Me.TreeView1.Name = "TreeView1"
  '
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.SplitterItem1, Me.LayoutControlItem3, Me.SplitterItem2})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "Root"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(613, 426)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.TehnicalExamVehiclePartsGridControl
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 60)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(609, 203)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem1.TextToControlDistance = 0
  Me.LayoutControlItem1.TextVisible = False
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.UxKopcinja1
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(609, 54)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem2.TextToControlDistance = 0
  Me.LayoutControlItem2.TextVisible = False
  '
  'SplitterItem1
  '
  resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
  Me.SplitterItem1.Location = New System.Drawing.Point(0, 54)
  Me.SplitterItem1.Name = "SplitterItem1"
  Me.SplitterItem1.Size = New System.Drawing.Size(609, 6)
  '
  'LayoutControlItem3
  '
  Me.LayoutControlItem3.Control = Me.TreeView1
  resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
  Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 269)
  Me.LayoutControlItem3.Name = "LayoutControlItem3"
  Me.LayoutControlItem3.Size = New System.Drawing.Size(609, 153)
  Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem3.TextToControlDistance = 0
  Me.LayoutControlItem3.TextVisible = False
  '
  'SplitterItem2
  '
  resources.ApplyResources(Me.SplitterItem2, "SplitterItem2")
  Me.SplitterItem2.Location = New System.Drawing.Point(0, 263)
  Me.SplitterItem2.Name = "SplitterItem2"
  Me.SplitterItem2.Size = New System.Drawing.Size(609, 6)
  '
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.TehnicalExamVehiclePartsBindingSource
  '
  'uxTehnicalExamVehicleParts
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.LayoutControl1)
  Me.Name = "uxTehnicalExamVehicleParts"
  Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.TehnicalExamVehiclePartsGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.TehnicalExamVehiclePartsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LookUpEditCategory, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.TehnicalExamVehiclePartsListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.SplitterItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents TehnicalExamVehiclePartsGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents TehnicalExamVehiclePartsBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdCategoryVehicleParts As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCode As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDescription As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents LookUpEditCategory As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents TehnicalExamVehiclePartsListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem2 As DevExpress.XtraLayout.SplitterItem

End Class
