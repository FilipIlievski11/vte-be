<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.txtBIOS = New DevExpress.XtraEditors.TextEdit
    Me.txtCPU = New DevExpress.XtraEditors.TextEdit
    Me.txtMAC = New DevExpress.XtraEditors.TextEdit
    Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
    Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl
    Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl
    CType(Me.txtBIOS.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtCPU.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtMAC.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'txtBIOS
    '
    Me.txtBIOS.Location = New System.Drawing.Point(63, 12)
    Me.txtBIOS.Name = "txtBIOS"
    Me.txtBIOS.Size = New System.Drawing.Size(352, 20)
    Me.txtBIOS.TabIndex = 0
    '
    'txtCPU
    '
    Me.txtCPU.Location = New System.Drawing.Point(63, 67)
    Me.txtCPU.Name = "txtCPU"
    Me.txtCPU.Size = New System.Drawing.Size(352, 20)
    Me.txtCPU.TabIndex = 0
    '
    'txtMAC
    '
    Me.txtMAC.Location = New System.Drawing.Point(63, 122)
    Me.txtMAC.Name = "txtMAC"
    Me.txtMAC.Size = New System.Drawing.Size(352, 20)
    Me.txtMAC.TabIndex = 0
    '
    'LabelControl1
    '
    Me.LabelControl1.Location = New System.Drawing.Point(32, 15)
    Me.LabelControl1.Name = "LabelControl1"
    Me.LabelControl1.Size = New System.Drawing.Size(28, 13)
    Me.LabelControl1.TabIndex = 1
    Me.LabelControl1.Text = "BIOS:"
    '
    'LabelControl2
    '
    Me.LabelControl2.Location = New System.Drawing.Point(32, 70)
    Me.LabelControl2.Name = "LabelControl2"
    Me.LabelControl2.Size = New System.Drawing.Size(24, 13)
    Me.LabelControl2.TabIndex = 1
    Me.LabelControl2.Text = "CPU:"
    '
    'LabelControl3
    '
    Me.LabelControl3.Location = New System.Drawing.Point(32, 125)
    Me.LabelControl3.Name = "LabelControl3"
    Me.LabelControl3.Size = New System.Drawing.Size(26, 13)
    Me.LabelControl3.TabIndex = 1
    Me.LabelControl3.Text = "MAC:"
    '
    'Form1
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(444, 162)
    Me.Controls.Add(Me.LabelControl3)
    Me.Controls.Add(Me.LabelControl2)
    Me.Controls.Add(Me.LabelControl1)
    Me.Controls.Add(Me.txtMAC)
    Me.Controls.Add(Me.txtCPU)
    Me.Controls.Add(Me.txtBIOS)
    Me.Name = "Form1"
    Me.Text = "Form1"
    CType(Me.txtBIOS.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtCPU.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtMAC.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents txtBIOS As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txtCPU As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txtMAC As DevExpress.XtraEditors.TextEdit
  Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl

End Class
