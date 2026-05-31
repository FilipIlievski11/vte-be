
<Serializable()> _
Public Class DocumentType
  Inherits Csla.BusinessBase(Of DocumentType)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentTypeByID"
  Private Const spGetAll As String = "GetDocumentTypes"
  Private Const spUpdate As String = "updateDocumentType"
  Private Const spAdd As String = "addDocumentType"
  Private Const spDelete As String = "deleteDocumentType"
  Private Const spGetChildrenOptions As String = "getDocumentTypesOptionByIdDocumentTypes"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentType), New PropertyInfo(Of Integer)("Id"))
  Private Shared DocumentTypeNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentType), New PropertyInfo(Of String)("DocumentTypeName"))
  Private Shared IsBidirectionalProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentType), New PropertyInfo(Of Boolean)("IsBidirectional"))
  Private Shared IsVehiceRequiredProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentType), New PropertyInfo(Of Boolean)("IsVehiceRequired"))
  Private Shared IsTechnicalExamRequiredProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentType), New PropertyInfo(Of Boolean)("IsTechnicalExamRequired"))
  Private Shared IsPayRequiredProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentType), New PropertyInfo(Of Boolean)("IsPayRequired"))
  Private Shared IdDocumentTypePrintProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentType), New PropertyInfo(Of Integer)("IdDocumentTypePrint"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property DocumentTypeName() As String
    Get
      Return GetProperty(Of String)(DocumentTypeNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(DocumentTypeNameProperty, value)
    End Set
  End Property
  Public Property IsBidirectional() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsBidirectionalProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsBidirectionalProperty, value)
    End Set
  End Property
  Public Property IsVehiceRequired() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsVehiceRequiredProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsVehiceRequiredProperty, value)
    End Set
  End Property
  Public Property IsTechnicalExamRequired() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsTechnicalExamRequiredProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsTechnicalExamRequiredProperty, value)
    End Set
  End Property
  Public Property IsPayRequired() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsPayRequiredProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsPayRequiredProperty, value)
    End Set
  End Property
  Public Property IdDocumentTypePrint() As Integer
    Get
      Return GetProperty(Of Integer)(IdDocumentTypePrintProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdDocumentTypePrintProperty, value)
    End Set
  End Property

  Private Shared DocumentTypesOptionsProperty As PropertyInfo(Of DocumentTypesOptions) = _
RegisterProperty(Of DocumentTypesOptions)(GetType(DocumentType), New PropertyInfo(Of DocumentTypesOptions)("DocumentTypesOptions"))

  Public ReadOnly Property DocumentTypesOptions() As DocumentTypesOptions
    Get
      If Not FieldManager.FieldExists(DocumentTypesOptionsProperty) Then
        SetProperty(Of DocumentTypesOptions) _
        (DocumentTypesOptionsProperty, DocumentTypesOptions.NewDocumentTypesOptions)
      End If
      Return GetProperty(Of DocumentTypesOptions)(DocumentTypesOptionsProperty)
    End Get
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DocumentTypeName") Then
      AuthorizationRules.AllowWrite("DocumentTypeName", roleName)
    Else
      AuthorizationRules.DenyWrite("DocumentTypeName", roleName)
    End If
    'AuthorizationRules.AllowWrite("DocumentTypeName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsBidirectional") Then
      AuthorizationRules.AllowWrite("IsBidirectional", roleName)
    Else
      AuthorizationRules.DenyWrite("IsBidirectional", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsBidirectional")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsVehiceRequired") Then
      AuthorizationRules.AllowWrite("IsVehiceRequired", roleName)
    Else
      AuthorizationRules.DenyWrite("IsVehiceRequired", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsVehiceRequired")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsTechnicalExamRequired") Then
      AuthorizationRules.AllowWrite("IsTechnicalExamRequired", roleName)
    Else
      AuthorizationRules.DenyWrite("IsTechnicalExamRequired", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsTechnicalExamRequired")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsPayRequired") Then
      AuthorizationRules.AllowWrite("IsPayRequired", roleName)
    Else
      AuthorizationRules.DenyWrite("IsPayRequired", roleName)
    End If
    'AuthorizationRules.AllowWrite("IsPayRequired")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentType")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentType")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentType")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentType")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' DocumentTypeNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, DocumentTypeNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(DocumentTypeNameProperty, 50))
    'Me.BrokenRulesCollection(0).
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewDocumentType() As DocumentType
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a DocumentType")
    End If
    Return DataPortal.Create(Of DocumentType)()
  End Function

  Public Shared Function GetDocumentType(ByVal id As Integer) As DocumentType
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a DocumentType")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of DocumentType, Integer)(Id))
  End Function

  Public Shared Sub DeleteDocumentType(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a DocumentType")
    End If
    DataPortal.Delete(New SingleCriteria(Of DocumentType, Integer)(Id))
  End Sub

  Public Overrides Function Save() As DocumentType
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a DocumentType")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a DocumentType")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a DocumentType")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewDocumentTypeChild() As DocumentType
    Return DataPortal.CreateChild(Of DocumentType)()
  End Function

  Friend Shared Function GetDocumentType(ByVal dr As SafeDataReader) As DocumentType
    Return DataPortal.FetchChild(Of DocumentType)(dr)
  End Function

