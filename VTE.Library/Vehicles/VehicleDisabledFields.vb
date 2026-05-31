
<Serializable()> _
Public Class VehicleDisabledFields
  Inherits Csla.BusinessListBase(Of VehicleDisabledFields, VehicleDisabledField)

  Public Function ContainsField(ByVal strFieldName) As Boolean
    For Each ch As VehicleDisabledField In Me
      If ch.FieldName = strFieldName Then
        Return True
      End If
    Next
    Return False
  End Function

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleDisabledField = VehicleDisabledField.NewVehicleDisabledFieldChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Factory Methods "

  Friend Shared Function NewVehicleDisabledFields() As VehicleDisabledFields
    Return DataPortal.CreateChild(Of VehicleDisabledFields)()
  End Function

  Friend Shared Function GetVehicleDisabledFields(ByVal dr As SafeDataReader) As VehicleDisabledFields
    Return DataPortal.FetchChild(Of VehicleDisabledFields)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleDisabledFields.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(VehicleDisabledField.GetVehicleDisabledField(dr))
      End While
    Catch ex As Exception
      Database.LogException("VehicleDisabledFields.Child_Fetch", ex)
      Throw New DbCslaException("VehicleDisabledFields.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
