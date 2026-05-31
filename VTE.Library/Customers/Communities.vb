
<Serializable()> _
Public Class Communities
  Inherits Csla.BusinessListBase(Of Communities, Community)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCommunitieByID"
  Private Const spGetAll As String = "GetCommunities"
  Private Const spUpdate As String = "updateCommunitie"
  Private Const spAdd As String = "addCommunitie"
  Private Const spDelete As String = "deleteCommunitie"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As Community = Community.NewCommunityChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Communities")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Communities")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Communities")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Communities")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetCommunities() As Communities
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a Communities")
    End If
    Return DataPortal.Fetch(Of Communities)()
  End Function


  Public Overrides Function Save() As Communities
    Dim result As Communities = MyBase.Save()

    OnCommunitiesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("Communities.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(Community.GetCommunity(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("Communities.Child_Fetch", ex)
      Throw New DbCslaException("Communities.Child_Fetch", ex)
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
  Public Shared Event CommunitiesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnCommunitiesSaved(ByVal sender As Communities, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent CommunitiesSaved(sender, e)
  End Sub
#End Region

End Class
