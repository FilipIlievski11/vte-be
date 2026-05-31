Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.Xpo
Imports DevExpress.XtraEditors
Imports System.Threading

Public Class Splash
  Implements IDisposable
    'ovie za splash
    'NEKOJA PROBA
  Private thread As Thread
  Private [Event] As ManualResetEvent = New ManualResetEvent(False)
  Private _message As String
  Private disposedValue As Boolean = False    ' To detect redundant calls
  Sub New(ByVal strPokazi As String)
    'pokazi go splash
    _message = strPokazi
    thread = New Thread(AddressOf t_DoWork)
    thread.Priority = ThreadPriority.AboveNormal
    thread.Start()

  End Sub
  ' IDisposable
  Protected Overridable Sub Dispose(ByVal disposing As Boolean)
    If Not Me.disposedValue Then
      If disposing Then
        ' TODO: free managed resources when explicitly called
      End If


      'uxMainForm.UnitOfWork1.CommitChanges()
      thread.Sleep(20)
      [Event].Set()

    End If
    Me.disposedValue = True
  End Sub

  Private Sub t_DoWork()
    Dim f As uxSplash = New uxSplash([Event], _message)
    'f.LookAndFeel.SkinName = uSettings.Skin
    'f.TopMost = True
    f.StartPosition = FormStartPosition.CenterScreen
    f.ShowDialog()
    f.Dispose()
    f = Nothing
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
