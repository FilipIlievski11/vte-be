
<Serializable()> _
Public Class VehicleCategoryForPayments
  Inherits Csla.BusinessListBase(Of VehicleCategoryForPayments, VehicleCategoryForPayment)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleCategoryForPaymentByID"
  Private Const spGetAll As String = "GetVehicleCategoryForPayments"
  Private Const spUpdate As String = "updateVehicleCategoryForPayment"
  Private Const spAdd As String = "addVehicleCategoryForPayment"
  Private Const spDelete As String = "deleteVehicleCategoryForPayment"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleCategoryForPayment = VehicleCategoryForPayment.NewVehicleCategoryForPaymentChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleCategoryForPayments")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleCategoryForPayments")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleCategoryForPayments")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleCategoryForPayments")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetVehicleCategoryForPayments() As VehicleCategoryForPayments
        'If Not CanGetObject() Then
        '  Throw New System.Security.SecurityException("User Not authorized to view a VehicleCategoryForPayments")
        'End If
    Return DataPortal.Fetch(Of VehicleCategoryForPayments)()
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleCategoryForPayments.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleCategoryForPayment.GetVehicleCategoryForPayment(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleCategoryForPayments.Child_Fetch", ex)
      Throw New DbCslaException("VehicleCategoryForPayments.Child_Fetch", ex)
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
