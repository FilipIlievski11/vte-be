<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxVehicleUse
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxVehicleUse))
        Me.VehicleUsesBindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.VehicleUsesGridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView3 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.GridColumn1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridColumn2 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colRegistrationMask = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridView4 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.LayoutControl2 = New DevExpress.XtraLayout.LayoutControl
        Me.UxKopcinja2 = New VTE.BaseParts.uxKopcinja
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem2 = New DevExpress.XtraLayout.SplitterItem
        CType(Me.VehicleUsesBindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleUsesGridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl2.SuspendLayout()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'VehicleUsesBindingSource1
        '
        Me.VehicleUsesBindingSource1.DataSource = GetType(VTE.Library.VehicleUse)
        '
        'VehicleUsesGridControl1
        '
        Me.VehicleUsesGridControl1.AccessibleDescription = Nothing
        Me.VehicleUsesGridControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.VehicleUsesGridControl1, "VehicleUsesGridControl1")
        Me.VehicleUsesGridControl1.BackgroundImage = Nothing
        Me.VehicleUsesGridControl1.DataSource = Me.VehicleUsesBindingSource1
        Me.VehicleUsesGridControl1.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.VehicleUsesGridControl1.EmbeddedNavigator.AccessibleName = Nothing
        Me.VehicleUsesGridControl1.EmbeddedNavigator.Anchor = CType(resources.GetObject("VehicleUsesGridControl1.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.VehicleUsesGridControl1.EmbeddedNavigator.BackgroundImage = Nothing
        Me.VehicleUsesGridControl1.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("VehicleUsesGridControl1.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.VehicleUsesGridControl1.EmbeddedNavigator.ImeMode = CType(resources.GetObject("VehicleUsesGridControl1.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.VehicleUsesGridControl1.EmbeddedNavigator.TextLocation = CType(resources.GetObject("VehicleUsesGridControl1.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.VehicleUsesGridControl1.EmbeddedNavigator.ToolTip = resources.GetString("VehicleUsesGridControl1.EmbeddedNavigator.ToolTip")
        Me.VehicleUsesGridControl1.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("VehicleUsesGridControl1.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.VehicleUsesGridControl1.EmbeddedNavigator.ToolTipTitle = resources.GetString("VehicleUsesGridControl1.EmbeddedNavigator.ToolTipTitle")
        Me.VehicleUsesGridControl1.Font = Nothing
        Me.VehicleUsesGridControl1.MainView = Me.GridView3
        Me.VehicleUsesGridControl1.Name = "VehicleUsesGridControl1"
        Me.VehicleUsesGridControl1.UseEmbeddedNavigator = True
        Me.VehicleUsesGridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView3, Me.GridView4})
        '
        'GridView3
        '
        Me.GridView3.Appearance.HeaderPanel.Options.UseTextOptions = True
        Me.GridView3.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView3.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView3.Appearance.Row.Options.UseTextOptions = True
        Me.GridView3.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView3.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        resources.ApplyResources(Me.GridView3, "GridView3")
        Me.GridView3.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.GridColumn1, Me.GridColumn2, Me.colRegistrationMask})
        Me.GridView3.GridControl = Me.VehicleUsesGridControl1
        Me.GridView3.Name = "GridView3"
        Me.GridView3.OptionsNavigation.AutoFocusNewRow = True
        Me.GridView3.OptionsNavigation.EnterMoveNextColumn = True
        Me.GridView3.OptionsView.ShowAutoFilterRow = True
        '
        'GridColumn1
        '
        resources.ApplyResources(Me.GridColumn1, "GridColumn1")
        Me.GridColumn1.FieldName = "Id"
        Me.GridColumn1.Name = "GridColumn1"
        Me.GridColumn1.OptionsColumn.ReadOnly = True
        '
        'GridColumn2
        '
        resources.ApplyResources(Me.GridColumn2, "GridColumn2")
        Me.GridColumn2.FieldName = "UseDescription"
        Me.GridColumn2.Name = "GridColumn2"
        '
        'colRegistrationMask
        '
        resources.ApplyResources(Me.colRegistrationMask, "colRegistrationMask")
        Me.colRegistrationMask.FieldName = "RegistrationMask"
        Me.colRegistrationMask.Name = "colRegistrationMask"
        '
        'GridView4
        '
        resources.ApplyResources(Me.GridView4, "GridView4")
        Me.GridView4.GridControl = Me.VehicleUsesGridControl1
        Me.GridView4.Name = "GridView4"
        '
        'LayoutControl2
        '
        Me.LayoutControl2.AccessibleDescription = Nothing
        Me.LayoutControl2.AccessibleName = Nothing
        resources.ApplyResources(Me.LayoutControl2, "LayoutControl2")
        Me.LayoutControl2.BackgroundImage = Nothing
        Me.LayoutControl2.Controls.Add(Me.UxKopcinja2)
        Me.LayoutControl2.Controls.Add(Me.VehicleUsesGridControl1)
        Me.LayoutControl2.Font = Nothing
        Me.LayoutControl2.Name = "LayoutControl2"
        Me.LayoutControl2.Root = Me.LayoutControlGroup2
        '
        'UxKopcinja2
        '
        Me.UxKopcinja2.AccessibleDescription = Nothing
        Me.UxKopcinja2.AccessibleName = Nothing
        resources.ApplyResources(Me.UxKopcinja2, "UxKopcinja2")
        Me.UxKopcinja2.BackgroundImage = Nothing
        Me.UxKopcinja2.Name = "UxKopcinja2"
        '
        'LayoutControlGroup2
        '
        resources.ApplyResources(Me.LayoutControlGroup2, "LayoutControlGroup2")
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem3, Me.LayoutControlItem4, Me.SplitterItem2})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup2.Name = "Root"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(672, 450)
        Me.LayoutControlGroup2.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.VehicleUsesGridControl1
        resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 61)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(670, 387)
        Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.UxKopcinja2
        resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(670, 55)
        Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextToControlDistance = 0
        Me.LayoutControlItem4.TextVisible = False
        '
        'SplitterItem2
        '
        resources.ApplyResources(Me.SplitterItem2, "SplitterItem2")
        Me.SplitterItem2.Location = New System.Drawing.Point(0, 55)
        Me.SplitterItem2.Name = "SplitterItem2"
        Me.SplitterItem2.Size = New System.Drawing.Size(670, 6)
        '
        'uxVehicleUse
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.Controls.Add(Me.LayoutControl2)
        Me.Name = "uxVehicleUse"
        Me.Controls.SetChildIndex(Me.LayoutControl2, 0)
        CType(Me.VehicleUsesBindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleUsesGridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl2.ResumeLayout(False)
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents VehicleUsesGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents VehicleUsesBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colUseDescription As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents VehicleUsesBindingSource1 As System.Windows.Forms.BindingSource
  Friend WithEvents VehicleUsesGridControl1 As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView3 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents GridColumn1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents GridColumn2 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents GridView4 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents LayoutControl2 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents UxKopcinja2 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem2 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents colRegistrationMask As DevExpress.XtraGrid.Columns.GridColumn

End Class
