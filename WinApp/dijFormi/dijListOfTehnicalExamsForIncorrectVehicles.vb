Public Class dijListOfTehnicalExamsForIncorrectVehicles 

    Private WithEvents _docList As DocumentsTehnicalExamsReportList
 Private _isRight As Boolean = True
  Private _dokTehExamReport As DocumentsTehnicalExamsReport
  Public ReadOnly Property DokTehExamReport() As DocumentsTehnicalExamsReport
    Get
      Return _dokTehExamReport
    End Get
  End Property
 
  Public Sub New(ByVal InIsRight As Boolean)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
        Me.Text = My.Resources.uxDocumentTehnicalExamReportsListIncorect
    ' Add any initialization after the InitializeComponent() call.
  _isRight = InIsRight

        If Not _isRight Then
            _docList = DocumentsTehnicalExamsReportList.GetDocumentsTehnicalExamsReportListNotRightVehicles(Now.Date, Now.Date, InIsRight)
        Else
            _docList = DocumentsTehnicalExamsReportList.GetDocumentsTehnicalExamsReportListOdDo(Now.Date, Now.Date)
        End If
        Dim user As System.Security.Principal.IPrincipal = _
       Csla.ApplicationContext.User
        SimpleButton1.Visible = user.IsInRole("Administrator")
    End Sub

    'Public Sub New(ByVal InPom As Integer)

    '  ' This call is required by the Windows Form Designer.
    '  InitializeComponent()

    '  ' Add any initialization after the InitializeComponent() call.

    '  _docList = DocumentsTehnicalExamsReportList.GetDocumentsTehnicalExamsReportListVallidTill(InPom)
    '      Dim user As System.Security.Principal.IPrincipal = _
    '      Csla.ApplicationContext.User
    '      SimpleButton1.Visible = User.IsInRole("Administrator")
    'End Sub

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
        Me.Text = My.Resources.uxDocumentTehnicalExamReportsList
    ' Add any initialization after the InitializeComponent() call.
        _docList = DocumentsTehnicalExamsReportList.GetDocumentsTehnicalExamsReportListOdDo(Now.Date, Now.Date)

  Dim user As System.Security.Principal.IPrincipal = _
        Csla.ApplicationContext.User
        SimpleButton1.Visible = user.IsInRole("Administrator")
  End Sub
  'Public Sub New(ByVal strFilter As String)

  '  ' This call is required by the Windows Form Designer.
  '  InitializeComponent()

  '  ' Add any initialization after the InitializeComponent() call.
  '  _docList = DocumentsTehnicalExamsReportList.GetDocumentsTehnicalExamsReportList
  '  GridView1.Columns("ValidTillDate").FilterInfo = New DevExpress.XtraGrid.Columns.ColumnFilterInfo(strFilter)
  'End Sub


  Private Sub dijListOfTehnicalExamsForIncorrectVehicles_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.DocumentsTehnicalExamsReportListBindingSource.DataSource = _docList
        DateEdit1.EditValue = Now
        DateEdit2.EditValue = Now
  End Sub

  Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
  Handles btnSelect.Click, HyperLinkEditId.DoubleClick, HyperLinkCustomerName.DoubleClick, HyperLinkRegNumber.DoubleClick
    If Me.DocumentsTehnicalExamsReportListBindingSource.Position >= 0 Then
      Dim pomId As Long = (Me.DocumentsTehnicalExamsReportListBindingSource.Current).Id
      _dokTehExamReport = DocumentsTehnicalExamsReport.GetDocumentsTehnicalExamsReport(pomId)
      Me.DialogResult = Windows.Forms.DialogResult.OK
      Me.Close()
    End If
  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
  End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        If MsgBox("Дали сакате да го избришете техничкиот преглед", MsgBoxStyle.YesNoCancel, "Внимание") = MsgBoxResult.Yes Then
            Try
                Using busy As New Splash("Бришам...")
                    Dim idTehExam As Long
                    idTehExam = _docList.Item(Me.DocumentsTehnicalExamsReportListBindingSource.Position).Id

                    Dim requestPom As Request
                    Try

                        requestPom = Request.GetRequestByIdTehEx(idTehExam, 2)
                        If requestPom.Id > 0 Then
                            requestPom.IdTechnicalExamReport = 0
                            requestPom.Save()
                        Else
                            requestPom = Nothing
                        End If
                    Catch ex As Exception
                        requestPom = Nothing
                        Exit Try
                    End Try

                    Dim dok As DocumentsTehnicalExamsReport = _
                           DocumentsTehnicalExamsReport.GetDocumentsTehnicalExamsReport(idTehExam)
                    dok.Details.Clear()
                    dok.Save()
                    dok.Delete()
                    dok.Save()
                End Using
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
 
  If Not _isRight Then
   _docList = DocumentsTehnicalExamsReportList.GetDocumentsTehnicalExamsReportListNotRightVehicles(DateEdit1.DateTime, DateEdit2.DateTime, _isRight)
  Else
   _docList = DocumentsTehnicalExamsReportList.GetDocumentsTehnicalExamsReportListOdDo(DateEdit1.DateTime, DateEdit2.DateTime)
  End If


  Me.DocumentsTehnicalExamsReportListBindingSource.DataSource = _docList
    End Sub
End Class