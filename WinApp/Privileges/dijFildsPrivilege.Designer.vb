<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijFildsPrivilege
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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.FieldsPrivilegesGridControl = New DevExpress.XtraGrid.GridControl
        Me.FieldsPrivilegesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdRole = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemLookUpEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.RoolListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.colIdCSLAObject = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemLookUpEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.CSLAObjectsListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.colcSLAObjectPropertyName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
        Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
        Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.FieldsPrivilegesGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FieldsPrivilegesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemLookUpEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RoolListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CSLAObjectsListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.LayoutControl1.Controls.Add(Me.FieldsPrivilegesGridControl)
        Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(744, 448)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'FieldsPrivilegesGridControl
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.FieldsPrivilegesGridControl, False)
        Me.FieldsPrivilegesGridControl.DataSource = Me.FieldsPrivilegesBindingSource
        Me.FieldsPrivilegesGridControl.Location = New System.Drawing.Point(8, 98)
        Me.FieldsPrivilegesGridControl.MainView = Me.GridView1
        Me.FieldsPrivilegesGridControl.Name = "FieldsPrivilegesGridControl"
        Me.FieldsPrivilegesGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemLookUpEdit1, Me.RepositoryItemLookUpEdit2})
        Me.FieldsPrivilegesGridControl.Size = New System.Drawing.Size(729, 343)
        Me.FieldsPrivilegesGridControl.TabIndex = 5
        Me.FieldsPrivilegesGridControl.UseEmbeddedNavigator = True
        Me.FieldsPrivilegesGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'FieldsPrivilegesBindingSource
        '
        Me.FieldsPrivilegesBindingSource.DataSource = GetType(VTE.Library.FieldsPrivilege)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.FieldsPrivilegesBindingSource, False)
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdRole, Me.colIdCSLAObject, Me.colcSLAObjectPropertyName})
        Me.GridView1.GridControl = Me.FieldsPrivilegesGridControl
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
        '
        'colId
        '
        Me.colId.Caption = "Id"
        Me.colId.FieldName = "Id"
        Me.colId.Name = "colId"
        Me.colId.OptionsColumn.ReadOnly = True
        '
        'colIdRole
        '
        Me.colIdRole.Caption = "IdRole"
        Me.colIdRole.ColumnEdit = Me.RepositoryItemLookUpEdit2
        Me.colIdRole.FieldName = "IdRole"
        Me.colIdRole.Name = "colIdRole"
        Me.colIdRole.Visible = True
        Me.colIdRole.VisibleIndex = 0
        '
        'RepositoryItemLookUpEdit2
        '
        Me.RepositoryItemLookUpEdit2.AutoHeight = False
        Me.RepositoryItemLookUpEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
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
        Me.colIdCSLAObject.Caption = "IdCSLAObject"
        Me.colIdCSLAObject.ColumnEdit = Me.RepositoryItemLookUpEdit1
        Me.colIdCSLAObject.FieldName = "IdCSLAObject"
        Me.colIdCSLAObject.Name = "colIdCSLAObject"
        Me.colIdCSLAObject.Visible = True
        Me.colIdCSLAObject.VisibleIndex = 1
        '
        'RepositoryItemLookUpEdit1
        '
        Me.RepositoryItemLookUpEdit1.AutoHeight = False
        Me.RepositoryItemLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemLookUpEdit1.DataSource = Me.CSLAObjectsListBindingSource
        Me.RepositoryItemLookUpEdit1.DisplayMember = "CSLAObjectName"
        Me.RepositoryItemLookUpEdit1.Name = "RepositoryItemLookUpEdit1"
        Me.RepositoryItemLookUpEdit1.ValueMember = "Id"
        '
        'CSLAObjectsListBindingSource
        '
        Me.CSLAObjectsListBindingSource.DataSource = GetType(VTE.Library.CSLAObjectsList)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CSLAObjectsListBindingSource, False)
        '
        'colcSLAObjectPropertyName
        '
        Me.colcSLAObjectPropertyName.Caption = "cSLAObjectPropertyName"
        Me.colcSLAObjectPropertyName.FieldName = "cSLAObjectPropertyName"
        Me.colcSLAObjectPropertyName.Name = "colcSLAObjectPropertyName"
        Me.colcSLAObjectPropertyName.Visible = True
        Me.colcSLAObjectPropertyName.VisibleIndex = 2
        '
        'UxKopcinja1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UxKopcinja1, False)
        Me.UxKopcinja1.Location = New System.Drawing.Point(8, 8)
        Me.UxKopcinja1.Name = "UxKopcinja1"
        Me.UxKopcinja1.Size = New System.Drawing.Size(729, 73)
        Me.UxKopcinja1.TabIndex = 4
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.SplitterItem1})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(744, 448)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.Text = "Root"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.UxKopcinja1
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(740, 84)
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.FieldsPrivilegesGridControl
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 90)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(740, 354)
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'SplitterItem1
        '
        Me.SplitterItem1.CustomizationFormText = "SplitterItem1"
        Me.SplitterItem1.Location = New System.Drawing.Point(0, 84)
        Me.SplitterItem1.Name = "SplitterItem1"
        Me.SplitterItem1.Size = New System.Drawing.Size(740, 6)
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        Me.DxErrorProvider1.DataSource = Me.FieldsPrivilegesBindingSource
        '
        'dijFildsPrivilege
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(744, 448)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "dijFildsPrivilege"
        Me.Text = "dijFildsPrivilege"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.FieldsPrivilegesGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FieldsPrivilegesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemLookUpEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RoolListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CSLAObjectsListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents FieldsPrivilegesGridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents FieldsPrivilegesBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdRole As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemLookUpEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents RoolListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents colIdCSLAObject As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemLookUpEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents CSLAObjectsListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents colcSLAObjectPropertyName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
    Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
    Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
    Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
End Class
