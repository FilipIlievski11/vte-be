
<Serializable()> _
Public Class DocumentsTehnicalExamsReportsDetailsStatus
  Inherits Csla.BusinessBase(Of DocumentsTehnicalExamsReportsDetailsStatus)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentsTehnicalExamsReportsDetailsStatuByID"
  Private Const spGetAll As String = "GetDocumentsTehnicalExamsReportsDetailsStatus"
  Private Const spUpdate As String = "updateDocumentsTehnicalExamsReportsDetailsStatu"
  Private Const spAdd As String = "addDocumentsTehnicalExamsReportsDetailsStatu"
  Private Const spDelete As String = "deleteDocumentsTehnicalExamsReportsDetailsStatu"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsTehnicalExamsReportsDetailsStatus), New PropertyInfo(Of Integer)("Id"))
  Private Shared StatusNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsTehnicalExamsReportsDetailsStatus), New PropertyInfo(Of String)("StatusName"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property StatusName() As String
    Get
      Return GetProperty(Of String)(StatusNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(StatusNameProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("StatusName") Then
      AuthorizationRules.AllowWrite("StatusName", roleName)
    Else
      AuthorizationRules.DenyWrite("StatusName", roleName)
    End If
    'AuthorizationRules.AllowWrite("StatusName")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
      AuthorizationRules.AllowWrite("Active", roleName)
    Else
      AuthorizationRules.DenyWrite("Active", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsTehnicalExamsReportsDetailsStatus")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsTehnicalExamsReportsDetailsStatus")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsTehnicalExamsReportsDetailsStatus")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsTehnicalExamsReportsDetailsStatus")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' StatusNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, StatusNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(StatusNameProperty, 50))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewDocumentsTehnicalExamsReportsDetailsStatus() As DocumentsTehnicalExamsReportsDetailsStatus
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a DocumentsTehnicalExamsReportsDetailsStatus")
    End If
    Return DataPortal.Create(Of DocumentsTehnicalExamsReportsDetailsStatus)()
  End Function

  Public Shared Function GetDocumentsTehnicalExamsReportsDetailsStatus(ByVal id As Integer) As DocumentsTehnicalExamsReportsDetailsStatus
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a DocumentsTehnicalExamsReportsDetailsStatus")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of DocumentsTehnicalExamsReportsDetailsStatus, Integer)(Id))
  End Function

  Public Shared Sub DeleteDocumentsTehnicalExamsReportsDetailsStatus(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a DocumentsTehnicalExamsReportsDetailsStatus")
    End If
    DataPortal.Delete(New SingleCriteria(Of DocumentsTehnicalExamsReportsDetailsStatus, Integer)(Id))
  End Sub

  Public Overrides Function Save() As DocumentsTehnicalExamsReportsDetailsStatus
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a DocumentsTehnicalExamsReportsDetailsStatus")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a DocumentsTehnicalExamsReportsDetailsStatus")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a DocumentsTehnicalExamsReportsDetailsStatus")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewDocumentsTehnicalExamsReportsDetailsStatusChild() As DocumentsTehnicalExamsReportsDetailsStatus
    Return DataPortal.CreateChild(Of DocumentsTehnicalExamsReportsDetailsStatus)()
  End Function

  Friend Shared Function GetDocumentsTehnicalExamsReportsDetailsStatus(ByVal dr As SafeDataReader) As DocumentsTehnicalExamsReportsDetailsStatus
    Return DataPortal.FetchChild(Of DocumentsTehnicalExamsReportsDetailsStatus)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of DocumentsTehnicalExamsReportsDetailsStatus, Integer))
    Database.LogInfo("DocumentsTehnicalExamsReportsDetailsStatus.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(StatusNameProperty, dr.GetString("StatusName"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DocumentsTehnicalExamsReportsDetailsStatus.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportsDetailsStatus.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@StatusName", ReadProperty(Of String)(StatusNameProperty))

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
      Database.LogException("DocumentsTehnicalExamsReportsDetailsStatus.DataPortal_Insert", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportsDetailsStatus.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("DocumentsTehnicalExamsReportsDetailsStatus.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@StatusName", ReadProperty(Of String)(StatusNameProperty))
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
    DataPortal_Delete(New SingleCriteria(Of DocumentsTehnicalExamsReportsDetailsStatus, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of DocumentsTehnicalExamsReportsDetailsStatus, Integer))
    Database.LogInfo("DocumentsTehnicalExamsReportsDetailsStatus.DataPortal_Delete", GetHashCode())
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
      Database.LogException("DocumentsTehnicalExamsReportsDetailsStatus.DataPortal_Delete", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportsDetailsStatus.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("DocumentsTehnicalExamsReportsDetailsStatus.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(StatusNameProperty, dr.GetString("StatusName"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("DocumentsTehnicalExamsReportsDetailsStatus.Child_Fetch", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportsDetailsStatus.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@StatusName", ReadProperty(Of String)(StatusNameProperty))

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
      Database.LogException("DocumentsTehnicalExamsReportsDetailsStatus.Child_Insert", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportsDetailsStatus.Child_Insert", ex)
    Finally
      Database.LogInfo("DocumentsTehnicalExamsReportsDetailsStatus.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("DocumentsTehnicalExamsReportsDetailsStatus.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext("cn") = cn
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@StatusName", ReadProperty(Of String)(StatusNameProperty))
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
      Database.LogException("DocumentsTehnicalExamsReportsDetailsStatus.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("DocumentsTehnicalExamsReportsDetailsStatus.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("DocumentsTehnicalExamsReportsDetailsStatus.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("DocumentsTehnicalExamsReportsDetailsStatus.Child_Fetch", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportsDetailsStatus.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
