
<Serializable()> _
Public Class DocumentsPermisionsInfo
  Inherits ReadOnlyBase(Of DocumentsPermisionsInfo)

#Region " Calculated Fields "
  Private _customerDisplay As String = String.Empty
  Private _ownerDisplay As String = String.Empty
  Private _lastRegistrationNumber As String = String.Empty

  Public ReadOnly Property CustomerDisplay() As String
    Get
      Return _customerDisplay
    End Get
  End Property

  Public ReadOnly Property OwnerDisplay() As String
    Get
      Return _ownerDisplay
    End Get
  End Property

  Public ReadOnly Property LastRegistrationNumber() As String
    Get
      Return _lastRegistrationNumber
    End Get
  End Property

#End Region

  Private _id As Long
  Public ReadOnly Property Id() As Long
    Get
      Return _id
    End Get
  End Property
  Private _idCustomerVehicleRelation As Long
  Public ReadOnly Property IdCustomerVehicleRelation() As Long
    Get
      Return _idCustomerVehicleRelation
    End Get
  End Property
  Private _customerMB As String
  Public ReadOnly Property CustomerMB() As String
    Get
      Return _customerMB
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
  Private _idCustomerVehicleRelationOwner As Long
  Public ReadOnly Property IdCustomerVehicleRelationOwner() As Long
    Get
      Return _idCustomerVehicleRelationOwner
    End Get
  End Property
  Private _idCustomer As Long
  Public ReadOnly Property IdCustomer() As Long
    Get
      Return _idCustomer
    End Get
  End Property
  Private _ownerMB As String
  Public ReadOnly Property OwnerMB() As String
    Get
      Return _ownerMB
    End Get
  End Property
  Private _ownerSurname As String = String.Empty
  Public ReadOnly Property OwnerSurname() As String
    Get
      Return _ownerSurname
    End Get
  End Property
  Private _ownerFirstName As String
  Public ReadOnly Property OwnerFirstName() As String
    Get
      Return _ownerFirstName
    End Get
  End Property
  Private _idVehicle As Long
  Public ReadOnly Property IdVehicle() As Long
    Get
      Return _idVehicle
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
  Private _companyName As String
  Public ReadOnly Property CompanyName() As String
    Get
      Return _companyName
    End Get
  End Property
  Private _trafficLicenceNumber As String
  Public ReadOnly Property TrafficLicenceNumber() As String
    Get
      Return _trafficLicenceNumber
    End Get
  End Property
  Private _triptiqueNumber As String
  Public ReadOnly Property TriptiqueNumber() As String
    Get
      Return _triptiqueNumber
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
    Private _LivingAddressNumber As String
    Public ReadOnly Property LivingAddressNumber() As String
        Get
            Return _LivingAddressNumber
        End Get
    End Property
    Private _StreetName As String
    Public ReadOnly Property StreetName() As String
        Get
            Return _StreetName
        End Get
    End Property
    Private _CityName As String
    Public ReadOnly Property CityName() As String
        Get
            Return _CityName
        End Get
    End Property
    Public ReadOnly Property LivingAddress() As String
        Get
            If _StreetName <> String.Empty OrElse _LivingAddressNumber <> String.Empty Then
                Return _StreetName & " " & _LivingAddressNumber & " " & _CityName
            Else
                Return _CityName
            End If
        End Get
    End Property

    Private _OwnerLivingAddressNumber As String
    Public ReadOnly Property OwnerLivingAddressNumber() As String
        Get
            Return _OwnerLivingAddressNumber
        End Get
    End Property
    Private _OwnerStreet As String
    Public ReadOnly Property OwnerStreet() As String
        Get
            Return _OwnerStreet
        End Get
    End Property
    Private _OwnerCity As String
    Public ReadOnly Property OwnerCity() As String
        Get
            Return _OwnerCity
        End Get
    End Property
    Public ReadOnly Property OwnerLivingAddress() As String
        Get
            If _OwnerStreet <> String.Empty OrElse _OwnerLivingAddressNumber <> String.Empty Then
                Return _OwnerStreet & " " & _OwnerLivingAddressNumber & " " & _OwnerCity
            Else
                Return _OwnerCity
            End If
        End Get
    End Property

    Private _PermissionNumber As String
    Public ReadOnly Property PermissionNumber() As String
        Get
            Return _PermissionNumber
        End Get
    End Property
    Private _OwnerBlk As String
    Public ReadOnly Property OwnerBlk() As String
        Get
            Return _OwnerBlk
        End Get
    End Property
    Public ReadOnly Property OwnerBlkEMB() As String
        Get
            If _ownerMB <> String.Empty Then
                Return _ownerMB
            Else
                Return _OwnerBlk
            End If
        End Get
    End Property
    Private _PassportNumber As String
    Public ReadOnly Property PassportNumber() As String
        Get
            Return _PassportNumber
        End Get
    End Property
    Private _PassIssuer As String
    Public ReadOnly Property PassIssuer() As String
        Get
            Return _PassIssuer
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
    _idcustomervehiclerelation = dr.GetInt64("IdCustomerVehicleRelation")
    _customermb = dr.GetString("CustomerMB")
    _customersurname = dr.GetString("CustomerSurname")
    _customerfirstname = dr.GetString("CustomerFirstName")
    _idcustomervehiclerelationowner = dr.GetInt64("IdCustomerVehicleRelationOwner")
    _idcustomer = dr.GetInt64("IdCustomer")
    _ownermb = dr.GetString("OwnerMB")
    _ownersurname = dr.GetString("OwnerSurname")
    _ownerfirstname = dr.GetString("OwnerFirstName")
    _idvehicle = dr.GetInt64("IdVehicle")
    _enginenumber = dr.GetString("EngineNumber")
    _shellnumber = dr.GetString("ShellNumber")
    _idvehiclemodel = dr.GetInt32("IdVehicleModel")
    _modelname = dr.GetString("ModelName")
    _companyname = dr.GetString("CompanyName")
    _trafficlicencenumber = dr.GetString("TrafficLicenceNumber")
    _triptiquenumber = dr.GetString("TriptiqueNumber")
    _datecreated = dr.GetDateTime("DateCreated")
    _validtilldate = dr.GetDateTime("ValidTillDate")
        _note = dr.GetString("Note")
        _lastRegistrationNumber = dr.GetString("LastRegistratinNumber")
    If _customerSurname <> String.Empty Then
      _customerDisplay = _customerSurname & " " & _customerFirstName
    Else
      _customerDisplay = _customerFirstName
    End If
    If _ownerSurname <> String.Empty Then
      _ownerDisplay = _ownerSurname & " " & _ownerFirstName
    Else
      _ownerDisplay = _ownerFirstName
    End If
        ' Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_idVehicle)
        'regInfo.RegistrationNumber
        _LivingAddressNumber = dr.GetString("LivingAddressNumber")
        _StreetName = dr.GetString("StreetName")
        _CityName = dr.GetString("CityName")
        _OwnerLivingAddressNumber = dr.GetString("OwnerLivingAddressNumber")
        _OwnerStreet = dr.GetString("OwnerStreet")
        _OwnerCity = dr.GetString("OwnerCity")
        _PermissionNumber = dr.GetString("PermissionNumber")
        _OwnerBlk = dr.GetString("OwnerBlk")
        _PassportNumber = dr.GetString("PassportNumber")
        _PassIssuer = dr.GetString("PassIssuer")
   
  End Sub

End Class
