
<Serializable()> _
Public Class CountriesInfo
  Inherits ReadOnlyBase(Of CountriesInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _countryName As String
  Public ReadOnly Property CountryName() As String
    Get
      Return _countryName
    End Get
  End Property
  Private _Citizenship As String
  Public ReadOnly Property Citizenship() As String
    Get
      Return _Citizenship
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
    _countryName = dr.GetString("CountryName")
    _Citizenship = dr.GetString("Citizenship")
  End Sub

End Class