
<Serializable()> _
Public Class VehicleAxis
  Inherits Csla.BusinessBase(Of VehicleAxis)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleAxiByID"
  Private Const spGetAll As String = "GetVehicleAxis"
  Private Const spUpdate As String = "updateVehicleAxi"
  Private Const spAdd As String = "addVehicleAxi"
  Private Const spDelete As String = "deleteVehicleAxi"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleAxis), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdVehicleProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(VehicleAxis), New PropertyInfo(Of Long)("IdVehicle"))
  Private Shared AxisNumberProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleAxis), New PropertyInfo(Of Integer)("AxisNumber"))
  Private Shared CarryingCapacityProperty As PropertyInfo(Of Single) = RegisterProperty(Of Single)(GetType(VehicleAxis), New PropertyInfo(Of Single)("CarryingCapacity"))
  Private Shared AxisLengthProperty As PropertyInfo(Of Single) = RegisterProperty(Of Single)(GetType(VehicleAxis), New PropertyInfo(Of Single)("AxisLength"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdVehicle() As Long
    Get
      Return GetProperty(Of Long)(IdVehicleProperty)
    End Get
    Set(ByVal value As Long)
      SetProperty(Of Long)(IdVehicleProperty, value)
    End Set
  End Property
  Public Property AxisNumber() As Integer
    Get
      Return GetProperty(Of Integer)(AxisNumberProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(AxisNumberProperty, value)
    End Set
  End Property
  Public Property CarryingCapacity() As Single
    Get
      Return GetProperty(Of Single)(CarryingCapacityProperty)
    End Get
    Set(ByVal value As Single)
      SetProperty(Of Single)(CarryingCapacityProperty, value)
    End Set
  End Property
  Public Property AxisLength() As Single
    Get
      Return GetProperty(Of Single)(AxisLengthProperty)
    End Get
    Set(ByVal value As Single)
      SetProperty(Of Single)(AxisLengthProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()

        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicle") Then
            AuthorizationRules.AllowWrite("IdVehicle", roleName)
        Else
            AuthorizationRules.DenyWrite("IdVehicle", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("AxisNumber") Then
            AuthorizationRules.AllowWrite("AxisNumber", roleName)
        Else
            AuthorizationRules.DenyWrite("AxisNumber", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("CarryingCapacity") Then
            AuthorizationRules.AllowWrite("CarryingCapacity", roleName)
        Else
            AuthorizationRules.DenyWrite("CarryingCapacity", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("AxisLength") Then
            AuthorizationRules.AllowWrite("AxisLength", roleName)
        Else
            AuthorizationRules.DenyWrite("AxisLength", roleName)
        End If
    End Sub



    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleAxis")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleAxis")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleAxis")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleAxis")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()

  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewVehicleAxisChild() As VehicleAxis
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a VehicleAxis")
        End If
        Return DataPortal.CreateChild(Of VehicleAxis)()
  End Function

  Friend Shared Function GetVehicleAxis(ByVal dr As SafeDataReader) As VehicleAxis
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a VehicleAxis")
        End If
        Return DataPortal.FetchChild(Of VehicleAxis)(dr)
  End Function

    Public Shared Sub DeleteVehicleAxis(ByVal id As Integer)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a VehicleAxis")
        End If
        DataPortal.Delete(New SingleCriteria(Of VehicleAxis, Integer)(id))
    End Sub

    Public Overrides Function Save() As VehicleAxis
        If IsDeleted AndAlso Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a VehicleAxis")
        ElseIf IsNew AndAlso Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a VehicleAxis")
        ElseIf Not CanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a VehicleAxis")
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
    Database.LogInfo("VehicleAxis.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Long)(IdVehicleProperty, dr.GetInt64("IdVehicle"))
      LoadProperty(Of Integer)(AxisNumberProperty, dr.GetInt32("AxisNumber"))
      LoadProperty(Of Single)(CarryingCapacityProperty, dr.GetValue("CarryingCapacity"))
      LoadProperty(Of Single)(AxisLengthProperty, dr.GetValue("AxisLength"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("VehicleAxis.Child_Fetch", ex)
      Throw New DbCslaException("VehicleAxis.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@IdVehicle", parent.Id)
            .Parameters.AddWithValue("@AxisNumber", ReadProperty(Of Integer)(AxisNumberProperty))
            .Parameters.AddWithValue("@CarryingCapacity", ReadProperty(Of Single)(CarryingCapacityProperty))
            .Parameters.AddWithValue("@AxisLength", ReadProperty(Of Single)(AxisLengthProperty))

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
      Database.LogException("VehicleAxis.Child_Insert", ex)
      Throw New DbCslaException("VehicleAxis.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleAxis.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As Vehicle)
    Database.LogInfo("VehicleAxis.Child_Update", GetHashCode)
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
            .Parameters.AddWithValue("@IdVehicle", parent.Id)
            .Parameters.AddWithValue("@AxisNumber", ReadProperty(Of Integer)(AxisNumberProperty))
            .Parameters.AddWithValue("@CarryingCapacity", ReadProperty(Of Single)(CarryingCapacityProperty))
            .Parameters.AddWithValue("@AxisLength", ReadProperty(Of Single)(AxisLengthProperty))
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
      Database.LogException("VehicleAxis.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleAxis.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleAxis.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleAxis.Child_Fetch", ex)
      Throw New DbCslaException("VehicleAxis.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
