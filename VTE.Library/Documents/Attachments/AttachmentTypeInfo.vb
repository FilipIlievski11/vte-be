
<Serializable()> _
Public Class AttachmentTypeInfo
  Inherits ReadOnlyBase(Of AttachmentTypeInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _attachmentType As String
  Public ReadOnly Property AttachmentType() As String
    Get
      Return _attachmentType
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
    _attachmenttype = dr.GetString("AttachmentType")
  End Sub

End Class