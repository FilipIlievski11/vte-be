Public Class printZelen
  Private WithEvents _printZelen As PrintZelenList
  Public ReadOnly Property PrintZelen() As PrintZelenList
    Get
      Return _printZelen
    End Get
  End Property
  Private WithEvents _requestTypeList As RequestTypeList
  Private WithEvents _requestType As RequestTypeInfo

  Public Sub New(ByVal printZelen As PrintZelenList)

    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    Me.Margins.Top = objOpcii.ZelenTopMargin
    Me.Margins.Left = objOpcii.ZelenLeftMargin
    Me.Margins.Right = objOpcii.ZelenRightMargin
    Me.Margins.Bottom = objOpcii.ZelenButtonMargin
    'load list
    _requestTypeList = RequestTypeList.GetRequestTypeList
    _printZelen = printZelen
    _requestType = _requestTypeList.getInfoById(_printZelen.Item(0).IdRequestType)
    Me.PrintZelenListBindingSource.DataSource = _printZelen
    'If _printZelen.Item(0).CurrentVehicle.Item(0).LastRegIdCommunity > 0 Then

    '    lblToOrganization.Text = (_printZelen.Item(0).CurrentVehicle.Item(0).LastRegIssuer)
    'Else
    '    lblToOrganization.Text = objRegistrationIssuerList.GetRegistrationIssuerInfo _
    '    (objCurentTehExamOrganization.IdDefaultRegistrationIssuer).IssuerName
    'End If

    ' '' lblToOrganization.Text = objRegistrationIssuerList.GetRegistrationIssuerInfo(objCurentTehExamOrganization.IdDefaultRegistrationIssuer).IssuerName
    'Dim pom As String = ""
    'If _printZelen.Item(0).CurrentVehicle.Item(0).LastRegIdCommunity > 0 Then
    '    pom = objCommunityList.GetCommunitiesListById(_printZelen.Item(0).CurrentVehicle.Item(0).LastRegIdCommunity).RegistrationCode
    'Else
    '    pom = objCommunityList.GetCommunitiesListById(objCurentTehExamOrganization.IdCommunity).RegistrationCode
    'End If
    Dim pom As CommunitiesInfo
    pom = objCommunityList.GetCommunitiesListById(_printZelen.Item(0).NewOwner.Item(0).IdCommunityCode)
    If _requestType.IsNewRegistration Then
      'Dim pom As String = objCommunityList.GetCommunitiesListById(objCurentTehExamOrganization.IdCommunity).RegistrationCode
      Try
        lblToOrganization.Text = objRegistrationIssuerList.GetRegistrationIssuerInfoByCommunity _
          (pom.Id).IssuerName
      Catch ex As Exception
        lblToOrganization.Text = ""
      End Try

      lblNovaReg.Text = pom.RegistrationCode & "-"
    Else
      lblNovaReg.Text = _printZelen.Item(0).CurrentVehicle.Item(0).LastRegistration 'RegistrationNumberPrevios
      lblToOrganization.Text = (_printZelen.Item(0).CurrentVehicle.Item(0).LastRegIssuer)
    End If
    'inicijalizacija
    SetupRequestType1()
    SetupVehicleCategory()
    SetupVehicleIsRight()
    SetupRequestTypeA3()
    SetupVehicleIsCompany()
    SetupRequestTypeV()
    ProcessNulls()
    'pokazi/skri paneli
    Me.panelVozilo.Visible = _printZelen(0).IsVehicleChanged
    Me.PanelCustomer.Visible = _printZelen(0).IsCustomerChanged
    lblLastRegistration.Visible = _requestType.IsNewRegistration


  End Sub


#Region " Helpers "
  Private Sub ProcessNulls()
    For Each ctr As DevExpress.XtraReports.UI.XRLabel In Me.panelVozilo.Controls

      If IsNumeric(ctr.Text) AndAlso (ctr.Text = 0) Then
        ctr.Visible = False
      End If
    Next
  End Sub

  Private Sub SetupRequestTypeV()
    Dim tmp As String = _requestTypeList.getDisplayText(_requestType.Id)
        For I As Integer = 1 To 9
            If tmp.Contains("Ã" & I & " -") Then
                Me.FindControl("CheckBoxV" & I, True).Visible = True
            End If
        Next

  End Sub
  Private Sub SetupVehicleIsCompany()
    If _printZelen(0).NewOwner(0).IsCompany Then
      CheckBoxIsCompany.Visible = True
            'lblNewOccupation.Text = _printZelen(0).NewOwner(0).BusinessTypeDescription
    Else
      CheckBoxIsNotCompany.Visible = True
            'lblNewOccupation.Text = _printZelen(0).NewOwner(0).Occupation
    End If
  End Sub

  Private Sub SetupRequestTypeA3()
    Dim tmp As String = _requestTypeList.getDisplayText(_requestType.Id)
        If tmp.Contains("Á21 -") Then
            Me.CheckBoxA31.Visible = True
        End If
        If tmp.Contains("Á22 -") Then
            Me.CheckBoxA32.Visible = True
        End If
        If tmp.Contains("Á23 -") Then
            Me.CheckBoxA33.Visible = True
        End If
        If tmp.Contains("Á24 -") Then
            Me.CheckBoxA34.Visible = True
        End If
  End Sub

  Private Sub SetupVehicleIsRight()
    Me.lblIsReady.Visible = True
    'If _printZelen(0).VehicleIsRight Then
    '  Me.lblIsReady.Visible = True
    'Else
    '  Me.lblIsNotReady.Visible = True
    'End If
  End Sub

  Private Sub SetupVehicleCategory()
    Try

      Dim catInfo As VehicleCategoryForPayment = VehicleCategoryForPayment.GetVehicleCategoryForPayment(_printZelen(0).CurrentVehicle(0).IdVehicleCategoryForPayments)
      FindControl("VehicelCategory" & catInfo.ZelenMap.ToString, True).Visible = True

    Catch ex As Exception

    End Try
  End Sub

  Private Sub SetupRequestType1()
    Dim tmp As String = _requestTypeList.getDisplayText(_requestType.Id)
    If tmp.Contains("À -") Then
      Me.CheckBoxA.Visible = True
    End If
    If tmp.Contains("Á -") Then
            Me.CheckBoxB.Visible = True
    End If
    If tmp.Contains("Â -") Then
            Me.CheckBoxV.Visible = True
        End If
        If tmp.Contains("Ã -") Then
            Me.CheckBoxG.Visible = True
        End If
  End Sub

#End Region

End Class