<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijVremenskiFiskalniIzvestaii
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
    Me.deStartDate = New DevExpress.XtraEditors.DateEdit
    Me.deEndDate = New DevExpress.XtraEditors.DateEdit
    Me.btnCancel = New DevExpress.XtraEditors.SimpleButton
    Me.btnOK = New DevExpress.XtraEditors.SimpleButton
    Me.btnSkraten = New DevExpress.XtraEditors.SimpleButton
    CType(Me.deStartDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deStartDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deEndDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deEndDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'deStartDate
    '
    Me.deStartDate.EditValue = Nothing
    Me.deStartDate.Location = New System.Drawing.Point(12, 12)
    Me.deStartDate.Name = "deStartDate"
    Me.deStartDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.deStartDate.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.deStartDate.Size = New System.Drawing.Size(128, 20)
    Me.deStartDate.TabIndex = 0
    '
    'deEndDate
    '
    Me.deEndDate.EditValue = Nothing
    Me.deEndDate.Location = New System.Drawing.Point(146, 12)
    Me.deEndDate.Name = "deEndDate"
    Me.deEndDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.deEndDate.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.deEndDate.Size = New System.Drawing.Size(128, 20)
    Me.deEndDate.TabIndex = 1
    '
    'btnCancel
    '
    Me.btnCancel.Location = New System.Drawing.Point(492, 12)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(100, 37)
    Me.btnCancel.TabIndex = 4
    Me.btnCancel.Text = "Откажи"
    '
    'btnOK
    '
    Me.btnOK.Location = New System.Drawing.Point(386, 12)
    Me.btnOK.Name = "btnOK"
    Me.btnOK.Size = New System.Drawing.Size(100, 37)
    Me.btnOK.TabIndex = 3
    Me.btnOK.Text = "Печати детален"
    '
    'btnSkraten
    '
    Me.btnSkraten.Location = New System.Drawing.Point(280, 12)
    Me.btnSkraten.Name = "btnSkraten"
    Me.btnSkraten.Size = New System.Drawing.Size(100, 37)
    Me.btnSkraten.TabIndex = 5
    Me.btnSkraten.Text = "Печати скратен"
    '
    'dijVremenskiFiskalniIzvestaii
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(598, 71)
    Me.Controls.Add(Me.btnSkraten)
    Me.Controls.Add(Me.btnCancel)
    Me.Controls.Add(Me.btnOK)
    Me.Controls.Add(Me.deEndDate)
    Me.Controls.Add(Me.deStartDate)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
    Me.Name = "dijVremenskiFiskalniIzvestaii"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Временски фискални извештаии"
    CType(Me.deStartDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deStartDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deEndDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deEndDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents deStartDate As DevExpress.XtraEditors.DateEdit
  Friend WithEvents deEndDate As DevExpress.XtraEditors.DateEdit
  Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnSkraten As DevExpress.XtraEditors.SimpleButton
End Class
