<Serializable()> _
Public Class ActiveDocumentsDepInfo
    Inherits ReadOnlyBase(Of ActiveDocumentsDepInfo)

#Region " Calculated Fields "
    Private _customerDisplay As String = String.Empty
    Private _lastRegistrationNumber As String = String.Empty
    'Private _operatorCreated As String = String.Empty
    Public ReadOnly Property LastRegistrationNumber() As String
        Get
            Return _lastRegistrationNumber
        End Get
    End Property
    Public ReadOnly Property VehicleDisplay() As String
        Get
            Return _lastRegistrationNumber & "(" & _shellNumber & ")"
        End Get
    End Property
    Public ReadOnly Property VehicleModelMaker() As String
        Get
            Return _vehicleMaker & ", " & _vehicleMaker
        End Get
    End Property
    Public ReadOnly Property CustomerDisplay() As String
        Get
            Return _customerDisplay
        End Get
    End Property
    'Public ReadOnly Property OperatorCreated() As String
    '  Get
    '    Return _operatorCreated
    '  End Get
    'End Property

#End Region

    Private _id As Long
    Public ReadOnly Property Id() As Long
        Get
            Return _id
        End Get
    End Property
    Private _requestPrintName As String
    Public ReadOnly Property RequestPrintName() As String
        Get
            Return _requestPrintName
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
    Private _shellNumber As String
    Public ReadOnly Property ShellNumber() As String
        Get
            Return _shellNumber
        End Get
    End Property
    Private _idVehicleModel As Integer
    Public ReadOnly Property IdVehicleModel() As Integer
        Get
            Return _idVehicleModel
        End Get
    End Property
    Private _modelName As String
    Public ReadOnly Property ModelName() As String
        Get
            Return _modelName
        End Get
    End Property
    Private _idVehicleMaker As Integer
    Public ReadOnly Property IdVehicleMaker() As Integer
        Get
            Return _idVehicleMaker
        End Get
    End Property
    Private _vehicleMaker As String
    Public ReadOnly Property VehicleMaker() As String
        Get
            Return _vehicleMaker
        End Get
    End Property
    Private _idOperatorCreated As Integer
    Public ReadOnly Property IdOperatorCreated() As Integer
        Get
            Return _idOperatorCreated
        End Get
    End Property
    Private _idOperatorModified As Integer
    Public ReadOnly Property IdOperatorModified() As Integer
        Get
            Return _idOperatorModified
        End Get
    End Property
    Private _idOperatorEnded As Integer
    Public ReadOnly Property IdOperatorEnded() As Integer
        Get
            Return _idOperatorEnded
        End Get
    End Property
    Private _idTechnicalExamReport As Long
    Public ReadOnly Property IdTechnicalExamReport() As Long
        Get
            Return _idTechnicalExamReport
        End Get
    End Property
    Private _dateCreated As Date
    Public ReadOnly Property DateCreated() As Date
        Get
            Return _dateCreated
        End Get
    End Property
    Private _dateModified As Date
    Public ReadOnly Property DateModified() As Date
        Get
            Return _dateModified
        End Get
    End Property
    Private _dateEnded As Date
    Public ReadOnly Property DateEnded() As Date
        Get
            Return _dateEnded
        End Get
    End Property
    Private _idDocumentPrint As Integer
    Public ReadOnly Property IdDocumentPrint() As Integer
        Get
            Return _idDocumentPrint
        End Get
    End Property
    Private _IdRequestType As Integer
    Public ReadOnly Property IdRequestType() As Integer
        Get
            Return _IdRequestType
        End Get
    End Property

    Protected Overrides Function GetIdValue() As Object
        Return _id
    End Function

    Public Overrides Function ToString() As String
        Return _id
    End Function
#Region " Factory Methods "

    Friend Shared Function GetActiveDocumentsDepInfo(ByVal dr As SafeDataReader) As ActiveDocumentsDepInfo
        Return New ActiveDocumentsDepInfo(dr)
    End Function

    Private Sub New(ByVal dr As SafeDataReader)
        Fetch(dr)
    End Sub


#End Region
    Friend Sub Fetch(ByVal dr As SafeDataReader)
        _id = dr.GetInt64("Id")
        _requestPrintName = dr.GetString("RequestPrintName")
        _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
        _idCustomer = dr.GetInt64("IdCustomer")
        _idVehicle = dr.GetInt64("IdVehicle")
        _mb = dr.GetString("MB")
        _customerSurname = dr.GetString("CustomerSurname")
        _customerFirstName = dr.GetString("CustomerFirstName")
        _shellNumber = dr.GetString("ShellNumber")
        _idVehicleModel = dr.GetInt32("IdVehicleModel")
        _modelName = dr.GetString("ModelName")
        _idVehicleMaker = dr.GetInt32("IdVehicleMaker")
        _vehicleMaker = dr.GetString("VehicleMaker")
        _idOperatorCreated = dr.GetInt32("IdOperatorCreated")
        _idOperatorModified = dr.GetInt32("IdOperatorModified")
        _idOperatorEnded = dr.GetInt32("IdOperatorEnded")
        _idTechnicalExamReport = dr.GetInt64("IdTechnicalExamReport")
        _dateCreated = dr.GetDateTime("DateCreated")
        _dateModified = dr.GetDateTime("DateModified")
        _dateEnded = dr.GetDateTime("DateEnded")
        _idDocumentPrint = dr.GetInt32("IdDocumentPrint")
        _IdRequestType = dr.GetInt32("IdRequestType")
        _lastRegistrationNumber = dr.GetString("LastRegistratinNumber")
        If _customerSurname <> String.Empty Then
            _customerDisplay = _customerSurname & " " & _customerFirstName
        Else
            _customerDisplay = _customerFirstName
        End If
        'Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_id)
        '_lastRegistrationNumber = regInfo.RegistrationNumber

        '_operatorCreated = CType(Csla.ApplicationContext.LocalContext("objUsersList"), UsersList).getInfoById(_idOperatorCreated).FullName
    End Sub

End Class