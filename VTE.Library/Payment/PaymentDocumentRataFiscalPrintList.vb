
<Serializable()> _
Public Class PaymentDocumentRataFiscalPrintList
    Inherits ReadOnlyListBase(Of PaymentDocumentRataFiscalPrintList, PaymentDocumentRataFiscalPrintInfo)
    Public ReadOnly Property VkupnaSuma()
        Get
            Dim sum As Decimal = 0
            Try
                'Dim _doc As PaymentDocument = PaymentDocument.GetPaymentDocument(_idpaymentdocument)
                For Each detal As PaymentDocumentRataFiscalPrintInfo In Me
                    sum += detal.Price
                Next
            Catch ex As Exception
                sum = 0
            End Try

            Return FicalRound(sum)
        End Get
    End Property
#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "GetPaymentDocumentRataForFiscalPrintByIdRata"
    Private Const SpZemiSiteRatiZaDogovor As String = "GetPaymentDocumentRataForFiscalPrintByIdDocument"
#End Region

#Region " Factory Methods "

    Public Shared Function GetPaymentDocumentRataFiscalPrintList(ByVal idRata As Long, ByVal isRata As Boolean) As PaymentDocumentRataFiscalPrintList

        Return DataPortal.Fetch(Of PaymentDocumentRataFiscalPrintList)(New CriteriaByIdIsRata(idRata, isRata))

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

    <Serializable()> _
  Private Class CriteriaByIdIsRata
        Private _idRata As Long
        Private _isRata As Boolean
        Public ReadOnly Property IdRata() As Long
            Get
                Return _idRata
            End Get
        End Property
        Public ReadOnly Property IsRata() As Boolean
            Get
                Return _isRata
            End Get
        End Property
        Public Sub New(ByVal idRata As Long, ByVal isRata As Boolean)
            _idRata = idRata
            _isRata = isRata
        End Sub
    End Class
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByIdIsRata)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Using cn As SqlConnection = Database.VTE_SqlConnection

            Using cm As SqlCommand = cn.CreateCommand()
                cm.CommandType = CommandType.StoredProcedure
                If criteria.IsRata Then
                    cm.CommandText = SpZemiSite
                Else
                    cm.CommandText = SpZemiSiteRatiZaDogovor
                End If

                cm.Parameters.AddWithValue("@idRata", criteria.IdRata)
                Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                    While dr.Read()
                        Me.Add(PaymentDocumentRataFiscalPrintInfo.GetPaymentDocumentRataFiscalPrintInfo(dr))
                    End While
                End Using
            End Using
        End Using
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access
End Class