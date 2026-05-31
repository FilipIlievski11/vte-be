
<Serializable()> _
Public Class PrintDocumentInternationalDriveingLicenceInfo
  Inherits ReadOnlyBase(Of PrintDocumentInternationalDriveingLicenceInfo)

  Private _id As Long
  Public ReadOnly Property Id() As Long
    Get
      Return _id
    End Get
  End Property
  Private _idCustomer As Long
  Public ReadOnly Property IdCustomer() As Long
    Get
      Return _idCustomer
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
      Return UCase(ToLat(_customerSurname))
    End Get
  End Property
  Private _customerFirstName As String
  Public ReadOnly Property CustomerFirstName() As String
    Get
      Return UCase(ToLat(_customerFirstName))
    End Get
    End Property
    Public ReadOnly Property CustomerName() As String
        Get
            Return UCase(ToLat(_customerFirstName) & " " & ToLat(_customerSurname))
        End Get
    End Property
  Private _idIssuer As Integer
  Public ReadOnly Property IdIssuer() As Integer
    Get
      Return _idIssuer
    End Get
  End Property
  Private _organizationName As String
  Public ReadOnly Property OrganizationName() As String
    Get
      Return UCase(ToLat(_organizationName))
    End Get
  End Property
  Private _station As String
  Public ReadOnly Property Station() As String
    Get
      Return UCase(ToLat(_station))
    End Get
  End Property
  Private _numberOfLicence As String
  Public ReadOnly Property NumberOfLicence() As String
    Get
      Return _numberOfLicence
    End Get
  End Property
  Private _numberOfNationalLicence As String
  Public ReadOnly Property NumberOfNationalLicence() As String
    Get
      Return _numberOfNationalLicence
    End Get
  End Property
  Private _dateCreated As Date
  Public ReadOnly Property DateCreated() As Date
    Get
      Return _dateCreated
    End Get
  End Property
  Private _validTillDate As Date
  Public ReadOnly Property ValidTillDate() As Date
    Get
      Return _validTillDate
    End Get
  End Property
  Private _note As String
  Public ReadOnly Property Note() As String
    Get
            Return ToLat(_note)
    End Get
  End Property
  Private _idBirhCity As Integer
  Public ReadOnly Property IdBirhCity() As Integer
    Get
      Return _idBirhCity
    End Get
  End Property
  Private _birthCity As String
  Public ReadOnly Property BirthCity() As String
    Get
      Return UCase(ToLat(_birthCity))
    End Get
  End Property
  Private _dateOfBirth As Date
  Public ReadOnly Property DateOfBirth() As Date
    Get
      Return _dateOfBirth
    End Get
  End Property
  Private _CityIssuedFrom As String
  Public ReadOnly Property CityIssuedFrom() As String
    Get
      Return UCase(ToLat(_CityIssuedFrom))
    End Get
  End Property
  Private _LivingCity As String
  Public ReadOnly Property LivingCity() As String
    Get
      Return UCase(ToLat(_LivingCity))
    End Get
  End Property

    Private _passNum As String
    Public ReadOnly Property PassNum() As String
        Get
            Return UCase(ToLat(_passNum))
        End Get
    End Property
    Private _birthAddressNumber As String
    Public ReadOnly Property BirthAddressNumber() As String
        Get
            Return UCase(ToLat(_birthAddressNumber))
        End Get
    End Property
    Private _citizenship As String
    Public ReadOnly Property Citizenship() As String
        Get
            Return UCase(ToLat(_citizenship))
        End Get
    End Property
    Private _blk As String
    Public ReadOnly Property BLK() As String
        Get
            Return UCase(ToLat(_blk))
        End Get
    End Property
    Private _livingAddressNumber As String
    Public ReadOnly Property LivingAddressNumber() As String
        Get
            Return UCase(ToLat(_livingAddressNumber))
        End Get
    End Property
    Private _streetName As String
    Public ReadOnly Property StreetName() As String
        Get
            Return UCase(ToLat(_streetName))
        End Get
    End Property
    Public ReadOnly Property LivingAddressStreetAndNumber() As String
        Get
            Return UCase(ToLat(_streetName) & " " & ToLat(_livingAddressNumber) & " " & LivingCity)
        End Get
    End Property
    Private _DriveingLicenceIssuer As String
    Public ReadOnly Property DriveingLicenceIssuer() As String
        Get
            Return UCase(ToLat(_DriveingLicenceIssuer))
        End Get
    End Property
    Private _DriveingLicenceDateIssued As Date
    Public ReadOnly Property DriveingLicenceDateIssued() As String
        Get
            If _DriveingLicenceDateIssued.Date = DateTime.MinValue.Date Then
                Return ""
            Else
                Return _DriveingLicenceDateIssued
            End If

        End Get
    End Property
    Private _PassIssuer As String
    Public ReadOnly Property PassIssuer() As String
        Get
            Return UCase(ToLat(_PassIssuer))
        End Get
    End Property
    Private _PassDateIssued As Date
    Public ReadOnly Property PassDateIssued() As String
        Get
            If _PassDateIssued.Date = DateTime.MinValue.Date Then
                Return ""
            Else
                Return _PassDateIssued
            End If

        End Get
    End Property
    Private _BLKIssuer As String
    Public ReadOnly Property BLKIssuer() As String
        Get
            Return UCase(ToLat(_BLKIssuer))
        End Get
    End Property
    Private _BLKDateIssued As Date
    Public ReadOnly Property BLKDateIssued() As String
        Get
            If _BLKDateIssued.Date = DateTime.MinValue.Date Then
                Return ""
            Else
                Return _BLKDateIssued
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
    _idcustomer = dr.GetInt64("IdCustomer")
    _mb = dr.GetString("MB")
    _customersurname = dr.GetString("CustomerSurname")
    _customerfirstname = dr.GetString("CustomerFirstName")
    _idissuer = dr.GetInt32("IdIssuer")
    _organizationname = dr.GetString("OrganizationName")
    _station = dr.GetString("Station")
    _numberoflicence = dr.GetString("NumberOfLicence")
    _numberofnationallicence = dr.GetString("NumberOfNationalLicence")
    _datecreated = dr.GetDateTime("DateCreated")
    _validtilldate = dr.GetDateTime("ValidTillDate")
    _note = dr.GetString("Note")
    _idbirhcity = dr.GetInt32("IdBirhCity")
    _birthcity = dr.GetString("BirthCity")
    _dateOfBirth = dr.GetDateTime("DateOfBirth")
    _CityIssuedFrom = dr.GetString("CityIssuedFrom")
        _LivingCity = dr.GetString("LivingCity")
        _passNum = dr.GetString("PassportNumber")
        _birthAddressNumber = dr.GetString("BrithAddressNumber")
        _citizenship = dr.GetString("Citizenship")
        _blk = dr.GetString("BLK")
        _livingAddressNumber = dr.GetString("LivingAddressNumber")
        _streetName = dr.GetString("OwnerStreet")
        _DriveingLicenceIssuer = dr.GetString("DriveingLicenceIssuer")
        _DriveingLicenceDateIssued = dr.GetDateTime("DriveingLicenceDateIssued")
        _PassIssuer = dr.GetString("PassIssuer")
        _PassDateIssued = dr.GetDateTime("PassDateIssued")
        _BLKDateIssued = dr.GetDateTime("BLKDateIssued")
        _BLKIssuer = dr.GetString("BLKIssuer")
  End Sub

End Class