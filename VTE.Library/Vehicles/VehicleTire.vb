
<Serializable()> _
Public Class VehicleTyre
  Inherits Csla.BusinessBase(Of VehicleTyre)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleTyreByID"
  Private Const spGetAll As String = "GetVehicleTyres"
  Private Const spUpdate As String = "updateVehicleTyre"
  Private Const spAdd As String = "addVehicleTyre"
  Private Const spDelete As String = "deleteVehicleTyre"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleTyre), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdVehicleProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleTyre), New PropertyInfo(Of Integer)("IdVehicle"))
  Private Shared IdTireTypeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleTyre), New PropertyInfo(Of Integer)("IdTireType"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdVehicle() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleProperty, value)
    End Set
  End Property
  Public Property IdTireType() As Integer
    Get
      Return GetProperty(Of Integer)(IdTireTypeProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdTireTypeProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                            New Csla.Validation.IntegerMinValueRuleArgs(IdTireTypeProperty, 1))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewVehicleTyreChild() As VehicleTyre
    Return DataPortal.CreateChild(Of VehicleTyre)()
  End Function

  Friend Shared Function GetVehicleTyre(ByVal dr As SafeDataReader) As VehicleTyre
    Return DataPortal.FetchChild(Of VehicleTyre)(dr)
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
    Database.LogInfo("VehicleTyre.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdVehicleProperty, dr.GetInt64("IdVehicle"))
      LoadProperty(Of Integer)(IdTireTypeProperty, dr.GetInt32("IdTireType"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("VehicleTyre.Child_Fetch", ex)
      Throw New DbCslaException("VehicleTyre.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@IdTireType", ReadProperty(Of Integer)(IdTireTypeProperty))

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
      Database.LogException("VehicleTyre.Child_Insert", ex)
      Throw New DbCslaException("VehicleTyre.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleTyre.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As Vehicle)
    Database.LogInfo("VehicleTyre.Child_Update", GetHashCode)
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
            .Parameters.AddWithValue("@IdTireType", ReadProperty(Of Integer)(IdTireTypeProperty))
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
      Database.LogException("VehicleTyre.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleTyre.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleTyre.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleTyre.Child_Fetch", ex)
      Throw New DbCslaException("VehicleTyre.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access



End Class
