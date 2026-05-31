
<Serializable()> _
Public Class CustomerVehiclesRelationsList
  Inherits ReadOnlyListBase(Of CustomerVehiclesRelationsList, CustomerVehiclesRelationsInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getCustomerVehiclesRelationsList"
  Private Const SpZemiPoCustomerVehicleAndType As String = "getCustomerVehiclesRelationsListByCustomerVehicleAndType"
    Private Const SpZemiSiteByString As String = "getCustomerVehiclesRelationsListByString"
    Private Const SpZemiSiteByStringVehicle As String = "getCustomerVehiclesRelationsListByStringVehicle"
    Private Const SpZemiSiteByStringCustomer As String = "getCustomerVehiclesRelationsListByStringCustomer"
#End Region

  Public Function getInfoByIdVehicle(ByVal idVehicle As Long)
    For Each ch As CustomerVehiclesRelationsInfo In Me
      If ch.IdVehicle = idVehicle AndAlso (ch.EndDate = Nothing) Then

        Return ch

      End If
    Next
    Return Nothing
  End Function

  Public Function getInfoByIdVehicleAndIdCostomer(ByVal idVehicle As Long, ByVal idCustomer As Long) As CustomerVehiclesRelationsInfo
    For Each ch As CustomerVehiclesRelationsInfo In Me
      If (ch.IdVehicle = idVehicle) And (ch.IdCustomer = idCustomer) Then
        Return ch
      End If
    Next
    Return Nothing
  End Function

#Region " Factory Methods "

  Public Shared Function GetCustomerVehiclesRelationsList() As CustomerVehiclesRelationsList

    Return DataPortal.Fetch(Of CustomerVehiclesRelationsList)()

  End Function
  Public Function GetInfoRelationById(ByVal inId As Long) As CustomerVehiclesRelationsInfo
    For Each child As CustomerVehiclesRelationsInfo In Me
      If child.Id = inId Then
        Return child
      End If
    Next
    Return Nothing
    End Function
 
  Public Shared Function EmptyList() As CustomerVehiclesRelationsList

    Return New CustomerVehiclesRelationsList

    End Function

    Public Shared Function GetCustomerVehiclesRelationsListByString(ByVal inStr As String, ByVal isVehicle As Boolean) As CustomerVehiclesRelationsList

        Return DataPortal.Fetch(Of CustomerVehiclesRelationsList)(New CriteriaByString(inStr, isVehicle))

    End Function

  Public Shared Function GetCustomerVehiclesRelationsListByCustomer(ByVal idCustomerIn As Long) As CustomerVehiclesRelationsList

    Return DataPortal.Fetch(Of CustomerVehiclesRelationsList)(New CriteriaByCustomer(idCustomerIn))

  End Function

  Public Shared Function GetCustomerVehiclesRelationsListByCustomerVehicleAndType(ByVal idCustomerIn As Long, ByVal idVehicleIn As Long, ByVal idType As Integer) As CustomerVehiclesRelationsList

    Return DataPortal.Fetch(Of CustomerVehiclesRelationsList)(New CriteriaByCustomerVehicleType(idCustomerIn, idVehicleIn, idType))

  End Function

  Public Shared Function GetCustomerVehiclesRelationsListByVehicle(ByVal idVehicleIn As Long) As CustomerVehiclesRelationsList

    Return DataPortal.Fetch(Of CustomerVehiclesRelationsList)(New CriteriaByVehicle(idVehicleIn))

  End Function

  Public Shared Function GetCustomerVehiclesRelationsListByTypeOfRelation(ByVal idType As Integer) As CustomerVehiclesRelationsList

    Return DataPortal.Fetch(Of CustomerVehiclesRelationsList)(New CriteriaByType(idType))

  End Function

    Public Shared Function GetCustomerVehiclesRelationsListById(ByVal idIn As Long) As CustomerVehiclesRelationsList

        Return DataPortal.Fetch(Of CustomerVehiclesRelationsList)(New CriteriaById(idIn))

    End Function

  Public Shared Function GetCustomerVehiclesRelationsListByTrafficLicenceList(ByVal inIdRelationType As Integer) As CustomerVehiclesRelationsList
    Return DataPortal.Fetch(Of CustomerVehiclesRelationsList)(New CriteriaByIdRelationType(inIdRelationType))
  End Function

  Private Sub New()
    ' require use of factory methods
        'AddHandler CustomerVehiclesRelations.CustomerVehiclesRelationsSaved, AddressOf CustomerVehiclesRelations_saved
        'AddHandler CustomerVehiclesRelation.CustomerVehiclesRelationSaved, AddressOf CustomerVehiclesRelations_saved
        'AddHandler Customers.CustomersSaved, AddressOf CustomerVehiclesRelations_saved
        'AddHandler Customer.CustomerSaved, AddressOf CustomerVehiclesRelations_saved
        ' AddHandler Vehicle.VehicleSaved, AddressOf CustomerVehiclesRelations_saved
  End Sub

  Private Sub CustomerVehiclesRelations_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
    IsReadOnly = False
    Me.Clear()
    IsReadOnly = True
    DataPortal_Fetch()
    Me.ResetBindings()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  <Serializable()> _
Private Class CriteriaByCustomer
    Private _idCustomer As Long

    Public ReadOnly Property IdCustomer() As Long
      Get
        Return _idCustomer
      End Get
    End Property

    Public Sub New(ByVal idCustomer As Long)
      _idCustomer = idCustomer
    End Sub
    End Class

    <Serializable()> _
Private Class CriteriaById
        Private _idIn As Integer

        Public ReadOnly Property IdIn() As Integer
            Get
                Return _idIn
            End Get
        End Property

        Public Sub New(ByVal idIn As Integer)
            _idIn = idIn
        End Sub
    End Class

    <Serializable()> _
Private Class CriteriaByString
        Private _inStr As String
        Private _isVehicle As Boolean

        Public ReadOnly Property InString() As String
            Get
                Return _inStr
            End Get
        End Property
        Public ReadOnly Property isVehicle() As Boolean
            Get
                Return _isVehicle
            End Get
        End Property

        Public Sub New(ByVal inStr As String, ByVal isVehicle As Boolean)
            _inStr = inStr
            _isVehicle = isVehicle
        End Sub
    End Class
  <Serializable()> _
Private Class CriteriaByCustomerVehicleType
    Private _idCustomer As Long
    Private _idVehicle As Long
    Private _idRelationType As Integer

    Public ReadOnly Property IdRelationType() As Integer
      Get
        Return _idRelationType
      End Get
    End Property
    Public ReadOnly Property IdVehicle() As Long
      Get
        Return _idVehicle
      End Get
    End Property
    Public ReadOnly Property IdCustomer() As Long
      Get
        Return _idCustomer
      End Get
    End Property

    Public Sub New(ByVal idCustomer As Long, ByVal idVehicle As Long, ByVal idRelationType As Integer)
      _idCustomer = idCustomer
      _idVehicle = idVehicle
      _idRelationType = idRelationType
    End Sub
  End Class

  <Serializable()> _
Private Class CriteriaByVehicle
    Private _idVehicle As Long

    Public ReadOnly Property IdVehicle() As Long
      Get
        Return _idVehicle
      End Get
    End Property

    Public Sub New(ByVal idVehicle As Long)
      _idVehicle = idVehicle
    End Sub
  End Class

  <Serializable()> _
Private Class CriteriaByIdRelationType
    Private _idRelationType As Integer

    Public ReadOnly Property IdRelationType() As Integer
      Get
        Return _idRelationType
      End Get
    End Property

    Public Sub New(ByVal idRelationType As Integer)
      _idRelationType = idRelationType
    End Sub
  End Class

  <Serializable()> _
Private Class CriteriaByType
    Private _idType As Integer

    Public ReadOnly Property IdType() As Integer
      Get
        Return _idType
      End Get
    End Property

    Public Sub New(ByVal idType As Integer)
      _idType = idType
    End Sub
  End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CustomerVehiclesRelationsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CustomerVehiclesRelationsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByString)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("CustomerVehiclesRelationsInfo.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    If criteria.isVehicle Then
                        cm.CommandText = SpZemiSiteByStringVehicle
                    Else
                        cm.CommandText = SpZemiSiteByStringCustomer
                    End If
                    cm.Parameters.AddWithValue("@str", criteria.InString)
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New CustomerVehiclesRelationsInfo(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
            Throw New DbCslaException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByCustomer)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CustomerVehiclesRelationsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getCustomerVehiclesRelationsByCustomer"
          cm.Parameters.AddWithValue("@idCustomer", criteria.IdCustomer)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CustomerVehiclesRelationsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByVehicle)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CustomerVehiclesRelationsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getCustomerVehiclesRelationsByVehicle"
          cm.Parameters.AddWithValue("@idVehicle", criteria.IdVehicle)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CustomerVehiclesRelationsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByIdRelationType)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CustomerVehiclesRelationsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getCustomerVehiclesRelationsMinusTrafficLicence"
          cm.Parameters.AddWithValue("@IdRlationType", criteria.IdRelationType)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CustomerVehiclesRelationsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub


  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByType)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CustomerVehiclesRelationsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getCustomerVehiclesRelationListByType"
          cm.Parameters.AddWithValue("@IdRelationType", criteria.IdType)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CustomerVehiclesRelationsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub


  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByCustomerVehicleType)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("CustomerVehiclesRelationsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiPoCustomerVehicleAndType
          cm.Parameters.AddWithValue("@IdCustomer", criteria.IdCustomer)
          cm.Parameters.AddWithValue("@IdVehicle", criteria.IdVehicle)
          cm.Parameters.AddWithValue("@IdRelationType", criteria.IdRelationType)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New CustomerVehiclesRelationsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaById)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("CustomerVehiclesRelationsInfo.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "getCustomerVehiclesRelationsListById"
                    cm.Parameters.AddWithValue("@id", criteria.IdIn)
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New CustomerVehiclesRelationsInfo(dr)
                            Me.Add(Info)

                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
            Throw New DbCslaException("CustomerVehiclesRelationsInfo.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
#End Region ' Data Access
End Class