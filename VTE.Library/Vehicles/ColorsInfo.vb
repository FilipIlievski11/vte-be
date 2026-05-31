
<Serializable()> _
Public Class ColorsInfo
  Inherits ReadOnlyBase(Of ColorsInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
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
  Public ReadOnly Property Color() As String
    Get
      Return _colorCode & "-" & _colorDescription
    End Get
  End Property
  Private _newColorEffects As String
  Public ReadOnly Property NewColorEffects() As String
    Get
      Return _newColorEffects
    End Get
  End Property
  Private _newColorCode As Integer
  Public ReadOnly Property NewColorCode() As Integer
    Get
      Return _newColorCode
    End Get
  End Property
  Private _newColorDarkness As String
  Public ReadOnly Property NewColorDarkness() As String
    Get
      Return _newColorDarkness
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
    _colorcode = dr.GetString("ColorCode")
    _colordescription = dr.GetString("ColorDescription")
    _newcoloreffects = dr.GetString("NewColorEffects")
    _newcolorcode = dr.GetInt32("NewColorCode")
    _newcolordarkness = dr.GetString("NewColorDarkness")
  End Sub

  Friend Sub New(ByVal intId As Integer, ByVal strColorCode As String, ByVal strColorDescription As String, ByVal inEffect As String, ByVal inColorCodeNew As Integer, ByVal inDarkness As String)
    _id = intId
    _colorCode = strColorCode
    _colorDescription = strColorDescription
    _newColorEffects = inEffect
    _newColorCode = inColorCodeNew
    _newColorDarkness = inDarkness
  End Sub

End Class