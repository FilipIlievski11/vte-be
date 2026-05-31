

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Csla
Imports Csla.Data

<Serializable()> _
Public Class RequestAttachments
    Inherits Csla.BusinessListBase(Of RequestAttachments, RequestAttachment)

#Region " BindingList Overrides "

    Protected Overrides Function AddNewCore() As Object
        Dim item As RequestAttachment = RequestAttachment.NewRequestAttachmentChild()
        Me.Add(item)
        Return item
    End Function

#End Region ' BindingList Overrides

#Region " Factory Methods "

    Friend Shared Function NewRequestAttachments() As RequestAttachments
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a DocumentAttachment")
        'End If
        Return DataPortal.CreateChild(Of RequestAttachments)()
    End Function

    Friend Shared Function GetRequestAttachments(ByVal dr As SafeDataReader) As RequestAttachments
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a DocumentAttachment")
        'End If
        Return DataPortal.FetchChild(Of RequestAttachments)(dr)
    End Function

    Private Sub New()
        AllowNew = True
    End Sub

#End Region ' Factory Methods

#Region " Data Access "

    Private Sub Child_Fetch(ByVal dr As SafeDataReader)
        RaiseListChangedEvents = False
        Database.LogInfo("RequestAttachments.Child_Fetch", GetHashCode())
        Try
            While dr.Read()
                Me.Add(RequestAttachment.GetRequestAttachment(dr))
            End While
        Catch ex As Exception
            Database.LogException("RequestAttachments.Child_Fetch", ex)
            Throw New DbCslaException("RequestAttachments.Child_Fetch", ex)
        End Try
        RaiseListChangedEvents = True
    End Sub


#End Region ' Data Access

End Class
