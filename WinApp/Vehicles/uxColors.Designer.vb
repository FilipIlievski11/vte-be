<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxColors
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxColors))
    Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode
    Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.colId1 = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdColor = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colColorCode1 = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colColorDescription1 = New DevExpress.XtraGrid.Columns.GridColumn
    Me.GridControl1 = New DevExpress.XtraGrid.GridControl
    Me.ColorsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colColorCode = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colColorDescription = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colColor = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colNewColorEffects = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colNewColorCode = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colNewColorDarkness = New DevExpress.XtraGrid.Columns.GridColumn
    Me.RepositoryItemLookUpEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
    Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
    Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
    CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.ColorsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'GridView2
    '
    Me.GridView2.Appearance.HeaderPanel.Options.UseTextOptions = True
    Me.GridView2.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    Me.GridView2.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    Me.GridView2.Appearance.Row.Options.UseTextOptions = True
    Me.GridView2.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    Me.GridView2.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    resources.ApplyResources(Me.GridView2, "GridView2")
    Me.GridView2.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId1, Me.colIdColor, Me.colColorCode1, Me.colColorDescription1})
    Me.GridView2.GridControl = Me.GridControl1
    Me.GridView2.Name = "GridView2"
    Me.GridView2.OptionsNavigation.AutoFocusNewRow = True
    Me.GridView2.OptionsNavigation.EnterMoveNextColumn = True
    Me.GridView2.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
    '
    'colId1
    '
    resources.ApplyResources(Me.colId1, "colId1")
    Me.colId1.FieldName = "Id"
    Me.colId1.Name = "colId1"
    Me.colId1.OptionsColumn.ReadOnly = True
    '
    'colIdColor
    '
    resources.ApplyResources(Me.colIdColor, "colIdColor")
    Me.colIdColor.FieldName = "IdColor"
    Me.colIdColor.Name = "colIdColor"
    '
    'colColorCode1
    '
    resources.ApplyResources(Me.colColorCode1, "colColorCode1")
    Me.colColorCode1.FieldName = "ColorCode"
    Me.colColorCode1.Name = "colColorCode1"
    '
    'colColorDescription1
    '
    resources.ApplyResources(Me.colColorDescription1, "colColorDescription1")
    Me.colColorDescription1.FieldName = "ColorDescription"
    Me.colColorDescription1.Name = "colColorDescription1"
    '
    'GridControl1
    '
    Me.GridControl1.AccessibleDescription = Nothing
    Me.GridControl1.AccessibleName = Nothing
    resources.ApplyResources(Me.GridControl1, "GridControl1")
    Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.GridControl1, False)
    Me.GridControl1.BackgroundImage = Nothing
    Me.GridControl1.DataSource = Me.ColorsBindingSource
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
    GridLevelNode1.LevelTemplate = Me.GridView2
    GridLevelNode1.RelationName = "ColorDetails"
    Me.GridControl1.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
    Me.GridControl1.MainView = Me.GridView1
    Me.GridControl1.Name = "GridControl1"
    Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemLookUpEdit1})
    Me.GridControl1.UseEmbeddedNavigator = True
    Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
    '
    'ColorsBindingSource
    '
    Me.ColorsBindingSource.DataSource = GetType(VTE.Library.Colors)
    Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.ColorsBindingSource, False)
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
    Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colColorCode, Me.colColorDescription, Me.colColor, Me.colNewColorEffects, Me.colNewColorCode, Me.colNewColorDarkness})
    Me.GridView1.GridControl = Me.GridControl1
    Me.GridView1.Name = "GridView1"
    Me.GridView1.OptionsDetail.AllowExpandEmptyDetails = True
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
    'colColorCode
    '
    resources.ApplyResources(Me.colColorCode, "colColorCode")
    Me.colColorCode.FieldName = "ColorCode"
    Me.colColorCode.Name = "colColorCode"
    '
    'colColorDescription
    '
    resources.ApplyResources(Me.colColorDescription, "colColorDescription")
    Me.colColorDescription.FieldName = "ColorDescription"
    Me.colColorDescription.Name = "colColorDescription"
    '
    'colColor
    '
    resources.ApplyResources(Me.colColor, "colColor")
    Me.colColor.FieldName = "Color"
    Me.colColor.Name = "colColor"
    '
    'colNewColorEffects
    '
    resources.ApplyResources(Me.colNewColorEffects, "colNewColorEffects")
    Me.colNewColorEffects.FieldName = "NewColorEffects"
    Me.colNewColorEffects.Name = "colNewColorEffects"
    '
    'colNewColorCode
    '
    resources.ApplyResources(Me.colNewColorCode, "colNewColorCode")
    Me.colNewColorCode.FieldName = "NewColorCode"
    Me.colNewColorCode.Name = "colNewColorCode"
    '
    'colNewColorDarkness
    '
    resources.ApplyResources(Me.colNewColorDarkness, "colNewColorDarkness")
    Me.colNewColorDarkness.FieldName = "NewColorDarkness"
    Me.colNewColorDarkness.Name = "colNewColorDarkness"
    '
    'RepositoryItemLookUpEdit1
    '
    Me.RepositoryItemLookUpEdit1.AccessibleDescription = Nothing
    Me.RepositoryItemLookUpEdit1.AccessibleName = Nothing
    resources.ApplyResources(Me.RepositoryItemLookUpEdit1, "RepositoryItemLookUpEdit1")
    Me.RepositoryItemLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.RepositoryItemLookUpEdit1.DisplayMember = "ColorDescription"
    Me.RepositoryItemLookUpEdit1.Name = "RepositoryItemLookUpEdit1"
    Me.RepositoryItemLookUpEdit1.ValueMember = "Id"
    '
    'LayoutControl1
    '
    Me.LayoutControl1.AccessibleDescription = Nothing
    Me.LayoutControl1.AccessibleName = Nothing
    resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
    Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LayoutControl1, False)
    Me.LayoutControl1.BackgroundImage = Nothing
    Me.LayoutControl1.Controls.Add(Me.GridControl1)
    Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
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
    'LayoutControlGroup1
    '
    resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.SplitterItem1, Me.LayoutControlItem2, Me.LayoutControlItem1})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "Root"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(659, 480)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.TextVisible = False
    '
    'SplitterItem1
    '
    resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
    Me.SplitterItem1.Location = New System.Drawing.Point(0, 62)
    Me.SplitterItem1.Name = "SplitterItem1"
    Me.SplitterItem1.Size = New System.Drawing.Size(655, 6)
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.GridControl1
    resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
    Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 68)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(655, 408)
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
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(655, 62)
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem1.TextToControlDistance = 0
    Me.LayoutControlItem1.TextVisible = False
    '
    'DxErrorProvider1
    '
    Me.DxErrorProvider1.ContainerControl = Me
    Me.DxErrorProvider1.DataSource = Me.ColorsBindingSource
    '
    'uxColors
    '
    Me.AccessibleDescription = Nothing
    Me.AccessibleName = Nothing
    Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
    resources.ApplyResources(Me, "$this")
    Me.BackgroundImage = Nothing
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "uxColors"
    Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
    CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.ColorsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents ColorsBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colColorCode As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colColorDescription As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents colColor As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents RepositoryItemLookUpEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdColor As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colColorCode1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colColorDescription1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colNewColorEffects As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colNewColorCode As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colNewColorDarkness As DevExpress.XtraGrid.Columns.GridColumn

End Class
