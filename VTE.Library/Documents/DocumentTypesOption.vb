


<Serializable()> _
Public Class DocumentTypesOption
  Inherits Csla.BusinessBase(Of DocumentTypesOption)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentTypesOptionByID"
  Private Const spGetAll As String = "GetDocumentTypesOptions"
  Private Const spUpdate As String = "updateDocumentTypesOption"
  Private Const spAdd As String = "addDocumentTypesOption"
  Private Const spDelete As String = "deleteDocumentTypesOption"
  Private Const spChildrenDetails As String = "getDocumentTypesOptionsDetailByIdDocumentTypesOptions"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentTypesOption), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdDocumentTypesProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentTypesOption), New PropertyInfo(Of Integer)("IdDocumentTypes"))
  Private Shared OptionNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentTypesOption), New PropertyInfo(Of String)("OptionName"))
  Private Shared IsNewRegistrationProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentTypesOption), New PropertyInfo(Of Boolean)("IsNewRegistration"))
  Private Shared IsTehnicalExamRquiredProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentTypesOption), New PropertyInfo(Of Boolean)("IsTehnicalExamRquired"))
  Private Shared RelationDeletedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentTypesOption), New PropertyInfo(Of Boolean)("RelationDeleted"))
  Private Shared VehicleDeletedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentTypesOption), New PropertyInfo(Of Boolean)("VehicleDeleted"))
  Private Shared DetailsProperty As PropertyInfo(Of DocumentTypesOptionDetails) = RegisterProperty(Of DocumentTypesOptionDetails)(GetType(DocumentTypesOption), New PropertyInfo(Of DocumentTypesOptionDetails)("DocumentTypesOptionDetails"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property

  Public ReadOnly Property Details() As DocumentTypesOptionDetails
    Get
      If Not FieldManager.FieldExists(DetailsProperty) Then
        SetProperty(Of DocumentTypesOptionDetails) _
        (DetailsProperty, DocumentTypesOptionDetails.NewDocumentTypesOptionDetails)
      End If
      Return GetProperty(Of DocumentTypesOptionDetails)(DetailsProperty)
    End Get
  End Property
  Public Property IdDocumentTypes() As Integer
    Get
      Return GetProperty(Of Integer)(IdDocumentTypesProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdDocumentTypesProperty, value)
    End Set
  End Property
  Public Property OptionName() As String
    Get
      Return GetProperty(Of String)(OptionNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(OptionNameProperty, value)
    End Set
  End Property
  Public Property IsNewRegistration() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsNewRegistrationProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsNewRegistrationProperty, value)
    End Set
  End Property
  Public Property IsTehnicalExamRquired() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsTehnicalExamRquiredProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsTehnicalExamRquiredProperty, value)
    End Set
  End Property
  Public Property RelationDeleted() As Boolean
    Get
      Return GetProperty(Of Boolean)(RelationDeletedProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(RelationDeletedProperty, value)
    End Set
  End Property
  Public Property VehicleDeleted() As Boolean
    Get
      Return GetProperty(Of Boolean)(VehicleDeletedProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(VehicleDeletedProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdDocumentTypes") Then
            AuthorizationRules.AllowWrite("IdDocumentTypes", roleName)
        Else
            AuthorizationRules.DenyWrite("IdDocumentTypes", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("OptionName") Then
            AuthorizationRules.AllowWrite("OptionName", roleName)
        Else
            AuthorizationRules.DenyWrite("OptionName", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsNewRegistration") Then
            AuthorizationRules.AllowWrite("IsNewRegistration", roleName)
        Else
            AuthorizationRules.DenyWrite("IsNewRegistration", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsTehnicalExamRquired") Then
            AuthorizationRules.AllowWrite("IsTehnicalExamRquired", roleName)
        Else
            AuthorizationRules.DenyWrite("IsTehnicalExamRquired", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("RelationDeleted") Then
            AuthorizationRules.AllowWrite("RelationDeleted", roleName)
        Else
            AuthorizationRules.DenyWrite("RelationDeleted", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("VehicleDeleted") Then
            AuthorizationRules.AllowWrite("VehicleDeleted", roleName)
        Else
            AuthorizationRules.DenyWrite("VehicleDeleted", roleName)
        End If
    End Sub



    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentTypesOption")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentTypesOption")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentTypesOption")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentTypesOption")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' OptionNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, OptionNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(OptionNameProperty, 500))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

    Friend Shared Function NewDocumentTypesOptionChild() As DocumentTypesOption
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User Not authorized to add a DocumentTypesOption")
        End If
        Return DataPortal.CreateChild(Of DocumentTypesOption)()
    End Function

  Friend Shared Function GetDocumentTypesOption(ByVal dr As SafeDataReader) As DocumentTypesOption
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User Not authorized to view a DocumentTypesOption")
        End If
        Return DataPortal.FetchChild(Of DocumentTypesOption)(dr)
  End Function

    Public Shared Sub DeleteDocumentTypesOption(ByVal id As Integer)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a DocumentTypeOption")
        End If
        DataPortal.Delete(New SingleCriteria(Of DocumentTypesOption, Integer)(id))
    End Sub

    Public Overrides Function Save() As DocumentTypesOption
        If IsDeleted AndAlso Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a DocumentTypesOption")
        ElseIf IsNew AndAlso Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a DocumentTypesOption")
        ElseIf Not CanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a DocumentTypesOption")
        End If
        Return MyBase.Save()
    End Function

  Private Sub New()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Create "
  Protected Overloads Sub Child_Create()
  End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("DocumentTypesOption.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdDocumentTypesProperty, dr.GetInt32("IdDocumentTypes"))
      LoadProperty(Of String)(OptionNameProperty, dr.GetString("OptionName"))
      LoadProperty(Of Boolean)(IsNewRegistrationProperty, dr.GetBoolean("IsNewRegistration"))
      LoadProperty(Of Boolean)(IsTehnicalExamRquiredProperty, dr.GetBoolean("IsTehnicalExamRquired"))
      LoadProperty(Of Boolean)(RelationDeletedProperty, dr.GetBoolean("RelationDeleted"))
      LoadProperty(Of Boolean)(VehicleDeletedProperty, dr.GetBoolean("VehicleDeleted"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spChildrenDetails
          cm1.Parameters.AddWithValue("@IdDocumentTypesOptions", ReadProperty(Of Integer)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of DocumentTypesOptionDetails) _
            (DetailsProperty, DocumentTypesOptionDetails.GetDocumentTypesOptionDetails(drc))
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentTypesOption.Child_Fetch", ex)
      Throw New DbCslaException("DocumentTypesOption.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As DocumentType)
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
            .Parameters.AddWithValue("@IdDocumentTypes", parent.Id)
            .Parameters.AddWithValue("@OptionName", ReadProperty(Of String)(OptionNameProperty))
            .Parameters.AddWithValue("@IsNewRegistration", ReadProperty(Of Boolean)(IsNewRegistrationProperty))
            .Parameters.AddWithValue("@IsTehnicalExamRquired", ReadProperty(Of Boolean)(IsTehnicalExamRquiredProperty))
            .Parameters.AddWithValue("@RelationDeleted", ReadProperty(Of Boolean)(RelationDeletedProperty))
            .Parameters.AddWithValue("@VehicleDeleted", ReadProperty(Of Boolean)(VehicleDeletedProperty))

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
        FieldManager.UpdateChildren(Me)
      End Using
    Catch ex As Exception
      Database.LogException("DocumentTypesOption.Child_Insert", ex)
      Throw New DbCslaException("DocumentTypesOption.Child_Insert", ex)
    Finally
      Database.LogInfo("DocumentTypesOption.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As DocumentType)
    Database.LogInfo("DocumentTypesOption.Child_Update", GetHashCode)
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
            .Parameters.AddWithValue("@IdDocumentTypes", parent.Id)
            .Parameters.AddWithValue("@OptionName", ReadProperty(Of String)(OptionNameProperty))
            .Parameters.AddWithValue("@IsNewRegistration", ReadProperty(Of Boolean)(IsNewRegistrationProperty))
            .Parameters.AddWithValue("@IsTehnicalExamRquired", ReadProperty(Of Boolean)(IsTehnicalExamRquiredProperty))
            .Parameters.AddWithValue("@RelationDeleted", ReadProperty(Of Boolean)(RelationDeletedProperty))
            .Parameters.AddWithValue("@VehicleDeleted", ReadProperty(Of Boolean)(VehicleDeletedProperty))
            .Parameters.AddWithValue("@lastChanged", _lastChanged)
            Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
        FieldManager.UpdateChildren(Me)
      End Using
    Catch ex As Exception
      Database.LogException("DocumentTypesOption.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("DocumentTypesOption.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("DocumentTypesOption.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("DocumentTypesOption.Child_Fetch", ex)
      Throw New DbCslaException("DocumentTypesOption.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
