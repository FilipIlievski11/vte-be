
<Serializable()> _
Public Class VehicleBodytype
  Inherits Csla.BusinessBase(Of VehicleBodytype)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleBodytypByID"
  Private Const spGetAll As String = "GetVehicleBodytype"
  Private Const spUpdate As String = "updateVehicleBodytyp"
  Private Const spAdd As String = "addVehicleBodytyp"
  Private Const spDelete As String = "deleteVehicleBodytyp"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleBodytype), New PropertyInfo(Of Integer)("Id"))
  Private Shared OldBodytypeDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleBodytype), New PropertyInfo(Of String)("OldBodytypeDescription"))
  Private Shared BodytypeCodeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleBodytype), New PropertyInfo(Of String)("BodytypeCode"))
  Private Shared BodytypeDescriprionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleBodytype), New PropertyInfo(Of String)("BodytypeDescriprion"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property OldBodytypeDescription() As String
    Get
      Return GetProperty(Of String)(OldBodytypeDescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(OldBodytypeDescriptionProperty, value)
    End Set
  End Property
  Public Property BodytypeCode() As String
    Get
      Return GetProperty(Of String)(BodytypeCodeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(BodytypeCodeProperty, value)
    End Set
  End Property
  Public Property BodytypeDescriprion() As String
    Get
      Return GetProperty(Of String)(BodytypeDescriprionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(BodytypeDescriprionProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("OldBodytypeDescription") Then
      AuthorizationRules.AllowWrite("OldBodytypeDescription", roleName)
    Else
      AuthorizationRules.DenyWrite("OldBodytypeDescription", roleName)
    End If
    'AuthorizationRules.AllowWrite("OldBodytypeDescription")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("BodytypeCode") Then
      AuthorizationRules.AllowWrite("BodytypeCode", roleName)
    Else
      AuthorizationRules.DenyWrite("BodytypeCode", roleName)
    End If
    'AuthorizationRules.AllowWrite("BodytypeCode")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("BodytypeDescriprion") Then
      AuthorizationRules.AllowWrite("BodytypeDescriprion", roleName)
    Else
      AuthorizationRules.DenyWrite("BodytypeDescriprion", roleName)
    End If
    'AuthorizationRules.AllowWrite("BodytypeDescriprion")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleBodytype")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleBodytype")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleBodytype")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleBodytype")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' OldBodytypeDescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(OldBodytypeDescriptionProperty, 150))
    ' BodytypeCodeProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, BodytypeCodeProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BodytypeCodeProperty, 5))
    ' BodytypeDescriprionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, BodytypeDescriprionProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(BodytypeDescriprionProperty, 250))
    ValidationRules.AddRule(Of VehicleBodytype)(AddressOf _
      BodytypeCodeDescriprionUnique(Of VehicleBodytype), BodytypeDescriprionProperty)
    ValidationRules.AddRule(Of VehicleBodytype)(AddressOf _
      BodytypeCodeDescriprionUnique(Of VehicleBodytype), BodytypeCodeProperty)
    ValidationRules.AddDependentProperty(BodytypeDescriprionProperty, BodytypeCodeProperty, True)
  End Sub
  Private Shared Function BodytypeCodeDescriprionUnique(Of T As VehicleBodytype) _
  (ByVal target As T, ByVal e As Csla.Validation.RuleArgs)

    Dim parent As VehicleBodytypes = CType(target.Parent, VehicleBodytypes)
    If parent IsNot Nothing Then
      For Each child As VehicleBodytype In parent
        If Not ReferenceEquals(target, child) Then
          If (Trim(child.GetProperty(Of String)(BodytypeDescriprionProperty)) _
              = Trim(target.GetProperty(Of String)(BodytypeDescriprionProperty))) _
              AndAlso (child.GetProperty(Of String)(BodytypeCodeProperty).Trim _
              = target.GetProperty(Of String)(BodytypeCodeProperty)) Then
            e.Description = "Такова комбинација од опис/код постои"
            'e.StopProcessing = True
            Return False
          End If
        End If
      Next
    End If
    Return True
  End Function

#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewVehicleBodytype() As VehicleBodytype
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleBodytype")
    End If
    Return DataPortal.Create(Of VehicleBodytype)()
  End Function

  Public Shared Function GetVehicleBodytype(ByVal id As Integer) As VehicleBodytype
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a VehicleBodytype")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of VehicleBodytype, Integer)(Id))
  End Function

  Public Shared Sub DeleteVehicleBodytype(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleBodytype")
    End If
    DataPortal.Delete(New SingleCriteria(Of VehicleBodytype, Integer)(Id))
  End Sub

  Public Overrides Function Save() As VehicleBodytype
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleBodytype")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleBodytype")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a VehicleBodytype")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleBodytypeChild() As VehicleBodytype
    Return DataPortal.CreateChild(Of VehicleBodytype)()
  End Function

  Friend Shared Function GetVehicleBodytype(ByVal dr As SafeDataReader) As VehicleBodytype
    Return DataPortal.FetchChild(Of VehicleBodytype)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleBodytype, Integer))
    Database.LogInfo("VehicleBodytype.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(OldBodytypeDescriptionProperty, dr.GetString("OldBodytypeDescription"))
            LoadProperty(Of String)(BodytypeCodeProperty, dr.GetString("BodytypeCode"))
            LoadProperty(Of String)(BodytypeDescriprionProperty, dr.GetString("BodytypeDescriprion"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleBodytype.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleBodytype.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@OldBodytypeDescription", ReadProperty(Of String)(OldBodytypeDescriptionProperty))
            .Parameters.AddWithValue("@BodytypeCode", ReadProperty(Of String)(BodytypeCodeProperty))
            .Parameters.AddWithValue("@BodytypeDescriprion", ReadProperty(Of String)(BodytypeDescriprionProperty))

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
        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("VehicleBodytype.DataPortal_Insert", ex)
      Throw New DbCslaException("VehicleBodytype.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("VehicleBodytype.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@OldBodytypeDescription", ReadProperty(Of String)(OldBodytypeDescriptionProperty))
            .Parameters.AddWithValue("@BodytypeCode", ReadProperty(Of String)(BodytypeCodeProperty))
            .Parameters.AddWithValue("@BodytypeDescriprion", ReadProperty(Of String)(BodytypeDescriprionProperty))
            .Parameters.AddWithValue("@lastChanged", _lastChanged)
            Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
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
    DataPortal_Delete(New SingleCriteria(Of VehicleBodytype, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleBodytype, Integer))
    Database.LogInfo("VehicleBodytype.DataPortal_Delete", GetHashCode())
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
      Database.LogException("VehicleBodytype.DataPortal_Delete", ex)
      Throw New DbCslaException("VehicleBodytype.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleBodytype.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(OldBodytypeDescriptionProperty, dr.GetString("OldBodytypeDescription"))
      LoadProperty(Of String)(BodytypeCodeProperty, dr.GetString("BodytypeCode"))
      LoadProperty(Of String)(BodytypeDescriprionProperty, dr.GetString("BodytypeDescriprion"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("VehicleBodytype.Child_Fetch", ex)
      Throw New DbCslaException("VehicleBodytype.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@OldBodytypeDescription", ReadProperty(Of String)(OldBodytypeDescriptionProperty))
            .Parameters.AddWithValue("@BodytypeCode", ReadProperty(Of String)(BodytypeCodeProperty))
            .Parameters.AddWithValue("@BodytypeDescriprion", ReadProperty(Of String)(BodytypeDescriprionProperty))

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
        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("VehicleBodytype.Child_Insert", ex)
      Throw New DbCslaException("VehicleBodytype.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleBodytype.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("VehicleBodytype.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@OldBodytypeDescription", ReadProperty(Of String)(OldBodytypeDescriptionProperty))
            .Parameters.AddWithValue("@BodytypeCode", ReadProperty(Of String)(BodytypeCodeProperty))
            .Parameters.AddWithValue("@BodytypeDescriprion", ReadProperty(Of String)(BodytypeDescriprionProperty))
            .Parameters.AddWithValue("@lastChanged", _lastChanged)
            Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
        If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
          ApplicationContext.LocalContext.Remove("cn")
        End If
      End Using
    Catch ex As Exception
      Database.LogException("VehicleBodytype.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleBodytype.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleBodytype.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleBodytype.Child_Fetch", ex)
      Throw New DbCslaException("VehicleBodytype.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
