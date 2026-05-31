
<Serializable()> _
Public Class DriveingLicenceCtegoryList
  Inherits ReadOnlyListBase(Of DriveingLicenceCtegoryList, DriveingLicenceCtegoryInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getDriveingLicenceCtegories"
#End Region

#Region " Factory Methods "

  Public Shared Function GetDriveingLicenceCtegoryList() As DriveingLicenceCtegoryList

    Return DataPortal.Fetch(Of DriveingLicenceCtegoryList)()

  End Function
  Public Function GetInfo(ByVal inId As Integer) As DriveingLicenceCtegoryInfo
    For Each detal As DriveingLicenceCtegoryInfo In Me
      If detal.Id = inId Then
        Return detal
      End If
    Next
    Return Nothing
  End Function
  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("DriveingLicenceCtegoryInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DriveingLicenceCtegoryInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DriveingLicenceCtegoryInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DriveingLicenceCtegoryInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class