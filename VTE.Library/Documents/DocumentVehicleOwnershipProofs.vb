
<Serializable()> _
Public Class DocumentVehicleOwnershipProofs
  Inherits Csla.BusinessListBase(Of DocumentVehicleOwnershipProofs, DocumentVehicleOwnershipProof)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentVehicleOwnershipProoByID"
  Private Const spGetAll As String = "GetDocumentVehicleOwnershipProof"
  Private Const spUpdate As String = "updateDocumentVehicleOwnershipProo"
  Private Const spAdd As String = "addDocumentVehicleOwnershipProo"
  Private Const spDelete As String = "deleteDocumentVehicleOwnershipProo"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As DocumentVehicleOwnershipProof = DocumentVehicleOwnershipProof.NewDocumentVehicleOwnershipProofChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentVehicleOwnershipProofs")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentVehicleOwnershipProofs")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentVehicleOwnershipProofs")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentVehicleOwnershipProofs")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetDocumentVehicleOwnershipProofs() As DocumentVehicleOwnershipProofs
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a DocumentVehicleOwnershipProofs")
    End If
    Return DataPortal.Fetch(Of DocumentVehicleOwnershipProofs)()
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("DocumentVehicleOwnershipProofs.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(DocumentVehicleOwnershipProof.GetDocumentVehicleOwnershipProof(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("DocumentVehicleOwnershipProofs.Child_Fetch", ex)
      Throw New DbCslaException("DocumentVehicleOwnershipProofs.Child_Fetch", ex)
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
