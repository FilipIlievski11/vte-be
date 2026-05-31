
<Serializable()> _
Public Class ObjectPrivilegeInfo
  Inherits ReadOnlyBase(Of ObjectPrivilegeInfo)

  Private _id As Long
  Public ReadOnly Property Id() As Long
    Get
      Return _id
    End Get
  End Property
  Private _canAddObject As Boolean
  Public ReadOnly Property CanAddObject() As Boolean
    Get
      Return _canAddObject
    End Get
  End Property
  Private _canGetObject As Boolean
  Public ReadOnly Property CanGetObject() As Boolean
    Get
      Return _canGetObject
    End Get
  End Property
  Private _canDeleteObject As Boolean
  Public ReadOnly Property CanDeleteObject() As Boolean
    Get
      Return _canDeleteObject
    End Get
  End Property
  Private _canEditObject As Boolean
  Public ReadOnly Property CanEditObject() As Boolean
    Get
      Return _canEditObject
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
    Return _canaddobject
  End Function

  Public Overrides Function ToString() As String
    Return _canaddobject
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _canAddObject = dr.GetBoolean("CanAddObject")
    _canGetObject = dr.GetBoolean("CanGetObject")
    _canDeleteObject = dr.GetBoolean("CanDeleteObject")
    _canEditObject = dr.GetBoolean("CanEditObject")
    _cSLAObjectName = dr.GetString("CSLAObjectName")
    _cSLAObjectType = dr.GetString("CSLAObjectType")
    _id = dr.GetInt64("ID")
  End Sub

End Class