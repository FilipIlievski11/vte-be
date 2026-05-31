
<Serializable()> _
Public Class VehicleCategoriesRelation
  Inherits Csla.BusinessBase(Of VehicleCategoriesRelation)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleCategoriesRelationByID"
  Private Const spGetAll As String = "GetVehicleCategoriesRelations"
  Private Const spUpdate As String = "updateVehicleCategoriesRelation"
  Private Const spAdd As String = "addVehicleCategoriesRelation"
  Private Const spDelete As String = "deleteVehicleCategoriesRelation"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleCategoriesRelation), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdCategoryProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleCategoriesRelation), New PropertyInfo(Of Integer)("IdCategory"))
  Private Shared IdBodytypeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleCategoriesRelation), New PropertyInfo(Of Integer)("IdBodytype"))
  Private Shared IdUseProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleCategoriesRelation), New PropertyInfo(Of Integer)("IdUse"))
  Private Shared IdVehicleCategoryForPaymentsProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleCategoriesRelation), New PropertyInfo(Of Integer)("IdVehicleCategoryForPayments"))
  Private Shared DetailDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleCategoriesRelation), New PropertyInfo(Of String)("DetailDescription"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdCategory() As Integer
    Get
      Return GetProperty(Of Integer)(IdCategoryProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdCategoryProperty, value)
    End Set
  End Property
  Public Property IdBodytype() As Integer
    Get
      Return GetProperty(Of Integer)(IdBodytypeProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdBodytypeProperty, value)
    End Set
  End Property
  Public Property IdUse() As Integer
    Get
      Return GetProperty(Of Integer)(IdUseProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdUseProperty, value)
    End Set
  End Property
  Public Property IdVehicleCategoryForPayments() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty, value)
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

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCategory") Then
            AuthorizationRules.AllowWrite("IdCategory", roleName)
        Else
            AuthorizationRules.DenyWrite("IdCategory", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdBodytype") Then
            AuthorizationRules.AllowWrite("IdBodytype", roleName)
        Else
            AuthorizationRules.DenyWrite("IdBodytype", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdUse") Then
            AuthorizationRules.AllowWrite("IdUse", roleName)
        Else
            AuthorizationRules.DenyWrite("IdUse", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicleCategoryForPayments") Then
            AuthorizationRules.AllowWrite("IdVehicleCategoryForPayments", roleName)
        Else
            AuthorizationRules.DenyWrite("IdVehicleCategoryForPayments", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DetailDescription") Then
            AuthorizationRules.AllowWrite("DetailDescription", roleName)
        Else
            AuthorizationRules.DenyWrite("DetailDescription", roleName)
        End If
       
    End Sub



    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleCategoriesRelation")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleCategoriesRelation")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleCategoriesRelation")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleCategoriesRelation")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' DetailDescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(DetailDescriptionProperty, 250))
    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                            New Csla.Validation.IntegerMinValueRuleArgs(IdBodytypeProperty, 1))
    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                            New Csla.Validation.IntegerMinValueRuleArgs(IdUseProperty, 0))

  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewVehicleCategoriesRelationChild() As VehicleCategoriesRelation
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a VehicleCategoriesRelation")
        'End If
        Return DataPortal.CreateChild(Of VehicleCategoriesRelation)()
  End Function

  Friend Shared Function GetVehicleCategoriesRelation(ByVal dr As SafeDataReader) As VehicleCategoriesRelation
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a VehicleCategoriesRelation")
        'End If
        Return DataPortal.FetchChild(Of VehicleCategoriesRelation)(dr)
  End Function

    Public Shared Sub DeleteVehicleCategoriesRelation(ByVal id As Integer)
        'If Not CanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a VehicleCategoriesRelation")
        'End If
        DataPortal.Delete(New SingleCriteria(Of VehicleCategoriesRelation, Integer)(id))
    End Sub

    Public Overrides Function Save() As VehicleCategoriesRelation
        'If IsDeleted AndAlso Not CanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a VehicleCategoriesRelation")
        'ElseIf IsNew AndAlso Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a VehicleCategoriesRelation")
        'ElseIf Not CanEditObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to update a VehicleCategoriesRelation")
        'End If
        Return MyBase.Save()
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
    Database.LogInfo("VehicleCategoriesRelation.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdCategoryProperty, dr.GetInt32("IdCategory"))
      LoadProperty(Of Integer)(IdBodytypeProperty, dr.GetInt32("IdBodytype"))
      LoadProperty(Of Integer)(IdUseProperty, dr.GetInt32("IdUse"))
      LoadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty, dr.GetInt32("IdVehicleCategoryForPayments"))
      LoadProperty(Of String)(DetailDescriptionProperty, dr.GetString("DetailDescription"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("VehicleCategoriesRelation.Child_Fetch", ex)
      Throw New DbCslaException("VehicleCategoriesRelation.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As VehicleCategorie)
    Try
      Dim cn As SqlConnection = ApplicationContext.LocalContext("cn")
      If cn.State = ConnectionState.Closed Then
        cn.ConnectionString = Database.VTEConnection
        cn.Open()
      End If
      Using cm As SqlCommand = cn.CreateCommand
        With cm
          .CommandType = CommandType.StoredProcedure
          .CommandText = spAdd
          'Smeni go Id so Parent.Id
          .Parameters.AddWithValue("@IdCategory", parent.Id)
          .Parameters.AddWithValue("@IdBodytype", ReadProperty(Of Integer)(IdBodytypeProperty))
          .Parameters.AddWithValue("@IdUse", ReadProperty(Of Integer)(IdUseProperty))
          .Parameters.AddWithValue("@IdVehicleCategoryForPayments", ReadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty))

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

    Catch ex As Exception
      Database.LogException("VehicleCategoriesRelation.Child_Insert", ex)
      Throw New DbCslaException("VehicleCategoriesRelation.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleCategoriesRelation.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As VehicleCategorie)
    Database.LogInfo("VehicleCategoriesRelation.Child_Update", GetHashCode)
    Try
      Dim cn As SqlConnection = ApplicationContext.LocalContext("cn")
      If cn.State = ConnectionState.Closed Then
        cn.ConnectionString = Database.VTEConnection
        cn.Open()
      End If
      Using cm As SqlCommand = cn.CreateCommand
        With cm
          .CommandType = CommandType.StoredProcedure
          .CommandText = spUpdate

          .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
          .Parameters.AddWithValue("@IdCategory", parent.Id)
          .Parameters.AddWithValue("@IdBodytype", ReadProperty(Of Integer)(IdBodytypeProperty))
          .Parameters.AddWithValue("@IdUse", ReadProperty(Of Integer)(IdUseProperty))
          .Parameters.AddWithValue("@IdVehicleCategoryForPayments", ReadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty))
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

    Catch ex As Exception
      Database.LogException("VehicleCategoriesRelation.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleCategoriesRelation.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleCategoriesRelation.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleCategoriesRelation.Child_Fetch", ex)
      Throw New DbCslaException("VehicleCategoriesRelation.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
