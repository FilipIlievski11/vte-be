<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxVehicleRegList
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxVehicleRegList))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.VehicleRegistrationListGridControl = New DevExpress.XtraGrid.GridControl
        Me.VehicleRegistrationListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdVehicle = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIdRegistrationIssuer = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIssuerName = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colRegistrationNumber = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDateOfRegistration = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDateRegistrationValidTill = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colPlaceOfRegistration = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colIsFirstRegistration = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDateAdded = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colUserChanged = New DevExpress.XtraGrid.Columns.GridColumn
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.VehicleRegistrationListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleRegistrationListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.AccessibleDescription = Nothing
        Me.LayoutControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
        Me.LayoutControl1.BackgroundImage = Nothing
        Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl1.Controls.Add(Me.VehicleRegistrationListGridControl)
        Me.LayoutControl1.Font = Nothing
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        '
        'SimpleButton1
        '
        Me.SimpleButton1.AccessibleDescription = Nothing
        Me.SimpleButton1.AccessibleName = Nothing
        resources.ApplyResources(Me.SimpleButton1, "SimpleButton1")
        Me.SimpleButton1.BackgroundImage = Nothing
        Me.SimpleButton1.MinimumSize = New System.Drawing.Size(0, 40)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.StyleController = Me.LayoutControl1
        '
        'VehicleRegistrationListGridControl
        '
        Me.VehicleRegistrationListGridControl.AccessibleDescription = Nothing
        Me.VehicleRegistrationListGridControl.AccessibleName = Nothing
        resources.ApplyResources(Me.VehicleRegistrationListGridControl, "VehicleRegistrationListGridControl")
        Me.VehicleRegistrationListGridControl.BackgroundImage = Nothing
        Me.VehicleRegistrationListGridControl.DataSource = Me.VehicleRegistrationListBindingSource
        Me.VehicleRegistrationListGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
        Me.VehicleRegistrationListGridControl.EmbeddedNavigator.AccessibleName = Nothing
        Me.VehicleRegistrationListGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("VehicleRegistrationListGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.VehicleRegistrationListGridControl.EmbeddedNavigator.BackgroundImage = Nothing
        Me.VehicleRegistrationListGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("VehicleRegistrationListGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
        Me.VehicleRegistrationListGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("VehicleRegistrationListGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
        Me.VehicleRegistrationListGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("VehicleRegistrationListGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
        Me.VehicleRegistrationListGridControl.EmbeddedNavigator.ToolTip = resources.GetString("VehicleRegistrationListGridControl.EmbeddedNavigator.ToolTip")
        Me.VehicleRegistrationListGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("VehicleRegistrationListGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
        Me.VehicleRegistrationListGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("VehicleRegistrationListGridControl.EmbeddedNavigator.ToolTipTitle")
        Me.VehicleRegistrationListGridControl.Font = Nothing
        Me.VehicleRegistrationListGridControl.MainView = Me.GridView1
        Me.VehicleRegistrationListGridControl.Name = "VehicleRegistrationListGridControl"
        Me.VehicleRegistrationListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
        '
        'VehicleRegistrationListBindingSource
        '
        Me.VehicleRegistrationListBindingSource.DataSource = GetType(VTE.Library.VehicleRegistrationInfo)
        '
        'GridView1
        '
        resources.ApplyResources(Me.GridView1, "GridView1")
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colIdVehicle, Me.colIdRegistrationIssuer, Me.colIssuerName, Me.colRegistrationNumber, Me.colDateOfRegistration, Me.colDateRegistrationValidTill, Me.colPlaceOfRegistration, Me.colIsFirstRegistration, Me.colDateAdded, Me.colUserChanged})
        Me.GridView1.GridControl = Me.VehicleRegistrationListGridControl
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
        Me.colIdVehicle.OptionsColumn.ReadOnly = True
        '
        'colIdRegistrationIssuer
        '
        resources.ApplyResources(Me.colIdRegistrationIssuer, "colIdRegistrationIssuer")
        Me.colIdRegistrationIssuer.FieldName = "IdRegistrationIssuer"
        Me.colIdRegistrationIssuer.Name = "colIdRegistrationIssuer"
        Me.colIdRegistrationIssuer.OptionsColumn.ReadOnly = True
        '
        'colIssuerName
        '
        resources.ApplyResources(Me.colIssuerName, "colIssuerName")
        Me.colIssuerName.FieldName = "IssuerName"
        Me.colIssuerName.Name = "colIssuerName"
        Me.colIssuerName.OptionsColumn.ReadOnly = True
        '
        'colRegistrationNumber
        '
        resources.ApplyResources(Me.colRegistrationNumber, "colRegistrationNumber")
        Me.colRegistrationNumber.FieldName = "RegistrationNumber"
        Me.colRegistrationNumber.Name = "colRegistrationNumber"
        Me.colRegistrationNumber.OptionsColumn.ReadOnly = True
        '
        'colDateOfRegistration
        '
        resources.ApplyResources(Me.colDateOfRegistration, "colDateOfRegistration")
        Me.colDateOfRegistration.FieldName = "DateOfRegistration"
        Me.colDateOfRegistration.Name = "colDateOfRegistration"
        Me.colDateOfRegistration.OptionsColumn.ReadOnly = True
        '
        'colDateRegistrationValidTill
        '
        resources.ApplyResources(Me.colDateRegistrationValidTill, "colDateRegistrationValidTill")
        Me.colDateRegistrationValidTill.FieldName = "DateRegistrationValidTill"
        Me.colDateRegistrationValidTill.Name = "colDateRegistrationValidTill"
        Me.colDateRegistrationValidTill.OptionsColumn.ReadOnly = True
        '
        'colPlaceOfRegistration
        '
        resources.ApplyResources(Me.colPlaceOfRegistration, "colPlaceOfRegistration")
        Me.colPlaceOfRegistration.FieldName = "PlaceOfRegistration"
        Me.colPlaceOfRegistration.Name = "colPlaceOfRegistration"
        Me.colPlaceOfRegistration.OptionsColumn.ReadOnly = True
        '
        'colIsFirstRegistration
        '
        resources.ApplyResources(Me.colIsFirstRegistration, "colIsFirstRegistration")
        Me.colIsFirstRegistration.FieldName = "IsFirstRegistration"
        Me.colIsFirstRegistration.Name = "colIsFirstRegistration"
        Me.colIsFirstRegistration.OptionsColumn.ReadOnly = True
        '
        'colDateAdded
        '
        resources.ApplyResources(Me.colDateAdded, "colDateAdded")
        Me.colDateAdded.FieldName = "DateAdded"
        Me.colDateAdded.Name = "colDateAdded"
        Me.colDateAdded.OptionsColumn.ReadOnly = True
        '
        'colUserChanged
        '
        resources.ApplyResources(Me.colUserChanged, "colUserChanged")
        Me.colUserChanged.FieldName = "UserChanged"
        Me.colUserChanged.Name = "colUserChanged"
        Me.colUserChanged.OptionsColumn.ReadOnly = True
        '
        'GridView2
        '
        resources.ApplyResources(Me.GridView2, "GridView2")
        Me.GridView2.GridControl = Me.VehicleRegistrationListGridControl
        Me.GridView2.Name = "GridView2"
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(928, 601)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.VehicleRegistrationListGridControl
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 55)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(926, 544)
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.SimpleButton1
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(926, 55)
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'uxVehicleRegList
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.LayoutControl1)
        Me.Icon = Nothing
        Me.Name = "uxVehicleRegList"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.VehicleRegistrationListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleRegistrationListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents VehicleRegistrationListGridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents VehicleRegistrationListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdVehicle As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIdRegistrationIssuer As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIssuerName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colRegistrationNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDateOfRegistration As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDateRegistrationValidTill As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPlaceOfRegistration As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIsFirstRegistration As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDateAdded As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colUserChanged As DevExpress.XtraGrid.Columns.GridColumn
End Class
