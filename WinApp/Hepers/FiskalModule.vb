Module FiskalModule


#Region " Accent fiskalen printer PF500 "



  Public Sub PecatiDetalenIzvPoDatumePF500(ByVal strStartDate As String, ByVal strEndDate As String)

    Dim strFileName As String = objOpcii.FiskalFolderPath & "\" & "SkratenIzvestajDatum" & ".tx"

    Dim strSmetka = " " & Chr(94) & strStartDate & "," & strEndDate & vbCrLf

    My.Computer.FileSystem.WriteAllText(strFileName, strSmetka, False, System.Text.Encoding.ASCII)

    My.Computer.FileSystem.RenameFile(strFileName, "SkratenIzvestajDatum" & ".txt")

    'Shell(objOpcii.FiskalFolderPath & "\Fiscal32.exe " & strFileName, AppWinStyle.Hide, True)
  End Sub

  Public Sub PecatiSkratenIzvPoDatumePF500(ByVal strStartDate As String, ByVal strEndDate As String)

    Dim strFileName As String = objOpcii.FiskalFolderPath & "\" & "SkratenIzvestajDatum" & ".tx"

    Dim strSmetka = " " & Chr(79) & strStartDate & "," & strEndDate & vbCrLf

    My.Computer.FileSystem.WriteAllText(strFileName, strSmetka, False, System.Text.Encoding.ASCII)

    My.Computer.FileSystem.RenameFile(strFileName, "SkratenIzvestajDatum" & ".txt")

    'Shell(objOpcii.PatekaDoFiskalenFolder & "\Fiscal32.exe " & strFileName, AppWinStyle.Hide, True)
  End Sub

  Public Sub PromenaNaDatumPF500(ByVal datum As String)

    Dim strFileName As String = objOpcii.FiskalFolderPath & "\" & "PromenaNaDatum" & ".tx"

    Dim strSmetka = " " & Chr(61) & datum & vbCrLf

    My.Computer.FileSystem.WriteAllText(strFileName, strSmetka, False, System.Text.Encoding.ASCII)

    My.Computer.FileSystem.RenameFile(strFileName, "PromenaNaDatum" & ".txt")

    'Shell(objOpcii.PatekaDoFiskalenFolder & "\Fiscal32.exe " & strFileName, AppWinStyle.Hide, True)
  End Sub

  Public Sub PecatiDnevenKontrolenIzvestajPF500()

    Dim strFileName As String = objOpcii.FiskalFolderPath & "\" & "DnevenKontrolenIzvestaj" & ".tx"

    'Dim strSmetka = " " & Chr(69) & "3" & vbCrLf

    Dim strSmetka = " " & Chr(69) & "2" & vbCrLf

    My.Computer.FileSystem.WriteAllText(strFileName, strSmetka, False, System.Text.Encoding.ASCII)

    My.Computer.FileSystem.RenameFile(strFileName, "DnevenKontrolenIzvestaj" & ".txt")

    'Shell(objOpcii.PatekaDoFiskalenFolder & "\Fiscal32.exe " & strFileName, AppWinStyle.Hide, True)
  End Sub

  Public Sub PecatiDnevnoFiskalnoZatvaranjePF500()

        Dim strFileName As String = objOpcii.FiskalFolderPath & "\" & "DnevnoFiskalnoZatvaranje" & ".tx"

        Dim strSmetka = " " & Chr(69) & vbCrLf

        My.Computer.FileSystem.WriteAllText(strFileName, strSmetka, False, System.Text.Encoding.ASCII)

        My.Computer.FileSystem.RenameFile(strFileName, "DnevnoFiskalnoZatvaranje" & ".txt")

    'Shell(objOpcii.PatekaDoFiskalenFolder & "\Fiscal32.exe " & strFileName, AppWinStyle.Hide, True)
  End Sub


  Public Sub SluzbenoVnesuvanjePariPF500(ByVal intAmount As Integer)
    If intAmount = 0 Then Exit Sub

    Dim strFileName As String = objOpcii.FiskalFolderPath & "\" & "SluzbenoVnesuvanjePari" & ".tx"

    Dim strSmetka = " " & Chr(70) & intAmount

    My.Computer.FileSystem.WriteAllText(strFileName, strSmetka, False, System.Text.Encoding.ASCII)

    My.Computer.FileSystem.RenameFile(strFileName, "SluzbenoVnesuvanjePari" & ".txt")

    'Shell(objOpcii.FiskalFolderPath & "\Fiscal32.exe " & strFileName, AppWinStyle.Hide, True)
  End Sub

  Public Sub SluzbenoVadenjePariPF500(ByVal intAmount As Integer)
    If intAmount = 0 Then Exit Sub

    Dim strFileName As String = objOpcii.FiskalFolderPath & "\" & "SluzbenoVadenjePari" & ".tx"

    Dim strSmetka = " " & Chr(70) & -intAmount

    My.Computer.FileSystem.WriteAllText(strFileName, strSmetka, False, System.Text.Encoding.ASCII)

    My.Computer.FileSystem.RenameFile(strFileName, "SluzbenoVadenjePari" & ".txt")

    'Shell(objOpcii.FiskalFolderPath & "\Fiscal32.exe " & strFileName, AppWinStyle.Hide, True)
  End Sub

  Public Sub PecatiFiskalnaSmetaAccentPF500(ByVal idDocumnet As Long)
    Try
      Dim boDocument As PaymentDocumentFiscalPrintList = _
        PaymentDocumentFiscalPrintList.GetPaymentDocumentFiscalPrintList(idDocumnet)
      Dim plak As PaymentTypeInfo = _
        PaymentTypeList.GetPaymentTypeList().GetPaymentTypeInfoById(boDocument(0).IdPaymentType)
      If Not plak.FiskalnaKes Then Exit Sub

      'izlezi ako ne se plaka danok
      If boDocument.GetTotalAmmount = 0 Then
        Exit Sub
      End If

      Dim strSmetka As String = String.Empty
      'zaglavje
      If boDocument(0).Storno Then
        ' U1,0000,1
        strSmetka &= " U1,0000,1"
      Else
        '01,0000,1
        'so ova se otvara fiskalna smetka
        strSmetka &= " 01,0000,1"
      End If
      strSmetka &= vbCrLf

      'registriranje(prodazba) na stoka
            For Each item As PaymentDocumentFiscalPrintInfo In boDocument

                Dim cenaSoPopust As Double
                cenaSoPopust = Math.Round(item.Price - (item.Price * item.Discount / 100), 0)

                Dim ddvStapkaASC As Integer = 0
                Select Case item.DDV
                    Case 18
                        'If objArtiklZemjaNaPotekloList.IsMaticnaZemja(item.IdZemjaNaPoteklo) Then
                        'asc na latinica A
                        ddvStapkaASC = 192 '65
                        'Else
                        '  'asc na kirilica А
                        '  ddvStapkaASC = 192
                        'End If
                    Case 5
                        'If objArtiklZemjaNaPotekloList.IsMaticnaZemja(item.IdZemjaNaPoteklo) Then
                        'asc na latinica B
                        ddvStapkaASC = 193 '66
                        'Else
                        ''asc na kirilica Б
                        'ddvStapkaASC = 193
                        'End If
                    Case 0
                        ddvStapkaASC = 194
                    Case Else
                        'ne se pecati ne se odanocuva
                        Exit For
                End Select

                Dim payCatalog As PaymentCataologList = PaymentCataologList.GetPaymentCataologList()
                Dim payIteam As PaymentCataologInfo = payCatalog.GetInfoByIdPaymentParametar(item.IdPriceCatalog)

                'chr(192) e A e 18%

                'nekkoj brojac
                If boDocument.IndexOf(item) Mod 2 = 0 Then
                    strSmetka &= "'1"
                Else
                    strSmetka &= " 1"
                End If
                strSmetka &= _
                Strings.Left(ToLat(payIteam.CategoryName), 24) & _
                vbTab & _
                Chr(ddvStapkaASC).ToString & _
                FormatNumber(cenaSoPopust, 2, TriState.False, TriState.False, TriState.False).ToString.Replace(",", ".") '& _
                '"*" & _
                'FormatNumber(1, 3, TriState.False, TriState.False, TriState.False).ToString.Replace(",", ".")

                strSmetka &= vbCrLf
            Next

      '5 znak za ???
      strSmetka &= " " & Chr(53) & " Smetka" & vbTab & vbCrLf


      Dim boolPecatiSmetka As Boolean = True

      'Калкулација на вкупна сума(Плаќање).
      'If Not boDocument(0).Storno Then




      '  If plak.FiskalnaKes Then
      '    'P
      '    strSmetka &= "Vo gotovo" & vbTab & Chr(80)
      '    boolPecatiSmetka = True
      '    GoTo PLAKANJE
      '  End If
      '  'If plak.FiskalnaSmetkaNaKredit Then
      '  '  'N
      '  '  strSmetka &= "Na kredit" & vbTab & Chr(78)
      '  '  boolPecatiSmetka = True
      '  '  GoTo PLAKANJE
      '  'End If
      '  'If plak.FiskalnaSmetkaNaCek Then
      '  '  'C
      '  '  strSmetka &= "Na `ek" & vbTab & Chr(67)
      '  '  boolPecatiSmetka = True
      '  '  GoTo PLAKANJE
      '  'End If
      '  If plak.FiskalnaKarticka Then
      '    'D
      '    strSmetka &= "So karitica" & vbTab & Chr(68)
      '    boolPecatiSmetka = True
      '    GoTo PLAKANJE
      '  End If
      'Else
      '  strSmetka &= vbCrLf
      'End If


