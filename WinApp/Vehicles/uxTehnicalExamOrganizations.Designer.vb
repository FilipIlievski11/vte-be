<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxTehnicalExamOrganizations
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxTehnicalExamOrganizations))
  Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.TehnicalExamOrganizationsGridControl = New DevExpress.XtraGrid.GridControl
  Me.TehnicalExamOrganizationsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colOrganizationName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colStation = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIdCity = New DevExpress.XtraGrid.Columns.GridColumn
  Me.LookUpEditCity = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Me.CityListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.colIdCompany = New DevExpress.XtraGrid.Columns.GridColumn
  Me.RepositoryItemLookUpEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Me.CompanyListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
  Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
  Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
  Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
  Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
  Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.LayoutControl1.SuspendLayout()
  CType(Me.TehnicalExamOrganizationsGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.TehnicalExamOrganizationsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LookUpEditCity, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CityListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CompanyListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
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
  'LayoutControl1
  '
  Me.LayoutControl1.AccessibleDescription = Nothing
  Me.LayoutControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LayoutControl1, False)
  Me.LayoutControl1.BackgroundImage = Nothing
  Me.LayoutControl1.Controls.Add(Me.TehnicalExamOrganizationsGridControl)
  Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
  Me.LayoutControl1.Font = Nothing
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
  '
  'TehnicalExamOrganizationsGridControl
  '
  Me.TehnicalExamOrganizationsGridControl.AccessibleDescription = Nothing
  Me.TehnicalExamOrganizationsGridControl.AccessibleName = Nothing
  resources.ApplyResources(Me.TehnicalExamOrganizationsGridControl, "TehnicalExamOrganizationsGridControl")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.TehnicalExamOrganizationsGridControl, False)
  Me.TehnicalExamOrganizationsGridControl.BackgroundImage = Nothing
  Me.TehnicalExamOrganizationsGridControl.DataSource = Me.TehnicalExamOrganizationsBindingSource
  Me.TehnicalExamOrganizationsGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
  Me.TehnicalExamOrganizationsGridControl.EmbeddedNavigator.AccessibleName = Nothing
  Me.TehnicalExamOrganizationsGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("TehnicalExamOrganizationsGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
  Me.TehnicalExamOrganizationsGridControl.EmbeddedNavigator.BackgroundImage = Nothing
  Me.TehnicalExamOrganizationsGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("TehnicalExamOrganizationsGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
  Me.TehnicalExamOrganizationsGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("TehnicalExamOrganizationsGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
  Me.TehnicalExamOrganizationsGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("TehnicalExamOrganizationsGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
  Me.TehnicalExamOrganizationsGridControl.EmbeddedNavigator.ToolTip = resources.GetString("TehnicalExamOrganizationsGridControl.EmbeddedNavigator.ToolTip")
  Me.TehnicalExamOrganizationsGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("TehnicalExamOrganizationsGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
  Me.TehnicalExamOrganizationsGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("TehnicalExamOrganizationsGridControl.EmbeddedNavigator.ToolTipTitle")
  Me.TehnicalExamOrganizationsGridControl.Font = Nothing
  Me.TehnicalExamOrganizationsGridControl.MainView = Me.GridView1
  Me.TehnicalExamOrganizationsGridControl.Name = "TehnicalExamOrganizationsGridControl"
  Me.TehnicalExamOrganizationsGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.LookUpEditCity, Me.RepositoryItemLookUpEdit1})
  Me.TehnicalExamOrganizationsGridControl.UseEmbeddedNavigator = True
  Me.TehnicalExamOrganizationsGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'TehnicalExamOrganizationsBindingSource
  '
  Me.TehnicalExamOrganizationsBindingSource.DataSource = GetType(VTE.Library.TehnicalExamOrganization)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.TehnicalExamOrganizationsBindingSource, False)
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
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colOrganizationName, Me.colStation, Me.colIdCity, Me.colIdCompany})
  Me.GridView1.GridControl = Me.TehnicalExamOrganizationsGridControl
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
  'colOrganizationName
  '
  resources.ApplyResources(Me.colOrganizationName, "colOrganizationName")
  Me.colOrganizationName.FieldName = "OrganizationName"
  Me.colOrganizationName.Name = "colOrganizationName"
  '
  'colStation
  '
  resources.ApplyResources(Me.colStation, "colStation")
  Me.colStation.FieldName = "Station"
  Me.colStation.Name = "colStation"
  '
  'colIdCity
  '
  resources.ApplyResources(Me.colIdCity, "colIdCity")
  Me.colIdCity.ColumnEdit = Me.LookUpEditCity
  Me.colIdCity.FieldName = "IdCity"
  Me.colIdCity.Name = "colIdCity"
  '
  'LookUpEditCity
  '
  Me.LookUpEditCity.AccessibleDescription = Nothing
  Me.LookUpEditCity.AccessibleName = Nothing
  resources.ApplyResources(Me.LookUpEditCity, "LookUpEditCity")
  Me.LookUpEditCity.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCity.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCity.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditCity.Buttons2"), CType(resources.GetObject("LookUpEditCity.Buttons3"), Integer), CType(resources.GetObject("LookUpEditCity.Buttons4"), Boolean), CType(resources.GetObject("LookUpEditCity.Buttons5"), Boolean), CType(resources.GetObject("LookUpEditCity.Buttons6"), Boolean), CType(resources.GetObject("LookUpEditCity.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing)})
  Me.LookUpEditCity.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CityName", "CityName", 52, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name", 33, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CityZip", "CityZip", 39, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdCommunity", "IdCommunity", 69, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CommunityCode", "CommunityCode", 84, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CommunityName", "CommunityName", 86, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdCountry", "IdCountry", 55, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CountryName", "CountryName", 72, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
  Me.LookUpEditCity.DataSource = Me.CityListBindingSource
  Me.LookUpEditCity.DisplayMember = "CityName"
  Me.LookUpEditCity.Name = "LookUpEditCity"
  Me.LookUpEditCity.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
  Me.LookUpEditCity.ValueMember = "Id"
  '
  'CityListBindingSource
  '
  Me.CityListBindingSource.DataSource = GetType(VTE.Library.CityList)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CityListBindingSource, False)
  '
  'colIdCompany
  '
  resources.ApplyResources(Me.colIdCompany, "colIdCompany")
  Me.colIdCompany.ColumnEdit = Me.RepositoryItemLookUpEdit1
  Me.colIdCompany.FieldName = "IdCompany"
  Me.colIdCompany.Name = "colIdCompany"
  '
  'RepositoryItemLookUpEdit1
  '
  Me.RepositoryItemLookUpEdit1.AccessibleDescription = Nothing
  Me.RepositoryItemLookUpEdit1.AccessibleName = Nothing
  resources.ApplyResources(Me.RepositoryItemLookUpEdit1, "RepositoryItemLookUpEdit1")
  Me.RepositoryItemLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
  Me.RepositoryItemLookUpEdit1.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Companyname", "Companyname", 77, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near)})
  Me.RepositoryItemLookUpEdit1.DataSource = Me.CompanyListBindingSource
  Me.RepositoryItemLookUpEdit1.DisplayMember = "Companyname"
  Me.RepositoryItemLookUpEdit1.Name = "RepositoryItemLookUpEdit1"
  Me.RepositoryItemLookUpEdit1.ReadOnly = True
  Me.RepositoryItemLookUpEdit1.ValueMember = "Id"
  '
  'CompanyListBindingSource
  '
  Me.CompanyListBindingSource.DataSource = GetType(VTE.Library.CompanyList)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CompanyListBindingSource, False)
  '
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.SplitterItem1, Me.LayoutControlItem2})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "Root"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(726, 487)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.UxKopcinja1
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(722, 62)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem1.TextToControlDistance = 0
  Me.LayoutControlItem1.TextVisible = False
  '
  'SplitterItem1
  '
  resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
  Me.SplitterItem1.Location = New System.Drawing.Point(0, 62)
  Me.SplitterItem1.Name = "SplitterItem1"
  Me.SplitterItem1.Size = New System.Drawing.Size(722, 6)
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.TehnicalExamOrganizationsGridControl
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 68)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(722, 415)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem2.TextToControlDistance = 0
  Me.LayoutControlItem2.TextVisible = False
  '
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.TehnicalExamOrganizationsBindingSource
  '
  'uxTehnicalExamOrganizations
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.LayoutControl1)
  Me.Name = "uxTehnicalExamOrganizations"
  Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.TehnicalExamOrganizationsGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.TehnicalExamOrganizationsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LookUpEditCity, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CityListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CompanyListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents TehnicalExamOrganizationsGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents TehnicalExamOrganizationsBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colOrganizationName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colStation As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdCity As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents LookUpEditCity As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents CityListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
 Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
 Friend WithEvents colIdCompany As DevExpress.XtraGrid.Columns.GridColumn
 Friend WithEvents RepositoryItemLookUpEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
 Friend WithEvents CompanyListBindingSource As System.Windows.Forms.BindingSource

End Class
