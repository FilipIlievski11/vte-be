
<Serializable()> _
Public Class DocumentTypesOptionsDetailsList
  Inherits ReadOnlyListBase(Of DocumentTypesOptionsDetailsList, DocumentTypesOptionsDetailsInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getDocumentTypesOptionsDetails"
#End Region

#Region " Factory Methods "

  Public Shared Function GetDocumentTypesOptionsDetailsList() As DocumentTypesOptionsDetailsList

    Return DataPortal.Fetch(Of DocumentTypesOptionsDetailsList)()

  End Function

  Public Function GetDocumentTypesOptionsDetailById(ByVal inId As Integer) As DocumentTypesOptionsDetailsInfo
    For Each child As DocumentTypesOptionsDetailsInfo In Me
      If child.Id = inId Then
        Return child
      End If
    Next
    Return Nothing
  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("DocumentTypesOptionsDetailsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentTypesOptionsDetailsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentTypesOptionsDetailsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentTypesOptionsDetailsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class