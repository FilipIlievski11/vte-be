<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxBusinessTypes
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxBusinessTypes))
  Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl
  Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
  Me.BusinessTypesGridControl = New DevExpress.XtraGrid.GridControl
  Me.BusinessTypesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colBusinessType = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colBusinessTypeDescription = New DevExpress.XtraGrid.Columns.GridColumn
  Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
  Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
  Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
  CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SplitContainerControl1.SuspendLayout()
  CType(Me.BusinessTypesGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.BusinessTypesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'SplitContainerControl1
  '
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SplitContainerControl1, True)
  resources.ApplyResources(Me.SplitContainerControl1, "SplitContainerControl1")
  Me.SplitContainerControl1.Horizontal = False
  Me.SplitContainerControl1.Name = "SplitContainerControl1"
  Me.SplitContainerControl1.Panel1.Controls.Add(Me.UxKopcinja1)
  resources.ApplyResources(Me.SplitContainerControl1.Panel1, "SplitContainerControl1.Panel1")
  Me.SplitContainerControl1.Panel2.Controls.Add(Me.BusinessTypesGridControl)
  resources.ApplyResources(Me.SplitContainerControl1.Panel2, "SplitContainerControl1.Panel2")
  Me.SplitContainerControl1.SplitterPosition = 71
  '
  'UxKopcinja1
  '
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UxKopcinja1, False)
  resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
  Me.UxKopcinja1.Name = "UxKopcinja1"
  '
  'BusinessTypesGridControl
  '
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.BusinessTypesGridControl, True)
  Me.BusinessTypesGridControl.DataSource = Me.BusinessTypesBindingSource
  resources.ApplyResources(Me.BusinessTypesGridControl, "BusinessTypesGridControl")
  Me.BusinessTypesGridControl.MainView = Me.GridView1
  Me.BusinessTypesGridControl.Name = "BusinessTypesGridControl"
  Me.BusinessTypesGridControl.UseEmbeddedNavigator = True
  Me.BusinessTypesGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
  '
  'BusinessTypesBindingSource
  '
  Me.BusinessTypesBindingSource.DataSource = GetType(VTE.Library.BusinessType)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.BusinessTypesBindingSource, False)
  '
  'GridView1
  '
  Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
  Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  Me.GridView1.Appearance.Row.Options.UseTextOptions = True
  Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colBusinessType, Me.colBusinessTypeDescription})
  Me.GridView1.GridControl = Me.BusinessTypesGridControl
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
  'colBusinessType
  '
  resources.ApplyResources(Me.colBusinessType, "colBusinessType")
  Me.colBusinessType.FieldName = "BusinessType"
  Me.colBusinessType.Name = "colBusinessType"
  '
  'colBusinessTypeDescription
  '
  resources.ApplyResources(Me.colBusinessTypeDescription, "colBusinessTypeDescription")
  Me.colBusinessTypeDescription.FieldName = "BusinessTypeDescription"
  Me.colBusinessTypeDescription.Name = "colBusinessTypeDescription"
  '
  'GridView2
  '
  Me.GridView2.GridControl = Me.BusinessTypesGridControl
  Me.GridView2.Name = "GridView2"
  '
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.BusinessTypesBindingSource
  '
  'uxBusinessTypes
  '
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, True)
  resources.ApplyResources(Me, "$this")
  Me.Controls.Add(Me.SplitContainerControl1)
  Me.Name = "uxBusinessTypes"
  Me.Controls.SetChildIndex(Me.SplitContainerControl1, 0)
  CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.SplitContainerControl1.ResumeLayout(False)
  CType(Me.BusinessTypesGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.BusinessTypesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents BusinessTypesGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents BusinessTypesBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colBusinessType As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colBusinessTypeDescription As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider

End Class