PLAKANJE:
      'If Not boDocument(0).Storno Then
      '  strSmetka &= FormatNumber(boDocument.GetTotalAmmount, 2, TriState.False, TriState.False, TriState.False).ToString.Replace(",", ".") & vbCrLf
      'End If

      'zatvarenje na smetka
      If boDocument(0).Storno Then
        strSmetka &= "%" & Chr(86)
      Else
        'so ova se otvara fiskalna smetka
        strSmetka &= "%" & Chr(56)
      End If

      strSmetka &= vbCrLf
      If boolPecatiSmetka Then
        'napisi go fajlot
        Dim strFileName As String = objOpcii.FiskalFolderPath & "\" & "smetkaID" & idDocumnet & ".tx"
        My.Computer.FileSystem.WriteAllText(strFileName, strSmetka, False, System.Text.Encoding.Default)

        'povikaj go fiskalniot driver
        'Dim procID As Integer
        'Dim newProc As New Diagnostics.Process()

        'newProc.StartInfo.CreateNoWindow = False
        'newProc.StartInfo.Arguments = strFileName
        'newProc.StartInfo.FileName = objOpcii.PatekaDoFiskalenFolder & "\Fiscal32.exe"
        'newProc.Start()

                My.Computer.FileSystem.RenameFile(strFileName, "smetkaID" & idDocumnet & ".txt")

        'Shell(objOpcii.PatekaDoFiskalenFolder & "\Fiscal32.exe " & strFileName, AppWinStyle.Hide, True)

        'procID = newProc.Id
        'newProc.WaitForExit()

        'Kill(strFileName)

      End If

    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

    Public Sub PecatiFiskalnaSmetaZaRataAccentPF500(ByVal idRata As Long)
        Try
            Dim boDocumentRata As PaymentDocumentRataFiscalPrintList = _
              PaymentDocumentRataFiscalPrintList.GetPaymentDocumentRataFiscalPrintList(idRata, True)
            
            'izlezi ako ne se plaka danok
            'If boDocument.GetTotalAmmount = 0 Then
            '    Exit Sub
            'End If

            Dim strSmetka As String = String.Empty
            'zaglavje
            If boDocumentRata(0).Storno Then
                ' U1,0000,1
                strSmetka &= " U1,0000,1"
            Else
                '01,0000,1
                'so ova se otvara fiskalna smetka
                strSmetka &= " 01,0000,1"
            End If
            strSmetka &= vbCrLf

            'registriranje(prodazba) na stoka
            For Each item As PaymentDocumentRataFiscalPrintInfo In boDocumentRata
                Dim cenaSoPopust As Double
                cenaSoPopust = item.Price

                Dim ddvStapkaASC As Integer = 0
             
                ddvStapkaASC = 194 'ddv 0%


                'chr(192) e A e 18%

                'nekkoj brojac
                If boDocumentRata.IndexOf(item) Mod 2 = 0 Then
                    strSmetka &= "'1"
                Else
                    strSmetka &= " 1"
                End If
                strSmetka &= _
                Strings.Left(ToLat("Uplata po rata"), 24) & _
                vbTab & _
                Chr(ddvStapkaASC).ToString & _
                FormatNumber(cenaSoPopust, 2, TriState.False, TriState.False, TriState.False).ToString.Replace(",", ".") '& _
                '"*" & _
                'FormatNumber(1, 3, TriState.False, TriState.False, TriState.False).ToString.Replace(",", ".")

                strSmetka &= vbCrLf
            Next

            '5 znak za ???
            strSmetka &= " " & Chr(53) & " Smetka" & vbTab & vbCrLf


            Dim boolPecatiSmetka As Boolean = True

            'Калкулација на вкупна сума(Плаќање).
            'If Not boDocument(0).Storno Then




            '  If plak.FiskalnaKes Then
            '    'P
            '    strSmetka &= "Vo gotovo" & vbTab & Chr(80)
            '    boolPecatiSmetka = True
            '    GoTo PLAKANJE
            '  End If
            '  'If plak.FiskalnaSmetkaNaKredit Then
            '  '  'N
            '  '  strSmetka &= "Na kredit" & vbTab & Chr(78)
            '  '  boolPecatiSmetka = True
            '  '  GoTo PLAKANJE
            '  'End If
            '  'If plak.FiskalnaSmetkaNaCek Then
            '  '  'C
            '  '  strSmetka &= "Na `ek" & vbTab & Chr(67)
            '  '  boolPecatiSmetka = True
            '  '  GoTo PLAKANJE
            '  'End If
            '  If plak.FiskalnaKarticka Then
            '    'D
            '    strSmetka &= "So karitica" & vbTab & Chr(68)
            '    boolPecatiSmetka = True
            '    GoTo PLAKANJE
            '  End If
            'Else
            '  strSmetka &= vbCrLf
            'End If


