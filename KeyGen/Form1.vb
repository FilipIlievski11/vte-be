Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtBIOS.Text = HardwareInfo.GetBIOS
        txtCPU.Text = HardwareInfo.GetCPU
        txtMAC.Text = HardwareInfo.GetMAC
        txtDatum.Text = Format(Now.Date.AddMonths(3), "dd/MM/yyyy")
        Dim cry As New Crypt("VTEBSS")
        txtSN.Text = cry.Encrypt(txtDatum.Text) & cry.Encrypt(HardwareInfo.GetCPU) & cry.Encrypt(HardwareInfo.GetBIOS) & cry.Encrypt(HardwareInfo.GetMAC)
    End Sub

    Private Sub txtSN_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSN.TextChanged

    End Sub

    Private Sub btnMake_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMake.Click
        Dim cry As New Crypt("VTEBSS")
        Dim crydata As New Crypt("VTEData")
        txtSN.Text = cry.Encrypt(txtDatum.Text) & cry.Encrypt(txtCPU.Text) & cry.Encrypt(txtBIOS.Text) & cry.Encrypt(txtMAC.Text)
        txtEncodedId.Text = crydata.Encrypt(txtUserId.Text)
        txtEncodedPWD.Text = crydata.Encrypt(txtPWD.Text)
    End Sub
End Class
