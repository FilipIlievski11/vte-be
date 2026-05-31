
<Serializable()> _
Public Class TehnicalExamsTypesInfo
  Inherits ReadOnlyBase(Of TehnicalExamsTypesInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _description As String
  Public ReadOnly Property Description() As String
    Get
      Return _description
    End Get
    End Property
    Private _code As String
    Public ReadOnly Property Code() As String
        Get
            Return _code
        End Get
    End Property
  Private _validNumOfDays As Integer
  Public ReadOnly Property ValidNumOfDays() As Integer
    Get
      Return _validNumOfDays
    End Get
  End Property
  Private _percentOfFullExam As Integer
  Public ReadOnly Property PercentOfFullExam() As Integer
    Get
      Return _percentOfFullExam
    End Get
    End Property
    Private _IsInRegistar As Boolean
    Public ReadOnly Property IsInRegistar() As Boolean
        Get
            Return _IsInRegistar
        End Get
    End Property

  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("Id")
        _description = dr.GetString("Description")
        _code = dr.GetString("Code")
    _validNumOfDays = dr.GetInt32("ValidNumOfDays")
        _percentOfFullExam = dr.GetInt32("PercentOfFullExam")
        _IsInRegistar = dr.GetBoolean("IsInRegistar")
  End Sub
    Friend Sub New(ByVal intId As Integer, ByVal strDescription As String, ByVal strCode As String, _
                   ByVal inValidNumDays As Integer, ByVal inPercent As Integer, ByVal IsInRegistar As Boolean)
        _id = intId
        _description = strDescription
        _code = strCode
        _validNumOfDays = inValidNumDays
        _percentOfFullExam = inPercent
        _IsInRegistar = IsInRegistar
    End Sub
End Class