

<Serializable()> _
Public Class VehicleSupporting
  Inherits Csla.BusinessBase(Of VehicleSupporting)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleSupportinByID"
  Private Const spGetAll As String = "GetVehicleSupporting"
  Private Const spUpdate As String = "updateVehicleSupportin"
  Private Const spAdd As String = "addVehicleSupportin"
  Private Const spDelete As String = "deleteVehicleSupportin"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleSupporting), New PropertyInfo(Of Integer)("Id"))
  Private Shared SupportingCodeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleSupporting), New PropertyInfo(Of Integer)("SupportingCode"))
  Private Shared SupportingDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleSupporting), New PropertyInfo(Of String)("SupportingDescription"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property SupportingCode() As Integer
    Get
      Return GetProperty(Of Integer)(SupportingCodeProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(SupportingCodeProperty, value)
    End Set
  End Property
  Public Property SupportingDescription() As String
    Get
      Return GetProperty(Of String)(SupportingDescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(SupportingDescriptionProperty, value)
    End Set
  End Property

  Public ReadOnly Property Supporting() As String
    Get
      Return SupportingCode & "-" & SupportingDescription
    End Get

  End Property


  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()

    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("SupportingCode") Then
      AuthorizationRules.AllowWrite("SupportingCode", roleName)
    Else
      AuthorizationRules.DenyWrite("SupportingCode", roleName)
    End If
    'AuthorizationRules.AllowWrite("SupportingCode")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("SupportingDescription") Then
      AuthorizationRules.AllowWrite("SupportingDescription", roleName)
    Else
      AuthorizationRules.DenyWrite("SupportingDescription", roleName)
    End If
    'AuthorizationRules.AllowWrite("SupportingDescription")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleSupportings")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleSupportings")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleSupportings")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleSupportings")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' SupportingDescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, SupportingDescriptionProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(SupportingDescriptionProperty, 150))
    ' ValidationRules.AddRule(Of VehicleSupporting)(AddressOf NoDuplicates, "SupportingCode")
  End Sub
  Private Shared Function NoDuplicates(Of T As VehicleSupporting)(ByVal target As T, _
  ByVal e As Csla.Validation.RuleArgs) As Boolean

    Dim parent As VehicleSupportings = CType(target.Parent, VehicleSupportings)
    If parent IsNot Nothing Then
      For Each item As VehicleSupporting In parent
        If item.SupportingCode = target.SupportingCode AndAlso Not ReferenceEquals(item, target) Then
          e.Description = "Шифрата мора да биде единствена"
          Return False
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

  Public Shared Function NewVehicleSupporting() As VehicleSupporting
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleSupporting")
    End If
    Return DataPortal.Create(Of VehicleSupporting)()
  End Function

  Public Shared Function GetVehicleSupporting(ByVal id As Integer) As VehicleSupporting
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a VehicleSupporting")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of VehicleSupporting, Integer)(Id))
  End Function

  Public Shared Sub DeleteVehicleSupporting(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleSupporting")
    End If
    DataPortal.Delete(New SingleCriteria(Of VehicleSupporting, Integer)(Id))
  End Sub

  Public Overrides Function Save() As VehicleSupporting
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleSupporting")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleSupporting")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a VehicleSupporting")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleSupportingChild() As VehicleSupporting
    Return DataPortal.CreateChild(Of VehicleSupporting)()
  End Function

  Friend Shared Function GetVehicleSupporting(ByVal dr As SafeDataReader) As VehicleSupporting
    Return DataPortal.FetchChild(Of VehicleSupporting)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleSupporting, Integer))
    Database.LogInfo("VehicleSupporting.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of Integer)(SupportingCodeProperty, dr.GetInt32("SupportingCode"))
            LoadProperty(Of String)(SupportingDescriptionProperty, dr.GetString("SupportingDescription"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleSupporting.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleSupporting.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@SupportingCode", ReadProperty(Of Integer)(SupportingCodeProperty))
            .Parameters.AddWithValue("@SupportingDescription", ReadProperty(Of String)(SupportingDescriptionProperty))

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
      Database.LogException("VehicleSupporting.DataPortal_Insert", ex)
      Throw New DbCslaException("VehicleSupporting.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("VehicleSupporting.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@SupportingCode", ReadProperty(Of Integer)(SupportingCodeProperty))
            .Parameters.AddWithValue("@SupportingDescription", ReadProperty(Of String)(SupportingDescriptionProperty))
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
    DataPortal_Delete(New SingleCriteria(Of VehicleSupporting, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleSupporting, Integer))
    Database.LogInfo("VehicleSupporting.DataPortal_Delete", GetHashCode())
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
      Database.LogException("VehicleSupporting.DataPortal_Delete", ex)
      Throw New DbCslaException("VehicleSupporting.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleSupporting.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(SupportingCodeProperty, dr.GetInt32("SupportingCode"))
      LoadProperty(Of String)(SupportingDescriptionProperty, dr.GetString("SupportingDescription"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("VehicleSupporting.Child_Fetch", ex)
      Throw New DbCslaException("VehicleSupporting.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@SupportingCode", ReadProperty(Of Integer)(SupportingCodeProperty))
            .Parameters.AddWithValue("@SupportingDescription", ReadProperty(Of String)(SupportingDescriptionProperty))

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
      Database.LogException("VehicleSupporting.Child_Insert", ex)
      Throw New DbCslaException("VehicleSupporting.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleSupporting.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("VehicleSupporting.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@SupportingCode", ReadProperty(Of Integer)(SupportingCodeProperty))
            .Parameters.AddWithValue("@SupportingDescription", ReadProperty(Of String)(SupportingDescriptionProperty))
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
      Database.LogException("VehicleSupporting.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleSupporting.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleSupporting.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleSupporting.Child_Fetch", ex)
      Throw New DbCslaException("VehicleSupporting.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
