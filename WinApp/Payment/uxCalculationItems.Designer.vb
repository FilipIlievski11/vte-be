<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxCalculationItems
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxCalculationItems))
        Me.IssuersLayoutControl = New DevExpress.XtraLayout.LayoutControl
        Me.CalculationItemsGridControl = New DevExpress.XtraGrid.GridControl
        Me.CalculationItemsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colItemName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colBankAccount = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colBank = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colForm = New DevExpress.XtraGrid.Columns.GridColumn
        Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
        Me.RegistrationIssuersBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
        CType(Me.IssuersLayoutControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.IssuersLayoutControl.SuspendLayout()
        CType(Me.CalculationItemsGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CalculationItemsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RegistrationIssuersBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.IssuersLayoutControl.Controls.Add(Me.CalculationItemsGridControl)
        Me.IssuersLayoutControl.Controls.Add(Me.UxKopcinja1)
        Me.IssuersLayoutControl.Font = Nothing
        Me.IssuersLayoutControl.Name = "IssuersLayoutControl"
        Me.IssuersLayoutControl.Root = Me.LayoutControlGroup1
        '
        'CalculationItemsGridControl
        '
        Me.CalculationItemsGridControl.AccessibleDescription = Nothing
        Me.CalculationItemsGridControl.AccessibleName = Nothing
        resources.ApplyResources(Me.CalculationItemsGridControl, "CalculationItemsGridControl")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.CalculationItemsGridControl, False)
        Me.CalculationItemsGridControl.BackgroundImage = Nothing
        Me.CalculationItemsGridControl.DataSource = Me.CalculationItemsBindingSource
        Me.CalculationItemsGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.CalculationItemsGridControl.EmbeddedNavigator.AccessibleName = Nothing
        Me.CalculationItemsGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("CalculationItemsGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.CalculationItemsGridControl.EmbeddedNavigator.BackgroundImage = Nothing
        Me.CalculationItemsGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("CalculationItemsGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.CalculationItemsGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("CalculationItemsGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.CalculationItemsGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("CalculationItemsGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.CalculationItemsGridControl.EmbeddedNavigator.ToolTip = resources.GetString("CalculationItemsGridControl.EmbeddedNavigator.ToolTip")
        Me.CalculationItemsGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("CalculationItemsGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.CalculationItemsGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("CalculationItemsGridControl.EmbeddedNavigator.ToolTipTitle")
        Me.CalculationItemsGridControl.Font = Nothing
        Me.CalculationItemsGridControl.MainView = Me.GridView1
        Me.CalculationItemsGridControl.Name = "CalculationItemsGridControl"
        Me.CalculationItemsGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'CalculationItemsBindingSource
        '
        Me.CalculationItemsBindingSource.DataSource = GetType(VTE.Library.CalculationItems)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CalculationItemsBindingSource, False)
        '
        'GridView1
        '
        resources.ApplyResources(Me.GridView1, "GridView1")
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colItemName, Me.colBankAccount, Me.colBank, Me.colForm})
        Me.GridView1.GridControl = Me.CalculationItemsGridControl
        Me.GridView1.Name = "GridView1"
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
        'colItemName
        '
        resources.ApplyResources(Me.colItemName, "colItemName")
        Me.colItemName.FieldName = "ItemName"
        Me.colItemName.Name = "colItemName"
        '
        'colBankAccount
        '
        resources.ApplyResources(Me.colBankAccount, "colBankAccount")
        Me.colBankAccount.FieldName = "BankAccount"
        Me.colBankAccount.Name = "colBankAccount"
        '
        'colBank
        '
        resources.ApplyResources(Me.colBank, "colBank")
        Me.colBank.FieldName = "Bank"
        Me.colBank.Name = "colBank"
        '
        'colForm
        '
        resources.ApplyResources(Me.colForm, "colForm")
        Me.colForm.FieldName = "Form"
        Me.colForm.Name = "colForm"
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
        Me.LayoutControlItem1.Size = New System.Drawing.Size(667, 64)
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.CalculationItemsGridControl
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 70)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(667, 449)
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
        Me.SplitterItem1.Size = New System.Drawing.Size(667, 6)
        '
        'RegistrationIssuersBindingSource
        '
        Me.RegistrationIssuersBindingSource.DataSource = GetType(VTE.Library.RegistrationIssuers)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.RegistrationIssuersBindingSource, False)
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        Me.DxErrorProvider1.DataSource = Me.CalculationItemsBindingSource
        '
        'uxCalculationItems
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.Controls.Add(Me.IssuersLayoutControl)
        Me.Name = "uxCalculationItems"
        Me.Controls.SetChildIndex(Me.IssuersLayoutControl, 0)
        CType(Me.IssuersLayoutControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.IssuersLayoutControl.ResumeLayout(False)
        CType(Me.CalculationItemsGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CalculationItemsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RegistrationIssuersBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
  Friend WithEvents IssuersLayoutControl As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents CalculationItemsGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents RegistrationIssuersBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents CalculationItemsBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colItemName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colBankAccount As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colBank As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colForm As DevExpress.XtraGrid.Columns.GridColumn

End Class
