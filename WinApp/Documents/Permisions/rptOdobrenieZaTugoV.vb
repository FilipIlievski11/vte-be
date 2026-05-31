Imports System.Drawing.Imaging
Imports DevExpress.XtraReports.UI
Public Class rptOdobrenieZaTugoV
  Private WithEvents _printPermisionList As printPermisionList
  Dim _vozilo As Vehicle
  Public Sub New(ByVal inIdDocPermission As Long)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    Me.Margins.Top = objOpcii.OdobrenieTopMargin
    Me.Margins.Left = objOpcii.OdobrenieLeftMargin
    Me.Margins.Right = objOpcii.OdobrenieRightmargin
    Me.Margins.Bottom = objOpcii.OdobrenieButtonMargin

    ' Add any initialization after the InitializeComponent() call.
    _printPermisionList = printPermisionList.GetprintPermisionList(inIdDocPermission)
    Me.BindingSourcePermission.DataSource = _printPermisionList
    Try
      If _printPermisionList.Item(0).VehicleDisplay.Length > 19 Then
        lblMadeAndModel2.Font = New Font("Times New Roman", 9, FontStyle.Bold)
        lblMadeAndModel2.Multiline = True
        'lblMadeAndModel2.Text = pomModelType
      End If
      'Dim pomModelType As String = VehicleModelList.GetVehicleModelList.GetVehicleModelInfoById(_vozilo.IdVehicleModel).MakerModel
      'lblMadeAndModel1.Text = pomModelType
      'If pomModelType.ToString.Length > 19 Then
      '  lblMadeAndModel2.Font = New Font("Times New Roman", 8)
      '  lblMadeAndModel2.Multiline = True
      '  lblMadeAndModel2.Text = pomModelType
      'End If
    Catch ex As Exception

    End Try

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
  Private Sub lblIzdadenOd_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles lblIzdadenOd.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e)
    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Bold)
  End Sub

  Private Sub lblOwnerLine2_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles lblOwnerLine2.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e)
    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Bold)
  End Sub

  Private Sub lblCustomerDisplay1_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles lblCustomerDisplay1.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e)
    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Bold)
  End Sub

  Private Sub lblOwnerLine1_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles lblOwnerLine1.BeforePrint
    Dim golemina As Single = GetStringWidth(sender, e)
    CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Bold)
  End Sub
End Class