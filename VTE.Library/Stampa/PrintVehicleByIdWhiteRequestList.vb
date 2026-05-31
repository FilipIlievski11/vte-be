
<Serializable()> _
Public Class PrintVehicleByIdWhiteRequestList
  Inherits ReadOnlyListBase(Of PrintVehicleByIdWhiteRequestList, PrintVehicleByIdWhiteRequestInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "PrintVehicleByIdWhiteRequest"
#End Region

#Region " Factory Methods "

  Public Shared Function GetPrintVehicleByIdWhiteRequestList(ByVal IdDoc As Long) As PrintVehicleByIdWhiteRequestList

    Return DataPortal.Fetch(Of PrintVehicleByIdWhiteRequestList)(New Criteria(IdDoc))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  <Serializable()> _
Private Class Criteria

    Private _InIdDocument As Long

    Public ReadOnly Property InIdDocument() As Long
      Get
        Return _InIdDocument
      End Get
    End Property


    Public Sub New(ByVal InIdDoc As Long)
      _InIdDocument = InIdDoc
    End Sub

  End Class
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("PrintVehicleByIdWhiteRequestInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          cm.Parameters.AddWithValue("@id", criteria.InIdDocument)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New PrintVehicleByIdWhiteRequestInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("PrintVehicleByIdWhiteRequestInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("PrintVehicleByIdWhiteRequestInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class