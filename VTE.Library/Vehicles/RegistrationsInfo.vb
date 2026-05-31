
<Serializable()> _
Public Class RegistrationsInfo
  Inherits ReadOnlyBase(Of RegistrationsInfo)

  Private _id As Long
  Public ReadOnly Property Id() As Long
    Get
      Return _id
    End Get
  End Property
  Private _idVehicle As Long
  Public ReadOnly Property IdVehicle() As Long
    Get
      Return _idVehicle
    End Get
  End Property
  Private _idRegistrationIssuer As Integer
  Public ReadOnly Property IdRegistrationIssuer() As Integer
    Get
      Return _idRegistrationIssuer
    End Get
  End Property
  Private _issuerName As String
  Public ReadOnly Property IssuerName() As String
    Get
      Return _issuerName
    End Get
  End Property
  Private _registrationNumber As String
  Public ReadOnly Property RegistrationNumber() As String
    Get
      Return _registrationNumber
    End Get
  End Property
  Private _dateOfRegistration As Date
  Public ReadOnly Property DateOfRegistration() As Date
    Get
      Return _dateOfRegistration
    End Get
  End Property
  Private _dateRegistrationValidTill As Date
  Public ReadOnly Property DateRegistrationValidTill() As Date
    Get
      Return _dateRegistrationValidTill
    End Get
  End Property
  Private _placeOfRegistration As String
  Public ReadOnly Property PlaceOfRegistration() As String
    Get
      Return _placeOfRegistration
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
    _idVehicle = dr.GetInt64("IdVehicle")
    _idRegistrationIssuer = dr.GetInt32("IdRegistrationIssuer")
    _issuerName = dr.GetString("IssuerName")
    _registrationnumber = dr.GetString("RegistrationNumber")
    _dateofregistration = dr.GetDateTime("DateOfRegistration")
    _dateregistrationvalidtill = dr.GetDateTime("DateRegistrationValidTill")
    _placeofregistration = dr.GetString("PlaceOfRegistration")
  End Sub

End Class