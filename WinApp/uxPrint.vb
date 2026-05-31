Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.Native

Public Class uxPrint

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return "Печатење"

  End Function

  Public Overrides Function ToString() As String

    Return "Печатење"

  End Function

  Private Sub uxEdinicniMeri_CurrentPrincipalChanged( _
    ByVal sender As Object, _
    ByVal e As System.EventArgs) _
    Handles Me.CurrentPrincipalChanged
  End Sub


#End Region



  Public Sub New(ByVal report As XtraReport)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.

    report.PrintingSystem = Me.PrintingSystem1
    report.CreateDocument()



  End Sub

  Private Sub PrintPreviewBarItem25_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles PrintPreviewBarItem25.ItemClick
    Me.Close()
  End Sub

  Private Sub uxPrint_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Me.IsShowingBrokenRules = False
  End Sub
End Class
