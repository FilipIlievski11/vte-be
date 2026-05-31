
<Serializable()> _
Public Class DriveingLicenceCtegoryInfo
  Inherits ReadOnlyBase(Of DriveingLicenceCtegoryInfo)

  Private _id As Long
  Public ReadOnly Property Id() As Long
    Get
      Return _id
    End Get
  End Property
  Private _code As String
  Public ReadOnly Property Code() As String
    Get
      Return _code
    End Get
  End Property
  Private _description As String
  Public ReadOnly Property Description() As String
    Get
      Return _description
    End Get
  End Property
  Public ReadOnly Property CodeAndDescription() As String
    Get
      Return _code & "-" & _description
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt64("Id")
    _code = dr.GetString("Code")
    _description = dr.GetString("Description")
  End Sub

End Class