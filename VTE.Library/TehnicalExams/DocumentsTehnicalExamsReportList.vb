
<Serializable()> _
Public Class DocumentsTehnicalExamsReportList
  Inherits ReadOnlyListBase(Of DocumentsTehnicalExamsReportList, DocumentsTehnicalExamsReportInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "getDocumentsTehnicalExamsReportsList"
  Private Const SpZemiVozilaPoIspravnost As String = "getDocumentsTehnicalExamsReportsListVehicleRightOrNot"
  Private Const SpZemiValidniTehnickiPregledi As String = "getDocumentsTehnicalExamsReportsListByVallidTill"
  Private Const SpZemiSiteOdDo As String = "getDocumentsTehnicalExamsReportsListBetweenDates"
#End Region

#Region " Factory Methods "

  Public Shared Function GetDocumentsTehnicalExamsReportList() As DocumentsTehnicalExamsReportList

    Return DataPortal.Fetch(Of DocumentsTehnicalExamsReportList)()

  End Function
  Public Shared Function GetDocumentsTehnicalExamsReportListNotRightVehicles(ByVal DatumOd As Date, ByVal DatumDo As Date, ByVal IsRight As Boolean) As DocumentsTehnicalExamsReportList

    Return DataPortal.Fetch(Of DocumentsTehnicalExamsReportList)(New CriteriaIsRight(DatumOd, DatumDo, IsRight))

  End Function

  Public Shared Function GetDocumentsTehnicalExamsReportListOdDo(ByVal DatumOd As Date, ByVal DatumDo As Date) As DocumentsTehnicalExamsReportList

    Return DataPortal.Fetch(Of DocumentsTehnicalExamsReportList)(New CriteriaOdDo(DatumOd, DatumDo))

  End Function
  'Public Shared Function GetDocumentsTehnicalExamsReportListOdDoRegistar(ByVal DatumOd As Date, ByVal DatumDo As Date) As DocumentsTehnicalExamsReportList

  ' Return DataPortal.Fetch(Of DocumentsTehnicalExamsReportList)(New CriteriaOdDoRegistar(DatumOd, DatumDo))

  'End Function
  'Public Shared Function GetDocumentsTehnicalExamsReportListVallidTill(ByVal inPom As Integer) As DocumentsTehnicalExamsReportList

  '  Return DataPortal.Fetch(Of DocumentsTehnicalExamsReportList)(New CriteriaVallidTill(inPom))

  'End Function
  Public Function GetById(ByVal inId As Long) As DocumentsTehnicalExamsReportInfo
    For Each child As DocumentsTehnicalExamsReportInfo In Me
      If child.Id = inId Then
        Return child
      End If
    Next
    Return Nothing
  End Function
  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  <Serializable()> _
 Private Class CriteriaIsRight
    Private _InVehicleIsRight As Boolean
    Private _od As Date
    Private _do As Date
    Public ReadOnly Property InVehicleIsRight() As Boolean
      Get
        Return _InVehicleIsRight
      End Get
    End Property
    Public ReadOnly Property DatumOd() As Date
      Get
        Return _od
      End Get
    End Property
    Public ReadOnly Property DatumDo() As Date
      Get
        Return _do
      End Get
    End Property
    Public Sub New(ByVal datumOd As Date, ByVal datumDo As Date, ByVal InVehicleIsRight As Boolean)
      _InVehicleIsRight = InVehicleIsRight
      _od = datumOd.Date
      _do = datumDo.Date
    End Sub

  End Class

  <Serializable()> _
 Private Class CriteriaVallidTill
    Private _InPom As Integer
    Public ReadOnly Property InPom() As Integer
      Get
        Return _InPom
      End Get
    End Property

    Public Sub New(ByVal InPom As Integer)
      _InPom = InPom
    End Sub

  End Class
  <Serializable()> _
 Private Class CriteriaOdDo
    Private _od As Date
    Private _do As Date
    Public ReadOnly Property DatumOd() As Date
      Get
        Return _od
      End Get
    End Property
    Public ReadOnly Property DatumDo() As Date
      Get
        Return _do
      End Get
    End Property
    Public Sub New(ByVal datumOd As Date, ByVal datumDo As Date)
      _od = datumOd.Date
      _do = datumDo.Date
    End Sub

  End Class

  ' <Serializable()> _
  'Private Class CriteriaOdDoRegistar
  '  Private _od As Date
  '  Private _do As Date
  '  Public ReadOnly Property DatumOd() As Date
  '   Get
  '    Return _od
  '   End Get
  '  End Property
  '  Public ReadOnly Property DatumDo() As Date
  '   Get
  '    Return _do
  '   End Get
  '  End Property
  '  Public Sub New(ByVal datumOd As Date, ByVal datumDo As Date)
  '   _od = datumOd.Date
  '   _do = datumDo.Date
  '  End Sub

  ' End Class
  Private Overloads Sub DataPortal_Fetch()
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSite
          cm.Parameters.AddWithValue("@IdOrganisation", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentsTehnicalExamsReportInfo(dr, True)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaIsRight)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiVozilaPoIspravnost
          cm.Parameters.AddWithValue("@isRight", criteria.InVehicleIsRight)
          cm.Parameters.AddWithValue("@od", Format(criteria.DatumOd, "yyyy-MM-dd"))
          cm.Parameters.AddWithValue("@do", Format(criteria.DatumDo.AddDays(1), "yyyy-MM-dd"))
          cm.Parameters.AddWithValue("@IdOrganisation", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentsTehnicalExamsReportInfo(dr, True)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaOdDo)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = SpZemiSiteOdDo
          cm.Parameters.AddWithValue("@od", Format(criteria.DatumOd, "yyyy-MM-dd"))
          cm.Parameters.AddWithValue("@do", Format(criteria.DatumDo.AddDays(1), "yyyy-MM-dd"))
          cm.Parameters.AddWithValue("@IdOrganisation", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New DocumentsTehnicalExamsReportInfo(dr, True)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub
  'Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaOdDoRegistar)
  ' RaiseListChangedEvents = False
  ' IsReadOnly = False
  ' Database.LogInfo("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", GetHashCode())
  ' Try
  '  Using cn As SqlConnection = Database.VTE_SqlConnection
  '   Using cm As SqlCommand = cn.CreateCommand()
  '    cm.CommandType = CommandType.StoredProcedure
  '    cm.CommandText = "getDocumentsTehnicalExamsReportsListBetweenDatesForRegistar"
  '    cm.Parameters.AddWithValue("@od", Format(criteria.DatumOd, "yyyy-MM-dd"))
  '    cm.Parameters.AddWithValue("@do", Format(criteria.DatumDo.AddDays(1), "yyyy-MM-dd"))
  '    cm.Parameters.AddWithValue("@IdOrganisation", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
  '    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
  '     While dr.Read()
  '      Dim Info As New DocumentsTehnicalExamsReportInfo(dr)
  '      Me.Add(Info)
  '     End While
  '    End Using
  '   End Using
  '  End Using
  ' Catch ex As Exception
  '  Database.LogException("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", ex)
  '  Throw New DbCslaException("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", ex)
  ' End Try
  ' IsReadOnly = True
  ' RaiseListChangedEvents = True
  'End Sub
  'Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaVallidTill)
  ' RaiseListChangedEvents = False
  ' IsReadOnly = False
  ' Database.LogInfo("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", GetHashCode())
  ' Try
  '  Using cn As SqlConnection = Database.VTE_SqlConnection
  '   Using cm As SqlCommand = cn.CreateCommand()
  '    cm.CommandType = CommandType.StoredProcedure
  '    cm.CommandText = SpZemiValidniTehnickiPregledi
  '    cm.Parameters.AddWithValue("@IdOrganisation", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
  '    Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
  '     While dr.Read()
  '      Dim Info As New DocumentsTehnicalExamsReportInfo(dr)
  '      Me.Add(Info)
  '     End While
  '    End Using
  '   End Using
  '  End Using
  ' Catch ex As Exception
  '  Database.LogException("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", ex)
  '  Throw New DbCslaException("DocumentsTehnicalExamsReportInfo.DataPortal_Fetch", ex)
  ' End Try
  ' IsReadOnly = True
  ' RaiseListChangedEvents = True
  'End Sub
#End Region ' Data Access
End Class