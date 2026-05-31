
<Serializable()> _
Public Class CustomerVehiclesRelationsInfo
    Inherits ReadOnlyBase(Of CustomerVehiclesRelationsInfo)

#Region " Calculated Fields "

    Private _customerDuisplay As String = String.Empty

    Public ReadOnly Property RelationDescription() As String
        Get
            If _idVehicle <= 0 Then
                Return CustomerName
            Else
                Return _customerDuisplay & "; " & _lastRegistrationNumber & " " & _shellNumber
            End If
        End Get
    End Property

    Public ReadOnly Property CustomerName() As String
        Get
            Return _customerDuisplay
        End Get
    End Property

#End Region

    Private _id As Long
    Public ReadOnly Property Id() As Long
        Get
            Return _id
        End Get
    End Property
    Private _idRelationType As Integer
    Public ReadOnly Property IdRelationType() As Integer
        Get
            Return _idRelationType
        End Get
    End Property
    Private _idCustomer As Long
    Public ReadOnly Property IdCustomer() As Long
        Get
            Return _idCustomer
        End Get
    End Property
    Private _idVehicle As Long
    Public ReadOnly Property IdVehicle() As Long
        Get
            Return _idVehicle
        End Get
    End Property
    Private _startDate As Date
    Public ReadOnly Property StartDate() As Date
        Get
            Return _startDate
        End Get
    End Property
    Private _endDate As Date
    Public ReadOnly Property EndDate() As Date
        Get
            Return _endDate
        End Get
    End Property
    Private _beginNote As String
    Public ReadOnly Property BeginNote() As String
        Get
            Return _beginNote
        End Get
    End Property
    Private _terminationNote As String
    Public ReadOnly Property TerminationNote() As String
        Get
            Return _terminationNote
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
    Private _mb As String
    Public ReadOnly Property MB() As String
        Get
            Return _mb
        End Get
    End Property

    Private _engineNumber As String
    Public ReadOnly Property EngineNumber() As String
        Get
            Return _engineNumber
        End Get
    End Property
    Private _shellNumber As String
    Public ReadOnly Property ShellNumber() As String
        Get
            Return _shellNumber
        End Get
    End Property
    Private _relationTypeName As String
    Public ReadOnly Property RelationTypeName() As String
        Get
            Return _relationTypeName
        End Get
    End Property
    Private _lastRegistrationNumber As String
    Public ReadOnly Property LastRegistration() As String
        Get
            Return _lastRegistrationNumber
        End Get
    End Property
    Public ReadOnly Property ShellNumberAndLastRg() As String
        Get
            Return _shellNumber & "; " & _lastRegistrationNumber
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
        _idRelationType = dr.GetInt32("IdRelationType")
        _idCustomer = dr.GetInt64("IdCustomer")
        _idVehicle = dr.GetInt64("IdVehicle")
        _startDate = dr.GetDateTime("StartDate")
        _endDate = dr.GetDateTime("EndDate")
        _beginNote = dr.GetString("BeginNote")
        _terminationNote = dr.GetString("TerminationNote")
        _customerSurname = dr.GetString("CustomerSurname")
        _customerFirstName = dr.GetString("CustomerFirstName")
        _mb = dr.GetString("MB")
        _engineNumber = dr.GetString("EngineNumber")
        _shellNumber = dr.GetString("ShellNumber")
        _relationTypeName = dr.GetString("RelationTypeName")
        _lastRegistrationNumber = dr.GetString("RegistrationNumber")

        If _customerSurname <> String.Empty Then
            _customerDuisplay = _customerSurname & " " & _customerFirstName & " " & _mb
        Else
            _customerDuisplay = _customerFirstName & " " & _mb
        End If
    End Sub


End Class
