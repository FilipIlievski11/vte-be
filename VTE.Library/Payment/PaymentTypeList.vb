
<Serializable()> _
Public Class PaymentTypeList
 Inherits ReadOnlyListBase(Of PaymentTypeList, PaymentTypeInfo)

 Public Function GetPaymentTypeInfoById(ByVal intId As Integer) As PaymentTypeInfo
  For Each ch In Me
   If ch.Id = intId Then
    Return ch
   End If
  Next
  Return Nothing
 End Function

#Region " Stored Procedures Names "
 Private Const SpZemiSite As String = "getPaymentTypes"
#End Region

#Region " Factory Methods "

 Public Shared Function GetPaymentTypeList() As PaymentTypeList

  Return DataPortal.Fetch(Of PaymentTypeList)()

 End Function

 Private Sub New()
  ' require use of factory methods
 End Sub

#End Region ' Factory Methods

#Region " Data Access "

 Private Overloads Sub DataPortal_Fetch()
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("PaymentTypeInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     Dim tehOrg As TehnicalExamOrganizationsInfo = Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization")
     cm.Parameters.AddWithValue("@IdCompany", tehOrg.IdCompany)
     cm.CommandText = SpZemiSite
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New PaymentTypeInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("PaymentTypeInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("PaymentTypeInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub

#End Region ' Data Access

End Class