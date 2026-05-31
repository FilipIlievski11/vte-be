Imports VTE.BaseParts
Imports System.Configuration
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.Xpo
Imports DevExpress.XtraEditors
Imports System.Threading

Public Class MainForm

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()


    DevExpress.UserSkins.BonusSkins.Register()
    DevExpress.UserSkins.OfficeSkins.Register()


    DevExpress.Skins.SkinManager.Default.RegisterAssembly(GetType(DevExpress.UserSkins.BonusSkins).Assembly)
    DevExpress.Skins.SkinManager.Default.RegisterAssembly(GetType(DevExpress.UserSkins.OfficeSkins).Assembly)

    Dim tmp As New LookAndFeelMenu.XtraBarsHelper(Me.BarManager1, DefaultLookAndFeel1)
    Me.DefaultLookAndFeel1.LookAndFeel.SetSkinStyle(uSettings.Skin)
    Me.NavBarMain.PaintStyleName = uSettings.Skin

    System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo(uSettings.Culture))


  End Sub

  Private Sub MainForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      DoLogin()
    Catch ex As Exception
      System.IO.File.WriteAllText("C:\Users\FilipIlievski\Desktop\oldapp-error.txt", "MainForm_Load: " & ex.ToString())
    End Try

    Try : ApplyAuthorizationRules() : Catch : End Try
  End Sub

  Private Function AUpdate() As Boolean
    Try


      Dim myAutoUpdate As AutoUpdate = New AutoUpdate
      If myAutoUpdate.AutoUpdate(Microsoft.VisualBasic.Command(), "ftp.bransys.com") Then
        Return True
      Else
        Return False
      End If
    Catch ex As Exception
      Return True
    End Try
    '  Return True
  End Function

#Region "ApplyAuthorizationRules"
  Private Sub ApplyAuthorizationRules()

    NavBarItemEditCities.Visible = Cities.CanAddObject
    NavBarItemCommunities.Visible = Communities.CanAddObject
    NavBarItemCountries.Visible = Countries.CanAddObject
    NavBarItemColors.Visible = Colors.CanAddObject
    NavBarItemStreets.Visible = Streets.CanAddObject
    NavBarGroup1.Visible = VehicleCategories.CanAddObject
    NavBarItemDriveingLicenceCategories.Visible = DriveingLicenceCtegories.CanAddObject
    NavBarGroupMainData.Visible = Customer.CanAddObject
    NavBarGroupVehicles.Visible = Vehicle.CanAddObject
    NavBarGroupRequests.Visible = Request.CanAddObject
    NavBarGroupTehnicalExamReports.Visible = DocumentsTehnicalExamsReport.CanAddObject
    NavBarGroupPayment.Visible = PaymentDocument.CanAddObject
    NavBarGroupDocumentPermisions.Visible = DocumentsPermision.CanAddObject
    NavBarGroupInternationalDriveingLicences.Visible = DocumentsInternationalDriveingLicence.CanAddObject
    NavBarGroup1.Visible = Vehicle.CanGetObject Or VehicleModels.CanGetObject
    NavBarGroupVehicles.Visible = Vehicle.CanGetObject
    NavBarItemBrakes.Visible = VehicleBrakes.CanGetObject
    NavBarItemVehicleMakers.Visible = VehicleMakers.CanGetObject
    NavBarItemVehicleModels.Visible = VehicleModels.CanGetObject
    NavBarItemVehicleEngineTypes.Visible = VehicleEngineTypes.CanGetObject
    NavBarItemVehicleCategories.Visible = VehicleCategories.CanGetObject
    NavBarItemVehicleCategoryPayments.Visible = VehicleCategoryForPayments.CanGetObject
    NavBarItemVehicleSupporting.Visible = VehicleSupportings.CanGetObject
    NavBarItemVehicleEngineEcoProgram.Visible = VehicleEngineEcoPrograms.CanGetObject
    NavBarItemVehicleTireTypes.Visible = VehicleTireTypes.CanGetObject
    NavBarItemVehicleUse.Visible = VehicleUses.CanGetObject
    NavBarItemColors.Visible = Colors.CanGetObject
    NavBarItemVehicleEnginePowerSourceTypes.Visible = VehicleEnginePowerSourceTypes.CanGetObject
    NavBarItemGearBoxes.Visible = VehicleGearBoxes.CanGetObject
    NavBarItemVehicleBodytype.Visible = VehicleBodytypes.CanGetObject
    NavBarGroupTrafficLicences.Visible = DocumentsTrafficLicence.CanAddObject
    NavBarItemOperators.Visible = Users.CanAddObject
  End Sub
#End Region

