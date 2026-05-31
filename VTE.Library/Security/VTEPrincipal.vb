
Imports System.Security.Principal

Namespace Security

  <Serializable()> _
Public Class VTEPrincipal
    Inherits Csla.Security.BusinessPrincipalBase



    Private Sub New(ByVal identity As IIdentity)
      MyBase.New(identity)
    End Sub

    Public Shared Function Login(ByVal username As String, ByVal password As String) As Boolean

      Return SetPrincipal(VTEIdentity.GetIdentity(username, password))

    End Function

    Public Shared Sub LoadPrincipal(ByVal username As String)

      SetPrincipal(VTEIdentity.GetIdentity(username))

    End Sub

    Private Shared Function SetPrincipal(ByVal identity As VTEIdentity) As Boolean

      If identity.IsAuthenticated Then
        Dim principal As VTEPrincipal = New VTEPrincipal(identity)
        Csla.ApplicationContext.User = principal
      End If
      Return identity.IsAuthenticated

    End Function

    Public Shared Sub Logout()

      Dim identity As VTEIdentity = VTEIdentity.UnauthenticatedIdentity()
      Dim principal As VTEPrincipal = New VTEPrincipal(identity)
      Csla.ApplicationContext.User = principal

    End Sub

    Public Overrides Function IsInRole(ByVal role As String) As Boolean

      Dim identity As VTEIdentity = CType(Me.Identity, VTEIdentity)
      Return identity.IsInRole(role)

    End Function

    Public Function IsInObjectPrivilegesCanAdd(ByVal strCSLAObjectName As String) As Boolean
      Dim identity As VTEIdentity = CType(Me.Identity, VTEIdentity)
      For Each item As ObjectPrivilegeInfo In identity.ObjectPrivileges
        If item.CSLAObjectName = strCSLAObjectName Then
          If item.CanAddObject Then
            Return True
          Else
            Return False
          End If
        End If
      Next
      Return False
    End Function

    Public Function IsInObjectPrivilegesCanDelete(ByVal strCSLAObjectName As String) As Boolean
      Dim identity As VTEIdentity = CType(Me.Identity, VTEIdentity)
      For Each item As ObjectPrivilegeInfo In identity.ObjectPrivileges
        If item.CSLAObjectName = strCSLAObjectName Then
          If item.CanDeleteObject Then
            Return True
          Else
            Return False
          End If
        End If
      Next
      Return False
    End Function

    Public Function IsInObjectPrivilegesCanEdit(ByVal strCSLAObjectName As String) As Boolean
      Dim identity As VTEIdentity = CType(Me.Identity, VTEIdentity)
      For Each item As ObjectPrivilegeInfo In identity.ObjectPrivileges
        If item.CSLAObjectName = strCSLAObjectName Then
          If item.CanEditObject Then
            Return True
          Else
            Return False
          End If
        End If
      Next
      Return False
    End Function

    Public Function IsInObjectPrivilegesCanGet(ByVal strCSLAObjectName As String) As Boolean
      Dim identity As VTEIdentity = CType(Me.Identity, VTEIdentity)
      For Each item As ObjectPrivilegeInfo In identity.ObjectPrivileges
        If item.CSLAObjectName = strCSLAObjectName Then
                    If item.CanGetObject Then
                        Return True
                    Else
                        Return False
                    End If
        End If
      Next
      Return False
    End Function

    Public Function IsInFieldsPrivilegesCanWrite(ByVal strCSLAObjectName As String) As Boolean
      Dim identity As VTEIdentity = CType(Me.Identity, VTEIdentity)

      For Each item As FieldsPrivilegeInfo In identity.FildPrivileges
        If IsInObjectPrivilegesCanAdd(item.CSLAObjectName) Or IsInObjectPrivilegesCanEdit(item.CSLAObjectName) Then
          If item.CSLAObjectPropertyName = strCSLAObjectName Then
            Return False
          End If
        Else
          Return False
        End If
      Next
      Return True

    End Function

    Public Function GetCurrrentRoleName() As String
      Dim identity As VTEIdentity = CType(Me.Identity, VTEIdentity)
      Return identity.RoleName
    End Function

    Public Function GetVteConnectionString() As String
      Dim identity As VTEIdentity = CType(Me.Identity, VTEIdentity)
      Return identity.VteConnectionString
    End Function

    Public Shared Function VerifyOtherIdentity(ByVal username As String, ByVal password As String) As Integer
      Return VTEIdentity.VerifyIdentity(username, password)
    End Function

  End Class

End Namespace

