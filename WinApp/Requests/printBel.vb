Imports DevExpress.XtraReports.UI
Public Class printBel

  Private WithEvents _printBelList As printBelList
  Public ReadOnly Property PrintBel() As printBelList
    Get
      Return _printBelList
    End Get
  End Property
  Private WithEvents _requestTypeList As RequestTypeList
  Private WithEvents _requestType As RequestTypeInfo

#Region " Helpers "
  Private Sub SetupFirstRegistration()
    Try

      If _printBelList(0).CurrentVehicle(0).FirstRegistrationPlace = String.Empty Then
        XrLabel21.Visible = False
        XrLabel18FirstRegPlace.Visible = False
      End If
    Catch ex As Exception

    End Try
  End Sub
  Private Sub SetupVehicleCategories()
    Try
      Dim catInfo As VehicleCategoryForPayment = VehicleCategoryForPayment.GetVehicleCategoryForPayment(_printBelList(0).CurrentVehicle(0).IdVehicleCategoryForPayments)
      FindControl("BelVehicelCategory" & catInfo.BelMap.ToString, True).Visible = True


      '       Select Case _printBelList(0).CurrentVehicle(0).IdVehicleCategoryForPayments
      '           Case 6
      'BelVehicelCategory1.Visible = True
      '           Case 3
      'BelVehicelCategory4.Visible = True
      '           Case 8
      'BelVehicelCategory2.Visible = True
      '           Case 12, 13, 14
      'BelVehicelCategory5.Visible = True
      '       End Select
    Catch ex As Exception

    End Try
  End Sub

  Private Sub SetupRequestType()
    Try
            If _requestType.TypeName.Contains("œŒ¬“Œ–ÕŒ") Then
                XrLabel1.Visible = True
                XrLabel2.Visible = True
                XrLabel3.Visible = True
                XrLabel4.Visible = True
                XrLabel5.Visible = True
                XrLabel6.Visible = True
                XrLabel7.Visible = True
                XrLabel9.Visible = True
            Else
                XrLabel1.Visible = False
                XrLabel2.Visible = False
                XrLabel3.Visible = False
                XrLabel4.Visible = False
                XrLabel5.Visible = False
                XrLabel6.Visible = False
                XrLabel7.Visible = False
                XrLabel9.Visible = False
            End If
      FindControl("CheckBoxDocTyprOption" & _requestType.TypeName.Chars(0).ToString, True).Visible = True
      If Not _requestType.IsNewCustomer Then
        XrLabel38.Visible = False
                'XrLabel39.Visible = False
                'XrLabel40.Visible = False
        XrLabel1.Visible = False
                XrLabel2.Visible = False
                XrLabel3.Visible = False
                XrLabel4.Visible = False
                XrLabel5.Visible = False
                XrLabel6.Visible = False
                XrLabel7.Visible = False
                XrLabel9.Visible = False
      Else
        XrLabel38.Visible = True
                'XrLabel39.Visible = True
                'XrLabel40.Visible = True
        XrLabel1.Visible = False
        XrLabel2.Visible = False
      End If
      'If Not _requestType.TypeName.Contains("5") Then
      '  XrLabel1.Visible = False
      '  XrLabel2.Visible = False
      'End If

    Catch ex As Exception

    End Try
  End Sub
