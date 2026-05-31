<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxWinPart
    Inherits DevExpress.XtraEditors.XtraUserControl

    'UserControl overrides dispose to clean up the component list.
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxWinPart))
    Me.DockManager1 = New DevExpress.XtraBars.Docking.DockManager(Me.components)
    Me.hideContainerBottom = New DevExpress.XtraBars.Docking.AutoHideContainer
    Me.dpBrokenRules = New DevExpress.XtraBars.Docking.DockPanel
    Me.DockPanel1_Container = New DevExpress.XtraBars.Docking.ControlContainer
    Me.lblMessage = New DevExpress.XtraEditors.LabelControl
    CType(Me.DockManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.hideContainerBottom.SuspendLayout()
    Me.dpBrokenRules.SuspendLayout()
    Me.DockPanel1_Container.SuspendLayout()
    Me.SuspendLayout()
    '
    'DockManager1
    '
    Me.DockManager1.AutoHideContainers.AddRange(New DevExpress.XtraBars.Docking.AutoHideContainer() {Me.hideContainerBottom})
    Me.DockManager1.Form = Me
    Me.DockManager1.TopZIndexControls.AddRange(New String() {"DevExpress.XtraBars.BarDockControl", "System.Windows.Forms.StatusBar", "DevExpress.XtraBars.Ribbon.RibbonStatusBar", "DevExpress.XtraBars.Ribbon.RibbonControl"})
    '
    'hideContainerBottom
    '
    Me.hideContainerBottom.Controls.Add(Me.dpBrokenRules)
    resources.ApplyResources(Me.hideContainerBottom, "hideContainerBottom")
    Me.hideContainerBottom.Name = "hideContainerBottom"
    '
    'dpBrokenRules
    '
    Me.dpBrokenRules.BackColor = System.Drawing.Color.Transparent
    Me.dpBrokenRules.Controls.Add(Me.DockPanel1_Container)
    Me.dpBrokenRules.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom
    Me.dpBrokenRules.FloatVertical = True
    Me.dpBrokenRules.ID = New System.Guid("ae44f894-77a8-48e9-9316-5d8ea681ec19")
    resources.ApplyResources(Me.dpBrokenRules, "dpBrokenRules")
    Me.dpBrokenRules.Name = "dpBrokenRules"
    Me.dpBrokenRules.Options.ShowCloseButton = False
    Me.dpBrokenRules.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Bottom
    Me.dpBrokenRules.SavedIndex = 0
    Me.dpBrokenRules.TabStop = False
    Me.dpBrokenRules.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide
    '
    'DockPanel1_Container
    '
    Me.DockPanel1_Container.Controls.Add(Me.lblMessage)
    resources.ApplyResources(Me.DockPanel1_Container, "DockPanel1_Container")
    Me.DockPanel1_Container.Name = "DockPanel1_Container"
    '
    'lblMessage
    '
    Me.lblMessage.Appearance.ForeColor = System.Drawing.Color.Red
    Me.lblMessage.Appearance.Options.UseForeColor = True
    resources.ApplyResources(Me.lblMessage, "lblMessage")
    Me.lblMessage.Name = "lblMessage"
    '
    'uxWinPart
    '
    resources.ApplyResources(Me, "$this")
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.hideContainerBottom)
    Me.Name = "uxWinPart"
    CType(Me.DockManager1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.hideContainerBottom.ResumeLayout(False)
    Me.dpBrokenRules.ResumeLayout(False)
    Me.DockPanel1_Container.ResumeLayout(False)
    Me.DockPanel1_Container.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents DockManager1 As DevExpress.XtraBars.Docking.DockManager
  Friend WithEvents hideContainerBottom As DevExpress.XtraBars.Docking.AutoHideContainer
  Friend WithEvents dpBrokenRules As DevExpress.XtraBars.Docking.DockPanel
  Friend WithEvents DockPanel1_Container As DevExpress.XtraBars.Docking.ControlContainer
  Friend WithEvents lblMessage As DevExpress.XtraEditors.LabelControl

End Class
