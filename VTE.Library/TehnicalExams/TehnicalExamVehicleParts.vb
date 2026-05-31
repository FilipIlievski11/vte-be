
<Serializable()> _
Public Class TehnicalExamVehicleParts
  Inherits Csla.BusinessListBase(Of TehnicalExamVehicleParts, TehnicalExamVehiclePart)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetTehnicalExamVehiclePartByID"
  Private Const spGetAll As String = "GetTehnicalExamVehicleParts"
  Private Const spUpdate As String = "updateTehnicalExamVehiclePart"
  Private Const spAdd As String = "addTehnicalExamVehiclePart"
  Private Const spDelete As String = "deleteTehnicalExamVehiclePart"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As TehnicalExamVehiclePart = TehnicalExamVehiclePart.NewTehnicalExamVehiclePartChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("TehnicalExamVehicleParts")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("TehnicalExamVehicleParts")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("TehnicalExamVehicleParts")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("TehnicalExamVehicleParts")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetTehnicalExamVehicleParts() As TehnicalExamVehicleParts
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a TehnicalExamVehicleParts")
    End If
    Return DataPortal.Fetch(Of TehnicalExamVehicleParts)()
  End Function

  Public Overrides Function Save() As TehnicalExamVehicleParts
    Dim result As TehnicalExamVehicleParts = MyBase.Save()

    OnTehnicalExamVehiclePartsSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function
#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("TehnicalExamVehicleParts.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(TehnicalExamVehiclePart.GetTehnicalExamVehiclePart(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("TehnicalExamVehicleParts.Child_Fetch", ex)
      Throw New DbCslaException("TehnicalExamVehicleParts.Child_Fetch", ex)
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
  Public Shared Event TehnicalExamVehiclePartsSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnTehnicalExamVehiclePartsSaved(ByVal sender As TehnicalExamVehicleParts, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent TehnicalExamVehiclePartsSaved(sender, e)
  End Sub
#End Region

End Class
