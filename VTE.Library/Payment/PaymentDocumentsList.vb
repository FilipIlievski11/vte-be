
<Serializable()> _
Public Class PaymentDocumentsList
  Inherits ReadOnlyListBase(Of PaymentDocumentsList, PaymentDocumentsInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getPaymentDocuments"
#End Region

#Region " Factory Methods "

  Public Shared Function GetPaymentDocumentsList() As PaymentDocumentsList

    Return DataPortal.Fetch(Of PaymentDocumentsList)()

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("PaymentDocumentsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New PaymentDocumentsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("PaymentDocumentsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("PaymentDocumentsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class