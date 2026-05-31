Imports System.Drawing
Public Class uxPaymentDocument


    Private WithEvents _customerVehiclesRelationsList As CustomerVehiclesRelationsSearchList
    Private WithEvents _paymentTypeList As PaymentTypeList
    Private WithEvents _ddvList As DDVList
    Private WithEvents _paymentDocument As PaymentDocument
    Private WithEvents _customerSearchList As CustomersSearchList = Nothing

    Private pomRelacijaCustomer As String = ""
    Private pomRelacijaVehicle As String = ""
    Public ReadOnly Property Document() As PaymentDocument
        Get
            Return _paymentDocument
        End Get
    End Property


    Public Sub New(ByVal InPaymentDocument As PaymentDocument)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        'If InPaymentDocument.IsNew Then
        '    RelationLookUpEdit.ed()
        'End If
        _paymentDocument = InPaymentDocument

        LoadList()

        BindUI()
        If _paymentDocument.IsNew Then
            btnCreateCalculation.Enabled = True
            'RelationLookUpEdit.Enabled = True
            DatePayDateEdit.Enabled = True
            DateRequiredDateEdit.Enabled = True
            PaymentTypeLookUpEdit.Enabled = True
            StornoCheckEdit.Enabled = True
            PayedCheckEdit.Enabled = True
            NoteTextEdit.Enabled = True
            DetaliGridControl.Enabled = True
            PaymentDocumentRatiGridControl.Enabled = True
            'LayoutControlGroup2.Enabled = True
            txtPolisa.Enabled = True
        Else
            btnCreateCalculation.Enabled = False
            btnNew.Enabled = False
            btnStorno.Enabled = False
            btnPrint.Enabled = False
            btnNew.Enabled = False
            'RelationLookUpEdit.Enabled = False
            DatePayDateEdit.Enabled = False
            DateRequiredDateEdit.Enabled = False
            PaymentTypeLookUpEdit.Enabled = False
            StornoCheckEdit.Enabled = False
            PayedCheckEdit.Enabled = False
            NoteTextEdit.Enabled = False
            DetaliGridControl.Enabled = False
            PaymentDocumentRatiGridControl.Enabled = True
            txtPolisa.Enabled = False
            ' LayoutControlGroup2.Enabled = False

            'For Each item As DevExpress.XtraLayout.LayoutControlItem In LayoutControl1.Controls
            '    If item.TypeName = "LayoutControlItem" Then
            '        If item.Name = "RatiLayoutControlItem" Then
            '            item.Control.Enabled = True
            '        Else
            '            item.Control.Enabled = False
            '        End If
            '    End If

            'Next
        End If

        ApplyAuthorizationRules()
        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Sub ApplyAuthorizationRules()
        If Not VTE.Library.PaymentDocument.CanGetObject Then
            Me.Close()
        End If
        btnStorno.Enabled = VTE.Library.PaymentDocument.CanEditObject

    End Sub

    Private Sub uxPaymentDocument_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.btnStorno.Enabled = _paymentDocument.IsSavable
        Me.btnNew.Enabled = _paymentDocument.IsValid
        Me.btnPrint.Enabled = _paymentDocument.IsValid

        If (_paymentDocument.IdCustomerVehicleRelation <> 0) AndAlso _paymentDocument.IsNew Then
            'load detalis
            Dim tmpRelacija As CustomerVehiclesRelation = _
          CustomerVehiclesRelation.GetCustomerVehiclesRelation(_paymentDocument.IdCustomerVehicleRelation)
            Dim tmpVehicleId As Long = tmpRelacija.IdVehicle
            Dim vInfo As Vehicle = Vehicle.GetVehicle(tmpVehicleId)
            Dim pCatalog As PaymentCataologList
            pCatalog = _
             CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
             PaymentCataologList).GetPaymentForDepts("TrigerdByTechnicalExam", vInfo)
            For Each pInfo As PaymentCataologInfo In pCatalog
                If pInfo.CategoryName.Contains("јавни патишта") AndAlso pInfo.Price > 0 Then
                    txt99CrvenKrst.EditValue = Math.Round(pInfo.Price * 99)
                End If
            Next

            _paymentDocument.CreateDetails(_paymentDocument.IdCustomerVehicleRelation)

        End If

        If _paymentDocument.IsNew Then
            RatiLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutBtnSave.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutPrintDogovor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            ' Me.RelationLookUpEdit.Focus()
        Else
            If _paymentTypeList.GetPaymentTypeInfoById(PaymentTypeLookUpEdit.EditValue).Rati Then
                LayoutBtnSave.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                LayoutPrintDogovor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                RatiLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            Else
                RatiLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutBtnSave.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutPrintDogovor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            End If
            ' Me.RelationLookUpEdit.Properties.ReadOnly = True
            Me.RelationVehicleLookUpEdit.Enabled = _paymentDocument.IsNew
            Me.RelationCustomerLookUpEdit.Enabled = _paymentDocument.IsNew

        End If
    End Sub

