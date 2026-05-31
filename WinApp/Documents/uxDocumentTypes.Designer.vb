<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxDocumentTypes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxDocumentTypes))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.DocumentTypesGridControl = New DevExpress.XtraGrid.GridControl
        Me.DocumentTypesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDocumentTypeName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsBidirectional = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsVehiceRequired = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsTechnicalExamRequired = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsPayRequired = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdDocumentTypePrint = New DevExpress.XtraGrid.Columns.GridColumn
        Me.RepositoryItemLookUpEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
        Me.DocumentTypePrintsListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
        Me.DocumentTypesOptionsGridControl = New DevExpress.XtraGrid.GridControl
        Me.DocumentTypesOptionsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId1 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdDocumentTypes = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colOptionName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsNewRegistration = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsTehnicalExamRquired = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colRelationDeleted = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colVehicleDeleted = New DevExpress.XtraGrid.Columns.GridColumn
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
        Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.DocumentTypesGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DocumentTypesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DocumentTypePrintsListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DocumentTypesOptionsGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DocumentTypesOptionsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LayoutControl1, False)
        Me.LayoutControl1.Controls.Add(Me.DocumentTypesGridControl)
        Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
        Me.LayoutControl1.Controls.Add(Me.DocumentTypesOptionsGridControl)
        resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        '
        'DocumentTypesGridControl
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.DocumentTypesGridControl, False)
        Me.DocumentTypesGridControl.DataSource = Me.DocumentTypesBindingSource
        resources.ApplyResources(Me.DocumentTypesGridControl, "DocumentTypesGridControl")
        Me.DocumentTypesGridControl.MainView = Me.GridView1
        Me.DocumentTypesGridControl.Name = "DocumentTypesGridControl"
        Me.DocumentTypesGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemLookUpEdit1})
        Me.DocumentTypesGridControl.UseEmbeddedNavigator = True
        Me.DocumentTypesGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'DocumentTypesBindingSource
        '
        Me.DocumentTypesBindingSource.DataSource = GetType(VTE.Library.DocumentType)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.DocumentTypesBindingSource, False)
        '
        'GridView1
        '
        Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
        Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView1.Appearance.Row.Options.UseTextOptions = True
        Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colDocumentTypeName, Me.colIsBidirectional, Me.colIsVehiceRequired, Me.colIsTechnicalExamRequired, Me.colIsPayRequired, Me.colIdDocumentTypePrint})
        Me.GridView1.GridControl = Me.DocumentTypesGridControl
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
        'colDocumentTypeName
        '
        resources.ApplyResources(Me.colDocumentTypeName, "colDocumentTypeName")
        Me.colDocumentTypeName.FieldName = "DocumentTypeName"
        Me.colDocumentTypeName.Name = "colDocumentTypeName"
        '
        'colIsBidirectional
        '
        resources.ApplyResources(Me.colIsBidirectional, "colIsBidirectional")
        Me.colIsBidirectional.FieldName = "IsBidirectional"
        Me.colIsBidirectional.Name = "colIsBidirectional"
        '
        'colIsVehiceRequired
        '
        resources.ApplyResources(Me.colIsVehiceRequired, "colIsVehiceRequired")
        Me.colIsVehiceRequired.FieldName = "IsVehiceRequired"
        Me.colIsVehiceRequired.Name = "colIsVehiceRequired"
        '
        'colIsTechnicalExamRequired
        '
        resources.ApplyResources(Me.colIsTechnicalExamRequired, "colIsTechnicalExamRequired")
        Me.colIsTechnicalExamRequired.FieldName = "IsTechnicalExamRequired"
        Me.colIsTechnicalExamRequired.Name = "colIsTechnicalExamRequired"
        '
        'colIsPayRequired
        '
        resources.ApplyResources(Me.colIsPayRequired, "colIsPayRequired")
        Me.colIsPayRequired.FieldName = "IsPayRequired"
        Me.colIsPayRequired.Name = "colIsPayRequired"
        '
        'colIdDocumentTypePrint
        '
        resources.ApplyResources(Me.colIdDocumentTypePrint, "colIdDocumentTypePrint")
        Me.colIdDocumentTypePrint.ColumnEdit = Me.RepositoryItemLookUpEdit1
        Me.colIdDocumentTypePrint.FieldName = "IdDocumentTypePrint"
        Me.colIdDocumentTypePrint.Name = "colIdDocumentTypePrint"
        '
        'RepositoryItemLookUpEdit1
        '
        resources.ApplyResources(Me.RepositoryItemLookUpEdit1, "RepositoryItemLookUpEdit1")
        Me.RepositoryItemLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.RepositoryItemLookUpEdit1.DataSource = Me.DocumentTypePrintsListBindingSource
        Me.RepositoryItemLookUpEdit1.DisplayMember = "Opis"
        Me.RepositoryItemLookUpEdit1.Name = "RepositoryItemLookUpEdit1"
        Me.RepositoryItemLookUpEdit1.ValueMember = "Id"
        '
        'DocumentTypePrintsListBindingSource
        '
        Me.DocumentTypePrintsListBindingSource.DataSource = GetType(VTE.Library.DocumentTypePrintsList)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.DocumentTypePrintsListBindingSource, False)
        '
        'UxKopcinja1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UxKopcinja1, False)
        resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
        Me.UxKopcinja1.Name = "UxKopcinja1"
        '
        'DocumentTypesOptionsGridControl
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.DocumentTypesOptionsGridControl, False)
        Me.DocumentTypesOptionsGridControl.DataSource = Me.DocumentTypesOptionsBindingSource
        resources.ApplyResources(Me.DocumentTypesOptionsGridControl, "DocumentTypesOptionsGridControl")
        Me.DocumentTypesOptionsGridControl.MainView = Me.GridView2
        Me.DocumentTypesOptionsGridControl.Name = "DocumentTypesOptionsGridControl"
        Me.DocumentTypesOptionsGridControl.UseEmbeddedNavigator = True
        Me.DocumentTypesOptionsGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView2})
        '
        'DocumentTypesOptionsBindingSource
        '
        Me.DocumentTypesOptionsBindingSource.DataMember = "DocumentTypesOptions"
        Me.DocumentTypesOptionsBindingSource.DataSource = Me.DocumentTypesBindingSource
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.DocumentTypesOptionsBindingSource, False)
        '
        'GridView2
        '
        Me.GridView2.Appearance.HeaderPanel.Options.UseTextOptions = True
        Me.GridView2.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView2.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView2.Appearance.Row.Options.UseTextOptions = True
        Me.GridView2.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView2.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView2.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId1, Me.colIdDocumentTypes, Me.colOptionName, Me.colIsNewRegistration, Me.colIsTehnicalExamRquired, Me.colRelationDeleted, Me.colVehicleDeleted})
        Me.GridView2.GridControl = Me.DocumentTypesOptionsGridControl
        Me.GridView2.Name = "GridView2"
        Me.GridView2.OptionsNavigation.AutoFocusNewRow = True
        Me.GridView2.OptionsNavigation.EnterMoveNextColumn = True
        Me.GridView2.OptionsView.ShowAutoFilterRow = True
        '
        'colId1
        '
        resources.ApplyResources(Me.colId1, "colId1")
        Me.colId1.FieldName = "Id"
        Me.colId1.Name = "colId1"
        Me.colId1.OptionsColumn.ReadOnly = True
        '
        'colIdDocumentTypes
        '
        resources.ApplyResources(Me.colIdDocumentTypes, "colIdDocumentTypes")
        Me.colIdDocumentTypes.FieldName = "IdDocumentTypes"
        Me.colIdDocumentTypes.Name = "colIdDocumentTypes"
        '
        'colOptionName
        '
        resources.ApplyResources(Me.colOptionName, "colOptionName")
        Me.colOptionName.FieldName = "OptionName"
        Me.colOptionName.Name = "colOptionName"
        '
        'colIsNewRegistration
        '
        resources.ApplyResources(Me.colIsNewRegistration, "colIsNewRegistration")
        Me.colIsNewRegistration.FieldName = "IsNewRegistration"
        Me.colIsNewRegistration.Name = "colIsNewRegistration"
        '
        'colIsTehnicalExamRquired
        '
        resources.ApplyResources(Me.colIsTehnicalExamRquired, "colIsTehnicalExamRquired")
        Me.colIsTehnicalExamRquired.FieldName = "IsTehnicalExamRquired"
        Me.colIsTehnicalExamRquired.Name = "colIsTehnicalExamRquired"
        '
        'colRelationDeleted
        '
        resources.ApplyResources(Me.colRelationDeleted, "colRelationDeleted")
        Me.colRelationDeleted.FieldName = "RelationDeleted"
        Me.colRelationDeleted.Name = "colRelationDeleted"
        '
        'colVehicleDeleted
        '
        resources.ApplyResources(Me.colVehicleDeleted, "colVehicleDeleted")
        Me.colVehicleDeleted.FieldName = "VehicleDeleted"
        Me.colVehicleDeleted.Name = "colVehicleDeleted"
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.SplitterItem1, Me.LayoutControlItem3})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(901, 639)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.UxKopcinja1
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(899, 72)
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.DocumentTypesGridControl
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 78)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(899, 279)
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'SplitterItem1
        '
        resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
        Me.SplitterItem1.Location = New System.Drawing.Point(0, 72)
        Me.SplitterItem1.Name = "SplitterItem1"
        Me.SplitterItem1.Size = New System.Drawing.Size(899, 6)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.DocumentTypesOptionsGridControl
        resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 357)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(899, 280)
        Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        Me.DxErrorProvider1.DataSource = Me.DocumentTypesBindingSource
        '
        'uxDocumentTypes
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "uxDocumentTypes"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.DocumentTypesGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DocumentTypesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DocumentTypePrintsListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DocumentTypesOptionsGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DocumentTypesOptionsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents DocumentTypesGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents DocumentTypesBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDocumentTypeName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIsBidirectional As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIsVehiceRequired As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIsTechnicalExamRequired As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIsPayRequired As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdDocumentTypePrint As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents RepositoryItemLookUpEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents DocumentTypePrintsListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents DocumentTypesOptionsGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents DocumentTypesOptionsBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdDocumentTypes As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colOptionName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents colIsNewRegistration As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIsTehnicalExamRquired As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colRelationDeleted As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colVehicleDeleted As DevExpress.XtraGrid.Columns.GridColumn

End Class
