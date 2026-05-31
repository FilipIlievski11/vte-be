
<Serializable()> _
Public Class Colors
  Inherits Csla.BusinessListBase(Of Colors, Color)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetColorByID"
  Private Const spGetAll As String = "GetColors"
  Private Const spUpdate As String = "updateColor"
  Private Const spAdd As String = "addColor"
  Private Const spDelete As String = "deleteColor"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As Color = Color.NewColorChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Colors")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Colors")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Colors")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Colors")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetColors() As Colors
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a Colors")
    End If
    Return DataPortal.Fetch(Of Colors)()
  End Function

  Public Overrides Function Save() As Colors
    Dim result As Colors = MyBase.Save()

    OnColorsSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("Colors.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(Color.GetColor(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("Colors.Child_Fetch", ex)
      Throw New DbCslaException("Colors.Child_Fetch", ex)
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
  Public Shared Event ColorsSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnColorsSaved(ByVal sender As Colors, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent ColorsSaved(sender, e)
  End Sub
#End Region

End Class
