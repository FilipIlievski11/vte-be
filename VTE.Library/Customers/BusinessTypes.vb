
<Serializable()> _
Public Class BusinessTypes
  Inherits Csla.BusinessListBase(Of BusinessTypes, BusinessType)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetBusinessTypeByID"
  Private Const spGetAll As String = "GetBusinessTypes"
  Private Const spUpdate As String = "updateBusinessType"
  Private Const spAdd As String = "addBusinessType"
  Private Const spDelete As String = "deleteBusinessType"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As BusinessType = BusinessType.NewBusinessTypeChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanAddObject() As Boolean

    Return CType(Csla.ApplicationContext.User,  _
           VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("BusinessTypes")

  End Function

  Public Shared Function CanGetObject() As Boolean

    Return CType(Csla.ApplicationContext.User,  _
           VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("BusinessTypes")
  End Function

  Public Shared Function CanDeleteObject() As Boolean

    Return CType(Csla.ApplicationContext.User,  _
            VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("BusinessTypes")

  End Function

  Public Shared Function CanEditObject() As Boolean

    Return CType(Csla.ApplicationContext.User,  _
            VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("BusinessTypes")

  End Function

  
#End Region

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

    Public Shared Function GetBusinessTypes() As BusinessTypes
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User Not authorized to view a BusinessTypes")
        End If
        Return DataPortal.Fetch(Of BusinessTypes)()
    End Function
  Public Overrides Function Save() As BusinessTypes
    Dim result As BusinessTypes = MyBase.Save()

    OnBusinessTypesSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo(" BusinessTypes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(BusinessType.GetBusinessType(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException(" BusinessTypes.Child_Fetch", ex)
      Throw New DbCslaException(" BusinessTypes.Child_Fetch", ex)
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
  Public Shared Event BusinessTypesSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnBusinessTypesSaved(ByVal sender As BusinessTypes, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent BusinessTypesSaved(sender, e)
  End Sub
#End Region

End Class

