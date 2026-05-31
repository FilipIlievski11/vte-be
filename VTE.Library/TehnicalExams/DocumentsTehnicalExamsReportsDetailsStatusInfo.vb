
<Serializable()> _
Public Class DocumentsTehnicalExamsReportsDetailsStatusInfo
  Inherits ReadOnlyBase(Of DocumentsTehnicalExamsReportsDetailsStatusInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _statusName As String
  Public ReadOnly Property StatusName() As String
    Get
      Return _statusName
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
    _statusname = dr.GetString("StatusName")
  End Sub

End Class