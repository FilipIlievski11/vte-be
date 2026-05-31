Module MaticenBroj
  Public Function CheckMaticenBroj(ByVal Broj As String) As Boolean

    If Not Len(Broj) = 13 Then
      CheckMaticenBroj = False
      Exit Function
    End If
    Dim tmp As Integer

    Try

      Dim A As Integer = CInt(Mid(Broj, 1, 1))
      Dim B As Integer = CInt(Mid(Broj, 2, 1))
      Dim C As Integer = CInt(Mid(Broj, 3, 1))
      Dim D As Integer = CInt(Mid(Broj, 4, 1))
      Dim E As Integer = CInt(Mid(Broj, 5, 1))
      Dim F As Integer = CInt(Mid(Broj, 6, 1))
      Dim G As Integer = CInt(Mid(Broj, 7, 1))
      Dim H As Integer = CInt(Mid(Broj, 8, 1))
      Dim I As Integer = CInt(Mid(Broj, 9, 1))
      Dim J As Integer = CInt(Mid(Broj, 10, 1))
      Dim K As Integer = CInt(Mid(Broj, 11, 1))
      Dim L As Integer = CInt(Mid(Broj, 12, 1))
      Dim M As Integer = CInt(Mid(Broj, 13, 1))

      tmp = 11 - (7 * (A + G) + 6 * (B + H) + 5 * (C + I) + 4 * (D + J) + 3 * (E + K) + 2 * (F + L)) Mod 11

      If tmp = 10 Then
        CheckMaticenBroj = False
        Exit Function
      End If
      If tmp = 11 Then tmp = 0

      CheckMaticenBroj = (M = tmp)

    Catch ex As Exception
      CheckMaticenBroj = False
      Exit Function
    End Try




  End Function
End Module
