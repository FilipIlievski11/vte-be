
<Serializable()> _
Public Class Cities
  Inherits Csla.BusinessListBase(Of Cities, City)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCitieByID"
  Private Const spGetAll As String = "GetCities"
  Private Const spUpdate As String = "updateCitie"
  Private Const spAdd As String = "addCitie"
  Private Const spDelete As String = "deleteCitie"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As City = City.NewCityChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Cities")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Cities")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Cities")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Cities")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

    Public Shared Function GetCities() As Cities
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User Not authorized to view a Cities")
        End If
        Return DataPortal.Fetch(Of Cities)()
    End Function


  Public Overrides Function Save() As Cities
    Dim result As Cities = MyBase.Save()

    OnCitiesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("Cities.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(City.GetCity(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("Cities.Child_Fetch", ex)
      Throw New DbCslaException("Cities.Child_Fetch", ex)
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
  Public Shared Event CitiesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnCitiesSaved(ByVal sender As Cities, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent CitiesSaved(sender, e)
  End Sub
#End Region
End Class



