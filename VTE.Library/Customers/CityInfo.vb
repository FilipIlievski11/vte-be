
<Serializable()> _
Public Class CityInfo
  Inherits ReadOnlyBase(Of CityInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _cityName As String
  Public ReadOnly Property CityName() As String
    Get
      Return _cityName
    End Get
  End Property

  Public ReadOnly Property Name() As String
    Get
      Return _cityName & "-" & _communityName & "-" & _countryName
    End Get
  End Property

  Private _cityZip As Integer
  Public ReadOnly Property CityZip() As Integer
    Get
      Return _cityZip
    End Get
  End Property
  Private _idCommunity As Integer
  Public ReadOnly Property IdCommunity() As Integer
    Get
      Return _idCommunity
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
  Private _idCountry As Integer
  Public ReadOnly Property IdCountry() As Integer
    Get
      Return _idCountry
    End Get
  End Property
  Private _countryName As String
  Public ReadOnly Property CountryName() As String
    Get
      Return _countryName
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _cityName
  End Function



  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("Id")
    _cityname = dr.GetString("CityName")
    _cityzip = dr.GetInt32("CityZip")
    _idcommunity = dr.GetInt32("IdCommunity")
    _communitycode = dr.GetString("CommunityCode")
    _communityname = dr.GetString("CommunityName")
    _idcountry = dr.GetInt32("IdCountry")
    _countryname = dr.GetString("CountryName")
  End Sub

End Class