<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxUnpayedDealsList
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxUnpayedDealsList))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.btnPrint = New DevExpress.XtraEditors.SimpleButton
        Me.btnExit = New DevExpress.XtraEditors.SimpleButton
        Me.btnEdit = New DevExpress.XtraEditors.SimpleButton
        Me.UnpayedDealsListGridControl = New DevExpress.XtraGrid.GridControl
        Me.UnpayedDealsListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colRati = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDocumentnumber = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colPayed = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomersurname = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomerfirstname = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colMb = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colShellnumber = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colCustomer = New DevExpress.XtraGrid.Columns.GridColumn
        Me.HyperLinkCustomer = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
        Me.colOstanatoZaPlakanje = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDatePay = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colPhoneNumber = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        Me.PrintingSystem1 = New DevExpress.XtraPrinting.PrintingSystem(Me.components)
        Me.PrintableComponentLink1 = New DevExpress.XtraPrinting.PrintableComponentLink(Me.components)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.UnpayedDealsListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UnpayedDealsListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HyperLinkCustomer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.AccessibleDescription = Nothing
        Me.LayoutControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
        Me.LayoutControl1.BackgroundImage = Nothing
        Me.LayoutControl1.Controls.Add(Me.btnPrint)
        Me.LayoutControl1.Controls.Add(Me.btnExit)
        Me.LayoutControl1.Controls.Add(Me.btnEdit)
        Me.LayoutControl1.Controls.Add(Me.UnpayedDealsListGridControl)
        Me.LayoutControl1.Font = Nothing
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        '
        'btnPrint
        '
        Me.btnPrint.AccessibleDescription = Nothing
        Me.btnPrint.AccessibleName = Nothing
        resources.ApplyResources(Me.btnPrint, "btnPrint")
        Me.btnPrint.BackgroundImage = Nothing
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.StyleController = Me.LayoutControl1
        '
        'btnExit
        '
        Me.btnExit.AccessibleDescription = Nothing
        Me.btnExit.AccessibleName = Nothing
        resources.ApplyResources(Me.btnExit, "btnExit")
        Me.btnExit.BackgroundImage = Nothing
        Me.btnExit.Name = "btnExit"
        Me.btnExit.StyleController = Me.LayoutControl1
        '
        'btnEdit
        '
        Me.btnEdit.AccessibleDescription = Nothing
        Me.btnEdit.AccessibleName = Nothing
        resources.ApplyResources(Me.btnEdit, "btnEdit")
        Me.btnEdit.BackgroundImage = Nothing
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.StyleController = Me.LayoutControl1
        '
        'UnpayedDealsListGridControl
        '
        Me.UnpayedDealsListGridControl.AccessibleDescription = Nothing
        Me.UnpayedDealsListGridControl.AccessibleName = Nothing
        resources.ApplyResources(Me.UnpayedDealsListGridControl, "UnpayedDealsListGridControl")
        Me.UnpayedDealsListGridControl.BackgroundImage = Nothing
        Me.UnpayedDealsListGridControl.DataSource = Me.UnpayedDealsListBindingSource
        Me.UnpayedDealsListGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.UnpayedDealsListGridControl.EmbeddedNavigator.AccessibleName = Nothing
        Me.UnpayedDealsListGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("UnpayedDealsListGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.UnpayedDealsListGridControl.EmbeddedNavigator.BackgroundImage = Nothing
        Me.UnpayedDealsListGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("UnpayedDealsListGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.UnpayedDealsListGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("UnpayedDealsListGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.UnpayedDealsListGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("UnpayedDealsListGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.UnpayedDealsListGridControl.EmbeddedNavigator.ToolTip = resources.GetString("UnpayedDealsListGridControl.EmbeddedNavigator.ToolTip")
        Me.UnpayedDealsListGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("UnpayedDealsListGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.UnpayedDealsListGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("UnpayedDealsListGridControl.EmbeddedNavigator.ToolTipTitle")
        Me.UnpayedDealsListGridControl.Font = Nothing
        Me.UnpayedDealsListGridControl.MainView = Me.GridView1
        Me.UnpayedDealsListGridControl.Name = "UnpayedDealsListGridControl"
        Me.UnpayedDealsListGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.HyperLinkCustomer})
        Me.UnpayedDealsListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
        '
        'UnpayedDealsListBindingSource
        '
        Me.UnpayedDealsListBindingSource.DataSource = GetType(VTE.Library.UnpayedDealsInfo)
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
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colRati, Me.colId, Me.colDocumentnumber, Me.colPayed, Me.colCustomersurname, Me.colCustomerfirstname, Me.colMb, Me.colShellnumber, Me.colCustomer, Me.colOstanatoZaPlakanje, Me.colDatePay, Me.colPhoneNumber})
        Me.GridView1.GridControl = Me.UnpayedDealsListGridControl
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ShowAutoFilterRow = True
        Me.GridView1.OptionsView.ShowFooter = True
        '
        'colRati
        '
        resources.ApplyResources(Me.colRati, "colRati")
        Me.colRati.FieldName = "Rati"
        Me.colRati.Name = "colRati"
        Me.colRati.OptionsColumn.ReadOnly = True
        '
        'colId
        '
        resources.ApplyResources(Me.colId, "colId")
        Me.colId.FieldName = "Id"
        Me.colId.Name = "colId"
        Me.colId.OptionsColumn.ReadOnly = True
        Me.colId.SummaryItem.DisplayFormat = resources.GetString("colId.SummaryItem.DisplayFormat")
        Me.colId.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
        '
        'colDocumentnumber
        '
        resources.ApplyResources(Me.colDocumentnumber, "colDocumentnumber")
        Me.colDocumentnumber.FieldName = "Documentnumber"
        Me.colDocumentnumber.Name = "colDocumentnumber"
        Me.colDocumentnumber.OptionsColumn.ReadOnly = True
        Me.colDocumentnumber.SummaryItem.DisplayFormat = resources.GetString("colDocumentnumber.SummaryItem.DisplayFormat")
        Me.colDocumentnumber.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
        '
        'colPayed
        '
        resources.ApplyResources(Me.colPayed, "colPayed")
        Me.colPayed.FieldName = "Payed"
        Me.colPayed.Name = "colPayed"
        Me.colPayed.OptionsColumn.ReadOnly = True
        '
        'colCustomersurname
        '
        resources.ApplyResources(Me.colCustomersurname, "colCustomersurname")
        Me.colCustomersurname.FieldName = "Customersurname"
        Me.colCustomersurname.Name = "colCustomersurname"
        Me.colCustomersurname.OptionsColumn.ReadOnly = True
        '
        'colCustomerfirstname
        '
        resources.ApplyResources(Me.colCustomerfirstname, "colCustomerfirstname")
        Me.colCustomerfirstname.FieldName = "Customerfirstname"
        Me.colCustomerfirstname.Name = "colCustomerfirstname"
        Me.colCustomerfirstname.OptionsColumn.ReadOnly = True
        '
        'colMb
        '
        resources.ApplyResources(Me.colMb, "colMb")
        Me.colMb.FieldName = "Mb"
        Me.colMb.Name = "colMb"
        Me.colMb.OptionsColumn.ReadOnly = True
        '
        'colShellnumber
        '
        resources.ApplyResources(Me.colShellnumber, "colShellnumber")
        Me.colShellnumber.FieldName = "Shellnumber"
        Me.colShellnumber.Name = "colShellnumber"
        Me.colShellnumber.OptionsColumn.ReadOnly = True
        '
        'colCustomer
        '
        Me.colCustomer.AppearanceCell.Options.UseTextOptions = True
        Me.colCustomer.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        resources.ApplyResources(Me.colCustomer, "colCustomer")
        Me.colCustomer.ColumnEdit = Me.HyperLinkCustomer
        Me.colCustomer.FieldName = "Customer"
        Me.colCustomer.Name = "colCustomer"
        Me.colCustomer.OptionsColumn.ReadOnly = True
        Me.colCustomer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
        '
        'HyperLinkCustomer
        '
        Me.HyperLinkCustomer.AccessibleDescription = Nothing
        Me.HyperLinkCustomer.AccessibleName = Nothing
        resources.ApplyResources(Me.HyperLinkCustomer, "HyperLinkCustomer")
        Me.HyperLinkCustomer.Mask.AutoComplete = CType(resources.GetObject("HyperLinkCustomer.Mask.AutoComplete"), DevExpress.XtraEditors.Mask.AutoCompleteType)
        Me.HyperLinkCustomer.Mask.BeepOnError = CType(resources.GetObject("HyperLinkCustomer.Mask.BeepOnError"), Boolean)
        Me.HyperLinkCustomer.Mask.EditMask = resources.GetString("HyperLinkCustomer.Mask.EditMask")
        Me.HyperLinkCustomer.Mask.IgnoreMaskBlank = CType(resources.GetObject("HyperLinkCustomer.Mask.IgnoreMaskBlank"), Boolean)
        Me.HyperLinkCustomer.Mask.MaskType = CType(resources.GetObject("HyperLinkCustomer.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.HyperLinkCustomer.Mask.PlaceHolder = CType(resources.GetObject("HyperLinkCustomer.Mask.PlaceHolder"), Char)
        Me.HyperLinkCustomer.Mask.SaveLiteral = CType(resources.GetObject("HyperLinkCustomer.Mask.SaveLiteral"), Boolean)
        Me.HyperLinkCustomer.Mask.ShowPlaceHolders = CType(resources.GetObject("HyperLinkCustomer.Mask.ShowPlaceHolders"), Boolean)
        Me.HyperLinkCustomer.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("HyperLinkCustomer.Mask.UseMaskAsDisplayFormat"), Boolean)
        Me.HyperLinkCustomer.Name = "HyperLinkCustomer"
        '
        'colOstanatoZaPlakanje
        '
        resources.ApplyResources(Me.colOstanatoZaPlakanje, "colOstanatoZaPlakanje")
        Me.colOstanatoZaPlakanje.FieldName = "OstanatoZaPlakanje"
        Me.colOstanatoZaPlakanje.Name = "colOstanatoZaPlakanje"
        Me.colOstanatoZaPlakanje.SummaryItem.DisplayFormat = resources.GetString("colOstanatoZaPlakanje.SummaryItem.DisplayFormat")
        Me.colOstanatoZaPlakanje.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
        '
        'colDatePay
        '
        resources.ApplyResources(Me.colDatePay, "colDatePay")
        Me.colDatePay.FieldName = "DatePay"
        Me.colDatePay.Name = "colDatePay"
        '
        'colPhoneNumber
        '
        resources.ApplyResources(Me.colPhoneNumber, "colPhoneNumber")
        Me.colPhoneNumber.FieldName = "PhoneNumber"
        Me.colPhoneNumber.Name = "colPhoneNumber"
        '
        'GridView2
        '
        resources.ApplyResources(Me.GridView2, "GridView2")
        Me.GridView2.GridControl = Me.UnpayedDealsListGridControl
        Me.GridView2.Name = "GridView2"
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(613, 426)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.UnpayedDealsListGridControl
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(611, 391)
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.btnEdit
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 391)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(196, 33)
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.btnExit
        resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
        Me.LayoutControlItem3.Location = New System.Drawing.Point(396, 391)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(215, 33)
        Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.btnPrint
        resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
        Me.LayoutControlItem4.Location = New System.Drawing.Point(196, 391)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(200, 33)
        Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextToControlDistance = 0
        Me.LayoutControlItem4.TextVisible = False
        '
        'PrintingSystem1
        '
        Me.PrintingSystem1.ExportOptions.Csv.EncodingType = CType(resources.GetObject("PrintingSystem1.ExportOptions.Csv.EncodingType"), DevExpress.XtraPrinting.EncodingType)
        Me.PrintingSystem1.ExportOptions.Html.CharacterSet = resources.GetString("PrintingSystem1.ExportOptions.Html.CharacterSet")
        Me.PrintingSystem1.ExportOptions.Html.Title = resources.GetString("PrintingSystem1.ExportOptions.Html.Title")
        Me.PrintingSystem1.ExportOptions.Mht.CharacterSet = resources.GetString("PrintingSystem1.ExportOptions.Mht.CharacterSet")
        Me.PrintingSystem1.ExportOptions.Mht.Title = resources.GetString("PrintingSystem1.ExportOptions.Mht.Title")
        Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Application = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Application")
        Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Author = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Author")
        Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Keywords = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Keywords")
        Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Subject = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Subject")
        Me.PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Title = resources.GetString("PrintingSystem1.ExportOptions.Pdf.DocumentOptions.Title")
        Me.PrintingSystem1.ExportOptions.Text.EncodingType = CType(resources.GetObject("PrintingSystem1.ExportOptions.Text.EncodingType"), DevExpress.XtraPrinting.EncodingType)
        Me.PrintingSystem1.ExportOptions.Xls.SheetName = resources.GetString("PrintingSystem1.ExportOptions.Xls.SheetName")
        Me.PrintingSystem1.Links.AddRange(New Object() {Me.PrintableComponentLink1})
        '
        'PrintableComponentLink1
        '
        Me.PrintableComponentLink1.Component = Me.UnpayedDealsListGridControl
        Me.PrintableComponentLink1.CustomPaperSize = New System.Drawing.Size(0, 0)
        Me.PrintableComponentLink1.ImageStream = CType(resources.GetObject("PrintableComponentLink1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.PrintableComponentLink1.Margins = New System.Drawing.Printing.Margins(30, 30, 30, 30)
        Me.PrintableComponentLink1.PaperKind = System.Drawing.Printing.PaperKind.A4
        Me.PrintableComponentLink1.PrintingSystem = Me.PrintingSystem1
        Me.PrintableComponentLink1.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart
        '
        'uxUnpayedDealsList
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "uxUnpayedDealsList"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.UnpayedDealsListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UnpayedDealsListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HyperLinkCustomer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents UnpayedDealsListGridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents UnpayedDealsListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colRati As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDocumentnumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPayed As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomersurname As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCustomerfirstname As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMb As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colShellnumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents colCustomer As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents HyperLinkCustomer As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Friend WithEvents btnPrint As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents PrintingSystem1 As DevExpress.XtraPrinting.PrintingSystem
    Friend WithEvents PrintableComponentLink1 As DevExpress.XtraPrinting.PrintableComponentLink
    Friend WithEvents colOstanatoZaPlakanje As DevExpress.XtraGrid.Columns.GridColumn
 Friend WithEvents colDatePay As DevExpress.XtraGrid.Columns.GridColumn
 Friend WithEvents colPhoneNumber As DevExpress.XtraGrid.Columns.GridColumn

End Class
