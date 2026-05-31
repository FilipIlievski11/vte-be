<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxBrakes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxBrakes))
        Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl
        Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
        Me.VehicleBrakesGridControl = New DevExpress.XtraGrid.GridControl
        Me.VehicleBrakesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colBreakesCode = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colBreakesDescription = New DevExpress.XtraGrid.Columns.GridColumn
        Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
        Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerControl1.SuspendLayout()
        CType(Me.VehicleBrakesGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleBrakesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainerControl1
        '
        Me.SplitContainerControl1.AccessibleDescription = Nothing
        Me.SplitContainerControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.SplitContainerControl1, "SplitContainerControl1")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SplitContainerControl1, False)
        Me.SplitContainerControl1.Horizontal = False
        Me.SplitContainerControl1.Name = "SplitContainerControl1"
        Me.SplitContainerControl1.Panel1.Controls.Add(Me.UxKopcinja1)
        resources.ApplyResources(Me.SplitContainerControl1.Panel1, "SplitContainerControl1.Panel1")
        Me.SplitContainerControl1.Panel2.Controls.Add(Me.VehicleBrakesGridControl)
        resources.ApplyResources(Me.SplitContainerControl1.Panel2, "SplitContainerControl1.Panel2")
        Me.SplitContainerControl1.SplitterPosition = 64
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
        'VehicleBrakesGridControl
        '
        Me.VehicleBrakesGridControl.AccessibleDescription = Nothing
        Me.VehicleBrakesGridControl.AccessibleName = Nothing
        resources.ApplyResources(Me.VehicleBrakesGridControl, "VehicleBrakesGridControl")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.VehicleBrakesGridControl, False)
        Me.VehicleBrakesGridControl.BackgroundImage = Nothing
        Me.VehicleBrakesGridControl.DataSource = Me.VehicleBrakesBindingSource
        Me.VehicleBrakesGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.VehicleBrakesGridControl.EmbeddedNavigator.AccessibleName = Nothing
        Me.VehicleBrakesGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("VehicleBrakesGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.VehicleBrakesGridControl.EmbeddedNavigator.BackgroundImage = Nothing
        Me.VehicleBrakesGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("VehicleBrakesGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.VehicleBrakesGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("VehicleBrakesGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.VehicleBrakesGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("VehicleBrakesGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.VehicleBrakesGridControl.EmbeddedNavigator.ToolTip = resources.GetString("VehicleBrakesGridControl.EmbeddedNavigator.ToolTip")
        Me.VehicleBrakesGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("VehicleBrakesGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.VehicleBrakesGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("VehicleBrakesGridControl.EmbeddedNavigator.ToolTipTitle")
        Me.VehicleBrakesGridControl.Font = Nothing
        Me.VehicleBrakesGridControl.MainView = Me.GridView1
        Me.VehicleBrakesGridControl.Name = "VehicleBrakesGridControl"
        Me.VehicleBrakesGridControl.UseEmbeddedNavigator = True
        Me.VehicleBrakesGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'VehicleBrakesBindingSource
        '
        Me.VehicleBrakesBindingSource.DataSource = GetType(VTE.Library.VehicleBrake)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.VehicleBrakesBindingSource, False)
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
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colBreakesCode, Me.colBreakesDescription})
        Me.GridView1.GridControl = Me.VehicleBrakesGridControl
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
        'colBreakesCode
        '
        resources.ApplyResources(Me.colBreakesCode, "colBreakesCode")
        Me.colBreakesCode.FieldName = "BreakesCode"
        Me.colBreakesCode.Name = "colBreakesCode"
        '
        'colBreakesDescription
        '
        resources.ApplyResources(Me.colBreakesDescription, "colBreakesDescription")
        Me.colBreakesDescription.FieldName = "BreakesDescription"
        Me.colBreakesDescription.Name = "colBreakesDescription"
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        Me.DxErrorProvider1.DataSource = Me.VehicleBrakesBindingSource
        '
        'uxBrakes
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, True)
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.Controls.Add(Me.SplitContainerControl1)
        Me.Name = "uxBrakes"
        Me.Controls.SetChildIndex(Me.SplitContainerControl1, 0)
        CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerControl1.ResumeLayout(False)
        CType(Me.VehicleBrakesGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleBrakesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
  Friend WithEvents VehicleBrakesBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents VehicleBrakesGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colBreakesCode As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colBreakesDescription As DevExpress.XtraGrid.Columns.GridColumn

End Class
