<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxCities
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxCities))
  Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl
  Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
  Me.CitiesGridControl = New DevExpress.XtraGrid.GridControl
  Me.CitiesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCityName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCityZip = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIdCountry = New DevExpress.XtraGrid.Columns.GridColumn
  Me.LookUpEditCountry = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Me.CountriesListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.colIdCommunityCode = New DevExpress.XtraGrid.Columns.GridColumn
  Me.LookUpEditCummunity = New VTE.BaseParts.FancyLookupEdit.CustomRepositoryItemLookupEdit
  Me.CommunitiesListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.RepositoryItemLookUpEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
  Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
  Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
  CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SplitContainerControl1.SuspendLayout()
  CType(Me.CitiesGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CitiesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LookUpEditCountry, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CountriesListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LookUpEditCummunity, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CommunitiesListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'SplitContainerControl1
  '
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SplitContainerControl1, False)
  resources.ApplyResources(Me.SplitContainerControl1, "SplitContainerControl1")
  Me.SplitContainerControl1.Horizontal = False
  Me.SplitContainerControl1.Name = "SplitContainerControl1"
  Me.SplitContainerControl1.Panel1.Controls.Add(Me.UxKopcinja1)
  resources.ApplyResources(Me.SplitContainerControl1.Panel1, "SplitContainerControl1.Panel1")
  Me.SplitContainerControl1.Panel2.Controls.Add(Me.CitiesGridControl)
  resources.ApplyResources(Me.SplitContainerControl1.Panel2, "SplitContainerControl1.Panel2")
  Me.SplitContainerControl1.SplitterPosition = 60
  '
  'UxKopcinja1
  '
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UxKopcinja1, False)
  resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
  Me.UxKopcinja1.Name = "UxKopcinja1"
  '
  'CitiesGridControl
  '
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.CitiesGridControl, False)
  Me.CitiesGridControl.DataSource = Me.CitiesBindingSource
  resources.ApplyResources(Me.CitiesGridControl, "CitiesGridControl")
  Me.CitiesGridControl.MainView = Me.GridView1
  Me.CitiesGridControl.Name = "CitiesGridControl"
  Me.CitiesGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.LookUpEditCountry, Me.RepositoryItemLookUpEdit1, Me.LookUpEditCummunity})
  Me.CitiesGridControl.UseEmbeddedNavigator = True
  Me.CitiesGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'CitiesBindingSource
  '
  Me.CitiesBindingSource.DataSource = GetType(VTE.Library.City)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CitiesBindingSource, False)
  '
  'GridView1
  '
  Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
  Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  Me.GridView1.Appearance.Row.Options.UseTextOptions = True
  Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colCityName, Me.colCityZip, Me.colIdCountry, Me.colIdCommunityCode})
  Me.GridView1.GridControl = Me.CitiesGridControl
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
  'colCityName
  '
  resources.ApplyResources(Me.colCityName, "colCityName")
  Me.colCityName.FieldName = "CityName"
  Me.colCityName.Name = "colCityName"
  '
  'colCityZip
  '
  resources.ApplyResources(Me.colCityZip, "colCityZip")
  Me.colCityZip.FieldName = "CityZip"
  Me.colCityZip.Name = "colCityZip"
  '
  'colIdCountry
  '
  resources.ApplyResources(Me.colIdCountry, "colIdCountry")
  Me.colIdCountry.ColumnEdit = Me.LookUpEditCountry
  Me.colIdCountry.FieldName = "IdCountry"
  Me.colIdCountry.Name = "colIdCountry"
  '
  'LookUpEditCountry
  '
  resources.ApplyResources(Me.LookUpEditCountry, "LookUpEditCountry")
  Me.LookUpEditCountry.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCountry.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCountry.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditCountry.Buttons2"), CType(resources.GetObject("LookUpEditCountry.Buttons3"), Integer), CType(resources.GetObject("LookUpEditCountry.Buttons4"), Boolean), CType(resources.GetObject("LookUpEditCountry.Buttons5"), Boolean), CType(resources.GetObject("LookUpEditCountry.Buttons6"), Boolean), CType(resources.GetObject("LookUpEditCountry.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing)})
  Me.LookUpEditCountry.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, "", False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CountryName", "Држава", 72, DevExpress.Utils.FormatType.None, "", True, DevExpress.Utils.HorzAlignment.Near)})
  Me.LookUpEditCountry.DataSource = Me.CountriesListBindingSource
  Me.LookUpEditCountry.DisplayMember = "CountryName"
  Me.LookUpEditCountry.Name = "LookUpEditCountry"
  Me.LookUpEditCountry.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
  Me.LookUpEditCountry.ValueMember = "Id"
  '
  'CountriesListBindingSource
  '
  Me.CountriesListBindingSource.DataSource = GetType(VTE.Library.CountriesList)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CountriesListBindingSource, False)
  '
  'colIdCommunityCode
  '
  resources.ApplyResources(Me.colIdCommunityCode, "colIdCommunityCode")
  Me.colIdCommunityCode.ColumnEdit = Me.LookUpEditCummunity
  Me.colIdCommunityCode.FieldName = "IdCommunityCode"
  Me.colIdCommunityCode.Name = "colIdCommunityCode"
  '
  'LookUpEditCummunity
  '
  Me.LookUpEditCummunity.AllowNullInput = DevExpress.Utils.DefaultBoolean.[True]
  resources.ApplyResources(Me.LookUpEditCummunity, "LookUpEditCummunity")
  Me.LookUpEditCummunity.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCummunity.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCummunity.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditCummunity.Buttons2"), CType(resources.GetObject("LookUpEditCummunity.Buttons3"), Integer), CType(resources.GetObject("LookUpEditCummunity.Buttons4"), Boolean), CType(resources.GetObject("LookUpEditCummunity.Buttons5"), Boolean), CType(resources.GetObject("LookUpEditCummunity.Buttons6"), Boolean), CType(resources.GetObject("LookUpEditCummunity.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing)})
  Me.LookUpEditCummunity.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, "", False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CommunityCode", "CommunityCode", 84, DevExpress.Utils.FormatType.None, "", False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CommunityName", "CommunityName", 86, DevExpress.Utils.FormatType.None, "", False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("RegistrationCode", "RegistrationCode", 89, DevExpress.Utils.FormatType.None, "", False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "код-општина", 33, DevExpress.Utils.FormatType.None, "", True, DevExpress.Utils.HorzAlignment.Near)})
  Me.LookUpEditCummunity.DataSource = Me.CommunitiesListBindingSource
  Me.LookUpEditCummunity.DisplayMember = "CommunityName"
  Me.LookUpEditCummunity.Name = "LookUpEditCummunity"
  Me.LookUpEditCummunity.PopupSizeable = False
  Me.LookUpEditCummunity.ShowFooter = False
  Me.LookUpEditCummunity.ShowHeader = False
  Me.LookUpEditCummunity.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
  Me.LookUpEditCummunity.ValueMember = "Id"
  '
  'CommunitiesListBindingSource
  '
  Me.CommunitiesListBindingSource.DataSource = GetType(VTE.Library.CommunitiesList)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CommunitiesListBindingSource, False)
  '
  'RepositoryItemLookUpEdit1
  '
  resources.ApplyResources(Me.RepositoryItemLookUpEdit1, "RepositoryItemLookUpEdit1")
  Me.RepositoryItemLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
  Me.RepositoryItemLookUpEdit1.Name = "RepositoryItemLookUpEdit1"
  '
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.CitiesBindingSource
  '
  'uxCities
  '
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
  resources.ApplyResources(Me, "$this")
  Me.Controls.Add(Me.SplitContainerControl1)
  Me.Name = "uxCities"
  Me.Controls.SetChildIndex(Me.SplitContainerControl1, 0)
  CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.SplitContainerControl1.ResumeLayout(False)
  CType(Me.CitiesGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CitiesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LookUpEditCountry, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CountriesListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LookUpEditCummunity, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CommunitiesListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents CitiesBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents CitiesGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCityName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCityZip As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdCountry As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents LookUpEditCountry As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents CountriesListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents colIdCommunityCode As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents RepositoryItemLookUpEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents CommunitiesListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents LookUpEditCummunity As VTE.BaseParts.FancyLookupEdit.CustomRepositoryItemLookupEdit

End Class
