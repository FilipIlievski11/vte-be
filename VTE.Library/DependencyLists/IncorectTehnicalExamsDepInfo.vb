
<Serializable()> _
Public Class IncorectTehnicalExamsDepInfo
    Inherits ReadOnlyBase(Of IncorectTehnicalExamsDepInfo)

    Private _idTehnicalExam As Long
    Public ReadOnly Property IdTehnicalExam() As Long
        Get
            Return _idTehnicalExam
        End Get
    End Property
    Private _madeDate As Date
    Public ReadOnly Property MadeDate() As Date
        Get
            Return _madeDate
        End Get
    End Property
    Private _validTillDate As Date
    Public ReadOnly Property ValidTillDate() As Date
        Get
            Return _validTillDate
        End Get
    End Property
    Private _idFirsControler As Integer
    Public ReadOnly Property IdFirsControler() As Integer
        Get
            Return _idFirsControler
        End Get
    End Property
    Private _idSecondControler As Integer
    Public ReadOnly Property IdSecondControler() As Integer
        Get
            Return _idSecondControler
        End Get
    End Property
    Private _explanationNote As String
    Public ReadOnly Property ExplanationNote() As String
        Get
            Return _explanationNote
        End Get
    End Property
    Private _driversWarning As String
    Public ReadOnly Property DriversWarning() As String
        Get
            Return _driversWarning
        End Get
    End Property

    Public ReadOnly Property CustomerDisplay() As String
        Get
            If _customerSurname <> String.Empty Then
                Return _customerSurname & " " & _customerFirstName
            Else
                Return _customerFirstName
            End If
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

    Public ReadOnly Property VehicleDisplay() As String
        Get
            Return _shellNumber & " (" & _vehicleMaker & ", " & _vehicleModel & ") "
        End Get
    End Property

    Private _vehicleLastRegistrationNumber As String
    Public ReadOnly Property VehicleLastRegistrationNumber() As String
        Get
            Return _vehicleLastRegistrationNumber
        End Get
    End Property

    Private _vehicleLastRegistrationDate As Date
    Public ReadOnly Property VehicleLastRegistrationDate() As Date
        Get
            Return _vehicleLastRegistrationDate
        End Get
    End Property

    Private _vehicleModel As String
    Private _vehicleMaker As String
    Private _shellNumber As String
    Public ReadOnly Property ShellNumber() As String
        Get
            Return _shellNumber
        End Get
    End Property
    Private _engineNumber As String
    Public ReadOnly Property EngineNumber() As String
        Get
            Return _engineNumber
        End Get
    End Property
    Private _idCustomerVehicleRelation As Long
    Public ReadOnly Property IdCustomerVehicleRelation() As Long
        Get
            Return _idCustomerVehicleRelation
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
    Private _TypeOfTehnicalExam As String
    Public ReadOnly Property TypeOfTehnicalExam() As String
        Get
            Return _TypeOfTehnicalExam
        End Get
    End Property
    Protected Overrides Function GetIdValue() As Object
        Return _idTehnicalExam
    End Function

    Public Overrides Function ToString() As String
        Return _idTehnicalExam
    End Function
#Region " Factory Methods "

    Friend Shared Function GetIncorectTehnicalExamsDepInfo(ByVal dr As SafeDataReader) As IncorectTehnicalExamsDepInfo
        Return New IncorectTehnicalExamsDepInfo(dr)
    End Function

    Private Sub New(ByVal dr As SafeDataReader)
        Fetch(dr)
    End Sub


#End Region
    Friend Sub Fetch(ByVal dr As SafeDataReader)
        _idTehnicalExam = dr.GetInt64("IdTehnicalExam")
        _madeDate = dr.GetDateTime("MadeDate")
        _validTillDate = dr.GetDateTime("ValidTillDate")
        _idFirsControler = dr.GetInt32("IdFirsControler")
        _idSecondControler = dr.GetInt32("IdSecondControler")
        _explanationNote = dr.GetString("ExplanationNote")
        _driversWarning = dr.GetString("DriversWarning")
        _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
        _idCustomer = dr.GetInt64("IdCustomer")
        _idVehicle = dr.GetInt64("IdVehicle")

        _customerSurname = dr.GetString("CustomerSurname")
        _customerFirstName = dr.GetString("CustomerFirstName")

        _shellNumber = dr.GetString("ShellNumber")
        _engineNumber = dr.GetString("EngineNumber")

        _vehicleModel = dr.GetString("VehicleModel")
        _vehicleMaker = dr.GetString("VehicleMaker")
        _TypeOfTehnicalExam = dr.GetString("Description")
        'Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_idVehicle)
        _vehicleLastRegistrationNumber = dr.GetString("LastRegistratinNumber") 'regInfo.RegistrationNumber
        _vehicleLastRegistrationDate = dr.GetDateTime("LastRegistrationMakeDate") 'regInfo.RegistrationDate
        '_vehicleLastRegistrationNumber = dr.GetString("RegistrationNumber")
        '_vehicleLastRegistrationDate = dr.GetDateTime("LastRegistrationDate")
    End Sub

End Class