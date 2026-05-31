
<Serializable()> _
Public Class VehicleRegistrations
  Inherits Csla.BusinessListBase(Of VehicleRegistrations, VehicleRegistration)

  Public Function GetFirstRegistration() As String
  Dim result As String = My.Resources.Nema
    If Me.Count <> 0 Then
      For Each child As VehicleRegistration In Me
        If child.IsFirstRegistration Then
          Return child.RegistrationNumber
        End If
      Next
    End If

    Return result
  End Function

  Public Function GetLastRegistration() As String
    If Me.Count <> 0 Then
      Dim maxDat As VehicleRegistration = Me.Items(0)

      For Each child As VehicleRegistration In Me
        If maxDat.DateOfRegistration < child.DateOfRegistration Then
          maxDat = Nothing
          maxDat = child
        End If
      Next
      Return maxDat.RegistrationNumber
    End If
  Return My.Resources.Nema
  End Function


#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleRegistration = VehicleRegistration.NewVehicleRegistrationChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Factory Methods "

  Friend Shared Function NewVehicleRegistrations() As VehicleRegistrations
    Return DataPortal.CreateChild(Of VehicleRegistrations)()
  End Function

  Friend Shared Function GetVehicleRegistrations(ByVal dr As SafeDataReader) As VehicleRegistrations
    Return DataPortal.FetchChild(Of VehicleRegistrations)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleRegistrations.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(VehicleRegistration.GetVehicleRegistration(dr))
      End While
    Catch ex As Exception
      Database.LogException("VehicleRegistrations.Child_Fetch", ex)
      Throw New DbCslaException("VehicleRegistrations.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
