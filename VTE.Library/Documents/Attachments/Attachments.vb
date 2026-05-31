
<Serializable()> _
Public Class Attachments
    Inherits Csla.BusinessListBase(Of Attachments, Attachment)

#Region " Stored Procedures Names "
    Private Const spGetByID As String = "GetDocumentAttachmentByID"
    Private Const spGetAll As String = "GetDocumentAttachments"
    Private Const spUpdate As String = "updateDocumentAttachment"
    Private Const spAdd As String = "addDocumentAttachment"
    Private Const spDelete As String = "deleteDocumentAttachment"
    Private Const spGetByIdCustomer As String = "getDocumentAttachmentByIdCustomer"
    Private Const spGetByIdVehicle As String = "getDocumentAttachmentByIdVehicle"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

    Protected Overrides Function AddNewCore() As Object
        Dim item As Attachment = Attachment.NewAttachment()
        Me.Add(item)
        Return item
    End Function

#End Region ' BindingList Overrides

    '#Region " Authorization Rules "

    '    Public Shared Function CanGetObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentTypes")
    '    End Function

    '    Public Shared Function CanAddObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentTypes")
    '    End Function

    '    Public Shared Function CanEditObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentTypes")
    '    End Function

    '    Public Shared Function CanDeleteObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentTypes")
    '    End Function

    '#End Region ' Authorization Rules

#Region " Factory Methods "
    Private Sub New()
        AllowNew = True
    End Sub

    Public Shared Function GetAttachments() As Attachments
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User Not authorized to view a DocumentTypes")
        'End If
        Return DataPortal.Fetch(Of Attachments)()
    End Function
    Public Shared Function GetAttachmentByIdCustomer(ByVal inId As Long, ByVal isCustomer As Boolean) As Attachments
        Return DataPortal.Fetch(New filterCriteria(inId, isCustomer))
    End Function
    Public Function GetAttachmentById(ByVal idIn As Long) As Attachment
        For Each child As Attachment In Me
            If child.Id = idIn Then
                Return child
            End If
        Next
        Return Nothing
    End Function
#End Region ' Factory Methods

#Region " Data Access "
    <Serializable()> _
Private Class filterCriteria
        Private _idIn As Long
        Private _isCustomer As Boolean

        Public ReadOnly Property IdIn() As Long
            Get
                Return _idIn
            End Get
        End Property

        Public ReadOnly Property isCustomer() As Boolean
            Get
                Return _isCustomer
            End Get
        End Property
        Public Sub New(ByVal idIn As Long, ByVal isCustomer As Boolean)
            _idIn = idIn
            _isCustomer = isCustomer
        End Sub
    End Class
    Private Overloads Sub DataPortal_Fetch()
        RaiseListChangedEvents = False
        Database.LogInfo("Attachments.Child_Fetch", GetHashCode())
        Try

            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = spGetAll
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        While dr.Read()
                            Me.Add(Attachment.GetAttachment(dr))
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("Attachments.Child_Fetch", ex)
            Throw New DbCslaException("Attachments.Child_Fetch", ex)
        End Try
        RaiseListChangedEvents = True
    End Sub

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As filterCriteria)
        RaiseListChangedEvents = False
        Database.LogInfo("Attachments.Child_Fetch", GetHashCode())
        Try

            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.StoredProcedure
                    If criteria.isCustomer Then
                        cm.CommandText = spGetByIdCustomer
                        cm.Parameters.AddWithValue("@idCustomer", criteria.IdIn)
                    Else
                        cm.CommandText = spGetByIdVehicle
                        cm.Parameters.AddWithValue("@idVehicle", criteria.IdIn)
                    End If

                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        While dr.Read()
                            Me.Add(Attachment.GetAttachment(dr))
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("Attachments.Child_Fetch", ex)
            Throw New DbCslaException("Attachments.Child_Fetch", ex)
        End Try
        RaiseListChangedEvents = True
    End Sub

    Protected Overrides Sub DataPortal_Update()
        RaiseListChangedEvents = False
        Child_Update()
        RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access


End Class
