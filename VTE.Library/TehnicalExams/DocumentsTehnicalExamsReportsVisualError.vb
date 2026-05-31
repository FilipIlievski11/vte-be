
<Serializable()> _
Public Class DocumentsTehnicalExamsReportsVisualError
    Inherits Csla.BusinessBase(Of DocumentsTehnicalExamsReportsVisualError)

#Region " Stored Procedures Names "
    Private Const spGetByID As String = "getDocumentsTehnicalExamsReportsVisualErrorById "
    Private Const spGetAll As String = "getDocumentsTehnicalExamsReportsVisualErrors"
    Private Const spUpdate As String = "updateDocumentsTehnicalExamsReportsVisualError"
    Private Const spAdd As String = "addDocumentsTehnicalExamsReportsVisualError"
    Private Const spDelete As String = "deleteDocumentsTehnicalExamsReportsVisualError"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
    'register properties
    Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsTehnicalExamsReportsVisualError), New PropertyInfo(Of Long)("Id"))
    Private Shared idDocumentsTehnicalExamsReportsProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsTehnicalExamsReportsVisualError), New PropertyInfo(Of Long)("idDocumentsTehnicalExamsReports"))
    Private Shared VisualErrorProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsTehnicalExamsReportsVisualError), New PropertyInfo(Of String)("VisualError"))

    Private _lastChanged(7) As Byte

    <System.ComponentModel.DataObjectField(True, True)> _
    Public ReadOnly Property Id() As Long
        Get
            Return GetProperty(Of Long)(IdProperty)
        End Get
    End Property
    Public Property IdDocumentsTehnicalExamsReports() As Long
        Get
            Return GetProperty(Of Long)(idDocumentsTehnicalExamsReportsProperty)
        End Get
        Set(ByVal value As Long)
            SetProperty(Of Long)(idDocumentsTehnicalExamsReportsProperty, value)
        End Set
    End Property
    
    Public Property VisualError() As String
        Get
            Return GetProperty(Of String)(VisualErrorProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(VisualErrorProperty, value)
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Id.ToString
    End Function
#End Region 'Business Properties and Methods

    '#Region " Authorization Rules "

    '    Protected Overrides Sub AddAuthorizationRules()
    '        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdTehnicalExamsReports") Then
    '            AuthorizationRules.AllowWrite("IdTehnicalExamsReports", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("IdTehnicalExamsReports", roleName)
    '        End If

    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdTehnicalExamVehivlePart") Then
    '            AuthorizationRules.AllowWrite("IdTehnicalExamVehivlePart", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("IdTehnicalExamVehivlePart", roleName)
    '        End If

    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdStatus") Then
    '            AuthorizationRules.AllowWrite("IdStatus", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("IdStatus", roleName)
    '        End If

    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Front") Then
    '            AuthorizationRules.AllowWrite("Front", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("Front", roleName)
    '        End If

    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Back") Then
    '            AuthorizationRules.AllowWrite("Back", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("Back", roleName)
    '        End If

    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("OnLeft") Then
    '            AuthorizationRules.AllowWrite("OnLeft", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("OnLeft", roleName)
    '        End If

    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("OnRight") Then
    '            AuthorizationRules.AllowWrite("OnRight", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("OnRight", roleName)
    '        End If

    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DateEnter") Then
    '            AuthorizationRules.AllowWrite("DateEnter", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("DateEnter", roleName)
    '        End If

    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Note") Then
    '            AuthorizationRules.AllowWrite("Note", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("Note", roleName)
    '        End If

    '    End Sub



    '    Public Shared Function CanGetObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsTehnicalExamsReportsDetail")
    '    End Function

    '    Public Shared Function CanAddObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsTehnicalExamsReportsDetail")
    '    End Function

    '    Public Shared Function CanEditObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsTehnicalExamsReportsDetail")
    '    End Function

    '    Public Shared Function CanDeleteObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsTehnicalExamsReportsDetail")
    '    End Function

    '#End Region ' Authorization Rules

#Region " Validation Rules "
    Protected Overrides Sub AddBusinessRules()
        ' DateEnterProperty rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, VisualErrorProperty)
        ' NoteProperty rules
       
        'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
        '                        New Validation.IntegerMinValueRuleArgs(idDocumentsTehnicalExamsReportsProperty, 1))

    End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

    Friend Shared Function NewDocumentsTehnicalExamsReportsVisualErrorChild() As DocumentsTehnicalExamsReportsVisualError

        Return DataPortal.CreateChild(Of DocumentsTehnicalExamsReportsVisualError)()
    End Function

    Friend Shared Function GetDocumentsTehnicalExamsReportsVisualError(ByVal dr As SafeDataReader) As DocumentsTehnicalExamsReportsVisualError
        Return DataPortal.FetchChild(Of DocumentsTehnicalExamsReportsVisualError)(dr)
    End Function

    Public Shared Sub DeleteDocumentsTehnicalExamsReportsVisualError(ByVal id As Long)

        DataPortal.Delete(New SingleCriteria(Of DocumentsTehnicalExamsReportsVisualError, Integer)(id))
    End Sub

    Public Overrides Function Save() As DocumentsTehnicalExamsReportsVisualError

        Return MyBase.Save()
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
        Database.LogInfo("DocumentsTehnicalExamsReportsVisualError.Child_Fetch", GetHashCode())
        Try

            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of Long)(idDocumentsTehnicalExamsReportsProperty, dr.GetInt64("IdDocumentsTehnicalExamsReports"))
            LoadProperty(Of String)(VisualErrorProperty, dr.GetString("VisualError"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

            ValidationRules.CheckRules()

        Catch ex As Exception
            Database.LogException("DocumentsTehnicalExamsReportsVisualError.Child_Fetch", ex)
            Throw New DbCslaException("DocumentsTehnicalExamsReportsVisualError.Child_Fetch", ex)
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
                        .Parameters.AddWithValue("@idDocumentsTehnicalExamsReports", parent.Id)
                        .Parameters.AddWithValue("@VisualError", ReadProperty(Of String)(VisualErrorProperty))

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
            Database.LogException("DocumentsTehnicalExamsReportsVisualError.Child_Insert", ex)
            Throw New DbCslaException("DocumentsTehnicalExamsReportsVisualError.Child_Insert", ex)
        Finally
            Database.LogInfo("DocumentsTehnicalExamsReportsVisualError.Child_Insert", GetHashCode)
        End Try

    End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

    Private Sub Child_Update(ByVal parent As DocumentsTehnicalExamsReport)
        Database.LogInfo("DocumentsTehnicalExamsReportsVisualError.Child_Update", GetHashCode)
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
                        .Parameters.AddWithValue("@idDocumentsTehnicalExamsReports", parent.Id)
                        .Parameters.AddWithValue("@VisualError", ReadProperty(Of String)(VisualErrorProperty))
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
            Database.LogException("DocumentsTehnicalExamsReportsVisualError.Child_Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DbCslaException("DocumentsTehnicalExamsReportsVisualError.Child_Update", ex)
            End If
        End Try
    End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

    Private Sub Child_DeleteSelf()

        Database.LogInfo("DocumentsTehnicalExamsReportsVisualError.Child_DeleteSelf", GetHashCode)
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
            Database.LogException("DocumentsTehnicalExamsReportsVisualError.Child_Fetch", ex)
            Throw New DbCslaException("DocumentsTehnicalExamsReportsVisualError.Child_Fetch", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
