
<Serializable()> _
Public Class VehicleDisabledField
  Inherits Csla.BusinessBase(Of VehicleDisabledField)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleDisabledFieldByID"
  Private Const spGetAll As String = "GetVehicleDisabledFields"
  Private Const spUpdate As String = "updateVehicleDisabledField"
  Private Const spAdd As String = "addVehicleDisabledField"
  Private Const spDelete As String = "deleteVehicleDisabledField"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleDisabledField), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdCategoryProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleDisabledField), New PropertyInfo(Of Integer)("IdCategory"))
  Private Shared FieldNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleDisabledField), New PropertyInfo(Of String)("FieldName"))

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
  Public Property FieldName() As String
    Get
      Return GetProperty(Of String)(FieldNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(FieldNameProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' FieldNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, FieldNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(FieldNameProperty, 250))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewVehicleDisabledFieldChild() As VehicleDisabledField
    Return DataPortal.CreateChild(Of VehicleDisabledField)()
  End Function

  Friend Shared Function GetVehicleDisabledField(ByVal dr As SafeDataReader) As VehicleDisabledField
    Return DataPortal.FetchChild(Of VehicleDisabledField)(dr)
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
    Database.LogInfo("VehicleDisabledField.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdCategoryProperty, dr.GetInt32("IdCategory"))
      LoadProperty(Of String)(FieldNameProperty, dr.GetString("FieldName"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("VehicleDisabledField.Child_Fetch", ex)
      Throw New DbCslaException("VehicleDisabledField.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As VehicleCategorie)
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
            .Parameters.AddWithValue("@IdCategory", parent.Id)
            .Parameters.AddWithValue("@FieldName", ReadProperty(Of String)(FieldNameProperty))

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
      Database.LogException("VehicleDisabledField.Child_Insert", ex)
      Throw New DbCslaException("VehicleDisabledField.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleDisabledField.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As VehicleCategorie)
    Database.LogInfo("VehicleDisabledField.Child_Update", GetHashCode)
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
            .Parameters.AddWithValue("@IdCategory", parent.Id)
            .Parameters.AddWithValue("@FieldName", ReadProperty(Of String)(FieldNameProperty))
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
      Database.LogException("VehicleDisabledField.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleDisabledField.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleDisabledField.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleDisabledField.Child_Fetch", ex)
      Throw New DbCslaException("VehicleDisabledField.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
