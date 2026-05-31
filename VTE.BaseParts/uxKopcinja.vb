Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.Data
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Public Class uxKopcinja

  Public Event kopce_klik(ByVal sender As System.Object, ByVal e As System.EventArgs)

  Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
    RaiseEvent kopce_klik(sender, e)
  End Sub

  Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
    RaiseEvent kopce_klik(sender, e)
  End Sub

  Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
    RaiseEvent kopce_klik(sender, e)
  End Sub

  Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
    RaiseEvent kopce_klik(sender, e)
  End Sub

  Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
    RaiseEvent kopce_klik(sender, e)
  End Sub


End Class
