<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxRelationsTypes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxRelationsTypes))
        Me.DataLayoutControl1 = New DevExpress.XtraDataLayout.DataLayoutControl
        Me.CustomerVehicleRelationTypesGridControl = New DevExpress.XtraGrid.GridControl
        Me.CustomerVehicleRelationTypesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colRelationTypeName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsCustomerOnly = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsOwner = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsAuthorized = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colRelationDescription = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
        Me.SpinEdit1 = New DevExpress.XtraEditors.SpinEdit
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
        Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
        CType(Me.DataLayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DataLayoutControl1.SuspendLayout()
        CType(Me.CustomerVehicleRelationTypesGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CustomerVehicleRelationTypesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SpinEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataLayoutControl1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.DataLayoutControl1, False)
        Me.DataLayoutControl1.Controls.Add(Me.CustomerVehicleRelationTypesGridControl)
        Me.DataLayoutControl1.Controls.Add(Me.UxKopcinja1)
        Me.DataLayoutControl1.Controls.Add(Me.SpinEdit1)
        Me.DataLayoutControl1.DataSource = Me.CustomerVehicleRelationTypesBindingSource
        resources.ApplyResources(Me.DataLayoutControl1, "DataLayoutControl1")
        Me.DataLayoutControl1.HiddenItems.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.DataLayoutControl1.Name = "DataLayoutControl1"
        Me.DataLayoutControl1.Root = Me.LayoutControlGroup1
        '
        'CustomerVehicleRelationTypesGridControl
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.CustomerVehicleRelationTypesGridControl, False)
        Me.CustomerVehicleRelationTypesGridControl.DataSource = Me.CustomerVehicleRelationTypesBindingSource
        resources.ApplyResources(Me.CustomerVehicleRelationTypesGridControl, "CustomerVehicleRelationTypesGridControl")
        Me.CustomerVehicleRelationTypesGridControl.MainView = Me.GridView1
        Me.CustomerVehicleRelationTypesGridControl.Name = "CustomerVehicleRelationTypesGridControl"
        Me.CustomerVehicleRelationTypesGridControl.UseEmbeddedNavigator = True
        Me.CustomerVehicleRelationTypesGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
        '
        'CustomerVehicleRelationTypesBindingSource
        '
        Me.CustomerVehicleRelationTypesBindingSource.DataSource = GetType(VTE.Library.CustomerVehicleRelationTypes)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CustomerVehicleRelationTypesBindingSource, False)
        '
        'GridView1
        '
        Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
        Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView1.Appearance.Row.Options.UseTextOptions = True
        Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colRelationTypeName, Me.colIsCustomerOnly, Me.colIsOwner, Me.colIsAuthorized, Me.colRelationDescription})
        Me.GridView1.GridControl = Me.CustomerVehicleRelationTypesGridControl
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
        'colRelationTypeName
        '
        resources.ApplyResources(Me.colRelationTypeName, "colRelationTypeName")
        Me.colRelationTypeName.FieldName = "RelationTypeName"
        Me.colRelationTypeName.Name = "colRelationTypeName"
        '
        'colIsCustomerOnly
        '
        resources.ApplyResources(Me.colIsCustomerOnly, "colIsCustomerOnly")
        Me.colIsCustomerOnly.FieldName = "IsCustomerOnly"
        Me.colIsCustomerOnly.Name = "colIsCustomerOnly"
        '
        'colIsOwner
        '
        resources.ApplyResources(Me.colIsOwner, "colIsOwner")
        Me.colIsOwner.FieldName = "IsOwner"
        Me.colIsOwner.Name = "colIsOwner"
        '
        'colIsAuthorized
        '
        resources.ApplyResources(Me.colIsAuthorized, "colIsAuthorized")
        Me.colIsAuthorized.FieldName = "IsAuthorized"
        Me.colIsAuthorized.Name = "colIsAuthorized"
        '
        'colRelationDescription
        '
        resources.ApplyResources(Me.colRelationDescription, "colRelationDescription")
        Me.colRelationDescription.FieldName = "RelationDescription"
        Me.colRelationDescription.Name = "colRelationDescription"
        '
        'GridView2
        '
        Me.GridView2.GridControl = Me.CustomerVehicleRelationTypesGridControl
        Me.GridView2.Name = "GridView2"
        '
        'UxKopcinja1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UxKopcinja1, False)
        resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
        Me.UxKopcinja1.Name = "UxKopcinja1"
        '
        'SpinEdit1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SpinEdit1, False)
        Me.SpinEdit1.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.CustomerVehicleRelationTypesBindingSource, "Id", True))
        resources.ApplyResources(Me.SpinEdit1, "SpinEdit1")
        Me.SpinEdit1.Name = "SpinEdit1"
        Me.SpinEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.SpinEdit1.StyleController = Me.DataLayoutControl1
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.SpinEdit1
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "ItemForId"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(50, 20)
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem7, Me.SplitterItem1, Me.LayoutControlItem2})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(613, 426)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.UxKopcinja1
        resources.ApplyResources(Me.LayoutControlItem7, "LayoutControlItem7")
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem7.MinSize = New System.Drawing.Size(212, 31)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(611, 59)
        Me.LayoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem7.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem7.TextToControlDistance = 0
        Me.LayoutControlItem7.TextVisible = False
        '
        'SplitterItem1
        '
        resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
        Me.SplitterItem1.Location = New System.Drawing.Point(0, 59)
        Me.SplitterItem1.Name = "SplitterItem1"
        Me.SplitterItem1.Size = New System.Drawing.Size(611, 6)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.CustomerVehicleRelationTypesGridControl
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 65)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(611, 359)
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        '
        'uxRelationsTypes
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.DataLayoutControl1)
        Me.Name = "uxRelationsTypes"
        Me.Controls.SetChildIndex(Me.DataLayoutControl1, 0)
        CType(Me.DataLayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DataLayoutControl1.ResumeLayout(False)
        CType(Me.CustomerVehicleRelationTypesGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CustomerVehicleRelationTypesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SpinEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
  Friend WithEvents DataLayoutControl1 As DevExpress.XtraDataLayout.DataLayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents SpinEdit1 As DevExpress.XtraEditors.SpinEdit
  Friend WithEvents CustomerVehicleRelationTypesBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents CustomerVehicleRelationTypesGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colRelationTypeName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIsCustomerOnly As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIsOwner As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIsAuthorized As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colRelationDescription As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider

End Class
