Public Class PrintPlav

  Private WithEvents _printPlavList As PrintPlavList
  Public ReadOnly Property PrintPlavList() As PrintPlavList
    Get
      Return _printPlavList
    End Get
  End Property
  Private WithEvents _requestTypeList As RequestTypeList
  Private WithEvents _requestType As RequestTypeInfo

  Public Sub New(ByVal printPlavList As PrintPlavList)
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    Me.Margins.Top = objOpcii.PlavTopMargin
    Me.Margins.Left = objOpcii.PlavLeftMargin
    Me.Margins.Right = objOpcii.PlavRightMargin
        Me.Margins.Bottom = objOpcii.PlavButtonMargin
      

    _requestTypeList = RequestTypeList.GetRequestTypeList
    _printPlavList = printPlavList
    _requestType = _requestTypeList.getInfoById(_printPlavList.Item(0).IdRequestType)
    Me.PrintPlavBindingSource.DataSource = _printPlavList
        Me.panelPreviosRegistrationOwner.Visible = _requestType.IsPreviosRegistrationReqired
        'Dim pom As String = ""
        'If _printPlavList.Item(0).CurrentVehicle.Item(0).LastRegIdCommunity > 0 Then
        '    pom = objCommunityList.GetCommunitiesListById(_printPlavList.Item(0).CurrentVehicle.Item(0).LastRegIdCommunity).RegistrationCode
        'Else
        '    pom = objCommunityList.GetCommunitiesListById(objCurentTehExamOrganization.IdCommunity).RegistrationCode
        'End If
        'If _printPlavList.Item(0).CurrentVehicle.Item(0).LastRegIdCommunity > 0 Then
        '    lblToOrganization.Text = (_printPlavList.Item(0).CurrentVehicle.Item(0).LastRegIssuer)
        'Else
        '    lblToOrganization.Text = objRegistrationIssuerList.GetRegistrationIssuerInfo _
        '    (objCurentTehExamOrganization.IdDefaultRegistrationIssuer).IssuerName
        'End If
        Dim pom As CommunitiesInfo
        pom = objCommunityList.GetCommunitiesListById(_printPlavList.Item(0).NewOwner.Item(0).IdCommunityCode)

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
            lblNovaReg.Text = _printPlavList.Item(0).CurrentVehicle.Item(0).LastRegistration
            lblDatumReg.Text = _printPlavList.Item(0).CurrentVehicle.Item(0).DateOfLastRegistrationa
            lblDaumPrvaReg.Text = _printPlavList.Item(0).CurrentVehicle.Item(0).FirstRegistrationDate
            lblToOrganization.Text = objRegistrationIssuerList.GetRegistrationIssuerInfoByCommunity _
       (pom.Id).IssuerName
        End If
    ' SetupTehnicalExam()
    lblOrganization.Text = objCurentTehExamOrganization.OrganizationName
    lblStation.Text = objCurentTehExamOrganization.Station
        SetupRequestType()
        SetupVehicleCategory()
        SetupEnginePowerSource()
        SetupKuka()
        SetupCustomer()
        SetupFirstRegistration()
        ' Me.panelTehnicalExam.visible = _requestType.IsTehnicalExamRequired
    End Sub

#Region " Helpers "
  Private Sub SetupFirstRegistration()
    If _printPlavList(0).CurrentVehicle(0).FirstRegistration = "ÌÂÔÓÁÌ‡Ú‡" Then
      Me.XrLabel3.Visible = False
      Me.XrLabel4.Visible = False
      Me.XrLabel5.Visible = False
    End If
  End Sub
  'Private Sub SetupTehnicalExam()
  '  If _printPlavList(0).OrganizationName = String.Empty Then
  '    panelTehnicalExam.Visible = False
  '  Else
  '    panelTehnicalExam.Visible = True
  '  End If
  'End Sub
  Private Sub SetupCustomer()

        If _printPlavList(0).NewOwner(0).IsCompany Then
            Me.PanelCustomerPravno.Visible = True

            lblMBCompany.Visible = True
        Else
            Me.PanelCustomerFisicko.Visible = True

            lblMBCompany.Visible = False
        End If
  End Sub
  Private Sub SetupKuka()
    Dim da As Boolean = _printPlavList(0).CurrentVehicle(0).Hook Or _printPlavList(0).CurrentVehicle(0).Vitlo
        'If da Then
        '  CheckBoxHookYes.Visible = True
        'Else
        '  CheckBoxHookNo.Visible = True
        'End If
  End Sub
    Private Sub SetupEnginePowerSource()
        Try
            If _printPlavList(0).CurrentVehicle(0).IdEnginePowerSource > 0 Then
                FindControl("CheckBoxPowerSource" & _printPlavList(0).CurrentVehicle(0).IdEnginePowerSource, True).Visible = True
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub SetupVehicleCategory()
        Try
            Dim catInfo As VehicleCategoryForPayment = VehicleCategoryForPayment.GetVehicleCategoryForPayment(_printPlavList(0).CurrentVehicle(0).IdVehicleCategoryForPayments)
            FindControl("CheckBoxVehicleCategory" & catInfo.ZelenMap.ToString, True).Visible = True
        Catch ex As Exception

        End Try
    End Sub

  Private Sub SetupRequestType()
    If _requestType.TypeName.Contains("œŒ œ–¬ œ¿“") Then
      CheckBoxPrvPat.Visible = True
    End If
    If _requestType.TypeName.Contains("œŒ¬“Œ–ÕŒ") Then
      CheckBoxPovtorno.Visible = True
      panelPreviosRegistrationOwner.Visible = True
    End If
    If _requestType.TypeName.Contains("œ–»¬–≈Ã≈ÕŒ") Then
      CheckBoxPrivremeno.Visible = True
    End If
        If _requestType.TypeName.Contains("œ–ŒÃ≈Õ»") Then
            CheckBoxTehnickiPromeni.Visible = True
        End If

  End Sub
#End Region

End Class