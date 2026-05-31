<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijDocumentRequestsWithoutTehnicalExam
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dijDocumentRequestsWithoutTehnicalExam))
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.btnExit = New DevExpress.XtraEditors.SimpleButton
    Me.btnCreateTehExamReport = New DevExpress.XtraEditors.SimpleButton
    Me.DocumentActiveListGridControl = New DevExpress.XtraGrid.GridControl
    Me.DocumentActiveListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdDocumentType = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdCustomerVehicleRelation = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdOperatorCreated = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdOperatorModified = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdOperatorEnded = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdTechnicalExamReport = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colDateCreated = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colDateModified = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colDateEnded = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colVehicleOwnershipProof = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdPayAttachment = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colNote = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdCustomer = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdVehicle = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colDocumentTypeName = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colCustomerSurname = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colCustomerFirstName = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colCustomerName = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIsCompany = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colShellNumber = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colEngineNumber = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colVehicleDisplay = New DevExpress.XtraGrid.Columns.GridColumn
    Me.HyperLinkEditShellNum = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
    Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.DocumentActiveListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DocumentActiveListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.HyperLinkEditShellNum, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'LayoutControl1
    '
    Me.LayoutControl1.Appearance.DisabledLayoutGroupCaption.ForeColor = System.Drawing.SystemColors.GrayText
    Me.LayoutControl1.Appearance.DisabledLayoutGroupCaption.Options.UseForeColor = True
    Me.LayoutControl1.Appearance.DisabledLayoutItem.ForeColor = System.Drawing.SystemColors.GrayText
    Me.LayoutControl1.Appearance.DisabledLayoutItem.Options.UseForeColor = True
    Me.LayoutControl1.Controls.Add(Me.btnExit)
    Me.LayoutControl1.Controls.Add(Me.btnCreateTehExamReport)
    Me.LayoutControl1.Controls.Add(Me.DocumentActiveListGridControl)
    resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    '
    'btnExit
    '
    Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
    resources.ApplyResources(Me.btnExit, "btnExit")
    Me.btnExit.Name = "btnExit"
    Me.btnExit.StyleController = Me.LayoutControl1
    '
    'btnCreateTehExamReport
    '
    resources.ApplyResources(Me.btnCreateTehExamReport, "btnCreateTehExamReport")
    Me.btnCreateTehExamReport.Name = "btnCreateTehExamReport"
    Me.btnCreateTehExamReport.StyleController = Me.LayoutControl1
    '
    'DocumentActiveListGridControl
    '
    Me.DocumentActiveListGridControl.DataSource = Me.DocumentActiveListBindingSource
    resources.ApplyResources(Me.DocumentActiveListGridControl, "DocumentActiveListGridControl")
    Me.DocumentActiveListGridControl.MainView = Me.GridView1
    Me.DocumentActiveListGridControl.Name = "DocumentActiveListGridControl"
    Me.DocumentActiveListGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.HyperLinkEditShellNum})
    Me.DocumentActiveListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
    '
    'DocumentActiveListBindingSource
    '
    Me.DocumentActiveListBindingSource.DataSource = GetType(VTE.Library.DocumentActiveInfo)
    '
    'GridView1
    '
    Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
    Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    Me.GridView1.Appearance.Row.Options.UseTextOptions = True
    Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdDocumentType, Me.colIdCustomerVehicleRelation, Me.colIdOperatorCreated, Me.colIdOperatorModified, Me.colIdOperatorEnded, Me.colIdTechnicalExamReport, Me.colDateCreated, Me.colDateModified, Me.colDateEnded, Me.colVehicleOwnershipProof, Me.colIdPayAttachment, Me.colNote, Me.colIdCustomer, Me.colIdVehicle, Me.colDocumentTypeName, Me.colCustomerSurname, Me.colCustomerFirstName, Me.colCustomerName, Me.colIsCompany, Me.colShellNumber, Me.colEngineNumber, Me.colVehicleDisplay})
    Me.GridView1.GridControl = Me.DocumentActiveListGridControl
    Me.GridView1.Name = "GridView1"
    '
    'colId
    '
    resources.ApplyResources(Me.colId, "colId")
    Me.colId.FieldName = "Id"
    Me.colId.Name = "colId"
    Me.colId.OptionsColumn.ReadOnly = True
    '
    'colIdDocumentType
    '
    resources.ApplyResources(Me.colIdDocumentType, "colIdDocumentType")
    Me.colIdDocumentType.FieldName = "IdDocumentType"
    Me.colIdDocumentType.Name = "colIdDocumentType"
    Me.colIdDocumentType.OptionsColumn.ReadOnly = True
    '
    'colIdCustomerVehicleRelation
    '
    resources.ApplyResources(Me.colIdCustomerVehicleRelation, "colIdCustomerVehicleRelation")
    Me.colIdCustomerVehicleRelation.FieldName = "IdCustomerVehicleRelation"
    Me.colIdCustomerVehicleRelation.Name = "colIdCustomerVehicleRelation"
    Me.colIdCustomerVehicleRelation.OptionsColumn.ReadOnly = True
    '
    'colIdOperatorCreated
    '
    resources.ApplyResources(Me.colIdOperatorCreated, "colIdOperatorCreated")
    Me.colIdOperatorCreated.FieldName = "IdOperatorCreated"
    Me.colIdOperatorCreated.Name = "colIdOperatorCreated"
    Me.colIdOperatorCreated.OptionsColumn.ReadOnly = True
    '
    'colIdOperatorModified
    '
    resources.ApplyResources(Me.colIdOperatorModified, "colIdOperatorModified")
    Me.colIdOperatorModified.FieldName = "IdOperatorModified"
    Me.colIdOperatorModified.Name = "colIdOperatorModified"
    Me.colIdOperatorModified.OptionsColumn.ReadOnly = True
    '
    'colIdOperatorEnded
    '
    resources.ApplyResources(Me.colIdOperatorEnded, "colIdOperatorEnded")
    Me.colIdOperatorEnded.FieldName = "IdOperatorEnded"
    Me.colIdOperatorEnded.Name = "colIdOperatorEnded"
    Me.colIdOperatorEnded.OptionsColumn.ReadOnly = True
    '
    'colIdTechnicalExamReport
    '
    Me.colIdTechnicalExamReport.FieldName = "IdTechnicalExamReport"
    Me.colIdTechnicalExamReport.Name = "colIdTechnicalExamReport"
    Me.colIdTechnicalExamReport.OptionsColumn.ReadOnly = True
    '
    'colDateCreated
    '
    resources.ApplyResources(Me.colDateCreated, "colDateCreated")
    Me.colDateCreated.FieldName = "DateCreated"
    Me.colDateCreated.Name = "colDateCreated"
    Me.colDateCreated.OptionsColumn.ReadOnly = True
    '
    'colDateModified
    '
    resources.ApplyResources(Me.colDateModified, "colDateModified")
    Me.colDateModified.FieldName = "DateModified"
    Me.colDateModified.Name = "colDateModified"
    Me.colDateModified.OptionsColumn.ReadOnly = True
    '
    'colDateEnded
    '
    resources.ApplyResources(Me.colDateEnded, "colDateEnded")
    Me.colDateEnded.FieldName = "DateEnded"
    Me.colDateEnded.Name = "colDateEnded"
    Me.colDateEnded.OptionsColumn.ReadOnly = True
    '
    'colVehicleOwnershipProof
    '
    resources.ApplyResources(Me.colVehicleOwnershipProof, "colVehicleOwnershipProof")
    Me.colVehicleOwnershipProof.FieldName = "VehicleOwnershipProof"
    Me.colVehicleOwnershipProof.Name = "colVehicleOwnershipProof"
    Me.colVehicleOwnershipProof.OptionsColumn.ReadOnly = True
    '
    'colIdPayAttachment
    '
    resources.ApplyResources(Me.colIdPayAttachment, "colIdPayAttachment")
    Me.colIdPayAttachment.FieldName = "IdPayAttachment"
    Me.colIdPayAttachment.Name = "colIdPayAttachment"
    Me.colIdPayAttachment.OptionsColumn.ReadOnly = True
    '
    'colNote
    '
    resources.ApplyResources(Me.colNote, "colNote")
    Me.colNote.FieldName = "Note"
    Me.colNote.Name = "colNote"
    Me.colNote.OptionsColumn.ReadOnly = True
    '
    'colIdCustomer
    '
    resources.ApplyResources(Me.colIdCustomer, "colIdCustomer")
    Me.colIdCustomer.FieldName = "IdCustomer"
    Me.colIdCustomer.Name = "colIdCustomer"
    Me.colIdCustomer.OptionsColumn.ReadOnly = True
    '
    'colIdVehicle
    '
    resources.ApplyResources(Me.colIdVehicle, "colIdVehicle")
    Me.colIdVehicle.FieldName = "IdVehicle"
    Me.colIdVehicle.Name = "colIdVehicle"
    Me.colIdVehicle.OptionsColumn.ReadOnly = True
    '
    'colDocumentTypeName
    '
    resources.ApplyResources(Me.colDocumentTypeName, "colDocumentTypeName")
    Me.colDocumentTypeName.FieldName = "DocumentTypeName"
    Me.colDocumentTypeName.Name = "colDocumentTypeName"
    Me.colDocumentTypeName.OptionsColumn.ReadOnly = True
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
    'colCustomerName
    '
    resources.ApplyResources(Me.colCustomerName, "colCustomerName")
    Me.colCustomerName.FieldName = "CustomerName"
    Me.colCustomerName.Name = "colCustomerName"
    Me.colCustomerName.OptionsColumn.AllowEdit = False
    Me.colCustomerName.OptionsColumn.AllowFocus = False
    Me.colCustomerName.OptionsColumn.ReadOnly = True
    '
    'colIsCompany
    '
    resources.ApplyResources(Me.colIsCompany, "colIsCompany")
    Me.colIsCompany.FieldName = "IsCompany"
    Me.colIsCompany.Name = "colIsCompany"
    Me.colIsCompany.OptionsColumn.ReadOnly = True
    '
    'colShellNumber
    '
    resources.ApplyResources(Me.colShellNumber, "colShellNumber")
    Me.colShellNumber.FieldName = "ShellNumber"
    Me.colShellNumber.Name = "colShellNumber"
    Me.colShellNumber.OptionsColumn.AllowEdit = False
    Me.colShellNumber.OptionsColumn.AllowFocus = False
    Me.colShellNumber.OptionsColumn.ReadOnly = True
    '
    'colEngineNumber
    '
    resources.ApplyResources(Me.colEngineNumber, "colEngineNumber")
    Me.colEngineNumber.FieldName = "EngineNumber"
    Me.colEngineNumber.Name = "colEngineNumber"
    Me.colEngineNumber.OptionsColumn.AllowEdit = False
    Me.colEngineNumber.OptionsColumn.AllowFocus = False
    Me.colEngineNumber.OptionsColumn.ReadOnly = True
    '
    'colVehicleDisplay
    '
    Me.colVehicleDisplay.FieldName = "VehicleDisplay"
    Me.colVehicleDisplay.Name = "colVehicleDisplay"
    '
    'HyperLinkEditShellNum
    '
    resources.ApplyResources(Me.HyperLinkEditShellNum, "HyperLinkEditShellNum")
    Me.HyperLinkEditShellNum.Name = "HyperLinkEditShellNum"
    '
    'GridView2
    '
    Me.GridView2.GridControl = Me.DocumentActiveListGridControl
    Me.GridView2.Name = "GridView2"
    '
    'LayoutControlGroup1
    '
    resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(992, 666)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.TextVisible = False
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.DocumentActiveListGridControl
    resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(990, 631)
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem1.TextToControlDistance = 0
    Me.LayoutControlItem1.TextVisible = False
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.btnCreateTehExamReport
    resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
    Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 631)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(494, 33)
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem2.TextToControlDistance = 0
    Me.LayoutControlItem2.TextVisible = False
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.btnExit
    resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
    Me.LayoutControlItem3.Location = New System.Drawing.Point(494, 631)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(496, 33)
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem3.TextToControlDistance = 0
    Me.LayoutControlItem3.TextVisible = False
    '
    'dijDocumentRequestsWithoutTehnicalExam
    '
    Me.AcceptButton = Me.btnCreateTehExamReport
    resources.ApplyResources(Me, "$this")
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.btnExit
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "dijDocumentRequestsWithoutTehnicalExam"
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.DocumentActiveListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DocumentActiveListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.HyperLinkEditShellNum, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnCreateTehExamReport As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents DocumentActiveListGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents DocumentActiveListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents HyperLinkEditShellNum As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
  Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdDocumentType As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdCustomerVehicleRelation As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdOperatorCreated As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdOperatorModified As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdOperatorEnded As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdTechnicalExamReport As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDateCreated As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDateModified As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDateEnded As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colVehicleOwnershipProof As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdPayAttachment As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colNote As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdCustomer As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdVehicle As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDocumentTypeName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCustomerSurname As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCustomerFirstName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCustomerName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIsCompany As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colShellNumber As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colEngineNumber As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colVehicleDisplay As DevExpress.XtraGrid.Columns.GridColumn
End Class
