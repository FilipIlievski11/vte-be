
<Serializable()> _
Public Class DocumentTypesInfo
  Inherits ReadOnlyBase(Of DocumentTypesInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _documentTypeName As String
  Public ReadOnly Property DocumentTypeName() As String
    Get
      Return _documentTypeName
    End Get
  End Property
  Private _isBidirectional As Boolean
  Public ReadOnly Property IsBidirectional() As Boolean
    Get
      Return _isBidirectional
    End Get
  End Property
  Private _isVehiceRequired As Boolean
  Public ReadOnly Property IsVehiceRequired() As Boolean
    Get
      Return _isVehiceRequired
    End Get
  End Property
  Private _isTechnicalExamRequired As Boolean
  Public ReadOnly Property IsTechnicalExamRequired() As Boolean
    Get
      Return _isTechnicalExamRequired
    End Get
  End Property
  Private _isPayRequired As Boolean
  Public ReadOnly Property IsPayRequired() As Boolean
    Get
      Return _isPayRequired
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
    _documenttypename = dr.GetString("DocumentTypeName")
    _isbidirectional = dr.GetBoolean("IsBidirectional")
    _isvehicerequired = dr.GetBoolean("IsVehiceRequired")
    _istechnicalexamrequired = dr.GetBoolean("IsTechnicalExamRequired")
    _ispayrequired = dr.GetBoolean("IsPayRequired")
  End Sub

End Class