Imports System.Drawing.Imaging
Imports DevExpress.XtraReports.UI

Public Class rptTrafficLicence4
    Private WithEvents _trafficLicence As PrintTrafficLicenceInfo
    Public Sub New(ByVal inIdTrafficLicence As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.PrintingSystem.ShowMarginsWarning = False

        _trafficLicence = PrintTrafficLicenceList.GetPrintTrafficLicenceList(inIdTrafficLicence).Item(0)
        Me.BindingSourceTrafficLicenceList.DataSource = _trafficLicence

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
    Private Sub XrLabel_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles XrTableCell20.BeforePrint, XrTableCell26.BeforePrint, XrTableCell32.BeforePrint, XrTableCell50.BeforePrint
        Dim golemina As Single = GetStringWidth(sender, e)
        CType(sender, XRLabel).Font = New Font(CType(sender, XRLabel).Font.Name, golemina, FontStyle.Regular)
    End Sub

End Class