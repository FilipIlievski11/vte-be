Public Class dijValidTrafficLicences 

  Private WithEvents _traficLicenceList As DocumentsTrafficLicencesList

  Private _trafficLicence As DocumentsTrafficLicence
  Public ReadOnly Property TrafficLicence() As DocumentsTrafficLicence
    Get
      Return _trafficLicence
    End Get
  End Property
  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
    Try
            _traficLicenceList = DocumentsTrafficLicencesList. _
            GetDocumentsTrafficLicencesListOdDo(Today.Date, Today.Date)
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub
 
  Private Sub dijVehicleList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Me.DocumentsTrafficLicencesListBindingSource.DataSource = _traficLicenceList
    DocumentsTrafficLicencesListGridControl.ForceInitialize()
    DocumentsTrafficLicencesListGridControl.Focus()
        '
        DateEdit1.EditValue = Today.Date
        DateEdit2.EditValue = Today.Date
    Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
    Me.GridView1.FocusedColumn = colTrafficLicenceNumber
  End Sub

  Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
  Handles btnSelect.Click, HyperLinkEditTrafficLicenceNumber.DoubleClick, HyperLinkEditCustomer.DoubleClick
        If Me.DocumentsTrafficLicencesListBindingSource.Position >= 0 Then
            Dim traffLicenceID As Integer = (Me.DocumentsTrafficLicencesListBindingSource.Current).ID
            _trafficLicence = DocumentsTrafficLicence.GetDocumentsTrafficLicence(traffLicenceID)
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
  End Sub

  Private Sub dij_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
    End Select
  End Sub

  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    If GridView1.FocusedColumn IsNot Nothing Then
      Select Case GridView1.FocusedColumn.Name
        Case "colTrafficLicenceNumber", "colVehicleLastRegistration", "colShellNumber", "colVehicleMaker", "colVehicleModel"
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
        Case Else
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
      End Select
    End If
  End Sub

  Private Sub btnAddExtension_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddExtension.Click

  End Sub

  Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    PrintableComponentLink1.CreateDocument()
    PrintableComponentLink1.ShowPreview()
  End Sub

  Private Sub RepositoryItemButtonEdit1_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles RepositoryItemButtonEdit1.ButtonPressed
    Select Case e.Button.Index
      Case 0
                'pecati
                If Me.DocumentsTrafficLicencesListBindingSource.Position >= 0 Then
                    Dim _vehicle As VehicleInfo
                    _vehicle = VehicleList.GetVehicleById _
            (_traficLicenceList.Item(Me.DocumentsTrafficLicencesListBindingSource.Position).IdVehicle)
                    '_vehicle = VehicleList.GetVehicleList.GetVehicleListById(_traficLicenceList.Item(Me.DocumentsTrafficLicencesListBindingSource.Position).IdVehicle)
                    Dim dij As New dijKratkaDolga
                    ' Dim dij As New dijKratkaDolga1234
                    If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        Select Case dij.Dolga
                            Case 0
                                Dim stampaTrafficLicence As rptTrafficLicence = New rptTrafficLicence(_trafficLicence.Id)
                                Dim ux As New uxPrint(stampaTrafficLicence)
                                CType(Me.ParentForm, MainForm).AddWinPart(ux)
                            Case 1
                                Dim stampaTrafficLicence As rptTrafficLicenceZaTraktori = New rptTrafficLicenceZaTraktori(_trafficLicence.Id)
                                Dim ux As New uxPrint(stampaTrafficLicence)
                                CType(Me.ParentForm, MainForm).AddWinPart(ux)
                            Case 2
                                Dim stampaTrafficLicence As rptTrafficLicenceZaTraktoriZemjodelski = New rptTrafficLicenceZaTraktoriZemjodelski(_trafficLicence.Id)
                                Dim ux As New uxPrint(stampaTrafficLicence)
                                CType(Me.ParentForm, MainForm).AddWinPart(ux)
                        End Select
                        'Select Case dij.Dolga
                        '    Case 0
                        '        Dim stampaTrafficLicence As rptTrafficLicence1 = New rptTrafficLicence1(_trafficLicence.Id)
                        '        Dim ux As New uxPrint(stampaTrafficLicence)
                        '        CType(Me.ParentForm, MainForm).AddWinPart(ux)
                        '    Case 1
                        '        Dim stampaTrafficLicence As rptTrafficLicence2 = New rptTrafficLicence2(_trafficLicence.Id)
                        '        Dim ux As New uxPrint(stampaTrafficLicence)
                        '        CType(Me.ParentForm, MainForm).AddWinPart(ux)
                        '    Case 2
                        '        Dim stampaTrafficLicence As rptTrafficLicence3 = New rptTrafficLicence3(_trafficLicence.Id)
                        '        Dim ux As New uxPrint(stampaTrafficLicence)
                        '        CType(Me.ParentForm, MainForm).AddWinPart(ux)
                        '    Case 3
                        '        Dim stampaTrafficLicence As rptTrafficLicence4 = New rptTrafficLicence4(_trafficLicence.Id)
                        '        Dim ux As New uxPrint(stampaTrafficLicence)
                        '        CType(Me.ParentForm, MainForm).AddWinPart(ux)
                        'End Select
                    End If

                End If
        End Select
  End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        _traficLicenceList = DocumentsTrafficLicencesList. _
          GetDocumentsTrafficLicencesListOdDo(DateEdit1.DateTime, DateEdit2.DateTime)
        Me.DocumentsTrafficLicencesListBindingSource.DataSource = _traficLicenceList

    End Sub
End Class