#End Region 'Child Factory Methods


#Region " Data Access "

#Region " Data Access - Create "

  <RunLocal()> _
  Private Overloads Sub DataPortal_Create()
    ValidationRules.CheckRules()
  End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of DocumentType, Integer))
    Database.LogInfo("DocumentType.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(DocumentTypeNameProperty, dr.GetString("DocumentTypeName"))
            LoadProperty(Of Boolean)(IsBidirectionalProperty, dr.GetBoolean("IsBidirectional"))
            LoadProperty(Of Boolean)(IsVehiceRequiredProperty, dr.GetBoolean("IsVehiceRequired"))
            LoadProperty(Of Boolean)(IsTechnicalExamRequiredProperty, dr.GetBoolean("IsTechnicalExamRequired"))
            LoadProperty(Of Boolean)(IsPayRequiredProperty, dr.GetBoolean("IsPayRequired"))
            LoadProperty(Of Integer)(IdDocumentTypePrintProperty, dr.GetInt32("IdDocumentTypePrint"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildrenOptions
          cm1.Parameters.AddWithValue("@IdDocumentTypes", ReadProperty(Of Integer)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of DocumentTypesOptions) _
            (DocumentTypesOptionsProperty, DocumentTypesOptions.GetDocumentTypesOptions(drc))
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DocumentType.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentType.DataPortal_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Protected Overrides Sub DataPortal_Insert()
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spAdd

            .Parameters.AddWithValue("@DocumentTypeName", ReadProperty(Of String)(DocumentTypeNameProperty))
            .Parameters.AddWithValue("@IsBidirectional", ReadProperty(Of Boolean)(IsBidirectionalProperty))
            .Parameters.AddWithValue("@IsVehiceRequired", ReadProperty(Of Boolean)(IsVehiceRequiredProperty))
            .Parameters.AddWithValue("@IsTechnicalExamRequired", ReadProperty(Of Boolean)(IsTechnicalExamRequiredProperty))
            .Parameters.AddWithValue("@IsPayRequired", ReadProperty(Of Boolean)(IsPayRequiredProperty))
            .Parameters.AddWithValue("@IdDocumentTypePrint", ReadProperty(Of Integer)(IdDocumentTypePrintProperty))

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

        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("DocumentType.DataPortal_Insert", ex)
      Throw New DbCslaException("DocumentType.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("DocumentType.DataPortal_Insert", GetHashCode())
    End Try
  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Protected Overrides Sub DataPortal_Update()
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@DocumentTypeName", ReadProperty(Of String)(DocumentTypeNameProperty))
            .Parameters.AddWithValue("@IsBidirectional", ReadProperty(Of Boolean)(IsBidirectionalProperty))
            .Parameters.AddWithValue("@IsVehiceRequired", ReadProperty(Of Boolean)(IsVehiceRequiredProperty))
            .Parameters.AddWithValue("@IsTechnicalExamRequired", ReadProperty(Of Boolean)(IsTechnicalExamRequiredProperty))
            .Parameters.AddWithValue("@IsPayRequired", ReadProperty(Of Boolean)(IsPayRequiredProperty))
            .Parameters.AddWithValue("@IdDocumentTypePrint", ReadProperty(Of Integer)(IdDocumentTypePrintProperty))
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

        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("Employee.DataPortal_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DBConcurrencyException("Employee.DataPortal_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Protected Overrides Sub DataPortal_DeleteSelf()
    DataPortal_Delete(New SingleCriteria(Of DocumentType, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of DocumentType, Integer))
    Database.LogInfo("DocumentType.DataPortal_Delete", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spDelete
            .Parameters.AddWithValue("@id", criteria.Value)
            .ExecuteNonQuery()
          End With
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentType.DataPortal_Delete", ex)
      Throw New DbCslaException("DocumentType.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("DocumentType.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(DocumentTypeNameProperty, dr.GetString("DocumentTypeName"))
      LoadProperty(Of Boolean)(IsBidirectionalProperty, dr.GetBoolean("IsBidirectional"))
      LoadProperty(Of Boolean)(IsVehiceRequiredProperty, dr.GetBoolean("IsVehiceRequired"))
      LoadProperty(Of Boolean)(IsTechnicalExamRequiredProperty, dr.GetBoolean("IsTechnicalExamRequired"))
      LoadProperty(Of Boolean)(IsPayRequiredProperty, dr.GetBoolean("IsPayRequired"))
      LoadProperty(Of Integer)(IdDocumentTypePrintProperty, dr.GetInt32("IdDocumentTypePrint"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildrenOptions
          cm1.Parameters.AddWithValue("@IdDocumentTypes", ReadProperty(Of Integer)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of DocumentTypesOptions) _
            (DocumentTypesOptionsProperty, DocumentTypesOptions.GetDocumentTypesOptions(drc))
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentType.Child_Fetch", ex)
      Throw New DbCslaException("DocumentType.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert()
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spAdd
            .Parameters.AddWithValue("@DocumentTypeName", ReadProperty(Of String)(DocumentTypeNameProperty))
            .Parameters.AddWithValue("@IsBidirectional", ReadProperty(Of Boolean)(IsBidirectionalProperty))
            .Parameters.AddWithValue("@IsVehiceRequired", ReadProperty(Of Boolean)(IsVehiceRequiredProperty))
            .Parameters.AddWithValue("@IsTechnicalExamRequired", ReadProperty(Of Boolean)(IsTechnicalExamRequiredProperty))
            .Parameters.AddWithValue("@IsPayRequired", ReadProperty(Of Boolean)(IsPayRequiredProperty))
            .Parameters.AddWithValue("@IdDocumentTypePrint", ReadProperty(Of Integer)(IdDocumentTypePrintProperty))

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

        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("DocumentType.Child_Insert", ex)
      Throw New DbCslaException("DocumentType.Child_Insert", ex)
    Finally
      Database.LogInfo("DocumentType.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("DocumentType.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@DocumentTypeName", ReadProperty(Of String)(DocumentTypeNameProperty))
            .Parameters.AddWithValue("@IsBidirectional", ReadProperty(Of Boolean)(IsBidirectionalProperty))
            .Parameters.AddWithValue("@IsVehiceRequired", ReadProperty(Of Boolean)(IsVehiceRequiredProperty))
            .Parameters.AddWithValue("@IsTechnicalExamRequired", ReadProperty(Of Boolean)(IsTechnicalExamRequiredProperty))
            .Parameters.AddWithValue("@IsPayRequired", ReadProperty(Of Boolean)(IsPayRequiredProperty))
            .Parameters.AddWithValue("@IdDocumentTypePrint", ReadProperty(Of Integer)(IdDocumentTypePrintProperty))
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

        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("DocumentType.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("DocumentType.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("DocumentType.Child_DeleteSelf", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
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
      Database.LogException("DocumentType.Child_Fetch", ex)
      Throw New DbCslaException("DocumentType.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
