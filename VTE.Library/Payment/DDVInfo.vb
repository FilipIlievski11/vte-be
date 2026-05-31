
<Serializable()> _
Public Class DDVInfo
  Inherits ReadOnlyBase(Of DDVInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _dDVName As String
  Public ReadOnly Property DDVName() As String
    Get
      Return _dDVName
    End Get
  End Property
  Private _dDVValue As Decimal
  Public ReadOnly Property DDVValue() As Decimal
    Get
      Return _dDVValue
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
    _ddvname = dr.GetString("DDVName")
    _ddvvalue = dr.GetDecimal("DDVValue")
  End Sub

End Class