Public Class rptTehnickiPregledZapisnik
  Private WithEvents _printTehExamReportList As PrintDocumentsTehnicalExamsReportsList
  Private _isRight As Boolean = False
  Public Sub New(ByVal idTexReport As Long)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    Try
      _printTehExamReportList = PrintDocumentsTehnicalExamsReportsList.GetPrintDocumentsTehnicalExamsReportsList(idTexReport)
      Me.BindingSourcePrintTehExamReport.DataSource = _printTehExamReportList
      lblCountryMade.Text = CountriesList.GetCountriesList.GetCountriesListById(_printTehExamReportList.Item(0).IdCountryOfProduction).CountryName

      If _printTehExamReportList.Item(0).IsSocialNotPrivate Then
        CheckBoxSocial.Visible = True
      Else
        CheckBoxPrivate.Visible = True
            End If
            Dim pom As CommunitiesInfo
            pom = objCommunityList.GetCommunitiesListById(_printTehExamReportList.Item(0).communityId)

            If _printTehExamReportList.Item(0).IsNewRegistration Then 'If _printTehExamReportList.Item(0).IsNewCustomer Then '
                lblNovaReg.Text = pom.RegistrationCode & "-"
            Else
                lblNovaReg.Text = _printTehExamReportList.Item(0).LastRegistration

            End If
      'If _printTehExamReportList.Item(0).ForPrivateTransportNotPublic Then
      '  CheckBoxPersonal.Visible = True
      'Else
      '  CheckBoxPublic.Visible = True
      'End If

      Select Case _printTehExamReportList.Item(0).IdTypeOfTehnicalExam
        Case 1
          CheckBoxRedoven.Visible = True
        Case 2
          CheckBoxRedovenNa6.Visible = True
        Case 3
          CheckBoxDelumno.Visible = True
        Case 4
          CheckBoxPotpoln.Visible = True
      End Select
            'Dim detali As DocumentsTehnicalExamsReportsDetails = _
            'DocumentsTehnicalExamsReport.GetDocumentsTehnicalExamsReport(idTexReport).Details
            'For Each detal As DocumentsTehnicalExamsReportsDetail In detali
            '  Dim pomVehiclePart As TehnicalExamVehiclePartsInfo = TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsListById(detal.IdTehnicalExamVehivlePart)
            '  Dim strName As String = "CheckBoxD" & pomVehiclePart.Code
            '  Select Case pomVehiclePart.Code
            '    Case 1
            '      CheckBoxD1.Visible = True
            '    Case 2
            '      CheckBoxD2.Visible = True
            '    Case 3
            '      CheckBoxD3.Visible = True
            '    Case 4
            '      CheckBoxD4.Visible = True
            '    Case 5
            '      CheckBoxD5.Visible = True
            '    Case 6
            '      CheckBoxD6.Visible = True
            '    Case 7
            '      CheckBoxD7.Visible = True
            '    Case 8
            '      CheckBoxD8.Visible = True
            '    Case 9
            '      CheckBoxD9.Visible = True
            '    Case 10
            '      CheckBoxD10.Visible = True
            '    Case 11
            '      CheckBoxD11.Visible = True
            '    Case 12
            '      CheckBoxD12.Visible = True
            '    Case 13

            '    Case 14
            '      CheckBoxD14.Visible = True
            '    Case 15
            '      CheckBoxD15.Visible = True
            '    Case 16
            '      CheckBoxD16.Visible = True
            '    Case 17
            '      CheckBoxD17.Visible = True
            '    Case 18
            '      CheckBoxD18.Visible = True
            '    Case 19
            '      CheckBoxD19.Visible = True
            '    Case 20
            '      CheckBoxD20.Visible = True
            '    Case 21
            '      CheckBoxD21.Visible = True
            '    Case 22

            '    Case 23
            '      CheckBoxD23.Visible = True
            '    Case 24
            '      CheckBoxD24.Visible = True
            '    Case 25
            '      CheckBoxD25.Visible = True
            '    Case 26
            '      CheckBoxD26.Visible = True
            '    Case 27
            '      CheckBoxD27.Visible = True
            '    Case 28
            '      CheckBoxD28.Visible = True
            '    Case 29
            '      CheckBoxD29.Visible = True
            '    Case 30
            '      CheckBoxD30.Visible = True
            '    Case 31
            '      CheckBoxD31.Visible = True
            '    Case 32
            '      CheckBoxD32.Visible = True
            '    Case 33
            '      CheckBoxD33.Visible = True
            '    Case 34
            '      CheckBoxD34.Visible = True
            '    Case 35

            '    Case 36
            '      CheckBoxD36.Visible = True
            '    Case 37
            '      CheckBoxD37.Visible = True
            '    Case 38
            '      CheckBoxD38.Visible = True
            '    Case 39
            '      CheckBoxD39.Visible = True
            '    Case 40
            '      CheckBoxD40.Visible = True
            '    Case 41
            '      CheckBoxD41.Visible = True
            '    Case 42
            '      CheckBoxD42.Visible = True
            '    Case 43
            '      CheckBoxD43.Visible = True
            '    Case 44
            '      CheckBoxD44.Visible = True
            '    Case 45
            '      CheckBoxD45.Visible = True
            '    Case 46
            '      CheckBoxD46.Visible = True
            '    Case 47
            '      CheckBoxD47.Visible = True
            '    Case 48
            '      CheckBoxD48.Visible = True
            '    Case 49

            '    Case 50
            '      CheckBoxD50.Visible = True
            '    Case 51
            '      CheckBoxD51.Visible = True
            '    Case 52
            '      CheckBoxD52.Visible = True
            '    Case 53
            '      CheckBoxD53.Visible = True
            '    Case 54
            '      CheckBoxD54.Visible = True
            '    Case 55
            '      CheckBoxD55.Visible = True
            '    Case 56

            '    Case 57
            '      CheckBoxD57.Visible = True
            '    Case 58
            '      CheckBoxD58.Visible = True
            '    Case 59
            '      CheckBoxD59.Visible = True
            '    Case 60
            '      CheckBoxD60.Visible = True
            '    Case 61

            '    Case 62
            '      CheckBoxD62.Visible = True
            '    Case 63
            '      CheckBoxD63.Visible = True
            '    Case 64
            '      CheckBoxD64.Visible = True
            '    Case 65

            '    Case 66
            '      CheckBoxD66.Visible = True
            '    Case 67
            '      CheckBoxD67.Visible = True
            '    Case 68

            '    Case 69
            '      CheckBoxD69.Visible = True
            '    Case 70
            '      CheckBoxD70.Visible = True
            '    Case 71
            '      CheckBoxD71.Visible = True
            '    Case 72
            '      CheckBoxD72.Visible = True
            '    Case 73
            '      CheckBoxD73.Visible = True
            '    Case 74
            '      CheckBoxD74.Visible = True
            '    Case 75
            '      CheckBoxD75.Visible = True
            '    Case 76
            '      CheckBoxD76.Visible = True
            '    Case 77
            '      CheckBoxD77.Visible = True
            '    Case 78
            '      CheckBoxD78.Visible = True
            '    Case 79
            '      CheckBoxD79.Visible = True
            '    Case 80
            '      CheckBoxD80.Visible = True
            '    Case 81
            '      CheckBoxD81.Visible = True
            '    Case 82

            '    Case 83

            '    Case 84
            '      CheckBoxD84.Visible = True
            '    Case 85
            '      CheckBoxD85.Visible = True
            '    Case 86
            '      CheckBoxD86.Visible = True
            '    Case 87
            '      CheckBoxD87.Visible = True
            '    Case 88
            '      CheckBoxD88.Visible = True
            '    Case 89
            '      CheckBoxD89.Visible = True
            '    Case 90

            '  End Select
            'Next
      _isRight = _printTehExamReportList.Item(0).VehicleIsRight
    Catch ex As Exception

    End Try
    ' Add any initialization after the InitializeComponent() call.

  End Sub
  Public ReadOnly Property IsRight() As Boolean
    Get
      Return _isRight
    End Get
  End Property
End Class