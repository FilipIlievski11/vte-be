
<Serializable()> _
Public Class VehicleUseList
  Inherits ReadOnlyListBase(Of VehicleUseList, VehicleUseInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getVehicleUse"
  Private Const SpZemiSiteByCriteriaAndBodytype As String = "getVehicleUseByCategoryAndBodytype"

#End Region

#Region " Factory Methods "

  Public Shared Function GetVehicleUseList() As VehicleUseList

    Return DataPortal.Fetch(Of VehicleUseList)()

  End Function

  Public Shared Function GetVehicleUseListByCategoryAndBodytype(ByVal inCategory As Integer, ByVal inBodytype As Integer) As VehicleUseList

    Return DataPortal.Fetch(Of VehicleUseList)(New CriteriaByCategoryAndBodyType(inCategory, inBodytype))

  End Function
  Public Function GetUseIById(ByVal inid As Integer) As VehicleUseInfo
    For Each child As VehicleUseInfo In Me
      If child.Id = inid Then
        Return child
      End If
    Next
    Return Nothing
  End Function


  Private Sub New()
    ' require use of factory methods
    AddHandler VehicleUses.VehicleUsesSaved, AddressOf VehicleUses_saved
  End Sub

  Private Sub VehicleUses_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
    IsReadOnly = False
    Me.Clear()
    IsReadOnly = True
    DataPortal_Fetch()
    Me.ResetBindings()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  <Serializable()> _
Private Class CriteriaByCategoryAndBodyType
    Private _idCategory As Integer
    Private _idBodytype As Integer

    Public ReadOnly Property IdCategory() As Integer
      Get
        Return _idCategory
      End Get
    End Property

    Public ReadOnly Property IdBodytype() As Integer
      Get
        Return _idBodytype
      End Get
    End Property

    Public Sub New(ByVal idCategory As Integer, ByVal idBodytype As Integer)
      _idCategory = idCategory
      _idBodytype = idBodytype
    End Sub
  End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleUseInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
     Dim nullItem As New VehicleUseInfo(0, My.Resources.Nema, "") '"([A-Z0-9^Ž^Đ^Š^Č][A-Z0-9^Ž^Đ^Š^Č]?[A-Z0-9^Ž^Đ^Š^Č])-([0-9][0-9][0-9])-([A-Z^Ž^Đ^Š^Č][A-Z^Ž^Đ^Š^Č]?[A-Z^Ž^Đ^Š^Č])")
          Me.Add(nullItem)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleUseInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleUseInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleUseInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaByCategoryAndBodyType)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("VehicleUseInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.Parameters.AddWithValue("@idCategoty", criteria.IdCategory)
          cm.Parameters.AddWithValue("@idBodytype", criteria.IdBodytype)
          cm.CommandText = SpZemiSiteByCriteriaAndBodytype
     Dim nullItem As New VehicleUseInfo(0, My.Resources.Nema, "") '"([A-Z0-9^Ž^Đ^Š^Č][A-Z0-9^Ž^Đ^Š^Č]?[A-Z0-9^Ž^Đ^Š^Č])-([0-9][0-9][0-9])-([A-Z^Ž^Đ^Š^Č][A-Z^Ž^Đ^Š^Č]?[A-Z^Ž^Đ^Š^Č])")
          Me.Add(nullItem)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New VehicleUseInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("VehicleUseInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("VehicleUseInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class