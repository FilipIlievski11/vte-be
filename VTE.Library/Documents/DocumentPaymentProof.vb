
<Serializable()> _
Public Class DocumentPaymentProof
  Inherits Csla.BusinessBase(Of DocumentPaymentProof)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentPaymentProoByID"
  Private Const spGetAll As String = "GetDocumentPaymentProof"
  Private Const spUpdate As String = "updateDocumentPaymentProo"
  Private Const spAdd As String = "addDocumentPaymentProo"
  Private Const spDelete As String = "deleteDocumentPaymentProo"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentPaymentProof), New PropertyInfo(Of Integer)("Id"))
  Private Shared PaymentProofNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentPaymentProof), New PropertyInfo(Of String)("PaymentProofName"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property PaymentProofName() As String
    Get
      Return GetProperty(Of String)(PaymentProofNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(PaymentProofNameProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PaymentProofName") Then
      AuthorizationRules.AllowWrite("PaymentProofName", roleName)
    Else
      AuthorizationRules.DenyWrite("PaymentProofName", roleName)
    End If
    'AuthorizationRules.AllowWrite("PaymentProofName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentPaymentProof")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentPaymentProof")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentPaymentProof")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentPaymentProof")
    End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' PaymentProofNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, PaymentProofNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(PaymentProofNameProperty, 50))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewDocumentPaymentProof() As DocumentPaymentProof
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a DocumentPaymentProof")
    End If
    Return DataPortal.Create(Of DocumentPaymentProof)()
  End Function

  Public Shared Function GetDocumentPaymentProof(ByVal id As Integer) As DocumentPaymentProof
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a DocumentPaymentProof")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of DocumentPaymentProof, Integer)(Id))
  End Function

  Public Shared Sub DeleteDocumentPaymentProof(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a DocumentPaymentProof")
    End If
    DataPortal.Delete(New SingleCriteria(Of DocumentPaymentProof, Integer)(Id))
  End Sub

  Public Overrides Function Save() As DocumentPaymentProof
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a DocumentPaymentProof")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a DocumentPaymentProof")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a DocumentPaymentProof")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewDocumentPaymentProofChild() As DocumentPaymentProof
    Return DataPortal.CreateChild(Of DocumentPaymentProof)()
  End Function

  Friend Shared Function GetDocumentPaymentProof(ByVal dr As SafeDataReader) As DocumentPaymentProof
    Return DataPortal.FetchChild(Of DocumentPaymentProof)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of DocumentPaymentProof, Integer))
    Database.LogInfo("DocumentPaymentProof.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(PaymentProofNameProperty, dr.GetString("PaymentProofName"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DocumentPaymentProof.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentPaymentProof.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@PaymentProofName", ReadProperty(Of String)(PaymentProofNameProperty))

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
      Database.LogException("DocumentPaymentProof.DataPortal_Insert", ex)
      Throw New DbCslaException("DocumentPaymentProof.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("DocumentPaymentProof.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@PaymentProofName", ReadProperty(Of String)(PaymentProofNameProperty))
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
    DataPortal_Delete(New SingleCriteria(Of DocumentPaymentProof, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of DocumentPaymentProof, Integer))
    Database.LogInfo("DocumentPaymentProof.DataPortal_Delete", GetHashCode())
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
      Database.LogException("DocumentPaymentProof.DataPortal_Delete", ex)
      Throw New DbCslaException("DocumentPaymentProof.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("DocumentPaymentProof.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(PaymentProofNameProperty, dr.GetString("PaymentProofName"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("DocumentPaymentProof.Child_Fetch", ex)
      Throw New DbCslaException("DocumentPaymentProof.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@PaymentProofName", ReadProperty(Of String)(PaymentProofNameProperty))

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
      Database.LogException("DocumentPaymentProof.Child_Insert", ex)
      Throw New DbCslaException("DocumentPaymentProof.Child_Insert", ex)
    Finally
      Database.LogInfo("DocumentPaymentProof.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("DocumentPaymentProof.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@PaymentProofName", ReadProperty(Of String)(PaymentProofNameProperty))
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
      Database.LogException("DocumentPaymentProof.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("DocumentPaymentProof.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("DocumentPaymentProof.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("DocumentPaymentProof.Child_Fetch", ex)
      Throw New DbCslaException("DocumentPaymentProof.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
