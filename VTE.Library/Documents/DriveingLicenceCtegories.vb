
<Serializable()> _
Public Class DriveingLicenceCtegories
  Inherits Csla.BusinessListBase(Of DriveingLicenceCtegories, DriveingLicenceCtegory)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDriveingLicenceCtegorieByID"
  Private Const spGetAll As String = "GetDriveingLicenceCtegories"
  Private Const spUpdate As String = "updateDriveingLicenceCtegorie"
  Private Const spAdd As String = "addDriveingLicenceCtegorie"
  Private Const spDelete As String = "deleteDriveingLicenceCtegorie"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DriveingLicenceCtegory = DriveingLicenceCtegory.NewDriveingLicenceCtegoryChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DriveingLicenceCtegories")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DriveingLicenceCtegories")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DriveingLicenceCtegories")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DriveingLicenceCtegories")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetDriveingLicenceCtegories() As DriveingLicenceCtegories
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a DriveingLicenceCtegories")
    End If
    Return DataPortal.Fetch(Of DriveingLicenceCtegories)()
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("DriveingLicenceCtegories.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(DriveingLicenceCtegory.GetDriveingLicenceCtegory(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DriveingLicenceCtegories.Child_Fetch", ex)
      Throw New DbCslaException("DriveingLicenceCtegories.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


  Protected Overrides Sub DataPortal_Update()
    RaiseListChangedEvents = False
    Child_Update()
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access


End Class
