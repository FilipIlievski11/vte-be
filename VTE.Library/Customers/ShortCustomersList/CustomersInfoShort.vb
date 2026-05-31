

<Serializable()> _
Public Class CustomersInfoShort
    Inherits ReadOnlyBase(Of CustomersInfoShort)

    Private _id As Long
    Public ReadOnly Property Id() As Long
        Get
            Return _id
        End Get
    End Property
    Private _mb As String
    Public ReadOnly Property MB() As String
        Get
            Return _mb
        End Get
    End Property
    Private _customerSurname As String
    Public ReadOnly Property CustomerSurname() As String
        Get
            Return _customerSurname
        End Get
    End Property
    Private _customerFirstName As String
    Public ReadOnly Property CustomerFirstName() As String
        Get
            Return _customerFirstName
        End Get
    End Property
    Private _livingAddressNumber As String
    Public ReadOnly Property LivingAddressNumber() As String
        Get
            Return _livingAddressNumber
        End Get
    End Property
    Private _isCompany As Boolean
    Public ReadOnly Property IsCompany() As Boolean
        Get
            Return _isCompany
        End Get
    End Property
    Private _cityName As String
    Public ReadOnly Property CityName() As String
        Get
            Return _cityName
        End Get
    End Property
    Private _countryName As String
    Public ReadOnly Property CountryName() As String
        Get
            Return _countryName
        End Get
    End Property
    Private _streetName As String
    Public ReadOnly Property StreetName() As String
        Get
            Return _streetName
        End Get
    End Property
    Public ReadOnly Property Name() As String
        Get
            Return _customerFirstName & " " & _customerSurname & " " & _mb
        End Get
    End Property
    Public ReadOnly Property AddressOfLiving() As String
        Get
            If _streetName = String.Empty Then
                If _cityName <> String.Empty Then
                    Return _cityName
                Else
                    Return Nothing
                End If
            Else
                If _livingAddressNumber = String.Empty Then
                    If _cityName = String.Empty Then
                        Return _streetName
                    Else
                        Return _streetName & "; " & _cityName
                    End If
                    Return _streetName
                Else
                    Return _streetName & " бр." & _livingAddressNumber & "; " & _cityName
                End If
            End If
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
        _mb = dr.GetString("MB")
        _customerSurname = dr.GetString("CustomerSurname")
        _customerFirstName = dr.GetString("CustomerFirstName")
        _livingAddressNumber = dr.GetString("LivingAddressNumber")
        _isCompany = dr.GetBoolean("IsCompany")
        _cityName = dr.GetString("CityName")
        _countryName = dr.GetString("CountryName")
        _streetName = dr.GetString("StreetName")

    End Sub


End Class
