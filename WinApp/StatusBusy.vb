Public Class StatusBusy

  Implements IDisposable

  Private _OldStatus As String
  Private _OldCursor As Cursor

  Public Sub New(ByVal statusText As String)

    _OldStatus = insMainForm.BarStaticStatus.Caption
    insMainForm.BarStaticStatus.Caption = statusText
    _OldCursor = insMainForm.Cursor
    insMainForm.Cursor = Cursors.WaitCursor
    insMainForm.Refresh()
  End Sub

  Private disposedValue As Boolean = False    ' To detect redundant calls
  ' IDisposable
  Protected Overridable Sub Dispose(ByVal disposing As Boolean)
    If Not Me.disposedValue Then
      If disposing Then

        insMainForm.BarStaticStatus.Caption = _OldStatus
        insMainForm.Cursor = _OldCursor

      End If
    End If
    Me.disposedValue = True
  End Sub

#Region " IDisposable Support "
  ' This code added by Visual Basic to correctly implement the disposable pattern.
  Public Sub Dispose() Implements IDisposable.Dispose
    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub
#End Region

End Class
