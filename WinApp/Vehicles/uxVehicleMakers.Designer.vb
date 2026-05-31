<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxVehicleMakers
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxVehicleMakers))
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.GridControl1 = New DevExpress.XtraGrid.GridControl
  Me.VehicleMakersBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIdCountry = New DevExpress.XtraGrid.Columns.GridColumn
  Me.LookUpEditCountry = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Me.CountriesListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.colCompanyName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCompanyTrademark = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCompanyNameAndTrademark = New DevExpress.XtraGrid.Columns.GridColumn
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
  CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.VehicleMakersBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LookUpEditCountry, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CountriesListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
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
  Me.LayoutControl1.AccessibleDescription = Nothing
  Me.LayoutControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LayoutControl1, False)
  Me.LayoutControl1.BackgroundImage = Nothing
  Me.LayoutControl1.Controls.Add(Me.GridControl1)
  Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
  Me.LayoutControl1.Font = Nothing
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
  '
  'GridControl1
  '
  Me.GridControl1.AccessibleDescription = Nothing
  Me.GridControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.GridControl1, "GridControl1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.GridControl1, False)
  Me.GridControl1.BackgroundImage = Nothing
  Me.GridControl1.DataSource = Me.VehicleMakersBindingSource
  Me.GridControl1.EmbeddedNavigator.AccessibleDescription = Nothing
  Me.GridControl1.EmbeddedNavigator.AccessibleName = Nothing
  Me.GridControl1.EmbeddedNavigator.Anchor = CType(resources.GetObject("GridControl1.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
  Me.GridControl1.EmbeddedNavigator.BackgroundImage = Nothing
  Me.GridControl1.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("GridControl1.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
  Me.GridControl1.EmbeddedNavigator.ImeMode = CType(resources.GetObject("GridControl1.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
  Me.GridControl1.EmbeddedNavigator.TextLocation = CType(resources.GetObject("GridControl1.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
  Me.GridControl1.EmbeddedNavigator.ToolTip = resources.GetString("GridControl1.EmbeddedNavigator.ToolTip")
  Me.GridControl1.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("GridControl1.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
  Me.GridControl1.EmbeddedNavigator.ToolTipTitle = resources.GetString("GridControl1.EmbeddedNavigator.ToolTipTitle")
  Me.GridControl1.Font = Nothing
  Me.GridControl1.MainView = Me.GridView1
  Me.GridControl1.Name = "GridControl1"
  Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.LookUpEditCountry})
  Me.GridControl1.UseEmbeddedNavigator = True
  Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'VehicleMakersBindingSource
  '
  Me.VehicleMakersBindingSource.DataSource = GetType(VTE.Library.VehicleMakers)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.VehicleMakersBindingSource, False)
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
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdCountry, Me.colCompanyName, Me.colCompanyTrademark, Me.colCompanyNameAndTrademark})
  Me.GridView1.GridControl = Me.GridControl1
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
  'colIdCountry
  '
  resources.ApplyResources(Me.colIdCountry, "colIdCountry")
  Me.colIdCountry.ColumnEdit = Me.LookUpEditCountry
  Me.colIdCountry.FieldName = "IdCountry"
  Me.colIdCountry.Name = "colIdCountry"
  '
  'LookUpEditCountry
  '
  Me.LookUpEditCountry.AccessibleDescription = Nothing
  Me.LookUpEditCountry.AccessibleName = Nothing
  resources.ApplyResources(Me.LookUpEditCountry, "LookUpEditCountry")
  Me.LookUpEditCountry.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCountry.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines)), New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEditCountry.Buttons1"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("LookUpEditCountry.Buttons2"), CType(resources.GetObject("LookUpEditCountry.Buttons3"), Integer), CType(resources.GetObject("LookUpEditCountry.Buttons4"), Boolean), CType(resources.GetObject("LookUpEditCountry.Buttons5"), Boolean), CType(resources.GetObject("LookUpEditCountry.Buttons6"), Boolean), CType(resources.GetObject("LookUpEditCountry.Buttons7"), DevExpress.XtraEditors.ImageLocation), Nothing)})
  Me.LookUpEditCountry.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CountryName", "Име на држава", 72, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near)})
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
  'colCompanyName
  '
  resources.ApplyResources(Me.colCompanyName, "colCompanyName")
  Me.colCompanyName.FieldName = "CompanyName"
  Me.colCompanyName.Name = "colCompanyName"
  '
  'colCompanyTrademark
  '
  resources.ApplyResources(Me.colCompanyTrademark, "colCompanyTrademark")
  Me.colCompanyTrademark.FieldName = "CompanyTrademark"
  Me.colCompanyTrademark.Name = "colCompanyTrademark"
  '
  'colCompanyNameAndTrademark
  '
  resources.ApplyResources(Me.colCompanyNameAndTrademark, "colCompanyNameAndTrademark")
  Me.colCompanyNameAndTrademark.FieldName = "CompanyNameAndTrademark"
  Me.colCompanyNameAndTrademark.Name = "colCompanyNameAndTrademark"
  Me.colCompanyNameAndTrademark.OptionsColumn.ReadOnly = True
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
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.SplitterItem1})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "Root"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(576, 389)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.UxKopcinja1
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(572, 57)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem1.TextToControlDistance = 0
  Me.LayoutControlItem1.TextVisible = False
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.GridControl1
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 63)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(572, 322)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem2.TextToControlDistance = 0
  Me.LayoutControlItem2.TextVisible = False
  '
  'SplitterItem1
  '
  resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
  Me.SplitterItem1.Location = New System.Drawing.Point(0, 57)
  Me.SplitterItem1.Name = "SplitterItem1"
  Me.SplitterItem1.Size = New System.Drawing.Size(572, 6)
  '
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.VehicleMakersBindingSource
  '
  'uxVehicleMakers
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.LayoutControl1)
  Me.Name = "uxVehicleMakers"
  Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.VehicleMakersBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LookUpEditCountry, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CountriesListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
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
  Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
  Friend WithEvents VehicleMakersBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdCountry As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents LookUpEditCountry As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Friend WithEvents colCompanyName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCompanyTrademark As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCompanyNameAndTrademark As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents CountriesListBindingSource As System.Windows.Forms.BindingSource

End Class
