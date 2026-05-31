<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxDriveingLicenceCtegories
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxDriveingLicenceCtegories))
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
  Me.DriveingLicenceCtegoriesGridControl = New DevExpress.XtraGrid.GridControl
  Me.DriveingLicenceCtegoriesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCode = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colDescription = New DevExpress.XtraGrid.Columns.GridColumn
  Me.RepositoryItemTextEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
  Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
  Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
  Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
  Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
  Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.LayoutControl1.SuspendLayout()
  CType(Me.DriveingLicenceCtegoriesGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DriveingLicenceCtegoriesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'LayoutControl1
  '
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LayoutControl1, False)
  Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
  Me.LayoutControl1.Controls.Add(Me.DriveingLicenceCtegoriesGridControl)
  resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
  '
  'UxKopcinja1
  '
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UxKopcinja1, False)
  resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
  Me.UxKopcinja1.Name = "UxKopcinja1"
  '
  'DriveingLicenceCtegoriesGridControl
  '
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.DriveingLicenceCtegoriesGridControl, False)
  Me.DriveingLicenceCtegoriesGridControl.DataSource = Me.DriveingLicenceCtegoriesBindingSource
  resources.ApplyResources(Me.DriveingLicenceCtegoriesGridControl, "DriveingLicenceCtegoriesGridControl")
  Me.DriveingLicenceCtegoriesGridControl.MainView = Me.GridView1
  Me.DriveingLicenceCtegoriesGridControl.Name = "DriveingLicenceCtegoriesGridControl"
  Me.DriveingLicenceCtegoriesGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemTextEdit1})
  Me.DriveingLicenceCtegoriesGridControl.UseEmbeddedNavigator = True
  Me.DriveingLicenceCtegoriesGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'DriveingLicenceCtegoriesBindingSource
  '
  Me.DriveingLicenceCtegoriesBindingSource.DataSource = GetType(VTE.Library.DriveingLicenceCtegory)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.DriveingLicenceCtegoriesBindingSource, False)
  '
  'GridView1
  '
  Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
  Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  Me.GridView1.Appearance.Row.Options.UseTextOptions = True
  Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colCode, Me.colDescription})
  Me.GridView1.GridControl = Me.DriveingLicenceCtegoriesGridControl
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
  'RepositoryItemTextEdit1
  '
  resources.ApplyResources(Me.RepositoryItemTextEdit1, "RepositoryItemTextEdit1")
  Me.RepositoryItemTextEdit1.Mask.AutoComplete = CType(resources.GetObject("RepositoryItemTextEdit1.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.RepositoryItemTextEdit1.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("RepositoryItemTextEdit1.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.RepositoryItemTextEdit1.Name = "RepositoryItemTextEdit1"
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
  Me.LayoutControlItem1.Control = Me.DriveingLicenceCtegoriesGridControl
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 60)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(609, 362)
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
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.DriveingLicenceCtegoriesBindingSource
  '
  'uxDriveingLicenceCtegories
  '
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
  resources.ApplyResources(Me, "$this")
  Me.Controls.Add(Me.LayoutControl1)
  Me.Name = "uxDriveingLicenceCtegories"
  Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.DriveingLicenceCtegoriesGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DriveingLicenceCtegoriesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents DriveingLicenceCtegoriesGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents DriveingLicenceCtegoriesBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCode As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDescription As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents RepositoryItemTextEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit

End Class
