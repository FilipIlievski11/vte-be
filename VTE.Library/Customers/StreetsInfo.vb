
<Serializable()> _
Public Class StreetsInfo
  Inherits ReadOnlyBase(Of StreetsInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _streetName As String
  Public ReadOnly Property StreetName() As String
    Get
      Return _streetName
    End Get
  End Property
  Private _note As String
  Public ReadOnly Property Note() As String
    Get
      Return _note
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
    _streetname = dr.GetString("StreetName")
    _note = dr.GetString("Note")
  End Sub
    Friend Sub New(ByVal intId As Integer, ByVal strName As String, ByVal strNote As String)
        _id = intId
        _streetName = strName
        _note = strNote
       
    End Sub
End Class