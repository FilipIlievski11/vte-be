
<Serializable()> _
Public Class ActiveDocumentList
 Inherits ReadOnlyListBase(Of ActiveDocumentList, ActiveDocumentsDepInfo)

#Region " Stored Procedures Names "
 Private Const SpZemiSiteOtvoreni As String = "depActiveDocuments"
 Private Const SpZemiSiteZatvoreniBaranja As String = "depClosedDocuments"
 Private Const SpZemiSiteOtvoreniAll As String = "depActiveDocumentsAll"

#End Region

#Region " Factory Methods "

 Public Shared Function GetActiveDocumentList(ByVal isOpen As Boolean, ByVal all As Boolean) As ActiveDocumentList

  Return DataPortal.Fetch(Of ActiveDocumentList)(New FilterCritetria(isOpen, all))

 End Function
 Public Function GetActivListByRelationAndType(ByVal inRelation As Long, ByVal inType As Integer) As ActiveDocumentsDepInfo
  For Each child As ActiveDocumentsDepInfo In Me
   If child.IdCustomerVehicleRelation = inRelation AndAlso _
   child.IdRequestType = inType Then
    Return child
    Exit Function
   End If
  Next
  Return Nothing
 End Function

 Private Sub New()
  ' require use of factory methods
  AddHandler Request.RequestSaved, AddressOf Request_saved
 End Sub

 Private Sub Request_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
  IsReadOnly = False
  Me.Clear()
  IsReadOnly = True
  DataPortal_Fetch(New FilterCritetria(True, False))
  Me.ResetBindings()
 End Sub

#End Region ' Factory Methods

#Region " Data Access "
 <Serializable()> _
Private Class FilterCritetria

  'Public StartDate As Date
  'Public dateEnd As Date

  Public isOpen As Boolean
  Public all As Boolean

  Public Sub New(ByVal isOpen As Boolean, ByVal all As Boolean)
   ' Me.StartDate = StartDate
   ' Me.dateEnd = dateEnd
   Me.isOpen = isOpen
   Me.all = all
  End Sub
 End Class

 ' <Serializable()> _
 'Private Class DateCritetria

 '  Public StartDate As Date
 '  Public dateEnd As Date

 '  Public isOpen As Boolean

 '  Public Sub New(ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal isOpen As Boolean)
 '   Me.StartDate = StartDate
 '   Me.dateEnd = endDate
 '   Me.isOpen = isOpen

 '  End Sub
 ' End Class

 <Serializable()> _
 Private Class Criteria
  ' no criteria - retrieve all projects
 End Class

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As FilterCritetria)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Using cn As SqlConnection = Database.VTE_SqlConnection

   Using cm As SqlCommand = cn.CreateCommand()
    cm.CommandType = CommandType.StoredProcedure
    If criteria.all Then
     If criteria.isOpen Then
      cm.CommandText = SpZemiSiteOtvoreniAll
     Else
      cm.CommandText = SpZemiSiteZatvoreniBaranja
     End If
    Else
     If criteria.isOpen Then
      cm.CommandText = SpZemiSiteOtvoreni
     Else
      cm.CommandText = SpZemiSiteZatvoreniBaranja
     End If
    End If
    
    '  cm.Parameters.AddWithValue("@StartDate", Format(criteria.StartDate, "yyyy-MM-dd"))
    '  cm.Parameters.AddWithValue("@EndDate", Format(criteria.dateEnd.AddDays(1), "yyyy-MM-dd"))
    Dim objCurentStationId As Integer = CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id
    cm.Parameters.AddWithValue("@IdOrganisation", objCurentStationId)
    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
     While dr.Read()
      Me.Add(ActiveDocumentsDepInfo.GetActiveDocumentsDepInfo(dr))
     End While
    End Using
   End Using
  End Using
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub
 'Private Overloads Sub DataPortal_Fetch(ByVal criteria As DateCritetria)
 ' RaiseListChangedEvents = False
 ' IsReadOnly = False
 ' Using cn As SqlConnection = Database.VTE_SqlConnection

 '  Using cm As SqlCommand = cn.CreateCommand()
 '   cm.CommandType = CommandType.StoredProcedure
 '   If criteria.isOpen Then
 '    cm.CommandText = SpZemiSiteOtvoreni
 '   Else
 '    cm.CommandText = SpZemiSiteZatvoreniBaranja
 '   End If
 '   cm.Parameters.AddWithValue("@StartDate", Format(criteria.StartDate, "yyyy-MM-dd"))
 '   cm.Parameters.AddWithValue("@EndDate", Format(criteria.dateEnd.AddDays(1), "yyyy-MM-dd"))
 '   Dim objCurentStationId As Integer = CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id
 '   cm.Parameters.AddWithValue("@IdOrganisation", objCurentStationId)
 '   Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
 '    While dr.Read()
 '     Me.Add(ActiveDocumentsDepInfo.GetActiveDocumentsDepInfo(dr))
 '    End While
 '   End Using
 '  End Using
 ' End Using
 ' IsReadOnly = True
 ' RaiseListChangedEvents = True
 'End Sub
#End Region ' Data Access
End Class