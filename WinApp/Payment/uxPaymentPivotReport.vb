Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPivotGrid



Public Class uxPaymentPivotReport
 Private WithEvents _payments As PrintPaymentDocumetnByIdDocumetnList
 Private WithEvents _paymentTypeList As PaymentTypeList
 Private WithEvents _customersList As CustomersSearchList


 Public Sub New()

  ' This call is required by the Windows Form Designer.
  InitializeComponent()

  ' Add any initialization after the InitializeComponent() call.

 End Sub

#Region " WinPart Code "

 Protected Overrides Function GetIdValue() As Object

  Return My.Resources.uxPaymentPivotReport '"Извештај за плаќања"

 End Function

 Public Overrides Function ToString() As String

  Return My.Resources.uxPaymentPivotReport '"Извештај за плаќања"

 End Function

 Private Sub uxEdinicniMeri_CurrentPrincipalChanged( _
   ByVal sender As Object, _
   ByVal e As System.EventArgs) _
   Handles Me.CurrentPrincipalChanged
 End Sub


#End Region

 Private Sub uxPaymentPivotReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  Me.deStartDate.DateTime = DateAndTime.Now.AddHours(-Now.Hour).AddMinutes(-Now.Minute)
  Me.deEndDate.DateTime = DateAndTime.Now.AddHours(23).AddMinutes(59)

  _payments = PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList(Format(Me.deStartDate.EditValue.date, "yyyy-MM-dd"), Format(Me.deEndDate.EditValue.Date, "yyyy-MM-dd"), True)
  '_customersList = objCustomersListShort
  'Me.CustomersListShortBindingSource.DataSource = _customersList

  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.RaiseListChangedEvents = False
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.DataSource = _payments
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.RaiseListChangedEvents = True

  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.ResetBindings(False)

 End Sub

 Private Sub RefreshData()

  If ceRange.Checked Then
   _payments = _
             PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList()
  Else
   _payments = _
             PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList( _
             Format(Me.deStartDate.EditValue.date, "yyyy-MM-dd"), Format(Me.deEndDate.EditValue.Date, "yyyy-MM-dd"), True)
  End If

  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.RaiseListChangedEvents = False
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.DataSource = _payments
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.RaiseListChangedEvents = True
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.ResetBindings(False)


  'If CommunityLookUpEdit.EditValue > 0 Then
  '    Dim field As PivotGridField = PivotGridControl1.Fields("CommunityName")
  '    PivotGridControl1.BeginUpdate()
  '    Try
  '        field.FilterValues.Clear()
  '        field.FilterValues.Add(CommunityLookUpEdit.Text)
  '        field.FilterValues.FilterType = DevExpress.Data.PivotGrid.PivotFilterType.Included
  '        PivotGridControl1.Refresh()
  '    Finally
  '        ' Unlock the control.
  '        PivotGridControl1.EndUpdate()
  '    End Try
  'End If
  'If OperatorLookUpEdit.EditValue > 0 Then
  '    Dim field As PivotGridField = PivotGridControl1.Fields("OperatorName")
  '    PivotGridControl1.BeginUpdate()
  '    Try
  '        field.FilterValues.Clear()
  '        field.FilterValues.Add(OperatorLookUpEdit.Text)
  '        field.FilterValues.FilterType = DevExpress.Data.PivotGrid.PivotFilterType.Included
  '        PivotGridControl1.Refresh()
  '    Finally
  '        ' Unlock the control.
  '        PivotGridControl1.EndUpdate()
  '    End Try
  'End If

 End Sub

 Private Sub RefreshDataRata()
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.RaiseListChangedEvents = False

  _payments = _
           PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList( _
            Format(Me.rataOd.EditValue.date, "yyyy-MM-dd"), Format(Me.rataDo.EditValue.Date, "yyyy-MM-dd"), False)

  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.DataSource = _payments
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.RaiseListChangedEvents = True
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.ResetBindings(False)
 End Sub

 Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
  RefreshData()
 End Sub

 Private Sub ceRange_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ceRange.CheckedChanged
  Me.deStartDate.Properties.ReadOnly = ceRange.Checked
  Me.deEndDate.Properties.ReadOnly = ceRange.Checked

 End Sub


 Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

  PrintableComponentLink1.CreateDocument()
  Dim phf As PageHeaderFooter = _
     TryCast(PrintableComponentLink1.PageHeaderFooter, PageHeaderFooter)

  If Not ceRange.Checked Then
   'tuka

   ' Clear the PageHeaderFooter's contents.
   phf.Header.Content.Clear()

   ' Add custom information to the link's header.
   phf.Header.Content.AddRange(New String() _
       {("Оператор: " & OperatorLookUpEdit.Text), ("Датум од: " & deStartDate.Text & "  до: " & deEndDate.Text), ("Општина: " & CommunityLookUpEdit.Text)})

   'phf.Header.Content.AddRange(New String() _
   '  {0, 1, 2})
   'phf.Header.Content(1) = ("Датум од: " & deStartDate.Text & "  до: " & deEndDate.Text)
   phf.Header.LineAlignment = BrickAlignment.Center
  Else
   phf.Header.Content.Clear()

   ' Add custom information to the link's header.

   phf.Header.Content.AddRange(New String() _
       {("Оператор: " & OperatorLookUpEdit.Text), "", ("Општина: " & CommunityLookUpEdit.Text)})


   'phf.Header.Content.AddRange(New String() _
   '  {0, 1, 2})
   'phf.Header.Content(1) = ("Датум од: " & deStartDate.Text & "  до: " & deEndDate.Text)
   phf.Header.LineAlignment = BrickAlignment.Center
  End If
  PrintableComponentLink1.ShowPreview()
 End Sub



 Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
  Me.Close()
 End Sub

 Private Sub btnPrintSmetka_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintSmetka.Click
  'Try
  '    Dim idDocumenr As Integer = InputBox("Внесете број на документ", "Печатење", 0)
  '    If IsNumeric(idDocumenr) AndAlso (idDocumenr > 0) Then
  '        Dim doc As PrintPaymentDocumetnByIdDocumetnList = _
  '       PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList(idDocumenr)
  '        If Not doc(0).Storno Then
  '            Dim getPaymentType As PaymentTypeInfo = _
  '                PaymentTypeList.GetPaymentTypeList.GetPaymentTypeInfoById _
  '                (doc(0).IdPaymentType)
  '            If getPaymentType.Faktura Then
  '                'Dim docum As PrintPaymentDocumetnByIdDocumetnList = _
  '                '         PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList(_paymentDocument.Id)
  '                Dim rpt As New rptFaktura(doc)
  '                Dim parForm As MainForm = Me.ParentForm
  '                parForm.AddWinPart(New uxPrint(rpt))


  '            End If
  '            Select Case objCurentTehExamOrganization.IdPaymentPrintOption
  '                Case PaymentPrintOption.Osnovna
  '                    Dim rpt As New rptPaymentDocumentByID(doc)
  '                    Dim parForm As MainForm = Me.ParentForm
  '                    parForm.AddWinPart(New uxPrint(rpt))
  '                Case PaymentPrintOption.Kompaktna
  '                    If objOpcii.DuplaSmetka Then
  '                        Dim rpt As New rptPaymentDocumentByIDCompactDouble(doc)
  '                        rpt.Landscape = True
  '                        Dim parForm As MainForm = Me.ParentForm
  '                        parForm.AddWinPart(New uxPrint(rpt))
  '                    Else
  '                        Dim rpt As New rptPaymentDocumentByIDCompact(doc)
  '                        Dim parForm As MainForm = Me.ParentForm
  '                        parForm.AddWinPart(New uxPrint(rpt))
  '                    End If
  '            End Select

  '        Else
  '            MsgBox("Документот е стониран")
  '        End If
  '    End If
  'Catch ex As Exception
  '    MsgBox(ex.Message)
  'End Try
  Try
   Dim idDocumenr As String = InputBox(My.Resources.VnrsiBRDoc, My.Resources.Pecatenje, 0) '("Внесете број на документ", "Печатење", 0)
   If (idDocumenr <> String.Empty) Then '

    Dim doc As PrintPaymentDocumetnByIdDocumetnList = _
   PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByDocumetnNumber(idDocumenr) '(idDocumenr)
    If doc.Count > 0 Then
     If Not doc(0).Storno Then
      'Dim uplatnici As New rptUplatnici(doc(0).DocumentID)
      Dim parForm As MainForm = Me.ParentForm
      'parForm.AddWinPart(New uxPrint(uplatnici))
      Dim getPaymentType As PaymentTypeInfo = _
          PaymentTypeList.GetPaymentTypeList.GetPaymentTypeInfoById _
          (doc(0).IdPaymentType)
      If getPaymentType.Faktura Then
       'Dim docum As PrintPaymentDocumetnByIdDocumetnList = _
       '         PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList(_paymentDocument.Id)
       Dim rpt As New rptFaktura(doc)
       ' Dim parForm As MainForm = Me.ParentForm
       parForm.AddWinPart(New uxPrint(rpt))


      End If
      Select Case objCurentTehExamOrganization.IdPaymentPrintOption
       Case PaymentPrintOption.Osnovna
        Dim rpt As New rptPaymentDocumentByID(doc)
        ' Dim parForm As MainForm = Me.ParentForm
        parForm.AddWinPart(New uxPrint(rpt))
       Case PaymentPrintOption.Kompaktna
        If objOpcii.DuplaSmetka Then
         Dim rpt As New rptPaymentDocumentByIDCompactDouble(doc)
         rpt.Landscape = True
         '  Dim parForm As MainForm = Me.ParentForm
         parForm.AddWinPart(New uxPrint(rpt))
        Else
         Dim rpt As New rptPaymentDocumentByIDCompact(doc)
         'Dim parForm As MainForm = Me.ParentForm
         parForm.AddWinPart(New uxPrint(rpt))
        End If
      End Select

     Else
      MsgBox(My.Resources.DocIsStorno) '"Документот е стониран")
     End If
    Else
     MsgBox(My.Resources.GresenBr)
    End If
   End If
  Catch ex As Exception
   MsgBox(ex.Message)
  End Try

 End Sub

 Private Sub btnStorno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStorno.Click
  Try
   'Dim dokBr As Integer = InputBox("Внесете број на сметка", "Сторнирање", 0)
   'If IsNumeric(dokBr) AndAlso (dokBr > 0) Then
   '    Dim dok As PaymentDocument = PaymentDocument.GetPaymentDocument(dokBr)
   Dim dokBr As String = InputBox(My.Resources.VnrsiBRDoc, My.Resources.Storno, 0)
   If (dokBr <> String.Empty) Then
    Dim dok As PaymentDocument = PaymentDocument.GetPaymentDocumentByDocNum(dokBr)
    If dok.Note.Contains("Сторнирана во сметка") Or dok.Storno Then 'dok.Storno Then
     MsgBox(My.Resources.DocIsStorno) '"Документот е претходно сторниран")
     Exit Sub
    Else
     'napravi nov dokument
     Dim stDok As PaymentDocument = PaymentDocument.NewPaymentDocument
     stDok.Storno = True
     stDok.DatePay = Now
     stDok.DateRequired = Now
     stDok.Payed = True
     stDok.IdCustomerVehicleRelation = dok.IdCustomerVehicleRelation
     stDok.IdPaymentType = dok.IdPaymentType
     stDok.Note = "Автоматски генерирана сторно сметка за бр." & dok.Id
     For Each detal As PaymentDocumentsDetail In dok.PaymentDocumentDetails
      If Not detal.PrePayed Then
       Dim newDetal As PaymentDocumentsDetail = stDok.PaymentDocumentDetails.AddNew
       newDetal.IdPriceCatalog = detal.IdPriceCatalog
       newDetal.Ddv = detal.Ddv
       'pazi dali moze da se stava popust
       newDetal.Discount = detal.Discount
       newDetal.Price = detal.Price
       newDetal.IdCustomerFinancialState = 0
       'Dim pomZadolzuvanje As CustomerFinanceDepInfo

      End If
      detal.VratiZadolzi()
     Next
     stDok = stDok.Save
     PecatiFiskalnaSmetaAccentPF500(stDok.Id)
     dok.Note = "Сторнирана во сметка бр." & stDok.Id
     dok.Save()
     RefreshData()
    End If
   End If
  Catch ex As Exception
   MsgBox(ex.Message)
  End Try

 End Sub



 Private Sub btnRata_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRata.Click
  Try
   Dim idDocumenr As String = InputBox(My.Resources.VnrsiBRDoc, My.Resources.RatiIzmeni, 0)
   'If IsNumeric(idDocumenr) AndAlso (idDocumenr > 0) Then
   '    Dim doc As PaymentDocument = PaymentDocument.GetPaymentDocument(idDocumenr)
   If (idDocumenr <> String.Empty) Then
    Dim doc As PaymentDocument = PaymentDocument.GetPaymentDocumentByDocNum(idDocumenr)
    _paymentTypeList = objPaymentTypeList 'PaymentTypeList.GetPaymentTypeList
    If Not doc.Storno Then
     If Not doc.Payed AndAlso _paymentTypeList.GetPaymentTypeInfoById(doc.IdPaymentType).Rati Then
      Dim par As MainForm = Me.ParentForm
      par.AddWinPart(New uxPaymentDocument(doc))
     Else
      MsgBox(My.Resources.PartialNot) '"За документот не се дозволени рати")
     End If
    Else
     MsgBox(My.Resources.DocIsStorno) '"Документот е стониран")
    End If
   End If
  Catch ex As Exception
   MsgBox(ex.Message)
  End Try

 End Sub

 Private Sub btnDogovor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDogovor.Click
  Try
   Dim idDocumenr As String = InputBox(My.Resources.VnrsiBRDoc, My.Resources.PrintDogRata, 0) '"Печати договор со рати", 0)
   If (idDocumenr <> String.Empty) Then
    Dim doc As PaymentDocument = PaymentDocument.GetPaymentDocumentByDocNum(idDocumenr)
    _paymentTypeList = PaymentTypeList.GetPaymentTypeList
    If Not doc.Storno Then
     If _paymentTypeList.GetPaymentTypeInfoById(doc.IdPaymentType).Rati Then
      Dim rptDogovor As New rptPaymentDocumentDogovor(doc.Id)
      Dim parForm As MainForm = Me.ParentForm
      parForm.AddWinPart(New uxPrint(rptDogovor))
     Else
      MsgBox("За документот не постои договор за плаќање на рати")
     End If
    Else
     MsgBox(My.Resources.DocIsStorno) '"Документот е стониран")
    End If
   End If
  Catch ex As Exception
   MsgBox(ex.Message)
  End Try
 End Sub

 Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
  'Dim idDocumenr As Integer = InputBox("Внесете број на документ", "Печати фискална", 0)
  'If IsNumeric(idDocumenr) AndAlso (idDocumenr > 0) Then
  '    Dim doc As PaymentDocument = PaymentDocument.GetPaymentDocument(idDocumenr)
  Dim dokBr As String = InputBox(My.Resources.VnrsiBRDoc, My.Resources.Fiscal, 0) '"Фискална сметка", 0)
  If (dokBr <> String.Empty) Then
   Dim dok As PaymentDocument = PaymentDocument.GetPaymentDocumentByDocNum(dokBr)
   _paymentTypeList = objPaymentTypeList 'PaymentTypeList.GetPaymentTypeList
   If Not dok.Storno Then
    PecatiFiskalnaSmetaAccentPF500(dok.Id)
   Else
    MsgBox(My.Resources.DocIsStorno) '"Документот е стониран")
   End If
  End If

 End Sub
 Private Sub btnStornoSamoSmetka_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStornoSamoSmetka.Click
  Try
   Dim dokBr As String = InputBox(My.Resources.VnrsiBRDoc, My.Resources.Storno, 0)
   If (dokBr <> String.Empty) Then
    Dim dok As PaymentDocument = PaymentDocument.GetPaymentDocumentByDocNum(dokBr)
    'Dim dokBr As Integer = InputBox("Внесете број на сметка", "Сторнирање", 0)
    'If IsNumeric(dokBr) AndAlso (dokBr > 0) Then
    '    Dim dok As PaymentDocument = PaymentDocument.GetPaymentDocument(dokBr)
    If dok.Note.Contains("Сторнирана во сметка") Or dok.Storno Then
     MsgBox(My.Resources.DocIsStorno) '"Документот е претходно сторниран")
     Exit Sub
    Else
     'napravi nov dokument
     Dim stDok As PaymentDocument = PaymentDocument.NewPaymentDocument
     stDok.Storno = True
     stDok.DatePay = Now
     stDok.DateRequired = Now
     stDok.Payed = True
     stDok.IdCustomerVehicleRelation = dok.IdCustomerVehicleRelation
     stDok.IdPaymentType = dok.IdPaymentType
     stDok.Note = "Автоматски генерирана сторно сметка за бр." & dok.Id
     For Each detal As PaymentDocumentsDetail In dok.PaymentDocumentDetails
      Dim newDetal As PaymentDocumentsDetail = stDok.PaymentDocumentDetails.AddNew
      newDetal.IdPriceCatalog = detal.IdPriceCatalog
      newDetal.Ddv = detal.Ddv
      'pazi dali moze da se stava popust
      newDetal.Discount = detal.Discount
      newDetal.Price = detal.Price

     Next
     stDok = stDok.Save
     dok.Note = "Сторнирана во сметка бр." & stDok.Id
     dok.Save()
     RefreshData()
    End If
   End If
  Catch ex As Exception
   MsgBox(ex.Message)
  End Try

 End Sub

 Private Sub btnCustomer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustomer.Click
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.RaiseListChangedEvents = False

  _payments = _
            PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdCustomer(LookUpEditCustomer.EditValue)

  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.DataSource = _payments
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.RaiseListChangedEvents = True
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.ResetBindings(False)

 End Sub

 Private Sub btnShowRati_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowRati.Click
  RefreshDataRata()
 End Sub

 Private Sub btnFullPlusRata_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFullPlusRata.Click
  _payments = PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnListPlusRati( _
  Format(Me.deStartDate.EditValue.date, "yyyy-MM-dd"), Format(Me.deEndDate.EditValue.Date, "yyyy-MM-dd"))

  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.RaiseListChangedEvents = False
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.DataSource = _payments
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.RaiseListChangedEvents = True
  Me.PrintPaymentDocumetnByIdDocumetnListBindingSource.ResetBindings(False)

 End Sub

 Private Sub LookUpEditCustomer_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEditCustomer.ButtonPressed
  Select Case e.Button.Index
   Case 1
    Dim par As MainForm = Me.ParentForm
    Dim pomTekst As String = LookUpEditCustomer.Text
    Using cekaj As New StatusBusy(My.Resources.txtLoading)
     Try
      If LookUpEditCustomer.Text <> " " AndAlso LookUpEditCustomer.Text <> String.Empty Then
       Try
        _customersList = CustomersSearchList.GetCustomersListShortByString(LookUpEditCustomer.Text)
       Catch ex As Exception
        _customersList = Nothing
       End Try

       Me.CustomersSearchListBindingSource.DataSource = _customersList

       LookUpEditCustomer.ClosePopup()
       LookUpEditCustomer.ShowPopup()
       'If _customersList.Count > 0 Then
       '    LookUpEditCustomer.EditValue = _customersList.Item(0).Id
       'Else
       '    LookUpEditCustomer.EditValue = Nothing
       'End If
      End If
     Catch ex As Exception
      MsgBox(ex.Message)
     End Try
    End Using
  End Select
 End Sub
End Class
