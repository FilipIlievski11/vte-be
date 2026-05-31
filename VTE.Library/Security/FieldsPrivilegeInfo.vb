
<Serializable()> _
Public Class FieldsPrivilegeInfo
  Inherits ReadOnlyBase(Of FieldsPrivilegeInfo)

  Private _cSLAObjectPropertyName As String
  Public ReadOnly Property CSLAObjectPropertyName() As String
    Get
      Return _cSLAObjectPropertyName
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
  Private _id As Long
  Public ReadOnly Property ID() As Long
    Get
      Return _id
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _cslaobjectpropertyname
  End Function

  Public Overrides Function ToString() As String
    Return _cslaobjectpropertyname
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _cslaobjectpropertyname = dr.GetString("CSLAObjectPropertyName")
    _cslaobjectname = dr.GetString("CSLAObjectName")
    _cslaobjecttype = dr.GetString("CSLAObjectType")
    _id = dr.GetInt64("ID")
  End Sub

End Class