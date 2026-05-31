Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid

Public Class uxPaymentCategories
 Private WithEvents _paymentCategories As PaymentCategories
 Private WithEvents _ddvList As DDVList
 Private WithEvents _vehicleCategoryForPaymentsList As VehicleCategoryForPaymentsList
 Private WithEvents _vehicleFieldList As VehicleFieldList
 Private WithEvents _calculationItemList As CalculationItemList
 Private WithEvents _communityList As CommunitiesList = objCommunityList
 Public Sub New()

  ' This call is required by the Windows Form Designer.
  InitializeComponent()

  ' Add any initialization after the InitializeComponent() call.
  Try
   _paymentCategories = PaymentCategories.GetPaymentCategoriesByIdCompany

   LoadList()

   BindUI()

   Me.UxKopcinja1.cmdSave.Enabled = _paymentCategories.IsSavable
   Me.UxKopcinja1.cmdCancel.Enabled = _paymentCategories.IsDirty


   ApplyAuthorizationRules()
  Catch ex As Exception
   MsgBox(ex.Message)
  End Try
 End Sub

 Private Sub uxPaymentCategories_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  Try

   Me.UxKopcinja1.cmdSave.Enabled = _paymentCategories.IsSavable
   Me.UxKopcinja1.cmdCancel.Enabled = _paymentCategories.IsDirty
   Me.GridControl1.ForceInitialize()

  Catch ex As Exception
   MsgBox(ex.Message)
  End Try
 End Sub

 Private Sub ApplyAuthorizationRules()
  Dim tmp As Boolean = VTE.Library.PaymentCategories.CanGetObject
  If Not tmp Then
   Me.Close()
  End If
  Me.UxKopcinja1.cmdAdd.Enabled = tmp
  Me.UxKopcinja1.cmdDelete.Enabled = tmp
  Me.UxKopcinja1.cmdSave.Enabled = tmp
 End Sub

