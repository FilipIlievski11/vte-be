
<Serializable()> _
Public Class TehnicalExamsTypes
  Inherits Csla.BusinessListBase(Of TehnicalExamsTypes, TehnicalExamsType)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetTehnicalExamsTypeByID"
  Private Const spGetAll As String = "GetTehnicalExamsTypes"
  Private Const spUpdate As String = "updateTehnicalExamsType"
  Private Const spAdd As String = "addTehnicalExamsType"
  Private Const spDelete As String = "deleteTehnicalExamsType"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As TehnicalExamsType = TehnicalExamsType.NewTehnicalExamsTypeChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("TehnicalExamsTypes")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("TehnicalExamsTypes")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("TehnicalExamsTypes")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("TehnicalExamsTypes")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetTehnicalExamsTypes() As TehnicalExamsTypes
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a TehnicalExamsTypes")
    End If
    Return DataPortal.Fetch(Of TehnicalExamsTypes)()
  End Function

  Public Overrides Function Save() As TehnicalExamsTypes
    Dim result As TehnicalExamsTypes = MyBase.Save()

    OnTehnicalExamsTypesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("TehnicalExamsTypes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(TehnicalExamsType.GetTehnicalExamsType(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("TehnicalExamsTypes.Child_Fetch", ex)
      Throw New DbCslaException("TehnicalExamsTypes.Child_Fetch", ex)
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
  Public Shared Event TehnicalExamsTypesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnTehnicalExamsTypesSaved(ByVal sender As TehnicalExamsTypes, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent TehnicalExamsTypesSaved(sender, e)
  End Sub
#End Region
End Class
