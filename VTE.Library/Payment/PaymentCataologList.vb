
<Serializable()> _
Public Class PaymentCataologList
 Inherits ReadOnlyListBase(Of PaymentCataologList, PaymentCataologInfo)

 Public Function GetCrventKrstInfo() As PaymentCataologInfo
  For Each ch As PaymentCataologInfo In Me
   If ch.PrametarName = "Црвен крст" Then
    Return ch
   End If
  Next
  Return Nothing
 End Function

 Public Function GetRSBSPInfo() As PaymentCataologInfo
  Dim rsbsp As TehnicalExamOrganizationsInfo = Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization")
  For Each ch As PaymentCataologInfo In Me
   If ch.IdPaymentCategory = rsbsp.idStavkaZavisnaOdTehnicalExam Then
    Return ch
   End If
  Next
  Return Nothing
    End Function

    
 Public Function GetPaymentForDepts(ByVal strTrigerdBy As String) As PaymentCataologList
  Dim result As PaymentCataologList = PaymentCataologList.EmptyList
  For Each it As PaymentCataologInfo In Me
   If (CBool(CallByName(it, strTrigerdBy, CallType.Get)) = True) Then
    'fiksno
    If it.VehicleField = "Null" Then
     result.AddItemForDepts(it)
    End If
   End If
  Next
  Return result
 End Function


 Public Function GetPaymentForDepts(ByVal strTrigerdBy As String, _
                                    ByVal objVehicle As Vehicle) As PaymentCataologList
  Dim result As PaymentCataologList = PaymentCataologList.EmptyList
  For Each it As PaymentCataologInfo In Me
   'ova e za plakanja nezavisni od vidot na voziloto
   If (CBool(CallByName(it, strTrigerdBy, CallType.Get)) = True) AndAlso _
      (it.VehicleCategoriesForPaymantCode = "?") Then
    'fiksno
    If it.VehicleField = "Null" Then
     result.AddItemForDepts(it)
    End If
   End If
   If (CBool(CallByName(it, strTrigerdBy, CallType.Get)) = True) AndAlso _
      (it.IdVehicleCategoryForPayments = objVehicle.IdVehicleCategoryForPayments) Then
    'bez kategorii za plakanje

    'fiksno
    If it.VehicleField = "Null" Then
     result.AddItemForDepts(it)
    Else
     'presmetlivo
     If (CObj(CallByName(objVehicle, it.VehicleField, CallType.Get)) >= it.ParametarFrom) _
        And (CObj(CallByName(objVehicle, it.VehicleField, CallType.Get)) <= it.ParametarTo) Then
      Try
       result.AddItemForDepts(it)
      Catch ex As Exception
       MsgBox(ex.Message)
      End Try
     End If
    End If
   End If
  Next
  'ako ne najden zapis proveri dali ima dve 0 (po sediste ili po vozilo ...)
  If result.Count = 0 Then
   For Each it As PaymentCataologInfo In Me
    If (CBool(CallByName(it, strTrigerdBy, CallType.Get)) = True) AndAlso _
        (it.IdVehicleCategoryForPayments = objVehicle.IdVehicleCategoryForPayments) AndAlso _
        (it.ParametarFrom = 0) AndAlso _
        (it.ParametarTo = 0) Then
     result.AddItemForDepts(it)
    End If

   Next
  End If

  Return result
 End Function

 Private Sub AddItemForDepts(ByVal it As PaymentCataologInfo)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Me.Add(it)
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub

 Public Function GetInfo(ByVal intId As Integer) As PaymentCataologInfo
  For Each it As PaymentCataologInfo In Me
   If it.IdPaymentParametar = intId Then
    Return it
   End If
  Next
  Return Nothing
 End Function

 Public Function GetInfoByIdPaymentParametar(ByVal intId As Integer) As PaymentCataologInfo
  For Each it As PaymentCataologInfo In Me
   If it.IdPaymentParametar = intId Then
    Return it
   End If
  Next
  Return Nothing
 End Function
 Public Function GetInfoByIdPaymentCategory(ByVal intId As Integer) As PaymentCataologInfo
  For Each it As PaymentCataologInfo In Me
   If it.IdPaymentCategory = intId Then
    Return it
    Exit Function
   End If
  Next
  Return Nothing
 End Function
#Region " Stored Procedures Names "
 Private Const SpZemiSiteCatalog As String = "getPaymentCatalog"
#End Region

#Region " Factory Methods "
 Public Shared Function EmptyList() As PaymentCataologList
  Return New PaymentCataologList
 End Function

 Public Shared Function GetPaymentCataologList() As PaymentCataologList

  Return DataPortal.Fetch(Of PaymentCataologList)()

 End Function

 Private Sub New()
  ' require use of factory methods
  AddHandler PaymentCategories.PaymentCategoriesSaved, AddressOf PaymentCataologList_saved
 End Sub

 Private Sub PaymentCataologList_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
  IsReadOnly = False
  Me.Clear()
  IsReadOnly = True
  DataPortal_Fetch()
  Me.ResetBindings()
 End Sub

#End Region ' Factory Methods

#Region " Data Access "

 Private Overloads Sub DataPortal_Fetch()
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("PaymentCataologInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     Dim tehOrg As TehnicalExamOrganizationsInfo = Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization")
     cm.Parameters.AddWithValue("@IdCompany", tehOrg.IdCompany)
     cm.CommandText = SpZemiSiteCatalog
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New PaymentCataologInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("PaymentCataologInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("PaymentCataologInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub

#End Region ' Data Access
End Class