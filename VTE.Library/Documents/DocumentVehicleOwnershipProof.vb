
<Serializable()> _
Public Class DocumentVehicleOwnershipProof
  Inherits Csla.BusinessBase(Of DocumentVehicleOwnershipProof)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentVehicleOwnershipProoByID"
  Private Const spGetAll As String = "GetDocumentVehicleOwnershipProof"
  Private Const spUpdate As String = "updateDocumentVehicleOwnershipProo"
  Private Const spAdd As String = "addDocumentVehicleOwnershipProo"
  Private Const spDelete As String = "deleteDocumentVehicleOwnershipProo"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentVehicleOwnershipProof), New PropertyInfo(Of Integer)("Id"))
  Private Shared VehicleOwnershipProofNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentVehicleOwnershipProof), New PropertyInfo(Of String)("VehicleOwnershipProofName"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property VehicleOwnershipProofName() As String
    Get
      Return GetProperty(Of String)(VehicleOwnershipProofNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(VehicleOwnershipProofNameProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("VehicleOwnershipProofName") Then
      AuthorizationRules.AllowWrite("VehicleOwnershipProofName", roleName)
    Else
      AuthorizationRules.DenyWrite("VehicleOwnershipProofName", roleName)
    End If
    'AuthorizationRules.AllowWrite("VehicleOwnershipProofName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentVehicleOwnershipProof")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentVehicleOwnershipProof")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentVehicleOwnershipProof")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentVehicleOwnershipProof")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' VehicleOwnershipProofNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, VehicleOwnershipProofNameProperty)
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(VehicleOwnershipProofNameProperty, 100))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewDocumentVehicleOwnershipProof() As DocumentVehicleOwnershipProof
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a DocumentVehicleOwnershipProof")
    End If
    Return DataPortal.Create(Of DocumentVehicleOwnershipProof)()
  End Function

  Public Shared Function GetDocumentVehicleOwnershipProof(ByVal id As Integer) As DocumentVehicleOwnershipProof
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a DocumentVehicleOwnershipProof")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of DocumentVehicleOwnershipProof, Integer)(Id))
  End Function

  Public Shared Sub DeleteDocumentVehicleOwnershipProof(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a DocumentVehicleOwnershipProof")
    End If
    DataPortal.Delete(New SingleCriteria(Of DocumentVehicleOwnershipProof, Integer)(Id))
  End Sub

  Public Overrides Function Save() As DocumentVehicleOwnershipProof
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a DocumentVehicleOwnershipProof")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a DocumentVehicleOwnershipProof")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a DocumentVehicleOwnershipProof")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewDocumentVehicleOwnershipProofChild() As DocumentVehicleOwnershipProof
    Return DataPortal.CreateChild(Of DocumentVehicleOwnershipProof)()
  End Function

  Friend Shared Function GetDocumentVehicleOwnershipProof(ByVal dr As SafeDataReader) As DocumentVehicleOwnershipProof
    Return DataPortal.FetchChild(Of DocumentVehicleOwnershipProof)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of DocumentVehicleOwnershipProof, Integer))
    Database.LogInfo("DocumentVehicleOwnershipProof.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(VehicleOwnershipProofNameProperty, dr.GetString("VehicleOwnershipProofName"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DocumentVehicleOwnershipProof.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentVehicleOwnershipProof.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@VehicleOwnershipProofName", ReadProperty(Of String)(VehicleOwnershipProofNameProperty))

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
      Database.LogException("DocumentVehicleOwnershipProof.DataPortal_Insert", ex)
      Throw New DbCslaException("DocumentVehicleOwnershipProof.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("DocumentVehicleOwnershipProof.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@VehicleOwnershipProofName", ReadProperty(Of String)(VehicleOwnershipProofNameProperty))
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
    DataPortal_Delete(New SingleCriteria(Of DocumentVehicleOwnershipProof, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of DocumentVehicleOwnershipProof, Integer))
    Database.LogInfo("DocumentVehicleOwnershipProof.DataPortal_Delete", GetHashCode())
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
      Database.LogException("DocumentVehicleOwnershipProof.DataPortal_Delete", ex)
      Throw New DbCslaException("DocumentVehicleOwnershipProof.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("DocumentVehicleOwnershipProof.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(VehicleOwnershipProofNameProperty, dr.GetString("VehicleOwnershipProofName"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("DocumentVehicleOwnershipProof.Child_Fetch", ex)
      Throw New DbCslaException("DocumentVehicleOwnershipProof.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@VehicleOwnershipProofName", ReadProperty(Of String)(VehicleOwnershipProofNameProperty))

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
      Database.LogException("DocumentVehicleOwnershipProof.Child_Insert", ex)
      Throw New DbCslaException("DocumentVehicleOwnershipProof.Child_Insert", ex)
    Finally
      Database.LogInfo("DocumentVehicleOwnershipProof.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("DocumentVehicleOwnershipProof.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@VehicleOwnershipProofName", ReadProperty(Of String)(VehicleOwnershipProofNameProperty))
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
      Database.LogException("DocumentVehicleOwnershipProof.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("DocumentVehicleOwnershipProof.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("DocumentVehicleOwnershipProof.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("DocumentVehicleOwnershipProof.Child_Fetch", ex)
      Throw New DbCslaException("DocumentVehicleOwnershipProof.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
