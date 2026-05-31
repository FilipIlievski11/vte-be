
<Serializable()> _
Public Class TireType
  Inherits Csla.BusinessBase(Of TireType)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleTireTypeByID"
  Private Const spGetAll As String = "GetVehicleTireTypes"
  Private Const spUpdate As String = "updateVehicleTireType"
  Private Const spAdd As String = "addVehicleTireType"
  Private Const spDelete As String = "deleteVehicleTireType"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(TireType), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdVehicleModelProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(TireType), New PropertyInfo(Of Integer)("IdVehicleModel"))
  Private Shared TireTypeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(TireType), New PropertyInfo(Of String)("TireType"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdVehicleModel() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleModelProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleModelProperty, value)
    End Set
  End Property
  Public Property TireType() As String
    Get
      Return GetProperty(Of String)(TireTypeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(TireTypeProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()

        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicleModel") Then
            AuthorizationRules.AllowWrite("IdVehicleModel", roleName)
        Else
            AuthorizationRules.DenyWrite("IdVehicleModel", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TireType") Then
            AuthorizationRules.AllowWrite("TireType", roleName)
        Else
            AuthorizationRules.DenyWrite("TireType", roleName)
        End If
       
    End Sub



    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("TireType")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("TireType")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("TireType")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("TireType")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' TireTypeProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, TireTypeProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(TireTypeProperty, 150))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewTireTypeChild() As TireType
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a TireType")
        End If
        Return DataPortal.CreateChild(Of TireType)()
  End Function

  Friend Shared Function GetTireType(ByVal dr As SafeDataReader) As TireType
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a TireType")
        End If
        Return DataPortal.FetchChild(Of TireType)(dr)
  End Function

    Public Shared Sub DeleteTireType(ByVal id As Integer)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a TireType")
        End If
        DataPortal.Delete(New SingleCriteria(Of TireType, Integer)(id))
    End Sub

    Public Overrides Function Save() As TireType
        If IsDeleted AndAlso Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a TireType")
        ElseIf IsNew AndAlso Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a TireType")
        ElseIf Not CanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a TireType")
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
    Database.LogInfo("TireType.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdVehicleModelProperty, dr.GetInt32("IdVehicleModel"))
      LoadProperty(Of String)(TireTypeProperty, dr.GetString("TireType"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("TireType.Child_Fetch", ex)
      Throw New DbCslaException("TireType.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As Vehicle)
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
            .Parameters.AddWithValue("@IdVehicleModel", parent.IdVehicleModel)
            .Parameters.AddWithValue("@TireType", ReadProperty(Of String)(TireTypeProperty))

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
      Database.LogException("TireType.Child_Insert", ex)
      Throw New DbCslaException("TireType.Child_Insert", ex)
    Finally
      Database.LogInfo("TireType.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As Vehicle)
    Database.LogInfo("TireType.Child_Update", GetHashCode)
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
            .Parameters.AddWithValue("@IdVehicleModel", parent.IdVehicleModel)
            .Parameters.AddWithValue("@TireType", ReadProperty(Of String)(TireTypeProperty))
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
      Database.LogException("TireType.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("TireType.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("TireType.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("TireType.Child_Fetch", ex)
      Throw New DbCslaException("TireType.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
