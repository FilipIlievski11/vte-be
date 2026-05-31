Public Class dijCustomersList 
    Private WithEvents _customers As CustomersListShort
    Private _Customer As CustomersInfoShort
    Public ReadOnly Property Customer() As CustomersInfoShort
        Get
            Return _Customer
        End Get
    End Property
  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
        _customers = CustomersListShort.GetCustomersList

  End Sub



  Private Sub dijCustomersList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Me.CustomersListBindingSource.DataSource = _customers

    CustomersListGridControl.ForceInitialize()
    CustomersListGridControl.Focus()
    Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
  End Sub

  Private Sub HyperLinkEditName_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles HyperLinkEditName.DoubleClick
    If Me.CustomersListBindingSource.Position >= 0 Then
            _Customer = _customers.Item(Me.CustomersListBindingSource.Position)
      Me.DialogResult = Windows.Forms.DialogResult.OK
      Me.Close()
    End If
  End Sub

  Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
    Dim par As MainForm = Me.ParentForm
    Dim customerN As Customer
    Using busy As New StatusBusy(My.Resources.LoadingData)
      Try
        customerN = VTE.Library.Customer.NewCustomer
        par.ShowWinPart(New uxCustomers(customerN))
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

  Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
    If Me.CustomersListBindingSource.Position >= 0 Then
      _Customer = _customers.Item(Me.CustomersListBindingSource.Position)
      Me.DialogResult = Windows.Forms.DialogResult.OK
      Me.Close()
    End If
  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
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

  Private Sub dijVehicleList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
    End Select
  End Sub

End Class