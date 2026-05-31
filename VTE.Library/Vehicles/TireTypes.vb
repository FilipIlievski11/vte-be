
<Serializable()> _
Public Class TireTypes
  Inherits Csla.BusinessListBase(Of TireTypes, TireType)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As TireType = TireType.NewTireTypeChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides


#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("TireTypes")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("TireTypes")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("TireTypes")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("TireTypes")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

  Friend Shared Function NewTireTypes() As TireTypes
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a TireTypes")
        'End If
        Return DataPortal.CreateChild(Of TireTypes)()
  End Function

  Friend Shared Function GetTireTypes(ByVal dr As SafeDataReader) As TireTypes
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a TireType")
        'End If
        Return DataPortal.FetchChild(Of TireTypes)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("TireTypes.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(TireType.GetTireType(dr))
      End While
    Catch ex As Exception
      Database.LogException("TireTypes.Child_Fetch", ex)
      Throw New DbCslaException("TireTypes.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
