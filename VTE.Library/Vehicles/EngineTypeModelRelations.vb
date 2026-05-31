
<Serializable()> _
Public Class EngineTypeModelRelations
  Inherits Csla.BusinessListBase(Of EngineTypeModelRelations, EngineTypeModelRelation)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetEngineTypeModelRelationByID"
  Private Const spGetAll As String = "GetEngineTypeModelRelations"
  Private Const spUpdate As String = "updateEngineTypeModelRelation"
  Private Const spAdd As String = "addEngineTypeModelRelation"
  Private Const spDelete As String = "deleteEngineTypeModelRelation"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As EngineTypeModelRelation = EngineTypeModelRelation.NewEngineTypeModelRelationChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("EngineTypeModelRelations")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("EngineTypeModelRelations")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("EngineTypeModelRelations")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("EngineTypeModelRelations")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetEngineTypeModelRelations() As EngineTypeModelRelations
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a EngineTypeModelRelations")
    End If
    Return DataPortal.Fetch(Of EngineTypeModelRelations)()
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("EngineTypeModelRelations.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(EngineTypeModelRelation.GetEngineTypeModelRelation(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("EngineTypeModelRelations.Child_Fetch", ex)
      Throw New DbCslaException("EngineTypeModelRelations.Child_Fetch", ex)
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
