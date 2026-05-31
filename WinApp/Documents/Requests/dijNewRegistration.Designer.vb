<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijNewRegistration
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dijNewRegistration))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.btnCancel = New DevExpress.XtraEditors.SimpleButton
        Me.btnOk = New DevExpress.XtraEditors.SimpleButton
        Me.LookUpEdit1 = New DevExpress.XtraEditors.LookUpEdit
        Me.LastRegistrationValidTillDateEdit = New DevExpress.XtraEditors.DateEdit
        Me.LastRegistrationMakeDateDateEdit = New DevExpress.XtraEditors.DateEdit
        Me.LastRegistrationCommunityTextEdit = New DevExpress.XtraEditors.TextEdit
        Me.LastRegistratinNumberTextEdit = New DevExpress.XtraEditors.TextEdit
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        Me.VehicleBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
        Me.RegistrationIssuerListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.LookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LastRegistrationValidTillDateEdit.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LastRegistrationValidTillDateEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LastRegistrationMakeDateDateEdit.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LastRegistrationMakeDateDateEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LastRegistrationCommunityTextEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LastRegistratinNumberTextEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VehicleBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RegistrationIssuerListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.btnCancel)
        Me.LayoutControl1.Controls.Add(Me.btnOk)
        Me.LayoutControl1.Controls.Add(Me.LookUpEdit1)
        Me.LayoutControl1.Controls.Add(Me.LastRegistrationValidTillDateEdit)
        Me.LayoutControl1.Controls.Add(Me.LastRegistrationMakeDateDateEdit)
        Me.LayoutControl1.Controls.Add(Me.LastRegistrationCommunityTextEdit)
        Me.LayoutControl1.Controls.Add(Me.LastRegistratinNumberTextEdit)
        resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        '
        'btnCancel
        '
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.MinimumSize = New System.Drawing.Size(0, 44)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.StyleController = Me.LayoutControl1
        '
        'btnOk
        '
        resources.ApplyResources(Me.btnOk, "btnOk")
        Me.btnOk.MinimumSize = New System.Drawing.Size(0, 44)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.StyleController = Me.LayoutControl1
        '
        'LookUpEdit1
        '
        Me.LookUpEdit1.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.VehicleBindingSource, "LastIdRegistrationIssuer", True))
        resources.ApplyResources(Me.LookUpEdit1, "LookUpEdit1")
        Me.LookUpEdit1.Name = "LookUpEdit1"
        Me.LookUpEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEdit1.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.LookUpEdit1.Properties.DataSource = Me.RegistrationIssuerListBindingSource
        Me.LookUpEdit1.Properties.DisplayMember = "IssuerName"
        Me.LookUpEdit1.Properties.ValueMember = "Id"
        Me.LookUpEdit1.StyleController = Me.LayoutControl1
        '
        'LastRegistrationValidTillDateEdit
        '
        Me.LastRegistrationValidTillDateEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.VehicleBindingSource, "LastRegistrationValidTill", True))
        Me.LastRegistrationValidTillDateEdit.EditValue = Nothing
        resources.ApplyResources(Me.LastRegistrationValidTillDateEdit, "LastRegistrationValidTillDateEdit")
        Me.LastRegistrationValidTillDateEdit.Name = "LastRegistrationValidTillDateEdit"
        Me.LastRegistrationValidTillDateEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LastRegistrationValidTillDateEdit.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.LastRegistrationValidTillDateEdit.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.LastRegistrationValidTillDateEdit.StyleController = Me.LayoutControl1
        '
        'LastRegistrationMakeDateDateEdit
        '
        Me.LastRegistrationMakeDateDateEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.VehicleBindingSource, "LastRegistrationMakeDate", True))
        Me.LastRegistrationMakeDateDateEdit.EditValue = Nothing
        resources.ApplyResources(Me.LastRegistrationMakeDateDateEdit, "LastRegistrationMakeDateDateEdit")
        Me.LastRegistrationMakeDateDateEdit.Name = "LastRegistrationMakeDateDateEdit"
        Me.LastRegistrationMakeDateDateEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LastRegistrationMakeDateDateEdit.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.LastRegistrationMakeDateDateEdit.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.LastRegistrationMakeDateDateEdit.StyleController = Me.LayoutControl1
        '
        'LastRegistrationCommunityTextEdit
        '
        Me.LastRegistrationCommunityTextEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.VehicleBindingSource, "LastRegistrationCommunity", True))
        resources.ApplyResources(Me.LastRegistrationCommunityTextEdit, "LastRegistrationCommunityTextEdit")
        Me.LastRegistrationCommunityTextEdit.Name = "LastRegistrationCommunityTextEdit"
        Me.LastRegistrationCommunityTextEdit.StyleController = Me.LayoutControl1
        '
        'LastRegistratinNumberTextEdit
        '
        Me.LastRegistratinNumberTextEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.VehicleBindingSource, "LastRegistratinNumber", True))
        resources.ApplyResources(Me.LastRegistratinNumberTextEdit, "LastRegistratinNumberTextEdit")
        Me.LastRegistratinNumberTextEdit.Name = "LastRegistratinNumberTextEdit"
        Me.LastRegistratinNumberTextEdit.StyleController = Me.LayoutControl1
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem5, Me.LayoutControlItem3, Me.LayoutControlItem1, Me.LayoutControlItem6, Me.LayoutControlItem7})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(366, 217)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        Me.DxErrorProvider1.DataSource = Me.VehicleBindingSource
        '
        'VehicleBindingSource
        '
        Me.VehicleBindingSource.DataSource = GetType(VTE.Library.Vehicle)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.LastRegistratinNumberTextEdit
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(362, 31)
        Me.LayoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(119, 20)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.LastRegistrationMakeDateDateEdit
        resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 62)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(362, 31)
        Me.LayoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(139, 20)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.LastRegistrationValidTillDateEdit
        resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 93)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(362, 31)
        Me.LayoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(125, 20)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.LastRegistrationCommunityTextEdit
        resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 31)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(362, 31)
        Me.LayoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(141, 20)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.LookUpEdit1
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 124)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(362, 31)
        Me.LayoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(96, 20)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.btnOk
        resources.ApplyResources(Me.LayoutControlItem6, "LayoutControlItem6")
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 155)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(181, 58)
        Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem6.TextToControlDistance = 0
        Me.LayoutControlItem6.TextVisible = False
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.btnCancel
        resources.ApplyResources(Me.LayoutControlItem7, "LayoutControlItem7")
        Me.LayoutControlItem7.Location = New System.Drawing.Point(181, 155)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(181, 58)
        Me.LayoutControlItem7.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem7.TextToControlDistance = 0
        Me.LayoutControlItem7.TextVisible = False
        '
        'RegistrationIssuerListBindingSource
        '
        Me.RegistrationIssuerListBindingSource.DataSource = GetType(VTE.Library.RegistrationIssuerList)
        '
        'dijNewRegistration
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "dijNewRegistration"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.LookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LastRegistrationValidTillDateEdit.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LastRegistrationValidTillDateEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LastRegistrationMakeDateDateEdit.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LastRegistrationMakeDateDateEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LastRegistrationCommunityTextEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LastRegistratinNumberTextEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VehicleBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RegistrationIssuerListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnOk As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LookUpEdit1 As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents VehicleBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents RegistrationIssuerListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents LastRegistrationValidTillDateEdit As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LastRegistrationMakeDateDateEdit As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LastRegistrationCommunityTextEdit As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LastRegistratinNumberTextEdit As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
End Class
