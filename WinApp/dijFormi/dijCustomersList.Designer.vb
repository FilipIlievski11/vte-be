<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijCustomersList
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dijCustomersList))
        Me.CustomersListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.CustomersListGridControl = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colMB = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerSurname = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerFirstName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colLivingAddressNumber = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsCompany = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCityName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCountryName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colStreetName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.HyperLinkEditName = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
        Me.colAddressOfLiving = New DevExpress.XtraGrid.Columns.GridColumn
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.btnSelect = New DevExpress.XtraEditors.SimpleButton
        Me.btnExit = New DevExpress.XtraEditors.SimpleButton
        Me.btnNew = New DevExpress.XtraEditors.SimpleButton
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        CType(Me.CustomersListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CustomersListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HyperLinkEditName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CustomersListBindingSource
        '
        Me.CustomersListBindingSource.DataSource = GetType(VTE.Library.CustomersListShort)
        '
        'CustomersListGridControl
        '
        Me.CustomersListGridControl.AccessibleDescription = Nothing
        Me.CustomersListGridControl.AccessibleName = Nothing
        resources.ApplyResources(Me.CustomersListGridControl, "CustomersListGridControl")
        Me.CustomersListGridControl.BackgroundImage = Nothing
        Me.CustomersListGridControl.DataSource = Me.CustomersListBindingSource
        Me.CustomersListGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.CustomersListGridControl.EmbeddedNavigator.AccessibleName = Nothing
        Me.CustomersListGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("CustomersListGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.CustomersListGridControl.EmbeddedNavigator.BackgroundImage = Nothing
        Me.CustomersListGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("CustomersListGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.CustomersListGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("CustomersListGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.CustomersListGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("CustomersListGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.CustomersListGridControl.EmbeddedNavigator.ToolTip = resources.GetString("CustomersListGridControl.EmbeddedNavigator.ToolTip")
        Me.CustomersListGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("CustomersListGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.CustomersListGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("CustomersListGridControl.EmbeddedNavigator.ToolTipTitle")
        Me.CustomersListGridControl.Font = Nothing
        Me.CustomersListGridControl.MainView = Me.GridView1
        Me.CustomersListGridControl.Name = "CustomersListGridControl"
        Me.CustomersListGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.HyperLinkEditName})
        Me.CustomersListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
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
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colMB, Me.colCustomerSurname, Me.colCustomerFirstName, Me.colLivingAddressNumber, Me.colIsCompany, Me.colCityName, Me.colCountryName, Me.colStreetName, Me.colName, Me.colAddressOfLiving})
        Me.GridView1.GridControl = Me.CustomersListGridControl
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ShowAutoFilterRow = True
        Me.GridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways
        Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colId, DevExpress.Data.ColumnSortOrder.Descending)})
        '
        'colId
        '
        resources.ApplyResources(Me.colId, "colId")
        Me.colId.FieldName = "Id"
        Me.colId.Name = "colId"
        Me.colId.OptionsColumn.ReadOnly = True
        '
        'colMB
        '
        resources.ApplyResources(Me.colMB, "colMB")
        Me.colMB.FieldName = "MB"
        Me.colMB.MinWidth = 191
        Me.colMB.Name = "colMB"
        Me.colMB.OptionsColumn.AllowEdit = False
        Me.colMB.OptionsColumn.AllowFocus = False
        Me.colMB.OptionsColumn.ReadOnly = True
        Me.colMB.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
        '
        'colCustomerSurname
        '
        resources.ApplyResources(Me.colCustomerSurname, "colCustomerSurname")
        Me.colCustomerSurname.FieldName = "CustomerSurname"
        Me.colCustomerSurname.Name = "colCustomerSurname"
        Me.colCustomerSurname.OptionsColumn.ReadOnly = True
        '
        'colCustomerFirstName
        '
        resources.ApplyResources(Me.colCustomerFirstName, "colCustomerFirstName")
        Me.colCustomerFirstName.FieldName = "CustomerFirstName"
        Me.colCustomerFirstName.Name = "colCustomerFirstName"
        Me.colCustomerFirstName.OptionsColumn.ReadOnly = True
        '
        'colLivingAddressNumber
        '
        resources.ApplyResources(Me.colLivingAddressNumber, "colLivingAddressNumber")
        Me.colLivingAddressNumber.FieldName = "LivingAddressNumber"
        Me.colLivingAddressNumber.Name = "colLivingAddressNumber"
        Me.colLivingAddressNumber.OptionsColumn.ReadOnly = True
        '
        'colIsCompany
        '
        resources.ApplyResources(Me.colIsCompany, "colIsCompany")
        Me.colIsCompany.FieldName = "IsCompany"
        Me.colIsCompany.MinWidth = 96
        Me.colIsCompany.Name = "colIsCompany"
        Me.colIsCompany.OptionsColumn.AllowEdit = False
        Me.colIsCompany.OptionsColumn.AllowFocus = False
        Me.colIsCompany.OptionsColumn.ReadOnly = True
        '
        'colCityName
        '
        resources.ApplyResources(Me.colCityName, "colCityName")
        Me.colCityName.FieldName = "CityName"
        Me.colCityName.Name = "colCityName"
        Me.colCityName.OptionsColumn.ReadOnly = True
        '
        'colCountryName
        '
        resources.ApplyResources(Me.colCountryName, "colCountryName")
        Me.colCountryName.FieldName = "CountryName"
        Me.colCountryName.Name = "colCountryName"
        Me.colCountryName.OptionsColumn.ReadOnly = True
        '
        'colStreetName
        '
        resources.ApplyResources(Me.colStreetName, "colStreetName")
        Me.colStreetName.FieldName = "StreetName"
        Me.colStreetName.Name = "colStreetName"
        Me.colStreetName.OptionsColumn.ReadOnly = True
        '
        'colName
        '
        resources.ApplyResources(Me.colName, "colName")
        Me.colName.ColumnEdit = Me.HyperLinkEditName
        Me.colName.FieldName = "Name"
        Me.colName.MinWidth = 200
        Me.colName.Name = "colName"
        Me.colName.OptionsColumn.ReadOnly = True
        Me.colName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
        '
        'HyperLinkEditName
        '
        Me.HyperLinkEditName.AccessibleDescription = Nothing
        Me.HyperLinkEditName.AccessibleName = Nothing
        resources.ApplyResources(Me.HyperLinkEditName, "HyperLinkEditName")
        Me.HyperLinkEditName.Mask.AutoComplete = CType(resources.GetObject("HyperLinkEditName.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.HyperLinkEditName.Mask.BeepOnError = CType(resources.GetObject("HyperLinkEditName.Mask.BeepOnError"), Boolean)
        Me.HyperLinkEditName.Mask.EditMask = resources.GetString("HyperLinkEditName.Mask.EditMask")
        Me.HyperLinkEditName.Mask.IgnoreMaskBlank = CType(resources.GetObject("HyperLinkEditName.Mask.IgnoreMaskBlank"), Boolean)
        Me.HyperLinkEditName.Mask.MaskType = CType(resources.GetObject("HyperLinkEditName.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.HyperLinkEditName.Mask.PlaceHolder = CType(resources.GetObject("HyperLinkEditName.Mask.PlaceHolder"), Char)
        Me.HyperLinkEditName.Mask.SaveLiteral = CType(resources.GetObject("HyperLinkEditName.Mask.SaveLiteral"), Boolean)
        Me.HyperLinkEditName.Mask.ShowPlaceHolders = CType(resources.GetObject("HyperLinkEditName.Mask.ShowPlaceHolders"), Boolean)
        Me.HyperLinkEditName.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("HyperLinkEditName.Mask.UseMaskAsDisplayFormat"), Boolean)
        Me.HyperLinkEditName.Name = "HyperLinkEditName"
        '
        'colAddressOfLiving
        '
        resources.ApplyResources(Me.colAddressOfLiving, "colAddressOfLiving")
        Me.colAddressOfLiving.FieldName = "AddressOfLiving"
        Me.colAddressOfLiving.MinWidth = 243
        Me.colAddressOfLiving.Name = "colAddressOfLiving"
        Me.colAddressOfLiving.OptionsColumn.AllowEdit = False
        Me.colAddressOfLiving.OptionsColumn.AllowFocus = False
        Me.colAddressOfLiving.OptionsColumn.ReadOnly = True
        Me.colAddressOfLiving.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
        '
        'LayoutControl1
        '
        Me.LayoutControl1.AccessibleDescription = Nothing
        Me.LayoutControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
        Me.LayoutControl1.BackgroundImage = Nothing
        Me.LayoutControl1.Controls.Add(Me.btnSelect)
        Me.LayoutControl1.Controls.Add(Me.btnExit)
        Me.LayoutControl1.Controls.Add(Me.btnNew)
        Me.LayoutControl1.Controls.Add(Me.CustomersListGridControl)
        Me.LayoutControl1.Font = Nothing
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        '
        'btnSelect
        '
        Me.btnSelect.AccessibleDescription = Nothing
        Me.btnSelect.AccessibleName = Nothing
        resources.ApplyResources(Me.btnSelect, "btnSelect")
        Me.btnSelect.BackgroundImage = Nothing
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.StyleController = Me.LayoutControl1
        '
        'btnExit
        '
        Me.btnExit.AccessibleDescription = Nothing
        Me.btnExit.AccessibleName = Nothing
        resources.ApplyResources(Me.btnExit, "btnExit")
        Me.btnExit.BackgroundImage = Nothing
        Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExit.Name = "btnExit"
        Me.btnExit.StyleController = Me.LayoutControl1
        '
        'btnNew
        '
        Me.btnNew.AccessibleDescription = Nothing
        Me.btnNew.AccessibleName = Nothing
        resources.ApplyResources(Me.btnNew, "btnNew")
        Me.btnNew.BackgroundImage = Nothing
        Me.btnNew.Name = "btnNew"
        Me.btnNew.StyleController = Me.LayoutControl1
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(992, 666)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CustomersListGridControl
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(988, 616)
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.btnNew
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 616)
        Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(0, 46)
        Me.LayoutControlItem2.MinSize = New System.Drawing.Size(50, 46)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(332, 46)
        Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.btnExit
        resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
        Me.LayoutControlItem3.Location = New System.Drawing.Point(649, 616)
        Me.LayoutControlItem3.MaxSize = New System.Drawing.Size(0, 46)
        Me.LayoutControlItem3.MinSize = New System.Drawing.Size(60, 46)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(339, 46)
        Me.LayoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.btnSelect
        resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
        Me.LayoutControlItem4.Location = New System.Drawing.Point(332, 616)
        Me.LayoutControlItem4.MaxSize = New System.Drawing.Size(0, 46)
        Me.LayoutControlItem4.MinSize = New System.Drawing.Size(70, 46)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(317, 46)
        Me.LayoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextToControlDistance = 0
        Me.LayoutControlItem4.TextVisible = False
        '
        'dijCustomersList
        '
        Me.AcceptButton = Me.btnSelect
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnExit
        Me.Controls.Add(Me.LayoutControl1)
        Me.Icon = Nothing
        Me.KeyPreview = True
        Me.Name = "dijCustomersList"
        CType(Me.CustomersListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CustomersListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HyperLinkEditName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
  Friend WithEvents CustomersListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents CustomersListGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colMB As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCustomerSurname As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCustomerFirstName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLivingAddressNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIsCompany As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCityName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCountryName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colStreetName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colAddressOfLiving As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents HyperLinkEditName As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
  Friend WithEvents btnSelect As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnNew As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
End Class
