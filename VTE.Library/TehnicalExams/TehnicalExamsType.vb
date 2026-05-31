
<Serializable()> _
Public Class TehnicalExamsType
  Inherits Csla.BusinessBase(Of TehnicalExamsType)


#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetTehnicalExamsTypeByID"
  Private Const spGetAll As String = "GetTehnicalExamsTypes"
  Private Const spUpdate As String = "updateTehnicalExamsType"
  Private Const spAdd As String = "addTehnicalExamsType"
  Private Const spDelete As String = "deleteTehnicalExamsType"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(TehnicalExamsType), New PropertyInfo(Of Integer)("Id"))
  Private Shared DescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(TehnicalExamsType), New PropertyInfo(Of String)("Description"))
  Private Shared CodeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(TehnicalExamsType), New PropertyInfo(Of String)("Code"))
  Private Shared ValidNumOfDaysProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(TehnicalExamsType), New PropertyInfo(Of Integer)("ValidNumOfDays"))
  Private Shared PercentOfFullExamProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(TehnicalExamsType), New PropertyInfo(Of Integer)("PercentOfFullExam", "PercentOfFullExam", 100))
  Private Shared IsInRegistarProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(TehnicalExamsType), New PropertyInfo(Of Boolean)("IsInRegistar"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property Description() As String
    Get
      Return GetProperty(Of String)(DescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(DescriptionProperty, value)
    End Set
  End Property
  Public Property Code() As String
    Get
      Return GetProperty(Of String)(CodeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CodeProperty, value)
    End Set
  End Property
  Public Property ValidNumOfDays() As Integer
    Get
      Return GetProperty(Of Integer)(ValidNumOfDaysProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(ValidNumOfDaysProperty, value)
    End Set
  End Property
  Public Property PercentOfFullExam() As Integer
    Get
      Return GetProperty(Of Integer)(PercentOfFullExamProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(PercentOfFullExamProperty, value)
    End Set
  End Property
  Public Property IsInRegistar() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsInRegistarProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsInRegistarProperty, value)
    End Set
  End Property
  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods


#Region " Authorization Rules "

  Protected Overrides Sub AddAuthorizationRules()
    Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Description") Then
      AuthorizationRules.AllowWrite("Description", roleName)
    Else
      AuthorizationRules.DenyWrite("Description", roleName)
    End If
    'AuthorizationRules.AllowWrite("Description")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ValidNumOfDays") Then
      AuthorizationRules.AllowWrite("ValidNumOfDays", roleName)
    Else
      AuthorizationRules.DenyWrite("ValidNumOfDays", roleName)
    End If
    'AuthorizationRules.AllowWrite("ValidNumOfDays")
    If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PercentOfFullExam") Then
      AuthorizationRules.AllowWrite("PercentOfFullExam", roleName)
    Else
      AuthorizationRules.DenyWrite("PercentOfFullExam", roleName)
    End If
    'AuthorizationRules.AllowWrite("Active")
  End Sub



  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("TehnicalExamsType")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("TehnicalExamsType")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("TehnicalExamsType")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("TehnicalExamsType")
  End Function

#End Region ' Authorization Rules


#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' DescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, DescriptionProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(DescriptionProperty, 150))
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(CodeProperty, 10))
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.IntegerMinValue, _
                            New Csla.Validation.IntegerMinValueRuleArgs(PercentOfFullExamProperty, 0))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewTehnicalExamsType() As TehnicalExamsType
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a TehnicalExamsType")
    End If
    Return DataPortal.Create(Of TehnicalExamsType)()
  End Function

  Public Shared Function GetTehnicalExamsType(ByVal id As Integer) As TehnicalExamsType
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User not authorized to view a TehnicalExamsType")
    End If
    Return DataPortal.Fetch(New SingleCriteria(Of TehnicalExamsType, Integer)(id))
  End Function

  Public Shared Sub DeleteTehnicalExamsType(ByVal id As Integer)
    If Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a TehnicalExamsType")
    End If
    DataPortal.Delete(New SingleCriteria(Of TehnicalExamsType, Integer)(id))
  End Sub

  Public Overrides Function Save() As TehnicalExamsType
    If IsDeleted AndAlso Not CanDeleteObject() Then
      Throw New System.Security.SecurityException("User not authorized to remove a TehnicalExamsType")
    ElseIf IsNew AndAlso Not CanAddObject() Then
      Throw New System.Security.SecurityException("User not authorized to add a TehnicalExamsType")
    ElseIf Not CanEditObject() Then
      Throw New System.Security.SecurityException("User not authorized to update a TehnicalExamsType")
    End If
    Return MyBase.Save()
  End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

  Friend Shared Function NewTehnicalExamsTypeChild() As TehnicalExamsType
    Return DataPortal.CreateChild(Of TehnicalExamsType)()
  End Function

  Friend Shared Function GetTehnicalExamsType(ByVal dr As SafeDataReader) As TehnicalExamsType
    Return DataPortal.FetchChild(Of TehnicalExamsType)(dr)
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

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of TehnicalExamsType, Integer))
    Database.LogInfo("TehnicalExamsType.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetByID
          cm.Parameters.AddWithValue("@Id", criteria.Value)
          Using dr As New SafeDataReader(cm.ExecuteReader)
            dr.Read()

            LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
            LoadProperty(Of String)(DescriptionProperty, dr.GetString("Description"))
            LoadProperty(Of String)(CodeProperty, dr.GetString("Code"))
            LoadProperty(Of Integer)(ValidNumOfDaysProperty, dr.GetInt32("ValidNumOfDays"))
            LoadProperty(Of Integer)(PercentOfFullExamProperty, dr.GetInt32("PercentOfFullExam"))
            LoadProperty(Of Boolean)(IsInRegistarProperty, dr.GetBoolean("IsInRegistar"))
            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("TehnicalExamsType.DataPortal_Fetch", ex)
      Throw New DbCslaException("TehnicalExamsType.DataPortal_Fetch", ex)
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

            .Parameters.AddWithValue("@Description", ReadProperty(Of String)(DescriptionProperty))
            .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
            .Parameters.AddWithValue("@ValidNumOfDays", ReadProperty(Of Integer)(ValidNumOfDaysProperty))
            .Parameters.AddWithValue("@PercentOfFullExam", ReadProperty(Of Integer)(PercentOfFullExamProperty))
            .Parameters.AddWithValue("@IsInRegistar", ReadProperty(Of Boolean)(IsInRegistarProperty))
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
      Database.LogException("TehnicalExamsType.DataPortal_Insert", ex)
      Throw New DbCslaException("TehnicalExamsType.DataPortal_Insert", ex)
    Finally
      Database.LogInfo("TehnicalExamsType.DataPortal_Insert", GetHashCode())
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
            .Parameters.AddWithValue("@Description", ReadProperty(Of String)(DescriptionProperty))
            .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
            .Parameters.AddWithValue("@ValidNumOfDays", ReadProperty(Of Integer)(ValidNumOfDaysProperty))
            .Parameters.AddWithValue("@lastChanged", _lastChanged)
            .Parameters.AddWithValue("@PercentOfFullExam", ReadProperty(Of Integer)(PercentOfFullExamProperty))
            .Parameters.AddWithValue("@IsInRegistar", ReadProperty(Of Boolean)(IsInRegistarProperty))
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
    DataPortal_Delete(New SingleCriteria(Of TehnicalExamsType, Integer)(Id))
  End Sub

  Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of TehnicalExamsType, Integer))
    Database.LogInfo("TehnicalExamsType.DataPortal_Delete", GetHashCode())
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
      Database.LogException("TehnicalExamsType.DataPortal_Delete", ex)
      Throw New DbCslaException("TehnicalExamsType.DataPortal_Delete", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("TehnicalExamsType.Child_Fetch", GetHashCode())
    Try
      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of String)(DescriptionProperty, dr.GetString("Description"))
      LoadProperty(Of String)(CodeProperty, dr.GetString("Code"))
      LoadProperty(Of Integer)(ValidNumOfDaysProperty, dr.GetInt32("ValidNumOfDays"))
      LoadProperty(Of Integer)(PercentOfFullExamProperty, dr.GetInt32("PercentOfFullExam"))
      LoadProperty(Of Boolean)(IsInRegistarProperty, dr.GetBoolean("IsInRegistar"))
      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
    Catch ex As Exception
      Database.LogException("TehnicalExamsType.Child_Fetch", ex)
      Throw New DbCslaException("TehnicalExamsType.Child_Fetch", ex)
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
            .Parameters.AddWithValue("@Description", ReadProperty(Of String)(DescriptionProperty))
            .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
            .Parameters.AddWithValue("@ValidNumOfDays", ReadProperty(Of Integer)(ValidNumOfDaysProperty))
            .Parameters.AddWithValue("@PercentOfFullExam", ReadProperty(Of Integer)(PercentOfFullExamProperty))
            .Parameters.AddWithValue("@IsInRegistar", ReadProperty(Of Boolean)(IsInRegistarProperty))
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
      Database.LogException("TehnicalExamsType.Child_Insert", ex)
      Throw New DbCslaException("TehnicalExamsType.Child_Insert", ex)
    Finally
      Database.LogInfo("TehnicalExamsType.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update()
    Database.LogInfo("TehnicalExamsType.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        ApplicationContext.LocalContext.Remove("cn")
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
            .Parameters.AddWithValue("@Description", ReadProperty(Of String)(DescriptionProperty))
            .Parameters.AddWithValue("@Code", ReadProperty(Of String)(CodeProperty))
            .Parameters.AddWithValue("@ValidNumOfDays", ReadProperty(Of Integer)(ValidNumOfDaysProperty))
            .Parameters.AddWithValue("@PercentOfFullExam", ReadProperty(Of Integer)(PercentOfFullExamProperty))
            .Parameters.AddWithValue("@IsInRegistar", ReadProperty(Of Boolean)(IsInRegistarProperty))
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
      Database.LogException("TehnicalExamsType.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("TehnicalExamsType.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("TehnicalExamsType.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("TehnicalExamsType.Child_Fetch", ex)
      Throw New DbCslaException("TehnicalExamsType.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
