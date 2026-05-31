<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxStreets
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxStreets))
  Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl
  Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
  Me.StreetsGridControl = New DevExpress.XtraGrid.GridControl
  Me.StreetsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colStreetName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colNote = New DevExpress.XtraGrid.Columns.GridColumn
  Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
  Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
  Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
  CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SplitContainerControl1.SuspendLayout()
  CType(Me.StreetsGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.StreetsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
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
  Me.SplitContainerControl1.Panel2.Controls.Add(Me.StreetsGridControl)
  resources.ApplyResources(Me.SplitContainerControl1.Panel2, "SplitContainerControl1.Panel2")
  Me.SplitContainerControl1.SplitterPosition = 63
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
  'StreetsGridControl
  '
  Me.StreetsGridControl.AccessibleDescription = Nothing
  Me.StreetsGridControl.AccessibleName = Nothing
  resources.ApplyResources(Me.StreetsGridControl, "StreetsGridControl")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.StreetsGridControl, False)
  Me.StreetsGridControl.BackgroundImage = Nothing
  Me.StreetsGridControl.DataSource = Me.StreetsBindingSource
  Me.StreetsGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
  Me.StreetsGridControl.EmbeddedNavigator.AccessibleName = Nothing
  Me.StreetsGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("StreetsGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
  Me.StreetsGridControl.EmbeddedNavigator.BackgroundImage = Nothing
  Me.StreetsGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("StreetsGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
  Me.StreetsGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("StreetsGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
  Me.StreetsGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("StreetsGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
  Me.StreetsGridControl.EmbeddedNavigator.ToolTip = resources.GetString("StreetsGridControl.EmbeddedNavigator.ToolTip")
  Me.StreetsGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("StreetsGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
  Me.StreetsGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("StreetsGridControl.EmbeddedNavigator.ToolTipTitle")
  Me.StreetsGridControl.Font = Nothing
  Me.StreetsGridControl.MainView = Me.GridView1
  Me.StreetsGridControl.Name = "StreetsGridControl"
  Me.StreetsGridControl.UseEmbeddedNavigator = True
  Me.StreetsGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'StreetsBindingSource
  '
  Me.StreetsBindingSource.DataSource = GetType(VTE.Library.Street)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.StreetsBindingSource, False)
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
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colStreetName, Me.colNote})
  Me.GridView1.GridControl = Me.StreetsGridControl
  Me.GridView1.Name = "GridView1"
  Me.GridView1.OptionsNavigation.AutoFocusNewRow = True
  Me.GridView1.OptionsNavigation.EnterMoveNextColumn = True
  Me.GridView1.OptionsView.ShowAutoFilterRow = True
  Me.GridView1.OptionsView.ShowGroupPanel = False
  '
  'colId
  '
  resources.ApplyResources(Me.colId, "colId")
  Me.colId.FieldName = "Id"
  Me.colId.Name = "colId"
  Me.colId.OptionsColumn.ReadOnly = True
  '
  'colStreetName
  '
  resources.ApplyResources(Me.colStreetName, "colStreetName")
  Me.colStreetName.FieldName = "StreetName"
  Me.colStreetName.Name = "colStreetName"
  '
  'colNote
  '
  resources.ApplyResources(Me.colNote, "colNote")
  Me.colNote.FieldName = "Note"
  Me.colNote.Name = "colNote"
  '
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.StreetsBindingSource
  '
  'uxStreets
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.SplitContainerControl1)
  Me.Name = "uxStreets"
  Me.Controls.SetChildIndex(Me.SplitContainerControl1, 0)
  CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.SplitContainerControl1.ResumeLayout(False)
  CType(Me.StreetsGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.StreetsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents StreetsBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents StreetsGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colStreetName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colNote As DevExpress.XtraGrid.Columns.GridColumn

End Class
