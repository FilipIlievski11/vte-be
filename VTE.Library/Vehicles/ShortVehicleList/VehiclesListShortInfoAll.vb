
<Serializable()> _
Public Class VehiclesListShortInfoAll
    Inherits ReadOnlyBase(Of VehiclesListShortInfoAll)

    Private _vehiceMaker As String
    Public ReadOnly Property VehiceMaker() As String
        Get
            Return _vehiceMaker
        End Get
    End Property
    Private _modelName As String
    Public ReadOnly Property ModelName() As String
        Get
            Return _modelName
        End Get
    End Property
    Private _shellNumber As String
    Public ReadOnly Property ShellNumber() As String
        Get
            Return _shellNumber
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
    Private _ownerMB As String
    Public ReadOnly Property OwnerMB() As String
        Get
            Return _ownerMB
        End Get
    End Property
    Private _ownerName As String
    Public ReadOnly Property OwnerName() As String
        Get
            Return _ownerName
        End Get
    End Property
    Private _id As Long
    Public ReadOnly Property Id() As Long
        Get
            Return _id
        End Get
    End Property
    Private _lastRegistrationValidTill As Date
    Public ReadOnly Property LastRegistrationValidTill() As Date
        Get
            Return _lastRegistrationValidTill
        End Get
    End Property
    Private _lastRegistrationNumber As String
    Public ReadOnly Property LastRegistration() As String
        Get
            Return _lastRegistrationNumber
        End Get
    End Property


    Friend Sub New(ByVal dr As SafeDataReader)
        _vehiceMaker = dr.GetString("CompanyName")
        _modelname = dr.GetString("ModelName")
        _shellnumber = dr.GetString("ShellNumber")
        _customersurname = dr.GetString("CustomerSurname")
        _customerfirstname = dr.GetString("CustomerFirstName")
        _ownerMB = dr.GetString("MB")
        _id = dr.GetInt64("Id")
        _lastRegistrationValidTill = dr.GetDateTime("LastRegistration")
        _lastRegistrationNumber = dr.GetString("RegistrationNumber")
        _ownerName = _customerSurname & " " & _customerFirstName
    End Sub

End Class