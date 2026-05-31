
<Serializable()> _
Public Class ObjectPrivileges
  Inherits Csla.BusinessListBase(Of ObjectPrivileges, ObjectPrivilege)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetObjectPrivilegeByID"
  Private Const spGetAll As String = "GetObjectPrivileges"
  Private Const spUpdate As String = "updateObjectPrivilege"
  Private Const spAdd As String = "addObjectPrivilege"
  Private Const spDelete As String = "deleteObjectPrivilege"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As ObjectPrivilege = ObjectPrivilege.NewObjectPrivilegeChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("ObjectPrivileges")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("ObjectPrivileges")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("ObjectPrivileges")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("ObjectPrivileges")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

    'Public Shared Function GetObjectPrivileges(ByVal idRole As Integer, ByVal idCSLAObject As Integer) As ObjectPrivileges
    Public Shared Function GetObjectPrivileges() As ObjectPrivileges
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User Not authorized to view a ObjectPrivileges")
        End If
        Return DataPortal.Fetch(Of ObjectPrivileges)()
    End Function

#End Region ' Factory Methods

#Region " Data Access "

    Private Overloads Sub DataPortal_Fetch()
        RaiseListChangedEvents = False
        Database.LogInfo("ObjectPrivileges.Child_Fetch", GetHashCode())
        Try

            Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = spGetAll
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        While dr.Read()
                            Me.Add(ObjectPrivilege.GetObjectPrivilege(dr))
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("ObjectPrivileges.Child_Fetch", ex)
            Throw New DbCslaException("ObjectPrivileges.Child_Fetch", ex)
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
