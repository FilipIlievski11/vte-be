Imports System.Drawing.Imaging
Imports DevExpress.XtraReports.UI

Public Class rptTrafficLicence
  Private WithEvents _trafficLicence As PrintTrafficLicenceInfo
  Public Sub New(ByVal inIdTrafficLicence As Long)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    Try

      _trafficLicence = PrintTrafficLicenceList.GetPrintTrafficLicenceList(inIdTrafficLicence).Item(0)
      Me.BindingSourceTrafficLicenceList.DataSource = _trafficLicence

      'If _trafficLicence.MaximunAllowedWaight = 0 Then
      '    Me.lblMaksimalnaMasa.Visible = False
      'End If
      'If _trafficLicence.NumberOfSeats = 0 Then
      '    Me.lblBrojNaSedista.Visible = False
      'End If
      'If _trafficLicence.NumberOfStandingSeats = 0 Then
      '    Me.lblBrojNaStoeckiMesta.Visible = False
      'End If
      'If _trafficLicence.NumberOfLieingSeats = 0 Then
      '    Me.lblBrojNaLezeckiMesta.Visible = False
      'End If
      'Select Case _trafficLicence.IdVehicleCategory
      '    Case 16, 17, 18, 19
      '        'If _trafficLicence.NumberOfAxis = 0 Then
      '        '    Me.lblBrojNaOski.Visible = False
      '        'End If
      '        Me.lblBrojNaOski.Visible = True
      '    Case Else
      '        Me.lblBrojNaOski.Visible = False
      'End Select
      If Me._trafficLicence.FirstRegistrationNumber = String.Empty Then
        Me.XrLabel1.Visible = False
        Me.XrLabel2.Visible = False
        Me.XrLabel3.Visible = False
      End If
      If _trafficLicence.TNG Then
        lblTNG.Visible = True
      Else
        lblTNG.Visible = False
      End If

      If _trafficLicence.IsCompany AndAlso _trafficLicence.CustomerSurname = String.Empty Then
        lblFirstName.Text = _trafficLicence.CustomerFirstName
        Dim factor As Int32
        Dim gr As Graphics = Graphics.FromHwnd(IntPtr.Zero)
        If Me.ReportUnit = ReportUnit.HundredthsOfAnInch Then
          gr.PageUnit = GraphicsUnit.Inch
          factor = 100
        Else
          gr.PageUnit = GraphicsUnit.Millimeter
          factor = 11
        End If

        Dim size As SizeF = gr.MeasureString(lblFirstName.Text, lblFirstName.Font)
        Dim tempgolemina As Single = lblFirstName.Font.Size
        If size.Width * factor > lblFirstName.Width() Then
          Dim dolzina As Integer = _trafficLicence.CustomerFirstName.Length
          Dim prazniMesta As Integer = 1

          For i As Integer = 0 To dolzina - 1
            If _trafficLicence.CustomerFirstName.Chars(i) = " " Then
              prazniMesta += 1
            End If
          Next
          Dim pozicija(prazniMesta - 1) As Integer
          Dim j As Integer = 0
          For i As Integer = 0 To dolzina - 1
            If _trafficLicence.CustomerFirstName.Chars(i) = " " Then
              pozicija(j) = i
              j += 1
            End If
          Next
          Dim pola As Integer = Math.Round(dolzina / 2)
          Dim odDo As Integer = 0
          For i As Integer = 0 To (prazniMesta - 1)
            If pozicija(i) >= pola Then
              odDo = pozicija(i)
              Exit For
            End If
          Next
          lblSurename.Text = _trafficLicence.CustomerFirstName.Substring(0, odDo)
          lblFirstName.Text = _trafficLicence.CustomerFirstName.Substring(odDo + 1, (dolzina - odDo - 1))

        End If

        gr.Dispose()
      Else
        lblFirstName.Text = _trafficLicence.CustomerFirstName
        lblSurename.Text = _trafficLicence.CustomerSurname
      End If

    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

    ' Add any initialization after the InitializeComponent() call.

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

  Private Sub XrLabel5_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles lblFirstName.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e)
    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Regular)
  End Sub

  Private Sub XrLabel4_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles lblSurename.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e)

    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Regular)


  End Sub



  Private Sub lblStreet_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles lblStreet.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e) * 2
    If golemina < 11 Then
      CType(sender, XRLabel).TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
      CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Regular)
    Else
      CType(sender, XRLabel).TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
      CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, 11, FontStyle.Regular)
    End If
  End Sub

  Private Sub XrLabel14_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel14.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e) * 2
    If golemina < 11 Then
      CType(sender, XRLabel).TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
      CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Regular)
    Else
      CType(sender, XRLabel).TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
      CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, 11, FontStyle.Regular)
    End If
  End Sub

  Private Sub XrLabel15_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel15.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e)
    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Regular)
  End Sub

  Private Sub XrLabel16_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel16.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e)
    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Regular)
  End Sub

  Private Sub lblPrimaryColor_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles lblPrimaryColor.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e)
    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Regular)
  End Sub

  Private Sub XrLabel11_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel11.BeforePrint
    XrLabel11.Text = _trafficLicence.EngineType + _trafficLicence.EngineNumber
    Dim golemina As Single = GetStringWidth(sender, e)
    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Regular)
  End Sub

  Private Sub XrLabel22_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel22.BeforePrint

        Dim golemina As Single = GetStringWidth(sender, e) * 1.5
    If golemina < 11 Then
      CType(sender, XRLabel).TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
      CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Regular)
    Else
      CType(sender, XRLabel).TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
      CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, 11, FontStyle.Regular)
    End If

  End Sub

  Private Sub XrLabel29_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrLabel29.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e) * 3
    If golemina < 11 Then
      CType(sender, XRLabel).TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
      CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Regular)
    Else
      CType(sender, XRLabel).TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
      CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, 11, FontStyle.Regular)
    End If
  End Sub
End Class