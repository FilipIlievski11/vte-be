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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtBIOS = New System.Windows.Forms.TextBox
        Me.txtCPU = New System.Windows.Forms.TextBox
        Me.txtMAC = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtDatum = New System.Windows.Forms.TextBox
        Me.txtSN = New System.Windows.Forms.TextBox
        Me.btnMake = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtUserId = New System.Windows.Forms.TextBox
        Me.txtEncodedId = New System.Windows.Forms.TextBox
        Me.txtPWD = New System.Windows.Forms.TextBox
        Me.txtEncodedPWD = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "CPU"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 85)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(32, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "BIOS"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(21, 148)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(30, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "MAC"
        '
        'txtBIOS
        '
        Me.txtBIOS.Location = New System.Drawing.Point(24, 113)
        Me.txtBIOS.Name = "txtBIOS"
        Me.txtBIOS.Size = New System.Drawing.Size(349, 20)
        Me.txtBIOS.TabIndex = 1
        '
        'txtCPU
        '
        Me.txtCPU.Location = New System.Drawing.Point(24, 50)
        Me.txtCPU.Name = "txtCPU"
        Me.txtCPU.Size = New System.Drawing.Size(349, 20)
        Me.txtCPU.TabIndex = 1
        '
        'txtMAC
        '
        Me.txtMAC.Location = New System.Drawing.Point(24, 176)
        Me.txtMAC.Name = "txtMAC"
        Me.txtMAC.Size = New System.Drawing.Size(349, 20)
        Me.txtMAC.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(21, 211)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Date"
        '
        'txtDatum
        '
        Me.txtDatum.Location = New System.Drawing.Point(24, 239)
        Me.txtDatum.Name = "txtDatum"
        Me.txtDatum.Size = New System.Drawing.Size(349, 20)
        Me.txtDatum.TabIndex = 1
        '
        'txtSN
        '
        Me.txtSN.Location = New System.Drawing.Point(24, 340)
        Me.txtSN.Name = "txtSN"
        Me.txtSN.ReadOnly = True
        Me.txtSN.Size = New System.Drawing.Size(720, 20)
        Me.txtSN.TabIndex = 2
        '
        'btnMake
        '
        Me.btnMake.Location = New System.Drawing.Point(24, 274)
        Me.btnMake.Name = "btnMake"
        Me.btnMake.Size = New System.Drawing.Size(708, 35)
        Me.btnMake.TabIndex = 3
        Me.btnMake.Text = "Make"
        Me.btnMake.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(21, 317)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(22, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "SN"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(439, 22)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(43, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "User ID"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(439, 85)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(89, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Encoded User ID"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(439, 148)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(33, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "PWD"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(439, 211)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(79, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Encoded PWD"
        '
        'txtUserId
        '
        Me.txtUserId.Location = New System.Drawing.Point(442, 50)
        Me.txtUserId.Name = "txtUserId"
        Me.txtUserId.Size = New System.Drawing.Size(290, 20)
        Me.txtUserId.TabIndex = 5
        '
        'txtEncodedId
        '
        Me.txtEncodedId.Location = New System.Drawing.Point(442, 113)
        Me.txtEncodedId.Name = "txtEncodedId"
        Me.txtEncodedId.ReadOnly = True
        Me.txtEncodedId.Size = New System.Drawing.Size(290, 20)
        Me.txtEncodedId.TabIndex = 5
        '
        'txtPWD
        '
        Me.txtPWD.Location = New System.Drawing.Point(442, 176)
        Me.txtPWD.Name = "txtPWD"
        Me.txtPWD.Size = New System.Drawing.Size(290, 20)
        Me.txtPWD.TabIndex = 5
        '
        'txtEncodedPWD
        '
        Me.txtEncodedPWD.Location = New System.Drawing.Point(442, 239)
        Me.txtEncodedPWD.Name = "txtEncodedPWD"
        Me.txtEncodedPWD.ReadOnly = True
        Me.txtEncodedPWD.Size = New System.Drawing.Size(290, 20)
        Me.txtEncodedPWD.TabIndex = 5
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(778, 377)
        Me.Controls.Add(Me.txtEncodedPWD)
        Me.Controls.Add(Me.txtPWD)
        Me.Controls.Add(Me.txtEncodedId)
        Me.Controls.Add(Me.txtUserId)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnMake)
        Me.Controls.Add(Me.txtSN)
        Me.Controls.Add(Me.txtDatum)
        Me.Controls.Add(Me.txtMAC)
        Me.Controls.Add(Me.txtCPU)
        Me.Controls.Add(Me.txtBIOS)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "VTE Hardware Info and Login"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtBIOS As System.Windows.Forms.TextBox
    Friend WithEvents txtCPU As System.Windows.Forms.TextBox
    Friend WithEvents txtMAC As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtDatum As System.Windows.Forms.TextBox
    Friend WithEvents txtSN As System.Windows.Forms.TextBox
    Friend WithEvents btnMake As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtUserId As System.Windows.Forms.TextBox
    Friend WithEvents txtEncodedId As System.Windows.Forms.TextBox
    Friend WithEvents txtPWD As System.Windows.Forms.TextBox
    Friend WithEvents txtEncodedPWD As System.Windows.Forms.TextBox

End Class
