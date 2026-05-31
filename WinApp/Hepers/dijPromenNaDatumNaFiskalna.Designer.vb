<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dijPromenNaDatumNaFiskalna
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
    Me.DateEdit1 = New DevExpress.XtraEditors.DateEdit
    Me.btnOK = New DevExpress.XtraEditors.SimpleButton
    Me.btnCancel = New DevExpress.XtraEditors.SimpleButton
    CType(Me.DateEdit1.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'DateEdit1
    '
    Me.DateEdit1.EditValue = Nothing
    Me.DateEdit1.Location = New System.Drawing.Point(13, 13)
    Me.DateEdit1.Name = "DateEdit1"
    Me.DateEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.DateEdit1.Properties.DisplayFormat.FormatString = "g"
    Me.DateEdit1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    Me.DateEdit1.Properties.EditFormat.FormatString = "g"
    Me.DateEdit1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    Me.DateEdit1.Properties.Mask.EditMask = "g"
    Me.DateEdit1.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
    Me.DateEdit1.Size = New System.Drawing.Size(189, 20)
    Me.DateEdit1.TabIndex = 0
    '
    'btnOK
    '
    Me.btnOK.Location = New System.Drawing.Point(208, 10)
    Me.btnOK.Name = "btnOK"
    Me.btnOK.Size = New System.Drawing.Size(75, 37)
    Me.btnOK.TabIndex = 1
    Me.btnOK.Text = "Промени"
    '
    'btnCancel
    '
    Me.btnCancel.Location = New System.Drawing.Point(289, 10)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(75, 37)
    Me.btnCancel.TabIndex = 2
    Me.btnCancel.Text = "Откажи"
    '
    'dijPromenNaDatumNaFiskalna
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(386, 59)
    Me.Controls.Add(Me.btnCancel)
    Me.Controls.Add(Me.btnOK)
    Me.Controls.Add(Me.DateEdit1)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
    Me.Name = "dijPromenNaDatumNaFiskalna"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Промена на датум на фискален апарат"
    CType(Me.DateEdit1.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents DateEdit1 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
End Class
