
<Serializable()> _
Public Class DocumentsTehnicalExamsReports
    Inherits Csla.BusinessListBase(Of DocumentsTehnicalExamsReports, DocumentsTehnicalExamsReport)

#Region " Stored Procedures Names "
    Private Const spGetByID As String = "GetDocumentsTehnicalExamsReportByID"
    Private Const spGetAll As String = "GetDocumentsTehnicalExamsReports"
    Private Const spUpdate As String = "updateDocumentsTehnicalExamsReport"
    Private Const spAdd As String = "addDocumentsTehnicalExamsReport"
    Private Const spDelete As String = "deleteDocumentsTehnicalExamsReport"

#End Region 'Stored Procedures Names

#Region " BindingList Overrides "

    Protected Overrides Function AddNewCore() As Object
        Dim item As DocumentsTehnicalExamsReport = DocumentsTehnicalExamsReport.NewDocumentsTehnicalExamsReportChild()
        Me.Add(item)
        Return item
    End Function

#End Region ' BindingList Overrides

#Region " Authorization Rules "

    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsTehnicalExamsReports")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsTehnicalExamsReports")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsTehnicalExamsReports")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsTehnicalExamsReports")
    End Function

#End Region ' Authorization Rules

#Region " Factory Methods "
    Private Sub New()
        AllowNew = True
    End Sub

    Public Shared Function GetDocumentsTehnicalExamsReports() As DocumentsTehnicalExamsReports
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User Not authorized to view a DocumentsTehnicalExamsReports")
        End If
        Return DataPortal.Fetch(Of DocumentsTehnicalExamsReports)()
    End Function
    Public Shared Function GetDocumentsTehnicalExamsReports(ByVal idVehicle As Long, ByVal idOrganizationForTehnicalExam As Integer) As DocumentsTehnicalExamsReports
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User Not authorized to view a DocumentsTehnicalExamsReports")
        End If
        Return DataPortal.Fetch(Of DocumentsTehnicalExamsReports)()
    End Function
    Public Function GetTechnicalExamReportById(ByVal inId As Long) As DocumentsTehnicalExamsReport
        For Each child As DocumentsTehnicalExamsReport In Me
            If child.Id = inId Then
                Return child
                Exit Function
            End If
        Next
        Return Nothing
    End Function
    Public Shared Function GetDocumentTehnicalExamReportByIdRelation(ByVal inIdRelation As Long) As DocumentsTehnicalExamsReport
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User Not authorized to view a DocumentsTehnicalExamsReports")
        End If
        Dim pom As DocumentsTehnicalExamsReport = Nothing
        pom = (DataPortal.Fetch(Of DocumentsTehnicalExamsReports)(New filterCriteriaByIDRelation(inIdRelation))).Item(0)

        If pom IsNot Nothing Then
            Return pom
        Else
            Return Nothing
        End If
        'For Each child As DocumentsTehnicalExamsReport In Me
        '    If child.IdCustomerVehicleRelation = inIdRelation AndAlso child.ValidTillDate > Now Then
        '        Return child
        '    End If
        'Next
        'Return Nothing
    End Function
#End Region ' Factory Methods

#Region " Data Access "
    <Serializable()> _
Private Class filterCriteriaByIDRelation
        Private _inId As Long

        Public ReadOnly Property InId() As Long
            Get
                Return _inId
            End Get
        End Property

        Public Sub New(ByVal inId As Long)
            _inId = inId
        End Sub
    End Class
    Private Overloads Sub DataPortal_Fetch()
        RaiseListChangedEvents = False
        Database.LogInfo("DocumentsTehnicalExamsReports.Child_Fetch", GetHashCode())
        Try

            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = spGetAll
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        While dr.Read()
                            Me.Add(DocumentsTehnicalExamsReport.GetDocumentsTehnicalExamsReport(dr))
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("DocumentsTehnicalExamsReports.Child_Fetch", ex)
            Throw New DbCslaException("DocumentsTehnicalExamsReports.Child_Fetch", ex)
        End Try
        RaiseListChangedEvents = True
    End Sub

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As filterCriteriaByIDRelation)
        RaiseListChangedEvents = False
        Database.LogInfo("DocumentsTehnicalExamsReports.Child_Fetch", GetHashCode())
        Try

            Using cn As SqlConnection = Database.VTE_SqlConnection
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "getDocumentsTehnicalExamsReportsByIdRelation"
                    cm.Parameters.AddWithValue("@idRelation", criteria.InId)
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        While dr.Read()
                            Me.Add(DocumentsTehnicalExamsReport.GetDocumentsTehnicalExamsReport(dr))
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Database.LogException("DocumentsTehnicalExamsReports.Child_Fetch", ex)
            Throw New DbCslaException("DocumentsTehnicalExamsReports.Child_Fetch", ex)
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
