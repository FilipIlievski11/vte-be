
<Serializable()> _
Public Class TehnicalExamOrganizationsList
  Inherits ReadOnlyListBase(Of TehnicalExamOrganizationsList, TehnicalExamOrganizationsInfo)

#Region " Stored Procedures Names "
 Private Const SpZemiSite As String = "getTehnicalExamOrganizationsList"
 Private Const SpZemiBezMomentalnaStanica As String = "getTehnicalExamOrganizationsListBezMomentalnaStanica"
#End Region

#Region " Factory Methods "

  Public Shared Function GetTehnicalExamOrganizationsList() As TehnicalExamOrganizationsList

    Return DataPortal.Fetch(Of TehnicalExamOrganizationsList)()

  End Function

  Public Shared Function GetTehnicalExamOrganizationsListBezMomentalna(ByVal inId As Integer) As TehnicalExamOrganizationsList

    Return DataPortal.Fetch(Of TehnicalExamOrganizationsList)(New FiltCriteria(inId))
  End Function

  Public Function GetTehnicalExamOrganizationsInfoById(ByVal inId As Integer) As TehnicalExamOrganizationsInfo
    For Each child As TehnicalExamOrganizationsInfo In Me
      If child.Id = inId Then
        Return child
      End If
    Next
    Return Nothing
 End Function
 Public Shared Function EmptyList() As TehnicalExamOrganizationsList
  Return New TehnicalExamOrganizationsList
 End Function
 Public Function GetStationsForCompany() As TehnicalExamOrganizationsList
  Dim lista As TehnicalExamOrganizationsList = TehnicalExamOrganizationsList.EmptyList
   
    For Each Child As TehnicalExamOrganizationsInfo In Me
      If Child.IdCompany = CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).IdCompany Then
        lista.RaiseListChangedEvents = False
        lista.IsReadOnly = False
        lista.Add(Child)
        lista.IsReadOnly = True
        lista.RaiseListChangedEvents = True
      End If
    Next
  Return lista
 End Function
  Private Sub New()
    ' require use of factory methods
    AddHandler TehnicalExamOrganizations.TehnicalExamOrganizationsSaved, AddressOf TehnicalExamOrganizations_saved
  End Sub

  Private Sub TehnicalExamOrganizations_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
    IsReadOnly = False
    Me.Clear()
    IsReadOnly = True
    DataPortal_Fetch()
    Me.ResetBindings()
  End Sub
#End Region ' Factory Methods

#Region " Data Access "
  <Serializable()> _
Private Class FiltCriteria
    Private _idIn As Integer

    Public ReadOnly Property IdIn() As Integer
      Get
        Return _idIn
      End Get
    End Property

    Public Sub New(ByVal idIn As Long)
      _idIn = idIn
    End Sub
  End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("TehnicalExamOrganizationsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New TehnicalExamOrganizationsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("TehnicalExamOrganizationsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("TehnicalExamOrganizationsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As FiltCriteria)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("TehnicalExamOrganizationsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiBezMomentalnaStanica
          cm.Parameters.AddWithValue("@id", criteria.IdIn)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New TehnicalExamOrganizationsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("TehnicalExamOrganizationsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("TehnicalExamOrganizationsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
#End Region ' Data Access

End Class