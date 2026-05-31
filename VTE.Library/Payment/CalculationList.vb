<Serializable()> _
Public Class CalculationList
  Inherits ReadOnlyListBase(Of CalculationList, CalculationInfo)

#Region " Factory Methods "

  Private Sub New()
    'require use for Factory Methods
  End Sub

  Public Shared Function GetCalculationList(ByVal dokument As PaymentDocument) As CalculationList
    Return DataPortal.Fetch(Of CalculationList)(New SingleCriteria(Of CalculationList, PaymentDocument)(dokument))
  End Function
    Public Shared Function GetCalculationListByVehicle(ByVal inIdVehicle As Long) As CalculationList
        Return DataPortal.Fetch(Of CalculationList)(New SingleCriteria(Of CalculationList, Long)(inIdVehicle))
    End Function
#End Region

#Region " Data Access "
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of CalculationList, PaymentDocument))
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("CalculationList.DataPortal_Fetch", GetHashCode())
        Try
            'vcitaj od global PaymentCatalog
            Dim objPaymentCatalogList As PaymentCataologList = _
            CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"), PaymentCataologList)
            'vcitaj Lista na Calcuclation Items
            Dim calculationItemList As CalculationItemList = calculationItemList.GetCalculationItemList

            For Each detal As PaymentDocumentsDetail In criteria.Value.PaymentDocumentDetails
                'zemi CalculationItem by IdPriceCatlaog
                Dim idCalcItem As Integer = objPaymentCatalogList.GetInfoByIdPaymentParametar(detal.IdPriceCatalog).IdCalculationItem
                Dim calcInfo As CalculationItemInfo = calculationItemList.GetCalculationItemById(idCalcItem)
                Dim info As New CalculationInfo(calcInfo.ItemName, calcInfo.BankAccount, calcInfo.Bank, calcInfo.Form, detal.Price)
                Me.Add(info)
            Next

        Catch ex As Exception
            Database.LogException("CalculationList.DataPortal_Fetch", ex)
            Throw New DbCslaException("CalculationList.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of CalculationList, Long))
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("CalculationList.DataPortal_Fetch", GetHashCode())
        Try
            'vcitaj od global PaymentCatalog
            Dim objPaymentCatalogList As PaymentCataologList = _
            CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"), PaymentCataologList)
            'vcitaj Lista na Calcuclation Items
            Dim calculationItemList As CalculationItemList = calculationItemList.GetCalculationItemList

            For Each detal As CustomerFinancialStateTempInfo In CustomerFinancialStateTempList.GetCustomerFinancialStateTempListByVehicle(criteria.Value)
                'zemi CalculationItem by IdPriceCatlaog
                Dim idCalcItem As Integer = objPaymentCatalogList.GetInfoByIdPaymentParametar(detal.Idpricecatalog).IdCalculationItem
                Dim calcInfo As CalculationItemInfo = calculationItemList.GetCalculationItemById(idCalcItem)
                Dim info As New CalculationInfo(calcInfo.ItemName, calcInfo.BankAccount, calcInfo.Bank, calcInfo.Form, detal.Price)
                Me.Add(info)
            Next

        Catch ex As Exception
            Database.LogException("CalculationList.DataPortal_Fetch", ex)
            Throw New DbCslaException("CalculationList.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
#End Region

End Class
