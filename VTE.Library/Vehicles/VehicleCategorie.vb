
<Serializable()> _
Public Class VehicleCategorie
  Inherits Csla.BusinessBase(Of VehicleCategorie)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleCategorieByID"
  Private Const spGetAll As String = "GetVehicleCategories"
  Private Const spUpdate As String = "updateVehicleCategorie"
  Private Const spAdd As String = "addVehicleCategorie"
  Private Const spDelete As String = "deleteVehicleCategorie"
  Private Const spGetChildRequiredFields As String = "getVehicleRequiredFieldByIdCategory"
  Private Const spGetChildRelation As String = "getVehicleCategoriesRelationByIdCategory"
#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleCategorie), New PropertyInfo(Of Integer)("Id"))
  Private Shared OldCategoryNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleCategorie), New PropertyInfo(Of String)("OldCategoryName"))
  Private Shared CategoryCodeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleCategorie), New PropertyInfo(Of String)("CategoryCode"))
  Private Shared CategoryNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleCategorie), New PropertyInfo(Of String)("CategoryName"))
  Private Shared MksjusProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleCategorie), New PropertyInfo(Of String)("Mksjus"))
  Private Shared IsoProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleCategorie), New PropertyInfo(Of String)("Iso"))
  Private Shared MKSJUSDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleCategorie), New PropertyInfo(Of String)("MKSJUSDescription"))
  Private Shared PicturePathProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleCategorie), New PropertyInfo(Of String)("PicturePath"))
  Private Shared DeskriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleCategorie), New PropertyInfo(Of String)("Deskription"))
  Private Shared DetailDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleCategorie), New PropertyInfo(Of String)("DetailDescription"))

  Private Shared RequiredFieldsProperty As PropertyInfo(Of VehicleRequiredFields) = _
  RegisterProperty(Of VehicleRequiredFields)(GetType(VehicleCategorie), New PropertyInfo(Of VehicleRequiredFields)("RequiredFields"))

  Private Shared DisabledFieldsProperty As PropertyInfo(Of VehicleDisabledFields) = _
  RegisterProperty(Of VehicleDisabledFields)(GetType(VehicleCategorie), New PropertyInfo(Of VehicleDisabledFields)("DisabledFields"))

  Private Shared RelationsProperty As PropertyInfo(Of VehicleCategoriesRelations) = _
  RegisterProperty(Of VehicleCategoriesRelations)(GetType(VehicleCategorie), New PropertyInfo(Of VehicleCategoriesRelations)("Relations"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
   Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property OldCategoryName() As String
    Get
      Return GetProperty(Of String)(OldCategoryNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(OldCategoryNameProperty, value)
    End Set
  End Property
  Public Property CategoryCode() As String
    Get
      Return GetProperty(Of String)(CategoryCodeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CategoryCodeProperty, value)
    End Set
  End Property
  Public Property CategoryName() As String
    Get
      Return GetProperty(Of String)(CategoryNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CategoryNameProperty, value)
    End Set
  End Property
  Public Property Mksjus() As String
    Get
      Return GetProperty(Of String)(MksjusProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(MksjusProperty, value)
    End Set
  End Property
  Public Property Iso() As String
    Get
      Return GetProperty(Of String)(IsoProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(IsoProperty, value)
    End Set
  End Property
  Public Property MKSJUSDescription() As String
    Get
      Return GetProperty(Of String)(MKSJUSDescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(MKSJUSDescriptionProperty, value)
    End Set
  End Property
  Public Property PicturePath() As String
    Get
      Return GetProperty(Of String)(PicturePathProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(PicturePathProperty, value)
    End Set
  End Property
  Public Property Deskription() As String
    Get
      Return GetProperty(Of String)(DeskriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(DeskriptionProperty, value)
    End Set
  End Property
  Public Property DetailDescription() As String
    Get
      Return GetProperty(Of String)(DetailDescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(DetailDescriptionProperty, value)
    End Set
  End Property

  Public ReadOnly Property RequiredFields() As VehicleRequiredFields
    Get
      If Not FieldManager.FieldExists(RequiredFieldsProperty) Then
        SetProperty(Of VehicleRequiredFields)(RequiredFieldsProperty, _
                                              VehicleRequiredFields.NewVehicleRequiredFields)
      End If
      Return GetProperty(Of VehicleRequiredFields)(RequiredFieldsProperty)
    End Get
  End Property

  Public ReadOnly Property DisabledFields() As VehicleDisabledFields
    Get
      If Not FieldManager.FieldExists(DisabledFieldsProperty) Then
        SetProperty(Of VehicleDisabledFields)(DisabledFieldsProperty, _
                                                  VehicleDisabledFields.NewVehicleDisabledFields)
      End If
      Return GetProperty(Of VehicleDisabledFields)(DisabledFieldsProperty)
    End Get
  End Property

  Public ReadOnly Property Relations() As VehicleCategoriesRelations
    Get
      If Not FieldManager.FieldExists(RelationsProperty) Then
        SetProperty(Of VehicleCategoriesRelations)(RelationsProperty, _
                                              VehicleCategoriesRelations.NewVehicleCategoriesRelations)
      End If
      Return GetProperty(Of VehicleCategoriesRelations)(RelationsProperty)
    End Get
  End Property


  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("OldCategoryName") Then
      AuthorizationRules.AllowWrite("OldCategoryName", roleName)
    Else
      AuthorizationRules.DenyWrite("OldCategoryName", roleName)
    End If
    'AuthorizationRules.AllowWrite("OldCategoryName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CategoryCode") Then
      AuthorizationRules.AllowWrite("CategoryCode", roleName)
    Else
      AuthorizationRules.DenyWrite("CategoryCode", roleName)
    End If
    'AuthorizationRules.AllowWrite("CategoryCode")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CategoryName") Then
      AuthorizationRules.AllowWrite("CategoryName", roleName)
    Else
      AuthorizationRules.DenyWrite("CategoryName", roleName)
    End If
    'AuthorizationRules.AllowWrite("CategoryName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Mksjus") Then
      AuthorizationRules.AllowWrite("Mksjus", roleName)
    Else
      AuthorizationRules.DenyWrite("Mksjus", roleName)
    End If
    'AuthorizationRules.AllowWrite("Mksjus")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Iso") Then
      AuthorizationRules.AllowWrite("Iso", roleName)
    Else
      AuthorizationRules.DenyWrite("Iso", roleName)
    End If
    'AuthorizationRules.AllowWrite("Iso")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("MKSJUSDescription") Then
      AuthorizationRules.AllowWrite("MKSJUSDescription", roleName)
    Else
      AuthorizationRules.DenyWrite("MKSJUSDescription", roleName)
    End If
    'AuthorizationRules.AllowWrite("MKSJUSDescription")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PicturePath") Then
      AuthorizationRules.AllowWrite("PicturePath", roleName)
    Else
      AuthorizationRules.DenyWrite("PicturePath", roleName)
    End If
    'AuthorizationRules.AllowWrite("PicturePath")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleCategorie")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleCategorie")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleCategorie")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleCategorie")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' OldCategoryNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(OldCategoryNameProperty, 50))
    ' CategoryCodeProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CategoryCodeProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CategoryCodeProperty, 5))
    ' CategoryNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CategoryNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CategoryNameProperty, 50))
    ' MksjusProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(MksjusProperty, 50))
    ' IsoProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(IsoProperty, 50))
    ' MKSJUSDescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(MKSJUSDescriptionProperty, 250))
    ' PicturePathProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(PicturePathProperty, 250))
  End Sub
#End Region ' Validation Rules


#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewVehicleCategorie() As VehicleCategorie
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleCategorie")
    End If
    Return DataPortal.Create(Of VehicleCategorie)()
  End Function

  Public Shared Function GetVehicleCategorie(ByVal id As Integer) As VehicleCategorie
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a VehicleCategorie")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of VehicleCategorie, Integer)(id))
  End Function

  Public Shared Sub DeleteVehicleCategorie(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleCategorie")
    End If
    DataPortal.Delete(New SingleCriteria(Of VehicleCategorie, Integer)(id))
  End Sub

  Public Overrides Function Save() As VehicleCategorie
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleCategorie")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleCategorie")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a VehicleCategorie")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleCategorieChild() As VehicleCategorie
    Return DataPortal.CreateChild(Of VehicleCategorie)()
  End Function

  Friend Shared Function GetVehicleCategorie(ByVal dr As SafeDataReader) As VehicleCategorie
    Return DataPortal.FetchChild(Of VehicleCategorie)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleCategorie, Integer))
    Database.LogInfo("VehicleCategorie.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(OldCategoryNameProperty, dr.GetString("OldCategoryName"))
            LoadProperty(Of String)(CategoryCodeProperty, dr.GetString("CategoryCode"))
            LoadProperty(Of String)(CategoryNameProperty, dr.GetString("CategoryName"))
            LoadProperty(Of String)(MksjusProperty, dr.GetString("MKSJUS"))
            LoadProperty(Of String)(IsoProperty, dr.GetString("ISO"))
            LoadProperty(Of String)(MKSJUSDescriptionProperty, dr.GetString("MKSJUSDescription"))
            LoadProperty(Of String)(PicturePathProperty, dr.GetString("PicturePath"))
            LoadProperty(Of String)(DeskriptionProperty, dr.GetString("Deskription"))
            LoadProperty(Of String)(DetailDescriptionProperty, dr.GetString("DetailDescription"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildRequiredFields
          cm1.Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleRequiredFields) _
            (RequiredFieldsProperty, VehicleRequiredFields.GetVehicleRequiredFields(drc))
          End Using
        End Using
        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildRelation
          cm1.Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleCategoriesRelations) _
            (RelationsProperty, VehicleCategoriesRelations.GetVehicleCategoriesRelations(drc))
          End Using
        End Using
        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = "getVehicleDisabledFieldByIdCategory"
          cm1.Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleDisabledFields) _
            (DisabledFieldsProperty, VehicleDisabledFields.GetVehicleDisabledFields(drc))
          End Using
        End Using

      End Using

    Catch ex As Exception
      Database.LogException("VehicleCategorie.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleCategorie.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@OldCategoryName", ReadProperty(Of String)(OldCategoryNameProperty))
            .Parameters.AddWithValue("@CategoryCode", ReadProperty(Of String)(CategoryCodeProperty))
            .Parameters.AddWithValue("@CategoryName", ReadProperty(Of String)(CategoryNameProperty))
            .Parameters.AddWithValue("@MKSJUS", ReadProperty(Of String)(MksjusProperty))
            .Parameters.AddWithValue("@ISO", ReadProperty(Of String)(IsoProperty))
            .Parameters.AddWithValue("@MKSJUSDescription", ReadProperty(Of String)(MKSJUSDescriptionProperty))
            .Parameters.AddWithValue("@PicturePath", ReadProperty(Of String)(PicturePathProperty))
            .Parameters.AddWithValue("@Deskription", ReadProperty(Of String)(DeskriptionProperty))
            .Parameters.AddWithValue("@DetailDescription", ReadProperty(Of String)(DetailDescriptionProperty))

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
      Database.LogException("VehicleCategorie.DataPortal_Insert", ex)
      Throw New DbCslaException("VehicleCategorie.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("VehicleCategorie.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@OldCategoryName", ReadProperty(Of String)(OldCategoryNameProperty))
            .Parameters.AddWithValue("@CategoryCode", ReadProperty(Of String)(CategoryCodeProperty))
            .Parameters.AddWithValue("@CategoryName", ReadProperty(Of String)(CategoryNameProperty))
            .Parameters.AddWithValue("@MKSJUS", ReadProperty(Of String)(MksjusProperty))
            .Parameters.AddWithValue("@ISO", ReadProperty(Of String)(IsoProperty))
            .Parameters.AddWithValue("@MKSJUSDescription", ReadProperty(Of String)(MKSJUSDescriptionProperty))
            .Parameters.AddWithValue("@PicturePath", ReadProperty(Of String)(PicturePathProperty))
            .Parameters.AddWithValue("@Deskription", ReadProperty(Of String)(DeskriptionProperty))
            .Parameters.AddWithValue("@DetailDescription", ReadProperty(Of String)(DetailDescriptionProperty))

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
    DataPortal_Delete(New SingleCriteria(Of VehicleCategorie, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleCategorie, Integer))
    Database.LogInfo("VehicleCategorie.DataPortal_Delete", GetHashCode())
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
      Database.LogException("VehicleCategorie.DataPortal_Delete", ex)
      Throw New DbCslaException("VehicleCategorie.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleCategorie.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(OldCategoryNameProperty, dr.GetString("OldCategoryName"))
      LoadProperty(Of String)(CategoryCodeProperty, dr.GetString("CategoryCode"))
      LoadProperty(Of String)(CategoryNameProperty, dr.GetString("CategoryName"))
      LoadProperty(Of String)(MksjusProperty, dr.GetString("MKSJUS"))
      LoadProperty(Of String)(IsoProperty, dr.GetString("ISO"))
      LoadProperty(Of String)(MKSJUSDescriptionProperty, dr.GetString("MKSJUSDescription"))
      LoadProperty(Of String)(PicturePathProperty, dr.GetString("PicturePath"))
      LoadProperty(Of String)(DeskriptionProperty, dr.GetString("Deskription"))
      LoadProperty(Of String)(DetailDescriptionProperty, dr.GetString("DetailDescription"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildRequiredFields
          cm1.Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleRequiredFields) _
            (RequiredFieldsProperty, VehicleRequiredFields.GetVehicleRequiredFields(drc))
          End Using
        End Using
        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = spGetChildRelation
          cm1.Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleCategoriesRelations) _
            (RelationsProperty, VehicleCategoriesRelations.GetVehicleCategoriesRelations(drc))
          End Using
        End Using
        Using cm1 As SqlCommand = cn.CreateCommand
          cm1.CommandType = CommandType.StoredProcedure
          cm1.CommandText = "getVehicleDisabledFieldByIdCategory"
          cm1.Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
          Using drc As New SafeDataReader(cm1.ExecuteReader)
            LoadProperty(Of VehicleDisabledFields) _
            (DisabledFieldsProperty, VehicleDisabledFields.GetVehicleDisabledFields(drc))
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleCategorie.Child_Fetch", ex)
      Throw New DbCslaException("VehicleCategorie.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@OldCategoryName", ReadProperty(Of String)(OldCategoryNameProperty))
            .Parameters.AddWithValue("@CategoryCode", ReadProperty(Of String)(CategoryCodeProperty))
            .Parameters.AddWithValue("@CategoryName", ReadProperty(Of String)(CategoryNameProperty))
            .Parameters.AddWithValue("@MKSJUS", ReadProperty(Of String)(MksjusProperty))
            .Parameters.AddWithValue("@ISO", ReadProperty(Of String)(IsoProperty))
            .Parameters.AddWithValue("@MKSJUSDescription", ReadProperty(Of String)(MKSJUSDescriptionProperty))
            .Parameters.AddWithValue("@PicturePath", ReadProperty(Of String)(PicturePathProperty))
            .Parameters.AddWithValue("@Deskription", ReadProperty(Of String)(DeskriptionProperty))
            .Parameters.AddWithValue("@DetailDescription", ReadProperty(Of String)(DetailDescriptionProperty))

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
        FieldManager.UpdateChildren(Me)
        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("VehicleCategorie.Child_Insert", ex)
      Throw New DbCslaException("VehicleCategorie.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleCategorie.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("VehicleCategorie.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@OldCategoryName", ReadProperty(Of String)(OldCategoryNameProperty))
            .Parameters.AddWithValue("@CategoryCode", ReadProperty(Of String)(CategoryCodeProperty))
            .Parameters.AddWithValue("@CategoryName", ReadProperty(Of String)(CategoryNameProperty))
            .Parameters.AddWithValue("@MKSJUS", ReadProperty(Of String)(MksjusProperty))
            .Parameters.AddWithValue("@ISO", ReadProperty(Of String)(IsoProperty))
            .Parameters.AddWithValue("@MKSJUSDescription", ReadProperty(Of String)(MKSJUSDescriptionProperty))
            .Parameters.AddWithValue("@PicturePath", ReadProperty(Of String)(PicturePathProperty))
            .Parameters.AddWithValue("@Deskription", ReadProperty(Of String)(DeskriptionProperty))
            .Parameters.AddWithValue("@DetailDescription", ReadProperty(Of String)(DetailDescriptionProperty))

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
      Database.LogException("VehicleCategorie.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleCategorie.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleCategorie.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleCategorie.Child_Fetch", ex)
      Throw New DbCslaException("VehicleCategorie.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
