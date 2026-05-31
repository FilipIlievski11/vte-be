
<Serializable()> _
Public Class PrintTrafficLicenceList
  Inherits ReadOnlyListBase(Of PrintTrafficLicenceList, PrintTrafficLicenceInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "printTrafficLicenceById"
#End Region

#Region " Factory Methods "

  Public Shared Function GetPrintTrafficLicenceList(ByVal inId As Long) As PrintTrafficLicenceList

    Return DataPortal.Fetch(Of PrintTrafficLicenceList)(New CriteriaById(inId))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "
  <Serializable()> _
Private Class CriteriaById

    Private _InId As Long

    Public ReadOnly Property InId() As Long
      Get
        Return _InId
      End Get
    End Property


    Public Sub New(ByVal InId As Long)
      _InId = InId
    End Sub

  End Class

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaById)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("PrintTrafficLicenceByIdVehicleInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          cm.Parameters.AddWithValue("@id", criteria.InId)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New PrintTrafficLicenceInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("PrintTrafficLicenceByIdVehicleInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("PrintTrafficLicenceByIdVehicleInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access

End Class