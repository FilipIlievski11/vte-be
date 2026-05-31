
<Serializable()> _
Public Class VehicleGearBox
  Inherits Csla.BusinessBase(Of VehicleGearBox)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleGearBoxByID"
  Private Const spGetAll As String = "GetVehicleGearBox"
  Private Const spUpdate As String = "updateVehicleGearBox"
  Private Const spAdd As String = "addVehicleGearBox"
  Private Const spDelete As String = "deleteVehicleGearBox"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleGearBox), New PropertyInfo(Of Integer)("Id"))
  Private Shared GearBoxCodeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleGearBox), New PropertyInfo(Of Integer)("GearBoxCode"))
  Private Shared GearBoxDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleGearBox), New PropertyInfo(Of String)("GearBoxDescription"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property GearBoxCode() As Integer
    Get
      Return GetProperty(Of Integer)(GearBoxCodeProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(GearBoxCodeProperty, value)
    End Set
  End Property
  Public Property GearBoxDescription() As String
    Get
      Return GetProperty(Of String)(GearBoxDescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(GearBoxDescriptionProperty, value)
    End Set
  End Property

  Public ReadOnly Property GearBox() As String
    Get
      Return GearBoxCode & "-" & GearBoxDescription
    End Get
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' GearBoxDescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, GearBoxDescriptionProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(GearBoxDescriptionProperty, 150))
    ' ValidationRules.AddRule(Of VehicleGearBox)(AddressOf NoDuplicates, "GearBoxCode")
  End Sub
  Private Shared Function NoDuplicates(Of T As VehicleGearBox)(ByVal target As T, _
  ByVal e As Csla.Validation.RuleArgs) As Boolean

    Dim parent As VehicleGearBoxes = CType(target.Parent, VehicleGearBoxes)
    If parent IsNot Nothing Then
      For Each item As VehicleGearBox In parent
        If item.GearBoxCode = target.GearBoxCode AndAlso Not ReferenceEquals(item, target) Then
          e.Description = "Шифрата мора да биде единствена"
          Return False
        End If
      Next
    End If
    Return True

  End Function
#End Region ' Validation Rules

#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("GearBoxCode") Then
      AuthorizationRules.AllowWrite("GearBoxCode", roleName)
    Else
      AuthorizationRules.DenyWrite("GearBoxCode", roleName)
    End If
    'AuthorizationRules.AllowWrite("GearBoxCode")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("GearBoxDescription") Then
      AuthorizationRules.AllowWrite("GearBoxDescription", roleName)
    Else
      AuthorizationRules.DenyWrite("GearBoxDescription", roleName)
    End If
    'AuthorizationRules.AllowWrite("GearBoxDescription")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub
    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleGearBox")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleGearBox")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleGearBox")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleGearBox")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewVehicleGearBox() As VehicleGearBox
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a VehicleGearBox")
        End If
        Return DataPortal.Create(Of VehicleGearBox)()
  End Function

  Public Shared Function GetVehicleGearBox(ByVal id As Integer) As VehicleGearBox
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to get a VehicleGearBox")
        End If
        Return DataPortal.Fetch(New SingleCriteria(Of VehicleGearBox, Integer)(id))
  End Function

  Public Shared Sub DeleteVehicleGearBox(ByVal id As Integer)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a VehicleGearBox")
        End If
        DataPortal.Delete(New SingleCriteria(Of VehicleGearBox, Integer)(id))
  End Sub


    Public Overrides Function Save() As VehicleGearBox
        If IsDeleted AndAlso Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a VehicleGearBox")
        ElseIf IsNew AndAlso Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a VehicleGearBox")
        ElseIf Not CanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a VehicleGearBox")
        End If
        Return MyBase.Save()
    End Function

#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleGearBoxChild() As VehicleGearBox
    Return DataPortal.CreateChild(Of VehicleGearBox)()
  End Function

  Friend Shared Function GetVehicleGearBox(ByVal dr As SafeDataReader) As VehicleGearBox
    Return DataPortal.FetchChild(Of VehicleGearBox)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleGearBox, Integer))
    Database.LogInfo("VehicleGearBox.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of Integer)(GearBoxCodeProperty, dr.GetInt32("GearBoxCode"))
            LoadProperty(Of String)(GearBoxDescriptionProperty, dr.GetString("GearBoxDescription"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleGearBox.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleGearBox.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@GearBoxCode", ReadProperty(Of Integer)(GearBoxCodeProperty))
            .Parameters.AddWithValue("@GearBoxDescription", ReadProperty(Of String)(GearBoxDescriptionProperty))

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
      Database.LogException("VehicleGearBox.DataPortal_Insert", ex)
      Throw New DbCslaException("VehicleGearBox.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("VehicleGearBox.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@GearBoxCode", ReadProperty(Of Integer)(GearBoxCodeProperty))
            .Parameters.AddWithValue("@GearBoxDescription", ReadProperty(Of String)(GearBoxDescriptionProperty))
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
    DataPortal_Delete(New SingleCriteria(Of VehicleGearBox, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleGearBox, Integer))
    Database.LogInfo("VehicleGearBox.DataPortal_Delete", GetHashCode())
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
      Database.LogException("VehicleGearBox.DataPortal_Delete", ex)
      Throw New DbCslaException("VehicleGearBox.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleGearBox.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(GearBoxCodeProperty, dr.GetInt32("GearBoxCode"))
      LoadProperty(Of String)(GearBoxDescriptionProperty, dr.GetString("GearBoxDescription"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("VehicleGearBox.Child_Fetch", ex)
      Throw New DbCslaException("VehicleGearBox.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@GearBoxCode", ReadProperty(Of Integer)(GearBoxCodeProperty))
            .Parameters.AddWithValue("@GearBoxDescription", ReadProperty(Of String)(GearBoxDescriptionProperty))

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
      Database.LogException("VehicleGearBox.Child_Insert", ex)
      Throw New DbCslaException("VehicleGearBox.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleGearBox.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("VehicleGearBox.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@GearBoxCode", ReadProperty(Of Integer)(GearBoxCodeProperty))
            .Parameters.AddWithValue("@GearBoxDescription", ReadProperty(Of String)(GearBoxDescriptionProperty))
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
      Database.LogException("VehicleGearBox.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleGearBox.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleGearBox.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleGearBox.Child_Fetch", ex)
      Throw New DbCslaException("VehicleGearBox.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
