
<Serializable()> _
Public Class TrafficLicencesExtension
  Inherits Csla.BusinessBase(Of TrafficLicencesExtension)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentsTrafficLicencesExtensionByID"
  Private Const spGetAll As String = "GetDocumentsTrafficLicencesExtensions"
  Private Const spUpdate As String = "updateDocumentsTrafficLicencesExtension"
  Private Const spAdd As String = "addDocumentsTrafficLicencesExtension"
  Private Const spDelete As String = "deleteDocumentsTrafficLicencesExtension"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(TrafficLicencesExtension), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdTrafficLicenceProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(TrafficLicencesExtension), New PropertyInfo(Of Integer)("IdTrafficLicence"))
  Private Shared IdOperatorProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(TrafficLicencesExtension), New PropertyInfo(Of Integer)("IdOperator"))
  Private Shared ValidTillProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(TrafficLicencesExtension), New PropertyInfo(Of SmartDate)("ValidTill", New SmartDate(Now.AddYears(1).Date, True)))
  Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(TrafficLicencesExtension), New PropertyInfo(Of String)("Note"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdTrafficLicence() As Integer
    Get
      Return GetProperty(Of Integer)(IdTrafficLicenceProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdTrafficLicenceProperty, value)
    End Set
  End Property
  Public Property IdOperator() As Integer
    Get
      Return GetProperty(Of Integer)(IdOperatorProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdOperatorProperty, value)
    End Set
  End Property
  Public Property ValidTill() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(ValidTillProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(ValidTillProperty, value)
    End Set
  End Property
  Public Property Note() As String
    Get
      Return GetProperty(Of String)(NoteProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(NoteProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdTrafficLicence") Then
            AuthorizationRules.AllowWrite("IdTrafficLicence", roleName)
        Else
            AuthorizationRules.DenyWrite("IdTrafficLicence", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdOperator") Then
            AuthorizationRules.AllowWrite("IdOperator", roleName)
        Else
            AuthorizationRules.DenyWrite("IdOperator", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ValidTill") Then
            AuthorizationRules.AllowWrite("ValidTill", roleName)
        Else
            AuthorizationRules.DenyWrite("ValidTill", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Note") Then
            AuthorizationRules.AllowWrite("Note", roleName)
        Else
            AuthorizationRules.DenyWrite("Note", roleName)
        End If
    End Sub
    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("TrafficLicencesExtension")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("TrafficLicencesExtension")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("TrafficLicencesExtension")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("TrafficLicencesExtension")
    End Function
#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' ValidTillProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ValidTillProperty)
    ' NoteProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NoteProperty, 250))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewTrafficLicencesExtensionChild() As TrafficLicencesExtension
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a TrafficLicencesExtension")
        'End If
        Return DataPortal.CreateChild(Of TrafficLicencesExtension)()
  End Function

  Friend Shared Function GetTrafficLicencesExtension(ByVal dr As SafeDataReader) As TrafficLicencesExtension
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a TrafficLicencesExtension")
        'End If
        Return DataPortal.FetchChild(Of TrafficLicencesExtension)(dr)
  End Function

  Private Sub New()
  End Sub
    Public Shared Sub DeleteRegistrationIssuer(ByVal id As Integer)
        'If Not CanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a TrafficLicencesExtension")
        'End If
        DataPortal.Delete(New SingleCriteria(Of TrafficLicencesExtension, Integer)(id))
    End Sub

    Public Overrides Function Save() As TrafficLicencesExtension
        'If IsDeleted AndAlso Not CanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a TrafficLicencesExtension")
        'ElseIf IsNew AndAlso Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a TrafficLicencesExtension")
        'ElseIf Not CanEditObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to update a TrafficLicencesExtension")
        'End If
        Return MyBase.Save()
    End Function
#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Create "
  Protected Overloads Sub Child_Create()
    SetProperty(Of SmartDate, Date)(ValidTillProperty, Now.AddYears(1).Date)
    ValidationRules.CheckRules()
  End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("TrafficLicencesExtension.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdTrafficLicenceProperty, dr.GetInt32("IdTrafficLicence"))
      LoadProperty(Of Integer)(IdOperatorProperty, dr.GetInt32("IdOperator"))
      LoadProperty(Of SmartDate, Date?)(ValidTillProperty, dr.GetSmartDate("ValidTill", True))
      LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("TrafficLicencesExtension.Child_Fetch", ex)
      Throw New DbCslaException("TrafficLicencesExtension.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As DocumentsTrafficLicence)
    Try
      Using cn As SqlConnection = ApplicationContext.LocalContext("cn")
        If cn.State = ConnectionState.Closed Then
          cn.ConnectionString = Database.VTEConnection
          cn.Open()
        End If
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spAdd
            'Smeni go Id so Parent.Id
            .Parameters.AddWithValue("@IdTrafficLicence", parent.Id)
            .Parameters.AddWithValue("@IdOperator", ReadProperty(Of Integer)(IdOperatorProperty))
            .Parameters.AddWithValue("@ValidTill", ReadProperty(Of SmartDate)(ValidTillProperty).DBValue)
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))

            Dim param As New SqlParameter("@newId", SqlDbType.Int)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)
            param = New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            LoadProperty(Of Integer)(IdProperty, CInt(.Parameters("@newId").Value))
            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
      End Using
    Catch ex As Exception
      Database.LogException("TrafficLicencesExtension.Child_Insert", ex)
      Throw New DbCslaException("TrafficLicencesExtension.Child_Insert", ex)
    Finally
      Database.LogInfo("TrafficLicencesExtension.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As DocumentsTrafficLicence)
    Database.LogInfo("TrafficLicencesExtension.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = ApplicationContext.LocalContext("cn")
        If cn.State = ConnectionState.Closed Then
          cn.ConnectionString = Database.VTEConnection
          cn.Open()
        End If
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@IdTrafficLicence", parent.Id)
            .Parameters.AddWithValue("@IdOperator", ReadProperty(Of Integer)(IdOperatorProperty))
            .Parameters.AddWithValue("@ValidTill", ReadProperty(Of SmartDate)(ValidTillProperty).DBValue)
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
            .Parameters.AddWithValue("@lastChanged", _lastChanged)
            Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
      End Using
    Catch ex As Exception
      Database.LogException("TrafficLicencesExtension.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("TrafficLicencesExtension.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("TrafficLicencesExtension.Child_DeleteSelf", GetHashCode)
    Try
      Using cn As SqlConnection = Csla.ApplicationContext.LocalContext("cn")
        If cn.State = ConnectionState.Closed Then
          cn.ConnectionString = Database.VTEConnection
          cn.Open()
        End If
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spDelete
            .Parameters.AddWithValue("@id", ReadProperty(Of Integer)(IdProperty))
            .ExecuteNonQuery()
          End With
        End Using

      End Using
    Catch ex As Exception
      Database.LogException("TrafficLicencesExtension.Child_Fetch", ex)
      Throw New DbCslaException("TrafficLicencesExtension.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class