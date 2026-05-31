<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxVehicleGearBoxes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxVehicleGearBoxes))
        Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl
        Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
        Me.VehicleGearBoxesGridControl = New DevExpress.XtraGrid.GridControl
        Me.VehicleGearBoxesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colGearBoxCode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colGearBoxDescription = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
        Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerControl1.SuspendLayout()
        CType(Me.VehicleGearBoxesGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleGearBoxesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainerControl1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SplitContainerControl1, False)
        resources.ApplyResources(Me.SplitContainerControl1, "SplitContainerControl1")
        Me.SplitContainerControl1.Horizontal = False
        Me.SplitContainerControl1.Name = "SplitContainerControl1"
        Me.SplitContainerControl1.Panel1.Controls.Add(Me.UxKopcinja1)
        resources.ApplyResources(Me.SplitContainerControl1.Panel1, "SplitContainerControl1.Panel1")
        Me.SplitContainerControl1.Panel2.Controls.Add(Me.VehicleGearBoxesGridControl)
        resources.ApplyResources(Me.SplitContainerControl1.Panel2, "SplitContainerControl1.Panel2")
        Me.SplitContainerControl1.SplitterPosition = 64
        '
        'UxKopcinja1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UxKopcinja1, False)
        resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
        Me.UxKopcinja1.Name = "UxKopcinja1"
        '
        'VehicleGearBoxesGridControl
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.VehicleGearBoxesGridControl, True)
        Me.VehicleGearBoxesGridControl.DataSource = Me.VehicleGearBoxesBindingSource
        resources.ApplyResources(Me.VehicleGearBoxesGridControl, "VehicleGearBoxesGridControl")
        Me.VehicleGearBoxesGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("VehicleGearBoxesGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.VehicleGearBoxesGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("VehicleGearBoxesGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.VehicleGearBoxesGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("VehicleGearBoxesGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.VehicleGearBoxesGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("VehicleGearBoxesGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.VehicleGearBoxesGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("VehicleGearBoxesGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.VehicleGearBoxesGridControl.MainView = Me.GridView1
        Me.VehicleGearBoxesGridControl.Name = "VehicleGearBoxesGridControl"
        Me.VehicleGearBoxesGridControl.UseEmbeddedNavigator = True
        Me.VehicleGearBoxesGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
        '
        'VehicleGearBoxesBindingSource
        '
        Me.VehicleGearBoxesBindingSource.DataSource = GetType(VTE.Library.VehicleGearBox)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.VehicleGearBoxesBindingSource, False)
        '
        'GridView1
        '
        Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
        Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView1.Appearance.Row.Options.UseTextOptions = True
        Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colGearBoxCode, Me.colGearBoxDescription})
        Me.GridView1.GridControl = Me.VehicleGearBoxesGridControl
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
        'colGearBoxCode
        '
        resources.ApplyResources(Me.colGearBoxCode, "colGearBoxCode")
        Me.colGearBoxCode.FieldName = "GearBoxCode"
        Me.colGearBoxCode.Name = "colGearBoxCode"
        '
        'colGearBoxDescription
        '
        resources.ApplyResources(Me.colGearBoxDescription, "colGearBoxDescription")
        Me.colGearBoxDescription.FieldName = "GearBoxDescription"
        Me.colGearBoxDescription.Name = "colGearBoxDescription"
        '
        'GridView2
        '
        Me.GridView2.GridControl = Me.VehicleGearBoxesGridControl
        Me.GridView2.Name = "GridView2"
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        Me.DxErrorProvider1.DataSource = Me.VehicleGearBoxesBindingSource
        '
        'uxVehicleGearBoxes
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, True)
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.SplitContainerControl1)
        Me.Name = "uxVehicleGearBoxes"
        Me.Controls.SetChildIndex(Me.SplitContainerControl1, 0)
        CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerControl1.ResumeLayout(False)
        CType(Me.VehicleGearBoxesGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleGearBoxesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
  Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents VehicleGearBoxesGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents VehicleGearBoxesBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colGearBoxCode As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colGearBoxDescription As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider

End Class
