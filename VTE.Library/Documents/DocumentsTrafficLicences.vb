
<Serializable()> _
Public Class DocumentsTrafficLicences
  Inherits Csla.BusinessListBase(Of DocumentsTrafficLicences, DocumentsTrafficLicence)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentsTrafficLicenceByID"
  Private Const spGetAll As String = "GetDocumentsTrafficLicences"
  Private Const spUpdate As String = "updateDocumentsTrafficLicence"
  Private Const spAdd As String = "addDocumentsTrafficLicence"
  Private Const spDelete As String = "deleteDocumentsTrafficLicence"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DocumentsTrafficLicence = DocumentsTrafficLicence.NewDocumentsTrafficLicenceChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsTrafficLcences")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsTrafficLcences")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsTrafficLcences")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsTrafficLcences")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetDocumentsTrafficLcences() As DocumentsTrafficLicences
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a DocumentsTrafficLcences")
    End If
    Return DataPortal.Fetch(Of DocumentsTrafficLicences)()
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("DocumentsTrafficLcences.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(DocumentsTrafficLicence.GetDocumentsTrafficLicence(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DocumentsTrafficLcences.Child_Fetch", ex)
      Throw New DbCslaException("DocumentsTrafficLcences.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


  Protected Overrides Sub DataPortal_Update()
    RaiseListChangedEvents = False
    Child_Update()
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access

#Region " NumOfTrafficLicenceExists "

  Public Shared Function TrafficLicenceExists(ByVal strTrafficLicence As String, ByVal idDoc As Long) As Boolean

    Return TrafficLicenceExistsCommand.TrafficLicenceExists(strTrafficLicence, idDoc)

  End Function

  <Serializable()> _
  Private Class TrafficLicenceExistsCommand
    Inherits CommandBase
    Private _TrafficLicence As String
    Private _idDoc As Long
    Private _TrafficLicenceExists As Boolean
    Public ReadOnly Property ExistsTrafficLicence() As Boolean
      Get
        Return _TrafficLicenceExists
      End Get
    End Property

    Public Shared Function TrafficLicenceExists(ByVal numTrafficLicence As String, ByVal idDoc As Long) As Boolean

      Dim result As TrafficLicenceExistsCommand
      result = DataPortal.Execute(Of TrafficLicenceExistsCommand)(New TrafficLicenceExistsCommand(numTrafficLicence, idDoc))
      Return result.ExistsTrafficLicence
    End Function

    Private Sub New(ByVal strTrafficLicence As String, ByVal idDoc As Long)
      _TrafficLicence = strTrafficLicence
      _idDoc = idDoc
      _TrafficLicenceExists = False
    End Sub

    Protected Overrides Sub DataPortal_Execute()
      Dim pom As Integer = 0
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "NumOfTrafficLicenceExists"
          cm.Parameters.AddWithValue("@num", _TrafficLicence)
          cm.Parameters.AddWithValue("@idDoc", _idDoc)
          pom = cm.ExecuteScalar
          If pom = 0 Then
            _TrafficLicenceExists = False
          Else
            _TrafficLicenceExists = True
          End If
        End Using
      End Using
    End Sub

  End Class

#End Region
End Class
