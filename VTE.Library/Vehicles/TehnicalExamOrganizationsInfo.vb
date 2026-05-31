
<Serializable()> _
Public Class TehnicalExamOrganizationsInfo
 Inherits ReadOnlyBase(Of TehnicalExamOrganizationsInfo)

 Private _id As Integer
 Public ReadOnly Property Id() As Integer
  Get
   Return _id
  End Get
 End Property
 Private _IdCompany As Integer
 Public ReadOnly Property IdCompany() As Integer
  Get
   Return _IdCompany
  End Get
 End Property
 Private _organizationName As String
 Public ReadOnly Property OrganizationName() As String
  Get
   Return _organizationName
  End Get
 End Property
 Public ReadOnly Property OrganizationAndStationName() As String
  Get
   Return _organizationName & " - " & _station
  End Get
 End Property
 Private _station As String
 Public ReadOnly Property Station() As String
  Get
   Return _station
  End Get
 End Property
 Private _idCity As Integer
 Public ReadOnly Property IdCity() As Integer
  Get
   Return _idCity
  End Get
 End Property
 Private _code As String
 Public ReadOnly Property Code() As String
  Get
   Return _code
  End Get
 End Property
 Private _ApproveRequestAutomate As Boolean
 Public ReadOnly Property ApproveRequestAutomate() As Boolean
  Get
   Return (_ApproveRequestAutomate)
  End Get

 End Property
 Private _NewTechnicalExamReport As Boolean
 Public ReadOnly Property NewTechnicalExamReport() As Boolean
  Get
   Return (_NewTechnicalExamReport)
  End Get
 End Property
 Private _IdCommunity As Integer
 Public ReadOnly Property IdCommunity() As Integer
  Get
   Return _IdCommunity
  End Get
 End Property
 Private _TrafficLicenceVlidNumOfMonths As Integer
 Public ReadOnly Property TrafficLicenceVlidNumOfMonths() As Integer
  Get
   Return _TrafficLicenceVlidNumOfMonths
  End Get

 End Property
 Private _RegistrationVlidNumOfMonths As Integer
 Public ReadOnly Property RegistrationVlidNumOfMonths() As Integer
  Get
   Return _RegistrationVlidNumOfMonths
  End Get

 End Property

 Private _OdgovorenOrgan As String
 Public ReadOnly Property OdgovorenOrgan() As String
  Get
   Return _OdgovorenOrgan
  End Get
 End Property
 Private _Sekretar As String
 Public ReadOnly Property Sekretar() As String
  Get
   Return _Sekretar
  End Get
 End Property
 Private _IdPaymentPrintOption As Integer
 Public ReadOnly Property IdPaymentPrintOption() As Integer
  Get
   Return _IdPaymentPrintOption
  End Get

 End Property
 Private _IdDefaultRegistrationIssuer As Integer
 Public ReadOnly Property IdDefaultRegistrationIssuer() As Integer
  Get
   Return _IdDefaultRegistrationIssuer
  End Get
 End Property
 Private _IdDefaultCity As Integer
 Public ReadOnly Property IdDefaultCity() As Integer
  Get
   Return _IdDefaultCity
  End Get

 End Property
 Private _AutmateProceses As Boolean
 Public ReadOnly Property AutmateProceses() As Boolean
  Get
   Return _AutmateProceses
  End Get
 End Property
 Private _ZiroSmetka As String
 Public ReadOnly Property ZiroSmetka() As String
  Get
   Return _ZiroSmetka
  End Get
 End Property
 Private _Deponent As String
 Public ReadOnly Property Deponent() As String
  Get
   Return _Deponent
  End Get
 End Property
 Private _EDB As String
 Public ReadOnly Property EDB() As String
  Get
   Return _EDB
  End Get

 End Property
 Private _StationAddress As String
 Public ReadOnly Property StationAddress() As String
  Get
   Return _StationAddress
  End Get
 End Property
 Private _tel As String
 Public ReadOnly Property Tel() As String
  Get
   Return _tel
  End Get
 End Property
 Private _Fax As String
 Public ReadOnly Property Fax() As String
  Get
   Return _Fax
  End Get
 End Property
 Private _Valuta As Integer
 Public ReadOnly Property Valuta() As Integer
  Get
   Return _Valuta
  End Get
 End Property
 Private _PictureServerPath As String
 Public ReadOnly Property PictureServerPath() As String
  Get
   Return _PictureServerPath
  End Get

 End Property
 Private _LogoPath As String
 Public ReadOnly Property LogoPath() As String
  Get
   Return _LogoPath
  End Get

 End Property

 Private _PriceWithTax As Boolean
 Public ReadOnly Property PriceWithTax() As Boolean
  Get
   Return _PriceWithTax
  End Get
 End Property
 Private _PriceWithoutTax As Boolean
 Public ReadOnly Property PriceWithoutTax() As Boolean
  Get
   Return _PriceWithoutTax
  End Get
 End Property
 Private _CalculatePercentOfTeh As Boolean
 Public ReadOnly Property CalculatePercentOfTeh() As Boolean
  Get
   Return _CalculatePercentOfTeh
  End Get
 End Property
 Private _PercentOfTeh As Double
 Public ReadOnly Property PercentOfTeh() As Double
  Get
   Return _PercentOfTeh
  End Get
 End Property
 Private _idTehnicalExam As Integer
 Public ReadOnly Property idTehnicalExam() As Integer
  Get
   Return _idTehnicalExam
  End Get
 End Property
 Private _idStavkaZavisnaOdTehnicalExam As Integer
 Public ReadOnly Property idStavkaZavisnaOdTehnicalExam() As Integer
  Get
   Return _idStavkaZavisnaOdTehnicalExam
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
  _IdCompany = dr.GetInt32("IdCompany")
  _organizationName = dr.GetString("OrganizationName")
  _station = dr.GetString("Station")
  _idCity = dr.GetInt32("IdCity")
  _code = dr.GetString("Code")
  _ApproveRequestAutomate = dr.GetBoolean("ApproveRequestAutomate")
  _NewTechnicalExamReport = dr.GetBoolean("NewTechnicalExamReport")
  _IdCommunity = dr.GetInt32("IdCommunity")
  _TrafficLicenceVlidNumOfMonths = dr.GetInt32("TrafficLicenceVlidNumOfMonths")
  _RegistrationVlidNumOfMonths = dr.GetInt32("RegistrationVlidNumOfMonths")

  _OdgovorenOrgan = dr.GetString("OdgovorenOrgan")
  _Sekretar = dr.GetString("Sekretar")
  _IdPaymentPrintOption = dr.GetInt32("IdPaymentPrintOption")
  _IdDefaultRegistrationIssuer = dr.GetInt32("IdDefaultRegistrationIssuer")
  '_IdDefaultCity = dr.GetInt32("IdDefaultCity")
  _AutmateProceses = dr.GetBoolean("AutmateProceses")
  _ZiroSmetka = dr.GetString("ZiroSmetka")
  _Deponent = dr.GetString("Deponent")
  _EDB = dr.GetString("EDB")
  _StationAddress = dr.GetString("StationAddress")
  _tel = dr.GetString("Tel")
  _Fax = dr.GetString("Fax")
  _Valuta = dr.GetInt32("Valuta")
  _PictureServerPath = dr.GetString("PictureServerPath")
  _LogoPath = dr.GetString("LogoPath")
  _PriceWithoutTax = dr.GetBoolean("PriceWithoutTax")
  _PriceWithTax = dr.GetBoolean("PriceWithTax")
  _CalculatePercentOfTeh = dr.GetBoolean("CalculatePercentOfTeh")
  _PercentOfTeh = dr.GetValue("PercentOfTeh")
  _idTehnicalExam = dr.GetInt32("idTehnicalExam")
  _idStavkaZavisnaOdTehnicalExam = dr.GetInt32("idStavkaZavisnaOdTehnicalExam")
 End Sub

  
End Class