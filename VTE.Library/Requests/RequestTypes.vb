
<Serializable()> _
Public Class RequestTypes
  Inherits Csla.BusinessListBase(Of RequestTypes, RequestType)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetRequestTypeByID"
  Private Const spGetAll As String = "GetRequestTypes"
  Private Const spUpdate As String = "updateRequestType"
  Private Const spAdd As String = "addRequestType"
  Private Const spDelete As String = "deleteRequestType"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As RequestType = RequestType.NewRequestTypeChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("RequestTypes")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("RequestTypes")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("RequestTypes")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("RequestTypes")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetRequestTypes() As RequestTypes
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a RequestTypes")
    End If
    Return DataPortal.Fetch(Of RequestTypes)()
  End Function

  Public Overrides Function Save() As RequestTypes
    Dim result As RequestTypes = MyBase.Save

    OnRequestTypesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("RequestTypes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(RequestType.GetRequestType(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("RequestTypes.Child_Fetch", ex)
      Throw New DbCslaException("RequestTypes.Child_Fetch", ex)
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
  Public Shared Event RequestTypesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnRequestTypesSaved(ByVal sender As RequestTypes, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent RequestTypesSaved(sender, e)
  End Sub
#End Region

End Class
