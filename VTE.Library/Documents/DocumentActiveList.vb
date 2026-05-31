
<Serializable()> _
Public Class DocumentActiveList
  Inherits ReadOnlyListBase(Of DocumentActiveList, DocumentActiveInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSiteNeZavrseni As String = "getDocumentsNotEnded"
  Private Const SpZamiSiteBezTehnickiPregled As String = "getDocumentsRequestsWithoutTehnicalExam"
  Private Const SpZamiSiteList As String = "getDocumentsList"

#End Region

#Region " Factory Methods "

  Public Shared Function GetDocumentList(ByVal InIdCustomer As Long, ByVal InIdVehicle As Long, _
                                                                ByVal InIdDocType As Integer) As DocumentActiveList

    Return DataPortal.Fetch(Of DocumentActiveList)(New CriteriaIsNotEnded(InIdCustomer, InIdVehicle, InIdDocType))

  End Function
  Public Shared Function GetDocumentListWithoutTehExam() As DocumentActiveList

    Return DataPortal.Fetch(Of DocumentActiveList)()

  End Function
    Public Shared Function GetDocumentAllList(ByVal inInt As Integer) As DocumentActiveList

        Return DataPortal.Fetch(Of DocumentActiveList)(New CriteriaAll(inInt))

    End Function
  Private Sub New()
    ' require use of factory methods
    'AddHandler Document.DocumentSaved, AddressOf Document_saved
  End Sub
  'Private Sub Document_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
  '  IsReadOnly = False
  '  Me.Clear()
  '  IsReadOnly = True
  '  DataPortal_Fetch()
  '  Me.ResetBindings()
  'End Sub

#End Region ' Factory Methods

#Region " Data Access "

  <Serializable()> _
Private Class CriteriaIsNotEnded

    Private _InIdCustomer As Long
    Private _InIdVehicle As Long
    Private _InIdDocType As Integer

    Public ReadOnly Property InIdCustomer() As Long
      Get
        Return _InIdCustomer
      End Get
    End Property
    Public ReadOnly Property InIdVehicle() As Long
      Get
        Return _InIdVehicle
      End Get
    End Property
    Public ReadOnly Property InIdDocType() As Integer
      Get
        Return _InIdDocType
      End Get
    End Property

    Public Sub New(ByVal InIdCustomer As Long, ByVal InIdVehicle As Long, ByVal InIdDocType As Integer)
      _InIdCustomer = InIdCustomer
      _InIdVehicle = InIdVehicle
      _InIdDocType = InIdDocType
    End Sub

  End Class
  <Serializable()> _
Private Class CriteriaAll
    Private _InNekoj As Integer

    Public ReadOnly Property InNekoj() As Integer
      Get
        Return _InNekoj
      End Get
    End Property

    Public Sub New(ByVal InNekoj As Integer)
      _InNekoj = InNekoj
    End Sub

  End Class
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaIsNotEnded)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("DocumentInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSiteNeZavrseni
          cm.Parameters.AddWithValue("@idCustomer", criteria.InIdCustomer)
          cm.Parameters.AddWithValue("@idVehicle", criteria.InIdVehicle)
          cm.Parameters.AddWithValue("@idDocType", criteria.InIdDocType)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentActiveInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("DocumentInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZamiSiteBezTehnickiPregled
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentActiveInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaAll)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("DocumentInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZamiSiteList
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentActiveInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
#End Region ' Data Access
End Class