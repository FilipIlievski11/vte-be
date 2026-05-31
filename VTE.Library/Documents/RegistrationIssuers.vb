
<Serializable()> _
Public Class RegistrationIssuers
  Inherits Csla.BusinessListBase(Of RegistrationIssuers, RegistrationIssuer)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetRegistrationIssuerByID"
  Private Const spGetAll As String = "GetRegistrationIssuers"
  Private Const spUpdate As String = "updateRegistrationIssuer"
  Private Const spAdd As String = "addRegistrationIssuer"
  Private Const spDelete As String = "deleteRegistrationIssuer"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As RegistrationIssuer = RegistrationIssuer.NewRegistrationIssuerChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("RegistrationIssuers")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("RegistrationIssuers")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("RegistrationIssuers")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("RegistrationIssuers")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetRegistrationIssuers() As RegistrationIssuers
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a RegistrationIssuers")
    End If
    Return DataPortal.Fetch(Of RegistrationIssuers)()
  End Function

  Public Shared Function NewRegistrationIssuers() As RegistrationIssuers
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a RegistrationIssuers")
    End If
    Return DataPortal.Create(Of RegistrationIssuers)()
  End Function

    Public Overrides Function Save() As RegistrationIssuers

        Dim result As RegistrationIssuers = MyBase.Save()

        OnRegistrationIssuersSaved(Me, New Csla.Core.SavedEventArgs(result))

        Return result

    End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("RegistrationIssuers.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(RegistrationIssuer.GetRegistrationIssuer(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("RegistrationIssuers.Child_Fetch", ex)
      Throw New DbCslaException("RegistrationIssuers.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


  Protected Overrides Sub DataPortal_Update()
    RaiseListChangedEvents = False
    Child_Update()
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access

#Region " Readonlylist refresh "
    Public Shared Event RegistrationIssuersSaved As EventHandler(Of Csla.Core.SavedEventArgs)
    Protected Shared Sub OnRegistrationIssuersSaved(ByVal sender As RegistrationIssuers, ByVal e As Csla.Core.SavedEventArgs)
        RaiseEvent RegistrationIssuersSaved(sender, e)
    End Sub
#End Region
End Class
