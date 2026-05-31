Public Class uxListOfNotEndedRequests
    Private WithEvents _requestsList As ActiveDocumentList 'ActiveDocumentsDepList
    Private WithEvents _document As Document
    Private WithEvents _documentTypeOption As DocumentTypesOptionsInfo
    Private WithEvents _docType As DocumentType
    Private _isOpen As Boolean
#Region "PritisnatoKopce"

    Private Sub uxListOfNotEndedRequests_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DateEdit1.EditValue = Today.Date.AddDays(-7)
        DateEdit2.EditValue = Today.Date

        If _isOpen Then

            LayoutApprove.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            LayoutCancel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            LayoutSelect.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

        Else

            LayoutApprove.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutCancel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutSelect.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never

        End If
    End Sub

    Private Sub uxRequestNotEndedList_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
        Select Case Asc(e.KeyChar)
            Case 13
                SendKeys.Send("{TAB}")
        End Select
    End Sub
#End Region

#Region "WinPart"

    Protected Overrides Function GetIdValue() As Object
        If _isOpen Then
            Return My.Resources.uxRequestsOpen
        Else
            Return My.Resources.uxRequestsClosed
        End If
    End Function

    Public Overrides Function ToString() As String
        If _isOpen Then
            Return My.Resources.uxRequestsOpen
        Else
            Return My.Resources.uxRequestsClosed
        End If
    End Function

