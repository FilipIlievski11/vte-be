Public Class dijDocumentRequestsWithoutTehnicalExam

  Private WithEvents _docReguestsList As DocumentActiveList
  Private _customerVehicleRalationId As Integer
  Public ReadOnly Property CustomerVehicleRelationId() As Integer
    Get
      Return _customerVehicleRalationId
    End Get
  End Property

  Private _documentRequestId As Long
  Public ReadOnly Property DocumentRequestId() As Long
    Get
      Return _documentRequestId
    End Get
  End Property

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.

    _docReguestsList = DocumentActiveList.GetDocumentListWithoutTehExam()

  End Sub
 
  Private Sub dijDocumentRequestsWithoutTehnicalExam_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Me.DocumentActiveListBindingSource.DataSource = _docReguestsList
  End Sub


  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
  End Sub



  Private Sub btnCreateTehExamReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateTehExamReport.Click, HyperLinkEditShellNum.DoubleClick
    Dim _docActiveInfo As DocumentActiveInfo = _docReguestsList.Item(Me.DocumentActiveListBindingSource.Position)
    If Me.DocumentActiveListBindingSource.Position >= 0 Then
      _customerVehicleRalationId = _docActiveInfo.IdCustomerVehicleRelation
      _documentRequestId = _docActiveInfo.Id
      Me.DialogResult = Windows.Forms.DialogResult.OK
      Me.Close()
    End If
  End Sub
End Class