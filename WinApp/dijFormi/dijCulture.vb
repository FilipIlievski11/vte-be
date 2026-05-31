Public Class dijCulture 


  Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
    uSettings.Culture = ComboBoxEdit1.Text
    uSettings = uSettings.Save
    ChangeCulture(uSettings.Culture)
    MsgBox(My.Resources.msgRestartTheProgram)
    Me.Close()
  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
  End Sub

  Private Sub dijCulture_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    ComboBoxEdit1.Text = uSettings.Culture
  End Sub
End Class