#End Region

    Public Sub New(ByVal isOpen As Boolean)
        _isOpen = isOpen
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Try
            'If _isOpen Then
   _requestsList = ActiveDocumentList.GetActiveDocumentList(_isOpen, True) 'ActiveDocumentsDepList.GetActiveDocumentsDepList
            LayoutApprove.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            LayoutCancel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            'Else
            '_requestsList = ActiveDocumentList.GetActiveDocumentList(Today.Date.AddDays(-7), Today.Date, False) 'ActiveDocumentsDepList.GetClosedDocumentsDepList(True)
            'LayoutApprove.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            'LayoutCancel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            'End If
            Me.ActiveDocumentsDepListBindingSource.DataSource = _requestsList
            Me.RequestTypeListBindingSource.DataSource = RequestTypeList.GetRequestTypeList
        Catch ex As Exception
            MsgBox(ex)

        End Try
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click, HyperLinkEditRequestNum.DoubleClick
        Try
            Using bus As New Splash(My.Resources.LoadingData)
                Dim _request As Request = Request.GetRequest(Me._requestsList.Item(Me.ActiveDocumentsDepListBindingSource.Position).Id)
                Dim par As MainForm = Me.ParentForm
                For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
                    For Each ctl As Control In page.Controls
                        If (TypeOf ctl Is uxRequestEdit) AndAlso (CType(ctl, uxRequestEdit).request.Id = _request.Id) Then
                            par.ShowWinPart(CType(ctl, uxRequestEdit))
                            Exit Sub
                        End If
                    Next
                Next
                par.AddWinPart(New uxRequestEdit(_request))

            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        'Dim par As MainForm = Me.ParentForm
        '' Dim doc As Document

        'Using busy As New StatusBusy(My.Resources.LoadingData)
        '  Try
        '    If _requestsList.Count > 0 Then
        '      _document = Document.GetDocument(Me._requestsList.Item(Me.DocumentActiveListBindingSource.Position).Id)
        '      _docType = DocumentType.GetDocumentType(_document.IdDocumentType)
        '      _documentTypeOption = DocumentTypesOptionsList.GetDocumentTypesOptionsList.GetDocumentTypesOptionsListById(_document.IdDocumentTypeOption)
        '    Else
        '      Exit Sub
        '    End If
        '    For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        '      For Each ctl As Control In page.Controls
        '        If (TypeOf ctl Is uxRequest) AndAlso (CType(ctl, uxRequest).Document.Id = _document.Id) Then
        '          par.ShowWinPart(CType(ctl, uxRequest))
        '          Exit Sub
        '        End If
        '      Next
        '    Next
        '    par.AddWinPart(New uxRequest(_docType, _document))
        '  Catch ex As Csla.DataPortalException
        '    MessageBox.Show(ex.BusinessException.ToString, _
        '      "Error loading", MessageBoxButtons.OK, _
        '      MessageBoxIcon.Exclamation)
        '  Catch ex As Exception
        '    MessageBox.Show(ex.ToString, _
        '      "Error loading", MessageBoxButtons.OK, _
        '      MessageBoxIcon.Exclamation)
        '  End Try
        'End Using
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnApproveit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApproveit.Click
        If Me.ActiveDocumentsDepListBindingSource.Position > -1 Then
            Dim activeRequest As ActiveDocumentsDepInfo = _
      Me._requestsList(Me.ActiveDocumentsDepListBindingSource.Position)
            'proveri dali treba da se stavi/izmeni registracija
            Dim _request As Request = Request.GetRequest(activeRequest.Id)
            Dim reqType As RequestTypeInfo = _
               CType(Csla.ApplicationContext.LocalContext("objRequestTypeList"), RequestTypeList).getInfoById(activeRequest.IdRequestType)

            If reqType.IsNewRegistration Then
                Dim curRel As CustomerVehiclesRelation = CustomerVehiclesRelation.GetCustomerVehiclesRelation(_request.IdCustomerVehicleRelation)
                Dim _vehicle = Vehicle.GetVehicle(curRel.IdVehicle)
                Dim dij As New dijNewRegistration(_vehicle)
                dij.ShowDialog(Me.ParentForm)
                'Dim dij As New dijAddNewRegistration(_vehicle)
                'dij.ShowDialog(Me.ParentForm)
            
            End If

            Try
                _request.CloseRequest()

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            'Try
            '  'Dim pomRelation As Document = Document.get
            '  _document = Document.GetDocument(Me._requestsList.Item(Me.DocumentActiveListBindingSource.Position).Id)
            '  Dim pomVehicle As Vehicle = Vehicle.GetVehicle _
            '  (CustomerVehiclesRelation.GetCustomerVehiclesRelation(_document.IdCustomerVehicleRelation).IdVehicle)
            '  If _documentTypeOption.IsNewRegistration Then
            '    Dim dij As New dijAddNewRegistration(pomVehicle)
            '    If dij.ShowDialog() = Windows.Forms.DialogResult.OK Then
            '      _document.IdOperatorEnded = Csla.ApplicationContext.LocalContext.Item("EmployeeID")
            '      _document.Save()
            '      Exit Sub
            '    Else
            '      Exit Sub
            '    End If
            '  End If
            '  Dim dokType As DocumentType = DocumentType.GetDocumentType(_document.IdDocumentType)
            '  If dokType.IsTechnicalExamRequired AndAlso _document.IdTechnicalExamReport < 1 Then
            '    MsgBox("не е извршен технички преглед за возилото")
            '  Else
            '    _document.IdOperatorEnded = Csla.ApplicationContext.LocalContext.Item("EmployeeID")
            '    _document.Save()
            '  End If
            'Catch ex As Exception
            '  MessageBox.Show(ex.Message)
            'End Try
        End If
        
    End Sub

    'Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
    '  _documentTypeOption = DocumentActiveList.GetDocumentList.item(GridView1.FocusedRowHandle).Options
    'End Sub

    ' Private Sub DocumentActiveListBindingSource_PositionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    'Try

    '    _document = Document.GetDocument(Me._requestsList.Item(Me.DocumentActiveListBindingSource.Position).Id)
    '    _documentTypeOption = DocumentTypesOptionsList.GetDocumentTypesOptionsList.GetDocumentTypesOptionsListById _
    '    (_document.IdDocumentTypeOption)
    'Catch ex As Exception

    'End Try
    ' End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If MsgBox("Дали сакате да го откажете барањето", MsgBoxStyle.YesNoCancel, "Внимание") = MsgBoxResult.Yes Then
            Using busy As New Splash("Бришам...")
                Try
                    Dim _request As Request = Request.GetRequest(Me._requestsList.Item(Me.ActiveDocumentsDepListBindingSource.Position).Id)
                    'priveri dali e nova relacija i izbrisi ja taa

                    _request.Delete()
                    _request.Save()
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End Using
        End If
        '_document = Document.GetDocument(Me._requestsList.Item(Me.DocumentActiveListBindingSource.Position).Id)
        '_document.Delete()
        '_document.Save()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim rpt As DevExpress.XtraReports.UI.XtraReport
        Dim RequestTInfo As RequestTypeInfo = objRequestTypeList.getInfoById _
        (_requestsList.Item(Me.ActiveDocumentsDepListBindingSource.Position).IdRequestType)

        Dim reqId As Long = (_requestsList.Item(Me.ActiveDocumentsDepListBindingSource.Position).Id)
        Select Case RequestTInfo.IdDocumentPrint
            Case 1
                Dim print As PrintZelenList = PrintZelenList.GetPrintZelenList(reqId)
                rpt = New printZelen(print)
            Case 2
                Dim print As PrintPlavList = PrintPlavList.GetPrintPlavList(reqId)
                rpt = New PrintPlav(print)
            Case Else
                Dim print As printBelList = printBelList.GetprintBelList(reqId)
                rpt = New printBel(print)

        End Select


        Dim ux As New uxPrint(rpt)
        'ux.Show()
        CType(Me.ParentForm, MainForm).AddWinPart(ux)
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
  '  _requestsList = ActiveDocumentList.GetActiveDocumentList(DateEdit1.DateTime, DateEdit2.DateTime, _isOpen)
  _requestsList = ActiveDocumentList.GetActiveDocumentList(_isOpen, True)
        Me.ActiveDocumentsDepListBindingSource.DataSource = _requestsList
    End Sub

  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    If GridView1.FocusedColumn IsNot Nothing Then
      Select Case GridView1.FocusedColumn.Name
        Case "colVehicleDisplay"
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
        Case Else
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
      End Select
    End If
  End Sub
End Class
