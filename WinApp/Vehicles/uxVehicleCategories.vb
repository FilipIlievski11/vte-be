Public Class uxVehicleCategories
    ' Private WithEvents _vehicleFieldList As VehicleFieldList
    Private WithEvents _VehicleCategorie As VehicleCategories
    ' Private WithEvents _vehicleBodyTypeList As VehicleBodytypeList
    ' Private WithEvents _vehicleUseList As VehicleUseList
    '  Private WithEvents _vehiclePaymentCategoryList As VehicleCategoryForPaymentsList
  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.

  End Sub

#Region "PritisnatoKopce"

  Private Sub uxVehicleCategorie_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")

    End Select
  End Sub
#End Region

#Region "WinPart"

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxVehicleCategories
  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxVehicleCategories
  End Function

#End Region

    Private Sub ApplyAuthorizationRules()
        Dim user As System.Security.Principal.IPrincipal = _
    Csla.ApplicationContext.User

        If Not VTE.Library.VehicleCategories.CanGetObject Then
            Me.Close()

        End If
        If User.IsInRole("Administrator") Then
            LayoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            LayoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        Else
            LayoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
        Me.UxKopcinja1.cmdAdd.Enabled = VTE.Library.VehicleCategories.CanAddObject
        Me.UxKopcinja1.cmdDelete.Enabled = VTE.Library.VehicleCategories.CanDeleteObject
        Me.UxKopcinja1.cmdSave.Enabled = VTE.Library.VehicleCategories.CanEditObject
    End Sub

  Private Sub uxVehicleCategories_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      ApplyAuthorizationRules()
      LoadLists()
            _VehicleCategorie = VehicleCategories.GetVehicleCategories
      If _VehicleCategorie IsNot Nothing Then
        Me.VehicleCategoriesBindingSource.DataSource = _VehicleCategorie
        Me.UxKopcinja1.cmdSave.Enabled = _VehicleCategorie.IsSavable
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

  Private Sub LoadLists()
        '_vehicleFieldList = VehicleFieldList.GetList
        Me.VehicleFieldListBindingSource.DataSource = objVehicleFieldList
        '_vehicleBodyTypeList = VehicleBodytypeList.GetVehicleBodytypeList
        Me.VehicleBodytypeListBindingSource.DataSource = objVehicleBodytypeList
        '_vehicleUseList = VehicleUseList.GetVehicleUseList
        Me.VehicleUseListBindingSource.DataSource = objVehicleUseList
        '  _vehiclePaymentCategoryList = VehicleCategoryForPaymentsList.GetVehicleCategoryForPaymentsList
        Me.VehicleCategoryForPaymentsListBindingSource.DataSource = objVehicleCategoryForPaymentsList

  End Sub

  Private Sub uxVehicleCategories_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub

  Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
    Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name
      Case "cmdAdd"
        _VehicleCategorie.AddNew()
        GridView1.Focus()

      Case "cmdSave"
        RebindUI(True, True)
        UxKopcinja1.cmdAdd.Focus()

      Case "cmdDelete"
        If _VehicleCategorie.Count > 0 Then
          _VehicleCategorie.RemoveAt(VehicleCategoriesBindingSource.Position)
        End If

      Case "cmdCancel"
        RebindUI(False, True)

      Case "cmdExit"
        If _VehicleCategorie.IsDirty Then
          Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
            Case MsgBoxResult.Yes
              If VTE.Library.VehicleCategories.CanEditObject Then
                RebindUI(True, False)
                Me.Close()
              Else
                Me.Close()
              End If
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
    Me.VehicleCategoriesBindingSource.RaiseListChangedEvents = False
    Me.RequiredFieldsBindingSource.RaiseListChangedEvents = False
    Me.RelationsBindingSource.RaiseListChangedEvents = False
    Me.DisabledFieldsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.RequiredFieldsBindingSource, saveObject, False)
    UnbindBindingSource(Me.RelationsBindingSource, saveObject, False)
    UnbindBindingSource(Me.DisabledFieldsBindingSource, saveObject, False)
    UnbindBindingSource(Me.VehicleCategoriesBindingSource, saveObject, True)
    Me.RequiredFieldsBindingSource.DataSource = Me.VehicleCategoriesBindingSource
    Me.RelationsBindingSource.DataSource = Me.VehicleCategoriesBindingSource
    Me.DisabledFieldsBindingSource.DataSource = Me.VehicleCategoriesBindingSource
    Try
      ' save or cancel changes
      If saveObject Then
        Try
                    _VehicleCategorie = _VehicleCategorie.Save
                Catch ex As Csla.Validation.ValidationException
                    MsgBox(My.Resources.ValidationError)

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
        _VehicleCategorie = Nothing
        Try
          _VehicleCategorie = VehicleCategories.GetVehicleCategories
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
      Me.VehicleCategoriesBindingSource.DataSource = _VehicleCategorie
      Me.VehicleCategoriesBindingSource.RaiseListChangedEvents = True
      Me.RequiredFieldsBindingSource.RaiseListChangedEvents = True
      Me.RelationsBindingSource.RaiseListChangedEvents = True
      Me.DisabledFieldsBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.VehicleCategoriesBindingSource.ResetBindings(False)
        Me.RequiredFieldsBindingSource.ResetBindings(False)
        Me.RelationsBindingSource.ResetBindings(False)
        Me.DisabledFieldsBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub


  'Private Sub GridControl1_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridControl1.ProcessGridKey
  '  Select Case e.KeyCode
  '    Case Keys.Add, Keys.Oemplus
  '      VehicleCategoriesBindingSource.AddNew()
  '      GridView1.Focus()
  '  End Select
  'End Sub

  Private Sub VehicleCategoriesBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles VehicleCategoriesBindingSource.CurrentItemChanged, RequiredFieldsBindingSource.CurrentItemChanged, _
     RelationsBindingSource.CurrentItemChanged, DisabledFieldsBindingSource.CurrentItemChanged
    Try

      UxKopcinja1.cmdSave.Enabled = _VehicleCategorie.IsDirty

      UxKopcinja1.cmdCancel.Enabled = _VehicleCategorie.IsDirty

      Dim message As New System.Text.StringBuilder
      message.AppendFormat("{0}" + vbCrLf, "")
      For Each child As VTE.Library.VehicleCategorie In _VehicleCategorie
        For Each rule As Csla.Validation.BrokenRule In _
              child.BrokenRulesCollection
          message.AppendFormat( _
            "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
        Next
        For Each det As VehicleRequiredField In child.RequiredFields
          For Each rule As Csla.Validation.BrokenRule In _
                det.BrokenRulesCollection
            message.AppendFormat( _
              "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
          Next

        Next
        For Each det As VehicleDisabledField In child.DisabledFields
          For Each rule As Csla.Validation.BrokenRule In _
                det.BrokenRulesCollection
            message.AppendFormat( _
              "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
          Next

        Next
      Next
      ShowBrokenRules(message.ToString)
    Catch
    End Try
  End Sub


  Private Sub RequiredFieldsBindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _
  RequiredFieldsBindingSource.ListChanged, RelationsBindingSource.ListChanged
    If e.ListChangedType = System.ComponentModel.ListChangedType.ItemDeleted Then
      UxKopcinja1.cmdSave.Enabled = _VehicleCategorie.IsDirty
      UxKopcinja1.cmdCancel.Enabled = _VehicleCategorie.IsDirty
    End If
  End Sub

  Private Sub GridView3_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView3.FocusedColumnChanged
    If GridView3.FocusedColumn Is colIdBodytype Then
      System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    Else
      System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
    End If
  End Sub
End Class





