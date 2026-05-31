

<Serializable()> _
Public Class FieldsPrivileges
    Inherits Csla.BusinessListBase(Of FieldsPrivileges, FieldsPrivilege)


#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "getFieldsPrivileges"
    Private Const SpZemiPoID As String = "getFieldsPrivilegeById"
    Private Const SpSnimi As String = "updateFieldsPrivilege"
    Private Const SpDodadi As String = "addFieldsPrivilege"
    Private Const SpIzbrisi As String = "deleteFieldsPrivilege"

#End Region

#Region " BindingList Overrides "

    Protected Overrides Function AddNewCore() As Object
        Dim item As FieldsPrivilege = FieldsPrivilege.NewFieldsPrivilegeChild()
        Me.Add(item)
        Return item
    End Function

#End Region ' BindingList Overrides

    '#Region " Authorization Rules "

    '    Public Shared Function CanGetObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanGet("FieldsPrivileges")
    '    End Function

    '    Public Shared Function CanAddObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanAdd("FieldsPrivileges")
    '    End Function

    '    Public Shared Function CanEditObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanEdit("FieldsPrivileges")
    '    End Function

    '    Public Shared Function CanDeleteObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanDelete("FieldsPrivileges")
    '    End Function

    '#End Region ' Authorization Rules

#Region " Factory Methods "
    Private Sub New()
        AllowNew = True
    End Sub

    'Public Shared Function GetFieldsPrivileges(ByVal idRole As Integer, ByVal idCSLAObject As Integer) As FieldsPrivileges
    Public Shared Function GetFieldsPrivileges() As FieldsPrivileges
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User Not authorized to view a FieldsPrivileges")
        'End If
        Return DataPortal.Fetch(Of FieldsPrivileges)()
    End Function

#End Region ' Factory Methods

#Region " Data Access "

    Private Overloads Sub DataPortal_Fetch()
        RaiseListChangedEvents = False
        Database.LogInfo("FieldsPrivileges.Child_Fetch", GetHashCode())
        Try

            Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = SpZemiSite
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        While dr.Read()
                            Me.Add(FieldsPrivilege.GetFieldsPrivilege(dr))
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("FieldsPrivileges.Child_Fetch", ex)
            Throw New DbCslaException("FieldsPrivileges.Child_Fetch", ex)
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
