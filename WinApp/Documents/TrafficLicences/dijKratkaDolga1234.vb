Public Class dijKratkaDolga1234
    Private _dolga As Integer = 0
    Public ReadOnly Property Dolga() As Integer
        Get
            Return _dolga
        End Get
    End Property
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'If RadioGroup1.SelectedIndex = 0 Then
        '    _dolga = True
        'Else
        '    _dolga = False
        'End If
        _dolga = RadioGroup1.SelectedIndex
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class