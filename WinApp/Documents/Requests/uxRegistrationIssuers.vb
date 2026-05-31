Public Class uxRegistrationIssuers

  Private WithEvents _issuers As RegistrationIssuers

#Region " KeyPress "

  Private Sub uxDocumentPaymentProof_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxRegistrationIssuersCaption

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxRegistrationIssuersCaption

  End Function
#End Region



  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
        CommunitiesListBindingSource.DataSource = objCommunityList
    ' Add any initialization after the InitializeComponent() call.

  End Sub

  Private Sub uxRegistrationIssuers_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    ApplyAuthorizationRules()
    Try
      _issuers = RegistrationIssuers.GetRegistrationIssuers
      If _issuers IsNot Nothing Then
        Me.RegistrationIssuersBindingSource.DataSource = _issuers
        Me.UxKopcinja1.cmdSave.Enabled = _issuers.IsSavable
        Me.UxKopcinja1.cmdCancel.Enabled = _issuers.IsDirty
      End If
    Catch ex As Csla.DataPortalException
      MessageBox.Show(ex.BusinessException.ToString, _
        "Error loading", MessageBoxButtons.OK, _
        MessageBoxIcon.Exclamation)
    Catch ex As Exception
      MessageBox.Show(ex.ToString, _
        "Error loading", MessageBoxButtons.OK, _
        MessageBoxIcon.Exclamation)
    End Try
  End Sub

  Private Sub ApplyAuthorizationRules()
    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()
    Dim canEdit As Boolean = VTE.Library.RegistrationIssuers.CanGetObject
    If Not canEdit Then
      Me.Close()
    End If

    Me.UxKopcinja1.cmdSave.Enabled = canEdit
    RegistrationIssuersGridControl.Enabled = canEdit

    Me.UxKopcinja1.cmdAdd.Enabled = canEdit

    Me.UxKopcinja1.cmdDelete.Enabled = canEdit
  End Sub


  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdAdd"
        Me.RegistrationIssuersBindingSource.AddNew()
        GridView1.Focus()

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdDelete"
        If _issuers.Count <> 0 Then
          _issuers.RemoveAt(Me.RegistrationIssuersBindingSource.Position)
        End If
      Case "cmdExit"
        If _issuers.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If DocumentTypes.CanEditObject Then
                RebindUI(True, False)
              End If
              Me.Close()
            Case MsgBoxResult.No
              RebindUI(False, False)
              Me.Close()
            Case MsgBoxResult.Cancel
              Exit Sub
          End Select
        Else
          Me.Close()
        End If

    End Select
  End Sub

  Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
    ' stop the flow of events
    Me.RegistrationIssuersBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.RegistrationIssuersBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _issuers = _issuers.Save
        Catch ex As Csla.DataPortalException
          MessageBox.Show(ex.BusinessException.ToString, _
            "Error saving", MessageBoxButtons.OK, _
            MessageBoxIcon.Exclamation)

        Catch ex As Exception
          MessageBox.Show(ex.ToString, _
            "Error saving", MessageBoxButtons.OK, _
            MessageBoxIcon.Exclamation)
        End Try

      Else
        _issuers = Nothing
        Try
          _issuers = RegistrationIssuers.GetRegistrationIssuers
        Catch ex As Csla.DataPortalException
          MessageBox.Show(ex.BusinessException.ToString, _
            "Error saving", MessageBoxButtons.OK, _
            MessageBoxIcon.Exclamation)

        Catch ex As Exception
          MessageBox.Show(ex.ToString, _
            "Error saving", MessageBoxButtons.OK, _
            MessageBoxIcon.Exclamation)

        End Try
      End If
    Finally
      Me.RegistrationIssuersBindingSource.DataSource = _issuers


      Me.RegistrationIssuersBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.RegistrationIssuersBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub RegistrationIssuersBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
  Handles RegistrationIssuersBindingSource.CurrentItemChanged
    Try
      UxKopcinja1.cmdSave.Enabled = _issuers.IsSavable
      UxKopcinja1.cmdCancel.Enabled = _issuers.IsDirty
      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As RegistrationIssuer In _issuers
        For Each rule As Csla.Validation.BrokenRule In _
              child.BrokenRulesCollection
          message.AppendFormat( _
            "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
        Next
      Next
      ShowBrokenRules(message.ToString)
    Catch
    End Try
  End Sub

End Class
