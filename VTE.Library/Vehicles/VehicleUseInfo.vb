
<Serializable()> _
Public Class VehicleUseInfo
  Inherits ReadOnlyBase(Of VehicleUseInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _useDescription As String
  Public ReadOnly Property UseDescription() As String
    Get
      Return _useDescription
    End Get
  End Property
  Private _registrationMask As String
  Public ReadOnly Property RegistrationMask() As String
    Get
      Return _RegistrationMask
    End Get
  End Property

  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal intId As Integer, ByVal strUseDescription As String, ByVal registrationMask As String)
    _id = intId
    _useDescription = strUseDescription
    _registrationMask = registrationMask
  End Sub

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("Id")
    _useDescription = dr.GetString("UseDescription")
    _registrationMask = dr.GetString("RegistrationMask")
  End Sub

End Class