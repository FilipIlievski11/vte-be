
<Serializable()> _
Public Class VehicleBetweenAxesDestination
  Inherits Csla.BusinessBase(Of VehicleBetweenAxesDestination)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleBetweenAxesDestinationByID"
  Private Const spGetAll As String = "GetVehicleBetweenAxesDestinations"
  Private Const spUpdate As String = "updateVehicleBetweenAxesDestination"
  Private Const spAdd As String = "addVehicleBetweenAxesDestination"
  Private Const spDelete As String = "deleteVehicleBetweenAxesDestination"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(VehicleBetweenAxesDestination), New PropertyInfo(Of Long)("Id"))
  Private Shared IdVehicleProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(VehicleBetweenAxesDestination), New PropertyInfo(Of Long)("IdVehicle"))
  Private Shared FromToProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleBetweenAxesDestination), New PropertyInfo(Of String)("FromTo"))
  Private Shared DestinationProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(VehicleBetweenAxesDestination), New PropertyInfo(Of Decimal)("Destination"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Long
    Get
      Return GetProperty(Of Long)(IdProperty)
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
  Public Property FromTo() As String
    Get
      Return GetProperty(Of String)(FromToProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(FromToProperty, value)
    End Set
  End Property
  Public Property Destination() As Decimal
    Get
      Return GetProperty(Of Decimal)(DestinationProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(DestinationProperty, value)
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

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("FromTo") Then
            AuthorizationRules.AllowWrite("FromTo", roleName)
        Else
            AuthorizationRules.DenyWrite("FromTo", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Destination") Then
            AuthorizationRules.AllowWrite("Destination", roleName)
        Else
            AuthorizationRules.DenyWrite("Destination", roleName)
        End If
        
    End Sub



    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleBetweenAxesDestination")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleBetweenAxesDestination")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleBetweenAxesDestination")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleBetweenAxesDestination")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' FromToProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, FromToProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(FromToProperty, 5))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewVehicleBetweenAxesDestinationChild() As VehicleBetweenAxesDestination
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a VehicleBetweenAxesDestination")
        'End If
        Return DataPortal.CreateChild(Of VehicleBetweenAxesDestination)()
  End Function

  Friend Shared Function GetVehicleBetweenAxesDestination(ByVal dr As SafeDataReader) As VehicleBetweenAxesDestination
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a VehicleBetweenAxesDestination")
        'End If
        Return DataPortal.FetchChild(Of VehicleBetweenAxesDestination)(dr)
  End Function

    Public Shared Sub DeleteVehicleBetweenAxesDestination(ByVal id As Integer)
        'If Not CanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a VehicleBetweenAxesDestination")
        'End If
        DataPortal.Delete(New SingleCriteria(Of VehicleBetweenAxesDestination, Integer)(id))
    End Sub

    Public Overrides Function Save() As VehicleBetweenAxesDestination
        'If IsDeleted AndAlso Not CanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a VehicleBetweenAxesDestination")
        'ElseIf IsNew AndAlso Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a VehicleBetweenAxesDestination")
        'ElseIf Not CanEditObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to update a VehicleBetweenAxesDestination")
        'End If
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
    Database.LogInfo("VehicleBetweenAxesDestination.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Long)(IdVehicleProperty, dr.GetInt64("IdVehicle"))
      LoadProperty(Of String)(FromToProperty, dr.GetString("FromTo"))
      LoadProperty(Of Decimal)(DestinationProperty, dr.GetDecimal("Destination"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("VehicleBetweenAxesDestination.Child_Fetch", ex)
      Throw New DbCslaException("VehicleBetweenAxesDestination.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@FromTo", ReadProperty(Of String)(FromToProperty))
            .Parameters.AddWithValue("@Destination", ReadProperty(Of Decimal)(DestinationProperty))

            Dim param As New SqlParameter("@newId", SqlDbType.Int)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)
            param = New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            .Parameters.Add(param)

            .ExecuteNonQuery()

            LoadProperty(Of Long)(IdProperty, CInt(.Parameters("@newId").Value))
            _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
          End With
        End Using
        'update child objects
      End Using
    Catch ex As Exception
      Database.LogException("VehicleBetweenAxesDestination.Child_Insert", ex)
      Throw New DbCslaException("VehicleBetweenAxesDestination.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleBetweenAxesDestination.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As Vehicle)
    Database.LogInfo("VehicleBetweenAxesDestination.Child_Update", GetHashCode)
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

            .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
            .Parameters.AddWithValue("@IdVehicle", parent.Id)
            .Parameters.AddWithValue("@FromTo", ReadProperty(Of String)(FromToProperty))
            .Parameters.AddWithValue("@Destination", ReadProperty(Of Decimal)(DestinationProperty))
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
      Database.LogException("VehicleBetweenAxesDestination.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleBetweenAxesDestination.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleBetweenAxesDestination.Child_DeleteSelf", GetHashCode)
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
            .Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
            .ExecuteNonQuery()
          End With
        End Using

      End Using
    Catch ex As Exception
      Database.LogException("VehicleBetweenAxesDestination.Child_Fetch", ex)
      Throw New DbCslaException("VehicleBetweenAxesDestination.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
