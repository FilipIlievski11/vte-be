
<Serializable()> _
Public Class DocumentsTehnicalExamsReportsDetail
  Inherits Csla.BusinessBase(Of DocumentsTehnicalExamsReportsDetail)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentsTehnicalExamsReportsDetailByID"
  Private Const spGetAll As String = "GetDocumentsTehnicalExamsReportsDetails"
  Private Const spUpdate As String = "updateDocumentsTehnicalExamsReportsDetail"
  Private Const spAdd As String = "addDocumentsTehnicalExamsReportsDetail"
  Private Const spDelete As String = "deleteDocumentsTehnicalExamsReportsDetail"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsTehnicalExamsReportsDetail), New PropertyInfo(Of Long)("Id"))
  Private Shared IdTehnicalExamsReportsProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsTehnicalExamsReportsDetail), New PropertyInfo(Of Long)("IdTehnicalExamsReports"))
  Private Shared IdTehnicalExamVehivlePartProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsTehnicalExamsReportsDetail), New PropertyInfo(Of Integer)("IdTehnicalExamVehivlePart"))
  Private Shared IdStatusProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsTehnicalExamsReportsDetail), New PropertyInfo(Of Integer)("IdStatus"))
  Private Shared FrontProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentsTehnicalExamsReportsDetail), New PropertyInfo(Of Boolean)("Front"))
  Private Shared BackProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentsTehnicalExamsReportsDetail), New PropertyInfo(Of Boolean)("Back"))
  Private Shared OnLeftProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentsTehnicalExamsReportsDetail), New PropertyInfo(Of Boolean)("OnLeft"))
  Private Shared OnRightProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentsTehnicalExamsReportsDetail), New PropertyInfo(Of Boolean)("OnRight"))
  Private Shared DateEnterProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(DocumentsTehnicalExamsReportsDetail), New PropertyInfo(Of SmartDate)("DateEnter", "DateEnter", New SmartDate(DateTime.Today, True)))
  Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsTehnicalExamsReportsDetail), New PropertyInfo(Of String)("Note"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Long
    Get
      Return GetProperty(Of Long)(IdProperty)
    End Get
  End Property
  Public Property IdTehnicalExamsReports() As Long
    Get
      Return GetProperty(Of Long)(IdTehnicalExamsReportsProperty)
    End Get
    Set(ByVal value As Long)
      SetProperty(Of Long)(IdTehnicalExamsReportsProperty, value)
    End Set
  End Property
  Public Property IdTehnicalExamVehivlePart() As Integer
    Get
      Return GetProperty(Of Integer)(IdTehnicalExamVehivlePartProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdTehnicalExamVehivlePartProperty, value)
    End Set
  End Property
  Public Property IdStatus() As Integer
    Get
      Return GetProperty(Of Integer)(IdStatusProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdStatusProperty, value)
    End Set
  End Property
  Public Property Front() As Boolean
    Get
      Return GetProperty(Of Boolean)(FrontProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(FrontProperty, value)
    End Set
  End Property
  Public Property Back() As Boolean
    Get
      Return GetProperty(Of Boolean)(BackProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(BackProperty, value)
    End Set
  End Property
  Public Property OnLeft() As Boolean
    Get
      Return GetProperty(Of Boolean)(OnLeftProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(OnLeftProperty, value)
    End Set
  End Property
  Public Property OnRight() As Boolean
    Get
      Return GetProperty(Of Boolean)(OnRightProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(OnRightProperty, value)
    End Set
  End Property
  Public Property DateEnter() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(DateEnterProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(DateEnterProperty, value)
    End Set
  End Property
  Public Property Note() As String
    Get
      Return GetProperty(Of String)(NoteProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(NoteProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdTehnicalExamsReports") Then
            AuthorizationRules.AllowWrite("IdTehnicalExamsReports", roleName)
        Else
            AuthorizationRules.DenyWrite("IdTehnicalExamsReports", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdTehnicalExamVehivlePart") Then
            AuthorizationRules.AllowWrite("IdTehnicalExamVehivlePart", roleName)
        Else
            AuthorizationRules.DenyWrite("IdTehnicalExamVehivlePart", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdStatus") Then
            AuthorizationRules.AllowWrite("IdStatus", roleName)
        Else
            AuthorizationRules.DenyWrite("IdStatus", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Front") Then
            AuthorizationRules.AllowWrite("Front", roleName)
        Else
            AuthorizationRules.DenyWrite("Front", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Back") Then
            AuthorizationRules.AllowWrite("Back", roleName)
        Else
            AuthorizationRules.DenyWrite("Back", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("OnLeft") Then
            AuthorizationRules.AllowWrite("OnLeft", roleName)
        Else
            AuthorizationRules.DenyWrite("OnLeft", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("OnRight") Then
            AuthorizationRules.AllowWrite("OnRight", roleName)
        Else
            AuthorizationRules.DenyWrite("OnRight", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DateEnter") Then
            AuthorizationRules.AllowWrite("DateEnter", roleName)
        Else
            AuthorizationRules.DenyWrite("DateEnter", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Note") Then
            AuthorizationRules.AllowWrite("Note", roleName)
        Else
            AuthorizationRules.DenyWrite("Note", roleName)
        End If

    End Sub



    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsTehnicalExamsReportsDetail")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsTehnicalExamsReportsDetail")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsTehnicalExamsReportsDetail")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsTehnicalExamsReportsDetail")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' DateEnterProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, DateEnterProperty)
    ' NoteProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NoteProperty, 150))

    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                            New Validation.IntegerMinValueRuleArgs(IdStatusProperty, 1))
    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                            New Validation.IntegerMinValueRuleArgs(IdTehnicalExamVehivlePartProperty, 1))

  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewDocumentsTehnicalExamsReportsDetailChild() As DocumentsTehnicalExamsReportsDetail
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a DocumentsTehnicalExamsReportsDetail")
        End If
        Return DataPortal.CreateChild(Of DocumentsTehnicalExamsReportsDetail)()
  End Function

  Friend Shared Function GetDocumentsTehnicalExamsReportsDetail(ByVal dr As SafeDataReader) As DocumentsTehnicalExamsReportsDetail
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a DocumentsTehnicalExamsReportsDetail")
        End If
        Return DataPortal.FetchChild(Of DocumentsTehnicalExamsReportsDetail)(dr)
  End Function

    Public Shared Sub DeleteDocumentsTehnicalExamsReportsDetail(ByVal id As Long)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a DocumentsTehnicalExamsReportsDetail")
        End If
        DataPortal.Delete(New SingleCriteria(Of DocumentsTehnicalExamsReportsDetail, Integer)(id))
    End Sub

    Public Overrides Function Save() As DocumentsTehnicalExamsReportsDetail
        If IsDeleted AndAlso Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a DocumentsTehnicalExamsReportsDetail")
        ElseIf IsNew AndAlso Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a DocumentsTehnicalExamsReportsDetail")
        ElseIf Not CanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a DocumentsTehnicalExamsReportsDetail")
        End If
        Return MyBase.Save()
    End Function

  Private Sub New()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Create "
  Protected Overloads Sub Child_Create()
    Me.IdStatus = 3
    ValidationRules.CheckRules()
  End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("DocumentsTehnicalExamsReportsDetail.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Long)(IdTehnicalExamsReportsProperty, dr.GetInt64("IdTehnicalExamsReports"))
      LoadProperty(Of Integer)(IdTehnicalExamVehivlePartProperty, dr.GetInt32("IdTehnicalExamVehivlePart"))
      LoadProperty(Of Integer)(IdStatusProperty, dr.GetInt32("IdStatus"))
      LoadProperty(Of Boolean)(FrontProperty, dr.GetBoolean("Front"))
      LoadProperty(Of Boolean)(BackProperty, dr.GetBoolean("Back"))
      LoadProperty(Of Boolean)(OnLeftProperty, dr.GetBoolean("OnLeft"))
      LoadProperty(Of Boolean)(OnRightProperty, dr.GetBoolean("OnRight"))
      LoadProperty(Of SmartDate, Date?)(DateEnterProperty, dr.GetSmartDate("DateEnter", True))
      LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("DocumentsTehnicalExamsReportsDetail.Child_Fetch", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportsDetail.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As DocumentsTehnicalExamsReport)
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
            .Parameters.AddWithValue("@IdTehnicalExamsReports", parent.Id)
            .Parameters.AddWithValue("@IdTehnicalExamVehivlePart", ReadProperty(Of Integer)(IdTehnicalExamVehivlePartProperty))
            .Parameters.AddWithValue("@IdStatus", ReadProperty(Of Integer)(IdStatusProperty))
            .Parameters.AddWithValue("@Front", ReadProperty(Of Boolean)(FrontProperty))
            .Parameters.AddWithValue("@Back", ReadProperty(Of Boolean)(BackProperty))
            .Parameters.AddWithValue("@OnLeft", ReadProperty(Of Boolean)(OnLeftProperty))
            .Parameters.AddWithValue("@OnRight", ReadProperty(Of Boolean)(OnRightProperty))
            .Parameters.AddWithValue("@DateEnter", ReadProperty(Of SmartDate)(DateEnterProperty).DBValue)
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))

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
      Database.LogException("DocumentsTehnicalExamsReportsDetail.Child_Insert", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportsDetail.Child_Insert", ex)
    Finally
      Database.LogInfo("DocumentsTehnicalExamsReportsDetail.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As DocumentsTehnicalExamsReport)
    Database.LogInfo("DocumentsTehnicalExamsReportsDetail.Child_Update", GetHashCode)
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
            .Parameters.AddWithValue("@IdTehnicalExamsReports", parent.Id)
            .Parameters.AddWithValue("@IdTehnicalExamVehivlePart", ReadProperty(Of Integer)(IdTehnicalExamVehivlePartProperty))
            .Parameters.AddWithValue("@IdStatus", ReadProperty(Of Integer)(IdStatusProperty))
            .Parameters.AddWithValue("@Front", ReadProperty(Of Boolean)(FrontProperty))
            .Parameters.AddWithValue("@Back", ReadProperty(Of Boolean)(BackProperty))
            .Parameters.AddWithValue("@OnLeft", ReadProperty(Of Boolean)(OnLeftProperty))
            .Parameters.AddWithValue("@OnRight", ReadProperty(Of Boolean)(OnRightProperty))
            .Parameters.AddWithValue("@DateEnter", ReadProperty(Of SmartDate)(DateEnterProperty).DBValue)
            .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
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
      Database.LogException("DocumentsTehnicalExamsReportsDetail.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("DocumentsTehnicalExamsReportsDetail.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("DocumentsTehnicalExamsReportsDetail.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("DocumentsTehnicalExamsReportsDetail.Child_Fetch", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportsDetail.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
