
<Serializable()> _
Public Class VehicleEnginePowerSourceTypeList
  Inherits ReadOnlyListBase(Of VehicleEnginePowerSourceTypeList, VehicleEnginePowerSourceTypeInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleEnginePowerSourceTypes"
#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleEnginePowerSourceTypeList() As VehicleEnginePowerSourceTypeList

    Return DataPortal.Fetch(Of VehicleEnginePowerSourceTypeList)()

  End Function

  Public Function GetPowerSourceTypeInfo(ByVal inId As Integer) As VehicleEnginePowerSourceTypeInfo
    For Each child As VehicleEnginePowerSourceTypeInfo In Me
      If child.Id = inId Then
        Return child
      End If
    Next
    Return Nothing
  End Function
  Private Sub New()
    ' require use of factory methods
    AddHandler VehicleEnginePowerSourceTypes.VehicleEnginePowerSourceTypesSaved, AddressOf VehicleEnginePowerSourceTypes_saved
  End Sub

  Private Sub VehicleEnginePowerSourceTypes_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
    IsReadOnly = False
    Me.Clear()
    IsReadOnly = True
    DataPortal_Fetch()
    Me.ResetBindings()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleEnginePowerSourceTypeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Dim InfoNull As New VehicleEnginePowerSourceTypeInfo(0, "[нема]")
      Me.Add(InfoNull)
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleEnginePowerSourceTypeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleEnginePowerSourceTypeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleEnginePowerSourceTypeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class