
<Serializable()> _
Public Class DocumentsInternationalDriveingLicenceInfo
  Inherits ReadOnlyBase(Of DocumentsInternationalDriveingLicenceInfo)


  Private _customerDisplay As String
  Public ReadOnly Property CustomerDisplay() As String
    Get
      Return _customerDisplay
    End Get
  End Property

  Public ReadOnly Property IssuerDisplay() As String
    Get
      Return _organizationName & ", " & _station
    End Get
  End Property



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
  Private _customerSurname As String = String.Empty
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
  Private _idIssuer As Integer
  Public ReadOnly Property IdIssuer() As Integer
    Get
      Return _idIssuer
    End Get
  End Property
  Private _organizationName As String
  Public ReadOnly Property OrganizationName() As String
    Get
      Return _organizationName
    End Get
  End Property
  Private _station As String
  Public ReadOnly Property Station() As String
    Get
      Return _station
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
      Return _note
    End Get
  End Property

  Private _ParentName As String
  Public ReadOnly Property ParentName() As String
    Get
      Return _ParentName
    End Get
  End Property
  Private _DateOfBirth As Date
  Public ReadOnly Property DateOfBirth() As Date
    Get
      Return _DateOfBirth
    End Get
  End Property
  Private _BirthCity As String
  Public ReadOnly Property BirthCity() As String
    Get
      Return _BirthCity
    End Get
  End Property
  Private _LiveInCity As String
  Public ReadOnly Property LiveInCity() As String
    Get
      Return _LiveInCity
    End Get
  End Property
  Private _StreetName As String
  Public ReadOnly Property StreetName() As String
    Get
      Return _StreetName
    End Get
  End Property
  Private _LivingAddressNumber As String
  Public ReadOnly Property LivingAddressNumber() As String
    Get
      Return _LivingAddressNumber
    End Get
  End Property
  Private _WorksInCompany As String
  Public ReadOnly Property WorksInCompany() As String
    Get
      Return _WorksInCompany
    End Get
  End Property
  Private _Occupation As String
  Public ReadOnly Property Occupation() As String
    Get
      Return _Occupation
    End Get
  End Property

  Public ReadOnly Property LivingAddress() As String
    Get
      Return _StreetName & " " & _LivingAddressNumber
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function
  Private _detali As String
  Public ReadOnly Property Categories()
    Get
      Return _detali
    End Get
  End Property
  Private _IssuerName As String
  Public ReadOnly Property IssuerName() As String
    Get
      Return _IssuerName
    End Get
  End Property
  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt64("Id")
    _idCustomer = dr.GetInt64("IdCustomer")
    _mb = dr.GetString("MB")
    _customerSurname = dr.GetString("CustomerSurname")
    _customerFirstName = dr.GetString("CustomerFirstName")
    _idIssuer = dr.GetInt32("IdIssuer")
    _organizationName = dr.GetString("OrganizationName")
    _station = dr.GetString("Station")
    _numberOfLicence = dr.GetString("NumberOfLicence")
    _numberOfNationalLicence = dr.GetString("NumberOfNationalLicence")
    _dateCreated = dr.GetDateTime("DateCreated")
    _validTillDate = dr.GetDateTime("ValidTillDate")
    _note = dr.GetString("Note")
    _ParentName = dr.GetString("ParentName")
    _DateOfBirth = dr.GetDateTime("DateOfBirth")
    _BirthCity = dr.GetString("BirthCity")
    _LiveInCity = dr.GetString("LiveInCity")
    _StreetName = dr.GetString("StreetName")
    _LivingAddressNumber = dr.GetString("LivingAddressNumber")
    _WorksInCompany = dr.GetString("WorksInCompany")
    _Occupation = dr.GetString("Occupation")
    If _customerSurname <> String.Empty Then
      _customerDisplay = _customerFirstName & " " & _ParentName & " " & _customerSurname
    Else
      _customerDisplay = _customerFirstName & " " & _ParentName
    End If
    Dim detali As DocumentsInternationalDriveingLicenceValidForCategories = _
     DocumentsInternationalDriveingLicence.GetDocumentsInternationalDriveingLicence(_id).ValidForCategories
    Dim pomStr As String = ""
    Dim categories As DriveingLicenceCtegoryList = CType(Csla.ApplicationContext.LocalContext.Item("objDriveingLicenceCtegoryList"), DriveingLicenceCtegoryList)

    For Each detal As DocumentsInternationalDriveingLicenceValidForCategorie In detali
      If detal.IsCheck Then

        pomStr &= categories.GetInfo(detal.IdLicenceCategorie).Code & "; "
      End If
    Next
    If pomStr.Length > 0 Then
      _detali = pomStr.Substring(0, pomStr.Length - 2)
    Else
      _detali = ""
    End If

    _IssuerName = dr.GetString("IssuerName")
  End Sub

End Class