
<Serializable()> _
Public Class PrintPaymentDocumetnByIdDocumetnList
  Inherits ReadOnlyListBase(Of PrintPaymentDocumetnByIdDocumetnList, PrintPaymentDocumetnByIdDocumetnInfo)


  Public ReadOnly Property DDVSumi() As Dictionary(Of String, Decimal)
    Get
      Dim result As New Dictionary(Of String, Decimal)
      For Each item As PrintPaymentDocumetnByIdDocumetnInfo In Me
        If Not result.ContainsKey(item.DDVProcent) Then
          'ako ne sodrzi
          result.Add(item.DDVProcent, item.DDV)
        Else
          'ako sodrzi
          result.Item(item.DDVProcent) += item.DDV
        End If
      Next
      Return result
    End Get

  End Property

#Region " Stored Procedures Names "
 Private Const SpZemiSite As String = "printPaymentDocumetnByIdDocumetn"
 Private Const SpZemiSiteForCustomer As String = "printPaymentDocumentByIdCustomer"

#End Region

#Region " Factory Methods "

 Public Shared Function GetPrintPaymentDocumetnByIdDocumetnList() As PrintPaymentDocumetnByIdDocumetnList

  Return DataPortal.Fetch(Of PrintPaymentDocumetnByIdDocumetnList)()

 End Function

 Public Shared Function GetPrintPaymentDocumetnByIdDocumetnList(ByVal idDocument As Integer) As PrintPaymentDocumetnByIdDocumetnList

  Return DataPortal.Fetch(Of PrintPaymentDocumetnByIdDocumetnList) _
    (New SingleCriteria(Of PrintPaymentDocumetnByIdDocumetnList, Integer)(idDocument))

 End Function

 Public Shared Function GetPrintPaymentDocumetnByIdDocumetnList(ByVal StartDate As Date, ByVal EndDate As Date, ByVal isPayDocDate As Boolean) As PrintPaymentDocumetnByIdDocumetnList

  Return DataPortal.Fetch(Of PrintPaymentDocumetnByIdDocumetnList)(New DateCritetria(StartDate, EndDate, isPayDocDate))

 End Function

 Public Shared Function GetPrintPaymentDocumetnByIdDocumetnListPlusRati(ByVal StartDate As Date, ByVal EndDate As Date) As PrintPaymentDocumetnByIdDocumetnList

  Return DataPortal.Fetch(Of PrintPaymentDocumetnByIdDocumetnList)(New FullCritetria(StartDate, EndDate))

 End Function
 Public Shared Function GetPrintPaymentDocumetnByDocumetnNumber(ByVal inNum As String) As PrintPaymentDocumetnByIdDocumetnList

  Return DataPortal.Fetch(Of PrintPaymentDocumetnByIdDocumetnList)(New CriteriaByDocNum(inNum))

 End Function
 Public Shared Function GetPrintPaymentDocumetnByIdCustomer(ByVal idCustomer As Long) As PrintPaymentDocumetnByIdDocumetnList

  Return DataPortal.Fetch(Of PrintPaymentDocumetnByIdDocumetnList) _
    (New CustomerCritetria(idCustomer))

 End Function

 Private Sub New()
  ' require use of factory methods
 End Sub

#End Region ' Factory Methods

#Region " Data Access "

 <Serializable()> _
 Private Class DateCritetria

  Public StartDate As Date
  Public EndDate As Date
  Public isPayDocDate As Boolean

  Public Sub New(ByVal StartDate As Date, ByVal EndDate As Date, ByVal isPayDocDate As Boolean)
   Me.StartDate = StartDate.Date
   Me.EndDate = EndDate.Date
   Me.isPayDocDate = isPayDocDate
  End Sub
 End Class

 <Serializable()> _
Private Class CriteriaByDocNum

  Public DocNum As String

  Public Sub New(ByVal DocNum As String)

   Me.DocNum = DocNum
  End Sub
 End Class

 <Serializable()> _
 Private Class FullCritetria

  Public StartDate As Date
  Public EndDate As Date

  Public Sub New(ByVal StartDate As Date, ByVal EndDate As Date)
   Me.StartDate = StartDate.Date
   Me.EndDate = EndDate.Date
  End Sub
 End Class
 <Serializable()> _
 Private Class CustomerCritetria

  Public idCustomer As Long

  Public Sub New(ByVal idCustomer As Long)
   Me.idCustomer = idCustomer
  End Sub
 End Class
 Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of PrintPaymentDocumetnByIdDocumetnList, Integer))
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = SpZemiSite
     cm.Parameters.AddWithValue("@idDocument", criteria.Value)
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New PrintPaymentDocumetnByIdDocumetnInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub

 Private Overloads Sub DataPortal_Fetch()
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "printPaymentDocument"
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New PrintPaymentDocumetnByIdDocumetnInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", ex)
  End Try

  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub
 Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByDocNum)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = "printPaymentDocumetnByDocNum"
     cm.Parameters.AddWithValue("@docNum", criteria.DocNum)
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New PrintPaymentDocumetnByIdDocumetnInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   MsgBox(My.Resources.GresenBr)
   Database.LogException("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", ex)
   'Throw New DbCslaException("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub
 Private Overloads Sub DataPortal_Fetch(ByVal criteria As DateCritetria)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     If criteria.isPayDocDate Then
      cm.CommandText = "printPaymentDocumentByDate"
     Else
      cm.CommandText = "DolgIzvestaj" '"printPaymentDocumentByDateRata"
     End If
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
     cm.Parameters.AddWithValue("@StartDate", Format(criteria.StartDate, "yyyy-MM-dd"))
     cm.Parameters.AddWithValue("@EndDate", Format(criteria.EndDate.AddDays(1), "yyyy-MM-dd"))
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New PrintPaymentDocumetnByIdDocumetnInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub
 Private Overloads Sub DataPortal_Fetch(ByVal criteria As FullCritetria)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure

     cm.CommandText = "printPaymentDocumentByDatePlusRati"
     cm.Parameters.AddWithValue("@StartDate", Format(criteria.StartDate, "yyyy-MM-dd"))
     cm.Parameters.AddWithValue("@EndDate", Format(criteria.EndDate.AddDays(1), "yyyy-MM-dd"))
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New PrintPaymentDocumetnByIdDocumetnInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub
 Private Overloads Sub DataPortal_Fetch(ByVal criteria As CustomerCritetria)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = SpZemiSiteForCustomer
     cm.Parameters.AddWithValue("@idCustomer", criteria.idCustomer)
     cm.Parameters.AddWithValue("@IdCompany", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany)
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New PrintPaymentDocumetnByIdDocumetnInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("PrintPaymentDocumetnByIdDocumetnInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub
#End Region ' Data Access

End Class