<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijAddNewRegistration
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dijAddNewRegistration))
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.btnExit = New DevExpress.XtraEditors.SimpleButton
    Me.btnSave = New DevExpress.XtraEditors.SimpleButton
    Me.RegistrationsGridControl = New DevExpress.XtraGrid.GridControl
    Me.RegistrationsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.VehicleBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdVehicle = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colRegistrationNumber = New DevExpress.XtraGrid.Columns.GridColumn
    Me.RepositoryItemTextEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Me.colDateOfRegistration = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colDateRegistrationValidTill = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colPlaceOfRegistration = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colIdRegistrationIssuer = New DevExpress.XtraGrid.Columns.GridColumn
    Me.CustomRepositoryItemLookupEdit1 = New VTE.BaseParts.FancyLookupEdit.CustomRepositoryItemLookupEdit
    Me.RegistrationIssuerListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.ShellNumberTextEdit = New DevExpress.XtraEditors.TextEdit
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.RegistrationsGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RegistrationsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.VehicleBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CustomRepositoryItemLookupEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RegistrationIssuerListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.ShellNumberTextEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'LayoutControl1
    '
    Me.LayoutControl1.Controls.Add(Me.btnExit)
    Me.LayoutControl1.Controls.Add(Me.btnSave)
    Me.LayoutControl1.Controls.Add(Me.RegistrationsGridControl)
    Me.LayoutControl1.Controls.Add(Me.ShellNumberTextEdit)
    resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    '
    'btnExit
    '
    resources.ApplyResources(Me.btnExit, "btnExit")
    Me.btnExit.Name = "btnExit"
    Me.btnExit.StyleController = Me.LayoutControl1
    '
    'btnSave
    '
    resources.ApplyResources(Me.btnSave, "btnSave")
    Me.btnSave.Name = "btnSave"
    Me.btnSave.StyleController = Me.LayoutControl1
    '
    'RegistrationsGridControl
    '
    Me.RegistrationsGridControl.DataSource = Me.RegistrationsBindingSource
    resources.ApplyResources(Me.RegistrationsGridControl, "RegistrationsGridControl")
    Me.RegistrationsGridControl.MainView = Me.GridView1
    Me.RegistrationsGridControl.Name = "RegistrationsGridControl"
    Me.RegistrationsGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemTextEdit1, Me.CustomRepositoryItemLookupEdit1})
    Me.RegistrationsGridControl.UseEmbeddedNavigator = True
    Me.RegistrationsGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
    '
    'RegistrationsBindingSource
    '
    Me.RegistrationsBindingSource.DataMember = "Registrations"
    Me.RegistrationsBindingSource.DataSource = Me.VehicleBindingSource
    '
    'VehicleBindingSource
    '
    Me.VehicleBindingSource.DataSource = GetType(VTE.Library.Vehicle)
    '
    'GridView1
    '
    Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
    Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    Me.GridView1.Appearance.Row.Options.UseTextOptions = True
    Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdVehicle, Me.colRegistrationNumber, Me.colDateOfRegistration, Me.colDateRegistrationValidTill, Me.colPlaceOfRegistration, Me.colIdRegistrationIssuer})
    Me.GridView1.GridControl = Me.RegistrationsGridControl
    Me.GridView1.Name = "GridView1"
    '
    'colId
    '
    resources.ApplyResources(Me.colId, "colId")
    Me.colId.FieldName = "Id"
    Me.colId.Name = "colId"
    Me.colId.OptionsColumn.ReadOnly = True
    '
    'colIdVehicle
    '
    resources.ApplyResources(Me.colIdVehicle, "colIdVehicle")
    Me.colIdVehicle.FieldName = "IdVehicle"
    Me.colIdVehicle.Name = "colIdVehicle"
    '
    'colRegistrationNumber
    '
    resources.ApplyResources(Me.colRegistrationNumber, "colRegistrationNumber")
    Me.colRegistrationNumber.ColumnEdit = Me.RepositoryItemTextEdit1
    Me.colRegistrationNumber.FieldName = "RegistrationNumber"
    Me.colRegistrationNumber.Name = "colRegistrationNumber"
    '
    'RepositoryItemTextEdit1
    '
    resources.ApplyResources(Me.RepositoryItemTextEdit1, "RepositoryItemTextEdit1")
    Me.RepositoryItemTextEdit1.Mask.EditMask = resources.GetString("RepositoryItemTextEdit1.Mask.EditMask")
    Me.RepositoryItemTextEdit1.Mask.MaskType = CType(resources.GetObject("RepositoryItemTextEdit1.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
    Me.RepositoryItemTextEdit1.Mask.UseMaskAsDisplayFormat = CType(resources.GetObject("RepositoryItemTextEdit1.Mask.UseMaskAsDisplayFormat"), Boolean)
    Me.RepositoryItemTextEdit1.Name = "RepositoryItemTextEdit1"
    '
    'colDateOfRegistration
    '
    resources.ApplyResources(Me.colDateOfRegistration, "colDateOfRegistration")
    Me.colDateOfRegistration.FieldName = "DateOfRegistration"
    Me.colDateOfRegistration.Name = "colDateOfRegistration"
    '
    'colDateRegistrationValidTill
    '
    resources.ApplyResources(Me.colDateRegistrationValidTill, "colDateRegistrationValidTill")
    Me.colDateRegistrationValidTill.FieldName = "DateRegistrationValidTill"
    Me.colDateRegistrationValidTill.Name = "colDateRegistrationValidTill"
    '
    'colPlaceOfRegistration
    '
    resources.ApplyResources(Me.colPlaceOfRegistration, "colPlaceOfRegistration")
    Me.colPlaceOfRegistration.FieldName = "PlaceOfRegistration"
    Me.colPlaceOfRegistration.Name = "colPlaceOfRegistration"
    '
    'colIdRegistrationIssuer
    '
    resources.ApplyResources(Me.colIdRegistrationIssuer, "colIdRegistrationIssuer")
    Me.colIdRegistrationIssuer.ColumnEdit = Me.CustomRepositoryItemLookupEdit1
    Me.colIdRegistrationIssuer.FieldName = "IdRegistrationIssuer"
    Me.colIdRegistrationIssuer.Name = "colIdRegistrationIssuer"
    '
    'CustomRepositoryItemLookupEdit1
    '
    resources.ApplyResources(Me.CustomRepositoryItemLookupEdit1, "CustomRepositoryItemLookupEdit1")
    Me.CustomRepositoryItemLookupEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("CustomRepositoryItemLookupEdit1.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
    Me.CustomRepositoryItemLookupEdit1.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, "", False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("IssuerName", "IssuerName", 63, DevExpress.Utils.FormatType.None, "", True, DevExpress.Utils.HorzAlignment.Near)})
    Me.CustomRepositoryItemLookupEdit1.DataSource = Me.RegistrationIssuerListBindingSource
    Me.CustomRepositoryItemLookupEdit1.DisplayMember = "IssuerName"
    Me.CustomRepositoryItemLookupEdit1.Name = "CustomRepositoryItemLookupEdit1"
    Me.CustomRepositoryItemLookupEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
    Me.CustomRepositoryItemLookupEdit1.ValueMember = "Id"
    '
    'RegistrationIssuerListBindingSource
    '
    Me.RegistrationIssuerListBindingSource.DataSource = GetType(VTE.Library.RegistrationIssuerList)
    '
    'ShellNumberTextEdit
    '
    Me.ShellNumberTextEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.VehicleBindingSource, "ShellNumber", True))
    resources.ApplyResources(Me.ShellNumberTextEdit, "ShellNumberTextEdit")
    Me.ShellNumberTextEdit.Name = "ShellNumberTextEdit"
    Me.ShellNumberTextEdit.StyleController = Me.LayoutControl1
    '
    'LayoutControlGroup1
    '
    resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem3, Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem4})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(654, 339)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.TextVisible = False
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.ShellNumberTextEdit
    resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
    Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(652, 31)
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(66, 20)
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.RegistrationsGridControl
    resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 31)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(652, 273)
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem1.TextToControlDistance = 0
    Me.LayoutControlItem1.TextVisible = False
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.btnSave
    resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
    Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 304)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(326, 33)
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem2.TextToControlDistance = 0
    Me.LayoutControlItem2.TextVisible = False
    '
    'LayoutControlItem4
    '
    Me.LayoutControlItem4.Control = Me.btnExit
    resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
    Me.LayoutControlItem4.Location = New System.Drawing.Point(326, 304)
    Me.LayoutControlItem4.Name = "LayoutControlItem4"
    Me.LayoutControlItem4.Size = New System.Drawing.Size(326, 33)
    Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem4.TextToControlDistance = 0
    Me.LayoutControlItem4.TextVisible = False
    '
    'dijAddNewRegistration
    '
    resources.ApplyResources(Me, "$this")
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "dijAddNewRegistration"
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.RegistrationsGridControl, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RegistrationsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.VehicleBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CustomRepositoryItemLookupEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RegistrationIssuerListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.ShellNumberTextEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents ShellNumberTextEdit As DevExpress.XtraEditors.TextEdit
  Friend WithEvents VehicleBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents RegistrationsBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents RegistrationsGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colIdVehicle As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colRegistrationNumber As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDateOfRegistration As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colDateRegistrationValidTill As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colPlaceOfRegistration As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents RepositoryItemTextEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
  Friend WithEvents colIdRegistrationIssuer As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents CustomRepositoryItemLookupEdit1 As VTE.BaseParts.FancyLookupEdit.CustomRepositoryItemLookupEdit
  Friend WithEvents RegistrationIssuerListBindingSource As System.Windows.Forms.BindingSource
End Class
