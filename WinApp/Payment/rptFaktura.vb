Imports DevExpress.XtraReports.UI
Imports Csla
Imports Csla.Data
Imports Csla.Validation
Imports System.Text.RegularExpressions
Public Class rptFaktura
  Private Broj As Integer = 0
  Private WithEvents _dokument As Csla.SortedBindingList(Of PrintPaymentDocumetnByIdDocumetnInfo)
  '  Private WithEvents _vehiclePrint As PrintVehcileList
  Private WithEvents _pomDoc As PrintPaymentDocumetnByIdDocumetnList
  Public Sub New(ByVal inDokumetn As PrintPaymentDocumetnByIdDocumetnList)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    _pomDoc = inDokumetn
    Broj = 1
    ' Add any initialization after the InitializeComponent() call.
    _dokument = New Csla.SortedBindingList(Of PrintPaymentDocumetnByIdDocumetnInfo)(inDokumetn)
    _dokument.ApplySort("VisibleOrder", System.ComponentModel.ListSortDirection.Ascending)
    ' _vehiclePrint = PrintVehcileList.GetPrintVehcileList(_dokument(0).IdVehicle)
    Me.BindingSource1.DataSource = _dokument
    If objCurentTehExamOrganization.LogoPath <> String.Empty Then
      pictureLogoTop.Image = Image.FromFile(objCurentTehExamOrganization.LogoPath)
      pictureLogoTop.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage
      'Me.Watermark.Image = Image.FromFile(objCurentTehExamOrganization.LogoPath)
      'Me.Watermark.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Zoom
      'Me.Watermark.ImageTransparency = 190
    Else
      ' Me.Watermark.Image = Nothing
      pictureLogoTop.Image = Nothing
    End If
    ' Dim organization As TehnicalExamOrganizationsInfo = _
    ' TehnicalExamOrganizationsList.GetTehnicalExamOrganizationsList. _
    'GetTehnicalExamOrganizationsInfoById(objCurentTehExamOrganization.Company)
    Dim city As CityInfo = CityList.GetCityList.GetCityListById(objCurentTehExamOrganization.IdCity)

    lblOIzdava.Text = UCase(objCurentTehExamOrganization.OrganizationName)
    lblOAddress.Text = objCurentTehExamOrganization.StationAddress

    lblTel.Text = "тел: " & objCurentTehExamOrganization.Tel & "   факс: " & objCurentTehExamOrganization.Fax
    'lblFax.Text = objCurentTehExamOrganization.Fax
    lblCityNameCode.Text = city.CityZip & " " & city.CityName
    lblSekretar.Text = objCurentTehExamOrganization.Sekretar
    lblEDB.Text = objCurentTehExamOrganization.EDB
    Dim _opstina As String = objCommunityList.GetCommunitiesListById(objCurentTehExamOrganization.IdCommunity).CommunityName
    lblOpstina.Text = _opstina
        ' lblOpstinaSud.Text = _opstina
        If _dokument.Item(0).IdCustomerFaktura > 0 Then
            Dim custInfo As CustomerInfoVeryShort = CustomerListVeryShort.GetCustomersListShortById(_dokument.Item(0).IdCustomerFaktura)
            lblToCustomerDisplay.Text = custInfo.CustomerSurname
            lblToAddressDisplay.Text = custInfo.LivingAddressNumber
            lblZip.Text = "" 'custInfo.CityZip
            lblToCity.Text = custInfo.CityName
        Else
            lblToCustomerDisplay.Text = _dokument.Item(0).CustomerDisplayName
            lblToAddressDisplay.Text = _dokument.Item(0).AddressDisplay
            lblZip.Text = _dokument.Item(0).CityZip
            lblToCity.Text = _dokument.Item(0).CityName
        End If
    If _dokument.Item(0).CityZip = 0 Then
      lblZip.Visible = False
    Else
      lblZip.Visible = True
    End If
    If (_dokument.Item(0).IdVehicle > 0) Then
      Dim pomRegBr As String = _dokument.Item(0).PoslednaRegZaSmetka()
      If pomRegBr <> "" Then
        lblRegNum.Text = pomRegBr
      Else
        lblRegNum.Text = _dokument.Item(0).RegistrationNumber
      End If
    Else
      lblRegNum.Text = ""
    End If
    Try
      ZiroSmetki()

    Catch ex As Exception

    End Try

    'lblODeponent.Text = objOpcii.Deponent
    'lblOEDB.Text = objOpcii.EDB
    'lblOFax.Text = objOpcii.Fax

    'lblOTel.Text = objOpcii.Tel
    'lblOZiroSmetka.Text = objOpcii.ZiroSmetka

    'If _dokument(0).IdVehicle < 1 Then
    '    XrPanel1.Visible = False
    'Else
    '    XrPanel1.Visible = True
    '    If _dokument(0).CategoryName <> String.Empty AndAlso _dokument(0).CategoryName <> "" Then
    '        lblVid.Visible = True
    '        lblVid1.Visible = True
    '    Else
    '        lblVid.Visible = False
    '        lblVid1.Visible = False
    '    End If
    '    If _dokument(0).ShellNumber <> String.Empty AndAlso _dokument(0).ShellNumber <> "" Then
    '        lblSasija.Visible = True
    '        lblSasija1.Visible = True
    '    Else
    '        lblSasija.Visible = False
    '        lblSasija1.Visible = False
    '    End If
    '    If _dokument(0).VehicleMakerModel <> String.Empty AndAlso _dokument(0).VehicleMakerModel <> "" Then
    '        lblMarkaTip.Visible = True
    '        lblMarkaTip1.Visible = True
    '    Else
    '        lblMarkaTip.Visible = False
    '        lblMarkaTip1.Visible = False
    '    End If
    'End If
    'Me.VehicleBindingSource.DataSource = _vehiclePrint
    Dim br As Integer = 0
    For i As Integer = 0 To _dokument.Count - 1
      br += _dokument.Item(i).VkupnoZaRed
    Next
    lblNumToText.Text = NumberToText.ConvertNum(br)


    ''ddv stapki 

    'Dim tbl As XRTable = New XRTable()
    'Dim rwPrazen As XRTableRow = New XRTableRow()
    'tbl.Width = 150
    'Dim clNamePrazen As XRTableCell = New XRTableCell
    'clNamePrazen.Text = ""
    'clNamePrazen.Borders = DevExpress.XtraPrinting.BorderSide.None
    'rwPrazen.Cells.Add(clNamePrazen)

    'Dim clValuePrazen As XRTableCell = New XRTableCell
    'clValuePrazen.Text = ""
    'clValuePrazen.Borders = DevExpress.XtraPrinting.BorderSide.None
    'rwPrazen.Cells.Add(clValuePrazen)

    'tbl.Location = New Point(Me.PageWidth - 40 - Me.Margins.Right - Me.Margins.Left - tbl.Width, 10)
    'Me.GroupFooter2.Controls.Add(tbl)

    '' tbl.Rows.Add(rwPrazen)

    'For Each strDDVStapka As String In _pomDoc.DDVSumi.Keys
    '  Dim rw As XRTableRow = New XRTableRow()
    '  tbl.Width = 150

    '  Dim clName As XRTableCell = New XRTableCell
    '  clName.Text = "ДДВ " & strDDVStapka
    '  clName.Borders = DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom
    '  rw.Cells.Add(clName)

    '  Dim clValue As XRTableCell = New XRTableCell
    '  clValue.Text = _pomDoc.DDVSumi.Item(strDDVStapka)
    '  clValue.Borders = DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom
    '  rw.Cells.Add(clValue)

    '  tbl.Location = New Point(Me.PageWidth - 40 - Me.Margins.Right - Me.Margins.Left - tbl.Width, 10)
    '  Me.GroupFooter2.Controls.Add(tbl)

    '  tbl.Rows.Add(rw)
    'Next
    ''  tbl.Rows.Add(rwPrazen)
    If lblSekretar.Text <> String.Empty Then
      lineSekretar.Visible = True
    Else
      lineSekretar.Visible = False
    End If
  End Sub

  Private Sub ZiroSmetki()
    Dim zSmetki As String = ""
    Dim vkupno As Integer = 0
    Dim vkupnoPola As Integer = 0
    ' Dim karakterPozicii() As Integer = Nothing
    zSmetki = objCurentTehExamOrganization.ZiroSmetka
    If zSmetki.Count > 1 Then
      Dim j As Integer = 0
      zSmetki = Replace(zSmetki, Char.ConvertFromUtf32(13), ",")
      zSmetki = Replace(zSmetki, ";,", ",")
      zSmetki = Replace(zSmetki, ",,", ",")
      For i As Integer = 0 To zSmetki.Count - 1
        If zSmetki(i) = ";" Or zSmetki(i) = "," Then
          vkupno += 1
        End If
      Next
      If zSmetki.Count > 0 AndAlso vkupno = 0 Then
        lblZiroSka1.Text = zSmetki
        Exit Sub
      End If
      Dim karakterPozicii(vkupno - 1) As Integer
      For i As Integer = 0 To zSmetki.Count - 1
        If zSmetki(i) = ";" Or zSmetki(i) = "," Then
          karakterPozicii(j) = i
          j += 1
        End If
      Next

      j -= 1


      If ((vkupno + 1) Mod 2) = 0 Then
        vkupnoPola = (vkupno + 1) / 2
      Else
        vkupnoPola = CType((vkupno + 1) / 2, Integer) + 1
      End If
      Dim pomtext1 As String = Trim(zSmetki.Substring(0, karakterPozicii(0)))
      Dim pomtext2 As String = ""
      For i As Integer = 0 To j
        'If vkupnoPola - 1 > 0 Then
        '    pomtext1 &= Chr(13)
        '    pomtext1 &= Trim(zSmetki.Substring(karakterPozicii(i) + 1, karakterPozicii(i + 1) - karakterPozicii(i) - 1))
        '    vkupnoPola -= 1
        'Else
        '    pomtext1 &= Chr(13)
        '    pomtext1 &= Trim(zSmetki.Substring(karakterPozicii(i) + 1, zSmetki.Length - karakterPozicii(i) - 1))
        '    If i = j Then
        '        pomtext2 &= Trim(zSmetki.Substring(karakterPozicii(i) + 1, zSmetki.Length - karakterPozicii(i) - 1))
        '    Else
        '        pomtext2 &= Trim(zSmetki.Substring(karakterPozicii(i) + 1, karakterPozicii(i + 1) - karakterPozicii(i) - 1))
        '        pomtext2 &= Chr(13)
        '    End If
        'End If
        If vkupnoPola - 1 > 0 Then
          pomtext1 &= Chr(13)
          pomtext1 &= Trim(zSmetki.Substring(karakterPozicii(i) + 1, karakterPozicii(i + 1) - karakterPozicii(i) - 1))
          vkupnoPola -= 1
        Else

          If i = j Then
            pomtext2 &= Trim(zSmetki.Substring(karakterPozicii(i) + 1, zSmetki.Length - karakterPozicii(i) - 1))
          Else
            pomtext2 &= Trim(zSmetki.Substring(karakterPozicii(i) + 1, karakterPozicii(i + 1) - karakterPozicii(i) - 1))
            pomtext2 &= Chr(13)
          End If
        End If

      Next
      lblZiroSka1.Text = pomtext1
      lblZiroSka2.Text = pomtext2
    End If
  End Sub

  'Private Sub redenBr_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles redenBr.BeforePrint
  '    Dim value As Integer = Convert.ToInt32(redenBr.Summary.GetResult())
  '    If value >= 4 Then
  '        Convert.ToInt32(redenBr.Summary.GetResult()) = value - 1
  '    End If
  'End Sub

  Private Sub cellBr_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles cellBr.BeforePrint
    'Dim value As Integer = Convert.ToInt32(redenBr.Summary.GetResult())
    'If value >= 4 Then
    '    cellBr.Text = value - 1
    'Else
    '    cellBr.Text = value
    'End If
    cellBr.Text = Broj
    Broj += 1
  End Sub
End Class