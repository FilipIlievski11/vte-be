
<Serializable()> _
Public Class DocumentPaymentProofList
  Inherits ReadOnlyListBase(Of DocumentPaymentProofList, DocumentPaymentProofInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getDocumentPaymentProof"
#End Region

#Region " Factory Methods "

  Public Shared Function GetDocumentPaymentProofList() As DocumentPaymentProofList

    Return DataPortal.Fetch(Of DocumentPaymentProofList)()

  End Function

  Public Function GetDocumentPaymentProofInfo(ByVal inId As Integer) As DocumentPaymentProofInfo
    For Each child As DocumentPaymentProofInfo In Me
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
    Database.LogInfo("DocumentPaymentProofInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Dim nullInfo As New DocumentPaymentProofInfo(0, "[нема]")
          Me.Add(nullInfo)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentPaymentProofInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentPaymentProofInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentPaymentProofInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class