PLAKANJE:
            'If Not boDocument(0).Storno Then
            '  strSmetka &= FormatNumber(boDocument.GetTotalAmmount, 2, TriState.False, TriState.False, TriState.False).ToString.Replace(",", ".") & vbCrLf
            'End If

            'zatvarenje na smetka
            If boDocumentRata(0).Storno Then
                strSmetka &= "%" & Chr(86)
            Else
                'so ova se otvara fiskalna smetka
                strSmetka &= "%" & Chr(56)
            End If

            strSmetka &= vbCrLf
            If boolPecatiSmetka Then
                'napisi go fajlot
                Dim strFileName As String = objOpcii.FiskalFolderPath & "\" & "smetkaID" & boDocumentRata.Item(0).Idpaymentdocument '& "RataId" & idRata & ".tx"
                My.Computer.FileSystem.WriteAllText(strFileName, strSmetka, False, System.Text.Encoding.Default)

                'povikaj go fiskalniot driver
                'Dim procID As Integer
                'Dim newProc As New Diagnostics.Process()

                'newProc.StartInfo.CreateNoWindow = False
                'newProc.StartInfo.Arguments = strFileName
                'newProc.StartInfo.FileName = objOpcii.PatekaDoFiskalenFolder & "\Fiscal32.exe"
                'newProc.Start()

                My.Computer.FileSystem.RenameFile(strFileName, "smetkaID" & boDocumentRata.Item(0).Idpaymentdocument & ".txt")

                'Shell(objOpcii.PatekaDoFiskalenFolder & "\Fiscal32.exe " & strFileName, AppWinStyle.Hide, True)

                'procID = newProc.Id
                'newProc.WaitForExit()

                'Kill(strFileName)

            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub PecatiFiskalnaStornoZaRataAccentPF500(ByVal idRata As Long)
        Try
            Dim boDocumentRata As PaymentDocumentRataFiscalPrintList = _
              PaymentDocumentRataFiscalPrintList.GetPaymentDocumentRataFiscalPrintList(idRata, True)

            'izlezi ako ne se plaka danok
            'If boDocument.GetTotalAmmount = 0 Then
            '    Exit Sub
            'End If

            Dim strSmetka As String = String.Empty
            'zaglavje

            ' U1,0000,1
            strSmetka &= " U1,0000,1"
           
            strSmetka &= vbCrLf

            'registriranje(prodazba) na stoka
            For Each item As PaymentDocumentRataFiscalPrintInfo In boDocumentRata
                Dim cenaSoPopust As Double
                cenaSoPopust = item.Price

                Dim ddvStapkaASC As Integer = 0

                ddvStapkaASC = 194


                'chr(192) e A e 18%

                'nekkoj brojac
                If boDocumentRata.IndexOf(item) Mod 2 = 0 Then
                    strSmetka &= "'1"
                Else
                    strSmetka &= " 1"
                End If
                strSmetka &= _
                Strings.Left(ToLat("Uplata po rata"), 24) & _
                vbTab & _
                Chr(ddvStapkaASC).ToString & _
                FormatNumber(cenaSoPopust, 2, TriState.False, TriState.False, TriState.False).ToString.Replace(",", ".") '& _
                '"*" & _
                'FormatNumber(1, 3, TriState.False, TriState.False, TriState.False).ToString.Replace(",", ".")

                strSmetka &= vbCrLf
            Next

            '5 znak za ???
            strSmetka &= " " & Chr(53) & " Smetka" & vbTab & vbCrLf


            Dim boolPecatiSmetka As Boolean = True

            'Калкулација на вкупна сума(Плаќање).
            'If Not boDocument(0).Storno Then




            '  If plak.FiskalnaKes Then
            '    'P
            '    strSmetka &= "Vo gotovo" & vbTab & Chr(80)
            '    boolPecatiSmetka = True
            '    GoTo PLAKANJE
            '  End If
            '  'If plak.FiskalnaSmetkaNaKredit Then
            '  '  'N
            '  '  strSmetka &= "Na kredit" & vbTab & Chr(78)
            '  '  boolPecatiSmetka = True
            '  '  GoTo PLAKANJE
            '  'End If
            '  'If plak.FiskalnaSmetkaNaCek Then
            '  '  'C
            '  '  strSmetka &= "Na `ek" & vbTab & Chr(67)
            '  '  boolPecatiSmetka = True
            '  '  GoTo PLAKANJE
            '  'End If
            '  If plak.FiskalnaKarticka Then
            '    'D
            '    strSmetka &= "So karitica" & vbTab & Chr(68)
            '    boolPecatiSmetka = True
            '    GoTo PLAKANJE
            '  End If
            'Else
            '  strSmetka &= vbCrLf
            'End If


