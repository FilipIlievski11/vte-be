
<Serializable()> _
Public Class VehiclesListShortListAll
    Inherits ReadOnlyListBase(Of VehiclesListShortListAll, VehiclesListShortInfoAll)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "getVehiclesListShortALL"
#End Region

#Region " Factory Methods "

    Public Shared Function GetVehiclesListShortListAll() As VehiclesListShortListAll

        Return DataPortal.Fetch(Of VehiclesListShortListAll)()

    End Function

    Private Sub New()
        ' require use of factory methods
        'AddHandler CustomerVehiclesRelations.CustomerVehiclesRelationsSaved, AddressOf Vehicle_saved
        'AddHandler CustomerVehiclesRelation.CustomerVehiclesRelationSaved, AddressOf Vehicle_saved
        'AddHandler Customers.CustomersSaved, AddressOf Vehicle_saved
        'AddHandler Customer.CustomerSaved, AddressOf Vehicle_saved
        'AddHandler Vehicle.VehicleSaved, AddressOf Vehicle_saved

    End Sub
    Private Sub Vehicle_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
        Database.LogInfo("VehiclesListShortInfoAll.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = SpZemiSite
                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New VehiclesListShortInfoAll(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("VehiclesListShortInfoAll.DataPortal_Fetch", ex)
            Throw New DbCslaException("VehiclesListShortInfoAll.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub

#End Region ' Data Access


End Class