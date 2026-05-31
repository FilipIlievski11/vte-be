<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxDocumentVehicleOwnershipProof
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
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxDocumentVehicleOwnershipProof))
  Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
  Me.DocumentVehicleOwnershipProofsGridControl = New DevExpress.XtraGrid.GridControl
  Me.DocumentVehicleOwnershipProofsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colVehicleOwnershipProofName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
  Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
  Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
  Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
  Me.SplitterItem1 = New DevExpress.XtraLayout.SplitterItem
  Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
  Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
  Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.LayoutControl1.SuspendLayout()
  CType(Me.DocumentVehicleOwnershipProofsGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DocumentVehicleOwnershipProofsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'LayoutControl1
  '
  Me.LayoutControl1.AccessibleDescription = Nothing
  Me.LayoutControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LayoutControl1, False)
  Me.LayoutControl1.BackgroundImage = Nothing
  Me.LayoutControl1.Controls.Add(Me.DocumentVehicleOwnershipProofsGridControl)
  Me.LayoutControl1.Controls.Add(Me.UxKopcinja1)
  Me.LayoutControl1.Font = Nothing
  Me.LayoutControl1.Name = "LayoutControl1"
  Me.LayoutControl1.Root = Me.LayoutControlGroup1
  '
  'DocumentVehicleOwnershipProofsGridControl
  '
  Me.DocumentVehicleOwnershipProofsGridControl.AccessibleDescription = Nothing
  Me.DocumentVehicleOwnershipProofsGridControl.AccessibleName = Nothing
  resources.ApplyResources(Me.DocumentVehicleOwnershipProofsGridControl, "DocumentVehicleOwnershipProofsGridControl")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.DocumentVehicleOwnershipProofsGridControl, False)
  Me.DocumentVehicleOwnershipProofsGridControl.BackgroundImage = Nothing
  Me.DocumentVehicleOwnershipProofsGridControl.DataSource = Me.DocumentVehicleOwnershipProofsBindingSource
  Me.DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
  Me.DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.AccessibleName = Nothing
  Me.DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
  Me.DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.BackgroundImage = Nothing
  Me.DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.BackgroundImageLayout" & _
          ""), System.Windows.Forms.ImageLayout)
  Me.DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
  Me.DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
  Me.DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.ToolTip = resources.GetString("DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.ToolTip")
  Me.DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
  Me.DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("DocumentVehicleOwnershipProofsGridControl.EmbeddedNavigator.ToolTipTitle")
  Me.DocumentVehicleOwnershipProofsGridControl.Font = Nothing
  Me.DocumentVehicleOwnershipProofsGridControl.MainView = Me.GridView1
  Me.DocumentVehicleOwnershipProofsGridControl.Name = "DocumentVehicleOwnershipProofsGridControl"
  Me.DocumentVehicleOwnershipProofsGridControl.UseEmbeddedNavigator = True
  Me.DocumentVehicleOwnershipProofsGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'DocumentVehicleOwnershipProofsBindingSource
  '
  Me.DocumentVehicleOwnershipProofsBindingSource.DataSource = GetType(VTE.Library.DocumentVehicleOwnershipProof)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.DocumentVehicleOwnershipProofsBindingSource, False)
  '
  'GridView1
  '
  Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
  Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  Me.GridView1.Appearance.Row.Options.UseTextOptions = True
  Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  resources.ApplyResources(Me.GridView1, "GridView1")
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colVehicleOwnershipProofName})
  Me.GridView1.GridControl = Me.DocumentVehicleOwnershipProofsGridControl
  Me.GridView1.Name = "GridView1"
  Me.GridView1.OptionsNavigation.AutoFocusNewRow = True
  Me.GridView1.OptionsNavigation.EnterMoveNextColumn = True
  Me.GridView1.OptionsView.ShowAutoFilterRow = True
  '
  'colId
  '
  resources.ApplyResources(Me.colId, "colId")
  Me.colId.FieldName = "Id"
  Me.colId.Name = "colId"
  Me.colId.OptionsColumn.ReadOnly = True
  '
  'colVehicleOwnershipProofName
  '
  resources.ApplyResources(Me.colVehicleOwnershipProofName, "colVehicleOwnershipProofName")
  Me.colVehicleOwnershipProofName.FieldName = "VehicleOwnershipProofName"
  Me.colVehicleOwnershipProofName.Name = "colVehicleOwnershipProofName"
  '
  'UxKopcinja1
  '
  Me.UxKopcinja1.AccessibleDescription = Nothing
  Me.UxKopcinja1.AccessibleName = Nothing
  resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UxKopcinja1, False)
  Me.UxKopcinja1.BackgroundImage = Nothing
  Me.UxKopcinja1.Name = "UxKopcinja1"
  '
  'LayoutControlGroup1
  '
  resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
  Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.SplitterItem1})
  Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlGroup1.Name = "Root"
  Me.LayoutControlGroup1.Size = New System.Drawing.Size(668, 515)
  Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
  Me.LayoutControlGroup1.TextVisible = False
  '
  'LayoutControlItem1
  '
  Me.LayoutControlItem1.Control = Me.UxKopcinja1
  resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
  Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
  Me.LayoutControlItem1.Name = "LayoutControlItem1"
  Me.LayoutControlItem1.Size = New System.Drawing.Size(664, 54)
  Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem1.TextToControlDistance = 0
  Me.LayoutControlItem1.TextVisible = False
  '
  'LayoutControlItem2
  '
  Me.LayoutControlItem2.Control = Me.DocumentVehicleOwnershipProofsGridControl
  resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
  Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 60)
  Me.LayoutControlItem2.Name = "LayoutControlItem2"
  Me.LayoutControlItem2.Size = New System.Drawing.Size(664, 451)
  Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
  Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
  Me.LayoutControlItem2.TextToControlDistance = 0
  Me.LayoutControlItem2.TextVisible = False
  '
  'SplitterItem1
  '
  resources.ApplyResources(Me.SplitterItem1, "SplitterItem1")
  Me.SplitterItem1.Location = New System.Drawing.Point(0, 54)
  Me.SplitterItem1.Name = "SplitterItem1"
  Me.SplitterItem1.Size = New System.Drawing.Size(664, 6)
  '
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.DocumentVehicleOwnershipProofsBindingSource
  '
  'uxDocumentVehicleOwnershipProof
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.LayoutControl1)
  Me.Name = "uxDocumentVehicleOwnershipProof"
  Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
  CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.LayoutControl1.ResumeLayout(False)
  CType(Me.DocumentVehicleOwnershipProofsGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DocumentVehicleOwnershipProofsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.SplitterItem1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents DocumentVehicleOwnershipProofsGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents DocumentVehicleOwnershipProofsBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colVehicleOwnershipProofName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents SplitterItem1 As DevExpress.XtraLayout.SplitterItem
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider

End Class
