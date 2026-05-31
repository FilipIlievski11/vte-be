
<Serializable()> _
Public Class AttachmentTypes
  Inherits Csla.BusinessListBase(Of AttachmentTypes, AttachmentType)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetAttachmentTypeByID"
  Private Const spGetAll As String = "GetAttachmentTypes"
  Private Const spUpdate As String = "updateAttachmentType"
  Private Const spAdd As String = "addAttachmentType"
  Private Const spDelete As String = "deleteAttachmentType"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As AttachmentType = AttachmentType.NewAttachmentTypeChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("AttachmentTypes")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("AttachmentTypes")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("AttachmentTypes")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("AttachmentTypes")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetAttachmentTypes() As AttachmentTypes
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a AttachmentTypes")
    End If
    Return DataPortal.Fetch(Of AttachmentTypes)()
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("AttachmentTypes.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(AttachmentType.GetAttachmentType(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("AttachmentTypes.Child_Fetch", ex)
      Throw New DbCslaException("AttachmentTypes.Child_Fetch", ex)
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
