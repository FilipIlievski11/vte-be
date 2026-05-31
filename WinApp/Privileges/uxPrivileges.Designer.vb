<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxPrivileges
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxPrivileges))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
        Me.ObjectPrivilegesGridControl = New DevExpress.XtraGrid.GridControl
        Me.ObjectPrivilegesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdRole = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemLookUpEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.RoolListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.colIdCSLAObject = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemLookUpEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.CSLAObjectsListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.colCanAddObject = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCanGetObject = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCanDeleteObject = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCanEditObject = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
        Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.ObjectPrivilegesGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ObjectPrivilegesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemLookUpEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RoolListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CSLAObjectsListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.LayoutControl1.Controls.Add(Me.ObjectPrivilegesGridControl)
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
        'ObjectPrivilegesGridControl
        '
        Me.ObjectPrivilegesGridControl.AccessibleDescription = Nothing
        Me.ObjectPrivilegesGridControl.AccessibleName = Nothing
        resources.ApplyResources(Me.ObjectPrivilegesGridControl, "ObjectPrivilegesGridControl")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.ObjectPrivilegesGridControl, False)
        Me.ObjectPrivilegesGridControl.BackgroundImage = Nothing
        Me.ObjectPrivilegesGridControl.DataSource = Me.ObjectPrivilegesBindingSource
        Me.ObjectPrivilegesGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.ObjectPrivilegesGridControl.EmbeddedNavigator.AccessibleName = Nothing
        Me.ObjectPrivilegesGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("ObjectPrivilegesGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.ObjectPrivilegesGridControl.EmbeddedNavigator.BackgroundImage = Nothing
        Me.ObjectPrivilegesGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("ObjectPrivilegesGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.ObjectPrivilegesGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("ObjectPrivilegesGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.ObjectPrivilegesGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("ObjectPrivilegesGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.ObjectPrivilegesGridControl.EmbeddedNavigator.ToolTip = resources.GetString("ObjectPrivilegesGridControl.EmbeddedNavigator.ToolTip")
        Me.ObjectPrivilegesGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("ObjectPrivilegesGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.ObjectPrivilegesGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("ObjectPrivilegesGridControl.EmbeddedNavigator.ToolTipTitle")
        Me.ObjectPrivilegesGridControl.Font = Nothing
        Me.ObjectPrivilegesGridControl.MainView = Me.GridView1
        Me.ObjectPrivilegesGridControl.Name = "ObjectPrivilegesGridControl"
        Me.ObjectPrivilegesGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemLookUpEdit1, Me.RepositoryItemLookUpEdit2})
        Me.ObjectPrivilegesGridControl.UseEmbeddedNavigator = True
        Me.ObjectPrivilegesGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
        '
        'ObjectPrivilegesBindingSource
        '
        Me.ObjectPrivilegesBindingSource.DataSource = GetType(VTE.Library.ObjectPrivilege)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.ObjectPrivilegesBindingSource, False)
        '
        'GridView1
        '
        resources.ApplyResources(Me.GridView1, "GridView1")
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdRole, Me.colIdCSLAObject, Me.colCanAddObject, Me.colCanGetObject, Me.colCanDeleteObject, Me.colCanEditObject})
        Me.GridView1.GridControl = Me.ObjectPrivilegesGridControl
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
        Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colIdCSLAObject, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colId
        '
        resources.ApplyResources(Me.colId, "colId")
        Me.colId.FieldName = "Id"
        Me.colId.Name = "colId"
        Me.colId.OptionsColumn.ReadOnly = True
        '
        'colIdRole
        '
        resources.ApplyResources(Me.colIdRole, "colIdRole")
        Me.colIdRole.ColumnEdit = Me.RepositoryItemLookUpEdit2
        Me.colIdRole.FieldName = "IdRole"
        Me.colIdRole.Name = "colIdRole"
        '
        'RepositoryItemLookUpEdit2
        '
        Me.RepositoryItemLookUpEdit2.AccessibleDescription = Nothing
        Me.RepositoryItemLookUpEdit2.AccessibleName = Nothing
        resources.ApplyResources(Me.RepositoryItemLookUpEdit2, "RepositoryItemLookUpEdit2")
        Me.RepositoryItemLookUpEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit2.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.RepositoryItemLookUpEdit2.DataSource = Me.RoolListBindingSource
        Me.RepositoryItemLookUpEdit2.DisplayMember = "RoleName"
        Me.RepositoryItemLookUpEdit2.Name = "RepositoryItemLookUpEdit2"
        Me.RepositoryItemLookUpEdit2.ValueMember = "Id"
        '
        'RoolListBindingSource
        '
        Me.RoolListBindingSource.DataSource = GetType(VTE.Library.RoolList)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.RoolListBindingSource, False)
        '
        'colIdCSLAObject
        '
        resources.ApplyResources(Me.colIdCSLAObject, "colIdCSLAObject")
        Me.colIdCSLAObject.ColumnEdit = Me.RepositoryItemLookUpEdit1
        Me.colIdCSLAObject.FieldName = "IdCSLAObject"
        Me.colIdCSLAObject.Name = "colIdCSLAObject"
        '
        'RepositoryItemLookUpEdit1
        '
        Me.RepositoryItemLookUpEdit1.AccessibleDescription = Nothing
        Me.RepositoryItemLookUpEdit1.AccessibleName = Nothing
        resources.ApplyResources(Me.RepositoryItemLookUpEdit1, "RepositoryItemLookUpEdit1")
        Me.RepositoryItemLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.RepositoryItemLookUpEdit1.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, "", False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdCSLAObject", "IdCSLAObject", 73, DevExpress.Utils.FormatType.Numeric, "", False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CSLAObjectName", "CSLAObjectName", 90, DevExpress.Utils.FormatType.None, "", True, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.Ascending), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CSLAObjectType", "CSLAObjectType", 87, DevExpress.Utils.FormatType.None, "", False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
        Me.RepositoryItemLookUpEdit1.DataSource = Me.CSLAObjectsListBindingSource
        Me.RepositoryItemLookUpEdit1.DisplayMember = "CSLAObjectName"
        Me.RepositoryItemLookUpEdit1.Name = "RepositoryItemLookUpEdit1"
        Me.RepositoryItemLookUpEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.RepositoryItemLookUpEdit1.ValueMember = "Id"
        '
        'CSLAObjectsListBindingSource
        '
        Me.CSLAObjectsListBindingSource.DataSource = GetType(VTE.Library.CSLAObjectsList)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CSLAObjectsListBindingSource, False)
        '
        'colCanAddObject
        '
        resources.ApplyResources(Me.colCanAddObject, "colCanAddObject")
        Me.colCanAddObject.FieldName = "CanAddObject"
        Me.colCanAddObject.Name = "colCanAddObject"
        '
        'colCanGetObject
        '
        resources.ApplyResources(Me.colCanGetObject, "colCanGetObject")
        Me.colCanGetObject.FieldName = "CanGetObject"
        Me.colCanGetObject.Name = "colCanGetObject"
        '
        'colCanDeleteObject
        '
        resources.ApplyResources(Me.colCanDeleteObject, "colCanDeleteObject")
        Me.colCanDeleteObject.FieldName = "CanDeleteObject"
        Me.colCanDeleteObject.Name = "colCanDeleteObject"
        '
        'colCanEditObject
        '
        resources.ApplyResources(Me.colCanEditObject, "colCanEditObject")
        Me.colCanEditObject.FieldName = "CanEditObject"
        Me.colCanEditObject.Name = "colCanEditObject"
        '
        'GridView2
        '
        resources.ApplyResources(Me.GridView2, "GridView2")
        Me.GridView2.GridControl = Me.ObjectPrivilegesGridControl
        Me.GridView2.Name = "GridView2"
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.SplitterItem1})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(634, 354)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.ObjectPrivilegesGridControl
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 67)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(630, 283)
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
        Me.LayoutControlItem2.Size = New System.Drawing.Size(630, 61)
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'SplitterItem1
        '
        resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
        Me.SplitterItem1.Location = New System.Drawing.Point(0, 61)
        Me.SplitterItem1.Name = "SplitterItem1"
        Me.SplitterItem1.Size = New System.Drawing.Size(630, 6)
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        Me.DxErrorProvider1.DataSource = Me.ObjectPrivilegesBindingSource
        '
        'uxPrivileges
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.LayoutControl1)
        Me.Icon = Nothing
        Me.Name = "uxPrivileges"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.ObjectPrivilegesGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ObjectPrivilegesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemLookUpEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RoolListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CSLAObjectsListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
    Friend WithEvents ObjectPrivilegesGridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents ObjectPrivilegesBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdRole As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemLookUpEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents colIdCSLAObject As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemLookUpEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents CSLAObjectsListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents colCanAddObject As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCanGetObject As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCanDeleteObject As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCanEditObject As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
    Friend WithEvents RoolListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
    Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
    Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
End Class
