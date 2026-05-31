Imports System.Drawing
<Serializable()> _
Public Class Attachment
    Inherits Csla.BusinessBase(Of Attachment)


#Region " Stored Procedures Names "
    Private Const spGetByID As String = "GetDocumentAttachmentByID"
    Private Const spGetAll As String = "GetDocumentAttachments"
    Private Const spUpdate As String = "updateDocumentAttachment"
    Private Const spAdd As String = "addDocumentAttachment"
    Private Const spDelete As String = "deleteDocumentAttachment"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
    'register properties

    Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Attachment), New PropertyInfo(Of Long)("Id"))
    Private Shared IdDocumentProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Attachment), New PropertyInfo(Of Long)("IdDocument"))
    Private Shared IdAttachmentTypeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(Attachment), New PropertyInfo(Of Integer)("IdAttachmentType"))
    Private Shared AttachmentStatusProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Attachment), New PropertyInfo(Of String)("AttachmentStatus"))
    Private Shared AttachmentNotesProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Attachment), New PropertyInfo(Of String)("AttachmentNotes"))
    Private Shared AttachmentPathProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(Attachment), New PropertyInfo(Of String)("AttachmentPath"))
    Private Shared IdCustomerProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Attachment), New PropertyInfo(Of Long)("IdCustomer"))
    Private Shared IdVehicleProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(Attachment), New PropertyInfo(Of Long)("IdVehicle"))
  
    Private _lastChanged(7) As Byte

    <System.ComponentModel.DataObjectField(True, True)> _
    Public ReadOnly Property Id() As Long
        Get
            Return GetProperty(Of Long)(IdProperty)
        End Get
    End Property
    Public Property IdDocument() As Long
        Get
            Return GetProperty(Of Long)(IdDocumentProperty)
        End Get
        Set(ByVal value As Long)
            SetProperty(Of Long)(IdDocumentProperty, value)
        End Set
    End Property
   
    Public Property IdAttachmentType() As Integer
        Get
            Return GetProperty(Of Integer)(IdAttachmentTypeProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(IdAttachmentTypeProperty, value)
        End Set
    End Property

    Public Property AttachmentStatus() As String
        Get
            Return GetProperty(Of String)(AttachmentStatusProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(AttachmentStatusProperty, value)
        End Set
    End Property

    Public Property AttachmentNotes() As String
        Get
            Return GetProperty(Of String)(AttachmentNotesProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(AttachmentNotesProperty, value)
        End Set
    End Property

    Public Property AttachmentPath() As String
        Get
            Return GetProperty(Of String)(AttachmentPathProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(AttachmentPathProperty, value)
        End Set
    End Property

    Public Property IdCustomer() As Long
        Get
            Return GetProperty(Of Long)(IdCustomerProperty)
        End Get
        Set(ByVal value As Long)
            SetProperty(Of Long)(IdCustomerProperty, value)
        End Set
    End Property

    Public Property IdVehicle() As Long
        Get
            Return GetProperty(Of Long)(IdVehicleProperty)
        End Get
        Set(ByVal value As Long)
            SetProperty(Of Long)(IdVehicleProperty, value)
        End Set
    End Property
    Public ReadOnly Property AttachmentPicture() As Image
        Get
            Dim pateka As String = GetProperty(Of String)(AttachmentPathProperty)
            If pateka <> String.Empty Then
                Return Image.FromFile(pateka)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Id.ToString
    End Function
#End Region 'Business Properties and Methods


    '#Region " Authorization Rules "

    '    Protected Overrides Sub AddAuthorizationRules()
    '        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("DocumentTypeName") Then
    '            AuthorizationRules.AllowWrite("DocumentTypeName", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("DocumentTypeName", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("DocumentTypeName")
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsBidirectional") Then
    '            AuthorizationRules.AllowWrite("IsBidirectional", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("IsBidirectional", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("IsBidirectional")
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsVehiceRequired") Then
    '            AuthorizationRules.AllowWrite("IsVehiceRequired", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("IsVehiceRequired", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("IsVehiceRequired")
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsTechnicalExamRequired") Then
    '            AuthorizationRules.AllowWrite("IsTechnicalExamRequired", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("IsTechnicalExamRequired", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("IsTechnicalExamRequired")
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsPayRequired") Then
    '            AuthorizationRules.AllowWrite("IsPayRequired", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("IsPayRequired", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("IsPayRequired")
    '        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
    '            AuthorizationRules.AllowWrite("Active", roleName)
    '        Else
    '            AuthorizationRules.DenyWrite("Active", roleName)
    '        End If
    '        'AuthorizationRules.AllowWrite("Active")
    '    End Sub



    '    Public Shared Function CanGetObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentType")
    '    End Function

    '    Public Shared Function CanAddObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentType")
    '    End Function

    '    Public Shared Function CanEditObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentType")
    '    End Function

    '    Public Shared Function CanDeleteObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentType")
    '    End Function

    '#End Region ' Authorization Rules


#Region " Validation Rules "
    Protected Overrides Sub AddBusinessRules()
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, "AttachmentStatus")
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("AttachmentStatus", 50))
        ' AttachmentPath rules
        ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("AttachmentPath", 550))

    End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

    Private Sub New()
        ' require use of factory method 
    End Sub

    Public Shared Function NewAttachment() As Attachment
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a DocumentType")
        'End If
        Return DataPortal.Create(Of Attachment)()
    End Function

    Public Shared Function GetAttachment(ByVal id As Long) As Attachment
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a DocumentType")
        'End If
        Return DataPortal.Fetch(New SingleCriteria(Of Attachment, Long)(id))
    End Function

    Public Shared Sub DeleteAttachment(ByVal id As Long)
        'If Not CanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a DocumentType")
        'End If
        DataPortal.Delete(New SingleCriteria(Of Attachment, Long)(id))
    End Sub

    Public Overrides Function Save() As Attachment
        'If IsDeleted AndAlso Not CanDeleteObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to remove a DocumentType")
        'ElseIf IsNew AndAlso Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a DocumentType")
        'ElseIf Not CanEditObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to update a DocumentType")
        'End If
        Return MyBase.Save()
    End Function
#End Region ' Factory Methods

#Region " Child Factory Methods "

    Friend Shared Function NewAttachmentChild() As Attachment
        Return DataPortal.CreateChild(Of Attachment)()
    End Function

    Friend Shared Function GetAttachment(ByVal dr As SafeDataReader) As Attachment
        Return DataPortal.FetchChild(Of Attachment)(dr)
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

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of Attachment, Long))
        Database.LogInfo("Attachment.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = spGetByID
                    cm.Parameters.AddWithValue("@Id", criteria.Value)
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        dr.Read()

                        LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
                        LoadProperty(Of Long)(IdDocumentProperty, dr.GetInt64("IdDocument"))
                        LoadProperty(Of Integer)(IdAttachmentTypeProperty, dr.GetInt32("IdAttachmentType"))
                        LoadProperty(Of String)(AttachmentStatusProperty, dr.GetString("AttachmentStatus"))
                        LoadProperty(Of String)(AttachmentNotesProperty, dr.GetString("AttachmentNotes"))
                        LoadProperty(Of String)(AttachmentPathProperty, dr.GetString("AttachmentPath"))
                        LoadProperty(Of Long)(IdCustomerProperty, dr.GetInt64("IdCustomer"))
                        LoadProperty(Of Long)(IdVehicleProperty, dr.GetInt64("IdVehicle"))

                        dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
                    End Using
                End Using
               
            End Using

        Catch ex As Exception
            Database.LogException("Attachment.DataPortal_Fetch", ex)
            Throw New DbCslaException("Attachment.DataPortal_Fetch", ex)
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

                        .Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdDocumentProperty))
                        .Parameters.AddWithValue("@IdAttachmentType", ReadProperty(Of Integer)(IdAttachmentTypeProperty))
                        .Parameters.AddWithValue("@AttachmentStatus", ReadProperty(Of String)(AttachmentStatusProperty))
                        .Parameters.AddWithValue("@AttachmentNotes", ReadProperty(Of String)(AttachmentNotesProperty))
                        .Parameters.AddWithValue("@AttachmentPath", ReadProperty(Of String)(AttachmentPathProperty))
                        .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
                        .Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdVehicleProperty))

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
                FieldManager.UpdateChildren(Me)

                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("Attachment.DataPortal_Insert", ex)
            Throw New DbCslaException("Attachment.DataPortal_Insert", ex)
        Finally
            Database.LogInfo("Attachment.DataPortal_Insert", GetHashCode())
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
                        .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
                        .Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdDocumentProperty))
                        .Parameters.AddWithValue("@IdAttachmentType", ReadProperty(Of Integer)(IdAttachmentTypeProperty))
                        .Parameters.AddWithValue("@AttachmentStatus", ReadProperty(Of String)(AttachmentStatusProperty))
                        .Parameters.AddWithValue("@AttachmentNotes", ReadProperty(Of String)(AttachmentNotesProperty))
                        .Parameters.AddWithValue("@AttachmentPath", ReadProperty(Of String)(AttachmentPathProperty))
                        .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
                        .Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdVehicleProperty))
                        .Parameters.AddWithValue("@lastChanged", _lastChanged)
                        Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
                        param.Direction = ParameterDirection.Output
                        .Parameters.Add(param)

                        .ExecuteNonQuery()

                        _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
                    End With
                End Using

                'update child objects
                FieldManager.UpdateChildren(Me)

                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("Attachment.DataPortal_Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DBConcurrencyException("Attachment.DataPortal_Update", ex)
            End If
        End Try
    End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

    Protected Overrides Sub DataPortal_DeleteSelf()
        DataPortal_Delete(New SingleCriteria(Of DocumentType, Integer)(Id))
    End Sub

    Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of DocumentType, Integer))
        Database.LogInfo("Attachment.DataPortal_Delete", GetHashCode())
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
            Database.LogException("Attachment.DataPortal_Delete", ex)
            Throw New DbCslaException("Attachment.DataPortal_Delete", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access


#Region " Child Data Access "

#Region " Data Access - Fetch "

    Private Sub Child_Fetch(ByVal dr As SafeDataReader)
        Database.LogInfo("Attachment.Child_Fetch", GetHashCode())
        Try
            LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
            LoadProperty(Of Long)(IdDocumentProperty, dr.GetInt64("IdDocument"))
            LoadProperty(Of Integer)(IdAttachmentTypeProperty, dr.GetInt32("IdAttachmentType"))
            LoadProperty(Of String)(AttachmentStatusProperty, dr.GetString("AttachmentStatus"))
            LoadProperty(Of String)(AttachmentNotesProperty, dr.GetString("AttachmentNotes"))
            LoadProperty(Of String)(AttachmentPathProperty, dr.GetString("AttachmentPath"))
            LoadProperty(Of Long)(IdCustomerProperty, dr.GetInt64("IdCustomer"))
            LoadProperty(Of Long)(IdVehicleProperty, dr.GetInt64("IdVehicle"))

            dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
            
        Catch ex As Exception
            Database.LogException("Attachment.Child_Fetch", ex)
            Throw New DbCslaException("Attachment.Child_Fetch", ex)
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
                        .Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdDocumentProperty))
                        .Parameters.AddWithValue("@IdAttachmentType", ReadProperty(Of Integer)(IdAttachmentTypeProperty))
                        .Parameters.AddWithValue("@AttachmentStatus", ReadProperty(Of String)(AttachmentStatusProperty))
                        .Parameters.AddWithValue("@AttachmentNotes", ReadProperty(Of String)(AttachmentNotesProperty))
                        .Parameters.AddWithValue("@AttachmentPath", ReadProperty(Of String)(AttachmentPathProperty))
                        .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
                        .Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdVehicleProperty))

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
                FieldManager.UpdateChildren(Me)

                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("Attachment.Child_Insert", ex)
            Throw New DbCslaException("Attachment.Child_Insert", ex)
        Finally
            Database.LogInfo("Attachment.Child_Insert", GetHashCode)
        End Try

    End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

    Private Sub Child_Update()
        Database.LogInfo("Attachment.Child_Update", GetHashCode)
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                ApplicationContext.LocalContext("cn") = cn
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = spUpdate
                        .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
                        .Parameters.AddWithValue("@IdDocument", ReadProperty(Of Long)(IdDocumentProperty))
                        .Parameters.AddWithValue("@IdAttachmentType", ReadProperty(Of Integer)(IdAttachmentTypeProperty))
                        .Parameters.AddWithValue("@AttachmentStatus", ReadProperty(Of String)(AttachmentStatusProperty))
                        .Parameters.AddWithValue("@AttachmentNotes", ReadProperty(Of String)(AttachmentNotesProperty))
                        .Parameters.AddWithValue("@AttachmentPath", ReadProperty(Of String)(AttachmentPathProperty))
                        .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
                        .Parameters.AddWithValue("@IdVehicle", ReadProperty(Of Long)(IdVehicleProperty))
                        .Parameters.AddWithValue("@lastChanged", _lastChanged)
                        Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
                        param.Direction = ParameterDirection.Output
                        .Parameters.Add(param)

                        .ExecuteNonQuery()

                        _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
                    End With
                End Using

                'update child objects
                FieldManager.UpdateChildren(Me)

                If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
                    ApplicationContext.LocalContext.Remove("cn")
                End If
            End Using
        Catch ex As Exception
            Database.LogException("Attachment.Child_Update", ex)
            If Not ex.Message.EndsWith("drug korisnik") Then
                Throw New DbCslaException("Attachment.Child_Update", ex)
            End If
        End Try
    End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

    Private Sub Child_DeleteSelf()

        Database.LogInfo("Attachment.Child_DeleteSelf", GetHashCode)
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
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
            Database.LogException("Attachment.Child_Fetch", ex)
            Throw New DbCslaException("Attachment.Child_Fetch", ex)
        End Try
    End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

End Class
