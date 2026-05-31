
<Serializable()> _
Public Class PaymentDocumentFiscalPrintList
  Inherits ReadOnlyListBase(Of PaymentDocumentFiscalPrintList, PaymentDocumentFiscalPrintInfo)

  Public Function GetTotalAmmount() As Double
    Dim result As Double = 0
    For Each child As PaymentDocumentFiscalPrintInfo In Me
      'If child.DDV <> 0 Then
      result += ((1 * child.Price) - ((1 * child.Price) * child.Discount / 100))
      'End If
    Next
    Return result
  End Function

  Public Function ContainsP(ByVal info As PaymentDocumentFiscalPrintInfo) As Boolean
    For Each it As PaymentDocumentFiscalPrintInfo In Me
      If it.CategoryName = info.CategoryName Then
        it.AddPrice(info.Price)
        Return True
      End If
    Next
    Return False
  End Function

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "GetPaymentDocumentForFiscalPrintByIdDocument"
#End Region

#Region " Factory Methods "

  Public Shared Function GetPaymentDocumentFiscalPrintList(ByVal idPaymentDocument As Long) As PaymentDocumentFiscalPrintList

    Return DataPortal.Fetch(Of PaymentDocumentFiscalPrintList)(New SingleCriteria(Of PaymentDocumentFiscalPrintList, Integer)(idPaymentDocument))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of PaymentDocumentFiscalPrintList, Integer))
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("PaymentDocumentFiscalPrintInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          cm.Parameters.AddWithValue("@idDocument", criteria.Value)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New PaymentDocumentFiscalPrintInfo(dr)
              If Not ContainsP(Info) Then
                Me.Add(Info)
              End If
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("PaymentDocumentFiscalPrintInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("PaymentDocumentFiscalPrintInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class