#Region " Bindings "
 Private Sub LoadList()
  '_ddvList = DDVList.GetDDVList
  Me.DDVListBindingSource.DataSource = objDDVList
  _vehicleCategoryForPaymentsList = VehicleCategoryForPaymentsList.GetVehicleCategoryForPaymentsList
  Me.VehicleCategoryForPaymentsListBindingSource.DataSource = _vehicleCategoryForPaymentsList
  _vehicleFieldList = VehicleFieldList.GetList
  Me.VehicleFieldListBindingSource.DataSource = _vehicleFieldList
  _calculationItemList = CalculationItemList.GetCalculationItemList
  Me.CalculationItemListBindingSource.DataSource = _calculationItemList
  Me.CommunitiesListBindingSource.DataSource = _communityList
  'Me.CompanyListBindingSource.DataSource = objCompanyList
 End Sub

 Private Sub BindUI()
  '_paymentCategories.BeginEdit()
  Me.PaymentCategoriesBindingSource.DataSource = _paymentCategories
 End Sub

 Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
  ' stop the flow of events
  Me.PaymentCategoriesBindingSource.RaiseListChangedEvents = False
  Me.PaymentsItemsBindingSource.RaiseListChangedEvents = False
  Me.ParametarsBindingSource.RaiseListChangedEvents = False
  ' commit edits in memory

  UnbindBindingSource(Me.ParametarsBindingSource, saveObject, False)
  UnbindBindingSource(Me.PaymentsItemsBindingSource, saveObject, False)
  UnbindBindingSource(Me.PaymentCategoriesBindingSource, saveObject, True)

  Me.ParametarsBindingSource.DataSource = Me.PaymentsItemsBindingSource
  Me.PaymentsItemsBindingSource.DataSource = Me.PaymentCategoriesBindingSource

  Try
   ' save or cancel changes
   If saveObject Then
    '_paymentCategories.ApplyEdit()

    Try
     _paymentCategories = _paymentCategories.Save
     'stavi vo memorija
     objPaymentCatalogList = PaymentCataologList.GetPaymentCataologList
     If Csla.ApplicationContext.LocalContext.Contains("objPaymentCatalogList") Then
      Csla.ApplicationContext.LocalContext.Remove("objPaymentCatalogList")
     End If
     Csla.ApplicationContext.LocalContext.Add("objPaymentCatalogList", objPaymentCatalogList)

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
    _paymentCategories.CancelEdit()
   End If
  Finally
   If rebind Then
    _paymentCategories = PaymentCategories.GetPaymentCategoriesByIdCompany
    BindUI()
   End If
   Me.PaymentCategoriesBindingSource.RaiseListChangedEvents = True
   Me.PaymentsItemsBindingSource.RaiseListChangedEvents = True
   Me.ParametarsBindingSource.RaiseListChangedEvents = True
   If rebind Then
    Me.PaymentCategoriesBindingSource.ResetBindings(False)
    Me.PaymentsItemsBindingSource.ResetBindings(False)
    Me.ParametarsBindingSource.ResetBindings(False)
   End If

  End Try
 End Sub

 Private Sub BindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
   PaymentCategoriesBindingSource.CurrentItemChanged, _
   PaymentsItemsBindingSource.CurrentItemChanged, _
   ParametarsBindingSource.CurrentItemChanged
  Try

   UxKopcinja1.cmdSave.Enabled = _paymentCategories.IsSavable
   UxKopcinja1.cmdCancel.Enabled = _paymentCategories.IsDirty
   Dim message As New System.Text.StringBuilder
   message.AppendFormat("{0}" + vbCrLf, "")
   For Each child As VTE.Library.PaymentCategorie In _paymentCategories
    For Each rule As Csla.Validation.BrokenRule In _
          child.BrokenRulesCollection
     message.AppendFormat( _
       "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
    Next
    For Each it As PaymentItem In child.PaymentsItems
     For Each rule As Csla.Validation.BrokenRule In _
           it.BrokenRulesCollection
      message.AppendFormat( _
        "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
     Next

     For Each parm As PaymentItemParametar In it.Parametars
      For Each rule As Csla.Validation.BrokenRule In _
            parm.BrokenRulesCollection
       message.AppendFormat( _
         "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
      Next
     Next
    Next
   Next
   ShowBrokenRules(message.ToString)
  Catch
  End Try
 End Sub


 Private AllowShowEditor As Boolean = True

 Private Sub GridView1_FocusedRowChanged1(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
 Handles GridView1.FocusedRowChanged, GridView4.FocusedRowChanged, GridView5.FocusedRowChanged
  AllowShowEditor = False
  BeginInvoke(New MethodInvoker(AddressOf tr))
 End Sub
 Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) _
 Handles GridView1.FocusedColumnChanged, GridView4.FocusedColumnChanged, GridView5.FocusedColumnChanged
  AllowShowEditor = False
  BeginInvoke(New MethodInvoker(AddressOf tr))
 End Sub
 Private Sub GridView1_ShowingEditor1(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) _
 Handles GridView1.ShowingEditor, GridView4.ShowingEditor, GridView5.ShowingEditor
  e.Cancel = Not AllowShowEditor
 End Sub

 Private Sub tr()
  AllowShowEditor = True
 End Sub


 Private Sub BindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _
   PaymentsItemsBindingSource.ListChanged, _
   ParametarsBindingSource.ListChanged

  If (Me.PaymentsItemsBindingSource IsNot Nothing) AndAlso (e.ListChangedType = System.ComponentModel.ListChangedType.ItemDeleted) Then
   Me.UxKopcinja1.cmdSave.Enabled = _paymentCategories.IsValid
  End If

  If (Me.ParametarsBindingSource IsNot Nothing) AndAlso (e.ListChangedType = System.ComponentModel.ListChangedType.ItemDeleted) Then
   Me.UxKopcinja1.cmdSave.Enabled = _paymentCategories.IsValid
  End If
 End Sub

 Private Sub ExpandAllDetails()
  GridView1.BeginUpdate()
  Try
   Dim dataRowCount As Integer = GridView1.DataRowCount
   Dim rHandle As Integer
   For rHandle = 0 To dataRowCount - 1
    RecursExpand(GridView1, rHandle)
   Next
  Finally
   GridView1.EndUpdate()
  End Try
 End Sub

 Public Sub RecursExpand(ByVal masterView As GridView, ByVal masterRowHandle As Integer)
  ' Prevent excessive visual updates.
  masterView.BeginUpdate()
  Try
   ' Get the number of master-detail relationships.
   Dim relationCount As Integer = masterView.GetRelationCount(masterRowHandle)
   ' Iterate through relationships.
   Dim index As Integer
   For index = relationCount - 1 To 0 Step -1
    ' Open the detail View for the current relationship.
    masterView.ExpandMasterRow(masterRowHandle, index)
    ' Get the detail View.
    Dim childView As ColumnView = CType(masterView.GetDetailView(masterRowHandle, index), DevExpress.XtraGrid.Views.Base.ColumnView)
    If TypeOf childView Is GridView Then
     ' Get the number of rows in the detail View.
     Dim childRowCount As Integer = CType(childView, GridView).DataRowCount
     ' Expand child rows recursively.
     Dim handle As Integer
     For handle = 0 To childRowCount - 1
      RecursExpand(CType(childView, DevExpress.XtraGrid.Views.Grid.GridView), handle)
     Next
    End If
   Next
  Finally
   ' Enable visual updates.
   masterView.EndUpdate()
  End Try
 End Sub


#End Region


#Region " KeyPress "
 Private Sub ux_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
  Select Case Asc(e.KeyChar)
   Case 13
    SendKeys.Send("{TAB}")

  End Select
 End Sub

#End Region

#Region " WinPart "

 Protected Overrides Function GetIdValue() As Object

  Return My.Resources.uxPaymentCategories

 End Function

 Public Overrides Function ToString() As String

  Return My.Resources.uxPaymentCategories

 End Function

 Private Sub ux_CurrentPrincipalChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.CurrentPrincipalChanged
  ApplyAuthorizationRules()
 End Sub

#End Region

 Private Sub UxKopcinja1_kopce_klik(ByVal sender As Object, ByVal e As System.EventArgs) Handles UxKopcinja1.kopce_klik
  Select Case CType(sender, DevExpress.XtraEditors.SimpleButton).Name
   Case "cmdAdd"
    _paymentCategories.AddNew()
    GridView1.Focus()

   Case "cmdSave"
    RebindUI(True, True)
    UxKopcinja1.cmdAdd.Focus()

   Case "cmdDelete"
    If _paymentCategories.Count > 0 Then
     _paymentCategories.RemoveAt(PaymentCategoriesBindingSource.Position)
    End If

   Case "cmdCancel"
    RebindUI(False, True)

   Case "cmdExit"
    If _paymentCategories.IsDirty Then
     Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
      Case MsgBoxResult.Yes
       If VTE.Library.DDVCatalogs.CanEditObject Then
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

End Class
