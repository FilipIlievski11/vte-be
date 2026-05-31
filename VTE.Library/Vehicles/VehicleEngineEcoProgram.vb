
<Serializable()> _
Public Class VehicleEngineEcoProgram
  Inherits Csla.BusinessBase(Of VehicleEngineEcoProgram)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleEngineEcoPrograByID"
  Private Const spGetAll As String = "GetVehicleEngineEcoProgram"
  Private Const spUpdate As String = "updateVehicleEngineEcoProgra"
  Private Const spAdd As String = "addVehicleEngineEcoProgra"
  Private Const spDelete As String = "deleteVehicleEngineEcoProgra"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleEngineEcoProgram), New PropertyInfo(Of Integer)("Id"))
  Private Shared CodeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleEngineEcoProgram), New PropertyInfo(Of String)("Code"))
  Private Shared EcoProgramProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleEngineEcoProgram), New PropertyInfo(Of String)("EcoProgram"))
  Private Shared TechincalDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(VehicleEngineEcoProgram), New PropertyInfo(Of String)("TechincalDescription"))
  Private Shared PercentForPaymentProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(VehicleEngineEcoProgram), New PropertyInfo(Of Decimal)("PercentForPayment"))
  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property Code() As String
    Get
      Return GetProperty(Of String)(CodeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CodeProperty, value)
    End Set
  End Property
  Public Property EcoProgram() As String
    Get
      Return GetProperty(Of String)(EcoProgramProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(EcoProgramProperty, value)
    End Set
  End Property
  Public Property TechincalDescription() As String
    Get
      Return GetProperty(Of String)(TechincalDescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(TechincalDescriptionProperty, value)
    End Set
  End Property
  Public Property PercentForPayment() As Decimal
    Get
      Return GetProperty(Of Decimal)(PercentForPaymentProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(PercentForPaymentProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Code") Then
      AuthorizationRules.AllowWrite("Code", roleName)
    Else
      AuthorizationRules.DenyWrite("Code", roleName)
    End If
    'AuthorizationRules.AllowWrite("Code")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("EcoProgram") Then
      AuthorizationRules.AllowWrite("EcoProgram", roleName)
    Else
      AuthorizationRules.DenyWrite("EcoProgram", roleName)
    End If
    'AuthorizationRules.AllowWrite("EcoProgram")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("TechincalDescription") Then
      AuthorizationRules.AllowWrite("TechincalDescription", roleName)
    Else
      AuthorizationRules.DenyWrite("TechincalDescription", roleName)
    End If
    'AuthorizationRules.AllowWrite("TechincalDescription")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleEngineEcoProgram")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleEngineEcoProgram")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleEngineEcoProgram")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleEngineEcoProgram")
  End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' CodeProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, CodeProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CodeProperty, 10))
    ' EcoProgramProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, EcoProgramProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(EcoProgramProperty, 250))
    ' TechincalDescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, TechincalDescriptionProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(TechincalDescriptionProperty, 250))
    'PercentForPaymentProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.IntegerMinValue, _
                            New Validation.IntegerMinValueRuleArgs(PercentForPaymentProperty, 0))

  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewVehicleEngineEcoProgram() As VehicleEngineEcoProgram
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleEngineEcoProgram")
    End If
    Return DataPortal.Create(Of VehicleEngineEcoProgram)()
  End Function

  Public Shared Function GetVehicleEngineEcoProgram(ByVal id As Integer) As VehicleEngineEcoProgram
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a VehicleEngineEcoProgram")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of VehicleEngineEcoProgram, Integer)(Id))
  End Function

  Public Shared Sub DeleteVehicleEngineEcoProgram(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleEngineEcoProgram")
    End If
    DataPortal.Delete(New SingleCriteria(Of VehicleEngineEcoProgram, Integer)(Id))
  End Sub

  Public Overrides Function Save() As VehicleEngineEcoProgram
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a VehicleEngineEcoProgram")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a VehicleEngineEcoProgram")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a VehicleEngineEcoProgram")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewVehicleEngineEcoProgramChild() As VehicleEngineEcoProgram
    Return DataPortal.CreateChild(Of VehicleEngineEcoProgram)()
  End Function

  Friend Shared Function GetVehicleEngineEcoProgram(ByVal dr As SafeDataReader) As VehicleEngineEcoProgram
    Return DataPortal.FetchChild(Of VehicleEngineEcoProgram)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleEngineEcoProgram, Integer))
    Database.LogInfo("VehicleEngineEcoProgram.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(CodeProperty, dr.GetString("Code"))
            LoadProperty(Of String)(EcoProgramProperty, dr.GetString("EcoProgram"))
            LoadProperty(Of String)(TechincalDescriptionProperty, dr.GetString("TechincalDescription"))
            LoadProperty(Of Decimal)(PercentForPaymentProperty, dr.GetValue("PercentForPayment"))
            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleEngineEcoProgram.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleEngineEcoProgram.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
            .Parameters.AddWithValue("@EcoProgram", ReadProperty(Of String)(EcoProgramProperty))
            .Parameters.AddWithValue("@TechincalDescription", ReadProperty(Of String)(TechincalDescriptionProperty))
            .Parameters.AddWithValue("@PercentForPayment", ReadProperty(Of Decimal)(PercentForPaymentProperty))
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
      Database.LogException("VehicleEngineEcoProgram.DataPortal_Insert", ex)
      Throw New DbCslaException("VehicleEngineEcoProgram.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("VehicleEngineEcoProgram.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
            .Parameters.AddWithValue("@EcoProgram", ReadProperty(Of String)(EcoProgramProperty))
            .Parameters.AddWithValue("@TechincalDescription", ReadProperty(Of String)(TechincalDescriptionProperty))
            .Parameters.AddWithValue("@PercentForPayment", ReadProperty(Of Decimal)(PercentForPaymentProperty))
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
    DataPortal_Delete(New SingleCriteria(Of VehicleEngineEcoProgram, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of VehicleEngineEcoProgram, Integer))
    Database.LogInfo("VehicleEngineEcoProgram.DataPortal_Delete", GetHashCode())
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
      Database.LogException("VehicleEngineEcoProgram.DataPortal_Delete", ex)
      Throw New DbCslaException("VehicleEngineEcoProgram.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleEngineEcoProgram.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(CodeProperty, dr.GetString("Code"))
      LoadProperty(Of String)(EcoProgramProperty, dr.GetString("EcoProgram"))
      LoadProperty(Of String)(TechincalDescriptionProperty, dr.GetString("TechincalDescription"))
      LoadProperty(Of Decimal)(PercentForPaymentProperty, dr.GetValue("PercentForPayment"))
      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("VehicleEngineEcoProgram.Child_Fetch", ex)
      Throw New DbCslaException("VehicleEngineEcoProgram.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
            .Parameters.AddWithValue("@EcoProgram", ReadProperty(Of String)(EcoProgramProperty))
            .Parameters.AddWithValue("@TechincalDescription", ReadProperty(Of String)(TechincalDescriptionProperty))
            .Parameters.AddWithValue("@PercentForPayment", ReadProperty(Of Decimal)(PercentForPaymentProperty))
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
      Database.LogException("VehicleEngineEcoProgram.Child_Insert", ex)
      Throw New DbCslaException("VehicleEngineEcoProgram.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleEngineEcoProgram.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("VehicleEngineEcoProgram.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
            .Parameters.AddWithValue("@EcoProgram", ReadProperty(Of String)(EcoProgramProperty))
            .Parameters.AddWithValue("@TechincalDescription", ReadProperty(Of String)(TechincalDescriptionProperty))
            .Parameters.AddWithValue("@lastChanged", _lastChanged)
            .Parameters.AddWithValue("@PercentForPayment", ReadProperty(Of Decimal)(PercentForPaymentProperty))
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
      Database.LogException("VehicleEngineEcoProgram.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleEngineEcoProgram.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleEngineEcoProgram.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("VehicleEngineEcoProgram.Child_Fetch", ex)
      Throw New DbCslaException("VehicleEngineEcoProgram.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
