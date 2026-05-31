<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxDocumentTehnicalExamVehiclePartsStatus
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxDocumentTehnicalExamVehiclePartsStatus))
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl = New DevExpress.XtraGrid.GridControl
  Me.DocumentsTehnicalExamsReportsDetailsStatusesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colStatusName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
  Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.LayoutControl1.SuspendLayout()
  CType(Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DocumentsTehnicalExamsReportsDetailsStatusesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'LayoutControl1
  '
  Me.LayoutControl1.AccessibleDescription = Nothing
  Me.LayoutControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
  Me.LayoutControl1.BackgroundImage = Nothing
  Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
  Me.LayoutControl1.Controls.Add(Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl)
  Me.LayoutControl1.Font = Nothing
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
  '
  'UxKopcinja1
  '
  Me.UxKopcinja1.AccessibleDescription = Nothing
  Me.UxKopcinja1.AccessibleName = Nothing
  resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
  Me.UxKopcinja1.BackgroundImage = Nothing
  Me.UxKopcinja1.Name = "UxKopcinja1"
  '
  'DocumentsTehnicalExamsReportsDetailsStatusesGridControl
  '
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.AccessibleDescription = Nothing
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.AccessibleName = Nothing
  resources.ApplyResources(Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl, "DocumentsTehnicalExamsReportsDetailsStatusesGridControl")
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.BackgroundImage = Nothing
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.DataSource = Me.DocumentsTehnicalExamsReportsDetailsStatusesBindingSource
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.AccessibleName = Nothing
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.BackgroundImage = Nothing
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.Backgro" & _
          "undImageLayout"), System.Windows.Forms.ImageLayout)
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.ImeMode" & _
          ""), System.Windows.Forms.ImeMode)
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.TextLoc" & _
          "ation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.ToolTip = resources.GetString("DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.ToolTip" & _
          "")
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.ToolTip" & _
          "IconType"), DevExpress.Utils.ToolTipIconType)
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("DocumentsTehnicalExamsReportsDetailsStatusesGridControl.EmbeddedNavigator.ToolTip" & _
          "Title")
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.Font = Nothing
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.MainView = Me.GridView1
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.Name = "DocumentsTehnicalExamsReportsDetailsStatusesGridControl"
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.UseEmbeddedNavigator = True
  Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
  '
  'DocumentsTehnicalExamsReportsDetailsStatusesBindingSource
  '
  Me.DocumentsTehnicalExamsReportsDetailsStatusesBindingSource.DataSource = GetType(VTE.Library.DocumentsTehnicalExamsReportsDetailsStatus)
  '
  'GridView1
  '
  Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
  Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.Row.Options.UseTextOptions = True
  Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  resources.ApplyResources(Me.GridView1, "GridView1")
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colStatusName})
  Me.GridView1.GridControl = Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl
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
  'colStatusName
  '
  resources.ApplyResources(Me.colStatusName, "colStatusName")
  Me.colStatusName.FieldName = "StatusName"
  Me.colStatusName.Name = "colStatusName"
  '
  'GridView2
  '
  resources.ApplyResources(Me.GridView2, "GridView2")
  Me.GridView2.GridControl = Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl
  Me.GridView2.Name = "GridView2"
  '
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.SplitterItem1})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "Root"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(613, 426)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 64)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(609, 358)
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
  Me.LayoutControlItem2.Size = New System.Drawing.Size(609, 58)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem2.TextToControlDistance = 0
  Me.LayoutControlItem2.TextVisible = False
  '
  'SplitterItem1
  '
  resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
  Me.SplitterItem1.Location = New System.Drawing.Point(0, 58)
  Me.SplitterItem1.Name = "SplitterItem1"
  Me.SplitterItem1.Size = New System.Drawing.Size(609, 6)
  '
  'uxDocumentTehnicalExamVehiclePartsStatus
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.LayoutControl1)
  Me.Name = "uxDocumentTehnicalExamVehiclePartsStatus"
  Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.DocumentsTehnicalExamsReportsDetailsStatusesGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DocumentsTehnicalExamsReportsDetailsStatusesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents DocumentsTehnicalExamsReportsDetailsStatusesGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents DocumentsTehnicalExamsReportsDetailsStatusesBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colStatusName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem

End Class
