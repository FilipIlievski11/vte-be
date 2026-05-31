
<Serializable()> _
Public Class ColorDetailsInfo
  Inherits ReadOnlyBase(Of ColorDetailsInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _idColor As Integer
  Public ReadOnly Property IdColor() As Integer
    Get
      Return _idColor
    End Get
  End Property
  Private _colorCode As String
  Public ReadOnly Property ColorCode() As String
    Get
      Return _colorCode
    End Get
  End Property
  Private _colorDescription As String
  Public ReadOnly Property ColorDescription() As String
    Get
      Return _colorDescription
    End Get
  End Property
  Private _baseColorCode As String
  Public ReadOnly Property BaseColorCode() As String
    Get
      Return _baseColorCode
    End Get
  End Property
  Public ReadOnly Property FullColor() As String
    Get
      Return _baseColorCode & "-" & _baseColorDescription & "; " & _colorCode & "-" & _colorDescription
    End Get
  End Property

  Public ReadOnly Property ColorCodeAndDescription() As String
    Get
      Return _colorCode & "-" & _colorDescription
    End Get
  End Property
  Private _baseColorDescription As String
  Public ReadOnly Property BaseColorDescription() As String
    Get
      Return _baseColorDescription
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
    _idcolor = dr.GetInt32("IdColor")
    _colorcode = dr.GetString("ColorCode")
    _colordescription = dr.GetString("ColorDescription")
    _basecolorcode = dr.GetString("BaseColorCode")
    _basecolordescription = dr.GetString("BaseColorDescription")
  End Sub

End Class