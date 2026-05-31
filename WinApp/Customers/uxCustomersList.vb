Public Class uxCustomersList
    Private WithEvents _customersList As CustomersListShort '= CustomersListShort.GetCustomersList

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.

    ApplyAuthorizationRules()
  End Sub

  Private Sub uxCustomersList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle

    End Select
  End Sub

  Private Sub uxCustomersList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _customersList = CustomersListShort.GetCustomersList 'CustomersListShort.GetCustomersList
    If _customersList IsNot Nothing Then
      Me.CustomersListBindingSource.DataSource = _customersList
      Dim tmp As Boolean = (_customersList.Count > 0)
      btnShowDetails.Enabled = tmp
    End If
    CustomersListGridControl.ForceInitialize()
    CustomersListGridControl.Focus()
    Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
  End Sub

  Private Sub ApplyAuthorizationRules()
    ' kontroli(eanble / disable)
    Me.ReadWriteAuthorization1.ResetControlAuthorization()

    If Not VTE.Library.Customers.CanGetObject Then
      Me.Close()
    End If

    Me.btnAddNew.Enabled = VTE.Library.Customers.CanAddObject

    'Me.btnDelete.Enabled = VTE.Library.Customers.CanDeleteObject


  End Sub

#Region " KeyPress "

  Private Sub uxCustomersList_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxCustomers

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxCustomers

  End Function

  Private Sub uxCustomersList_CurrentPrincipalChanged( _
    ByVal sender As Object, _
    ByVal e As System.EventArgs) _
    Handles Me.CurrentPrincipalChanged
    ApplyAuthorizationRules()
  End Sub


#End Region

  Private Sub ShowCustomer()
    Dim par As MainForm = Me.ParentForm
    Dim customer As Customer
    Using busy As New StatusBusy(My.Resources.LoadingData)
      Try
        customer = customer.GetCustomer(Me._customersList.Item(Me.CustomersListBindingSource.Position).Id)
        For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
          For Each ctl As Control In page.Controls
            If (TypeOf ctl Is uxCustomers) AndAlso (CType(ctl, uxCustomers).Customer.Id = customer.Id) Then
              par.ShowWinPart(CType(ctl, uxCustomers))
              Exit Sub
            End If
          Next
        Next

        'Me.Close()
        par.AddWinPart(New uxCustomers(customer.GetCustomer(_customersList.Item(Me.CustomersListBindingSource.Position).Id)))
      Catch ex As Csla.DataPortalException
        MessageBox.Show(ex.BusinessException.ToString, _
          "Error loading", MessageBoxButtons.OK, _
          MessageBoxIcon.Exclamation)
      Catch ex As Exception
        MessageBox.Show(ex.ToString, _
          "Error loading", MessageBoxButtons.OK, _
          MessageBoxIcon.Exclamation)
      End Try
    End Using
  
  End Sub

  Private Sub NewCustomer()
    Dim par As MainForm = Me.ParentForm
    Dim customer As Customer = customer.NewCustomer
    'Me.Close()
    par.AddWinPart(New uxCustomers(customer))
  End Sub


  Private Sub btnShowDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowDetails.Click
    ShowCustomer()
  End Sub

  Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
    NewCustomer()
  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
  End Sub


  Private Sub HyperLinkEditCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HyperLinkEditCustomer.DoubleClick
    ShowCustomer()
  End Sub

  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    If GridView1.FocusedColumn IsNot Nothing Then
      Select Case GridView1.FocusedColumn
        Case Else
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
      End Select
    End If
  End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim customer As Customer
        customer = customer.GetCustomer(Me._customersList.Item(Me.CustomersListBindingSource.Position).Id)
        Using cekaj As New StatusBusy(My.Resources.txtLoading)
            Try

                Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
                    Case MsgBoxResult.Yes

                        customer.Delete()
                        customer.Save()
                  
                End Select

            Catch ex As Exception

            End Try
        End Using
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        _customersList = CustomersListShort.GetCustomersList
        Me.CustomersListBindingSource.DataSource = _customersList
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Me.PrintableComponentLink1.ShowPreviewDialog(Me)
    End Sub
End Class
