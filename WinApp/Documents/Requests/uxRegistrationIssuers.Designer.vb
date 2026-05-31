<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxRegistrationIssuers
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxRegistrationIssuers))
  Me.IssuersLayoutControl = New DevExpress.XtraLayout.LayoutControl
  Me.RegistrationIssuersGridControl = New DevExpress.XtraGrid.GridControl
  Me.RegistrationIssuersBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIssuerName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colIdCommunity = New DevExpress.XtraGrid.Columns.GridColumn
  Me.RepositoryItemLookUpEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
  Me.CommunitiesListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
  Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
  Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
  Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
  Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
  Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
  CType(Me.IssuersLayoutControl, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.IssuersLayoutControl.SuspendLayout()
  CType(Me.RegistrationIssuersGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.RegistrationIssuersBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CommunitiesListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'IssuersLayoutControl
  '
  Me.IssuersLayoutControl.AccessibleDescription = Nothing
  Me.IssuersLayoutControl.AccessibleName = Nothing
  resources.ApplyResources(Me.IssuersLayoutControl, "IssuersLayoutControl")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.IssuersLayoutControl, False)
  Me.IssuersLayoutControl.BackgroundImage = Nothing
  Me.IssuersLayoutControl.Controls.Add(Me.RegistrationIssuersGridControl)
  Me.IssuersLayoutControl.Controls.Add(Me.UxKopcinja1)
  Me.IssuersLayoutControl.Font = Nothing
  Me.IssuersLayoutControl.Name = "IssuersLayoutControl"
  Me.IssuersLayoutControl.Root = Me.LayoutControlGroup1
  '
  'RegistrationIssuersGridControl
  '
  Me.RegistrationIssuersGridControl.AccessibleDescription = Nothing
  Me.RegistrationIssuersGridControl.AccessibleName = Nothing
  resources.ApplyResources(Me.RegistrationIssuersGridControl, "RegistrationIssuersGridControl")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.RegistrationIssuersGridControl, False)
  Me.RegistrationIssuersGridControl.BackgroundImage = Nothing
  Me.RegistrationIssuersGridControl.DataSource = Me.RegistrationIssuersBindingSource
  Me.RegistrationIssuersGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
  Me.RegistrationIssuersGridControl.EmbeddedNavigator.AccessibleName = Nothing
  Me.RegistrationIssuersGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("RegistrationIssuersGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
  Me.RegistrationIssuersGridControl.EmbeddedNavigator.BackgroundImage = Nothing
  Me.RegistrationIssuersGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("RegistrationIssuersGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
  Me.RegistrationIssuersGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("RegistrationIssuersGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
  Me.RegistrationIssuersGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("RegistrationIssuersGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
  Me.RegistrationIssuersGridControl.EmbeddedNavigator.ToolTip = resources.GetString("RegistrationIssuersGridControl.EmbeddedNavigator.ToolTip")
  Me.RegistrationIssuersGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("RegistrationIssuersGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
  Me.RegistrationIssuersGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("RegistrationIssuersGridControl.EmbeddedNavigator.ToolTipTitle")
  Me.RegistrationIssuersGridControl.Font = Nothing
  Me.RegistrationIssuersGridControl.MainView = Me.GridView1
  Me.RegistrationIssuersGridControl.Name = "RegistrationIssuersGridControl"
  Me.RegistrationIssuersGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemLookUpEdit1})
  Me.RegistrationIssuersGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'RegistrationIssuersBindingSource
  '
  Me.RegistrationIssuersBindingSource.DataSource = GetType(VTE.Library.RegistrationIssuers)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.RegistrationIssuersBindingSource, False)
  '
  'GridView1
  '
  resources.ApplyResources(Me.GridView1, "GridView1")
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIssuerName, Me.colIdCommunity})
  Me.GridView1.GridControl = Me.RegistrationIssuersGridControl
  Me.GridView1.Name = "GridView1"
  Me.GridView1.OptionsNavigation.AutoFocusNewRow = True
  Me.GridView1.OptionsNavigation.EnterMoveNextColumn = True
  Me.GridView1.OptionsView.ShowAutoFilterRow = True
  Me.GridView1.OptionsView.ShowFooter = True
  '
  'colId
  '
  resources.ApplyResources(Me.colId, "colId")
  Me.colId.FieldName = "Id"
  Me.colId.Name = "colId"
  Me.colId.OptionsColumn.ReadOnly = True
  '
  'colIssuerName
  '
  resources.ApplyResources(Me.colIssuerName, "colIssuerName")
  Me.colIssuerName.FieldName = "IssuerName"
  Me.colIssuerName.Name = "colIssuerName"
  Me.colIssuerName.SummaryItem.DisplayFormat = resources.GetString("colIssuerName.SummaryItem.DisplayFormat")
  Me.colIssuerName.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
  '
  'colIdCommunity
  '
  resources.ApplyResources(Me.colIdCommunity, "colIdCommunity")
  Me.colIdCommunity.ColumnEdit = Me.RepositoryItemLookUpEdit1
  Me.colIdCommunity.FieldName = "IdCommunity"
  Me.colIdCommunity.Name = "colIdCommunity"
  Me.colIdCommunity.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
  '
  'RepositoryItemLookUpEdit1
  '
  Me.RepositoryItemLookUpEdit1.AccessibleDescription = Nothing
  Me.RepositoryItemLookUpEdit1.AccessibleName = Nothing
  resources.ApplyResources(Me.RepositoryItemLookUpEdit1, "RepositoryItemLookUpEdit1")
  Me.RepositoryItemLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("RepositoryItemLookUpEdit1.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
  Me.RepositoryItemLookUpEdit1.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CommunityCode", "CommunityCode", 84, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CommunityName", "CommunityName", 86, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("RegistrationCode", "RegistrationCode", 89, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name", 33, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
  Me.RepositoryItemLookUpEdit1.DataSource = Me.CommunitiesListBindingSource
  Me.RepositoryItemLookUpEdit1.DisplayMember = "CommunityName"
  Me.RepositoryItemLookUpEdit1.Name = "RepositoryItemLookUpEdit1"
  Me.RepositoryItemLookUpEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
  Me.RepositoryItemLookUpEdit1.ValueMember = "Id"
  '
  'CommunitiesListBindingSource
  '
  Me.CommunitiesListBindingSource.DataSource = GetType(VTE.Library.CommunitiesList)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CommunitiesListBindingSource, False)
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
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(669, 521)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.UxKopcinja1
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(665, 64)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem1.TextToControlDistance = 0
  Me.LayoutControlItem1.TextVisible = False
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.RegistrationIssuersGridControl
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 70)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(665, 447)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem2.TextToControlDistance = 0
  Me.LayoutControlItem2.TextVisible = False
  '
  'SplitterItem1
  '
  resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
  Me.SplitterItem1.Location = New System.Drawing.Point(0, 64)
  Me.SplitterItem1.Name = "SplitterItem1"
  Me.SplitterItem1.Size = New System.Drawing.Size(665, 6)
  '
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.RegistrationIssuersBindingSource
  '
  'uxRegistrationIssuers
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.IssuersLayoutControl)
  Me.Name = "uxRegistrationIssuers"
  Me.Controls.SetChildIndex(Me.IssuersLayoutControl, 0)
  CType(Me.IssuersLayoutControl, System.ComponentModel.ISupportInitialize).EndInit()
  Me.IssuersLayoutControl.ResumeLayout(False)
  CType(Me.RegistrationIssuersGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.RegistrationIssuersBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.RepositoryItemLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CommunitiesListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents IssuersLayoutControl As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents RegistrationIssuersGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents RegistrationIssuersBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIssuerName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
    Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
    Friend WithEvents colIdCommunity As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemLookUpEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents CommunitiesListBindingSource As System.Windows.Forms.BindingSource

End Class
