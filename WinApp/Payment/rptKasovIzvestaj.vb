Public Class rptKasovIzvestaj
  Public Sub New(ByVal DateStart As Date, _
                 ByVal DateEnd As Date, ByVal InOperator As Integer)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False
    Dim kasovalista As KasovList = KasovList.GetKasovList(DateStart, DateEnd, InOperator)
    BindingSource1.DataSource = kasovalista
    ' Add any initialization after the InitializeComponent() call.
    ' Dim organization As TehnicalExamOrganizationsInfo = _
    ' TehnicalExamOrganizationsList.GetTehnicalExamOrganizationsList. _
    'GetTehnicalExamOrganizationsInfoById(objOpcii.Company)
    lblOrganization.Text = objCurentTehExamOrganization.OrganizationAndStationName
    lblDateStart.Text = DateStart.Date
    lblDateEnd.Text = DateEnd.Date
    'Dim vkupno As Double = 0
    'For Each child As KasovInfo In KasovList.GetKasovList(DateStart, DateEnd, InOperator)
    '    vkupno += child.Price
    'Next
    'lblVkupno.Text = vkupno
    If InOperator > 0 Then
      lblOperator.Text = User.GetUser(InOperator).UserFullName
    Else
      lblOperator.Text = "яхре ноепюрнпх"
    End If
  End Sub

End Class