
<Serializable()> _
Public Class CalculationItems
  Inherits Csla.BusinessListBase(Of CalculationItems, CalculationItem)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetCalculationItemByID"
  Private Const spGetAll As String = "GetCalculationItems"
  Private Const spUpdate As String = "updateCalculationItem"
  Private Const spAdd As String = "addCalculationItem"
  Private Const spDelete As String = "deleteCalculationItem"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As CalculationItem = CalculationItem.NewCalculationItemChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("CalculationItems")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("CalculationItems")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("CalculationItems")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("CalculationItems")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
  Private Sub New()
    AllowNew = True
  End Sub

  Public Shared Function GetCalculationItems() As CalculationItems
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a CalculationItems")
    End If
    Return DataPortal.Fetch(Of CalculationItems)()
  End Function

#End Region ' Factory Methods

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    Database.LogInfo("CalculationItems.Child_Fetch", GetHashCode())
    Try

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = spGetAll
          Using dr As New SafeDataReader(cm.ExecuteReader)
            While dr.Read()
              Me.Add(CalculationItem.GetCalculationItem(dr))
            End While
          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("CalculationItems.Child_Fetch", ex)
      Throw New DbCslaException("CalculationItems.Child_Fetch", ex)
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
