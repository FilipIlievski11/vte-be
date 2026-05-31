
<Serializable()> _
Public Class DocumentTypesOptionsList
  Inherits ReadOnlyListBase(Of DocumentTypesOptionsList, DocumentTypesOptionsInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getDocumentTypesOptions"
#End Region

#Region " Factory Methods "

  Public Shared Function GetDocumentTypesOptionsList() As DocumentTypesOptionsList

    Return DataPortal.Fetch(Of DocumentTypesOptionsList)()

  End Function

  Public Function GetDocumentTypesOptionsListById(ByVal inId As Integer) As DocumentTypesOptionsInfo
    For Each child As DocumentTypesOptionsInfo In Me
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
    Database.LogInfo("DocumentTypesOptionsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentTypesOptionsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentTypesOptionsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentTypesOptionsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class