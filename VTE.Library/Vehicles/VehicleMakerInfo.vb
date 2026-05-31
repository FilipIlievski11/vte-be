
<Serializable()> _
Public Class VehicleMakerInfo
  Inherits ReadOnlyBase(Of VehicleMakerInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _idCountry As Integer
  Public ReadOnly Property IdCountry() As Integer
    Get
      Return _idCountry
    End Get
  End Property
  Private _companyName As String
  Public ReadOnly Property CompanyName() As String
    Get
      Return _companyName
    End Get
  End Property
  Private _companyTrademark As String
  Public ReadOnly Property CompanyTrademark() As String
    Get
      Return _companyTrademark
    End Get
  End Property
  Public ReadOnly Property CompanyNameAndTrademark() As String
    Get
      Return (CompanyName & ": " & CompanyTrademark)
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
    _idcountry = dr.GetInt32("IdCountry")
    _companyname = dr.GetString("CompanyName")
    _companytrademark = dr.GetString("CompanyTrademark")
  End Sub

End Class