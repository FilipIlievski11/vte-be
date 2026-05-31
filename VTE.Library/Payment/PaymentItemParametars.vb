
<Serializable()> _
Public Class PaymentItemParametars
  Inherits Csla.BusinessListBase(Of PaymentItemParametars, PaymentItemParametar)

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As PaymentItemParametar = PaymentItemParametar.NewPaymentItemParametarChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("PaymentItemParametars")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("PaymentItemParametars")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("PaymentItemParametars")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("PaymentItemParametars")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

  Friend Shared Function NewPaymentItemParametars() As PaymentItemParametars
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User Not authorized to add a PaymentItemParametars")
        'End If
        Return DataPortal.CreateChild(Of PaymentItemParametars)()
  End Function

  Friend Shared Function GetPaymentItemParametars(ByVal dr As SafeDataReader) As PaymentItemParametars
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User Not authorized to view a PaymentItemParametars")
        'End If
        Return DataPortal.FetchChild(Of PaymentItemParametars)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("PaymentItemParametars.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(PaymentItemParametar.GetPaymentItemParametar(dr))
      End While
    Catch ex As Exception
      Database.LogException("PaymentItemParametars.Child_Fetch", ex)
      Throw New DbCslaException("PaymentItemParametars.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True
  End Sub


#End Region ' Data Access

End Class
