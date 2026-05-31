Public Class dijPromenNaDatumNaFiskalna 

  Private Sub dijPromenNaDatumNaFiskalna_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Me.DateEdit1.DateTime = Now
  End Sub

  Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
    Dim strDatum As String = String.Empty
    strDatum = Format(DateEdit1.DateTime.Day, "00") & "-" & _
      Format(DateEdit1.DateTime.Month, "00") & "-" & _
      Strings.Right(CStr(DateEdit1.DateTime.Year), 2) & " " & _
      Format(DateEdit1.DateTime.Hour, "00") & ":" & _
      Format(DateEdit1.DateTime.Minute, "00")

    PromenaNaDatumPF500(strDatum)
    Me.Close()
  End Sub

  Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    Me.Close()
  End Sub
End Class