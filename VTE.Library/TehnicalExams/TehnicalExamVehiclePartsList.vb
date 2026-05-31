
<Serializable()> _
Public Class TehnicalExamVehiclePartsList
  Inherits ReadOnlyListBase(Of TehnicalExamVehiclePartsList, TehnicalExamVehiclePartsInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getTehnicalExamVehiclePartsList"
  Private Const SpZemiPoIdCategory As String = "getTehnicalExamVehiclePartsListByIdCategory"
#End Region

#Region " Factory Methods "

  Public Shared Function GetTehnicalExamVehiclePartsList() As TehnicalExamVehiclePartsList

    Return DataPortal.Fetch(Of TehnicalExamVehiclePartsList)()

  End Function

  Public Shared Function GetTehnicalExamVehiclePartsListByCategory(ByVal inIdCategory As Integer) As TehnicalExamVehiclePartsList

    Return DataPortal.Fetch(Of TehnicalExamVehiclePartsList)(New CriteriaByIdCategory(inIdCategory))

  End Function

  Public Function GetTehnicalExamVehiclePartsListById(ByVal InId As Integer) As TehnicalExamVehiclePartsInfo
    For Each Item As TehnicalExamVehiclePartsInfo In Me
      If Item.Id = InId Then
        Return Item
      End If
    Next
    Return Nothing
  End Function

  Private Sub New()
    ' require use of factory methods
    AddHandler TehnicalExamVehicleParts.TehnicalExamVehiclePartsSaved, AddressOf TehnicalExamVehicleParts_saved
  End Sub

  Private Sub TehnicalExamVehicleParts_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
    IsReadOnly = False
    Me.Clear()
    IsReadOnly = True
    DataPortal_Fetch()
    Me.ResetBindings()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "
  <Serializable()> _
Private Class CriteriaByIdCategory
    Private _idCategory As Integer

    Public ReadOnly Property IdCategory() As Integer
      Get
        Return _idCategory
      End Get
    End Property

    Public Sub New(ByVal IdCategory As Integer)
      _idCategory = IdCategory
    End Sub
  End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("TehnicalExamVehiclePartsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New TehnicalExamVehiclePartsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("TehnicalExamVehiclePartsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("TehnicalExamVehiclePartsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByIdCategory)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("TehnicalExamVehiclePartsInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiPoIdCategory
          cm.Parameters.AddWithValue("@IdCategoryVehicleParts", criteria.IdCategory)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New TehnicalExamVehiclePartsInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("TehnicalExamVehiclePartsInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("TehnicalExamVehiclePartsInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
#End Region ' Data Access
End Class