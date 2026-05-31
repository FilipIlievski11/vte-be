Public Module KondnaTastaturaModule
  Private _cyr() As Char = {"А", "Б", "В", "Г", "Д", "Е", "З", "И", "Ј", "К", "Л", "М", "Н", "О", _
                            "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "а", "б", "в", "г", "д", _
                            "е", "з", "и", "ј", "к", "л", "м", "н", "о", "п", "р", _
                            "с", "т", "у", "ф", "х", "ц"}
  Private _lat() As Char = {"A", "B", "V", "G", "D", "E", "Z", "I", "J", "K", "L", "M", "N", "O", _
                            "P", "R", "S", "T", "U", "F", "H", "C", "a", "b", "v", "g", "d", _
                            "e", "z", "i", "j", "k", "l", "m", "n", "o", "p", "r", _
                            "s", "t", "u", "f", "h", "c"}
  Public Function ToCyr(ByVal strInput As String) As String
    Dim result As String = String.Empty
    Try
      For Each ch As Char In strInput
        If Array.IndexOf(Of Char)(_lat, ch) > 0 Then
          result = result & _cyr(Array.IndexOf(_lat, ch))
        Else
          'vidi dali e prazno mesto, ili neso so ne e mapirano
        End If
      Next

    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

    Return result
  End Function

  Public Function ToLat(ByVal strInput As String) As String
    Dim result As String = String.Empty
    Try
      For Each ch As Char In strInput
        If Array.IndexOf(Of Char)(_cyr, ch) > 0 Then
          result = result & _lat(Array.IndexOf(_cyr, ch))
        Else
          'vidi dali e prazno mesto, ili neso so ne e mapirano
          Select Case ch.ToString
            Case " "
                            result = result & (" ") '.ToCharArray(0, 1)
            Case "Ѕ"
                            result = result & ("DZ") '.ToCharArray(0, 1)
            Case "ѕ"
                            result = result & ("dz") '.ToCharArray(0, 1)
            Case "Љ"
                            result = result & ("LJ") '.ToCharArray(0, 1)
            Case "љ"
                            result = result & ("lj") '.ToCharArray(0, 1)
            Case "Њ"
                            result = result & ("NJ") '.ToCharArray(0, 1)
            Case "њ"
                            result = result & ("nj") '.ToCharArray(0, 1)
            Case "Ѓ"
                            result = result & ("GJ") '.ToCharArray(0, 1)
            Case "ѓ"
                            result = result & ("gj") '.ToCharArray(0, 1)
            Case "Ж"
                            result = result & ("ZH") '.ToCharArray(0, 1)
            Case "ж"
                            result = result & ("zh") '.ToCharArray(0, 1)
            Case "ќ"
                            result = result & ("kj") '.ToCharArray(0, 1)
            Case "Ќ"
                            result = result & ("KJ") '.ToCharArray(0, 1)
            Case "Ч"
                            result = result & ("CH") '.ToCharArray(0, 1)
            Case "ч"
                            result = result & ("ch")
            Case "Ш"
                            result = result & ("SH") '.ToCharArray(0, 1)
            Case "ш"
                            result = result & ("sh") '.ToCharArray(0, 1)
            Case "Џ"
                            result = result & ("DJ") '.ToCharArray(0, 1)
            Case "џ"
                            result = result & ("dj") '.ToCharArray(0, 1)
            Case Else
              result = result & ch.ToString
          End Select
        End If
      Next

    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

    Return result
  End Function


End Module
