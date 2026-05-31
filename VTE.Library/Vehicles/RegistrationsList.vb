
<Serializable()> _
Public Class RegistrationsList
  Inherits ReadOnlyListBase(Of RegistrationsList, RegistrationsInfo)



#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleRegistrations"
  Private Const SpZemiPoId As String = "getVehicleRegistrationByIdList"
#End Region

#Region " Factory Methods "

  Public Shared Function GetRegistrationInfoById(ByVal id As Integer) As RegistrationsList
    Return DataPortal.Fetch(Of RegistrationsList) _
    (New SingleCriteria(Of RegistrationsList, Integer)(id))
  End Function

  Public Shared Function GetRegistrationsList() As RegistrationsList

    Return DataPortal.Fetch(Of RegistrationsList)()

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of RegistrationsList, Integer))
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("RegistrationsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiPoId
          cm.Parameters.AddWithValue("@id", criteria.Value)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New RegistrationsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("RegistrationsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("RegistrationsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("RegistrationsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New RegistrationsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("RegistrationsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("RegistrationsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class