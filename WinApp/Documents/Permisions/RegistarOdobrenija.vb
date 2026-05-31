Public Class RegistarOdobrenija

  Private _list As DocumentsPermisionsList

  Public Sub New(ByVal inDoc As DocumentsPermisionsList, ByVal dateOd As String, ByVal dateDo As String)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    _list = inDoc
    txtOdDo.Text = "за период од " & dateOd & " до " & dateDo
    Me.BindingSource1.DataSource = _list
    ' Add any initialization after the InitializeComponent() call.

  End Sub
End Class