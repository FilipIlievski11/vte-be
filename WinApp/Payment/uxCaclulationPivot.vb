Public Class uxCaclulationPivot
#Region " WinPart Code "

    Protected Overrides Function GetIdValue() As Object

        Return "Пресметка"

    End Function

    Public Overrides Function ToString() As String

        Return "Пресметка"

    End Function


    Private Sub ux_CurrentPrincipalChanged( _
      ByVal sender As Object, _
      ByVal e As System.EventArgs) _
      Handles Me.CurrentPrincipalChanged
    End Sub


#End Region



    Public Sub New(ByVal calculation As CalculationList, ByVal customer As String, ByVal vehic As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.CalculationListBindingSource.DataSource = calculation
        PivotGridControl1.BestFit()
        Me.CustomerTextEdit.Text = customer
        Me.VehicleTextEdit.Text = vehic
        LookUpVehicle.Enabled = False
        LookUpReqType.Enabled = False
        btnCalculate.Enabled = False

    End Sub
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        LookUpVehicle.Enabled = True
        LookUpReqType.Enabled = True
        btnCalculate.Enabled = False
        ' Add any initialization after the InitializeComponent() call.
        VehicleListShortBindingSource.DataSource = VehicleListShort.GetVehicleListShort
        RequestTypeListBindingSource.DataSource = objRequestTypeList
        PivotGridControl1.BestFit()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        Dim opstina As CommunitiesInfo = objCommunityList.GetCommunitiesListById(objCurentTehExamOrganization.IdCommunity)
        Me.PrintableComponentLink1.PageHeaderFooter = New DevExpress.XtraPrinting.PageHeaderFooter _
        (New DevExpress.XtraPrinting.PageHeaderArea(New String() _
                                                    {Global.WinApp.My.Resources.Resources.String1, "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) _
                                                     & "УПЛАТНИ СМЕТКИ", Global.WinApp.My.Resources.Resources.String1}, New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, _
                                                                                                                                                System.Drawing.GraphicsUnit.Point, CType(204, Byte)), DevExpress.XtraPrinting.BrickAlignment.Center), New DevExpress.XtraPrinting.PageFooterArea(New String() {"" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Датум: [Date Printed]", "Ознака на општината: " & opstina.CommunityName & "-" & opstina.CommunityCode, "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Референт: [User Name]"}, New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte)), DevExpress.XtraPrinting.BrickAlignment.Far))
        PrintableComponentLink1.CreateDocument()
        PivotGridControl1.BestFit()
        Dim user As System.Security.Principal.IPrincipal = _
      Csla.ApplicationContext.User
        PrintableComponentLink1.RtfReportFooter = user.Identity.Name
        PrintableComponentLink1.ShowPreview()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub CreateFinacialStateTemp(ByVal inIdReqType As Integer, ByVal inIdVehicle As Long)
        Dim requestType As RequestTypeInfo = objRequestTypeList.getInfoById(inIdReqType)
        Dim sumZaCrvenKrst As Double = 0
        Dim sumJavniPatista As Double = 0
        Try

            Dim vInfo As Vehicle = Vehicle.GetVehicle(inIdVehicle)
            'filtriraj go katalogot so paramaetri:
            '1. TrigerdBy? (Request)
            '2. IdVehicleCategoryForPayments (od vozilito)
            '3. prebaraj dali konkretnoto vozilo spaga vo nekoi od tie
            Dim pCatalog As PaymentCataologList = _
              CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
              PaymentCataologList).GetPaymentForDepts("TrigerdByRequest", vInfo)
            'zapamti gi site plakanje vo listata
            Using cn As SqlClient.SqlConnection = Database.VTE_SqlConnection
                For Each pInfo As PaymentCataologInfo In pCatalog
                    'vo sluaj poedinecno(so formula)
                    If (pInfo.ParametarFrom = 0) AndAlso (pInfo.ParametarTo = 0) Then
                        Using cm As SqlClient.SqlCommand = cn.CreateCommand
                            cm.CommandType = CommandType.StoredProcedure
                            cm.CommandText = "addCustomerFinancialStateTem"

                            cm.Parameters.AddWithValue("@IdVehicle", inIdVehicle)
                            cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)
                            If pInfo.VehicleField = "Null" Then
                                'fiksno

                                sumZaCrvenKrst += pInfo.Price
                                If pInfo.CategoryName.Contains("јавни патишта") Then
                                    sumJavniPatista = pInfo.Price
                                    cm.Parameters.AddWithValue("@Price", pInfo.Price - Math.Round(pInfo.Price * 1 / 100, 1))
                                Else
                                    cm.Parameters.AddWithValue("@Price", pInfo.Price)
                                End If
                            Else
                                'presmetlivo

                                Dim pomPrice As Double = pInfo.Price * CInt(CallByName(vInfo, pInfo.VehicleField, CallType.Get))
                                sumZaCrvenKrst += pomPrice
                                If pInfo.CategoryName.Contains("јавни патишта") Then
                                    sumJavniPatista = pomPrice
                                    cm.Parameters.AddWithValue("@Price", pomPrice - (Math.Round(1 / 100 * pomPrice, 1)))
                                Else
                                    cm.Parameters.AddWithValue("@Price", pomPrice)
                                End If
                            End If

                            cm.ExecuteNonQuery()
                        End Using
                    Else
                        Using cm As SqlClient.SqlCommand = cn.CreateCommand
                            cm.CommandType = CommandType.StoredProcedure
                            cm.CommandText = "addCustomerFinancialStateTem"

                            cm.Parameters.AddWithValue("@IdVehicle", inIdVehicle)
                            cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)

                            cm.Parameters.AddWithValue("@Price", pInfo.Price)
                            If pInfo.CategoryName.Contains("јавни патишта") Then
                                sumJavniPatista = pInfo.Price
                                cm.Parameters.AddWithValue("@Price", pInfo.Price - (Math.Round(1 / 100 * pInfo.Price, 1)))
                            Else
                                cm.Parameters.AddWithValue("@Price", pInfo.Price)

                            End If
                            cm.ExecuteNonQuery()
                        End Using

                    End If
                Next
            End Using

            'MsgBox("da")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        If requestType.IsTehnicalExamRequired > 0 Then
            Try

                Dim vInfo As Vehicle = Vehicle.GetVehicle(inIdVehicle)

                Dim pCatalog As PaymentCataologList
                Dim delitel As Single = 1

                pCatalog = _
                CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
                PaymentCataologList).GetPaymentForDepts("TrigerdByTechnicalExam", vInfo)
                Using cn As SqlClient.SqlConnection = Database.VTE_SqlConnection
                    'zapamti gi site plakanje vo listata
                    If delitel > 0 Then
                        For Each pInfo As PaymentCataologInfo In pCatalog
                            'vo sluaj poedinecno(so formula)
                            Dim pomDelitel As Single = 1 ' delitel
                            If (pInfo.ParametarFrom = 0) AndAlso (pInfo.ParametarTo = 0) Then


                                Using cm As SqlClient.SqlCommand = cn.CreateCommand
                                    cm.CommandType = CommandType.StoredProcedure
                                    cm.CommandText = "addCustomerFinancialStateTem"

                                    cm.Parameters.AddWithValue("@IdVehicle", inIdVehicle)
                                    cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)
                                    If pInfo.PaymentName.Contains("животна средина") Then
                                        If vInfo.IdEngineEcoProgram > 0 Then
                                            Dim ecoPercent As Decimal = _
                                            CType(Csla.ApplicationContext.LocalContext("objVehicleEngineEcoProgramList"),  _
                                            VehicleEngineEcoProgramList).GetInfoById(vInfo.IdEngineEcoProgram).PercentForPayment
                                            cm.Parameters.AddWithValue("@Price", pInfo.Price * ecoPercent / 100)
                                            sumZaCrvenKrst += pInfo.Price * ecoPercent / 100
                                        Else
                                            cm.Parameters.AddWithValue("@Price", pInfo.Price)
                                            sumZaCrvenKrst += pInfo.Price
                                        End If
                                    Else
                                        If (pInfo.PaymentName.Contains("Технички преглед")) Then
                                            pomDelitel = delitel
                                        Else
                                            pomDelitel = 1
                                        End If

                                        If pInfo.VehicleField = "Null" Then
                                            'fiksno
                                            If pInfo.CategoryName.Contains("јавни патишта") Then
                                                sumJavniPatista = pInfo.Price * pomDelitel
                                                cm.Parameters.AddWithValue("@Price", pInfo.Price * pomDelitel - (Math.Round(1 / 100 * pInfo.Price * pomDelitel, 1)))
                                            Else
                                                cm.Parameters.AddWithValue("@Price", pInfo.Price * pomDelitel)

                                            End If

                                            sumZaCrvenKrst += pInfo.Price * pomDelitel
                                        Else
                                            'presmetlivo
                                            Dim pomV As Double = pInfo.Price * CInt(CallByName(vInfo, pInfo.VehicleField, CallType.Get)) * pomDelitel
                                            If pInfo.CategoryName.Contains("јавни патишта") Then
                                                sumJavniPatista = pomV
                                                cm.Parameters.AddWithValue("@Price", pomV - (Math.Round(1 / 100 * pomV, 1)))
                                            Else
                                                cm.Parameters.AddWithValue("@Price", pomV)

                                            End If

                                            sumZaCrvenKrst += (pInfo.Price * CInt(CallByName(vInfo, pInfo.VehicleField, CallType.Get))) * pomDelitel
                                        End If

                                    End If
                                    cm.ExecuteNonQuery()
                                End Using
                            Else

                                Using cm As SqlClient.SqlCommand = cn.CreateCommand
                                    cm.CommandType = CommandType.StoredProcedure
                                    cm.CommandText = "addCustomerFinancialStateTem"
                                    cm.Parameters.AddWithValue("@IdVehicle", inIdVehicle)

                                    cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)

                                    'pazi tuka
                                    If pInfo.PaymentName.Contains("животна средина") Then
                                        If vInfo.IdEngineEcoProgram > 0 Then
                                            Dim ecoPercent As Decimal = _
                                            CType(Csla.ApplicationContext.LocalContext("objVehicleEngineEcoProgramList"),  _
                                            VehicleEngineEcoProgramList).GetInfoById(vInfo.IdEngineEcoProgram).PercentForPayment
                                            cm.Parameters.AddWithValue("@Price", pInfo.Price * ecoPercent / 100)
                                            sumZaCrvenKrst += pInfo.Price * ecoPercent / 100
                                        Else
                                            cm.Parameters.AddWithValue("@Price", pInfo.Price)
                                            sumZaCrvenKrst += pInfo.Price
                                        End If
                                    Else
                                        If (pInfo.PaymentName.Contains("Технички преглед")) Then
                                            pomDelitel = delitel
                                        Else
                                            pomDelitel = 1
                                        End If
                                        If pInfo.CategoryName.Contains("јавни патишта") Then
                                            sumJavniPatista = pInfo.Price * pomDelitel
                                            cm.Parameters.AddWithValue("@Price", pInfo.Price * pomDelitel - (Math.Round(1 / 100 * pInfo.Price * pomDelitel, 1)))
                                        Else
                                            cm.Parameters.AddWithValue("@Price", pInfo.Price * pomDelitel)

                                        End If

                                        sumZaCrvenKrst += pInfo.Price * pomDelitel
                                    End If

                                    cm.ExecuteNonQuery()
                                End Using

                            End If
                        Next

                    End If
                    Dim objPriceCatalog As PaymentCataologList = CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"), PaymentCataologList)

                    Dim infoCrvenKrst As PaymentCataologInfo = objPriceCatalog.GetCrventKrstInfo

                    Using cm As SqlClient.SqlCommand = cn.CreateCommand
                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = "addCustomerFinancialStateTem"

                        cm.Parameters.AddWithValue("@IdVehicle", inIdVehicle)
                        cm.Parameters.AddWithValue("@IdPriceCatalog", infoCrvenKrst.IdPaymentParametar)
                        cm.Parameters.AddWithValue("@Price", Math.Round(sumZaCrvenKrst * infoCrvenKrst.Price / 100))
                        cm.ExecuteNonQuery()
                    End Using



                    If objCurentTehExamOrganization.CalculatePercentOfTeh Then
                        Dim infoRSBS As PaymentCataologInfo = objPriceCatalog.GetRSBSPInfo
                        Dim sumRSBS As Double = 0

                        If infoRSBS IsNot Nothing Then
                            Dim name As String = objPriceCatalog.GetInfoByIdPaymentCategory(objCurentTehExamOrganization.idTehnicalExam).CategoryName

                            Using cm As SqlClient.SqlCommand = cn.CreateCommand
                                cm.CommandType = CommandType.StoredProcedure
                                cm.CommandText = "getCustomerFinancialStateTempZaRSBSByVehicle"

                                cm.Parameters.AddWithValue("@IdVehicle", inIdVehicle)
                                cm.Parameters.AddWithValue("@CatName", name)
                                sumRSBS = cm.ExecuteScalar
                            End Using
                            Using cm As SqlClient.SqlCommand = cn.CreateCommand
                                cm.CommandType = CommandType.StoredProcedure
                                cm.CommandText = "addCustomerFinancialStateTem"
                                cm.Parameters.AddWithValue("@IdVehicle", inIdVehicle)
                                cm.Parameters.AddWithValue("@IdPriceCatalog", infoRSBS.IdPaymentParametar)
                                cm.Parameters.AddWithValue("@Price", Math.Round(sumRSBS * objCurentTehExamOrganization.PercentOfTeh / 100 + Math.Round(sumJavniPatista * 1 / 100)))
                                cm.ExecuteNonQuery()
                            End Using
                        End If
                    End If
                End Using



                'MsgBox("da")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        End If
    End Sub

    Private Sub btnCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalculate.Click
        Try
            CistiPomCustomFInState(LookUpVehicle.EditValue)
            CreateFinacialStateTemp(LookUpReqType.EditValue, LookUpVehicle.EditValue)
            Dim calc As CalculationList = CalculationList.GetCalculationListByVehicle(LookUpVehicle.EditValue)
            Me.CalculationListBindingSource.DataSource = calc
            PivotGridControl1.BestFit()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub uxCaclulationPivot_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        CistiPomCustomFInState(LookUpVehicle.EditValue)
    End Sub
    Private Sub CistiPomCustomFInState(ByVal pomVId As Long)
        If pomVId > 0 Then
            Try

                Using cn As SqlClient.SqlConnection = Database.VTE_SqlConnection
                    Using cm As SqlClient.SqlCommand = cn.CreateCommand
                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = "deleteCustomerFinancialStateTemForVehicle"

                        cm.Parameters.AddWithValue("@IdVehicle", pomVId)
                        cm.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub LookUpVehicle_ButtonPressed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpVehicle.ButtonPressed
        If e.Button.Index = 1 Then
            Dim par As MainForm = Me.ParentForm

            Using cekaj As New StatusBusy(My.Resources.txtLoading)
                Try
                    par.AddWinPart(New uxVehicle(Vehicle.NewVehicle))
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End Using
        End If
    End Sub

    Private Sub LookUpVehicle_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LookUpVehicle.EditValueChanged, LookUpReqType.EditValueChanged
        If LookUpVehicle.EditValue > 0 AndAlso LookUpReqType.EditValue > 0 Then
            btnCalculate.Enabled = True
        Else
            btnCalculate.Enabled = False
        End If
    End Sub

    Private Sub uxCaclulationPivot_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PivotGridControl1.BestFit()
    End Sub
End Class
