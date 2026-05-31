Public Class rptTrafficLicenceExtension

  Public Sub New(ByVal validTill As Date, ByVal position As Integer, ByVal note As String)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    Dim pTop As Integer = 150
    Dim pLeft As Integer = 50
    If validTill.Date <> Date.MinValue Then
      Me.lbl.Text = validTill.Date
    Else
      Me.lbl.Text = ""
    End If
    Me.lblNote.Text = note
    Select Case position
      Case 0
        Me.lbl.Top = pTop
        Me.lblNote.Top = pTop
        Me.lbl.Left = 2 * pLeft + lbl.Width
      Case 1
        Me.lbl.Top = 2 * pTop + lbl.Height
        Me.lblNote.Top = 2 * pTop + lbl.Height
        Me.lbl.Left = pLeft
      Case 2
        Me.lbl.Top = 2 * pTop + lbl.Height
        Me.lblNote.Top = 2 * pTop + lbl.Height
        Me.lbl.Left = 2 * pLeft + lbl.Width
      Case 3
        Me.lbl.Top = 3 * pTop + lbl.Height + lbl.Height
        Me.lblNote.Top = 3 * pTop + lbl.Height + lbl.Height
        Me.lbl.Left = pLeft
      Case 4
        Me.lbl.Top = 3 * pTop + lbl.Height + lbl.Height
        Me.lblNote.Top = 3 * pTop + lbl.Height + lbl.Height
        Me.lbl.Left = 2 * pLeft + lbl.Width
    End Select

  End Sub

End Class