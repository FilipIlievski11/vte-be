Public Class RegistarMegunarodni
  Private _list As DocumentsInternationalDriveingLicenceList
  Public Sub New(ByVal inDoc As DocumentsInternationalDriveingLicenceList, ByVal dateOd As String, ByVal dateDo As String)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    txtOdDo.Text = "за период од " & dateOd & " до " & dateDo
    _list = inDoc
    Me.BindingSource1.DataSource = _list
    ' Add any initialization after the InitializeComponent() call.

  End Sub
End Class