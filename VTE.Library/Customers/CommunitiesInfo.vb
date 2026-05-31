
<Serializable()> _
Public Class CommunitiesInfo
  Inherits ReadOnlyBase(Of CommunitiesInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _communityCode As String
  Public ReadOnly Property CommunityCode() As String
    Get
      Return _communityCode
    End Get
  End Property
  Private _communityName As String
  Public ReadOnly Property CommunityName() As String
    Get
      Return _communityName
    End Get
  End Property
  Private _registrationCode As String
  Public ReadOnly Property RegistrationCode() As String
    Get
      Return _registrationCode
    End Get
  End Property
  Public ReadOnly Property Name() As String
    Get
      Return _communityCode & "-" & _communityName
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
    _communitycode = dr.GetString("CommunityCode")
    _communityName = dr.GetString("CommunityName")
    _registrationCode = dr.GetString("RegistrationCode")
  End Sub
    Friend Sub New(ByVal intId As Integer, ByVal strCommunityCode As String, ByVal strCommunityName As String, _
                  ByVal strRegistrationCode As String)
        _id = intId
        _communityCode = strCommunityCode
        _communityName = strCommunityName
        _registrationCode = strRegistrationCode
    End Sub
End Class