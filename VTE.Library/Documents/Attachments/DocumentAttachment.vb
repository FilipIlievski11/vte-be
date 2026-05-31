Imports System.Drawing
<Serializable()> _
Public Class DocumentAttachment
  Inherits Csla.BusinessBase(Of DocumentAttachment)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentAttachmentByID"
  Private Const spGetAll As String = "GetDocumentAttachments"
  Private Const spUpdate As String = "updateDocumentAttachment"
  Private Const spAdd As String = "addDocumentAttachment"
  Private Const spDelete As String = "deleteDocumentAttachment"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentAttachment), New PropertyInfo(Of Long)("Id"))
  Private Shared IdDocumentProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentAttachment), New PropertyInfo(Of Long)("IdDocument"))
  Private Shared IdAttachmentTypeProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentAttachment), New PropertyInfo(Of Integer)("IdAttachmentType", "IdAttachmentType", 1))
  Private Shared AttachmentStatusProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentAttachment), New PropertyInfo(Of String)("AttachmentStatus"))
  Private Shared AttachmentNotesProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentAttachment), New PropertyInfo(Of String)("AttachmentNotes"))
  Private Shared AttachmentPathProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentAttachment), New PropertyInfo(Of String)("AttachmentPath"))

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

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdDocument") Then
            AuthorizationRules.AllowWrite("IdDocument", roleName)
        Else
            AuthorizationRules.DenyWrite("IdDocument", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdAttachmentType") Then
            AuthorizationRules.AllowWrite("IdAttachmentType", roleName)
        Else
            AuthorizationRules.DenyWrite("IdAttachmentType", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("AttachmentStatus") Then
            AuthorizationRules.AllowWrite("AttachmentStatus", roleName)
        Else
            AuthorizationRules.DenyWrite("AttachmentStatus", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("AttachmentNotes") Then
            AuthorizationRules.AllowWrite("AttachmentNotes", roleName)
        Else
            AuthorizationRules.DenyWrite("AttachmentNotes", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("AttachmentPath") Then
            AuthorizationRules.AllowWrite("AttachmentPath", roleName)
        Else
            AuthorizationRules.DenyWrite("AttachmentPath", roleName)
        End If

    End Sub



    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentAttachment")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentAttachment")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentAttachment")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentAttachment")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' AttachmentStatusProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, AttachmentStatusProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(AttachmentStatusProperty, 50))
    ' AttachmentNotesProperty rules
    'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, AttachmentNotesProperty)
    ' AttachmentPathProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(AttachmentPathProperty, 550))
    '
    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                            New Csla.Validation.IntegerMinValueRuleArgs(IdAttachmentTypeProperty, 1))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

    Friend Shared Function NewDocumentAttachmentChild() As DocumentAttachment
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a DocumentAttachment")
        'End If
        Return DataPortal.CreateChild(Of DocumentAttachment)()
    End Function

    Friend Shared Function GetDocumentAttachment(ByVal dr As SafeDataReader) As DocumentAttachment
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a DocumentAttachment")
        'End If
        Return DataPortal.FetchChild(Of DocumentAttachment)(dr)
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
    Database.LogInfo("DocumentAttachment.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Long)(IdDocumentProperty, dr.GetInt64("IdDocument"))
      LoadProperty(Of Integer)(IdAttachmentTypeProperty, dr.GetInt32("IdAttachmentType"))
      LoadProperty(Of String)(AttachmentStatusProperty, dr.GetString("AttachmentStatus"))
      LoadProperty(Of String)(AttachmentNotesProperty, dr.GetString("AttachmentNotes"))
      LoadProperty(Of String)(AttachmentPathProperty, dr.GetString("AttachmentPath"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("DocumentAttachment.Child_Fetch", ex)
      Throw New DbCslaException("DocumentAttachment.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As Document)
    Try
      Dim cn As SqlConnection = ApplicationContext.LocalContext("cn")
      If cn.State = ConnectionState.Closed Then
        cn.ConnectionString = Database.VTEConnection
        cn.Open()
      End If
      Using cm As SqlCommand = cn.CreateCommand
        With cm
          .CommandType = CommandType.StoredProcedure
          .CommandText = spAdd
          'Smeni go Id so Parent.Id
          .Parameters.AddWithValue("@IdDocument", parent.Id)
          .Parameters.AddWithValue("@IdAttachmentType", ReadProperty(Of Integer)(IdAttachmentTypeProperty))
          .Parameters.AddWithValue("@AttachmentStatus", ReadProperty(Of String)(AttachmentStatusProperty))
          .Parameters.AddWithValue("@AttachmentNotes", ReadProperty(Of String)(AttachmentNotesProperty))
          .Parameters.AddWithValue("@AttachmentPath", ReadProperty(Of String)(AttachmentPathProperty))

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

    Catch ex As Exception
      Database.LogException("DocumentAttachment.Child_Insert", ex)
      Throw New DbCslaException("DocumentAttachment.Child_Insert", ex)
    Finally
      Database.LogInfo("DocumentAttachment.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As Document)
    Database.LogInfo("DocumentAttachment.Child_Update", GetHashCode)
    Try
      Dim cn As SqlConnection = ApplicationContext.LocalContext("cn")
      If cn.State = ConnectionState.Closed Then
        cn.ConnectionString = Database.VTEConnection
        cn.Open()
      End If
      Using cm As SqlCommand = cn.CreateCommand
        With cm
          .CommandType = CommandType.StoredProcedure
          .CommandText = spUpdate

          .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
          .Parameters.AddWithValue("@IdDocument", parent.Id)
          .Parameters.AddWithValue("@IdAttachmentType", ReadProperty(Of Integer)(IdAttachmentTypeProperty))
          .Parameters.AddWithValue("@AttachmentStatus", ReadProperty(Of String)(AttachmentStatusProperty))
          .Parameters.AddWithValue("@AttachmentNotes", ReadProperty(Of String)(AttachmentNotesProperty))
          .Parameters.AddWithValue("@AttachmentPath", ReadProperty(Of String)(AttachmentPathProperty))
          .Parameters.AddWithValue("@lastChanged", _lastChanged)
          Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
          param.Direction = ParameterDirection.Output
          .Parameters.Add(param)

          .ExecuteNonQuery()

          _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
        End With
      End Using
      'update child objects

    Catch ex As Exception
      Database.LogException("DocumentAttachment.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("DocumentAttachment.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("DocumentAttachment.Child_DeleteSelf", GetHashCode)
    Try
      Dim cn As SqlConnection = Csla.ApplicationContext.LocalContext("cn")
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


    Catch ex As Exception
      Database.LogException("DocumentAttachment.Child_Fetch", ex)
      Throw New DbCslaException("DocumentAttachment.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
