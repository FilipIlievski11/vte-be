
<Serializable()> _
Public Class RegistrationIssuerList
  Inherits ReadOnlyListBase(Of RegistrationIssuerList, RegistrationIssuerInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getRegistrationIssuers"
#End Region

#Region " Factory Methods "

  Public Shared Function GetRegistrationIssuerList() As RegistrationIssuerList

    Return DataPortal.Fetch(Of RegistrationIssuerList)()

  End Function

    Public Function GetRegistrationIssuerInfo(ByVal inId As Integer) As RegistrationIssuerInfo
        For Each col As RegistrationIssuerInfo In Me
            If col.Id = inId Then
                Return col
            End If
        Next
        Return Nothing
    End Function
    Public Function GetRegistrationIssuerInfoByCommunity(ByVal inId As Integer) As RegistrationIssuerInfo
        For Each col As RegistrationIssuerInfo In Me
            If col.IdCommunity = inId Then
                Return col
            End If
        Next
        Return Nothing
    End Function

    Private Sub New()
        ' require use of factory methods
        AddHandler RegistrationIssuers.RegistrationIssuersSaved, AddressOf RegistrationIssuers_saved
    End Sub

    Private Sub RegistrationIssuers_saved(ByVal sender As Object, ByVal e As Csla.Core.SavedEventArgs)
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
    Database.LogInfo("RegistrationIssuerInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
     Dim nullItem As New RegistrationIssuerInfo(0, My.Resources.Nema)
          Me.Add(nullItem)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New RegistrationIssuerInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("RegistrationIssuerInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("RegistrationIssuerInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class