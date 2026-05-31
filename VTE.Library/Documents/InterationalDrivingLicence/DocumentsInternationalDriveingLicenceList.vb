
<Serializable()> _
Public Class DocumentsInternationalDriveingLicenceList
 Inherits ReadOnlyListBase(Of DocumentsInternationalDriveingLicenceList, DocumentsInternationalDriveingLicenceInfo)

#Region " Stored Procedures Names "
 Private Const SpZemiSite As String = "getDocumentsInternationalDriveingLicencesList"
 Private Const SpZemiSiteOdDo As String = "getDocumentsInternationalDriveingLicencesListFromTo"
#End Region

#Region " Factory Methods "

 Public Shared Function GetDocumentsInternationalDriveingLicenceList() As DocumentsInternationalDriveingLicenceList

  Return DataPortal.Fetch(Of DocumentsInternationalDriveingLicenceList)()

 End Function
 Public Shared Function GetDocumentsInternationalDriveingLicenceListFromTo(ByVal dateStart As Date, ByVal dateEnd As Date) As DocumentsInternationalDriveingLicenceList

  Return DataPortal.Fetch(Of DocumentsInternationalDriveingLicenceList)(New DateCritetria(dateStart, dateEnd))

 End Function

 Private Sub New()
  ' require use of factory methods
 End Sub

#End Region ' Factory Methods

#Region " Data Access "
 <Serializable()> _
Private Class DateCritetria

  Public StartDate As Date
  Public dateEnd As Date

  Public Sub New(ByVal StartDate As Date, ByVal dateEnd As Date)
   Me.StartDate = StartDate
   Me.dateEnd = dateEnd
  End Sub
 End Class
 Private Overloads Sub DataPortal_Fetch()
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("DocumentsInternationalDriveingLicenceInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = SpZemiSite
     cm.Parameters.AddWithValue("@IdOrganisation", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New DocumentsInternationalDriveingLicenceInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("DocumentsInternationalDriveingLicenceInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("DocumentsInternationalDriveingLicenceInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub
 Private Overloads Sub DataPortal_Fetch(ByVal criteria As DateCritetria)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Database.LogInfo("DocumentsInternationalDriveingLicenceInfo.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = SpZemiSiteOdDo
     cm.Parameters.AddWithValue("@StartDate", Format(criteria.StartDate, "yyyy-MM-dd"))
     cm.Parameters.AddWithValue("@EndDate", Format(criteria.dateEnd.AddDays(1), "yyyy-MM-dd"))
     cm.Parameters.AddWithValue("@IdOrganisation", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)

     Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
      While dr.Read()
       Dim Info As New DocumentsInternationalDriveingLicenceInfo(dr)
       Me.Add(Info)
      End While
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("DocumentsInternationalDriveingLicenceInfo.DataPortal_Fetch", ex)
   Throw New DbCslaException("DocumentsInternationalDriveingLicenceInfo.DataPortal_Fetch", ex)
  End Try
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub

#End Region ' Data Access
End Class