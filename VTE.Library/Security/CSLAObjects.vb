
<Serializable()> _
Public Class CSLAObjects
  Inherits Csla.BusinessListBase(Of CSLAObjects, CSLAObject)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCSLAObjectByID"
  Private Const spGetAll As String = "GetCSLAObjects"
  Private Const spUpdate As String = "updateCSLAObject"
  Private Const spAdd As String = "addCSLAObject"
  Private Const spDelete As String = "deleteCSLAObject"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As CSLAObject = CSLAObject.NewCSLAObjectChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("CSLAObjects")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("CSLAObjects")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("CSLAObjects")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("CSLAObjects")
    End Function

#End Region ' Authorization Rules
#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

    Public Shared Function GetCSLAObjects() As CSLAObjects
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User Not authorized to view a CSLAObjects")
        End If
        Return DataPortal.Fetch(Of CSLAObjects)()
    End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("CSLAObjects.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.SecurityConnection_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(CSLAObject.GetCSLAObject(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("CSLAObjects.Child_Fetch", ex)
      Throw New DbCslaException("CSLAObjects.Child_Fetch", ex)
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
