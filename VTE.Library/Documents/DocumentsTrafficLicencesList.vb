
<Serializable()> _
Public Class DocumentsTrafficLicencesList
  Inherits ReadOnlyListBase(Of DocumentsTrafficLicencesList, DocumentsTrafficLicencesInfo)

#Region " Stored Procedures Names "
    Private Const SpZemiSite As String = "getDocumentsTrafficLicencesList"
    Private Const SpZemiSiteOdDo As String = "getDocumentsTrafficLicencesListFromTo"
#End Region

#Region " Factory Methods "

  Public Shared Function GetDocumentsTrafficLicencesList() As DocumentsTrafficLicencesList

    Return DataPortal.Fetch(Of DocumentsTrafficLicencesList)()

  End Function
    Public Shared Function GetDocumentsTrafficLicencesListOdDo(ByVal dateStart As Date, ByVal dateEnd As Date) As DocumentsTrafficLicencesList

        Return DataPortal.Fetch(Of DocumentsTrafficLicencesList)(New DateCritetria(dateStart, dateEnd))

    End Function
  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "
    <Serializable()> _
Private Class DateCritetria

        Public StartDate As Date
        Public dateEnd As Date

        Public Sub New(ByVal StartDate As Date, ByVal dateEnd As Date)
            Me.StartDate = StartDate
            Me.dateEnd = dateEnd
        End Sub
    End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("DocumentsTrafficLicencesInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentsTrafficLicencesInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentsTrafficLicencesInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentsTrafficLicencesInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
    Private Overloads Sub DataPortal_Fetch(ByVal criteria As DateCritetria)
        RaiseListChangedEvents = False
        IsReadOnly = False
        Database.LogInfo("DocumentsTrafficLicencesInfo.DataPortal_Fetch", GetHashCode())
        Try
            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = SpZemiSiteOdDo
                    cm.Parameters.AddWithValue("@StartDate", Format(criteria.StartDate, "yyyy-MM-dd"))
                    cm.Parameters.AddWithValue("@EndDate", Format(criteria.dateEnd.AddDays(1), "yyyy-MM-dd"))

                    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
                        While dr.Read()
                            Dim Info As New DocumentsTrafficLicencesInfo(dr)
                            Me.Add(Info)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Database.LogException("DocumentsTrafficLicencesInfo.DataPortal_Fetch", ex)
            Throw New DbCslaException("DocumentsTrafficLicencesInfo.DataPortal_Fetch", ex)
        End Try
        IsReadOnly = True
        RaiseListChangedEvents = True
    End Sub
#End Region ' Data Access
End Class