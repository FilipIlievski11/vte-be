
<Serializable()> _
Public Class NovRegistarList
  Inherits ReadOnlyListBase(Of NovRegistarList, NovRegistarInfo)

#Region " Stored Procedures Names "
  Private Const SpZemiSite As String = "NovRegistar"
#End Region

#Region " Factory Methods "

  Public Shared Function GetNovRegistarList(ByVal DatumOd As Date, ByVal DatumDo As Date) As NovRegistarList

    Return DataPortal.Fetch(Of NovRegistarList)(New CriteriaOdDoRegistar(DatumOd, DatumDo))

  End Function

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region ' Factory Methods

#Region " Data Access "
  <Serializable()> _
Private Class CriteriaOdDoRegistar
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
  Private Overloads Sub DataPortal_Fetch(ByVal criteria As CriteriaOdDoRegistar)
    RaiseListChangedEvents = False
    IsReadOnly = False
    Database.LogInfo("NovRegistarInfo.DataPortal_Fetch", GetHashCode())
    Try
      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand()
          cm.CommandType = CommandType.StoredProcedure
          cm.Parameters.AddWithValue("@od", Format(Criteria.DatumOd, "yyyy-MM-dd"))
          cm.Parameters.AddWithValue("@do", Format(criteria.DatumDo.AddDays(1), "yyyy-MM-dd"))
          cm.Parameters.AddWithValue("@IdOrganisation", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)

          cm.CommandText = SpZemiSite
          Using dr As SafeDataReader = New SafeDataReader(cm.ExecuteReader())
            While dr.Read()
              Dim Info As New NovRegistarInfo(dr)
              Me.Add(Info)
            End While
          End Using
        End Using
      End Using
    Catch ex As Exception
      Database.LogException("NovRegistarInfo.DataPortal_Fetch", ex)
      Throw New DbCslaException("NovRegistarInfo.DataPortal_Fetch", ex)
    End Try
    IsReadOnly = True
    RaiseListChangedEvents = True
  End Sub

#End Region ' Data Access
End Class