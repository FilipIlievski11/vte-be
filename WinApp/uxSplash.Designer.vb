<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxSplash
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxSplash))
    Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker
    Me.MarqueeProgressBarControl1 = New DevExpress.XtraEditors.MarqueeProgressBarControl
    Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
    Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
    Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem
    Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
    Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
    CType(Me.MarqueeProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.LayoutControl1.SuspendLayout()
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'BackgroundWorker1
    '
    '
    'MarqueeProgressBarControl1
    '
    resources.ApplyResources(Me.MarqueeProgressBarControl1, "MarqueeProgressBarControl1")
    Me.MarqueeProgressBarControl1.Name = "MarqueeProgressBarControl1"
    Me.MarqueeProgressBarControl1.Properties.MarqueeAnimationSpeed = 50
    Me.MarqueeProgressBarControl1.Properties.ProgressAnimationMode = DevExpress.Utils.Drawing.ProgressAnimationMode.Cycle
    Me.MarqueeProgressBarControl1.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid
    Me.MarqueeProgressBarControl1.StyleController = Me.LayoutControl1
    '
    'LayoutControl1
    '
    Me.LayoutControl1.Appearance.DisabledLayoutGroupCaption.ForeColor = System.Drawing.SystemColors.GrayText
    Me.LayoutControl1.Appearance.DisabledLayoutGroupCaption.Options.UseForeColor = True
    Me.LayoutControl1.Appearance.DisabledLayoutItem.ForeColor = System.Drawing.SystemColors.GrayText
    Me.LayoutControl1.Appearance.DisabledLayoutItem.Options.UseForeColor = True
    Me.LayoutControl1.Controls.Add(Me.MarqueeProgressBarControl1)
    Me.LayoutControl1.Cursor = System.Windows.Forms.Cursors.Default
    resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
    Me.LayoutControl1.Name = "LayoutControl1"
    Me.LayoutControl1.Root = Me.LayoutControlGroup1
    '
    'LayoutControlGroup1
    '
    resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
    Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.EmptySpaceItem1, Me.LayoutControlItem1})
    Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
    Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
    Me.LayoutControlGroup1.Size = New System.Drawing.Size(261, 124)
    Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
    Me.LayoutControlGroup1.TextVisible = False
    '
    'EmptySpaceItem1
    '
    Me.EmptySpaceItem1.AppearanceItemCaption.Options.UseFont = True
    Me.EmptySpaceItem1.AppearanceItemCaption.Options.UseTextOptions = True
    Me.EmptySpaceItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    Me.EmptySpaceItem1.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.EmptySpaceItem1.ControlAlignment = System.Drawing.ContentAlignment.MiddleCenter
    resources.ApplyResources(Me.EmptySpaceItem1, "EmptySpaceItem1")
    Me.EmptySpaceItem1.Location = New System.Drawing.Point(0, 0)
    Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
    Me.EmptySpaceItem1.Size = New System.Drawing.Size(259, 88)
    Me.EmptySpaceItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
    Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 20)
    Me.EmptySpaceItem1.TextVisible = True
    '
    'LayoutControlItem1
    '
    Me.LayoutControlItem1.Control = Me.MarqueeProgressBarControl1
    Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.BottomCenter
    resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
    Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 88)
    Me.LayoutControlItem1.Name = "LayoutControlItem1"
    Me.LayoutControlItem1.Size = New System.Drawing.Size(259, 34)
    Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
    Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
    Me.LayoutControlItem1.TextToControlDistance = 0
    Me.LayoutControlItem1.TextVisible = False
    '
    'uxSplash
    '
    resources.ApplyResources(Me, "$this")
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.LayoutControl1)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
    Me.Name = "uxSplash"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    CType(Me.MarqueeProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.LayoutControl1.ResumeLayout(False)
    CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
  Friend WithEvents MarqueeProgressBarControl1 As DevExpress.XtraEditors.MarqueeProgressBarControl
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
End Class
