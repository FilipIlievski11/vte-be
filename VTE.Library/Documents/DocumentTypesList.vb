
<Serializable()> _
Public Class DocumentTypesList
  Inherits ReadOnlyListBase(Of DocumentTypesList, DocumentTypesInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getDocumentTypes"
#End Region

#Region " Factory Methods "

  Public Shared Function GetDocumentTypesList() As DocumentTypesList

    Return DataPortal.Fetch(Of DocumentTypesList)()

  End Function

  Public Function GetDocumentTypesListById(ByVal idIn As Integer) As DocumentTypesInfo
    For Each child As DocumentTypesInfo In Me
      If child.Id = idIn Then
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
    Database.LogInfo("DocumentTypesInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentTypesInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentTypesInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentTypesInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class