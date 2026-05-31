
<Serializable()> _
Public Class VehicleRequiredFields
  Inherits Csla.BusinessListBase(Of VehicleRequiredFields, VehicleRequiredField)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleRequiredField = VehicleRequiredField.NewVehicleRequiredFieldChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Factory Methods "

  Friend Shared Function NewVehicleRequiredFields() As VehicleRequiredFields
    Return DataPortal.CreateChild(Of VehicleRequiredFields)()
  End Function

  Friend Shared Function GetVehicleRequiredFields(ByVal dr As SafeDataReader) As VehicleRequiredFields
    Return DataPortal.FetchChild(Of VehicleRequiredFields)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleRequiredFields.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(VehicleRequiredField.GetVehicleRequiredField(dr))
      End While
    Catch ex As Exception
      Database.LogException("VehicleRequiredFields.Child_Fetch", ex)
      Throw New DbCslaException("VehicleRequiredFields.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
