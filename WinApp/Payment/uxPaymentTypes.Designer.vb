<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxPaymentTypes
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxPaymentTypes))
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.btnNuliranjeZapisnici = New DevExpress.XtraEditors.SimpleButton
  Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
  Me.PaymentTypesGridControl = New DevExpress.XtraGrid.GridControl
  Me.PaymentTypesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colFiskalnaKes = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colFiskalnaKarticka = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colRati = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colSmetka = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colFaktura = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colPrintText = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colPrefix = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colPayedAmount = New DevExpress.XtraGrid.Columns.GridColumn
  Me.GridColumn1 = New DevExpress.XtraGrid.Columns.GridColumn
  Me.btnNuliranje = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
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
  CType(Me.PaymentTypesGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.PaymentTypesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.btnNuliranje, System.ComponentModel.ISupportInitialize).BeginInit()
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
  Me.LayoutControl1.AccessibleDescription = Nothing
  Me.LayoutControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LayoutControl1, False)
  Me.LayoutControl1.BackgroundImage = Nothing
  Me.LayoutControl1.Controls.Add(Me.btnNuliranjeZapisnici)
  Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
  Me.LayoutControl1.Controls.Add(Me.PaymentTypesGridControl)
  Me.LayoutControl1.Font = Nothing
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
  '
  'btnNuliranjeZapisnici
  '
  Me.btnNuliranjeZapisnici.AccessibleDescription = Nothing
  Me.btnNuliranjeZapisnici.AccessibleName = Nothing
  resources.ApplyResources(Me.btnNuliranjeZapisnici, "btnNuliranjeZapisnici")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.btnNuliranjeZapisnici, False)
  Me.btnNuliranjeZapisnici.BackgroundImage = Nothing
  Me.btnNuliranjeZapisnici.Name = "btnNuliranjeZapisnici"
  Me.btnNuliranjeZapisnici.StyleController = Me.LayoutControl1
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
  'PaymentTypesGridControl
  '
  Me.PaymentTypesGridControl.AccessibleDescription = Nothing
  Me.PaymentTypesGridControl.AccessibleName = Nothing
  resources.ApplyResources(Me.PaymentTypesGridControl, "PaymentTypesGridControl")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.PaymentTypesGridControl, False)
  Me.PaymentTypesGridControl.BackgroundImage = Nothing
  Me.PaymentTypesGridControl.DataSource = Me.PaymentTypesBindingSource
  Me.PaymentTypesGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
  Me.PaymentTypesGridControl.EmbeddedNavigator.AccessibleName = Nothing
  Me.PaymentTypesGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("PaymentTypesGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
  Me.PaymentTypesGridControl.EmbeddedNavigator.BackgroundImage = Nothing
  Me.PaymentTypesGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("PaymentTypesGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
  Me.PaymentTypesGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("PaymentTypesGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
  Me.PaymentTypesGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("PaymentTypesGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
  Me.PaymentTypesGridControl.EmbeddedNavigator.ToolTip = resources.GetString("PaymentTypesGridControl.EmbeddedNavigator.ToolTip")
  Me.PaymentTypesGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("PaymentTypesGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
  Me.PaymentTypesGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("PaymentTypesGridControl.EmbeddedNavigator.ToolTipTitle")
  Me.PaymentTypesGridControl.Font = Nothing
  Me.PaymentTypesGridControl.MainView = Me.GridView1
  Me.PaymentTypesGridControl.Name = "PaymentTypesGridControl"
  Me.PaymentTypesGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.btnNuliranje})
  Me.PaymentTypesGridControl.UseEmbeddedNavigator = True
  Me.PaymentTypesGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'PaymentTypesBindingSource
  '
  Me.PaymentTypesBindingSource.DataSource = GetType(VTE.Library.PaymentType)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.PaymentTypesBindingSource, False)
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
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colName, Me.colFiskalnaKes, Me.colFiskalnaKarticka, Me.colRati, Me.colSmetka, Me.colFaktura, Me.colPrintText, Me.colPrefix, Me.colPayedAmount, Me.GridColumn1})
  Me.GridView1.GridControl = Me.PaymentTypesGridControl
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
  'colName
  '
  resources.ApplyResources(Me.colName, "colName")
  Me.colName.FieldName = "Name"
  Me.colName.Name = "colName"
  '
  'colFiskalnaKes
  '
  resources.ApplyResources(Me.colFiskalnaKes, "colFiskalnaKes")
  Me.colFiskalnaKes.FieldName = "FiskalnaKes"
  Me.colFiskalnaKes.Name = "colFiskalnaKes"
  '
  'colFiskalnaKarticka
  '
  resources.ApplyResources(Me.colFiskalnaKarticka, "colFiskalnaKarticka")
  Me.colFiskalnaKarticka.FieldName = "FiskalnaKarticka"
  Me.colFiskalnaKarticka.Name = "colFiskalnaKarticka"
  '
  'colRati
  '
  resources.ApplyResources(Me.colRati, "colRati")
  Me.colRati.FieldName = "Rati"
  Me.colRati.Name = "colRati"
  '
  'colSmetka
  '
  resources.ApplyResources(Me.colSmetka, "colSmetka")
  Me.colSmetka.FieldName = "Smetka"
  Me.colSmetka.Name = "colSmetka"
  '
  'colFaktura
  '
  resources.ApplyResources(Me.colFaktura, "colFaktura")
  Me.colFaktura.FieldName = "Faktura"
  Me.colFaktura.Name = "colFaktura"
  '
  'colPrintText
  '
  resources.ApplyResources(Me.colPrintText, "colPrintText")
  Me.colPrintText.FieldName = "PrintText"
  Me.colPrintText.Name = "colPrintText"
  '
  'colPrefix
  '
  resources.ApplyResources(Me.colPrefix, "colPrefix")
  Me.colPrefix.FieldName = "Prefix"
  Me.colPrefix.Name = "colPrefix"
  '
  'colPayedAmount
  '
  resources.ApplyResources(Me.colPayedAmount, "colPayedAmount")
  Me.colPayedAmount.FieldName = "PayedAmount"
  Me.colPayedAmount.Name = "colPayedAmount"
  '
  'GridColumn1
  '
  resources.ApplyResources(Me.GridColumn1, "GridColumn1")
  Me.GridColumn1.ColumnEdit = Me.btnNuliranje
  Me.GridColumn1.FieldName = "GridColumn1"
  Me.GridColumn1.Name = "GridColumn1"
  Me.GridColumn1.UnboundType = DevExpress.Data.UnboundColumnType.[Object]
  '
  'btnNuliranje
  '
  Me.btnNuliranje.AccessibleDescription = Nothing
  Me.btnNuliranje.AccessibleName = Nothing
  resources.ApplyResources(Me.btnNuliranje, "btnNuliranje")
  Me.btnNuliranje.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("btnNuliranje.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("btnNuliranje.Buttons1"), CType(resources.GetObject("btnNuliranje.Buttons2"), Integer), CType(resources.GetObject("btnNuliranje.Buttons3"), Boolean), CType(resources.GetObject("btnNuliranje.Buttons4"), Boolean), CType(resources.GetObject("btnNuliranje.Buttons5"), Boolean), CType(resources.GetObject("btnNuliranje.Buttons6"), DevExpress.XtraEditors.ImageLocation), Nothing)})
  Me.btnNuliranje.Mask.AutoComplete = CType(resources.GetObject("btnNuliranje.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
  Me.btnNuliranje.Mask.BeepOnError = CType(resources.GetObject("btnNuliranje.Mask.BeepOnError"), Boolean)
  Me.btnNuliranje.Mask.EditMask = resources.GetString("btnNuliranje.Mask.EditMask")
  Me.btnNuliranje.Mask.IgnoreMaskBlank = CType(resources.GetObject("btnNuliranje.Mask.IgnoreMaskBlank"), Boolean)
  Me.btnNuliranje.Mask.MaskType = CType(resources.GetObject("btnNuliranje.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
  Me.btnNuliranje.Mask.PlaceHolder = CType(resources.GetObject("btnNuliranje.Mask.PlaceHolder"), Char)
  Me.btnNuliranje.Mask.SaveLiteral = CType(resources.GetObject("btnNuliranje.Mask.SaveLiteral"), Boolean)
  Me.btnNuliranje.Mask.ShowPlaceHolders = CType(resources.GetObject("btnNuliranje.Mask.ShowPlaceHolders"), Boolean)
  Me.btnNuliranje.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("btnNuliranje.Mask.UseMaskAsDisplayFormat"), Boolean)
  Me.btnNuliranje.Name = "btnNuliranje"
  Me.btnNuliranje.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
  '
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.SplitterItem1, Me.LayoutControlItem3})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "Root"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(613, 426)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.PaymentTypesGridControl
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 59)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(609, 330)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem1.TextToControlDistance = 0
  Me.LayoutControlItem1.TextVisible = False
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.UxKopcinja1
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(609, 53)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem2.TextToControlDistance = 0
  Me.LayoutControlItem2.TextVisible = False
  '
  'SplitterItem1
  '
  resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
  Me.SplitterItem1.Location = New System.Drawing.Point(0, 53)
  Me.SplitterItem1.Name = "SplitterItem1"
  Me.SplitterItem1.Size = New System.Drawing.Size(609, 6)
  '
  'LayoutControlItem3
  '
  Me.LayoutControlItem3.Control = Me.btnNuliranjeZapisnici
  resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
  Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 389)
  Me.LayoutControlItem3.Name = "LayoutControlItem3"
  Me.LayoutControlItem3.Size = New System.Drawing.Size(609, 33)
  Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem3.TextToControlDistance = 0
  Me.LayoutControlItem3.TextVisible = False
  '
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.PaymentTypesBindingSource
  '
  'uxPaymentTypes
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.LayoutControl1)
  Me.Name = "uxPaymentTypes"
  Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.PaymentTypesGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.PaymentTypesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.btnNuliranje, System.ComponentModel.ISupportInitialize).EndInit()
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
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents PaymentTypesGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents PaymentTypesBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colFiskalnaKes As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colFiskalnaKarticka As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colRati As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colSmetka As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colFaktura As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents colPrintText As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPrefix As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPayedAmount As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btnNuliranje As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents btnNuliranjeZapisnici As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem

End Class
