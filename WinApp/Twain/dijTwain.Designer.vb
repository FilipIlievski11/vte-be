<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijTwain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dijTwain))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.btnOk = New DevExpress.XtraEditors.SimpleButton
        Me.btnCancel = New DevExpress.XtraEditors.SimpleButton
        Me.MemoEdit1 = New DevExpress.XtraEditors.MemoEdit
        Me.DocumentAttachmentBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ComboBoxEdit1 = New DevExpress.XtraEditors.ComboBoxEdit
        Me.LookUpEdit1 = New DevExpress.XtraEditors.LookUpEdit
        Me.AttachmentTypeListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DevicesListBox = New DevExpress.XtraEditors.ListBoxControl
        Me.btnScan = New DevExpress.XtraEditors.SimpleButton
        Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar3 = New DevExpress.XtraBars.Bar
        Me.StatusBarStaticItem = New DevExpress.XtraBars.BarStaticItem
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
        Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.MemoEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DocumentAttachmentBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ComboBoxEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AttachmentTypeListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DevicesListBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.AccessibleDescription = Nothing
        Me.LayoutControl1.AccessibleName = Nothing
        resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LayoutControl1, False)
        Me.LayoutControl1.BackgroundImage = Nothing
        Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl1.Controls.Add(Me.btnOk)
        Me.LayoutControl1.Controls.Add(Me.btnCancel)
        Me.LayoutControl1.Controls.Add(Me.MemoEdit1)
        Me.LayoutControl1.Controls.Add(Me.ComboBoxEdit1)
        Me.LayoutControl1.Controls.Add(Me.LookUpEdit1)
        Me.LayoutControl1.Controls.Add(Me.DevicesListBox)
        Me.LayoutControl1.Controls.Add(Me.btnScan)
        Me.LayoutControl1.Controls.Add(Me.PictureEdit1)
        Me.LayoutControl1.Font = Nothing
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsItemText.TextAlignMode = DevExpress.XtraLayout.TextAlignMode.AutoSize
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        '
        'SimpleButton1
        '
        Me.SimpleButton1.AccessibleDescription = Nothing
        Me.SimpleButton1.AccessibleName = Nothing
        resources.ApplyResources(Me.SimpleButton1, "SimpleButton1")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SimpleButton1, False)
        Me.SimpleButton1.BackgroundImage = Nothing
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.StyleController = Me.LayoutControl1
        '
        'btnOk
        '
        Me.btnOk.AccessibleDescription = Nothing
        Me.btnOk.AccessibleName = Nothing
        resources.ApplyResources(Me.btnOk, "btnOk")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.btnOk, False)
        Me.btnOk.BackgroundImage = Nothing
        Me.btnOk.Name = "btnOk"
        Me.btnOk.StyleController = Me.LayoutControl1
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleDescription = Nothing
        Me.btnCancel.AccessibleName = Nothing
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.btnCancel, False)
        Me.btnCancel.BackgroundImage = Nothing
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.StyleController = Me.LayoutControl1
        '
        'MemoEdit1
        '
        resources.ApplyResources(Me.MemoEdit1, "MemoEdit1")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.MemoEdit1, False)
        Me.MemoEdit1.BackgroundImage = Nothing
        Me.MemoEdit1.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.DocumentAttachmentBindingSource, "AttachmentNotes", True))
        Me.MemoEdit1.EditValue = Nothing
        Me.MemoEdit1.Name = "MemoEdit1"
        Me.MemoEdit1.Properties.AccessibleDescription = Nothing
        Me.MemoEdit1.Properties.AccessibleName = Nothing
        Me.MemoEdit1.StyleController = Me.LayoutControl1
        '
        'DocumentAttachmentBindingSource
        '
        Me.DocumentAttachmentBindingSource.DataSource = GetType(VTE.Library.Attachment)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.DocumentAttachmentBindingSource, False)
        '
        'ComboBoxEdit1
        '
        resources.ApplyResources(Me.ComboBoxEdit1, "ComboBoxEdit1")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.ComboBoxEdit1, False)
        Me.ComboBoxEdit1.BackgroundImage = Nothing
        Me.ComboBoxEdit1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DocumentAttachmentBindingSource, "AttachmentStatus", True))
        Me.ComboBoxEdit1.EditValue = Nothing
        Me.ComboBoxEdit1.Name = "ComboBoxEdit1"
        Me.ComboBoxEdit1.Properties.AccessibleDescription = Nothing
        Me.ComboBoxEdit1.Properties.AccessibleName = Nothing
        Me.ComboBoxEdit1.Properties.AutoHeight = CType(resources.GetObject("ComboBoxEdit1.Properties.AutoHeight"), Boolean)
        Me.ComboBoxEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("ComboBoxEdit1.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.ComboBoxEdit1.Properties.Items.AddRange(New Object() {resources.GetString("ComboBoxEdit1.Properties.Items"), resources.GetString("ComboBoxEdit1.Properties.Items1"), resources.GetString("ComboBoxEdit1.Properties.Items2")})
        Me.ComboBoxEdit1.StyleController = Me.LayoutControl1
        '
        'LookUpEdit1
        '
        resources.ApplyResources(Me.LookUpEdit1, "LookUpEdit1")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LookUpEdit1, False)
        Me.LookUpEdit1.BackgroundImage = Nothing
        Me.LookUpEdit1.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.DocumentAttachmentBindingSource, "IdAttachmentType", True))
        Me.LookUpEdit1.EditValue = Nothing
        Me.LookUpEdit1.Name = "LookUpEdit1"
        Me.LookUpEdit1.Properties.AccessibleDescription = Nothing
        Me.LookUpEdit1.Properties.AccessibleName = Nothing
        Me.LookUpEdit1.Properties.AutoHeight = CType(resources.GetObject("LookUpEdit1.Properties.AutoHeight"), Boolean)
        Me.LookUpEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("LookUpEdit1.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.LookUpEdit1.Properties.Columns.AddRange(New DevExpress.XtraEditors.Controls.LookUpColumnInfo() {New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 29, DevExpress.Utils.FormatType.Numeric, "", False, DevExpress.Utils.HorzAlignment.Far, DevExpress.Data.ColumnSortOrder.None), New DevExpress.XtraEditors.Controls.LookUpColumnInfo("AttachmentType", "AttachmentType", 150, DevExpress.Utils.FormatType.None, "", True, DevExpress.Utils.HorzAlignment.Near)})
        Me.LookUpEdit1.Properties.DataSource = Me.AttachmentTypeListBindingSource
        Me.LookUpEdit1.Properties.DisplayMember = "AttachmentType"
        Me.LookUpEdit1.Properties.ValueMember = "Id"
        Me.LookUpEdit1.StyleController = Me.LayoutControl1
        '
        'AttachmentTypeListBindingSource
        '
        Me.AttachmentTypeListBindingSource.DataSource = GetType(VTE.Library.AttachmentTypeList)
        Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.AttachmentTypeListBindingSource, False)
        '
        'DevicesListBox
        '
        Me.DevicesListBox.AccessibleDescription = Nothing
        Me.DevicesListBox.AccessibleName = Nothing
        resources.ApplyResources(Me.DevicesListBox, "DevicesListBox")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.DevicesListBox, False)
        Me.DevicesListBox.BackgroundImage = Nothing
        Me.DevicesListBox.Name = "DevicesListBox"
        Me.DevicesListBox.StyleController = Me.LayoutControl1
        '
        'btnScan
        '
        Me.btnScan.AccessibleDescription = Nothing
        Me.btnScan.AccessibleName = Nothing
        resources.ApplyResources(Me.btnScan, "btnScan")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.btnScan, False)
        Me.btnScan.BackgroundImage = Nothing
        Me.btnScan.Name = "btnScan"
        Me.btnScan.StyleController = Me.LayoutControl1
        '
        'PictureEdit1
        '
        resources.ApplyResources(Me.PictureEdit1, "PictureEdit1")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.PictureEdit1, False)
        Me.PictureEdit1.BackgroundImage = Nothing
        Me.PictureEdit1.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.DocumentAttachmentBindingSource, "AttachmentPicture", True))
        Me.PictureEdit1.EditValue = Nothing
        Me.PictureEdit1.Name = "PictureEdit1"
        Me.PictureEdit1.Properties.AccessibleDescription = Nothing
        Me.PictureEdit1.Properties.AccessibleName = Nothing
        Me.PictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
        Me.PictureEdit1.StyleController = Me.LayoutControl1
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem5, Me.LayoutControlItem6, Me.LayoutControlItem7, Me.LayoutControlItem2, Me.LayoutControlItem8, Me.LayoutControlItem9})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(710, 552)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.PictureEdit1
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(234, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(474, 487)
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.DevicesListBox
        resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 66)
        Me.LayoutControlItem3.MaxSize = New System.Drawing.Size(0, 179)
        Me.LayoutControlItem3.MinSize = New System.Drawing.Size(74, 179)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(234, 179)
        Me.LayoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(93, 15)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.LookUpEdit1
        resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 245)
        Me.LayoutControlItem4.MaxSize = New System.Drawing.Size(0, 51)
        Me.LayoutControlItem4.MinSize = New System.Drawing.Size(107, 51)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(234, 51)
        Me.LayoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(85, 15)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.ComboBoxEdit1
        resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 296)
        Me.LayoutControlItem5.MaxSize = New System.Drawing.Size(0, 51)
        Me.LayoutControlItem5.MinSize = New System.Drawing.Size(107, 51)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(234, 51)
        Me.LayoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(103, 15)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.MemoEdit1
        resources.ApplyResources(Me.LayoutControlItem6, "LayoutControlItem6")
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 347)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(234, 140)
        Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(56, 15)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.btnCancel
        resources.ApplyResources(Me.LayoutControlItem7, "LayoutControlItem7")
        Me.LayoutControlItem7.Location = New System.Drawing.Point(353, 487)
        Me.LayoutControlItem7.MaxSize = New System.Drawing.Size(0, 63)
        Me.LayoutControlItem7.MinSize = New System.Drawing.Size(63, 63)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(355, 63)
        Me.LayoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem7.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem7.TextToControlDistance = 0
        Me.LayoutControlItem7.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.btnScan
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(234, 33)
        Me.LayoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Right
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(16, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.btnOk
        resources.ApplyResources(Me.LayoutControlItem8, "LayoutControlItem8")
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 487)
        Me.LayoutControlItem8.MaxSize = New System.Drawing.Size(0, 63)
        Me.LayoutControlItem8.MinSize = New System.Drawing.Size(44, 63)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(353, 63)
        Me.LayoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem8.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem8.TextToControlDistance = 0
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.SimpleButton1
        resources.ApplyResources(Me.LayoutControlItem9, "LayoutControlItem9")
        Me.LayoutControlItem9.Location = New System.Drawing.Point(0, 33)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(234, 33)
        Me.LayoutControlItem9.TextLocation = DevExpress.Utils.Locations.Right
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(16, 20)
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.StatusBarStaticItem})
        Me.BarManager1.MaxItemId = 1
        Me.BarManager1.StatusBar = Me.Bar3
        '
        'Bar3
        '
        Me.Bar3.BarName = "Status bar"
        Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
        Me.Bar3.DockCol = 0
        Me.Bar3.DockRow = 0
        Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
        Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.StatusBarStaticItem)})
        Me.Bar3.OptionsBar.AllowQuickCustomization = False
        Me.Bar3.OptionsBar.DrawDragBorder = False
        Me.Bar3.OptionsBar.UseWholeRow = True
        resources.ApplyResources(Me.Bar3, "Bar3")
        '
        'StatusBarStaticItem
        '
        Me.StatusBarStaticItem.AccessibleDescription = Nothing
        Me.StatusBarStaticItem.AccessibleName = Nothing
        resources.ApplyResources(Me.StatusBarStaticItem, "StatusBarStaticItem")
        Me.StatusBarStaticItem.Id = 0
        Me.StatusBarStaticItem.Name = "StatusBarStaticItem"
        Me.StatusBarStaticItem.TextAlignment = System.Drawing.StringAlignment.Near
        '
        'barDockControlTop
        '
        Me.barDockControlTop.AccessibleDescription = Nothing
        Me.barDockControlTop.AccessibleName = Nothing
        resources.ApplyResources(Me.barDockControlTop, "barDockControlTop")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.barDockControlTop, False)
        Me.barDockControlTop.Font = Nothing
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.AccessibleDescription = Nothing
        Me.barDockControlBottom.AccessibleName = Nothing
        resources.ApplyResources(Me.barDockControlBottom, "barDockControlBottom")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.barDockControlBottom, False)
        Me.barDockControlBottom.Font = Nothing
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.AccessibleDescription = Nothing
        Me.barDockControlLeft.AccessibleName = Nothing
        resources.ApplyResources(Me.barDockControlLeft, "barDockControlLeft")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.barDockControlLeft, False)
        Me.barDockControlLeft.Font = Nothing
        '
        'barDockControlRight
        '
        Me.barDockControlRight.AccessibleDescription = Nothing
        Me.barDockControlRight.AccessibleName = Nothing
        resources.ApplyResources(Me.barDockControlRight, "barDockControlRight")
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.barDockControlRight, False)
        Me.barDockControlRight.Font = Nothing
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        Me.DxErrorProvider1.DataSource = Me.DocumentAttachmentBindingSource
        '
        'BindingSourceRefresh1
        '
        Me.BindingSourceRefresh1.Host = Me
        '
        'OpenFileDialog1
        '
        resources.ApplyResources(Me.OpenFileDialog1, "OpenFileDialog1")
        '
        'dijTwain
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.LayoutControl1)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Icon = Nothing
        Me.Name = "dijTwain"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.MemoEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DocumentAttachmentBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ComboBoxEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AttachmentTypeListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DevicesListBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
  Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
  Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
  Friend WithEvents btnScan As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
  Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents DevicesListBox As DevExpress.XtraEditors.ListBoxControl
  Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents ComboBoxEdit1 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents LookUpEdit1 As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents MemoEdit1 As DevExpress.XtraEditors.MemoEdit
  Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents btnOk As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents DocumentAttachmentBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents AttachmentTypeListBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
  Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
  Friend WithEvents StatusBarStaticItem As DevExpress.XtraBars.BarStaticItem
End Class