PLAKANJE:
            'If Not boDocument(0).Storno Then
            '  strSmetka &= FormatNumber(boDocument.GetTotalAmmount, 2, TriState.False, TriState.False, TriState.False).ToString.Replace(",", ".") & vbCrLf
            'End If

            'zatvarenje na smetka
            'If boDocumentRata(0).Storno Then
            strSmetka &= "%" & Chr(86)
            'Else
            ''so ova se otvara fiskalna smetka
            'strSmetka &= "%" & Chr(56)
            'End If

            strSmetka &= vbCrLf
            If boolPecatiSmetka Then
                'napisi go fajlot
                Dim strFileName As String = objOpcii.FiskalFolderPath & "\" & "smetkaID" & boDocumentRata.Item(0).Idpaymentdocument '& "RataId" & idRata & ".tx"
                My.Computer.FileSystem.WriteAllText(strFileName, strSmetka, False, System.Text.Encoding.Default)

                'povikaj go fiskalniot driver
                'Dim procID As Integer
                'Dim newProc As New Diagnostics.Process()

                'newProc.StartInfo.CreateNoWindow = False
                'newProc.StartInfo.Arguments = strFileName
                'newProc.StartInfo.FileName = objOpcii.PatekaDoFiskalenFolder & "\Fiscal32.exe"
                'newProc.Start()

                My.Computer.FileSystem.RenameFile(strFileName, "smetkaID" & boDocumentRata.Item(0).Idpaymentdocument & ".txt")

                'Shell(objOpcii.PatekaDoFiskalenFolder & "\Fiscal32.exe " & strFileName, AppWinStyle.Hide, True)

                'procID = newProc.Id
                'newProc.WaitForExit()

                'Kill(strFileName)

            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region

End Module
