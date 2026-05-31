
<Serializable()> _
Public Class PrintDocumentInternationalDriveingLicenceList
  Inherits ReadOnlyListBase(Of PrintDocumentInternationalDriveingLicenceList, PrintDocumentInternationalDriveingLicenceInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "PrintDocumentInternatiomalDriveingLicence"
#End Region

#Region " Factory Methods "

  Public Shared Function GetPrintDocumentInternationalDriveingLicenceList(ByVal idIn As Long) As PrintDocumentInternationalDriveingLicenceList

    Return DataPortal.Fetch(Of PrintDocumentInternationalDriveingLicenceList)(New CriteriaById(idIn))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  <Serializable()> _
Private Class CriteriaById
    Private _idIn As Long

    Public ReadOnly Property IdIn() As Long
      Get
        Return _idIn
      End Get
    End Property

    Public Sub New(ByVal idIn As Long)
      _idIn = idIn
    End Sub
  End Class
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaById)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("PrintDocumentInternationalDriveingLicenceInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          cm.Parameters.AddWithValue("@id", criteria.IdIn)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New PrintDocumentInternationalDriveingLicenceInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("PrintDocumentInternationalDriveingLicenceInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("PrintDocumentInternationalDriveingLicenceInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class