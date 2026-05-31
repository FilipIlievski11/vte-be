Public Class Form1

  Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    txtBIOS.Text = HardwareInfo.GetBIOS
    txtCPU.Text = HardwareInfo.GetCPU
    txtMAC.Text = HardwareInfo.GetMAC
  End Sub
End Class
