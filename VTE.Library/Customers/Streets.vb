
<Serializable()> _
Public Class Streets
  Inherits Csla.BusinessListBase(Of Streets, Street)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetStreetByID"
  Private Const spGetAll As String = "GetStreets"
  Private Const spUpdate As String = "updateStreet"
  Private Const spAdd As String = "addStreet"
  Private Const spDelete As String = "deleteStreet"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As Street = Street.NewStreetChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Streets")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Streets")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Streets")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Streets")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

    Public Shared Function GetStreets() As Streets
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a Street")
        End If
        Return DataPortal.Fetch(Of Streets)()
    End Function

  Public Overrides Function Save() As Streets
    Dim result As Streets = MyBase.Save()

    OnStreetsSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("Streets.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(Street.GetStreet(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("Streets.Child_Fetch", ex)
      Throw New DbCslaException("Streets.Child_Fetch", ex)
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
  Public Shared Event StreetsSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnStreetsSaved(ByVal sender As Streets, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent StreetsSaved(sender, e)
  End Sub
#End Region

End Class
