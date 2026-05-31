
<Serializable()> _
Public Class VehicleBodytypeList
  Inherits ReadOnlyListBase(Of VehicleBodytypeList, VehicleBodytypeInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleBodytype"
  Private Const SpZemiSitePoKategorija As String = "getVehicleBodytypeByIdCategory"

#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleBodytypeList() As VehicleBodytypeList

    Return DataPortal.Fetch(Of VehicleBodytypeList)()

  End Function

  Public Function GetVehicleBodytypeInfo(ByVal inId As Integer) As VehicleBodytypeInfo
    For Each child As VehicleBodytypeInfo In Me
      If child.Id = inId Then
        Return child
      End If
    Next
    Return Nothing
  End Function
  Public Shared Function GetVehicleBodytypeListByCategory(ByVal inIdCategory As Integer) As VehicleBodytypeList

    Return DataPortal.Fetch(Of VehicleBodytypeList)(New CriteriaByCategory(inIdCategory))

  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler VehicleBodytypes.VehicleBodytypesSaved, AddressOf VehicleBodytypes_saved

  End Sub

  Private Sub VehicleBodytypes_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
    IsReadOnly = False
    Me.Clear()
    IsReadOnly = True
    DataPortal_Fetch()
    Me.ResetBindings()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  <Serializable()> _
Private Class CriteriaByCategory
    Private _idCategory As Integer

    Public ReadOnly Property IdCategory() As Integer
      Get
        Return _idCategory
      End Get
    End Property

    Public Sub New(ByVal idCategory As Integer)
      _idCategory = idCategory
    End Sub
  End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleBodytypeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleBodytypeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleBodytypeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleBodytypeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub


  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByCategory)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleBodytypeInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSitePoKategorija
          cm.Parameters.AddWithValue("@idCategory", criteria.IdCategory)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleBodytypeInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleBodytypeInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleBodytypeInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class