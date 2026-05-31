Imports Microsoft.Win32
Public Class dijGarant

    Private WithEvents _dogovor As PaymentRatiDogovor
    Public ReadOnly Property Dogovor() As PaymentRatiDogovor
        Get
            Return _dogovor
        End Get
    End Property
    Private _prvaRata As Decimal
    Public ReadOnly Property PrvaRata() As Decimal
        Get
            Return _prvaRata
        End Get
    End Property
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        _dogovor = PaymentRatiDogovor.NewPaymentRatiDogovor
        ' Add any initialization after the InitializeComponent() call.
        PaymentRatiDogovorBindingSource.DataSource = _dogovor
        BindUI()
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(ByVal customer As String, ByVal address As String, ByVal mb As String, ByVal brrati As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        _dogovor = PaymentRatiDogovor.NewPaymentRatiDogovor
        ' Add any initialization after the InitializeComponent() call.
        PaymentRatiDogovorBindingSource.DataSource = _dogovor
        BindUI()
        _dogovor.GarantNaziv = customer
        _dogovor.GarantAdresa = address
        _dogovor.GartEMB = mb
        _dogovor.Datum = Now.Date
        _dogovor.BrNaRati = brrati
        BrNaRatiSpinEdit.Focus()
    End Sub



    
    Protected Sub UnbindBindingSource( _
    ByVal source As BindingSource, ByVal apply As Boolean, ByVal isRoot As Boolean)

        Dim current As System.ComponentModel.IEditableObject = _
                TryCast(source.Current, System.ComponentModel.IEditableObject)
        If isRoot Then
            source.DataSource = Nothing
        End If
        If current IsNot Nothing Then
            If apply Then
                current.EndEdit()
            Else
                current.CancelEdit()
            End If
        End If

    End Sub
    Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
        ' disable events
        Me.PaymentRatiDogovorBindingSource.RaiseListChangedEvents = False

        Try

            UnbindBindingSource(Me.PaymentRatiDogovorBindingSource, saveObject, True)

            ' save or cancel changes
            If saveObject Then
                _dogovor.ApplyEdit()
                Try
                    'Dim tmp As Customer = _customer.Clone
                    _prvaRata = txtPrvaRata.EditValue
                    _dogovor = _dogovor.Save 'tmp.Save
                Catch ex As Csla.Validation.ValidationException
                    MsgBox("Some validation errors has occurred," + vbCrLf + " please check Broken rules collection on bottom for details")

                Catch ex As Csla.DataPortalException
                    MessageBox.Show(ex.BusinessException.ToString(), _
                      "Error saving", MessageBoxButtons.OK, _
                      MessageBoxIcon.Exclamation)

                Catch ex As Exception
                    MessageBox.Show(ex.ToString(), _
                      "Error Saving", MessageBoxButtons.OK, _
                      MessageBoxIcon.Exclamation)
                End Try
            Else
                _dogovor.CancelEdit()
            End If
        Finally
            ' rebind UI if requested
            If rebind Then
                BindUI()
            End If

            ' restore events
            Me.PaymentRatiDogovorBindingSource.RaiseListChangedEvents = True

            If rebind Then
                ' refresh the UI if rebinding
                Me.PaymentRatiDogovorBindingSource.ResetBindings(False)

            End If

        End Try
    End Sub

    Private Sub BindUI()
        _dogovor.BeginEdit()
        Me.PaymentRatiDogovorBindingSource.DataSource = _dogovor
    End Sub

    Private Sub PaymentRatiDogovorBindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PaymentRatiDogovorBindingSource.CurrentItemChanged
        btnOK.Enabled = _dogovor.IsValid
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        RebindUI(True, True)
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dijGarant_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Select Case Asc(e.KeyChar)
            Case 13
                e.Handled = True
                SendKeys.Send("{TAB}")
        End Select
    End Sub
End Class