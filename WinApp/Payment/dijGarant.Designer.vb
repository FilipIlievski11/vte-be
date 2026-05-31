<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijGarant
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dijGarant))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl
        Me.txtPrvaRata = New DevExpress.XtraEditors.TextEdit
        Me.btnCancel = New DevExpress.XtraEditors.SimpleButton
        Me.btnOK = New DevExpress.XtraEditors.SimpleButton
        Me.GartEMBTextEdit = New DevExpress.XtraEditors.TextEdit
        Me.PaymentRatiDogovorBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GarantNazivTextEdit = New DevExpress.XtraEditors.TextEdit
        Me.GarantAdresaTextEdit = New DevExpress.XtraEditors.TextEdit
        Me.DatumDateEdit = New DevExpress.XtraEditors.DateEdit
        Me.BrNaRatiSpinEdit = New DevExpress.XtraEditors.SpinEdit
        Me.BrojTextEdit = New DevExpress.XtraEditors.TextEdit
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem
        Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.txtPrvaRata.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GartEMBTextEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PaymentRatiDogovorBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GarantNazivTextEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GarantAdresaTextEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DatumDateEdit.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DatumDateEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BrNaRatiSpinEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BrojTextEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.txtPrvaRata)
        Me.LayoutControl1.Controls.Add(Me.btnCancel)
        Me.LayoutControl1.Controls.Add(Me.btnOK)
        Me.LayoutControl1.Controls.Add(Me.GartEMBTextEdit)
        Me.LayoutControl1.Controls.Add(Me.GarantNazivTextEdit)
        Me.LayoutControl1.Controls.Add(Me.GarantAdresaTextEdit)
        Me.LayoutControl1.Controls.Add(Me.DatumDateEdit)
        Me.LayoutControl1.Controls.Add(Me.BrNaRatiSpinEdit)
        Me.LayoutControl1.Controls.Add(Me.BrojTextEdit)
        resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        '
        'txtPrvaRata
        '
        resources.ApplyResources(Me.txtPrvaRata, "txtPrvaRata")
        Me.txtPrvaRata.Name = "txtPrvaRata"
        Me.txtPrvaRata.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.txtPrvaRata.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.txtPrvaRata.StyleController = Me.LayoutControl1
        '
        'btnCancel
        '
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.StyleController = Me.LayoutControl1
        '
        'btnOK
        '
        resources.ApplyResources(Me.btnOK, "btnOK")
        Me.btnOK.Name = "btnOK"
        Me.btnOK.StyleController = Me.LayoutControl1
        '
        'GartEMBTextEdit
        '
        Me.GartEMBTextEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.PaymentRatiDogovorBindingSource, "GartEMB", True))
        resources.ApplyResources(Me.GartEMBTextEdit, "GartEMBTextEdit")
        Me.GartEMBTextEdit.Name = "GartEMBTextEdit"
        Me.GartEMBTextEdit.Properties.AutoHeight = CType(resources.GetObject("GartEMBTextEdit.Properties.AutoHeight"), Boolean)
        Me.GartEMBTextEdit.Properties.Mask.EditMask = Nothing
        Me.GartEMBTextEdit.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("GartEMBTextEdit.Properties.Mask.IgnoreMaskBlank"), Boolean)
        Me.GartEMBTextEdit.Properties.Mask.SaveLiteral = CType(resources.GetObject("GartEMBTextEdit.Properties.Mask.SaveLiteral"), Boolean)
        Me.GartEMBTextEdit.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("GartEMBTextEdit.Properties.Mask.ShowPlaceHolders"), Boolean)
        Me.GartEMBTextEdit.StyleController = Me.LayoutControl1
        '
        'PaymentRatiDogovorBindingSource
        '
        Me.PaymentRatiDogovorBindingSource.DataSource = GetType(VTE.Library.PaymentRatiDogovor)
        '
        'GarantNazivTextEdit
        '
        Me.GarantNazivTextEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.PaymentRatiDogovorBindingSource, "GarantNaziv", True))
        resources.ApplyResources(Me.GarantNazivTextEdit, "GarantNazivTextEdit")
        Me.GarantNazivTextEdit.Name = "GarantNazivTextEdit"
        Me.GarantNazivTextEdit.Properties.AutoHeight = CType(resources.GetObject("GarantNazivTextEdit.Properties.AutoHeight"), Boolean)
        Me.GarantNazivTextEdit.Properties.Mask.EditMask = Nothing
        Me.GarantNazivTextEdit.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("GarantNazivTextEdit.Properties.Mask.IgnoreMaskBlank"), Boolean)
        Me.GarantNazivTextEdit.Properties.Mask.SaveLiteral = CType(resources.GetObject("GarantNazivTextEdit.Properties.Mask.SaveLiteral"), Boolean)
        Me.GarantNazivTextEdit.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("GarantNazivTextEdit.Properties.Mask.ShowPlaceHolders"), Boolean)
        Me.GarantNazivTextEdit.StyleController = Me.LayoutControl1
        '
        'GarantAdresaTextEdit
        '
        Me.GarantAdresaTextEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.PaymentRatiDogovorBindingSource, "GarantAdresa", True))
        resources.ApplyResources(Me.GarantAdresaTextEdit, "GarantAdresaTextEdit")
        Me.GarantAdresaTextEdit.Name = "GarantAdresaTextEdit"
        Me.GarantAdresaTextEdit.Properties.AutoHeight = CType(resources.GetObject("GarantAdresaTextEdit.Properties.AutoHeight"), Boolean)
        Me.GarantAdresaTextEdit.Properties.Mask.EditMask = Nothing
        Me.GarantAdresaTextEdit.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("GarantAdresaTextEdit.Properties.Mask.IgnoreMaskBlank"), Boolean)
        Me.GarantAdresaTextEdit.Properties.Mask.SaveLiteral = CType(resources.GetObject("GarantAdresaTextEdit.Properties.Mask.SaveLiteral"), Boolean)
        Me.GarantAdresaTextEdit.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("GarantAdresaTextEdit.Properties.Mask.ShowPlaceHolders"), Boolean)
        Me.GarantAdresaTextEdit.StyleController = Me.LayoutControl1
        '
        'DatumDateEdit
        '
        Me.DatumDateEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.PaymentRatiDogovorBindingSource, "Datum", True))
        Me.DatumDateEdit.EditValue = Nothing
        resources.ApplyResources(Me.DatumDateEdit, "DatumDateEdit")
        Me.DatumDateEdit.Name = "DatumDateEdit"
        Me.DatumDateEdit.Properties.AutoHeight = CType(resources.GetObject("DatumDateEdit.Properties.AutoHeight"), Boolean)
        Me.DatumDateEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("DatumDateEdit.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.DatumDateEdit.Properties.Mask.EditMask = Nothing
        Me.DatumDateEdit.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DatumDateEdit.Properties.Mask.IgnoreMaskBlank"), Boolean)
        Me.DatumDateEdit.Properties.Mask.MaskType = CType(resources.GetObject("DatumDateEdit.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.DatumDateEdit.Properties.Mask.SaveLiteral = CType(resources.GetObject("DatumDateEdit.Properties.Mask.SaveLiteral"), Boolean)
        Me.DatumDateEdit.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("DatumDateEdit.Properties.Mask.ShowPlaceHolders"), Boolean)
        Me.DatumDateEdit.Properties.VistaTimeProperties.AutoHeight = CType(resources.GetObject("DatumDateEdit.Properties.VistaTimeProperties.AutoHeight"), Boolean)
        Me.DatumDateEdit.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.DatumDateEdit.Properties.VistaTimeProperties.Mask.EditMask = Nothing
        Me.DatumDateEdit.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank = CType(resources.GetObject("DatumDateEdit.Properties.VistaTimeProperties.Mask.IgnoreMaskBlank"), Boolean)
        Me.DatumDateEdit.Properties.VistaTimeProperties.Mask.MaskType = CType(resources.GetObject("DatumDateEdit.Properties.VistaTimeProperties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.DatumDateEdit.Properties.VistaTimeProperties.Mask.SaveLiteral = CType(resources.GetObject("DatumDateEdit.Properties.VistaTimeProperties.Mask.SaveLiteral"), Boolean)
        Me.DatumDateEdit.Properties.VistaTimeProperties.Mask.ShowPlaceHolders = CType(resources.GetObject("DatumDateEdit.Properties.VistaTimeProperties.Mask.ShowPlaceHolders"), Boolean)
        Me.DatumDateEdit.StyleController = Me.LayoutControl1
        '
        'BrNaRatiSpinEdit
        '
        Me.BrNaRatiSpinEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.PaymentRatiDogovorBindingSource, "BrNaRati", True))
        resources.ApplyResources(Me.BrNaRatiSpinEdit, "BrNaRatiSpinEdit")
        Me.BrNaRatiSpinEdit.Name = "BrNaRatiSpinEdit"
        Me.BrNaRatiSpinEdit.Properties.AutoHeight = CType(resources.GetObject("BrNaRatiSpinEdit.Properties.AutoHeight"), Boolean)
        Me.BrNaRatiSpinEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.BrNaRatiSpinEdit.Properties.Mask.EditMask = Nothing
        Me.BrNaRatiSpinEdit.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("BrNaRatiSpinEdit.Properties.Mask.IgnoreMaskBlank"), Boolean)
        Me.BrNaRatiSpinEdit.Properties.Mask.MaskType = CType(resources.GetObject("BrNaRatiSpinEdit.Properties.Mask.MaskType"), DevExpress.XtraEditors.Mask.MaskType)
        Me.BrNaRatiSpinEdit.Properties.Mask.SaveLiteral = CType(resources.GetObject("BrNaRatiSpinEdit.Properties.Mask.SaveLiteral"), Boolean)
        Me.BrNaRatiSpinEdit.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("BrNaRatiSpinEdit.Properties.Mask.ShowPlaceHolders"), Boolean)
        Me.BrNaRatiSpinEdit.StyleController = Me.LayoutControl1
        '
        'BrojTextEdit
        '
        Me.BrojTextEdit.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.PaymentRatiDogovorBindingSource, "Broj", True))
        resources.ApplyResources(Me.BrojTextEdit, "BrojTextEdit")
        Me.BrojTextEdit.Name = "BrojTextEdit"
        Me.BrojTextEdit.Properties.AutoHeight = CType(resources.GetObject("BrojTextEdit.Properties.AutoHeight"), Boolean)
        Me.BrojTextEdit.Properties.Mask.EditMask = Nothing
        Me.BrojTextEdit.Properties.Mask.IgnoreMaskBlank = CType(resources.GetObject("BrojTextEdit.Properties.Mask.IgnoreMaskBlank"), Boolean)
        Me.BrojTextEdit.Properties.Mask.SaveLiteral = CType(resources.GetObject("BrojTextEdit.Properties.Mask.SaveLiteral"), Boolean)
        Me.BrojTextEdit.Properties.Mask.ShowPlaceHolders = CType(resources.GetObject("BrojTextEdit.Properties.Mask.ShowPlaceHolders"), Boolean)
        Me.BrojTextEdit.StyleController = Me.LayoutControl1
        '
        'LayoutControlGroup1
        '
        resources.ApplyResources(Me.LayoutControlGroup1, "LayoutControlGroup1")
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.LayoutControlItem1, Me.LayoutControlItem8, Me.LayoutControlItem6, Me.LayoutControlItem4, Me.LayoutControlItem5, Me.LayoutControlItem7, Me.LayoutControlItem3, Me.LayoutControlItem9})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(477, 197)
        Me.LayoutControlGroup1.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.BrojTextEdit
        resources.ApplyResources(Me.LayoutControlItem2, "LayoutControlItem2")
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 33)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(145, 31)
        Me.LayoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(23, 20)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.btnOK
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(237, 33)
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.btnCancel
        resources.ApplyResources(Me.LayoutControlItem8, "LayoutControlItem8")
        Me.LayoutControlItem8.Location = New System.Drawing.Point(237, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(236, 33)
        Me.LayoutControlItem8.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem8.TextToControlDistance = 0
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.GarantNazivTextEdit
        resources.ApplyResources(Me.LayoutControlItem6, "LayoutControlItem6")
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 95)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(473, 31)
        Me.LayoutControlItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(66, 20)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.DatumDateEdit
        resources.ApplyResources(Me.LayoutControlItem4, "LayoutControlItem4")
        Me.LayoutControlItem4.Location = New System.Drawing.Point(145, 33)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(328, 31)
        Me.LayoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(35, 20)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.GarantAdresaTextEdit
        resources.ApplyResources(Me.LayoutControlItem5, "LayoutControlItem5")
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 126)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(473, 31)
        Me.LayoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(74, 20)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.GartEMBTextEdit
        resources.ApplyResources(Me.LayoutControlItem7, "LayoutControlItem7")
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 157)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(473, 36)
        Me.LayoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem7.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(48, 20)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.BrNaRatiSpinEdit
        resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 64)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(237, 31)
        Me.LayoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(52, 20)
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.txtPrvaRata
        resources.ApplyResources(Me.LayoutControlItem9, "LayoutControlItem9")
        Me.LayoutControlItem9.Location = New System.Drawing.Point(237, 64)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(236, 31)
        Me.LayoutControlItem9.TextLocation = DevExpress.Utils.Locations.Left
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(49, 20)
        '
        'DxErrorProvider1
        '
        Me.DxErrorProvider1.ContainerControl = Me
        Me.DxErrorProvider1.DataSource = Me.PaymentRatiDogovorBindingSource
        '
        'dijGarant
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "dijGarant"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.txtPrvaRata.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GartEMBTextEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PaymentRatiDogovorBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GarantNazivTextEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GarantAdresaTextEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DatumDateEdit.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DatumDateEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BrNaRatiSpinEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BrojTextEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GartEMBTextEdit As DevExpress.XtraEditors.TextEdit
    Friend WithEvents PaymentRatiDogovorBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GarantNazivTextEdit As DevExpress.XtraEditors.TextEdit
    Friend WithEvents GarantAdresaTextEdit As DevExpress.XtraEditors.TextEdit
    Friend WithEvents DatumDateEdit As DevExpress.XtraEditors.DateEdit
    Friend WithEvents BrNaRatiSpinEdit As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents BrojTextEdit As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents txtPrvaRata As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
End Class
