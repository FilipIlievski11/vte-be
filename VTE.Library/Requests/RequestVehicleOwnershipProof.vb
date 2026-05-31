
<Serializable()> _
Public Class RequestVehicleOwnershipProof
  Inherits Csla.BusinessBase(Of RequestVehicleOwnershipProof)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetRequestVehicleOwnershipProofByID"
  Private Const spGetAll As String = "GetRequestVehicleOwnershipProofs"
  Private Const spUpdate As String = "updateRequestVehicleOwnershipProof"
  Private Const spAdd As String = "addRequestVehicleOwnershipProof"
  Private Const spDelete As String = "deleteRequestVehicleOwnershipProof"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(RequestVehicleOwnershipProof), New PropertyInfo(Of Long)("Id"))
  Private Shared IdRequestProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(RequestVehicleOwnershipProof), New PropertyInfo(Of Long)("IdRequest"))
  Private Shared IdVehicleOwnershipProofProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(RequestVehicleOwnershipProof), New PropertyInfo(Of Integer)("IdVehicleOwnershipProof"))
  Private Shared VehicleOwnershipProofProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(RequestVehicleOwnershipProof), New PropertyInfo(Of String)("VehicleOwnershipProof"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Long
    Get
      Return GetProperty(Of Long)(IdProperty)
    End Get
  End Property
  Public Property IdRequest() As Long
    Get
      Return GetProperty(Of Long)(IdRequestProperty)
    End Get
    Set(ByVal value As Long)
      SetProperty(Of Long)(IdRequestProperty, value)
    End Set
  End Property
  Public Property IdVehicleOwnershipProof() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleOwnershipProofProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleOwnershipProofProperty, value)
    End Set
  End Property
  Public Property VehicleOwnershipProof() As String
    Get
      Return GetProperty(Of String)(VehicleOwnershipProofProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(VehicleOwnershipProofProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdRequest") Then
            AuthorizationRules.AllowWrite("IdRequest", roleName)
        Else
            AuthorizationRules.DenyWrite("IdRequest", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicleOwnershipProof") Then
            AuthorizationRules.AllowWrite("IdVehicleOwnershipProof", roleName)
        Else
            AuthorizationRules.DenyWrite("IdVehicleOwnershipProof", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("VehicleOwnershipProof") Then
            AuthorizationRules.AllowWrite("VehicleOwnershipProof", roleName)
        Else
            AuthorizationRules.DenyWrite("VehicleOwnershipProof", roleName)
        End If
       
    End Sub

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("RequestVehicleOwnershipProof")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("RequestVehicleOwnershipProof")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("RequestVehicleOwnershipProof")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("RequestVehicleOwnershipProof")
    End Function
#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' VehicleOwnershipProofProperty rules
        'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, VehicleOwnershipProofProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(VehicleOwnershipProofProperty, 50))

    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                            New Validation.IntegerMinValueRuleArgs(IdVehicleOwnershipProofProperty, 1))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewRequestVehicleOwnershipProofChild() As RequestVehicleOwnershipProof
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a RequestVehicleOwnershipProof")
        'End If
        Return DataPortal.CreateChild(Of RequestVehicleOwnershipProof)()
  End Function

  Friend Shared Function GetRequestVehicleOwnershipProof(ByVal dr As SafeDataReader) As RequestVehicleOwnershipProof
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a RequestVehicleOwnershipProof")
        'End If
        Return DataPortal.FetchChild(Of RequestVehicleOwnershipProof)(dr)
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
    Database.LogInfo("RequestVehicleOwnershipProof.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Long)(IdRequestProperty, dr.GetInt64("IdRequest"))
      LoadProperty(Of Integer)(IdVehicleOwnershipProofProperty, dr.GetInt32("IdVehicleOwnershipProof"))
      LoadProperty(Of String)(VehicleOwnershipProofProperty, dr.GetString("VehicleOwnershipProof"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("RequestVehicleOwnershipProof.Child_Fetch", ex)
      Throw New DbCslaException("RequestVehicleOwnershipProof.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As Request)
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
          .Parameters.AddWithValue("@IdRequest", parent.Id)
          .Parameters.AddWithValue("@IdVehicleOwnershipProof", ReadProperty(Of Integer)(IdVehicleOwnershipProofProperty))
          .Parameters.AddWithValue("@VehicleOwnershipProof", ReadProperty(Of String)(VehicleOwnershipProofProperty))

          Dim param As New SqlParameter("@newId", SqlDbType.Int)
          param.Direction = ParameterDirection.Output
          .Parameters.Add(param)
          param = New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
          param.Direction = ParameterDirection.Output
          .Parameters.Add(param)

          .ExecuteNonQuery()

          LoadProperty(Of Long)(IdProperty, CLng(.Parameters("@newId").Value))
          _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
        End With
      End Using
      'update child objects

    Catch ex As Exception
      Database.LogException("RequestVehicleOwnershipProof.Child_Insert", ex)
      Throw New DbCslaException("RequestVehicleOwnershipProof.Child_Insert", ex)
    Finally
      Database.LogInfo("RequestVehicleOwnershipProof.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As Request)
    Database.LogInfo("RequestVehicleOwnershipProof.Child_Update", GetHashCode)
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

          .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
          .Parameters.AddWithValue("@IdRequest", parent.Id)
          .Parameters.AddWithValue("@IdVehicleOwnershipProof", ReadProperty(Of Integer)(IdVehicleOwnershipProofProperty))
          .Parameters.AddWithValue("@VehicleOwnershipProof", ReadProperty(Of String)(VehicleOwnershipProofProperty))
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
      Database.LogException("RequestVehicleOwnershipProof.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("RequestVehicleOwnershipProof.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("RequestVehicleOwnershipProof.Child_DeleteSelf", GetHashCode)
    Try
      Dim cn As SqlConnection = Csla.ApplicationContext.LocalContext("cn")
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


    Catch ex As Exception
      Database.LogException("RequestVehicleOwnershipProof.Child_Fetch", ex)
      Throw New DbCslaException("RequestVehicleOwnershipProof.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