#Region " WinPart handling "

  Public Sub AddWinPart(ByVal part As uxWinPart)

    AddHandler part.CloseWinPart, AddressOf CloseWinPart
    Dim page As New DevExpress.XtraTab.XtraTabPage
    page.Text = part.ToString
    page.Name = part.ToString
    page.Controls.Add(part)
    tabMain.TabPages.Add(page)
    part.tPage = page

    ShowWinPart(part)

  End Sub



  Public Sub ShowWinPart(ByVal part As uxWinPart)
    part.Dock = DockStyle.Fill
    part.Visible = True
    part.BringToFront()
    tabMain.SelectedTabPage = part.tPage
    Me.Text = "VTE - " & part.ToString
  End Sub

  Private Sub tabControls_CloseButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabMain.CloseButtonClick
    CloseWinPart(tabMain.SelectedTabPage.Controls.Item(0), System.EventArgs.Empty)
  End Sub

  Public Sub CloseWinPart(ByVal sender As Object, ByVal e As EventArgs)

    Dim part As uxWinPart = CType(sender, uxWinPart)
    RemoveHandler part.CloseWinPart, AddressOf CloseWinPart
    part.Visible = False
    tabMain.TabPages.Remove(part.tPage)
    part.Dispose()

    For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
      For Each ctl As Control In page.Controls
        If TypeOf ctl Is uxWinPart Then
          Me.Text = "VTE - " + CType(ctl, uxWinPart).ToString
          Exit For
        End If
      Next
    Next
    If tabMain.TabPages.Count <> 0 Then
      tabMain.SelectedTabPage = tabMain.TabPages(tabMain.TabPages.Count - 1)
    End If
  End Sub

  Public Sub panelControls_Resize( _
    ByVal sender As Object, ByVal e As System.EventArgs)
    For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
      For Each ctl As Control In page.Controls
        If TypeOf ctl Is uxWinPart Then
          ctl.Size = tabMain.ClientSize
        End If
      Next
    Next
  End Sub

#End Region

