
<Serializable()> _
Public Class VehicleTyres
  Inherits Csla.BusinessListBase(Of VehicleTyres, VehicleTyre)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleTyre = VehicleTyre.NewVehicleTyreChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Factory Methods "

  Friend Shared Function NewVehicleTyres() As VehicleTyres

    Return DataPortal.CreateChild(Of VehicleTyres)()
  End Function

  Friend Shared Function GetVehicleTyres(ByVal dr As SafeDataReader) As VehicleTyres
    Return DataPortal.FetchChild(Of VehicleTyres)(dr)
  End Function


  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "
  
  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleTyres.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(VehicleTyre.GetVehicleTyre(dr))
      End While
    Catch ex As Exception
      Database.LogException("VehicleTyres.Child_Fetch", ex)
      Throw New DbCslaException("VehicleTyres.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub

   
#End Region ' Data Access

End Class
