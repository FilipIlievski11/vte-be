
<Serializable()> _
Public Class VehicleRegistrationInfo
    Inherits ReadOnlyBase(Of VehicleRegistrationInfo)

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
    'Private _placeOfRegistration As String
    'Public ReadOnly Property PlaceOfRegistration() As String
    '    Get
    '        Return _placeOfRegistration
    '    End Get
    'End Property
    Private _isFirstRegistration As Boolean
    Public ReadOnly Property IsFirstRegistration() As Boolean
        Get
            Return _isFirstRegistration
        End Get
    End Property
    Private _dateAdded As Date
    Public ReadOnly Property DateAdded() As Date
        Get
            Return _dateAdded
        End Get
    End Property
    Private _userChanged As String
    Public ReadOnly Property UserChanged() As String
        Get
            Return _userChanged
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
        _registrationNumber = dr.GetString("RegistrationNumber")
        _dateOfRegistration = dr.GetDateTime("DateOfRegistration")
        _dateRegistrationValidTill = dr.GetDateTime("DateRegistrationValidTill")
        ' _placeOfRegistration = dr.GetString("PlaceOfRegistration")
        _isFirstRegistration = dr.GetBoolean("IsFirstRegistration")
        _dateAdded = dr.GetDateTime("DateAdded")
        _userChanged = dr.GetString("UserChanged")
        Try
            _issuerName = dr.GetString("IssuerName")
        Catch ex As Exception

        End Try
    End Sub

End Class