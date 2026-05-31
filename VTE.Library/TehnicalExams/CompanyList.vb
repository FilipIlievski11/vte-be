
<Serializable()> _
Public Class CompanyList
 Inherits ReadOnlyListBase(Of CompanyList, CompanyInfo)

#Region " Stored Procedures Names "
 Private Const SpZemiSite As String = "getCompanies"
#End Region

#Region " Factory Methods "

 Public Shared Function GetCompanyList() As CompanyList

  Return DataPortal.Fetch(Of CompanyList)(New Criteria)

 End Function

 Private Sub New()
  ' require use of factory methods
 End Sub

#End Region ' Factory Methods

#Region " Data Access "

 <Serializable()> _
 Private Class Criteria
  ' no criteria - retrieve all projects
 End Class

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
  RaiseListChangedEvents = False
  IsReadOnly = False
  Using cn As SqlConnection = Database.VTE_SqlConnection

   Using cm As SqlCommand = cn.CreateCommand()
    cm.CommandType = CommandType.StoredProcedure
    cm.CommandText = SpZemiSite
    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
     While dr.Read()
      Me.Add(CompanyInfo.GetCompanyInfo(dr))
     End While
    End Using
   End Using
  End Using
  IsReadOnly = True
  RaiseListChangedEvents = True
 End Sub

#End Region ' Data Access
End Class