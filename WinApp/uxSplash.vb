Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports System.Threading

Public Class uxSplash

  Private v As ManualResetEvent

  Public Sub New()
    InitializeComponent()
    'Me.DefaultLookAndFeel1.LookAndFeel.SetSkinStyle(uSettings.Skin)
  End Sub

  Public Sub New(ByVal v As ManualResetEvent, ByVal strMessage As String)
    Me.v = v
    InitializeComponent()
    'Me.DefaultLookAndFeel1.LookAndFeel.SetSkinStyle(uSettings.Skin)
    Me.EmptySpaceItem1.Text = strMessage
  End Sub

  Private Sub uxSplash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    BackgroundWorker1.RunWorkerAsync()
  End Sub
  Private Sub backgroundWorker1_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork
    v.WaitOne()
  End Sub
  Private Sub backgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
    Close()
  End Sub
End Class