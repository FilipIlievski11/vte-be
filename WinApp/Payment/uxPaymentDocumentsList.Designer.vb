<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxPaymentDocumentsList
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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.PaymentDocumentsListGridControl = New DevExpress.XtraGrid.GridControl
        Me.PaymentDocumentsListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdPaymentType = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdCustomerVehicleRelation = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdOperator = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDatePay = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDateRequired = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDiscount = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colPayed = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colNote = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDocumentNumber = New DevExpress.XtraGrid.Columns.GridColumn
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.PaymentDocumentsListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PaymentDocumentsListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.PaymentDocumentsListGridControl)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(613, 426)
        Me.LayoutControl1.TabIndex = 1
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'PaymentDocumentsListGridControl
        '
        Me.PaymentDocumentsListGridControl.DataSource = Me.PaymentDocumentsListBindingSource
        Me.PaymentDocumentsListGridControl.Location = New System.Drawing.Point(7, 7)
        Me.PaymentDocumentsListGridControl.MainView = Me.GridView1
        Me.PaymentDocumentsListGridControl.Name = "PaymentDocumentsListGridControl"
        Me.PaymentDocumentsListGridControl.Size = New System.Drawing.Size(600, 413)
        Me.PaymentDocumentsListGridControl.TabIndex = 4
        Me.PaymentDocumentsListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'PaymentDocumentsListBindingSource
        '
        Me.PaymentDocumentsListBindingSource.DataSource = GetType(VTE.Library.PaymentDocumentsInfo)
        '
        'GridView1
        '
        Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
        Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView1.Appearance.Row.Options.UseTextOptions = True
        Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdPaymentType, Me.colIdCustomerVehicleRelation, Me.colIdOperator, Me.colDatePay, Me.colDateRequired, Me.colDiscount, Me.colPayed, Me.colNote, Me.colDocumentNumber})
        Me.GridView1.GridControl = Me.PaymentDocumentsListGridControl
        Me.GridView1.Name = "GridView1"
        '
        'colId
        '
        Me.colId.Caption = "Id"
        Me.colId.FieldName = "Id"
        Me.colId.Name = "colId"
        Me.colId.OptionsColumn.ReadOnly = True
        Me.colId.Visible = True
        Me.colId.VisibleIndex = 0
        '
        'colIdPaymentType
        '
        Me.colIdPaymentType.Caption = "IdPaymentType"
        Me.colIdPaymentType.FieldName = "IdPaymentType"
        Me.colIdPaymentType.Name = "colIdPaymentType"
        Me.colIdPaymentType.OptionsColumn.ReadOnly = True
        Me.colIdPaymentType.Visible = True
        Me.colIdPaymentType.VisibleIndex = 1
        '
        'colIdCustomerVehicleRelation
        '
        Me.colIdCustomerVehicleRelation.Caption = "IdCustomerVehicleRelation"
        Me.colIdCustomerVehicleRelation.FieldName = "IdCustomerVehicleRelation"
        Me.colIdCustomerVehicleRelation.Name = "colIdCustomerVehicleRelation"
        Me.colIdCustomerVehicleRelation.OptionsColumn.ReadOnly = True
        Me.colIdCustomerVehicleRelation.Visible = True
        Me.colIdCustomerVehicleRelation.VisibleIndex = 2
        '
        'colIdOperator
        '
        Me.colIdOperator.Caption = "IdOperator"
        Me.colIdOperator.FieldName = "IdOperator"
        Me.colIdOperator.Name = "colIdOperator"
        Me.colIdOperator.OptionsColumn.ReadOnly = True
        '
        'colDatePay
        '
        Me.colDatePay.Caption = "DatePay"
        Me.colDatePay.FieldName = "DatePay"
        Me.colDatePay.Name = "colDatePay"
        Me.colDatePay.OptionsColumn.ReadOnly = True
        Me.colDatePay.Visible = True
        Me.colDatePay.VisibleIndex = 3
        '
        'colDateRequired
        '
        Me.colDateRequired.Caption = "DateRequired"
        Me.colDateRequired.FieldName = "DateRequired"
        Me.colDateRequired.Name = "colDateRequired"
        Me.colDateRequired.OptionsColumn.ReadOnly = True
        Me.colDateRequired.Visible = True
        Me.colDateRequired.VisibleIndex = 4
        '
        'colDiscount
        '
        Me.colDiscount.Caption = "Discount"
        Me.colDiscount.FieldName = "Discount"
        Me.colDiscount.Name = "colDiscount"
        Me.colDiscount.OptionsColumn.ReadOnly = True
        Me.colDiscount.Visible = True
        Me.colDiscount.VisibleIndex = 5
        '
        'colPayed
        '
        Me.colPayed.Caption = "Payed"
        Me.colPayed.FieldName = "Payed"
        Me.colPayed.Name = "colPayed"
        Me.colPayed.OptionsColumn.ReadOnly = True
        Me.colPayed.Visible = True
        Me.colPayed.VisibleIndex = 6
        '
        'colNote
        '
        Me.colNote.Caption = "Note"
        Me.colNote.FieldName = "Note"
        Me.colNote.Name = "colNote"
        Me.colNote.OptionsColumn.ReadOnly = True
        Me.colNote.Visible = True
        Me.colNote.VisibleIndex = 7
        '
        'colDocumentNumber
        '
        Me.colDocumentNumber.Caption = "DocumentNumber"
        Me.colDocumentNumber.FieldName = "DocumentNumber"
        Me.colDocumentNumber.Name = "colDocumentNumber"
        Me.colDocumentNumber.Visible = True
        Me.colDocumentNumber.VisibleIndex = 8
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(613, 426)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.Text = "LayoutControlGroup1"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.PaymentDocumentsListGridControl
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(611, 424)
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'uxPaymentDocumentsList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "uxPaymentDocumentsList"
        Me.Size = New System.Drawing.Size(613, 445)
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.PaymentDocumentsListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PaymentDocumentsListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents PaymentDocumentsListGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents PaymentDocumentsListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdPaymentType As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdCustomerVehicleRelation As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdOperator As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDatePay As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDateRequired As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDiscount As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colPayed As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colNote As DevExpress.XtraGrid.Columns.GridColumn
 Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
 Friend WithEvents colDocumentNumber As DevExpress.XtraGrid.Columns.GridColumn

End Class
