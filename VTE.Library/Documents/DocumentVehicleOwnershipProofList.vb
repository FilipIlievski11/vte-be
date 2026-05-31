
<Serializable()> _
Public Class DocumentVehicleOwnershipProofList
  Inherits ReadOnlyListBase(Of DocumentVehicleOwnershipProofList, DocumentVehicleOwnershipProofInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getDocumentVehicleOwnershipProof"
#End Region

#Region " Factory Methods "

  Public Shared Function GetDocumentVehicleOwnershipProofList() As DocumentVehicleOwnershipProofList

    Return DataPortal.Fetch(Of DocumentVehicleOwnershipProofList)()

  End Function
  Public Function GetDocumentVehicleOwnershipProofInfo(ByVal inId As Integer) As DocumentVehicleOwnershipProofInfo
    For Each child As DocumentVehicleOwnershipProofInfo In Me
      If child.Id = inId Then
        Return child
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
    Database.LogInfo("DocumentVehicleOwnershipProofInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          Dim nullItem As New DocumentVehicleOwnershipProofInfo(0, "[нема]")
          Me.Add(nullItem)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentVehicleOwnershipProofInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentVehicleOwnershipProofInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentVehicleOwnershipProofInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access

End Class