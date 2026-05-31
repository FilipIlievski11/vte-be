
<Serializable()> _
Public Class Rools
    Inherits Csla.BusinessListBase(Of Rools, Rool)

#Region " Stored Procedures Names "
    Private Const spGetByID As String = "GetRoleByID"
    Private Const spGetAll As String = "GetRoles"
    Private Const spUpdate As String = "updateRole"
    Private Const spAdd As String = "addRole"
    Private Const spDelete As String = "deleteRole"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

    Protected Overrides Function AddNewCore() As Object
        Dim item As Rool = Rool.NewRool()
        Me.Add(item)
        Return item
    End Function

#End Region ' BindingList Overrides

    '#Region " Authorization Rules "

    '    Public Shared Function CanGetObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInRoolsCanGet("Rools")
    '    End Function

    '    Public Shared Function CanAddObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInRoolsCanAdd("Rools")
    '    End Function

    '    Public Shared Function CanEditObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInRoolsCanEdit("Rools")
    '    End Function

    '    Public Shared Function CanDeleteObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInRoolsCanDelete("Rools")
    '    End Function

    '#End Region ' Authorization Rules

#Region " Factory Methods "
    Private Sub New()
        AllowNew = True
    End Sub

    'Public Shared Function GetRools(ByVal idRole As Integer, ByVal idCSLAObject As Integer) As Rools
    Public Shared Function GetRools() As Rools
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User Not authorized to view a Rools")
        'End If
        Return DataPortal.Fetch(Of Rools)()
    End Function

#End Region ' Factory Methods

#Region " Data Access "

    Private Overloads Sub DataPortal_Fetch()
        RaiseListChangedEvents = False
        Database.LogInfo("Rools.Child_Fetch", GetHashCode())
        Try

            Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = spGetAll
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        While dr.Read()
                            Me.Add(Rool.GetRool(dr))
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("Rools.Child_Fetch", ex)
            Throw New DbCslaException("Rools.Child_Fetch", ex)
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