#End Region

  Public Sub New(ByVal printBelList As printBelList)
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    Me.Margins.Top = objOpcii.BelTopMargin
    Me.Margins.Left = objOpcii.BelLeftMargin
    Me.Margins.Right = objOpcii.BelRightmargin
    Me.Margins.Bottom = objOpcii.BelButtonMargin



    _requestTypeList = RequestTypeList.GetRequestTypeList
    _printBelList = printBelList
    _requestType = _requestTypeList.getInfoById(_printBelList.Item(0).IdRequestType)
    Me.printBelListBindingSource.DataSource = _printBelList
    'Dim pom As String = ""
    'If _printBelList.Item(0).CurrentVehicle.Item(0).LastRegIdCommunity > 0 Then
    '    pom = objCommunityList.GetCommunitiesListById(_printBelList.Item(0).CurrentVehicle.Item(0).LastRegIdCommunity).RegistrationCode
    'Else
    '    pom = objCommunityList.GetCommunitiesListById(objCurentTehExamOrganization.IdCommunity).RegistrationCode
    'End If
    Dim pom As CommunitiesInfo
    pom = objCommunityList.GetCommunitiesListById(_printBelList.Item(0).NewOwner.Item(0).IdCommunityCode)

    'If _printBelList.Item(0).CurrentVehicle.Item(0).LastRegIdCommunity > 0 Then

    '    lblToOrganization.Text = (_printBelList.Item(0).CurrentVehicle.Item(0).LastRegIssuer)
    'Else
    '    lblToOrganization.Text = objRegistrationIssuerList.GetRegistrationIssuerInfo _
    '    (objCurentTehExamOrganization.IdDefaultRegistrationIssuer).IssuerName
    'End If
    If _requestType.IsNewRegistration Then

      ' Dim pom As String = objCommunityList.GetCommunitiesListById(objCurentTehExamOrganization.IdCommunity).RegistrationCode

      lblNovaReg.Text = pom.RegistrationCode & "-"
      Try
        lblToOrganization.Text = objRegistrationIssuerList.GetRegistrationIssuerInfoByCommunity _
            (pom.Id).IssuerName
      Catch ex As Exception
        lblToOrganization.Text = ""
      End Try

    Else
            lblNovaReg.Text = _printBelList.Item(0).RegistrationNumberPrevios
            lblDatumReg.Text = _printBelList.Item(0).DateOfRegistration.Date
            lblFirstRegDate.Text = _printBelList.Item(0).CurrentVehicle.Item(0).FirstRegistrationDate
            lblFirstRegistrationDate.Text = _printBelList.Item(0).CurrentVehicle.Item(0).FirstRegistrationDate


      lblToOrganization.Text = (_printBelList.Item(0).CurrentVehicle.Item(0).LastRegIssuer)
    End If
    SetupVehicleCategories()
    SetupRequestType()
        SetupFirstRegistration()
        SetupCustomer()
    'If _customers.Item(0).IsCompany Then
    '  lblCompanyName.Text = _customers.Item(0).CustomerFirstName
    'Else
    '  lblCompanyName.Text = "‘»«»◊ Œ À»÷≈"
    'End If
    'Me.BindingSourceDocument.DataSource = _document
    'Me.BindingSourceVehicle.DataSource = _vehicles
    'Me.BindingSourceCustomer.DataSource = _customers

    'Dim _lastTehnicalExam As VehicleLastTehnicalExamList = _
    'VehicleLastTehnicalExamList.GetVehicleLastTehnicalExamList(_vehicles.Id)
    'Me.BindingSourceLastTehnicalExam.DataSource = _lastTehnicalExam

    'Dim powerSources As VehicleEnginePowerSourceTypeList = _
    'VehicleEnginePowerSourceTypeList.GetVehicleEnginePowerSourceTypeList

    'lblPrimaryPowerSource.Text = powerSources.GetPowerSourceTypeInfo _
    '(_vehicles.IdEnginePowerSource).PowerSourceName
    'lblSecondaryPowerSource.Text = powerSources.GetPowerSourceTypeInfo _
    '(_vehicles.IdEngineSecondPowerSource).PowerSourceName

    'If lblSecondaryPowerSource.Text = "[ÌÂÏ‡]" Then
    '  lblSecondaryPowerSource.Visible = False
    'End If

    'Select Case indexOptions
    '  Case 0
    '    CheckBoxDocTyprOption1.Visible = True
    '  Case 1
    '    CheckBoxDocTyprOption2.Visible = True
    '  Case 2
    '    CheckBoxDocTyprOption3.Visible = True
    '  Case 3
    '    CheckBoxDocTyprOption4.Visible = True
    '  Case 4
    '    CheckBoxDocTyprOption5.Visible = True
    'End Select

    'If _vehicles.IdVehicleCategories > 0 Then
    '  lblVehicleCtegory.Text = VehicleCategoryList.GetVehicleCategoryList.GetVehicleCategoryInfo _
    '  (_vehicles.IdVehicleCategories).Category
    'Else
    '  lblVehicleCtegory.Text = ""
    'End If

    'If _vehicles.IdMadeCountry > 0 Then
    '  lblCountryMade.Text = CountriesList.GetCountriesList.GetCountriesListById _
    '  (_vehicles.IdMadeCountry).CountryName
    'Else
    '  lblCountryMade.Text = ""
    'End If

    'If _vehicles.FirstRegistration = "ÌÂÔÓÁÌ‡Ú‡" Then
    '  XrLabel21.Visible = False
    '  XrLabel18.Visible = False
    'End If

  End Sub

  Public Function GetStringWidth(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) As Single
    Dim factor As Int32

    Dim gr As Graphics = Graphics.FromHwnd(IntPtr.Zero)
    If Me.ReportUnit = ReportUnit.HundredthsOfAnInch Then
      gr.PageUnit = GraphicsUnit.Inch
      factor = 100
    Else
      gr.PageUnit = GraphicsUnit.Millimeter
      factor = 11
    End If

    Dim size As SizeF = gr.MeasureString(CType(sender, XRLabel).Text, CType(sender, XRLabel).Font)
    Dim tempgolemina As Single = CType(sender, XRLabel).Font.Size
    While size.Width * factor > CType(sender, XRLabel).Width()
      tempgolemina = CType(sender, XRLabel).Font.Size
      CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, tempgolemina - 1)
      size = gr.MeasureString(CType(sender, XRLabel).Text, CType(sender, XRLabel).Font)
    End While

    gr.Dispose()
    Return tempgolemina
  End Function

    Private Sub XrLabel_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs)
        Try
            Dim golemina As Single = GetStringWidth(sender, e)
            CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Regular)
        Catch ex As Exception

        End Try

    End Sub
    Private Sub SetupCustomer()

        If _printBelList(0).NewOwner(0).IsCompany Then
            Me.PanelPravnoLice.Visible = True
            Me.PanelOwner.Visible = False
        Else
            Me.PanelOwner.Visible = True
            Me.PanelPravnoLice.Visible = False
        End If
    End Sub
    'Private Sub SetupRequestType()

    '    If _requestType.TypeName.Contains("œŒ¬“Œ–ÕŒ") Then
    '        CheckBoxPovtorno.Visible = True
    '        panelPreviosRegistrationOwner.Visible = True
    '    End If
    '    If _requestType.TypeName.Contains("œ–»¬–≈Ã≈ÕŒ") Then
    '        CheckBoxPrivremeno.Visible = True
    '    End If
    '    If _requestType.TypeName.Contains("œ–ŒÃ≈Õ»") Then
    '        CheckBoxTehnickiPromeni.Visible = True
    '    End If

    'End Sub
End Class