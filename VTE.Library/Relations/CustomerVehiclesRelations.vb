
<Serializable()> _
Public Class CustomerVehiclesRelations
  Inherits Csla.BusinessListBase(Of CustomerVehiclesRelations, CustomerVehiclesRelation)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCustomerVehiclesRelationByID"
  Private Const spGetAll As String = "GetCustomerVehiclesRelations"
  Private Const spUpdate As String = "updateCustomerVehiclesRelation"
  Private Const spAdd As String = "addCustomerVehiclesRelation"
  Private Const spDelete As String = "deleteCustomerVehiclesRelation"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As CustomerVehiclesRelation = CustomerVehiclesRelation.NewCustomerVehiclesRelationChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("CustomerVehiclesRelations")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("CustomerVehiclesRelations")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("CustomerVehiclesRelations")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("CustomerVehiclesRelations")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetCustomerVehiclesRelations() As CustomerVehiclesRelations 'ByVal idRelationType As Integer, ByVal idCustomer As Integer, ByVal idVehicle As Long
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a CustomerVehiclesRelations")
    End If
    Return DataPortal.Fetch(Of CustomerVehiclesRelations)()
  End Function

  Public Overrides Function Save() As CustomerVehiclesRelations
    Dim result As CustomerVehiclesRelations = MyBase.Save()

        'OnCustomerVehiclesRelationsSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("CustomerVehiclesRelations.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(CustomerVehiclesRelation.GetCustomerVehiclesRelation(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("CustomerVehiclesRelations.Child_Fetch", ex)
      Throw New DbCslaException("CustomerVehiclesRelations.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


  Protected Overrides Sub DataPortal_Update()
    RaiseListChangedEvents = False
    Child_Update()
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access

    '#Region " Readonlylist refresh "
    '  Public Shared Event CustomerVehiclesRelationsSaved As EventHandler(Of Csla.Core.SavedEventArgs)
    '  Protected Shared Sub OnCustomerVehiclesRelationsSaved(ByVal sender As CustomerVehiclesRelations, ByVal e As Csla.Core.SavedEventArgs)
    '    RaiseEvent CustomerVehiclesRelationsSaved(sender, e)
    '  End Sub
    '#End Region


End Class
