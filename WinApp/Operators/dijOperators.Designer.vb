<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijOperators
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dijOperators))
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.UsersGridControl = New DevExpress.XtraGrid.GridControl
    Me.UsersBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdRole1 = New DevExpress.XtraGrid.Columns.GridColumn
    Me.RepositoryItemLookUpEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Me.RoolListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.colIdDataBase1 = New DevExpress.XtraGrid.Columns.GridColumn
    Me.RepositoryItemLookUpEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Me.DataBasesListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.colUserFullName1 = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colUserName1 = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colUserPass1 = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colFirstName = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colSureName = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colAddress = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colEmbg = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colBlk = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colDateOfBirth = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colDateOfHireing = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colRfid = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdStation = New DevExpress.XtraGrid.Columns.GridColumn
    Me.RepositoryItemLookUpEdit3 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Me.TehnicalExamOrganizationsListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.GridView3 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
    Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
    Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.UsersGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.UsersBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RoolListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemLookUpEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DataBasesListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemLookUpEdit3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.TehnicalExamOrganizationsListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'LayoutControl1
    '
    Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LayoutControl1, False)
    Me.LayoutControl1.Controls.Add(Me.UsersGridControl)
    Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
    resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    '
    'UsersGridControl
    '
    Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UsersGridControl, False)
    Me.UsersGridControl.DataSource = Me.UsersBindingSource
    resources.ApplyResources(Me.UsersGridControl, "UsersGridControl")
    Me.UsersGridControl.MainView = Me.GridView1
    Me.UsersGridControl.Name = "UsersGridControl"
    Me.UsersGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemLookUpEdit1, Me.RepositoryItemLookUpEdit2, Me.RepositoryItemLookUpEdit3})
    Me.UsersGridControl.UseEmbeddedNavigator = True
    Me.UsersGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView3})
    '
    'UsersBindingSource
    '
    Me.UsersBindingSource.DataSource = GetType(VTE.Library.User)
    Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.UsersBindingSource, False)
    '
    'GridView1
    '
    Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
    Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    Me.GridView1.Appearance.Row.Options.UseTextOptions = True
    Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdRole1, Me.colIdDataBase1, Me.colUserFullName1, Me.colUserName1, Me.colUserPass1, Me.colFirstName, Me.colSureName, Me.colAddress, Me.colEmbg, Me.colBlk, Me.colDateOfBirth, Me.colDateOfHireing, Me.colRfid, Me.colIdStation})
    Me.GridView1.GridControl = Me.UsersGridControl
    Me.GridView1.Name = "GridView1"
    Me.GridView1.OptionsNavigation.AutoFocusNewRow = True
    Me.GridView1.OptionsNavigation.EnterMoveNextColumn = True
    Me.GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
    '
    'colId
    '
    resources.ApplyResources(Me.colId, "colId")
    Me.colId.FieldName = "Id"
    Me.colId.Name = "colId"
    Me.colId.OptionsColumn.ReadOnly = True
    '
    'colIdRole1
    '
    resources.ApplyResources(Me.colIdRole1, "colIdRole1")
    Me.colIdRole1.ColumnEdit = Me.RepositoryItemLookUpEdit1
    Me.colIdRole1.FieldName = "IdRole"
    Me.colIdRole1.Name = "colIdRole1"
    '
    'RepositoryItemLookUpEdit1
    '
    resources.ApplyResources(Me.RepositoryItemLookUpEdit1, "RepositoryItemLookUpEdit1")
    Me.RepositoryItemLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.RepositoryItemLookUpEdit1.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("RoleName", "RoleName", 54, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near)})
    Me.RepositoryItemLookUpEdit1.DataSource = Me.RoolListBindingSource
    Me.RepositoryItemLookUpEdit1.DisplayMember = "RoleName"
    Me.RepositoryItemLookUpEdit1.Name = "RepositoryItemLookUpEdit1"
    Me.RepositoryItemLookUpEdit1.ValueMember = "Id"
    '
    'RoolListBindingSource
    '
    Me.RoolListBindingSource.DataSource = GetType(VTE.Library.RoolList)
    Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.RoolListBindingSource, False)
    '
    'colIdDataBase1
    '
    resources.ApplyResources(Me.colIdDataBase1, "colIdDataBase1")
    Me.colIdDataBase1.ColumnEdit = Me.RepositoryItemLookUpEdit2
    Me.colIdDataBase1.FieldName = "IdDataBase"
    Me.colIdDataBase1.Name = "colIdDataBase1"
    Me.colIdDataBase1.OptionsFilter.AllowAutoFilter = False
    Me.colIdDataBase1.OptionsFilter.AllowFilter = False
    Me.colIdDataBase1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Equals
    '
    'RepositoryItemLookUpEdit2
    '
    resources.ApplyResources(Me.RepositoryItemLookUpEdit2, "RepositoryItemLookUpEdit2")
    Me.RepositoryItemLookUpEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit2.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.RepositoryItemLookUpEdit2.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("EndUserName", "EndUserName", 73, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("DatabaseName", "DatabaseName", 79, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ConnetionString", "ConnetionString", 83, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
    Me.RepositoryItemLookUpEdit2.DataSource = Me.DataBasesListBindingSource
    Me.RepositoryItemLookUpEdit2.DisplayMember = "EndUserName"
    Me.RepositoryItemLookUpEdit2.Name = "RepositoryItemLookUpEdit2"
    Me.RepositoryItemLookUpEdit2.ValueMember = "Id"
    '
    'DataBasesListBindingSource
    '
    Me.DataBasesListBindingSource.DataSource = GetType(VTE.Library.DataBasesList)
    Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.DataBasesListBindingSource, False)
    '
    'colUserFullName1
    '
    resources.ApplyResources(Me.colUserFullName1, "colUserFullName1")
    Me.colUserFullName1.FieldName = "UserFullName"
    Me.colUserFullName1.Name = "colUserFullName1"
    '
    'colUserName1
    '
    resources.ApplyResources(Me.colUserName1, "colUserName1")
    Me.colUserName1.FieldName = "UserName"
    Me.colUserName1.Name = "colUserName1"
    '
    'colUserPass1
    '
    resources.ApplyResources(Me.colUserPass1, "colUserPass1")
    Me.colUserPass1.FieldName = "UserPass"
    Me.colUserPass1.Name = "colUserPass1"
    '
    'colFirstName
    '
    resources.ApplyResources(Me.colFirstName, "colFirstName")
    Me.colFirstName.FieldName = "FirstName"
    Me.colFirstName.Name = "colFirstName"
    '
    'colSureName
    '
    resources.ApplyResources(Me.colSureName, "colSureName")
    Me.colSureName.FieldName = "SureName"
    Me.colSureName.Name = "colSureName"
    '
    'colAddress
    '
    resources.ApplyResources(Me.colAddress, "colAddress")
    Me.colAddress.FieldName = "Address"
    Me.colAddress.Name = "colAddress"
    '
    'colEmbg
    '
    resources.ApplyResources(Me.colEmbg, "colEmbg")
    Me.colEmbg.FieldName = "Embg"
    Me.colEmbg.Name = "colEmbg"
    '
    'colBlk
    '
    resources.ApplyResources(Me.colBlk, "colBlk")
    Me.colBlk.FieldName = "Blk"
    Me.colBlk.Name = "colBlk"
    '
    'colDateOfBirth
    '
    resources.ApplyResources(Me.colDateOfBirth, "colDateOfBirth")
    Me.colDateOfBirth.FieldName = "DateOfBirth"
    Me.colDateOfBirth.Name = "colDateOfBirth"
    '
    'colDateOfHireing
    '
    resources.ApplyResources(Me.colDateOfHireing, "colDateOfHireing")
    Me.colDateOfHireing.FieldName = "DateOfHireing"
    Me.colDateOfHireing.Name = "colDateOfHireing"
    '
    'colRfid
    '
    resources.ApplyResources(Me.colRfid, "colRfid")
    Me.colRfid.FieldName = "Rfid"
    Me.colRfid.Name = "colRfid"
    '
    'colIdStation
    '
    Me.colIdStation.ColumnEdit = Me.RepositoryItemLookUpEdit3
    Me.colIdStation.FieldName = "IdStation"
    Me.colIdStation.Name = "colIdStation"
    resources.ApplyResources(Me.colIdStation, "colIdStation")
    '
    'RepositoryItemLookUpEdit3
    '
    Me.RepositoryItemLookUpEdit3.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit3.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.RepositoryItemLookUpEdit3.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("OrganizationName", "OrganizationName", 94, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("OrganizationAndStationName", "OrganizationAndStationName", 147, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Station", "Station", 40, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdCity", "IdCity", 35, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None)})
    Me.RepositoryItemLookUpEdit3.DataSource = Me.TehnicalExamOrganizationsListBindingSource
    Me.RepositoryItemLookUpEdit3.DisplayMember = "OrganizationAndStationName"
    Me.RepositoryItemLookUpEdit3.Name = "RepositoryItemLookUpEdit3"
    Me.RepositoryItemLookUpEdit3.ValueMember = "Id"
    '
    'TehnicalExamOrganizationsListBindingSource
    '
    Me.TehnicalExamOrganizationsListBindingSource.DataSource = GetType(VTE.Library.TehnicalExamOrganizationsList)
    Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.TehnicalExamOrganizationsListBindingSource, False)
    '
    'GridView3
    '
    Me.GridView3.GridControl = Me.UsersGridControl
    Me.GridView3.Name = "GridView3"
    '
    'UxKopcinja1
    '
    Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UxKopcinja1, False)
    resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
    Me.UxKopcinja1.Name = "UxKopcinja1"
    '
    'LayoutControlGroup1
    '
    resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.SplitterItem1, Me.LayoutControlItem2})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "Root"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(855, 632)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.TextVisible = False
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.UxKopcinja1
    resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(853, 81)
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem1.TextToControlDistance = 0
    Me.LayoutControlItem1.TextVisible = False
    '
    'SplitterItem1
    '
    resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
    Me.SplitterItem1.Location = New System.Drawing.Point(0, 81)
    Me.SplitterItem1.Name = "SplitterItem1"
    Me.SplitterItem1.Size = New System.Drawing.Size(853, 6)
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.UsersGridControl
    resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
    Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 87)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(853, 543)
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem2.TextToControlDistance = 0
    Me.LayoutControlItem2.TextVisible = False
    '
    'DxErrorProvider1
    '
    Me.DxErrorProvider1.ContainerControl = Me
    Me.DxErrorProvider1.DataSource = Me.UsersBindingSource
    '
    'dijOperators
    '
    Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
    resources.ApplyResources(Me, "$this")
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "dijOperators"
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.UsersGridControl, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.UsersBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RoolListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemLookUpEdit2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DataBasesListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemLookUpEdit3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.TehnicalExamOrganizationsListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents RoolListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents DataBasesListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents UsersGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents UsersBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdRole1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdDataBase1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colUserFullName1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colUserName1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colUserPass1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colFirstName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colSureName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colAddress As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colEmbg As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colBlk As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDateOfBirth As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDateOfHireing As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colRfid As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents GridView3 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents RepositoryItemLookUpEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
    Friend WithEvents RepositoryItemLookUpEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents colIdStation As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemLookUpEdit3 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents TehnicalExamOrganizationsListBindingSource As System.Windows.Forms.BindingSource
End Class
