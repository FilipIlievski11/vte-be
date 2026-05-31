Public Class DDVPresmetki
  'Private _cena As Decimal
  'Private _ddvVrednost As Decimal
  'Private _kolicina As Decimal

#Region " Calculated properties "

  Public Shared Function EdinicnaCenaBezDDV(ByVal cena As Decimal, ByVal ddvVrednost As Integer) As Decimal

    Return Math.Round(cena / (1 + ddvVrednost / 100), 2)

  End Function

  Public Shared Function DanocnaOsnovica(ByVal cena As Decimal, ByVal ddvVrednost As Integer, ByVal kolicina As Decimal) As Decimal
    Return Math.Round(cena / (1 + ddvVrednost / 100) * kolicina, 2)
  End Function


  Public Shared Function DDVIsnos(ByVal cena As Decimal, ByVal ddvVrednost As Integer, ByVal kolicina As Decimal) As Decimal
    Return Math.Round((cena - cena / (1 + ddvVrednost / 100)) * kolicina, 2)
  End Function

  'Public Sub New(ByVal Cena As Decimal, ByVal ddvVrednost As Decimal, ByVal Kolicina As Decimal)
  '  Cena = Cena
  '  ddvVrednost = ddvVrednost
  '  Kolicina = Kolicina
  'End Sub

#End Region

End Class
