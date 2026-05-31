
<Serializable()> _
Public Class VehicleLastTehnicalExams
  Inherits Csla.BusinessListBase(Of VehicleLastTehnicalExams, VehicleLastTehnicalExam)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As VehicleLastTehnicalExam = VehicleLastTehnicalExam.NewVehicleLastTehnicalExamChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Factory Methods "

  Friend Shared Function NewVehicleLastTehnicalExams() As VehicleLastTehnicalExams
    Return DataPortal.CreateChild(Of VehicleLastTehnicalExams)()
  End Function

  Friend Shared Function GetVehicleLastTehnicalExams(ByVal dr As SafeDataReader) As VehicleLastTehnicalExams
    Return DataPortal.FetchChild(Of VehicleLastTehnicalExams)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("VehicleLastTehnicalExams.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(VehicleLastTehnicalExam.GetVehicleLastTehnicalExam(dr))
      End While
    Catch ex As Exception
      Database.LogException("VehicleLastTehnicalExams.Child_Fetch", ex)
      Throw New DbCslaException("VehicleLastTehnicalExams.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
