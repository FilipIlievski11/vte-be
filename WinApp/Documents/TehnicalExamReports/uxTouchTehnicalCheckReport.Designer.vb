<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxTouchTehnicalCheckReport
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
    Me.btnExit = New DevExpress.XtraEditors.SimpleButton
    Me.btnOK = New DevExpress.XtraEditors.SimpleButton
    Me.LookUpEdit1 = New DevExpress.XtraEditors.LookUpEdit
    Me.CustomerVehiclesRelationsListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.LookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CustomerVehiclesRelationsListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'LayoutControl1
    '
    Me.LayoutControl1.Controls.Add(Me.btnExit)
    Me.LayoutControl1.Controls.Add(Me.btnOK)
    Me.LayoutControl1.Controls.Add(Me.LookUpEdit1)
    Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    Me.LayoutControl1.Size = New System.Drawing.Size(576, 389)
    Me.LayoutControl1.TabIndex = 2
    Me.LayoutControl1.Text = "LayoutControl1"
    '
    'btnExit
    '
    Me.btnExit.Location = New System.Drawing.Point(294, 8)
    Me.btnExit.Name = "btnExit"
    Me.btnExit.Size = New System.Drawing.Size(275, 72)
    Me.btnExit.StyleController = Me.LayoutControl1
    Me.btnExit.TabIndex = 6
    Me.btnExit.Text = "Exit"
    '
    'btnOK
    '
    Me.btnOK.Location = New System.Drawing.Point(8, 8)
    Me.btnOK.Name = "btnOK"
    Me.btnOK.Size = New System.Drawing.Size(275, 72)
    Me.btnOK.StyleController = Me.LayoutControl1
    Me.btnOK.TabIndex = 5
    Me.btnOK.Text = "OK"
    '
    'LookUpEdit1
    '
    Me.LookUpEdit1.Location = New System.Drawing.Point(109, 91)
    Me.LookUpEdit1.Name = "LookUpEdit1"
    Me.LookUpEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.LookUpEdit1.Properties.DataSource = Me.CustomerVehiclesRelationsListBindingSource
    Me.LookUpEdit1.Properties.DisplayMember = "RelationDescription"
    Me.LookUpEdit1.Properties.ReadOnly = True
    Me.LookUpEdit1.Properties.ValueMember = "Id"
    Me.LookUpEdit1.Size = New System.Drawing.Size(460, 20)
    Me.LookUpEdit1.StyleController = Me.LayoutControl1
    Me.LookUpEdit1.TabIndex = 4
    '
    'CustomerVehiclesRelationsListBindingSource
    '
    Me.CustomerVehiclesRelationsListBindingSource.DataSource = GetType(VTE.Library.CustomerVehiclesRelationsList)
    '
    'LayoutControlGroup1
    '
    Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(576, 389)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.Text = "LayoutControlGroup1"
    Me.LayoutControlGroup1.TextVisible = False
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.LookUpEdit1
    Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
    Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 83)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(572, 302)
    Me.LayoutControlItem2.Text = "LayoutControlItem2"
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(96, 20)
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.btnOK
    Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
    Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem3.MaxSize = New System.Drawing.Size(0, 83)
    Me.LayoutControlItem3.MinSize = New System.Drawing.Size(38, 83)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(286, 83)
    Me.LayoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem3.Text = "LayoutControlItem3"
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem3.TextToControlDistance = 0
    Me.LayoutControlItem3.TextVisible = False
    '
    'LayoutControlItem4
    '
    Me.LayoutControlItem4.Control = Me.btnExit
    Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
    Me.LayoutControlItem4.Location = New System.Drawing.Point(286, 0)
    Me.LayoutControlItem4.MaxSize = New System.Drawing.Size(0, 83)
    Me.LayoutControlItem4.MinSize = New System.Drawing.Size(42, 83)
    Me.LayoutControlItem4.Name = "LayoutControlItem4"
    Me.LayoutControlItem4.Size = New System.Drawing.Size(286, 83)
    Me.LayoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
    Me.LayoutControlItem4.Text = "LayoutControlItem4"
    Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem4.TextToControlDistance = 0
    Me.LayoutControlItem4.TextVisible = False
    '
    'uxTouchTehnicalCheckReport
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "uxTouchTehnicalCheckReport"
    Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.LookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CustomerVehiclesRelationsListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LookUpEdit1 As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents CustomerVehiclesRelationsListBindingSource As System.Windows.Forms.BindingSource

End Class
