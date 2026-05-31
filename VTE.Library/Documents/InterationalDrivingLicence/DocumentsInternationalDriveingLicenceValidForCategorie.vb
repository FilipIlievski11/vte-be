
<Serializable()> _
Public Class DocumentsInternationalDriveingLicenceValidForCategorie
  Inherits Csla.BusinessBase(Of DocumentsInternationalDriveingLicenceValidForCategorie)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentsInternationalDriveingLicencesValidForCategorieByID"
  Private Const spGetAll As String = "GetDocumentsInternationalDriveingLicencesValidForCategories"
  Private Const spUpdate As String = "updateDocumentsInternationalDriveingLicencesValidForCategorie"
  Private Const spAdd As String = "addDocumentsInternationalDriveingLicencesValidForCategorie"
  Private Const spDelete As String = "deleteDocumentsInternationalDriveingLicencesValidForCategorie"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsInternationalDriveingLicenceValidForCategorie), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdInternationalDrivingLicenceProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsInternationalDriveingLicenceValidForCategorie), New PropertyInfo(Of Long)("IdInternationalDrivingLicence"))
  Private Shared IdLicenceCategorieProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsInternationalDriveingLicenceValidForCategorie), New PropertyInfo(Of Integer)("IdLicenceCategorie"))
  Private Shared IsCheckProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentsInternationalDriveingLicenceValidForCategorie), New PropertyInfo(Of Boolean)("IsCheck"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdInternationalDrivingLicence() As Long
    Get
      Return GetProperty(Of Long)(IdInternationalDrivingLicenceProperty)
    End Get
    Set(ByVal value As Long)
      SetProperty(Of Long)(IdInternationalDrivingLicenceProperty, value)
    End Set
  End Property
  Public Property IdLicenceCategorie() As Integer
    Get
      Return GetProperty(Of Integer)(IdLicenceCategorieProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdLicenceCategorieProperty, value)
    End Set
  End Property
  Public Property IsCheck() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsCheckProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsCheckProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdInternationalDrivingLicence") Then
            AuthorizationRules.AllowWrite("IdInternationalDrivingLicence", roleName)
        Else
            AuthorizationRules.DenyWrite("IdInternationalDrivingLicence", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdLicenceCategorie") Then
            AuthorizationRules.AllowWrite("IdLicenceCategorie", roleName)
        Else
            AuthorizationRules.DenyWrite("IdLicenceCategorie", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsCheck") Then
            AuthorizationRules.AllowWrite("IsCheck", roleName)
        Else
            AuthorizationRules.DenyWrite("IsCheck", roleName)
        End If
        
    End Sub


    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsInternationalDriveingLicenceValidForCategorie")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsInternationalDriveingLicenceValidForCategorie")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsInternationalDriveingLicenceValidForCategorie")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsInternationalDriveingLicenceValidForCategorie")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                            New Validation.IntegerMinValueRuleArgs(IdLicenceCategorieProperty, 1))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewDocumentsInternationalDriveingLicenceValidForCategorieChild() As DocumentsInternationalDriveingLicenceValidForCategorie
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a DocumentsInternationalDriveingLicenceValidForCategorie")
        'End If
        Return DataPortal.CreateChild(Of DocumentsInternationalDriveingLicenceValidForCategorie)()
  End Function

    Friend Shared Function GetDocumentsInternationalDriveingLicenceValidForCategorie(ByVal dr As SafeDataReader) As DocumentsInternationalDriveingLicenceValidForCategorie
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a DocumentsInternationalDriveingLicenceValidForCategorie")
        'End If
        Return DataPortal.FetchChild(Of DocumentsInternationalDriveingLicenceValidForCategorie)(dr)
    End Function
  Private Sub New()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Create "
  Protected Overloads Sub Child_Create()
    ValidationRules.CheckRules()
  End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("DocumentsInternationalDriveingLicenceValidForCategorie.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Long)(IdInternationalDrivingLicenceProperty, dr.GetInt64("IdInternationalDrivingLicence"))
      LoadProperty(Of Integer)(IdLicenceCategorieProperty, dr.GetInt32("IdLicenceCategorie"))
      LoadProperty(Of Boolean)(IsCheckProperty, dr.GetBoolean("IsCheck"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("DocumentsInternationalDriveingLicenceValidForCategorie.Child_Fetch", ex)
      Throw New DbCslaException("DocumentsInternationalDriveingLicenceValidForCategorie.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As DocumentsInternationalDriveingLicence)
    Try
      Dim cn As SqlConnection = ApplicationContext.LocalContext("cn")
      If cn Is Nothing Then cn = Database.VTE_SqlConnection
      If cn.State = ConnectionState.Closed Then
        cn.ConnectionString = Database.VTEConnection
        cn.Open()
      End If
      Using cm As SqlCommand = cn.CreateCommand
        With cm
          .CommandType = CommandType.StoredProcedure
          .CommandText = spAdd
          'Smeni go Id so Parent.Id
          .Parameters.AddWithValue("@IdInternationalDrivingLicence", parent.Id)
          .Parameters.AddWithValue("@IdLicenceCategorie", ReadProperty(Of Integer)(IdLicenceCategorieProperty))
          .Parameters.AddWithValue("@IsCheck", ReadProperty(Of Boolean)(IsCheckProperty))

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

    Catch ex As Exception
      Database.LogException("DocumentsInternationalDriveingLicenceValidForCategorie.Child_Insert", ex)
      Throw New DbCslaException("DocumentsInternationalDriveingLicenceValidForCategorie.Child_Insert", ex)
    Finally
      Database.LogInfo("DocumentsInternationalDriveingLicenceValidForCategorie.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As DocumentsInternationalDriveingLicence)
    Database.LogInfo("DocumentsInternationalDriveingLicenceValidForCategorie.Child_Update", GetHashCode)
    Try
      Dim cn As SqlConnection = ApplicationContext.LocalContext("cn")
      If cn Is Nothing Then cn = Database.VTE_SqlConnection
      If cn.State = ConnectionState.Closed Then
        cn.ConnectionString = Database.VTEConnection
        cn.Open()
      End If
      Using cm As SqlCommand = cn.CreateCommand
        With cm
          .CommandType = CommandType.StoredProcedure
          .CommandText = spUpdate

          .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
          .Parameters.AddWithValue("@IdInternationalDrivingLicence", parent.Id)
          .Parameters.AddWithValue("@IdLicenceCategorie", ReadProperty(Of Integer)(IdLicenceCategorieProperty))
          .Parameters.AddWithValue("@IsCheck", ReadProperty(Of Boolean)(IsCheckProperty))
          .Parameters.AddWithValue("@lastChanged", _lastChanged)
          Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
          param.Direction = ParameterDirection.Output
          .Parameters.Add(param)

          .ExecuteNonQuery()

          _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
        End With
      End Using
      'update child objects

    Catch ex As Exception
      Database.LogException("DocumentsInternationalDriveingLicenceValidForCategorie.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("DocumentsInternationalDriveingLicenceValidForCategorie.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("DocumentsInternationalDriveingLicenceValidForCategorie.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("DocumentsInternationalDriveingLicenceValidForCategorie.Child_Fetch", ex)
      Throw New DbCslaException("DocumentsInternationalDriveingLicenceValidForCategorie.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