#Region " NavBar "

  Private Sub NavBarControl1_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles NavBarMain.LinkClicked

    Select Case e.Link.ItemName
      Case NavBarItemRegistar.Name
        Dim dij As New dijTechExamReportFromTo
        If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
          Dim rpt As New rptTehnicalExamsRegistar(dij.DatuOd, dij.DatumDo)
          AddWinPart(New uxPrint(rpt))
        End If
      Case NavBarItemRequestTypes.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxRequestTypes Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxRequestTypes())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemCalculationItems.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxCalculationItems Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxCalculationItems())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemReportByPaymentCategory.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxReportByPaymentCategory Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxReportByPaymentCategory())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case NavBarItemPayDocList.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxFakturiList Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxFakturiList())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case NavBarItemReportByPaymentCategoryFromTo.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxReportByParametarsFromToForPayment Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxReportByParametarsFromToForPayment())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemUnpayedDeals.Name
        'For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
        '    For Each ctl As Control In page.Controls
        '        If TypeOf ctl Is uxUnpayedDealsList Then
        '            ShowWinPart(CType(ctl, uxWinPart))
        '            Exit Sub
        '        End If
        '    Next
        'Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxUnpayedDealsList(False))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemClosedDeals.Name
        'For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
        '    For Each ctl As Control In page.Controls
        '        If TypeOf ctl Is uxUnpayedDealsList Then
        '            ShowWinPart(CType(ctl, uxWinPart))
        '            Exit Sub
        '        End If
        '    Next
        'Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxUnpayedDealsList(True))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemRegistrationIssuers.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxRegistrationIssuers Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxRegistrationIssuers())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemVehicleTireTypes.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleTires Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleTires())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemAttachmentTypes.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxAttachmentTypes Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxAttachmentTypes())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemNewCustomer.Name
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxCustomers(Customer.NewCustomer))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case NavBarItemRequestList.Name
        'For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
        '    For Each ctl As Control In page.Controls
        '        If TypeOf ctl Is uxListOfNotEndedRequests Then
        '            ShowWinPart(CType(ctl, uxWinPart))
        '            Exit Sub
        '        End If
        '    Next
        'Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxListOfNotEndedRequests(True))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemClosedRequestsList.Name
        'For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
        '    For Each ctl As Control In page.Controls
        '        If TypeOf ctl Is uxListOfNotEndedRequests Then
        '            ShowWinPart(CType(ctl, uxWinPart))
        '            Exit Sub
        '        End If
        '    Next
        'Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxListOfNotEndedRequests(False))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemOwnershipProof.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxDocumentVehicleOwnershipProof Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxDocumentVehicleOwnershipProof())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case NavBarItemPaymentProof.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxDocumentPaymentProof Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxDocumentPaymentProof())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case NavBarItemNewVehicle.Name

        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicle(Vehicle.NewVehicle))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

        'najdi po
      Case "NavBarItemVehicleList"
        'Dim dij As New dijVehicleList
        'dij.ShowDialog(Me)
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleList Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleList())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

        'komitenti
      Case "NavBarItemCountries"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxCountries Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxCountries())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemEditCities"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxCities Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxCities())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemBusinessTypes"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxBusinessTypes Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxBusinessTypes())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemStreets"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxStreets Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxStreets())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemCustomer"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxCustomersList Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxCustomersList())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using



        'vozila

      Case "NavBarItemShowRelations"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxCustomerVehicleRelation Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxCustomerVehicleRelation())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemCommunities"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxCommunities Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxCommunities())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemVehicleEngineEcoProgram"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleEngineEcoProgram Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleEngineEcoProgram())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemVehicleSupporting"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleSupporting Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleSupporting())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemColors"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxColors Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxColors())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case NavBarItemCompanies.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxCompany Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxCompany())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case "NavBarItemVehicleEngineTypes"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleEngineTypes Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleEngineTypes())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemVehicleEnginePowerSourceTypes"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleEnginePowerSourceTypes Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleEnginePowerSourceTypes())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemVehicleCategories"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleCategories Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleCategories())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemVehicleCategoryPayments"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleCategoryForPayments Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleCategoryForPayments())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemVehicleMakers"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleMakers Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleMakers())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemVehicleModels"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleModels Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleModels())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using


      Case "NavBarItemBrakes"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxBrakes Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxBrakes())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemVehicleUse"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleUse Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleUse())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemGearBoxes"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleGearBoxes Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleGearBoxes())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemVehicleBodytype"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehicleBodytypes Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehicleBodytypes())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

        'samo za nas relacii

      Case "NavBarItemRelationsTypes"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxRelationsTypes Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxRelationsTypes())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using


      Case "NavBarItemDocTypePrint"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxDocumentTypePrint Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxDocumentTypePrint())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemDocumentTypes"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxDocumentTypes Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxDocumentTypes())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

        'relations dinamic

      Case "NavBarItemTehnicalExamOrganisations"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxTehnicalExamOrganizations Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxTehnicalExamOrganizations())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemTehnicalExamDetailsStatus"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxDocumentTehnicalExamVehiclePartsStatus Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxDocumentTehnicalExamVehiclePartsStatus())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemTehnicalExamVehicleParts"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxTehnicalExamVehicleParts Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxTehnicalExamVehicleParts())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemTehnicalExamTypes"
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxTehnicalExamTypes Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxTehnicalExamTypes())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using


        'administrator

      Case "NavBarItemOperators"
        dijOperators.ShowDialog()
      Case "NavBarItemRools"
        dijRools.ShowDialog()
        'TehExamReportByRequest

      Case "NavBarItemTehExamReportByRequest"

        Dim dij As New dijDocumentRequestsWithoutTehnicalExam
        If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
          Using busy As New Splash(My.Resources.txtLoading)
            Try
              'AddWinPart(New uxDocumentTehnicalExamReports(DocumentsTehnicalExamsReport.NewDocumentsTehnicalExamsReport, _
              '                                             dij.CustomerVehicleRelationId, dij.DocumentRequestId))
            Catch ex As Csla.DataPortalException
              MessageBox.Show(ex.BusinessException.ToString, _
                "Error loading", MessageBoxButtons.OK, _
                MessageBoxIcon.Exclamation)
            Catch ex As Exception
              MessageBox.Show(ex.ToString, _
                "Error loading", MessageBoxButtons.OK, _
                MessageBoxIcon.Exclamation)
            End Try
          End Using
        End If

      Case "NavBarItemTehExamReportIndependent"
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxDocumentTehnicalExamReports( _
         DocumentsTehnicalExamsReport.NewDocumentsTehnicalExamsReport))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemTehExamReportIncorrect"
        'Dim dij As New dijListOfTehnicalExamsForIncorrectVehicles(False)
        'If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            ' Dim _documentTehExamReport As DocumentsTehnicalExamsReport = dij.DokTehExamReport
            'AddWinPart(New uxDocumentTehnicalExamReports(_documentTehExamReport))
            AddWinPart(New uxTechExamReportList(False))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
        'End If

      Case NavBarTehExamReportTouch.Name
        Using busy As New Splash(My.Resources.txtLoading)
          Try

            AddWinPart(New uxTouchTehnicalCheckReport())

          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemListOfTehnicalExams"
        'Dim dij As New dijListOfTehnicalExamsForIncorrectVehicles() '(String.Format("[ValidTillDate]>='{0}'", Now.Date))
        'If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxTechExamReportList(True))
            'AddWinPart(New uxDocumentTehnicalExamReports(dij.DokTehExamReport))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
        ' End If

        'Traffic licences
      Case "NavBarItemTrafficLicenceNew"
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxDocumentTrafficLicences())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemTrafficLicencesList"

        Dim dij As New dijValidTrafficLicences()
        dij.ShowDialog(Me)
        If dij.DialogResult = Windows.Forms.DialogResult.OK Then

          Using busy As New Splash(My.Resources.txtLoading)
            Try
              AddWinPart(New uxDocumentTrafficLicences(dij.TrafficLicence))
            Catch ex As Csla.DataPortalException
              MessageBox.Show(ex.BusinessException.ToString, _
                "Error loading", MessageBoxButtons.OK, _
                MessageBoxIcon.Exclamation)
            Catch ex As Exception
              MessageBox.Show(ex.ToString, _
                "Error loading", MessageBoxButtons.OK, _
                MessageBoxIcon.Exclamation)
            End Try
          End Using
        End If

        'payment
      Case PaymentReportNavBarItem.Name
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxPaymentPivotReport)
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemNewPayment.Name
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxPaymentDocument(PaymentDocument.NewPaymentDocument))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemPaymentTypes"
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxPaymentTypes())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemPriceCatalog"
        Using busy As New Splash(My.Resources.txtLoading)
          For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
            For Each ctl As Control In page.Controls
              If TypeOf ctl Is uxPaymentCategories Then
                ShowWinPart(CType(ctl, uxWinPart))
                Exit Sub
              End If
            Next
          Next
          Try
            AddWinPart(New uxPaymentCategories())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemListOfPaymentDocuments"
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxFinancialStateCreatePaymentDocuments())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

        'documentPermision

      Case "NavBarItemNewDocumentPermision"
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxDocumentPermisionRequest(DocumentsPermision.NewDocumentsPermision))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case NavBarItemPermissionForV.Name
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxDocumentPermisionRequest(DocumentsPermision.NewDocumentsPermision))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case "NavBarItemDocumentPermisionList"
        Dim dij As New dijDocPermissionsList()
        dij.ShowDialog(Me)
        If dij.DialogResult = Windows.Forms.DialogResult.OK Then
          Using busy As New Splash(My.Resources.txtLoading)
            Try
              AddWinPart(New uxDocumentPermisionRequest(dij.DocumenPermision))
            Catch ex As Csla.DataPortalException
              MessageBox.Show(ex.BusinessException.ToString, _
                "Error loading", MessageBoxButtons.OK, _
                MessageBoxIcon.Exclamation)
            Catch ex As Exception
              MessageBox.Show(ex.ToString, _
                "Error loading", MessageBoxButtons.OK, _
                MessageBoxIcon.Exclamation)
            End Try
          End Using
        Else
          If dij.DialogResult = Windows.Forms.DialogResult.Retry Then
            Using busy As New Splash(My.Resources.txtLoading)
              Try
                AddWinPart(New uxDocumentPermisionRequest(DocumentsPermision.NewDocumentsPermision))
              Catch ex As Csla.DataPortalException
                MessageBox.Show(ex.BusinessException.ToString, _
                  "Error loading", MessageBoxButtons.OK, _
                  MessageBoxIcon.Exclamation)
              Catch ex As Exception
                MessageBox.Show(ex.ToString, _
                  "Error loading", MessageBoxButtons.OK, _
                  MessageBoxIcon.Exclamation)
              End Try
            End Using
          Else
            If dij.DialogResult = Windows.Forms.DialogResult.Yes Then
              Dim rpt As RegistarOdobrenija = New RegistarOdobrenija(dij.DocumenPermisionList, dij.Od, dij.DoDat)
              Dim ux1 As New uxPrint(rpt)
              Me.AddWinPart(ux1)
            End If
          End If
        End If

        'international driving licences
      Case "NavBarItemDriveingLicenceCategories"
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxDriveingLicenceCtegories())
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemInternationalDriveingLicence"
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxInternationalDriveingLicences(DocumentsInternationalDriveingLicence.NewDocumentsInternationalDriveingLicence))
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case "NavBarItemInternationalDriveingLicenceList"
        Dim dij As New dijInternationalDriveingLicencesList()
        Dim docOdgovor As DocumentsInternationalDriveingLicence
        dij.ShowDialog(Me)
        If dij.DialogResult = Windows.Forms.DialogResult.OK Then
          docOdgovor = (dij.DocumenInternationalLicence)
        Else
          If dij.DialogResult = Windows.Forms.DialogResult.Retry Then
            docOdgovor = (DocumentsInternationalDriveingLicence.NewDocumentsInternationalDriveingLicence)
          Else
            If dij.DialogResult = Windows.Forms.DialogResult.Yes Then
              Dim rpt As RegistarMegunarodni = New RegistarMegunarodni(dij.DocInterLicenceList, dij.Od, dij.DoDat)
              Dim ux1 As New uxPrint(rpt)
              Me.AddWinPart(ux1)
              docOdgovor = Nothing
            Else
              docOdgovor = Nothing
            End If
          End If
        End If
        If docOdgovor IsNot Nothing Then
          Using busy As New Splash(My.Resources.txtLoading)
            Try
              AddWinPart(New uxInternationalDriveingLicences(docOdgovor))
            Catch ex As Csla.DataPortalException
              MessageBox.Show(ex.BusinessException.ToString, _
                "Error loading", MessageBoxButtons.OK, _
                MessageBoxIcon.Exclamation)
            Catch ex As Exception
              MessageBox.Show(ex.ToString, _
                "Error loading", MessageBoxButtons.OK, _
                MessageBoxIcon.Exclamation)
            End Try
          End Using
        End If
        'pivotReports
      Case NavBarItemCustomerPivotReport.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxCustomerPivotReport Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxCustomerPivotReport)
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemVehiclePivotReport.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxVehiclesPivotReport Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxVehiclesPivotReport)
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemTehExamPivotReport.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxTechnicalExamPivotReport Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxTechnicalExamPivotReport)
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case NavBarItemTehExamPivotBr.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxTehnicalWxamPivotReportBR Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxTehnicalWxamPivotReportBR)
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using

      Case NavBarItemCustomerVehiclePivotReport.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxPivotReportCustomerVehicle Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxPivotReportCustomerVehicle)
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
      Case NavBarItemRequestPivotReport.Name
        For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
          For Each ctl As Control In page.Controls
            If TypeOf ctl Is uxRequestPivotReport Then
              ShowWinPart(CType(ctl, uxWinPart))
              Exit Sub
            End If
          Next
        Next
        Using busy As New Splash(My.Resources.txtLoading)
          Try
            AddWinPart(New uxRequestPivotReport)
          Catch ex As Csla.DataPortalException
            MessageBox.Show(ex.BusinessException.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          Catch ex As Exception
            MessageBox.Show(ex.ToString, _
              "Error loading", MessageBoxButtons.OK, _
              MessageBoxIcon.Exclamation)
          End Try
        End Using
    End Select
  End Sub
#End Region

#Region " KeyPress "
  Private Sub MainForm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
    If (tabMain.TabPages.Count <> 0) AndAlso TypeOf (DirectCast(sender, MainForm).ActiveControl) Is uxWinPart Then
      CType(DirectCast(sender, MainForm).ActiveControl, uxWinPart).OnPritisnatoKopce(sender, e)
    End If

  End Sub
#End Region

#Region " Login "

  Private Sub BarButtonLogin_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonLogin.ItemClick
    DoLogin()
  End Sub


  Private Sub DoLogin()
    VTE.Library.Security.VTEPrincipal.Logout()

    If Me.BarButtonLogin.Caption = My.Resources.loginStatusLogin Then
      Login.ShowDialog(Me)
      '  Me.BarButtonLogin.Caption = My.Resources.loginStatusLogout
      'Else
      '  VTE.Library.Security.VTEPrincipal.Logout()
      '  Me.BarButtonLogin.Caption = My.Resources.loginStatusLogin
    End If

    Dim user As System.Security.Principal.IPrincipal = _
     Csla.ApplicationContext.User

    If user.Identity.IsAuthenticated Then
      Me.BarButtonLogin.Caption = My.Resources.loginStatusLogout
      Me.Text = "������ ���� " & Csla.ApplicationContext.LocalContext("EmployeeFullName") 'user.Identity.Name
      Me.OperatorBarButtonItem.Caption = Csla.ApplicationContext.LocalContext("EmployeeFullName") 'user.Identity.Name
      Using sp As New Splash(My.Resources.LoadingData)
        LoadPublicLists()
        AddDashBoard()

      End Using
      If user.IsInRole("Administrator") Then
        NavBarGroupAdministartor.Visible = True
        NavBarGroup1.Visible = True
        'BarSubItemPrivileges.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        NavBarReports.Visible = True
        BarSubItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        BarSubItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        NavBarItemVehicleList.Visible = True
        NavBarItemCustomer.Visible = True
        BarSubItemPrivileges.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

      Else
        NavBarGroupAdministartor.Visible = False
        NavBarGroup1.Visible = False
        'BarSubItemPrivileges.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        NavBarReports.Visible = False
        'BarSubItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BarSubItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        NavBarItemVehicleList.Visible = False
        NavBarItemCustomer.Visible = False
        BarSubItemPrivileges.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
      End If
      BarButtonOptions.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
      ActivateDashboardBarButtonItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
      BarSubItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
      BarButtonChangePass.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
      objPaymentCatalogList = PaymentCataologList.GetPaymentCataologList
      If Csla.ApplicationContext.LocalContext.Contains("objPaymentCatalogList") Then
        Csla.ApplicationContext.LocalContext.Remove("objPaymentCatalogList")
      End If
      Csla.ApplicationContext.LocalContext.Add("objPaymentCatalogList", objPaymentCatalogList)

    Else
      'zatvori ja main
      RemoveDashboard()
      Me.Text = "�� �� ������"
      Me.OperatorBarButtonItem.Caption = "�������� ��������"
    End If


    ApplyAuthorizationRules()
    'i vo site dokumenti
    On Error Resume Next
    For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
      For Each ctl As Control In page.Controls
        If TypeOf (ctl) Is uxWinPart Then
          'tabControls.TabPages.Remove(CType(ctl, uxWinPart).tPage)
          'CloseWinPart(CType(ctl, uxWinPart), System.EventArgs.Empty)
          CType(ctl, uxWinPart).OnCurrentPrincipalChanged(Me, EventArgs.Empty)

        End If
      Next
      'If tabControls.TabPages.Count = 0 Then Exit For
    Next

  End Sub

#End Region

#Region "BarButtons"

  Private Sub BarButtonOptions_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonOptions.ItemClick
    dijOptions.ShowDialog()
  End Sub

  Private Sub BarButtonCulture_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
    dijCulture.ShowDialog()
  End Sub


  Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
    Dim dij As New dijTest
    dij.Show(Me)
  End Sub

  Private Sub BarButtonItemMK1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItemMK1.ItemClick
    uSettings.Culture = "mk-MK"
    uSettings.Save()
    ChangeCulture(uSettings.Culture)
  End Sub

  Private Sub BarButtonItemEn1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItemEn1.ItemClick
    uSettings.Culture = "en-US"
    uSettings.Save()
    ChangeCulture(uSettings.Culture)
  End Sub


  Private Sub BarButtonChangePass_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonChangePass.ItemClick
    dijChangeUserNameAndPass.ShowDialog()
  End Sub
#End Region

#Region "BarSubItem"

  Private Sub BarSubItemCSLAOjects_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarSubItemCSLAOjects.ItemClick
    'DevExpress.XtraBars.BarSubItem.Focused()
    uxCSLAObjects.ShowDialog(Me)
  End Sub

  Private Sub BarSubItemObjectPrivileges_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarSubItemObjectPrivileges.ItemClick
    'DevExpress.XtraBars.BarSubItem.Focused()
    uxPrivileges.ShowDialog(Me)
  End Sub

  Private Sub BarSubItemFieldsPrivileges_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarSubItemFieldsPrivileges.ItemClick
    dijFildsPrivilege.ShowDialog(Me)
  End Sub


#End Region

  Private Sub LoadPublicLists()
    uSettings = UserSettings.GetUserSettings
    If Csla.ApplicationContext.LocalContext.Contains("uSettings") Then
      Csla.ApplicationContext.LocalContext.Remove("uSettings")
    End If
    Csla.ApplicationContext.LocalContext.Add("uSettings", uSettings)


    'objVehicleListShort = VehiclesListShortListAll.GetVehiclesListShortListAll
    'If Csla.ApplicationContext.LocalContext.Contains("objVehicleListShort") Then
    '    Csla.ApplicationContext.LocalContext.Remove("objVehicleListShort")
    'End If
    'Csla.ApplicationContext.LocalContext.Add("objVehicleListShort", objVehicleListShort)

    objUsersList = UsersList.GetUsersList
    If Csla.ApplicationContext.LocalContext.Contains("objUsersList") Then
      Csla.ApplicationContext.LocalContext.Remove("objUsersList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objUsersList", objUsersList)

    objCurentUser = objUsersList.getInfoById(Csla.ApplicationContext.LocalContext("EmployeeID"))
    If Csla.ApplicationContext.LocalContext.Contains("objCurentUser") Then
      Csla.ApplicationContext.LocalContext.Remove("objCurentUser")
    End If
    Csla.ApplicationContext.LocalContext.Add("objCurentUser", objCurentUser)

    objCompanyList = CompanyList.GetCompanyList
    If Csla.ApplicationContext.LocalContext.Contains("objCompanyList") Then
      Csla.ApplicationContext.LocalContext.Remove("objCompanyList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objCompanyList", objCompanyList)

    objOpcii = Options.GetOptions
    If Csla.ApplicationContext.LocalContext.Contains("objOpcii") Then
      Csla.ApplicationContext.LocalContext.Remove("objOpcii")
    End If
    Csla.ApplicationContext.LocalContext.Add("objOpcii", objOpcii)


    'objCustomersListShort = CustomersListShort.GetCustomersList
    'If Csla.ApplicationContext.LocalContext.Contains("objCustomersListShort") Then
    '    Csla.ApplicationContext.LocalContext.Remove("objCustomersListShort")
    'End If
    'Csla.ApplicationContext.LocalContext.Add("objCustomersListShort", objCustomersListShort)


    objDDVList = DDVList.GetDDVList
    If Csla.ApplicationContext.LocalContext.Contains("objDDVList") Then
      Csla.ApplicationContext.LocalContext.Remove("objDDVList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objDDVList", objDDVList)

    'objRlationList = CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsListByTypeOfRelation(1)
    'If Csla.ApplicationContext.LocalContext.Contains("objRlationList") Then
    '    Csla.ApplicationContext.LocalContext.Remove("objRlationList")
    'End If
    'Csla.ApplicationContext.LocalContext.Add("objRlationList", objRlationList)

    objTehExamOrganizations = TehnicalExamOrganizationsList.GetTehnicalExamOrganizationsList()
    If Csla.ApplicationContext.LocalContext.Contains("objTehExamOrganizations") Then
      Csla.ApplicationContext.LocalContext.Remove("objTehExamOrganizations")
    End If
    Csla.ApplicationContext.LocalContext.Add("objTehExamOrganizations", objTehExamOrganizations)

    objCurentTehExamOrganization = objTehExamOrganizations.GetTehnicalExamOrganizationsInfoById(objCurentUser.IdStation)
    If Csla.ApplicationContext.LocalContext.Contains("objCurentTehExamOrganization") Then
      Csla.ApplicationContext.LocalContext.Remove("objCurentTehExamOrganization")
    End If
    Csla.ApplicationContext.LocalContext.Add("objCurentTehExamOrganization", objCurentTehExamOrganization)

    'objUsersList = UsersList.GetUsersListByStation(objCurentUser.IdStation, objCurentUser.IdDataBase)
    'If Csla.ApplicationContext.LocalContext.Contains("objUsersList") Then
    ' Csla.ApplicationContext.LocalContext.Remove("objUsersList")
    'End If
    'Csla.ApplicationContext.LocalContext.Add("objUsersList", objUsersList)


    objTehnicalExamsTypesList = TehnicalExamsTypesList.GetTehnicalExamsTypesList()
    If Csla.ApplicationContext.LocalContext.Contains("objTehnicalExamsTypesList") Then
      Csla.ApplicationContext.LocalContext.Remove("objTehnicalExamsTypesList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objTehnicalExamsTypesList", objTehnicalExamsTypesList)

    objRequestTypeList = RequestTypeList.GetRequestTypeList
    If Csla.ApplicationContext.LocalContext.Contains("objRequestTypeList") Then
      Csla.ApplicationContext.LocalContext.Remove("objRequestTypeList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objRequestTypeList", objRequestTypeList)

    objVehicleEngineEcoProgramList = VehicleEngineEcoProgramList.GetVehicleEngineEcoProgramList
    If Csla.ApplicationContext.LocalContext.Contains("objVehicleEngineEcoProgramList") Then
      Csla.ApplicationContext.LocalContext.Remove("objVehicleEngineEcoProgramList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objVehicleEngineEcoProgramList", objVehicleEngineEcoProgramList)

    objPaymentTypeList = PaymentTypeList.GetPaymentTypeList
    If Csla.ApplicationContext.LocalContext.Contains("objPaymentTypeList") Then
      Csla.ApplicationContext.LocalContext.Remove("objPaymentTypeList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objPaymentTypeList", objPaymentTypeList)

    objCommunityList = CommunitiesList.GetCommunitiesList
    If Csla.ApplicationContext.LocalContext.Contains("objCommunityList") Then
      Csla.ApplicationContext.LocalContext.Remove("objCommunityList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objCommunityList", objCommunityList)

    'objPaymentTypeList = PaymentTypeList.GetPaymentTypeList
    'If Csla.ApplicationContext.LocalContext.Contains("objPaymentTypeList") Then
    '    Csla.ApplicationContext.LocalContext.Remove("objPaymentTypeList")
    'End If
    'Csla.ApplicationContext.LocalContext.Add("objPaymentTypeList", objPaymentTypeList)

    objVehicleCategoryForPaymentsList = VehicleCategoryForPaymentsList.GetVehicleCategoryForPaymentsList
    If Csla.ApplicationContext.LocalContext.Contains("objVehicleCategoryForPaymentsList") Then
      Csla.ApplicationContext.LocalContext.Remove("objVehicleCategoryForPaymentsList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objVehicleCategoryForPaymentsList", objVehicleCategoryForPaymentsList)

    objVehicleFieldList = VehicleFieldList.GetList
    If Csla.ApplicationContext.LocalContext.Contains("objVehicleFieldList") Then
      Csla.ApplicationContext.LocalContext.Remove("objVehicleFieldList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objVehicleFieldList", objVehicleFieldList)

    objVehicleUseList = VehicleUseList.GetVehicleUseList
    If Csla.ApplicationContext.LocalContext.Contains("objVehicleUseList") Then
      Csla.ApplicationContext.LocalContext.Remove("objVehicleUseList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objVehicleUseList", objVehicleUseList)

    objRegistrationIssuerList = RegistrationIssuerList.GetRegistrationIssuerList
    If Csla.ApplicationContext.LocalContext.Contains("objRegistrationIssuerList") Then
      Csla.ApplicationContext.LocalContext.Remove("objRegistrationIssuerList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objRegistrationIssuerList", objRegistrationIssuerList)

    objVehicleSupportingList = VehicleSupportingList.GetVehicleSupportingList
    If Csla.ApplicationContext.LocalContext.Contains("objVehicleSupportingList") Then
      Csla.ApplicationContext.LocalContext.Remove("objVehicleSupportingList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objVehicleSupportingList", objVehicleSupportingList)

    objVehicleGearBoxList = VehicleGearBoxList.GetVehicleGearBoxList
    If Csla.ApplicationContext.LocalContext.Contains("objVehicleGearBoxList") Then
      Csla.ApplicationContext.LocalContext.Remove("objVehicleGearBoxList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objVehicleGearBoxList", objVehicleGearBoxList)

    objVehicleBrakesList = VehicleBrakesList.GetVehicleBrakesList
    If Csla.ApplicationContext.LocalContext.Contains("objVehicleBrakesList") Then
      Csla.ApplicationContext.LocalContext.Remove("objVehicleBrakesList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objVehicleBrakesList", objVehicleBrakesList)

    objVehicleEnginePowerSourceTypeList = VehicleEnginePowerSourceTypeList.GetVehicleEnginePowerSourceTypeList
    If Csla.ApplicationContext.LocalContext.Contains("objVehicleEnginePowerSourceTypeList") Then
      Csla.ApplicationContext.LocalContext.Remove("objVehicleEnginePowerSourceTypeList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objVehicleEnginePowerSourceTypeList", objVehicleEnginePowerSourceTypeList)

    objCountriesList = CountriesList.GetCountriesList
    If Csla.ApplicationContext.LocalContext.Contains("objCountriesList") Then
      Csla.ApplicationContext.LocalContext.Remove("objCountriesList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objCountriesList", objCountriesList)

    objCityList = CityList.GetCityList
    If Csla.ApplicationContext.LocalContext.Contains("objCityList") Then
      Csla.ApplicationContext.LocalContext.Remove("objCityList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objCityList", objCityList)

    objColorsList = ColorsList.GetColorsList
    If Csla.ApplicationContext.LocalContext.Contains("objColorsList") Then
      Csla.ApplicationContext.LocalContext.Remove("objColorsList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objColorsList", objColorsList)

    objVehicleCategoryList = VehicleCategoryList.GetVehicleCategoryList
    If Csla.ApplicationContext.LocalContext.Contains("objVehicleCategoryList") Then
      Csla.ApplicationContext.LocalContext.Remove("objVehicleCategoryList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objVehicleCategoryList", objVehicleCategoryList)

    objVehicleBodytypeList = VehicleBodytypeList.GetVehicleBodytypeList
    If Csla.ApplicationContext.LocalContext.Contains("objVehicleBodytypeList") Then
      Csla.ApplicationContext.LocalContext.Remove("objVehicleBodytypeList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objVehicleBodytypeList", objVehicleBodytypeList)

    objOwnershipProofList = DocumentVehicleOwnershipProofList.GetDocumentVehicleOwnershipProofList
    If Csla.ApplicationContext.LocalContext.Contains("objOwnershipProofList") Then
      Csla.ApplicationContext.LocalContext.Remove("objOwnershipProofList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objOwnershipProofList", objOwnershipProofList)

    objPaymentProofList = DocumentPaymentProofList.GetDocumentPaymentProofList
    If Csla.ApplicationContext.LocalContext.Contains("objPaymentProofList") Then
      Csla.ApplicationContext.LocalContext.Remove("objPaymentProofList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objPaymentProofList", objPaymentProofList)

    objDriveingLicenceCtegoryList = DriveingLicenceCtegoryList.GetDriveingLicenceCtegoryList
    If Csla.ApplicationContext.LocalContext.Contains("objDriveingLicenceCtegoryList") Then
      Csla.ApplicationContext.LocalContext.Remove("objDriveingLicenceCtegoryList")
    End If
    Csla.ApplicationContext.LocalContext.Add("objDriveingLicenceCtegoryList", objDriveingLicenceCtegoryList)

    objVehicleParts = TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsList()
    If Csla.ApplicationContext.LocalContext.Contains("objVehicleParts") Then
      Csla.ApplicationContext.LocalContext.Remove("objVehicleParts")
    End If
    Csla.ApplicationContext.LocalContext.Add("objVehicleParts", objVehicleParts)


  End Sub

  Private Sub AddDashBoard()
    Me.AddWinPart(New uxDashboard)
  End Sub

  Private Sub RemoveDashboard()
    NavBarGroupAdministartor.Visible = False
    NavBarReports.Visible = False
    BarSubItemObjectPrivileges.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    BarButtonOptions.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    ActivateDashboardBarButtonItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    BarSubItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    ' BarSubItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    NavBarGroupTrafficLicences.Visible = False
    BarSubItemPrivileges.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    BarButtonChangePass.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    For Each page As DevExpress.XtraTab.XtraTabPage In tabMain.TabPages
      For Each ctl As Control In page.Controls
        If (TypeOf ctl Is uxDashboard) Then
          CloseWinPart(CType(ctl, uxDashboard), System.EventArgs.Empty)
          GoTo ExitFor
        End If
      Next
    Next
ExitFor:
    Me.Text = "�� �� ������"
    Me.BarButtonLogin.Caption = My.Resources.loginStatusLogin
  End Sub

  'Private Sub InitMenus()
  '  For Each item As DocumentType In DocumentTypes.GetDocumentTypes
  '    Dim nbItem As DevExpress.XtraNavBar.NavBarItemLink = Me.NavBarMain.Groups.Item("NavBarGroupRequests").AddItem
  '    nbItem.ItemName = "nbItemBaranje" & item.Id
  '    nbItem.Item.Caption = item.DocumentTypeName.ToString
  '  Next
  'End Sub

  Private Sub ActivateDashboardBarButtonItem_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles ActivateDashboardBarButtonItem.ItemClick
    ' RemoveDashboard()
    AddDashBoard()
  End Sub



#Region " Fiskalno rabotenje "

  Private Sub frZatvori_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles frZatvori.ItemClick
    Try
      PecatiDnevnoFiskalnoZatvaranjePF500()

    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

  Private Sub frKontrolen_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles frKontrolen.ItemClick
    Try
      PecatiDnevenKontrolenIzvestajPF500()
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

  End Sub

  Private Sub frPodesiDatumCas_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles frPodesiDatumCas.ItemClick
    Try
      Dim dij As New dijPromenNaDatumNaFiskalna
      dij.ShowDialog(Me)
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

  End Sub

  Private Sub frVremenskiIzvestai_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles frVremenskiIzvestai.ItemClick
    Try
      Dim dij As New dijVremenskiFiskalniIzvestaii
      dij.ShowDialog(Me)
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

  End Sub

  Private Sub fxSluzbenoVnesuvanjePari_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles fxSluzbenoVnesuvanjePari.ItemClick
    SluzbenoVnesuvanjePariPF500(InputBox("������� �� ��������� �� ���� ��� ������ �� �� ������� �� ����", _
                                         "�������� ��������� �� ����", 0))

  End Sub

  Private Sub fxSluzbenoVadenjePari_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles fxSluzbenoVadenjePari.ItemClick
    SluzbenoVadenjePariPF500(InputBox("������� �� ��������� �� ���� ��� ������ �� �� �������� �� ����", _
                                     "�������� ����� �� ����", 0))
  End Sub

#End Region



End Class