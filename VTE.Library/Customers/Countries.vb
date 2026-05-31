
<Serializable()> _
Public Class Countries
  Inherits Csla.BusinessListBase(Of Countries, Country)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCountrieByID"
  Private Const spGetAll As String = "GetCountries"
  Private Const spUpdate As String = "updateCountrie"
  Private Const spAdd As String = "addCountrie"
  Private Const spDelete As String = "deleteCountrie"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As Country = Country.NewCountryChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("Countries")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("Countries")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("Countries")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("Countries")
  End Function
  
#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

    Public Shared Function GetCountries() As Countries
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a Country")
        End If
        Return DataPortal.Fetch(Of Countries)()
    End Function
  Public Overrides Function Save() As Countries
    Dim result As Countries = MyBase.Save()

    OnCountriesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("Countries.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(Country.GetCountry(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("Countries.Child_Fetch", ex)
      Throw New DbCslaException("Countries.Child_Fetch", ex)
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
  Public Shared Event CountriesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnCountriesSaved(ByVal sender As Countries, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent CountriesSaved(sender, e)
  End Sub
#End Region
End Class