#Region " Bindings "

    Private Sub BindUI()
        _paymentDocument.BeginEdit()
        Me.PaymentDocumentBindingSource.DataSource = _paymentDocument
    End Sub

    Private Sub LoadList()
        Me.PaymentCataologListBindingSource.DataSource = objPaymentCatalogList
        If _paymentDocument.IdCustomerVehicleRelation > 0 Then
            _customerVehiclesRelationsList = CustomerVehiclesRelationsSearchList. _
            GetCustomerVehiclesRelationsListById(_paymentDocument.IdCustomerVehicleRelation)
            ' Me.RelationLookUpEdit.Properties.ReadOnly = True
            Me.RelationCustomerLookUpEdit.Enabled = False

            Me.DateRequiredDateEdit.Focus()
        Else
            _customerVehiclesRelationsList = Nothing
            ' Me.RelationLookUpEdit.Properties.ReadOnly = False
            Me.RelationCustomerLookUpEdit.Enabled = True
            Me.RelationCustomerLookUpEdit.Focus()
        End If

        Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehiclesRelationsList
        _paymentTypeList = PaymentTypeList.GetPaymentTypeList
        Me.PaymentTypeListBindingSource.DataSource = _paymentTypeList
        _ddvList = DDVList.GetDDVList
        Me.DDVListBindingSource.DataSource = _ddvList
        'Try
        '    Dim customerListaNova As CustomersSearchList = CustomersSearchList.GetCustomersListShortById(_customerVehiclesRelationsList.Item(0).IdCustomer)
        '    Me.CustomersSearchListBindingSource.DataSource = customerListaNova
        'Catch ex As Exception

        'End Try

    End Sub

    Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
        ' stop the flow of events
        Me.PaymentDocumentBindingSource.RaiseListChangedEvents = False
        Me.PaymentDocumentDetailsBindingSource.RaiseListChangedEvents = False
        Me.PaymentDocumentRatiBindingSource.RaiseListChangedEvents = False
        ' commit edits in memory
        UnbindBindingSource(Me.PaymentDocumentDetailsBindingSource, saveObject, False)
        UnbindBindingSource(Me.PaymentDocumentRatiBindingSource, saveObject, False)
        UnbindBindingSource(Me.PaymentDocumentBindingSource, saveObject, True)

        Me.PaymentDocumentDetailsBindingSource.DataSource = Me.PaymentDocumentBindingSource
        Try
            ' save or cancel changes
            If saveObject Then

                _paymentDocument.ApplyEdit()
                Try
                    If _paymentDocument.IsNew Then
                        _paymentDocument.IdOrganization = CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id
                    End If
                    _paymentDocument = _paymentDocument.Save

                    'pecati fiskalna
                    PecatiFiskalnaSmetaAccentPF500(_paymentDocument.Id)
                    If Not _paymentDocument.Storno Then
                        Dim getPaymentType As PaymentTypeInfo = _
                        PaymentTypeList.GetPaymentTypeList.GetPaymentTypeInfoById _
                        (_paymentDocument.IdPaymentType)
                        If getPaymentType.FiskalnaKes Then
                            _paymentDocument.Payed = True
                            _paymentDocument.Save()
                        End If
                        If Not getPaymentType.Rati Then
                            _paymentDocument.PaymentDocumentRati(0).Payed = _paymentDocument.Payed
                            _paymentDocument.Save()
                        Else
                            Dim sumRati As Decimal = 0
                            For Each rata As PaymentDocumentsRata In _paymentDocument.PaymentDocumentRati
                                If rata.Payed Then
                                    sumRati += rata.Price
                                End If
                            Next
                            Dim sumDetali As Decimal = 0
                            For Each detal As PaymentDocumentsDetail In _paymentDocument.PaymentDocumentDetails
                                If detal.PrePayed = False Then
                                    sumDetali += detal.Price * (1 - detal.Discount / 100)
                                End If
                            Next
                            _paymentDocument.Payed = (Math.Round(sumRati) >= Math.Round(sumDetali))
                            '
                            _paymentDocument.Save()
                        End If
                        If getPaymentType.Faktura Then
                            Dim doc As PrintPaymentDocumetnByIdDocumetnList = _
                                     PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList(_paymentDocument.Id)
                            Dim rpt As New rptFaktura(doc)
                            Dim parForm As MainForm = Me.ParentForm
                            parForm.AddWinPart(New uxPrint(rpt))
                        End If
                        If getPaymentType.Smetka Then
                            Select Case objCurentTehExamOrganization.IdPaymentPrintOption
                                Case PaymentPrintOption.Osnovna
                                    Dim doc As PrintPaymentDocumetnByIdDocumetnList = _
                                      PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList(_paymentDocument.Id)
                                    Dim rpt As New rptPaymentDocumentByID(doc)
                                    Dim parForm As MainForm = Me.ParentForm
                                    parForm.AddWinPart(New uxPrint(rpt))
                                Case PaymentPrintOption.Kompaktna
                                    If objOpcii.DuplaSmetka Then
                                        Dim doc As PrintPaymentDocumetnByIdDocumetnList = _
                                      PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList(_paymentDocument.Id)
                                        Dim rpt As New rptPaymentDocumentByIDCompactDouble(doc)
                                        rpt.Landscape = True
                                        Dim parForm As MainForm = Me.ParentForm
                                        parForm.AddWinPart(New uxPrint(rpt))
                                    Else
                                        Dim doc As PrintPaymentDocumetnByIdDocumetnList = _
                                      PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList(_paymentDocument.Id)
                                        Dim rpt As New rptPaymentDocumentByIDCompact(doc)
                                        Dim parForm As MainForm = Me.ParentForm
                                        parForm.AddWinPart(New uxPrint(rpt))
                                    End If

                            End Select
                        End If

                    End If

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
                _paymentDocument.CancelEdit()
            End If
        Finally
            'rebind UI if requested
            If rebind Then
                BindUI()
            End If

            ' restore events
            Me.PaymentDocumentBindingSource.RaiseListChangedEvents = True
            Me.PaymentDocumentDetailsBindingSource.RaiseListChangedEvents = True
            Me.PaymentDocumentRatiBindingSource.RaiseListChangedEvents = True
            If rebind Then
                ' refresh the UI if rebinding
                Me.PaymentDocumentBindingSource.ResetBindings(False)
                Me.PaymentDocumentDetailsBindingSource.ResetBindings(False)
                Me.PaymentDocumentRatiBindingSource.ResetBindings(False)
                Me.tPage.Text = Me.ToString
            End If
        End Try

    End Sub

    Private Sub BindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        PaymentDocumentBindingSource.CurrentItemChanged, _
        PaymentDocumentDetailsBindingSource.CurrentItemChanged, _
        PaymentDocumentRatiBindingSource.CurrentItemChanged
        If _paymentDocument.IsNew Then
            Me.btnStorno.Enabled = _paymentDocument.IsSavable
            Me.btnNew.Enabled = _paymentDocument.IsValid
            Me.btnPrint.Enabled = _paymentDocument.IsValid
        End If
        Dim message As New System.Text.StringBuilder

        message.AppendFormat("{0}" + vbCrLf, "")
        For Each rule As Csla.Validation.BrokenRule In _paymentDocument.BrokenRulesCollection
            message.AppendFormat( _
              "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
        Next
        'detali
        For Each child As PaymentDocumentsDetail In _paymentDocument.PaymentDocumentDetails
            For Each rule As Csla.Validation.BrokenRule In child.BrokenRulesCollection
                message.AppendFormat( _
                  "* {0}: {1}" + vbCrLf, rule.Property, rule.Description)
            Next
        Next

        ShowBrokenRules(message.ToString, True)

    End Sub

    Private Sub DetaliBindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _
        PaymentDocumentDetailsBindingSource.ListChanged
        If (Me.PaymentDocumentDetailsBindingSource IsNot Nothing) AndAlso (e.ListChangedType = System.ComponentModel.ListChangedType.ItemDeleted) Then
            Me.btnStorno.Enabled = _paymentDocument.IsValid
        End If

    End Sub

    Private Sub _paymentDocument_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _paymentDocument.PropertyChanged
        If (e.PropertyName = "IdCustomerVehicleRelation") AndAlso (_paymentDocument.IdCustomerVehicleRelation <> 0) Then
            'load detalis
            If Not _paymentDocument.IsNew Then
                _paymentDocument.CreateDetails(_paymentDocument.IdCustomerVehicleRelation)
            End If
        End If
        If e.PropertyName = "IdPaymentType" AndAlso (_paymentDocument.IdPaymentType > 0) Then
            Dim paymenType As PaymentTypeInfo = _paymentTypeList.GetPaymentTypeInfoById( _
            _paymentDocument.IdPaymentType)
            _paymentDocument.Payed = paymenType.FiskalnaKes Or paymenType.Faktura
        End If
    End Sub

#End Region

#Region "KeyPress"
    Private Sub uxPaymentDocument_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
        Select Case Asc(e.KeyChar)
            Case 13
                SendKeys.Send("{TAB}")
        End Select
    End Sub
#End Region

#Region "WinPart"

    Protected Overrides Function GetIdValue() As Object
        If _paymentDocument IsNot Nothing Then
            Return My.Resources.uxDocumentPayment & _paymentDocument.Id
        Else
            Return My.Resources.uxDocumentPayment & 0
        End If
    End Function

    Public Overrides Function ToString() As String
        If _paymentDocument IsNot Nothing Then
            Return My.Resources.uxDocumentPayment & _paymentDocument.Id
        Else
            Return My.Resources.uxDocumentPayment & 0
        End If
    End Function

#End Region

#Region "Buttons"

    Private Sub btnCreateCalculation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateCalculation.Click

        Try
            Using busy As New Splash(My.Resources.LoadingData)
                Dim cust As String = Me.RelationCustomerLookUpEdit.Text
                Dim vehi As String = Me.RelationCustomerLookUpEdit.Text
                Dim par As MainForm = Me.ParentForm
                Dim calc As CalculationList = CalculationList.GetCalculationList(_paymentDocument)
                par.AddWinPart(New uxCaclulationPivot(calc, cust, vehi))
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        If _paymentDocument.IsDirty Then
            Select Case MsgBox(My.Resources.izmeniZacuvaj, MsgBoxStyle.YesNoCancel, "излез")
                Case MsgBoxResult.Yes
                    If PaymentDocument.CanEditObject Then
                        RebindUI(True, False)
                        'tuka pecati
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
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStorno.Click
        Try
            _paymentDocument.Storno = True
            RebindUI(True, True)
        Catch ex As Exception
            Exit Sub
        End Try
        _paymentDocument = Nothing
        _paymentDocument = PaymentDocument.NewPaymentDocument
        BindUI()

        Me.RelationCustomerLookUpEdit.Focus()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'pecati
        If RatiLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always Then
            If _paymentDocument.PaymentDocumentRati.Count < 1 Then
                MsgBox("Мора да се внесе барем една рата")
                _paymentDocument.PaymentDocumentRati.AddNew()
                Exit Sub
            End If
        End If
        Try
            RebindUI(True, True)
            'tuka otvori pecati

        Catch ex As Exception
            Exit Sub
        End Try
        Me.Close()
        ' btnExit.Focus()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        RebindUI(False, True)
        Me.RelationCustomerLookUpEdit.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            RebindUI(True, True)
            'tuka otvori pecati

        Catch ex As Exception
            Exit Sub
        End Try

        _paymentDocument = Nothing
        _paymentDocument = PaymentDocument.NewPaymentDocument
        BindUI()

        Me.RelationCustomerLookUpEdit.Focus()
    End Sub

#End Region

#Region " Grid events "

    'Private Sub GridView1_CustomColumnSort(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs) Handles GridView1.CustomColumnSort
    '  If e.Column Is colVisibleOrder Then
    '    If e.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending Then
    '      e.Handled = True
    '      Dim val1 As Integer = GridView1.GetRowCellDisplayText(e.RowHandle1, colVisibleOrder)
    '      Dim val2 As Integer = GridView1.GetRowCellDisplayText(e.RowHandle2, colVisibleOrder)
    '      'e.Result = Comparer.Default.Compare(e.Value1, e.Value2)
    '      If CInt(val1) < CInt(val2) Then
    '        e.Result = 1
    '      ElseIf val1 = val2 Then
    '        e.Result = Comparer.Default.Compare(val1, val1)

    '      Else
    '        e.Result = -1
    '      End If
    '    End If
    '  End If
    'End Sub

    Private Sub DetaliGridControl_ProcessGridKey(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _
        Handles DetaliGridControl.ProcessGridKey
        Select Case e.KeyCode
            Case Keys.Add, Keys.Oemplus
                GridView1.AddNewRow()
                'If GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle Then
                ' GridView1.PostEditor()
                ' GridView1.UpdateCurrentRow()
                'End If
                GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle
                Me.GridView1.FocusedColumn = colIdPriceCatalog
                BindingSource_CurrentItemChanged(Me, New System.EventArgs)
                e.SuppressKeyPress = True
                ''za radovis ova ne treba
            Case Keys.OemMinus, Keys.Subtract
                Select Case CType(Csla.ApplicationContext.LocalContext.Item("objCurentUser"), UsersInfo).IdDataBase
                    Case 9
                    Case Else
                        If Me.PaymentDocumentDetailsBindingSource.Current IsNot Nothing Then
                            Me.PaymentDocumentDetailsBindingSource.RemoveCurrent()
                            e.SuppressKeyPress = True
                        End If
                End Select

        End Select
    End Sub



    Private Sub GridView1_InitNewRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) _
        Handles GridView1.InitNewRow
        'Console.WriteLine(e.RowHandle)
        Dim det As PaymentDocumentsDetail = _paymentDocument.PaymentDocumentDetails.Item(Me.PaymentDocumentDetailsBindingSource.Position)

        For Each column As DevExpress.XtraGrid.Columns.GridColumn In GridView1.Columns
            column.OptionsColumn.ReadOnly = Not det.CanWriteProperty(column.FieldName)
        Next
        GridView1.UpdateCurrentRow()
    End Sub

    Private Sub GridView1_RowCountChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.RowCountChanged
        GridView1.UpdateCurrentRow()
    End Sub

    'Private Sub GridView1_CellValueChanged(ByVal sender As Object, _
    'ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) _
    ' Handles GridView1.CellValueChanged
    '    Select Case e.Column.FieldName

    '        Case "IdPriceCatalog"
    '            'Select Case e.Column.Name

    '            '    Case "colIdPriceCatalog"
    '            'stavi(ddv)

    '            On Error Resume Next


    '            Dim idPrice As Integer = GridView1.GetRowCellValue(GridView1.FocusedRowHandle, colIdPriceCatalog)
    '            Dim priceInfo As PaymentCataologInfo = objPaymentCatalogList.GetInfo(idPrice)


    '            Dim intIdDDV As Integer = priceInfo.IdDDV
    '            Dim ddvVrednos As Single = _ddvList.GetInfo(intIdDDV).DDVValue
    '            Dim DefaultCean As Decimal = priceInfo.Price

    '            GridView1.SetFocusedRowCellValue(colDdv, ddvVrednos)
    '            GridView1.SetFocusedRowCellValue(colPrice, DefaultCean)
    '    End Select
    'End Sub
    'Private Sub CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
    '               PaymentDocumentDetailsBindingSource.CurrentItemChanged
    '    'PaymentDocumentBindingSource.CurrentItemChanged, _

    '    If GridView1.FocusedRowHandle = -2147483647 Or GridView1.FocusedRowHandle >= 0 Then
    '        Select Case GridView1.FocusedColumn.FieldName

    '            Case "IdPriceCatalog"
    '                'Select Case e.Column.Name

    '                '    Case "colIdPriceCatalog"
    '                'stavi(ddv)

    '                On Error Resume Next


    '                Dim idPrice As Integer = GridView1.GetRowCellValue(GridView1.FocusedRowHandle, colIdPriceCatalog)
    '                Dim priceInfo As PaymentCataologInfo = objPaymentCatalogList.GetInfo(idPrice)


    '                Dim intIdDDV As Integer = priceInfo.IdDDV
    '                Dim ddvVrednos As Single = _ddvList.GetInfo(intIdDDV).DDVValue

    '                Dim DefaultCean As Decimal = priceInfo.Price

    '                GridView1.SetFocusedRowCellValue(colDdv, ddvVrednos)
    '                GridView1.SetFocusedRowCellValue(colPrice, DefaultCean)
    '        End Select
    '    End If
    'End Sub
    Private Sub promenaDetal() Handles _
    GridView1.ValidateRow ', PaymentDocumentDetailsBindingSource.CurrentChanged    'GridView1.CellValueChanged ', _paymentDocument.ChildChanged ', GridView1.ValidateRow
        If GridView1.FocusedRowHandle = -2147483647 Then

            Select Case GridView1.FocusedColumn.FieldName

                Case "IdPriceCatalog"


                    On Error Resume Next


                    Dim idPrice As Integer = GridView1.GetRowCellValue(GridView1.FocusedRowHandle, colIdPriceCatalog)
                    'Dim idPrice As Integer = GridView1.GetRowCellValue(GridView1.GetSelectedRows(0), colIdPriceCatalog)

                    Dim priceInfo As PaymentCataologInfo = objPaymentCatalogList.GetInfo(idPrice)
                    Dim ecoPercent As Decimal = 100

                    If priceInfo IsNot Nothing AndAlso priceInfo.PaymentName.Contains("животна средина") Then
                        Dim relation As CustomerVehiclesRelationsSearchInfo = _
                   CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(_paymentDocument.IdCustomerVehicleRelation).Item(0)
                        Dim vehicle As Vehicle = vehicle.GetVehicle(relation.IdVehicle)
                        ecoPercent = CType(Csla.ApplicationContext.LocalContext("objVehicleEngineEcoProgramList"),  _
                        VehicleEngineEcoProgramList).GetInfoById(vehicle.IdEngineEcoProgram).PercentForPayment
                    Else
                        If ((priceInfo.PaymentName.Contains("Технички преглед")) Or _
                            (priceInfo.IdPaymentItem = objPaymentCatalogList.GetCrventKrstInfo.IdPaymentItem) Or _
                            (priceInfo.IdPaymentItem = objPaymentCatalogList.GetRSBSPInfo.IdPaymentItem)) AndAlso _
                            GridView1.GetFocusedRowCellValue(colPrice) > 0 Then
                            If objPaymentCatalogList.GetRSBSPInfo IsNot Nothing Then
                                If priceInfo.IdPaymentItem = objPaymentCatalogList.GetRSBSPInfo.IdPaymentItem Then
                                    GridView1.UpdateCurrentRow()
                                    Exit Select
                                    ' Exit Sub
                                End If
                            Else
                                GridView1.UpdateCurrentRow()
                                Exit Select
                                '  Exit Sub
                            End If
                        End If
                    End If
                    If GridView1.RowCount = 1 Then
                        GridView1.UpdateCurrentRow()

                    End If
                    Dim intIdDDV As Integer = priceInfo.IdDDV
                    Dim ddvVrednos As Single = _ddvList.GetInfo(intIdDDV).DDVValue
                    Dim DefaultCean As Decimal = (priceInfo.Price * ecoPercent / 100)
                    'Me.GridView1.FocusedColumn = colPrice
                    GridView1.SetFocusedRowCellValue(colDdv, ddvVrednos)
                    GridView1.SetFocusedRowCellValue(colPrice, DefaultCean)

            End Select

        End If

    End Sub
    Private Sub promenaDetal2() Handles _
   PaymentDocumentDetailsBindingSource.CurrentChanged    'GridView1.CellValueChanged ', _paymentDocument.ChildChanged ', GridView1.ValidateRow
        If GridView1.FocusedRowHandle >= 0 Then

            Select Case GridView1.FocusedColumn.FieldName

                Case "IdPriceCatalog"
                    'Select Case e.Column.Name

                    '    Case "colIdPriceCatalog"
                    'stavi(ddv)

                    On Error Resume Next


                    Dim idPrice As Integer = GridView1.GetRowCellValue(GridView1.FocusedRowHandle, colIdPriceCatalog)
                    'Dim idPrice As Integer = GridView1.GetRowCellValue(GridView1.GetSelectedRows(0), colIdPriceCatalog)

                    Dim priceInfo As PaymentCataologInfo = objPaymentCatalogList.GetInfo(idPrice)
                    Dim ecoPercent As Decimal = 100

                    If priceInfo IsNot Nothing AndAlso priceInfo.PaymentName.Contains("животна средина") Then
                        Dim relation As CustomerVehiclesRelationsSearchInfo = _
                        CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(_paymentDocument.IdCustomerVehicleRelation).Item(0)
                        Dim vehicle As Vehicle = vehicle.GetVehicle(relation.IdVehicle)
                        ecoPercent = CType(Csla.ApplicationContext.LocalContext("objVehicleEngineEcoProgramList"),  _
                        VehicleEngineEcoProgramList).GetInfoById(vehicle.IdEngineEcoProgram).PercentForPayment
                    Else
                        If ((priceInfo.PaymentName.Contains("Технички преглед")) Or _
                            (priceInfo.IdPaymentItem = objPaymentCatalogList.GetCrventKrstInfo.IdPaymentItem) Or _
                            (priceInfo.IdPaymentItem = objPaymentCatalogList.GetRSBSPInfo.IdPaymentItem)) _
                            AndAlso GridView1.GetFocusedRowCellValue(colPrice) > 0 Then
                            If objPaymentCatalogList.GetRSBSPInfo IsNot Nothing Then
                                If priceInfo.IdPaymentItem = objPaymentCatalogList.GetRSBSPInfo.IdPaymentItem Then
                                    GridView1.UpdateCurrentRow()
                                    Exit Select
                                End If
                            Else
                                GridView1.UpdateCurrentRow()
                                ' Exit Select
                            End If
                        End If
                    End If
                    If GridView1.RowCount = 1 Then
                        GridView1.UpdateCurrentRow()
                        'For Each det As PaymentDocumentsDetail In _paymentDocument.PaymentDocumentDetails
                        '    
                        'Next
                    End If
                    Dim intIdDDV As Integer = priceInfo.IdDDV
                    Dim ddvVrednos As Single = _ddvList.GetInfo(intIdDDV).DDVValue
                    Dim DefaultCean As Decimal = (priceInfo.Price * ecoPercent / 100)
                    'Me.GridView1.FocusedColumn = colPrice
                    GridView1.SetFocusedRowCellValue(colDdv, ddvVrednos)
                    GridView1.SetFocusedRowCellValue(colPrice, DefaultCean)
                Case "Price"
                    GridView1.MoveNext()
            End Select

        End If

    End Sub
#End Region

#Region " Focus "
    Private Sub DetaliGridControl_Enter(ByVal sender As Object, ByVal e As System.EventArgs) _
     Handles DetaliGridControl.Enter

        DetaliGridControl.FocusedView = GridView1
        If GridView1.RowCount = 0 Then

            GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle

        End If
        Me.GridView1.FocusedColumn = colIdPriceCatalog
    End Sub

    Private Sub DetaliGridControl_Leave(ByVal sender As Object, ByVal e As System.EventArgs) _
     Handles DetaliGridControl.Leave
        GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle
    End Sub
#End Region

    Private Sub SpinEdit3_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SpinEdit3.EditValueChanged
        Me.SpinEdit1.Value = Me.SpinEdit3.Value - _paymentDocument.Vkupno

    End Sub


    Private Sub RelationLookUpEdit_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles RelationCustomerLookUpEdit.ButtonPressed, RelationVehicleLookUpEdit.ButtonPressed
        Select Case e.Button.Index
            Case 1
                If DockManager1.Panels.Count <= 0 Then

                    Dim panel As DevExpress.XtraBars.Docking.DockPanel = Me.DockManager1.AddPanel(DevExpress.XtraBars.Docking.DockingStyle.Bottom)
                    panel.Name = "addRelationPanel"
                    panel.Text = "Додавње на нова релација"
                    Dim ux As New uxAddNewRelation()
                    AddHandler ux.Disposed, AddressOf ux_dispose
                    panel.Size = New Size(600, 150)
                    panel.Controls.Add(ux)
                    ux.Dock = DockStyle.Fill
                    ux.BringToFront()
                    panel.Top = True
                    panel.Options.AllowDockBottom = True
                    panel.Options.AllowDockFill = False
                    panel.Options.AllowDockLeft = False
                    panel.Options.AllowDockRight = False
                    panel.Options.ShowCloseButton = False
                    panel.Options.ShowAutoHideButton = False
                    panel.BringToFront()

                    panel.Show()

                Else
                    DockManager1.Panels.Item(DockManager1.Panels.Count - 1).Focus()
                End If
        End Select
    End Sub
    Private Sub ux_dispose(ByVal sender As Object, ByVal e As System.EventArgs)
        If CType(sender, uxAddNewRelation).SelectedRelationId <> 0 Then
            Me.CustomerVehiclesRelationsSearchListBindingSource.RaiseListChangedEvents = False
            _customerVehiclesRelationsList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(CType(sender, uxAddNewRelation).SelectedRelationId)
            Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehiclesRelationsList
            Me.CustomerVehiclesRelationsSearchListBindingSource.RaiseListChangedEvents = True
            Me.CustomerVehiclesRelationsSearchListBindingSource.ResetBindings(False)
            _paymentDocument.IdCustomerVehicleRelation = CType(sender, uxAddNewRelation).SelectedRelationId
            RelationCustomerLookUpEdit.EditValue = CType(sender, uxAddNewRelation).SelectedRelationId
        End If
        Me.DockManager1.RemovePanel(Me.DockManager1.Panels("addRelationPanel"))
    End Sub


    'Private Sub PrePayedCheckEdit_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PrePayedCheckEdit.CheckStateChanged

    '    NotePrePayedTextEdit.Focus()
    '    PaymentDocumentDetailsBindingSource.Position = pomSelectiran
    'End Sub

    'Private pomSelectiran As Integer = 0
    'Private Sub GridView1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.LostFocus
    '    Try
    '        pomSelectiran = GridView1.GetSelectedRows(0)
    '    Catch ex As Exception

    '    End Try

    'End Sub


#Region "Rati"

    Private Sub RepositoryItemButtonEdit1_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles RepositoryItemButtonEdit1.ButtonPressed
        Dim selectiranaRata As PaymentDocumentsRata = _
        (_paymentDocument.PaymentDocumentRati.Item(PaymentDocumentRatiBindingSource.Position)) '.Id)'_paymentDocument.PaymentDocumentRati.GetItem _

        Select Case objCurentTehExamOrganization.IdPaymentPrintOption
            Case PaymentPrintOption.Osnovna
                'Dim doc As PrintPaymentDocumetnByIdDocumetnList = _
                '  PrintPaymentDocumetnByIdDocumetnList.GetPrintPaymentDocumetnByIdDocumetnList(_paymentDocument.Id)
                'Dim rpt As New rptPaymentDocumentByID(doc)
                'Dim parForm As MainForm = Me.ParentForm
                'parForm.AddWinPart(New uxPrint(rpt))
            Case PaymentPrintOption.Kompaktna
                If objOpcii.DuplaSmetka Then
                    Dim rpt As New rptPaymentDocumentRataByIDCompactDouble(selectiranaRata.Id)
                    rpt.Landscape = True
                    Dim parForm As MainForm = Me.ParentForm
                    parForm.AddWinPart(New uxPrint(rpt))
                Else
                    Dim rpt As New rptPaymentDocumentRataByIDCompact(selectiranaRata.Id)
                    Dim parForm As MainForm = Me.ParentForm
                    parForm.AddWinPart(New uxPrint(rpt))
                End If

        End Select

    End Sub


    Private Sub RepositoryItemButtonFiskal_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles RepositoryItemButtonFiskal.ButtonPressed
        Dim selectiranaRata As PaymentDocumentsRata = _
       (_paymentDocument.PaymentDocumentRati.Item(PaymentDocumentRatiBindingSource.Position))
        If Not selectiranaRata.Payed Then
            selectiranaRata.Payed = True
        End If
        PecatiFiskalnaSmetaZaRataAccentPF500(selectiranaRata.Id)
        RebindUI(True, True)
    End Sub

    Private Sub RepositoryItemButtonEditStorno_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles RepositoryItemButtonEditStorno.ButtonPressed
        ' Dim dok As PaymentDocument = PaymentDocument.GetPaymentDocument(dokBr)
        If _paymentDocument.Note.Contains("Сторнирана во сметка") Or _paymentDocument.Storno Then
            MsgBox("Документот е претходно сторниран")
            Exit Sub
        Else
            Dim selectiranaRata As PaymentDocumentsRata = _
         (_paymentDocument.PaymentDocumentRati.Item(PaymentDocumentRatiBindingSource.Position))

            PecatiFiskalnaStornoZaRataAccentPF500(selectiranaRata.Id)

            'GridView2.DeleteRow(PaymentDocumentRatiBindingSource.Position)
            RebindUI(True, True)
        End If
    End Sub

    Private Sub btnSaveRati_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveRati.Click
        RebindUI(True, True)
    End Sub
    Private Sub btnDogovorPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDogovorPrint.Click
        'Dim dogovor As PaymentRatiDogovor
        'dogovor = PaymentRatiDogovor.GetPaymentRatiDogovor(_paymentDocument.IdDogovor)
        Dim rptDogovor As New rptPaymentDocumentDogovor(_paymentDocument.Id)
        Dim parForm As MainForm = Me.ParentForm
        parForm.AddWinPart(New uxPrint(rptDogovor))
    End Sub
    Private Sub btnZbirnaSmetka_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZbirnaSmetka.Click
        Try

            If Not _paymentDocument.Storno Then
                Dim doc As PrintPaymentDocumetnByIdDocumetnList = PrintPaymentDocumetnByIdDocumetnList. _
                GetPrintPaymentDocumetnByIdDocumetnList(_paymentDocument.Id)
                Select Case objCurentTehExamOrganization.IdPaymentPrintOption
                    Case PaymentPrintOption.Osnovna
                        Dim rpt As New rptPaymentDocumentByID(doc)
                        Dim parForm As MainForm = Me.ParentForm
                        parForm.AddWinPart(New uxPrint(rpt))
                    Case PaymentPrintOption.Kompaktna
                        If objOpcii.DuplaSmetka Then
                            Dim rpt As New rptPaymentDocumentByIDCompactDouble(doc)
                            rpt.Landscape = True
                            Dim parForm As MainForm = Me.ParentForm
                            parForm.AddWinPart(New uxPrint(rpt))
                        Else
                            Dim rpt As New rptPaymentDocumentByIDCompact(doc)
                            Dim parForm As MainForm = Me.ParentForm
                            parForm.AddWinPart(New uxPrint(rpt))
                        End If

                End Select

            Else
                MsgBox("Документот е стониран")
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub PaymentTypeLookUpEdit_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles PaymentTypeLookUpEdit.Validated
        Dim tip As PaymentTypeInfo = _paymentTypeList.GetPaymentTypeInfoById(PaymentTypeLookUpEdit.EditValue)
        Try

            If tip.Rati Then
                RatiLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

                'btnSaveRati.Visible = True
                'btnDogovorPrint.Visible = True
                LayoutBtnSave.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                LayoutPrintDogovor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

                Dim customer As CustomersInfo = CustomersList.GetCustomersList.GetCustomersListById _
                (_customerVehiclesRelationsList.Item(0).IdCustomer)
                If PaymentTypeLookUpEdit.EditValue > 0 Then
                    Dim dij As New dijGarant(customer.Name, customer.AddressOfLiving, customer.MB, _paymentDocument.PaymentDocumentRati.Count)
                    dij.ShowDialog(Me.ParentForm)
                    If dij.DialogResult = DialogResult.OK Then
                        _paymentDocument.IdDogovor = dij.Dogovor.Id
                        _paymentDocument.PaymentDocumentRati(0).Payed = True

                        _paymentDocument.PaymentDocumentRati(0).Price = dij.PrvaRata
                        _paymentDocument.PaymentDocumentRati(0).Payed = True
                        _paymentDocument.PaymentDocumentRati(0).DatePayed = Today.Date
                        If _paymentDocument.PaymentDocumentRati.Count > 1 Then
                            For i As Integer = 1 To _paymentDocument.PaymentDocumentRati.Count - 1
                                _paymentDocument.PaymentDocumentRati.Remove(_paymentDocument.PaymentDocumentRati(1))
                            Next
                        End If
                        For i As Integer = 1 To dij.Dogovor.BrNaRati - 1
                            _paymentDocument.PaymentDocumentRati.AddNew()
                            _paymentDocument.PaymentDocumentRati(i).Price = (_paymentDocument.Vkupno - dij.PrvaRata) / (dij.Dogovor.BrNaRati - 1)
                            _paymentDocument.PaymentDocumentRati(i).DatePayed = Today.Date.AddMonths(i)

                        Next
                    Else
                        MsgBox("Морате да внесете податоци за склучен договор")
                        Dim dij2 As New dijGarant()
                    End If
                End If
            Else
                RatiLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutBtnSave.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutPrintDogovor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never

                'btnSaveRati.Visible = False
                'btnDogovorPrint.Visible = False
            End If
            If _paymentTypeList.GetPaymentTypeInfoById(PaymentTypeLookUpEdit.EditValue).Faktura Then
                _paymentDocument.DateRequired = _paymentDocument.DatePay.AddDays(objCurentTehExamOrganization.Valuta)
            End If
        Catch ex As Exception
            If PaymentTypeLookUpEdit.EditValue > 0 Then
                Dim dij1 As New dijGarant()
                dij1.ShowDialog(Me.ParentForm)
                If dij1.DialogResult = DialogResult.OK Then
                    _paymentDocument.IdDogovor = dij1.Dogovor.Id
                    _paymentDocument.PaymentDocumentRati(0).Payed = True

                    _paymentDocument.PaymentDocumentRati(0).Price = dij1.PrvaRata
                    _paymentDocument.PaymentDocumentRati(0).Payed = True
                    _paymentDocument.PaymentDocumentRati(0).DatePayed = Today.Date
                    If _paymentDocument.PaymentDocumentRati.Count > 1 Then
                        For i As Integer = 1 To _paymentDocument.PaymentDocumentRati.Count - 1
                            _paymentDocument.PaymentDocumentRati.Remove(_paymentDocument.PaymentDocumentRati(1))
                        Next
                    End If
                    For i As Integer = 1 To dij1.Dogovor.BrNaRati - 1
                        _paymentDocument.PaymentDocumentRati.AddNew()
                        _paymentDocument.PaymentDocumentRati(i).Price = (_paymentDocument.Vkupno - dij1.PrvaRata) / (dij1.Dogovor.BrNaRati - 1)
                        _paymentDocument.PaymentDocumentRati(i).DatePayed = Today.Date.AddMonths(i)

                    Next

                End If
            End If
        End Try
        If PaymentTypeLookUpEdit.EditValue > 0 Then
            If Not tip.Rati AndAlso tip.FiskalnaKes Then 'PaymentTypeLookUpEdit.Text.Contains("во готово") Then
                PayedCheckEdit.Checked = True
            Else
                If tip.Faktura Then
                    _paymentDocument.DateRequired = _paymentDocument.DatePay.AddDays(objCurentTehExamOrganization.Valuta)
                    ''za Skopje
                    '------------------------
                    _paymentDocument.Payed = False
                    '------------------------
                End If
            End If
        End If
        If tip IsNot Nothing AndAlso tip.Faktura Then
            FakturiraNaLookUp.Enabled = True
        Else
            FakturiraNaLookUp.Enabled = False
        End If
    End Sub

#End Region



    Private Sub RelationCustomerLookUpEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles RelationCustomerLookUpEdit.GotFocus
        System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo(uSettings.Culture))
    End Sub




    'Private Sub PaymentTypeLookUpEdit_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PaymentTypeLookUpEdit.EditValueChanged
    '    If Not _paymentTypeList.GetPaymentTypeInfoById(PaymentTypeLookUpEdit.EditValue).Rati AndAlso _paymentTypeList.GetPaymentTypeInfoById(PaymentTypeLookUpEdit.EditValue).FiskalnaKes Then 'PaymentTypeLookUpEdit.Text.Contains("во готово") Then
    '        PayedCheckEdit.Checked = True
    '    Else

    '    End If

    'End Sub


    Private Sub RelationLookUpEdit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RelationCustomerLookUpEdit.KeyUp

        If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
        e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
        AndAlso e.KeyData <> Keys.Tab Then
            pomRelacijaCustomer = RelationCustomerLookUpEdit.Text
        Else
            Exit Sub
        End If
        If pomRelacijaCustomer <> String.Empty AndAlso pomRelacijaCustomer.Length >= 7 Then
            Dim par As MainForm = Me.ParentForm
            'Dim pomTekst As String = LookUpEditCustomer.Text
            Using cekaj As New StatusBusy(My.Resources.txtLoading)

                Try
                    _customerVehiclesRelationsList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListByString(pomRelacijaCustomer, False)
                Catch ex As Exception
                    _customerVehiclesRelationsList = Nothing
                End Try

                Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehiclesRelationsList
                If _customerVehiclesRelationsList.Count > 0 Then

                    RelationCustomerLookUpEdit.ClosePopup()
                    RelationCustomerLookUpEdit.ShowPopup()
                    RelationCustomerLookUpEdit.Text = pomRelacijaCustomer

                Else
                    If DockManager1.Panels.Count <= 0 Then

                        pomRelacijaCustomer = ""
                        RelationCustomerLookUpEdit.Text = pomRelacijaCustomer
                        Dim panel As DevExpress.XtraBars.Docking.DockPanel = Me.DockManager1.AddPanel(DevExpress.XtraBars.Docking.DockingStyle.Bottom)
                        panel.Name = "addRelationPanel"
                        panel.Text = "Додавње на нова релација"
                        Dim ux As New uxAddNewRelation()
                        AddHandler ux.Disposed, AddressOf ux_dispose
                        panel.Size = New Size(600, 150)
                        panel.Controls.Add(ux)
                        ux.Dock = DockStyle.Fill
                        ux.BringToFront()

                        panel.Top = True
                        panel.Options.AllowDockBottom = True
                        panel.Options.AllowDockFill = False
                        panel.Options.AllowDockLeft = False
                        panel.Options.AllowDockRight = False
                        panel.Options.AllowFloating = False
                        panel.Options.ShowCloseButton = False
                        panel.Options.ShowAutoHideButton = False
                        panel.BringToFront()

                        panel.Show()

                    Else
                        DockManager1.Panels.Item(DockManager1.Panels.Count - 1).Focus()
                    End If

                End If

            End Using
        End If
    End Sub



    Private Sub RelationVehicleLookUpEdit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RelationVehicleLookUpEdit.KeyUp
        If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
       e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
       AndAlso e.KeyData <> Keys.Tab Then
            pomRelacijaVehicle = RelationVehicleLookUpEdit.Text
        Else
            Exit Sub
        End If
        If pomRelacijaVehicle <> String.Empty AndAlso pomRelacijaVehicle.Length >= 4 Then
            Dim par As MainForm = Me.ParentForm
            'Dim pomTekst As String = LookUpEditCustomer.Text
            Using cekaj As New StatusBusy(My.Resources.txtLoading)

                Try
                    _customerVehiclesRelationsList = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListByString(pomRelacijaVehicle, True)
                Catch ex As Exception
                    _customerVehiclesRelationsList = Nothing
                End Try

                Me.CustomerVehiclesRelationsSearchListBindingSource.DataSource = _customerVehiclesRelationsList
                If _customerVehiclesRelationsList.Count > 0 Then

                    RelationVehicleLookUpEdit.ClosePopup()
                    RelationVehicleLookUpEdit.ShowPopup()
                    RelationVehicleLookUpEdit.Text = pomRelacijaVehicle

                Else
                    If DockManager1.Panels.Count <= 0 Then

                        pomRelacijaCustomer = ""
                        RelationVehicleLookUpEdit.Text = pomRelacijaVehicle
                        Dim panel As DevExpress.XtraBars.Docking.DockPanel = Me.DockManager1.AddPanel(DevExpress.XtraBars.Docking.DockingStyle.Bottom)
                        panel.Name = "addRelationPanel"
                        panel.Text = "Додавње на нова релација"
                        Dim ux As New uxAddNewRelation()
                        AddHandler ux.Disposed, AddressOf ux_dispose
                        panel.Size = New Size(600, 150)
                        panel.Controls.Add(ux)
                        ux.Dock = DockStyle.Fill
                        ux.BringToFront()

                        panel.Top = True
                        panel.Options.AllowDockBottom = True
                        panel.Options.AllowDockFill = False
                        panel.Options.AllowDockLeft = False
                        panel.Options.AllowDockRight = False
                        panel.Options.AllowFloating = False
                        panel.Options.ShowCloseButton = False
                        panel.Options.ShowAutoHideButton = False
                        panel.BringToFront()

                        panel.Show()

                    Else
                        DockManager1.Panels.Item(DockManager1.Panels.Count - 1).Focus()
                    End If

                End If

            End Using
        End If
    End Sub
    Private Sub RelationVehicleLookUpEdit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles RelationVehicleLookUpEdit.GotFocus
        System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    End Sub

    Private Sub RelationCustomerLookUpEdit_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles RelationCustomerLookUpEdit.Validated
        Try
            If _paymentDocument.IdCustomerVehicleRelation > 0 Then '_paymentDocument.IdCustomerVehicleRelation > 0 Then
                Dim pomStat As String = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(_paymentDocument.IdCustomerVehicleRelation).Item(0).Status
                RelationCustomerLookUpEdit.BackColor = Color.FromArgb(CType(pomStat, Integer))
            End If
        Catch ex As Exception
            RelationCustomerLookUpEdit.BackColor = Color.White
        End Try
        PaymentTypeLookUpEdit.Focus()
    End Sub



    'Private Sub RelationCustomerLookUpEdit_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RelationCustomerLookUpEdit.EditValueChanged
    ' Try
    '   If _paymentDocument.IdCustomerVehicleRelation > 0 Then
    '   Dim pomStat As String = CustomerVehiclesRelationsSearchList.GetCustomerVehiclesRelationsListById(_paymentDocument.IdCustomerVehicleRelation).Item(0).Status
    '   RelationCustomerLookUpEdit.BackColor = Color.FromArgb(CType(pomStat, Integer))
    '  End If
    ' Catch ex As Exception

    ' End Try

    'End Sub


    Private Sub txtPolisa_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPolisa.Validated
        If _paymentDocument.PaymentDocumentDetails.Count > 0 Then
            _paymentDocument.PaymentDocumentDetails(0).Note &= " "
        End If
    End Sub

    Private Sub FakturiraNaLookUp_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles FakturiraNaLookUp.ButtonPressed
        If e.Button.Index = 1 Then
            Dim par As MainForm = Me.ParentForm
            For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
                For Each ctl As Control In page.Controls
                    If (TypeOf ctl Is uxCustomers) Then
                        par.ShowWinPart(CType(ctl, uxCustomers))
                        Exit Sub
                    End If
                Next
            Next
            Using cekaj As New StatusBusy(My.Resources.txtLoading)
                Try
                    par.AddWinPart(New uxCustomers(Customer.NewCustomer))
                Catch ex As Exception
                    'Dim dij As New MsgBoxYesNo(My.Resources.msgServerGreska, "OK", My.Resources.Cancel)
                    'dij.ShowDialog()
                    MsgBox(ex.Message)
                End Try
            End Using
        End If
    End Sub

    Private Sub FakturiraNaLookUp_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles FakturiraNaLookUp.KeyUp
        Dim pomC As String = FakturiraNaLookUp.Text
        If e.KeyData <> Keys.Enter AndAlso e.KeyData <> Keys.Down AndAlso _
       e.KeyData <> Keys.Up AndAlso e.KeyData <> Keys.Left AndAlso e.KeyData <> Keys.Right _
       AndAlso e.KeyData <> Keys.Tab Then
        Else
            Exit Sub
        End If
        If pomC <> String.Empty AndAlso pomC.Length >= 7 Then
            Dim par As MainForm = Me.ParentForm
            'Dim pomTekst As String = LookUpEditCustomer.Text
            Using cekaj As New StatusBusy(My.Resources.txtLoading)
                Try
                    _customerSearchList = CustomersSearchList.GetCustomersListShortByString(pomC)
                Catch ex As Exception
                    _customerSearchList = Nothing
                End Try

                Me.CustomersSearchListBindingSource.DataSource = _customerSearchList
                If _customerSearchList.Count > 0 Then

                    FakturiraNaLookUp.ClosePopup()
                    FakturiraNaLookUp.ShowPopup()
                    ' FakturiraNaLookUpEdit.Text = pomRelacijaCustomer
                End If

            End Using
        End If
    End Sub

  
End Class
