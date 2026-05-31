Public Class dijVremenskiFiskalniIzvestaii 

  Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    Me.Close()

  End Sub

  Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
    Dim startDatum As String
    Dim endDatum As String

    startDatum = Format(deStartDate.DateTime.Day, "00") & _
      Format(deStartDate.DateTime.Month, "00") & _
      Strings.Right(CStr(deStartDate.DateTime.Year), 2)

    endDatum = Format(deEndDate.DateTime.Day, "00") & _
      Format(deEndDate.DateTime.Month, "00") & _
      Strings.Right(CStr(deEndDate.DateTime.Year), 2)

    PecatiDetalenIzvPoDatumePF500(startDatum, endDatum)


    Me.Close()
  End Sub

  Private Sub btnSkraten_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSkraten.Click
    Dim startDatum As String
    Dim endDatum As String

    startDatum = Format(deStartDate.DateTime.Day, "00") & _
      Format(deStartDate.DateTime.Month, "00") & _
      Strings.Right(CStr(deStartDate.DateTime.Year), 2)

    endDatum = Format(deEndDate.DateTime.Day, "00") & _
      Format(deEndDate.DateTime.Month, "00") & _
      Strings.Right(CStr(deEndDate.DateTime.Year), 2)

    PecatiSkratenIzvPoDatumePF500(startDatum, endDatum)
    Me.Close()
  End Sub

  Private Sub dijVremenskiFiskalniIzvestaii_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Me.deEndDate.DateTime = Now
    Me.deStartDate.DateTime = Now
  End Sub
End Class