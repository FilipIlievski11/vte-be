
<Serializable()> _
Public Class CustomerVehicleRelationTypes
  Inherits Csla.BusinessListBase(Of CustomerVehicleRelationTypes, CustomerVehicleRelationType)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCustomerVehiclesRelationTypeByID"
  Private Const spGetAll As String = "GetCustomerVehiclesRelationTypes"
  Private Const spUpdate As String = "updateCustomerVehiclesRelationType"
  Private Const spAdd As String = "addCustomerVehiclesRelationType"
  Private Const spDelete As String = "deleteCustomerVehiclesRelationType"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As CustomerVehicleRelationType = CustomerVehicleRelationType.NewCustomerVehicleRelationTypeChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("CustomerVehicleRelationTypes")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("CustomerVehicleRelationTypes")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("CustomerVehicleRelationTypes")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("CustomerVehicleRelationTypes")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetCustomerVehicleRelationTypes() As CustomerVehicleRelationTypes
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a CustomerVehicleRelationTypes")
    End If
    Return DataPortal.Fetch(Of CustomerVehicleRelationTypes)()
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("CustomerVehicleRelationTypes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(CustomerVehicleRelationType.GetCustomerVehicleRelationType(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("CustomerVehicleRelationTypes.Child_Fetch", ex)
      Throw New DbCslaException("CustomerVehicleRelationTypes.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


  Protected Overrides Sub DataPortal_Update()
    RaiseListChangedEvents = False
    Child_Update()
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access


End Class
