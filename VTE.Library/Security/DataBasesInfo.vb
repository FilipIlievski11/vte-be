
<Serializable()> _
Public Class DataBasesInfo
  Inherits ReadOnlyBase(Of DataBasesInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _endUserName As String
  Public ReadOnly Property EndUserName() As String
    Get
      Return _endUserName
    End Get
  End Property
  Private _databaseName As String
  Public ReadOnly Property DatabaseName() As String
    Get
      Return _databaseName
    End Get
  End Property
  Private _connetionString As String
  Public ReadOnly Property ConnetionString() As String
    Get
      Return _connetionString
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
    _endusername = dr.GetString("EndUserName")
    _databasename = dr.GetString("DatabaseName")
    _connetionstring = dr.GetString("ConnetionString")
  End Sub

End Class