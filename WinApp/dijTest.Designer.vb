<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijTest
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
    Me.GridControl1 = New DevExpress.XtraGrid.GridControl
    Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.PaymentCataologListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.SpinEdit2 = New DevExpress.XtraEditors.SpinEdit
    Me.SpinEdit1 = New DevExpress.XtraEditors.SpinEdit
    Me.ListBoxControl1 = New DevExpress.XtraEditors.ListBoxControl
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
    Me.FontDialog1 = New System.Windows.Forms.FontDialog
    CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PaymentCataologListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.SpinEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.SpinEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.ListBoxControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'GridControl1
    '
    Me.GridControl1.EmbeddedNavigator.Name = ""
    Me.GridControl1.Location = New System.Drawing.Point(7, 299)
    Me.GridControl1.MainView = Me.GridView1
    Me.GridControl1.Name = "GridControl1"
    Me.GridControl1.Size = New System.Drawing.Size(686, 176)
    Me.GridControl1.TabIndex = 0
    Me.GridControl1.UseEmbeddedNavigator = True
    Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1, Me.GridView2})
    '
    'GridView1
    '
    Me.GridView1.GridControl = Me.GridControl1
    Me.GridView1.Name = "GridView1"
    Me.GridView1.OptionsView.ShowFooter = True
    '
    'GridView2
    '
    Me.GridView2.GridControl = Me.GridControl1
    Me.GridView2.Name = "GridView2"
    '
    'PaymentCataologListBindingSource
    '
    Me.PaymentCataologListBindingSource.DataSource = GetType(VTE.Library.PaymentCataologList)
    '
    'SimpleButton1
    '
    Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.SimpleButton1.Appearance.Options.UseFont = True
    Me.SimpleButton1.Location = New System.Drawing.Point(7, 7)
    Me.SimpleButton1.Name = "SimpleButton1"
    Me.SimpleButton1.Size = New System.Drawing.Size(686, 28)
    Me.SimpleButton1.StyleController = Me.LayoutControl1
    Me.SimpleButton1.TabIndex = 1
    Me.SimpleButton1.Text = "SimpleButton1"
    '
    'LayoutControl1
    '
    Me.LayoutControl1.Appearance.Control.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.LayoutControl1.Appearance.Control.Options.UseFont = True
    Me.LayoutControl1.Controls.Add(Me.SpinEdit2)
    Me.LayoutControl1.Controls.Add(Me.SpinEdit1)
    Me.LayoutControl1.Controls.Add(Me.ListBoxControl1)
    Me.LayoutControl1.Controls.Add(Me.GridControl1)
    Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
    Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.LayoutControl1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    Me.LayoutControl1.Size = New System.Drawing.Size(699, 481)
    Me.LayoutControl1.TabIndex = 2
    Me.LayoutControl1.Text = "LayoutControl1"
    '
    'SpinEdit2
    '
    Me.SpinEdit2.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
    Me.SpinEdit2.Location = New System.Drawing.Point(153, 83)
    Me.SpinEdit2.Name = "SpinEdit2"
    Me.SpinEdit2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.SpinEdit2.Size = New System.Drawing.Size(540, 26)
    Me.SpinEdit2.StyleController = Me.LayoutControl1
    Me.SpinEdit2.TabIndex = 6
    '
    'SpinEdit1
    '
    Me.SpinEdit1.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
    Me.SpinEdit1.Location = New System.Drawing.Point(153, 46)
    Me.SpinEdit1.Name = "SpinEdit1"
    Me.SpinEdit1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.SpinEdit1.Properties.Appearance.Options.UseFont = True
    Me.SpinEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.SpinEdit1.Size = New System.Drawing.Size(540, 26)
    Me.SpinEdit1.StyleController = Me.LayoutControl1
    Me.SpinEdit1.TabIndex = 5
    '
    'ListBoxControl1
    '
    Me.ListBoxControl1.Location = New System.Drawing.Point(7, 120)
    Me.ListBoxControl1.Name = "ListBoxControl1"
    Me.ListBoxControl1.Size = New System.Drawing.Size(686, 168)
    Me.ListBoxControl1.StyleController = Me.LayoutControl1
    Me.ListBoxControl1.TabIndex = 4
    '
    'LayoutControlGroup1
    '
    Me.LayoutControlGroup1.AppearanceItemCaption.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.LayoutControlGroup1.AppearanceItemCaption.Options.UseFont = True
    Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem5})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(699, 481)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.Text = "LayoutControlGroup1"
    Me.LayoutControlGroup1.TextVisible = False
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.GridControl1
    Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 292)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(697, 187)
    Me.LayoutControlItem1.Text = "LayoutControlItem1"
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem1.TextToControlDistance = 0
    Me.LayoutControlItem1.TextVisible = False
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.SimpleButton1
    Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
    Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(697, 39)
    Me.LayoutControlItem2.Text = "LayoutControlItem2"
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem2.TextToControlDistance = 0
    Me.LayoutControlItem2.TextVisible = False
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.ListBoxControl1
    Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
    Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 113)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(697, 179)
    Me.LayoutControlItem3.Text = "LayoutControlItem3"
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem3.TextToControlDistance = 0
    Me.LayoutControlItem3.TextVisible = False
    '
    'LayoutControlItem4
    '
    Me.LayoutControlItem4.AppearanceItemCaption.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
    Me.LayoutControlItem4.AppearanceItemCaption.Options.UseFont = True
    Me.LayoutControlItem4.Control = Me.SpinEdit1
    Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
    Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 39)
    Me.LayoutControlItem4.Name = "LayoutControlItem4"
    Me.LayoutControlItem4.Size = New System.Drawing.Size(697, 37)
    Me.LayoutControlItem4.Text = "LayoutControlItem4"
    Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem4.TextSize = New System.Drawing.Size(141, 20)
    '
    'LayoutControlItem5
    '
    Me.LayoutControlItem5.Control = Me.SpinEdit2
    Me.LayoutControlItem5.CustomizationFormText = "LayoutControlItem5"
    Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 76)
    Me.LayoutControlItem5.Name = "LayoutControlItem5"
    Me.LayoutControlItem5.Size = New System.Drawing.Size(697, 37)
    Me.LayoutControlItem5.Text = "LayoutControlItem5"
    Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem5.TextSize = New System.Drawing.Size(141, 20)
    '
    'dijTest
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(699, 481)
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "dijTest"
    Me.Text = "dijTest"
    CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PaymentCataologListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.SpinEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.SpinEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.ListBoxControl1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents PaymentCataologListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents ListBoxControl1 As DevExpress.XtraEditors.ListBoxControl
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SpinEdit1 As DevExpress.XtraEditors.SpinEdit
  Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SpinEdit2 As DevExpress.XtraEditors.SpinEdit
  Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents FontDialog1 As System.Windows.Forms.FontDialog
End Class
