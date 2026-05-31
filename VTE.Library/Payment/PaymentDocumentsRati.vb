
<Serializable()> _
Public Class PaymentDocumentsRati
    Inherits Csla.BusinessListBase(Of PaymentDocumentsRati, PaymentDocumentsRata)

    Public Function GetItem(ByVal id As Integer) As PaymentDocumentsRata
        For Each ch As PaymentDocumentsRata In Me
            If ch.Id = id Then
                Return ch
            End If
        Next
        Return Nothing
    End Function

#Region " BindingList Overrides "

    Protected Overrides Function AddNewCore() As Object
        Dim item As PaymentDocumentsRata = PaymentDocumentsRata.NewPaymentDocumentsRataChild()
        Me.Add(item)
        Return item
    End Function

#End Region ' BindingList Overrides


    '#Region " Authorization Rules "

    '    Public Shared Function CanGetObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("PaymentDocumentsDetails")
    '    End Function

    '    Public Shared Function CanAddObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("PaymentDocumentsDetails")
    '    End Function

    '    Public Shared Function CanEditObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("PaymentDocumentsDetails")
    '    End Function

    '    Public Shared Function CanDeleteObject() As Boolean
    '        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("PaymentDocumentsDetails")
    '    End Function

    '#End Region ' Authorization Rules

#Region " Factory Methods "

    Friend Shared Function NewPaymentDocumentsRati() As PaymentDocumentsRati
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User Not authorized to add a PaymentDocumentsDetails")
        'End If
        Return DataPortal.CreateChild(Of PaymentDocumentsRati)()
    End Function

    Friend Shared Function GetPaymentDocumentsRati(ByVal dr As SafeDataReader) As PaymentDocumentsRati
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User Not authorized to view a PaymentDocumentsDetails")
        'End If
        Return DataPortal.FetchChild(Of PaymentDocumentsRati)(dr)
    End Function

    Private Sub New()
        AllowNew = True
    End Sub

#End Region ' Factory Methods

#Region " Data Access "

    Private Sub Child_Fetch(ByVal dr As SafeDataReader)
        RaiseListChangedEvents = False
        Database.LogInfo("PaymentDocumentsDetails.Child_Fetch", GetHashCode())
        Try
            While dr.Read()
                Me.Add(PaymentDocumentsRata.GetPaymentDocumentsRata(dr))
            End While
        Catch ex As Exception
            Database.LogException("PaymentDocumentsRati.Child_Fetch", ex)
            Throw New DbCslaException("PaymentDocumentsRati.Child_Fetch", ex)
        End Try
        RaiseListChangedEvents = True

    End Sub


#End Region ' Data Access


End Class
