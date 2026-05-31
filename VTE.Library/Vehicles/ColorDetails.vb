
<Serializable()> _
Public Class ColorDetails
  Inherits Csla.BusinessListBase(Of ColorDetails, ColorDetail)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As ColorDetail = ColorDetail.NewColorDetailChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Factory Methods "

  Friend Shared Function NewColorDetails() As ColorDetails
    Return DataPortal.CreateChild(Of ColorDetails)()
  End Function

  Friend Shared Function GetColorDetails(ByVal dr As SafeDataReader) As ColorDetails
    Return DataPortal.FetchChild(Of ColorDetails)(dr)
  End Function
  Public Overrides Function Save() As ColorDetails
    Dim result As ColorDetails = MyBase.Save()

    OnColorDetailsSaved(Me, New Csla.Core.SavedEventArgs(result))

    Return result

  End Function
  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("ColorDetails.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(ColorDetail.GetColorDetail(dr))
      End While
    Catch ex As Exception
      Database.LogException("ColorDetails.Child_Fetch", ex)
      Throw New DbCslaException("ColorDetails.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

#Region " Readonlylist refresh "
  Public Shared Event ColorDetailsSaved As EventHandler(Of Csla.Core.SavedEventArgs)
  Protected Shared Sub OnColorDetailsSaved(ByVal sender As ColorDetails, ByVal e As Csla.Core.SavedEventArgs)
    RaiseEvent ColorDetailsSaved(sender, e)
  End Sub
#End Region

End Class
