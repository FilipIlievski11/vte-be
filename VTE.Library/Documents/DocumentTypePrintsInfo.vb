
<Serializable()> _
Public Class DocumentTypePrintsInfo
  Inherits ReadOnlyBase(Of DocumentTypePrintsInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _opis As String
  Public ReadOnly Property Opis() As String
    Get
      Return _opis
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
    _opis = dr.GetString("Opis")
  End Sub

End Class