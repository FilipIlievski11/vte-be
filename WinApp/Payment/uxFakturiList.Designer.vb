<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxFakturiList
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxFakturiList))
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.DateEdit2 = New DevExpress.XtraEditors.DateEdit
    Me.DateEdit1 = New DevExpress.XtraEditors.DateEdit
    Me.btnExit = New DevExpress.XtraEditors.SimpleButton
    Me.btnPrint = New DevExpress.XtraEditors.SimpleButton
    Me.btnShow = New DevExpress.XtraEditors.SimpleButton
    Me.CustomLookUpEdit1 = New VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
    Me.PaymentTypeListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.PayDocListGridControl = New DevExpress.XtraGrid.GridControl
    Me.PayDocListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.colName = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colDocumentNumber = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colPayed = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colCustomer = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colMb = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colShellnumber = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colLastRegistratinNumber = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colDatePay = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colDateRequired = New DevExpress.XtraGrid.Columns.GridColumn
    Me.GridColumn1 = New DevExpress.XtraGrid.Columns.GridColumn
    Me.btnPay = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
    Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
    Me.PrintingSystem1 = New DevExpress.XtraPrinting.PrintingSystem(Me.components)
    Me.PrintableComponentLink1 = New DevExpress.XtraPrinting.PrintableComponentLink(Me.components)
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.DateEdit2.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DateEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DateEdit1.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CustomLookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PaymentTypeListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PayDocListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PayDocListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.btnPay, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'LayoutControl1
    '
    Me.LayoutControl1.Controls.Add(Me.DateEdit2)
    Me.LayoutControl1.Controls.Add(Me.DateEdit1)
    Me.LayoutControl1.Controls.Add(Me.btnExit)
    Me.LayoutControl1.Controls.Add(Me.btnPrint)
    Me.LayoutControl1.Controls.Add(Me.btnShow)
    Me.LayoutControl1.Controls.Add(Me.CustomLookUpEdit1)
    Me.LayoutControl1.Controls.Add(Me.PayDocListGridControl)
    resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    '
    'DateEdit2
    '
    Me.DateEdit2.EditValue = Nothing
    resources.ApplyResources(Me.DateEdit2, "DateEdit2")
    Me.DateEdit2.Name = "DateEdit2"
    Me.DateEdit2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("DateEdit2.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.DateEdit2.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.DateEdit2.StyleController = Me.LayoutControl1
    '
    'DateEdit1
    '
    Me.DateEdit1.EditValue = Nothing
    resources.ApplyResources(Me.DateEdit1, "DateEdit1")
    Me.DateEdit1.Name = "DateEdit1"
    Me.DateEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("DateEdit1.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.DateEdit1.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.DateEdit1.StyleController = Me.LayoutControl1
    '
    'btnExit
    '
    resources.ApplyResources(Me.btnExit, "btnExit")
    Me.btnExit.Name = "btnExit"
    Me.btnExit.StyleController = Me.LayoutControl1
    '
    'btnPrint
    '
    resources.ApplyResources(Me.btnPrint, "btnPrint")
    Me.btnPrint.Name = "btnPrint"
    Me.btnPrint.StyleController = Me.LayoutControl1
    '
    'btnShow
    '
    resources.ApplyResources(Me.btnShow, "btnShow")
    Me.btnShow.Name = "btnShow"
    Me.btnShow.StyleController = Me.LayoutControl1
    '
    'CustomLookUpEdit1
    '
    resources.ApplyResources(Me.CustomLookUpEdit1, "CustomLookUpEdit1")
    Me.CustomLookUpEdit1.Name = "CustomLookUpEdit1"
    Me.CustomLookUpEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("CustomLookUpEdit1.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.CustomLookUpEdit1.Properties.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IdCompany", "IdCompany", 61, DevExpress.Utils.FormatType.Numeric, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name", 33, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, True, DevExpress.Utils.HorzAlignment.Near), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FiskalnaKes", "FiskalnaKes", 61, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FiskalnaKarticka", "FiskalnaKarticka", 82, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Rati", "Rati", 25, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Smetka", "Smetka", 41, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Faktura", "Faktura", 43, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("PrintText", "PrintText", 50, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Prefix", "Prefix", 34, DevExpress.Utils.FormatType.None, Global.WinApp.My.Resources.Resources.String1, False, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None)})
    Me.CustomLookUpEdit1.Properties.DataSource = Me.PaymentTypeListBindingSource
    Me.CustomLookUpEdit1.Properties.DisplayMember = "Name"
    Me.CustomLookUpEdit1.Properties.ValueMember = "Id"
    Me.CustomLookUpEdit1.StyleController = Me.LayoutControl1
    '
    'PaymentTypeListBindingSource
    '
    Me.PaymentTypeListBindingSource.DataSource = GetType(VTE.Library.PaymentTypeList)
    '
    'PayDocListGridControl
    '
    Me.PayDocListGridControl.DataSource = Me.PayDocListBindingSource
    resources.ApplyResources(Me.PayDocListGridControl, "PayDocListGridControl")
    Me.PayDocListGridControl.MainView = Me.GridView1
    Me.PayDocListGridControl.Name = "PayDocListGridControl"
    Me.PayDocListGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.btnPay})
    Me.PayDocListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
    '
    'PayDocListBindingSource
    '
    Me.PayDocListBindingSource.DataSource = GetType(VTE.Library.PayDocInfo)
    '
    'GridView1
    '
    Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colName, Me.colDocumentNumber, Me.colPayed, Me.colCustomer, Me.colMb, Me.colShellnumber, Me.colLastRegistratinNumber, Me.colDatePay, Me.colDateRequired, Me.GridColumn1, Me.colId})
    Me.GridView1.GridControl = Me.PayDocListGridControl
    Me.GridView1.Name = "GridView1"
    Me.GridView1.OptionsView.ShowAutoFilterRow = True
    '
    'colName
    '
    resources.ApplyResources(Me.colName, "colName")
    Me.colName.FieldName = "Name"
    Me.colName.Name = "colName"
    Me.colName.OptionsColumn.ReadOnly = True
    '
    'colDocumentNumber
    '
    resources.ApplyResources(Me.colDocumentNumber, "colDocumentNumber")
    Me.colDocumentNumber.FieldName = "DocumentNumber"
    Me.colDocumentNumber.Name = "colDocumentNumber"
    Me.colDocumentNumber.OptionsColumn.ReadOnly = True
    '
    'colPayed
    '
    resources.ApplyResources(Me.colPayed, "colPayed")
    Me.colPayed.FieldName = "Payed"
    Me.colPayed.Name = "colPayed"
    Me.colPayed.OptionsColumn.ReadOnly = True
    '
    'colCustomer
    '
    resources.ApplyResources(Me.colCustomer, "colCustomer")
    Me.colCustomer.FieldName = "Customer"
    Me.colCustomer.Name = "colCustomer"
    Me.colCustomer.OptionsColumn.ReadOnly = True
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
    'colLastRegistratinNumber
    '
    resources.ApplyResources(Me.colLastRegistratinNumber, "colLastRegistratinNumber")
    Me.colLastRegistratinNumber.FieldName = "LastRegistratinNumber"
    Me.colLastRegistratinNumber.Name = "colLastRegistratinNumber"
    Me.colLastRegistratinNumber.OptionsColumn.ReadOnly = True
    '
    'colDatePay
    '
    resources.ApplyResources(Me.colDatePay, "colDatePay")
    Me.colDatePay.FieldName = "DatePay"
    Me.colDatePay.Name = "colDatePay"
    Me.colDatePay.OptionsColumn.ReadOnly = True
    '
    'colDateRequired
    '
    resources.ApplyResources(Me.colDateRequired, "colDateRequired")
    Me.colDateRequired.FieldName = "DateRequired"
    Me.colDateRequired.Name = "colDateRequired"
    Me.colDateRequired.OptionsColumn.ReadOnly = True
    '
    'GridColumn1
    '
    Me.GridColumn1.ColumnEdit = Me.btnPay
    Me.GridColumn1.Name = "GridColumn1"
    resources.ApplyResources(Me.GridColumn1, "GridColumn1")
    '
    'btnPay
    '
    Me.btnPay.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("btnPay.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines), resources.GetString("btnPay.Buttons1"), CType(resources.GetObject("btnPay.Buttons2"), Integer), CType(resources.GetObject("btnPay.Buttons3"), Boolean), CType(resources.GetObject("btnPay.Buttons4"), Boolean), CType(resources.GetObject("btnPay.Buttons5"), Boolean), CType(resources.GetObject("btnPay.Buttons6"), DevExpress.XtraEditors.ImageLocation), Nothing)})
    Me.btnPay.Name = "btnPay"
    Me.btnPay.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
    '
    'colId
    '
    Me.colId.FieldName = "Id"
    Me.colId.Name = "colId"
    '
    'GridView2
    '
    Me.GridView2.GridControl = Me.PayDocListGridControl
    Me.GridView2.Name = "GridView2"
    '
    'LayoutControlGroup1
    '
    resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem5, Me.LayoutControlItem6, Me.LayoutControlItem4, Me.LayoutControlItem7})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(779, 461)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.TextVisible = False
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.PayDocListGridControl
    resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 33)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(775, 391)
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem1.TextToControlDistance = 0
    Me.LayoutControlItem1.TextVisible = False
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.CustomLookUpEdit1
    resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
    Me.LayoutControlItem2.Location = New System.Drawing.Point(284, 0)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(250, 33)
    Me.LayoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(67, 20)
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.btnShow
    resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
    Me.LayoutControlItem3.Location = New System.Drawing.Point(534, 0)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(241, 33)
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem3.TextToControlDistance = 0
    Me.LayoutControlItem3.TextVisible = False
    '
    'LayoutControlItem5
    '
    Me.LayoutControlItem5.Control = Me.btnPrint
    resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
    Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 424)
    Me.LayoutControlItem5.Name = "LayoutControlItem5"
    Me.LayoutControlItem5.Size = New System.Drawing.Size(378, 33)
    Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem5.TextToControlDistance = 0
    Me.LayoutControlItem5.TextVisible = False
    '
    'LayoutControlItem6
    '
    Me.LayoutControlItem6.Control = Me.btnExit
    resources.ApplyResources(Me.LayoutControlItem6, "LayoutControlItem6")
    Me.LayoutControlItem6.Location = New System.Drawing.Point(378, 424)
    Me.LayoutControlItem6.Name = "LayoutControlItem6"
    Me.LayoutControlItem6.Size = New System.Drawing.Size(397, 33)
    Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem6.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem6.TextToControlDistance = 0
    Me.LayoutControlItem6.TextVisible = False
    '
    'LayoutControlItem4
    '
    Me.LayoutControlItem4.Control = Me.DateEdit1
    resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
    Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem4.Name = "LayoutControlItem4"
    Me.LayoutControlItem4.Size = New System.Drawing.Size(145, 33)
    Me.LayoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem4.TextSize = New System.Drawing.Size(24, 20)
    '
    'LayoutControlItem7
    '
    Me.LayoutControlItem7.Control = Me.DateEdit2
    resources.ApplyResources(Me.LayoutControlItem7, "LayoutControlItem7")
    Me.LayoutControlItem7.Location = New System.Drawing.Point(145, 0)
    Me.LayoutControlItem7.Name = "LayoutControlItem7"
    Me.LayoutControlItem7.Size = New System.Drawing.Size(139, 33)
    Me.LayoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.LayoutControlItem7.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem7.TextSize = New System.Drawing.Size(12, 20)
    '
    'PrintingSystem1
    '
    Me.PrintingSystem1.Links.AddRange(New Object() {Me.PrintableComponentLink1})
    '
    'PrintableComponentLink1
    '
    Me.PrintableComponentLink1.Component = Me.PayDocListGridControl
    Me.PrintableComponentLink1.CustomPaperSize = New System.Drawing.Size(0, 0)
    Me.PrintableComponentLink1.ImageStream = CType(resources.GetObject("PrintableComponentLink1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
    Me.PrintableComponentLink1.PaperKind = System.Drawing.Printing.PaperKind.A4
    Me.PrintableComponentLink1.PrintingSystem = Me.PrintingSystem1
    Me.PrintableComponentLink1.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart
    '
    'uxFakturiList
    '
    resources.ApplyResources(Me, "$this")
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "uxFakturiList"
    Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.DateEdit2.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DateEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DateEdit1.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CustomLookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PaymentTypeListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PayDocListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PayDocListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.btnPay, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PrintingSystem1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnPrint As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnShow As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents CustomLookUpEdit1 As VTE.BaseParts.FancyLookupEdit.CustomLookUpEdit
  Friend WithEvents PayDocListGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents PayDocListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDocumentNumber As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colPayed As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCustomer As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colMb As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colShellnumber As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colLastRegistratinNumber As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDatePay As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDateRequired As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents PaymentTypeListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridColumn1 As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents btnPay As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
  Friend WithEvents PrintingSystem1 As DevExpress.XtraPrinting.PrintingSystem
  Friend WithEvents PrintableComponentLink1 As DevExpress.XtraPrinting.PrintableComponentLink
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents DateEdit2 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents DateEdit1 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem

End Class
