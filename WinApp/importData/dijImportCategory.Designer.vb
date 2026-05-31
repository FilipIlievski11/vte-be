<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijImportCategory
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
    Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.ImportCategoryListGridControl = New DevExpress.XtraGrid.GridControl
    Me.ColorsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
    Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
    Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colColorCode = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colColorDescription = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colNewColorEffects = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colColor = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colNewColorCode = New DevExpress.XtraGrid.Columns.GridColumn
    Me.colNewColorDarkness = New DevExpress.XtraGrid.Columns.GridColumn
    Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
    Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
    Me.txtPath = New DevExpress.XtraEditors.TextEdit
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
    Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.ImportCategoryListGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.ColorsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtPath.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'OpenFileDialog1
    '
    Me.OpenFileDialog1.FileName = "OpenFileDialog1"
    '
    'LayoutControl1
    '
    Me.LayoutControl1.Appearance.DisabledLayoutGroupCaption.ForeColor = System.Drawing.SystemColors.GrayText
    Me.LayoutControl1.Appearance.DisabledLayoutGroupCaption.Options.UseForeColor = True
    Me.LayoutControl1.Appearance.DisabledLayoutItem.ForeColor = System.Drawing.SystemColors.GrayText
    Me.LayoutControl1.Appearance.DisabledLayoutItem.Options.UseForeColor = True
    Me.LayoutControl1.Controls.Add(Me.ImportCategoryListGridControl)
    Me.LayoutControl1.Controls.Add(Me.SimpleButton2)
    Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
    Me.LayoutControl1.Controls.Add(Me.txtPath)
    Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    Me.LayoutControl1.Size = New System.Drawing.Size(536, 361)
    Me.LayoutControl1.TabIndex = 0
    Me.LayoutControl1.Text = "LayoutControl1"
    '
    'ImportCategoryListGridControl
    '
    Me.ImportCategoryListGridControl.DataSource = Me.ColorsBindingSource
    Me.ImportCategoryListGridControl.Location = New System.Drawing.Point(8, 74)
    Me.ImportCategoryListGridControl.MainView = Me.GridView1
    Me.ImportCategoryListGridControl.Name = "ImportCategoryListGridControl"
    Me.ImportCategoryListGridControl.Size = New System.Drawing.Size(521, 280)
    Me.ImportCategoryListGridControl.TabIndex = 7
    Me.ImportCategoryListGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
    '
    'ColorsBindingSource
    '
    Me.ColorsBindingSource.DataSource = GetType(VTE.Library.Colors)
    '
    'GridView1
    '
    Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colColorCode, Me.colColorDescription, Me.colNewColorEffects, Me.colColor, Me.colNewColorCode, Me.colNewColorDarkness})
    Me.GridView1.GridControl = Me.ImportCategoryListGridControl
    Me.GridView1.Name = "GridView1"
    '
    'colId
    '
    Me.colId.Caption = "Id"
    Me.colId.FieldName = "Id"
    Me.colId.Name = "colId"
    Me.colId.OptionsColumn.ReadOnly = True
    '
    'colColorCode
    '
    Me.colColorCode.Caption = "ColorCode"
    Me.colColorCode.FieldName = "ColorCode"
    Me.colColorCode.Name = "colColorCode"
    Me.colColorCode.Visible = True
    Me.colColorCode.VisibleIndex = 0
    '
    'colColorDescription
    '
    Me.colColorDescription.Caption = "ColorDescription"
    Me.colColorDescription.FieldName = "ColorDescription"
    Me.colColorDescription.Name = "colColorDescription"
    Me.colColorDescription.Visible = True
    Me.colColorDescription.VisibleIndex = 1
    '
    'colNewColorEffects
    '
    Me.colNewColorEffects.Caption = "NewColorEffects"
    Me.colNewColorEffects.FieldName = "NewColorEffects"
    Me.colNewColorEffects.Name = "colNewColorEffects"
    Me.colNewColorEffects.Visible = True
    Me.colNewColorEffects.VisibleIndex = 2
    '
    'colColor
    '
    Me.colColor.Caption = "Color"
    Me.colColor.FieldName = "Color"
    Me.colColor.Name = "colColor"
    Me.colColor.OptionsColumn.ReadOnly = True
    '
    'colNewColorCode
    '
    Me.colNewColorCode.Caption = "NewColorCode"
    Me.colNewColorCode.FieldName = "NewColorCode"
    Me.colNewColorCode.Name = "colNewColorCode"
    Me.colNewColorCode.Visible = True
    Me.colNewColorCode.VisibleIndex = 3
    '
    'colNewColorDarkness
    '
    Me.colNewColorDarkness.Caption = "NewColorDarkness"
    Me.colNewColorDarkness.FieldName = "NewColorDarkness"
    Me.colNewColorDarkness.Name = "colNewColorDarkness"
    Me.colNewColorDarkness.Visible = True
    Me.colNewColorDarkness.VisibleIndex = 4
    '
    'SimpleButton2
    '
    Me.SimpleButton2.Location = New System.Drawing.Point(8, 41)
    Me.SimpleButton2.Name = "SimpleButton2"
    Me.SimpleButton2.Size = New System.Drawing.Size(521, 22)
    Me.SimpleButton2.StyleController = Me.LayoutControl1
    Me.SimpleButton2.TabIndex = 6
    Me.SimpleButton2.Text = "SimpleButton2"
    '
    'SimpleButton1
    '
    Me.SimpleButton1.Location = New System.Drawing.Point(273, 8)
    Me.SimpleButton1.Name = "SimpleButton1"
    Me.SimpleButton1.Size = New System.Drawing.Size(256, 22)
    Me.SimpleButton1.StyleController = Me.LayoutControl1
    Me.SimpleButton1.TabIndex = 5
    Me.SimpleButton1.Text = "SimpleButton1"
    '
    'txtPath
    '
    Me.txtPath.Location = New System.Drawing.Point(109, 8)
    Me.txtPath.Name = "txtPath"
    Me.txtPath.Size = New System.Drawing.Size(153, 20)
    Me.txtPath.StyleController = Me.LayoutControl1
    Me.txtPath.TabIndex = 4
    '
    'LayoutControlGroup1
    '
    Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(536, 361)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.Text = "LayoutControlGroup1"
    Me.LayoutControlGroup1.TextVisible = False
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.txtPath
    Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(265, 33)
    Me.LayoutControlItem1.Text = "LayoutControlItem1"
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(96, 20)
    '
    'LayoutControlItem2
    '
    Me.LayoutControlItem2.Control = Me.SimpleButton1
    Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
    Me.LayoutControlItem2.Location = New System.Drawing.Point(265, 0)
    Me.LayoutControlItem2.Name = "LayoutControlItem2"
    Me.LayoutControlItem2.Size = New System.Drawing.Size(267, 33)
    Me.LayoutControlItem2.Text = "LayoutControlItem2"
    Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem2.TextToControlDistance = 0
    Me.LayoutControlItem2.TextVisible = False
    '
    'LayoutControlItem3
    '
    Me.LayoutControlItem3.Control = Me.SimpleButton2
    Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
    Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 33)
    Me.LayoutControlItem3.Name = "LayoutControlItem3"
    Me.LayoutControlItem3.Size = New System.Drawing.Size(532, 33)
    Me.LayoutControlItem3.Text = "LayoutControlItem3"
    Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem3.TextToControlDistance = 0
    Me.LayoutControlItem3.TextVisible = False
    '
    'LayoutControlItem4
    '
    Me.LayoutControlItem4.Control = Me.ImportCategoryListGridControl
    Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
    Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 66)
    Me.LayoutControlItem4.Name = "LayoutControlItem4"
    Me.LayoutControlItem4.Size = New System.Drawing.Size(532, 291)
    Me.LayoutControlItem4.Text = "LayoutControlItem4"
    Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem4.TextToControlDistance = 0
    Me.LayoutControlItem4.TextVisible = False
    '
    'dijImportCategory
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(536, 361)
    Me.Controls.Add(Me.LayoutControl1)
    Me.Name = "dijImportCategory"
    Me.Text = "ImportCategory"
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.ImportCategoryListGridControl, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.ColorsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtPath.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents txtPath As DevExpress.XtraEditors.TextEdit
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents ImportCategoryListGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents ColorsBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colColorCode As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colColorDescription As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colNewColorEffects As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colColor As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colNewColorCode As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colNewColorDarkness As DevExpress.XtraGrid.Columns.GridColumn
End Class
