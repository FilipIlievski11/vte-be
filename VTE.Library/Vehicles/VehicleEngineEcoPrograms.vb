
<Serializable()> _
Public Class VehicleEngineEcoPrograms
  Inherits Csla.BusinessListBase(Of VehicleEngineEcoPrograms, VehicleEngineEcoProgram)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetVehicleEngineEcoPrograByID"
  Private Const spGetAll As String = "GetVehicleEngineEcoProgram"
  Private Const spUpdate As String = "updateVehicleEngineEcoProgra"
  Private Const spAdd As String = "addVehicleEngineEcoProgra"
  Private Const spDelete As String = "deleteVehicleEngineEcoProgra"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleEngineEcoProgram = VehicleEngineEcoProgram.NewVehicleEngineEcoProgramChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleEngineEcoPrograms")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleEngineEcoPrograms")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleEngineEcoPrograms")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleEngineEcoPrograms")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetVehicleEngineEcoPrograms() As VehicleEngineEcoPrograms
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a VehicleEngineEcoPrograms")
    End If
    Return DataPortal.Fetch(Of VehicleEngineEcoPrograms)()
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleEngineEcoPrograms.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(VehicleEngineEcoProgram.GetVehicleEngineEcoProgram(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("VehicleEngineEcoPrograms.Child_Fetch", ex)
      Throw New DbCslaException("VehicleEngineEcoPrograms.Child_Fetch", ex)
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
