
<Serializable()> _
Public Class DocumentTypesOptionDetail
  Inherits Csla.BusinessBase(Of DocumentTypesOptionDetail)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentTypesOptionsDetailByID"
  Private Const spGetAll As String = "GetDocumentTypesOptionsDetails"
  Private Const spUpdate As String = "updateDocumentTypesOptionsDetail"
  Private Const spAdd As String = "addDocumentTypesOptionsDetail"
  Private Const spDelete As String = "deleteDocumentTypesOptionsDetail"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentTypesOptionDetail), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdDocumentTypesOptionsProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentTypesOptionDetail), New PropertyInfo(Of Integer)("IdDocumentTypesOptions"))
  Private Shared NameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentTypesOptionDetail), New PropertyInfo(Of String)("Name"))
  Private Shared IsVehicleDeletedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentTypesOptionDetail), New PropertyInfo(Of Boolean)("IsVehicleDeleted"))
  Private Shared IsNewCustomerProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentTypesOptionDetail), New PropertyInfo(Of Boolean)("IsNewCustomer"))
  Private Shared IsRelationDeletedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentTypesOptionDetail), New PropertyInfo(Of Boolean)("IsRelationDeleted"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdDocumentTypesOptions() As Integer
    Get
      Return GetProperty(Of Integer)(IdDocumentTypesOptionsProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdDocumentTypesOptionsProperty, value)
    End Set
  End Property
  Public Property Name() As String
    Get
      Return GetProperty(Of String)(NameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(NameProperty, value)
    End Set
  End Property
  Public Property IsVehicleDeleted() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsVehicleDeletedProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsVehicleDeletedProperty, value)
    End Set
  End Property

  Public Property IsNewCustomer() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsNewCustomerProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsNewCustomerProperty, value)
    End Set
  End Property
  Public Property IsRelationDeleted() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsRelationDeletedProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsRelationDeletedProperty, value)
    End Set
  End Property
  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdDocumentTypesOptions") Then
            AuthorizationRules.AllowWrite("IdDocumentTypesOptions", roleName)
        Else
            AuthorizationRules.DenyWrite("IdDocumentTypesOptions", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Name") Then
            AuthorizationRules.AllowWrite("Name", roleName)
        Else
            AuthorizationRules.DenyWrite("Name", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsVehicleDeleted") Then
            AuthorizationRules.AllowWrite("IsVehicleDeleted", roleName)
        Else
            AuthorizationRules.DenyWrite("IsVehicleDeleted", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsNewCustomer") Then
            AuthorizationRules.AllowWrite("IsNewCustomer", roleName)
        Else
            AuthorizationRules.DenyWrite("IsNewCustomer", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsRelationDeleted") Then
            AuthorizationRules.AllowWrite("IsRelationDeleted", roleName)
        Else
            AuthorizationRules.DenyWrite("IsRelationDeleted", roleName)
        End If

    End Sub

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentTypesOptionDetail")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentTypesOptionDetail")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentTypesOptionDetail")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentTypesOptionDetail")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' NameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, NameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NameProperty, 250))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewDocumentTypesOptionDetailChild() As DocumentTypesOptionDetail
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a DocumentTypesOptionDetail")
        End If
        Return DataPortal.CreateChild(Of DocumentTypesOptionDetail)()
  End Function

  Friend Shared Function GetDocumentTypesOptionDetail(ByVal dr As SafeDataReader) As DocumentTypesOptionDetail
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a DocumentTypesOptionDetail")
        End If
        Return DataPortal.FetchChild(Of DocumentTypesOptionDetail)(dr)
  End Function

    Public Shared Sub DeleteDocumentTypesOption(ByVal id As Integer)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a DocumentTypesOptionDetail")
        End If
        DataPortal.Delete(New SingleCriteria(Of DocumentTypesOptionDetail, Integer)(id))
    End Sub

    Public Overrides Function Save() As DocumentTypesOptionDetail
        If IsDeleted AndAlso Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a DocumentTypesOptionDetail")
        ElseIf IsNew AndAlso Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a DocumentTypesOptionDetail")
        ElseIf Not CanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a DocumentTypesOptionDetail")
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
    Database.LogInfo("DocumentTypesOptionDetail.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdDocumentTypesOptionsProperty, dr.GetInt32("IdDocumentTypesOptions"))
      LoadProperty(Of String)(NameProperty, dr.GetString("Name"))
      LoadProperty(Of Boolean)(IsVehicleDeletedProperty, dr.GetBoolean("IsVehicleDeleted"))
      LoadProperty(Of Boolean)(IsNewCustomerProperty, dr.GetBoolean("IsNewCustomer"))
      LoadProperty(Of Boolean)(IsRelationDeletedProperty, dr.GetBoolean("IsRelationDeleted"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("DocumentTypesOptionDetail.Child_Fetch", ex)
      Throw New DbCslaException("DocumentTypesOptionDetail.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As DocumentTypesOption)
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
            .Parameters.AddWithValue("@IdDocumentTypesOptions", ReadProperty(Of Integer)(IdDocumentTypesOptionsProperty))
            .Parameters.AddWithValue("@Name", ReadProperty(Of String)(NameProperty))
            .Parameters.AddWithValue("@IsVehicleDeleted", ReadProperty(Of Boolean)(IsVehicleDeletedProperty))
            .Parameters.AddWithValue("@IsNewCustomer", ReadProperty(Of Boolean)(IsNewCustomerProperty))
            .Parameters.AddWithValue("@IsRelationDeleted", ReadProperty(Of Boolean)(IsRelationDeletedProperty))

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
      Database.LogException("DocumentTypesOptionDetail.Child_Insert", ex)
      Throw New DbCslaException("DocumentTypesOptionDetail.Child_Insert", ex)
    Finally
      Database.LogInfo("DocumentTypesOptionDetail.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As DocumentTypesOption)
    Database.LogInfo("DocumentTypesOptionDetail.Child_Update", GetHashCode)
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
            .Parameters.AddWithValue("@IdDocumentTypesOptions", ReadProperty(Of Integer)(IdDocumentTypesOptionsProperty))
            .Parameters.AddWithValue("@Name", ReadProperty(Of String)(NameProperty))
            .Parameters.AddWithValue("@IsVehicleDeleted", ReadProperty(Of Boolean)(IsVehicleDeletedProperty))
            .Parameters.AddWithValue("@IsNewCustomer", ReadProperty(Of Boolean)(IsNewCustomerProperty))
            .Parameters.AddWithValue("@IsRelationDeleted", ReadProperty(Of Boolean)(IsRelationDeletedProperty))

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
      Database.LogException("DocumentTypesOptionDetail.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("DocumentTypesOptionDetail.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("DocumentTypesOptionDetail.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("DocumentTypesOptionDetail.Child_Fetch", ex)
      Throw New DbCslaException("DocumentTypesOptionDetail.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
