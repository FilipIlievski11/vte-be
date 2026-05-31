
<Serializable()> _
Public Class CSLAObjectsInfo
  Inherits ReadOnlyBase(Of CSLAObjectsInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _idCSLAObject As Integer
  Public ReadOnly Property IdCSLAObject() As Integer
    Get
      Return _idCSLAObject
    End Get
  End Property
  Private _cSLAObjectName As String
  Public ReadOnly Property CSLAObjectName() As String
    Get
      Return _cSLAObjectName
    End Get
  End Property
  Private _cSLAObjectType As String
  Public ReadOnly Property CSLAObjectType() As String
    Get
      Return _cSLAObjectType
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
    _idcslaobject = dr.GetInt32("IdCSLAObject")
    _cslaobjectname = dr.GetString("CSLAObjectName")
    _cslaobjecttype = dr.GetString("CSLAObjectType")
  End Sub

End Class