

<Serializable()> _
Public Class PayDocList
  Inherits ReadOnlyListBase(Of PayDocList, PayDocInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getPayDocList"
#End Region

#Region " Factory Methods "

  Public Shared Function GetPayDocList(ByVal idPaymentType As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As PayDocList

    Return DataPortal.Fetch(Of PayDocList)(New filterCriteria(idPaymentType, dateFrom, dateTo))

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
 Private Class filterCriteria
    Private _idPaymentType As Integer
    Private _dateFrom As Date
    Private _dateTo As Date
    Public ReadOnly Property idPaymentType() As Integer
      Get
        Return _idPaymentType
      End Get
    End Property
    Public ReadOnly Property DateFrom() As Date
      Get
        Return _dateFrom
      End Get
    End Property
    Public ReadOnly Property DateTo() As Date
      Get
        Return _dateTo
      End Get
    End Property

    Public Sub New(ByVal idPaymentType As Integer, ByVal Dfrom As Date, ByVal Dto As Date)
      _idPaymentType = idPaymentType
      _dateFrom = Dfrom
      _dateTo = Dto
    End Sub
  End Class
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As filterCriteria)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Using cn As SqlConnection = Database.VTE_SqlConnection

      Using cm As SqlCommand = cn.CreateCommand()
        cm.CommandType = CommandType.StoredProcedure
        cm.CommandText = SpZemiSite
        cm.Parameters.AddWithValue("@id", criteria.idPaymentType)
        cm.Parameters.AddWithValue("@StartDate", Format(criteria.DateFrom, "yyyy-MM-dd"))
        cm.Parameters.AddWithValue("@EndDate", Format(criteria.DateTo.AddDays(1), "yyyy-MM-dd"))
        Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
          While dr.Read()
            Me.Add(PayDocInfo.GetPayDocInfo(dr))
          End While
        End Using
      End Using
    End Using
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access

End Class
