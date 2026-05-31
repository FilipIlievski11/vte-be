

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Csla
Imports Csla.Data

<Serializable()> _
Public Class DocumentsTehnicalExamsReportsVisualErrors
    Inherits Csla.BusinessListBase(Of DocumentsTehnicalExamsReportsVisualErrors, DocumentsTehnicalExamsReportsVisualError)

#Region " BindingList Overrides "

    Protected Overrides Function AddNewCore() As Object
        Dim item As DocumentsTehnicalExamsReportsVisualError = DocumentsTehnicalExamsReportsVisualError.NewDocumentsTehnicalExamsReportsVisualErrorChild()
        Me.Add(item)
        Return item
    End Function

#End Region ' BindingList Overrides

#Region " Factory Methods "

    Friend Shared Function NewDocumentsTehnicalExamsReportsVisualErrors() As DocumentsTehnicalExamsReportsVisualErrors
        Return DataPortal.CreateChild(Of DocumentsTehnicalExamsReportsVisualErrors)()
    End Function

    Friend Shared Function GetDocumentsTehnicalExamsReportsVisualErrors(ByVal dr As SafeDataReader) As DocumentsTehnicalExamsReportsVisualErrors
        Return DataPortal.FetchChild(Of DocumentsTehnicalExamsReportsVisualErrors)(dr)
    End Function

    Private Sub New()
        Me.AllowNew = True
    End Sub

  

#End Region ' Factory Methods

#Region " Data Access "

    Private Sub Child_Fetch(ByVal dr As SafeDataReader)
        RaiseListChangedEvents = False
        Database.LogInfo("DocumentsTehnicalExamsReportsVisualErrors.Child_Fetch", GetHashCode())
        Try
            While dr.Read()
                Me.Add(DocumentsTehnicalExamsReportsVisualError.GetDocumentsTehnicalExamsReportsVisualError(dr))
            End While
        Catch ex As Exception
            Database.LogException("DocumentsTehnicalExamsReportsVisualErrors.Child_Fetch", ex)
            Throw New DbCslaException("DocumentsTehnicalExamsReportsVisualErrors.Child_Fetch", ex)
        End Try
        RaiseListChangedEvents = True
    End Sub


#End Region ' Data Access

End Class
